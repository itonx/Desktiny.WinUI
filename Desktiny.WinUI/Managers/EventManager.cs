using System;
using System.Collections.Generic;
using System.Linq;

namespace Desktiny.WinUI.Managers
{
    public class EventManager
    {
        private static readonly object _lock = new();
        private static readonly List<Subscription> _subscribers = new();

        private class Subscription
        {
            public WeakReference Subscriber { get; set; }
            public Type EventType { get; set; }
            public Delegate Handler { get; set; }
        }

        public static void Subscribe<TEvent>(object subscriber, Action<TEvent> handler)
        {
            lock (_lock)
            {
                _subscribers.RemoveAll(s => !s.Subscriber.IsAlive);

                bool isAlreadyRegistered = _subscribers.Any(s =>
                    s.Subscriber.Target == subscriber &&
                    s.EventType == typeof(TEvent));

                if (isAlreadyRegistered)
                {
                    return;
                }

                _subscribers.Add(new Subscription
                {
                    Subscriber = new WeakReference(subscriber),
                    EventType = typeof(TEvent),
                    Handler = handler
                });
            }
        }

        public static void Publish<TEvent>(TEvent eventData)
        {
            List<Action<TEvent>> handlersToInvoke = new();

            lock (_lock)
            {
                _subscribers.RemoveAll(s => !s.Subscriber.IsAlive);

                foreach (var sub in _subscribers.Where(s => s.EventType == typeof(TEvent)))
                {
                    if (sub.Handler is Action<TEvent> typedHandler)
                    {
                        handlersToInvoke.Add(typedHandler);
                    }
                }
            }

            foreach (var handler in handlersToInvoke)
            {
                handler?.Invoke(eventData);
            }
        }

        public static void Unsubscribe<TEvent>(object subscriber)
        {
            lock (_lock)
            {
                var subscriptionToRemove = _subscribers.FirstOrDefault(s =>
                    s.Subscriber.Target == subscriber &&
                    s.EventType == typeof(TEvent));

                if (subscriptionToRemove != null)
                {
                    _subscribers.Remove(subscriptionToRemove);
                }
            }
        }
    }
}
