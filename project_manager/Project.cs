using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using commandlib;

namespace project_manager
{
    public class Project
    {
        #region property

        public string Name { get; set; }
        public string Description{ get; set; }

        #endregion

        #region api

        public void Execute()
        {
            foreach(ActionBase action in _actionList)
            {
                action.DoAction();
            }
        }

        public void LoadFromFile(string filePath)
        {
            _actionList = new List<ActionBase>();

            ProjectParser parser = new ProjectParser(this,this._actionList);
            parser.ParseProjectFromFile(filePath);
        }

        #endregion

        #region members

        private List<ActionBase> _actionList;

        #endregion

    }
}
