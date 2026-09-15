// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class ItemDescriptionCatalogTests : CatalogTestBase
    {
        private static readonly IFileIO s_fileIO = new FileSystemIO();
        private static readonly IJsonSerializer s_serializer = new SystemTextJsonSerializer();

        [Fact]
        public void ItemDescriptionCatalogLoader_LoadsFromAuthoritativeData()
        {
            var result = ItemDescriptionCatalogLoader.LoadCatalogWithResult(DataDirectory, s_fileIO, s_serializer);

            Assert.True(result.IsSuccess, $"Failed to load item description catalog: {string.Join(", ", result.Messages.Select(m => m.Message))}");
            Assert.NotEmpty(result.Entries);

            var catalog = result.Entries[0];
            Assert.True(catalog.Count >= 180, $"Expected >= 180 descriptions, got {catalog.Count}");
        }

        [Fact]
        public void ItemDescriptionCatalog_DeduplicatesDuplicateIds()
        {
            var catalog = new ItemDescriptionCatalog();
            var entry1 = new ItemDescriptionEntry
            {
                ItemId = "test_item_unique",
                Category = "tool",
                BaseDescription = "First entry"
            };
            var entry2 = new ItemDescriptionEntry
            {
                ItemId = "test_item_unique",
                Category = "tool",
                BaseDescription = "Second duplicate entry"
            };

            Assert.True(catalog.Register(entry1));
            Assert.False(catalog.Register(entry2), "Second registration of identical ID should be rejected/deduplicated");
            Assert.Equal(1, catalog.Count);
            Assert.Equal("First entry", catalog.Get("test_item_unique")?.BaseDescription);
        }

        [Fact]
        public void ItemDescriptionCatalog_ResolvesCoreSurvivalItems()
        {
            string[] itemIds =
            {
                "clean_water", "canned_food", "irradiated_water", "iodine_pills", "anti_rad",
                "gas_mask", "hazmat_suit", "water_filter", "air_filter", "battery", "bandage",
                "medical_kit", "fuel", "cloth", "scrap_metal", "mechanical_parts",
                "electronic_scrap", "chemicals", "handheld_radio", "dosimeter", "geiger_counter"
            };
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var failures = new List<string>();

            foreach (var itemId in itemIds)
            {
                var entry = catalog.Get(itemId);
                if (entry == null)
                {
                    failures.Add($"{itemId}: catalog entry is missing");
                    continue;
                }

                if (string.IsNullOrWhiteSpace(entry.BaseDescription))
                    failures.Add($"{itemId}: BaseDescription is empty");
                if (string.IsNullOrWhiteSpace(entry.Category))
                    failures.Add($"{itemId}: Category is empty");
                if (string.IsNullOrWhiteSpace(entry.SensoryDetails))
                    failures.Add($"{itemId}: SensoryDetails is empty");
                if (string.IsNullOrWhiteSpace(entry.Hazards))
                    failures.Add($"{itemId}: Hazards is empty");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void ItemDescriptionCatalog_ResolvesCanonicalAliases()
        {
            var cases = new[]
            {
                (AliasId: "item_dosimeter_pen", ExpectedTargetId: "dosimeter"),
                (AliasId: "item_dosimeter", ExpectedTargetId: "dosimeter"),
                (AliasId: "item_geiger_m3", ExpectedTargetId: "geiger_counter"),
                (AliasId: "item_air_filter_hepa", ExpectedTargetId: "air_filter"),
                (AliasId: "item_desal_membrane", ExpectedTargetId: "water_filter"),
                (AliasId: "rad_away", ExpectedTargetId: "anti_rad"),
                (AliasId: "item_rad_away", ExpectedTargetId: "anti_rad"),
                (AliasId: "scrap_mechanical", ExpectedTargetId: "scrap_metal"),
                (AliasId: "item_scrap_mechanical", ExpectedTargetId: "scrap_metal"),
                (AliasId: "scrap_electronic", ExpectedTargetId: "electronic_scrap"),
                (AliasId: "item_scrap_electronic", ExpectedTargetId: "electronic_scrap"),
                (AliasId: "ammo_9x19", ExpectedTargetId: "ammo_9mm"),
                (AliasId: "water_purification_tablets", ExpectedTargetId: "survival_water_purification_tablets"),
                (AliasId: "jewelry", ExpectedTargetId: "luxury_jewelry"),
                (AliasId: "book", ExpectedTargetId: "luxury_book"),
                (AliasId: "rope", ExpectedTargetId: "material_rope")
            };
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var entry = catalog.Get(test.AliasId);
                if (entry == null)
                {
                    failures.Add($"{test.AliasId}: catalog entry is missing");
                    continue;
                }

                if (!string.Equals(test.ExpectedTargetId, entry.ItemId, StringComparison.OrdinalIgnoreCase))
                    failures.Add($"{test.AliasId}: expected {test.ExpectedTargetId}, got {entry.ItemId}");
                if (string.IsNullOrWhiteSpace(entry.BaseDescription))
                    failures.Add($"{test.AliasId}: BaseDescription is empty");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void ItemDescriptionCatalog_GracefulDegradationOnUnknownItem()
        {
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);

            Assert.Null(catalog.Get(null));
            Assert.Null(catalog.Get(string.Empty));
            Assert.Null(catalog.Get("non_existent_item_9999"));
            Assert.False(catalog.Contains("non_existent_item_9999"));
        }

        [Fact]
        public void ItemInspectionModel_ComposesEnhancedDescriptionWithMechanics()
        {
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var def = new ItemDefinition
            {
                id = "dosimeter",
                displayName = "Pocket Dosimeter",
                description = "Fallback mechanical description",
                type = ItemType.Device,
                weight = 0.2f,
                radProtection = 0f,
                durability = 100f
            };

            var inspection = ItemInspectionModel.Create(def, catalog);

            Assert.NotNull(inspection);
            Assert.True(inspection.HasEnhancedDescription);
            Assert.Equal("dosimeter", inspection.ItemId);
            Assert.Equal("Pocket Dosimeter", inspection.DisplayName);
            Assert.NotEqual("Fallback mechanical description", inspection.BaseDescription);
            Assert.False(string.IsNullOrWhiteSpace(inspection.SensoryDetails));
            Assert.False(string.IsNullOrWhiteSpace(inspection.VisualIndicators));
            Assert.False(string.IsNullOrWhiteSpace(inspection.Hazards));
            Assert.Equal(0.2f, inspection.Weight);
            Assert.Equal(100f, inspection.Durability);
        }

        [Fact]
        public void ItemInspectionModel_FallsBackCleanlyWhenNoEnhancedProse()
        {
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var def = new ItemDefinition
            {
                id = "custom_mod_part_xyz",
                displayName = "Custom Mod Part",
                description = "Baseline mechanical description only.",
                type = ItemType.Material,
                weight = 1.5f,
                tradeValue = 25
            };

            var inspection = ItemInspectionModel.Create(def, catalog);

            Assert.NotNull(inspection);
            Assert.False(inspection.HasEnhancedDescription);
            Assert.Equal("custom_mod_part_xyz", inspection.ItemId);
            Assert.Equal("Baseline mechanical description only.", inspection.BaseDescription);
            Assert.Equal(string.Empty, inspection.SensoryDetails);
            Assert.Equal(string.Empty, inspection.VisualIndicators);
            Assert.Equal(string.Empty, inspection.Hazards);
            Assert.Equal(1.5f, inspection.Weight);
            Assert.Equal(25, inspection.TradeValue);
        }

        [Fact]
        public void ItemCatalogLoader_ExposesDescriptionCatalog()
        {
            var catalog = ItemCatalogLoader.LoadDescriptionCatalog(DataDirectory, s_fileIO, s_serializer);

            Assert.NotNull(catalog);
            Assert.True(catalog.Count >= 180);
            Assert.True(catalog.Contains("clean_water"));
        }
    }
}
