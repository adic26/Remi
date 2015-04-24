using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using TsdLib.Configuration;
using System.Configuration;
using TsdLib.Configuration.Connections;
using TsdLib.Configuration.Details;

namespace TestClient
{
    class TestClientProgram
    {
#if DEBUG
        private const OperatingMode DefaultMode = OperatingMode.Engineering;
#else
        private const OperatingMode DefaultMode = OperatingMode.Production;
#endif

        private const string TestSystemNameArg = "-testSystemName";
        private const string TestSystemVersionArg = "-testSystemVersion";
        private const string TestSystemVersionMaskArg = "-testSystemVersionMask";
        private const string TestSystemModeArg = "-testSystemMode";
        private const string LocalDomainArg = "-localDomain";
        private const string SettingsLocationArg = "-settingsLocation";
        private const string SeqFolderArg = "-seq";

        private static List<string> _argsList;

        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        private static void Main(string[] args)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Trace.Listeners.Add(new ConsoleTraceListener());

            _argsList = args.ToList();

            string testSystemName = getConfigValue(TestSystemNameArg) ?? Application.ProductName;
            Version testSystemVersion = Version.Parse(getConfigValue(TestSystemVersionArg) ?? Application.ProductVersion.Split('-')[0]);
            string testSystemVersionMask = getConfigValue(TestSystemVersionMaskArg) ?? @"\d+\.\d+";
            OperatingMode testSystemMode = (OperatingMode) Enum.Parse(typeof (OperatingMode), getConfigValue(TestSystemModeArg) ?? DefaultMode.ToString());
            bool localDomain = bool.Parse(getConfigValue(LocalDomainArg) ?? "false");
            string settingsLocation = getConfigValue(SettingsLocationArg) ?? @"";

            ITestDetails testDetails = new TestDetails(testSystemName, testSystemVersion, testSystemMode);

            IConfigConnection sharedConfigConnection = getConfigConnection(settingsLocation, testSystemVersionMask);

            if (args.Contains(SeqFolderArg))
            {
                SequenceSync.SynchronizeSequences(testDetails, sharedConfigConnection, getConfigValue(SeqFolderArg), true, false);
                return;
            }

            Controller c = new Controller(testDetails, sharedConfigConnection, localDomain);
            Application.Run(c.UI);
        }

        private static IConfigConnection getConfigConnection(string settingsLocation, string testSystemVersionMask)
        {
            IConfigConnection sharedConfigConnection;
#if REMICONTROL
            if (string.IsNullOrWhiteSpace(settingsLocation))
                sharedConfigConnection = new TsdLib.DataAccess.DatabaseConfigConnection(testSystemVersionMask);
            else
                sharedConfigConnection = new FileSystemConnection(new DirectoryInfo(settingsLocation), testSystemVersionMask);
#else
            if (string.IsNullOrWhiteSpace(settingsLocation))
                settingsLocation = @"\\fsg16ykf\personal\jmckee\TsdLibSettings";
            sharedConfigConnection = new FileSystemConnection(new DirectoryInfo(settingsLocation), testSystemVersionMask);
#endif
            return sharedConfigConnection;
        }

        private static string getConfigValue(string key)
        {
            try
            {
                if (_argsList.Contains(key))
                    return _argsList[_argsList.IndexOf(key) + 1];
                string appConfigValue = ConfigurationManager.AppSettings[key];
                return string.IsNullOrWhiteSpace(appConfigValue) ? null : appConfigValue;
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
                return null;
            }
        }
    }

}
