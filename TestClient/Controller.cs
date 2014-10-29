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
        public View TestClientView;

        public Controller(bool devMode, string testSystemName, string testSystemVersion, bool localDomain = false)
            : base(devMode, testSystemName, testSystemVersion, new DatabaseFolderConnection(@"C:\temp\TsdLibSettings", testSystemName, testSystemVersion, Released), localDomain)
        {
            TestClientView = View as View;
        }
    }
}