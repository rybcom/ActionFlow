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

            var jsonExecution = jsonObject["project"]["execution"].ToObject<JObject>();

            foreach (var property in jsonExecution.Properties())
            {
                ParseToken(property, this._actions);
            }
        }

        private void SetBaseDataForAction(JObject actionJson, ActionBase action)
        {
            string name = actionJson.ContainsKey("name") ? actionJson["name"].ToString() : null;
            string desc = actionJson.ContainsKey("desc") ? actionJson["desc"].ToString() : null;
            bool? enable = actionJson.ContainsKey("enabled") ? (bool?)(Convert.ToBoolean(actionJson["enabled"].ToString())) : null;

            action.Name = name ?? action.Name;
            action.Enabled = enable ?? action.Enabled;
            action.Description = desc ?? action.Description;
        }
        private void ParseToken(JProperty itemProperty,List<ActionBase> activeList)
        {
            var actionType = (ActionType)Enum.Parse(typeof(ActionType), DenumberActionWord(itemProperty.Name), true);

            JToken value = itemProperty.Value;

            if (value.Type == JTokenType.Object)

            {

                var actionJson = value.ToObject<JObject>();

                ActionBase action = ParseObjectAction(actionJson, actionType);
                action.Type = actionType;
                SetBaseDataForAction(actionJson, action);

                activeList.Add(action);

            }
            else
            {

                ActionBase action = ParseObjectAction(itemProperty, actionType);

                action.Type = actionType;

                activeList.Add(action);
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
            if (text.Trim().StartsWith("{") == false)
            {
                text = "{\n" + text + "}\n";
            }

            string[] lines = text.Split('\n');

            for (int i = 0; i < lines.Count(); i++)
            {
                lines[i] = NumberActionWord(lines[i], i);
            }

            return string.Join("\n", lines);

        }

        private ActionBase ParseObjectAction(JProperty node, ActionType action_type)
        {
            switch (action_type)
            {
                case ActionType.Wait:
                    int waittime = Convert.ToInt32((node.Value.ToString()));

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
                case ActionType.ControlFlow:
                    var condition = (node["condition"].ToString());

                    ControlFlow<DialogCondition, DialogResultYESNO> flow = new ControlFlow<DialogCondition, DialogResultYESNO>();
                    foreach(var result in Enum.GetValues(typeof(DialogResultYESNO)))
                    {
                        var resultPath = (node[result.ToString().ToLower()]);

                        var resultPathActions = new List<ActionBase>();
                        flow.ActionControlFlowList[(DialogResultYESNO)result] = resultPathActions;
                        
                        foreach(var property in resultPath.ToObject<JObject>().Properties())
                        {
                            ParseToken(property, resultPathActions);
                        }
                    }
                    return flow;

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
