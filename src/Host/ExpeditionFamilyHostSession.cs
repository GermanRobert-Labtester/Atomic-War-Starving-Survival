// SPDX-License-Identifier: MIT
// ============================================================================
// Host Session : ExpeditionFamilyHostSession
// Purpose      : PLAN-EXPEDITION-FAMILY-TRUTH-269 — wire the expedition family
//                engines no plan referenced: the aerial reconnaissance flight
//                window evaluator and the expedition loot reference resolver/
//                validator. Pure static engines — derived, no save section.
// ============================================================================

using System;
using System.Collections.Generic;
using Ashfall.Core.Expeditions;

namespace AtomicWar.GodotApp
{
    public sealed class ExpeditionFamilySnapshot
    {
        public string WindowCondition { get; set; } = string.Empty;
        public bool LaunchPermitted { get; set; }
        public int TotalFlightRiskPermille { get; set; }
        public int EffectiveRangeKm { get; set; }
        public int AirdropDriftMeters { get; set; }
        public int AirworthinessWearPermille { get; set; }
        public string LootResolutionType { get; set; } = string.Empty;
        public string LootCanonicalId { get; set; } = string.Empty;
    }

    public sealed class ExpeditionFamilyHostSession
    {
        private ExpeditionLootReferenceResolver _lootResolver = new();
        public ExpeditionLootReferenceResolver LootResolver => _lootResolver;
        public ExpeditionFamilySnapshot Snapshot { get; private set; } = new();
        public string LastEvent { get; private set; } = string.Empty;

        public void BindLootResolver(IEnumerable<string>? itemIds, IEnumerable<string>? knownCategories)
            => _lootResolver = new ExpeditionLootReferenceResolver(itemIds, knownCategories);

        /// <summary>Aerial reconnaissance window evaluation (pure, deterministic).</summary>
        public ExpeditionFamilySnapshot EvaluateFlightWindow(
            int baseRangeKm, int airworthinessPermille, int windSpeedKmh,
            int visibilityPermille, int temperatureCelsius, int payloadWeightKg, int maxPayloadKg)
        {
            var result = AerialReconWindowEngine.Evaluate(
                baseRangeKm, airworthinessPermille, windSpeedKmh, visibilityPermille,
                temperatureCelsius, payloadWeightKg, maxPayloadKg);
            Snapshot = new ExpeditionFamilySnapshot
            {
                WindowCondition = result.Condition.ToString(),
                LaunchPermitted = result.IsLaunchPermitted,
                TotalFlightRiskPermille = result.TotalFlightRiskPermille,
                EffectiveRangeKm = result.EffectiveRangeKm,
                AirdropDriftMeters = result.AirdropDriftMeters,
                AirworthinessWearPermille = result.AirworthinessWearPermille,
                LootResolutionType = Snapshot.LootResolutionType,
                LootCanonicalId = Snapshot.LootCanonicalId
            };
            LastEvent = $"Aerial window {result.Condition} (launch={result.IsLaunchPermitted}, risk={result.TotalFlightRiskPermille}‰).";
            return Snapshot;
        }

        /// <summary>Loot reference resolution via the canonical resolver (typed result).</summary>
        public ExpeditionFamilySnapshot ResolveLootReference(string reference)
        {
            var type = _lootResolver.Resolve(reference, out var canonicalId);
            Snapshot.LootResolutionType = type.ToString();
            Snapshot.LootCanonicalId = canonicalId ?? string.Empty;
            LastEvent = $"Loot reference '{reference}' → {type}.";
            return Snapshot;
        }

        /// <summary>Loot validation via the canonical validator (typed failures).</summary>
        public ExpeditionLootValidationResult ValidateLoot(IEnumerable<ExpeditionDefinition> expeditions)
            => ExpeditionLootValidator.Validate(expeditions, _lootResolver);
    }
}
