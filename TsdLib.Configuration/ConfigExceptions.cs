using System;

namespace TsdLib.Configuration
{
    public class ConfigException : TsdLibException
    {
        public ConfigException(string message)
            : base(message) { }

        public ConfigException(string message, Exception inner)
            : base(message, inner) { }
    }
}