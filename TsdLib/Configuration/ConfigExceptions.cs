using System;

namespace TsdLib.Configuration
{
    [Serializable]
    public class ConfigException : TsdLibException
    {
        public ConfigException(string message)
            : base(message) { }

        public ConfigException(string message, Exception inner)
            : base(message, inner) { }
    }
}