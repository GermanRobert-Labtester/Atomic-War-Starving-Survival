using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Ideological Friction System — differing survivor worldviews create
    /// interpersonal friction when sharing quarters, affecting sleep quality.
    ///
    /// Compares BeliefProfileIds of roommates to determine compatibility.
    /// Owns: Survivor.BeliefProfileId (read-only by this system).
    /// </summary>
    public class IdeologicalFrictionSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float ConflictSleepQualityPenalty = 0.20f;
        public const float SynergySleepQualityBonus = 0.10f;
        public const float ConflictAffinityDrainPerDay = 2f;
        public const float SynergyAffinityGainPerDay = 1f;

        // ── Conflict groups (belief profiles that clash) ───────────────
        public static readonly Dictionary<string, List<string>> ConflictGroups =
            new Dictionary<string, List<string>>
            {
                { "military_discipline", new List<string> {
                    "pragmatic_individualism", "pacifist", "religious_faith" } },
                { "religious_faith", new List<string> {
                    "atheist_rationalist", "military_discipline" } },
                { "atheist_rationalist", new List<string> {
                    "religious_faith", "superstitious_traditional" } },
                { "pragmatic_individualism", new List<string> {
                    "collectivist_solidarity", "military_discipline" } },
                { "collectivist_solidarity", new List<string> {
                    "pragmatic_individualism" } },
                { "superstitious_traditional", new List<string> {
                    "atheist_rationalist" } },
                { "pacifist", new List<string> {
                    "military_discipline" } }
            };

        // ── Events ─────────────────────────────────────────────────────
        public event Action<Survivor, Survivor, float> OnFrictionDetected;
        // a, b, sleepPenalty
        public event Action<Survivor, Survivor> OnRoommateSynergy;

        // ── Host hooks ─────────────────────────────────────────────────
        public Action<string, string, float> AdjustAffinity;
        public Func<Survivor, Survivor, bool> AreSharingRoom;
        public System.Random Rng;

        /// <summary>
        /// Check for ideological friction between two survivors sharing a room.
        /// Returns the sleep quality multiplier (1.0 = neutral, <1.0 = penalty).
        /// </summary>
        public float GetRoommateCompatibilityMultiplier(Survivor a, Survivor b)
        {
            if (a == null || b == null || a == b) return 1f;
            if (string.IsNullOrEmpty(a.BeliefProfileId) ||
                string.IsNullOrEmpty(b.BeliefProfileId))
                return 1f;

            // Same belief = synergy
            if (string.Equals(a.BeliefProfileId, b.BeliefProfileId,
                StringComparison.OrdinalIgnoreCase))
            {
                OnRoommateSynergy?.Invoke(a, b);
                return 1f + SynergySleepQualityBonus;
            }

            // Check conflict groups
            if (ConflictGroups.TryGetValue(a.BeliefProfileId, out var conflicts))
            {
                if (conflicts.Contains(b.BeliefProfileId))
                {
                    float penalty = 1f - ConflictSleepQualityPenalty;
                    OnFrictionDetected?.Invoke(a, b, penalty);
                    return penalty;
                }
            }

            if (ConflictGroups.TryGetValue(b.BeliefProfileId, out var conflictsB))
            {
                if (conflictsB.Contains(a.BeliefProfileId))
                {
                    float penalty = 1f - ConflictSleepQualityPenalty;
                    OnFrictionDetected?.Invoke(a, b, penalty);
                    return penalty;
                }
            }

            return 1f; // neutral
        }

        /// <summary>
        /// Tick — apply passive affinity drain/gain for roommates.
        /// </summary>
        public void TickRoommates(Survivor a, Survivor b, float gameHours)
        {
            if (a == null || b == null || a == b) return;
            float multiplier = GetRoommateCompatibilityMultiplier(a, b);

            if (multiplier < 1f)
            {
                // Conflict: drain affinity
                float drain = ConflictAffinityDrainPerDay * (gameHours / 24f);
                AdjustAffinity?.Invoke(a.Id, b.Id, -drain);
            }
            else if (multiplier > 1f)
            {
                // Synergy: gain affinity
                float gain = SynergyAffinityGainPerDay * (gameHours / 24f);
                AdjustAffinity?.Invoke(a.Id, b.Id, gain);
            }
        }
    }
}
