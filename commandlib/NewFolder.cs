using System;
using System.IO;

namespace commandlib
{
    public class NewFolder: ActionBase
    {

        #region property

        public string FolderPath{ get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {

            base.DoAction();
            CreateFolder();

        }

        #endregion

        #region private methods

        private void CreateFolder()
        {
            if (Directory.Exists(FolderPath))
            {
                Console.Error.WriteLine($"folder : {FolderPath} already exists");
                return;
            }
            
            Console.WriteLine($"\tcreating folder [{FolderPath}]");
            Directory.CreateDirectory(FolderPath);
        }

        #endregion

    }
}
