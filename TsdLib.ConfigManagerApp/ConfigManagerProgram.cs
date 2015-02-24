using System;
using System.Windows.Forms;
using TsdLib.Configuration.Managers;

namespace TsdLib.Configuration
{
    static class ConfigManagerProgram
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new ConfigManagerForm(@"C:\temp\TsdLibSettings", true));
        }
    }
}
