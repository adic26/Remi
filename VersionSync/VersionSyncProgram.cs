using System;
using System.Diagnostics;
using System.Linq;
using System.Xml.Linq;

namespace VersionSync
{
    class VersionSyncProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Length < 2)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the paths to the source.extension.vsixmanifest file and the atom.xml file.");
                return -1;
            }

            string manifestPath = args[0];

            XDocument manifestDocument = XDocument.Load(manifestPath);

            XElement manifestIdElement = manifestDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Identity");

            if (manifestIdElement == null)
                throw new ArgumentException(manifestPath + " does not contain a valid VSIX manifest.");

            string version = (string)manifestIdElement.Attribute("Version");

            string atomPath = args[1];

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
