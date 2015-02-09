using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using TsdLib.Configuration.Common;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Conatains the step-by-step instrument control and measurement capturing instructions that make up a test sequence.
    /// Can be parameterized by StationConfig, ProductConfig, TestConfig.
    /// </summary>
    [Serializable]
    public class Sequence : SequenceConfigCommon
    {
        /// <summary>
        /// Gets or sets a list of assemblies needed to be referenced declared in the test sequence.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [TypeConverter(typeof(HashSetConverter))]
        [Category("Dependencies")]
        public HashSet<string> AssemblyReferences { get; set; }

        /// <summary>
        /// Gets or sets the source code for the step-by-step execution of the test sequence.
        /// </summary>
        [Editor(typeof(MultiLineStringEditor), typeof(UITypeEditor))]
        [Category("Test Sequence")]
        public string SourceCode { get; set; }

        public string Namespace
        {
            get { return Regex.Match(SourceCode, @"(?<=namespace )[\w\.]+").Value; }
        }

        public string ClassName
        {
            get { return Regex.Match(SourceCode, @"(?<=class )\w+").Value; }
        }

        public string FullTypeName
        {
            get { return Namespace + "." + ClassName; }
        }

        /// <summary>
        /// Initialize the sequence configuration with an empty ExecuteTest method and basic assembly references.
        /// </summary>
        public override void InitializeDefaultValues()
        {
            string assemblyName = Assembly.GetEntryAssembly().GetName().Name;

            SourceCode = string.Format(
@"
using System.Diagnostics;
using {0}.Configuration;
using TsdLib.TestSequence;
namespace {0}.Sequences
{{
    public class DefaultSequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {{
        protected override void ExecuteTest(StationConfig stationConfig, ProductConfig productConfig, params TestConfig[] testConfigs)
        {{
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            foreach (TestConfig testConfig in testConfigs)
            {{
                //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
                Debugger.Break();
            }}
        }}
    }}
}}
",
                assemblyName);

            AssemblyReferences = new HashSet<string>(AppDomain.CurrentDomain.GetAssemblies().Select(asy => Path.GetFileName(asy.GetName().CodeBase))) { Path.GetFileName(Assembly.GetEntryAssembly().GetName().CodeBase) };
        }

        /// <summary>
        /// Initialize a new Sequence configuration instance from persisted settings.
        /// </summary>
        public Sequence()
        {

        }

        /// <summary>
        /// Initialize a new Sequence configuration instance from a source code file.
        /// </summary>
        /// <param name="csFile">C# code file containing the complete test sequence class.</param>
        /// <param name="storeInDatabase">True to store configuration locally and on the database. False to store locally only.</param>
        /// <param name="assemblyReferences">Zero or more assemblies that are referenced by the test sequence class.</param>
        public Sequence(string csFile, bool storeInDatabase, IEnumerable<string> assemblyReferences)
        {
            SourceCode = File.ReadAllText(csFile);
            AssemblyReferences = new HashSet<string>(assemblyReferences);
            StoreInDatabase = storeInDatabase;
            Name = Regex.Match(SourceCode, @"(?<=class )\w+").Value;

            var namespaceMatch = Regex.Match(SourceCode, @"(?<=namespace.*Sequences.)\w+");
            if (namespaceMatch.Success)
                Name = Name.Insert(0, namespaceMatch.Value + ".");
            Trace.WriteLine("Name = " + Name);
            IsDefault = false;
        }
    }
}
