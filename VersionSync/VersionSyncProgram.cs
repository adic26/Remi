using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml.Linq;

namespace VersionSync
{
    class VersionSyncProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());
            //TODO: add project path parameter
            if (args.Length != 3)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the paths to the following: " + Environment.NewLine +
                    "TsdLib.dll" + Environment.NewLine +
                    "source.extension.vsixmanifest file " + Environment.NewLine +
                    "atom.xml file.");
                return -1;
            }

            string tsdLibDll = args[0];
            string manifestPath = args[1];
            string atomPath = args[2];

            string version = Assembly.ReflectionOnlyLoadFrom(tsdLibDll)
                .GetName()
                .Version.ToString();

            Trace.WriteLine("TsdLib version = " + version);

            XDocument manifestDocument = XDocument.Load(manifestPath);

            XElement manifestIdElement = manifestDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Identity");

            if (manifestIdElement == null)
                throw new ArgumentException(manifestPath + " is not a valid VSIX manifest.");

            manifestIdElement.Attribute("Version").Value = version;
            manifestDocument.Save(manifestPath);

            Trace.WriteLine("Updated VSIX manifest file version to " + version);

            XDocument atomDocument = XDocument.Load(atomPath);
            XElement atomVsixElement = atomDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Vsix");

            if (atomVsixElement == null)
                throw new ArgumentException(atomPath + " does not contain a Vsix element.");

            XElement atomVersionElement = atomVsixElement
                .Elements()
                .FirstOrDefault(e => e.Name.LocalName == "Version");

            if (atomVersionElement == null)
                throw new ArgumentException(atomPath + " does not contain a Vsix->Version element.");

            atomVersionElement.Value = version;
            atomDocument.Save(atomPath, SaveOptions.None);

            Trace.WriteLine("Updated atom file version to " + version);

            return 0;
        }
    }
}
