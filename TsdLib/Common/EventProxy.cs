using System;
using System.Threading;

namespace TsdLib
{
    /// <summary>
    /// Proxy object that can be used to send events across thread and AppDomain boundaries.
    /// </summary>
    /// <typeparam name="T">Type of EventArgs to assign to the event.</typeparam>
    public class EventProxy<T> : MarshalByRefObject
    {
        private readonly SynchronizationContext _context;

        /// <summary>
        /// Exposes the event to attach handlers.
        /// </summary>
        public event EventHandler<T> Event;
        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="eventAgrs">EventArgs object to attach to the event.</param>
        public void FireEvent(T eventAgrs)
        {//TODO: make thread-safe for UI
            EventHandler<T> handler = Event;
            if (handler != null)
                _context.Post(s => handler(this, eventAgrs), null);
        }

        /// <summary>
        /// Initialize a new EventProxy.
        /// </summary>
        /// <param name="context"><see cref="System.Threading.SynchronizationContext"/> on which to fire the event.</param>
        public EventProxy(SynchronizationContext context)
        {
            _context = context;
        }
    }
}