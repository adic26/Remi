using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;
using VSLangProj;

namespace TsdLibStarterKitWizard
{
    public class SolutionWizard : IWizard
    {
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();
        public static string DependencyFolder;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                GlobalDictionary["$rootnamespace$"] = replacementsDictionary["$safeprojectname$"];
                DependencyFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "TsdLib",
                    "Dependencies");

                GlobalDictionary["$dependencyfolder$"] = DependencyFolder;

                if (!Directory.Exists(DependencyFolder))
                    Directory.CreateDirectory(DependencyFolder);

                //get the folder where the Visual Studio Extensions are installed
                string extensionFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft",
                    "VisualStudio",
                    "11.0",
                    "Extensions");

                FileInfo[] contentFiles = new DirectoryInfo(extensionFolder)
                    .GetDirectories() //one folder per extension
                    .OrderBy(d => d.CreationTime).Last() //get the newest directory (most recently installed extension)
                    .GetDirectories("Dependencies")
                    .First() //get the embedded folder where the VSIX installer placed the references and content files
                    .GetFiles()
                    ;



                if (replacementsDictionary.ContainsKey("$wizarddata$"))
                {
                    XElement wizardElement = XElement.Parse(replacementsDictionary["$wizarddata$"]);

                    XElement s = wizardElement.Elements().FirstOrDefault(e => e.Name.LocalName == "Version");
                    if (s != null)
                    {
                        
                    }

                    string currentDependencyFolder = Path.Combine(DependencyFolder, "Current");
                    if (!Directory.Exists(currentDependencyFolder))
                        Directory.CreateDirectory(currentDependencyFolder);

                    string versionedDependencyFolder = Path.Combine(DependencyFolder, "Some_Version");
                    if (!Directory.Exists(versionedDependencyFolder))
                        Directory.CreateDirectory(versionedDependencyFolder);

                    foreach (FileInfo file in contentFiles)
                    {
                        file.CopyTo(Path.Combine(currentDependencyFolder, file.Name), true);
                        file.CopyTo(Path.Combine(versionedDependencyFolder, file.Name), true);
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        #region Not implemented

        public void RunFinished() { }

        public void ProjectFinishedGenerating(Project project) { }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }

    public class ProjectWizard : IWizard
    {
        //private string _wizardData;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                replacementsDictionary.Add("$rootnamespace$", SolutionWizard.GlobalDictionary["$rootnamespace$"]);
                replacementsDictionary.Add("$dependencyfolder$", SolutionWizard.GlobalDictionary["$dependencyfolder$"]);

                //if (replacementsDictionary.ContainsKey("$wizarddata$"))
                //    _wizardData = replacementsDictionary["$wizarddata$"];
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            //try
            //{
            //    if (!string.IsNullOrEmpty(_wizardData))
            //    {
            //        VSProject vsProj = project.Object as VSProject;
            //        Debug.Assert(vsProj != null, "Could not cast EnvDTE.Project.Object to VSLangProj.VSProject");

            //        XElement wizardDataElement = XElement.Parse(_wizardData);

            //        foreach ( XElement referenceElement in wizardDataElement.Elements().Where(e => e.Name.LocalName == "Reference"))
            //        {
            //            string fileName = (string) referenceElement.Attribute("Include");
            //            if (fileName != null)
            //                vsProj.References.Add(Path.Combine(SolutionWizard.DependencyFolder, fileName));
            //        }

            //        foreach (XElement contentElement in wizardDataElement.Elements().Where(e => e.Name.LocalName == "Content"))
            //        {
            //            string fileName = (string) contentElement.Attribute("Include");
            //            if (fileName != null)
            //                project.ProjectItems.AddFromFileCopy(Path.Combine(SolutionWizard.DependencyFolder, fileName));
            //        }
            //    }
            //}
            //catch (Exception ex)
            //{
            //    MessageBox.Show(ex.ToString());
            //    throw;
            //}
        }

        #region Not Implemented
        
        public void RunFinished() {}

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
}
