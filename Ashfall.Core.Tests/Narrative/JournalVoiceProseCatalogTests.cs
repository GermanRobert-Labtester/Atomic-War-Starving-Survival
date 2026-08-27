using System;
using System.Collections.Generic;
using System.IO;
using Xunit;
using Ashfall.Core;
using Ashfall.Core.Journal;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Canonical-ID, reachability, and load-equivalence tests for journal_voice_prose.json.
    /// Validates that all prose definitions meet schema requirements and can be loaded correctly.
    /// </summary>
    public class JournalVoiceProseCatalogTests
    {
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

        private static void EnsureCatalogBound()
        {
            var catalog = LoadCatalog();
            JournalVoice.BindCatalog(catalog);
        }

        [Fact]
        public void JournalVoiceProseJsonHasSchemaVersion()
        {
            string jsonPath = Path.Combine(DataDir(), JournalVoiceProseCatalogLoader.ProseFile);
            Assert.True(File.Exists(jsonPath), "journal_voice_prose.json must exist");

            string json = File.ReadAllText(jsonPath);
            Assert.Contains("\"schema_version\"", json);
        }

        [Fact]
        public void JournalVoiceProseJsonLoadsWithoutErrors()
        {
            var catalog = LoadCatalog();
            Assert.NotNull(catalog);
            Assert.True(catalog.Count > 0, "Catalog should contain prose entries");
        }

        [Fact]
        public void AllKnowledgeKeysHaveProseEntries()
        {
            var catalog = LoadCatalog();

            foreach (string key in KnowledgeKeys.All)
            {
                Assert.True(catalog.HasKey(key),
                    $"Knowledge key '{key}' must have prose entry in catalog");
            }
        }

        [Fact]
        public void AllKnowledgeKeysHaveDefaultVariant()
        {
            var catalog = LoadCatalog();

            foreach (string key in KnowledgeKeys.All)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);
                Assert.NotNull(entry.@default);
                Assert.NotEmpty(entry.@default);
            }
        }

        [Fact]
        public void AllCoreBiasTraitsHaveVariants()
        {
            var catalog = LoadCatalog();
            var coreTraits = new[] {
                RiskBiasTrait.Paranoid, RiskBiasTrait.Cautious, RiskBiasTrait.Realist,
                RiskBiasTrait.Reckless, RiskBiasTrait.Denialist, RiskBiasTrait.Fatalist
            };

            foreach (string key in KnowledgeKeys.All)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);

                foreach (var trait in coreTraits)
                {
                    Assert.True(entry.HasVariantForBias(trait),
                        $"Key '{key}' missing variant for bias '{trait}'");
                }
            }
        }

        [Fact]
        public void AllExpansionBiasTraitsHaveVariantsWhereApplicable()
        {
            var catalog = LoadCatalog();
            var expansionTraits = new[] { RiskBiasTrait.Empath, RiskBiasTrait.Sociopath };

            // Expansion 06 knowledge keys should have empath/sociopath variants
            var expansionKeys = new[] {
                KnowledgeKeys.ContinuityReclamationDecree,
                KnowledgeKeys.HydroBaronRateCardOrigin,
                KnowledgeKeys.DeserterCoalitionFounding,
                KnowledgeKeys.ColdCountBeforeTheLab,
                KnowledgeKeys.ProvisionedAdvanceKnowledge,
                KnowledgeKeys.CheckpointConscriptsConfession,
                KnowledgeKeys.QuartermastersPaperwork,
                KnowledgeKeys.InterceptedCipher,
                KnowledgeKeys.LedgerNobodySigned
            };

            foreach (string key in expansionKeys)
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);

                foreach (var trait in expansionTraits)
                {
                    Assert.True(entry.HasVariantForBias(trait),
                        $"Expansion key '{key}' missing variant for bias '{trait}'");
                }
            }
        }

        [Fact]
        public void GetProseReturnsCorrectVariant()
        {
            EnsureCatalogBound();

            string prose = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid);
            Assert.NotEmpty(prose);
            Assert.Contains("poison", prose);
        }

        [Fact]
        public void GetProseFallsBackToDefault()
        {
            EnsureCatalogBound();

            // ComposeBody should never return empty
            foreach (string key in KnowledgeKeys.All)
            {
                foreach (RiskBiasTrait trait in Enum.GetValues(typeof(RiskBiasTrait)))
                {
                    string prose = JournalVoice.ComposeBody(key, trait);
                    Assert.NotEmpty(prose);
                }
            }
        }

        [Fact]
        public void ComposeFullTextFormatsCorrectly()
        {
            EnsureCatalogBound();

            string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 45);
            Assert.StartsWith("Day 45.", fullText);
        }

        [Fact]
        public void FormatTimestampWorksCorrectly()
        {
            string ts1 = JournalVoice.FormatTimestamp(90);
            Assert.Equal("Day 90", ts1);

            string ts2 = JournalVoice.FormatTimestamp(90, 14.5f);
            Assert.Equal("Day 90, 14h", ts2);

            string ts3 = JournalVoice.FormatTimestamp(0);
            Assert.Equal("Day 1", ts3);

            string ts4 = JournalVoice.FormatTimestamp(5, 25);
            Assert.Equal("Day 5, 01h", ts4);
        }

        [Fact]
        public void AllProseTextIsNonEmpty()
        {
            var catalog = LoadCatalog();

            foreach (string key in catalog.GetAllKeys())
            {
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);

                // Check all variants
                if (!string.IsNullOrEmpty(entry.paranoid)) Assert.NotEmpty(entry.paranoid);
                if (!string.IsNullOrEmpty(entry.cautious)) Assert.NotEmpty(entry.cautious);
                if (!string.IsNullOrEmpty(entry.realist)) Assert.NotEmpty(entry.realist);
                if (!string.IsNullOrEmpty(entry.reckless)) Assert.NotEmpty(entry.reckless);
                if (!string.IsNullOrEmpty(entry.denialist)) Assert.NotEmpty(entry.denialist);
                if (!string.IsNullOrEmpty(entry.fatalist)) Assert.NotEmpty(entry.fatalist);
                if (!string.IsNullOrEmpty(entry.empath)) Assert.NotEmpty(entry.empath);
                if (!string.IsNullOrEmpty(entry.sociopath)) Assert.NotEmpty(entry.sociopath);
                Assert.NotEmpty(entry.@default);
            }
        }

        [Fact]
        public void CatalogCountMatchesExpected()
        {
            var catalog = LoadCatalog();
            Assert.True(catalog.Count >= KnowledgeKeys.All.Length,
                $"Catalog should have at least {KnowledgeKeys.All.Length} entries, has {catalog.Count}");
        }

        [Fact]
        public void UnknownKnowledgeKeyReturnsFallback()
        {
            EnsureCatalogBound();

            string prose = JournalVoice.ComposeBody("nonexistent_key", RiskBiasTrait.Realist);
            Assert.Equal("Something changed. I wrote it down so I would not forget.", prose);
        }

        [Fact]
        public void PinHighCo2ParanoidOutput()
        {
            EnsureCatalogBound();

            string expected = "The air is poison. Thick. My skull is a vice. We crack the vents or we choke — ash or no ash.";
            string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Paranoid);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PinHighCo2CautiousOutput()
        {
            EnsureCatalogBound();

            string expected = "My head is pounding. The air feels thick. We need to open the vents, even if the ash gets in.";
            string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Cautious);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PinHighCo2RealistOutput()
        {
            EnsureCatalogBound();

            string expected = "CO₂ is climbing — headache, heavy air. Crack a vent or the filter is finished. Ash comes with it.";
            string actual = JournalVoice.ComposeBody(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist);
            Assert.Equal(expected, actual);
        }

        [Fact]
        public void PinSeenRadiationAllBiasTraits()
        {
            EnsureCatalogBound();

            var expected = new Dictionary<RiskBiasTrait, string>
            {
                [RiskBiasTrait.Paranoid] = "The dosimeter twitched. Or I imagined it. Either way I will not take my coat off indoors.",
                [RiskBiasTrait.Cautious] = "I felt the dose climb. Not much — enough. We log it, we scrub, we do not pretend it is nothing.",
                [RiskBiasTrait.Realist] = "Radiation is on us now. Small number, real number. Keep the suits sealed when we go out.",
                [RiskBiasTrait.Reckless] = "Got a tick on the counter. Still standing. Wash the boots and keep moving.",
                [RiskBiasTrait.Denialist] = "The needle moved. Instruments lie. I feel fine.",
                [RiskBiasTrait.Fatalist] = "The dose goes up. It always does. Write it down so the next one knows."
            };

            foreach (var kvp in expected)
            {
                string actual = JournalVoice.ComposeBody(KnowledgeKeys.HasSeenRadiation, kvp.Key);
                Assert.Equal(kvp.Value, actual);
            }
        }

        [Fact]
        public void PinExperiencedStormAllBiasTraits()
        {
            EnsureCatalogBound();

            var expected = new Dictionary<RiskBiasTrait, string>
            {
                [RiskBiasTrait.Paranoid] = "The sky is eating the world. Fallout on the roof. Do not open anything. Not the hatch. Not a crack.",
                [RiskBiasTrait.Cautious] = "Storm hit. Ash and worse. Seal the intake if we can. No trips until it breaks.",
                [RiskBiasTrait.Realist] = "Fallout storm. Outdoor exposure spikes. Stay under concrete until the wind dies.",
                [RiskBiasTrait.Reckless] = "Ugly sky. Storm. If someone has to go out, make it short and make them count.",
                [RiskBiasTrait.Denialist] = "Weather's loud. It will pass. Always does.",
                [RiskBiasTrait.Fatalist] = "Storm again. The ash settles on everything. We wait. That is the work."
            };

            foreach (var kvp in expected)
            {
                string actual = JournalVoice.ComposeBody(KnowledgeKeys.HasExperiencedStorm, kvp.Key);
                Assert.Equal(kvp.Value, actual);
            }
        }

        [Fact]
        public void PinExpansionProseForEmpathAndSociopath()
        {
            EnsureCatalogBound();

            // Check expansion 06 prose has empath/sociopath variants
            string empathProse = JournalVoice.ComposeBody(
                KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Empath);
            Assert.Contains("confession", empathProse);

            string sociopathProse = JournalVoice.ComposeBody(
                KnowledgeKeys.CheckpointConscriptsConfession, RiskBiasTrait.Sociopath);
            Assert.Contains("Source", sociopathProse);
        }

        [Fact]
        public void ComposeFullTextPreservesDayPrefix()
        {
            EnsureCatalogBound();

            // Body already starts with "Day " should not get prefixed
            string fullText = JournalVoice.ComposeFullText(KnowledgeKeys.HighCo2, RiskBiasTrait.Realist, 90);
            Assert.StartsWith("Day 90. CO", fullText);
        }

        [Fact]
        public void CatalogBindIsIdempotent()
        {
            var catalog1 = LoadCatalog();
            var catalog2 = LoadCatalog();

            JournalVoice.BindCatalog(catalog1);
            Assert.Same(catalog1, JournalVoice.GetCatalog());

            JournalVoice.BindCatalog(catalog2);
            Assert.Same(catalog2, JournalVoice.GetCatalog());
        }

        [Fact]
        public void AllKnowledgeKeysAreLowercaseSnakeCase()
        {
            var catalog = LoadCatalog();

            foreach (string key in catalog.GetAllKeys())
            {
                Assert.Equal(key.ToLowerInvariant(), key);
                Assert.DoesNotContain(" ", key);
                Assert.DoesNotContain("-", key);
            }
        }
    }
}
