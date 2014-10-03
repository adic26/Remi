using System.Windows.Forms;
using TestClient.Configuration;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
#if DEBUG
        private const bool Released = false;
#else
        private const bool Released = true;
#endif

        public Controller(bool devMode)
            : base(devMode, new DatabaseFolderConnection(@"C:\temp\RemiSettingsTest", "TestClient", Application.ProductVersion, Released)) { }
    }
}