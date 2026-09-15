// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 175 — ZealotrySystem (fictional ideological pressure layer).
// ALL belief content is fictional, setting-specific, and mechanically
// symmetrical (§1.6): no real-world faiths, no moral asymmetry, no shorthand
// where belief = violence. Models social influence, doctrinal cohesion,
// charismatic authority, conformity, and extremism in wasteland terms only.
//
// Authority split (one authority per concern):
//   • belief_movements.json / SpiritualCatalog — belief DEFINITIONS (creed,
//     themes). wasteland_religions.json adds MECHANICAL profiles keyed to them.
//   • IdeologicalFrictionSystem — interpersonal friction + conflict-group map.
//     Zealotry CONSUMES its groups; never duplicates them.
//   • NeedsSystem / MoraleContagionSystem — morale & despair truth. Zealotry
//     only REPORTS bounded resistance modifiers (§7.8: never bypasses morale).
//   • SpiritualMeaningCoordinator — ritual cooldown/mourning authority.
//   • Construction/room authority — shrine rooms (host registers influence).
//   • Inventory — ritual offering quantities (host fulfills through the port).
//   • Combat/encounter authority — lethal violence. The escalation ladder
//     STOPS at a typed threat event; Core never resolves a killing (§7.11).
// Determinism: conversion is a pure function of context + authored profile +
// the host-forked rng. Fervor/conviction/dissent are pure state arithmetic.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using Ashfall.Core.IO;

namespace Ashfall.Core.Survivors
{
    /// <summary>Bounded design constants (§1.5).</summary>
    public static class ZealotryCaps
    {
        public const int MaxConviction = 100;
        public const int MaxFervor = 100;
        public const int MaxDissent = 100;
        /// <summary>Hard cap on despair-resistance modifiers (bp of morale scale).</summary>
        public const int MaxDespairResistanceBp = 2000;
        /// <summary>Hard cap on group cohesion/work-persistence buffs (bp).</summary>
        public const int MaxCohesionBonusBp = 1500;
        /// <summary>Fervor above which dissent pressure grows daily (§7.6-7.7).</summary>
        public const int DissentGrowthFervorThreshold = 70;
        /// <summary>Escalation ladder: one stage per this many days of sustained tension.</summary>
        public const int EscalationIntervalDays = 4;
    }

    public enum ZealotEscalationStage
    {
        None = 0,
        Argument = 1,
        Ostracism = 2,
        WorkRefusal = 3,
        PropertyDamage = 4,
        AssaultThreat = 5,   // host MUST route through the combat/encounter authority (§7.11)
        Schism = 6
    }

    public enum BeliefRole
    {
        None = 0,
        Adherent = 1,
        Devout = 2,
        Leader = 3
    }

    /// <summary>One authored mechanical profile over an authored belief movement.</summary>
    [Serializable]
    public sealed class ZealotryBeliefProfile
    {
        public string belief_id { get; set; } = string.Empty;      // FK belief_movements.json
        public string display_name { get; set; } = string.Empty;
        public List<string> doctrine_tags { get; set; } = new List<string>();
        public int conversion_base_bp { get; set; }                // 0..3000 base conversion chance
        public int fervor_daily_decay_bp { get; set; }             // 0..2000 bp/day
        public int fervor_ritual_gain_bp { get; set; }             // 0..3000 per held ritual
        public int cohesion_bonus_bp { get; set; }                 // 0..MaxCohesionBonusBp
        public int despair_resistance_bp { get; set; }             // 0..MaxDespairResistanceBp
        public int fanaticism_threshold { get; set; }              // 0..100 fervor
        public string dissent_tolerance { get; set; } = "medium";  // low|medium|high
        public List<string> ritual_resource_item_ids { get; set; } = new List<string>();
        public List<string> shrine_room_tags { get; set; } = new List<string>();
        public string broadcast_profile { get; set; } = "neutral"; // receptive|neutral|hostile
        public List<string> tags { get; set; } = new List<string>();
    }

    [Serializable]
    public sealed class ZealotryCatalogRoot
    {
        public int schema_version { get; set; } = 1;
        public List<ZealotryBeliefProfile> religions { get; set; } = new List<ZealotryBeliefProfile>();
    }

