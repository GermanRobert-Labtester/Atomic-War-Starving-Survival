using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;
using Ashfall.Core;
using Ashfall.Core.Memorial;
using Xunit;

namespace Ashfall.Core.Tests.Memorial
{
    public sealed class WastelandGraveEpitaphsCatalogTests
    {
        private readonly string _catalogPath;

        public sealed class EpitaphRecord
        {
            [JsonPropertyName("cause")]
            public string Cause { get; set; } = string.Empty;

            [JsonPropertyName("epitaph")]
            public string Epitaph { get; set; } = string.Empty;
        }

        public sealed class EpitaphCatalog
        {
            [JsonPropertyName("schema_version")]
            public int SchemaVersion { get; set; }

            [JsonPropertyName("epitaphs")]
            public List<EpitaphRecord> Epitaphs { get; set; } = new List<EpitaphRecord>();
        }

        public WastelandGraveEpitaphsCatalogTests()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string candidate = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data", "wasteland_grave_epitaphs.json");
            if (File.Exists(candidate))
            {
                _catalogPath = Path.GetFullPath(candidate);
            }
            else
            {
                _catalogPath = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data", "wasteland_grave_epitaphs.json");
            }
        }

        private EpitaphCatalog LoadCatalog()
        {
            Assert.True(File.Exists(_catalogPath), $"Epitaph catalog not found at: {_catalogPath}");
            string json = File.ReadAllText(_catalogPath);
            var catalog = JsonSerializer.Deserialize<EpitaphCatalog>(json);
            Assert.NotNull(catalog);
            return catalog;
        }

        [Fact]
        public void Catalog_HasValidSchemaVersion()
        {
            var catalog = LoadCatalog();
            Assert.Equal(1, catalog.SchemaVersion);
        }

        [Fact]
        public void Catalog_HasExactly30Entries()
        {
            var catalog = LoadCatalog();
            Assert.Equal(30, catalog.Epitaphs.Count);
        }

