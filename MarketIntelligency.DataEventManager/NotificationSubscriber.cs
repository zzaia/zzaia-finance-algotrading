using System;

namespace MarketIntelligency.DataEventManager
{
    public class NotificationSubscriber<T> : IObserver<T>
    {
        private IDisposable _unsubscriber; 
        public virtual void Subscribe(IObservable<T> provider)
        {
            // Subscribe to the Observable
            if (provider != null)
                _unsubscriber = provider.Subscribe(this);
        }
        public virtual void OnCompleted()
        {
            Console.WriteLine("Done");
        }
        public virtual void OnError(Exception e)
        {
            Console.WriteLine($"Error: {e.Message}");
        }
        public virtual void OnNext(T ev)
        {
        }
        public virtual void Unsubscribe()
        {
            _unsubscriber.Dispose();
        }
    }
}
