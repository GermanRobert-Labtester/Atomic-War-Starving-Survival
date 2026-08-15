using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class VinylRecordCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void VinylRecords_LoadsAll30CanonicalRecordings()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "vinyl_record_archive.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new VinylRecordCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(30, catalog.AllRecords.Count);

            // Test first record (Sibelius Valse Triste)
            var r1 = catalog.GetById("record_01_valse_triste_sibelius_78rpm");
            Assert.NotNull(r1);
            Assert.Equal("MEL-78-4019", r1.catalog_number);
            Assert.Equal(1953, r1.recording_year);
            Assert.Equal(6, r1.daily_morale_modifier);
            Assert.Contains("Warm heavy surface crackle", r1.needle_audio_texture);

            // Test format filter
            var shellac78s = catalog.GetByFormat("78 RPM");
            Assert.True(shellac78s.Count >= 10);

            var vinylLPs = catalog.GetByFormat("33 RPM");
            Assert.True(vinylLPs.Count >= 15);

            // Test final record (Century Seed Spring Waltz)
            var r30 = catalog.GetById("record_30_century_seed_spring_waltz_broadcast");
            Assert.NotNull(r30);
            Assert.Equal(10, r30.daily_morale_modifier);
            Assert.Equal(1992, r30.recording_year);
            Assert.Contains("live bunker acoustics", r30.needle_audio_texture);

            // Test tag search
            var classical = catalog.GetByTag("classical");
            Assert.True(classical.Count >= 8);
        }

        [Fact]
        public void VinylRecords_AllEntriesHaveValidFieldsAndUniqueCatalogNumbers()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "vinyl_record_archive.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new VinylRecordCatalog();
            catalog.Load(json, serializer);

            var seenCatalogNumbers = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var r in catalog.AllRecords)
            {
                Assert.False(string.IsNullOrWhiteSpace(r.record_id), "Missing record_id");
                Assert.False(string.IsNullOrWhiteSpace(r.catalog_number), $"Missing catalog_number on {r.record_id}");
                Assert.True(seenCatalogNumbers.Add(r.catalog_number), $"Duplicate catalog_number: {r.catalog_number}");

                Assert.False(string.IsNullOrWhiteSpace(r.title), $"Missing title on {r.record_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.performer), $"Missing performer on {r.record_id}");
                Assert.True(r.recording_year > 1900 && r.recording_year <= 2000, $"Invalid year on {r.record_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.format_rpm), $"Missing format_rpm on {r.record_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.physical_condition), $"Missing condition on {r.record_id}");
                Assert.True(r.daily_morale_modifier > 0, $"Invalid morale modifier on {r.record_id}");
                Assert.True(r.broadcast_frequency_mhz > 0f, $"Invalid frequency on {r.record_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.needle_audio_texture), $"Missing needle texture on {r.record_id}");
                Assert.True(r.needle_audio_texture.Length > 20, $"Needle texture too brief on {r.record_id}");
                Assert.False(string.IsNullOrWhiteSpace(r.dweller_resonance_notes), $"Missing resonance note on {r.record_id}");
                Assert.NotNull(r.tags);
                Assert.True(r.tags.Length > 0, $"Tags empty on {r.record_id}");
            }
        }
    }
}
