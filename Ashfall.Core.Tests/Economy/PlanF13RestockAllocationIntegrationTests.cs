// SPDX-License-Identifier: MIT
// F13-C integration: the canonical merchant restock path (ShelterBarterSystem)
// now drives the signed RestockAllocationEngine instead of copying quantities.
// A capacity of 0 reproduces the legacy full restock exactly (one "general"
// category, target par = authored quantity); an authored/provided capacity
// bounds the manifest. The engine-level suite (RestockAllocationEngineTests)
// already covers the pure allocation math.
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Economy;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;
using Xunit;

namespace Ashfall.Core.Tests.Economy
{
    public sealed class PlanF13RestockAllocationIntegrationTests
    {
        private static ShelterBarterSystem CreateBarter()
            => new(new SeededRng(13), new InventoryContainer());

        private static MerchantCaravanDef Caravan(string id, int capacity = 0)
            => new()
            {
                caravan_id = id,
                name = "Test Caravan " + id,
                restock_capacity = capacity,
                stock = new List<CaravanStockItem>
                {
                    new() { item_id = "item_fuel", quantity = 10 },
                    new() { item_id = "item_canned_food", quantity = 5 }
                }
            };

        [Fact]
        public void Full_Restock_Reproduces_Authored_Quantities_Through_The_Engine()
        {
            var barter = CreateBarter();
            barter.RegisterCaravan(Caravan("caravan_full"));

            var remaining = barter.State.caravans["caravan_full"].remainingStock;
            Assert.Equal(10, remaining["item_fuel"]);
            Assert.Equal(5, remaining["item_canned_food"]);
            Assert.Equal(15, barter.LastRestockCapacityAllocated);
        }

        [Fact]
        public void Authored_Capacity_Bounds_The_Manifest()
        {
            var barter = CreateBarter();
            barter.RegisterCaravan(Caravan("caravan_capped", capacity: 4));

            var remaining = barter.State.caravans["caravan_capped"].remainingStock;
            Assert.Equal(4, barter.LastRestockCapacityAllocated);
            Assert.Equal(4, remaining["item_fuel"] + remaining["item_canned_food"]);
        }

        [Fact]
        public void Capacity_Provider_Overrides_Authored_Capacity()
        {
            var barter = CreateBarter();
            barter.RestockCapacityProvider = _ => 3;
            barter.RegisterCaravan(Caravan("caravan_provider"));

            var remaining = barter.State.caravans["caravan_provider"].remainingStock;
            Assert.Equal(3, barter.LastRestockCapacityAllocated);
            Assert.Equal(3, remaining["item_fuel"] + remaining["item_canned_food"]);
        }

        [Fact]
        public void Default_Caravans_Keep_Their_Authored_Stock_Under_The_Engine()
        {
            var barter = CreateBarter();
            foreach (var def in barter.Catalog.Values)
            {
                var remaining = barter.State.caravans[def.caravan_id].remainingStock;
                foreach (var item in def.stock)
                {
                    // Plan 147 day gate may zero future-gated stock; all built-in
                    // Default caravans gate at day 0, so authored quantity stands.
                    if (item.available_from_day <= 0)
                        Assert.Equal(item.quantity, remaining[item.item_id]);
                }
            }
        }
    }
}
