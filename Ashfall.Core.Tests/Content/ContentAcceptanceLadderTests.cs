// SPDX-License-Identifier: MIT
// ASHFALL Core Tests: Content Acceptance Ladder Tests
//
// Ticket REM-009 / R18 — Verifies 8-rung acceptance ladder, definition counting,
// and expiring exemption gate enforcement.

using System;
using System.Collections.Generic;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class ContentAcceptanceLadderTests
    {
        [Fact]
        public void ContentAcceptanceRung_Has8OrderedRungs()
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

        [Theory]
        [InlineData(ContentClassification.GAMEPLAY_CONSUMED, ContentAcceptanceRung.EFFECT_PRODUCED)]
        [InlineData(ContentClassification.UI_ONLY, ContentAcceptanceRung.PRESENTED)]
        [InlineData(ContentClassification.CODEX_ONLY, ContentAcceptanceRung.PRESENTED)]
        [InlineData(ContentClassification.OPTIONAL, ContentAcceptanceRung.LOADED)]
        [InlineData(ContentClassification.TEST_ONLY, ContentAcceptanceRung.LOADED)]
        [InlineData(ContentClassification.ORPHANED, ContentAcceptanceRung.CONSUMER_EXISTS)]
        public void ContentAcceptanceLadder_DefaultRequiredRung(ContentClassification classification, ContentAcceptanceRung expectedRung)
        {
            var cat = new CatalogEntry
            {
                Path = "test.json",
                Classification = classification
            };

            var required = ContentAcceptanceLadder.GetDefaultRequiredRung(cat);
            Assert.Equal(expectedRung, required);
        }

        [Fact]
        public void ContentAcceptanceLadder_EvaluateAchievedRung_Progression()
        {
            var cat = new CatalogEntry
            {
                Path = "test.json",
                DefinitionCount = 5,
                Classification = ContentClassification.UNRESOLVED
            };

            // No loader => IDS_RESOLVE
            Assert.Equal(ContentAcceptanceRung.IDS_RESOLVE, ContentAcceptanceLadder.EvaluateAchievedRung(cat));

            // Has loader => LOADED
            cat.Loader = "TestLoader";
            Assert.Equal(ContentAcceptanceRung.LOADED, ContentAcceptanceLadder.EvaluateAchievedRung(cat));

            // Has consumer => PLAYER_OR_SIM_REACHABLE
            cat.ConsumerSystems.Add("TestSystem");
            Assert.Equal(ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE, ContentAcceptanceLadder.EvaluateAchievedRung(cat));

            // Produces effect => EFFECT_PRODUCED
            cat.MaxStage = UtilizationStage.EFFECT_PRODUCED;
            cat.Classification = ContentClassification.GAMEPLAY_CONSUMED;
            Assert.Equal(ContentAcceptanceRung.EFFECT_PRODUCED, ContentAcceptanceLadder.EvaluateAchievedRung(cat));
        }

        [Fact]
        public void ContentAcceptanceLadder_IsAccepted_ValidatesRungs()
        {
            var cat = new CatalogEntry
            {
                Path = "test.json",
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                AchievedRung = ContentAcceptanceRung.LOADED
            };

            Assert.False(ContentAcceptanceLadder.IsAccepted(cat));

            cat.AchievedRung = ContentAcceptanceRung.EFFECT_PRODUCED;
            Assert.True(ContentAcceptanceLadder.IsAccepted(cat));

            cat.AchievedRung = ContentAcceptanceRung.SAVE_ROUNDTRIP;
            Assert.True(ContentAcceptanceLadder.IsAccepted(cat));
        }

        [Fact]
        public void ContentUtilizationGate_ExpiredExemption_FailsGate()
        {
            var graph = new ContentUtilizationGraph();
            graph.Catalogs.Add(new CatalogEntry
            {
                Path = "expired_content.json",
                Classification = ContentClassification.OPTIONAL,
                ExemptionId = "exempt_old"
            });

            var baseline = new UtilizationBaseline();
            baseline.CatalogClassifications["expired_content.json"] = ContentClassification.OPTIONAL.ToString();

            var registry = new ExemptionRegistry();
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_old",
                ContentPath = "expired_content.json",
                Owner = "team",
                Classification = "OPTIONAL",
                Rationale = "Was temporarily exempted",
                ExpiryDate = "2024-01-01" // In the past
            });

            var result = ContentUtilizationGate.Run(graph, baseline, registry, referenceDate: new DateTime(2026, 9, 4));

            Assert.False(result.Passed);
            Assert.Contains(result.Errors, e => e.StartsWith("EXPIRED EXEMPTION: exempt_old"));
        }

        [Fact]
        public void ContentUtilizationGate_InvalidExemption_MissingExpiry_FailsGate()
        {
            var graph = new ContentUtilizationGraph();
            var baseline = new UtilizationBaseline();

            var registry = new ExemptionRegistry();
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_bad_no_expiry",
                ContentPath = "test.json",
                Owner = "team",
                Classification = "OPTIONAL",
                Rationale = "Valid rationale but no expiry condition or date",
                ExpiryCondition = "",
                ExpiryDate = ""
            });

            var result = ContentUtilizationGate.Run(graph, baseline, registry);

            Assert.False(result.Passed);
            Assert.Contains(result.Errors, e => e.StartsWith("INVALID EXEMPTION: exempt_bad_no_expiry"));
        }

        [Fact]
        public void ContentUtilizationGate_ValidExemption_PassesGate()
        {
            var graph = new ContentUtilizationGraph();
            var baseline = new UtilizationBaseline();

            var registry = new ExemptionRegistry();
            registry.Exemptions.Add(new ContentExemption
            {
                ExemptionId = "exempt_good",
                ContentPath = "test.json",
                Owner = "team",
                Classification = "OPTIONAL",
                Rationale = "Valid rationale",
                ExpiryCondition = "Permanent codex flavor text",
                ExpiryDate = "2099-12-31"
            });

            var result = ContentUtilizationGate.Run(graph, baseline, registry, referenceDate: new DateTime(2026, 9, 4));

            Assert.True(result.Passed);
            Assert.Empty(result.Errors);
        }
    }
}
