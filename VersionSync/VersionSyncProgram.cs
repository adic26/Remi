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
            if (args.Length < 2)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the paths to the following: " + Environment.NewLine +
                    "source.extension.vsixmanifest file " + Environment.NewLine +
                    "atom.xml file." + Environment.NewLine +
                    "Optional: Assembly to base the versions on. If omitted, version from manifest designer will be used.");
                return -1;
            }

            string manifestPath = args[0];
            string atomPath = args[1];

            XDocument manifestDocument = XDocument.Load(manifestPath);

            XElement manifestIdElement = manifestDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Identity");

            if (manifestIdElement == null)
                throw new ArgumentException(manifestPath + " is not a valid VSIX manifest.");

            string version;
            if (args.Length > 2)
            {
                version = FileVersionInfo.GetVersionInfo(args[2]).FileVersion;

                manifestIdElement.Attribute("Version").Value = FileVersionInfo.GetVersionInfo(args[2]).FileVersion;
                manifestDocument.Save(manifestPath);

                Trace.WriteLine("Updated VSIX manifest file version to " + version);
            }
            else
            {
                version = manifestIdElement.Attribute("Version").Value;
                Trace.WriteLine("Using manifest version of " + version);
            }

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
