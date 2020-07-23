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
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Console.Error.WriteLine($"folder : {sourceDirName} not exists" );
                return;
            }

            if (dir.FullName.Split('\\').Length < 6)
            {
                actionCallback(dir.FullName);
            }

            var files = dir.GetFiles().Where(f => Regex.IsMatch(f.FullName, deletePattern));

            foreach (FileInfo file in files)
            {
                file.Delete();
            }

            if (delteInSubfolder)
            {
                foreach (DirectoryInfo subdir in dir.GetDirectories())
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
