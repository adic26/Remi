using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.Configuration
{
    public class RemiSettingsProvider : LocalFileSettingsProvider
    {
        static readonly object locker = new object();

        private IRemiControl _remiControl;

        public override string Name
        {
            get { return "RemiSettingsProvider"; }
        }

        public override string Description
        {
            get { return "Extends the LocalSettingsProvider by pushing/pulling local config values to/from the Remi database."; }
        }

        //TODO: initialize _remiControl to the live version
        public override void Initialize(string name, NameValueCollection values)
        {
            _remiControl = new RemiControlTest(@"C:\temp\RemiSettingsTest");
        }

        public override SettingsPropertyValueCollection GetPropertyValues(SettingsContext context, SettingsPropertyCollection properties)
        {
            lock (locker)
            {
                try
                {
                    SettingsPropertyValueCollection configFromRemi = new SettingsPropertyValueCollection();
                    
                    foreach (SettingsProperty settingProperty in properties)
                    {
                        string configType = settingProperty.PropertyType.GetGenericArguments()[0].Name;
                        Debug.WriteLine("Pulling " + configType + " from Remi.");
                        string valueFromRemi = _remiControl.ReadConfigStringFromRemi(Application.ProductName,
                            Application.ProductVersion, configType + ".xml");

                        SettingsPropertyValue settingValue = new SettingsPropertyValue(settingProperty)
                        {
                            SerializedValue = valueFromRemi,
                            Deserialized = false,
                            IsDirty = true,
                        };
                        configFromRemi.Add(settingValue);
                    }
                    
                    base.SetPropertyValues(context, configFromRemi);
                }
                catch (ConfigDoesNotExistInRemiException ex)
                {
                    Trace.WriteLine(ex.Message + " Pushing up local config data.");
                    SetPropertyValues(context, base.GetPropertyValues(context, properties));
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Could not pull config from REMI. Using local settings.");
                    Trace.WriteLine(ex.Message);
                }

                return base.GetPropertyValues(context, properties);
            }
        }

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
                try
                {
                    foreach (SettingsPropertyValue settingValue in values)
                    {
                        string configType = settingValue.Property.PropertyType.GetGenericArguments()[0].Name;

                        Debug.WriteLine("Pushing " + configType + " to Remi.");
                        
                        _remiControl.WriteConfigStringToRemi((string) settingValue.SerializedValue,
                            Application.ProductName, Application.ProductVersion, configType + ".xml");
                    }
                }
                catch (Exception ex)
                {
                    Trace.WriteLine("Could not push config to REMI.");
                    Trace.WriteLine("Exception type:" + ex.GetType().Name);
                    Trace.WriteLine("Exception message: " + ex.Message);
                }
            }
        }
    }
}