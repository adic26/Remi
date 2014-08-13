using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

namespace TsdLibStarterKitWizard
{
    public class RootWizard : IWizard
    {
        private string _dependencyDestinationFolder;
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>(); 

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                GlobalDictionary["$rootnamespace$"] = replacementsDictionary["$safeprojectname$"];
                _dependencyDestinationFolder = Path.Combine(replacementsDictionary["$destinationdirectory$"], "Dependencies");
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public void RunFinished()
        {
            string extensionFolder = Path.Combine(
                Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                "Microsoft",
                "VisualStudio",
                "11.0",
                "Extensions");

            DirectoryInfo dependencies = new DirectoryInfo(extensionFolder)
                .GetDirectories()
                .OrderBy(d => d.CreationTime)
                .Last();

            Directory.CreateDirectory(_dependencyDestinationFolder);

            foreach (FileInfo file in dependencies.GetDirectories("Dependencies").First().GetFiles())
                file.CopyTo(Path.Combine(_dependencyDestinationFolder, file.Name));
        }

        #region Not Implemented

        public void ProjectFinishedGenerating(Project project)
        {

        }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }


        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }

    public class ChildWizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                replacementsDictionary.Add("$rootnamespace$", RootWizard.GlobalDictionary["$rootnamespace$"]);
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        #region Not Implemented

        public void RunFinished()
        {

        }

        public void ProjectFinishedGenerating(Project project)
        {

        }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }


        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
}
