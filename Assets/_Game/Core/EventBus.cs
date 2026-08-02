using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Type-safe publish/subscribe bus that decouples the pure-C# systems, the
    /// UI layer, and save/load. Every public system raises events here on state
    /// change so consumers never hold direct references to each other.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        /// <summary>Register a handler for events of type T.</summary>
        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            var type = typeof(T);
            if (!_subscribers.TryGetValue(type, out var list))
            {
                list = new List<Delegate>();
                _subscribers[type] = list;
            }
            if (!list.Contains(handler))
            {
                list.Add(handler);
            }
        }

        /// <summary>Remove a previously registered handler.</summary>
        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            if (_subscribers.TryGetValue(typeof(T), out var list))
            {
                list.Remove(handler);
            }
        }

        /// <summary>Publish an event to all current subscribers of its type.</summary>
        public static void Raise<T>(T eventData)
        {
            if (!_subscribers.TryGetValue(typeof(T), out var list)) return;
            // Copy to avoid mutation during iteration
            var snapshot = new List<Delegate>(list);
            for (int i = 0; i < snapshot.Count; i++)
            {
                ((Action<T>)snapshot[i])?.Invoke(eventData);
            }
        }

        /// <summary>Drop all subscriptions (e.g. on scene teardown).</summary>
        public static void Clear()
        {
            _subscribers.Clear();
        }
    }
}
