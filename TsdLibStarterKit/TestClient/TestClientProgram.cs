using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using $rootnamespace$.Sequences;
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

            if (args.Contains("-seq"))
            {
                Trace.WriteLine("Pushing TestConfig to Remi");

                List<string> argsList = args.ToList();

                string sourceFolder = argsList[argsList.IndexOf("-seq") + 1];

                string destinationFolder = argsList[argsList.IndexOf("-seq") + 2];

                bool remiSetting = bool.Parse(argsList[argsList.IndexOf("-seq") + 3]);

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
                        RemiSetting = remiSetting
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


            Console.WriteLine("Done");
        }
    }
}
