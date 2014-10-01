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

// ReSharper disable once PossibleNullReferenceException - doc is validated against schema
                _serverLocation = wizardRootElement.Element(ns + "ServerLocation")
                    .Attribute("Path").Value;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Wizard Exception: " + ex.GetType().Name);
                throw;
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            string packageLocation = Path.Combine(_serverLocation, "Packages");
            IPackageRepository repository = PackageRepositoryFactory.Default.CreateRepository(packageLocation);
                
            IQueryable<IPackage> packages = repository.GetPackages()
                .GroupBy(p => p.Id)
                .Select(g => g.OrderBy(p => p.Version)
                    .Last());

            using (SelectPackagesForm form = new SelectPackagesForm(packages))
            {
                form.ShowDialog();
                foreach (IPackage selectedPackage in form.SelectedPackages)
                    NuGetPackageInstaller.InstallPackage(repository, project, selectedPackage.Id, selectedPackage.Version.ToString(), false, false);
            }


            string nugetConfigPath = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData),
                "NuGet",
                "nuget.config");

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

        #region Not implemented

        public void RunFinished() { }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
 
}
