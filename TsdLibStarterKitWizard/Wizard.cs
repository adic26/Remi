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
            GlobalDictionary["$rootnamespace$"] = replacementsDictionary["$safeprojectname$"];

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
                .GetFiles();

            string tsdLibVersion = FileVersionInfo.GetVersionInfo
                (
                    contentFiles
                    .First(f => f.Name == "TsdLib.dll")
                    .FullName
                ).FileVersion;

            string dependencyFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                "TsdLib",
                "Dependencies",
                tsdLibVersion);

            GlobalDictionary["$dependencyfolder$"] = dependencyFolder;

            if (!Directory.Exists(dependencyFolder))
            {
                Directory.CreateDirectory(dependencyFolder);
                foreach (FileInfo file in contentFiles)
                    file.CopyTo(Path.Combine(dependencyFolder, file.Name), true);
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
            replacementsDictionary.Add("$rootnamespace$", SolutionWizard.GlobalDictionary["$rootnamespace$"]);
            replacementsDictionary.Add("$dependencyfolder$", SolutionWizard.GlobalDictionary["$dependencyfolder$"]);
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
