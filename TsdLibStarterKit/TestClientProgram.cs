using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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
            string testSystemVersionString = args.Contains("-testSystemVersion") ? argsList[argsList.IndexOf("-testSystemVersion") + 1] : Application.ProductVersion;
            Version testSystemVersion = new Version(testSystemVersionString);
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

            IConfigConnection sharedConfigConnection = new FileSystemConnection(new DirectoryInfo(settingsLocation));

            if (args.Contains("-seq"))
            {
                ConfigManager<Sequence> sequenceConfigManager = new ConfigManager<Sequence>(testDetails, sharedConfigConnection);

                int seqArgIndex = argsList.IndexOf("-seq");
                string sequenceFolder = argsList[seqArgIndex + 1];
                bool storeInDatabase = bool.Parse(argsList[seqArgIndex + 2]);
                HashSet<string> assemblyReferences = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(asy => Path.GetFileName(asy.GetName().CodeBase)), StringComparer.InvariantCultureIgnoreCase) { Path.GetFileName(Assembly.GetEntryAssembly().GetName().CodeBase) };
                foreach (string fileName in Directory.GetFiles(AppDomain.CurrentDomain.BaseDirectory, "*.dll").Select(Path.GetFileName))
                    assemblyReferences.Add(fileName);

                foreach (Sequence sequence in sequenceConfigManager.GetConfigGroup().Where(seq => !seq.IsDefault))
                {
                    string vsFile = Path.Combine(sequenceFolder, sequence.Name + ".cs");
                    if (!File.Exists(vsFile))
                        File.WriteAllText(vsFile, sequence.SourceCode);
                }
                foreach (string seqFile in Directory.EnumerateFiles(sequenceFolder))
                {
                    Trace.WriteLine("Found" + seqFile);
                    sequenceConfigManager.Add(new Sequence(seqFile, storeInDatabase, assemblyReferences));
                }
                sequenceConfigManager.Save();
                return;
            }

            Controller c = new Controller(testDetails, sharedConfigConnection, localDomain);

            Application.Run(c.UI);
            
            Console.WriteLine("Done");
        }
    }
}
