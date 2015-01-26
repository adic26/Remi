using System;
using System.Diagnostics;

namespace TsdLib.Instrument.Visa
{
    public class VisaDriverNotInstalledException : TsdLibException
    {
        public VisaDriverNotInstalledException(Exception inner = null)
            : base("The National Instruments VISA driver or required assemblies are missing from this PC. Please install the drivers and make sure to select .NET Framework 4.5 Language Support in the installer.", inner)
        {
            try
            {
                Process.Start("NIVISA1401full_downloader.exe");
            }
            catch (Exception ex)
            {
                Trace.WriteLine(ex);
            }
        }
    }
}
