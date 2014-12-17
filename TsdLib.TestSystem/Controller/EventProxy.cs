using System;
using System.Collections.Generic;
using System.Threading;

namespace TsdLib.TestSystem.Controller
{
    /// <summary>
    /// Proxy object that can be used to send events across thread and AppDomain boundaries.
    /// </summary>
    /// <typeparam name="T">Type of EventArgs to assign to the event.</typeparam>
    public class EventProxy<T> : MarshalByRefObject
    {
        private readonly Dictionary<EventHandler<T>, SynchronizationContext> _handlers;

        /// <summary>
        /// Returns null to ensure that the remote object's lifetime is as long as the hosting AppDomain.
        /// </summary>
        /// <returns>Null, which corresponds to an unlimited lease time.</returns>
        public override object InitializeLifetimeService()
        {
            return null;
        }

        /// <summary>
        /// Attach the specified handler to an event.
        /// </summary>
        /// <param name="handler">EventHandler delegate to be called when the event is fired.</param>
        /// <param name="context">OPTIONAL: A <see cref="System.Threading.SynchronizationContext"/> on which to fire the event.</param>
        public void Attach(EventHandler<T> handler, SynchronizationContext context = null)
        {
            _handlers.Add(handler, context);
        }

        /// <summary>
        /// Fires the event.
        /// </summary>
        /// <param name="sender">The object where the event was raised.</param>
        /// <param name="eventAgrs">EventArgs object to attach to the event.</param>
        public void FireEvent(object sender, T eventAgrs)
        {
            foreach (var handlerPair in _handlers)
            {
                EventHandler<T> handler = handlerPair.Key;

                if (handler != null)
                    if (handlerPair.Value != null)
                        handlerPair.Value.Post(s => handler(sender, eventAgrs), null);
                    else
                        handler(sender, eventAgrs);
            }
        }

        /// <summary>
        /// Initialize a new EventProxy.
        /// </summary>
        public EventProxy()
        {
            _handlers = new Dictionary<EventHandler<T>, SynchronizationContext>();
        }
    }
}