    public sealed class ZealotryCatalogLoadResult
    {
        public List<ZealotryBeliefProfile> Religions { get; } = new List<ZealotryBeliefProfile>();
        public List<string> Errors { get; } = new List<string>();
        public bool HasErrors => Errors.Count > 0;
    }

    [Serializable]
    public sealed class SurvivorBeliefState
    {
        public string survivor_id { get; set; } = string.Empty;
        public string belief_id { get; set; } = string.Empty;      // "" = no belief
        public int conviction { get; set; }                        // 0..100 adherence strength
        public int fervor { get; set; }                            // 0..100 emotional intensity
        public int converted_day { get; set; } = -1;
        public int dissent { get; set; }                           // 0..100 internal doubt/hostility
        public int role { get; set; } = (int)BeliefRole.Adherent;
        public bool in_crisis { get; set; }                        // shattered-belief state (§7.8)
    }

    [Serializable]
    public sealed class ZealotrySystemState
    {
        public string system_id { get; set; } = "zealotry";
        public int schema_version { get; set; } = 1;
        public int last_tick_day { get; set; }
        public List<SurvivorBeliefState> believers { get; set; } = new List<SurvivorBeliefState>();
        public List<string> shrine_belief_ids { get; set; } = new List<string>();
        public int escalation_stage { get; set; } = 0;             // (int) ZealotEscalationStage
        public int escalation_progress_days { get; set; }
        public int last_ritual_day { get; set; } = -1;
        public List<string> unresolved_demands { get; set; } = new List<string>();
    }

    /// <summary>Conversion context the host computes from the real authorities
    /// (NeedsSystem stress, LeadershipSystem charisma, construction shrines,
    /// relationships). All inputs are 0..1 abstract bands (§7.4).</summary>
    [Serializable]
    public sealed class ConversionContext
    {
        public float Stress01;            // survivor stress/vulnerability
        public float LeaderCharisma01;    // leader influence reach
        public float ShrineInfluence01;   // authored shrine presence
        public float RelationshipBond01;  // bond to an existing believer
        public bool LeaderOfSameBelief;   // leader is actively recruiting
    }

    /// <summary>Conversion outcome (domain result, no exceptions).</summary>
    public sealed class ConversionResult
    {
        public bool Converted;
        public string ReasonCode = string.Empty;   // already_member | resisted | unknown_belief | converted
        public SurvivorBeliefState? Belief;
    }

    /// <summary>Bounded ritual-demand payload (§7.10): resource offerings only —
    /// NEVER a survivor-sacrifice transaction. Life-threatening cult content
    /// routes through the narrative/combat authorities with explicit choice.</summary>
    public sealed class RitualDemand
    {
        public string BeliefId = string.Empty;
        public List<string> ItemIds = new List<string>();
        public int Day;
    }

    /// <summary>
    /// Fictional ideological pressure authority: conversion, fervor, conviction,
    /// dissent, charismatic leaders, shrine influence, bounded buffs with costs,
    /// ritual resource demands, and the social escalation ladder.
    /// </summary>
    public sealed class ZealotrySystem
    {
        public const string SystemId = "zealotry";

        private readonly Dictionary<string, ZealotryBeliefProfile> _profiles =
            new Dictionary<string, ZealotryBeliefProfile>(StringComparer.Ordinal);
        private ZealotrySystemState _state = new ZealotrySystemState();

        public event Action<SurvivorBeliefState, string>? OnConverted;              // believer, beliefId
        public event Action<SurvivorBeliefState>? OnConversionResisted;
        public event Action<SurvivorBeliefState, int>? OnFervorChanged;
        public event Action<SurvivorBeliefState>? OnCrisisStarted;                  // shattered belief (§7.8)
        public event Action<SurvivorBeliefState>? OnCrisisResolved;
        public event Action<RitualDemand>? OnRitualDemanded;
        public event Action<string>? OnRitualFulfilled;
        public event Action<string>? OnRitualFailed;                               // resources missing (§16)
        public event Action<ZealotEscalationStage>? OnEscalationStageChanged;
        public event Action<string, string>? OnLeaderRegistered;                   // survivorId, beliefId

