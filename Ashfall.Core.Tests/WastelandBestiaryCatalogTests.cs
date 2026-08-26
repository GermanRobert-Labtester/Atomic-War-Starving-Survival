using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class WastelandBestiaryCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void WastelandBestiary_LoadsAll24CanonicalCreatures()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_wildlife_bestiary.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandBestiaryCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllCreatures.Count);

            // Test first creature (Two-Headed Wolf)
            var c1 = catalog.GetById("creature_01_two_headed_steppe_wolf");
            Assert.NotNull(c1);
            Assert.Equal("The Bicephalous Stalker", c1.colloquial_name);
            Assert.Equal(3, c1.threat_level);
            Assert.Contains("Fire flares at their feet", c1.harlan_scout_notes);

            // Test apex threats (Threat Level 5: Cave Bear & Iron Elk)
            var apex = catalog.GetByThreatLevel(5);
            Assert.True(apex.Count >= 2);

            // Test big game meat yields (>= 5000 kcal)
            var bigGame = catalog.GetGameYields(5000.0f);
            Assert.True(bigGame.Count >= 4); // Boar, Moose, Cave Bear, Iron Elk

            // Test final mythic creature (The Iron Elk)
            var c24 = catalog.GetById("creature_24_the_iron_elk_of_mount_karkov");
            Assert.NotNull(c24);
            Assert.Equal(5, c24.threat_level);
            Assert.Equal("The Keeper of the Ridge", c24.colloquial_name);
            Assert.Contains("lower your head in reverence", c24.harlan_scout_notes);

            // Test tag search
            var aquatic = catalog.GetByTag("aquatic");
            Assert.True(aquatic.Count >= 3);
        }

        [Fact]
        public void WastelandBestiary_AllEntriesHaveValidFieldsAndHarvestables()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "wasteland_wildlife_bestiary.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new WastelandBestiaryCatalog();
            catalog.Load(json, serializer);

            foreach (var c in catalog.AllCreatures)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.creature_id), "Missing creature_id");
                Assert.False(string.IsNullOrWhiteSpace(c.common_name), $"Missing common_name on {c.creature_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.colloquial_name), $"Missing colloquial_name on {c.creature_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.primary_habitat), $"Missing habitat on {c.creature_id}");
                Assert.InRange(c.threat_level, 1, 5);
                Assert.False(string.IsNullOrWhiteSpace(c.pack_size_range), $"Missing pack_size on {c.creature_id}");
                Assert.True(c.acoustic_lure_frequency_hz >= 0f, $"Invalid lure hz on {c.creature_id}");
                Assert.NotNull(c.harvestable_materials);
                Assert.True(c.harvestable_materials.Length > 0, $"Harvestables empty on {c.creature_id}");
                Assert.True(c.butchered_meat_calories >= 0f, $"Invalid calories on {c.creature_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.harlan_scout_notes), $"Missing scout notes on {c.creature_id}");
                Assert.True(c.harlan_scout_notes.Length > 30, $"Scout notes too brief on {c.creature_id}");
                Assert.NotNull(c.tags);
                Assert.True(c.tags.Length > 0, $"Tags empty on {c.creature_id}");
            }
        }
    }
}
