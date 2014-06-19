using System;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace TsdLib.Instrument
{
    public abstract class ConnectionBase : IDisposable
    {
        protected abstract void Write(string message);
        protected abstract string Query(string message);
        protected abstract bool CheckForError();

        public abstract void Connect();
        public abstract void Disconnect();
        public abstract bool IsConnected { get; }

        public string Address { get; internal set;}

        public T SendCommand<T>(string command, string regex = ".*", params object[] args)
        {
            string fullCommand = string.Format(command, args);

            Debug.WriteLine("Sending command " + fullCommand);

            string response = Query(fullCommand);

            if (CheckForError())
                throw new CommandException("Error sending command: " + fullCommand);

            Debug.WriteLine("Received response: " + response);

            Match match = Regex.Match(response, regex);
            string parsedResponse = match.Success ? match.Value : response;

            Debug.WriteLine("Parsed response: " + parsedResponse + " with RegEx pattern: " + regex);

            T retval = (T)Convert.ChangeType(parsedResponse, typeof(T));

            Debug.WriteLine("Returning: " + retval + " as " + typeof(T));

            return retval;
        }

        public void SendCommand(string command, params object[] args)
        {
            string fullCommand = string.Format(command, args);
            Debug.WriteLine("Trace Info: Sending Command " + fullCommand);
            Write(fullCommand);
            CheckForError();
        }

        /// <summary>
        /// Calls the Disconnect method. Overload to add additional logic.
        /// </summary>
        public virtual void Dispose()
        {
            Disconnect();
        }
    }
}
