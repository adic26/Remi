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
        internal CancellationToken Token = new CancellationToken();

        /// <summary>
        /// Synchronization object used to lock the connection for thread-safety.
        /// </summary>
        public readonly object SyncRoot;

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
        protected ConnectionBase(string address)
        {
            SyncRoot = new object();
            Address = address;
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
        /// <param name="errorString">A description of the error.</param>
        /// <returns>True in case of error; False otherwise.</returns>
        protected abstract bool CheckForError(out string errorString);
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
        /// <param name="delay">Time in milliseconds to wait after sending the command.</param>
        /// <param name="ignoreErrors">Pass true to ignore any errors resulting from this command.</param>
        /// <param name="args">Arguments to be inserted into the raw command string.</param>
        /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
        public void SendCommand(string command, int delay, bool ignoreErrors, params object[] args)
        {
            Token.ThrowIfCancellationRequested();
            lock (SyncRoot)
            {
                string fullCommand = string.Format(command, args);

                string[] split = CommandSeparators != null ? fullCommand.Split(CommandSeparators, StringSplitOptions.RemoveEmptyEntries) : new []{fullCommand};

                foreach (string partialCommand in split)
                {
                    Trace.WriteLine("Sending command to " + Description + ": " + partialCommand);
                    Write(partialCommand);
                    string error;
                    if (CheckForError(out error) && !ignoreErrors)
                        throw new CommandException(this, partialCommand, error);
                }
            }
            Thread.Sleep(delay);
        }

        /// <summary>
        /// Reads a string from the connected instrument and parses it into a specified type.
        /// </summary>
        /// <typeparam name="T">Return type.</typeparam>
        /// <param name="regex">Specify a regular expression string to filter the raw response.</param>
        /// <param name="ignoreErrors">Pass true to ignore any errors resulting from this command.</param>
        /// <param name="terminationCharacter">Optional: Specify a character that singals the end of the expected response from the connected instrument.</param>
        /// <returns>The response filtered by the regular expression specified by regex and parsed into the return type specified by T.</returns>
        /// <exception cref="OperationCanceledException">The token has had cancellation requested.</exception>
        public T GetResponse<T>(string regex, bool ignoreErrors, char terminationCharacter = '\uD800')
        {
            Token.ThrowIfCancellationRequested();
            string parsedResponse = "";

            lock (SyncRoot)
            {
                try
                {
                    string response = terminationCharacter == '\uD800' ?
                        ReadString() :
                        Encoding.ASCII.GetString(GetByteResponse(ignoreErrors, terminationCharacter: terminationCharacter));

                    string error;
                    if (CheckForError(out error) && !ignoreErrors)
                        throw new ResponseException(this, error);

                    Trace.WriteLine("Received response from: " + Description + ": " + response);

                    Match match = Regex.Match(response, regex, RegexOptions.Singleline);
                    if (!match.Success)
                    {
                        Trace.WriteLine("RegEx match was unsuccessful. Returning full response.");
                        parsedResponse = response;
                    }
                    else
                        parsedResponse = match.Value;

                    if (parsedResponse != response)
                        Trace.WriteLine("RegEx pattern: " + regex + "  Parsed response: " + parsedResponse);

                    if (typeof(T) != typeof(string))
                        Trace.WriteLine("Returning as: " + typeof(T).Name);

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
        /// <param name="ignoreErrors">Pass true to ignore any errors resulting from this command.</param>
        /// <param name="byteCount">Number of bytes to read.</param>
        /// <param name="terminationCharacter">OPTIONAL: Termination character that instrument uses to signal the end of the transmission.</param>
        /// <returns>A byte array from the instrument.</returns>
        public byte[] GetByteResponse(bool ignoreErrors, int byteCount = int.MaxValue, char terminationCharacter = '\uD800')
        {
            lock (SyncRoot)
            {
                List<byte> resp = new List<byte>();
                while (resp.LastOrDefault() != terminationCharacter && resp.Count < byteCount)
                    resp.Add(ReadByte());
                byte[] response = resp.ToArray();

                string error;
                if (CheckForError(out error) && !ignoreErrors)
                    throw new ResponseException(this, error);

                Trace.WriteLine("Received response from " + Description + ": " + Encoding.ASCII.GetString(response));

                return response;
            }
        }
    }
}
