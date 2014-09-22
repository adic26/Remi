using TsdLib.Controller;
using $safeprojectname$.Configuration;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(bool devMode)
            : base(devMode, "$safeprojectname$") { }
    }
}