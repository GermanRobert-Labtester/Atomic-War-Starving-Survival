using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Excavation;
using Ashfall.Core.Narrative;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public class Plan11ExplorationTests
    {
        private static string ResolveDataDir()
        {
            string baseDir = AppContext.BaseDirectory;
            string probe = Path.Combine(baseDir, "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return probe;

            probe = Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            probe = Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data");
            if (Directory.Exists(probe)) return Path.GetFullPath(probe);

            return string.Empty;
        }

        [Fact]
        public void Excavation_FiveAuthoredSites_LoadAndValidateFromCatalog()
        {
            string dataDir = ResolveDataDir();
            var sites = ExcavationCatalogLoader.Load(dataDir);

            Assert.NotNull(sites);
            Assert.True(sites.Count >= 5, $"Expected at least 5 excavation sites, got {sites.Count}");

            var expectedIds = new[]
            {
                "excavation_command_vault",
                "excavation_utility_tunnels",
                "excavation_metro_interchange",
                "excavation_mine_shaft",
                "excavation_archive_bunker"
            };

            foreach (var id in expectedIds)
            {
                var site = sites.FirstOrDefault(s => s.site_id == id);
                Assert.NotNull(site);
                Assert.False(string.IsNullOrEmpty(site.location_id));
                Assert.True(site.max_depth_meters > 0f);
                Assert.True(site.required_progress > 0f);
                Assert.NotNull(site.depth_bands);
                Assert.True(site.depth_bands.Count >= 3, $"Site {id} should have at least 3 depth bands.");
            }
        }

        [Fact]
        public void Excavation_DeterministicDailyProgress_AndCaveInRisk()
        {
            var rng1 = new SeededRng(1986);
            var sys1 = new ExcavationSystem(rng1);
            sys1.AddSite("site_test_1", "room_test_1", 100f, 0.4f);
            sys1.AssignWorkers("site_test_1", 2);

            var rng2 = new SeededRng(1986);
            var sys2 = new ExcavationSystem(rng2);
            sys2.AddSite("site_test_1", "room_test_1", 100f, 0.4f);
            sys2.AssignWorkers("site_test_1", 2);

            for (int day = 0; day < 10; day++)
            {
                sys1.TickDay();
                sys2.TickDay();
            }

            Assert.Equal(sys1.State.sites[0].progress, sys2.State.sites[0].progress);
            Assert.Equal(sys1.State.sites[0].hasCavedIn, sys2.State.sites[0].hasCavedIn);
            Assert.Equal(sys1.State.sites[0].isComplete, sys2.State.sites[0].isComplete);
        }

        [Fact]
        public void Excavation_ShoringApplication_HalvesRiskAndBoostsSpeed()
        {
            var rng = new SeededRng(42);
            var sys = new ExcavationSystem(rng);
            sys.AddSite("site_shored", "room_test", 100f, 0.5f);
            sys.AssignWorkers("site_shored", 2);

            var site = sys.State.sites[0];
            float initialRisk = site.structuralRisk;

            var res = sys.ApplyShoring("site_shored");
            Assert.True(res.IsSuccess);
            Assert.True(site.shoringApplied);
            Assert.Equal(initialRisk * 0.5f, site.structuralRisk, precision: 3);

            // Shoring boosts daily progress by 1.2x (2 workers * 5 * 1.2 = 12 progress per day)
            sys.TickDay();
            Assert.True(site.progress >= 12f);
        }

        [Fact]
        public void Cipher_MultiOrder_HeardThenKey_DecodesAndRevealsLocation()
        {
            string dataDir = ResolveDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var engine = new CipherQuestChainEngine();

            var chain = engine.GetState("relay_count");
            Assert.False(chain.isHeard);
            Assert.False(chain.isKeyFound);
            Assert.False(chain.isDecoded);
            Assert.False(chain.isLocationRevealed);
            Assert.False(map.IsDiscovered("loc_hidden_relay_bunker"));

            // 1. Hear broadcast
            engine.RecordBroadcastHeard("radio_broadcast_relay_count", map);
            Assert.True(chain.isHeard);
            Assert.False(chain.isDecoded);
            Assert.False(map.IsDiscovered("loc_hidden_relay_bunker"));

            // 2. Find codebook
            engine.RecordKeyAcquired("item_comm_codebook_alpha", map);
            Assert.True(chain.isKeyFound);
            Assert.True(chain.isDecoded);
            Assert.True(chain.isLocationRevealed);
            Assert.True(map.IsDiscovered("loc_hidden_relay_bunker"));
        }

        [Fact]
        public void Cipher_MultiOrder_KeyThenHeard_DecodesAndRevealsLocation()
        {
            string dataDir = ResolveDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var engine = new CipherQuestChainEngine();

            var chain = engine.GetState("winter_ledger");
            Assert.False(chain.isHeard);
            Assert.False(chain.isKeyFound);
            Assert.False(chain.isDecoded);
            Assert.False(chain.isLocationRevealed);
            Assert.False(map.IsDiscovered("loc_logistics_reserve_cache"));

            // 1. Find key first
            engine.RecordKeyAcquired("item_logistics_cipher_sheet", map);
            Assert.True(chain.isKeyFound);
            Assert.False(chain.isDecoded);
            Assert.False(map.IsDiscovered("loc_logistics_reserve_cache"));

            // 2. Hear broadcast later
            engine.RecordBroadcastHeard("radio_broadcast_winter_ledger", map);
            Assert.True(chain.isHeard);
            Assert.True(chain.isDecoded);
            Assert.True(chain.isLocationRevealed);
            Assert.True(map.IsDiscovered("loc_logistics_reserve_cache"));
        }

        [Fact]
        public void Cipher_ThreeChains_HaveDistinctAuthoredDestinations()
        {
            var chains = CipherQuestChainEngine.Chains;
            Assert.Equal(3, chains.Count);

            var destinationIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            var questIds = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            foreach (var c in chains)
            {
                Assert.True(destinationIds.Add(c.TargetLocationId), $"Duplicate destination {c.TargetLocationId}");
                Assert.True(questIds.Add(c.QuestId), $"Duplicate quest {c.QuestId}");
            }
        }

        [Fact]
        public void WorldEvolution_TenEvents_LoadAndValidate()
        {
            string dataDir = ResolveDataDir();
            var engine = new WorldEvolutionEngine(dataDir);

            Assert.NotNull(engine.Events);
            Assert.True(engine.Events.Count >= 10, $"Expected at least 10 evolution events, got {engine.Events.Count}");

            var expectedTypes = new[] { "blockade", "territory_flip", "site_degradation", "hazard_bloom" };
            foreach (var type in expectedTypes)
            {
                Assert.Contains(engine.Events, e => e.type == type);
            }
        }

        [Fact]
        public void WorldEvolution_DayTrigger_LocksNodeAndMutatesLocation()
        {
            string dataDir = ResolveDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var evolution = new LocationEvolutionSystem(new SeededRng(42));
            var landmarks = new LandmarkDegradationSystem(new SeededRng(42));
            var engine = new WorldEvolutionEngine(dataDir);

            var flags = new HashSet<string>(StringComparer.OrdinalIgnoreCase) { "flag_faction_escalation" };

            Assert.False(map.IsLocked("loc_cut_abandoned_depot"));

            // Advance day to 20
            engine.TickDay(20, flags, evolution, landmarks, map);

            Assert.Contains("event_evolution_checkpoint_kilo", engine.TriggeredEventIds);
            Assert.True(map.IsLocked("loc_cut_abandoned_depot"));
        }

        [Fact]
        public void WorldEvolution_RouteBlockade_ForcesDetourOrRejectsPath()
        {
            string dataDir = ResolveDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);

            // Discover nodes for BFS route planning
            map.Discover("loc_holdfast");
            map.Discover("loc_cut_abandoned_depot");
            map.Discover("loc_cut_merchant_caravanserai");
            map.Discover("loc_cut_arsenal_ruin");

            // Baseline route: holdfast -> abandoned_depot -> arsenal_ruin
            var routeBaseline = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            Assert.NotEmpty(routeBaseline);
            Assert.Contains("loc_cut_abandoned_depot", routeBaseline);

            // Lock abandoned depot due to checkpoint blockade
            map.Lock("loc_cut_abandoned_depot");

            // Plan route again: should not path through locked node
            var routeDetour = map.PlanRoute("loc_holdfast", "loc_cut_arsenal_ruin");
            if (routeDetour.Count > 0)
            {
                Assert.DoesNotContain("loc_cut_abandoned_depot", routeDetour);
            }
        }

        [Fact]
        public void WorldEvolution_SaveAndRestore_IsIdempotent()
        {
            string dataDir = ResolveDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);
            var map = new WastelandMapSystem(new WastelandMapState(), nodes, routes);
            var evolution = new LocationEvolutionSystem(new SeededRng(42));
            var landmarks = new LandmarkDegradationSystem(new SeededRng(42));
            var engine1 = new WorldEvolutionEngine(dataDir);

            engine1.TickDay(25, null, evolution, landmarks, map);
            var captured = engine1.CaptureState();

            var engine2 = new WorldEvolutionEngine(dataDir);
            engine2.RestoreState(captured, map);

            Assert.Equal(captured.triggeredEventIds.Count, engine2.TriggeredEventIds.Count);
            foreach (var id in captured.triggeredEventIds)
            {
                Assert.Contains(id, engine2.TriggeredEventIds);
            }
        }
    }
}
