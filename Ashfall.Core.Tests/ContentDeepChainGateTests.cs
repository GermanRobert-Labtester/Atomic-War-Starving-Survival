// SPDX-License-Identifier: MIT
// Plan 49 / Task 15 — deep content-utilization chain gate tests.
using System;
using System.Linq;
using Ashfall.Core.Content;
using Xunit;

namespace Ashfall.Core.Tests
{
    public sealed class ContentDeepChainGateTests
    {
        private static ContentUtilizationGraph CompleteGraph()
        {
            var g = new ContentUtilizationGraph();

            void Node(string id, ContentNodeKind kind) =>
                g.Nodes.Add(new ContentNode(id, kind, id));

            void Edge(string from, string to, ContentEdgeKind kind) =>
                g.Edges.Add(new ContentEdge(from, to, kind, EvidenceTier.STATIC));

            // Chain 1: research → recipe → craft
            Node("file:research_knowledge.json", ContentNodeKind.ContentFile);
            Node("loader:ResearchKnowledgeCatalogLoader", ContentNodeKind.Loader);
            Node("system:ResearchSystem", ContentNodeKind.RuntimeSystem);
            Node("ui:ResearchPanel", ContentNodeKind.UiSurface);
            Edge("file:research_knowledge.json", "loader:ResearchKnowledgeCatalogLoader", ContentEdgeKind.LOADED_BY);
            Edge("file:research_knowledge.json", "system:ResearchSystem", ContentEdgeKind.CONSUMED_BY);
            Edge("file:research_knowledge.json", "ui:ResearchPanel", ContentEdgeKind.DISPLAYED_BY);

            Node("file:recipes.json", ContentNodeKind.ContentFile);
            Node("loader:RecipeCatalogLoader", ContentNodeKind.Loader);
            Node("system:CraftingSystem", ContentNodeKind.RuntimeSystem);
            Edge("file:recipes.json", "loader:RecipeCatalogLoader", ContentEdgeKind.LOADED_BY);
            Edge("file:recipes.json", "system:CraftingSystem", ContentEdgeKind.CONSUMED_BY);

            Node("file:items.json", ContentNodeKind.ContentFile);
            Node("system:InventorySystem", ContentNodeKind.RuntimeSystem);
            Edge("file:items.json", "system:InventorySystem", ContentEdgeKind.CONSUMED_BY);

            // Chain 2: expedition → loot → inventory → use
            Node("file:expeditions.json", ContentNodeKind.ContentFile);
            Node("loader:ExpeditionCatalogLoader", ContentNodeKind.Loader);
            Node("system:ExpeditionSystem", ContentNodeKind.RuntimeSystem);
            Node("system:ExpeditionEncounterBridge", ContentNodeKind.RuntimeSystem);
            Node("ui:ExpeditionPanel", ContentNodeKind.UiSurface);
            Edge("file:expeditions.json", "loader:ExpeditionCatalogLoader", ContentEdgeKind.LOADED_BY);
            Edge("file:expeditions.json", "system:ExpeditionSystem", ContentEdgeKind.CONSUMED_BY);
            Edge("file:expeditions.json", "system:ExpeditionEncounterBridge", ContentEdgeKind.CONSUMED_BY);
            Edge("file:expeditions.json", "ui:ExpeditionPanel", ContentEdgeKind.DISPLAYED_BY);

            // Chain 3: faction → treaty → flag → briefing
            Node("file:diplomatic_treaties.json", ContentNodeKind.ContentFile);
            Node("loader:DiplomaticTreatyCatalogLoader", ContentNodeKind.Loader);
            Node("system:DiplomaticSummitSystem", ContentNodeKind.RuntimeSystem);
            Edge("file:diplomatic_treaties.json", "loader:DiplomaticTreatyCatalogLoader", ContentEdgeKind.LOADED_BY);
            Edge("file:diplomatic_treaties.json", "system:DiplomaticSummitSystem", ContentEdgeKind.CONSUMED_BY);

            Node("file:foundry_treaty_consequences.json", ContentNodeKind.ContentFile);
            Node("loader:SilentFoundryConsequencePolicy", ContentNodeKind.Loader);
            Node("system:SilentFoundrySystem", ContentNodeKind.RuntimeSystem);
            Node("ui:FoundryPanel", ContentNodeKind.UiSurface);
            Edge("file:foundry_treaty_consequences.json", "loader:SilentFoundryConsequencePolicy", ContentEdgeKind.LOADED_BY);
            Edge("file:foundry_treaty_consequences.json", "system:SilentFoundrySystem", ContentEdgeKind.CONSUMED_BY);
            Edge("file:foundry_treaty_consequences.json", "ui:FoundryPanel", ContentEdgeKind.DISPLAYED_BY);

            // Warn-tier: radio complete (so warn chains are quiet here)
            Node("file:radio.json", ContentNodeKind.ContentFile);
            Node("system:RadioBroadcastSystem", ContentNodeKind.RuntimeSystem);
            Node("ui:RadioPanel", ContentNodeKind.UiSurface);
            Edge("file:radio.json", "system:RadioBroadcastSystem", ContentEdgeKind.CONSUMED_BY);
            Edge("file:radio.json", "ui:RadioPanel", ContentEdgeKind.DISPLAYED_BY);

            Node("file:duty_roster_locations.json", ContentNodeKind.ContentFile);
            Node("system:DutyRosterCatalog", ContentNodeKind.RuntimeSystem);
            Edge("file:duty_roster_locations.json", "system:DutyRosterCatalog", ContentEdgeKind.CONSUMED_BY);

            Node("file:duty_roster_seasons.json", ContentNodeKind.ContentFile);
            Edge("file:duty_roster_seasons.json", "system:DutyRosterCatalog", ContentEdgeKind.CONSUMED_BY);

            Node("file:hydroponic_crops.json", ContentNodeKind.ContentFile);
            Edge("file:hydroponic_crops.json", "system:DutyRosterCatalog", ContentEdgeKind.CONSUMED_BY);

            return g;
        }

