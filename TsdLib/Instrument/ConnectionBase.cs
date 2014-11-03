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
        /// <summary>
        /// Synchronization object used to lock the connection for thread-safety.
        /// </summary>
        public readonly object SyncRoot;
        /// <summary>
        /// Gets or sets the default delay (in ms) to wait before sending each command.
        /// </summary>
        public int DefaultDelay { get; set; }

        /// <summary>
        /// Unique address of the instrument.
        /// </summary>
        public string Address { get; private set;}
        /// <summary>
        /// Gets a description of the connection.
        /// </summary>
        public string Description { get; private set; }

        /// <summary>
        /// Initialize a new Connection object.
        /// </summary>
        /// <param name="address">Unique address of the instrument.</param>
        /// <param name="defaultDelay">Default delay (in ms) to wait before sending each command.</param>
        protected ConnectionBase(string address, int defaultDelay = 0)
        {
            SyncRoot = new object();
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
        /// Gets the string used to separate commands sent in a single line.
        /// </summary>
        protected virtual string[] CommandSeparators { get { return null; } }

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
                
            lock (SyncRoot)
            {
                string fullCommand = string.Format(command, args);

                string[] split = CommandSeparators != null ? fullCommand.Split(CommandSeparators, StringSplitOptions.RemoveEmptyEntries) : new []{fullCommand};

                foreach (string partialCommand in split)
                {
                    Debug.WriteLine("Sending command to " + Description + ": " + partialCommand);
                    Write(partialCommand);
                    if (CheckForError())
                        throw new CommandException(this, partialCommand);
                }
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

            string parsedResponse = "";

            lock (SyncRoot)
            {
                try
                {
                    string response = terminationCharacter == '\uD800' ?
                        ReadString() :
                        Encoding.ASCII.GetString(GetByteResponse(terminationCharacter: terminationCharacter, delay: 0));

                    if (CheckForError())
                        throw new ResponseException(this);

                    Debug.WriteLine("Received response from: " + Description + ": " + response);

                    Match match = Regex.Match(response, regex, RegexOptions.Singleline);
                    parsedResponse = match.Success ? match.Value : response;

                    if (parsedResponse != response)
                        Debug.WriteLine("RegEx pattern: " + regex + "  Parsed response: " + parsedResponse);

                    if (typeof(T) != typeof(string))
                        Debug.WriteLine("Returning as: " + typeof(T).Name);

                    if (typeof(T) == typeof(bool))
                    {
                        if (parsedResponse == "0")
                            parsedResponse = "false";
                        if (parsedResponse == "1")
                            parsedResponse = "true";
                    }

                    T retval = (T)Convert.ChangeType(parsedResponse, typeof(T));

                    return retval;
                }
                catch (FormatException ex)
                {
                    throw new ResponseException(this, parsedResponse, ex);
                }
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

            lock (SyncRoot)
            {
                List<byte> resp = new List<byte>();
                while (resp.LastOrDefault() != terminationCharacter && resp.Count < byteCount)
                    resp.Add(ReadByte());
                byte[] response = resp.ToArray();

                if (CheckForError())
                    throw new ResponseException(this);

                Debug.WriteLine("Received response from " + Description + ": " + Encoding.ASCII.GetString(response));

                return response;
            }
        }
    }
}
