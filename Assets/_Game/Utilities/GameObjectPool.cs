using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Utilities
{
    /// <summary>
    /// Prefab pool for GameObjects/UI widgets: Acquire activates, Release
    /// deactivates + reparents under the pool root. No Instantiate/Destroy
    /// at runtime after warm-up. Thin wrapper over <see cref="GenericObjectPool{T}"/>
    /// for the day the UI layer moves from data-only view-models to real
    /// uGUI prefabs (journal rows, inventory icons, map nodes).
    /// </summary>
    public class GameObjectPool
    {
        private readonly GenericObjectPool<GameObject> _pool;
        private readonly Transform _poolRoot;

        /// <summary>Total prefab instances ever instantiated (test/profiling hook).</summary>
        public int InstancesCreated => _pool.InstancesCreated;

        /// <summary>Instances currently active in the scene.</summary>
        public int ActiveCount => _pool.ActiveCount;

        /// <summary>Instances parked inactive under the pool root.</summary>
        public int PooledCount => _pool.PooledCount;

        /// <param name="prefab">Inactive template; instantiated only while the pool grows.</param>
        /// <param name="poolRoot">Inactive instances live under this transform. Created automatically when null.</param>
        /// <param name="initialCapacity">Instances pre-instantiated up front.</param>
        /// <param name="maxCapacity">Hard ceiling; see <see cref="GenericObjectPool{T}"/> semantics.</param>
        public GameObjectPool(GameObject prefab, Transform poolRoot = null, int initialCapacity = 0, int maxCapacity = 0)
        {
            if (prefab == null) throw new ArgumentNullException(nameof(prefab));

            _poolRoot = poolRoot != null ? poolRoot : new GameObject("GameObjectPool_Root").transform;
            _pool = new GenericObjectPool<GameObject>(
                factory: () =>
                {
                    var go = UnityEngine.Object.Instantiate(prefab, _poolRoot);
                    go.SetActive(false);
                    return go;
                },
                onReset: go =>
                {
                    go.SetActive(false);
                    go.transform.SetParent(_poolRoot, false);
                },
                initialCapacity: initialCapacity,
                maxCapacity: maxCapacity);
        }

        /// <summary>Take an instance: activates it and optionally re-parents it. Null when the cap is hit.</summary>
        public GameObject Acquire(Transform parent = null)
        {
            var go = _pool.Acquire();
            if (go == null) return null;
            if (parent != null)
                go.transform.SetParent(parent, false);
            go.SetActive(true);
            return go;
        }

        /// <summary>Return an instance: deactivates it and parks it under the pool root. Never destroys.</summary>
        public void Release(GameObject instance)
        {
            _pool.Release(instance);
        }

        /// <summary>Release every instance in the given collection (null-safe).</summary>
        public void ReleaseAll(IEnumerable<GameObject> instances)
        {
            _pool.ReleaseAll(instances);
        }
    }
}
