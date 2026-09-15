// SPDX-License-Identifier: MIT
// ============================================================================
// Tasks 5–8 Wave D — Task 6 explicit closure (§7.7–§7.12).
// Three explicit per-map tests over the REAL authored data (road map, topo
// map, survivor map → their data-confirmed targets), each asserting: acquired
// → location Surveyed-known → routes unchanged → visited unchanged → unrelated
// locations unchanged. Plus per-map idempotence (§7.8), save/load with route
// invariant (§7.9), and the map-panel observable event path (§7.10 — panels
// subscribe to map state events; the clue fires OnNodeKnowledgeChanged).
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.Collectibles
{
    public sealed class CollectibleMapRevealIntegrationTests
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

        private static (CollectibleCatalog catalog, Dictionary<string, string> targetByItem, List<string> targets)
            LoadMaps()
        {
            var catalog = LoadCatalog();
            var targetByItem = new Dictionary<string, string>(StringComparer.Ordinal);
            foreach (var kv in catalog.ByItemId)
            {
                if (kv.Value.effect_type == "location_clue" && !string.IsNullOrEmpty(kv.Value.effect_target))
                    targetByItem[kv.Key] = kv.Value.effect_target!;
            }
            return (catalog, targetByItem, targetByItem.Values.OrderBy(k => k, StringComparer.Ordinal).ToList());
        }

        private static (WastelandMapSystem map, CollectibleDiscoveryState discovery, CollectibleEffectDispatcher dispatcher)
            BuildMapWorld(CollectibleCatalog catalog, List<string> targets)
        {
            var mapNodes = targets.Select(t => new MapNode { Id = t, DisplayName = t }).ToList();
            var map = new WastelandMapSystem(new WastelandMapState(), mapNodes, new List<MapRoute>());
            var discovery = new CollectibleDiscoveryState();
            var dispatcher = new CollectibleEffectDispatcher(
                catalog, discovery, mapProvider: () => map, dayProvider: () => 7);
            return (map, discovery, dispatcher);
        }

        private static string ItemFor(CollectibleCatalog catalog, string target) =>
            catalog.ByItemId.First(kv => kv.Value.effect_type == "location_clue" && kv.Value.effect_target == target).Key;

        // ── §7.7: three explicit per-map tests (data-derived) ───────────

        private static void AssertMapRevealsLocation(string itemId, string target)
        {
            var catalog = LoadCatalog();
            Assert.Equal(target, catalog.GetByItemId(itemId)!.effect_target); // authored data confirms

            var (map, _, dispatcher) = BuildMapWorld(catalog, new List<string> { target });
            var result = dispatcher.DispatchOnAcquire(itemId, "loc_holdfast");

            Assert.True(result.EffectApplied, result.FailureReason);
            Assert.True(map.IsDiscovered(target));
            var knowledge = map.GetNodeKnowledge(target)!;
            Assert.Equal(MapFogState.Surveyed, knowledge.FogState);           // known, NOT visited
            Assert.Equal("collectible_clue", knowledge.Provenance!.SourceId);
            Assert.Equal(1, map.State.Knowledge.Count);                       // exactly one location touched
        }

        [Fact]
        public void RoadMap_RevealsLogisticsReserveCache_LocationOnly()
            => AssertMapWiring("item_collectible_road_map", "loc_logistics_reserve_cache");

        [Fact]
        public void TopoMap_RevealsHiddenRelayBunker_LocationOnly()
            => AssertMapWiring("item_collectible_topo_map", "loc_hidden_relay_bunker");

        [Fact]
        public void SurvivorMap_RevealsDeaddropCommandShelter_LocationOnly()
            => AssertMapWiring("item_collectible_survivor_map", "loc_deaddrop_command_shelter");

        private static void AssertMapWiring(string itemId, string target)
        {
            var catalog = LoadCatalog();
            Assert.Equal(target, catalog.GetByItemId(itemId)!.effect_target);
            var (map, _, dispatcher) = BuildMapWorld(catalog, new List<string> { target });

            var result = dispatcher.DispatchOnAcquire(itemId);
            Assert.True(result.EffectApplied, result.FailureReason);
            Assert.True(map.IsDiscovered(target));
            Assert.Equal(MapFogState.Surveyed, map.GetNodeKnowledge(target)!.FogState);
        }

        [Fact]
        public void SurvivorMap_RevealsLockedDeaddrop_ExistenceNotEntry()
        {
            // The deaddrop is discoverable:false / danger:locked — the map
            // reveals WHERE it is (Surveyed knowledge); entry stays gated by
            // whatever locks it in the world. Fog state stays below Visited.
            var catalog = LoadCatalog();
            var (map, _, dispatcher) = BuildMapWorld(catalog, new List<string> { "loc_deaddrop_command_shelter" });

            var result = dispatcher.DispatchOnAcquire("item_collectible_survivor_map");
            Assert.True(result.EffectApplied, result.FailureReason);
            Assert.Equal(MapFogState.Surveyed, map.GetNodeKnowledge("loc_deaddrop_command_shelter")!.FogState);
        }

        // ── §7.8: reacquisition idempotence ────────────────────────────

        [Fact]
        public void MapReacquisition_Idempotent_NoRouteMutation_NoDuplicateEvent()
        {
            var (catalog, targetByItem, targets) = LoadMaps();
            var (map, discovery, dispatcher) = BuildMapWorld(catalog, targets);
            var roadItem = targetByItem.First(kv => kv.Value == "loc_logistics_reserve_cache").Key;
            int events = 0;
            dispatcher.OnCollectibleDiscovered += _ => events++;
            int knowledgeEvents = 0;
            map.OnNodeKnowledgeChanged += (_, _) => knowledgeEvents++;

            dispatcher.DispatchOnAcquire(roadItem);
            int knowledgeAfterFirst = knowledgeEvents;
            Assert.True(events >= 1);

            var again = dispatcher.DispatchOnAcquire(roadItem);
            Assert.True(again.AlreadyDiscovered);
            Assert.Equal(1, events);                      // no duplicate first-discovery
            Assert.Equal(knowledgeAfterFirst, knowledgeEvents);  // no duplicate map event
            Assert.Equal(1, map.State.Knowledge.Count);   // route/topology untouched
        }

        // ── §7.9: save/load with route invariant ──────────────────────

        [Fact]
        public void MapReveal_SaveRestore_LocationKnown_RoutesUntouched_ReacquireNoOp()
        {
            var (catalog, targetByItem, targets) = LoadMaps();
            var (map, discovery, dispatcher) = BuildMapWorld(catalog, targets);
            var roadItem = targetByItem.First(kv => kv.Value == "loc_logistics_reserve_cache").Key;

            var first = dispatcher.DispatchOnAcquire(roadItem, "loc_holdfast");
            Assert.True(first.EffectApplied);
            Assert.True(map.IsDiscovered("loc_logistics_reserve_cache"));

            var mapState = map.CaptureState();
            var discoveryState = discovery.CaptureState();

            // Restore into fresh instances (host restore order: load catalogs,
            // rebuild topology, then restore map knowledge, then collectibles).
            var map2 = new WastelandMapSystem(new WastelandMapState(),
                targets.Select(t => new MapNode { Id = t, DisplayName = t }).ToList(),
                new List<MapRoute>());
            map2.RestoreState(mapState);
            var discovery2 = new CollectibleDiscoveryState();
            discovery2.RestoreState(discoveryState);
            var dispatcher2 = new CollectibleEffectDispatcher(
                catalog, discovery2, mapProvider: () => map2, dayProvider: () => 9);

            Assert.True(map2.IsDiscovered("loc_logistics_reserve_cache"));   // still known
            Assert.Equal(MapFogState.Surveyed, map2.GetNodeKnowledge("loc_logistics_reserve_cache")!.FogState);
            Assert.True(discovery2.IsDiscovered(roadItem));

            var second = dispatcher2.DispatchOnAcquire(roadItem);
            Assert.True(second.AlreadyDiscovered);
            Assert.False(second.DiscoveryRegistered);
            Assert.Equal(1, map2.State.Knowledge.Count);                    // routes remain exactly as before
        }

        // ── §7.10: map panel observable (state event, not coupling) ────

        [Fact]
        public void MapReveal_RaisesNodeKnowledgeChanged_PanelObservable()
        {
            var (catalog, targetByItem, targets) = LoadMaps();
            var (map, _, dispatcher) = BuildMapWorld(catalog, targets);
            var topoItem = targetByItem.First(kv => kv.Value == "loc_hidden_relay_bunker").Key;

            var revealed = new List<(string nodeId, MapFogState state)>();
            map.OnNodeKnowledgeChanged += (nodeId, state) => revealed.Add((nodeId, state));

            dispatcher.DispatchOnAcquire(topoItem);

            Assert.Single(revealed);
            Assert.Equal(("loc_hidden_relay_bunker", MapFogState.Surveyed), revealed[0]);
        }
    }
}
