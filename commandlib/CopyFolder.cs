using System;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;

namespace commandlib
{
    public class CopyFolder : ActionBase
    {

        #region property

        public string Source { get; set; }

        public string Destination { get; set; }

        public string CopyFilePattern { get; set; } = "(.)";

        public string CopyDirPattern { get; set; } = "(.)";

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            this._allFiles = Directory.GetFiles(Source, "*", SearchOption.AllDirectories).Where(f => Regex.IsMatch(f, CopyFilePattern)).ToArray().Length;

            CopyDir(Source, Destination, CopyFilePattern, CopyDirPattern, true, printCallback);
        }

        #endregion

        #region private methods

        private void CopyDir(
            string sourceDirName,
            string destDirName,
            string copyFileRegexPattern,
            string copyDirRegexPattern,
            bool copySubDirs = true,
            Action<FileInfo> copyCallback = null)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Console.Error.WriteLine($"folder : {sourceDirName} not exists");
                return;
            }

            if (!Directory.Exists(destDirName))
            {
                Directory.CreateDirectory(destDirName);
            }

            var files = dir.GetFiles().Where(f => Regex.IsMatch(f.FullName, copyFileRegexPattern));

            foreach (FileInfo file in files)
            {
                try
                {
                    copyCallback(file);
                    string temppath = Path.Combine(destDirName, file.Name);
                    file.CopyTo(temppath, true);
                }
                catch (Exception e)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine(e.Message);
                    Console.ResetColor();
                }

            }


            if (copySubDirs)
            {
                var dirs = dir.GetDirectories().Where(d => Regex.IsMatch(d.FullName, copyDirRegexPattern)); ;

                foreach (DirectoryInfo subdir in dirs)
                {
                    string destTempPath = Path.Combine(destDirName, subdir.Name);
                    CopyDir(subdir.FullName, destTempPath, copyFileRegexPattern, copyDirRegexPattern, copySubDirs, copyCallback);
                }
            }
        }


        private void printCallback(FileInfo info)
        {
            this._processedFiles++;
            LogCopyFolderCallbackMessage(info);
        }

        private void LogCopyFolderCallbackMessage(FileInfo info)
        {
            int percentageProcessed = (int)(_processedFiles * 100.0 / (double)_allFiles);

            ClearCurrentConsoleLine();
            Console.Write($"\r\t   [ { percentageProcessed} % done ]  - {info.Name}");
        }

        private void ClearCurrentConsoleLine()
        {
            int currentLineCursor = Console.CursorTop;
            Console.SetCursorPosition(0, Console.CursorTop);
            Console.Write(new string(' ', Console.WindowWidth));
            Console.SetCursorPosition(0, currentLineCursor);
        }

        #endregion

        #region members

        private int _processedFiles;
        private int _allFiles;

        #endregion

    }
}
