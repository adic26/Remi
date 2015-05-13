using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbBluetooth : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            Aos_BT bt = Aos_BT.Connect(token);

            bt.EnableBtTestMode();
        }
    }
}
