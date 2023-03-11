using System;
using System.Threading;
using System.Collections.Specialized;
using System.Windows.Forms;

namespace commandlib
{
    public class CopyFileToClipboard : ActionBase
    {

        #region helpers

        public static CopyFileToClipboard GetCommandFrom(string text)
        {
            return new CopyFileToClipboard()
            {
                Name = $"CopyFileToClipboard: {text}",
                Source = text
            };
        }

        #endregion

        #region property

        public string Source { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            LogConsole();
            CopyFileToClipbaord_Interal(Source);
        }

        #endregion

        #region private methods

        private static void CopyFileToClipbaord_Interal(string filePath)
        {
	    //use windows file path format due to clipboard format issue
            filePath = filePath.Replace(@"/", @"\");

            // Create a new thread with an STA apartment state due to Cliboard 
            Thread clipboardThread = new Thread(() =>
            {
                StringCollection paths = new StringCollection();
                paths.Add(filePath);
                Clipboard.SetFileDropList(paths);

            });

            // Set the apartment state of the thread to STA
            clipboardThread.SetApartmentState(ApartmentState.STA);
            clipboardThread.Start();
        }

        private void LogConsole()
        {
            Console.WriteLine($"\tcopy file [{this.Source}] to Clipboard ");
        }

        #endregion
    }
}
