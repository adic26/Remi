namespace TsdLib.Configuration
{
    /// <summary>
    /// Describes the various modes that the test system can operate under. Defines read-write access to configuration and result storage options.
    /// </summary>
    public enum OperatingMode
    {
        /// <summary>
        /// All configuration is read-write. Results can only be stored under the Analysis stage.
        /// </summary>
        Developer,
        /// <summary>
        /// Built-in configuration is read-only, but new confgiurations can be created and modified. Results can only be stored under the Analysis stage.
        /// </summary>
        Engineering,
        /// <summary>
        /// All configuration is read-only. Results can be stored to any test stage.
        /// </summary>
        Production
    }

    /// <summary>
    /// Type of OS image loaded on the DUT.
    /// </summary>
    public enum FunctionalType
    {
        /// <summary>
        /// Do not include the OS image information.
        /// </summary>
        None = 0,
        /// <summary>
        /// Software Flash Image.
        /// </summary>
        SFI = 1,
        /// <summary>
        /// Manufacturing Flash Image.
        /// </summary>
        MFI = 2,
        /// <summary>
        /// No OS image, in case of accessory.
        /// </summary>
        Accessory = 3
    }
}