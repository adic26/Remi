using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbWlan : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            var BlackBerryWlan = Aos_BCM4339.Connect();

            TestInfo.Add(new TestInfo("WLAN Chipset Firmware Version", BlackBerryWlan.GetChipsetFirmwareVersion()));

            BlackBerryWlan.DisableWlan();
            BlackBerryWlan.EnableWlan();
            BlackBerryWlan.StopTx();
            BlackBerryWlan.DisableDriver();
            BlackBerryWlan.DisableMinimumPowerControl();
            BlackBerryWlan.DisableWatchdog();
            BlackBerryWlan.EnableDriver();

            BlackBerryWlan.SetCountryCode("US");
            BlackBerryWlan.GetCountry();
            BlackBerryWlan.SetBand("b");
            BlackBerryWlan.SetRate(2, "r", 1, 20);
            BlackBerryWlan.SetChannel(1, 20);

            BlackBerryWlan.EnableForceCal();
            BlackBerryWlan.GetActivecal();
            BlackBerryWlan.EnableScanSuppress();
            BlackBerryWlan.EnableClosedLoopPowerControl();
            BlackBerryWlan.SetTxPowerDefault();
            BlackBerryWlan.StartTx();
            BlackBerryWlan.StopTx();
        }
    }
}
