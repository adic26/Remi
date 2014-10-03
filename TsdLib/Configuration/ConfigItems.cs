using System;
using System.CodeDom;
using System.CodeDom.Compiler;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.CSharp;

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
        public string Name { get; set; }

        /// <summary>
        /// True to store configuration locally and on a database. False to store locally only.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        public bool StoreInDatabase { get; set; }

        /// <summary>
        /// Gets the name of the test system that the configuration item is used for.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        internal string TestSystemName
        {
            get { return ConfigManager.TestSystemName; }
        }

        /// <summary>
        /// Performs a deep clone of the ConfigItem object.
        /// </summary>
        /// <returns>A new ConfigItem object.</returns>
        public ConfigItem Clone()
        {
            IFormatter formatter = new BinaryFormatter();
            using (Stream stream = new MemoryStream())
            {
                formatter.Serialize(stream, this);
                stream.Seek(0, SeekOrigin.Begin);
                return (ConfigItem)formatter.Deserialize(stream);
            }
        }

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

    /// <summary>
    /// Conatins the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    /// Can be parameterized by StationConfig, ProductConfig, TestConfig.
    /// </summary>
    [Serializable]
    public class Sequence : ConfigItem
    {
        /// <summary>
        /// Gets the complete source code for the test sequence.
        /// </summary>
        [Browsable(false)]
        public string FullSourceCode { get; set; }

        private HashSet<string> _assemblyReferences;
        /// <summary>
        /// Gets or sets a list of assemblies needed to be referenced declared in the test sequence.
        /// </summary>
        //[Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HashSetConverter))]
        [Category("Dependencies")]
        public HashSet<string> AssemblyReferences
        {
            get { return _assemblyReferences ?? (_assemblyReferences = new HashSet<string> { "System.dll", "System.Xml.dll", "TsdLib.dll", TestSystemName + ".exe" }); }
            set { _assemblyReferences = value; }
        }


        private HashSet<string> _usingDirectives;
        /// <summary>
        /// Gets or sets a list of namespace import (using) statements declared in the test sequence.
        /// </summary>
        //[Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HashSetConverter))]
        [Category("Dependencies")]
        public HashSet<string> UsingDirectives
        {
            get { return _usingDirectives ?? (_usingDirectives = new HashSet<string> { "System", "TsdLib.TestResults", "TsdLib.TestSequence", TestSystemName + ".Configuration", TestSystemName + ".Instruments" }); }
            set { _usingDirectives = value; }
        }

        /// <summary>
        /// Gets the namespace declared in the test sequence.
        /// </summary>
        [Category("Declarations")]
        [ReadOnly(true)]
        public string NamespaceDeclaration { get { return TestSystemName + ".Sequences"; } }

        /// <summary>
        /// Gerts the name of the test sequence class.
        /// </summary>
        [Category("Declarations")]
        [ReadOnly(true)]
        public string ClassDeclaration { get { return TestSystemName; } }

        /// <summary>
        /// Gets the signature of the Execute method defined in the test sequence class.
        /// </summary>
        [Category("Declarations")]
        [ReadOnly(true)]
        public string ExecuteMethodSignature { get { return "protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)"; } }

        /// <summary>
        /// Gets or sets the source code for the step-by-step execution of the test sequence.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string SequenceCode { get; set; }

        /// <summary>
        /// Initialize a new Sequence configuration instance from persisted settings.
        /// </summary>
        public Sequence()
        {
            //Assembly clientAssembly = Assembly.GetEntryAssembly();
            //TestSystemName = clientAssembly.GetName().Name;
            //Type[] clientTypes = clientAssembly.GetTypes();
            //Type stationConfigType = clientTypes.FirstOrDefault(t => t.BaseType == typeof(StationConfigCommon)) ?? typeof(StationConfigCommon);
            //Type productConfigType = clientTypes.FirstOrDefault(t => t.BaseType == typeof(ProductConfigCommon)) ?? typeof(ProductConfigCommon);
            //Type testConfigType = clientTypes.FirstOrDefault(t => t.BaseType == typeof(TestConfigCommon)) ?? typeof(TestConfigCommon);

            string stationConfigType = ConfigManager.ConfigGroups
                .Select(c => c.BaseConfigType)
                .FirstOrDefault(bc => bc == "StationConfigCommon") ?? "StationConfigCommon";

            string productConfigType = ConfigManager.ConfigGroups
                .Select(c => c.BaseConfigType)
                .FirstOrDefault(bc => bc == "ProductConfigCommon") ?? "ProductConfigCommon";

            string testConfigType = ConfigManager.ConfigGroups
                .Select(c => c.BaseConfigType)
                .FirstOrDefault(bc => bc == "TestConfigCommon") ?? "TestConfigCommon";

            if (FullSourceCode == null)
            {
                try
                {
                    CodeNamespace cns = new CodeNamespace();

                    cns.Imports.AddRange(UsingDirectives.Select(s => new CodeNamespaceImport(s)).ToArray());

                    cns.Name = NamespaceDeclaration;

                    CodeTypeDeclaration sequenceClass = new CodeTypeDeclaration(ClassDeclaration);
                    CodeTypeReference sequenceBaseReference = new CodeTypeReference("TestSequenceBase", new CodeTypeReference(stationConfigType), new CodeTypeReference(productConfigType), new CodeTypeReference(testConfigType));
                    sequenceClass.BaseTypes.Add(sequenceBaseReference);

                    CodeMemberMethod executeMethod = new CodeMemberMethod { Name = "Execute" };
                    executeMethod.Parameters.AddRange(new[] { new CodeParameterDeclarationExpression(stationConfigType, "stationConfig"), new CodeParameterDeclarationExpression(productConfigType, "productConfig"), new CodeParameterDeclarationExpression(testConfigType, "testConfig") });
                    executeMethod.Statements.Add(new CodeSnippetStatement(SequenceCode));

                    sequenceClass.Members.Add(executeMethod);

                    cns.Types.Add(sequenceClass);

                    using (StringWriter writer = new StringWriter(new StringBuilder()))
                    {
                        using (CSharpCodeProvider provider = new CSharpCodeProvider())
                            provider.GenerateCodeFromNamespace(cns, writer, new CodeGeneratorOptions { BracingStyle = "C" });
                        FullSourceCode = writer.ToString();
                    }
                }
                catch (Exception ex)
                {
                    FullSourceCode = "ERROR: " + ex.GetType().Name + Environment.NewLine + ex.Message;
                }
            }
        }

        /// <summary>
        /// Initialize a new Sequence config uration instance from a source code file.
        /// </summary>
        /// <param name="csFile">C# code file containing the complete test sequence class.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on the database. False to store locally only.</param>
        /// <param name="assemblyReferences">Zero or more assemblies that are referenced by the test sequence class.</param>
        public Sequence(string csFile, bool storeInDatabase, params string[] assemblyReferences)
        {
            FullSourceCode = File.ReadAllText(csFile);
            Name = Path.GetFileNameWithoutExtension(csFile);
            StoreInDatabase = storeInDatabase;
            SequenceCode = Regex.Match(FullSourceCode, @"(?<=protected override void Execute.*\{).*(?=\}\s+\}\s+\})", RegexOptions.Singleline).Value;
            foreach (string assemblyReference in assemblyReferences)
                AssemblyReferences.Add(assemblyReference);
        }
    }

    internal class HashSetConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            HashSet<string> v = value as HashSet<string>;
            if (v != null && destinationType == typeof (string))
            {
                return string.Join(Environment.NewLine, v);
            }
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
