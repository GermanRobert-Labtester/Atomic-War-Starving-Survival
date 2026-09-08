// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Xunit;

namespace Ashfall.Core.Tests
{
    /// <summary>
    /// Flagship Task 7: Broken Trap RNG Non-Advancement Proof.
    /// Proves that broken traps are deterministic no-ops: after entering broken state,
    /// they consume zero random draws, ensuring future simulation, save/reload,
    /// and replay determinism are never desynchronized by broken gear.
    /// </summary>
    public class WildlifeTrapDeterminismTests
    {
        [Fact]
        public void Task7_01_SameSeedBaseline_ProducesIdenticalOutcomesAcrossFiveDays()
        {
            var rngA = new CountingSeededRng(42);
            var rngB = new CountingSeededRng(42);

            var sysA = new WildlifeTrappingSystem(rngA);
            var sysB = new WildlifeTrappingSystem(rngB);

            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(sysA);
            catalog.RegisterWith(sysB);

            // Deploy identical traps at day 0
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_ann", "snare", "trap_snare", 1, 3);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_ann", "snare", "trap_snare", 1, 3);

            for (int day = 1; day <= 5; day++)
            {
                sysA.TickDay(day);
                sysB.TickDay(day);

                var siteA = sysA.State.trapSites[0];
                var siteB = sysB.State.trapSites[0];

                Assert.Equal(siteA.hasCatch, siteB.hasCatch);
                Assert.Equal(siteA.catchSpecies, siteB.catchSpecies);
                Assert.Equal(siteA.remainingDurability, siteB.remainingDurability);
                Assert.Equal(siteA.isBroken, siteB.isBroken);
                Assert.Equal(rngA.DrawCount, rngB.DrawCount);

                // If catch occurred, butcher on both
                if (siteA.hasCatch)
                {
                    sysA.Butcher("site_1", "hunter_ann");
                    sysB.Butcher("site_1", "hunter_ann");
                }
            }
        }

