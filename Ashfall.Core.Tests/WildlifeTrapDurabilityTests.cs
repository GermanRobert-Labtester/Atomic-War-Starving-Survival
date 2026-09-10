// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Trapping Integration — Task 4:
    /// Durability Semantics, Outcome Independence, Off-Schedule & Ineligible Protection, and Save Continuity.
    /// </summary>
    public sealed class WildlifeTrapDurabilityTests
    {
        private static string FindDataDir()
        {
            if (CatalogLocator.TryFindDataDirectory(Directory.GetCurrentDirectory(), out var dir)) return dir;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out dir)) return dir;
            return "Assets/StreamingAssets/Data";
        }

        private static WildlifeTrappingCatalog LoadCatalog()
        {
            string dataDir = FindDataDir();
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var traps = WildlifeTrappingCatalogLoader.Load(dataDir, fileIO, json);
            Assert.NotNull(traps);
            return traps!;
        }

        // ====================================================================
        // WORKSTREAM A: Exactly 1 durability consumed on eligible check
        // ====================================================================

        [Fact]
        public void CheckTraps_OnEligibleDay_ConsumesExactlyOneDurability()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Set trap on Day 1 with check interval 1, durability 5 -> checkDay is Day 2
            sys.SetTrap("site_alpha", "bait_grain_lure", "hunter_1", "box", "trap_box", 1, 5);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_alpha");
            Assert.Equal(5, site.remainingDurability);
            Assert.Equal(2, site.checkDay);

            // Advance day to checkDay (Day 2)
            sys.TickDay(2);
            Assert.Equal(4, site.remainingDurability);
        }

        // ====================================================================
        // WORKSTREAM B: Outcome independence (Catch vs Miss)
        // ====================================================================

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void DurabilityLoss_IsIndependentOfCatchOutcome(bool forceCatchSuccess)
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(12345));
            catalog.RegisterWith(sys);

            // Set trap on Day 1 with interval 1 -> checkDay is Day 2
            sys.SetTrap("site_beta", "bait_grain_lure", "hunter_1", "box", "trap_box", 1, 10);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_beta");

            if (forceCatchSuccess)
            {
                sys.SetHunterSkill(100f);
                sys.TickDay(2, densityMultiplier: 10.0f);
                Assert.True(site.hasCatch);
            }
            else
            {
                sys.TickDay(2, densityMultiplier: 0.0f); // 0 catch chance
                Assert.False(site.hasCatch);
            }

            // In both cases, durability must decrease by exactly 1
            Assert.Equal(9, site.remainingDurability);
        }

        // ====================================================================
        // WORKSTREAM C: Check intervals (1, 2, 3) and off-schedule protection
        // ====================================================================

        [Theory]
        [InlineData(1, 2)]
        [InlineData(2, 3)]
        [InlineData(3, 4)]
        public void DurabilityOnlyDecrements_OnScheduledCheckDay(int intervalDays, int expectedCheckDay)
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(777));
            catalog.RegisterWith(sys);

            // Set trap on Day 1 -> checkDay is 1 + intervalDays
            sys.SetTrap("site_gamma", "bait_grain_lure", "hunter_1", "box", "trap_box", intervalDays, 10);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_gamma");
            Assert.Equal(expectedCheckDay, site.checkDay);

            for (int day = 2; day < expectedCheckDay; day++)
            {
                sys.TickDay(day);
                // Off-schedule: 0 durability lost
                Assert.Equal(10, site.remainingDurability);
            }

            // On the check day: exactly 1 durability lost
            sys.TickDay(expectedCheckDay);
            Assert.Equal(9, site.remainingDurability);
        }

        // ====================================================================
        // WORKSTREAM D: Ineligible state protection (broken, unset, pending catch)
        // ====================================================================

        [Fact]
        public void BrokenTrap_SuffersZeroDurabilityLossAndProducesNoCatch()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(999));
            catalog.RegisterWith(sys);

            sys.SetTrap("site_broken", "bait_grain_lure", "hunter_1", "box", "trap_box", 1, 1);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_broken");

            // Day 2: durability drops from 1 to 0 -> becomes broken
            sys.TickDay(2);
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);

            // Clear catch if any, to isolate broken check
            site.hasCatch = false;

            // Day 3: trap is broken -> must not decrement durability, must not catch
            sys.TickDay(3);
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);
            Assert.False(site.hasCatch);
        }

        [Fact]
        public void UnsetTrap_SuffersZeroDurabilityLoss()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.State.trapSites.Add(new TrapSite
            {
                siteId = "site_unset",
                trapId = "trap_box",
                trapType = "box",
                setDay = -1,
                checkDay = 0,
                checkIntervalDays = 1,
                remainingDurability = 5,
                isBroken = false
            });

            var site = sys.State.trapSites.Single(s => s.siteId == "site_unset");
            sys.TickDay(1);
            sys.TickDay(2);

            Assert.Equal(5, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void TrapWithPendingCatch_SuffersZeroDurabilityLossUntilProcessed()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            sys.SetTrap("site_catch", "bait_grain_lure", "hunter_1", "box", "trap_box", 1, 8);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_catch");

            // Force a pending catch on Day 2
            site.hasCatch = true;
            site.catchSpecies = "rabbit";
            site.checkDay = 2;

            // Tick day 2
            sys.TickDay(2);
            // Since hasCatch was already true, the trap was skipped
            Assert.Equal(8, site.remainingDurability);
        }

        // ====================================================================
        // WORKSTREAM E: Durability never drops below zero
        // ====================================================================

        [Fact]
        public void Durability_ClampedAtZero_NeverNegative()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(123));
            catalog.RegisterWith(sys);

            // Set trap on Day 1 with durability 2, interval 1
            sys.SetTrap("site_clamp", "bait_grain_lure", "hunter_1", "box", "trap_box", 1, 2);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_clamp");

            // Day 2: 2 -> 1 (checkDay becomes 3)
            sys.TickDay(2);
            Assert.Equal(1, site.remainingDurability);
            Assert.False(site.isBroken);

            // Day 3: 1 -> 0, broken = true
            sys.TickDay(3);
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);

            // Day 4, 5: broken trap stays at 0
            sys.TickDay(4);
            sys.TickDay(5);
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.remainingDurability >= 0);
        }

        // ====================================================================
        // WORKSTREAM F: Persistence & Save Continuity
        // ====================================================================

        [Fact]
        public void DurabilityAndBrokenState_PreservedAcrossSaveLoadRoundTrip()
        {
            var catalog = LoadCatalog();
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Set trap on Day 1 with interval 2, durability 7 -> checkDay is Day 3
            sys.SetTrap("site_save", "bait_grain_lure", "hunter_1", "box", "trap_box", 2, 7);
            var site = sys.State.trapSites.Single(s => s.siteId == "site_save");

            // Day 2: off-schedule (durability stays 7)
            sys.TickDay(2);
            Assert.Equal(7, site.remainingDurability);

            // Day 3: scheduled check day (durability drops to 6, checkDay becomes 5)
            sys.TickDay(3);
            Assert.Equal(6, site.remainingDurability);
            Assert.Equal(5, site.checkDay);

            // Capture state
            var state = sys.CaptureState();
            var json = new SystemTextJsonSerializer();
            string serialized = json.Serialize(state);

            // Restore in fresh system
            var loadedState = json.Deserialize<WildlifeTrappingState>(serialized);
            Assert.NotNull(loadedState);

            var restoredSys = new WildlifeTrappingSystem(new SeededRng(42));
            restoredSys.RestoreState(loadedState!);

            var restoredSite = restoredSys.State.trapSites.Single(s => s.siteId == "site_save");
            Assert.Equal(6, restoredSite.remainingDurability);
            Assert.Equal("trap_box", restoredSite.trapId);
            Assert.Equal(2, restoredSite.checkIntervalDays);
            Assert.Equal(site.checkDay, restoredSite.checkDay);
            Assert.Equal(site.isBroken, restoredSite.isBroken);
        }
    }
}
