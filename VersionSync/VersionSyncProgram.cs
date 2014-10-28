using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Xml;
using System.Xml.Linq;

namespace VersionSync
{
    class VersionSyncProgram
    {
        static int Main(string[] args)
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            if (args.Length != 2)
            {
                Trace.WriteLine("Wrong number of arguments. Please pass the following:" + Environment.NewLine +
                                "Path to extension.vsixmanifest file" + Environment.NewLine +
                                "Path to atom.xml file");
            }

            string manifestPath = args[0];
            string atomPath = args[1];

            #region manifest
            

            XDocument manifestDocument = XDocument.Load(manifestPath);

            XNamespace manifestNs = "http://schemas.microsoft.com/developer/vsx-schema/2011";
            
            // ReSharper disable PossibleNullReferenceException
            XElement manifestRoot = manifestDocument.Root;
            Debug.Assert(manifestRoot != null, "Invalid VSIX manifest. No root element.");
            XElement manifestMetaDataElement = manifestRoot.Element(manifestNs + "Metadata");
            Debug.Assert(manifestMetaDataElement != null, "Invalid VSIX manifest. No Metadata element.");
            XElement manifestIdentityElement = manifestMetaDataElement.Element(manifestNs + "Identity");
            Debug.Assert(manifestIdentityElement != null, "Invalid VSIX manifest. No Metadata.Identity element.");
            Version oldVersion = new Version(manifestIdentityElement.Attribute("Version").Value); 
            string version = new Version(oldVersion.Major, oldVersion.Minor, oldVersion.Build, oldVersion.Revision + 1).ToString();
            // ReSharper restore PossibleNullReferenceException
            manifestIdentityElement.Attribute("Version").Value = version;

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
