using commandlib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace project_manager
{
    internal  class ProjectParser
    {

        #region api

        public ProjectParser(Project projectToPopulate, List<ActionBase> projectActions)
        {
            _mainProject = projectToPopulate;
            _actions = projectActions;
        }

        public void ParseProjectFromFile(string file_path)
        {
            _mainProject.Name = "lala";
            _mainProject.Description = "juchuiuuuuuuuuuuuuuu";

            _actions.Add(new ZipFolder());
            _actions.Add(new CopyFolder());

        }

        #endregion


        #region members

        private Project _mainProject;
        private List<ActionBase> _actions;

        #endregion


    }
}
