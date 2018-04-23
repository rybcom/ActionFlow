using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using project_manager;
using mroot;

namespace action_flow
{
    class Program
    {
        static void Main(string[] args)
        {
            if (args.Length < 1)
            {
                Console.WriteLine("No project path");
            }
            else
            {
                string project_path = MRoot.Instance.SubstituteEnviroVariables(args[0]);

                Project project = new Project();

                project.LoadFromFile(project_path);

                project.Execute();
            }
        }
    }
}
