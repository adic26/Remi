using System;

namespace TsdLib.Instrument.UsbIp
{
    public class UsbIpException : TsdLibException
    {
        public UsbIpException(string message)
            : base(message)
        {
        }

        public UsbIpException(string message, Exception inner)
            : base(message, inner)
        {
        }
    }
}