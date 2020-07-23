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
                case ActionType.Execute:

                    string filename = MRoot.Instance.SubstituteEnviroVariables(node.Value.ToString());

                    ExecuteProcess executeAction = new ExecuteProcess();
                    executeAction.FileName = filename;
                    executeAction.Params = "";
                    executeAction.OnlyIfNotRunning = true;

                    return executeAction;

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

                    throw new NotSupportedException("Simple CopyFolder is not allowed");

                case ActionType.CopyFile:

                    throw new NotSupportedException("Simple CopyFile is not allowed");

                case ActionType.DeleteFile:
                    {
                        string sourceFile = MRoot.Instance.SubstituteEnviroVariables(node.Value.ToString());

                        DeleteFile delteFiles = new DeleteFile();
                        delteFiles.SourceFile = sourceFile;
                        return delteFiles;
                    }

                case ActionType.DeleteFiles:
                    {
                        string sourceFolder = MRoot.Instance.SubstituteEnviroVariables(node.Value.ToString());
                        string delete_pattern = "(.)";
                        bool recursive_delete = false;

                        DeleteFiles delteFiles = new DeleteFiles();
                        delteFiles.SourceFolder = sourceFolder;
                        delteFiles.DeletePattern = delete_pattern;
                        delteFiles.RecursiveDelete = recursive_delete;
                        return delteFiles;
                    }

                case ActionType.DeleteFolders:
                    {

                        string sourceFolder = MRoot.Instance.SubstituteEnviroVariables(node.Value.ToString());
                        string delete_pattern = "(.)";

                        DeleteFolders deleteFolders = new DeleteFolders();
                        deleteFolders.SourceFolder = sourceFolder;
                        deleteFolders.DeletePattern = delete_pattern;
                        return deleteFolders;
                    }

                case ActionType.ZipFolder:

                    throw new NotSupportedException("Simple ZipFolder is not allowed");

            }

            return null;
        }


        private ControlFlow<DialogCondition, DialogResultYESNO> ParseDialogControlFlow(JObject node)
        {

            ControlFlow<DialogCondition, DialogResultYESNO> flow = new ControlFlow<DialogCondition, DialogResultYESNO>();

            flow.Condition.DialogText = node.ContainsKey("dialogtext") ?
                node["dialogtext"].ToString() : flow.Condition.DialogText;

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

                case ActionType.Execute:

                    string filename = MRoot.Instance.SubstituteEnviroVariables(node["filename"].ToString());
                    string paramxs = node.ContainsKey("params") ? MRoot.Instance.SubstituteEnviroVariables(node["params"].ToString()) : "";
                    bool onlyIfnotRunning = node.ContainsKey("onlyIfNotRunning") ? node["params"].Value<bool>() : true;

                    ExecuteProcess executeAction = new ExecuteProcess();
                    executeAction.FileName = filename;
                    executeAction.Params = paramxs;
                    executeAction.OnlyIfNotRunning = onlyIfnotRunning;

                    return executeAction;


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

                case ActionType.CopyFile:

                    source = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());
                    destination = MRoot.Instance.SubstituteEnviroVariables(node["destination"].ToString());

                    CopyFile copyFile = new CopyFile();
                    copyFile.Source = source;
                    copyFile.Destination = destination;
                    return copyFile;

                case ActionType.DeleteFile:
                    {
                        string sourceFile = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());

                        DeleteFile delteFiles = new DeleteFile();
                        delteFiles.SourceFile = sourceFile;
                        return delteFiles;
                    }

                case ActionType.DeleteFiles:
                    {

                        string sourceFolder = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";
                        bool recursive_delete = node.ContainsKey("recursive") ? node["recursive"].Value<bool>() : false;

                        DeleteFiles delteFiles = new DeleteFiles();
                        delteFiles.SourceFolder = sourceFolder;
                        delteFiles.DeletePattern = delete_pattern;
                        delteFiles.RecursiveDelete = recursive_delete;
                        return delteFiles;
                    }

                case ActionType.DeleteFolders:
                    {

                        string sourceFolder = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";

                        DeleteFolders deleteFolders = new DeleteFolders();
                        deleteFolders.SourceFolder = sourceFolder;
                        deleteFolders.DeletePattern = delete_pattern;
                        return deleteFolders;
                    }

                case ActionType.ZipFolder:
                    {

                        string sourceFolder = MRoot.Instance.SubstituteEnviroVariables(node["source"].ToString());
                        string zipfile_destination = MRoot.Instance.SubstituteEnviroVariables(node["zipfile"].ToString());

                        ZipFolder zipFolder = new ZipFolder();
                        zipFolder.SourceFolder = sourceFolder;
                        zipFolder.DestinationZip = zipfile_destination;
                        return zipFolder;
                    }

            }

            return null;
        }
    }
}