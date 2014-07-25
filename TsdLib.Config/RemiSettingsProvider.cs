﻿using System;
using System.Collections.Specialized;
using System.Configuration;
using System.Diagnostics;
using System.Windows.Forms;

namespace TsdLib.Config
{
    public class RemiSettingsProvider : LocalFileSettingsProvider
    {
        static readonly object locker = new object();

        private IRemiConfigAccess _remiConfig;

        public override string Name
        {
            get { return "RemiSettingsProvider"; }
        }

        public override string Description
        {
            get { return "Extends the LocalSettingsProvider by pushing/pulling local config values to/from the Remi database."; }
        }

        public override void Initialize(string name, NameValueCollection values)
        {
            //TODO: initialize _remiConfig to the live version
            _remiConfig = new RemiConfigAccessTest(@"C:\temp\RemiSettingsTest");
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
                        string valueFromRemi = _remiConfig.ReadConfigStringFromRemi(Application.ProductName,
                            Application.ProductVersion, configType);

                        SettingsPropertyValue settingValue = new SettingsPropertyValue(settingProperty)
                        {
                            SerializedValue = valueFromRemi,
                            Deserialized = false,
                        };
                        settingValue.IsDirty = true; //make sure the local settings are written
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
                base.SetPropertyValues(context, values);
                try
                {
                    foreach (SettingsPropertyValue settingValue in values)
                    {
                        string configType = settingValue.Property.PropertyType.GetGenericArguments()[0].Name;

                        Debug.WriteLine("Pushing " + configType + " to Remi.");

                        _remiConfig.WriteConfigStringToRemi((string) settingValue.SerializedValue,
                            Application.ProductName, Application.ProductVersion, configType);
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