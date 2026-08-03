using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// 2D matrix of how much survivors like or hate each other, in the
    /// range [-100, +100]. Mutated by EventRunner choices
    /// (EventRunner.ApplyEffect) and by MentalBreakSystem when a
    /// ViolentParanoia survivor sabotages a room the affinity is
    /// already loaded with.
    ///
    /// Save/load safe: the underlying storage is a flat dictionary of
    /// (a,b) -> affinity. Undirected: a→b == b→a. Missing pairs are
    /// treated as 0 (neutral). The matrix is intentionally simple —
    /// we don't model the full directed social graph yet; that's a
    /// later prompt.
    /// </summary>
    [Serializable]
    public class InterpersonalAffinity
    {
        /// <summary>Storage: outer key is the lower-sorted survivor id, inner the higher.
        /// Flat lookup is O(1); symmetry is enforced on every write.</summary>
        private readonly Dictionary<string, Dictionary<string, float>> _matrix =
            new Dictionary<string, Dictionary<string, float>>();

        /// <summary>Get the affinity between two survivors in the range [-100, +100].
        /// Returns 0 (neutral) for unknown pairs or null inputs.</summary>
        public float Get(string a, string b)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b) || a == b) return 0f;
            var (low, high) = SortPair(a, b);
            if (_matrix.TryGetValue(low, out var inner) && inner.TryGetValue(high, out var v))
            {
                return v;
            }
            return 0f;
        }

        /// <summary>Adjust the affinity between two survivors by <paramref name="delta"/>,
        /// clamped to [-100, +100]. Symmetric (a→b == b→a). No-op on null / equal ids.</summary>
        public void Adjust(string a, string b, float delta)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b) || a == b || Mathf.Approximately(delta, 0f)) return;
            var (low, high) = SortPair(a, b);
            if (!_matrix.TryGetValue(low, out var inner))
            {
                inner = new Dictionary<string, float>();
                _matrix[low] = inner;
            }
            float cur = inner.TryGetValue(high, out var v) ? v : 0f;
            inner[high] = Mathf.Clamp(cur + delta, -100f, 100f);
        }

        /// <summary>Set the absolute affinity between two survivors, clamped to
        /// [-100, +100]. Symmetric. Useful for save/load restore.</summary>
        public void Set(string a, string b, float value)
        {
            if (string.IsNullOrEmpty(a) || string.IsNullOrEmpty(b) || a == b) return;
            var (low, high) = SortPair(a, b);
            if (!_matrix.TryGetValue(low, out var inner))
            {
                inner = new Dictionary<string, float>();
                _matrix[low] = inner;
            }
            inner[high] = Mathf.Clamp(value, -100f, 100f);
        }

        /// <summary>For the save snapshot: every (sortedA, sortedB, value) triple
        /// where value != 0. Empty list = empty matrix.</summary>
        public List<AffinityEntry> Snapshot()
        {
            var list = new List<AffinityEntry>();
            foreach (var kv in _matrix)
            {
                foreach (var inner in kv.Value)
                {
                    if (!Mathf.Approximately(inner.Value, 0f))
                    {
                        list.Add(new AffinityEntry { SurvivorA = kv.Key, SurvivorB = inner.Key, Value = inner.Value });
                    }
                }
            }
            return list;
        }

        /// <summary>Restore from a list of entries (as produced by Snapshot).</summary>
        public void Restore(List<AffinityEntry> entries)
        {
            _matrix.Clear();
            if (entries == null) return;
            for (int i = 0; i < entries.Count; i++)
            {
                var e = entries[i];
                if (e == null || string.IsNullOrEmpty(e.SurvivorA) || string.IsNullOrEmpty(e.SurvivorB)) continue;
                if (e.SurvivorA == e.SurvivorB) continue;
                var (low, high) = SortPair(e.SurvivorA, e.SurvivorB);
                if (!_matrix.TryGetValue(low, out var inner))
                {
                    inner = new Dictionary<string, float>();
                    _matrix[low] = inner;
                }
                inner[high] = Mathf.Clamp(e.Value, -100f, 100f);
            }
        }

        private static (string low, string high) SortPair(string a, string b)
        {
            return string.CompareOrdinal(a, b) <= 0 ? (a, b) : (b, a);
        }
    }

    /// <summary>One (survivorA, survivorB, affinity) triple for save/load.</summary>
    [Serializable]
    public class AffinityEntry
    {
        public string SurvivorA;
        public string SurvivorB;
        public float Value;
    }

    /// <summary>
    /// Save snapshot for the full affinity matrix. Lives in the
    /// Survivors assembly so it can use <see cref="AffinityEntry"/>
    /// directly. The SaveData class in Core holds a reference to
    /// this type via the Core→Survivors asmdef edge.
    /// </summary>
    [Serializable]
    public class AffinityMatrixSave
    {
        public List<AffinityEntry> Entries = new List<AffinityEntry>();
    }
}
