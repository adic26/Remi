using System;
using System.Collections.Generic;

namespace TsdLib.TestSystem.Observer
{
    public class Unsubscriber<T> : MarshalByRefObject, IDisposable
    {
        private readonly HashSet<IObserver<T>> _observers;
        private readonly IObserver<T> _observer;

        internal Unsubscriber(HashSet<IObserver<T>> observers, IObserver<T> observer)
        {
            _observers = observers;
            _observer = observer;
        }

        public void Dispose()
        {
            if (_observers.Contains(_observer))
                _observers.Remove(_observer);
        }

        public override object InitializeLifetimeService()
        {
            return null;
        }
    }
}
