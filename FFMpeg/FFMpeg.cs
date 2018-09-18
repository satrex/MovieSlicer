using System;
using Foundation;
using System.Diagnostics;
using System.Text;
using System.IO;

namespace Satrex.FFMpeg
{
    public class FFMpeg
    {
        public static string ExecutablePath{
            get;
            set;
        }

        public FFMpeg()
        {
            if(ExecutablePath == null){
                SetExcutablePath();
            }

        }

        public FileInfo InputFile
        {
            get;
            set;
        }

        public double StartSecond
        {
            get;
            set;
        }

        public double EndSecond
        {
            get;
            set;
        }

        public double Duration
        {
            get { return EndSecond - StartSecond; }
        }

        private double _speed = 1.0;
        public double Speed
        {
            get { return _speed; }
            set { _speed = value; }
        }

        public string OutputPath
        {
            get {
                try{
                    if (InputFile == null) return string.Empty;
                    if (Duration < 0) return string.Empty;

                    string baseName = Path.Combine(InputFile.Directory.FullName, Path.GetFileNameWithoutExtension(InputFile.FullName));
                    string extention = Path.GetExtension(InputFile.Name);
                    TimeSpan time = new TimeSpan(0, 0, (int)this.StartSecond);
                    string speedString = Math.Abs(Speed - 1.0) < 0.01 ? string.Empty : Speed.ToString();

                    string newFileName = string.Format("{0}{1}-{2}{3}{4}", baseName, time.ToString("%h%mss"), (int)Duration, speedString, extention);
                    return newFileName;

                }
                catch(Exception){
                    Trace.WriteLine("Exception raised on making output path.");
                    return string.Empty;
                }

           }
        }

        public void Convert(){
           
            Run(ExecutablePath, string.Format("-ss {1} -i {0} -t {2} {3}", InputFile.FullName, StartSecond, Duration, this.OutputPath)); 
        }

        private string Run(string fileName, string arguments){
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = fileName; // 実行するファイル
            psInfo.Arguments = arguments;
            psInfo.CreateNoWindow = true; // コンソール・ウィンドウを開かない
            
            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true;
            psInfo.RedirectStandardInput = false;
            psInfo.RedirectStandardError = true;
            psInfo.UserName = Environment.UserName;
            psInfo.StandardOutputEncoding = Encoding.UTF8;
            var p = Process.Start(psInfo);

            return p.StandardOutput.ReadLine();
        }

        private void SetExcutablePath()
        {
           try
            {
                var fileName = @"which"; // 実行するファイル
                var arguments = "FFMpeg";

                ExecutablePath = Run(fileName, arguments);
            }
            catch(Exception ex){
                Console.WriteLine("例外発生: {0} \n{1}:\n{2}", ex.GetType().Name, ex.Message, ex.StackTrace);
            }
        }




    }
}