        public ZealotrySystemState State => _state;

        public ZealotrySystem(IEnumerable<ZealotryBeliefProfile>? profiles = null)
        {
            if (profiles != null)
                foreach (var p in profiles)
                    if (p != null && !string.IsNullOrEmpty(p.belief_id)) _profiles[p.belief_id] = p;
        }

        public ZealotryBeliefProfile? Profile(string beliefId) =>
            !string.IsNullOrEmpty(beliefId) && _profiles.TryGetValue(beliefId, out var p) ? p : null;

        public SurvivorBeliefState? Believer(string survivorId) =>
            _state.believers.FirstOrDefault(b => b != null && string.Equals(b.survivor_id, survivorId, StringComparison.Ordinal));

        // ── Charismatic leader (§7.5) ──────────────────────────────────

        /// <summary>Register the charismatic authority for a belief. The HOST
        /// derives candidacy from LeadershipSystem charisma/conviction; Core
        /// stores the role and applies its influence in conversion/rituals.</summary>
        public bool RegisterLeader(string survivorId, string beliefId)
        {
            if (string.IsNullOrEmpty(survivorId) || Profile(beliefId) == null) return false;
            var b = Believer(survivorId);
            if (b == null)
            {
                b = new SurvivorBeliefState
                {
                    survivor_id = survivorId,
                    belief_id = beliefId,
                    conviction = 70,
                    fervor = 60,
                    converted_day = _state.last_tick_day,
                    role = (int)BeliefRole.Leader
                };
                _state.believers.Add(b);
            }
            else
            {
                b.role = (int)BeliefRole.Leader;
            }
            OnLeaderRegistered?.Invoke(survivorId, beliefId);
            return true;
        }

        // ── Shrine influence (§7.9 — routed from construction) ─────────

        /// <summary>Host registers a shrine presence for a belief (room built
        /// through the construction authority). Influence is a state flag, not
        /// a free buff — the room cost/space stays with construction.</summary>
        public void SetShrine(string beliefId, bool present)
        {
            if (Profile(beliefId) == null) return;
            bool has = _state.shrine_belief_ids.Contains(beliefId);
            if (present && !has) _state.shrine_belief_ids.Add(beliefId);
            else if (!present && has) _state.shrine_belief_ids.Remove(beliefId);
        }

        public bool HasShrine(string beliefId) => _state.shrine_belief_ids.Contains(beliefId);

        // ── Conversion (§7.4) ──────────────────────────────────────────

