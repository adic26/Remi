using System;
using System.Diagnostics;
using TsdLib.Instrument.Dynamic;
using TsdLib.Instrument.Telnet;
using TsdLib.Instrument.Visa;

namespace ConsoleTest
{
    class ConsoleTestProgram
    {
        static void Main(string[] args)
        {
            //Comment out this line to suppress detailed console logging
            //Trace.Listeners.Add(new ConsoleTraceListener());

            //Uncomment one or more of the methods to test functionality
            GetBlackBerryInfo();
            //MeasureImpedanceWithAimAnalyzer();
            //MeasureCurrentWithPowerSupply();

            Console.WriteLine("Press Enter to exit...");
            Console.ReadLine();
        }

        static void GetBlackBerryInfo()
        {
            using (BlackBerryRadio radio = new TelnetFactory().GetInstrument<BlackBerryRadio>())
            {
                Console.WriteLine("Connected to: " + radio.Description);
                Console.WriteLine("Model: " + radio.ModelNumber);
                Console.WriteLine("BSN: " + radio.SerialNumber);
                Console.WriteLine("OS version: " + radio.FirmwareVersion);
                Console.WriteLine("Radio is currently " + (radio.IsRadioEnabled() ? "enabled" : "disabled"));
            }
        }

        static void MeasureImpedanceWithAimAnalyzer()
        {
            using (AIM4170C aim = new VisaFactory().GetInstrument<AIM4170C>("COM11"))
            {
                Console.WriteLine("Connected to: " + aim.Description);
                Console.WriteLine("Model: " + aim.ModelNumber);
                Console.WriteLine("Serial: " + aim.SerialNumber);
                Console.WriteLine("Firmware version: " + aim.FirmwareVersion);

                aim.CloseRelay();
                byte[] nfcMeas = aim.MeasureNFC();
                aim.OpenRelay();

                Console.WriteLine(Environment.NewLine);
                Console.WriteLine("Impedance Measurement data:");
                Console.WriteLine(string.Join(",", nfcMeas));
                Console.WriteLine(Environment.NewLine);
            }
        }

        static void MeasureCurrentWithPowerSupply()
        {
            using (Aglient6632B ps = new VisaFactory().GetInstrument<Aglient6632B>())
            {
                Console.WriteLine("Connected to: " + ps.Description);
                Console.WriteLine("Model: " + ps.ModelNumber);
                Console.WriteLine("Serial: " + ps.SerialNumber);
                Console.WriteLine("Firmware version: " + ps.FirmwareVersion);

                ps.SetVoltage(3.8);
                double current = ps.ReadCurrent();
                Console.WriteLine("Measured " + current + " A");
            }
        }
    }
}
