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
         
            base.DoAction();

            CopyDir(Source, Destination, CopyPattern,true, printCallback);
        }

        #endregion

        #region private methods

        private static void CopyDir(
            string sourceDirName, 
            string destDirName,
            string searchPattern,
            bool copySubDirs = true,
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
            
            DirectoryInfo[] dirs = dir.GetDirectories();
            // If the destination directory doesn't exist, create it.
            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            if (dir.FullName.Split('\\').Length < 6)
            {
                copyCallback(dir.FullName);
            }

            // Get the files in the directory and copy them to the new location.
            FileInfo[] files = dir.GetFiles(searchPattern);

            foreach (FileInfo file in files)
            {
                //index++;
                string temppath = Path.Combine(destDirName, file.Name);
                file.CopyTo(temppath, true);

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

        private static void printCallback(string t)
        {
            Console.WriteLine($"\t {t}");
        }
    }
}
