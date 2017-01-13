using ConsoleBackUp.Infrastructure;
using EFolderDomain.Infrastructure;
using EFolderDomain.Interfaces;
using Microsoft.Owin.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleBackUp
{
    class Program
    {
        static void Main(string[] args)
        {
            // Initialize logger
            string configFilePath = AppDomain.CurrentDomain.SetupInformation.ConfigurationFile;
            Logger.InitLogger(configFilePath);

            IConfigurationMgr conf = new ConfMgr();

            // This code generates new portion of test users with todo items for JAVA service.
            //bool res = UserServiceGenerator.Generate(conf.GetToDoItemServiceUri(), conf.GetToDoItemServicePath()).Result;

            ClearToDoRepository();

            string appServiceUri = conf.GetAppServiceUri();
            WebApp.Start<Startup>(appServiceUri);
            Console.WriteLine("Press any keys to exit.");
            Console.ReadLine();
        }

        static void ClearToDoRepository()
        {
            string repoPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "ToDoRep");
            if (Directory.Exists(repoPath))
            {
                foreach(string file in Directory.EnumerateFiles(repoPath))
                {
                    File.Delete(file);
                }
            }
        }
    }
}
