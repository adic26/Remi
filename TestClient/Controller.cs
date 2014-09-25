﻿using System.Windows.Forms;
using TestClient.Configuration;
using TsdLib.Controller;

namespace TestClient
{
    public class Controller : ControllerBase<View, StationConfig, ProductConfig, TestConfig>
    {
        public Controller(bool devMode)
            : base(devMode, "TestClient", Application.ProductVersion, @"C:\temp\RemiSettingsTest") { }
    }
}