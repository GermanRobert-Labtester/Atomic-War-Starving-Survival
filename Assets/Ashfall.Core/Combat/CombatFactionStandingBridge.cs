// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;

namespace Ashfall.Core.Combat
{
    /// <summary>
    /// One deterministic political consequence derived from a completed combat.
    /// The authoritative faction system owns standing; this is a persisted combat fact.
    /// </summary>
    [Serializable]
    public class CombatFactionConsequence
    {
        public string IncidentId { get; set; } = string.Empty;
        public string EncounterId { get; set; } = string.Empty;
        public string FactionId { get; set; } = string.Empty;
        public int Day { get; set; } = 1;
        public int Kills { get; set; }
        public int Downed { get; set; }
        public int Assisted { get; set; }
        public bool IsSelfDefense { get; set; }
        public float StandingDelta { get; set; }
        public string Reason { get; set; } = string.Empty;
        public bool Applied { get; set; }
    }

    [Serializable]
    public sealed class FactionCombatThresholdDef
    {
        public string threshold_id { get; set; } = string.Empty;
        public string label { get; set; } = string.Empty;
        public int min_casualties { get; set; } = 1;
        public int max_casualties { get; set; } = 999;
        public float standing_penalty_multiplier { get; set; } = 1.0f;
        public float self_defense_multiplier { get; set; } = 0.5f;
        public string description { get; set; } = string.Empty;
    }

    [Serializable]
    public sealed class FactionCombatRuleDef
    {
        public string faction_id { get; set; } = string.Empty;
        public int standing_cap_loss_per_encounter { get; set; } = 20;
        public float forgiveness_rate_per_day { get; set; } = 0.5f;
        public int hostile_threshold { get; set; } = -50;
    }

    [Serializable]
    public sealed class FactionCombatThresholdCatalog
    {
        public int schema_version { get; set; } = 1;
        public List<FactionCombatThresholdDef> thresholds { get; set; } = new List<FactionCombatThresholdDef>();
        public List<FactionCombatRuleDef> faction_rules { get; set; } = new List<FactionCombatRuleDef>();
    }

    /// <summary>
    /// Plan 139 / C1[22] — pure combat-outcome projection plus an exactly-once
    /// adapter to one injected standing authority. The bridge owns no standing
    /// ledger; applied incident ids live in <see cref="CombatState"/> so they
    /// round-trip with the encounter that produced them.
    /// </summary>
    public sealed class CombatFactionStandingBridge
    {
        public const float BaseKillPenalty = -5.0f;
        public const float BaseDownedPenalty = -2.5f;
        public const float MaxCasualtyPenalty = -25.0f;
        public const float SelfDefenseMultiplier = 0.5f;
        public const float AssistanceBonus = 5.0f;
        public const float FriendlyFirePenalty = -15.0f;

        private readonly List<FactionCombatThresholdDef> _thresholds = new List<FactionCombatThresholdDef>();
        private readonly List<FactionCombatRuleDef> _factionRules = new List<FactionCombatRuleDef>();

