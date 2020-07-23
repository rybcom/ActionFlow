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

        private static void DeleteAction(string ditPath)
        {
            DirectoryInfo dirInfo = new DirectoryInfo(ditPath);
            if(dirInfo.Exists == false)
            {
                Console.Error.WriteLine($"folder : {ditPath} not exists");
                return;
            }
            dirInfo.Delete(true);
        }

        #endregion

    }
}
