using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;

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
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

#if DEBUG
            const OperatingMode defaultMode = OperatingMode.Engineering;
#else
            const OperatingMode defaultMode = OperatingMode.Production;
#endif

            Trace.Listeners.Add(new ConsoleTraceListener());
            List<string> argsList = args.ToList();

            string testSystemName = args.Contains("-testSystemName") ? argsList[argsList.IndexOf("-testSystemName") + 1] : Application.ProductName;
            string testSystemVersion = args.Contains("-testSystemVersion") ? argsList[argsList.IndexOf("-testSystemVersion") + 1] : Application.ProductVersion;
            OperatingMode testSystemMode = args.Contains("-testSystemMode") ? (OperatingMode)Enum.Parse(typeof(OperatingMode), argsList[argsList.IndexOf("-testSystemMode") + 1]) : defaultMode;
            bool localDomain = args.Length > 0 && args.Contains("-localDomain");

            string settingsLocation;
            if (args.Contains("-settingsLocation"))
            {
                int settingsLocationArgIndex = argsList.IndexOf("-settingsLocation");
                settingsLocation = argsList[settingsLocationArgIndex + 1];
            }
            else
                settingsLocation = @"C:\temp\TsdLibSettings";

            TestDetails testDetails = new TestDetails(testSystemName, testSystemVersion, testSystemMode);

            DatabaseFolderConnection databaseFolderConnection = new DatabaseFolderConnection(settingsLocation);

            if (args.Contains("-seq"))
            {
                IConfigGroup<Sequence> sequences = new ConfigManager(testDetails, databaseFolderConnection).GetConfigGroup<Sequence>(testDetails);

                int seqArgIndex = argsList.IndexOf("-seq");
                string sequenceFolder = argsList[seqArgIndex + 1];
                bool storeInDatabase = bool.Parse(argsList[seqArgIndex + 2]);
                List<string> assemblyReferences = new List<string> { "System.dll", "System.Xml.dll", "TsdLib.dll", testSystemName + ".exe" };

                foreach (Sequence sequence in sequences.Where(seq => !seq.IsDefault))
                {
                    string vsFile = Path.Combine(sequenceFolder, sequence.Name + ".cs");
                    if (!File.Exists(vsFile))
                        File.WriteAllText(vsFile, sequence.SourceCode);
                }
                foreach (string seqFile in Directory.EnumerateFiles(sequenceFolder))
                    sequences.Add(new Sequence(seqFile, storeInDatabase, assemblyReferences, testSystemName));
                sequences.Save();
                return;
            }

            Controller c = new Controller(testDetails, databaseFolderConnection, localDomain);

            Application.Run(c.View);
            
            Console.WriteLine("Done");
        }
    }
}
