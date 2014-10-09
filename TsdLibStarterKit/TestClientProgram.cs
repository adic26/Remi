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
#if DEBUG
        private const bool Released = false;
#else
        private const bool Released = true;
#endif

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Contains("-seq"))
            {
                IConfigGroup<Sequence> sequences = new ConfigManager(new DatabaseFolderConnection(@"C:\temp\RemiSettingsTest", "$safeprojectname$", Application.ProductVersion, Released)).GetConfigGroup<Sequence>();

                List<string> argsList = args.ToList();
                int seqArgIndex = argsList.IndexOf("-seq");
                string sequenceFolder = argsList[seqArgIndex + 1];
                bool storeInDatabase = bool.Parse(argsList[seqArgIndex + 2]);
                List<string> assemblyReferences = argsList.Count > seqArgIndex + 3 ? argsList[seqArgIndex + 3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (Sequence sequence in sequences)
                {
                    string vsFile = Path.Combine(sequenceFolder, sequence.Name + ".cs");
                    if (!File.Exists(vsFile))
                        File.WriteAllText(vsFile, sequence.FullSourceCode);
                }
                foreach (string seqFile in Directory.EnumerateFiles(sequenceFolder))
                    sequences.Add(new Sequence(seqFile, storeInDatabase, "$safeprojectname$", assemblyReferences));
                sequences.Save();
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
