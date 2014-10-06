﻿using System.Windows.Forms;
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

        public Controller(bool devMode)
            : base(devMode, new DatabaseFolderConnection(@"C:\temp\RemiSettingsTest", "$safeprojectname$", Application.ProductVersion, Released)) { }
    }
}