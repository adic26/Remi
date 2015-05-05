using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using TsdLib.Instrument.Dummy;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Common base class for all instrument implementations.
    /// </summary>
    /// <typeparam name="TConnection">Type of connection to use for communication with the instrument.</typeparam>
    public abstract class InstrumentBase<TConnection> : IInstrument
        where TConnection : ConnectionBase
    {
        /// <summary>
        /// A connection object to used to communicate with the instrument.
        /// </summary>
        public ConnectionBase Connection { get; protected set; }

        /// <summary>
        /// Gets a description of the instrument, including connection information.
        /// </summary>
        public string Description { get; protected set; }

        /// <summary>
        /// Initialize a new instrument object with the specified connection.
        /// </summary>
        /// <param name="connection">A connection object to communicate with the instrument.</param>
        protected InstrumentBase(TConnection connection)
        {
            Connection = connection;
            Description = GetType().Name + " via " + connection.Description;
        }

        /// <summary>
        /// Initialize a new instrument object with the specified connection.
        /// </summary>
        /// <param name="connection">A connection object to communicate with the instrument.</param>
        protected InstrumentBase(DummyConnection connection)
        {
            Connection = connection;
            Description = GetType().Name + " via " + connection.Description;
        }

        /// <summary>
        /// Gets the initialization commands that are sent to the instrument as part of the connection process.
        /// </summary>
        protected virtual string InitCommands { get { return ""; } }

        /// <summary>
        /// Override to implement the query used to obtain the instrument model number.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetModelNumber();
        private string _modelNumber;
        /// <summary>
        /// Gets the instrument model number.
        /// </summary>
        public string ModelNumber
        {
            get { return _modelNumber ?? (_modelNumber = GetModelNumber());} 
        }

        /// <summary>
        /// Gets the descriptor used for the <see cref="IInstrument.ModelNumber"/> property.
        /// </summary>
        public virtual string ModelNumberDescriptor
        {
            get { return "Model Number"; }
        }

        /// <summary>
        /// Override to implement the query used to obtain the instrument serial number.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetSerialNumber();
        private string _serialNumber;
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        public string SerialNumber
        {
            get { return _serialNumber ?? (_serialNumber = GetSerialNumber()); }
        }

        /// <summary>
        /// Gets the descriptor used for the <see cref="IInstrument.SerialNumber"/> property.
        /// </summary>
        public virtual string SerialNumberDescriptor
        {
            get { return "Serial Number"; }
        }

        /// <summary>
        /// Override to implement the query used to obtain the instrument serial number.
        /// </summary>
        /// <returns></returns>
        protected abstract string GetFirmwareVersion();
        private string _firmwareVersion;
        /// <summary>
        /// Gets the instrument serial number.
        /// </summary>
        public string FirmwareVersion
        {
            get { return _firmwareVersion ?? (_firmwareVersion = GetFirmwareVersion()); }
        }

        /// <summary>
        /// Gets the descriptor used for the <see cref="IInstrument.FirmwareVersion"/> property.
        /// </summary>
        public virtual string FirmwareVersionDescriptor
        {
            get { return "Firmware Version"; }
        }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Closes the instrument connection and disposes of any resources being used.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Trace.WriteLine("Disposing " + Description);

                IEnumerable<MethodInfo> disposeMethods = GetType()
                    .GetMethods()
                    .Where(m => m.GetCustomAttributes().OfType<InitCommandAttribute>().Any());

                foreach (MethodInfo disposeMethod in disposeMethods)
                    disposeMethod.Invoke(this, new object[0]);

                Connection.Dispose();
            }
        }
    }
}
