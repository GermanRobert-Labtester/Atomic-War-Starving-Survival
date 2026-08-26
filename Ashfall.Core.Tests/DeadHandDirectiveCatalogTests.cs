using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class DeadHandDirectiveCatalogTests
    : CatalogTestBase{
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void DeadHandDirectives_LoadsAll20CanonicalDirectives()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dead_hand_directives.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DeadHandDirectiveCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(20, catalog.AllDirectives.Count);

            // Test first directive (Perimeter sensor Day 0)
            var first = catalog.GetById("directive_01_perimeter_sensor_trigger");
            Assert.NotNull(first);
            Assert.Contains("LEVEL 5", first.clearance_level);
            Assert.Equal("SHA-0-DEAD-01-A9B4", first.crypto_checksum);
            Assert.Contains("14 high-amplitude seismic shockwaves", first.transcript);
            Assert.Contains("override key was found", first.archaeological_notes);

            // Test orbital Harrow directive
            var harrow = catalog.GetById("directive_02_orbital_kinetic_harrow_arming");
            Assert.NotNull(harrow);
            Assert.Contains("tungsten penetrator darts", harrow.transcript);

            // Test final human entry (Marshal Rostok)
            var final = catalog.GetById("directive_20_the_final_log_entry_of_human_command");
            Assert.NotNull(final);
            Assert.Equal("SHA-0-DEAD-20-OMEGA", final.crypto_checksum);
            Assert.Contains("Do not rebuild the rockets. Plant wheat. Forgive us.", final.transcript);

            // Test clearance search
            var level5 = catalog.GetByClearance("LEVEL 5");
            Assert.True(level5.Count >= 5);

            // Test tag search
            var deadHand = catalog.GetByTag("dead_hand");
            Assert.True(deadHand.Count >= 4);
        }

        [Fact]
        public void DeadHandDirectives_AllEntriesHaveValidFieldsAndChecksums()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "dead_hand_directives.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new DeadHandDirectiveCatalog();
            catalog.Load(json, serializer);

            foreach (var d in catalog.AllDirectives)
            {
                Assert.False(string.IsNullOrWhiteSpace(d.directive_id), "Missing directive_id");
                Assert.False(string.IsNullOrWhiteSpace(d.timestamp_utc), $"Missing timestamp on {d.directive_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.clearance_level), $"Missing clearance on {d.directive_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.issuing_authority), $"Missing authority on {d.directive_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.crypto_checksum), $"Missing checksum on {d.directive_id}");
                Assert.StartsWith("SHA-0-DEAD-", d.crypto_checksum);
                Assert.False(string.IsNullOrWhiteSpace(d.directive_title), $"Missing title on {d.directive_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.transcript), $"Missing transcript on {d.directive_id}");
                Assert.True(d.transcript.Length > 80, $"Transcript too brief on {d.directive_id}");
                Assert.False(string.IsNullOrWhiteSpace(d.archaeological_notes), $"Missing notes on {d.directive_id}");
                Assert.True(d.archaeological_notes.Length > 40, $"Notes too brief on {d.directive_id}");
                Assert.NotNull(d.tags);
                Assert.True(d.tags.Length > 0, $"Tags empty on {d.directive_id}");
            }
        }
    }
}
