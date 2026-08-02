using System;
using System.Collections.Generic;

namespace AtomicWar.Core.Events
{
    /// <summary>
    /// Type-safe publish/subscribe Event Bus for decoupling pure C# domain systems.
    /// </summary>
    public static class EventBus
    {
        private static readonly Dictionary<Type, List<Delegate>> _subscribers = new Dictionary<Type, List<Delegate>>();

        public static void Subscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (!_subscribers.ContainsKey(type))
            {
                _subscribers[type] = new List<Delegate>();
            }
            _subscribers[type].Add(handler);
        }

        public static void Unsubscribe<T>(Action<T> handler)
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var handlers))
            {
                handlers.Remove(handler);
            }
        }

        public static void Raise<T>(T eventData)
        {
            var type = typeof(T);
            if (_subscribers.TryGetValue(type, out var handlers))
            {
                // Iterate over copy to allow unsubscriptions during handling
                var copy = handlers.ToArray();
                foreach (var handler in copy)
                {
                    ((Action<T>)handler)?.Invoke(eventData);
                }
            }
        }

        public static void Clear()
        {
            _subscribers.Clear();
        }
    }
}
