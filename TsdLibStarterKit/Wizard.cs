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

namespace TsdLibStarterKit
{
    public class TsdLibStarterKitWizard : IWizard
    {
        [Import]
        internal IVsPackageInstaller NuGetPackageInstaller { get; set; }

        //private Dictionary<string,string> _packageRepositories;
        private List<PackageRepository> _packageRepositoryStructs; 

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
                
                using (Stream schemaStream = Assembly.GetExecutingAssembly().GetManifestResourceStream("TsdLibStarterKit.WizardData.xsd"))
                {
                    if (schemaStream == null)
                        throw new WizardBackoutException("The XML schema: WizardData.xsd is missing from TsdLibStarterKit.dll");
                    schema = XmlSchema.Read(schemaStream, null);
                }
                XNamespace ns = schema.TargetNamespace;

                XmlSchemaSet schemaSet = new XmlSchemaSet();
                schemaSet.Add(schema);
                schemaSet.Compile();
                XmlSchemaObject schemaObject = schemaSet.GlobalElements.Values.OfType<XmlSchemaObject>().First();

                XElement wizardRootElement = XElement.Parse(replacementsDictionary["$wizarddata$"]);

                wizardRootElement.Validate(schemaObject, schemaSet, (o, e) => Debug.Fail("TsdLibStarterKit.vstemplate could not be validated against schema: TsdLib.WizardData.xsd."));

                //_packageRepositories = wizardRootElement.Elements(ns + "NuGetPackageRepository")
                //    .ToDictionary(e => e.Attribute("Name").Value, e => e.Attribute("Location").Value);

                _packageRepositoryStructs = new List<PackageRepository>();
                foreach (XElement repositoryElement in wizardRootElement.Elements(ns + "NuGetPackageRepository"))
                {
                    PackageRepository repositoryStruct = new PackageRepository(repositoryElement.Attribute("Name").Value, repositoryElement.Attribute("Location").Value);
                    foreach (XElement packageElement in repositoryElement.Elements(ns + "NuGetPackage"))
                        repositoryStruct.Packages.Add(packageElement.Attribute("Name").Value);
                    _packageRepositoryStructs.Add(repositoryStruct);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), "Wizard Exception: " + ex.GetType().Name);
                throw;
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            try
            {
                if (NuGetPackageInstaller == null)
                {
                    MessageBox.Show("NuGet Package Manager not available. Please add packages manually from the following locations: " + string.Join(Environment.NewLine, _packageRepositoryStructs.Select(r => r.Name + ":" + r.Location)));
                    return;
                }

                Settings nugetSettings = new Settings(new PhysicalFileSystem(Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), "NuGet")));
                nugetSettings.SetValue("packageRestore", "enabled", "True");
                nugetSettings.SetValue("packageRestore", "automatic", "True");
                nugetSettings.SetValues("packageSources", _packageRepositoryStructs.ToDictionary(r => r.Name, r => r.Location).ToList());
                nugetSettings.GetValues("activePackageSource", false);
                nugetSettings.DeleteValue("activePackageSource", nugetSettings.GetValues("activePackageSource", false).First().Key);
                nugetSettings.SetValue("activePackageSource", _packageRepositoryStructs.First().Name, _packageRepositoryStructs.First().Location);

                foreach (PackageRepository packageRepository in _packageRepositoryStructs)
                {
                    IPackageRepository repo = PackageRepositoryFactory.Default.CreateRepository(packageRepository.Location);

                    if (repo.Source.StartsWith("http") && repo.SupportsPrereleasePackages) //It is an OData feed, we can read parameters on the IPackages
                    {
#if DEBUG
                        IQueryable<IPackage> packages = repo.GetPackages().Where(p => p.IsAbsoluteLatestVersion);
#else
                        IQueryable<IPackage> packages = repo.GetPackages().Where(p => p.IsLatestVersion);
#endif

                        //This expression isn't supported on the server, so we need to iterate the packages and use if statements
                        //TODO: figure out a different expression that doesn't require the server to perform evaluations
                        //foreach (IPackage requiredPackage in packages.Where(p => packagesSpecifiedInTemplate.Contains(p.Id)))
                        //    NuGetPackageInstaller.InstallPackage(repo, project, requiredPackage.Id, requiredPackage.Version.ToString(), false, false);

                        //this is very slow, requiring server-side evaluations
                        //List<string> packagesSpecifiedInTemplate = packageRepository.Packages;
                        //foreach (IPackage package in packages)
                        //{
                        //    if ((package.Tags != null && package.Tags.Contains("starterkit-required"))/* || packagesSpecifiedInTemplate.Contains(package.Id)*/)
                        //        NuGetPackageInstaller.InstallPackage(repo, project, package.Id, package.Version.ToString(), false, false);
                        //}

                        foreach (IPackage requiredPackage in packages.Where(p => p.Tags.Contains("starterkit-required")))
                            NuGetPackageInstaller.InstallPackage(repo, project, requiredPackage.Id, requiredPackage.Version.ToString(), false, false);


                        using (SelectPackagesForm form = new SelectPackagesForm(packageRepository.Name, packages.Where(p => p.Tags.Contains("starterkit-optional"))))
                        {
                            form.ShowDialog();
                            foreach (IPackage selectedPackage in form.SelectedPackages)
                                NuGetPackageInstaller.InstallPackage(repo, project, selectedPackage.Id, selectedPackage.Version.ToString(), false, false);
                        }
                    }
                    else //It is a network share - can't use IsLatestVersion or Tags
                    {
                        IQueryable<IPackage> packages = repo.GetPackages()
                            .GroupBy(p => p.Id)
                            .Select(g => g.OrderBy(p => p.Version)
                            .Last());

                        using (SelectPackagesForm form = new SelectPackagesForm(packageRepository.Name, packages))
                        {
                            form.ShowDialog();
                            foreach (IPackage selectedPackage in form.SelectedPackages)
                                NuGetPackageInstaller.InstallPackage(repo, project, selectedPackage.Id, selectedPackage.Version.ToString(), false, false);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString(), ex.GetType().Name);
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

    struct PackageRepository
    {
        public string Name;
        public string Location;
        public List<string> Packages;

        public PackageRepository(string name, string location)
        {
            Name = name;
            Location = location;
            Packages = new List<string>();
        }
    }
 
}
