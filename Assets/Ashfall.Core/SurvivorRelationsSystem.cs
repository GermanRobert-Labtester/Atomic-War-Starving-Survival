using System;
using System.Collections.Generic;

namespace Ashfall.Core
{
    [Serializable]
    public sealed class SurvivorRelationsState
    {
        public string systemId = SurvivorRelationsSystem.SystemId;
        public List<RelationshipEntry> relationships = new List<RelationshipEntry>();
        public List<ConflictEntry> activeConflicts = new List<ConflictEntry>();
        public List<MediationEntry> mediationHistory = new List<MediationEntry>();
    }

    [Serializable]
    public sealed class RelationshipEntry
    {
        public string dwellerA = string.Empty;
        public string dwellerB = string.Empty;
        public float affinity;      // -100 to 100
        public float trust;         // 0 to 100
        public float resentment;    // 0 to 100
        public float grief;         // 0 to 100
        public string bondType = string.Empty; // "friendship", "rivalry", "mentor", "caregiver", etc.
        public List<string> recentCauses = new List<string>();
    }

    [Serializable]
    public sealed class ConflictEntry
    {
        public string conflictId = string.Empty;
        public string dwellerA = string.Empty;
        public string dwellerB = string.Empty;
        public string cause = string.Empty;
        public int dayStarted;
        public bool isResolved;
        public string resolution = string.Empty;
    }

    [Serializable]
    public sealed class MediationEntry
    {
        public string conflictId = string.Empty;
        public int day;
        public string mediatorId = string.Empty;
        public string outcome = string.Empty;
        public float affinityChange;
    }

    public sealed class SurvivorRelationsSystem
    {
        public const string SystemId = "survivor_relations";
        private SurvivorRelationsState _state = new SurvivorRelationsState();
        private readonly ISeededRng _rng;
        private readonly ILog _log;
        private int _currentDay;

        public SurvivorRelationsState State => _state;
        public event Action<ConflictEntry> OnConflictStarted;
        public event Action<MediationEntry> OnConflictResolved;
        public event Action OnRelationsChanged;

        public SurvivorRelationsSystem(ISeededRng rng, ILog log = null)
        {
            _rng = rng ?? throw new ArgumentNullException(nameof(rng));
            _log = log ?? NullLog.Instance;
        }

        public RelationshipEntry GetOrCreateRelationship(string a, string b)
        {
            var key = MakeKey(a, b);
            var existing = _state.relationships.Find(r => MakeKey(r.dwellerA, r.dwellerB) == key);
            if (existing != null) return existing;
            var rel = new RelationshipEntry { dwellerA = a, dwellerB = b };
            _state.relationships.Add(rel);
            return rel;
        }

        private static string MakeKey(string a, string b) =>
            string.CompareOrdinal(a, b) <= 0 ? $"{a}|{b}" : $"{b}|{a}";

        public void ModifyAffinity(string a, string b, float delta)
        {
            var rel = GetOrCreateRelationship(a, b);
            rel.affinity = Math.Max(-100, Math.Min(100, rel.affinity + delta));
            if (delta < 0) rel.resentment = Math.Min(100, rel.resentment - delta);
            rel.recentCauses.Add($"affinity_{delta:F0} on day {_currentDay}");
            OnRelationsChanged?.Invoke();
        }

        public void ModifyTrust(string a, string b, float delta)
        {
            var rel = GetOrCreateRelationship(a, b);
            rel.trust = Math.Max(0, Math.Min(100, rel.trust + delta));
        }

        public void ApplyGrief(string survivorId, float amount)
        {
            foreach (var rel in _state.relationships)
            {
                if (rel.dwellerA == survivorId || rel.dwellerB == survivorId)
                {
                    rel.grief = Math.Min(100, rel.grief + amount);
                    rel.affinity = Math.Max(-100, rel.affinity - amount * 0.5f);
                }
            }
            OnRelationsChanged?.Invoke();
        }

        public ConflictEntry? TryTriggerConflict()
        {
            if (_state.activeConflicts.Exists(c => !c.isResolved)) return null;
            if (_rng.NextDouble() > 0.1f) return null; // 10% chance per day

            var stressed = _state.relationships.FindAll(r => r.resentment > 50f || r.affinity < -30f);
            if (stressed.Count == 0) return null;

            var rel = stressed[_rng.Next(0, stressed.Count)];
            var conflict = new ConflictEntry
            {
                conflictId = $"conflict_{_currentDay}_{rel.dwellerA}_{rel.dwellerB}",
                dwellerA = rel.dwellerA, dwellerB = rel.dwellerB,
                cause = $"resentment {rel.resentment:F0}/affinity {rel.affinity:F0}",
                dayStarted = _currentDay
            };
            _state.activeConflicts.Add(conflict);
            _log.Info($"[Relations] conflict: {rel.dwellerA} vs {rel.dwellerB}");
            OnConflictStarted?.Invoke(conflict);
            OnRelationsChanged?.Invoke();
            return conflict;
        }

        public ActionResult Mediate(string conflictId, string mediatorId, MediationStyle style)
        {
            var conflict = _state.activeConflicts.Find(c => c.conflictId == conflictId);
            if (conflict == null) return ActionResult.Failed("unknown_conflict", "relations.unknown_conflict");
            if (conflict.isResolved) return ActionResult.Blocked("already_resolved", "relations.already_resolved");

            float affinityDelta = style switch
            {
                MediationStyle.Apology => 15f,
                MediationStyle.ResourceSettlement => 20f,
                MediationStyle.Discipline => -10f,
                MediationStyle.Refusal => -25f,
                _ => 5f
            };

            ModifyAffinity(conflict.dwellerA, conflict.dwellerB, affinityDelta);
            conflict.isResolved = true;
            conflict.resolution = style.ToString();

            var entry = new MediationEntry
            {
                conflictId = conflictId, day = _currentDay,
                mediatorId = mediatorId ?? string.Empty,
                outcome = style.ToString(), affinityChange = affinityDelta
            };
            _state.mediationHistory.Add(entry);
            _log.Info($"[Relations] mediated {conflictId}: {style} ({affinityDelta:F0} affinity)");
            OnConflictResolved?.Invoke(entry);
            OnRelationsChanged?.Invoke();
            return ActionResult.Success("relations.mediated",
                new Dictionary<string, double> { { "affinity_change", affinityDelta } });
        }

        public void TickDay(int day)
        {
            _currentDay = day;
            TryTriggerConflict();
        }

        public SurvivorRelationsState CaptureState() => _state;
        public void RestoreState(SurvivorRelationsState saved)
        {
            if (saved == null) return;
            _state = saved;
            OnRelationsChanged?.Invoke();
        }
    }

    public enum MediationStyle { Apology, ResourceSettlement, Discipline, Refusal }
}
