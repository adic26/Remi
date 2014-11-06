using System.CodeDom.Compiler;
using $safeprojectname$.Configuration;
using TsdLib.Configuration;
using TsdLib.Controller;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
#if DEBUG
        private const bool Released = false;
#else
        private const bool Released = true;
#endif

        public Controller(bool devMode, string testSystemName, string testSystemVersion, bool localDomain, ICodeParser instrumentParser)
            : base(devMode, testSystemName, testSystemVersion, new DatabaseFolderConnection(@"C:\temp\TsdLibSettings", testSystemName, testSystemVersion, Released), localDomain, instrumentParser)
        {

        }
    }
}