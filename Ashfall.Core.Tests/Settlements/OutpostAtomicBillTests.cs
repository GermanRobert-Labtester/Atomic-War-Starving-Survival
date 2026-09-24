// SPDX-License-Identifier: MIT
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Settlements;
using Xunit;
using PlayerInventory = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Tests.Settlements
{
    public sealed class OutpostAtomicBillTests
    {
        private static OutpostSettlementSystem CreateSystem()
        {
            string path = Path.GetFullPath(Path.Combine(
                AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data/outposts.json"));
            if (!File.Exists(path))
                path = Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data/outposts.json");
            return OutpostSettlementSystem.FromJson(File.ReadAllText(path));
        }

        [Fact]
        public void EstablishAndSupply_ConsumeCanonicalBillsAtomically()
        {
            var system = CreateSystem();
            var inventory = new PlayerInventory { MaxWeight = 1000f };
            inventory.TryProduce("scrap_metal", 50);
            inventory.TryProduce("scrap_wood", 30);
            inventory.TryProduce("dried_rations", 19);

            Assert.False(system.TryEstablishOutpost("outpost_north_watch", inventory));
            Assert.False(system.GetInstance("outpost_north_watch")!.IsEstablished);
            Assert.Equal(50, inventory.CountById("scrap_metal"));
            Assert.Equal(30, inventory.CountById("scrap_wood"));
            Assert.Equal(19, inventory.CountById("dried_rations"));

            inventory.TryProduce("dried_rations", 1);
            Assert.True(system.TryEstablishOutpost("outpost_north_watch", inventory));
            Assert.True(system.GetInstance("outpost_north_watch")!.IsEstablished);
            Assert.Equal(0, inventory.CountById("scrap_metal"));
            Assert.Equal(0, inventory.CountById("scrap_wood"));
            Assert.Equal(0, inventory.CountById("dried_rations"));

            inventory.TryProduce("dried_rations", 2);
            Assert.False(system.TrySupplyOutpost("outpost_north_watch", "dried_rations", 3, inventory));
            Assert.Equal(2, inventory.CountById("dried_rations"));
            Assert.Equal(0, system.GetInstance("outpost_north_watch")!.RationReserve);

            Assert.True(system.TrySupplyOutpost("outpost_north_watch", "dried_rations", 2, inventory));
            Assert.Equal(0, inventory.CountById("dried_rations"));
            Assert.Equal(2, system.GetInstance("outpost_north_watch")!.RationReserve);
        }
    }
}
