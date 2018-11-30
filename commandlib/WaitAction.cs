using System;
using System.Threading;

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