        /// <summary>
        /// Deterministic conversion attempt. Chance = base × leader factor ×
        /// vulnerability × shrine/bond factors × resistance factor. Resistance
        /// EXISTS: an existing believer of another faith resists hard, and a
        /// roll below the threshold leaves the target unchanged (vulnerable ≠
        /// automatic target). Same-belief attempts deepen conviction instead.
        /// </summary>
        public ConversionResult TryConvert(string survivorId, string beliefId, int day, ConversionContext ctx, ISeededRng? rng)
        {
            var profile = Profile(beliefId);
            if (profile == null) return new ConversionResult { Converted = false, ReasonCode = "unknown_belief" };
            if (ctx == null) return new ConversionResult { Converted = false, ReasonCode = "unknown_belief" };

            var existing = Believer(survivorId);
            if (existing != null && string.Equals(existing.belief_id, beliefId, StringComparison.Ordinal))
            {
                // Deepen conviction for an existing member; never double-convert.
                existing.conviction = Math.Min(ZealotryCaps.MaxConviction, existing.conviction + 5);
                existing.fervor = Math.Min(ZealotryCaps.MaxFervor, existing.fervor + 3);
                return new ConversionResult { Converted = false, ReasonCode = "already_member", Belief = existing };
            }

            float leaderFactor = 0.5f + ctx.LeaderCharisma01 * (ctx.LeaderOfSameBelief ? 1.0f : 0.3f);
            float vulnerability = 0.6f + ctx.Stress01;                      // stressed, never auto
            float pullFactor = 0.5f + 0.5f * Math.Max(ctx.ShrineInfluence01, ctx.RelationshipBond01);
            // Resistance: a committed member of ANOTHER belief resists hard (agency).
            float resistanceFactor = existing != null ? Math.Max(0.25f, 1f - existing.conviction / 200f) : 1f;

            double p = profile.conversion_base_bp / 10000.0
                      * leaderFactor * vulnerability * pullFactor * resistanceFactor;
            p = Math.Clamp(p, 0.0, 0.9);

            if (rng == null || rng.NextDouble() >= p)
            {
                if (existing != null) OnConversionResisted?.Invoke(existing);
                return new ConversionResult { Converted = false, ReasonCode = "resisted", Belief = existing };
            }

            if (existing != null)
            {
                // Conversion REPLACES the old affiliation (schism-shaped change).
                existing.belief_id = beliefId;
                existing.conviction = Math.Clamp(30 + (int)(ctx.Stress01 * 20), 0, ZealotryCaps.MaxConviction);
                existing.fervor = 45;
                existing.converted_day = day;
                existing.in_crisis = false;
            }
            else
            {
                existing = new SurvivorBeliefState
                {
                    survivor_id = survivorId,
                    belief_id = beliefId,
                    conviction = Math.Clamp(30 + (int)(ctx.Stress01 * 20), 0, ZealotryCaps.MaxConviction),
                    fervor = 45,
                    converted_day = day
                };
                _state.believers.Add(existing);
            }
            OnConverted?.Invoke(existing, beliefId);
            return new ConversionResult { Converted = true, ReasonCode = "converted", Belief = existing };
        }

        // ── Rituals & demands (§7.10) ──────────────────────────────────

        /// <summary>A held ritual raises fervor/conviction for adherents of the
        /// belief (bounded by the authored profile). The cooldown/legitimacy of
        /// the ritual itself stays with the SpiritualMeaningCoordinator.</summary>
        public void RecordRitual(string beliefId, int day)
        {
            var profile = Profile(beliefId);
            if (profile == null) return;
            foreach (var b in _state.believers)
            {
                if (b == null || !string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)) continue;
                int gain = Math.Max(1, profile.fervor_ritual_gain_bp / 100);
                if (b.role == (int)BeliefRole.Leader) gain += 2;
                b.fervor = Math.Min(ZealotryCaps.MaxFervor, b.fervor + gain);
                b.conviction = Math.Min(ZealotryCaps.MaxConviction, b.conviction + 2);
                OnFervorChanged?.Invoke(b, b.fervor);
            }
            _state.last_ritual_day = day;
        }

        /// <summary>Emit the bounded resource demand for the next ritual (§7.10).
        /// The HOST fulfills via the canonical inventory — this system never
        /// touches item quantities itself.</summary>
        public RitualDemand? EmitRitualDemand(string beliefId, int day)
        {
            var profile = Profile(beliefId);
            if (profile == null) return null;
            if (_state.last_ritual_day >= 0 && day - _state.last_ritual_day < 3)
                return null; // bounded cadence
            var demand = new RitualDemand { BeliefId = beliefId, ItemIds = new List<string>(profile.ritual_resource_item_ids), Day = day };
            if (profile.ritual_resource_item_ids.Count > 0 && !_state.unresolved_demands.Contains(beliefId))
                _state.unresolved_demands.Add(beliefId);
            OnRitualDemanded?.Invoke(demand);
            return demand;
        }

