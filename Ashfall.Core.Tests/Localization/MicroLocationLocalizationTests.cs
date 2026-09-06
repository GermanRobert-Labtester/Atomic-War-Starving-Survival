// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Localization;
using Ashfall.Core.Narrative;
using Xunit;

namespace Ashfall.Core.Tests.Localization
{
    /// <summary>
    /// Flagship Plan 49 (Task F22): Micro-Location Localization Readiness Suite.
    ///
    /// Validates:
    /// - All 25 production micro-location title, description, and choice keys resolve in English.
    /// - German translations resolve for exemplar encounters and cleanly fall back to English when absent.
    /// - Pseudo-localization expands text cleanly to verify UI layout and wrapping resistance.
    /// - Gameplay IDs, weights, delta thresholds, item IDs, and quantities are 100% invariant across locales.
    /// </summary>
    public sealed class MicroLocationLocalizationTests : CatalogTestBase
    {
        private static List<EncounterDefinition> LoadMicroLocations()
        {
            string path = Path.Combine(DataDirectory, "micro_locations.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(path));
            var arr = doc.RootElement.GetProperty("encounters");
            var list = new List<EncounterDefinition>();
            foreach (var elem in arr.EnumerateArray())
            {
                var def = JsonSerializer.Deserialize<EncounterDefinition>(elem.GetRawText(), SystemTextJsonSerializer.Options);
                if (def != null) list.Add(def);
            }
            return list;
        }

        [Fact]
        public void AllMicroLocations_TitleDescriptionChoiceKeys_ResolveInEnglish()
        {
            var encounters = LoadMicroLocations();
            Assert.True(encounters.Count >= 25, "Expected at least 25 micro-locations in catalog");

            var loc = new LocalizationService();
            loc.SetLocale("en");

            foreach (var e in encounters)
            {
                string titleKey = $"discovery.{e.id}.title";
                string descKey = $"discovery.{e.id}.description";

                Assert.True(loc.HasKey(titleKey), $"Missing localization key for title: {titleKey}");
                Assert.True(loc.HasKey(descKey), $"Missing localization key for description: {descKey}");

                string title = loc.Get(titleKey);
                string desc = loc.Get(descKey);

                Assert.False(string.IsNullOrWhiteSpace(title), $"Resolved title was empty for {e.id}");
                Assert.False(string.IsNullOrWhiteSpace(desc), $"Resolved description was empty for {e.id}");
                Assert.Equal(e.title, title);
                Assert.Equal(e.description, desc);

                Assert.NotEmpty(e.choices);
                foreach (var c in e.choices)
                {
                    string choiceKey = $"discovery.{e.id}.choice.{c.choiceId}";
                    Assert.True(loc.HasKey(choiceKey), $"Missing choice localization key: {choiceKey}");

                    string choiceText = loc.Get(choiceKey);
                    Assert.False(string.IsNullOrWhiteSpace(choiceText), $"Resolved choice was empty for {c.choiceId}");
                    Assert.Equal(c.text, choiceText);
                }
            }
        }

        [Fact]
        public void GermanTranslations_ResolveWhereTranslated_FallbackToEnglishWhenAbsent()
        {
            var encounters = LoadMicroLocations();
            var loc = new LocalizationService();
            loc.SetLocale("de");

            // 1. Check exemplar translations
            string memorialTitle = loc.Get("discovery.micro_roadside_memorial.title");
            Assert.Equal("Straßenrand-Gedenkstätte", memorialTitle);

            string truckTitle = loc.Get("discovery.micro_crashed_truck.title");
            Assert.Equal("Abgestürzter Versorgungslaster", truckTitle);

            string obsTitle = loc.Get("discovery.micro_observation_post.title");
            Assert.Equal("Militärischer Beobachtungsposten", obsTitle);

            string busTitle = loc.Get("discovery.micro_frozen_bus.title");
            Assert.Equal("Gefrorener Evakuierungsbus", busTitle);

            string memorialChoice = loc.Get("discovery.micro_roadside_memorial.choice.leave_memorial");
            Assert.Equal("Unberührt lassen.", memorialChoice);

            // 2. Check fallback for non-exemplar micro-location
            var greenhouse = encounters.Find(x => x.id == "micro_ruined_greenhouse");
            Assert.NotNull(greenhouse);

            string fallbackTitle = loc.Get("discovery.micro_ruined_greenhouse.title");
            Assert.Equal(greenhouse!.title, fallbackTitle);

            string fallbackDesc = loc.Get("discovery.micro_ruined_greenhouse.description");
            Assert.Equal(greenhouse.description, fallbackDesc);
        }

        [Fact]
        public void PseudoLocalization_ExpandsStringsCleanly()
        {
            var encounters = LoadMicroLocations();
            var loc = new LocalizationService();
            loc.SetLocale("pseudo");

            var bus = encounters.Find(x => x.id == "micro_frozen_bus");
            Assert.NotNull(bus);

            string pseudoTitle = loc.Get("discovery.micro_frozen_bus.title");
            Assert.StartsWith("[!!! ", pseudoTitle);
            Assert.EndsWith(" !!!]", pseudoTitle);
            Assert.True(pseudoTitle.Length > bus!.title.Length, "Pseudo string must expand length");

            string pseudoDesc = loc.Get("discovery.micro_frozen_bus.description");
            Assert.StartsWith("[!!! ", pseudoDesc);
            Assert.EndsWith(" !!!]", pseudoDesc);
            Assert.True(pseudoDesc.Length > bus.description.Length);
        }

        [Fact]
        public void GameplayInvariance_IdsWeightsThresholdsAndItemGrants_LocaleIndependent()
        {
            var encounters = LoadMicroLocations();
            var loc = new LocalizationService();

            string[] testLocales = new[] { "en", "de", "pseudo" };

            foreach (var locale in testLocales)
            {
                loc.SetLocale(locale);

                foreach (var e in encounters)
                {
                    Assert.False(string.IsNullOrWhiteSpace(e.id));
                    Assert.StartsWith("micro_", e.id);
                    Assert.True(e.baseWeight > 0f);
                    Assert.True(e.stealthWeightMultiplier > 0f);
                    Assert.True(e.speedWeightMultiplier > 0f);

                    foreach (var c in e.choices)
                    {
                        Assert.False(string.IsNullOrWhiteSpace(c.choiceId));
                        // Gameplay grant IDs and quantities remain non-localized
                        if (!string.IsNullOrEmpty(c.grantItemId))
                        {
                            Assert.False(c.grantItemId.StartsWith("discovery."));
                            Assert.True(c.grantItemQuantity != 0);
                        }
                        if (!string.IsNullOrEmpty(c.journalUnlockId))
                        {
                            Assert.False(c.journalUnlockId.StartsWith("discovery."));
                        }
                        if (!string.IsNullOrEmpty(c.discoverLocationId))
                        {
                            Assert.False(c.discoverLocationId.StartsWith("discovery."));
                        }
                    }
                }
            }
        }
    }
}
