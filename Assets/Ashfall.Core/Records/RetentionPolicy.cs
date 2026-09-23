// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Records
{
    /// <summary>
    /// Retention action applied when a persisted log or history collection reaches capacity.
    /// </summary>
    public enum RetentionAction
    {
        /// <summary>Retains the newest K entries and purges older records.</summary>
        KeepNewestK = 0,

        /// <summary>Aggregates older records into a deterministic statistical summary.</summary>
        RollupSummary = 1,

        /// <summary>Preserves keys, timestamps, and identifiers while dropping prose bodies.</summary>
        DropProseKeepIds = 2
    }

    /// <summary>
    /// Declares the explicit retention limit and trimming behavior for a persisted collection.
    /// </summary>
    public sealed class RetentionPolicyDefinition
    {
        public string CollectionKey { get; set; } = string.Empty;
        public int MaxCapacity { get; set; } = 200;
        public RetentionAction Action { get; set; } = RetentionAction.KeepNewestK;

        /// <summary>
        /// Iron rule: obligation state (deadlines, grief, pair history, wills, memorials) must never be pruned.
        /// </summary>
        public bool IsProtectedObligation { get; set; }

        public RetentionPolicyDefinition() { }

        public RetentionPolicyDefinition(
            string collectionKey,
            int maxCapacity = 200,
            RetentionAction action = RetentionAction.KeepNewestK,
            bool isProtectedObligation = false)
        {
            CollectionKey = collectionKey ?? string.Empty;
            MaxCapacity = Math.Max(1, maxCapacity);
            Action = action;
            IsProtectedObligation = isProtectedObligation;
        }
    }

    /// <summary>
    /// Bounded generic rolling log container providing deterministic capacity enforcement
    /// and total historical tracking across multi-year and 400-year campaigns.
    /// </summary>
    /// <typeparam name="T">Entry type.</typeparam>
    public sealed class RollingLog<T>
    {
        private readonly List<T> _entries = new List<T>();

        public int Capacity { get; private set; }
        public int TotalRecordedCount { get; private set; }
        public int PrunedCount { get; private set; }

        public IReadOnlyList<T> CurrentEntries => _entries;
        public int Count => _entries.Count;

        public RollingLog(int capacity = 200)
        {
            Capacity = Math.Max(1, capacity);
        }

        public void Append(T entry)
        {
            TotalRecordedCount++;
            _entries.Add(entry);

            if (_entries.Count > Capacity)
            {
                int excess = _entries.Count - Capacity;
                _entries.RemoveRange(0, excess);
                PrunedCount += excess;
            }
        }

        public void AppendRange(IEnumerable<T> entries)
        {
            if (entries == null) return;
            foreach (var e in entries)
            {
                Append(e);
            }
        }

        public void SetCapacity(int newCapacity)
        {
            Capacity = Math.Max(1, newCapacity);
            if (_entries.Count > Capacity)
            {
                int excess = _entries.Count - Capacity;
                _entries.RemoveRange(0, excess);
                PrunedCount += excess;
            }
        }

        public void LoadSnapshot(IEnumerable<T> items, int totalRecorded = 0)
        {
            _entries.Clear();
            if (items != null)
            {
                _entries.AddRange(items);
            }

            if (_entries.Count > Capacity)
            {
                int excess = _entries.Count - Capacity;
                _entries.RemoveRange(0, excess);
                PrunedCount += excess;
            }

            TotalRecordedCount = Math.Max(_entries.Count, totalRecorded);
        }

        public void Clear()
        {
            _entries.Clear();
            TotalRecordedCount = 0;
            PrunedCount = 0;
        }
    }

    /// <summary>
    /// Audit report recording retention policy execution across campaign collections.
    /// </summary>
    public sealed class RetentionAuditReport
    {
        public int TotalCollectionsAudited { get; set; }
        public int TotalEntriesPruned { get; set; }
        public int TotalProtectedCollectionsPreserved { get; set; }
        public List<string> PrunedCollectionKeys { get; } = new List<string>();
        public List<string> ProtectedCollectionKeys { get; } = new List<string>();
    }

    /// <summary>
    /// Catalog of retention policies covering all growing collections in campaign state.
    /// Guarantees bounded save sizes over 400-year campaigns.
    /// </summary>
    public sealed class RetentionPolicyCatalog
    {
        private readonly Dictionary<string, RetentionPolicyDefinition> _policies =
            new Dictionary<string, RetentionPolicyDefinition>(StringComparer.OrdinalIgnoreCase);

        /// <summary>Delegate seam fired whenever a collection is trimmed by retention policy.</summary>
        public Action<string, int, int>? OnEntriesPrunedSeam { get; set; }

        public RetentionPolicyCatalog()
        {
            RegisterDefaultPolicies();
        }

        public void RegisterPolicy(RetentionPolicyDefinition policy)
        {
            if (policy == null || string.IsNullOrWhiteSpace(policy.CollectionKey))
                throw new ArgumentException("Policy must have a valid collection key.", nameof(policy));

            _policies[policy.CollectionKey] = policy;
        }

        public bool TryGetPolicy(string key, out RetentionPolicyDefinition? policy)
        {
            if (string.IsNullOrWhiteSpace(key))
            {
                policy = null;
                return false;
            }
            return _policies.TryGetValue(key, out policy);
        }

        /// <summary>All active policies (defaults plus any authored overlay).</summary>
        public IReadOnlyList<RetentionPolicyDefinition> ActivePolicies =>
            new List<RetentionPolicyDefinition>(_policies.Values);

        /// <summary>True when a policy exists for <paramref name="key"/>.</summary>
        public bool HasPolicy(string key)
            => !string.IsNullOrWhiteSpace(key) && _policies.ContainsKey(key);

        /// <summary>
        /// Plan 55 / Task 55A — overlay authored rows over the built-in defaults.
        /// The data authority wins for every key it names; unlisted collections
        /// keep their default bound, so a partial authored table can never
        /// accidentally remove a protective default.
        /// </summary>
        public int ApplyOverlay(IEnumerable<RetentionPolicyDefinition>? rows)
        {
            if (rows == null) return 0;
            int applied = 0;
            foreach (var row in rows)
            {
                if (row == null || string.IsNullOrWhiteSpace(row.CollectionKey)) continue;
                RegisterPolicy(row);
                applied++;
            }
            return applied;
        }

        /// <summary>
        /// Applies retention policy to a list of entries in a collection.
        /// Protects obligation state from ever being pruned.
        /// </summary>
        public bool ApplyRetention<T>(string collectionKey, List<T> list, out int prunedCount)
        {
            prunedCount = 0;
            if (list == null) return false;

            if (TryGetPolicy(collectionKey, out var policy) && policy != null)
            {
                // Protected obligations must NEVER be pruned
                if (policy.IsProtectedObligation)
                {
                    return true;
                }

                if (list.Count > policy.MaxCapacity)
                {
                    int before = list.Count;
                    prunedCount = list.Count - policy.MaxCapacity;
                    list.RemoveRange(0, prunedCount);
                    int after = list.Count;

                    OnEntriesPrunedSeam?.Invoke(collectionKey, before, after);
                    return true;
                }
            }

            return false;
        }

        private void RegisterDefaultPolicies()
        {
            // Standard growing collections:
            RegisterPolicy(new RetentionPolicyDefinition("kitchen_serving_log", 200, RetentionAction.KeepNewestK));
            RegisterPolicy(new RetentionPolicyDefinition("dose_ledger", 500, RetentionAction.RollupSummary));
            RegisterPolicy(new RetentionPolicyDefinition("machine_log", 300, RetentionAction.KeepNewestK));
            RegisterPolicy(new RetentionPolicyDefinition("faction_war_decrees", 100, RetentionAction.DropProseKeepIds));
            RegisterPolicy(new RetentionPolicyDefinition("pair_relation_history", 10, RetentionAction.KeepNewestK));

            // Obligation states (strictly protected, never pruned):
            RegisterPolicy(new RetentionPolicyDefinition("survivor_wills_and_legacies", int.MaxValue, RetentionAction.KeepNewestK, isProtectedObligation: true));
            RegisterPolicy(new RetentionPolicyDefinition("memorial_monuments", int.MaxValue, RetentionAction.KeepNewestK, isProtectedObligation: true));
            RegisterPolicy(new RetentionPolicyDefinition("campaign_deadlines", int.MaxValue, RetentionAction.KeepNewestK, isProtectedObligation: true));
            RegisterPolicy(new RetentionPolicyDefinition("survivor_grief_markers", int.MaxValue, RetentionAction.KeepNewestK, isProtectedObligation: true));
        }
    }
}