        /// <summary>Host reports the offering was consumed (real inventory).
        /// Missing resources block the ritual and cost a little fervor (§16).</summary>
        public void ResolveRitualDemand(string beliefId, int day, bool resourcesAvailable)
        {
            if (!_state.unresolved_demands.Contains(beliefId)) return;
            _state.unresolved_demands.Remove(beliefId);
            if (resourcesAvailable)
            {
                RecordRitual(beliefId, day);
                OnRitualFulfilled?.Invoke(beliefId);
            }
            else
            {
                // Failed ritual: modest fervor penalty, no free buff (§16).
                foreach (var b in _state.believers)
                {
                    if (b == null || !string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)) continue;
                    b.fervor = Math.Max(0, b.fervor - 10);
                    OnFervorChanged?.Invoke(b, b.fervor);
                }
                OnRitualFailed?.Invoke(beliefId);
            }
        }

        // ── Broadcast (§7.13 — PsyOps route) ───────────────────────────

        /// <summary>Broadcast reach raises fervor/conviction of existing
        /// adherents (capped); it never mind-controls anyone into converting.</summary>
        public void ApplyBroadcast(string beliefId, float reach01)
        {
            var profile = Profile(beliefId);
            if (profile == null) return;
            reach01 = Math.Clamp(reach01, 0f, 1f);
            int gain = profile.broadcast_profile == "receptive"
                ? (int)Math.Round(reach01 * 12)
                : profile.broadcast_profile == "hostile"
                    ? -(int)Math.Round(reach01 * 8)
                    : (int)Math.Round(reach01 * 5);
            foreach (var b in _state.believers)
            {
                if (b == null || !string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)) continue;
                b.fervor = Math.Clamp(b.fervor + gain, 0, ZealotryCaps.MaxFervor);
                if (gain > 0) b.conviction = Math.Min(ZealotryCaps.MaxConviction, b.conviction + gain / 3);
                OnFervorChanged?.Invoke(b, b.fervor);
            }
        }

        // ── Crisis (§7.8) ──────────────────────────────────────────────

        /// <summary>Shattered belief: fervor collapses, conviction shocks down,
        /// and the crisis flag opens. The MORALE DAMAGE is applied by the HOST
        /// through the canonical morale authority — this system never writes it.</summary>
        public void TriggerCrisis(string beliefId, int day)
        {
            var profile = Profile(beliefId);
            if (profile == null) return;
            foreach (var b in _state.believers)
            {
                if (b == null || !string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)) continue;
                if (b.in_crisis) continue;
                b.in_crisis = true;
                b.fervor = Math.Max(0, b.fervor / 4);
                b.conviction = Math.Max(0, b.conviction / 3);
                OnCrisisStarted?.Invoke(b);
            }
            _state.last_ritual_day = day;
        }

        /// <summary>Recovery from a shattered belief is slow and partial.</summary>
        public void ResolveCrisis(string beliefId)
        {
            foreach (var b in _state.believers)
            {
                if (b == null || !b.in_crisis || !string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)) continue;
                b.in_crisis = false;
                b.conviction = Math.Max(0, b.conviction + 20);
                OnCrisisResolved?.Invoke(b);
            }
        }

        // ── Bounded buff queries (§7.7-7.8: modify, never bypass) ─────

        /// <summary>Despair-resistance modifier (bp) — the MORALE AUTHORITY
        /// applies it; zealotry never bypasses despair. Scales with conviction,
        /// fervor, and shrine presence; crisis and dissent erode it.</summary>
        public int GetDespairResistanceBp(string survivorId)
        {
            var b = Believer(survivorId);
            if (b == null) return 0;
            var profile = Profile(b.belief_id);
            if (profile == null || b.in_crisis) return 0;
            float convictionFactor = 0.4f + 0.6f * (b.conviction / 100f);
            float fervorFactor = 0.6f + 0.4f * (b.fervor / 100f);
            float shrineFactor = HasShrine(b.belief_id) ? 1.1f : 1.0f;
            float dissentPenalty = 1f - b.dissent / 200f;
            return (int)Math.Clamp(profile.despair_resistance_bp * convictionFactor * fervorFactor * shrineFactor * dissentPenalty, 0, ZealotryCaps.MaxDespairResistanceBp);
        }

        /// <summary>Group cohesion/work-persistence modifier (bp) — the HOST
        /// applies through the duty/work seams; never a raw stat bypass.</summary>
        public int GetCohesionBonusBp(string beliefId)
        {
            var profile = Profile(beliefId);
            if (profile == null) return 0;
            var members = _state.believers.Where(b => b != null && string.Equals(b.belief_id, beliefId, StringComparison.Ordinal)).ToList();
            if (members.Count == 0) return 0;
            float avgConviction = (float)members.Average(b => b.conviction) / 100f;
            float shrineFactor = HasShrine(beliefId) ? 1.15f : 1f;
            return (int)Math.Clamp(profile.cohesion_bonus_bp * avgConviction * shrineFactor, 0f, ZealotryCaps.MaxCohesionBonusBp);
        }

        // ── Daily tick (§7.6-7.7, §7.11) ───────────────────────────────

        /// <summary>
        /// One zealotry day: fervor decay → dissent growth above the fanaticism
        /// threshold → conflict escalation over the friction groups. Stages
        /// advance ONLY while opposing fervent believers coexist; the ladder
        /// stops at AssaultThreat (host routes any real violence).
        /// </summary>
        public void TickDay(int day)
        {
            if (_state.last_tick_day == day) return;
            _state.last_tick_day = day;

            var ordered = _state.believers.Where(b => b != null)
                .OrderBy(b => b.survivor_id, StringComparer.Ordinal).ToList();
            foreach (var b in ordered)
            {
                var profile = Profile(b.belief_id);
                if (profile == null) continue;

                // Fervor decays daily (authored rate).
                int decay = Math.Max(1, profile.fervor_daily_decay_bp / 100);
                int old = b.fervor;
                b.fervor = Math.Max(0, b.fervor - decay);
                if (b.fervor != old) OnFervorChanged?.Invoke(b, b.fervor);

                // Dissent grows above the fanaticism threshold (§7.7 costs).
                if (b.fervor >= profile.fanaticism_threshold
                    && profile.dissent_tolerance != "high")
                {
                    int growth = profile.dissent_tolerance == "low" ? 3 : 2;
                    b.dissent = Math.Min(ZealotryCaps.MaxDissent, b.dissent + growth);
                }
                else if (b.dissent > 0)
                {
                    b.dissent = Math.Max(0, b.dissent - 1);
                }

                // In-crisis believers slowly recover conviction.
                if (b.in_crisis && b.conviction < 20)
                    b.conviction = Math.Min(20, b.conviction + 1);
            }

            // Conflict escalation over the friction authority's groups (§7.11).
            var stage = (ZealotEscalationStage)_state.escalation_stage;
            bool tension = HasOpposingFerventPair();
            if (tension && stage < ZealotEscalationStage.AssaultThreat)
            {
                _state.escalation_progress_days++;
                if (_state.escalation_progress_days >= ZealotryCaps.EscalationIntervalDays
                    && stage < ZealotEscalationStage.Schism)
                {
                    _state.escalation_progress_days = 0;
                    var next = (ZealotEscalationStage)Math.Min((int)stage + 1, (int)ZealotEscalationStage.AssaultThreat);
                    _state.escalation_stage = (int)next;
                    OnEscalationStageChanged?.Invoke(next);
                }
            }
            else if (!tension && stage != ZealotEscalationStage.None)
            {
                // Tension resolved: the ladder de-escalates one stage per day.
                _state.escalation_progress_days = 0;
                var prev = (ZealotEscalationStage)Math.Max(0, (int)stage - 1);
                _state.escalation_stage = (int)prev;
                OnEscalationStageChanged?.Invoke(prev);
            }
        }

        /// <summary>True when two believers of FRICTION-CONFLICTING beliefs are
        /// both above their fanaticism thresholds (§7.11 conditions).</summary>
        public bool HasOpposingFerventPair()
        {
            var fervent = _state.believers
                .Where(b => b != null && !string.IsNullOrEmpty(b.belief_id))
                .Where(b => Profile(b.belief_id) is { } p && b.fervor >= p.fanaticism_threshold)
                .Select(b => b.belief_id)
                .Distinct(StringComparer.Ordinal)
                .ToList();
            if (fervent.Count < 2) return false;
            for (int i = 0; i < fervent.Count; i++)
                for (int j = i + 1; j < fervent.Count; j++)
                    if (IdeologicalFrictionSystem.ConflictGroups.TryGetValue(fervent[i], out var enemies)
                        && enemies.Contains(fervent[j]))
                        return true;
            return false;
        }

        // ── Save (§7.17) ───────────────────────────────────────────────

        public ZealotrySystemState CaptureState()
        {
            var copy = new ZealotrySystemState
            {
                last_tick_day = _state.last_tick_day,
                escalation_stage = _state.escalation_stage,
                escalation_progress_days = _state.escalation_progress_days,
                last_ritual_day = _state.last_ritual_day,
                shrine_belief_ids = new List<string>(_state.shrine_belief_ids),
                unresolved_demands = new List<string>(_state.unresolved_demands),
                believers = new List<SurvivorBeliefState>(_state.believers.Count)
            };
            foreach (var b in _state.believers)
            {
                if (b == null) continue;
                copy.believers.Add(new SurvivorBeliefState
                {
                    survivor_id = b.survivor_id, belief_id = b.belief_id,
                    conviction = b.conviction, fervor = b.fervor,
                    converted_day = b.converted_day, dissent = b.dissent,
                    role = b.role, in_crisis = b.in_crisis
                });
            }
            return copy;
        }

        public void RestoreState(ZealotrySystemState? state)
        {
            if (state == null) return;
            _state = new ZealotrySystemState
            {
                last_tick_day = state.last_tick_day,
                escalation_stage = state.escalation_stage,
                escalation_progress_days = state.escalation_progress_days,
                last_ritual_day = state.last_ritual_day,
                shrine_belief_ids = new List<string>(state.shrine_belief_ids ?? new List<string>()),
                unresolved_demands = new List<string>(state.unresolved_demands ?? new List<string>()),
                believers = new List<SurvivorBeliefState>(state.believers?.Count ?? 0)
            };
            if (state.believers != null)
                foreach (var b in state.believers)
                    if (b != null)
                        _state.believers.Add(new SurvivorBeliefState
                        {
                            survivor_id = b.survivor_id, belief_id = b.belief_id,
                            conviction = b.conviction, fervor = b.fervor,
                            converted_day = b.converted_day, dissent = b.dissent,
                            role = b.role, in_crisis = b.in_crisis
                        });
        }
    }

    // ── Strict catalog loader (repo pattern: schema envelope, snake_case,
    //    collected errors, never throws) ──────────────────────────────────

    public static class ZealotryCatalogLoader
    {
        public const string FileName = "wasteland_religions.json";
        public const int CurrentSchemaVersion = 1;

        public static readonly IReadOnlyList<string> AcceptedTolerance = new[] { "low", "medium", "high" };
        public static readonly IReadOnlyList<string> AcceptedBroadcast = new[] { "receptive", "neutral", "hostile" };

        public const int MaxConversionBaseBp = 3000;
        public const int MaxFervorDecayBp = 2000;
        public const int MaxRitualGainBp = 3000;

        private static readonly Regex SnakeCase = new Regex("^[a-z0-9_]+$", RegexOptions.Compiled);

        public static ZealotryCatalogLoadResult Load(string dataDir, IFileIO fileIO, IJsonSerializer json)
        {
            var result = new ZealotryCatalogLoadResult();
            if (fileIO == null || json == null || string.IsNullOrEmpty(dataDir))
            {
                result.Errors.Add("loader requires dataDir, IFileIO and IJsonSerializer");
                return result;
            }

            string path = fileIO.Combine(dataDir, FileName);
            if (!fileIO.FileExists(path))
            {
                result.Errors.Add("catalog file missing: " + FileName);
                return result;
            }

            string raw = fileIO.ReadAllText(path);
            if (string.IsNullOrWhiteSpace(raw))
            {
                result.Errors.Add("catalog file empty: " + FileName);
                return result;
            }

            ZealotryCatalogRoot root;
            try
            {
                root = json.Deserialize<ZealotryCatalogRoot>(raw);
            }
            catch (Exception e)
            {
                result.Errors.Add("catalog malformed JSON: " + e.Message);
                return result;
            }
            if (root == null)
            {
                result.Errors.Add("catalog parsed to null");
                return result;
            }
            if (root.schema_version > CurrentSchemaVersion)
            {
                result.Errors.Add($"schema_version {root.schema_version} newer than supported {CurrentSchemaVersion}");
                return result;
            }
            if (root.schema_version < 1)
            {
                result.Errors.Add($"schema_version {root.schema_version} invalid");
                return result;
            }
            if (root.religions == null || root.religions.Count == 0)
            {
                result.Errors.Add("catalog has no religion rows");
                return result;
            }

            var seenIds = new HashSet<string>(StringComparer.Ordinal);
            for (int i = 0; i < root.religions.Count; i++)
            {
                var def = root.religions[i];
                string at = $"religions[{i}]";
                if (def == null)
                {
                    result.Errors.Add(at + ": null row");
                    continue;
                }

                if (string.IsNullOrEmpty(def.belief_id) || !SnakeCase.IsMatch(def.belief_id)
                    || !def.belief_id.StartsWith("belief_", StringComparison.Ordinal))
                {
                    result.Errors.Add(at + ": belief_id must be snake_case with belief_ prefix");
                    continue;
                }
                if (!seenIds.Add(def.belief_id))
                {
                    result.Errors.Add(at + ": duplicate belief_id " + def.belief_id);
                    continue;
                }
                if (string.IsNullOrWhiteSpace(def.display_name))
                    result.Errors.Add(at + " (" + def.belief_id + "): display_name required");
                if (!AcceptedTolerance.Contains(def.dissent_tolerance))
                    result.Errors.Add(at + " (" + def.belief_id + "): dissent_tolerance must be low|medium|high");
                if (!AcceptedBroadcast.Contains(def.broadcast_profile))
                    result.Errors.Add(at + " (" + def.belief_id + "): broadcast_profile must be receptive|neutral|hostile");

                if (def.conversion_base_bp < 0 || def.conversion_base_bp > MaxConversionBaseBp)
                    result.Errors.Add(at + " (" + def.belief_id + "): conversion_base_bp out of range [0," + MaxConversionBaseBp + "]");
                if (def.fervor_daily_decay_bp < 0 || def.fervor_daily_decay_bp > MaxFervorDecayBp)
                    result.Errors.Add(at + " (" + def.belief_id + "): fervor_daily_decay_bp out of range");
                if (def.fervor_ritual_gain_bp < 0 || def.fervor_ritual_gain_bp > MaxRitualGainBp)
                    result.Errors.Add(at + " (" + def.belief_id + "): fervor_ritual_gain_bp out of range");
                if (def.cohesion_bonus_bp < 0 || def.cohesion_bonus_bp > ZealotryCaps.MaxCohesionBonusBp)
                    result.Errors.Add(at + " (" + def.belief_id + "): cohesion_bonus_bp out of range");
                if (def.despair_resistance_bp < 0 || def.despair_resistance_bp > ZealotryCaps.MaxDespairResistanceBp)
                    result.Errors.Add(at + " (" + def.belief_id + "): despair_resistance_bp out of range");
                if (def.fanaticism_threshold < 0 || def.fanaticism_threshold > 100)
                    result.Errors.Add(at + " (" + def.belief_id + "): fanaticism_threshold out of range [0,100]");

                if (def.ritual_resource_item_ids == null || def.ritual_resource_item_ids.Count == 0)
                    result.Errors.Add(at + " (" + def.belief_id + "): ritual_resource_item_ids required");

                // Fictional-only guard (§1.6): every row must carry the
                // fictional marker so a real-world label can never slip in.
                if (def.tags == null || !def.tags.Contains("fictional"))
                    result.Errors.Add(at + " (" + def.belief_id + "): rows must be tagged fictional (setting-specific belief)");

                result.Religions.Add(def);
            }

            return result;
        }

        /// <summary>Index a successful load result into runtime profiles.</summary>
        public static List<ZealotryBeliefProfile> ToProfiles(ZealotryCatalogLoadResult result)
        {
            var list = new List<ZealotryBeliefProfile>();
            if (result == null) return list;
            foreach (var def in result.Religions)
                if (def != null && !string.IsNullOrEmpty(def.belief_id)) list.Add(def);
            return list;
        }
    }
}
