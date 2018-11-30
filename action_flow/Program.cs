using System;
using project_manager;

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
                string project_path = args[0];

                Project project = new Project();

                project.LoadFromFile(project_path);

                project.Execute();
            }
        }
    }
}
