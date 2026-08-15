using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Data;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;
using Random = System.Random;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Procedural Loot Generator — evaluates LocationDefinitionSO loot tables,
    /// location danger rating, world phase, and scavenger perks to return
    /// ProceduralItemInstance objects with dynamic condition, contamination,
    /// purity, and expiration states.
    ///
    /// Plain C#, save-safe. Used by LocationScavengingSystem during expeditions.
    /// </summary>
    public class ProceduralLootGenerator
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float BaseConditionMin = 0.30f;
        public const float BaseConditionMax = 0.95f;
        public const float DangerConditionPenalty = 0.15f;
        public const float ScavengerSkillConditionBonus = 0.10f;
        public const float LateWorldPhaseDegradation = 0.10f;
        public const float ContaminationChancePerDanger = 0.30f;
        public const float BaseContaminationMin = 0f;
        public const float BaseContaminationMax = 0.40f;
        public const float PurityVarianceRange = 0.25f;
        public const float ExpirationChanceAfterYear3 = 0.40f;
        public const float DegradedChanceAfterYear3 = 0.15f;

        // ── Item category detection ────────────────────────────────────
        private static readonly HashSet<string> WaterItemIds = new HashSet<string>
        {
            "water_canister", "clean_water_jug", "raw_water_barrel",
            "water_bottle", "filtered_water"
        };

        private static readonly HashSet<string> MedicalItemIds = new HashSet<string>
        {
            "antibiotics", "iodine_pills", "morphine", "bandage", "splint",
            "surgical_suture", "antiseptic", "sedative", "painkiller",
            "field_dressing_kit", "surgical_kit", "adhesive_bandages"
        };

        private static readonly HashSet<string> FoodItemIds = new HashSet<string>
        {
            "canned_stew", "dried_grain", "mre", "canned_food", "food_ration",
            "military_mre", "roots", "berries", "canned_beans", "dried_meat"
        };

        private static readonly HashSet<string> ScrapItemIds = new HashSet<string>
        {
            "scrap_metal", "copper_wiring", "steel_beams", "scrap_electronics",
            "aluminum_scrap", "lead_scrap", "iron_scrap"
        };

        private static readonly HashSet<string> FilterItemIds = new HashSet<string>
        {
            "hepa_filter", "air_filter", "water_filter", "carbon_filter",
            "ceramic_filter"
        };

        private readonly Random _rng;

        public ProceduralLootGenerator(int seed)
        {
            _rng = new Random(seed);
        }

        /// <summary>
        /// Generate a list of ProceduralItemInstances from a location's loot table
        /// with variance based on danger, world phase, and scavenger skill.
        /// </summary>
        public List<ProceduralItemInstance> GenerateLoot(
            LocationDefinitionSO location,
            WorldPhase currentPhase,
            Survivor scavenger,
            float searchHours,
            int itemsToGenerate = 4)
        {
            var results = new List<ProceduralItemInstance>();
            if (location == null || itemsToGenerate <= 0) return results;

            float dangerLevel = location.dangerLevel;
            float scavengerSkill = scavenger?.EffectiveScienceSkill ?? 0.3f;
            float skillBonus = scavengerSkill * ScavengerSkillConditionBonus;

            // Roll items from loot table
            for (int i = 0; i < itemsToGenerate; i++)
            {
                string itemId = PickItemFromLootTable(location, currentPhase);
                if (string.IsNullOrEmpty(itemId)) continue;

                var instance = GenerateInstance(itemId, dangerLevel, skillBonus,
                    currentPhase, searchHours);
                results.Add(instance);
            }

            return results;
        }

        private ProceduralItemInstance GenerateInstance(string itemId,
            float dangerLevel, float skillBonus, WorldPhase currentPhase,
            float searchHours)
        {
            var instance = new ProceduralItemInstance(itemId);

            // ── Condition ──────────────────────────────────────────────
            float conditionMin = BaseConditionMin;
            float conditionMax = BaseConditionMax;
            conditionMin = Mathf.Max(0.05f,
                conditionMin - dangerLevel * DangerConditionPenalty);
            conditionMax = Mathf.Max(conditionMin + 0.10f,
                conditionMax - dangerLevel * DangerConditionPenalty * 0.5f);
            conditionMin += skillBonus;
            conditionMax += skillBonus;
            if (currentPhase >= WorldPhase.NuclearWinter)
            {
                conditionMin -= LateWorldPhaseDegradation;
                conditionMax -= LateWorldPhaseDegradation * 0.5f;
            }
            instance.ConditionPct = Mathf.Clamp01(
                conditionMin + (float)_rng.NextDouble() * (conditionMax - conditionMin));

            // ── Contamination ──────────────────────────────────────────
            float contaminationChance = dangerLevel * ContaminationChancePerDanger;
            if ((float)_rng.NextDouble() < contaminationChance)
            {
                instance.ContaminationPct = Mathf.Clamp01(
                    BaseContaminationMin + (float)_rng.NextDouble() *
                    (BaseContaminationMax + dangerLevel * 0.3f));
            }

            // ── Type-specific variance ─────────────────────────────────
            ApplyTypeSpecificVariance(instance, currentPhase);

            // ── Quantity ───────────────────────────────────────────────
            instance.Quantity = 1 + _rng.Next(1 + (int)(searchHours / 4f));

            return instance;
        }

        private void ApplyTypeSpecificVariance(ProceduralItemInstance instance,
            WorldPhase currentPhase)
        {
            string itemId = instance.ItemId?.ToLowerInvariant() ?? "";

            // Water containers: volume (0.5-5.0L), purity %, integrity
            if (IsWaterItem(itemId))
            {
                instance.ContainerVolumeLitres = 0.5f +
                    (float)(_rng.NextDouble() * 4.5f);
                instance.CustomValueMultiplier = 0.7f +
                    (float)(_rng.NextDouble() * 0.6f);
                instance.ContainerIntegrityPct = 0.6f +
                    (float)(_rng.NextDouble() * 0.4f);
            }

            // Medical: expiration, doses remaining
            if (IsMedicalItem(itemId))
            {
                instance.DosesRemaining = 1 + _rng.Next(5);
                if (currentPhase >= WorldPhase.NuclearWinter)
                {
                    float roll = (float)_rng.NextDouble();
                    if (roll < DegradedChanceAfterYear3)
                        instance.Expiration = ExpirationState.Degraded;
                    else if (roll < ExpirationChanceAfterYear3 + DegradedChanceAfterYear3)
                        instance.Expiration = ExpirationState.Expired;
                }
            }

            // Food: calories, mold risk, radiation
            if (IsFoodItem(itemId))
            {
                instance.CaloricValueKcal = 200f +
                    (float)(_rng.NextDouble() * 1000f);
                instance.CustomValueMultiplier = instance.CaloricValueKcal / 600f;
                instance.MoldRiskPct = (float)(_rng.NextDouble() * 0.3f);
                instance.FoodRadiationAccumulation =
                    (float)(_rng.NextDouble() * 50f);
                if (currentPhase >= WorldPhase.NuclearWinter)
                {
                    float roll = (float)_rng.NextDouble();
                    if (roll < DegradedChanceAfterYear3)
                        instance.Expiration = ExpirationState.Degraded;
                    else if (roll < ExpirationChanceAfterYear3 + DegradedChanceAfterYear3)
                        instance.Expiration = ExpirationState.Expired;
                }
            }

            // Scrap: weight, purity grade
            if (IsScrapItem(itemId))
            {
                instance.ScrapWeightKg = 1f + (float)(_rng.NextDouble() * 9f);
                instance.ScrapPurityGrade = 0.3f +
                    (float)(_rng.NextDouble() * 0.7f);
                instance.CustomValueMultiplier = instance.ScrapPurityGrade;
            }

            // Filters: durability, filtration efficiency
            if (IsFilterItem(itemId))
            {
                instance.ConditionPct = Mathf.Max(0.1f,
                    instance.ConditionPct);
                instance.FiltrationEfficiencyMicrons = 0.3f +
                    (float)(_rng.NextDouble() * 4.7f);
                instance.CustomValueMultiplier =
                    1f - (instance.FiltrationEfficiencyMicrons / 5f) * 0.7f;
            }
        }

        private string PickItemFromLootTable(LocationDefinitionSO location,
            WorldPhase currentPhase)
        {
            // Simplified: pick random item category based on location type
            string[] items = GetLootItemsForLocation(location.id, currentPhase);
            if (items == null || items.Length == 0) return null;
            return items[_rng.Next(items.Length)];
        }

        private string[] GetLootItemsForLocation(string locationId,
            WorldPhase phase)
        {
            // Map location IDs to loot item pools
            if (locationId.Contains("hospital") || locationId.Contains("pharmacy"))
                return new[] { "antibiotics", "iodine_pills", "bandage",
                    "surgical_suture", "morphine", "field_dressing_kit" };
            if (locationId.Contains("military") || locationId.Contains("checkpoint")
                || locationId.Contains("precinct"))
                return new[] { "ammo_556", "military_mre", "field_dressing_kit",
                    "dog_tags", "riot_shield" };
            if (locationId.Contains("water") || locationId.Contains("aquifer"))
                return new[] { "water_canister", "water_filter", "water_valve",
                    "chemical_barrel" };
            if (locationId.Contains("grain") || locationId.Contains("silo")
                || locationId.Contains("agricultural"))
                return new[] { "dried_grain", "canned_food", "seeds",
                    "fertilizer", "hand_tools" };
            if (locationId.Contains("depot") || locationId.Contains("fuel"))
                return new[] { "gasoline", "kerosene", "diesel_fuel",
                    "rubber_hose", "scrap_metal" };
            if (locationId.Contains("mine") || locationId.Contains("tunnel")
                || locationId.Contains("metro"))
                return new[] { "coal", "scrap_metal", "drill_bits",
                    "clean_water", "copper_wiring" };
            if (locationId.Contains("nursery") || locationId.Contains("woodland"))
                return new[] { "medicinal_herbs", "firewood", "seeds",
                    "clean_water", "resin" };

            // Generic loot
            return new[] { "scrap_metal", "canned_food", "cloth", "copper_wiring",
                "water_bottle", "adhesive_bandages" };
        }

        private bool IsWaterItem(string id) =>
            WaterItemIds.Contains(id) || id.Contains("water");
        private bool IsMedicalItem(string id) =>
            MedicalItemIds.Contains(id) || id.Contains("medical") ||
            id.Contains("bandage") || id.Contains("pill");
        private bool IsFoodItem(string id) =>
            FoodItemIds.Contains(id) || id.Contains("food") ||
            id.Contains("ration") || id.Contains("mre");
        private bool IsScrapItem(string id) =>
            ScrapItemIds.Contains(id) || id.Contains("scrap") ||
            id.Contains("metal") || id.Contains("steel");
        private bool IsFilterItem(string id) =>
            FilterItemIds.Contains(id) || id.Contains("filter");
    }
}
