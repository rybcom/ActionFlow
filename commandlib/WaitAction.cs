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
            int actualTime = 0;
            int logStepTime = (int)(this.Milliseconds / 100);

            while (actualTime<this.Milliseconds)
            {
                LogWaitingMessage(actualTime);

                Thread.Sleep(logStepTime);

                actualTime += logStepTime;
            }

            LogWaitingMessage(this.Milliseconds);
        }

        #endregion

        #region private methods

        private void LogWaitingMessage(int actualTime)
        {
            int get_percentage_done(int currentTime)
            {
                return (int)(currentTime / (double)(this.Milliseconds) * 100);
            }

            ClearCurrentConsoleLine();
            Console.Write($"\tWaiting for {this.Milliseconds} milliseconds [ { get_percentage_done(actualTime)} % done ]");
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        #endregion
    }
}
