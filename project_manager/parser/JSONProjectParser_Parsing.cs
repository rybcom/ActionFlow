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
            switch (action_type)
            {
                case ActionType.Execute:

                    string filename = mroot.substitue_enviro_vars(node.Value.ToString());

                    return new ExecuteProcess
                    {
                        FileName = filename,
                        Params = "",
                        OnlyIfNotRunning = true
                    };

                case ActionType.Wait:
                    int waittime = Convert.ToInt32((node.Value.ToString()));

                    return new WaitAction
                    {
                        Milliseconds = waittime
                    };

                case ActionType.ShowDialog:

                    return new ShowDialog
                    {
                        Message = mroot.substitue_enviro_vars(node.Value.ToString())
                    };
                    
                case ActionType.NewFolder:
                    {
                        string folder = mroot.substitue_enviro_vars(node.Value.ToString());
                        return new NewFolder
                        {
                            FolderPath = folder
                        };
                    }

                case ActionType.CopyFolder:

                    throw new NotSupportedException("Simple CopyFolder is not allowed");

                case ActionType.CopyFile:

                    throw new NotSupportedException("Simple CopyFile is not allowed");

                case ActionType.DeleteFile:
                    {
                        string sourceFile = mroot.substitue_enviro_vars(node.Value.ToString());

                        return new DeleteFile
                        {
                            SourceFile = sourceFile
                        };
                    }

                case ActionType.DeleteFiles:
                    {
                        string sourceFolder = mroot.substitue_enviro_vars(node.Value.ToString());
                        string delete_pattern = "(.)";
                        bool recursive_delete = false;

                        return new DeleteFiles
                        {
                            SourceFolder = sourceFolder,
                            DeletePattern = delete_pattern,
                            RecursiveDelete = recursive_delete
                        };
                    }

                case ActionType.DeleteFolder:
                    {
                        string dirPath = mroot.substitue_enviro_vars(node.Value.ToString());

                        return new DeleteFolder
                        {
                            FolderPath = dirPath
                        };
                    }

                case ActionType.DeleteFolders:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node.Value.ToString());
                        string delete_pattern = "(.)";

                        return new DeleteFolders
                        {
                            SourceFolder = sourceFolder,
                            DeletePattern = delete_pattern
                        };
                    }

                case ActionType.ZipFolder:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node.Value.ToString());
                        string zipfile_destination = System.IO.Path.ChangeExtension(sourceFolder, ".zip");

                        return new ZipFolder
                        {
                            SourceFolder = sourceFolder,
                            DestinationZip = zipfile_destination
                        };
                    }
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

                    string filename = mroot.substitue_enviro_vars(node["filename"].ToString());
                    string paramxs = node.ContainsKey("params") ? mroot.substitue_enviro_vars(node["params"].ToString()) : "";
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
                    showDialog.Message = mroot.substitue_enviro_vars(node["message"].ToString());
                    if (node.ContainsKey("messagetype"))
                    {
                        showDialog.MessageType = (ShowDialog.Type)Enum.Parse(typeof(ShowDialog.Type), node["messagetype"].ToString(), true);
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
                    string file_pattern = node.ContainsKey("desc") ? node["copy_filepattern"].ToString() : null;
                    string dir_pattern = node.ContainsKey("desc") ? node["copy_dirpattern"].ToString() : null;

                    CopyFolder copyFolder = new CopyFolder();
                    copyFolder.Source = source ?? copyFolder.Source;
                    copyFolder.Destination = destination ?? copyFolder.Destination;
                    copyFolder.CopyFilePattern = file_pattern ?? copyFolder.CopyFilePattern;
                    copyFolder.CopyDirPattern = dir_pattern ?? copyFolder.CopyDirPattern;

                    return copyFolder;

                case ActionType.CopyFile:

                    source = mroot.substitue_enviro_vars(node["source"].ToString());
                    destination = mroot.substitue_enviro_vars(node["destination"].ToString());

                    CopyFile copyFile = new CopyFile();
                    copyFile.Source = source;
                    copyFile.Destination = destination;
                    return copyFile;

                case ActionType.DeleteFile:
                    {
                        string sourceFile = mroot.substitue_enviro_vars(node["source"].ToString());

                        DeleteFile delteFiles = new DeleteFile();
                        delteFiles.SourceFile = sourceFile;
                        return delteFiles;
                    }

                case ActionType.DeleteFiles:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";
                        bool recursive_delete = node.ContainsKey("recursive") ? node["recursive"].Value<bool>() : false;

                        DeleteFiles delteFiles = new DeleteFiles();
                        delteFiles.SourceFolder = sourceFolder;
                        delteFiles.DeletePattern = delete_pattern;
                        delteFiles.RecursiveDelete = recursive_delete;
                        return delteFiles;
                    }

                case ActionType.DeleteFolder:
                    {
                        string dirPath = mroot.substitue_enviro_vars(node["source"].ToString());

                        DeleteFolder deleteFolder = new DeleteFolder();
                        deleteFolder.FolderPath = dirPath;
                        return deleteFolder;
                    }

                case ActionType.DeleteFolders:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string delete_pattern = node.ContainsKey("pattern") ? node["pattern"].ToString() : "(.)";

                        DeleteFolders deleteFolders = new DeleteFolders();
                        deleteFolders.SourceFolder = sourceFolder;
                        deleteFolders.DeletePattern = delete_pattern;
                        return deleteFolders;
                    }

                case ActionType.ZipFolder:
                    {

                        string sourceFolder = mroot.substitue_enviro_vars(node["source"].ToString());
                        string zipfile_destination = mroot.substitue_enviro_vars(node["zipfile"].ToString());

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