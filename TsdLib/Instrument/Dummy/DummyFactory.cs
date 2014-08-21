using System.Collections.Generic;

namespace TsdLib.Instrument.Dummy
{
    /// <summary>
    /// Contains functionality to simulate the instrument discovery process and connect to simulated instruments.
    /// </summary>
    public class DummyFactory : FactoryBase<DummyConnection>
    {
        /// <summary>
        /// Simulates a search the system for instruments of the specified connection type.
        /// </summary>
        /// <returns>A single instrument address of "Dummy_Device_Address".</returns>
        protected override IEnumerable<string> SearchForInstruments()
        {
            return new[] { "Dummy_Device_Address" };
        }

        /// <summary>
        /// Creates a simulated connection using the specified address.
        /// </summary>
        /// <param name="address">Address to assign to the simulated instrument.</param>
        /// <param name="defaultDelay">Default delay to wait between commands.</param>
        /// <param name="attributes">Zero or more ConnectionSettingAttributes. Content will be defined by the instrument connection type.</param>
        /// <returns>A simulated instrument connection.</returns>
        protected override DummyConnection CreateConnection(string address, int defaultDelay,
            params ConnectionSettingAttribute[] attributes)
        {
            return new DummyConnection(address);
        }

        /// <summary>
        /// Simulates sending a request to identify the instrument via the specified connection.
        /// </summary>
        /// <param name="connection">Has no actual effect. DummyConnection object representing the connection to the instrument.</param>
        /// <param name="idAttribute">Has no actual effect. IdQueryAttribute object representing the command to send to the instrument and a termination character (if required) to signal the end of the instrument response.</param>
        /// <returns>An instrument identifier of "Dummy_Device".</returns>
        protected override string GetInstrumentIdentifier(DummyConnection connection, IdQueryAttribute idAttribute)
        {
            return "Dummy_Device";
        }
    }
}