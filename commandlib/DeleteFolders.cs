using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace commandlib
{
    public class DeleteFolders : ActionBase
    {

        #region property

        public string SourceFolder { get; set; }

        public string DeletePattern { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            DeleteAction(SourceFolder, DeletePattern, true, printCallback);
        }

        #endregion

        #region private methods

        private static void DeleteAction(
            string sourceDirName,
            string deletePattern,
            bool delteInSubfolder = true,
            Action<string> copyCallback = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            if (Regex.IsMatch(dir.FullName, deletePattern))
            {
                copyCallback(dir.FullName);
                dir.Delete(true);
                return;
            }






            var dirs = dir.GetDirectories();

            // If copying subdirectories, copy them and their contents to new location.
            if (delteInSubfolder)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    DeleteAction(subdir.FullName, deletePattern, delteInSubfolder, copyCallback);
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
