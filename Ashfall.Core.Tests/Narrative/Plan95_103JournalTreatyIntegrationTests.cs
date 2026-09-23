// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Ashfall.Core.Journal;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Narrative
{
    /// <summary>
    /// Wave 40 Batch 7 Cross-System Integration Test:
    /// Validates Plan 95 (Journal Voice Prose & Narrative Tone - 33+ situation keys with 7 personality biases)
    /// alongside Plan 103 (Foundry Treaty Consequences Expansion - 15 consequence policies with market modifiers).
    /// </summary>
    public sealed class Plan95_103JournalTreatyIntegrationTests : CatalogTestBase
    {
        private static readonly string[] Plan95SituationKeys =
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

        [Fact]
        public void Plan95_JournalVoiceProseCatalog_LoadsAllSituationKeys_WithDistinctBiasVariants()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var loader = new JournalVoiceProseCatalogLoader(files, json);
            var catalog = loader.Load(DataDirectory);

            Assert.NotNull(catalog);
            Assert.True(catalog.Count >= 33, $"Expected at least 33 situation keys, got {catalog.Count}");

            foreach (var key in Plan95SituationKeys)
            {
                Assert.True(catalog.HasKey(key), $"Catalog must contain situation key '{key}'");
                var entry = catalog.GetEntry(key);
                Assert.NotNull(entry);
                Assert.False(string.IsNullOrWhiteSpace(entry.@default), $"Key '{key}' missing default variant");

                var distinctVariants = new HashSet<string>(StringComparer.Ordinal) { entry.@default };

                foreach (var trait in CoreTraits)
                {
                    Assert.True(entry.HasVariantForBias(trait), $"Key '{key}' missing variant for {trait}");
                    string text = entry.GetProseForBias(trait);
                    Assert.False(string.IsNullOrWhiteSpace(text), $"Key '{key}' text empty for {trait}");
                    Assert.True(distinctVariants.Add(text), $"Duplicate variant detected for key '{key}' under trait {trait}");
                }
            }
        }

        [Fact]
        public void Plan103_FoundryTreatyConsequences_LoadsAllFifteenPolicies_WithValidSignatoriesAndModifiers()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
            Assert.NotNull(rawConsequences);

            var catalog = new SilentFoundryConsequencePolicyCatalog();
            catalog.Load(rawConsequences);

            Assert.False(catalog.HasErrors, $"Catalog reported errors: {string.Join("; ", catalog.Errors)}");
            Assert.Equal(15, catalog.PolicyCount);
            Assert.Equal(15, catalog.AllPolicies.Count);

            string accordsRaw = files.ReadAllText(Path.Combine(DataDirectory, SilentFoundryCatalogLoader.AccordsFileName));
            var accords = json.Deserialize<RegionalTreatiesFile>(accordsRaw)!;
            var treatyIds = new HashSet<string>(accords.treaties.Select(t => t.treaty_id), StringComparer.Ordinal);

            var validOutcomes = new HashSet<string>(StringComparer.Ordinal) { "met", "missed", "violated" };

            foreach (var policy in catalog.AllPolicies)
            {
                Assert.True(treatyIds.Contains(policy.treaty_id), $"Treaty id '{policy.treaty_id}' must exist in foundry_accords.json");
                Assert.Contains(policy.outcome, validOutcomes);
                Assert.False(string.IsNullOrWhiteSpace(policy.faction_id), "Faction id must not be empty");
                Assert.False(string.IsNullOrWhiteSpace(policy.reason), "Reason must not be empty");
                Assert.InRange(policy.standing_delta, -50f, 30f);

                Assert.NotNull(policy.market_modifiers);
                foreach (var mod in policy.market_modifiers)
                {
                    Assert.False(string.IsNullOrWhiteSpace(mod.good_id), "Good id must not be empty");
                    Assert.NotEqual(0f, mod.demand_delta);
                    Assert.False(string.IsNullOrWhiteSpace(mod.reason), "Modifier reason must not be empty");
                }
            }
        }

        [Fact]
        public void CrossSystem_TreatyConsequenceOutcomesAndJournalVoices_ExhibitSystemicResonance()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            var journalLoader = new JournalVoiceProseCatalogLoader(files, json);
            var journalCatalog = journalLoader.Load(DataDirectory);

            var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
            var consequenceCatalog = new SilentFoundryConsequencePolicyCatalog();
            consequenceCatalog.Load(rawConsequences);

            // 1. Power failure resonance: treaty_coal_window missed/violated policies result in fuel shortage and production drops,
            // directly resonating with the 'power_failure' journal voice prose.
            var coalPolicies = consequenceCatalog.AllPolicies.Where(p => p.treaty_id == "treaty_coal_window").ToList();
            Assert.NotEmpty(coalPolicies);
            var coalViolated = consequenceCatalog.Find("treaty_coal_window", FoundryTreatyOutcome.Violated)
                               ?? consequenceCatalog.Find("treaty_coal_window", FoundryTreatyOutcome.Missed);
            Assert.NotNull(coalViolated);
            Assert.True(coalViolated.standing_delta < 0, "Violated coal window must penalize standing");

            var powerJournal = journalCatalog.GetEntry("power_failure");
            Assert.NotNull(powerJournal);
            Assert.Contains("refrigeration", powerJournal.GetProseForBias(RiskBiasTrait.Realist).ToLowerInvariant());
            Assert.Contains("breakers", powerJournal.GetProseForBias(RiskBiasTrait.Paranoid).ToLowerInvariant());

            // 2. Low water resonance: treaty_saltworks_access violated policies restrict water and penalize trade,
            // matching the 'low_water' journal voice prose.
            var saltViolated = consequenceCatalog.Find("treaty_saltworks_access", FoundryTreatyOutcome.Violated);
            Assert.NotNull(saltViolated);

            var waterJournal = journalCatalog.GetEntry("low_water");
            Assert.NotNull(waterJournal);
            Assert.False(string.IsNullOrWhiteSpace(waterJournal.GetProseForBias(RiskBiasTrait.Fatalist)));
            Assert.False(string.IsNullOrWhiteSpace(waterJournal.GetProseForBias(RiskBiasTrait.Cautious)));

            // 3. Moral compromise resonance: mutual aid pacts failing during emergencies mirror the 'moral_compromise' voice.
            var aidViolated = consequenceCatalog.Find("treaty_crisis_mutual_aid", FoundryTreatyOutcome.Violated);
            Assert.NotNull(aidViolated);
            var moralJournal = journalCatalog.GetEntry("moral_compromise");
            Assert.NotNull(moralJournal);
            Assert.False(string.IsNullOrWhiteSpace(moralJournal.@default));
            Assert.False(string.IsNullOrWhiteSpace(moralJournal.GetProseForBias(RiskBiasTrait.Denialist)));
        }

        [Fact]
        public void CrossSystem_DeterministicExecution_UnderRepeatedReloadsPasses()
        {
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();

            for (int i = 0; i < 50; i++)
            {
                var journalLoader = new JournalVoiceProseCatalogLoader(files, json);
                var journalCatalog = journalLoader.Load(DataDirectory);
                Assert.True(journalCatalog.Count >= 33);

                var rawConsequences = SilentFoundryConsequenceCatalogLoader.Load(DataDirectory, files, json);
                var consequenceCatalog = new SilentFoundryConsequencePolicyCatalog();
                consequenceCatalog.Load(rawConsequences);
                Assert.Equal(15, consequenceCatalog.PolicyCount);
            }
        }
    }
}
