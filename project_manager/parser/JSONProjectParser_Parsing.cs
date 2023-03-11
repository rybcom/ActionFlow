using commandlib;
using mroot_lib;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;

namespace project_manager
{
    internal partial class JSONProjectParser : ProjectParser
    {
        private ActionBase ParseObjectAction(JProperty node, ActionType action_type)
        {

            string textValue = mroot.substitue_enviro_vars(node.Value.ToString());

            switch (action_type)
            {
                case ActionType.Execute:
                    return ExecuteProcess.GetCommandFrom(textValue);

                case ActionType.Execute_If:
                    throw new NotSupportedException("Simple Execute_If is not allowed");

                case ActionType.Wait:
                    return WaitAction.GetCommandFrom(textValue);

                case ActionType.ShowDialog:
                    return ShowDialog.GetCommandFrom(textValue);

                case ActionType.NewFolder:
                    return NewFolder.GetCommandFrom(textValue);

                case ActionType.CopyFolder:
                    return CopyFolder.GetCommandFrom(textValue);

                case ActionType.CopyFile:
                    return CopyFile.GetCommandFrom(textValue);

                case ActionType.DeleteFile:
                    return DeleteFile.GetCommandFrom(textValue);

                case ActionType.DeleteFiles:
                    return DeleteFiles.GetCommandFrom(textValue);

                case ActionType.DeleteFolder:
                    return DeleteFolder.GetCommandFrom(textValue);

                case ActionType.DeleteFolders:
                    return DeleteFolders.GetCommandFrom(textValue);

                case ActionType.ZipFolder:
                    return ZipFolder.GetCommandFrom(textValue);
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
                    {

                        var condition = (node["condition"].ToString());

                        if (condition.Equals("dialog"))
                        {
                            return ParseDialogControlFlow(node);
                        }

                        return null;
                    }

                case ActionType.Execute:
                    {
                        string filename = mroot.substitue_enviro_vars(node["filename"].ToString());
                        string paramxs = node.ContainsKey("params") ? mroot.substitue_enviro_vars(node["params"].ToString()) : "";
                        bool onlyIfnotRunning = node.ContainsKey("onlyIfNotRunning") ? node["onlyIfNotRunning"].Value<bool>() : false;

                        ExecuteProcess executeAction = new ExecuteProcess
                        {
                            FileName = filename,
                            Params = paramxs,
                            OnlyIfNotRunning = onlyIfnotRunning
                        };

                        return executeAction;
                    }

                case ActionType.Execute_If:
                    {
                        string condition = mroot.substitue_enviro_vars(node["condition"].ToString());
                        string filename = mroot.substitue_enviro_vars(node["filename"].ToString());
                        string paramxs = node.ContainsKey("params") ? mroot.substitue_enviro_vars(node["params"].ToString()) : "";
                        bool onlyIfnotRunning = node.ContainsKey("onlyIfNotRunning") ? node["onlyIfNotRunning"].Value<bool>() : false;

                        ExecuteIfProcess executeConditionalAction = new ExecuteIfProcess
                        {
                            ConditionExpression = condition,
                            FileName = filename,
                            Params = paramxs,
                            OnlyIfNotRunning = onlyIfnotRunning
                        };

                        return executeConditionalAction;
                    }

                case ActionType.Wait:
                    int waittime = (node["duration_ms"].Value<Int32>());

                    WaitAction wait = new WaitAction();
                    wait.Milliseconds = waittime;
                    return wait;

                case ActionType.ShowDialog:

                    ShowDialog showDialog = new ShowDialog();
                    showDialog.Message = mroot.substitue_enviro_vars(node["message"].ToString());
                    if (node.ContainsKey("messagetype"))
                    {
                        showDialog.MessageType = (ShowDialog.DialogType)Enum.Parse(typeof(ShowDialog.DialogType), node["messagetype"].ToString(), true);
                    }
                    return showDialog;

                case ActionType.NewFolder:
                    {
                        string folder = mroot.substitue_enviro_vars(node["path"].ToString());
                        return new NewFolder
                        {
                            FolderPath = folder
                        };
                    }

                case ActionType.CopyFolder:

                    string source = mroot.substitue_enviro_vars(node["source"].ToString());
                    string destination = mroot.substitue_enviro_vars(node["destination"].ToString());
                    string file_pattern = node.ContainsKey("copy_filepattern") ? node["copy_filepattern"].ToString() : null;
                    string dir_pattern = node.ContainsKey("copy_dirpattern") ? node["copy_dirpattern"].ToString() : null;

                    CopyFolder copyFolder = new CopyFolder();
                    copyFolder.Source = source ?? copyFolder.Source;
                    copyFolder.Destination = destination ?? copyFolder.Destination;
                    copyFolder.CopyFilePattern = file_pattern ?? copyFolder.CopyFilePattern;
                    copyFolder.CopyDirPattern = dir_pattern ?? copyFolder.CopyDirPattern;

                    return copyFolder;

                case ActionType.CopyFile:

                    source = mroot.substitue_enviro_vars(node["source"].ToString());
                    destination = mroot.substitue_enviro_vars(node["destination"].ToString());

                    CopyFile copyFile = new CopyFile
                    {
                        Source = source,
                        Destination = destination
                    };
                    return copyFile;

                case ActionType.DeleteFile:
                    {
                        string sourceFile = mroot.substitue_enviro_vars(node["source"].ToString());

                        DeleteFile delteFiles = new DeleteFile
                        {
                            SourceFile = sourceFile
                        };
                        return delteFiles;
                    }

                case ActionType.DeleteFiles:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";
                        bool recursive_delete = node.ContainsKey("recursive") ? node["recursive"].Value<bool>() : false;

                        DeleteFiles delteFiles = new DeleteFiles
                        {
                            SourceFolder = sourceFolder,
                            DeletePattern = delete_pattern,
                            RecursiveDelete = recursive_delete
                        };
                        return delteFiles;
                    }

                case ActionType.DeleteFolder:
                    {
                        string dirPath = mroot.substitue_enviro_vars(node["source"].ToString());

                        DeleteFolder deleteFolder = new DeleteFolder
                        {
                            FolderPath = dirPath
                        };
                        return deleteFolder;
                    }

                case ActionType.DeleteFolders:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";

                        DeleteFolders deleteFolders = new DeleteFolders
                        {
                            SourceFolder = sourceFolder,
                            DeletePattern = delete_pattern
                        };
                        return deleteFolders;
                    }

                case ActionType.ZipFolder:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string zipfile_destination = mroot.substitue_enviro_vars(node["zipfile"].ToString());

                        ZipFolder zipFolder = new ZipFolder
                        {
                            SourceFolder = sourceFolder,
                            DestinationZip = zipfile_destination
                        };
                        return zipFolder;
                    }

            }

            return null;
        }
    }
}