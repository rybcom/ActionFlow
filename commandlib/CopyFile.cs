using System;
using System.IO;

namespace commandlib
{
    public class CopyFile : ActionBase
    {

        #region helpers

        public static CopyFile GetCommandFrom(string text)
        {
            string[] tokens = text.Split(new string[] { "--->" }, 2, StringSplitOptions.RemoveEmptyEntries);
            string source = tokens.Length > 0 ? tokens[0] : "";
            string destinaiton = tokens.Length > 1 ? tokens[1] : "";

            return new CopyFile()
            {
                Name = $"CopyFile: {source}",
                Source = source,
                Destination = destinaiton
            };
        }

        #endregion

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
                Console.Error.WriteLine($"file : {sourcePath} not exists");
                return;
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
