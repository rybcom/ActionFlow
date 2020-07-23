using System;
using System.Security.Cryptography;

namespace commandlib
{
    public enum ActionType
    {
        Wait,
        Execute,
        CopyFolder,
        DeleteFiles,
        DeleteFile,
        DeleteFolders,
        CopyFile,
        ZipFolder,
        ShowDialog,
        ControlFlow
    }

    public abstract class ActionBase
    {
        #region property

        public string Name { get; set; } = "";
        public string Description { get; set; } = "";
        public bool Enabled { get; set; } = true;
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

            var header = $" \nExecute Type [ {Enum.GetName(typeof(ActionType), Type)} ] ";

            var name = $" Name[ { this.Name} ]";
            var desc = $" Desc[ { this.Description} ]";

            if (String.IsNullOrEmpty(this.Name) == false)
            {
                header += name;
            }
            if (String.IsNullOrEmpty(this.Description) == false)
            {
                header += desc;
            }

            Console.WriteLine(header);
            Console.ResetColor();
        }

        private void SetConsoleTitle()
        {
            Console.Title = this.Name;
        }

        #endregion

    }
}
