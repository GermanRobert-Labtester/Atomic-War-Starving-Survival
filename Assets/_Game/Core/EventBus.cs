using System;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Type-safe publish/subscribe bus that decouples the pure-C# systems, the
    /// UI layer, and save/load. Every public system raises events here on state
    /// change so consumers never hold direct references to each other.
    /// </summary>
    public static class EventBus
    {
        /// <summary>Register a handler for events of type <typeparamref name="T"/>.</summary>
        public static void Subscribe<T>(Action<T> handler) => throw new System.NotImplementedException();

        /// <summary>Remove a previously registered handler.</summary>
        public static void Unsubscribe<T>(Action<T> handler) => throw new System.NotImplementedException();

        /// <summary>Publish an event to all current subscribers of its type.</summary>
        public static void Raise<T>(T eventData) => throw new System.NotImplementedException();

        /// <summary>Drop all subscriptions (e.g. on scene teardown).</summary>
        public static void Clear() => throw new System.NotImplementedException();
    }
}
