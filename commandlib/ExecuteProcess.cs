using System;
using System.Diagnostics;

namespace commandlib
{

    public static class ProcessHelpers
    {
        public static bool IsRunning(string fullname)
        {
            string name = System.IO.Path.GetFileNameWithoutExtension(fullname);
            int processCount = Process.GetProcessesByName(name).Length;
            return processCount > 0;
        }
    }

    public class ExecuteProcess : ActionBase
    {

        #region property

        public string FileName { get; set; }

        public string Params { get; set; }

        public bool OnlyIfNotRunning { get; set; }


        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            if (this.OnlyIfNotRunning)
            {
                if (ProcessHelpers.IsRunning(this.FileName))
                {
                    Console.WriteLine($"\tProcess  [ {this.FileName} ] is already running");
                }
                else
                {
                    StartProcess();
                }
            }
            else
            {
                StartProcess();
            }

        }

        #endregion

        #region private methods
        private void StartProcess()
        {
            Console.WriteLine($"\tExecute file  [ {this.FileName} ] with params [ {this.Params} ]");

            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.FileName = this.FileName;
            pinfo.Arguments = this.Params;

            Process.Start(pinfo);
        }

        #endregion
    }
}
