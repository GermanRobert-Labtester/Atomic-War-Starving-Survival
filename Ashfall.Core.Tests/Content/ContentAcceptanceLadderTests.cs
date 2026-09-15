// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    /// <summary>
    /// Current eight-rung acceptance contract. Date-expiring exemption policy
    /// was removed from ContentExemption and is deliberately not resurrected.
    /// </summary>
    public sealed class ContentAcceptanceLadderTests
    {
        [Fact]
        public void ContentAcceptanceRung_HasEightOrderedRungs()
        {
            Assert.Equal(1, (int)ContentAcceptanceRung.PARSES);
            Assert.Equal(2, (int)ContentAcceptanceRung.IDS_RESOLVE);
            Assert.Equal(3, (int)ContentAcceptanceRung.LOADED);
            Assert.Equal(4, (int)ContentAcceptanceRung.CONSUMER_EXISTS);
            Assert.Equal(5, (int)ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE);
            Assert.Equal(6, (int)ContentAcceptanceRung.EFFECT_PRODUCED);
            Assert.Equal(7, (int)ContentAcceptanceRung.PRESENTED);
            Assert.Equal(8, (int)ContentAcceptanceRung.SAVE_ROUNDTRIP);
        }

        // TEST-AGGREGATION: source_rows=6 aggregate_cases=1 saved_cases=5
        [Fact]
        public void DefaultRequiredRung_MatchesEachContentClassification()
        {
            var failures = new List<string>();
            foreach (var testCase in new[]
            {
                (ContentClassification.GAMEPLAY_CONSUMED, ContentAcceptanceRung.EFFECT_PRODUCED),
                (ContentClassification.UI_ONLY, ContentAcceptanceRung.PRESENTED),
                (ContentClassification.CODEX_ONLY, ContentAcceptanceRung.PRESENTED),
                (ContentClassification.OPTIONAL, ContentAcceptanceRung.LOADED),
                (ContentClassification.TEST_ONLY, ContentAcceptanceRung.LOADED),
                (ContentClassification.ORPHANED, ContentAcceptanceRung.CONSUMER_EXISTS)
            })
            {
                var catalog = new CatalogEntry { Path = "test.json", Classification = testCase.Item1 };
                ContentAcceptanceRung actual = ContentAcceptanceLadder.GetDefaultRequiredRung(catalog);
                if (actual != testCase.Item2)
                    failures.Add($"{testCase.Item1}: expected {testCase.Item2}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void EvaluateAchievedRung_FollowsCurrentEvidenceProgression()
        {
            var catalog = new CatalogEntry
            {
                Path = "test.json",
                DefinitionCount = 5,
                Classification = ContentClassification.UNRESOLVED
            };

            Assert.Equal(ContentAcceptanceRung.IDS_RESOLVE, ContentAcceptanceLadder.EvaluateAchievedRung(catalog));

            catalog.Loader = "TestLoader";
            Assert.Equal(ContentAcceptanceRung.LOADED, ContentAcceptanceLadder.EvaluateAchievedRung(catalog));

            catalog.ConsumerSystems.Add("TestSystem");
            Assert.Equal(ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE, ContentAcceptanceLadder.EvaluateAchievedRung(catalog));

            catalog.MaxStage = UtilizationStage.EFFECT_PRODUCED;
            catalog.Classification = ContentClassification.GAMEPLAY_CONSUMED;
            Assert.Equal(ContentAcceptanceRung.EFFECT_PRODUCED, ContentAcceptanceLadder.EvaluateAchievedRung(catalog));
        }

        [Fact]
        public void IsAccepted_RequiresTheConfiguredRung()
        {
            var catalog = new CatalogEntry
            {
                Path = "test.json",
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                AchievedRung = ContentAcceptanceRung.LOADED
            };

            Assert.False(ContentAcceptanceLadder.IsAccepted(catalog));
            catalog.AchievedRung = ContentAcceptanceRung.EFFECT_PRODUCED;
            Assert.True(ContentAcceptanceLadder.IsAccepted(catalog));
            catalog.AchievedRung = ContentAcceptanceRung.SAVE_ROUNDTRIP;
            Assert.True(ContentAcceptanceLadder.IsAccepted(catalog));
        }
    }
}
