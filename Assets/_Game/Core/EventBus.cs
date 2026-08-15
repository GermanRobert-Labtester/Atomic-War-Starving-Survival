using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Type-safe publish/subscribe bus that decouples the pure-C# systems, the
    /// UI layer, and save/load. Every public system raises events here on state
    /// change so consumers never hold direct references to each other.
    ///
    /// Performance contract: Raise is allocation-free at steady state. The
    /// iteration snapshot is a cached array rebuilt only when the subscriber
    /// list changes (version-gated), so heavy per-tick event traffic produces
    /// no GC pressure. In the editor, every Raise is stopwatch-profiled and any
    /// event taking longer than <see cref="SlowRaiseThresholdMs"/> logs a
    /// warning — those are the micro-stutters players feel during time-skips.
    /// </summary>
    public static class EventBus
    {
        /// <summary>Editor-only budget per Raise before a warning is logged.</summary>
        public const double SlowRaiseThresholdMs = 2.0;

        private sealed class Subscription
        {
            public readonly List<Delegate> Handlers = new List<Delegate>();
            public Delegate[] Snapshot = Array.Empty<Delegate>();
            public int Version;        // bumped on Subscribe/Unsubscribe/Clear
            public int SnapshotVersion = -1;
        }

        private static readonly Dictionary<Type, Subscription> _subscribers = new Dictionary<Type, Subscription>();

#if UNITY_EDITOR
        private static readonly Stopwatch _raiseTimer = new Stopwatch();
        private static readonly Stopwatch _handlerTimer = new Stopwatch();
#endif

        /// <summary>Register a handler for events of type T.</summary>
        public static void Subscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            var sub = GetOrCreate(typeof(T));
            if (!sub.Handlers.Contains(handler))
            {
                sub.Handlers.Add(handler);
                sub.Version++;
            }
        }

        /// <summary>Remove a previously registered handler.</summary>
        public static void Unsubscribe<T>(Action<T> handler)
        {
            if (handler == null) return;
            if (_subscribers.TryGetValue(typeof(T), out var sub))
            {
                if (sub.Handlers.Remove(handler))
                    sub.Version++;
            }
        }

        /// <summary>
        /// When true, all Raise calls are silently dropped. Toggled by
        /// SaveSystem during restore so side effects from RestoreState do
        /// not cascade into partially-restored systems (audit M-8).
        /// </summary>
        public static bool IsSuppressed { get; set; }

        /// <summary>
        /// Publish an event to all current subscribers of its type.
        /// Steady-state allocation-free: the snapshot array is cached and only
        /// rebuilt after the subscriber list changes.
        /// Silently returns when <see cref="IsSuppressed"/> is true.
        /// </summary>
        public static void Raise<T>(T eventData)
        {
            if (IsSuppressed) return;
            if (!_subscribers.TryGetValue(typeof(T), out var sub)) return;
            if (sub.Handlers.Count == 0) return;

            // Rebuild the iteration snapshot only when the list mutated since
            // the last Raise (protects against Subscribe/Unsubscribe mid-dispatch).
            if (sub.SnapshotVersion != sub.Version)
            {
                sub.Snapshot = sub.Handlers.ToArray();
                sub.SnapshotVersion = sub.Version;
            }

#if UNITY_EDITOR
            _raiseTimer.Restart();
            double slowestMs = 0.0;
            string slowestHandler = null;
#endif

            var snapshot = sub.Snapshot;
            for (int i = 0; i < snapshot.Length; i++)
            {
#if UNITY_EDITOR
                _handlerTimer.Restart();
                ((Action<T>)snapshot[i])?.Invoke(eventData);
                _handlerTimer.Stop();
                double ms = _handlerTimer.Elapsed.TotalMilliseconds;
                if (ms > slowestMs)
                {
                    slowestMs = ms;
                    slowestHandler = DescribeHandler(snapshot[i]);
                }
#else
                ((Action<T>)snapshot[i])?.Invoke(eventData);
#endif
            }

#if UNITY_EDITOR
            _raiseTimer.Stop();
            double totalMs = _raiseTimer.Elapsed.TotalMilliseconds;
            if (totalMs > SlowRaiseThresholdMs)
            {
                UnityEngine.Debug.LogWarning(
                    $"[EventBus] Slow event: {typeof(T).Name} took {totalMs:0.00}ms " +
                    $"(> {SlowRaiseThresholdMs:0.0}ms budget) across {snapshot.Length} handler(s); " +
                    $"slowest: {slowestHandler ?? "?"} ({slowestMs:0.00}ms)");
            }
#endif
        }

        /// <summary>Drop all subscriptions (e.g. on scene teardown).</summary>
        public static void Clear()
        {
            _subscribers.Clear();
        }

        /// <summary>Number of current subscribers for events of type T (test/profiling hook).</summary>
        public static int SubscriberCount<T>()
        {
            return _subscribers.TryGetValue(typeof(T), out var sub) ? sub.Handlers.Count : 0;
        }

        private static Subscription GetOrCreate(Type type)
        {
            if (!_subscribers.TryGetValue(type, out var sub))
            {
                sub = new Subscription();
                _subscribers[type] = sub;
            }
            return sub;
        }

#if UNITY_EDITOR
        private static string DescribeHandler(Delegate handler)
        {
            if (handler == null) return null;
            var method = handler.Method;
            var declaring = method.DeclaringType;
            return declaring != null ? $"{declaring.Name}.{method.Name}" : method.Name;
        }
#endif
    }
}
