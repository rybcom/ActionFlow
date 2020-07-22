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


        private ControlFlow<DialogCondition, DialogResultYESNO> ParseDialogControlFlow(JObject node)
        {

            ControlFlow<DialogCondition, DialogResultYESNO> flow = new ControlFlow<DialogCondition, DialogResultYESNO>();
            foreach (var result in Enum.GetValues(typeof(DialogResultYESNO)))
            {
                var resultPath = (node[result.ToString().ToLower()]);

                var resultPathActions = new List<ActionBase>();
                flow.ActionControlFlowList[(DialogResultYESNO)result] = resultPathActions;

                foreach (var property in resultPath.ToObject<JObject>().Properties())
                {
                    ParseToken(property, resultPathActions);
                }
            }

            return flow;

        }

        private ActionBase ParseObjectAction(JObject node, ActionType action_type)
        {

            switch (action_type)
            {
                case ActionType.ControlFlow:
                    var condition = (node["condition"].ToString());

                    if (condition.Equals("dialog"))
                    {
                        return ParseDialogControlFlow(node);
                    }


                    return null;

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