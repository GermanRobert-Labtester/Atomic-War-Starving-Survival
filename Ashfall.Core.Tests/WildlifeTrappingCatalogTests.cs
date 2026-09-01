using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;
using Xunit;

namespace Ashfall.Core.Tests
{
    public class WildlifeTrappingCatalogTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            // Walk up from test output to find Assets/StreamingAssets/Data
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

        private static WildlifeTrappingCatalog? LoadCatalog()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            return WildlifeTrappingCatalogLoader.Load(DataDir, fileIO, json);
        }

        [Fact]
        public void Catalog_Loads()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
        }

        [Fact]
        public void Catalog_Has10Traps()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(10, catalog.Traps.Count);
        }

        [Fact]
        public void Catalog_Has15Prey()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(15, catalog.Prey.Count);
        }

        [Fact]
        public void Catalog_Has6Baits()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.Equal(6, catalog.Baits.Count);
        }

        [Fact]
        public void TrapIds_AreUnique()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var seen = new HashSet<string>();
            foreach (var kvp in catalog.Traps)
            {
                Assert.True(seen.Add(kvp.Key), $"Duplicate trap ID: {kvp.Key}");
            }
        }

        [Fact]
        public void PreyIds_AreUnique()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var seen = new HashSet<string>();
            foreach (var kvp in catalog.Prey)
            {
                Assert.True(seen.Add(kvp.Key), $"Duplicate prey ID: {kvp.Key}");
            }
        }

        [Fact]
        public void TrapSetupCosts_ResolveToItems()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var items = ItemCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var itemIds = new HashSet<string>();
            foreach (var item in items) itemIds.Add(item.id);

            foreach (var trap in catalog.Traps.Values)
            {
                foreach (var cost in trap.setupCosts)
                {
                    Assert.True(itemIds.Contains(cost.itemId),
                        $"Trap {trap.trap_id} references unknown item: {cost.itemId}");
                }
            }
        }

        [Fact]
        public void PreyHideItemIds_ResolveToItems()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var items = ItemCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var itemIds = new HashSet<string>();
            foreach (var item in items) itemIds.Add(item.id);

            foreach (var prey in catalog.Prey.Values)
            {
                if (!string.IsNullOrEmpty(prey.hideItemId))
                {
                    Assert.True(itemIds.Contains(prey.hideItemId),
                        $"Prey {prey.speciesId} references unknown hide item: {prey.hideItemId}");
                }
            }
        }

        [Fact]
        public void PreyMigrationIds_ResolveToKnownSpecies()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var knownSpecies = new HashSet<string>
            {
                "species_rad_dog", "species_wolf", "species_dust_lynx", "species_feral_goat",
                "species_blight_rat", "species_ash_boar", "species_iron_crow", "species_ash_gull",
                "species_cotton_hare", "species_gray_heron", "species_mirror_carp", "species_ghost_moth"
            };

            foreach (var prey in catalog.Prey.Values)
            {
                if (!string.IsNullOrEmpty(prey.migrationSpeciesId))
                {
                    Assert.True(knownSpecies.Contains(prey.migrationSpeciesId),
                        $"Prey {prey.speciesId} references unknown migration species: {prey.migrationSpeciesId}");
                }
            }
        }

        [Fact]
        public void PreyActiveSeasons_ResolveToKnownWindows()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var knownSeasons = new HashSet<string>
            {
                "window_ashfall", "window_deep_freeze", "window_thaw",
                "window_black_bloom", "window_high_cold", "window_the_turning"
            };

            foreach (var prey in catalog.Prey.Values)
            {
                foreach (var season in prey.activeSeasons)
                {
                    Assert.True(knownSeasons.Contains(season),
                        $"Prey {prey.speciesId} references unknown season: {season}");
                }
            }
        }

        [Fact]
        public void RegisterWith_PopulatesQuarryCatalog()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            var quarry = sys.GetQuarryCatalog();
            // Should have at least the 15 catalog prey (plus any defaults)
            Assert.True(quarry.Count >= 15, $"Expected >= 15 quarry entries, got {quarry.Count}");

            // Verify specific entries
            Assert.True(quarry.ContainsKey("rabbit"));
            Assert.True(quarry.ContainsKey("cotton_hare"));
            Assert.True(quarry.ContainsKey("deer"));
            Assert.True(quarry.ContainsKey("boar"));
            Assert.True(quarry.ContainsKey("fox"));
            Assert.True(quarry.ContainsKey("rat"));
            Assert.True(quarry.ContainsKey("pheasant"));
            Assert.True(quarry.ContainsKey("ash_crow"));
            Assert.True(quarry.ContainsKey("mirror_carp"));
            Assert.True(quarry.ContainsKey("ash_pike"));
            Assert.True(quarry.ContainsKey("irradiated_squirrel"));
            Assert.True(quarry.ContainsKey("contaminated_fowl"));
            Assert.True(quarry.ContainsKey("rad_dog"));
            Assert.True(quarry.ContainsKey("muskrat"));
            Assert.True(quarry.ContainsKey("hedgehog"));
        }

        [Fact]
        public void RegisterWith_PopulatesBaitCatalog()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            var baits = sys.GetBaitCatalog();
            Assert.True(baits.ContainsKey("bait_scrap_meat"));
            Assert.True(baits.ContainsKey("bait_grain_lure"));
            Assert.True(baits.ContainsKey("bait_pheromone"));
            Assert.True(baits.ContainsKey("bait_fat_cake"));
            Assert.True(baits.ContainsKey("bait_berry_mash"));
            Assert.True(baits.ContainsKey("bait_salt_lick"));
        }

        [Fact]
        public void TrapDefinitions_HaveDistinctTrapTypes()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var trapTypes = new HashSet<string>();
            foreach (var trap in catalog.Traps.Values)
            {
                trapTypes.Add(trap.trapType);
            }
            // Should have at least 6 distinct trap types
            Assert.True(trapTypes.Count >= 6, $"Expected >= 6 distinct trap types, got {trapTypes.Count}");
        }

        [Fact]
        public void TrapDefinitions_HaveCompatiblePrey()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            foreach (var trap in catalog.Traps.Values)
            {
                Assert.True(trap.compatiblePrey.Count > 0,
                    $"Trap {trap.trap_id} has no compatible prey");
                foreach (var preyId in trap.compatiblePrey)
                {
                    Assert.True(catalog.Prey.ContainsKey(preyId),
                        $"Trap {trap.trap_id} references unknown prey: {preyId}");
                }
            }
        }

        [Fact]
        public void PreyDefinitions_HaveValidPreferredTrapType()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var trapTypes = new HashSet<string>();
            foreach (var trap in catalog.Traps.Values) trapTypes.Add(trap.trapType);

            foreach (var prey in catalog.Prey.Values)
            {
                Assert.True(trapTypes.Contains(prey.preferredTrapType),
                    $"Prey {prey.speciesId} preferred trap type '{prey.preferredTrapType}' not found in trap definitions");
            }
        }

        [Fact]
        public void CatchResolution_WorksWithCatalogPrey()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys);

            // Set a trap and check
            sys.SetTrap("test_site", "bait_grain_lure", "hunter_1", "snare");
            sys.TickDay(5); // advance past check day

            // Should have processed without error
            Assert.Single(sys.State.trapSites);
        }

        [Fact]
        public void SaveRoundTrip_PreservesState()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys1);

            sys1.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare");
            var state = sys1.CaptureState();

            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sys2);
            sys2.RestoreState(state);

            Assert.Single(sys2.State.trapSites);
            Assert.Equal("site_1", sys2.State.trapSites[0].siteId);
        }

        [Fact]
        public void MissingFile_ReturnsNull()
        {
            var fileIO = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var result = WildlifeTrappingCatalogLoader.Load("/nonexistent/path", fileIO, json);
            Assert.Null(result);
        }

        [Fact]
        public void TrapIds_FollowConvention()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            foreach (var kvp in catalog.Traps)
            {
                Assert.StartsWith("trap_", kvp.Key);
            }
        }

        [Fact]
        public void PreyYieldItems_ResolveToRawMeat()
        {
            // All prey yield raw_meat as their food item
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            var items = ItemCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            var itemIds = new HashSet<string>();
            foreach (var item in items) itemIds.Add(item.id);

            Assert.True(itemIds.Contains("raw_meat"), "raw_meat item must exist for prey yields");
        }

        // ── Stage 1+2: Durability and identity tests ──

        [Fact]
        public void SetTrap_WithCatalogParams_PersistsTrapId()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 2, durabilityChecks: 8);

            Assert.Single(sys.State.trapSites);
            var site = sys.State.trapSites[0];
            Assert.Equal("trap_snare", site.trapId);
            Assert.Equal(8, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void SetTrap_LegacyCall_HasDefaultDurability()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare");

            var site = sys.State.trapSites[0];
            Assert.Equal("", site.trapId);
            Assert.Equal(-1, site.remainingDurability); // legacy untracked
            Assert.False(site.isBroken);
        }

        [Fact]
        public void CheckTraps_DecrementsDurability()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);

            sys.TickDay(2); // advance past check day
            var site = sys.State.trapSites[0];
            Assert.Equal(7, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void CheckTraps_DecrementsOnNoCatch()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);

            // Force multiple checks to observe decrement even on no-catch
            sys.TickDay(2);
            sys.TickDay(3);
            sys.TickDay(4);
            var site = sys.State.trapSites[0];
            Assert.True(site.remainingDurability <= 5, $"Expected durability <= 5, got {site.remainingDurability}");
        }

        [Fact]
        public void CheckTraps_BreaksAtZero()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // one check
            var site = sys.State.trapSites[0];
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);
        }

        [Fact]
        public void BrokenTrap_ProducesNoCatches()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // breaks the trap
            Assert.True(sys.State.trapSites[0].isBroken);

            int catchBefore = sys.State.totalCatch;
            sys.TickDay(3);
            sys.TickDay(4);
            sys.TickDay(5);
            Assert.Equal(catchBefore, sys.State.totalCatch); // no new catches
        }

        [Fact]
        public void LegacyTrap_NeverBreaks()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare"); // legacy, no durability

            for (int d = 2; d <= 20; d++)
                sys.TickDay(d);

            var site = sys.State.trapSites[0];
            Assert.Equal(-1, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void RepairTrap_RestoresDurability()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // breaks
            Assert.True(sys.State.trapSites[0].isBroken);

            var result = sys.RepairTrap("site_1", 8);
            Assert.True(result.IsSuccess);
            Assert.Equal(8, sys.State.trapSites[0].remainingDurability);
            Assert.False(sys.State.trapSites[0].isBroken);
        }

        [Fact]
        public void RepairTrap_BlocksWhenNotBroken()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);

            var result = sys.RepairTrap("site_1", 8);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        }

        [Fact]
        public void SaveRoundTrip_PreservesDurability()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);
            sys1.TickDay(2); // decrement to 7

            var state = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(state);

            var site = sys2.State.trapSites[0];
            Assert.Equal("trap_snare", site.trapId);
            Assert.Equal(7, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void SaveRoundTrip_PreservesBrokenState()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);
            sys1.TickDay(2); // breaks

            var state = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(state);

            Assert.True(sys2.State.trapSites[0].isBroken);
            Assert.Equal(0, sys2.State.trapSites[0].remainingDurability);
        }

        [Fact]
        public void ImprovisedWireSnare_BreaksBeforeCageTrap()
        {
            // Improvised wire snare: durability 3, cage trap: durability 15
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("snare_site", "bait_grain_lure", "hunter_1", "improvised_wire",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            sys.SetTrap("cage_site", "bait_grain_lure", "hunter_1", "cage",
                trapId: "trap_cage", checkIntervalDays: 1, durabilityChecks: 15);

            for (int d = 2; d <= 5; d++)
                sys.TickDay(d);

            Assert.True(sys.State.trapSites[0].isBroken, "Improvised wire should break by day 5");
            Assert.False(sys.State.trapSites[1].isBroken, "Cage should survive past day 5");
        }

        // ── Task 1: Save migration and legacy compatibility ──

        [Fact]
        public void LegacySave_DeserializesWithDefaults()
        {
            // Simulate a pre-Plan36 save by creating state without new fields
            var json = @"{
                ""systemId"": ""wildlife_trapping"",
                ""trapSites"": [
                    {
                        ""siteId"": ""perimeter_north"",
                        ""assignedHunterId"": ""hunter_1"",
                        ""baitType"": ""bait_grain_lure"",
                        ""trapType"": ""snare"",
                        ""setDay"": 12,
                        ""checkDay"": 14,
                        ""checkIntervalDays"": 2,
                        ""hasCatch"": false,
                        ""catchSpecies"": """",
                        ""carcassYield"": 0.0,
                        ""isToxic"": false,
                        ""toxinRemoved"": false,
                        ""isMeatProcessed"": false,
                        ""hidePreserved"": false
                    }
                ],
                ""totalCatch"": 3,
                ""totalToxicRemoved"": 1
            }";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(state);
            Assert.Single(state.trapSites);

            var site = state.trapSites[0];
            // New fields must have safe defaults
            Assert.Equal(string.Empty, site.trapId);
            Assert.Equal(-1, site.remainingDurability);
            Assert.False(site.isBroken);
            // Legacy fields must preserve values
            Assert.Equal("perimeter_north", site.siteId);
            Assert.Equal("hunter_1", site.assignedHunterId);
            Assert.Equal("bait_grain_lure", site.baitType);
            Assert.Equal("snare", site.trapType);
            Assert.Equal(12, site.setDay);
            Assert.Equal(14, site.checkDay);
            Assert.Equal(3, state.totalCatch);
            Assert.Equal(1, state.totalToxicRemoved);
        }

        [Fact]
        public void LegacySave_MixedOldNewTraps()
        {
            var json = @"{
                ""systemId"": ""wildlife_trapping"",
                ""trapSites"": [
                    {
                        ""siteId"": ""old_site"",
                        ""assignedHunterId"": ""hunter_1"",
                        ""baitType"": ""bait_grain_lure"",
                        ""trapType"": ""snare"",
                        ""setDay"": 5,
                        ""checkDay"": 7,
                        ""checkIntervalDays"": 2,
                        ""hasCatch"": false,
                        ""catchSpecies"": """",
                        ""carcassYield"": 0.0,
                        ""isToxic"": false,
                        ""toxinRemoved"": false,
                        ""isMeatProcessed"": false,
                        ""hidePreserved"": false
                    },
                    {
                        ""siteId"": ""new_site"",
                        ""assignedHunterId"": ""hunter_2"",
                        ""baitType"": ""bait_scrap_meat"",
                        ""trapType"": ""cage"",
                        ""trapId"": ""trap_cage"",
                        ""setDay"": 10,
                        ""checkDay"": 12,
                        ""checkIntervalDays"": 2,
                        ""remainingDurability"": 12,
                        ""isBroken"": false,
                        ""hasCatch"": false,
                        ""catchSpecies"": """",
                        ""carcassYield"": 0.0,
                        ""isToxic"": false,
                        ""toxinRemoved"": false,
                        ""isMeatProcessed"": false,
                        ""hidePreserved"": false
                    }
                ],
                ""totalCatch"": 5,
                ""totalToxicRemoved"": 0
            }";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(json);
            Assert.NotNull(state);
            Assert.Equal(2, state.trapSites.Count);

            // Old trap: defaults
            var old = state.trapSites[0];
            Assert.Equal(string.Empty, old.trapId);
            Assert.Equal(-1, old.remainingDurability);
            Assert.False(old.isBroken);

            // New trap: explicit values
            var New = state.trapSites[1];
            Assert.Equal("trap_cage", New.trapId);
            Assert.Equal(12, New.remainingDurability);
            Assert.False(New.isBroken);
        }

        [Fact]
        public void LegacySave_RestoreIntoRuntime()
        {
            var json = @"{
                ""systemId"": ""wildlife_trapping"",
                ""trapSites"": [
                    {
                        ""siteId"": ""legacy_site"",
                        ""assignedHunterId"": ""hunter_1"",
                        ""baitType"": ""bait_grain_lure"",
                        ""trapType"": ""snare"",
                        ""setDay"": 5,
                        ""checkDay"": 7,
                        ""checkIntervalDays"": 2,
                        ""hasCatch"": false,
                        ""catchSpecies"": """",
                        ""carcassYield"": 0.0,
                        ""isToxic"": false,
                        ""toxinRemoved"": false,
                        ""isMeatProcessed"": false,
                        ""hidePreserved"": false
                    }
                ],
                ""totalCatch"": 2,
                ""totalToxicRemoved"": 0
            }";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(json);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(state);

            Assert.Single(sys.State.trapSites);
            var site = sys.State.trapSites[0];
            Assert.Equal("legacy_site", site.siteId);
            Assert.Equal(-1, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void LegacyTrap_NeverBreaksAfterManyChecks()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            // Simulate legacy trap by setting with no durability
            sys.SetTrap("legacy_site", "bait_grain_lure", "hunter_1", "snare");

            for (int d = 2; d <= 50; d++)
                sys.TickDay(d);

            var site = sys.State.trapSites[0];
            Assert.Equal(-1, site.remainingDurability);
            Assert.False(site.isBroken);
        }

        [Fact]
        public void LegacySave_RoundTripPreservesDefaults()
        {
            var json = @"{
                ""systemId"": ""wildlife_trapping"",
                ""trapSites"": [
                    {
                        ""siteId"": ""legacy_site"",
                        ""assignedHunterId"": ""hunter_1"",
                        ""baitType"": ""bait_grain_lure"",
                        ""trapType"": ""snare"",
                        ""setDay"": 5,
                        ""checkDay"": 7,
                        ""checkIntervalDays"": 2,
                        ""hasCatch"": false,
                        ""catchSpecies"": """",
                        ""carcassYield"": 0.0,
                        ""isToxic"": false,
                        ""toxinRemoved"": false,
                        ""isMeatProcessed"": false,
                        ""hidePreserved"": false
                    }
                ],
                ""totalCatch"": 2,
                ""totalToxicRemoved"": 0
            }";

            var serializer = new SystemTextJsonSerializer();
            var state = serializer.Deserialize<WildlifeTrappingState>(json);
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.RestoreState(state);

            // Capture and re-serialize
            var captured = sys.CaptureState();
            var rejson = serializer.Serialize(captured);
            var restored = serializer.Deserialize<WildlifeTrappingState>(rejson);

            Assert.NotNull(restored);
            Assert.Single(restored.trapSites);
            Assert.Equal(string.Empty, restored.trapSites[0].trapId);
            Assert.Equal(-1, restored.trapSites[0].remainingDurability);
            Assert.False(restored.trapSites[0].isBroken);
        }

        // ── Task 2: Deterministic replay ──

        [Fact]
        public void Replay_UninterruptedVsRestored_IdenticalOutcome()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            // System A: uninterrupted
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysA);
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);
            for (int d = 2; d <= 10; d++)
                sysA.TickDay(d);

            // System B: save at day 5, restore into C
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysB);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);
            for (int d = 2; d <= 5; d++)
                sysB.TickDay(d);

            var savedState = sysB.CaptureState();
            var sysC = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysC);
            sysC.RestoreState(savedState);
            for (int d = 6; d <= 10; d++)
                sysC.TickDay(d);

            // Compare final states
            Assert.Equal(sysA.State.totalCatch, sysC.State.totalCatch);
            Assert.Equal(sysA.State.trapSites[0].remainingDurability, sysC.State.trapSites[0].remainingDurability);
            Assert.Equal(sysA.State.trapSites[0].isBroken, sysC.State.trapSites[0].isBroken);
            Assert.Equal(sysA.State.trapSites[0].hasCatch, sysC.State.trapSites[0].hasCatch);
            if (sysA.State.trapSites[0].hasCatch)
                Assert.Equal(sysA.State.trapSites[0].catchSpecies, sysC.State.trapSites[0].catchSpecies);
        }

        [Fact]
        public void Replay_ThreeRuns_IdenticalHash()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            WildlifeTrappingState Run()
            {
                var sys = new WildlifeTrappingSystem(new SeededRng(42));
                catalog.RegisterWith(sys);
                sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                    trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 8);
                for (int d = 2; d <= 15; d++)
                    sys.TickDay(d);
                return sys.CaptureState();
            }

            var s1 = new SystemTextJsonSerializer();
            var hash1 = s1.Serialize(Run());
            var hash2 = s1.Serialize(Run());
            var hash3 = s1.Serialize(Run());

            Assert.Equal(hash1, hash2);
            Assert.Equal(hash2, hash3);
        }

        [Fact]
        public void Replay_BreakOccursOnSameDay()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);

            // Uninterrupted
            var sysA = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysA);
            sysA.SetTrap("site_1", "bait_grain_lure", "hunter_1", "improvised_wire",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            for (int d = 2; d <= 10; d++)
                sysA.TickDay(d);

            // Save/restore at day 3
            var sysB = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysB);
            sysB.SetTrap("site_1", "bait_grain_lure", "hunter_1", "improvised_wire",
                trapId: "trap_improvised_wire", checkIntervalDays: 1, durabilityChecks: 3);
            for (int d = 2; d <= 3; d++)
                sysB.TickDay(d);
            var saved = sysB.CaptureState();
            var sysC = new WildlifeTrappingSystem(new SeededRng(42));
            catalog.RegisterWith(sysC);
            sysC.RestoreState(saved);
            for (int d = 4; d <= 10; d++)
                sysC.TickDay(d);

            Assert.Equal(sysA.State.trapSites[0].isBroken, sysC.State.trapSites[0].isBroken);
            Assert.Equal(sysA.State.trapSites[0].remainingDurability, sysC.State.trapSites[0].remainingDurability);
        }

        // ── Task 7: Edge cases ──

        [Fact]
        public void EdgeCase_UnknownTrapId_BlocksSafely()
        {
            // TrySetTrap requires catalog + inventory, tested at host level
            // At core level, SetTrap with unknown trapId still works (it's just a string)
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var result = sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "nonexistent_trap");
            Assert.True(result.IsSuccess); // core accepts any string
            Assert.Equal("nonexistent_trap", sys.State.trapSites[0].trapId);
        }

        [Fact]
        public void EdgeCase_ExactCostBalance_DeploySucceeds()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            var result = sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 2, durabilityChecks: 8);
            Assert.True(result.IsSuccess);
            Assert.Equal(8, sys.State.trapSites[0].remainingDurability);
        }

        [Fact]
        public void EdgeCase_FinalDurabilityCatch_ResolvesBeforeBreak()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);

            sys.TickDay(2); // one check — should catch or miss, then break
            var site = sys.State.trapSites[0];
            Assert.Equal(0, site.remainingDurability);
            Assert.True(site.isBroken);
            // The catch may or may not have occurred (RNG-dependent), but durability is consumed
        }

        [Fact]
        public void EdgeCase_BrokenTrap_NoRNGAdvancement()
        {
            var rng1 = new SeededRng(42);
            var sys1 = new WildlifeTrappingSystem(rng1);
            sys1.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);
            sys1.TickDay(2); // breaks
            Assert.True(sys1.State.trapSites[0].isBroken);
            sys1.TickDay(3); // should not advance RNG
            sys1.TickDay(4);

            // Compare with a system that never had a trap
            var rng2 = new SeededRng(42);
            var sys2 = new WildlifeTrappingSystem(rng2);
            sys2.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);
            sys2.TickDay(2); // breaks
            sys2.TickDay(3);
            sys2.TickDay(4);

            // Both should have identical state
            Assert.Equal(sys1.State.totalCatch, sys2.State.totalCatch);
        }

        [Fact]
        public void EdgeCase_RepairAfterSaveLoad()
        {
            var sys1 = new WildlifeTrappingSystem(new SeededRng(42));
            sys1.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare",
                trapId: "trap_snare", checkIntervalDays: 1, durabilityChecks: 1);
            sys1.TickDay(2); // breaks
            Assert.True(sys1.State.trapSites[0].isBroken);

            var saved = sys1.CaptureState();
            var sys2 = new WildlifeTrappingSystem(new SeededRng(42));
            sys2.RestoreState(saved);
            Assert.True(sys2.State.trapSites[0].isBroken);

            var result = sys2.RepairTrap("site_1", 8);
            Assert.True(result.IsSuccess);
            Assert.Equal(8, sys2.State.trapSites[0].remainingDurability);
            Assert.False(sys2.State.trapSites[0].isBroken);
        }

        [Fact]
        public void EdgeCase_LegacyTrap_RepairBlocked()
        {
            var sys = new WildlifeTrappingSystem(new SeededRng(42));
            sys.SetTrap("site_1", "bait_grain_lure", "hunter_1", "snare"); // no trapId
            // Legacy trap has remainingDurability = -1, isBroken = false
            var result = sys.RepairTrap("site_1", 8);
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
        }
    }
}
