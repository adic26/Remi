using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using $rootnamespace$.TestSequences;
using TsdLib.Configuration;
using $rootnamespace$.Configuration;

namespace $safeprojectname$
{
    class $safeprojectname$Program
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

                IConfigGroup<TestConfig> cfgGroup = Config<TestConfig>.GetConfigGroup();

                Trace.WriteLine(string.Format("Detected {0} TestConfig objects", cfgGroup.Count()));

                if (!cfgGroup.Any())
                {
                    Trace.WriteLine("Creating default TestConfig object.");
                    cfgGroup.Add(new TestConfig { Name = "Default" });
                }

                foreach (TestConfig testConfig in cfgGroup)
                    testConfig.TestSequenceSource = contents;

                cfgGroup.Save();

                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool devMode = args.Length > 0 && args[0] == "-d";

            View view = new View
            {
                StationConfigList = Config<StationConfig>.GetConfigGroup().GetList(),
                ProductConfigList = Config<ProductConfig>.GetConfigGroup().GetList()
            };

// ReSharper disable once UnusedVariable - constructor hooks up view events
            Controller c = new Controller(view, new TestSequence(), devMode);

            Application.Run(view);

            Console.WriteLine("Done");
        }
    }
}
