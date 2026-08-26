using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerContrabandCatalogTests
    : CatalogTestBase{
        private readonly string _catalogPath;

        public BunkerContrabandCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            _catalogPath = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "narrative", "bunker_contraband_barter.json");
            if (!File.Exists(_catalogPath))
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "narrative", "bunker_contraband_barter.json");
            }
        }

        [Fact]
        public void BunkerContrabandCatalog_LoadsAll20Entries()
        {
            Assert.True(File.Exists(_catalogPath), $"Catalog file not found at: {_catalogPath}");

            var catalog = BunkerContrabandCatalog.LoadFromFile(_catalogPath);
            Assert.NotNull(catalog);
            Assert.Equal(20, catalog.Count);
            Assert.Equal(20, catalog.All.Count);
        }

        [Fact]
        public void BunkerContrabandCatalog_AllEntriesHaveValidFields()
        {
            var catalog = BunkerContrabandCatalog.LoadFromFile(_catalogPath);

            var seenIds = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var item in catalog.All)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Id), "Item ID should not be empty");
                Assert.StartsWith("contraband_", item.Id);
                Assert.True(seenIds.Add(item.Id), $"Duplicate contraband ID: {item.Id}");

                Assert.False(string.IsNullOrWhiteSpace(item.Title), $"Item {item.Id} missing Title");
                Assert.False(string.IsNullOrWhiteSpace(item.Category), $"Item {item.Id} missing Category");
                Assert.InRange(item.ContrabandTier, 1, 3);
                Assert.False(string.IsNullOrWhiteSpace(item.RiskProfile), $"Item {item.Id} missing RiskProfile");
                Assert.True(item.MarketPriceScrip > 0, $"Item {item.Id} should have positive scrip price");
                Assert.False(string.IsNullOrWhiteSpace(item.HiddenStashLocation), $"Item {item.Id} missing HiddenStashLocation");
                Assert.False(string.IsNullOrWhiteSpace(item.Prose), $"Item {item.Id} missing Prose");
                Assert.NotNull(item.Tags);
                Assert.NotEmpty(item.Tags);
            }
        }

        [Fact]
        public void BunkerContrabandCatalog_QueriesFunctionCorrectly()
        {
            var catalog = BunkerContrabandCatalog.LoadFromFile(_catalogPath);

            // By ID
            var coil = catalog.GetById("contraband_copper_condenser_coil");
            Assert.NotNull(coil);
            Assert.Equal("distillation", coil.Category);
            Assert.Equal(45, coil.MarketPriceScrip);

            // By Category
            var luxury = catalog.GetByCategory("luxury_rations");
            Assert.True(luxury.Count >= 3, "Expected at least 3 luxury ration contraband items");

            // By Tier
            var tier3 = catalog.GetByTier(3);
            Assert.True(tier3.Count >= 5, "Expected at least 5 high-treason tier 3 items");

            // By Tag
            var seeds = catalog.GetByTag("century_seed");
            Assert.Single(seeds);
            Assert.Equal("contraband_century_seed_grain_vial", seeds[0].Id);
        }
    }
}
