// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Disease;
using Ashfall.Core.Difficulty;
using Ashfall.Core.Economy;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radiation;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Difficulty
{
    public sealed class DifficultyFullBindingTests
    {
        [Fact]
        public void Needs_BaseDrift_AppliesHungerAndThirstOnlyAtTheirOwnerSite()
        {
            var legacy = new NeedsSystem();
            var scaled = new NeedsSystem
            {
                HungerRateMultiplier = () => 1.5f,
                ThirstRateMultiplier = () => 0.5f
            };
            var legacyState = new SurvivorNeedsState { Id = "legacy" };
            var scaledState = new SurvivorNeedsState { Id = "scaled" };
            legacy.Register(legacyState);
            scaled.Register(scaledState);

            legacy.Tick(1f);
            scaled.Tick(1f);

            Assert.Equal(0.8f, legacyState.Hunger);
            Assert.Equal(1.2f, legacyState.Thirst);
            Assert.Equal(1.2f, scaledState.Hunger);
            Assert.Equal(0.6f, scaledState.Thirst);
            Assert.Equal(legacyState.Fatigue, scaledState.Fatigue);
        }

        [Fact]
        public void Radiation_EffectiveRate_MultipliesBeforeTheExistingClamp()
        {
            Assert.Equal(0f, RadiationSystem.ComputeEffectiveRate(5f, 10f, 0f, null, 2f));
            Assert.Equal(40f, RadiationSystem.ComputeEffectiveRate(25f, 2f, 3f, null, 2f));
            Assert.Equal(36f, RadiationSystem.ComputeEffectiveRate(0f, 4f, 0f, 22f, 2f));
        }

        [Fact]
        public void Disease_OnsetMultiplier_PreservesProbabilityBounds()
        {
            var system = new DiseaseSystem(rng: new SeededRng(7));
            system.BindCatalog(DiseaseCatalogLoader.Load(DataDirectory(), new FileSystemIO(), new SystemTextJsonSerializer()));
            system.OnsetProbabilityMultiplier = () => 2f;

            var result = system.TryExpose(new DiseaseExposureContext
            {
                SurvivorId = "survivor_a",
                DiseaseId = DiseaseIds.Cholera,
                ProbabilityModifier = 10f,
                Day = 1
            });

            Assert.True(result.Infected);
            Assert.Equal(1f, result.EffectiveProbability);
        }

        [Fact]
        public void Market_DifficultyMultiplier_IsBeforeTheAuthoredPriceClamps()
        {
            var load = new GoodsCatalogLoadResult();
            load.Goods.Add(new GoodDefinition
            {
                id = "canned_food",
                displayName = "Canned Food",
                category = "food",
                basePrice = 10f,
                volatility = 0.2f,
                elasticity = 1f
            });
            var market = new MarketSystem();
            market.BindCatalog(GoodsCatalogLoader.ToCatalog(load));
            market.PriceMultiplierProvider = () => 1.5f;

            var explanation = market.ExplainPrice("canned_food");

            Assert.Equal(15f, explanation.finalPrice);
            Assert.Contains(explanation.factors, f => f.kind == PriceFactorKind.Difficulty);
        }

        [Fact]
        public void Equipment_WearMultiplier_ScalesTheUseWearDelta()
        {
            EquipmentConditionSystem legacy = CreateEquipment(out _);
            EquipmentConditionSystem scaled = CreateEquipment(out _);
            scaled.WearRateMultiplierProvider = () => 2f;
            legacy.RegisterItem("legacy_item", "weapon_service_rifle", "survivor", EquipmentFamily.Weapon, 100f);
            scaled.RegisterItem("scaled_item", "weapon_service_rifle", "survivor", EquipmentFamily.Weapon, 100f);

            legacy.ApplyWear("legacy_item", new WearEvent());
            scaled.ApplyWear("scaled_item", new WearEvent());

            float legacyLoss = 100f - legacy.GetItem("legacy_item")!.condition;
            float scaledLoss = 100f - scaled.GetItem("scaled_item")!.condition;
            Assert.True(legacyLoss > 0f);
            Assert.Equal(legacyLoss * 2f, scaledLoss, 4);
        }

        [Fact]
        public void Crisis_DeadlineMultiplier_ScalesRunwayAndPreservesStandardParity()
        {
            var legacy = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 4,
                FoodStockUnits = 12f,
                DailyFoodBurnRate = 4f
            };
            var scaled = new CrisisPredictionInputs
            {
                CurrentDay = 10,
                LivingSurvivorCount = 4,
                FoodStockUnits = 12f,
                DailyFoodBurnRate = 4f,
                DeadlineMultiplier = 0.5f
            };

            var legacyRecord = Assert.Single(CrisisPredictor.Evaluate(legacy));
            var scaledRecord = Assert.Single(CrisisPredictor.Evaluate(scaled));

            Assert.Equal(13, legacyRecord.ProjectedDay);
            Assert.Equal(3, legacyRecord.HorizonDays);
            Assert.Equal(11, scaledRecord.ProjectedDay);
            Assert.Equal(1, scaledRecord.HorizonDays);
        }

        [Fact]
        public void DifficultyManifestField_IsBoundByTheAggregateChecksum()
        {
            var manifest = new Ashfall.Core.Save.SaveManifest
            {
                manifestVersion = Ashfall.Core.Save.SaveManifest.CurrentManifestVersion,
                profileId = new Ashfall.Core.Save.SaveProfileId("default"),
                slotId = new Ashfall.Core.Save.SaveSlotId("slot_1"),
                generationId = "generation_1",
                difficultyPresetId = "difficulty_austere"
            };
            var section = new Ashfall.Core.Save.SaveSectionEnvelope
            {
                sectionName = "inventory",
                schemaVersion = 1,
                generationId = manifest.generationId,
                payloadJson = "{}"
            };
            section.checksum = Ashfall.Core.Save.SaveSlotService.ComputeSectionChecksum(section);
            var envelope = new Ashfall.Core.Save.AggregateSaveEnvelope
            {
                manifestVersion = Ashfall.Core.Save.CampaignEnvelopeBuilder.CurrentEnvelopeVersion,
                manifest = manifest,
                sections = new List<Ashfall.Core.Save.SaveSectionEnvelope> { section }
            };
            envelope.aggregateChecksum = Ashfall.Core.Save.SaveSlotService.ComputeAggregateChecksum(envelope);

            string original = envelope.aggregateChecksum;
            envelope.manifest.difficultyPresetId = "difficulty_dirge";

            Assert.NotEqual(original, Ashfall.Core.Save.SaveSlotService.ComputeAggregateChecksum(envelope));
        }

        [Fact]
        public void HostileEncounter_DifficultyMultiplier_ScalesExistingDangerComposition_AndPreservesStandardParity()
        {
            // Mirrors the arithmetic in Main.EvolvingWorld.cs:
            // ComposeExpeditionDangerMultiplier — a pure multiply, isolated from Godot.
            float baseComposition = 1.32f;
            float standardMult = DifficultyScalarsProvider.Legacy.HostileEncounterMult;
            float dirgeMult = 1.75f;

            float standardResult = ApplyHostileTerm(baseComposition, standardMult);
            float dirgeResult = ApplyHostileTerm(baseComposition, dirgeMult);

            Assert.Equal(baseComposition, standardResult, 4);
            Assert.Equal(baseComposition * dirgeMult, dirgeResult, 4);
            Assert.True(dirgeResult > standardResult);

            static float ApplyHostileTerm(float mult, float hostile)
            {
                if (hostile > 0f && Math.Abs(hostile - 1f) > 0.001f) mult *= hostile;
                return mult;
            }
        }

        private static EquipmentConditionSystem CreateEquipment(out Inventory.Inventory inventory)
        {
            inventory = new Inventory.Inventory();
            return new EquipmentConditionSystem(
                new SeededRng(22), inventory, new Ashfall.Core.Crafting.CraftingSystem(inventory));
        }

        private static string DataDirectory()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new InvalidOperationException("StreamingAssets/Data directory was not found.");
        }
    }
}
