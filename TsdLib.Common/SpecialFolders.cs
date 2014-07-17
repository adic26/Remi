using System;
using System.Diagnostics;
using System.IO;

namespace TsdLib
{
    public static class SpecialFolders
    {
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
