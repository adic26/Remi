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
            var blackBerryWlan = Aos_BCM4339.Connect();

            AddTestInfo(new TestInfo("WLAN Chipset Firmware Version", blackBerryWlan.GetChipsetFirmwareVersion()));

            blackBerryWlan.DisableWlan();
            blackBerryWlan.EnableWlan();
            blackBerryWlan.StopTx();
            blackBerryWlan.DisableDriver();
            blackBerryWlan.DisableMinimumPowerControl();
            blackBerryWlan.DisableWatchdog();
            blackBerryWlan.EnableDriver();

            blackBerryWlan.SetCountryCode("US");
            blackBerryWlan.GetCountry();
            blackBerryWlan.SetBand("b");
            blackBerryWlan.SetRate(2, "r", 1, 20);
            blackBerryWlan.SetChannel(1, 20);

            blackBerryWlan.EnableForceCal();
            blackBerryWlan.GetActivecal();
            blackBerryWlan.EnableScanSuppress();
            blackBerryWlan.EnableClosedLoopPowerControl();
            blackBerryWlan.SetTxPowerDefault();
            blackBerryWlan.StartTx();
            blackBerryWlan.StopTx();
        }
    }
}
