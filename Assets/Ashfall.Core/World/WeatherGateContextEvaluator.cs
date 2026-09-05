using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Evaluates weather gates within the cross-system interaction layer (F17–F20).
    /// Pure; deterministic; non-mutating.
    /// Weather remains the sole passability authority; other systems contextualize
    /// consequence severity, shelter mitigation, encounter pressure, and debt timing.
    /// </summary>
    public static class WeatherGateContextEvaluator
    {
        public const float MaxSeverityCap = 2.0f;
        public const float DefaultSeverity = 1.0f;

        /// <summary>
        /// Explicit consequence severity precedence rule (Section 2.3 / 10.5 / 11.2):
        /// effectiveSeverity = min(2.0, max(1.0, war, territory, seasonal)).
        /// Compound modifiers do NOT stack or multiply across categories.
        /// </summary>
        public static float MergeSeverity(float warSeverity, float territorySeverity, float seasonalSeverity)
        {
            float maxHarmful = Math.Max(DefaultSeverity, Math.Max(warSeverity, Math.Max(territorySeverity, seasonalSeverity)));
            return Math.Min(MaxSeverityCap, maxHarmful);
        }

        internal static WeatherGateContextModifier? ResolveWarModifier(WeatherGate gate, in FactionWarSnapshot war)
        {
            if (gate.WarStateModifier == null || !gate.WarStateModifier.enabled)
                return null;

            if (!war.IsAtWar)
                return null;

            if (gate.WarStateModifier.hostile_only && !war.IsDominantFactionHostile)
                return null;

            if (war.ActiveWarTension < gate.WarStateModifier.min_tension)
                return null;

            return new WeatherGateContextModifier
            {
                SeverityMultiplier = gate.WarStateModifier.severity_multiplier,
                ForcedDetourSuggested = gate.WarStateModifier.force_detour,
                EncounterTag = gate.WarStateModifier.encounter_tag,
                EncounterWeightMultiplier = gate.WarStateModifier.encounter_weight_multiplier,
                Reason = $"war_hostile_tension_{war.ActiveWarTension}"
            };
        }

        internal static WeatherGateContextModifier? ResolveTerritoryModifier(WeatherGate gate, in TerritorySnapshot territory)
        {
            if (gate.TerritoryModifier == null)
                return null;

            var stateMod = gate.TerritoryModifier.GetForState(territory.State);
            if (stateMod == null)
                return null;

            return new WeatherGateContextModifier
            {
                SeverityMultiplier = stateMod.severity_multiplier,
                ShelterAvailable = stateMod.shelter_available,
                ForcedDetourSuggested = false,
                Reason = $"territory_{territory.State.ToString().ToLowerInvariant()}"
            };
        }

        internal static WeatherGateContextModifier? ResolveSeasonalModifier(WeatherGate gate, in SeasonalEventSnapshot seasonal, bool isBlocked)
        {
            if (gate.CompoundEventModifier == null || gate.CompoundEventModifier.Count == 0 ||
                seasonal.ActiveEventIds == null || seasonal.ActiveEventIds.Count == 0)
                return null;

            // Compound severity only applies when base gate is blocked (Section 10.4)
            if (!isBlocked)
                return null;

            string? bestEvent = null;
            float highestMultiplier = 1.0f;

            // Deterministic evaluation: sort by event ID
            foreach (var kvp in gate.CompoundEventModifier.OrderBy(k => k.Key, StringComparer.Ordinal))
            {
                if (seasonal.ActiveEventIds.Contains(kvp.Key))
                {
                    if (kvp.Value > highestMultiplier)
                    {
                        highestMultiplier = kvp.Value;
                        bestEvent = kvp.Key;
                    }
                }
            }

            if (bestEvent == null)
                return null;

            return new WeatherGateContextModifier
            {
                SeverityMultiplier = highestMultiplier,
                Reason = $"seasonal_compound_{bestEvent}"
            };
        }

        /// <summary>
        /// Main cross-system evaluation entry point.
        /// </summary>
        public static WeatherGateContextResult Evaluate(WeatherGate gate, WeatherGateEvaluationContext context)
        {
            if (gate == null) throw new ArgumentNullException(nameof(gate));
            if (context == null) throw new ArgumentNullException(nameof(context));

            // 1. Weather condition determines base passability (Section 2.1)
            var baseState = WeatherGateEvaluator.EvaluateGateStatic(gate, context.CurrentWeather);
            bool isBlocked = !baseState.IsOpen;
            string blockedReason = isBlocked ? baseState.Reason : string.Empty;

            // 2. Inventory override check
            bool overrideAvailable = false;
            if (!string.IsNullOrEmpty(gate.OverrideItem) && context.InventoryItems != null)
            {
                for (int i = 0; i < context.InventoryItems.Count; i++)
                {
                    if (string.Equals(context.InventoryItems[i], gate.OverrideItem, StringComparison.OrdinalIgnoreCase))
                    {
                        overrideAvailable = true;
                        break;
                    }
                }
            }

            // 3. Resolve contextual modifiers
            var warMod = ResolveWarModifier(gate, context.War);
            var terrMod = ResolveTerritoryModifier(gate, context.Territory);
            var seasMod = ResolveSeasonalModifier(gate, context.Seasonal, isBlocked);

            float warSeverity = warMod?.SeverityMultiplier ?? 1.0f;
            float terrSeverity = terrMod?.SeverityMultiplier ?? 1.0f;
            float seasSeverity = seasMod?.SeverityMultiplier ?? 1.0f;

            // 4. Merge consequence severity (applies to forced traversal when blocked)
            float effectiveSeverity = isBlocked ? MergeSeverity(warSeverity, terrSeverity, seasSeverity) : 1.0f;

            // 5. Shelter mitigation
            bool shelterAvailable = terrMod?.ShelterAvailable ?? false;

            // 6. Forced detour suggestion
            bool forcedDetour = (warMod?.ForcedDetourSuggested ?? false) || (terrMod?.ForcedDetourSuggested ?? false);

            // 7. Encounter pressure
            string encounterTag = warMod?.EncounterTag ?? string.Empty;
            float encounterWeight = warMod?.EncounterWeightMultiplier ?? 1.0f;

            // 8. Debt delay eligibility
            bool debtEligible = gate.WeatherDelayDebt && isBlocked;

            // 9. Deterministic reason ordering (Section 6.4)
            var reasons = new List<string>();
            if (isBlocked)
            {
                reasons.Add(baseState.Reason);
            }
            else
            {
                reasons.Add("weather_open");
            }

            if (warMod != null)
                reasons.Add(warMod.Reason);

            if (terrMod != null)
                reasons.Add(terrMod.Reason);

            if (seasMod != null)
                reasons.Add(seasMod.Reason);

            if (overrideAvailable)
                reasons.Add($"override_available_{gate.OverrideItem}");

            if (debtEligible)
                reasons.Add("debt_weather_pause_eligible");

            // 10. Deterministic trace (Section 6.5)
            string trace = string.Format(
                CultureInfo.InvariantCulture,
                "target={0};weather={1};baseBlocked={2};war={3}/tension={4}/dominant={5}/hostile={6};warSeverity={7:F2};territory={8};territorySeverity={9:F2};seasonal={10};seasonalSeverity={11:F2};effectiveSeverity={12:F2};shelter={13};encounter={14};encounterWeight={15:F2};weatherDelayDebt={16}",
                gate.TargetId,
                context.CurrentWeather,
                isBlocked,
                context.War.IsAtWar ? "War" : "Peace",
                context.War.ActiveWarTension,
                context.War.DominantFactionId,
                context.War.IsDominantFactionHostile,
                warSeverity,
                context.Territory.State,
                terrSeverity,
                seasMod != null ? seasMod.Reason : "none",
                seasSeverity,
                effectiveSeverity,
                shelterAvailable,
                string.IsNullOrEmpty(encounterTag) ? "none" : encounterTag,
                encounterWeight,
                debtEligible);

            return new WeatherGateContextResult
            {
                GateId = gate.Id,
                TargetId = gate.TargetId,
                IsBlocked = isBlocked,
                BlockedReason = blockedReason,
                OverrideAvailable = overrideAvailable,
                ConsequenceSeverityMultiplier = effectiveSeverity,
                ShelterAvailable = shelterAvailable,
                ForcedDetourSuggested = forcedDetour,
                FactionEncounterTag = encounterTag,
                FactionEncounterWeightMultiplier = encounterWeight,
                WeatherDelayDebtEligible = debtEligible,
                AppliedContextReasons = reasons,
                EvaluationTrace = trace
            };
        }
    }
}
