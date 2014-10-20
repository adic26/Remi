using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TestClient
{
    class TestClientProgram
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
            
            //TODO: move to separate application
            if (args.Contains("-seq"))
            {
                IConfigGroup<Sequence> sequences = new ConfigManager(new DatabaseFolderConnection(@"C:\temp\RemiSettingsTest", "TestClient", Application.ProductVersion, Released)).GetConfigGroup<Sequence>();

                //string sequenceFolder = @"C:\Users\jmckee\Source\Repos\TsdLib\TestClient\Sequences";
                //bool storeInDatabase = true;

                List<string> argsList = args.ToList();
                int seqArgIndex = argsList.IndexOf("-seq");
                string sequenceFolder = argsList[seqArgIndex + 1];
                bool storeInDatabase = bool.Parse(argsList[seqArgIndex + 2]);
                List<string> assemblyReferences = argsList.Count > seqArgIndex + 3 ? argsList[seqArgIndex + 3].Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

                foreach (Sequence sequence in sequences)
                {
                    string vsFile = Path.Combine(sequenceFolder, sequence.Name + ".cs");
                    if (!File.Exists(vsFile))
                        File.WriteAllText(vsFile, sequence.SourceCode);
                }
                foreach (string seqFile in Directory.EnumerateFiles(sequenceFolder))
                    sequences.Add(new Sequence(seqFile, storeInDatabase, "TestClient", assemblyReferences));
                sequences.Save();
                return;
            }

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            bool devMode = args.Length > 0 && args.Contains("-d");
            bool localDomain = args.Length > 0 && args.Contains("-localDomain");
            Controller c = new Controller(devMode, localDomain);

            if (c.View is Form)
                Application.Run(c.View as Form);
            
            //TODO: figure out how to launch non-form view

            Console.WriteLine("Done");
        }
    }
}
