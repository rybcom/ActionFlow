using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace commandlib
{
    public class ExecuteProcess : ActionBase
    {

        #region property

        public string FileName { get; set; }

        public string Params{ get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            Console.WriteLine($"\tExecute file  [ {this.FileName} ] with params [ {this.Params} ]" );

            ProcessStartInfo pinfo = new ProcessStartInfo();
            pinfo.FileName = this.FileName;
            pinfo.Arguments= this.Params;

            Process.Start(pinfo);
        }

        #endregion

        #region private methods
        #endregion
    }
}
