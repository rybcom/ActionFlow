﻿using commandlib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

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
                bool action_enabled = true;

                if (node.Attributes["enabled"]!=null)
                {
                    action_enabled = Convert.ToBoolean(node.Attributes["enabled"].Value);
                }

                ActionType action_type = (ActionType)Enum.Parse(typeof(ActionType), node.Attributes["type"].Value, true); ;



                ActionBase action = ParseAction(node, action_type);

                action.Type = action_type;
                action.Name = action_name;
                action.Enabled = action_enabled;
                action.Description = action_desc;

                this._actions.Add(action);
            }
        }

        private ActionBase ParseAction(XmlNode node, ActionType action_type)
        {

            switch (action_type)
            {
                case ActionType.Wait:
                    int waittime=Convert.ToInt32(node.Attributes["duration_ms"].Value);

                    WaitAction wait= new WaitAction();
                    wait.Milliseconds = waittime;
                    return wait;

                case ActionType.Execute:

                    string filename = node.Attributes["filename"].Value;
                    string paramxs = node.Attributes["params"].Value;

                    ExecuteProcess executeAction = new ExecuteProcess();
                    executeAction.FileName = filename;
                    executeAction.Params = paramxs;
                    return executeAction;


                case ActionType.CopyFolder:

                    string source = node.Attributes["source"].Value;
                    string destination = node.Attributes["destination"].Value;
                    string file_pattern = node.Attributes["copy_filepattern"].Value;
                    string dir_pattern = node.Attributes["copy_dirpattern"].Value;

                    CopyFolder copyFolder = new CopyFolder();
                    copyFolder.Source = source;
                    copyFolder.Destination = destination;
                    copyFolder.CopyFilePattern = file_pattern;
                    copyFolder.CopyDirPattern= dir_pattern;

                    return copyFolder;

                case ActionType.CopyFile:

                    source = node.Attributes["source"].Value;
                    destination = node.Attributes["destination"].Value;

                    CopyFile copyFile = new CopyFile();
                    copyFile.Source = source;
                    copyFile.Destination = destination;
                    return copyFile;

                case ActionType.DeleteFiles:

                    string sourceFolder = node.Attributes["source"].Value;
                    string delete_pattern = node.Attributes["delete_filepattern"].Value;
                    bool recursive_delete = Convert.ToBoolean(node.Attributes["recursive"].Value);

                    DeleteFiles delteFiles= new DeleteFiles();
                    delteFiles.SourceFolder = sourceFolder;
                    delteFiles.DeletePattern = delete_pattern;
                    delteFiles.RecursiveDelete = recursive_delete;
                    return delteFiles;

                case ActionType.DeleteFolders:

                    sourceFolder = node.Attributes["source"].Value;
                    delete_pattern = node.Attributes["delete_folderpattern"].Value;

                    DeleteFolders deleteFolders = new DeleteFolders();
                    deleteFolders.SourceFolder = sourceFolder;
                    deleteFolders.DeletePattern = delete_pattern;
                    return deleteFolders;

                case ActionType.ZipFolder:

                    sourceFolder = node.Attributes["source"].Value;
                    string zipfile_destination = node.Attributes["zipfile"].Value;

                    ZipFolder zipFolder = new ZipFolder();
                    zipFolder.SourceFolder = sourceFolder;
                    zipFolder.DestinationZip = zipfile_destination;
                    return zipFolder;


                case ActionType.ShowDialog:

                    ShowDialog showDialog = new ShowDialog();
                    showDialog.Message = node.Attributes["message"].Value;
                    showDialog.MessageType = (ShowDialog.Type)Enum.Parse(typeof(ShowDialog.Type), node.Attributes["messagetype"].Value, true);

                    return showDialog;

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
