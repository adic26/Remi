﻿using System;

namespace TsdLib.Instrument
{
    /// <summary>
    /// Custom attribute to define the initialization commands to send to the instrument during the connection process.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = false)]
    public class InitCommandsAttribute : Attribute
    {
        /// <summary>
        /// Gets the commands in an array.
        /// </summary>
        public string[] Commands { get; private set; }

        /// <summary>
        /// Initialize a new InitCommandsAttribute with the specified commands.
        /// </summary>
        /// <param name="commands">Zero or more commands to be sent to the instrument during the connection process.</param>
        public InitCommandsAttribute(params string[] commands)
        {
            Commands = commands;
        }
    }

    /// <summary>
    /// Custom attribute to define the command to query the instrument identification and the expected response.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class IdQueryAttribute : Attribute
    {
        /// <summary>
        /// Gets the identification query response expected to be returned by the instrument.
        /// </summary>
        public string Response { get; private set; }
        /// <summary>
        /// Gets the command to send to the instrument to request identification.
        /// </summary>
        public string Command { get; private set; }
        /// <summary>
        /// Gets the termination character (if any) that the instrument sends to signal the end of the identification query response.
        /// </summary>
        public char TermChar { get; private set; }

        /// <summary>
        /// Initialize a new IdQueryAttribute with the specified reponse, command and termination charater.
        /// </summary>
        /// <param name="response">Identification query response expected to be returned by the instrument.</param>
        /// <param name="command">OPTIONAL: Command to send to the instrument to request identification. Only required for instruments where an identification query is originated from the client.</param>
        /// <param name="termChar">OPTIONAL: Termination character (if any) that the instrument sends to signal the end of the identification query response.</param>
        public IdQueryAttribute(string response, string command = "", string termChar = null)
        {
            Response = response;
            Command = command;
            TermChar = termChar != null ? termChar[0] : '\uD800';
        }
    }

    /// <summary>
    /// Custom attribute to define any connection settings specific to the instrument (ie. serial port baud rate, etc.)
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = true, Inherited = true)]
    public class ConnectionSettingAttribute : Attribute
    {
        /// <summary>
        /// Gets the name of the connection setting.
        /// </summary>
        public string Name { get; private set; }
        /// <summary>
        /// Gets the data type of the connection setting.
        /// </summary>
        public Type ArgumentType { get; private set; }
        /// <summary>
        /// Gets the value of the connection setting.
        /// </summary>
        public object ArgumentValue { get; private set; }

        /// <summary>
        /// Initialize a new ConnectionSettingAttribute with the specified name, data type and value.
        /// </summary>
        /// <param name="name">Name of the connection setting.</param>
        /// <param name="type">Data type of the connection setting. Will be used to cast the value into a strongly-typed object.</param>
        /// <param name="val">Value of the connection setting.</param>
        public ConnectionSettingAttribute(string name, string type, string val)
        {
            Name = name;

            ArgumentType = Type.GetType("System." + type);
            if (ArgumentType == null)
                throw new ConnectionSettingAttributeException(type, name);

            ArgumentValue = Convert.ChangeType(val, ArgumentType);
        }
    }

    /// <summary>
    /// Custom attribute to define a default delay (in ms) to wait before sending each command to the instrument.
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, AllowMultiple = false, Inherited = true)]
    public class CommandDelayAttribute : Attribute
    {
        /// <summary>
        /// Gets the delay (in ms).
        /// </summary>
        public int Delay { get; private set; }

        /// <summary>
        /// Initialize a new CommandDelayAttribute with the specified delay.
        /// </summary>
        /// <param name="delay">Delay (in ms).</param>
        public CommandDelayAttribute(string delay)
        {
            Delay = int.Parse(delay);
        }
    }
    
}