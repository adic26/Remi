using System;
using System.Collections.Generic;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Windows.Forms;
using System.Xml.Linq;
using System.Xml.Schema;
using Microsoft.VisualStudio.ComponentModelHost;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using NuGet;
using NuGet.VisualStudio;

namespace TsdLibStarterKitInstaller
{
    public class Wizard : IWizard
    {
        [Import]
        internal IVsPackageInstaller NuGetPackageInstaller { get; set; }
        private string _serverLocation;
        private string _version;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                using (ServiceProvider serviceProvider = new ServiceProvider((Microsoft.VisualStudio.OLE.Interop.IServiceProvider)automationObject))
                {
                    IComponentModel componentModel = (IComponentModel)serviceProvider.GetService(typeof(SComponentModel));
                    using (CompositionContainer container = new CompositionContainer(componentModel.DefaultExportProvider))
                        container.ComposeParts(this);
                }


                if (NuGetPackageInstaller == null)
                {
                    MessageBox.Show("NuGet Package Manager not available.");

                    throw new WizardBackoutException("NuGet Package Manager not available.");
                }

                XmlSchema schema;
                
                using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TsdLibStarterKitInstaller.WizardData.xsd"))
                {
                    Debug.Assert(schemaStream != null, "The XML schema: TsdLib.CodeGenerator.TsdLib.Instruments.xsd is missing from the TsdLib.dll");
                    schema = XmlSchema.Read(schemaStream, null);
                }
                XNamespace ns = schema.TargetNamespace;

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(schema);
                schemaSet.Compile();
                XmlSchemaObject schemaObject = schemaSet.GlobalElements.Values.OfType<XmlSchemaObject>().First();

                XElement wizardRootElement = XElement.Parse(replacementsDictionary["$wizarddata$"]);

                wizardRootElement.Validate(schemaObject, schemaSet, (o, e) => Debug.Fail("TsdLibStarterKit.vstemplate could not be validated against schema: TsdLib.WizardData.xsd."));


// ReSharper disable once PossibleNullReferenceException
                _serverLocation = wizardRootElement.Element(ns + "ServerLocation")
                    .Attribute("Path").Value;

                XNamespace atomNs = "http://www.w3.org/2005/Atom";
                XNamespace vsixNs = "http://schemas.microsoft.com/developer/vsx-syndication-schema/2010";

                XDocument atomDoc = XDocument.Load(Path.Combine(_serverLocation, "atom.xml"));
                XElement feedElement = atomDoc.Root;
                Debug.Assert(feedElement != null, "Invalid atom.xml document. No root element.");
                XElement entryElement = feedElement.Element(atomNs + "entry");
                Debug.Assert(entryElement != null, "Invalid XML document. No entry element.");
                XElement vsixElement = entryElement.Element(vsixNs + "Vsix");
                Debug.Assert(vsixElement != null, "Invalid XML document. No Vsix element.");
                XElement versionElement = vsixElement.Element(vsixNs + "Version");
                Debug.Assert(versionElement != null, "Invalid XML document. No Version element.");

                _version = versionElement.Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Wizard Exception: " + ex.GetType().Name);
                throw;
            }
        }

        #region Not implemented

        public void RunFinished() { }

        public void ProjectFinishedGenerating(Project project)
        {
            if (NuGetPackageInstaller != null)
            {
                PackageRepositoryFactory factory = PackageRepositoryFactory.Default;
                IPackageRepository repository = factory.CreateRepository(Path.Combine(_serverLocation, "Packages"));

                NuGetPackageInstaller.InstallPackage(repository, project, "TsdLib", _version, false, false);

                string nugetConfigPath = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                    "NuGet",
                    "nuget.config"
                    );

                XDocument nugetConfig = XDocument.Load(nugetConfigPath);

                XElement tsdPackageSourcElement = new XElement("add", new XAttribute("key", "Tsd"), new XAttribute("value", repository.Source));

                XElement configurationElement = nugetConfig.Root;
                Debug.Assert(configurationElement != null, "Could not install NuGet package source. Invalid nuget.config file.");
                XElement packageSourcesElement = configurationElement.Element("packageSources");
                if (packageSourcesElement == null)
                    configurationElement.Add(new XElement("packageSources", tsdPackageSourcElement));
                else
                {
                    if (packageSourcesElement.Elements("add").All(e => e.Attribute("key").Value != "Tsd"))
                    {
                        packageSourcesElement.Add(tsdPackageSourcElement);
                        nugetConfig.Save(nugetConfigPath);
                    }
                }

            }
        }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
 
}
