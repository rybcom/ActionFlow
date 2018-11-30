using System;
using System.Collections.Generic;
using commandlib;

namespace project_manager
{
    public class Project
    {
        #region ascii_texts

        const string project_title = @"
 ██╗ ██╗      █████╗  ██████╗████████╗██╗ ██████╗ ███╗   ██╗    ███████╗██╗      ██████╗ ██╗    ██╗     ██╗ ██╗ 
████████╗    ██╔══██╗██╔════╝╚══██╔══╝██║██╔═══██╗████╗  ██║    ██╔════╝██║     ██╔═══██╗██║    ██║    ████████╗
╚██╔═██╔╝    ███████║██║        ██║   ██║██║   ██║██╔██╗ ██║    █████╗  ██║     ██║   ██║██║ █╗ ██║    ╚██╔═██╔╝
████████╗    ██╔══██║██║        ██║   ██║██║   ██║██║╚██╗██║    ██╔══╝  ██║     ██║   ██║██║███╗██║    ████████╗
╚██╔═██╔╝    ██║  ██║╚██████╗   ██║   ██║╚██████╔╝██║ ╚████║    ██║     ███████╗╚██████╔╝╚███╔███╔╝    ╚██╔═██╔╝
 ╚═╝ ╚═╝     ╚═╝  ╚═╝ ╚═════╝   ╚═╝   ╚═╝ ╚═════╝ ╚═╝  ╚═══╝    ╚═╝     ╚══════╝ ╚═════╝  ╚══╝╚══╝      ╚═╝ ╚═╝ 
";

        const string end_title = @"
 ██╗ ██╗     ███████╗██╗      ██████╗ ██╗    ██╗    ███████╗██╗███╗   ██╗██╗███████╗██╗  ██╗███████╗██████╗      ██╗ ██╗ 
████████╗    ██╔════╝██║     ██╔═══██╗██║    ██║    ██╔════╝██║████╗  ██║██║██╔════╝██║  ██║██╔════╝██╔══██╗    ████████╗
╚██╔═██╔╝    █████╗  ██║     ██║   ██║██║ █╗ ██║    █████╗  ██║██╔██╗ ██║██║███████╗███████║█████╗  ██║  ██║    ╚██╔═██╔╝
████████╗    ██╔══╝  ██║     ██║   ██║██║███╗██║    ██╔══╝  ██║██║╚██╗██║██║╚════██║██╔══██║██╔══╝  ██║  ██║    ████████╗
╚██╔═██╔╝    ██║     ███████╗╚██████╔╝╚███╔███╔╝    ██║     ██║██║ ╚████║██║███████║██║  ██║███████╗██████╔╝    ╚██╔═██╔╝
 ╚═╝ ╚═╝     ╚═╝     ╚══════╝ ╚═════╝  ╚══╝╚══╝     ╚═╝     ╚═╝╚═╝  ╚═══╝╚═╝╚══════╝╚═╝  ╚═╝╚══════╝╚═════╝      ╚═╝ ╚═╝ 
";

        #endregion  

        #region property

        public string Name { get; set; }
        public string Description { get; set; }

        #endregion

        #region api

        public void Execute()
        {
            LogConsoleStart();

            foreach (ActionBase action in _actionList)
            {
                ProcessAction(action);
            }

            LogConsoleEnd();
        }

        public void LoadFromFile(string filePath)
        {
            _actionList = new List<ActionBase>();

            ProjectParser parser = new ProjectParser(this, this._actionList);
            parser.ParseProjectFromFile(filePath);
        }

        #endregion

        #region private methods

        private void LogConsoleStart()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Project.project_title);
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"Run project : {this.Name} \n" +
                $"Description : {this.Description} ");

            Console.ResetColor();
        }

        private void LogConsoleEnd()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine(Project.end_title);
            Console.ResetColor();
        }

        private void ProcessAction(ActionBase action)
        {
            if (action.Enabled)
            {
                action.DoAction();
            }
            else
            {
                LogDisabledAction(action);
            }
        }

        private void LogDisabledAction(ActionBase action)  
        {
            Console.ForegroundColor = ConsoleColor.Red;

            Console.WriteLine($" \nExecute Type [ {Enum.GetName(typeof(ActionType), action.Type)} ]" +
                $" Name [ {this.Name} ] " +
             $"Description [ {this.Description} ] is disabled" );

            Console.ResetColor();
        }

        #endregion

        #region members

        private List<ActionBase> _actionList;

        #endregion

    }
}
