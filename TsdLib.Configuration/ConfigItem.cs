﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Xml.Serialization;
using TsdLib.Configuration.Utilities;

namespace TsdLib.Configuration

{
    /// <summary>
    /// Base class for a specific instance of a configuration.
    /// </summary>
    [Serializable]
    public abstract class ConfigItem : MarshalByRefObject, IConfigItem, IComponent
    {
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
        /// Initialize a new ConfigItem. Should only be used by the serialization engine.
        /// </summary>
        protected ConfigItem() { }

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
        /// Gets the name of the common base type.
        /// </summary>
        public string CommonBaseTypeName
        {
            get { return ConfigUtilities.GetBaseTypeName(GetType()); }
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
        /// Dispose the <see cref="ConfigItem"/>.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
            if (Disposed != null)
                Disposed(this, EventArgs.Empty);
        }

        /// <summary>
        /// Dispose of any resources being used by the test sequence.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {

        }

        #endregion


        //Can't use MarshalByRefObject, since it gives problems casting to and from the IConfigItem between the UI and Controller.
        public override object InitializeLifetimeService()
        {
            return null;
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