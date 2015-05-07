namespace TestClient.Instruments
{
    public interface IBlackBerryWlan
    {
        /// <summary>
        ///     Disable the Wlan interface.
        /// </summary>
        void DisableWlan();

        /// <summary>
        ///     Enable the Wlan interface.
        /// </summary>
        void EnableWlan();

        /// <summary>
        ///     Start the Tx packet engine.
        /// </summary>
        void StartTx();

        /// <summary>
        ///     Stop the Tx packet engine.
        /// </summary>
        void StopTx();

        /// <summary>
        ///     Start the Rx packet engine.
        /// </summary>
        void StartRx();

        /// <summary>
        ///     Stop the Rx packet engine.
        /// </summary>
        void StopRx();

        /// <summary>
        ///     Enable the Wlan adapter.
        /// </summary>
        void EnableDriver();

        /// <summary>
        ///     Disable the Wlan adapter.
        /// </summary>
        void DisableDriver();

        /// <summary>
        ///     Disable the minimum power control.
        /// </summary>
        void DisableMinimumPowerControl();

        /// <summary>
        ///     Disable the watchdog timer.
        /// </summary>
        void DisableWatchdog();

        void SetCountryCode(string countryCode);
        void SetBand(string band);
        void SetRate(int band, string rateType, double rate, int bandwidth);

        /// <summary>
        ///     Set the frequency, based on the band and channel specified.
        /// </summary>
        /// <param name="channel">Channel index to set, eg. 1, 6 or 11.</param>
        /// <param name="bandwidth">Channel bandwidth in MHz.</param>
        void SetChannel(int channel, int bandwidth);

        void EnableForceCal();
        void EnableScanSuppress();
        void SetPowerControlMode(int mode);
        void SetMimoTxBw(int bandwidth);
        void SetTxPowerDefault();
        void SetTxPower(double powerLevel);
        void SetTxPowerOverrideLimits(double powerLevel);
        void SetTxPowerIndex(int powerIndex);

        /// <summary>
        ///     Reset the received frame counter.
        /// </summary>
        void ResetCounter();

        /// <summary>
        ///     Gets the family of the Wlan chipset.
        /// </summary>
        /// <returns>An identifier describing the family of the Wlan chipset.</returns>
        string GetChipsetFamily();

        /// <summary>
        ///     Gets the type of firmware loaded onto the Wlan chipset.
        /// </summary>
        /// <returns>A string that should contain WLTEST if the Wlan driver can be controlled.</returns>
        string GetChipsetFirmwareType();

        /// <summary>
        ///     Gets the firmware version loaded onto the Wlan chipset.
        /// </summary>
        /// <returns>The firmware version in Major.Minor.Build.Revision format.</returns>
        string GetChipsetFirmwareVersion();

        /// <summary>
        ///     Gets the current state of the driver.
        /// </summary>
        /// <returns>True if the driver is enabled; false otherwise.</returns>
        bool IsDriverEnabled();

        /// <summary>
        ///     Gets the currently selected country.
        /// </summary>
        /// <returns>A string describing the country code and country description.</returns>
        string GetCountry();

        bool GetActivecal();

        /// <summary>
        ///     Get the number of received frames.
        /// </summary>
        /// <returns>Number of frames.</returns>
        int GetRxFrameCount();
    }
}