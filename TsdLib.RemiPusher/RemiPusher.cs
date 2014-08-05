using System;
using System.IO;
using TsdLib.Configuration;

namespace TsdLib.RemiPusher
{
    class RemiPusher
    {
        static int Main(string[] args)
        {
            if (args.Length < 4)
            {
                Console.WriteLine(string.Join(Environment.NewLine, "",
                    "                 TsdLib Remi Data Pusher",
                    "Pushes local configuration data to Remi",
                    "",
                    "Usage: TsdLib.RemiPusher.exe <fileName> <application name> <application version> TestSequences|Instruments",
                    ""));

                Console.ReadLine();
                return 1;
            }

            string fileName = args[0];
            string applicationName = args[1];
            string applicationVersion = args[2];
            string configType = args[3];

            PushToRemi(fileName, applicationName, applicationVersion, configType);

            return 0;
        }

        public static void PushToRemi(string fileName, string applicationName, string applicationVersion, string configType)
        {
            //TODO: initialize _remiControl to the live version
            RemiControlTest remiControl = new RemiControlTest(@"C:\temp\RemiSettingsTest");

            string data = File.ReadAllText(fileName);

            remiControl.WriteConfigStringToRemi(data, applicationName, applicationVersion, configType, fileName);
        }
    }
}
