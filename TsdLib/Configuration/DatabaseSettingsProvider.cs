using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;

namespace TsdLib.Configuration
{
    /// <summary>
    /// Extends System.Configuration.LocalFileSettingsProvider to persist application settings to a database.
    /// </summary>
    public class DatabaseSettingsProvider : LocalFileSettingsProvider
    {
        static readonly object locker = new object();

        /// <summary>
        /// Gets the name of the settings provider.
        /// </summary>
        public override string Name
        {
            get { return GetType().Name; }
        }

        /// <summary>
        /// Gets a description of the settings provider.
        /// </summary>
        public override string Description
        {
            get { return "Extends the LocalSettingsProvider by pushing/pulling local config values to/from the database."; }
        }

        /// <summary>
        /// Initialize the settings provider.
        /// </summary>
        /// <param name="name">The name of the settings provider.</param>
        /// <param name="values">The values for initialization.</param>
        public override void Initialize(string name, NameValueCollection values)
        {
            
            
        }

        /// <summary>
        /// Returns the collection of settings property values for the specified application instance and settings property group.
        /// </summary>
        /// <param name="context">A SettingsContext describing the current application use.</param>
        /// <param name="properties">A SettingsPropertyCollection containing the settings property group whose values are to be retrieved.</param>
        /// <returns>A SettingsPropertyValueCollection containing the values for the specified settings property group.</returns>
        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            lock (locker)
            {
                try
                {
                    SettingsPropertyValueCollection configFromDb = new SettingsPropertyValueCollection();

                    DatabaseConnection databaseConnection = (DatabaseConnection) context["DatabaseConnection"];

                    foreach (SettingsProperty settingProperty in properties)
                    {
                        string configType = settingProperty.PropertyType.GetGenericArguments()[0].Name;
                        Debug.WriteLine("Pulling " + configType + " from database.");
                        string valueFromDb = databaseConnection.ReadStringFromDatabase(configType + ".xml");

                        SettingsPropertyValue settingValue = new SettingsPropertyValue(settingProperty)
                        {
                            SerializedValue = valueFromDb,
                            Deserialized = false,
                            IsDirty = true,
                        };
                        configFromDb.Add(settingValue);
                    }
                    
                    base.SetPropertyValues(context, configFromDb);
                }
                catch (DataDoesNotExistException ex)
                {
                    Trace.WriteLine(ex.Message + " Pushing up local config data.");
                    SetPropertyValues(context, base.GetPropertyValues(context, properties));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine(ex.GetType().Name + ": Could not pull config from database. Using local settings.");
                    Trace.WriteLine("STACK TRACE: " + ex.StackTrace);
                    Trace.WriteLine(ex.Message);
                }

                return base.GetPropertyValues(context, properties);
            }
        }

        /// <summary>
        /// Sets the values of the specified group of property settings.
        /// </summary>
        /// <param name="context">A SettingsContext describing the current application usage.</param>
        /// <param name="values">A SettingsPropertyValueCollection representing the group of property settings to set.</param>
        public override void SetPropertyValues(SettingsContext context, SettingsPropertyValueCollection values)
        {
            lock (locker)
            {
                // ReSharper disable once NotAccessedVariable - used for reading the PropertyValue to set the IsDirty flag
                object dummy;
                foreach (SettingsPropertyValue settingsPropertyValue in values)
                    // ReSharper disable once RedundantAssignment
                    dummy = settingsPropertyValue.PropertyValue;

                base.SetPropertyValues(context, values);

                DatabaseConnection databaseConnection = (DatabaseConnection)context["DatabaseConnection"];

                try
                {
                    foreach (SettingsPropertyValue settingValue in values)
                    {
                        string configType = settingValue.Property.PropertyType.GetGenericArguments()[0].Name;

                        Debug.WriteLine("Pushing " + configType + " to database.");
                        
                        databaseConnection.WriteStringToDatabase((string) settingValue.SerializedValue, configType + ".xml");
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Could not push config to database.");
                    Trace.WriteLine("Exception type:" + ex.GetType().Name);
                    Trace.WriteLine("Exception message: " + ex.Message);
                }
            }
        }
    }
}