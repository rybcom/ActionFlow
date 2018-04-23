using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandlib
{
    public class CopyFolder : ActionBase
    {

        #region property

        public string Source { get; set; }

        public string Destination { get; set; }

        public string CopyPattern { get; set; }

        #endregion


        #region overridden

        public override void DoAction()
        {
            CopyDir(Source, Destination, CopyPattern);
        }

        #endregion

        #region private methods

        private static void CopyDir(
            string sourceDirName, 
            string destDirName,
            string searchPattern,
            bool copySubDirs = true,
            Action<int, string> copyCallback = null)
        {
            // Get the subdirectories for the specified directory.
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                throw new DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + sourceDirName);
            }

            DirectoryInfo[] dirs = dir.GetDirectories(searchPattern);
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles(searchPattern);

            foreach (FileInfo file in files)
            {
                //index++;
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);

                //copyCallback(0, file.Name);
            }

            // If copying subdirectories, copy them and their contents to new location.
            if (copySubDirs)
            {
                foreach (DirectoryInfo subdir in dirs)
                {
                    string temppath = Path.Combine(destDirName, subdir.Name);
                    CopyDir(subdir.FullName, temppath, searchPattern, copySubDirs, copyCallback);
                }
            }
        }

        #endregion


    }
}
