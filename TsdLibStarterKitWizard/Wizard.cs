using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

namespace TsdLibStarterKitWizard
{
    public class Wizard : IWizard
    {
        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectFinishedGenerating(Project project)
        {
            
        }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public void RunFinished()
        {
            
        }

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                string rootNamespace;
                using (UserInputForm form = new UserInputForm())
                {
                    form.ShowDialog();
                    rootNamespace = form.RootNamespace;
                }



                MessageBox.Show("Adding " + rootNamespace + " to replacementsDictionary as $rootnamespace$");
                replacementsDictionary.Add("$rootnamespace$", rootNamespace);

            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.ToString());
            }
        }

        public bool ShouldAddProjectItem(string filePath) { return true; } 
    }
}
