using System;
using System.CodeDom.Compiler;
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
            List<string> argsList = args.ToList();

            if (args.Contains("-seq"))
            {
                IConfigGroup<Sequence> sequences = new ConfigManager(new DatabaseFolderConnection(@"C:\temp\TsdLibSettings", "TestClient", Application.ProductVersion, Released)).GetConfigGroup<Sequence>();

                int seqArgIndex = argsList.IndexOf("-seq");
                string sequenceFolder = argsList[seqArgIndex + 1];
                bool storeInDatabase = bool.Parse(argsList[seqArgIndex + 2]);
                List<string> assemblyReferences = new List<string>{"System.dll","System.Xml.dll","TsdLib.dll","TestClient.exe"};

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

            string testSystemName = args.Contains("-testSystemName") ? argsList[argsList.IndexOf("-testSystemName") + 1] : Application.ProductName;
            string testSystemVersion = args.Contains("-testSystemVersion") ? argsList[argsList.IndexOf("-testSystemVersion") + 1] : Application.ProductVersion;
#if INSTRUMENT_LIBRARY
            ICodeParser instrumentParser = new TsdLib.InstrumentLibrary.InstrumentParser(Application.ProductName, Language.CSharp.ToString());
#else
            ICodeParser instrumentParser = new BasicCodeParser();
#endif
            Controller c = new Controller(devMode, testSystemName, testSystemVersion, localDomain, instrumentParser);

            Application.Run(c.View);
            
            Console.WriteLine("Done");
        }
    }
}
