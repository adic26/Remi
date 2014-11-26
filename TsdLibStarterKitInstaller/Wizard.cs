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

        private Dictionary<string,string> _packageRepositories; 

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
                
                XmlSchema schema;
                
                using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TsdLibStarterKitInstaller.WizardData.xsd"))
                {
                    if (schemaStream == null)
                        throw new WizardBackoutException("The XML schema: TsdLib.CodeGenerator.TsdLib.Instruments.xsd is missing from the TsdLib.dll");
                    schema = XmlSchema.Read(schemaStream, null);
                }
                XNamespace ns = schema.TargetNamespace;

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(schema);
                schemaSet.Compile();
                XmlSchemaObject schemaObject = schemaSet.GlobalElements.Values.OfType<XmlSchemaObject>().First();

                XElement wizardRootElement = XElement.Parse(replacementsDictionary["$wizarddata$"]);

                wizardRootElement.Validate(schemaObject, schemaSet, (o, e) => Debug.Fail("TsdLibStarterKit.vstemplate could not be validated against schema: TsdLib.WizardData.xsd."));

                _packageRepositories = wizardRootElement.Elements(ns + "NuGetPackageRepository")
                    .ToDictionary(e => e.Attribute("Name").Value, e => e.Attribute("Path").Value);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Wizard Exception: " + ex.GetType().Name);
                throw;
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            if (NuGetPackageInstaller == null)
            {
                MessageBox.Show("NuGet Package Manager not available. Please add packages manually from the following locations: " + string.Join(Environment.NewLine, _packageRepositories));
                return;
            }

            Settings nugetSettings = new Settings(new PhysicalFileSystem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NuGet")));
            nugetSettings.SetValue("packageRestore", "enabled", "True");
            nugetSettings.SetValue("packageRestore", "automatic", "True");
            nugetSettings.SetValues("packageSources", _packageRepositories.ToList());
            nugetSettings.GetValues("activePackageSource", false);
            nugetSettings.DeleteValue("activePackageSource", nugetSettings.GetValues("activePackageSource", false).First().Key);
            nugetSettings.SetValue("activePackageSource", _packageRepositories.First().Key, _packageRepositories.First().Value);

            foreach (var repoKvp in _packageRepositories)
            {
                //If using OData repository, use the IPackage.IsLatestVersion property - should also try reading tags
                IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(repoKvp.Value);

                IQueryable<IPackage> packages = repo.GetPackages()
                    .GroupBy(p => p.Id)
                    .Select(g => g.OrderBy(p => p.Version)
                    .Last());

                using (SelectPackagesForm form = new SelectPackagesForm(repoKvp.Key, packages))
                {
                    form.ShowDialog();
                    foreach (IPackage selectedPackage in form.SelectedPackages)
                        NuGetPackageInstaller.InstallPackage(repo, project, selectedPackage.Id, selectedPackage.Version.ToString(), false, false);
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
