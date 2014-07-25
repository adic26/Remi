using System;
using System.Diagnostics;
using TsdLib.Config;

namespace ConfigTest
{
    class ConfigTestProgram
    {
        static void Main()
        {
            Trace.Listeners.Add(new ConsoleTraceListener());

            //If the station is in 'Development Mode' the config will be writable and changes will be pushed up to Remi.
            //If the station is in 'Production Mode' the config will be read-only.
            //If the station is in 'Offline Mode' the config will be writeable but changes will not be pushed up to Remi.
            //IConfigGroup<ProductConfig> pcedit = Config.Manager.EditConfigGroup<ProductConfig>();

            Config.Manager.EditConfig<ProductConfig>();


            //IConfigGroup<StationConfig> stationConfig = Config.GetConfig<StationConfig>();

            //stationConfig.Edit();

            //foreach (StationConfig station in stationConfig)
            //    Console.WriteLine(station.Name + ": InstrumentAddress = " + station.InstrumentAddress);

            Console.ReadLine();
        }
    }
}
