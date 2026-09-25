// SPDX-License-Identifier: MIT
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Save;
using Ashfall.Core.Survivors;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// XP-01 bounded runtime gate. It exercises the authored difficulty
        /// authority and each scalar's existing Core owner without creating a
        /// second simulation or save path.
        /// </summary>
        public static int RunDifficultySelfTest(string dataDirectory)
        {
            int passed = 0;
            int failed = 0;

            void Check(string name, bool condition, string detail)
            {
                if (condition)
                {
                    passed++;
                    GD.Print($"[DIFFICULTY] PASS {name}: {detail}");
                }
                else
                {
                    failed++;
                    GD.PrintErr($"[DIFFICULTY] FAIL {name}: {detail}");
                }
            }

            try
            {
                CatalogLocator.UseInvariantCulture();
                var catalog = DifficultyPresetCatalogLoader.Load(dataDirectory, new FileSystemIO());
                Check("catalog", catalog.Validate(out string catalogError) && catalog.AllPresets.Count == 4,
                    catalogError.Length == 0 ? "four authored presets" : catalogError);

                var director = new DifficultyDirector(catalog);
                var standard = director.ResolveProvider("difficulty_standard");
                var austere = director.ResolveProvider("difficulty_austere");
                var dirge = director.ResolveProvider("difficulty_dirge");

                bool allScalarsValid = true;
                foreach (DifficultyPreset preset in catalog.AllPresets)
                {
                    if (!preset.Validate(out _))
                    {
                        allScalarsValid = false;
                        break;
                    }
                }
                Check("scalar_bounds", allScalarsValid,
                    "all eight authored multipliers are finite and within the catalog bounds");

                Check("legacy_parity",
                    DifficultyScalarsProvider.Legacy.HungerMult == 1f &&
                    DifficultyScalarsProvider.Legacy.ThirstMult == 1f &&
                    DifficultyScalarsProvider.Legacy.RadiationMult == 1f &&
                    DifficultyScalarsProvider.Legacy.DiseaseMult == 1f &&
                    DifficultyScalarsProvider.Legacy.HostileEncounterMult == 1f &&
                    DifficultyScalarsProvider.Legacy.MarketPriceMult == 1f &&
                    DifficultyScalarsProvider.Legacy.EquipmentDecayMult == 1f &&
                    DifficultyScalarsProvider.Legacy.CrisisDeadlineMult == 1f,
                    "legacy provider remains all ones");

                bool unknownRejected = false;
                try
                {
                    director.ResolveProvider("difficulty_unknown");
                }
                catch (InvalidOperationException)
                {
                    unknownRejected = true;
                }
                Check("unknown_fail_closed", unknownRejected,
                    "unknown campaign preset IDs are rejected by the director");

                DifficultyPreset sparingPreset = director.ResolvePreset("difficulty_sparing");
                var expectedBonusIds = new[] { "canned_food", "iodine_pills" };
                var actualBonusIds = sparingPreset.starting_bonus_item_ids ?? new List<string>();
                Check("starting_bonus_authority", actualBonusIds.SequenceEqual(expectedBonusIds),
                    "sparing bonus is exactly canned_food + iodine_pills");

                var inventory = new Ashfall.Core.Inventory.Inventory();
                foreach (string itemId in actualBonusIds)
                    inventory.AddById(itemId, 1);
                Check("starting_bonus_once", actualBonusIds.All(itemId => inventory.CountById(itemId) == 1),
                    "canonical bonus items can be granted exactly once");
                Check("standard_no_bonus", director.ResolvePreset("difficulty_standard").starting_bonus_item_ids.Count == 0,
                    "standard has no authored bonus items");

                var standardNeeds = new NeedsSystem
                {
                    HungerRateMultiplier = () => standard.HungerMult,
                    ThirstRateMultiplier = () => standard.ThirstMult
                };
                var dirgeNeeds = new NeedsSystem
                {
                    HungerRateMultiplier = () => dirge.HungerMult,
                    ThirstRateMultiplier = () => dirge.ThirstMult
                };
                var standardState = new SurvivorNeedsState { Id = "same_seed_standard" };
                var dirgeState = new SurvivorNeedsState { Id = "same_seed_dirge" };
                standardNeeds.Register(standardState);
                dirgeNeeds.Register(dirgeState);
                standardNeeds.Tick(4f);
                dirgeNeeds.Tick(4f);
                Check("needs_consumer", dirgeState.Hunger > standardState.Hunger &&
                    dirgeState.Thirst > standardState.Thirst &&
                    InBounds(standardState.Hunger) && InBounds(dirgeState.Hunger),
                    $"same-seed bounded drift differs ({standardState.Hunger:0.###}/{dirgeState.Hunger:0.###})");

                float standardRadiation = RadiationSystem.ComputeEffectiveRate(25f, 2f, 3f, null, standard.RadiationMult);
                float austereRadiation = RadiationSystem.ComputeEffectiveRate(25f, 2f, 3f, null, austere.RadiationMult);
                Check("radiation_consumer", austereRadiation > standardRadiation && InBounds(austereRadiation),
                    $"effective rate scales before clamp ({standardRadiation:0.###}/{austereRadiation:0.###})");

                var diseaseCatalog = DiseaseCatalogLoader.Load(dataDirectory, new FileSystemIO(), new SystemTextJsonSerializer());
                var standardDisease = new DiseaseSystem(rng: new SeededRng(31));
                var dirgeDisease = new DiseaseSystem(rng: new SeededRng(31));
                standardDisease.BindCatalog(diseaseCatalog);
                dirgeDisease.BindCatalog(diseaseCatalog);
                standardDisease.OnsetProbabilityMultiplier = () => standard.DiseaseMult;
                dirgeDisease.OnsetProbabilityMultiplier = () => dirge.DiseaseMult;
                var exposure = new DiseaseExposureContext
                {
                    SurvivorId = "same_seed",
                    DiseaseId = DiseaseIds.Cholera,
                    ProbabilityModifier = 1f,
                    Day = 1
                };
                float standardDiseaseProbability = standardDisease.TryExpose(exposure).EffectiveProbability;
                float dirgeDiseaseProbability = dirgeDisease.TryExpose(exposure).EffectiveProbability;
                Check("disease_consumer", dirgeDiseaseProbability > standardDiseaseProbability &&
                    dirgeDiseaseProbability >= 0f && dirgeDiseaseProbability <= 1f,
                    $"onset probability remains bounded ({standardDiseaseProbability:0.###}/{dirgeDiseaseProbability:0.###})");

                var goods = new GoodsCatalogLoadResult();
                goods.Goods.Add(new GoodDefinition
                {
                    id = "canned_food",
                    displayName = "Canned Food",
                    category = "food",
                    basePrice = 10f,
                    volatility = 0.2f,
                    elasticity = 1f
                });
                var market = new MarketSystem();
                market.BindCatalog(GoodsCatalogLoader.ToCatalog(goods));
                market.PriceMultiplierProvider = () => austere.MarketPriceMult;
                var price = market.ExplainPrice("canned_food");
                Check("market_consumer", price.finalPrice > 10f,
                    $"difficulty price factor applies before authored clamps ({price.finalPrice:0.###})");

                var wearInventory = new Ashfall.Core.Inventory.Inventory();
                var equipment = new EquipmentConditionSystem(
                    new SeededRng(31), wearInventory, new Ashfall.Core.Crafting.CraftingSystem(wearInventory));
                equipment.WearRateMultiplierProvider = () => dirge.EquipmentDecayMult;
                equipment.RegisterItem("difficulty_item", "weapon_service_rifle", "survivor", EquipmentFamily.Weapon, 100f);
                equipment.ApplyWear("difficulty_item", new WearEvent());
                float remainingCondition = equipment.GetItem("difficulty_item")?.condition ?? 0f;
                Check("equipment_consumer", remainingCondition < 100f && remainingCondition > 0f,
                    $"wear multiplier degrades the existing condition owner ({remainingCondition:0.###})");

                var standardCrisis = CrisisPredictor.Evaluate(new CrisisPredictionInputs
                {
                    CurrentDay = 10,
                    LivingSurvivorCount = 4,
                    FoodStockUnits = 12f,
                    DailyFoodBurnRate = 4f,
                    DeadlineMultiplier = standard.CrisisDeadlineMult
                }).FirstOrDefault();
                var dirgeCrisis = CrisisPredictor.Evaluate(new CrisisPredictionInputs
                {
                    CurrentDay = 10,
                    LivingSurvivorCount = 4,
                    FoodStockUnits = 12f,
                    DailyFoodBurnRate = 4f,
                    DeadlineMultiplier = dirge.CrisisDeadlineMult
                }).FirstOrDefault();
                Check("crisis_consumer", standardCrisis != null && dirgeCrisis != null &&
                    dirgeCrisis.HorizonDays < standardCrisis.HorizonDays,
                    $"deadline horizon changes ({standardCrisis?.HorizonDays}/{dirgeCrisis?.HorizonDays})");

                float standardHostile = standard.HostileEncounterMult;
                float dirgeHostile = dirge.HostileEncounterMult;
                Check("hostile_consumer",
                    Math.Abs(standardHostile - 1f) < 0.001f && dirgeHostile > standardHostile,
                    $"authored hostile_encounter_mult differs by preset ({standardHostile:0.###}/{dirgeHostile:0.###})");

                var manifest = new SaveManifest
                {
                    manifestVersion = SaveManifest.CurrentManifestVersion,
                    profileId = new SaveProfileId("default"),
                    slotId = new SaveSlotId("difficulty_selftest"),
                    generationId = "difficulty_generation",
                    difficultyPresetId = "difficulty_austere"
                };
                var section = new SaveSectionEnvelope
                {
                    sectionName = "difficulty_selftest",
                    schemaVersion = 1,
                    generationId = manifest.generationId,
                    payloadJson = "{}"
                };
                section.checksum = SaveSlotService.ComputeSectionChecksum(section);
                var envelope = new AggregateSaveEnvelope
                {
                    manifestVersion = CampaignEnvelopeBuilder.CurrentEnvelopeVersion,
                    manifest = manifest,
                    sections = new List<SaveSectionEnvelope> { section }
                };
                string checksum = SaveSlotService.ComputeAggregateChecksum(envelope);
                envelope.manifest.difficultyPresetId = "difficulty_dirge";
                Check("save_binding", checksum != SaveSlotService.ComputeAggregateChecksum(envelope),
                    "difficulty preset ID participates in the current aggregate checksum");
            }
            catch (Exception ex)
            {
                failed++;
                GD.PrintErr("[DIFFICULTY] unhandled self-test exception: " + ex);
            }

            return EmitSummary("difficulty_selftest", failed == 0, failed == 0 ? 0 : 1,
                passed, failed, $"XP-01 assertions: {passed} passed, {failed} failed");
        }

        private static bool InBounds(float value)
        {
            return !float.IsNaN(value) && !float.IsInfinity(value) && value >= 0f && value <= 100f;
        }
    }
}
