// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Xunit;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Tests.Fixtures;

namespace Ashfall.Core.Tests.Tooling
{
    /// <summary>
    /// Plan 27A — Data Authority Fidelity & Invariant Tests (INV-27.1, INV-27.2).
    /// Asserts that the testing harness runs against the real shipped data authority
    /// (Assets/StreamingAssets/Data/items.json) and preserves behavioral field parity.
    /// </summary>
    public sealed class DataAuthorityFidelityTests
    {
        private static string ResolveDataDir() => CampaignFixture.ResolveAuthorityDataDir();

        [Fact]
        public void AuthorityBackedFixture_LoadsShippedCatalogDirectly()
        {
            var fixture = CampaignFixture.CreateAuthorityBacked();
            Assert.True(fixture.IsAuthorityBacked);
            Assert.True(fixture.Catalog.Count > 100, $"Expected shipped catalog to contain >100 items, got {fixture.Catalog.Count}");

            // Verify core survival items exist in the shipped catalog
            Assert.NotNull(fixture.Catalog.Get("canned_food"));
            Assert.NotNull(fixture.Catalog.Get("clean_water"));
            Assert.NotNull(fixture.Catalog.Get("gas_mask"));
            Assert.NotNull(fixture.Catalog.Get("hazmat_suit"));
        }

        [Fact]
        public void ShippedCatalog_KeyBehavioralFields_MatchAuthoredSpecifications()
        {
            var dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            // canned_food: hungerRestore 40, weight 0.5, tradeValue 12
            var cannedFood = catalog.Get("canned_food");
            Assert.NotNull(cannedFood);
            Assert.Equal(40f, cannedFood!.hungerRestore);
            Assert.Equal(0.5f, cannedFood.weight);
            Assert.True(cannedFood.tradeValue > 0f);

            // clean_water: thirstRestore 40, weight 0.5
            var cleanWater = catalog.Get("clean_water");
            Assert.NotNull(cleanWater);
            Assert.Equal(40f, cleanWater!.thirstRestore);
            Assert.Equal(0.5f, cleanWater.weight);

            // gas_mask: equipable face slot, radProtection 30, durability 100
            var gasMask = catalog.Get("gas_mask");
            Assert.NotNull(gasMask);
            Assert.True(gasMask!.isEquipable);
            Assert.Equal(EquipSlot.Face, gasMask.equipSlot);
            Assert.Equal(30f, gasMask.radProtection);
            Assert.Equal(100f, gasMask.durability);

            // hazmat_suit: equipable body slot, radProtection 80, durability 100
            var hazmatSuit = catalog.Get("hazmat_suit");
            Assert.NotNull(hazmatSuit);
            Assert.True(hazmatSuit!.isEquipable);
            Assert.Equal(EquipSlot.Body, hazmatSuit.equipSlot);
            Assert.Equal(80f, hazmatSuit.radProtection);
            Assert.Equal(100f, hazmatSuit.durability);
        }

        [Fact]
        public void ShippedCatalog_StructuralIntegrity_NoNegativeWeightsOrZeroStacks()
        {
            var dataDir = ResolveDataDir();
            var fileIO = new FileSystemIO();
            var serializer = new SystemTextJsonSerializer();
            var catalog = ItemCatalogLoader.LoadCatalog(dataDir, fileIO, serializer);

            foreach (var id in catalog.Ids)
            {
                var item = catalog.Get(id)!;
                Assert.False(string.IsNullOrEmpty(item.id), "Item has empty ID");
                Assert.True(item.stackMax > 0, $"Item {item.id} has invalid stackMax: {item.stackMax}");
                Assert.True(item.weight >= 0f, $"Item {item.id} has negative weight: {item.weight}");
            }
        }

        [Fact]
        public void SyntheticFixture_ExplicitlyDiffersFromAuthority()
        {
            var synthetic = CampaignFixture.CreateSynthetic();
            Assert.False(synthetic.IsAuthorityBacked);
            // Synthetic fixture contains minimal sample items (around 16)
            Assert.True(synthetic.Catalog.Count < 50, $"Synthetic fixture should be minimal, got {synthetic.Catalog.Count}");
        }
    }
}
