using System;
using TsdLib.Controller;
using TsdLib.TestSequence;
using $rootnamespace$.Configuration;

namespace $safeprojectname$
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(bool devMode)
            : base(devMode) { }
    }
}