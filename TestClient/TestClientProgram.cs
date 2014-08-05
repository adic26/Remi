using System;
using System.Windows.Forms;
using TestClient.TestSequences;
using TsdLib.Configuration;

namespace TestClient
{
    class TestClientProgram
    {
        [STAThread]
        private static void Main(string[] args)
        {
            bool devMode = args.Length > 0 && args[0] == "-d";
            
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

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
