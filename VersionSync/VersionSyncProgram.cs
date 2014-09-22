using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace VersionSync
{
    class VersionSyncProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Length != 3)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the following:" + Environment.NewLine +
                                "Path to TsdLib.dll" + Environment.NewLine +
                                "Path to extension.vsixmanifest file" + Environment.NewLine +
                                "Path to atom.xml file");
            }

            string tsdLibPath = args[0];
            string manifestPath = args[1];
            string atomPath = args[2];

            string version = Assembly.ReflectionOnlyLoadFrom(tsdLibPath).GetName().Version.ToString();

            #region manifest
            
            XDocument manifestDocument = XDocument.Load(manifestPath);

            XElement manifestIdElement = manifestDocument
                .Descendants()
                .FirstOrDefault(e => e.Name.LocalName == "Identity");

            if (manifestIdElement == null)
                throw new ArgumentException(manifestPath + " is not a valid VSIX manifest.");

            manifestIdElement.Attribute("Version").Value = version;

            manifestDocument.Save(manifestPath, SaveOptions.None);

            Trace.WriteLine("Updating manifest version to " + version);

            #endregion

            #region atom

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

            IEnumerable<XElement> atomUpdatedElements = atomDocument
                .Descendants()
                .Where((e => e.Name.LocalName == "updated"));

            string currentTime = XmlConvert.ToString(DateTime.UtcNow, XmlDateTimeSerializationMode.Utc);

            foreach (XElement atomUpdatedElement in atomUpdatedElements)
                atomUpdatedElement.Value = currentTime;

            atomDocument.Save(atomPath, SaveOptions.None);

            Trace.WriteLine("Updating atom file version to " + version);
            Trace.WriteLine("Updating atom file timestamp to " + currentTime);

            #endregion

            return 0;
        }
    }
}
