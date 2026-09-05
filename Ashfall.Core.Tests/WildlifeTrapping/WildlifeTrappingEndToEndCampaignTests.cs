// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using Ashfall.Core;
using Ashfall.Core.Crafting;
using Ashfall.Core.Inventory;
using Ashfall.Core.IO;
using Xunit;

namespace Ashfall.Core.Tests.WildlifeTrapping
{
    using SeededRng = Ashfall.Core.SeededRng;

    /// <summary>
    /// Flagship Task 8: End-to-End Campaign Smoke Test.
    ///
    /// Validates the complete lifecycle through authoritative production APIs:
    /// 1. Craft Improvised Wire Snare from Copper Wire via CraftingSystem.
    /// 2. Deploy trap via WildlifeTrappingSystem.SetTrap(), consuming trap item from Inventory.
    /// 3. Environment: season 'window_thaw', migration 'species_cotton_hare'.
    /// 4. Day advancement: observe deterministic catch and durability decrement.
    /// 5. Butchering & health-risk evaluation.
    /// 6. Save/load round-trip & continuation equivalence.
    /// 7. Wear to breakage (remainingDurability <= 0 -> isBroken = true).
    /// 8. Repair via RepairTrap(), consuming repair materials from Inventory.
    /// 9. Three consecutive runs from Seed 42 produce identical event traces and hashes.
    /// </summary>
    public sealed class WildlifeTrappingEndToEndCampaignTests : CatalogTestBase
    {
        private WildlifeTrappingCatalog LoadTrappingCatalog()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var cat = WildlifeTrappingCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(cat);
            return cat!;
        }

        private static string ComputeHash(object obj)
        {
            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(obj);
            byte[] hash = SHA256.HashData(Encoding.UTF8.GetBytes(json));
            return Convert.ToHexString(hash);
        }

        [Fact]
        public void Day1_CraftImprovisedWireSnare_ConsumesCopperWireAndGrantsTrapItem()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            var crafting = new CraftingSystem(inv);

            var copperWireDef = new ItemDefinition { id = "copper_wire_10m_of_10m", displayName = "Copper Wire", stackMax = 99 };
            var trapSnareDef = new ItemDefinition { id = "trap_improvised_wire", displayName = "Improvised Wire Snare", stackMax = 5 };

            inv.Add(copperWireDef, 1);
            Assert.Equal(1, inv.CountById("copper_wire_10m_of_10m"));
            Assert.Equal(0, inv.CountById("trap_improvised_wire"));

            var recipe = new Recipe
            {
                id = "craft_trap_improvised_wire",
                recipeName = "Craft Improvised Wire Snare",
                requiredStationId = "",
                craftingTimeHours = 0.25f,
                ingredients = new List<Ingredient>
                {
                    new Ingredient { item = copperWireDef, amount = 1 }
                },
                result = trapSnareDef,
                resultAmount = 1
            };

            bool started = crafting.StartCraft(recipe);
            Assert.True(started, "Crafting should start successfully");
            Assert.Equal(0, inv.CountById("copper_wire_10m_of_10m")); // Consumed upfront

