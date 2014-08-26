using System;

namespace TsdLib.Proxies
{
    /// <summary>
    /// Proxy object that can be used to send events across AppDomain boundaries.
    /// </summary>
    /// <typeparam name="T">Type of EventArgs to assign to the event.</typeparam>
    public class EventProxy<T> : MarshalByRefObject
        where T : EventArgs
    {
        /// <summary>
        /// Exposes the event to attach handlers.
        /// </summary>
        public event EventHandler<T> Event;
        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="eventAgrs">EventArgs object to attach to the event.</param>
        public void FireEvent(T eventAgrs)
        {
            Event(this, eventAgrs);
        }
    }
}