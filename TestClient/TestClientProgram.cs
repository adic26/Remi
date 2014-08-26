using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
//using TestClient.TestSequences;
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

            if (args.Contains("-t"))
            {
                Trace.WriteLine("Pushing TestConfig to Remi");

                List<string> argsList = args.ToList();

                string fileName = argsList[argsList.IndexOf("-t") + 1];
                Trace.WriteLine("Filename = " + fileName);
                string contents = File.ReadAllText(fileName);

                IConfigGroup<SequenceConfig> cfgGroup = ConfigManager<SequenceConfig>.GetConfigGroup();

                Trace.WriteLine(string.Format("Detected {0} SequenceConfig objects", cfgGroup.Count()));

                if (!cfgGroup.Any())
                {
                    Trace.WriteLine("Creating default TestConfig object.");
                    cfgGroup.Add(new SequenceConfig { Name = "Default" });
                }

                foreach (SequenceConfig testConfig in cfgGroup)
                    testConfig.TestSequenceSourceCode = contents;

                cfgGroup.Save();

                return;
            }

            if (args.Contains("-t2"))
            {
                Trace.WriteLine("Pushing TestConfig to Remi");

                List<string> argsList = args.ToList();

                string sourceFolder = argsList[argsList.IndexOf("-t2") + 1];
                Trace.WriteLine("Source folder Name = " + sourceFolder);

                string destinationFolder = argsList[argsList.IndexOf("-t2") + 2];
                Trace.WriteLine("Destination folder Name = " + destinationFolder);

                if (!Directory.Exists(destinationFolder))
                    Directory.CreateDirectory(destinationFolder);

                IConfigGroup<SequenceConfig> cfgGroup = ConfigManager<SequenceConfig>.GetConfigGroup();


                Trace.WriteLine(string.Format("Detected {0} SequenceConfig objects", cfgGroup.Count()));

                foreach (string sourceFilePath in Directory.EnumerateFiles(sourceFolder))
                {
                    string sourceFileName = Path.GetFileName(sourceFilePath);
                    if (sourceFileName == null)
                        throw new ArgumentException(sourceFilePath + " is not a valid file path.");

                    string destinationFilePath = Path.Combine(destinationFolder, sourceFileName);

                    File.Copy(sourceFilePath, destinationFilePath, true);
                    Trace.WriteLine("Full file name = " + sourceFilePath);
                    Trace.WriteLine("Adding " + sourceFileName);
                    cfgGroup.Add(new SequenceConfig
                    {
                        LocalFile = destinationFilePath,
                        RemiSetting = true,
                        Name = "TestName"
                    });
                }

                cfgGroup.Save();

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
    }
}
