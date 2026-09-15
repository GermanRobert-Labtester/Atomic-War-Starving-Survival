// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave B — dispatcher hardening contract tests.
// Pins the two semantic contracts the flagship hinges on:
//   §6.4  manuals REVEAL knowledge (never complete research)
//   §7.4  map clues are Surveyed location knowledge (never Visited, never routes)
// plus typed target validation, idempotence, and the no-vinyl-morale rule
// (§8.4 — vinyl morale ownership stays with VinylMoraleSystem).
// Data-derived (real catalogs) — no hardcoded 40-ID lists (Trap J).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Research;
using Ashfall.Core.Survivors;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleDispatcherHardeningTests
    {
        private static readonly string DataDir = FindDataDir();

        private static string FindDataDir()
        {
            string? dir = AppContext.BaseDirectory;
            while (dir != null)
            {
                string candidate = Path.Combine(dir, "Assets", "StreamingAssets", "Data");
                if (File.Exists(Path.Combine(candidate, "collectibles.json"))) return candidate;
                dir = Path.GetDirectoryName(dir);
            }
            throw new DirectoryNotFoundException("data authority not found");
        }

        private static CollectibleCatalog LoadCatalog()
        {
            var catalog = CollectibleCatalogLoader.Load(DataDir, new FileSystemIO(), new SystemTextJsonSerializer());
            Assert.NotNull(catalog);
            return catalog!;
        }

        private sealed class Fixture
        {
            public CollectibleCatalog Catalog = null!;
            public CollectibleDiscoveryState Discovery = new CollectibleDiscoveryState();
            public ResearchSystem Research = new ResearchSystem();
            public WastelandMapSystem Map = null!;
            public NeedsSystem Needs = new NeedsSystem();
            public int Day = 12;
            public int DiscoveryEvents;

            private static WastelandMapSystem BuildMap(CollectibleCatalog catalog)
            {
                var mapNodes = new List<MapNode>();
                foreach (var c in catalog.ByItemId.Values.Where(c => c.effect_type == "location_clue"))
                    if (!string.IsNullOrEmpty(c.effect_target))
                        mapNodes.Add(new MapNode { Id = c.effect_target, DisplayName = c.effect_target });
                return new WastelandMapSystem(new WastelandMapState(), mapNodes, new List<MapRoute>());
            }

            public CollectibleEffectDispatcher BuildDispatcher() =>
                new CollectibleEffectDispatcher(
                    Catalog, Discovery,
                    needsProvider: () => Needs,
                    researchProvider: () => Research,
                    mapProvider: () => Map,
                    dayProvider: () => Day);

            public Fixture()
            {
                Catalog = LoadCatalog();
                Map = BuildMap(Catalog);
                Research.OnResearchCompleted += _ => DiscoveryEvents += 1000; // must NEVER fire from a manual
                Needs.Register(new SurvivorNeedsState { Id = "s1" });
            }

            /// <summary>A second catalog view with extra fixture definitions appended
            /// (never mutates the shipped catalog). The MAP is deliberately NOT
            /// rebuilt — fixture targets stay unknown to the map authority, which
            /// is exactly what the unknown-node tests exercise.</summary>
            public Fixture WithExtraDefinitions(params CollectibleDefinition[] extras)
            {
                var defs = Catalog.ByItemId.Values.ToList();
                defs.AddRange(extras);
                Catalog = new CollectibleCatalog(defs);
                return this;
            }
        }

        private static ResearchKnowledgeDef Knowledge(string id, params string[] prereqs) => new ResearchKnowledgeDef
        {
            id = id,
            displayName = id.Replace('_', ' '),
            daysToComplete = 4,
            prerequisites = prereqs
        };

        // ── Task 5 contract: reveal, never complete ────────────────────

        [Fact]
        public void ManualAcquisition_RevealsKnowledgeNode_NeverCompletesIt()
        {
            var f = new Fixture();
            // Register only the five manual target nodes (data-derived).
            var targets = f.Catalog.ByItemId.Values
                .Where(d => d.effect_type == "knowledge")
                .Select(d => d.effect_target)
                .Distinct(StringComparer.Ordinal).ToList();
            Assert.True(targets.Count >= 5, "expected ≥5 knowledge-target collectibles in authored data");
            foreach (var t in targets) f.Research.Register(Knowledge(t));

            var dispatcher = f.BuildDispatcher();

            var manual = f.Catalog.ByItemId.Keys.First(id =>
                f.Catalog.GetByItemId(id)!.effect_target == "knowledge_diesel_mechanics");
            var result = dispatcher.DispatchOnAcquire(manual);

            Assert.True(result.EffectApplied);
            Assert.True(f.Research.IsManualUnlocked("knowledge_diesel_mechanics"));
            Assert.DoesNotContain("knowledge_diesel_mechanics", f.Research.State.completedIds);
            Assert.Null(f.Research.GetActiveResearch());
        }

        [Fact]
        public void ManualReveal_UnrelatedNodesUnchanged()
        {
            var f = new Fixture();
            f.Research.Register(Knowledge("knowledge_diesel_mechanics"));
            f.Research.Register(Knowledge("knowledge_radio_repair"));
            f.Research.Register(Knowledge("knowledge_water_treatment"));
            f.Research.Register(Knowledge("knowledge_air_filtration"));
            f.Research.Register(Knowledge("knowledge_radiation_measurement"));

            var dispatcher = f.BuildDispatcher();
            var manual = f.Catalog.ByItemId.Keys.First(id =>
                f.Catalog.GetByItemId(id)!.effect_target == "knowledge_air_filtration");
            dispatcher.DispatchOnAcquire(manual);

            Assert.True(f.Research.IsManualUnlocked("knowledge_air_filtration"));
            Assert.False(f.Research.IsManualUnlocked("knowledge_diesel_mechanics"));
            Assert.False(f.Research.IsManualUnlocked("knowledge_radio_repair"));
            Assert.False(f.Research.IsManualUnlocked("knowledge_water_treatment"));
            Assert.False(f.Research.IsManualUnlocked("knowledge_radiation_measurement"));
            Assert.Empty(f.Research.State.completedIds);
        }

        [Fact]
        public void ManualAcquisition_AllFiveAuthoredTargetsReveal()
        {
            var f = new Fixture();
            var targets = f.Catalog.ByItemId.Values
                .Where(d => d.effect_type == "knowledge")
                .Select(d => d.effect_target!)
                .Distinct(StringComparer.Ordinal).ToList();
            foreach (var t in targets) f.Research.Register(Knowledge(t));

            var dispatcher = f.BuildDispatcher();
            foreach (var itemId in f.Catalog.ByItemId.Keys.Where(id => f.Catalog.GetByItemId(id)!.effect_type == "knowledge"))
            {
                var r = dispatcher.DispatchOnAcquire(itemId);
                Assert.True(r.EffectApplied, $"{itemId}: {r.FailureReason}");
            }
            foreach (var t in targets)
                Assert.True(f.Research.IsManualUnlocked(t), $"{t} should be revealed");
            Assert.Equal(targets.Count, f.Research.State.unlockedIds.Count);
        }

        // ── Task 6 contract: surveyed knowledge, never visited/routes ──

        [Fact]
        public void MapClue_RevealsSurveyed_NeverVisited_WithClueProvenance()
        {
            var f = new Fixture();
            var dispatcher = f.BuildDispatcher();
            var mapItem = f.Catalog.ByItemId.Keys.First(id =>
                f.Catalog.GetByItemId(id)!.effect_type == "location_clue");
            var target = f.Catalog.GetByItemId(mapItem)!.effect_target!;

            var result = dispatcher.DispatchOnAcquire(mapItem);
            Assert.True(result.EffectApplied, result.FailureReason);

            Assert.True(f.Map.IsDiscovered(target));
            var knowledge = f.Map.GetNodeKnowledge(target);
            Assert.NotNull(knowledge);
            Assert.Equal(MapFogState.Surveyed, knowledge!.FogState);       // NOT Visited
            Assert.Equal("collectible_clue", knowledge.Provenance!.SourceId);
            Assert.Equal(1, f.Map.State.Knowledge.Count);                   // exactly one node touched
        }

        [Fact]
        public void MapClue_RouteDiscoveryStateUntouched()
        {
            var f = new Fixture();
            var dispatcher = f.BuildDispatcher();
            var mapItem = f.Catalog.ByItemId.Keys.First(id =>
                f.Catalog.GetByItemId(id)!.effect_type == "location_clue");
            var target = f.Catalog.GetByItemId(mapItem)!.effect_target!;

            int nodeCountBefore = f.Map.State.Discovered.Count;
            dispatcher.DispatchOnAcquire(mapItem);

            // Location reveal must not cascade: exactly one node known, and the
            // clue itself is the only source (no neighboring reveals).
            Assert.Equal(1, f.Map.State.Discovered.Count - nodeCountBefore);
            Assert.Contains(target, f.Map.State.Discovered);
            // Fog state stays below Visited — the expedition visit truth remains
            // exclusively owned by expeditions.
            Assert.NotEqual(MapFogState.Visited, f.Map.GetNodeKnowledge(target)!.FogState);
        }

        [Fact]
        public void MapClue_UnknownNode_TypedFailure_DiscoveryNotRegistered()
        {
            var f = new Fixture().WithExtraDefinitions(new CollectibleDefinition
            {
                item_id = "item_collectible_fixture_bad_map",
                category = "map", rarity = "common",
                effect_type = "location_clue",
                effect_target = "loc_definitely_not_a_node",
                effect_value = 0, unique = false
            });

            var dispatcher = f.BuildDispatcher();
            var result = dispatcher.DispatchOnAcquire("item_collectible_fixture_bad_map");

            Assert.True(result.IsCollectible);
            Assert.False(result.EffectApplied);
            Assert.Contains("map_node_not_found", result.FailureReason);
            Assert.False(f.Discovery.IsDiscovered("item_collectible_fixture_bad_map"));
        }

        [Fact]
        public void KnowledgeUnknownTarget_TypedFailure_DiscoveryNotRegistered()
        {
            var f = new Fixture().WithExtraDefinitions(new CollectibleDefinition
            {
                item_id = "item_collectible_fixture_manual",
                category = "technical_manual", rarity = "common",
                effect_type = "knowledge",
                effect_target = "knowledge_not_in_catalog",
                effect_value = 0, unique = false
            });

            var dispatcher = f.BuildDispatcher();
            var result = dispatcher.DispatchOnAcquire("item_collectible_fixture_manual");

            Assert.False(result.EffectApplied);
            Assert.Contains("effect_target_unknown", result.FailureReason);
            Assert.False(f.Discovery.IsDiscovered("item_collectible_fixture_manual"));
            Assert.False(f.Research.IsManualUnlocked("knowledge_not_in_catalog"));
        }

        // ── idempotence (§1.4) ─────────────────────────────────────────

        [Fact]
        public void Reacquisition_Idempotent_NoDuplicateDiscoveryEvent()
        {
            var f = new Fixture();
            f.Research.Register(Knowledge("knowledge_diesel_mechanics"));
            var dispatcher = f.BuildDispatcher();
            dispatcher.OnCollectibleDiscovered += _ => f.DiscoveryEvents++;

            var manual = f.Catalog.ByItemId.Keys.First(id =>
                f.Catalog.GetByItemId(id)!.effect_target == "knowledge_diesel_mechanics");

            var first = dispatcher.DispatchOnAcquire(manual);
            Assert.True(first.DiscoveryRegistered);
            Assert.Equal(1, f.DiscoveryEvents);

            var second = dispatcher.DispatchOnAcquire(manual);
            Assert.True(second.AlreadyDiscovered);
            Assert.Equal(1, f.DiscoveryEvents);                          // never re-fired
            Assert.Equal(1, f.Discovery.Count);                          // ledger stable
        }

        [Fact]
        public void VinylCollectible_DispatcherAppliesNoMorale()
        {
            var f = new Fixture();
            float moraleBefore = f.Needs.Get("s1")!.Morale;

            var vinylItem = f.Catalog.ByItemId.Keys.First(id =>
                string.Equals(f.Catalog.GetByItemId(id)!.category, "vinyl", StringComparison.Ordinal));
            var result = f.BuildDispatcher().DispatchOnAcquire(vinylItem);

            Assert.True(result.IsCollectible);
            Assert.True(result.EffectApplied);   // "none" branch registers discovery only
            Assert.Equal(moraleBefore, f.Needs.Get("s1")!.Morale);  // ZERO morale from collectible path
        }
    }
}
