using TestClient.Configuration;
using TestClient.Instruments;
using TsdLib.TestSystem.TestSequence;

namespace TestClient.Sequences
{
    public class AdbOverWifi : SequentialTestSequence<StationConfig, ProductConfig, TestConfig>
    {
        protected override void ExecuteTest(System.Threading.CancellationToken token, StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            Aos_WiFi aos = Aos_WiFi.Connect(token);

            string ipAddress = aos.GetWlanIpAddress();

            aos.SetTcpPort(5555);

            aos.EnableTcpMode();

            aos.ConnectViaWifi(ipAddress);
        }
    }
}
