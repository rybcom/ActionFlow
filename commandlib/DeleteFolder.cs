using System;
using System.IO;

namespace commandlib
{
    public class DeleteFolder: ActionBase
    {

        #region property

        public string FolderPath { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {
            base.DoAction();

            DeleteAction(FolderPath);
        }

        #endregion

        #region private methods

        private static void DeleteAction(string dirPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(dirPath);
            if(dirInfo.Exists == false)
            {
                Console.Error.WriteLine($"folder : {dirPath} not exists");
                return;
            }

            Console.WriteLine($"delete folder [{dirPath}]");
            dirInfo.Delete(recursive:true);
        }

        #endregion

    }
}
