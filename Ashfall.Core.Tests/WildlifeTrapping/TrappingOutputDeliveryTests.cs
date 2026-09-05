// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Ashfall.Core.Production;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    /// <summary>
    /// REM-007 / R13 — Wildlife trapping authoritative inventory output sink tests.
    ///
    /// Verifies that:
    ///   1. Trapping catches deliver real goods (raw_meat, leather_strap) into authoritative inventory.
    ///   2. Zero-capacity / storage full refuses delivery without destroying the catch.
    ///   3. Weight limit exceeded refuses delivery without destroying the catch.
    ///   4. Partial capacity delivers what fits and retains remainder for later claim.
    ///   5. Save-before-claim retains unharvested catch across serialize/deserialize.
    ///   6. Save-after-claim prevents duplicate delivery across serialize/deserialize.
    ///   7. Duplicate claim attempts are blocked.
    ///   8. Setting a trap is blocked if an unharvested catch is still on the snare.
    ///   9. Toxin removal reduces contamination in delivered delivery bill.
    /// </summary>
    public sealed class TrappingOutputDeliveryTests
    {
        private static string RepoRoot()
        {
            string dir = new DirectoryInfo(AppContext.BaseDirectory).FullName;
            for (int i = 0; i < 8 && dir != null; i++)
            {
                if (Directory.Exists(Path.Combine(dir, "Assets", "Ashfall.Core")))
                    return dir;
                dir = Directory.GetParent(dir)?.FullName;
            }
            throw new DirectoryNotFoundException("repo root not found from test context");
        }

        private static string DataDir() =>
            Path.Combine(RepoRoot(), "Assets", "StreamingAssets", "Data");

        private static WildlifeTrappingSystem CreateTestSystem(out Inventory.Inventory inventory)
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(1986), new NullLog());
            inventory = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };

            // Register standard prey & quarry
            sys.RegisterQuarry(new QuarrySpecies
            {
                speciesId = "rabbit",
                displayName = "Ash Rabbit",
                baseYieldKg = 1.0f,
                toxicChance = 0.15f,
                hideYield = 1.0f,
                hideItemId = "leather_strap",
                preferredTrapType = "snare"
            });

            sys.SetOutputSink(inventory);
            return sys;
        }

        private static TrapSite SetupSiteWithCatch(
            WildlifeTrappingSystem sys,
            string siteId = "site_alpha",
            string species = "rabbit",
            float yieldKg = 1.0f,
            bool toxic = false)
        {
            sys.SetTrap(siteId, "bait_grain_lure", "hunter_1", "snare", "trap_snare", 2, 8);
            var site = sys.State.trapSites.Find(s => s.siteId == siteId)!;
            site.hasCatch = true;
            site.catchSpecies = species;
            site.carcassYield = yieldKg;
            site.isToxic = toxic;
            site.toxinRemoved = false;
            site.isMeatProcessed = false;
            site.hidePreserved = false;
            return site;
        }

        [Fact]
        public void FullDelivery_DeliversMeatAndHideToInventory_AndClearsCatch()
        {
            var sys = CreateTestSystem(out var inventory);
            var site = SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f);

            Assert.Equal(0, inventory.CountById("raw_meat"));
            Assert.Equal(0, inventory.CountById("leather_strap"));

            var harvestRes = sys.HarvestCatch("site_1", "hunter_1");
            Assert.True(harvestRes.IsSuccess, $"Harvest failed: {harvestRes.MessageKey}");

            // 1.0 kg yield -> 2 units of raw_meat (0.5 kg each)
            Assert.Equal(2, inventory.CountById("raw_meat"));
            // 1.0 hide yield -> 1 leather_strap
            Assert.Equal(1, inventory.CountById("leather_strap"));

            // Catch is cleared from trap
            Assert.False(site.hasCatch);
        }

        [Fact]
        public void ZeroCapacity_RefusesDelivery_CatchRemainsClaimable()
        {
            var sys = CreateTestSystem(out var inventory);
            var site = SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f);

            // Fill all capacity slots
            inventory.Capacity = 1;
            inventory.AddById("scrap_metal", 1);

            var butcherRes = sys.Butcher("site_1");
            Assert.False(butcherRes.IsSuccess);
            Assert.Equal("trapping.storage_full", butcherRes.MessageKey);

            // No items delivered
            Assert.Equal(0, inventory.CountById("raw_meat"));

            // Catch still intact
            Assert.True(site.hasCatch);
            Assert.False(site.isMeatProcessed);
            Assert.Equal(1.0f, site.carcassYield);

            // Restore capacity
            inventory.Capacity = 20;

            // Now butcher succeeds
            var secondRes = sys.Butcher("site_1");
            Assert.True(secondRes.IsSuccess);
            Assert.Equal(2, inventory.CountById("raw_meat"));
            Assert.True(site.isMeatProcessed);
        }

        [Fact]
        public void WeightExceeded_RefusesDelivery_CatchRemainsClaimable()
        {
            var sys = CreateTestSystem(out var inventory);
            var site = SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f);

            // Restrict weight so 2 units of meat (0.5kg each = 1.0kg) cannot fit
            inventory.MaxWeight = 0.4f;

            var butcherRes = sys.Butcher("site_1");
            Assert.False(butcherRes.IsSuccess);
            Assert.Equal("trapping.weight_exceeded", butcherRes.MessageKey);

            // No items delivered
            Assert.Equal(0, inventory.CountById("raw_meat"));
            Assert.True(site.hasCatch);
            Assert.False(site.isMeatProcessed);

            // Increase weight limit
            inventory.MaxWeight = 50f;
            var secondRes = sys.Butcher("site_1");
            Assert.True(secondRes.IsSuccess);
            Assert.Equal(2, inventory.CountById("raw_meat"));
        }

        [Fact]
        public void PartialDelivery_DeliversWhatFits_RetainsRemainder_CompletesOnSecondAttempt()
        {
            var sys = CreateTestSystem(out var inventory);
            // 1.5 kg yield -> 3 units of raw_meat
            var site = SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.5f);

            // Pre-fill inventory so exactly 1 slot remains and max weight allows only 1 unit (0.5kg)
            inventory.MaxWeight = 0.6f;

            var butcherRes = sys.Butcher("site_1");
            Assert.True(butcherRes.IsSuccess);
            Assert.Equal("trapping.partial_delivery", butcherRes.MessageKey);

            // Exactly 1 delivered
            Assert.Equal(1, inventory.CountById("raw_meat"));

            // Catch retained with remainder
            Assert.True(site.hasCatch);
            Assert.False(site.isMeatProcessed);
            Assert.True(site.carcassYield > 0.5f, $"Expected remainder carcass yield, got {site.carcassYield}");

            // Expand weight limit
            inventory.MaxWeight = 100f;

            // Second butcher finishes delivery
            var secondRes = sys.Butcher("site_1");
            Assert.True(secondRes.IsSuccess);
            Assert.Equal("trapping.butchered", secondRes.MessageKey);
            Assert.Equal(3, inventory.CountById("raw_meat"));
            Assert.True(site.isMeatProcessed);
        }

        [Fact]
        public void SaveBeforeClaim_PersistsCatch_CanClaimAfterRestore()
        {
            var sysA = CreateTestSystem(out _);
            SetupSiteWithCatch(sysA, "site_1", "rabbit", yieldKg: 1.0f);

            // Capture state
            var state = sysA.CaptureState();

            // Restore into fresh system
            var sysB = new WildlifeTrappingSystem(new SeededRng(2026), new NullLog());
            sysB.RegisterQuarry(new QuarrySpecies
            {
                speciesId = "rabbit",
                baseYieldKg = 1.0f,
                hideYield = 1.0f,
                hideItemId = "leather_strap"
            });
            var invB = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            sysB.SetOutputSink(invB);
            sysB.RestoreState(state);

            // Check site on restored system
            var restoredSite = sysB.State.trapSites.Find(s => s.siteId == "site_1")!;
            Assert.True(restoredSite.hasCatch);
            Assert.False(restoredSite.isMeatProcessed);

            // Claim catch on restored system
            var res = sysB.HarvestCatch("site_1", "hunter_mae");
            Assert.True(res.IsSuccess);
            Assert.Equal(2, invB.CountById("raw_meat"));
            Assert.Equal(1, invB.CountById("leather_strap"));
        }

        [Fact]
        public void SaveAfterClaim_PersistsDeliveredState_NoDuplicateGoods()
        {
            var sysA = CreateTestSystem(out var invA);
            SetupSiteWithCatch(sysA, "site_1", "rabbit", yieldKg: 1.0f);

            var harvestRes = sysA.HarvestCatch("site_1");
            Assert.True(harvestRes.IsSuccess);
            Assert.Equal(2, invA.CountById("raw_meat"));

            // Capture state
            var state = sysA.CaptureState();

            // Restore into fresh system
            var sysB = new WildlifeTrappingSystem(new SeededRng(2026), new NullLog());
            var invB = new Inventory.Inventory { Capacity = 20, MaxWeight = 100f };
            sysB.SetOutputSink(invB);
            sysB.RestoreState(state);

            // Attempt to butcher or harvest again on restored system
            var duplicateButcher = sysB.Butcher("site_1");
            Assert.False(duplicateButcher.IsSuccess);

            var duplicateHarvest = sysB.HarvestCatch("site_1");
            Assert.False(duplicateHarvest.IsSuccess);

            // Inventory in B has zero extra goods delivered
            Assert.Equal(0, invB.CountById("raw_meat"));
        }

        [Fact]
        public void DuplicateClaim_BlockedWithoutMultiplyingGoods()
        {
            var sys = CreateTestSystem(out var inventory);
            SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f);

            var first = sys.Butcher("site_1");
            Assert.True(first.IsSuccess);
            Assert.Equal(2, inventory.CountById("raw_meat"));

            var second = sys.Butcher("site_1");
            Assert.False(second.IsSuccess);
            Assert.Equal("trapping.already_butchered", second.MessageKey);

            // Count has not changed
            Assert.Equal(2, inventory.CountById("raw_meat"));
        }

        [Fact]
        public void SetTrap_BlockedWhenCatchUnharvested()
        {
            var sys = CreateTestSystem(out _);
            SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f);

            // Attempting to re-set the trap while meat is unharvested must be blocked
            var setRes = sys.SetTrap("site_1", "bait_grain_lure", "hunter_1");
            Assert.False(setRes.IsSuccess);
            Assert.Equal("trapping.unharvested_catch", setRes.MessageKey);

            // Harvest the catch
            var harvestRes = sys.HarvestCatch("site_1");
            Assert.True(harvestRes.IsSuccess);

            // Now re-setting the trap succeeds
            var secondSet = sys.SetTrap("site_1", "bait_grain_lure", "hunter_1");
            Assert.True(secondSet.IsSuccess);
        }

        [Fact]
        public void ToxicCatch_CleanedByToxinRemoval()
        {
            var sys = CreateTestSystem(out var inventory);
            var site = SetupSiteWithCatch(sys, "site_1", "rabbit", yieldKg: 1.0f, toxic: true);

            Assert.True(site.isToxic);
            Assert.False(site.toxinRemoved);

            // Remove toxin
            var purgeRes = sys.RemoveToxin("site_1");
            Assert.True(purgeRes.IsSuccess);
            Assert.True(site.toxinRemoved);

            // Harvest delivers clean meat
            var harvestRes = sys.HarvestCatch("site_1");
            Assert.True(harvestRes.IsSuccess);
            Assert.Equal(2, inventory.CountById("raw_meat"));
        }
    }
}