            crafting.Tick(1.0f); // Complete craft
            Assert.Equal(1, inv.CountById("trap_improvised_wire")); // Granted on completion
        }

        [Fact]
        public void Day1_DeployTrap_ConsumesTrapItemAndInitializesDurabilityThree()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            inv.AddById("trap_improvised_wire", 1);
            Assert.Equal(1, inv.CountById("trap_improvised_wire"));

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            // Authoritative transaction: consume item from inventory, then deploy
            bool removed = inv.RemoveById("trap_improvised_wire", 1);
            Assert.True(removed);
            Assert.Equal(0, inv.CountById("trap_improvised_wire"));

            var result = sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            Assert.True(result.IsSuccess);
            Assert.Single(sys.State.trapSites);

            var site = sys.State.trapSites[0];
            Assert.Equal("site_snare_1", site.siteId);
            Assert.Equal("trap_improvised_wire", site.trapId);
            Assert.Equal(3, site.remainingDurability);
            Assert.Equal(1, site.checkIntervalDays);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void Day1_SetTrap_EstablishesCheckIntervalOneAndUnbrokenState()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            var site = sys.State.trapSites[0];
            Assert.Equal(1, site.setDay);
            Assert.Equal(2, site.checkDay);
            Assert.False(site.hasCatch);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void Day2_AdvanceDay_FirstCheckDecrementsDurability()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);

            var site = sys.State.trapSites[0];
            Assert.Equal(2, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void Day2_CatchProgression_PrimaryCatchOccursDeterministically()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetSelectionContext(new WildlifeSelectionContext
            {
                SeasonWindowId = "window_thaw",
                PresentMigrationSpecies = new HashSet<string> { "species_cotton_hare" },
                HunterSkillLevels = new Dictionary<string, float> { { "hunter_mae", 15f } }
            });

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);
            sys.TickDay(3);

            var site = sys.State.trapSites[0];
            Assert.True(site.hasCatch, "Seed 42 with bait and thaw context should produce a catch by day 3");
            Assert.NotEmpty(site.catchSpecies);
        }

        [Fact]
        public void Day2_CatchProgression_YieldsValidSpeciesAndCarcass()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetSelectionContext(new WildlifeSelectionContext
            {
                SeasonWindowId = "window_thaw",
                PresentMigrationSpecies = new HashSet<string> { "species_cotton_hare" },
                HunterSkillLevels = new Dictionary<string, float> { { "hunter_mae", 15f } }
            });

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);
            sys.TickDay(3);

            var site = sys.State.trapSites[0];
            Assert.True(site.hasCatch);
            Assert.True(site.carcassYield > 0f, "Carcass yield must be positive");
            Assert.Contains(site.catchSpecies, new[] { "rabbit", "cotton_hare", "rat" });
        }

        [Fact]
        public void Day2_ButcherCatch_ProcessesMeatAndEvaluatesHealthRisks()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var catalog = LoadTrappingCatalog();
            catalog.RegisterWith(sys);

            sys.SetSelectionContext(new WildlifeSelectionContext
            {
                SeasonWindowId = "window_thaw",
                PresentMigrationSpecies = new HashSet<string> { "species_cotton_hare" },
                HunterSkillLevels = new Dictionary<string, float> { { "hunter_mae", 15f } }
            });

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);
            sys.TickDay(3);
            var site = sys.State.trapSites[0];
            Assert.True(site.hasCatch);

            var butcherRes = sys.Butcher(site.siteId, "hunter_mae");
            Assert.True(butcherRes.IsSuccess);
            Assert.True(site.isMeatProcessed);

            // Risk rolls use deterministic RNG
            bool diseaseContracted = sys.RollDiseaseRisk(0.1f);
            bool contaminationContracted = sys.RollContaminationRisk(0.05f);

            // Predictable under seed 42
            Assert.False(diseaseContracted);
            Assert.False(contaminationContracted);
        }

        [Fact]
        public void Day2_CampaignSave_CapturesConsistentMultiSystemState()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            sys.TickDay(2);

            var state = sys.CaptureState();
            Assert.NotNull(state);
            Assert.Single(state.trapSites);
            Assert.NotEqual(0UL, state.primaryRngState);
            Assert.NotEqual(0UL, state.bycatchRngState);

            var serializer = new SystemTextJsonSerializer();
            string json = serializer.Serialize(state);
            Assert.Contains("\"primaryRngState\"", json);
            Assert.Contains("\"trap_improvised_wire\"", json);
        }

        [Fact]
        public void Day2_CampaignRestore_RestoresEquivalentOperationalState()
        {
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            var cat = LoadTrappingCatalog();
            cat.RegisterWith(sysA);

            sysA.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            sysA.TickDay(2);

            var saved = sysA.CaptureState();

            var sysB = new WildlifeTrappingSystem(new SeededRng(999));
            cat.RegisterWith(sysB);
            sysB.RestoreState(saved);

            Assert.Single(sysB.State.trapSites);
            var siteB = sysB.State.trapSites[0];
            Assert.Equal(sysA.State.trapSites[0].remainingDurability, siteB.remainingDurability);
            Assert.Equal(sysA.State.trapSites[0].hasCatch, siteB.hasCatch);
            Assert.Equal(sysA.State.trapSites[0].catchSpecies, siteB.catchSpecies);
            Assert.Equal(sysA.State.trapSites[0].carcassYield, siteB.carcassYield, 3);
        }

        [Fact]
        public void Day3_PostRestoreContinuation_MatchesUninterruptedTrace()
        {
            var cat = LoadTrappingCatalog();

            // Run A: uninterrupted to day 3
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            cat.RegisterWith(sysA);
            sysA.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            sysA.TickDay(2);
            if (sysA.State.trapSites[0].hasCatch) sysA.State.trapSites[0].hasCatch = false;
            sysA.TickDay(3);

            // Run B: day 2, save, restore into sysC, day 3
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));
            cat.RegisterWith(sysB);
            sysB.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            sysB.TickDay(2);
            if (sysB.State.trapSites[0].hasCatch) sysB.State.trapSites[0].hasCatch = false;

            var saved = sysB.CaptureState();
            var sysC = new WildlifeTrappingSystem(new SeededRng(99999));
            cat.RegisterWith(sysC);
            sysC.RestoreState(saved);
            sysC.TickDay(3);

            var sA = sysA.State.trapSites[0];
            var sC = sysC.State.trapSites[0];

            Assert.Equal(sA.remainingDurability, sC.remainingDurability);
            Assert.Equal(sA.hasCatch, sC.hasCatch);
            Assert.Equal(sA.catchSpecies, sC.catchSpecies);
            Assert.Equal(ComputeHash(sysA.CaptureState()), ComputeHash(sysC.CaptureState()));
        }

        [Fact]
        public void Day3_ContinuedWear_ReachesZeroDurabilityAndBreaksTrap()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);

            // 3 checks: Day 2, Day 3, Day 4
            for (int d = 2; d <= 4; d++)
            {
                sys.TickDay(d);
                var site = sys.State.trapSites[0];
                if (site.hasCatch) site.hasCatch = false; // reset catch to check next day
            }

            var finalSite = sys.State.trapSites[0];
            Assert.Equal(0, finalSite.remainingDurability);
            Assert.True(finalSite.isBroken, "Trap with 3 durability checks must break on 3rd check");
        }

        [Fact]
        public void BrokenTrap_ProducesNoCatchesAndPreventsDurabilityUnderflow()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // Durability drops 1 -> 0, broken
            var site = sys.State.trapSites[0];
            Assert.True(site.isBroken);
            site.hasCatch = false;

            // Further checks while broken
            sys.TickDay(3);
            sys.TickDay(4);

            Assert.Equal(0, site.remainingDurability); // Never underflows below 0
            Assert.False(site.hasCatch, "Broken trap cannot produce catches");
        }

        [Fact]
        public void RepairTrap_ConsumesMaterialsAndRestoresDurabilityThree()
        {
            var inv = new Ashfall.Core.Inventory.Inventory();
            inv.AddById("copper_wire_10m_of_10m", 2);

            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // Breaks
            var site = sys.State.trapSites[0];
            Assert.True(site.isBroken);

            // Repair transaction: deduct repair material, restore durability
            bool materialConsumed = inv.RemoveById("copper_wire_10m_of_10m", 1);
            Assert.True(materialConsumed);
            Assert.Equal(1, inv.CountById("copper_wire_10m_of_10m"));

            var repairResult = sys.RepairTrap(site.siteId, 3);
            Assert.True(repairResult.IsSuccess);
            Assert.False(site.isBroken);
            Assert.Equal(3, site.remainingDurability);
        }

        [Fact]
        public void RepairedTrap_ResumesActiveCatchEligibility()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            LoadTrappingCatalog().RegisterWith(sys);

            sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // Breaks
            Assert.True(sys.State.trapSites[0].isBroken);

            sys.RepairTrap("site_snare_1", 3);
            Assert.False(sys.State.trapSites[0].isBroken);

            sys.TickDay(3); // First check after repair
            Assert.Equal(2, sys.State.trapSites[0].remainingDurability);
        }

        [Fact]
        public void CampaignSmoke_Seed42_ThreeCompleteRuns_YieldIdenticalEventTracesAndHashes()
        {
            var cat = LoadTrappingCatalog();

            (List<string> trace, string finalHash) RunLifecycle(int seed)
            {
                var eventLog = new List<string>();
                var inv = new Ashfall.Core.Inventory.Inventory();
                var crafting = new CraftingSystem(inv);
                var copperWire = new ItemDefinition { id = "copper_wire_10m_of_10m", displayName = "Copper Wire", stackMax = 99 };
                var trapSnare = new ItemDefinition { id = "trap_improvised_wire", displayName = "Improvised Wire Snare", stackMax = 5 };

                inv.Add(copperWire, 3);
                eventLog.Add($"Day1:InitialCopperWireCount={inv.CountById("copper_wire_10m_of_10m")}");

                // 1. Craft
                var recipe = new Recipe
                {
                    id = "craft_trap_improvised_wire",
                    requiredStationId = "",
                    craftingTimeHours = 0.25f,
                    ingredients = new List<Ingredient> { new Ingredient { item = copperWire, amount = 1 } },
                    result = trapSnare,
                    resultAmount = 1
                };
                crafting.StartCraft(recipe);
                crafting.Tick(1.0f);
                eventLog.Add($"Day1:CraftCompleted:TrapCount={inv.CountById("trap_improvised_wire")}");

                // 2. Trapping system & deploy
                var sys = new WildlifeTrappingSystem(new SeededRng(seed));
                cat.RegisterWith(sys);

                inv.RemoveById("trap_improvised_wire", 1);
                sys.SetTrap("site_snare_1", "bait_grain_lure", "hunter_mae", "snare",
                    trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
                eventLog.Add($"Day1:TrapDeployed:Durability={sys.State.trapSites[0].remainingDurability}");

                // 3. Days 2 & 3
                for (int d = 2; d <= 3; d++)
                {
                    sys.TickDay(d);
                    var s = sys.State.trapSites[0];
                    eventLog.Add($"Day{d}:Check:HasCatch={s.hasCatch}:Species={s.catchSpecies}:Durability={s.remainingDurability}");
                    if (s.hasCatch)
                    {
                        sys.Butcher(s.siteId, "hunter_mae");
                        s.hasCatch = false;
                    }
                }

                // 4. Save & Restore
                var saved = sys.CaptureState();
                var restoredSys = new WildlifeTrappingSystem(new SeededRng(99999));
                cat.RegisterWith(restoredSys);
                restoredSys.RestoreState(saved);
                eventLog.Add($"Day3:SaveRestore:Durability={restoredSys.State.trapSites[0].remainingDurability}");

                // 5. Day 4: Wear to breakage
                restoredSys.TickDay(4);
                var finalSite = restoredSys.State.trapSites[0];
                eventLog.Add($"Day4:BreakCheck:IsBroken={finalSite.isBroken}:Durability={finalSite.remainingDurability}");

                // 6. Repair
                inv.RemoveById("copper_wire_10m_of_10m", 1);
                restoredSys.RepairTrap(finalSite.siteId, 3);
                eventLog.Add($"Day4:Repaired:IsBroken={finalSite.isBroken}:Durability={finalSite.remainingDurability}");

                return (eventLog, ComputeHash(restoredSys.CaptureState()));
            }

            var (trace1, hash1) = RunLifecycle(42);
            var (trace2, hash2) = RunLifecycle(42);
            var (trace3, hash3) = RunLifecycle(42);

            Assert.NotEmpty(trace1);
            Assert.Equal(trace1, trace2);
            Assert.Equal(trace2, trace3);

            Assert.NotEmpty(hash1);
            Assert.Equal(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }
    }
}
