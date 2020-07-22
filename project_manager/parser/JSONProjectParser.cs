using commandlib;
using mroot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using System.Xml;

namespace project_manager
{
    internal class JSONProjectParser : ProjectParser
    {
        public JSONProjectParser(Project projectToPopulate, List<ActionBase> projectActions) : base(projectToPopulate, projectActions)
        {
        }

        #region api

        public override void ParseProjectFromFile(string file_path)
        {

            string jsonText = PreprocessJsonFile(File.ReadAllText(file_path));
            JObject jsonObject = JObject.Parse(jsonText);

            _mainProject.Name = jsonObject["project"]["name"].ToString();
            _mainProject.Description = jsonObject["project"]["desc"].ToString();

            var tookenList = jsonObject["project"]["execution"].ToList<JToken>();
            foreach (var item in tookenList)
            {
                ParseToken(item);
            }
        }

        private void ParseToken(JToken item)
        {
            var actionType = (ActionType)Enum.Parse(typeof(ActionType), DenumberActionWord(item.Value<JProperty>().Name), true);

            var tokenType = item.Value<JProperty>().Value.Type;
            if (tokenType == JTokenType.Object)
            {
                var actionJson = item.Value<JProperty>().Value.ToObject<JObject>();

                string name = actionJson.ContainsKey("name") ? actionJson["name"].ToString() : null;
                string desc = actionJson.ContainsKey("desc") ? actionJson["desc"].ToString() : null;
                bool? enable = actionJson.ContainsKey("enabled") ? (bool?)(Convert.ToBoolean(actionJson["desc"])) : null;



                ActionBase action = ParseObjectAction(actionJson, actionType);

                action.Type = actionType;
                action.Name = name ?? action.Name;
                action.Enabled = enable ?? action.Enabled;
                action.Description = desc ?? action.Description;

                this._actions.Add(action);
            }
            else
            {
                ActionBase action = ParseObjectAction_SHORT(item.Value<JProperty>(), actionType);

                action.Type = actionType;

                this._actions.Add(action);
            }






        }



        #endregion


        private IEnumerable<string> ActionTypeWordList
        {
            get
            {
                foreach (var actiontype in Enum.GetValues(typeof(ActionType)))
                {
                    yield return actiontype.ToString().ToLower();
                }
            }
        }

        private string DenumberActionWord(string textAction)
        {
            string pattern = @"_\d+$";
            string replacement = "";

            Regex regex = new Regex(pattern);
            return regex.Replace(textAction, replacement);
        }

        private string NumberActionWord(string text, int lineNumber)
        {
            foreach (var actionTypeWord in ActionTypeWordList)
            {
                text = text.Replace(actionTypeWord, $"{actionTypeWord}_{lineNumber}");
            }

            return text;
        }

        private string PreprocessJsonFile(string text)
        {
            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = NumberActionWord(lines[i], i);
            }

            return string.Join("\n", lines);

        }

        private ActionBase ParseObjectAction_SHORT(JProperty node, ActionType action_type)
        {
            switch (action_type)
            {
                case ActionType.Wait:
                    int waittime = Convert.ToInt32( (node.Value.ToString()));

                    WaitAction wait = new WaitAction();
                    wait.Milliseconds = waittime;
                    return wait;

                case ActionType.ShowDialog:

                    ShowDialog showDialog = new ShowDialog();
                    showDialog.Message = MRoot.Instance.SubstituteEnviroVariables(node.Value.ToString());
                    return showDialog;

                case ActionType.CopyFolder:
                    throw new NotSupportedException(); 
            }

            return null;
        }

        private ActionBase ParseObjectAction(JObject node, ActionType action_type)
        {

            switch (action_type)
            {
                case ActionType.Wait:
                    int waittime = (node["duration_ms"].Value<Int32>());

                    WaitAction wait = new WaitAction();
                    wait.Milliseconds = waittime;
                    return wait;

                case ActionType.ShowDialog:

                    ShowDialog showDialog = new ShowDialog();
                    showDialog.Message = MRoot.Instance.SubstituteEnviroVariables(node["message"].ToString());
                    if (node.ContainsKey("messagetype"))
                    {
                        showDialog.MessageType = (ShowDialog.Type)Enum.Parse(typeof(ShowDialog.Type), node["messagetype"].ToString(), true);
                    }
                    return showDialog;

                case ActionType.CopyFolder:

                    string source = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());
                    string destination = MRoot.Instance.SubstituteEnviroVariables(node["destination"].ToString());
                    string file_pattern = node.ContainsKey("desc") ? node["copy_filepattern"].ToString() : null;
                    string dir_pattern = node.ContainsKey("desc") ? node["copy_dirpattern"].ToString() : null;

                    CopyFolder copyFolder = new CopyFolder();
                    copyFolder.Source = source ?? copyFolder.Source;
                    copyFolder.Destination = destination ?? copyFolder.Destination;
                    copyFolder.CopyFilePattern = file_pattern ?? copyFolder.CopyFilePattern;
                    copyFolder.CopyDirPattern = dir_pattern ?? copyFolder.CopyDirPattern;

                    return copyFolder;
            }

            return null;
        }
    }
}
