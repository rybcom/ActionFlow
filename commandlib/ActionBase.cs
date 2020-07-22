﻿using System;

namespace commandlib
{
    public enum ActionType
    {
        Wait,
        Execute,
        CopyFolder,
        DeleteFiles,
        DeleteFolders,
        CopyFile,
        ZipFolder,
        ShowDialog
    }
    
    public abstract class ActionBase
    {
        #region property

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool   Enabled { get; set; } = true;
        public ActionType Type { get; set; }

        #endregion

        #region to override

        

        public virtual void DoAction()
        {
            LogConsole();

            SetConsoleTitle();
        }

        #endregion

        #region private methods

        private void LogConsole()
        {
            Console.ForegroundColor = ConsoleColor.Yellow;

            Console.WriteLine($" \nExecute Type [ {Enum.GetName(typeof(ActionType), Type)} ]" +
                $" Name [ {this.Name} ] " +
             $"Description [ {this.Description} ]");

            Console.ResetColor();
        }

        private void SetConsoleTitle()
        {
            Console.Title = this.Name;
        }

        #endregion

    }
}
