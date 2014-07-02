using System.ComponentModel;
using System.Configuration;

namespace TsdLib.Controller
{
    /// <summary>
    /// Common settings that will be used for every test station
    /// </summary>
    //[DefaultProperty("QraNumber")] //Specify which setting is selected when the PropertyGrid is launched
    public class Settings : ApplicationSettingsBase, ISettings
    {
        public Settings()
        {
            Synchronized(this);

            /* Define default settings for complex or custom types, eg.
             * if (this["complex_type"] == null)
             *      complex_type = new complex_type();
             */
        }

        public void Edit()
        {
            new SettingsForm(this).ShowDialog();
        }

        //Example setting
        [UserScopedSetting] //Setting is scoped to the currently logged in user and can be changed at runtime.
        [DefaultSettingValue("QRA-14-0001")]
        [Category("QRA details")] //Specify a category in the PropertyGrid for this setting to be sorted into
        [Description("QRA number for this batch")] //Description will appear at the bottom of the PropertyGrid
        public string QraNumber
        {
            get { return (string)this["QraNumber"]; }
            set { this["QraNumber"] = value; }
        }

        
    }

    //Expose via interface to hide inhertied members from intellisense
    public interface ISettings
    {
        void Edit();
        
        string QraNumber { get; set; }
    }
}


//public static string GetDefaultExeConfigPath(ConfigurationUserLevel userLevel)
//{
//  try
//  {
//    var UserConfig = ConfigurationManager.OpenExeConfiguration(userLevel);
//    return UserConfig.FilePath;
//  }
//  catch (ConfigurationException e)
//  {
//    return e.Filename;
//  }
//}