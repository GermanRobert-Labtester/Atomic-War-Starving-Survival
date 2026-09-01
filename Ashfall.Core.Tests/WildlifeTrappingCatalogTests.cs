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
    }
}
