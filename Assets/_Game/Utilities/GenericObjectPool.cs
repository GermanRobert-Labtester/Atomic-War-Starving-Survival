using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Generic growable pool that recycles heap objects instead of
    /// allocating/collecting them at runtime. Used for the churn-heavy UI
    /// data layer (journal entries, inventory icons, map-node views,
    /// expedition path lines) so long sessions and fast-forward time-skips
    /// produce no GC spikes.
    ///
    /// Infrastructure-only: holds no gameplay state, so it is inherently
    /// save/load safe (pooled instances are never serialized; live instances
    /// are owned by the system that acquired them).
    /// </summary>
    public class GenericObjectPool<T> where T : class
    {
        private readonly Stack<T> _available;
        private readonly Func<T> _factory;
        private readonly Action<T> _onReset;
        private readonly int _maxCapacity;

        /// <summary>Total instances ever created by this pool (test/profiling hook).</summary>
        public int InstancesCreated { get; private set; }

        /// <summary>Instances currently acquired and in use.</summary>
        public int ActiveCount { get; private set; }

        /// <summary>Instances parked in the pool waiting for reuse.</summary>
        public int PooledCount => _available.Count;

        /// <param name="factory">Creates a fresh instance when the pool is empty. Required.</param>
        /// <param name="onReset">
        /// Optional scrub hook invoked on Release so a recycled instance never
        /// leaks stale state to its next acquirer.
        /// </param>
        /// <param name="initialCapacity">Instances pre-created up front (avoids first-use spikes).</param>
        /// <param name="maxCapacity">
        /// Hard ceiling on InstancesCreated; 0 or less means unbounded. When the
        /// ceiling is hit and the pool is empty, Acquire returns null instead
        /// of allocating — callers must handle that (pool exhaustion is a
        /// tuning signal, not a crash).
        /// </param>
        public GenericObjectPool(Func<T> factory, Action<T> onReset = null, int initialCapacity = 0, int maxCapacity = 0)
        {
            _factory = factory ?? throw new ArgumentNullException(nameof(factory));
            _onReset = onReset;
            _maxCapacity = maxCapacity;
            _available = new Stack<T>(Math.Max(0, initialCapacity));

            for (int i = 0; i < initialCapacity; i++)
            {
                _available.Push(_factory());
                InstancesCreated++;
            }
        }

        /// <summary>Take an instance from the pool, creating one only if empty (and under the cap).</summary>
        public T Acquire()
        {
            if (_available.Count > 0)
            {
                ActiveCount++;
                return _available.Pop();
            }
            if (_maxCapacity > 0 && InstancesCreated >= _maxCapacity)
                return null;

            var instance = _factory();
            InstancesCreated++;
            ActiveCount++;
            return instance;
        }

        /// <summary>Return an instance to the pool (reset hook runs first). Null and double-releases are ignored defensively.</summary>
        public void Release(T instance)
        {
            if (instance == null || ActiveCount <= 0) return;
            _onReset?.Invoke(instance);
            _available.Push(instance);
            ActiveCount--;
        }

        /// <summary>Release every instance in the given collection (null-safe).</summary>
        public void ReleaseAll(IEnumerable<T> instances)
        {
            if (instances == null) return;
            foreach (var instance in instances)
                Release(instance);
        }

        /// <summary>Drop all pooled (non-active) instances. Active instances are untouched.</summary>
        public void ClearPooled()
        {
            _available.Clear();
        }
    }
}
