// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingEdgeCaseTests
    {
        private static string FindDataDir()
        {
            var dir = Directory.GetCurrentDirectory();
            for (int i = 0; i < 10; i++)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                candidate = Path.Combine(dir, "assets", "StreamingAssets", "Data");
                if (Directory.Exists(candidate)) return candidate;
                dir = Path.GetDirectoryName(dir) ?? dir;
            }
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = WildlifeTrappingCatalogLoader.Load(FindDataDir(), fileIO, json);
            Assert.NotNull(catalog);
            return catalog!;
        }

        [Fact]
        public void TrySetTrap_UnknownTrap_FailsLookupAndLeavesInventoryIntact()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();
            inv.AddById("scrap_metal", 5);

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            string unknownTrapId = "trap_unknown_invented_id";
            bool exists = catalog.Traps.TryGetValue(unknownTrapId, out var trapDef);

            Assert.False(exists);
            Assert.Null(trapDef);

            // Preflight prevents transaction initiation; inventory remains intact
            Assert.Equal(5, inv.CountById("scrap_metal"));
            Assert.Empty(sys.State.trapSites);
        }

        [Fact]
        public void TrySetTrap_InsufficientMaterials_RollsBackTransactionCompletely()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();

            // trap_body_grip requires 4 scrap_metal and 1 leather_strap
            var trapDef = catalog.Traps["trap_body_grip"];
            var setupBill = trapDef.CalculateSetupBill();

            inv.AddById("scrap_metal", 4);
            // 0 leather_strap provided

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Transaction attempt
            using (var tx = inv.BeginTransaction(setupBill))
            {
                Assert.False(tx.Validation.IsValid);
                Assert.False(tx.TryCommit());
            }

            // Transaction rollback: scrap_metal must NOT be consumed
            Assert.Equal(4, inv.CountById("scrap_metal"));
            Assert.Equal(0, inv.CountById("leather_strap"));
            Assert.Empty(sys.State.trapSites);
        }

        [Fact]
        public void TrySetTrap_ExactBalance_ConsumesToZeroAndDeploys()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();

            // trap_improvised_wire setupCosts from catalog
            var trapDef = catalog.Traps["trap_improvised_wire"];
            var setupBill = trapDef.CalculateSetupBill();

            foreach (var cost in setupBill.Costs)
            {
                inv.AddById(cost.ItemId, cost.Amount);
                Assert.Equal(cost.Amount, inv.CountById(cost.ItemId));
            }

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            using (var tx = inv.BeginTransaction(setupBill))
            {
                Assert.True(tx.Validation.IsValid);
                Assert.True(tx.TryCommit());
            }

            var setRes = sys.SetTrap("site_exact", "bait_grain_lure", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            Assert.True(setRes.IsSuccess);
            foreach (var cost in setupBill.Costs)
            {
                Assert.Equal(0, inv.CountById(cost.ItemId));
            }
            Assert.Single(sys.State.trapSites);

            var site = sys.State.trapSites[0];
            Assert.Equal("site_exact", site.siteId);
            Assert.Equal("trap_improvised_wire", site.trapId);
            Assert.Equal(3, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void TryRepairTrap_LegacyTrapWithoutDurabilityTracking_ReturnsBlocked()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.State.trapSites.Add(new TrapSite
            {
                siteId = "site_legacy_unrepairable",
                trapId = "",
                trapType = "snare",
                remainingDurability = -1,
                isBroken = false
            });

            // Attempt repair on untracked legacy trap
            var res = sys.RepairTrap("site_legacy_unrepairable", restoreDurability: 5);

            Assert.False(res.IsSuccess);
            Assert.Equal("not_tracked", res.FailureCode);
            Assert.Equal("trapping.durability_not_tracked", res.MessageKey);
        }

        [Fact]
        public void TryRepairTrap_UndamagedTrap_ReturnsBlocked()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            var trapDef = catalog.Traps["trap_improvised_wire"];
            sys.SetTrap("site_healthy", "bait_grain_lure", "hunter_dweller",
                trapDef.trapType, trapDef.trap_id, trapDef.checkIntervalDays, trapDef.durabilityChecks);

            // Attempt repair on undamaged trap
            var res = sys.RepairTrap("site_healthy", restoreDurability: trapDef.durabilityChecks);

            Assert.False(res.IsSuccess);
            Assert.Equal("not_damaged", res.FailureCode);
            Assert.Equal("trapping.not_damaged", res.MessageKey);
        }

        [Fact]
        public void TryRepairTrap_BrokenTrap_RestoresDurabilityAndClearsBrokenFlag()
        {
            var catalog = LoadCatalog();
            var inv = new Inventory.Inventory();
            var trapDef = catalog.Traps["trap_improvised_wire"];

            // Provide repair costs
            var repairBill = trapDef.CalculateRepairBill();
            foreach (var cost in repairBill.Costs)
            {
                inv.AddById(cost.ItemId, cost.Amount);
            }

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.State.trapSites.Add(new TrapSite
            {
                siteId = "site_broken_target",
                trapId = "trap_improvised_wire",
                trapType = trapDef.trapType,
                remainingDurability = 0,
                isBroken = true
            });

            // Execute transactional payment
            using (var tx = inv.BeginTransaction(repairBill))
            {
                Assert.True(tx.Validation.IsValid);
                Assert.True(tx.TryCommit());
            }

            var res = sys.RepairTrap("site_broken_target", trapDef.durabilityChecks);

            Assert.True(res.IsSuccess);
            var site = sys.State.trapSites[0];
            Assert.False(site.isBroken);
            Assert.Equal(trapDef.durabilityChecks, site.remainingDurability);

            // Repair costs consumed from inventory
            foreach (var cost in repairBill.Costs)
            {
                Assert.Equal(0, inv.CountById(cost.ItemId));
            }
        }

        [Fact]
        public void BrokenTrap_ConsumesZeroRngDraws_ComparedToZeroTraps()
        {
            const int seed = 777;

            // System A: Has 1 broken trap
            var rngA = new SeededRng(seed);
            var sysA = new WildlifeTrappingSystem(rngA);
            sysA.State.trapSites.Add(new TrapSite
            {
                siteId = "site_broken",
                trapId = "trap_snare",
                remainingDurability = 0,
                isBroken = true,
                setDay = 1,
                checkDay = 2
            });

            // System B: Has 0 traps
            var rngB = new SeededRng(seed);
            var sysB = new WildlifeTrappingSystem(rngB);

            // Advance days
            for (int day = 2; day <= 10; day++)
            {
                sysA.TickDay(day);
                sysB.TickDay(day);
            }

            // Both RNGs must have identical internal state and draw sequences
            Assert.Equal(rngA.NextDouble(), rngB.NextDouble());
            Assert.Equal(rngA.Next(0, 1000), rngB.Next(0, 1000));
            Assert.False(sysA.State.trapSites[0].hasCatch);
            Assert.True(sysA.State.trapSites[0].isBroken);
        }
    }
}
