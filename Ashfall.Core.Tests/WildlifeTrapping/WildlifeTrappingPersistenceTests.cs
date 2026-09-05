using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    public class WildlifeTrappingPersistenceTests
    {
        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found))
                return found;
            return string.Empty;
        }

        private static WildlifeTrappingState CreateHeterogeneousState()
        {
            var state = new WildlifeTrappingState
            {
                totalCatch = 12,
                totalToxicRemoved = 3,
                trapSites = new List<TrapSite>
                {
                    // 1. Trap A: Legacy / untracked durability
                    new TrapSite
                    {
                        siteId = "site_legacy",
                        trapId = "",
                        trapType = "snare",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "Hunter1",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 2,
                        remainingDurability = -1,
                        isBroken = false,
                        hasCatch = false
                    },
                    // 2. Trap B: Catalog-linked Healthy
                    new TrapSite
                    {
                        siteId = "site_healthy",
                        trapId = "trap_snare",
                        trapType = "snare",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "Hunter1",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 2,
                        remainingDurability = 8,
                        isBroken = false,
                        hasCatch = false
                    },
                    // 3. Trap C: Catalog-linked Damaged
                    new TrapSite
                    {
                        siteId = "site_damaged",
                        trapId = "trap_snare",
                        trapType = "snare",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "Hunter1",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 2,
                        remainingDurability = 3,
                        isBroken = false,
                        hasCatch = false
                    },
                    // 4. Trap D: Catalog-linked Broken
                    new TrapSite
                    {
                        siteId = "site_broken",
                        trapId = "trap_snare",
                        trapType = "snare",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "Hunter1",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 2,
                        remainingDurability = 0,
                        isBroken = true,
                        hasCatch = false
                    },
                    // 5. Trap E: Pending Catch with Bycatch
                    new TrapSite
                    {
                        siteId = "site_pending_catch",
                        trapId = "trap_net",
                        trapType = "net",
                        baitType = "bait_grain_lure",
                        assignedHunterId = "Hunter2",
                        setDay = 1,
                        checkDay = 2,
                        checkIntervalDays = 2,
                        remainingDurability = 4,
                        isBroken = false,
                        hasCatch = true,
                        catchSpecies = "pheasant",
                        bycatchSpecies = "rat",
                        carcassYield = 1.6f,
                        isToxic = true,
                        toxinRemoved = false,
                        isMeatProcessed = false,
                        hidePreserved = false
                    }
                }
            };
            return state;
        }

        [Fact]
        public void D1_RawSerializerRoundTrip_PreservesAllHeterogeneousTrapFields()
        {
            var serializer = new SystemTextJsonSerializer();
            var original = CreateHeterogeneousState();

            string json = serializer.Serialize(original);
            Assert.False(string.IsNullOrEmpty(json));

            var deserialized = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(deserialized);
            Assert.Equal(5, deserialized!.trapSites.Count);

            // Verify Trap A (Legacy)
            var legacy = deserialized.trapSites[0];
            Assert.Equal("site_legacy", legacy.siteId);
            Assert.Equal("", legacy.trapId);
            Assert.Equal(-1, legacy.remainingDurability);
            Assert.False(legacy.isBroken);

            // Verify Trap B (Healthy)
            var healthy = deserialized.trapSites[1];
            Assert.Equal("site_healthy", healthy.siteId);
            Assert.Equal("trap_snare", healthy.trapId);
            Assert.Equal(8, healthy.remainingDurability);
            Assert.False(healthy.isBroken);

            // Verify Trap C (Damaged)
            var damaged = deserialized.trapSites[2];
            Assert.Equal("site_damaged", damaged.siteId);
            Assert.Equal("trap_snare", damaged.trapId);
            Assert.Equal(3, damaged.remainingDurability);
            Assert.False(damaged.isBroken);

            // Verify Trap D (Broken)
            var broken = deserialized.trapSites[3];
            Assert.Equal("site_broken", broken.siteId);
            Assert.Equal("trap_snare", broken.trapId);
            Assert.Equal(0, broken.remainingDurability);
            Assert.True(broken.isBroken);

            // Verify Trap E (Pending Catch with Bycatch)
            var pending = deserialized.trapSites[4];
            Assert.Equal("site_pending_catch", pending.siteId);
            Assert.Equal("trap_net", pending.trapId);
            Assert.True(pending.hasCatch);
            Assert.Equal("pheasant", pending.catchSpecies);
            Assert.Equal("rat", pending.bycatchSpecies);
            Assert.Equal(1.6f, pending.carcassYield, precision: 3);
            Assert.True(pending.isToxic);
            Assert.False(pending.isMeatProcessed);
        }

        [Fact]
        public void D2_SystemRestoreState_RestoresWithoutCorruptionOrDataLoss()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42), new NullLog());
            var original = CreateHeterogeneousState();

            sys.RestoreState(original);

            Assert.Equal(12, sys.State.totalCatch);
            Assert.Equal(3, sys.State.totalToxicRemoved);
            Assert.Equal(5, sys.State.trapSites.Count);
            Assert.Equal(-1, sys.State.trapSites[0].remainingDurability);
            Assert.True(sys.State.trapSites[3].isBroken);
            Assert.Equal("rat", sys.State.trapSites[4].bycatchSpecies);
        }

        [Fact]
        public void D3_PostRestoreSimulation_BehavesCorrectlyAcrossAllTrapStates()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42), new NullLog());
            string dataDir = DataDir();
            if (!string.IsNullOrEmpty(dataDir))
            {
                var cat = WildlifeTrappingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new NullLog());
                cat?.RegisterWith(sys);
            }

            var original = CreateHeterogeneousState();
            sys.RestoreState(original);

            // Advance day to 2 so all traps are eligible for check
            sys.TickDay(2);
            sys.CheckTraps(densityMultiplier: 1.0f);

            // Trap A (Legacy): remains operational, not broken
            var legacy = sys.State.trapSites.Find(s => s.siteId == "site_legacy")!;
            Assert.False(legacy.isBroken, "Legacy trap should remain operational without catalog durability");

            // Trap D (Broken): produces no catches, remains broken
            var broken = sys.State.trapSites.Find(s => s.siteId == "site_broken")!;
            Assert.True(broken.isBroken, "Broken trap must remain broken");
            Assert.False(broken.hasCatch, "Broken trap must produce zero catches");

            // Trap E (Pending Catch): retains catch, can be butchered and retains bycatch
            var pending = sys.State.trapSites.Find(s => s.siteId == "site_pending_catch")!;
            Assert.True(pending.hasCatch);
            Assert.Equal("pheasant", pending.catchSpecies);
            Assert.Equal("rat", pending.bycatchSpecies);

            var butcherResult = sys.Butcher("site_pending_catch");
            Assert.True(butcherResult.IsSuccess);
            Assert.True(pending.isMeatProcessed);
            Assert.Equal("rat", pending.bycatchSpecies);
        }

        [Fact]
        public void D4_BrokenTrap_ConsumesZeroRngDraws_PairedSimulationProof()
        {
            const int seed = 12345;
            string dataDir = DataDir();

            // System A: Has active trap AND broken trap
            var sysA = new WildlifeTrappingSystem(new SeededRng(seed), new NullLog());
            // System B: Has active trap ONLY
            var sysB = new WildlifeTrappingSystem(new SeededRng(seed), new NullLog());

            if (!string.IsNullOrEmpty(dataDir))
            {
                var catA = WildlifeTrappingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new NullLog());
                catA?.RegisterWith(sysA);
                var catB = WildlifeTrappingCatalogLoader.Load(dataDir, new FileSystemIO(), new SystemTextJsonSerializer(), new NullLog());
                catB?.RegisterWith(sysB);
            }

            // Setup System A
            sysA.State.trapSites.Add(new TrapSite
            {
                siteId = "active_site",
                trapId = "trap_snare",
                trapType = "snare",
                baitType = "bait_grain_lure",
                setDay = 0,
                checkDay = 0,
                checkIntervalDays = 1,
                remainingDurability = 500,
                isBroken = false
            });
            sysA.State.trapSites.Add(new TrapSite
            {
                siteId = "broken_site",
                trapId = "trap_snare",
                trapType = "snare",
                baitType = "bait_grain_lure",
                setDay = 0,
                checkDay = 0,
                checkIntervalDays = 1,
                remainingDurability = 0,
                isBroken = true
            });

            // Setup System B
            sysB.State.trapSites.Add(new TrapSite
            {
                siteId = "active_site",
                trapId = "trap_snare",
                trapType = "snare",
                baitType = "bait_grain_lure",
                setDay = 0,
                checkDay = 0,
                checkIntervalDays = 1,
                remainingDurability = 500,
                isBroken = false
            });

            // Run 50 checks on both systems
            for (int day = 0; day < 50; day++)
            {
                sysA.TickDay(day);
                sysB.TickDay(day);

                sysA.CheckTraps(densityMultiplier: 1.0f);
                sysB.CheckTraps(densityMultiplier: 1.0f);

                var siteA = sysA.State.trapSites[0];
                var siteB = sysB.State.trapSites[0];
                var brokenA = sysA.State.trapSites[1];

                // Broken trap must never catch anything and remain broken
                Assert.True(brokenA.isBroken);
                Assert.False(brokenA.hasCatch);

                // Active trap in System A and System B must match identically on every single roll!
                Assert.Equal(siteB.hasCatch, siteA.hasCatch);
                Assert.Equal(siteB.catchSpecies, siteA.catchSpecies);
                Assert.Equal(siteB.bycatchSpecies, siteA.bycatchSpecies);
                Assert.Equal(siteB.carcassYield, siteA.carcassYield);
                Assert.Equal(siteB.isToxic, siteA.isToxic);
                Assert.Equal(siteB.remainingDurability, siteA.remainingDurability);

                // Reset catch for next iteration
                siteA.hasCatch = false;
                siteB.hasCatch = false;
                siteA.catchSpecies = string.Empty;
                siteB.catchSpecies = string.Empty;
                siteA.baitStolen = false;
                siteB.baitStolen = false;
                siteA.baitType = "bait_grain_lure";
                siteB.baitType = "bait_grain_lure";
            }
        }

        [Fact]
        public void D5_ReserializationNormalizationStability_IsIdempotent()
        {
            var serializer = new SystemTextJsonSerializer();
            var originalState = CreateHeterogeneousState();

            var sys1 = new WildlifeTrappingSystem(new SeededRng(42), new NullLog());
            sys1.RestoreState(originalState);
            var captured1 = sys1.CaptureState();
            string json1 = serializer.Serialize(captured1);

            var deserialized = serializer.Deserialize<WildlifeTrappingState>(json1);
            Assert.NotNull(deserialized);

            var sys2 = new WildlifeTrappingSystem(new SeededRng(42), new NullLog());
            sys2.RestoreState(deserialized!);
            var captured2 = sys2.CaptureState();
            string json2 = serializer.Serialize(captured2);

            Assert.Equal(json1, json2);
        }
    }
}
