using System;
using System.Threading;
using TsdLib.Configuration;
using TsdLib.Controller;
using TsdLib.View;

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
        /// Returns null to ensure that the remote object's lifetime is as long as the hosting AppDomain.
        /// </summary>
        /// <returns>Null, which corresponds to an unlimited lease time.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Exposes the event to attach handlers.
        /// </summary>
        public event EventHandler<T> Event;
        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="sender">The object where the event was raised.</param>
        /// <param name="eventAgrs">EventArgs object to attach to the event.</param>
        public void FireEvent(object sender, T eventAgrs)
        {
            EventHandler<T> handler = Event;
            
            if (handler != null)
                if (_context != null)
                    _context.Post(s => handler(sender, eventAgrs), null);
                else
                    handler(sender, eventAgrs);
        }

        /// <summary>
        /// Initialize a new EventProxy, optionally specifying which thread the event handlers will be executed on.
        /// </summary>
        /// <param name="context">OPTIONAL: A <see cref="System.Threading.SynchronizationContext"/> on which to fire the event.</param>
        public EventProxy(SynchronizationContext context = null)
        {
            _context = context;
        }
    }
}