using System;
using System.Collections.Generic;

namespace Ashfall.Core.Events
{
    public interface IEventBus
    {
        void Publish(string eventName, object payload = null);
        void Subscribe(string eventName, Action<object> handler);
        void Unsubscribe(string eventName, Action<object> handler);
    }

    public sealed class SimpleEventBus : IEventBus
    {
        private readonly Dictionary<string, List<Action<object>>> _handlers = new Dictionary<string, List<Action<object>>>();
        private readonly List<(string name, object payload)> _publishedEvents = new List<(string name, object payload)>();

        public IReadOnlyList<(string name, object payload)> PublishedEvents => _publishedEvents;

        public void Publish(string eventName, object payload = null)
        {
            if (string.IsNullOrEmpty(eventName)) return;

            _publishedEvents.Add((eventName, payload));

            if (_handlers.TryGetValue(eventName, out var list))
            {
                var copy = new List<Action<object>>(list);
                foreach (var handler in copy)
                {
                    handler?.Invoke(payload);
                }
            }
        }

        public void Subscribe(string eventName, Action<object> handler)
        {
            if (string.IsNullOrEmpty(eventName) || handler == null) return;

            if (!_handlers.TryGetValue(eventName, out var list))
            {
                list = new List<Action<object>>();
                _handlers[eventName] = list;
            }
            list.Add(handler);
        }

        public void Unsubscribe(string eventName, Action<object> handler)
        {
            if (string.IsNullOrEmpty(eventName) || handler == null) return;

            if (_handlers.TryGetValue(eventName, out var list))
            {
                list.Remove(handler);
            }
        }

        public void ClearHistory()
        {
            _publishedEvents.Clear();
        }
    }
}
