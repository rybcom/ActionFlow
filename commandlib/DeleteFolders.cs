﻿using System;
using System.IO;
using System.Text.RegularExpressions;

namespace commandlib
{
    public class DeleteFolders : ActionBase
    {
        #region helpers

        public static DeleteFolders GetCommandFrom(string text)
        {
            string[] tokens = text.Split(new string[] { "," }, 2, StringSplitOptions.RemoveEmptyEntries);
            string sourceFolder = tokens.Length > 0 ? tokens[0] : "";
            string pattern = tokens.Length > 1 ? tokens[1] : "";

            return new DeleteFolders()
            {
                Name = $"DeleteFolders: {sourceFolder},{pattern}",
                SourceFolder = sourceFolder,
                DeletePattern = pattern
            };
        }

        #endregion

        #region property

        public string SourceFolder { get; set; }

        public string DeletePattern { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();

            DirectoryInfo dir = new DirectoryInfo(SourceFolder);
            if (!dir.Exists)
            {
                Console.Error.WriteLine($"folder : {SourceFolder} not exists");
                return;
            }

            var dirs = dir.GetDirectories();

            foreach (DirectoryInfo subdir in dirs)
            {
                DeleteAction(subdir.FullName, DeletePattern, printCallback);
            }
        }

        #endregion

        #region private methods

        private static void DeleteAction(
            string sourceDirName,
            string deletePattern,
            Action<string> deleteCallback = null)
        {
            DirectoryInfo dir = new DirectoryInfo(sourceDirName);

            if (!dir.Exists)
            {
                Console.Error.WriteLine($"folder : {sourceDirName} not exists");
                return;
            }

            if (Regex.IsMatch(dir.FullName, deletePattern))
            {
                deleteCallback(dir.FullName);
                dir.Delete(recursive: true);
            }

        }

        #endregion

        private static void printCallback(string t)
        {
            Console.WriteLine($"\tdelete folder {t}");
        }
    }
}
