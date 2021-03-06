﻿using System;
using System.IO;
using System.IO.Compression;

namespace commandlib
{
    public class ZipFolder : ActionBase
    {
        #region property

        public string SourceFolder { get; set; }

        public string DestinationZip { get; set; }

        #endregion

        #region overridden

        public override void DoAction()
        {
            base.DoAction();

            if (File.Exists(DestinationZip))
            {
                File.Delete(DestinationZip);
            }

            if(Directory.Exists(SourceFolder)==false)
            {
                Console.Error.WriteLine($"folder : {SourceFolder} not exists");
                return;
            }

            Console.WriteLine($"\tpacking folder [{this.SourceFolder}] to [{this.DestinationZip}]");

            ZipFile.CreateFromDirectory(SourceFolder, DestinationZip);
        }

        #endregion

    }
}
