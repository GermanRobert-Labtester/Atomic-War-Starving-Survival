using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.Flags
{
    /// <summary>
    /// Represents an immutable history record of a single consequence, flag, or counter change.
    /// </summary>
    [Serializable]
    public class ConsequenceRecord
    {
        public string key { get; set; } = string.Empty;
        public string kind { get; set; } = "flag"; // "flag" or "counter"
        public int value { get; set; } = 1;
        public string originSystem { get; set; } = string.Empty;
        public string sourceEvent { get; set; } = string.Empty;
        public int day { get; set; } = 0;
        public string subjectId { get; set; } = string.Empty;

        public ConsequenceRecord() { }

        public ConsequenceRecord(string key, string kind, int value, string originSystem = "", string sourceEvent = "", int day = 0, string subjectId = "")
        {
            this.key = key ?? string.Empty;
            this.kind = kind ?? "flag";
            this.value = value;
            this.originSystem = originSystem ?? string.Empty;
            this.sourceEvent = sourceEvent ?? string.Empty;
            this.day = day;
            this.subjectId = subjectId ?? string.Empty;
        }

        public override string ToString() => $"[Day {day} | {originSystem}] {kind}:{key} = {value} ({sourceEvent} {subjectId})".Trim();
    }

    /// <summary>
    /// Save DTO for the unified CampaignConsequenceLedger.
    /// </summary>
    [Serializable]
    public class CampaignConsequenceSaveState
    {
        public int schemaVersion { get; set; } = 1;
        public List<string> flags { get; set; } = new List<string>();
        public Dictionary<string, int> counters { get; set; } = new Dictionary<string, int>(StringComparer.Ordinal);
        public List<ConsequenceRecord> history { get; set; } = new List<ConsequenceRecord>();
    }

    /// <summary>
    /// Single authoritative persisted campaign consequence and flag ledger.
    /// Replaces fragmented per-domain and private in-memory flag collections.
    /// Guarantees ordinal-normalized IDs, mutation provenance, and history queries.
    /// </summary>
    public sealed class CampaignConsequenceLedger : IFlagLedger
    {
        public const int CurrentSchemaVersion = 1;

        public static string Normalize(string? id) => id == null ? string.Empty : id.Trim().ToLowerInvariant();

        private readonly HashSet<string> _flags = new HashSet<string>(StringComparer.Ordinal);
        private readonly Dictionary<string, int> _counters = new Dictionary<string, int>(StringComparer.Ordinal);
        private readonly List<ConsequenceRecord> _history = new List<ConsequenceRecord>();
        private readonly object _lock = new object();

        public event Action<ConsequenceRecord>? OnConsequenceRecorded;

        public bool IsSet(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return false;
            lock (_lock)
            {
                return _flags.Contains(Normalize(flagId));
            }
        }

        public void Set(string flagId) => Set(flagId, "core", string.Empty, 0, string.Empty);

        public void Set(string flagId, string originSystem, string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(flagId)) return;
            string n = Normalize(flagId);
            ConsequenceRecord record;
            lock (_lock)
            {
                if (_flags.Add(n))
                {
                    record = new ConsequenceRecord(n, "flag", 1, originSystem, sourceEvent, day, subjectId);
                    _history.Add(record);
                }
                else
                {
                    return;
                }
            }
            OnConsequenceRecorded?.Invoke(record);
        }

        public void Clear(string flagId)
        {
            if (string.IsNullOrEmpty(flagId)) return;
            string n = Normalize(flagId);
            lock (_lock)
            {
                _flags.Remove(n);
            }
        }

        public int GetCounter(string counterId)
        {
            if (string.IsNullOrEmpty(counterId)) return 0;
            lock (_lock)
            {
                _counters.TryGetValue(Normalize(counterId), out var val);
                return val;
            }
        }

        public void Increment(string counterId, int amount = 1) => Increment(counterId, amount, "core", string.Empty, 0, string.Empty);

        public void Increment(string counterId, int amount, string originSystem, string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(counterId) || amount == 0) return;
            string n = Normalize(counterId);
            ConsequenceRecord record;
            lock (_lock)
            {
                _counters.TryGetValue(n, out var cur);
                int next = cur + amount;
                _counters[n] = next;
                record = new ConsequenceRecord(n, "counter", next, originSystem, sourceEvent, day, subjectId);
                _history.Add(record);
            }
            OnConsequenceRecorded?.Invoke(record);
        }

        public void SetCounter(string counterId, int value) => SetCounter(counterId, value, "core", string.Empty, 0, string.Empty);

        public void SetCounter(string counterId, int value, string originSystem, string sourceEvent = "", int day = 0, string subjectId = "")
        {
            if (string.IsNullOrEmpty(counterId)) return;
            string n = Normalize(counterId);
            ConsequenceRecord record;
            lock (_lock)
            {
                _counters[n] = value;
                record = new ConsequenceRecord(n, "counter", value, originSystem, sourceEvent, day, subjectId);
                _history.Add(record);
            }
            OnConsequenceRecorded?.Invoke(record);
        }

        public IReadOnlyList<string> GetAllFlags()
        {
            lock (_lock)
            {
                return _flags.OrderBy(f => f, StringComparer.Ordinal).ToList();
            }
        }

        public IReadOnlyDictionary<string, int> GetAllCounters()
        {
            lock (_lock)
            {
                return new Dictionary<string, int>(_counters, StringComparer.Ordinal);
            }
        }

        public IReadOnlyList<ConsequenceRecord> GetHistory()
        {
            lock (_lock)
            {
                return new List<ConsequenceRecord>(_history);
            }
        }

        public IReadOnlyList<ConsequenceRecord> GetHistoryForSystem(string originSystem)
        {
            if (string.IsNullOrEmpty(originSystem)) return Array.Empty<ConsequenceRecord>();
            string norm = originSystem.Trim().ToLowerInvariant();
            lock (_lock)
            {
                return _history.Where(h => string.Equals(h.originSystem, norm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        public IReadOnlyList<ConsequenceRecord> GetHistoryForSubject(string subjectId)
        {
            if (string.IsNullOrEmpty(subjectId)) return Array.Empty<ConsequenceRecord>();
            string norm = subjectId.Trim().ToLowerInvariant();
            lock (_lock)
            {
                return _history.Where(h => string.Equals(h.subjectId, norm, StringComparison.OrdinalIgnoreCase)).ToList();
            }
        }

        /// <summary>
        /// Imports legacy flags from domain saves, safely reconciling without duplicates.
        /// </summary>
        public int ImportLegacyFlags(IEnumerable<string>? legacyFlags, string originSystem, int day = 0)
        {
            if (legacyFlags == null) return 0;
            int imported = 0;
            foreach (var flag in legacyFlags)
            {
                if (string.IsNullOrWhiteSpace(flag)) continue;
                string n = Normalize(flag);
                if (!IsSet(n))
                {
                    Set(n, originSystem, "legacy_import", day);
                    imported++;
                }
            }
            return imported;
        }

        /// <summary>
        /// Imports legacy counters from domain saves, taking the maximum value.
        /// </summary>
        public int ImportLegacyCounters(IReadOnlyDictionary<string, int>? legacyCounters, string originSystem, int day = 0)
        {
            if (legacyCounters == null) return 0;
            int imported = 0;
            foreach (var kv in legacyCounters)
            {
                if (string.IsNullOrWhiteSpace(kv.Key)) continue;
                string n = Normalize(kv.Key);
                int cur = GetCounter(n);
                if (kv.Value > cur)
                {
                    SetCounter(n, kv.Value, originSystem, "legacy_import", day);
                    imported++;
                }
            }
            return imported;
        }

        public CampaignConsequenceSaveState CaptureState()
        {
            lock (_lock)
            {
                return new CampaignConsequenceSaveState
                {
                    schemaVersion = CurrentSchemaVersion,
                    flags = _flags.OrderBy(f => f, StringComparer.Ordinal).ToList(),
                    counters = new Dictionary<string, int>(_counters, StringComparer.Ordinal),
                    history = new List<ConsequenceRecord>(_history)
                };
            }
        }

        public void RestoreState(CampaignConsequenceSaveState? state)
        {
            if (state == null) return;
            lock (_lock)
            {
                _flags.Clear();
                _counters.Clear();
                _history.Clear();

                if (state.flags != null)
                {
                    foreach (var f in state.flags)
                    {
                        if (!string.IsNullOrWhiteSpace(f))
                            _flags.Add(Normalize(f));
                    }
                }

                if (state.counters != null)
                {
                    foreach (var kv in state.counters)
                    {
                        if (!string.IsNullOrWhiteSpace(kv.Key))
                            _counters[Normalize(kv.Key)] = kv.Value;
                    }
                }

                if (state.history != null)
                {
                    foreach (var rec in state.history)
                    {
                        if (rec != null)
                            _history.Add(rec);
                    }
                }
            }
        }

        public void ClearAll()
        {
            lock (_lock)
            {
                _flags.Clear();
                _counters.Clear();
                _history.Clear();
            }
        }
    }
}
