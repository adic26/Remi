using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TsdLib.CodeGenerator;
using TsdLib.Configuration;

namespace TestClient
{
    class TestClientProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Contains("-seq"))
            {
                Trace.WriteLine("Updating Sequence Config on the database");
                List<string> argsList = args.ToList();
                UpdateTestConfig(argsList[argsList.IndexOf("-seq") + 1], bool.Parse(argsList[argsList.IndexOf("-seq") + 2]));
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool devMode = args.Length > 0 && args[0] == "-d";

            Controller c = new Controller(devMode);
            
            if (c.View is Form)
                Application.Run(c.View as Form);

            //TODO: figure out how to launch non-form view

            Console.WriteLine("Done");
        }

        
        private static void UpdateTestConfig(string sourceFolder, bool storeInDatabase)
        {
            //TODO: move to separate application

            ConfigManager manager = new ConfigManager("TestClient", Application.ProductVersion, new DatabaseFolderConnection(@"C:\temp\RemiSettingsTest"));
            
            IConfigGroup<Sequence> cfgGroup = manager.GetConfigGroup<Sequence>();

            Trace.WriteLine(string.Format("Detected {0} SequenceConfig objects in the database", cfgGroup.Count()));

            foreach (string sourceFilePath in Directory.EnumerateFiles(sourceFolder))
            {
                Trace.WriteLine("Pushing " + sourceFilePath);
                string sourceFileName = Path.GetFileName(sourceFilePath);
                if (sourceFileName == null)
                    throw new ArgumentException(sourceFilePath + " is not a valid file path.");

                cfgGroup.Add(new Sequence
                {
                    Name = Path.GetFileNameWithoutExtension(sourceFilePath),
                    StoreInDatabase = storeInDatabase,
                    TestSequenceSourceCode = File.ReadAllText(sourceFilePath)
                });
            }
            cfgGroup.Save();
        }
    }
}
