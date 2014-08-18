using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

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
                string dependencyFolder = Path.Combine(
                    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                    "TsdLib",
                    "Dependencies");

                GlobalDictionary["$dependencyfolder$"] = dependencyFolder;

                if (!Directory.Exists(dependencyFolder))
                    Directory.CreateDirectory(dependencyFolder);

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

                string currentDependencyFolder = Path.Combine(dependencyFolder, "Current");
                if (!Directory.Exists(currentDependencyFolder))
                    Directory.CreateDirectory(currentDependencyFolder);
                foreach (FileInfo file in contentFiles)
                    file.CopyTo(Path.Combine(currentDependencyFolder, file.Name), true);

                string version = FileVersionInfo.GetVersionInfo
                    (
                        contentFiles
                        .First(f => f.Name == "TsdLib.dll")
                        .FullName
                    ).FileVersion;

                string versionedDependencyFolder = Path.Combine(dependencyFolder, version);
                if (!Directory.Exists(versionedDependencyFolder))
                    Directory.CreateDirectory(versionedDependencyFolder);

                foreach (FileInfo file in contentFiles)
                    file.CopyTo(Path.Combine(versionedDependencyFolder, file.Name), true);
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
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                replacementsDictionary.Add("$rootnamespace$", SolutionWizard.GlobalDictionary["$rootnamespace$"]);
                replacementsDictionary.Add("$dependencyfolder$", SolutionWizard.GlobalDictionary["$dependencyfolder$"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
                throw;
            }
        }

        #region Not Implemented

        public void ProjectFinishedGenerating(Project project) { }
        
        public void RunFinished() {}

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
}
