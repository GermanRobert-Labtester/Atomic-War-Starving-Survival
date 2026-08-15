using System;
using System.IO;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class BunkerCourtCatalogTests
    {
        private static string FindDataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found)) return found;
            if (CatalogLocator.TryFindDataDirectory(AppContext.BaseDirectory, out found)) return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        [Fact]
        public void BunkerCourt_LoadsAll24CanonicalTrials()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            Assert.True(File.Exists(filePath), $"File not found: {filePath}");

            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            Assert.Equal(24, catalog.AllCases.Count);

            // Test first case (The Air Duct Moonshine Still)
            var c1 = catalog.GetById("case_01_the_air_duct_moonshine_still");
            Assert.NotNull(c1);
            Assert.Equal("TRIB-084-MOONSHINE", c1.docket_number);
            Assert.Equal("Ilya Morozov (Instrument Technician)", c1.defendant_name);
            Assert.Contains("solder joints were immaculate", c1.clerk_margin_notes);

            // Test search by defendant
            var ilyaCases = catalog.GetByDefendant("Ilya");
            Assert.True(ilyaCases.Count >= 2); // Moonshine, Intercom Prank, Battery Acid

            // Test final case (Constitution Ratification)
            var c24 = catalog.GetById("case_24_the_ratification_of_the_century_constitution");
            Assert.NotNull(c24);
            Assert.Equal("TRIB-3650-CONSTITUTION", c24.docket_number);
            Assert.Contains("struck the brass anvil three times", c24.clerk_margin_notes);

            // Test tag search
            var humor = catalog.GetByTag("humor");
            Assert.True(humor.Count >= 6);
        }

        [Fact]
        public void BunkerCourt_AllEntriesHaveValidFieldsAndUniqueDockets()
        {
            string dataDir = FindDataDir();
            string filePath = Path.Combine(dataDir, "narrative", "bunker_court_verdicts_codex.json");
            string json = File.ReadAllText(filePath);
            var serializer = new SystemTextJsonSerializer();
            var catalog = new BunkerCourtCatalog();
            catalog.Load(json, serializer);

            var seenDockets = new System.Collections.Generic.HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var c in catalog.AllCases)
            {
                Assert.False(string.IsNullOrWhiteSpace(c.case_id), "Missing case_id");
                Assert.False(string.IsNullOrWhiteSpace(c.docket_number), $"Missing docket_number on {c.case_id}");
                Assert.True(seenDockets.Add(c.docket_number), $"Duplicate docket: {c.docket_number}");

                Assert.False(string.IsNullOrWhiteSpace(c.defendant_name), $"Missing defendant on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.presiding_magistrate), $"Missing magistrate on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.charge_summary), $"Missing charges on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.evidence_presented), $"Missing evidence on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.verdict_outcome), $"Missing verdict on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.disciplinary_penalty), $"Missing penalty on {c.case_id}");
                Assert.False(string.IsNullOrWhiteSpace(c.clerk_margin_notes), $"Missing clerk notes on {c.case_id}");
                Assert.True(c.clerk_margin_notes.Length > 25, $"Clerk notes too brief on {c.case_id}");
                Assert.NotNull(c.tags);
                Assert.True(c.tags.Length > 0, $"Tags empty on {c.case_id}");
            }
        }
    }
}
