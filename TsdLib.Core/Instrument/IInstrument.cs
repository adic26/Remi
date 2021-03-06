﻿using System;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Common interface to expose model/serial numbers and firmware version get accessors.
    /// </summary>
    public interface IInstrument : IDisposable
    {
        /// <summary>
        /// Gets a description of the instrument, including connection information.
        /// </summary>
        string Description { get; }
        /// <summary>
        /// Gets the instrument model number.
        /// </summary>
        string ModelNumber { get; }
        /// <summary>
        /// Gets the descriptor used for the <see cref="ModelNumber"/> property.
        /// </summary>
        string ModelNumberDescriptor { get; }
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        string SerialNumber { get; }
        /// <summary>
        /// Gets the descriptor used for the <see cref="SerialNumber"/> property.
        /// </summary>
        string SerialNumberDescriptor { get; }
        /// <summary>
        /// Gets the instrument firmware version.
        /// </summary>
        string FirmwareVersion { get; }
        /// <summary>
        /// Gets the descriptor used for the <see cref="FirmwareVersion"/> property.
        /// </summary>
        string FirmwareVersionDescriptor { get; }
    }
}
