using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class RelicProvenanceCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void RelicProvenance_LoadsAll32MasterDossiers()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "relic_provenance_dossiers.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RelicProvenanceCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(32, catalog.AllRelics.Count);

            // Verify balanced tone distribution (8 of each tone)
            var mysterious = catalog.GetByTone("Mysterious");
            var hilarious = catalog.GetByTone("Hilarious");
            var exciting = catalog.GetByTone("Exciting");
            var serious = catalog.GetByTone("Serious");

            Assert.Equal(8, mysterious.Count);
            Assert.Equal(8, hilarious.Count);
            Assert.Equal(8, exciting.Count);
            Assert.Equal(8, serious.Count);

            // Test first and last relics
            var first = catalog.GetById("relic_01_reverse_pocketwatch");
            Assert.NotNull(first);
            Assert.Equal("The Sapper's Backward Watch", first.name);
            Assert.Equal("Mysterious", first.tone);

            var last = catalog.GetById("relic_32_the_valley_constitution_chisel");
            Assert.NotNull(last);
            Assert.Equal("Sonya's Slate Scriber", last.name);
            Assert.Equal("Serious", last.tone);
            Assert.Contains("Century Seed", last.gameplay_effect);
        }

        [Fact]
        public void RelicProvenance_AllEntriesHaveValidCuratorNotesAndEffects()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "relic_provenance_dossiers.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new RelicProvenanceCatalog();
            catalog.Load(json, serializer);

            foreach (var r in catalog.AllRelics)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.relic_id), "Missing relic_id");
                Assert.False(string.IsNullOrWhiteSpace(r.name), $"Missing name on {r.relic_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.material), $"Missing material on {r.relic_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.discovery_location), $"Missing location on {r.relic_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.curator_note), $"Missing note on {r.relic_id}");
                Assert.True(r.curator_note.Length > 80, $"Curator note too brief on {r.relic_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.gameplay_effect), $"Missing effect on {r.relic_id}");
                Assert.NotNull(r.tags);
                Assert.True(r.tags.Length > 0, $"Tags empty on {r.relic_id}");
            }
        }
    }
}
