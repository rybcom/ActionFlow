using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandlib
{
    public enum ActionType
    {
        CopyFolder,
        CopyFile,
        ZipFolder
    }


   public abstract class ActionBase
    {

        public string Name { get; set; }
        public string Description { get; set; }
        public ActionType Type { get; set; }

        public virtual void DoAction()
        {
            Console.WriteLine($" \nExecute Type [ {Enum.GetName(typeof(ActionType),Type)} ]" +
                $" Name [ {this.Name} ] " +
             $"Description [ {this.Description} ]");
        }
    }
}
