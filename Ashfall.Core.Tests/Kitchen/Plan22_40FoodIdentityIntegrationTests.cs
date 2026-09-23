// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 22 & Plan 40 Integration Tests:
// - Plan 22: One Food Authority: Eating, Spoilage, Preservation & Food Safety
// - Plan 40: Authored Personality Traits, Identity Enrichment & Observability
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Kitchen;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Kitchen
{
    public sealed class Plan22_40FoodIdentityIntegrationTests : CatalogTestBase
    {
        private static ExpansionEnrichmentCatalog LoadRealEnrichment()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new ExpansionEnrichmentCatalogLoader(files, json);
            return loader.Load(DataDirectory);
        }

        [Fact]
        public void Plan22_FoodTypeSystem_DifferentiatesSpoilageByPreservationAndTemperature()
        {
            var system = new FoodTypeSystem();
            string foodTypesPath = Path.Combine(DataDirectory, "food_types.json");
            Assert.True(File.Exists(foodTypesPath), $"food_types.json must exist at {foodTypesPath}");

            system.LoadCatalog(File.ReadAllText(foodTypesPath));
            system.SetStorageTemperature(20.0f); // standard ambient room temperature

            // Add perishable meat with various preservation methods
            var rawMeat = system.AddFood("food_meat", initialFreshness: 100.0f, preservation: "none", day: 1);
            var cellarMeat = system.AddFood("food_meat", initialFreshness: 100.0f, preservation: "root_cellar", day: 1);
            var cannedMeat = system.AddFood("food_meat", initialFreshness: 100.0f, preservation: "canning", day: 1);

            Assert.NotNull(rawMeat);
            Assert.NotNull(cellarMeat);
            Assert.NotNull(cannedMeat);

            // Tick 1 day
            system.TickDay(2);

            // Raw meat base is 2 days -> loses 50% in 1 day
            Assert.Equal(50.0f, rawMeat.FreshnessPercent, precision: 1);
            // Root cellar is 0.50x multiplier -> loses 25% in 1 day
            Assert.Equal(75.0f, cellarMeat.FreshnessPercent, precision: 1);
            // Canning is 0.05x multiplier -> loses 2.5% in 1 day
            Assert.Equal(97.5f, cannedMeat.FreshnessPercent, precision: 1);

            // Verify preservation hierarchy: Canned > Root Cellar > Unpreserved
            Assert.True(cannedMeat.FreshnessPercent > cellarMeat.FreshnessPercent);
            Assert.True(cellarMeat.FreshnessPercent > rawMeat.FreshnessPercent);
        }

        [Fact]
        public void Plan22_FoodTypeSystem_TracksFreshnessDegradation_AndTransitionsToSpoiledState()
        {
            var system = new FoodTypeSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            system.SetStorageTemperature(20.0f);

            string? spoiledItemId = null;
            string? spoiledTypeId = null;
            system.OnFoodSpoiled += (itemId, foodTypeId) =>
            {
                spoiledItemId = itemId;
                spoiledTypeId = foodTypeId;
            };

            // food_vegetables base_spoilage_days = 5.0 (20% loss per day at 20°C)
            var veg = system.AddFood("food_vegetables", initialFreshness: 55.0f, preservation: "none", day: 1);
            Assert.NotNull(veg);

            Assert.Equal("Fresh", system.CheckFoodSafety(veg.ItemId));

            // Tick day 2: 55% - 20% = 35% -> Aging (>25%, <=50%)
            system.TickDay(2);
            Assert.Equal("Aging", system.CheckFoodSafety(veg.ItemId));
            Assert.False(veg.IsSpoiled);
            Assert.Null(spoiledItemId);

            // Tick day 3: 35% - 20% = 15% -> drops below 20% spoilage threshold
            system.TickDay(3);
            Assert.True(veg.IsSpoiled);
            Assert.Equal("Spoiling", system.CheckFoodSafety(veg.ItemId));
            Assert.Equal(veg.ItemId, spoiledItemId);
            Assert.Equal("food_vegetables", spoiledTypeId);

            // Severely degraded food (<= 10% freshness) is classified as Spoiled
            var rotten = system.AddFood("food_vegetables", initialFreshness: 5.0f, preservation: "none", day: 3);
            Assert.NotNull(rotten);
            Assert.True(rotten.IsSpoiled);
            Assert.Equal("Spoiled", system.CheckFoodSafety(rotten.ItemId));

            // Save / Restore roundtrip
            var captured = system.CaptureState();
            Assert.NotNull(captured);
            Assert.Equal(2, captured.FoodItems.Count);

            var restoredSystem = new FoodTypeSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            restoredSystem.RestoreState(captured);

            var restoredVeg = restoredSystem.GetFoodItem(veg.ItemId);
            Assert.NotNull(restoredVeg);
            Assert.True(restoredVeg.IsSpoiled);
            Assert.Equal(veg.FreshnessPercent, restoredVeg.FreshnessPercent, precision: 2);
            Assert.Equal(2, restoredSystem.GetSpoiledFoodCount());
            Assert.Equal(0, restoredSystem.GetFreshFoodCount());
        }

        [Fact]
        public void Plan40_ExpansionEnrichment_LoadsAuthoredIdentities_WithoutHeuristicGuessing()
        {
            var catalog = LoadRealEnrichment();
            Assert.True(catalog.SurvivorFieldCount > 0, "Catalog must load authored survivor fields.");
            Assert.True(catalog.ItemTagCount > 0, "Catalog must load authored item tags.");

            // Verify the_veteran baseline fields
            var veteran = catalog.GetSurvivorFields("the_veteran");
            Assert.NotNull(veteran);
            Assert.Equal("military_discipline", veteran.belief_profile_id);
            Assert.Equal("folded_flag", veteran.personal_keepsake_item_id);
            Assert.Equal("stoicism", veteran.philosophical_stance);

            // Verify queries by belief profile and keepsake tags
            var disciplineSurvivors = catalog.GetSurvivorsByBeliefProfile("military_discipline");
            Assert.Contains("the_veteran", disciplineSurvivors);

            var keepsakes = catalog.GetKeepsakeCandidates();
            Assert.NotEmpty(keepsakes);
            Assert.Contains("teddy_bear", keepsakes);
        }

        [Fact]
        public void Plan40_SurvivorEnrichmentService_ObservabilitySlate_ProjectsTruthfulIdentityWithoutStatInflation()
        {
            var catalog = LoadRealEnrichment();
            var service = new SurvivorEnrichmentService(catalog);

            // Enriched survivor
            var slate = service.GetObservabilitySlate("the_veteran");
            Assert.NotNull(slate);
            Assert.True(slate.IsEnriched);
            Assert.Equal("the_veteran", slate.SurvivorId);
            Assert.Equal("stoicism", slate.PhilosophicalStance);
            Assert.Equal("military_discipline_code", slate.ManifestoLawCode);
            Assert.NotEmpty(slate.PrimaryDutyAffinity);

            // Unenriched survivor receives truthful defaults
            var unenrichedSlate = service.GetObservabilitySlate("non_existent_survivor");
            Assert.NotNull(unenrichedSlate);
            Assert.False(unenrichedSlate.IsEnriched);
            Assert.Equal("Unspecified", unenrichedSlate.ProfessionLabel);
            Assert.Equal("Undeclared", unenrichedSlate.BeliefProfileLabel);
            Assert.Equal("None", unenrichedSlate.KeepsakeItemLabel);
            Assert.Equal(0, unenrichedSlate.DutyComfortBonusPermille);
        }
    }
}