        [Fact]
        public void Task7_02_BreakTiming_DurabilityOneBreaksOnFirstEvaluation()
        {
            var rng = new CountingSeededRng(100);
            var sys = new WildlifeTrappingSystem(rng);
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(sys);

            // Set trap at day 1 with durabilityChecks = 1 and checkIntervalDays = 1 -> checkDay = 2
            sys.SetTrap("site_fragile", "bait_scrap_meat", "hunter_1", "snare", "trap_snare", 1, 1);
            var site = sys.State.trapSites[0];

            Assert.Equal(1, site.remainingDurability);
            Assert.False(site.isBroken);

            // Day 2: first scheduled check runs, durability decrements 1 -> 0, trap breaks
            sys.TickDay(2);
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken, "Trap with initial durability 1 must transition to isBroken=true on day 2 evaluation");
        }

        [Fact]
        public void Task7_03_PostBreakZeroDrawProof_BrokenTrapConsumesZeroRngDraws()
        {
            var rng = new CountingSeededRng(200);
            var sys = new WildlifeTrappingSystem(rng);
            var catalog = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog.RegisterWith(sys);

            // Deploy trap with durability 1, checkInterval = 1 -> checkDay = 2
            sys.SetTrap("site_broken_test", "bait_scrap_meat", "hunter_1", "snare", "trap_snare", 1, 1);
            sys.TickDay(2); // breaks on day 2

            var site = sys.State.trapSites[0];
            Assert.True(site.isBroken);

            // Clear any catch harvested from day 2 so we can test broken trap yields no new catches
            site.hasCatch = false;
            site.catchSpecies = string.Empty;

            // Reset or record draw count after break
            int drawsAtBreak = rng.DrawCount;

            // Run checks on days 3, 4, 5, 6
            for (int day = 3; day <= 6; day++)
            {
                sys.TickDay(day);
                Assert.Equal(drawsAtBreak, rng.DrawCount); // Zero draws consumed!
                Assert.False(site.hasCatch, "Broken trap must never produce a catch");
            }
        }

        [Fact]
        public void Task7_04_NoTrapControl_ConsumesExactlyZeroRngDraws()
        {
            var rng = new CountingSeededRng(300);
            var sys = new WildlifeTrappingSystem(rng);

            Assert.Empty(sys.State.trapSites);

            for (int day = 1; day <= 5; day++)
            {
                sys.TickDay(day);
                Assert.Equal(0, rng.DrawCount);
            }
        }

        [Fact]
        public void Task7_05_OnlyBrokenTraps_ConsumesZeroDrawsAndYieldsZeroEvents()
        {
            var rng = new CountingSeededRng(400);
            var sys = new WildlifeTrappingSystem(rng);

            // Directly inject two broken traps
            sys.State.trapSites.Add(new TrapSite
            {
                siteId = "broken_1",
                trapId = "trap_snare",
                isBroken = true,
                remainingDurability = 0,
                setDay = 0,
                checkDay = 1
            });
            sys.State.trapSites.Add(new TrapSite
            {
                siteId = "broken_2",
                trapId = "trap_cage",
                isBroken = true,
                remainingDurability = 0,
                setDay = 0,
                checkDay = 1
            });

            rng.ResetCount();

            var res = sys.CheckTraps();
            Assert.True(res.IsSuccess);
            Assert.Equal("trapping.no_catch", res.MessageKey);
            Assert.Equal(0, rng.DrawCount); // Zero draws!
        }

        [Fact]
        public void Task7_06_MixedSetEquivalence_BrokenTrapDoesNotAdvanceRngForHealthyTrap()
        {
            // Compare healthy-only system vs healthy+broken system starting from the same seed
            const int testSeed = 555;

            // System 1: Healthy trap only
            var rng1 = new CountingSeededRng(testSeed);
            var sys1 = new WildlifeTrappingSystem(rng1);
            var catalog1 = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog1.RegisterWith(sys1);

            sys1.SetTrap("site_healthy", "bait_grain_lure", "hunter_1", "snare", "trap_snare", 1, 10);
            sys1.TickDay(1);

            int draws1 = rng1.DrawCount;
            var site1 = sys1.State.trapSites[0];

            // System 2: Healthy trap + Broken trap (Healthy first)
            var rng2 = new CountingSeededRng(testSeed);
            var sys2 = new WildlifeTrappingSystem(rng2);
            var catalog2 = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog2.RegisterWith(sys2);

            sys2.SetTrap("site_healthy", "bait_grain_lure", "hunter_1", "snare", "trap_snare", 1, 10);
            sys2.State.trapSites.Add(new TrapSite
            {
                siteId = "site_broken",
                trapId = "trap_snare",
                isBroken = true,
                remainingDurability = 0,
                setDay = 0,
                checkDay = 1
            });
            sys2.TickDay(1);

            int draws2 = rng2.DrawCount;
            var site2Healthy = sys2.State.trapSites.Find(s => s.siteId == "site_healthy");

            Assert.NotNull(site2Healthy);
            Assert.Equal(draws1, draws2); // Exact same RNG budget!
            Assert.Equal(site1.hasCatch, site2Healthy!.hasCatch);
            Assert.Equal(site1.catchSpecies, site2Healthy.catchSpecies);
            Assert.Equal(site1.carcassYield, site2Healthy.carcassYield);
            Assert.Equal(site1.isToxic, site2Healthy.isToxic);
            Assert.Equal(site1.diseaseId, site2Healthy.diseaseId);
            Assert.Equal(site1.contaminationDose, site2Healthy.contaminationDose);

            // System 3: Broken trap + Healthy trap (Broken first)
            var rng3 = new CountingSeededRng(testSeed);
            var sys3 = new WildlifeTrappingSystem(rng3);
            var catalog3 = WildlifeTrappingCatalogTestFixture.LoadCatalog();
            catalog3.RegisterWith(sys3);

            sys3.State.trapSites.Add(new TrapSite
            {
                siteId = "site_broken",
                trapId = "trap_snare",
                isBroken = true,
                remainingDurability = 0,
                setDay = 0,
                checkDay = 1
            });
            sys3.SetTrap("site_healthy", "bait_grain_lure", "hunter_1", "snare", "trap_snare", 1, 10);
            sys3.TickDay(1);

            int draws3 = rng3.DrawCount;
            var site3Healthy = sys3.State.trapSites.Find(s => s.siteId == "site_healthy");

            Assert.NotNull(site3Healthy);
            Assert.Equal(draws1, draws3); // Exact same RNG budget regardless of order!
            Assert.Equal(site1.hasCatch, site3Healthy!.hasCatch);
            Assert.Equal(site1.catchSpecies, site3Healthy.catchSpecies);
            Assert.Equal(site1.carcassYield, site3Healthy.carcassYield);
            Assert.Equal(site1.isToxic, site3Healthy.isToxic);
        }
    }
}
