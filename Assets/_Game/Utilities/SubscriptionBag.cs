using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Collects event-unsubscribe actions so a long-lived host (GameBootstrap) can
    /// release every `+=` it made during init with a single `DisposeAll()` call.
    /// Without this, anonymous lambdas and instance-method subscriptions keep the
    /// host alive via the closure/`this` capture even after the scene tears it down
    /// (ASHFALL audit C-1).
    /// </summary>
    public sealed class SubscriptionBag
    {
        private readonly List<Action> _unsubscribers = new List<Action>();

        /// <summary>
        /// Record an unsubscribe action to run on <see cref="DisposeAll"/>. Call this
        /// right after the matching `+=` — e.g.
        /// <code>
        /// Foo.OnBar += HandleBar;
        /// bag.Track(() => Foo.OnBar -= HandleBar);
        /// </code>
        /// </summary>
        public void Track(Action unsubscribe)
        {
            if (unsubscribe == null) return;
            _unsubscribers.Add(unsubscribe);
        }

        /// <summary>Number of tracked subscriptions (test/diagnostic use).</summary>
        public int Count => _unsubscribers.Count;

        /// <summary>
        /// Run every tracked unsubscribe action and clear the bag. Safe to call more
        /// than once. Errors in one unsubscribe do not block the rest — a bad
        /// subscription source shouldn't leak every subscription registered after it.
        /// </summary>
        public void DisposeAll()
        {
            for (int i = 0; i < _unsubscribers.Count; i++)
            {
                try
                {
                    _unsubscribers[i]?.Invoke();
                }
                catch (MissingReferenceException ex)
                {
                    Debug.LogWarning($"[SubscriptionBag] Unsubscribe #{i} skipped destroyed object: {ex.Message}");
                }
                catch (Exception ex)
                {
                    Debug.LogError($"[SubscriptionBag] Unsubscribe #{i} threw: {ex}");
                }
            }
            _unsubscribers.Clear();
        }
    }
}
