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

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                GlobalDictionary["$rootnamespace$"] = replacementsDictionary["$safeprojectname$"];
                //DependencyFolder = Path.Combine(replacementsDictionary["$destinationdirectory$"], "Dependencies");

                //get the folder where the Visual Studio Extensions are installed
                string extensionFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                    "Microsoft",
                    "VisualStudio",
                    "11.0",
                    "Extensions");

                string contentSourceFolder = new DirectoryInfo(extensionFolder)
                    .GetDirectories() //one folder per extension
                    .OrderBy(d => d.CreationTime).Last() //get the newest directory (most recently installed extension)
                    .GetDirectories("Dependencies")
                    .First() //get the embedded folder where the VSIX installer placed the references and content files
                    .FullName;

                GlobalDictionary["$contentsourcefolder$"] = contentSourceFolder;
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
        private string _wizardData;
        private string _contentSourceFolder;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                replacementsDictionary.Add("$rootnamespace$", SolutionWizard.GlobalDictionary["$rootnamespace$"]);
                if (replacementsDictionary.ContainsKey("$wizarddata$"))
                    _wizardData = replacementsDictionary["$wizarddata$"];

                _contentSourceFolder = SolutionWizard.GlobalDictionary["$contentsourcefolder$"];
                replacementsDictionary.Add("$contentsourcefolder$", _contentSourceFolder);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        public void ProjectFinishedGenerating(Project project)
        {
            try
            {
                if (!string.IsNullOrEmpty(_wizardData))
                {
                    VSProject vsProj = project.Object as VSProject;
                    Debug.Assert(vsProj != null, "Could not cast EnvDTE.Project.Object to VSLangProj.VSProject");

                    XElement itemGroupElement = XElement.Parse(_wizardData);

                    foreach ( XElement referenceElement in itemGroupElement.Elements().Where(e => e.Name.LocalName == "Reference"))
                    {
                        string fileName = (string) referenceElement.Attribute("Include");
                        if (fileName != null)
                            vsProj.References.Add(Path.Combine(_contentSourceFolder, fileName));
                    }

                    foreach (XElement contentElement in itemGroupElement.Elements().Where(e => e.Name.LocalName == "Content"))
                    {
                        string fileName = (string) contentElement.Attribute("Include");
                        if (fileName != null)
                            project.ProjectItems.AddFromFileCopy(Path.Combine(_contentSourceFolder, fileName));
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
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
