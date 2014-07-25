using System;

namespace TsdLib.Config
{
    public class ConfigException : TsdLibException
    {
        public ConfigException(string message)
            : base(message) { }

        public ConfigException(string message, Exception inner)
            : base(message, inner) { }
    }
}