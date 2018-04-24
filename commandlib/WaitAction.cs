using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace commandlib
{
    public class WaitAction : ActionBase
    {

        #region property

        public int Milliseconds { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            Console.WriteLine($"\tWaiting for {this.Milliseconds} milliseconds");

            Thread.Sleep(Milliseconds);
        }

        #endregion

        #region private methods
        #endregion
    }
}
