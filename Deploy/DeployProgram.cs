using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace Deploy
{
    class DeployProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            string outputFolderBase = args[0];

            string[] files = args.Skip(1).ToArray();

            string tsdLibAsy = files.FirstOrDefault(s => Path.GetFileName(s) == "TsdLib.dll");
            if (tsdLibAsy == null)
            {
                Trace.WriteLine("TsdLib.dll was not passed as a parameter.");
                return -1;
            }

            string version = Assembly.ReflectionOnlyLoadFrom(tsdLibAsy).GetName().Version.ToString();

            string outputFolder = Path.Combine(outputFolderBase, version);
            if (!Directory.Exists(outputFolder))
                Directory.CreateDirectory(outputFolder);

            foreach (string file in files)
                if (file != null)
                {
                    string destinationFile = Path.Combine(outputFolder, Path.GetFileName(file));
                    if (!File.Exists(destinationFile))
                        File.Copy(file, destinationFile);
                }

            return 0;
        }
    }
}
