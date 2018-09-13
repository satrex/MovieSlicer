﻿using System;
using AppKit;
using Foundation;
using AVFoundation;
using CoreMedia;
using Xabe.FFmpeg;
using System.IO;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Xabe.FFmpeg.Enums; 
using Xabe.FFmpeg.Model;
using System.Diagnostics;

namespace MovieSlicer
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }


        private NSUndoManager undoManager;

        private async Task ConvertFile(string filePath){
            FileInfo file = new FileInfo(filePath);
            Trace.Assert(file.Exists, string.Format(@"ファイル{0}が存在しません。", filePath));

            IMediaInfo inputFile = await MediaInfo.Get(file);
            string outputPath = Path.ChangeExtension(Path.GetTempFileName(), FileExtensions.Mp4);
            IConversionResult conversionResult = await Conversion.New()
            .AddStream(inputFile.VideoStreams.First().SetCodec(VideoCodec.H264)
            .ChangeSpeed(1.5))
            .SetOutput(outputPath)
            .Start();
        }



        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            ClickedLabel.StringValue = "Button has not clicked yet.";
            var appDelegate = NSApplication.SharedApplication.Delegate
                                           as AppDelegate;
            appDelegate.viewController = this;

            undoManager = new NSUndoManager();
            SwitchButtonsEnabled();
            this.speedText.StringValue = "1.0 x";
            ffmpeg = new Satrex.FFMpeg.FFMpeg();
        }

        private void SetExcutablePath() {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = @"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"; // 実行するファイル
            psInfo.Arguments = "which FFMpeg";
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput= true;

            var p = Process.Start(psInfo);

            p.EnableRaisingEvents = true;
            p.OutputDataReceived += (sender, e) => FFmpeg.ExecutablesPath = e.Data;
            p.BeginOutputReadLine();
        }

        partial void EndToStartClicked(NSObject sender)
        {
            var currentStartImage = this.StartImage.Image.Copy();
            var currentStartTime = this._startTime;
            this.StartImage.Image = this.EndImage.Image;
            this._startTime = this._endTime;

            this.EndImage.Image = null;
            this._endTime = CoreMedia.CMTime.Invalid;
        }

        private Satrex.FFMpeg.FFMpeg ffmpeg;


        public override NSObject RepresentedObject
        {
            get
            {
                return base.RepresentedObject;
            }
            set
            {
                base.RepresentedObject = value;
                // Update the view, if already loaded.
            }
        }

        private NSUrl movieUrl;
        private CMTime _startTime;
        private CMTime StartTime{
            get { return _startTime; }
            set {
                _startTime = value;
                if (value == CMTime.Invalid)
                {
                    this.startTimeLabel.StringValue = string.Empty;
                }
                else
                {
                    this.startTimeLabel.StringValue = TimeToString(_startTime);
                }
                SwitchButtonsEnabled();
            }
        }

        private CoreMedia.CMTime _endTime;
        private CMTime EndTime
        {
            get { return _endTime; }
            set
            {
                SetEndTime(value);
            }
        }

        public void SetEndTime(CMTime time){
            var currentEndTime = _endTime;
            _endTime = time;
            if (time == CMTime.Invalid)
            {
                this.endTimeLabel.StringValue = string.Empty;
            }
            else
            {
                this.endTimeLabel.StringValue = TimeToString(_endTime);
            }
            this.ffmpeg.EndSecond = _endTime.Seconds;
            SwitchButtonsEnabled();
            undoManager.RegisterUndo(this, (NSObject obj) => SetEndTime(currentEndTime));
        }

        public void SetStartTime(CMTime time)
        {
            var currentStartTime = _startTime;
            _startTime = time;
            if (time == CMTime.Invalid)
            {
                this.startTimeLabel.StringValue = string.Empty;
            }
            else
            {
                this.startTimeLabel.StringValue = TimeToString(_startTime);
            }
            this.ffmpeg.StartSecond = _startTime.Seconds;
            SwitchButtonsEnabled();
            undoManager.RegisterUndo(this, (NSObject obj) => SetStartTime(currentStartTime));
        }



        public string BaseFileName { get; set; }
        public void OpenFile()
        {
            NSOpenPanel openPanel = new NSOpenPanel();
            openPanel.CanChooseFiles = true;
            openPanel.CanChooseDirectories = false;
            openPanel.AllowsMultipleSelection = false;
            openPanel.Message = "動画ファイルを開く";

            var extensions = new string[3];
            extensions[0] = "mov";
            extensions[1] = "mp4";
            extensions[2] = "m4v";
            //["mov", "mp4", "m4v"];
            openPanel.AllowedFileTypes = extensions;
            openPanel.Begin(result =>
            {
                if (result != (int)NSModalResponse.OK)
                {
                    this.ClickedLabel.StringValue = string.Empty;
                    return;
                }

                this.ClickedLabel.StringValue = openPanel.Url.AbsoluteString;
                movieUrl = openPanel.Url;
                this.BaseFileName = Path.GetFileNameWithoutExtension(movieUrl.ToString());
                this.MoviePlayer.Player = new AVFoundation.AVPlayer(URL: openPanel.Url);
                Console.WriteLine(movieUrl.FilePathUrl);
                Console.WriteLine(movieUrl.AbsoluteString);
                string filePath = movieUrl.FilePathUrl.ToString().Remove(0, 7);
                this.ffmpeg.InputFile = new FileInfo(filePath);

                SwitchButtonsEnabled();
            });

        }

        private void SwitchButtonsEnabled(){
            if(movieUrl == null){
                this.endButton.Enabled = false;
                this.startButton.Enabled = false;
                this.invertButton.Enabled = false;
            }
            else{
                this.endButton.Enabled = true;
                this.startButton.Enabled = true;
                this.invertButton.Enabled = true;
            }

            if(this.StartImage.Image == null || this.EndImage.Image == null){
                this.exportButton.Enabled = false;
            }
            else{
                this.exportButton.Enabled = true;
            }

        }

        private NSImage TakeScreenshot2(CoreMedia.CMTime time, CoreGraphics.CGSize size){
            var asset = AVAsset.FromUrl(movieUrl);
            var imageGenerator = new AVAssetImageGenerator(asset);
            //var time = new CoreMedia.CMTime(1, 1);
            var imageRef = imageGenerator.CopyCGImageAtTime(time, out CoreMedia.CMTime outtime, out NSError nSError);
            var thumbnail = new NSImage(imageRef, size);
            thumbnail.Draw(
                thumbnail.AlignmentRect,
                thumbnail.AlignmentRect,
                NSCompositingOperation.SourceOver,
                0,
                respectContextIsFlipped: true,
                hints: null
            );

            return thumbnail;
        }
        private NSImage TakeScreenshot(CoreMedia.CMTime time, CoreGraphics.CGSize size)
        {
            var asset = AVAsset.FromUrl(movieUrl);
            var imageGenerator = new AVAssetImageGenerator(asset);
            var imageRef = imageGenerator.CopyCGImageAtTime(time, out CoreMedia.CMTime outtime, out NSError nSError);
            var thumbnail = new NSImage(imageRef, size);
            thumbnail.Draw(
                thumbnail.AlignmentRect,
                thumbnail.AlignmentRect,
                NSCompositingOperation.SourceOver,
                0,
                respectContextIsFlipped: true, 
                hints: null
            );

            return thumbnail;
        }

        partial void EndButtonClicked(NSObject sender)
        {
            var time = this.MoviePlayer.Player.CurrentTime;
            string videoDurationText = TimeToString(time);

            var size = this.StartImage.FittingSize;
            var thumbnail = TakeScreenshot(time, size);
            this.EndImage.Image = thumbnail;

            SetEndTime(time);
        }

        partial void StartButtonClicked(NSObject sender)
        {
            var time = this.MoviePlayer.Player.CurrentTime;
            string videoDurationText = TimeToString(time);

            var size = this.StartImage.FittingSize;
            var thumbnail = TakeScreenshot(time, size);
            this.StartImage.Image = thumbnail;

            SetStartTime(time);
        }

        private string TimeToString(CoreMedia.CMTime time){
            double dTotalSeconds = time.Seconds;

            double dHours = Math.Floor(dTotalSeconds / 3600);
            double dMinutes = Math.Floor(dTotalSeconds % 3600 / 60);
            double dSeconds = Math.Floor(dTotalSeconds % 3600 % 60);

            string videoDurationText = string.Format(@"{0:00}:{1:00}:{2:00}", dHours, dMinutes, dSeconds);
            return videoDurationText;

        }

        partial void exportButtonClicked(NSObject sender)
        {
            string filePath = movieUrl.FilePathUrl.ToString().Remove(0,7);
            Console.WriteLine(filePath);

            ffmpeg.Convert();
        }

        partial void SpeedChanged(NSTextField sender)
        {
            try{
                var number = double.Parse(sender.StringValue);
                sender.DoubleValue = number;
                sender.StringValue = string.Format("{0:F1} x", number);
                this.speedStepper.DoubleValue = number;
                }
            catch(Exception){
                sender.StringValue = "";
            }
        }

        partial void SpinValueChanged(NSStepper sender)
        {
            var number = sender.DoubleValue;
            this.speedText.StringValue = string.Format("{0:F1} x", number);
        }

    }
}
