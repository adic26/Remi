using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Reflection;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    public class VisaFactory : FactoryBase<VisaConnection>
    {
        protected override IEnumerable<string> SearchForInstruments()
        {
            return ResourceManager.GetLocalManager().FindResources("?*INSTR");
        }

        protected override VisaConnection CreateConnection(string address, int defaultDelay, params ConnectionSettingAttribute[] attributes)
        {
            try
            {
                try
                {
                    MessageBasedSession session = (MessageBasedSession)ResourceManager.GetLocalManager().Open(address);

                    foreach (ConnectionSettingAttribute attribute in attributes)
                    {
                        Assembly niVisa = Assembly.GetAssembly(typeof(AttributeType));
                        Type tVisaAttribute = niVisa.GetType("NationalInstruments.VisaNS.AttributeType");

                        AttributeType typeEnum = (AttributeType)Enum.Parse(tVisaAttribute, attribute.Name);

                        session.SetAttribute(typeEnum, attribute.ArgumentValue);
                    }

                    return new VisaConnection(session, defaultDelay); 
                }
                catch (TargetInvocationException ex)
                {
                    throw ex.InnerException;
                }
            }
            catch (VisaException ex)
            {
                if (ex.ErrorCode == VisaStatusCode.ErrorSystemError)
                    return null;
                throw;
            }
        }

        protected override string GetInstrumentIdentifier(VisaConnection connection, IdQueryAttribute idAttribute)
        {
            try
            {
                if (!connection.IsConnected)
                    Debug.Fail("Passed a disconnected connection into VisaFactory.GetInstrumentIdentifier!");

                connection.SendCommand(idAttribute.Command);

                string response = idAttribute.TermChar == '\uD800' ? connection.GetResponse<string>() : connection.GetResponse<string>(terminationCharacter:idAttribute.TermChar);
                
                return response;
            }
            catch (VisaException ex)
            {
                if (ex.ErrorCode == VisaStatusCode.ErrorTimeout || ex.ErrorCode == VisaStatusCode.ErrorResourceBusy)
                    return ex.ErrorCode.ToString();
                throw;
            }
        }
    }

    static class MessageBasedSessionExtensions
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
