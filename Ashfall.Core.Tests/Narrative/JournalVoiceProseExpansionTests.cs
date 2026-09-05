using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Tests for Plan 95: Journal Voice Prose Expansion (12 situation keys × 7 personality variants = 84 variants).
    /// </summary>
    public class JournalVoiceProseExpansionTests
    {
        public static readonly string[] Plan95SituationKeys =
        {
            "low_food",
            "low_water",
            "death_of_survivor",
            "successful_expedition",
            "failed_expedition",
            "faction_raid",
            "disease_outbreak",
            "power_failure",
            "new_survivor_arrived",
            "severe_cold",
            "high_radiation_zone",
            "moral_compromise"
        };

        private static readonly RiskBiasTrait[] CoreTraits =
        {
            RiskBiasTrait.Paranoid,
            RiskBiasTrait.Cautious,
            RiskBiasTrait.Realist,
            RiskBiasTrait.Reckless,
            RiskBiasTrait.Denialist,
            RiskBiasTrait.Fatalist
        };

        private static string DataDir()
        {
            string start = Directory.GetCurrentDirectory();
            if (CatalogLocator.TryFindDataDirectory(start, out string found))
                return found;
            if (CatalogLocator.TryFindDataDirectory(System.AppContext.BaseDirectory, out found))
                return found;
            throw new DirectoryNotFoundException("Assets/StreamingAssets/Data not found from " + start);
        }

        private static JournalVoiceProseCatalog LoadCatalog()
        {
            var loader = new JournalVoiceProseCatalogLoader(new FileSystemIO(), new SystemTextJsonSerializer());
            return loader.Load(DataDir());
        }

        [Fact]
        public void All12Plan95SituationKeysExistInCatalog()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Count >= 33, $"Catalog should have at least 33 entries, has {catalog.Count}");

            foreach (string key in Plan95SituationKeys)
            {
                Assert.True(catalog.HasKey(key), $"Catalog must contain expanded situation key '{key}'");
            }
        }

        [Fact]
        public void AllPlan95KeysHaveDefaultAndCoreBiasVariants()
        {
            var catalog = LoadCatalog();

            foreach (string key in Plan95SituationKeys)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);
                Assert.False(string.IsNullOrWhiteSpace(entry.@default), $"Key '{key}' missing non-empty 'default' variant");

                foreach (var trait in CoreTraits)
                {
                    Assert.True(entry.HasVariantForBias(trait), $"Key '{key}' missing variant for bias '{trait}'");
                    string variantText = entry.GetProseForBias(trait);
                    Assert.False(string.IsNullOrWhiteSpace(variantText), $"Key '{key}' variant for '{trait}' must not be whitespace");
                }
            }
        }

        [Fact]
        public void AllPlan95VariantsWithinEachKeyAreDistinct()
        {
            var catalog = LoadCatalog();

            foreach (string key in Plan95SituationKeys)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);

                var seenVariants = new HashSet<string>(StringComparer.Ordinal);

                Assert.True(seenVariants.Add(entry.@default), $"Duplicate default variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.paranoid), $"Duplicate paranoid variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.cautious), $"Duplicate cautious variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.realist), $"Duplicate realist variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.reckless), $"Duplicate reckless variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.denialist), $"Duplicate denialist variant found in '{key}'");
                Assert.True(seenVariants.Add(entry.fatalist), $"Duplicate fatalist variant found in '{key}'");

                Assert.Equal(7, seenVariants.Count);
            }
        }

        [Fact]
        public void AllPlan95SituationKeysAreStrictSnakeCase()
        {
            foreach (string key in Plan95SituationKeys)
            {
                Assert.Equal(key.ToLowerInvariant(), key);
                Assert.DoesNotContain(" ", key);
                Assert.DoesNotContain("-", key);
            }
        }

        [Fact]
        public void JournalVoiceComposeBodyProducesVariantsForPlan95Keys()
        {
            var catalog = LoadCatalog();
            JournalVoice.BindCatalog(catalog);

            foreach (string key in Plan95SituationKeys)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);

                foreach (var trait in CoreTraits)
                {
                    string actual = JournalVoice.ComposeBody(key, trait);
                    string expected = entry.GetProseForBias(trait);
                    Assert.Equal(expected, actual);
                }

                string actualDefault = JournalVoice.ComposeBody(key, (RiskBiasTrait)999);
                Assert.Equal(entry.@default, actualDefault);
            }
        }

        [Fact]
        public void JournalVoiceComposeFullTextFormatsCorrectly()
        {
            var catalog = LoadCatalog();
            JournalVoice.BindCatalog(catalog);

            string full = JournalVoice.ComposeFullText("low_food", RiskBiasTrait.Realist, 42);
            Assert.StartsWith("Day 42. ", full);
            Assert.Contains("There is less food than the shelter needs.", full);
        }

        [Fact]
        public void JournalVoiceFallsBackGracefullyOnUnknownKey()
        {
            var catalog = LoadCatalog();
            JournalVoice.BindCatalog(catalog);

            string fallback = JournalVoice.ComposeBody("nonexistent_unknown_situation_key", RiskBiasTrait.Paranoid);
            Assert.Equal("Something changed. I wrote it down so I would not forget.", fallback);
        }
    }
}
