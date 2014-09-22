using System;
using System.Diagnostics;
using System.IO;

namespace TsdLib
{
    /// <summary>
    /// Defines special folders used by the TsdLib Framework.
    /// </summary>
    public static class SpecialFolders
    {
        public static string Assemblies
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", "Assemblies");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Gets the folder where Test Sequence logs are stored.
        /// </summary>
        public static string Logs
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", "Logs", DateTime.Now.ToString("MMM_dd_yyyy"));
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        /// <summary>
        /// Gets the folder where Test Sequence measurements are stored.
        /// </summary>
        public static string Measurements
        {
            get
            {
                string path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", Process.GetCurrentProcess().ProcessName, "Measurements");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }
    }
}
