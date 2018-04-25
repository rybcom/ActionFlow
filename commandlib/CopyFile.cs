using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace commandlib
{
    public class CopyFile : ActionBase
    {

        #region property

        public string Source { get; set; }

        public string Destination { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            LogConsole();
            CopyFile_Interal(Source, Destination);
        }

        #endregion

        #region private methods

        private static void CopyFile_Interal(
            string sourcePath,
            string destinationPath)
        {
            FileInfo sourceFile = new FileInfo(sourcePath);

            if (!sourceFile.Exists)
            {
                throw new FileNotFoundException(
                    "Source file does not exist or could not be found: "
                    + sourcePath);
            }

            sourceFile.CopyTo(destinationPath, true);
        }

        #endregion

        #region private methods

        private void LogConsole()
        {
            Console.WriteLine($"\tcopy file [{this.Source}] to [{this.Destination}]");
        }

        #endregion
    }
}
