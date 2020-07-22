using commandlib;
using mroot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;

namespace project_manager
{
    internal abstract class ProjectParser
    {

        #region api

        public ProjectParser(Project projectToPopulate, List<ActionBase> projectActions)
        {
            _mainProject = projectToPopulate;
            _actions = projectActions;
        }

        public abstract void ParseProjectFromFile(string file_path);

        #endregion


        #region members

        protected Project _mainProject;
        protected List<ActionBase> _actions;

        #endregion


    }
}
