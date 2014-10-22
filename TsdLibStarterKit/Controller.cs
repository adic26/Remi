using System.Windows.Forms;
using TsdLib.Configuration;
using TsdLib.Controller;
using $safeprojectname$.Configuration;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
#if DEBUG
        private const bool Released = false;
#else
        private const bool Released = true;
#endif

        public View $safeprojectname$View;

        public Controller(bool devMode)
            : base(devMode, new DatabaseFolderConnection(@"C:\temp\TsdLibSettings", "$safeprojectname$", Application.ProductVersion, Released))
        {
            $safeprojectname$View = base.View as View;
        }
    }
}