        public event Action<CombatFactionConsequence>? OnConsequenceApplied;

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var catalog = JsonSerializer.Deserialize<FactionCombatThresholdCatalog>(json, options);
                if (catalog == null) return;
                if (catalog.thresholds != null)
                {
                    foreach (var t in catalog.thresholds)
                        if (!string.IsNullOrWhiteSpace(t.threshold_id)) _thresholds.Add(t);
                }
                if (catalog.faction_rules != null)
                {
                    foreach (var r in catalog.faction_rules)
                        if (!string.IsNullOrWhiteSpace(r.faction_id)) _factionRules.Add(r);
                }
            }
            catch (Exception) { /* malformed catalog falls back to built-in defaults; authoring errors are enforced by the data-integrity gate */ }
        }

        public IReadOnlyList<FactionCombatThresholdDef> GetAllThresholds() => _thresholds;
        public IReadOnlyList<FactionCombatRuleDef> GetAllFactionRules() => _factionRules;

        public FactionCombatThresholdDef? GetThresholdForCasualties(int totalCasualties)
        {
            foreach (var t in _thresholds)
                if (totalCasualties >= t.min_casualties && totalCasualties <= t.max_casualties)
                    return t;
            return null;
        }

        public FactionCombatRuleDef? GetFactionRule(string factionId)
            => _factionRules.FirstOrDefault(r =>
                string.Equals(r.faction_id, factionId, StringComparison.OrdinalIgnoreCase));

        public static IReadOnlyList<CombatFactionConsequence> EvaluateConsequences(
            CombatState state,
            bool isSelfDefense = false)
        {
            if (state == null || state.Combatants == null || string.IsNullOrEmpty(state.EncounterId))
                return Array.Empty<CombatFactionConsequence>();

            var enemyCounts = new SortedDictionary<string, (int kills, int downed)>(StringComparer.Ordinal);
            var allyCounts = new SortedDictionary<string, (int kills, int downed)>(StringComparer.Ordinal);
            var presentAlliedFactions = new SortedSet<string>(StringComparer.Ordinal);

            bool playerWon = state.Phase == (int)CombatPhase.Won
                || string.Equals(state.OutcomeText, "Victory", StringComparison.OrdinalIgnoreCase)
                || (state.Aftermath != null
                    && string.Equals(state.Aftermath.Outcome, "Victory", StringComparison.OrdinalIgnoreCase));

            for (int i = 0; i < state.Combatants.Count; i++)
            {
                var combatant = state.Combatants[i];
                if (combatant == null) continue;

                string factionId = (combatant.FactionId ?? string.Empty).Trim();
                if (IsFactionless(factionId)) continue;

                var counts = combatant.IsPlayer ? allyCounts : enemyCounts;
                counts.TryGetValue(factionId, out var current);
                if (combatant.Health <= 0f) current.kills++;
                else if (combatant.IsDowned) current.downed++;
                counts[factionId] = current;
                if (combatant.IsPlayer) presentAlliedFactions.Add(factionId);
            }

            var results = new List<CombatFactionConsequence>();
            foreach (var pair in enemyCounts)
            {
                int kills = pair.Value.kills;
                int downed = pair.Value.downed;
                if (kills == 0 && downed == 0) continue;

                float delta = Math.Max(MaxCasualtyPenalty,
                    (kills * BaseKillPenalty) + (downed * BaseDownedPenalty));
                if (isSelfDefense) delta *= SelfDefenseMultiplier;

                results.Add(new CombatFactionConsequence
                {
                    IncidentId = $"ccon_{state.EncounterId}_{pair.Key}_casualty",
                    EncounterId = state.EncounterId,
                    FactionId = pair.Key,
                    Day = state.Day,
                    Kills = kills,
                    Downed = downed,
                    IsSelfDefense = isSelfDefense,
                    StandingDelta = delta,
                    Reason = isSelfDefense ? "combat_self_defense" : "combat_aggression"
                });
            }

            foreach (var pair in allyCounts)
            {
                if (pair.Value.kills == 0 && pair.Value.downed == 0) continue;
                results.Add(new CombatFactionConsequence
                {
                    IncidentId = $"ccon_{state.EncounterId}_{pair.Key}_friendly_fire",
                    EncounterId = state.EncounterId,
                    FactionId = pair.Key,
                    Day = state.Day,
                    Kills = pair.Value.kills,
                    Downed = pair.Value.downed,
                    IsSelfDefense = isSelfDefense,
                    StandingDelta = FriendlyFirePenalty,
                    Reason = "combat_friendly_fire"
                });
            }

            if (playerWon && enemyCounts.Count > 0)
            {
                foreach (string factionId in presentAlliedFactions)
                {
                    results.Add(new CombatFactionConsequence
                    {
                        IncidentId = $"ccon_{state.EncounterId}_{factionId}_assisted",
                        EncounterId = state.EncounterId,
                        FactionId = factionId,
                        Day = state.Day,
                        Assisted = 1,
                        IsSelfDefense = isSelfDefense,
                        StandingDelta = AssistanceBonus,
                        Reason = "combat_assistance"
                    });
                }
            }

            results.Sort((a, b) => string.CompareOrdinal(a.IncidentId, b.IncidentId));
            return results;
        }

        /// <summary>
        /// Apply each incident once through one canonical integer standing sink.
        /// Missing sink or marker storage fails closed and leaves incidents pending.
        /// </summary>
        public int ApplyConsequences(
            IEnumerable<CombatFactionConsequence> consequences,
            Action<string, int>? applyStanding,
            ICollection<string>? appliedIncidentIds)
        {
            if (consequences == null || applyStanding == null || appliedIncidentIds == null) return 0;

            int appliedCount = 0;
            foreach (var consequence in consequences
                .Where(c => c != null && !string.IsNullOrEmpty(c.IncidentId))
                .OrderBy(c => c.IncidentId, StringComparer.Ordinal))
            {
                if (appliedIncidentIds.Contains(consequence.IncidentId))
                {
                    consequence.Applied = true;
                    continue;
                }

                int delta = (int)Math.Round(consequence.StandingDelta, MidpointRounding.AwayFromZero);
                applyStanding(consequence.FactionId, delta);
                // The canonical standing authority is integer-valued. Persist
                // and present the delta that actually landed, not the pre-round
                // projection (for example -7.5 becoming -8).
                consequence.StandingDelta = delta;
                appliedIncidentIds.Add(consequence.IncidentId);
                consequence.Applied = true;
                appliedCount++;
                OnConsequenceApplied?.Invoke(consequence);
            }

            return appliedCount;
        }

        private static bool IsFactionless(string factionId)
        {
            return string.IsNullOrEmpty(factionId)
                || string.Equals(factionId, "unaligned", StringComparison.OrdinalIgnoreCase)
                || string.Equals(factionId, "faction_unaligned", StringComparison.OrdinalIgnoreCase);
        }
    }
}
