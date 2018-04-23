using commandlib;
using mroot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using commandlib;

namespace project_manager
{
    internal class ProjectParser
    {

        #region api

        public ProjectParser(Project projectToPopulate, List<ActionBase> projectActions)
        {
            _mainProject = projectToPopulate;
            _actions = projectActions;
        }

        public void ParseProjectFromFile(string file_path)
        {
            XmlDocument doc = new XmlDocument();
            doc.LoadXml(File.ReadAllText(file_path));

            XmlNode project_node = doc.DocumentElement.SelectSingleNode("/project");

            _mainProject.Name = project_node.Attributes["name"].Value;
            _mainProject.Description = project_node.Attributes["description"].Value;

            this._actions.Clear();
            XmlNodeList items = doc.DocumentElement.SelectNodes("/project/action");

            foreach (XmlNode node in items)
            {

                string action_name = node.Attributes["name"].Value;
                string action_desc = node.Attributes["desc"].Value;
                ActionType action_type = (ActionType)Enum.Parse(typeof(ActionType), node.Attributes["type"].Value, true); ;



                ActionBase action = ParseAction(node, action_type);

                action.Type = action_type;
                action.Name = action_name;
                action.Description = action_desc;

                this._actions.Add(action);
            }
        }

        private ActionBase ParseAction(XmlNode node, ActionType action_type)
        {

            switch (action_type)
            {
                case ActionType.CopyFolder:

                    string source = MRoot.Instance.SubstituteEnviroVariables(node.Attributes["source"].Value);
                    string destination = MRoot.Instance.SubstituteEnviroVariables(node.Attributes["destination"].Value);
                    string pattern = node.Attributes["copy_pattern"].Value;

                    CopyFolder copyFolder = new CopyFolder();
                    copyFolder.Source = source;
                    copyFolder.Destination = destination;
                    copyFolder.CopyPattern = pattern;

                    return copyFolder;

                case ActionType.CopyFile:
                    return null;

                case ActionType.ZipFolder:

                    return null;

                default:
                    return null;
            }
        }




        #endregion


        #region members

        private Project _mainProject;
        private List<ActionBase> _actions;

        #endregion


    }
}
