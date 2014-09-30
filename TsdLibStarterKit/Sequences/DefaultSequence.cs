
using System;
using System.Globalization;
using System.Threading;
using $safeprojectname$.Configuration;
using $safeprojectname$.Instruments;
using TsdLib.TestSequence;

[assembly: AssemblyReference("System.dll")]
[assembly: AssemblyReference("System.Xml.dll")]
[assembly: AssemblyReference("TsdLib.dll")]
[assembly: AssemblyReference("$safeprojectname$.exe")]
[assembly: AssemblyReference("$safeprojectname$.Instruments.dll")]

namespace $safeprojectname$.Sequences
{
    public class DefaultSequence : TestSequenceBase<StationConfig, ProductConfig, TestConfig>
    {
        protected override void Execute(StationConfig stationConfig, ProductConfig productConfig, TestConfig testConfig)
        {
            //TODO: Create test sequence. This is the step-by-step sequence of instrument and/or DUT commands and measurements

            //Use the System.Diagnostics.Debugger.Break() method to insert breakpoints.
            //Debugger.Break();

            Random random = new Random();
            DummyPowerSupply ps = DummyPowerSupply.Connect();

            for (int i = 0; i < testConfig.LoopIterations; i++)
            {
                foreach (double voltageSetting in testConfig.VoltageSettings)
                {
                    Token.ThrowIfCancellationRequested();
                    ps.SetVoltage(voltageSetting);
                    Thread.Sleep(productConfig.SettlingTime);
                    ps.DummyConnection.StringToRead = random.NextDouble().ToString(CultureInfo.InvariantCulture);
                    TestResults.AddMeasurement("Current", ps.GetCurrent(), "Amps", 0.1, 0.8, new MeasurementParameter("Voltage", voltageSetting));
                }
            }
        }
        
        public override string TestSystemName
        {
            get { return "$safeprojectname$"; }
        }
    }
}
