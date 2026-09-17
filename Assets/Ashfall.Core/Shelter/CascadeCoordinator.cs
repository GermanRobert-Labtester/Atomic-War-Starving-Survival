// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Shelter
{
    /// <summary>
    /// C2[6] 23C — the facts a cascade rule may read. Every field is owned by an
    /// existing subsystem and supplied by the host; the coordinator owns none of
    /// them and never writes them back.
    /// </summary>
    public sealed class CascadeFacts
    {
        public int Day;

        // Power / pumping
        public bool PowerDeficit;
        public bool CriticalDeficit;
        public bool PumpingUnserved;
        public bool SumpRising;

        // Environment
        public bool HeatingUnserved;
        public bool FiltrationUnserved;
        public bool FalloutStorm;
        public bool ColdStorageUnpowered;
        public bool Darkness;

        // Shelter hazards
        public bool OutbreakActive;
        public bool FireActive;
        public bool HatchUnsealed;

        /// <summary>Off-ramp keys the host reports as currently engaged (e.g. a
        /// manual drain was ordered this day). Empty is normal.</summary>
        public HashSet<string> SatisfiedOffRamps = new HashSet<string>(StringComparer.Ordinal);
    }

    /// <summary>One active cascade rule as reported to the player surfaces.</summary>
    public readonly struct CascadeActiveStage
    {
        public string RuleId { get; init; }
        public string DisplayName { get; init; }
        public IReadOnlyList<string> EffectTags { get; init; }
        public IReadOnlyList<string> OffRamps { get; init; }
        public float WarningHours { get; init; }
    }

    /// <summary>A rule transition this day (exactly-once per transition).</summary>
    public readonly struct CascadeEvent
    {
        public int Day { get; init; }
        public string RuleId { get; init; }
        public string DisplayName { get; init; }
        /// <summary><c>cascade_warning</c> or <c>cascade_recovered</c>.</summary>
        public string Kind { get; init; }
        /// <summary>The off-ramp that resolved the cascade, when applicable.</summary>
        public string OffRamp { get; init; }
    }

    /// <summary>The canonical cascade read model for one day.</summary>
    public sealed class CascadeAssessment
    {
        public List<CascadeActiveStage> Active { get; } = new List<CascadeActiveStage>();
        public List<CascadeEvent> Transitions { get; } = new List<CascadeEvent>();
        public string NextConsequence { get; set; } = string.Empty;
        public float NextWarningHours { get; set; }
        public bool HasCriticalStress { get; set; }
    }

    /// <summary>
    /// C2[6] 23C — fact-reading cascade authority.
    ///
    /// It evaluates authored rules over facts owned by other systems, reports the
    /// active stressors, the next likely consequence and its warning window, and
    /// emits exactly-once warning/recovery transitions for attribution. It does
    /// not own or mutate any subsystem state, does not script sequences, and does
    /// not use RNG — evaluation order is the ordinal rule id.
    ///
    /// State is deliberately presentation-only (which rules were active last tick),
    /// so a reload re-seeds instead of replaying transitions. The physical
    /// consequences remain owned by the subsystems that already model them.
    /// </summary>
    public sealed class CascadeCoordinator
    {
        private static readonly Dictionary<string, Func<CascadeFacts, bool>> Conditions =
            new Dictionary<string, Func<CascadeFacts, bool>>(StringComparer.Ordinal)
            {
                ["power_deficit"] = f => f.PowerDeficit,
                ["critical_deficit"] = f => f.CriticalDeficit,
                ["pumping_unserved"] = f => f.PumpingUnserved,
                ["sump_rising"] = f => f.SumpRising,
                ["heating_unserved"] = f => f.HeatingUnserved,
                ["filtration_unserved"] = f => f.FiltrationUnserved,
                ["fallout_storm"] = f => f.FalloutStorm,
                ["outbreak_active"] = f => f.OutbreakActive,
                ["fire_active"] = f => f.FireActive,
                ["hatch_unsealed"] = f => f.HatchUnsealed,
                ["cold_storage_unpowered"] = f => f.ColdStorageUnpowered,
                ["darkness"] = f => f.Darkness
            };

        private readonly List<CascadeRule> _ordered;
        private readonly HashSet<string> _previousActive = new HashSet<string>(StringComparer.Ordinal);
        private bool _seeded;

        public CascadeCoordinator(CascadeRuleCatalog catalog)
        {
            if (!CascadeRuleCatalogLoader.Validate(catalog, out string error))
                throw new ArgumentException(error, nameof(catalog));
            _ordered = new List<CascadeRule>(catalog.rules);
            _ordered.Sort(static (a, b) => string.CompareOrdinal(a.id, b.id));
        }

        public IReadOnlyList<CascadeRule> Rules => _ordered;

        /// <summary>
        /// Evaluate the rules against today's facts. Returns the active stressors,
        /// the next likely consequence and its warning window, and the transition
        /// events for exactly-once attribution. Null facts are treated as empty.
        /// </summary>
        public CascadeAssessment Evaluate(CascadeFacts? facts)
        {
            facts ??= new CascadeFacts();
            var assessment = new CascadeAssessment();

            var activeRules = new List<CascadeRule>();
            foreach (var rule in _ordered)
            {
                if (AllConditionsMet(rule, facts) && !AnyOffRampEngaged(rule, facts))
                    activeRules.Add(rule);
            }

            var activeIds = new HashSet<string>(StringComparer.Ordinal);
            foreach (var rule in activeRules)
            {
                activeIds.Add(rule.id);
                assessment.Active.Add(new CascadeActiveStage
                {
                    RuleId = rule.id,
                    DisplayName = rule.display_name,
                    EffectTags = rule.effect_tags,
                    OffRamps = rule.off_ramps,
                    WarningHours = rule.warning_hours
                });
                if (rule.requires_all.Contains("critical_deficit"))
                    assessment.HasCriticalStress = true;
            }

            // Attribution transitions — exactly-once per state change. The first
            // evaluation after construction/load seeds silently (no replay).
            if (_seeded)
            {
                foreach (var rule in activeRules)
                {
                    if (!_previousActive.Contains(rule.id))
                        assessment.Transitions.Add(new CascadeEvent
                        {
                            Day = facts.Day, RuleId = rule.id, DisplayName = rule.display_name,
                            Kind = "cascade_warning", OffRamp = string.Empty
                        });
                }
                foreach (var rule in _ordered)
                {
                    if (_previousActive.Contains(rule.id) && !activeIds.Contains(rule.id))
                        assessment.Transitions.Add(new CascadeEvent
                        {
                            Day = facts.Day, RuleId = rule.id, DisplayName = rule.display_name,
                            Kind = "cascade_recovered", OffRamp = FirstEngagedOffRamp(rule, facts)
                        });
                }
            }

            _previousActive.Clear();
            foreach (string id in activeIds) _previousActive.Add(id);
            _seeded = true;

            // Deterministic "next likely consequence": the first active rule in
            // ordinal id order. No RNG, no dictionary-order dependence.
            if (activeRules.Count > 0)
            {
                assessment.NextConsequence = activeRules[0].display_name;
                assessment.NextWarningHours = activeRules[0].warning_hours;
            }
            return assessment;
        }

        private static bool AllConditionsMet(CascadeRule rule, CascadeFacts facts)
        {
            for (int i = 0; i < rule.requires_all.Count; i++)
            {
                if (!Conditions.TryGetValue(rule.requires_all[i], out var predicate) || !predicate(facts))
                    return false;
            }
            return true;
        }

        private static bool AnyOffRampEngaged(CascadeRule rule, CascadeFacts facts)
            => !string.IsNullOrEmpty(FirstEngagedOffRamp(rule, facts));

        private static string FirstEngagedOffRamp(CascadeRule rule, CascadeFacts facts)
        {
            if (facts.SatisfiedOffRamps == null) return string.Empty;
            for (int i = 0; i < rule.off_ramps.Count; i++)
            {
                if (facts.SatisfiedOffRamps.Contains(rule.off_ramps[i]))
                    return rule.off_ramps[i];
            }
            return string.Empty;
        }
    }
}