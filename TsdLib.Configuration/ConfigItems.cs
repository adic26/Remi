using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing.Design;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Text.RegularExpressions;
using System.Xml.Serialization;

namespace TsdLib.Configuration

{
    /// <summary>
    /// Base class for a specific instance of a configuration.
    /// </summary>
    [Serializable]
    public abstract class ConfigItem : IConfigItem
    {
        //Can't use MarshalByRefObject, since it gives problems casting to and from the IConfigItem between the UI and Controller.
        //public override object InitializeLifetimeService()
        //{
        //    return null;
        //}

        /// <summary>
        /// Initialize the configuration properties to default values.
        /// </summary>
        [Browsable(true)]
        [Description("Initialize configuration values to their defaults")]
        public abstract void InitializeDefaultValues();

        internal ITestDetails Details { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration item.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        [XmlAttribute]
        public string Name { get; set; }

        /// <summary>
        /// True to store configuration locally and on a database. False to store locally only.
        /// </summary>
        [ReadOnly(true)]
        [Category("Description")]
        [XmlAttribute]
        public bool StoreInDatabase { get; set; }
        /// <summary>
        /// Returns true if the config item is an auto-generated default item.
        /// </summary>
        [Browsable(false)]
        [XmlAttribute]
        public bool IsDefault { get; set; }

        /// <summary>
        /// Initialize a new ConfigItem.
        /// </summary>
        protected ConfigItem()
            : this("Not Assigned", false, true) 
        {
            //This is currently not suggested, since default values do not get serialized. It is better for derived types to use the Init method.
            //foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            //{
            //    DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
            //    if (attr != null)
            //        prop.SetValue(this, attr.Value);
            //}
        }

        protected ConfigItem(string name, bool storeInDatabase, bool isDefault)
        {
            Name = name;
            StoreInDatabase = storeInDatabase;
            IsDefault = isDefault;
        }

        /// <summary>
        /// Performs a deep clone of the IConfigItem object.
        /// </summary>
        /// <returns>A new IConfigItem object.</returns>
        public IConfigItem Clone()
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
        /// Gets the name of the <see cref="ConfigItem"/>
        /// </summary>
        /// <returns>Name of the <see cref="ConfigItem"/>.</returns>
        public override string ToString()
        {
            return Name;
        }

        #region IComponent implementation

        /// <summary>
        /// Gets the <see cref="ISite"/> of the <see cref="IComponent"/>
        /// </summary>
        [Browsable(false)]
        [XmlIgnore]
        public ISite Site
        {
            // return our "site" which connects back to us to expose our tagged methods
            get { return new DesignerVerbSite(this); }
            set { throw new NotImplementedException(); }
        }

        /// <summary>
        /// Event that is fired when the <see cref="IComponent"/> is disposed.
        /// </summary>
        public event EventHandler Disposed;

        /// <summary>
        /// Dispose the <see cref="IComponent"/>.
        /// </summary>
        public void Dispose()
        {
            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        #endregion
    }

    /// <summary>
    /// Contains base station config properties common to every system. Station config properties include those related to a specific system (based on host PC), eg. port/instrument addresses, physical location, etc.
    /// Station config is used to parameterize the test sequence, customizing the sequence to operate on different stations (ie. instruments with different addresses).
    /// </summary>
    [Serializable]
    public abstract class StationConfigCommon : ConfigItem
    {
        protected StationConfigCommon()
            : this("NotAssigned", false, true)
        {

        }

        protected StationConfigCommon(string name, bool storeInDatabase, bool isDefault)
            : base(name, storeInDatabase, isDefault)
        {

        }
    }

    [Serializable]
    public class NullStationConfig : StationConfigCommon
    {
        public NullStationConfig()
            : base("NullStationConfig", false, true)
        {

        }

        public override void InitializeDefaultValues()
        {
            
        }
    }

    /// <summary>
    /// Contains base station config properties common to every type of product. Product config properties include those related to a specific DUT model, eg. radio bands, CPU chipset, etc.
    /// Product config is used to parameterize the test sequence, customizing the sequence to operate on different DUT models.
    /// </summary>
    [Serializable]
    public abstract class ProductConfigCommon : ConfigItem
    {
        protected ProductConfigCommon()
            : this("NotAssigned", false, true)
        {

        }

        protected ProductConfigCommon(string name, bool storeInDatabase, bool isDefault)
            : base(name, storeInDatabase, isDefault)
        {

        }
    }

    [Serializable]
    public class NullProductConfig : ProductConfigCommon
    {
        public NullProductConfig()
            : base("NullProductConfig", false, true)
        {

        }

        public override void InitializeDefaultValues()
        {

        }
    }

    /// <summary>
    /// Contains base test config properties common to every type of test. Test config properties include those related to a test system, eg. temperature profile, loop iterations, etc.
    /// Test config is used to parameterize the test sequence, allowing the same sequence to perform different test cases (ie. strict vs. relaxed or functional vs. parametric)
    /// </summary>
    [Serializable]
    public abstract class TestConfigCommon : ConfigItem
    {
        protected TestConfigCommon()
            : this("NotAssigned", false, true)
        {

        }

        protected TestConfigCommon(string name, bool storeInDatabase, bool isDefault)
            : base(name, storeInDatabase, isDefault)
        {

        }
    }

    [Serializable]
    public class NullTestConfig : TestConfigCommon
    {
        public NullTestConfig()
            : base("NullTestConfig", false, true)
        {

        }

        public override void InitializeDefaultValues()
        {

        }
    }

    /// <summary>
    /// Contains base sequence config properties.
    /// </summary>
    [Serializable]
    public abstract class SequenceConfigCommon : ConfigItem
    {

    }

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

    internal class HashSetConverter : TypeConverter
    {
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            HashSet<string> v = value as HashSet<string>;
            if (v != null && destinationType == typeof (string))
                return string.Join(Environment.NewLine, v);
            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
