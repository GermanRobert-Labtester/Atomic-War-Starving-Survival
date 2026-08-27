using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class UnifiedInventoryOwnershipTests
    {
        [Fact]
        public void CanonicalAliases_MapLegacyAndPrefixedIds_ToAuthoritativeIds()
        {
            Assert.Equal("canned_food", ItemAliases.ToCanonical("item_canned_food"));
            Assert.Equal("clean_water", ItemAliases.ToCanonical("item_purified_water"));
            Assert.Equal("clean_water", ItemAliases.ToCanonical("item_clean_water"));
            Assert.Equal("fuel_canister", ItemAliases.ToCanonical("item_fuel_canister"));
            Assert.Equal("scrap_mechanical", ItemAliases.ToCanonical("item_scrap_metal"));
            Assert.Equal("scrap_electronic", ItemAliases.ToCanonical("item_electronics"));
            Assert.Equal("first_aid", ItemAliases.ToCanonical("item_first_aid_kit"));
            Assert.Equal("first_aid", ItemAliases.ToCanonical("first_aid_kit"));
            Assert.Equal("bandage", ItemAliases.ToCanonical("item_bandage"));
        }

        [Fact]
        public void Inventory_ImplementsPlayerInventoryPort_WithAtomicTransactions()
        {
            var inventory = new Ashfall.Core.Inventory.Inventory { Capacity = 10, MaxWeight = 50f };
            IPlayerInventoryPort port = inventory;

            port.TryProduce("canned_food", 10);
            port.TryProduce("clean_water", 10);

            Assert.Equal(10, port.CountById("canned_food"));
            Assert.Equal(10, port.CountById("item_canned_food")); // Alias resolution
            Assert.True(port.HasSufficient("canned_food", 5));
            Assert.True(port.HasSufficient("item_canned_food", 5));

            bool consumed = port.TryConsume("item_canned_food", 3);
            Assert.True(consumed);
            Assert.Equal(7, port.CountById("canned_food"));

            var bill = new Dictionary<string, int>
            {
                { "canned_food", 2 },
                { "clean_water", 4 }
            };

            bool billConsumed = port.TryConsumeBill(bill);
            Assert.True(billConsumed);
            Assert.Equal(5, port.CountById("canned_food"));
            Assert.Equal(6, port.CountById("clean_water"));
        }

        [Fact]
        public void HoldfastTrading_TransactsAgainstAuthoritativeInventory_AndMaintainsSeparateMerchantStock()
        {
            var catalog = new HoldfastCatalog();
            var itemDef = new HoldfastItemDefinition("canned_food", "Canned Food", "Survival rations", 10f, 1f);
            catalog.Items.Register(itemDef);

            var playerInventory = new Ashfall.Core.Inventory.Inventory();
            var tradeSession = new HoldfastTradeSession(catalog, startingValue: 100, playerInventory: playerInventory);

            // Merchant starts with stock 20, player has 0
            Assert.Equal(20, tradeSession.GetStock("canned_food"));
            Assert.Equal(0, tradeSession.GetHeld("canned_food"));
            Assert.Equal(0, playerInventory.CountById("canned_food"));

            // Buy 3 canned food
            var buyResult = tradeSession.Buy("canned_food", 3, "faction_the_office");
            Assert.True(buyResult.Success);
            Assert.Equal(17, tradeSession.GetStock("canned_food")); // Merchant stock decreased
            Assert.Equal(3, tradeSession.GetHeld("canned_food"));   // Player held increased
            Assert.Equal(3, playerInventory.CountById("canned_food")); // Unified authoritative ledger!
            Assert.Equal(70, tradeSession.PlayerValue);            // 100 - (3 * 10) = 70

            // Sell 1 canned food
            var sellResult = tradeSession.Sell("canned_food", 1, "faction_the_office");
            Assert.True(sellResult.Success);
            Assert.Equal(18, tradeSession.GetStock("canned_food")); // Merchant stock increased
            Assert.Equal(2, tradeSession.GetHeld("canned_food"));   // Player held decreased
            Assert.Equal(2, playerInventory.CountById("canned_food")); // Unified authoritative ledger!
            Assert.Equal(80, tradeSession.PlayerValue);            // 70 + (1 * 10) = 80
        }

        [Fact]
        public void LegacySaveMigration_MergesHoldings_WithoutDuplication()
        {
            var targetInventory = new Ashfall.Core.Inventory.Inventory();
            targetInventory.Add(new ItemDefinition { id = "canned_food", displayName = "Canned Food", stackMax = 99, weight = 1f }, 5);

            var legacySave = new HoldfastTradeSaveState
            {
                value = 50,
                held = new Dictionary<string, int>
                {
                    { "item_canned_food", 8 }, // 3 more than current 5
                    { "item_purified_water", 6 } // 6 new items
                }
            };

            int migrated = InventoryMigrator.MigrateHoldfastHeld(legacySave, targetInventory);

            Assert.Equal(9, migrated); // 3 additional food + 6 water
            Assert.Equal(8, targetInventory.CountById("canned_food"));
            Assert.Equal(6, targetInventory.CountById("clean_water"));
            Assert.Empty(legacySave.held); // Cleared to prevent subsequent duplicate migration
        }

        [Fact]
        public void CrossSystemFlow_HarvestToConsumeToTrade_OperatesOnSingleLedger()
        {
            var catalog = new HoldfastCatalog();
            var herbDef = new HoldfastItemDefinition("medicinal_herb", "Medicinal Herb", "Fresh herb", 5f, 0.5f);
            var medDef = new HoldfastItemDefinition("bandage", "Bandage", "Clean dressing", 15f, 0.2f);
            catalog.Items.Register(herbDef);
            catalog.Items.Register(medDef);

            var unifiedInventory = new Ashfall.Core.Inventory.Inventory();
            IPlayerInventoryPort port = unifiedInventory;
            var tradeSession = new HoldfastTradeSession(catalog, startingValue: 50, playerInventory: unifiedInventory);

            // Step 1: Greenhouse / production yields 4 medicinal herbs
            port.TryProduce("medicinal_herb", 4);
            Assert.Equal(4, unifiedInventory.CountById("medicinal_herb"));
            Assert.Equal(4, tradeSession.GetHeld("medicinal_herb"));

            // Step 2: Crafting / medical consumes 2 herbs to produce 1 bandage
            var craftingBill = new InventoryBill();
            craftingBill.AddCost("medicinal_herb", 2);
            craftingBill.AddGrant("bandage", 1);

            bool crafted = port.TryExecuteTransaction(craftingBill);
            Assert.True(crafted);
            Assert.Equal(2, unifiedInventory.CountById("medicinal_herb"));
            Assert.Equal(1, unifiedInventory.CountById("bandage"));

            // Step 3: Trade session sells the crafted bandage to the merchant
            var sellResult = tradeSession.Sell("bandage", 1, "faction_the_office");
            Assert.True(sellResult.Success);
            Assert.Equal(0, unifiedInventory.CountById("bandage"));
            Assert.Equal(65, tradeSession.PlayerValue); // 50 + 15 = 65

            // Step 4: Trade session buys 1 clean water with proceeds
            var waterDef = new HoldfastItemDefinition("clean_water", "Clean Water", "Purified water", 10f, 1f);
            catalog.Items.Register(waterDef);
            tradeSession.SetStock("clean_water", 5);

            var buyResult = tradeSession.Buy("clean_water", 1, "faction_the_office");
            Assert.True(buyResult.Success);
            Assert.Equal(1, unifiedInventory.CountById("clean_water"));
            Assert.Equal(55, tradeSession.PlayerValue); // 65 - 10 = 55
        }

        [Fact]
        public void ProvenanceRecords_TrackMutationCausality()
        {
            var rec1 = new InventoryProvenanceRecord("item_canned_food", -2, InventoryMutationSource.Consume, day: 3, context: "Daily ration consumption");
            var rec2 = new InventoryProvenanceRecord("scrap_mechanical", 5, InventoryMutationSource.Loot, day: 3, context: "Expedition iron cache");

            Assert.Equal("canned_food", rec1.ItemId);
            Assert.Equal(-2, rec1.Delta);
            Assert.Equal(InventoryMutationSource.Consume, rec1.Source);
            Assert.Contains("Daily ration consumption", rec1.ToString());

            Assert.Equal("scrap_mechanical", rec2.ItemId);
            Assert.Equal(5, rec2.Delta);
            Assert.Equal(InventoryMutationSource.Loot, rec2.Source);
            Assert.Contains("+5", rec2.ToString());
        }
    }
}
