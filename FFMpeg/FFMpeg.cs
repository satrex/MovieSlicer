using System;
using System.Diagnostics;

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

        private void SetExcutablePath()
        {
            ProcessStartInfo psInfo = new ProcessStartInfo();
            psInfo.FileName = @"/Applications/Utilities/Terminal.app/Contents/MacOS/Terminal"; // 実行するファイル
            psInfo.Arguments = "which FFMpeg";
            psInfo.CreateNoWindow = false; // コンソール・ウィンドウを開かない

            psInfo.UseShellExecute = false; // シェル機能を使用しない
            psInfo.RedirectStandardOutput = true;

            var p = Process.Start(psInfo);

            p.EnableRaisingEvents = true;
            p.OutputDataReceived += (sender, e) => ExecutablePath = e.Data;
            p.BeginOutputReadLine();
            p.WaitForExit();
        }




    }
}
