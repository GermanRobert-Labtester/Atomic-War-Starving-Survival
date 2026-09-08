// SPDX-License-Identifier: MIT
using System;
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

        [Theory]
        [InlineData("clean_water")]
        [InlineData("canned_food")]
        [InlineData("irradiated_water")]
        [InlineData("iodine_pills")]
        [InlineData("anti_rad")]
        [InlineData("gas_mask")]
        [InlineData("hazmat_suit")]
        [InlineData("water_filter")]
        [InlineData("air_filter")]
        [InlineData("battery")]
        [InlineData("bandage")]
        [InlineData("medical_kit")]
        [InlineData("fuel")]
        [InlineData("cloth")]
        [InlineData("scrap_metal")]
        [InlineData("mechanical_parts")]
        [InlineData("electronic_scrap")]
        [InlineData("chemicals")]
        [InlineData("handheld_radio")]
        [InlineData("dosimeter")]
        [InlineData("geiger_counter")]
        public void ItemDescriptionCatalog_ResolvesCoreSurvivalItems(string itemId)
        {
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var entry = catalog.Get(itemId);

            Assert.NotNull(entry);
            Assert.False(string.IsNullOrWhiteSpace(entry.BaseDescription), $"BaseDescription missing for {itemId}");
            Assert.False(string.IsNullOrWhiteSpace(entry.Category), $"Category missing for {itemId}");
            Assert.False(string.IsNullOrWhiteSpace(entry.SensoryDetails), $"SensoryDetails missing for {itemId}");
            Assert.False(string.IsNullOrWhiteSpace(entry.Hazards), $"Hazards missing for {itemId}");
        }

        [Theory]
        [InlineData("item_dosimeter_pen", "dosimeter")]
        [InlineData("item_dosimeter", "dosimeter")]
        [InlineData("item_geiger_m3", "geiger_counter")]
        [InlineData("item_air_filter_hepa", "air_filter")]
        [InlineData("item_desal_membrane", "water_filter")]
        [InlineData("rad_away", "anti_rad")]
        [InlineData("item_rad_away", "anti_rad")]
        [InlineData("scrap_mechanical", "scrap_metal")]
        [InlineData("item_scrap_mechanical", "scrap_metal")]
        [InlineData("scrap_electronic", "electronic_scrap")]
        [InlineData("item_scrap_electronic", "electronic_scrap")]
        [InlineData("ammo_9x19", "ammo_9mm")]
        [InlineData("water_purification_tablets", "survival_water_purification_tablets")]
        [InlineData("jewelry", "luxury_jewelry")]
        [InlineData("book", "luxury_book")]
        [InlineData("rope", "material_rope")]
        public void ItemDescriptionCatalog_ResolvesCanonicalAliases(string aliasId, string expectedTargetId)
        {
            var catalog = ItemDescriptionCatalogLoader.LoadCatalog(DataDirectory, s_fileIO, s_serializer);
            var entry = catalog.Get(aliasId);

            Assert.NotNull(entry);
            Assert.Equal(expectedTargetId, entry.ItemId, ignoreCase: true);
            Assert.False(string.IsNullOrWhiteSpace(entry.BaseDescription));
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
