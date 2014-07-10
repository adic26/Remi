using System;
using System.IO;

namespace TsdLib
{
    public static class SpecialFolders
    {
        [Obsolete("Source files are generated to the project directory and must be specified. Assembly files are generated to the current directory.")]
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
    }
}
