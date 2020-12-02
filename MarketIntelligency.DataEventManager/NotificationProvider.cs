using System;
using System.Collections.Generic;

namespace MarketIntelligency.DataEventManager
{
    public class NotificationProvider<T> : IObservable<T>
    {
        private List<IObserver<T>> _observers;
        public NotificationProvider()
        {
            _observers = new List<IObserver<T>>();
        }
        private class Unsubscriber : IDisposable
        {
            private List<IObserver<T>> _observers;
            private IObserver<T> _observer;
            public Unsubscriber(List<IObserver<T>> observers, IObserver<T> observer)
            {
                this._observers = observers;
                this._observer = observer;
            }
            public void Dispose()
            {
                if (!(_observer == null)) _observers.Remove(_observer);
            }
        }
        public IDisposable Subscribe(IObserver<T> observer)
        {
            if (!_observers.Contains(observer))
                _observers.Add(observer);
            return new Unsubscriber(_observers, observer);
        }
        public void NotificationEvent(T data)
        {
            foreach (var observer in _observers)
            {
                observer.OnNext(data);
            }
        }
    }
}
