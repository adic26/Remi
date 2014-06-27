using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using System.Text;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaFactory : FactoryBase<VisaConnection, VisaAttributeAttribute>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            return ResourceManager.GetLocalManager().FindResources("?*INSTR");
        }

        protected override VisaConnection CreateConnection(string address, params VisaAttributeAttribute[] attributes)
        {
            MessageBasedSession session = (MessageBasedSession) ResourceManager.GetLocalManager().Open(address);

            foreach (VisaAttributeAttribute attribute in attributes)
            {
                Assembly niVisa = Assembly.GetAssembly(typeof(AttributeType));
                Type t = niVisa.GetType("NationalInstruments.VisaNS.AttributeType");

                Type argType = Type.GetType("System." + attribute.Type);
                if (argType == null)
                    throw new CommandException(attribute.Type + " is not a valid type for the attribute " + attribute.Name);

                object argVal = Convert.ChangeType(attribute.Value, argType);

                AttributeType typeEnum = (AttributeType)Enum.Parse(t, attribute.Name);

                session.SetAttribute(typeEnum, argVal);
            }
            
            return new VisaConnection(session); 
        }

        protected override string GetInstrumentIdentifier(VisaConnection connection, string idCommand)
        {
            try
            {
                try
                {
                    if (!connection.IsConnected)
                        Debug.Fail("Passed a disconnected connection into GetInstrumentIdentifier!");
                    return connection.SendCommand<string>(idCommand, "");
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (VisaException ex)
            {
                if (ex.ErrorCode == VisaStatusCode.ErrorTimeout || ex.ErrorCode == VisaStatusCode.ErrorSystemError || ex.ErrorCode == VisaStatusCode.ErrorResourceBusy)
                    return ex.ErrorCode.ToString();
                throw;
            }
        }
    }

    static class MbsExtensions
    {
        public static void SetAttribute(this MessageBasedSession session, AttributeType attribute, object value)
        {
            SetVal(session, attribute, (dynamic)value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, bool value)
        {
            session.SetAttributeBoolean(attribute, value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, byte value)
        {
            session.SetAttributeByte(attribute, value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, short value)
        {
            session.SetAttributeInt16(attribute, value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, int value)
        {
            session.SetAttributeInt32(attribute, value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, long value)
        {
            session.SetAttributeInt64(attribute, value);
        }

        static void SetVal(MessageBasedSession session, AttributeType attribute, string value)
        {
            session.SetAttributeString(attribute, value);
        }
    }
}
