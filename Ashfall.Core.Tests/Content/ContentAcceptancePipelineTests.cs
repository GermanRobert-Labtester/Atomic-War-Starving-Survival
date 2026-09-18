// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests.Content
{
    public sealed class ContentAcceptancePipelineTests
    {
        [Fact]
        public void Pipeline_OrderedRungs_HasEightStrictRungs()
        {
            var rungs = ContentAcceptancePipeline.OrderedRungs;
            Assert.Equal(8, rungs.Count);
            Assert.Equal(ContentAcceptanceRung.PARSES, rungs[0]);
            Assert.Equal(ContentAcceptanceRung.IDS_RESOLVE, rungs[1]);
            Assert.Equal(ContentAcceptanceRung.LOADED, rungs[2]);
            Assert.Equal(ContentAcceptanceRung.CONSUMER_EXISTS, rungs[3]);
            Assert.Equal(ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE, rungs[4]);
            Assert.Equal(ContentAcceptanceRung.EFFECT_PRODUCED, rungs[5]);
            Assert.Equal(ContentAcceptanceRung.PRESENTED, rungs[6]);
            Assert.Equal(ContentAcceptanceRung.SAVE_ROUNDTRIP, rungs[7]);
        }

        [Fact]
        public void Evaluate_AllRungsMet_ReturnsSuccess()
        {
            var catalog = new CatalogEntry
            {
                Path = "items.json",
                DefinitionCount = 10,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "ItemCatalogLoader",
                MaxStage = UtilizationStage.EFFECT_PRODUCED
            };
            catalog.ConsumerSystems.Add("InventorySystem");

            var result = ContentAcceptancePipeline.Evaluate(catalog);
            Assert.True(result.IsSuccess);
            Assert.Null(result.FailedRung);
            Assert.Equal(6, result.RungResults.Count); // Rungs 1..6 evaluated up to required
        }

        [Fact]
        public void Evaluate_MissingLoader_FailsFastAtLoadedRung()
        {
            var catalog = new CatalogEntry
            {
                Path = "unloaded.json",
                DefinitionCount = 5,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "", // No loader!
                MaxStage = UtilizationStage.DISCOVERED
            };

            var result = ContentAcceptancePipeline.Evaluate(catalog, failFast: true);
            Assert.False(result.IsSuccess);
            Assert.Equal(ContentAcceptanceRung.LOADED, result.FailedRung);
            Assert.Contains("failed", result.Summary, StringComparison.OrdinalIgnoreCase);
        }

        [Fact]
        public void Evaluate_BrokenGate_FailsAtPlayerReachableRung()
        {
            var catalog = new CatalogEntry
            {
                Path = "blocked_quests.json",
                DefinitionCount = 4,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "QuestCatalogLoader",
                MaxStage = UtilizationStage.LOADED
            };
            catalog.ConsumerSystems.Add("QuestSystem");
            catalog.Findings.Add("BROKEN_GATE: quest condition unreachable");

            var result = ContentAcceptancePipeline.Evaluate(catalog, failFast: true);
            Assert.False(result.IsSuccess);
            Assert.Equal(ContentAcceptanceRung.PLAYER_OR_SIM_REACHABLE, result.FailedRung);
        }

        [Fact]
        public void EvaluateBatch_StopsAtFirstFailingCatalog()
        {
            var goodCatalog = new CatalogEntry
            {
                Path = "good.json",
                DefinitionCount = 5,
                Classification = ContentClassification.UI_ONLY,
                RequiredRung = ContentAcceptanceRung.LOADED,
                Loader = "GoodLoader",
                MaxStage = UtilizationStage.LOADED
            };

            var badCatalog = new CatalogEntry
            {
                Path = "bad.json",
                DefinitionCount = 2,
                Classification = ContentClassification.GAMEPLAY_CONSUMED,
                RequiredRung = ContentAcceptanceRung.EFFECT_PRODUCED,
                Loader = "",
                MaxStage = UtilizationStage.DISCOVERED
            };

            var result = ContentAcceptancePipeline.EvaluateBatch(new[] { goodCatalog, badCatalog }, failFast: true);
            Assert.False(result.IsSuccess);
            Assert.Equal(ContentAcceptanceRung.LOADED, result.FailedRung);
            Assert.Contains("bad.json", result.Summary);
        }
    }
}
