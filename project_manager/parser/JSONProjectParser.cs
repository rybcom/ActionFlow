using commandlib;
using mroot;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.AccessControl;
using System.Text.RegularExpressions;
using System.Xml;

namespace project_manager
{
    internal partial class JSONProjectParser : ProjectParser
    {

        #region api

        public JSONProjectParser(Project projectToPopulate, List<ActionBase> projectActions) : base(projectToPopulate, projectActions)
        {
        }


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


        #endregion


        #region private

   
        private void ParseToken(JProperty jsonProperty, List<ActionBase> activeList)
        {
            var actionType = (ActionType)Enum.Parse(typeof(ActionType), DenumberActionWord(jsonProperty.Name), true);

            JToken value = jsonProperty.Value;

            if (value.Type == JTokenType.Object)

            {

                JObject jsonObject = value.ToObject<JObject>();

                ActionBase action = ParseObjectAction(jsonObject, actionType);
                action.Type = actionType;
                SetBaseDataForAction(jsonObject, action);

                activeList.Add(action);

            }
            else
            {

                ActionBase action = ParseObjectAction(jsonProperty, actionType);

                action.Type = actionType;

                activeList.Add(action);
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
                text = Regex.Replace(text,$@"\b{actionTypeWord}\b",
                    $"{actionTypeWord}_{lineNumber}");
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

        #endregion

    }

}
