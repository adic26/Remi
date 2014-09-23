using System;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using TsdLib.TestSequence;

namespace TsdLib.Configuration
{

    /// <summary>
    /// Base class for a specific instance of a configuration. Multiple ConfigItems can be added to a ConfigGroup to form a selectable list used for parameterizing test sequences.
    /// </summary>
    [Serializable]
    public class ConfigItem
    {
        /// <summary>
        /// Gets or sets the name of the configuration item.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        public virtual string Name { get; set; }

        /// <summary>
        /// True to store configuration locally and on Remi. False to store locally only.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        public bool RemiSetting { get; set; }

        /// <summary>
        /// Returns the name of the configuration item.
        /// </summary>
        /// <returns>Name of the configuration item.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
    /// <summary>
    /// Contains base station config properties common to every system. Station config properties include those related to a specific system (based on host PC), eg. port/instrument addresses, physical location, etc.
    /// Station config is used to parameterize the test sequence, customizing the sequence to operate on different stations (ie. instruments with different addresses).
    /// </summary>
    [Serializable]
    public class StationConfigCommon : ConfigItem
    {
        private readonly StringCollection _assemblies;
        private readonly int _assemblyCount;

        /// <summary>
        /// Default constuctor required to initialize default values.
        /// </summary>
        public StationConfigCommon()
        {
            string[] tsdLibAssemblies =
                AppDomain.CurrentDomain.GetAssemblies()
                .Where(assy => assy.FullName.Contains("TsdLib"))
                .Select(assy => Regex.Match(assy.FullName, ".*(?=,.*,.*)").Value)
                .ToArray();

            _assemblies = new StringCollection();
            _assemblies.AddRange(tsdLibAssemblies);
            _assemblyCount = _assemblies.Count;
        }

        /// <summary>
        /// Gets a collection of the TsdLib assemblies currently loaded.
        /// </summary>
        [Editor("System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [ReadOnly(true)]
        [Category("Description")]
        public StringCollection Assemblies
        {
            get
            {
                //Duplicates will be added if the constructor is called and the object is deserialized
                while (_assemblyCount <= _assemblies.Count - 1)
                    _assemblies.RemoveAt(_assemblies.Count - 1);
                return _assemblies;
            }
        }
    }

    /// <summary>
    /// Contains base station config properties common to every type of product. Product config properties include those related to a specific DUT model, eg. radio bands, CPU chipset, etc.
    /// Product config is used to parameterize the test sequence, customizing the sequence to operate on different DUT models.
    /// </summary>
    [Serializable]
    public class ProductConfigCommon : ConfigItem
    {

    }

    /// <summary>
    /// Contains base test config properties common to every type of test. Test config properties include those related to a test system, eg. temperature profile, loop iterations, etc.
    /// Test config is used to parameterize the test sequence, allowing the same sequence to perform different test cases (ie. strict vs. relaxed or functional vs. parametric)
    /// </summary>
    [Serializable]
    public class TestConfigCommon : ConfigItem
    {

    }

    //TODO: don't edit Sequence config in a property grid - just launch the multi-line string editor
    /// <summary>
    /// Conatins the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    /// Can be parameterized by StationConfig, ProductConfig, TestConfig.
    /// </summary>
    [Serializable]
    public class SequenceConfig : ConfigItem
    {
        /// <summary>
        /// Gets the file name containing the test sequence source code.
        /// </summary>
        public override string Name
        {
            get { return Path.GetFileNameWithoutExtension(LocalFile); }
            set { LocalFile = Path.Combine(Directory.GetCurrentDirectory(), value + ".cs"); }
        }

        /// <summary>
        /// Gets the absolute path to the source code file.
        /// </summary>
        internal string LocalFile { get; set; }


        /// <summary>
        /// The source code containing the step-by-step instructions.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string TestSequenceSourceCode
        {
            get
            {
                if (!File.Exists(LocalFile))
                    File.WriteAllText(LocalFile, "//Add test sequence here by subclassing TsdLib.TestSequence.TestSequenceBase");
                return File.ReadAllText(LocalFile);
            }
            set { File.WriteAllText(LocalFile, value); }
        }

        /// <summary>
        /// Default constructor required for serialization.
        /// </summary>
        public SequenceConfig() { }

        /// <summary>
        /// Initialize a new SequenceConfig with the specified test sequence source code file.
        /// </summary>
        /// <param name="localFile">Path to the source code file containing the test sequence execute method.</param>
        public SequenceConfig(string localFile)
        {
            LocalFile = localFile;
        }

        private string _namespace;
        /// <summary>
        /// Gets the namespace declared in the test sequence source code.
        /// </summary>
        /// <returns>The namespace declaration.</returns>
        public string GetNamespace()
        {
            if (_namespace != null)
                return _namespace;

            string namespaceDeclarationLine = TestSequenceSourceCode
                .Split('\r', '\n')
                .FirstOrDefault(line => line.Trim().StartsWith("namespace"));

            if (namespaceDeclarationLine == null)
                throw new TestSequenceException(LocalFile);

            _namespace = namespaceDeclarationLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[1];
            return _namespace;
        }

        private string _className;
        /// <summary>
        /// Gets the name of the class declared in the test sequence source code.
        /// </summary>
        /// <returns>The class name.</returns>
        public string GetClassName()
        {
            if (_className != null)
                return _className;

            string classDeclarationLine = TestSequenceSourceCode
                .Split('\r', '\n')
                .FirstOrDefault(line => line.Trim().StartsWith("public class"));

            if (classDeclarationLine == null)
                throw new TestSequenceException(LocalFile);

            _className = classDeclarationLine.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries)[2];
            return _className;
        }
    }
}
