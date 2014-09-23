using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using TsdLib.Configuration;

namespace TsdLib.ConfigManager
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
            //Application.Run(new ConfigForm<StationConfigCommon>());
        }
    }
}
