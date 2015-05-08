using TsdLib.Instrument;

namespace TestClient.Instruments
{
    public interface IBlackBerry
    {
        /// <summary>
        /// Gets the family of the Wlan chipset.
        /// </summary>
        /// <returns>An identifier describing the family of the Wlan chipset.</returns>
        string GetChipsetFamily();

        ConnectionBase Connection { get; }
    }
}
