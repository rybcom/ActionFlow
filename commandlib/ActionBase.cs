using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace commandlib
{
   public abstract class ActionBase
    {
        public string Name { get; set; }
        public abstract void DoAction();
    }
}
