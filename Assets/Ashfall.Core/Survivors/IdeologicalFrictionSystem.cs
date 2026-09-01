using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    [Serializable]
    public sealed class IdeologicalFrictionSaveState
    {
        public List<IdeologicalAffinityEntry> affinities = new List<IdeologicalAffinityEntry>();
    }

    [Serializable]
    public sealed class IdeologicalAffinityEntry
    {
        public string pairKey = string.Empty;
        public float affinity;
    }

    /// <summary>
    /// ASHFALL: THE MASSIVE CONTENT EXPANSION — Ideological Friction System.
    /// Differing survivor worldviews create interpersonal friction when sharing
    /// quarters, affecting sleep quality. Engine-agnostic: uses string survivor
    /// IDs and belief profile IDs, raises events, save/load safe.
    /// </summary>
    public class IdeologicalFrictionSystem
    {
        public const float ConflictSleepQualityPenalty = 0.20f;
        public const float SynergySleepQualityBonus = 0.10f;
        public const float ConflictAffinityDrainPerDay = 2f;
        public const float SynergyAffinityGainPerDay = 1f;

        public static readonly Dictionary<string, List<string>> ConflictGroups =
            new Dictionary<string, List<string>>(StringComparer.Ordinal)
        {
            { "military_discipline", new List<string> { "pragmatic_individualism", "pacifist", "religious_faith" } },
            { "religious_faith", new List<string> { "atheist_rationalist", "military_discipline" } },
            { "atheist_rationalist", new List<string> { "religious_faith", "superstitious_traditional" } },
            { "pragmatic_individualism", new List<string> { "collectivist_solidarity", "military_discipline" } },
            { "collectivist_solidarity", new List<string> { "pragmatic_individualism" } },
            { "superstitious_traditional", new List<string> { "atheist_rationalist" } },
            { "pacifist", new List<string> { "military_discipline" } },
            // Plan 12B — bunk-level philosophical profiles that pair off below.
            // Same alphabetical order as the original list so a diff is plain.
            { "belief_ration_collectivist", new List<string> { "belief_every_soul_alone", "belief_ash_nihilist" } },
            { "belief_every_soul_alone",     new List<string> { "belief_ration_collectivist", "belief_faith_in_rebuild" } },
            { "belief_faith_in_rebuild",      new List<string> { "belief_every_soul_alone", "belief_ash_nihilist" } },
            { "belief_ash_nihilist",          new List<string> { "belief_ration_collectivist", "belief_faith_in_rebuild" } },
            // Plan 30 — Grounded post-Exchange belief movements
            { "belief_ash_witnesses",         new List<string> { "belief_rebuilders", "pragmatic_individualism", "atheist_rationalist" } },
            { "belief_rebuilders",            new List<string> { "belief_ash_witnesses", "belief_every_soul_alone", "belief_ash_nihilist" } },
            { "belief_listeners",             new List<string> { "atheist_rationalist", "military_discipline", "belief_every_soul_alone" } }
        };

        public event Action<string, string, float> OnFrictionDetected;
        public event Action<string, string> OnRoommateSynergy;
        public event Action<string, string, float> OnAffinityChanged;
        public event Action OnStateChanged;

        private readonly Dictionary<string, string> _beliefs = new Dictionary<string, string>(StringComparer.Ordinal);
        private readonly Dictionary<string, float> _affinities = new Dictionary<string, float>(StringComparer.Ordinal);

        public void RegisterBelief(string survivorId, string beliefProfileId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            _beliefs[survivorId] = beliefProfileId ?? string.Empty;
        }

        public string GetBelief(string survivorId)
        {
            return _beliefs.TryGetValue(survivorId, out var b) ? b : string.Empty;
        }

        public float GetAffinity(string survivorA, string survivorB)
        {
            return _affinities.TryGetValue(MakePairKey(survivorA, survivorB), out var a) ? a : 0f;
        }

        private static string MakePairKey(string a, string b)
        {
            return string.CompareOrdinal(a, b) <= 0 ? a + "|" + b : b + "|" + a;
        }

        public float GetRoommateCompatibilityMultiplier(string survivorA, string survivorB)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB) || survivorA == survivorB)
                return 1f;

            string beliefA = GetBelief(survivorA);
            string beliefB = GetBelief(survivorB);
            if (string.IsNullOrEmpty(beliefA) || string.IsNullOrEmpty(beliefB))
                return 1f;

            if (string.Equals(beliefA, beliefB, StringComparison.Ordinal))
            {
                OnRoommateSynergy?.Invoke(survivorA, survivorB);
                return 1f + SynergySleepQualityBonus;
            }

            if (AreInConflict(beliefA, beliefB))
            {
                float penalty = 1f - ConflictSleepQualityPenalty;
                OnFrictionDetected?.Invoke(survivorA, survivorB, penalty);
                return penalty;
            }

            return 1f;
        }

        private static bool AreInConflict(string beliefA, string beliefB)
        {
            if (ConflictGroups.TryGetValue(beliefA, out var conflictsA))
                if (conflictsA.Contains(beliefB)) return true;
            if (ConflictGroups.TryGetValue(beliefB, out var conflictsB))
                if (conflictsB.Contains(beliefA)) return true;
            return false;
        }

        public void TickRoommates(string survivorA, string survivorB, float gameHours)
        {
            if (string.IsNullOrEmpty(survivorA) || string.IsNullOrEmpty(survivorB) || survivorA == survivorB)
                return;

            float multiplier = GetRoommateCompatibilityMultiplier(survivorA, survivorB);
            string key = MakePairKey(survivorA, survivorB);

            if (!_affinities.ContainsKey(key))
                _affinities[key] = 0f;

            if (multiplier < 1f)
            {
                float drain = ConflictAffinityDrainPerDay * (gameHours / 24f);
                _affinities[key] -= drain;
                OnAffinityChanged?.Invoke(survivorA, survivorB, -drain);
            }
            else if (multiplier > 1f)
            {
                float gain = SynergyAffinityGainPerDay * (gameHours / 24f);
                _affinities[key] += gain;
                OnAffinityChanged?.Invoke(survivorA, survivorB, gain);
            }
            OnStateChanged?.Invoke();
        }

        public IdeologicalFrictionSaveState CaptureState()
        {
            var save = new IdeologicalFrictionSaveState();
            foreach (var kv in _affinities)
                save.affinities.Add(new IdeologicalAffinityEntry { pairKey = kv.Key, affinity = kv.Value });
            return save;
        }

        public void RestoreState(IdeologicalFrictionSaveState save)
        {
            _affinities.Clear();
            if (save?.affinities == null) return;
            foreach (var e in save.affinities)
                if (e != null && !string.IsNullOrEmpty(e.pairKey))
                    _affinities[e.pairKey] = e.affinity;
        }
    }
}
