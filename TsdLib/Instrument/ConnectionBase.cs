using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Contains base functionality to communicate with instruments.
    /// </summary>
    public abstract class ConnectionBase : IDisposable
    {
        private readonly object _locker;
        /// <summary>
        /// Gets or sets the default delay (in ms) to wait before sending each command.
        /// </summary>
        public int DefaultDelay { get; set; }

        /// <summary>
        /// Unique address of the instrument.
        /// </summary>
        public string Address { get; private set;}
        /// <summary>
        /// Description of the connection.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initialize a new Connection object.
        /// </summary>
        /// <param name="address">Unique address of the instrument.</param>
        /// <param name="defaultDelay">Default delay (in ms) to wait before sending each command.</param>
        protected ConnectionBase(string address, int defaultDelay = 0)
        {
            _locker = new object();
            Address = address;
            DefaultDelay = defaultDelay;
            Description = GetType().Name + " on " + Address;
        }

        /// <summary>
        /// Write a string to the instrument.
        /// </summary>
        /// <param name="message">String to write.</param>
        protected abstract void Write(string message);

        /// <summary>
        /// Read a string from the instrument.
        /// </summary>
        /// <returns>A string from the instrument.</returns>
        protected abstract string ReadString();
        /// <summary>
        /// Read one byte from the connection.
        /// </summary>
        /// <returns>One byte.</returns>
        protected abstract byte ReadByte();
        /// <summary>
        /// Checks if there is an error with the current connection or from the last command/response.
        /// </summary>
        /// <returns>True in case of error; False otherwise.</returns>
        protected abstract bool CheckForError();

        /// <summary>
        /// Returns true if the instrument is connected and ready to communicate.
        /// </summary>
        public abstract bool IsConnected { get; }

        /// <summary>
        /// Close the connection and dispose of any other resources being used.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        /// <summary>
        /// Close the connection and dispose of any other resources being used.
        /// </summary>
        /// <param name="disposing">True to dispose of unmanaged resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (disposing)
            {
                Address = "n/a - disconnected";
                Description = "n/a - disconnected";
            }
        }

        /// <summary>
        /// Sends a command to the connected instrument.
        /// </summary>
        /// <param name="command">Raw command to send. Use {0}, {1}, etc. to insert members of the args array.</param>
        /// <param name="delay">Time in milliseconds to delay before sending the command. Pass -1 to use the default delay for the connection.</param>
        /// <param name="args">Arguments to be inserted into the raw command string.</param>
        public void SendCommand(string command, int delay, params object[] args)
        {
            int localDelay = delay != -1 ? delay : DefaultDelay;
            if (localDelay > 0)
                Debug.WriteLine("Waiting " + localDelay + "ms before sending");
            Thread.Sleep(localDelay);
                
            lock (_locker)
            {
                string fullCommand = string.Format(command, args);
                Debug.WriteLine("Sending command to " + Description + ": " + fullCommand);
                Write(fullCommand);
                if (CheckForError())
                    throw new CommandException("Error sending command to " + Description + ": " + fullCommand);
            }
        }

        /// <summary>
        /// Reads a string from the connected instrument and parses it into a specified type.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="regex">Optional: Specify a regular expression string to filter the raw response.</param>
        /// <param name="terminationCharacter">Optional: Specify a character that singals the end of the expected response from the connected instrument.</param>
        /// <param name="delay">Optional: Overrides the default connection delay (in milliseconds) for this read.</param>
        /// <returns>The response filtered by the regular expression specified by regex and parsed into the return type specified by T.</returns>
        public T GetResponse<T>(string regex = ".*", char terminationCharacter = '\uD800', int delay = -1)
        {
            int localDelay = delay != -1 ? delay : DefaultDelay;
            if (localDelay > 0)
                Debug.WriteLine("Waiting " + localDelay + "ms before receiving string response");
            Thread.Sleep(localDelay);

            lock (_locker)
            {
                string response = terminationCharacter == '\uD800' ?
                    ReadString() :
                    Encoding.ASCII.GetString(GetByteResponse(terminationCharacter:terminationCharacter, delay:0));

                if (CheckForError())
                    throw new CommandException("Error receiving data from: " + Description);

                Debug.WriteLine("Received response from: " + Description + ": " + response);

                Match match = Regex.Match(response, regex, RegexOptions.Singleline);
                string parsedResponse = match.Success ? match.Value : response;

                if (parsedResponse != response)
                    Debug.WriteLine("RegEx pattern: " + regex + "  Parsed response: " + parsedResponse);

                T retval = (T)Convert.ChangeType(parsedResponse, typeof(T));

                if (typeof(T) != typeof(string))
                    Debug.WriteLine("Returning as: " + typeof(T).Name);

                return retval;
            }
        }

        /// <summary>
        /// Read an array of bytes from the instrument.
        /// </summary>
        /// <param name="byteCount">Number of bytes to read.</param>
        /// <param name="terminationCharacter">OPTIONAL: Termination character that instrument uses to signal the end of the transmission.</param>
        /// <param name="delay">Delay (in ms) to wait before reading the bytes.</param>
        /// <returns>A byte array from the instrument.</returns>
        public byte[] GetByteResponse(int byteCount = int.MaxValue, char terminationCharacter = '\uD800', int delay = -1)
        {
            int localDelay = delay != -1 ? delay : DefaultDelay;
            if (localDelay > 0)
                Debug.WriteLine("Waiting " + localDelay + "ms before receiving byte response");
            Thread.Sleep(localDelay);

            lock (_locker)
            {
                List<byte> resp = new List<byte>();
                while (resp.LastOrDefault() != terminationCharacter && resp.Count < byteCount)
                    resp.Add(ReadByte());
                byte[] response = resp.ToArray();

                if (CheckForError())
                    throw new CommandException("Error receiving data from: " + Description);

                Debug.WriteLine("Received response from " + Description + ": " + Encoding.ASCII.GetString(response));

                return response;
            }
        }
    }
}
