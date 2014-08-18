using System;
using System.Collections.Generic;
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

            if (args.Length != 2 && args.Length != 3)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the following:" + Environment.NewLine +
                                "Path to extension.vsixmanifest file" + Environment.NewLine +
                                "Path to atom.xml file" + Environment.NewLine +
                                "[-i] Optional: specify to increment least significant build version");
            }

            string manifestPath = args[0];
            string atomPath = args[1];

            XDocument manifestDocument = XDocument.Load(manifestPath);

            XElement manifestIdElement = manifestDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Identity");

            if (manifestIdElement == null)
                throw new ArgumentException(manifestPath + " is not a valid VSIX manifest.");

            string version = manifestIdElement.Attribute("Version").Value;

            if (args.Contains("-i"))
            {
                List<int> v = version.Split('.').Select(s => Convert.ToInt32(s)).ToList();
                v[v.IndexOf(v.Last())] = v.Last() + 1;
                version = string.Join(".", v);
                manifestIdElement.Attribute("Version").Value = version;
                manifestDocument.Save(manifestPath);
                Trace.WriteLine("Updated VSIX manifest file version to " + version);
            }
            else
                Trace.WriteLine("Using manifest version of " + version);

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
