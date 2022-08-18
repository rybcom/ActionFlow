using System;
using System.IO;

namespace commandlib
{
    public class DeleteFile : ActionBase
    {

        #region property

        public string SourceFile { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {
            base.DoAction();

            DeleteAction(SourceFile);
        }

        #endregion

        #region private methods

        private static void DeleteAction(string filePath)
        {
            FileInfo file = new FileInfo(filePath);
            if(file.Exists == false)
            {
                Console.Error.WriteLine($"file : {filePath} not exists");
                return;
            }

            Console.WriteLine($"\tdelete file [{filePath}]");
            file.Delete();
        }

        #endregion

    }
}
