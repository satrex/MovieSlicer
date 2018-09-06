using System;
using AppKit;
using Foundation;
using AVFoundation;
using CoreMedia;

namespace MovieSlicer
{
    public partial class ViewController : NSViewController
    {
        public ViewController(IntPtr handle) : base(handle)
        {
        }

        private NSUndoManager undoManager;

        public override void ViewDidLoad()
        {
            base.ViewDidLoad();

            // Do any additional setup after loading the view.
            ClickedLabel.StringValue = "Button has not clicked yet.";
            var appDelegate = NSApplication.SharedApplication.Delegate
                                           as AppDelegate;
            appDelegate.viewController = this;

            undoManager = new NSUndoManager();
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
            undoManager.RegisterUndo(this, (NSObject obj) => SetEndTime(currentEndTime));
        }

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
                this.MoviePlayer.Player = new AVFoundation.AVPlayer(URL: openPanel.Url);
            });

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

        partial void ClickedEndButton(NSObject sender)
        {
            var time = this.MoviePlayer.Player.CurrentTime;
            string videoDurationText = TimeToString(time);

            var size = this.StartImage.FittingSize;
            var thumbnail = TakeScreenshot(time, size);
            this.EndImage.Image = thumbnail;

            SetEndTime(time);
        }

        partial void ClickedButton(NSObject sender)
        {
            var time = this.MoviePlayer.Player.CurrentTime;
            string videoDurationText = TimeToString(time);

            var size = this.StartImage.FittingSize;
            var thumbnail = TakeScreenshot(time, size);

            this._startTime = time;
            this.startTimeLabel.StringValue = videoDurationText;
            this.StartImage.Image = thumbnail;
        }

        private string TimeToString(CoreMedia.CMTime time){
            double dTotalSeconds = time.Seconds;

            double dHours = Math.Floor(dTotalSeconds / 3600);
            double dMinutes = Math.Floor(dTotalSeconds % 3600 / 60);
            double dSeconds = Math.Floor(dTotalSeconds % 3600 % 60);

            string videoDurationText = string.Format(@"{0:00}:{1:00}:{2:00}", dHours, dMinutes, dSeconds);
            return videoDurationText;

        }

    }
}
