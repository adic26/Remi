using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Windows.Forms;

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
                UpdateTestConfig(argsList[argsList.IndexOf("-seq") + 1], argsList[argsList.IndexOf("-seq") + 2], bool.Parse(argsList[argsList.IndexOf("-seq") + 3]));
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

        
        private static void UpdateTestConfig(string sourceFolder, string destinationFolder, bool storeInDatabase)
        {
            //TODO: move to separate application

            //if (!Directory.Exists(destinationFolder))
            //    Directory.CreateDirectory(destinationFolder);

            //IConfigGroup<SequenceConfig> cfgGroup = ConfigManager<SequenceConfig>.GetInstance("TestClient", Application.ProductVersion, @"C:\temp\RemiSettingsTest").GetConfigGroup();

            //Trace.WriteLine(string.Format("Detected {0} SequenceConfig objects in the database", cfgGroup.Count()));

            //foreach (string sourceFilePath in Directory.EnumerateFiles(sourceFolder))
            //{
            //    string sourceFileName = Path.GetFileName(sourceFilePath);
            //    if (sourceFileName == null)
            //        throw new ArgumentException(sourceFilePath + " is not a valid file path.");

            //    string destinationFilePath = Path.Combine(destinationFolder, sourceFileName);

            //    File.Copy(sourceFilePath, destinationFilePath, true);
            //    cfgGroup.Add(new SequenceConfig(destinationFilePath) { StoreInDatabase = storeInDatabase });
            //}
            //cfgGroup.Save();
        }
    }
}
