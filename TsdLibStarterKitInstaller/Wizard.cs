using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Windows.Forms;
using System.Xml.Linq;
using Microsoft.VisualStudio.TemplateWizard;
using EnvDTE;

namespace TsdLibStarterKitInstaller
{
    public class Wizard : IWizard
    {
        public void RunStarted(object automationObject, Dictionary<string, string> replacementsDictionary, WizardRunKind runKind, object[] customParams)
        {
            try
            {
                string sourceBasePath = replacementsDictionary["$wizarddata$"];

                XDocument manifestDoc = XDocument.Load(Path.Combine(sourceBasePath, "extension.vsixmanifest"));
                XNamespace ns = "http://schemas.microsoft.com/developer/vsx-schema/2011";
                XElement root = manifestDoc.Root;
                Debug.Assert(root != null, "Invalid XML document. No root element.");
                XElement manifestElement = root.Element(ns + "Metadata");
                Debug.Assert(manifestElement != null, "Invalid XML document. No Manifest element.");
                XElement identityElement = manifestElement.Element(ns + "Identity");
                Debug.Assert(identityElement != null, "Invalid XML document. No Identity element.");
                string version = identityElement.Attribute("Version").Value;
                
                string sourcePath = Path.Combine(sourceBasePath, version);

                string destinationPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData), "TsdLib", "Assemblies", version);
                if (!Directory.Exists(destinationPath))
                    Directory.CreateDirectory(destinationPath);

                replacementsDictionary["$assemblyfolder$"] = destinationPath;

                foreach (string file in Directory.GetFiles(sourcePath))
                    if (file != null)
                    {
                        string destinationFile = Path.Combine(destinationPath, Path.GetFileName(file));
                        if (!File.Exists(destinationFile))
                            File.Copy(file, destinationFile);
                    }

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
            //var vsproject = project.Object as VSLangProj.VSProject;

            //foreach (VSLangProj.Reference reference in vsproject.References)
            //{
            //    if (reference.SourceProject == null)
            //    {
            //        // This is an assembly reference
            //        var fullName = GetFullName(reference);
            //        var assemblyName = new AssemblyName(fullName);
            //        //yield return assemblyName;
            //    }
            //    //else
            //    //{
            //    //    // This is a project reference
            //    //}
            //}

        }

        #endregion

        #region Not valid for Projects

        public void BeforeOpeningFile(ProjectItem projectItem) { }

        public void ProjectItemFinishedGenerating(ProjectItem projectItem) { }

        public bool ShouldAddProjectItem(string filePath) { return true; }

        #endregion
    }
 
}
