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

                IConfigGroup<SequenceConfig> cfgGroup = Config<SequenceConfig>.GetConfigGroup();

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
