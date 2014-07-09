﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;

namespace TsdLib.Instrument
{
    public abstract class ConnectionBase : IDisposable
    {
        private readonly object _locker;
        public int DefaultDelay { get; set; }

        public string Address { get; private set;}
        public string Description { get; private set; }

        protected ConnectionBase(string address, int defaultDelay = 0)
        {
            _locker = new object();
            Address = address;
            DefaultDelay = defaultDelay;
            Description = GetType().Name + " on " + Address;
        }

        protected abstract void Write(string message);
        protected abstract string ReadString();
        protected abstract byte ReadByte();
        protected abstract bool CheckForError();
        
        public abstract bool IsConnected { get; }
        public void Dispose()
        {
            Dispose(true);
        }

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
        /// <param name="delay">Time in milliseconds to delay before sending the command.</param>
        /// <param name="args">Arguments to be inserted into the raw command string.</param>
        public void SendCommand(string command, int delay = -1, params object[] args)
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

        ///// <summary>
        ///// Sends a command, reads the response string and parses it into type of T.
        ///// </summary>
        ///// <typeparam name="T">Return type.</typeparam>
        ///// <param name="command">Raw command to send. Use {0}, {1}, etc. to insert members of the args array.</param>
        ///// <param name="regex">Regular expression string to filter the raw response. Use ".*" to use the full response.</param>
        ///// <param name="delay">Time in milliseconds to delay before sending the command and before reading the response.</param>
        ///// <param name="terminationCharacter">A character that signals the end of the response from the instrument.</param>
        ///// <param name="args">Arguments to be inserted into the raw command string.</param>
        ///// <returns>The response filtered by the regular expression specified regex and parsed into the return type specified by T.</returns>
        //public T SendCommand<T>(string command, string regex, int delay, char terminationCharacter, params object[] args)
        //{
        //    lock (_locker)
        //    {
        //        SendCommand(command, delay, args);

        //        if (delay > 0)
        //            Debug.WriteLine("Waiting " + delay + "ms");
        //        Thread.Sleep(delay);

        //        string response = ReadString();

        //        if (CheckForError())
        //            throw new CommandException("Error receiving data from: " + Description);

        //        Debug.WriteLine("Received response from: " + Description + ": " + response);

        //        Match match = Regex.Match(response, regex, RegexOptions.Singleline);
        //        string parsedResponse = match.Success ? match.Value : response;

        //        if (parsedResponse != response)
        //            Debug.WriteLine("RegEx pattern: " + regex + "  Parsed response: " + parsedResponse);

        //        T retval = (T)Convert.ChangeType(parsedResponse, typeof(T));

        //        if (typeof(T) != typeof(string))
        //            Debug.WriteLine("Returning as: " + typeof(T).Name);

        //        return retval;
        //    }
        //}

        ///// <summary>
        ///// Sends a command, reads the response string and parses it into type of T.
        ///// </summary>
        ///// <typeparam name="T">Return type.</typeparam>
        ///// <param name="command">Raw command to send. Use {0}, {1}, etc. to insert members of the args array.</param>
        ///// <param name="regex">Regular expression string to filter the raw response. Use ".*" to use the full response.</param>
        ///// <param name="delay">Time in milliseconds to delay before sending the command and before reading the response.</param>
        ///// <param name="args">Arguments to be inserted into the raw command string.</param>
        ///// <returns>The response filtered by the regular expression specified regex and parsed into the return type specified by T.</returns>
        //public T SendCommand<T>(string command, string regex, int delay, params object[] args)
        //{
        //    var resp = SendCommand<T>(command, regex, delay, '\uD800', args);

        //    lock (_locker)
        //    {
        //        SendCommand(command, delay, args);

        //        if (delay > 0)
        //            Debug.WriteLine("Waiting " + delay + "ms");
        //        Thread.Sleep(delay);

        //        string response = ReadString();

        //        if (CheckForError())
        //            throw new CommandException("Error receiving data from: " + Description);

        //        Debug.WriteLine("Received response from: " + Description + ": " + response);

        //        Match match = Regex.Match(response, regex, RegexOptions.Singleline);
        //        string parsedResponse = match.Success ? match.Value : response;

        //        if (parsedResponse != response)
        //            Debug.WriteLine("RegEx pattern: " + regex + "  Parsed response: " + parsedResponse);

        //        T retval = (T)Convert.ChangeType(parsedResponse, typeof(T));

        //        if (typeof(T) != typeof(string))
        //            Debug.WriteLine("Returning as: " + typeof(T).Name);

        //        return retval;
        //    }
        //}

        //public byte[] SendCommand(string command, int byteCount, int delay, params  object[] args)
        //{
        //    return SendCommand(command, '\uD800', byteCount, delay);
        //}

        //public byte[] SendCommand(string command, char terminationCharacter, int delay, params object[] args)
        //{
        //    return SendCommand(command, terminationCharacter, Int32.MaxValue, delay);
        //}

        //public byte[] SendCommand(string command, char terminationCharacter, int byteCount, int delay, params object[] args)
        //{
        //    lock (_locker)
        //    {
        //        SendCommand(command, delay, args);

        //        List<byte> response = new List<byte>();

        //        while (response.LastOrDefault() != terminationCharacter && response.Count < byteCount)
        //            response.Add(ReadByte());

        //        if (CheckForError())
        //            throw new CommandException("Error receiving data from: " + Description);

        //        Debug.WriteLine("Received response from " + Description + ": " + Encoding.ASCII.GetString(response.ToArray()));

        //        return response.ToArray();
        //    }
        //}
    }
}
