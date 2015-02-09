using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;

namespace TsdLib.Configuration

{
    /// <summary>
    /// Base class for a specific instance of a configuration.
    /// </summary>
    [Serializable]
    public abstract class ConfigItem : IConfigItem, IComponent
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
        /// Returns true by default, indicating that the config item is valid in the current context. Override to add conditions to return false.
        /// </summary>
        [Browsable((false))]
        [XmlIgnore]
        public virtual bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Initialize a new ConfigItem.
        /// </summary>
        protected ConfigItem()
        {
            //This is currently not suggested, since default values do not get serialized. It is better for derived types to use the Init method.
            //foreach (PropertyDescriptor prop in TypeDescriptor.GetProperties(this))
            //{
            //    DefaultValueAttribute attr = (DefaultValueAttribute)prop.Attributes[typeof(DefaultValueAttribute)];
            //    if (attr != null)
            //        prop.SetValue(this, attr.Value);
            //}
        }

        /// <summary>
        /// Initialize a new ConfigItem.
        /// </summary>
        /// <param name="name">Name to assign to the new config item.</param>
        /// <param name="storeInDatabase">True if the config item will be stored in the shared config location.</param>
        /// <param name="isDefault">True if the config item is auto-generated, and should be excluded when 'real' configs are present.</param>
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
        /// <summary>
        /// Gets or sets a list of machine names that can use this station config item.
        /// </summary>
        public List<string> MachineNames { get; set; }

        public override bool IsValid
        {
            get { return MachineNames.Contains(Environment.MachineName); }
        }

        protected StationConfigCommon()
        {

        }

        protected StationConfigCommon(string name, bool storeInDatabase, bool isDefault)
            : base(name, storeInDatabase, isDefault)
        {
            MachineNames = new List<string> {Environment.MachineName};
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
