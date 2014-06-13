using System;

namespace TsdLib.Instrument
{
    public abstract class InstrumentBase<TConnection>
    {
        internal protected TConnection Connection;
    }

    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdentifierAttribute : Attribute
    {
        public readonly string Identifier;

        public IdentifierAttribute(string identifier)
        {
            Identifier = identifier;
        }
    }
}