        [Fact]
        public void Catalog_PreservesExisting8BaselineEntries()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Epitaphs.Count >= 8);

            Assert.Equal("radiation", catalog.Epitaphs[0].Cause);
            Assert.StartsWith("Lethal cellular degradation.", catalog.Epitaphs[0].Epitaph);

            Assert.Equal("combat", catalog.Epitaphs[1].Cause);
            Assert.StartsWith("Terminated by hostiles.", catalog.Epitaphs[1].Epitaph);

            Assert.Equal("starvation", catalog.Epitaphs[2].Cause);
            Assert.StartsWith("Caloric deficit reached terminal state.", catalog.Epitaphs[2].Epitaph);

            Assert.Equal("exhaustion", catalog.Epitaphs[3].Cause);
            Assert.StartsWith("Cardiovascular collapse due to sustained labor output.", catalog.Epitaphs[3].Epitaph);

            Assert.Equal("disease", catalog.Epitaphs[4].Cause);
            Assert.StartsWith("Pathological contamination event.", catalog.Epitaphs[4].Epitaph);

            Assert.Equal("expedition", catalog.Epitaphs[5].Cause);
            Assert.StartsWith("Asset failed to return from surface operations.", catalog.Epitaphs[5].Epitaph);

            Assert.Equal("trauma", catalog.Epitaphs[6].Cause);
            Assert.StartsWith("Severe structural damage to biological unit.", catalog.Epitaphs[6].Epitaph);

            Assert.Equal("unspecified", catalog.Epitaphs[7].Cause);
            Assert.StartsWith("Termination logged.", catalog.Epitaphs[7].Epitaph);
        }

        [Fact]
        public void Catalog_ContainsNoDuplicateStrings()
        {
            var catalog = LoadCatalog();
            var seen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            foreach (var item in catalog.Epitaphs)
            {
                Assert.False(string.IsNullOrWhiteSpace(item.Epitaph), "Epitaph cannot be null or empty.");
                Assert.True(seen.Add(item.Epitaph), $"Duplicate epitaph found: {item.Epitaph}");
            }
            Assert.Equal(30, seen.Count);
        }

        [Fact]
        public void Catalog_CoversAllRequiredCauses()
        {
            var catalog = LoadCatalog();
            var expectedCauses = new[]
            {
                "radiation", "combat", "starvation", "exhaustion",
                "disease", "expedition", "trauma", "unspecified",
                "exposure", "suicide", "infection", "old_age",
                "drowning", "frostbite", "poisoning", "execution", "unknown"
            };

            var causes = catalog.Epitaphs.Select(e => e.Cause).Distinct().ToHashSet(StringComparer.OrdinalIgnoreCase);
            foreach (var expected in expectedCauses)
            {
                Assert.True(causes.Contains(expected), $"Missing expected cause in catalog: {expected}");
            }
        }

        [Fact]
        public void Catalog_AllNew22Entries_AreOneSentenceWithin5To20Words()
        {
            var catalog = LoadCatalog();
            Assert.Equal(30, catalog.Epitaphs.Count);

            // New entries are indices 8..29 (22 additions)
            for (int i = 8; i < 30; i++)
            {
                var entry = catalog.Epitaphs[i];
                string text = entry.Epitaph.Trim();

                // Check terminal punctuation is single period
                Assert.EndsWith(".", text);
                int periodCount = text.Count(c => c == '.');
                Assert.Equal(1, periodCount);
                Assert.DoesNotContain(";", text);

                // Word count validation (5–20 words)
                var words = text.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                Assert.InRange(words.Length, 5, 20);
            }
        }

        [Fact]
        public void Catalog_DeterministicSeededSelection()
        {
            var catalog = LoadCatalog();
            string SelectForCause(string cause, int seed)
            {
                var candidates = catalog.Epitaphs.Where(e => string.Equals(e.Cause, cause, StringComparison.OrdinalIgnoreCase)).ToList();
                if (candidates.Count == 0)
                {
                    candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList();
                }
                var rng = new SeededRng(seed);
                int index = rng.Next(0, candidates.Count);
                return candidates[index].Epitaph;
            }

            // Deterministic repeated checks across seeds
            for (int seed = 100; seed < 120; seed++)
            {
                string r1 = SelectForCause("radiation", seed);
                string r2 = SelectForCause("radiation", seed);
                Assert.Equal(r1, r2);

                string c1 = SelectForCause("combat", seed);
                string c2 = SelectForCause("combat", seed);
                Assert.Equal(c1, c2);

                string s1 = SelectForCause("starvation", seed);
                string s2 = SelectForCause("starvation", seed);
                Assert.Equal(s1, s2);
            }
        }

        [Fact]
        public void Catalog_DifferentSeedsProduceVariety()
        {
            var catalog = LoadCatalog();
            var radiationCandidates = catalog.Epitaphs.Where(e => e.Cause == "radiation").ToList();
            Assert.Equal(3, radiationCandidates.Count);

            var selected = new HashSet<string>();
            for (int seed = 1; seed < 100; seed++)
            {
                var rng = new SeededRng(seed);
                int index = rng.Next(0, radiationCandidates.Count);
                selected.Add(radiationCandidates[index].Epitaph);
                if (selected.Count == 3) break;
            }

            Assert.Equal(3, selected.Count);
        }

        [Fact]
        public void Catalog_All30EntriesAreReachable()
        {
            var catalog = LoadCatalog();
            var allSelected = new HashSet<string>();

            foreach (var group in catalog.Epitaphs.GroupBy(e => e.Cause))
            {
                var candidates = group.ToList();
                for (ulong step = 0; step < (ulong)candidates.Count * 10; step++)
                {
                    int index = (int)(step % (ulong)candidates.Count);
                    allSelected.Add(candidates[index].Epitaph);
                }
            }

            Assert.Equal(30, allSelected.Count);
        }

        [Fact]
        public void Catalog_UnknownCause_FallsBackSafely()
        {
            var catalog = LoadCatalog();
            string nonExistentCause = "alien_ray";
            var candidates = catalog.Epitaphs.Where(e => string.Equals(e.Cause, nonExistentCause, StringComparison.OrdinalIgnoreCase)).ToList();
            if (candidates.Count == 0)
            {
                candidates = catalog.Epitaphs.Where(e => e.Cause == "unknown" || e.Cause == "unspecified").ToList();
            }

            Assert.NotEmpty(candidates);
            var rng = new SeededRng(42);
            int index = rng.Next(0, candidates.Count);
            string chosen = candidates[index].Epitaph;
            Assert.False(string.IsNullOrWhiteSpace(chosen));
        }

        [Fact]
        public void MemorialSystem_RoundTripsWithSelectedEpitaph()
        {
            var catalog = LoadCatalog();
            var combatEpitaph = catalog.Epitaphs.First(e => e.Cause == "combat").Epitaph;

            var sys = new MemorialSystem(new MemorialState());
            var entry = sys.Memorialize(new MemorialInput
            {
                SurvivorId = "survivor_val",
                Cause = "combat",
                Day = 25,
                BirthDay = 1,
                Epitaph = combatEpitaph,
                MoraleDelta = -5f,
            });

            Assert.Equal(combatEpitaph, entry.Epitaph);

            // Capture and restore
            var state = sys.CaptureState();
            var restoredSys = new MemorialSystem(new MemorialState());
            restoredSys.RestoreState(state);

            Assert.Single(restoredSys.Entries);
            Assert.Equal(combatEpitaph, restoredSys.Entries[0].Epitaph);
            Assert.Equal("combat", restoredSys.Entries[0].Cause);
        }
    }
}
