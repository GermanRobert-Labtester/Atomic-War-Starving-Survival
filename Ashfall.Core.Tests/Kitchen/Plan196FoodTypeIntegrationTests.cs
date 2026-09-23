// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 196: Food Type Differentiation & Temperature Spoilage — Integration Tests
// Verifies food types catalog loading, categorical spoilage differentiation
// between perishable meats and shelf-stable grains, temperature modulation,
// preservation method effectiveness, and save/restore persistence.
// ============================================================================
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Kitchen;

namespace Ashfall.Core.Tests.Kitchen
{
    public sealed class Plan196FoodTypeIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsSevenFoodTypes()
        {
            var system = new FoodTypeSystem();
            string path = Path.Combine(DataDirectory, "food_types.json");
            Assert.True(File.Exists(path), $"food_types.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var types = system.GetAllFoodTypes();
            Assert.Equal(7, types.Count);

            var meat = system.GetFoodType("food_meat");
            Assert.NotNull(meat);
            Assert.Equal("Perishable", meat.category);
            Assert.Equal(2.0f, meat.base_spoilage_days);
            Assert.Equal(1.5f, meat.temperature_sensitivity);

            var grains = system.GetFoodType("food_grains");
            Assert.NotNull(grains);
            Assert.Equal("ShelfStable", grains.category);
            Assert.Equal(35.0f, grains.base_spoilage_days);
        }

        [Fact]
        public void AddFood_InitializesFreshnessAndPreservation()
        {
            var system = new FoodTypeSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));

            TrackedFoodItem? added = null;
            system.OnFoodAdded += f => added = f;

            var item = system.AddFood("food_meat", initialFreshness: 100.0f, preservation: "smoking", day: 1);

            Assert.NotNull(item);
            Assert.NotNull(added);
            Assert.Equal(1, system.TrackedItemCount);
            Assert.Equal("food_meat", item.FoodTypeId);
            Assert.Equal("smoking", item.PreservationMethod);
            Assert.Equal(100.0f, item.FreshnessPercent);
            Assert.False(item.IsSpoiled);
        }

        [Fact]
        public void TickDay_DifferentiatesSpoilageBetweenMeatAndGrains()
        {
            var system = new FoodTypeSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            system.SetStorageTemperature(20.0f); // 1.0x baseline

            var meat = system.AddFood("food_meat", 100.0f, preservation: "none", day: 1);
            var grains = system.AddFood("food_grains", 100.0f, preservation: "none", day: 1);
            Assert.NotNull(meat);
            Assert.NotNull(grains);

            system.TickDay(2);

            // Meat (base 2 days) loses 50% freshness in 1 day
            Assert.Equal(50.0f, meat.FreshnessPercent);

            // Grains (base 35 days) lose ~2.86% freshness in 1 day
            Assert.True(grains.FreshnessPercent > 96.0f);
        }

        [Fact]
        public void Temperature_ModulatesSpoilageRate()
        {
            var systemCold = new FoodTypeSystem();
            systemCold.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            systemCold.SetStorageTemperature(4.0f); // Cold storage (~0.40x)

            var systemWarm = new FoodTypeSystem();
            systemWarm.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            systemWarm.SetStorageTemperature(30.0f); // Warm storage (>1.4x)

            var coldVeg = systemCold.AddFood("food_vegetables", 100.0f, "none", day: 1);
            var warmVeg = systemWarm.AddFood("food_vegetables", 100.0f, "none", day: 1);
            Assert.NotNull(coldVeg);
            Assert.NotNull(warmVeg);

            systemCold.TickDay(2);
            systemWarm.TickDay(2);

            // Cold storage retains significantly more freshness
            Assert.True(coldVeg.FreshnessPercent > warmVeg.FreshnessPercent);
        }

        [Fact]
        public void Preservation_SignificantlyExtendsShelfLife()
        {
            var system = new FoodTypeSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            system.SetStorageTemperature(20.0f);

            var rawMeat = system.AddFood("food_meat", 100.0f, preservation: "none", day: 1);
            var cannedMeat = system.AddFood("food_meat", 100.0f, preservation: "canning", day: 1);
            Assert.NotNull(rawMeat);
            Assert.NotNull(cannedMeat);

            // Day 2
            system.TickDay(2);
            // Day 3 -> raw meat drops to 0 and spoils, canned meat retains > 90%
            system.TickDay(3);

            Assert.True(rawMeat.IsSpoiled);
            Assert.False(cannedMeat.IsSpoiled);
            Assert.True(cannedMeat.FreshnessPercent > 90.0f);
            Assert.Equal("Fresh", system.CheckFoodSafety(cannedMeat.ItemId));
        }

        [Fact]
        public void SaveRestoreState_PreservesFoodItemsAndFreshness()
        {
            var system = new FoodTypeSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            system.SetStorageTemperature(12.0f);

            var f = system.AddFood("food_dairy", 80.0f, preservation: "refrigeration", day: 3);
            Assert.NotNull(f);

            var state = system.CaptureState();

            var restoredSystem = new FoodTypeSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "food_types.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal(12.0f, restoredSystem.StorageTemperatureC);
            Assert.Equal(1, restoredSystem.TrackedItemCount);

            var restoredItem = restoredSystem.GetFoodItem(f.ItemId);
            Assert.NotNull(restoredItem);
            Assert.Equal("food_dairy", restoredItem.FoodTypeId);
            Assert.Equal(80.0f, restoredItem.FreshnessPercent);
            Assert.Equal("refrigeration", restoredItem.PreservationMethod);
        }
    }
}
