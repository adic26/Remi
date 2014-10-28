﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;

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
        public string TestSystemName { get; set; }

        /// <summary>
        /// Initialize a new configuration instance from persisted settings.
        /// </summary>
        public ConfigItem() { }

        /// <summary>
        /// Initialize a new ConfigItem with the specified parameters.
        /// </summary>
        /// <param name="name">Name of the configuration instance..</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public ConfigItem(string name, bool storeInDatabase, string testSystemName)
        {
// ReSharper disable once DoNotCallOverridableMethodsInConstructor
            Name = name;
            StoreInDatabase = storeInDatabase;
            TestSystemName = testSystemName;
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
        /// <summary>
        /// Initialize a new station configuration instance from persisted settings.
        /// </summary>
        public StationConfigCommon() { }

        /// <summary>
        /// Initialize a new StationConfigCommon instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public StationConfigCommon(string name, bool storeInDatabase, string testSystemName)
            : base(name, storeInDatabase, testSystemName) { }
    }

    /// <summary>
    /// Contains base station config properties common to every type of product. Product config properties include those related to a specific DUT model, eg. radio bands, CPU chipset, etc.
    /// Product config is used to parameterize the test sequence, customizing the sequence to operate on different DUT models.
    /// </summary>
    [Serializable]
    public class ProductConfigCommon : ConfigItem
    {
        /// <summary>
        /// Initialize a new product configuration instance from persisted settings.
        /// </summary>
        public ProductConfigCommon() { }

        /// <summary>
        /// Initialize a new StationConfigCommon instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public ProductConfigCommon(string name, bool storeInDatabase, string testSystemName)
            : base(name, storeInDatabase, testSystemName) { }
    }

    /// <summary>
    /// Contains base test config properties common to every type of test. Test config properties include those related to a test system, eg. temperature profile, loop iterations, etc.
    /// Test config is used to parameterize the test sequence, allowing the same sequence to perform different test cases (ie. strict vs. relaxed or functional vs. parametric)
    /// </summary>
    [Serializable]
    public class TestConfigCommon : ConfigItem
    {
        /// <summary>
        /// Initialize a new test configuration instance from persisted settings.
        /// </summary>
        public TestConfigCommon() { }

        /// <summary>
        /// Initialize a new TestConfigCommon instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public TestConfigCommon(string name, bool storeInDatabase, string testSystemName)
            : base(name, storeInDatabase, testSystemName) { }
    }

    /// <summary>
    /// Conatins the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    /// Can be parameterized by StationConfig, ProductConfig, TestConfig.
    /// </summary>
    [Serializable]
    public class Sequence : ConfigItem
    {
        private string _name;
        /// <summary>
        /// Gets or sets the name of the test sequence. Also updates the class name in the source code.
        /// </summary>
        public override sealed string Name
        {
            get { return _name; }
            set
            {
                _name = value;
                if (!string.IsNullOrWhiteSpace(SourceCode))
                    SourceCode = Regex.Replace(SourceCode, @"(?<=class )\w+", value);
            }
        }

        /// <summary>
        /// Gets or sets a list of assemblies needed to be referenced declared in the test sequence.
        /// </summary>
        //[Editor(@"System.Windows.Forms.Design.StringCollectionEditor, System.Design, Version=2.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a", typeof(UITypeEditor))]
        [Editor(typeof (MultiLineStringEditor), typeof (UITypeEditor))]
        [TypeConverter(typeof (HashSetConverter))]
        [Category("Dependencies")]
        public HashSet<string> AssemblyReferences { get; set; }

        /// <summary>
        /// Gets or sets the source code for the step-by-step execution of the test sequence.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string SourceCode { get; set; }

        /// <summary>
        /// Initialize a new Sequence configuration instance from persisted settings.
        /// </summary>
        public Sequence()
        {
            //TODO: make sure sequence code is generated in pre-build event
        }

        /// <summary>
        /// Initialize a new Sequence instance.
        /// </summary>
        /// <param name="name">Name of the configuration instance.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on a database. False to store locally only.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public Sequence(string name, bool storeInDatabase, string testSystemName)
            : base(name, storeInDatabase, testSystemName)
        {
            SourceCode =
@"
using System.Diagnostics;
using " + testSystemName + @".Configuration;
using TsdLib.TestSequence;
namespace " + testSystemName + ".Sequences" + Environment.NewLine +
@"{
    public class " + name + @" : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            Debugger.Break();
        }
    }
}
";

            AssemblyReferences = new HashSet<string>(new [] { "System.dll", "System.Xml.dll", "TsdLib.dll", testSystemName + ".exe" });
        }

        /// <summary>
        /// Initialize a new Sequence config uration instance from a source code file.
        /// </summary>
        /// <param name="csFile">C# code file containing the complete test sequence class.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on the database. False to store locally only.</param>
        /// <param name="assemblyReferences">Zero or more assemblies that are referenced by the test sequence class.</param>
        /// <param name="testSystemName">Name of the test system the config item is used for.</param>
        public Sequence(string csFile, bool storeInDatabase, string testSystemName, IEnumerable<string> assemblyReferences)
            : base(Path.GetFileNameWithoutExtension(csFile), storeInDatabase, testSystemName)
        {
            SourceCode = File.ReadAllText(csFile);
            AssemblyReferences = new HashSet<string>(assemblyReferences);
            Name = Regex.Match(SourceCode, @"(?<=class )\w+").Value;
            StoreInDatabase = storeInDatabase;
            TestSystemName = testSystemName;
        }

        //private string writeSourceCode()
        //{
        //    //TODO: move to property initializer instead of constructor - this must run AFTER field initializers
        //    string stationConfigType = ConfigManager.ConfigGroups
        //        .Select(c => c.BaseConfigType)
        //        .FirstOrDefault(bc => bc == "StationConfigCommon") ?? "StationConfigCommon";

        //    string productConfigType = ConfigManager.ConfigGroups
        //        .Select(c => c.BaseConfigType)
        //        .FirstOrDefault(bc => bc == "ProductConfigCommon") ?? "ProductConfigCommon";

        //    string testConfigType = ConfigManager.ConfigGroups
        //        .Select(c => c.BaseConfigType)
        //        .FirstOrDefault(bc => bc == "TestConfigCommon") ?? "TestConfigCommon";

        //    try
        //    {
        //        CodeNamespace cns = new CodeNamespace();

        //        cns.Imports.AddRange(UsingDirectives.Select(s => new CodeNamespaceImport(s)).ToArray());

        //        cns.Name = TestSystemName + ".Sequences";

        //        CodeTypeDeclaration sequenceClass = new CodeTypeDeclaration(TestSystemName);
        //        CodeTypeReference sequenceBaseReference = new CodeTypeReference("TestSequenceBase", new CodeTypeReference(stationConfigType), new CodeTypeReference(productConfigType), new CodeTypeReference(testConfigType));
        //        sequenceClass.BaseTypes.Add(sequenceBaseReference);

        //        CodeMemberMethod executeMethod = new CodeMemberMethod { Name = "Execute" };
        //        executeMethod.Parameters.AddRange(new[] { new CodeParameterDeclarationExpression(stationConfigType, "stationConfig"), new CodeParameterDeclarationExpression(productConfigType, "productConfig"), new CodeParameterDeclarationExpression(testConfigType, "testConfig") });
        //        executeMethod.Statements.Add(new CodeSnippetStatement(SequenceCode));

        //        sequenceClass.Members.Add(executeMethod);

        //        cns.Types.Add(sequenceClass);

        //        using (StringWriter writer = new StringWriter(new StringBuilder()))
        //        {
        //            using (CSharpCodeProvider provider = new CSharpCodeProvider())
        //                provider.GenerateCodeFromNamespace(cns, writer, new CodeGeneratorOptions { BracingStyle = "C" });
        //            return writer.ToString();
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        return "ERROR: " + ex.GetType().Name + Environment.NewLine + ex.Message;
        //    }
            
        //}
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
