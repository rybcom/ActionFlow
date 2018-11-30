using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace commandlib
{
    public class DeleteFiles : ActionBase
    {

        #region property

        public string SourceFolder { get; set; }

        public string DeletePattern { get; set; }

        public bool RecursiveDelete { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            DeleteAction(SourceFolder, DeletePattern, RecursiveDelete, printCallback);
        }

        #endregion

        #region private methods

        private static void DeleteAction(
            string sourceDirName,
            string deletePattern,
            bool delteInSubfolder = true,
            Action<string> actionCallback = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

         

            if (dir.FullName.Split('\\').Length < 6)
            {
                actionCallback(dir.FullName);
            }

            // Get the files in the directory and copy them to the new location.
            var files = dir.GetFiles().Where(f => Regex.IsMatch(f.FullName, deletePattern));

            foreach (FileInfo file in files)
            {
                //index++;
                file.Delete();
            }

            var dirs = dir.GetDirectories();

            // If copying subdirectories, copy them and their contents to new location.
            if (delteInSubfolder)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    DeleteAction(subdir.FullName, deletePattern, delteInSubfolder, actionCallback);
                }
            }
        }

        #endregion

        private static void printCallback(string t)
        {
            Console.WriteLine($"\t {t}");
        }
    }
}
