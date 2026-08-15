using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class LostTechManualCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void LostTechManuals_LoadsAll24CanonicalManuals()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "lost_tech_manuals.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new LostTechManualCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllManuals.Count);

            // Test first manual (Turbine Governor)
            var m1 = catalog.GetById("manual_01_steam_turbine_centrifugal_governor");
            Assert.NotNull(m1);
            Assert.Equal("GOST-TURB-1961-A", m1.manual_code);
            Assert.Equal(3, m1.technical_complexity_tier);
            Assert.Contains("Flyball spring balance", m1.schematic_summary);
            Assert.Contains("panicked horse", m1.dmitri_engineering_notes);

            // Test complexity tier 5 manuals (Cryocooler & Orbital Ephemeris)
            var tier5 = catalog.GetByComplexityTier(5);
            Assert.True(tier5.Count >= 2);

            // Test discipline search
            var power = catalog.GetByDiscipline("Power");
            Assert.True(power.Count >= 2);

            // Test final manual (Orbital Ephemeris)
            var m24 = catalog.GetById("manual_24_orbital_perigee_mechanics_and_ephemeris");
            Assert.NotNull(m24);
            Assert.Equal(5, m24.technical_complexity_tier);
            Assert.Equal("ASTRO-ORB-1981-TOPSECRET", m24.manual_code);
            Assert.Contains("90-second warning", m24.dmitri_engineering_notes);

            // Test tag search
            var dmitri = catalog.GetByTag("dmitri");
            Assert.True(dmitri.Count >= 1);
        }

        [Fact]
        public void LostTechManuals_AllEntriesHaveValidFieldsAndUniqueCodes()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "lost_tech_manuals.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new LostTechManualCatalog();
            catalog.Load(json, serializer);

            var seenCodes = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var m in catalog.AllManuals)
            {
                Assert.False(string.IsNullOrWhiteSpace(m.manual_id), "Missing manual_id");
                Assert.False(string.IsNullOrWhiteSpace(m.manual_code), $"Missing manual_code on {m.manual_id}");
                Assert.True(seenCodes.Add(m.manual_code), $"Duplicate manual_code: {m.manual_code}");

                Assert.False(string.IsNullOrWhiteSpace(m.title), $"Missing title on {m.manual_id}");
                Assert.False(string.IsNullOrWhiteSpace(m.engineering_discipline), $"Missing discipline on {m.manual_id}");
                Assert.InRange(m.technical_complexity_tier, 1, 5);
                Assert.InRange(m.repair_failure_risk_pct, 5.0f, 100.0f);
                Assert.False(string.IsNullOrWhiteSpace(m.schematic_summary), $"Missing schematic on {m.manual_id}");
                Assert.False(string.IsNullOrWhiteSpace(m.dmitri_engineering_notes), $"Missing Dmitri notes on {m.manual_id}");
                Assert.True(m.dmitri_engineering_notes.Length > 25, $"Dmitri notes too brief on {m.manual_id}");
                Assert.NotNull(m.tags);
                Assert.True(m.tags.Length > 0, $"Tags empty on {m.manual_id}");
            }
        }
    }
}
