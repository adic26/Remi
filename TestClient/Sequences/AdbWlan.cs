using System.Threading;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Measurements;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbWlan : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            IBlackBerryWlan blackBerryWlan = Aos_BCM4339.Connect();
            //IBlackBerryWlan blackBerryWlan = new AosBCM4339_SafeCommands();

            AddTestInfo(new TestInfo("WLAN Chipset Family", blackBerryWlan.GetChipsetFamily()));

            AddTestInfo(new TestInfo("WLAN Chipset Firmware Version", blackBerryWlan.GetChipsetFirmwareVersion()));

            bool tx = false;

            if (tx)
            {
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

            else
            {
                blackBerryWlan.StopRx();
                blackBerryWlan.DisableMinimumPowerControl();
                blackBerryWlan.DisableWatchdog();
                blackBerryWlan.EnableDriver();
                blackBerryWlan.SetCountryCode("US");
                blackBerryWlan.SetBand("b");
                blackBerryWlan.SetChannel(1, 20);
                blackBerryWlan.EnableForceCal();
                blackBerryWlan.EnableScanSuppress();
                blackBerryWlan.ResetCounter();
                blackBerryWlan.StartRx();
                Thread.Sleep(2000);
                int framesReceived = blackBerryWlan.GetRxFrameCount();
            }
        }
    }
}
