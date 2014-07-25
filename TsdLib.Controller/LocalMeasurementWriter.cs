using System;
using System.IO;

namespace TsdLib.Controller
{
    class LocalMeasurementWriter
    {
        private readonly string _fileName = Path.Combine(SpecialFolders.Measurements, "Results.csv");
        public string FileName { get { return _fileName; }}

        public LocalMeasurementWriter()
        {
            File.WriteAllText(_fileName, string.Join(",", "Measurement Name", "Measured Value", "Units", "Lower Limit", "Upper Limit", "Result"));
        }

        public void Write(Measurement measurement)
        {
            File.AppendAllText(_fileName, measurement + Environment.NewLine);
            
        }
    }
}
