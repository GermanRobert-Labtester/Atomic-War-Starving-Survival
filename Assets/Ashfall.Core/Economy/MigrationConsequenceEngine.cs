// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Economy
{
    [Serializable]
    public sealed class MigrationConsequenceSaveState
    {
        public int schema_version { get; set; } = 1;
        public HashSet<string> AppliedConsequenceKeys { get; set; } = new();
    }

    /// <summary>
    /// XP-08-F6 / UNBLOCK-02 §5.7 / §17.2:
    /// Seasonal Human Migration Consequence & Regional Multiplier Engine.
    /// Consumes regional human population weights produced by SeasonalHumanMigrationEngine
    /// and deterministically projects market demand multipliers, labor candidate pool scaling,
    /// territorial friction event weights, and caravan demand hooks.
    /// Enforces exactly-once fired-phase keys so migration consequences never double-apply on reload.
    /// </summary>
    public sealed class MigrationConsequenceEngine
    {
        public const int BaselinePopulationWeight = 100;
        public const int PermilleScale = 1000;

        private readonly SeasonalHumanMigrationEngine _migrationEngine;
        private readonly HashSet<string> _appliedConsequenceKeys = new(StringComparer.Ordinal);

        public IReadOnlyCollection<string> AppliedConsequenceKeys => _appliedConsequenceKeys;

        public event Action<string, string, int, int>? OnMigrationConsequenceApplied;

        public MigrationConsequenceEngine(SeasonalHumanMigrationEngine migrationEngine)
        {
            _migrationEngine = migrationEngine ?? throw new ArgumentNullException(nameof(migrationEngine));
        }

        public int GetMarketDemandMultiplierPermille(string regionId, string itemCategory)
        {
            int weight = _migrationEngine.GetRegionPopulationWeight(regionId);
            int delta = weight - BaselinePopulationWeight;

            string cat = itemCategory?.ToLowerInvariant() ?? string.Empty;

            // Essentials (food, fuel, medicine) scale directly with population weight
            if (cat == "food" || cat == "fuel" || cat == "medicine" || cat == "rations")
            {
                // Weight 100 -> 1000 permille; Weight 150 -> 1500 permille; Weight 60 -> 600 permille
                return Math.Max(200, (weight * PermilleScale) / BaselinePopulationWeight);
            }

            // Luxury, entertainment, and contraband scale moderately
            if (cat == "luxury" || cat == "contraband" || cat == "alcohol")
            {
                return Math.Max(500, PermilleScale + (delta * 5));
            }

            // Labor services: excess population depresses labor cost / increases labor supply
            if (cat == "labor" || cat == "service")
            {
                return Math.Max(400, PermilleScale - (delta * 5));
            }

            // General goods default: minor scaling
            return Math.Max(500, PermilleScale + (delta * 3));
        }

        public int GetLaborPoolSizeMultiplierPermille(string regionId)
        {
            int weight = _migrationEngine.GetRegionPopulationWeight(regionId);
            // Weight 100 -> 1000 permille (1.0x). Higher population expands shelter recruitment candidate pool
            return Math.Max(100, (weight * PermilleScale) / BaselinePopulationWeight);
        }

        public int GetTerritorialFrictionMultiplierPermille(string regionId)
        {
            int weight = _migrationEngine.GetRegionPopulationWeight(regionId);
            int delta = weight - BaselinePopulationWeight;

            // High population density intensifies resource competition and border friction
            return Math.Max(400, PermilleScale + (delta * 8));
        }

        public string GetCaravanDemandPriority(string regionId)
        {
            int weight = _migrationEngine.GetRegionPopulationWeight(regionId);

            if (weight >= 120)
            {
                return "food_and_fuel"; // High influx requires emergency survival supplies
            }

            if (weight <= 80)
            {
                return "defense_and_labor"; // Depopulated outposts require security and repair labor
            }

            return "balanced_trade";
        }

        public bool TryApplyPhaseConsequence(int currentDay, string seasonPhase, string regionId)
        {
            if (string.IsNullOrWhiteSpace(seasonPhase) || string.IsNullOrWhiteSpace(regionId))
                return false;

            string key = $"mig_conseq_{regionId}_{seasonPhase}_day{currentDay}";
            if (_appliedConsequenceKeys.Contains(key))
                return false; // Exactly-once guard

            int weight = _migrationEngine.GetRegionPopulationWeight(regionId);
            int foodDemand = GetMarketDemandMultiplierPermille(regionId, "food");

            _appliedConsequenceKeys.Add(key);
            OnMigrationConsequenceApplied?.Invoke(regionId, seasonPhase, weight, foodDemand);
            return true;
        }

        public MigrationConsequenceSaveState CaptureState()
        {
            return new MigrationConsequenceSaveState
            {
                schema_version = 1,
                AppliedConsequenceKeys = new HashSet<string>(_appliedConsequenceKeys, StringComparer.Ordinal)
            };
        }

        public void RestoreState(MigrationConsequenceSaveState? state)
        {
            _appliedConsequenceKeys.Clear();
            if (state?.AppliedConsequenceKeys != null)
            {
                foreach (var key in state.AppliedConsequenceKeys)
                {
                    if (!string.IsNullOrWhiteSpace(key))
                    {
                        _appliedConsequenceKeys.Add(key);
                    }
                }
            }
        }
    }
}
