using System;
using System.Windows.Forms;

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
            Application.Run(new ConfigManagerForm(@"C:\temp\RemiSettingsTest"));
        }
    }
}
