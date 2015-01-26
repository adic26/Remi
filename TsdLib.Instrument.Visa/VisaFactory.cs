using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using NationalInstruments.VisaNS;

namespace TsdLib.Instrument.Visa
{
    /// <summary>
    /// Contains functionality to discover and connect to Visa-based instruments.
    /// </summary>
    public class VisaFactory : FactoryBase<VisaConnection>
    {
        /// <summary>
        /// Search the system for Visa-based instrument.
        /// </summary>
        /// <returns>A sequence of instrument addresses.</returns>
        protected override IEnumerable<string> SearchForInstruments()
        {
            try
            {
                return ResourceManager.GetLocalManager().FindResources("?*INSTR");

            }
            catch (FileNotFoundException ex)
            {
                if (ex.FileName.Contains("NationalInstruments.VisaNS"))
                    throw new VisaDriverNotInstalledException(ex);
                throw;
            }
        }

        /// <summary>
        /// Connects to the Visa-based instrument at the specified address.
        /// </summary>
        /// <param name="address">Visa resource name for the instrument (ie. GPIB0::6::INSTR).</param>
        /// <param name="defaultDelay">Default delay to wait between commands.</param>
        /// <param name="attributes">Zero or more ConnectionSettingAttributes with a name matching a member of NationalInstruments.VisaNS.AttributeType.</param>
        /// <returns>A VisaConnection object that can be used to communicate with the instrument.</returns>
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

        /// <summary>
        /// Send a request to identify the type of instrument via the specified VisaConnection.
        /// </summary>
        /// <param name="connection">VisaConnection object representing the connection to the instrument.</param>
        /// <param name="idAttribute">IdQueryAttribute object representing the command (ie. *IDN?) to send to the instrument and a termination character (if required) to signal the end of the instrument response.</param>
        /// <returns>The response from the instrument.</returns>
        protected override string GetInstrumentIdentifier(VisaConnection connection, IdQueryAttribute idAttribute)
        {
            try
            {
                if (!connection.IsConnected)
                    Debug.Fail("Passed a disconnected connection into VisaFactory.GetInstrumentIdentifier!");

                connection.SendCommand(idAttribute.Command, -1);

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
