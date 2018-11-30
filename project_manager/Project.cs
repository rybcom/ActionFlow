using System;
using System.Collections.Generic;
using commandlib;

namespace project_manager
{
    public class Project
    {
        const string project_title  = @"
 █████╗  ██████╗████████╗██╗ ██████╗ ███╗   ██╗    ███████╗██╗      ██████╗ ██╗    ██╗
██╔══██╗██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║    ██╔════╝██║     ██╔═══██╗██║    ██║
███████║██║        ██║   ██║██║   ██║██╔██╗ ██║    █████╗  ██║     ██║   ██║██║ █╗ ██║
██╔══██║██║        ██║   ██║██║   ██║██║╚██╗██║    ██╔══╝  ██║     ██║   ██║██║███╗██║
██║  ██║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║    ██║     ███████╗╚██████╔╝╚███╔███╔╝
╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝     ╚══════╝ ╚═════╝  ╚══╝╚══╝ 
";

        #region property

        public string Name { get; set; }
        public string Description{ get; set; }

        #endregion

        #region api

        public void Execute()
        {
            LogConsole();

            foreach (ActionBase action in _actionList)
            {
                action.DoAction();
            }
        }

        public void LoadFromFile(string filePath)
        {
            _actionList = new List<ActionBase>();

            ProjectParser parser = new ProjectParser(this,this._actionList);
            parser.ParseProjectFromFile(filePath);
        }

        #endregion

        #region private methods

        private void LogConsole()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(project_title);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Run project : {this.Name} \n" +
                $"Description : {this.Description} ");

            Console.ResetColor();
        }

        #endregion

        #region members

        private List<ActionBase> _actionList;

        #endregion

    }
}
