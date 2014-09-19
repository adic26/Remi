﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

namespace TsdLibStarterKitInstaller
{
    public class Wizard : IWizard
    {
        public static Dictionary<string, string> GlobalDictionary = new Dictionary<string, string>();
        private DTE _dte;

        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                GlobalDictionary["$rootnamespace$"] = replacementsDictionary["$safeprojectname$"];

                _dte = automationObject as DTE;
                Debug.Assert(_dte != null, "Wizard.RunStarted error. Could not obtain automation object.");

                string fileName = "";
                string destination = "";
                string projectName = "";

                


                MessageBox.Show(Directory.GetCurrentDirectory(), "NEW WIZARD");

                //string extensionsFolder = Path.Combine(
                //    Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
                //    dte.RegistryRoot.TrimStart(@"Software\".ToCharArray()),
                //    "Extensions");

                //string namedDirectory = Path.Combine(extensionsFolder, "BlackBerry", "TsdLibStarterKit");
                //if (Directory.Exists(namedDirectory))
                //    extensionsFolder = namedDirectory;

                //FileInfo[] contentFiles = new DirectoryInfo(extensionsFolder)
                //    .GetDirectories() //one folder per extension
                //    .OrderBy(d => d.CreationTime).Last() //get the newest directory (most recently installed extension)
                //    .GetDirectories("Dependencies")
                //    .First() //get the embedded folder where the VSIX installer placed the references and content files
                //    .GetFiles();

                //string tsdLibVersion = FileVersionInfo.GetVersionInfo
                //    (
                //        contentFiles
                //        .First(f => f.Name == "TsdLib.dll")
                //        .FullName
                //    ).FileVersion;
            
                //string dependencyFolder = Path.Combine(
                //    Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData),
                //    "TsdLib",
                //    "Dependencies",
                //    tsdLibVersion);

                //GlobalDictionary["$dependencyfolder$"] = dependencyFolder;

                //if (!Directory.Exists(dependencyFolder))
                //    Directory.CreateDirectory(dependencyFolder);
                //foreach (FileInfo contentFile in contentFiles)
                //    contentFile.CopyTo(Path.Combine(dependencyFolder, contentFile.Name), true);


                //if (replacementsDictionary.ContainsKey("$wizarddata$"))
                //{
                //    XElement dataElement = XElement.Parse(replacementsDictionary["$wizarddata$"]);
                //    IEnumerable<FileInfo> additionalFiles = dataElement
                //        .Elements()
                //        .Where(e => e.Name.LocalName == "File")
                //        .Select(e => (string)e.Attribute("Path"))
                //        .Select(s => new FileInfo(s));

                //    foreach (FileInfo additionalFile in additionalFiles)
                //        additionalFile.CopyTo(Path.Combine(dependencyFolder, additionalFile.Name), true);
                //}
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
            
        }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
}
