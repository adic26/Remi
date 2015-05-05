using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.Configuration.Null;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class TelnetWiFiTest : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            BlackBerry bb = BlackBerry.Connect(token, "10.231.176.190");

            var imei = bb.GetImei();
        }
    }
}