        [Fact]
        public void CompleteFlagshipChains_Pass()
        {
            var report = ContentDeepChainGate.Evaluate(CompleteGraph());
            Assert.True(report.HardGatePassed,
                "expected hard gate pass, got: " + string.Join("; ", report.Findings.Select(f => $"{f.ChainId}/{f.HopId}")));
            Assert.Equal(0, report.HardFailures);
            Assert.Equal(8, report.ChainsEvaluated);
        }

        [Fact]
        public void ProducerWithoutLoader_Fails()
        {
            var g = CompleteGraph();
            g.Edges.RemoveAll(e => e.From == "file:research_knowledge.json" && e.Kind == ContentEdgeKind.LOADED_BY);
            var report = ContentDeepChainGate.Evaluate(g);
            var f = Assert.Single(report.Findings, x => x.ChainId == "research-breakthrough-recipe-craft" && x.HopId == "research_loaded");
            Assert.Equal("DATA_WITHOUT_LOADER", f.MissingCategory);
            Assert.Equal("HARD", f.Severity);
        }

        [Fact]
        public void LoaderWithoutSystem_Fails()
        {
            var g = CompleteGraph();
            g.Edges.RemoveAll(e => e.From == "file:expeditions.json" && e.To == "system:ExpeditionSystem");
            var report = ContentDeepChainGate.Evaluate(g);
            var f = Assert.Single(report.Findings, x => x.ChainId == "expedition-loot-inventory-use" && x.HopId == "expedition_system_consumes");
            Assert.Equal("LOADER_WITHOUT_SYSTEM", f.MissingCategory);
        }

        [Fact]
        public void ConsumerWithoutSurface_Fails()
        {
            var g = CompleteGraph();
            g.Edges.RemoveAll(e => e.From == "file:research_knowledge.json" && e.Kind == ContentEdgeKind.DISPLAYED_BY);
            var report = ContentDeepChainGate.Evaluate(g);
            var f = Assert.Single(report.Findings, x => x.HopId == "player_surface" && x.ChainId == "research-breakthrough-recipe-craft");
            Assert.Equal("CONSUMER_WITHOUT_SURFACE", f.MissingCategory);
            Assert.Contains("ResearchPanel", f.Details, StringComparison.Ordinal);
        }

        [Fact]
        public void MissingProducer_Fails()
        {
            var g = CompleteGraph();
            g.Nodes.RemoveAll(n => n.Id == "file:diplomatic_treaties.json");
            g.Edges.RemoveAll(e => e.From == "file:diplomatic_treaties.json");
            var report = ContentDeepChainGate.Evaluate(g);
            Assert.Contains(report.Findings, x => x.ChainId == "faction-treaty-flag-briefing" && x.MissingCategory == "PRODUCER_MISSING");
        }

        [Fact]
        public void WarnTierBreaks_Warn_NeverHardFail()
        {
            var g = CompleteGraph();
            // Break radio chain: no consumer, no surface.
            g.Edges.RemoveAll(e => e.From == "file:radio.json");
            var report = ContentDeepChainGate.Evaluate(g);
            Assert.Equal(0, report.HardFailures);
            Assert.True(report.Warnings >= 2);
            Assert.All(report.Findings.Where(f => f.ChainId == "warn-radio-broadcast"),
                f => Assert.Equal("WARN", f.Severity));
        }

        [Fact]
        public void ExactMissingHop_IsReported()
        {
            var g = CompleteGraph();
            g.Edges.RemoveAll(e => e.From == "file:foundry_treaty_consequences.json" && e.To == "ui:FoundryPanel");
            var report = ContentDeepChainGate.Evaluate(g);
            var f = Assert.Single(report.Findings, x => x.HopId == "briefing_surface");
            Assert.Contains("ui:FoundryPanel", f.Details, StringComparison.Ordinal);
        }

        [Fact]
        public void Output_IsDeterministic()
        {
            var a = ContentDeepChainGate.Evaluate(CompleteGraph());
            var b = ContentDeepChainGate.Evaluate(CompleteGraph());
            Assert.Equal(
                string.Join("|", a.Findings.Select(f => $"{f.ChainId}/{f.HopId}/{f.MissingCategory}")),
                string.Join("|", b.Findings.Select(f => $"{f.ChainId}/{f.HopId}/{f.MissingCategory}")));
        }

        [Fact]
        public void Evaluation_IsBounded()
        {
            var g = CompleteGraph();
            var sw = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < 100; i++)
                ContentDeepChainGate.Evaluate(g);
            sw.Stop();
            Assert.True(sw.ElapsedMilliseconds < 5000, $"100 evaluations took {sw.ElapsedMilliseconds}ms");
        }
    }
}
