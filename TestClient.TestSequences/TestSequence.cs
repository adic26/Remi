﻿
using System;
using TestClient.Configuration;
using TsdLib.TestSequence;

namespace TestClient.TestSequences
{
    public class TestSequence : TestSequenceBase<StationConfig, ProductConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements
            throw new NotImplementedException();
        }
    }
}
