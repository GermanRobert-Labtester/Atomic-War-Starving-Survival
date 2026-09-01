using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Narrative;
using Ashfall.Core.Waystation;
using Ashfall.Core.World;
using Xunit;

namespace Ashfall.Core.Tests.World
{
    public sealed class Plan16CartographyTests
    {
        private static string GetDataDir()
        {
            return Path.Combine("..", "..", "..", "..", "Assets", "StreamingAssets", "Data");
        }

        [Fact]
        public void WastelandMap_60Nodes_202Routes_FullyConnectedAndReachable()
        {
            string dataDir = GetDataDir();
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDir);

            Assert.True(nodes.Count >= 6);
            Assert.True(routes.Count >= 7);

            var system = WastelandMapCatalogLoader.CreateSystem(dataDir);
            Assert.NotNull(system);

            // Discover all nodes first to enable full graph route planning
            foreach (var node in nodes)
            {
                system.Discover(node.Id);
            }

            // Verify all nodes reachable from loc_holdfast and returnable
            foreach (var node in nodes)
            {
                var path = system.PlanRoute("loc_holdfast", node.Id);
                Assert.NotEmpty(path);
                Assert.Equal("loc_holdfast", path.First());
                Assert.Equal(node.Id, path.Last());

                // Verify return route
                var returnPath = system.PlanRoute(node.Id, "loc_holdfast");
                Assert.NotEmpty(returnPath);
                Assert.Equal(node.Id, returnPath.First());
                Assert.Equal("loc_holdfast", returnPath.Last());
            }
        }

        [Fact]
        public void WastelandMap_AllRouteDistancesAndHazards_AreValid()
        {
            string dataDir = GetDataDir();
            var (_, routes) = WastelandMapCatalogLoader.Load(dataDir);

            foreach (var route in routes)
            {
                Assert.False(string.IsNullOrEmpty(route.From));
                Assert.False(string.IsNullOrEmpty(route.To));
                Assert.NotEqual(route.From, route.To);
                Assert.True(route.DistanceKm > 0f, $"Route {route.From}->{route.To} has invalid distance: {route.DistanceKm}");
                Assert.True(route.DistanceKm <= 25f, $"Route {route.From}->{route.To} exceeds distance limit: {route.DistanceKm}");
                Assert.True(route.WeatherHazard >= 0.05f && route.WeatherHazard <= 0.80f,
                    $"Route {route.From}->{route.To} has out-of-range hazard: {route.WeatherHazard}");
            }
        }

        [Fact]
        public void Waystations_CatalogLoadsAllSixWaystations_WithValidNodesAndKeepers()
        {
            string dataDir = GetDataDir();
            var waystations = WaystationCatalogLoader.Load(dataDir);

            Assert.Equal(6, waystations.Count);

            var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
            var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();

            foreach (var ws in waystations)
            {
                Assert.False(string.IsNullOrEmpty(ws.id));
                Assert.False(string.IsNullOrEmpty(ws.name));
                Assert.False(string.IsNullOrEmpty(ws.keeper_name));
                Assert.False(string.IsNullOrEmpty(ws.specialty));
                Assert.Contains(ws.node_id, mapNodeIds);
                Assert.NotEmpty(ws.services);
                Assert.NotEmpty(ws.stock_item_ids);
                Assert.True(ws.condition >= 50f && ws.condition <= 100f);
                Assert.True(ws.filter_health >= 50f && ws.filter_health <= 100f);
            }
        }

        [Fact]
        public void WaystationNetworkSystem_FilterDecayAndMaintenance_OperatesCorrectly()
        {
            string dataDir = GetDataDir();
            var catalog = WaystationCatalogLoader.Load(dataDir);
            var system = new WaystationNetworkSystem(catalog);

            var station = system.GetStation("waystation_alpha_cut");
            Assert.NotNull(station);
            float initialFilter = station.filterHealth;

            // Tick 5 days
            for (int i = 0; i < 5; i++)
            {
                system.TickDay();
            }

            Assert.True(station.filterHealth < initialFilter);

            // Perform maintenance
            bool repaired = system.RepairFilter("waystation_alpha_cut");
            Assert.True(repaired);
            Assert.Equal(100f, station.filterHealth);
            Assert.Equal(1, system.State.totalMaintenanceActions);

            // Assign watch
            bool assigned = system.AssignWatch("waystation_alpha_cut", new[] { "survivor_sentry_1", "survivor_sentry_2" });
            Assert.True(assigned);
            Assert.Equal(2, station.assignedWatchSurvivorIds.Count);
        }

        [Fact]
        public void Caravans_CatalogLoadsFourCircuits_AndAllNodesExistOnMap()
        {
            string dataDir = GetDataDir();
            var caravans = CaravanCatalogLoader.Load(dataDir);

            Assert.Equal(4, caravans.Count);

            var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
            var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();

            foreach (var c in caravans)
            {
                Assert.False(string.IsNullOrEmpty(c.caravan_id));
                Assert.False(string.IsNullOrEmpty(c.name));
                Assert.False(string.IsNullOrEmpty(c.faction_id));
                Assert.True(c.route_node_ids.Count >= 4);
                Assert.True(c.stay_duration_days >= 2);
                Assert.NotEmpty(c.specialty_goods);

                foreach (var nodeId in c.route_node_ids)
                {
                    Assert.Contains(nodeId, mapNodeIds);
                }
            }
        }

        [Fact]
        public void RegionalTreaties_LoadsAllTwelveAccords_WithDemarcationAndTariffs()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            var json = new SystemTextJsonSerializer();
            var catalog = new RegionalTreatyCatalog();
            catalog.Load(files.ReadAllText(Path.Combine(dataDir, "foundry_accords.json")), json);

            Assert.Equal(12, catalog.AllTreaties.Count);

            foreach (var treaty in catalog.AllTreaties)
            {
                Assert.False(string.IsNullOrEmpty(treaty.treaty_id));
                Assert.False(string.IsNullOrEmpty(treaty.treaty_title));
                Assert.NotEmpty(treaty.signatory_factions);
                Assert.False(string.IsNullOrEmpty(treaty.demarcated_territory));
                Assert.False(string.IsNullOrEmpty(treaty.treaty_articles));
            }
        }

        [Fact]
        public void DamagedMapZones_LoadsAllSixZones_WithValidFragments()
        {
            string dataDir = GetDataDir();
            var files = new FileSystemIO();
            string path = Path.Combine(dataDir, "damaged_map_zones.json");
            Assert.True(files.FileExists(path));

            var json = new SystemTextJsonSerializer();
            var raw = files.ReadAllText(path);
            var doc = System.Text.Json.JsonDocument.Parse(raw);
            var zones = doc.RootElement.GetProperty("zones");

            Assert.Equal(6, zones.GetArrayLength());

            var (mapNodes, _) = WastelandMapCatalogLoader.Load(dataDir);
            var mapNodeIds = mapNodes.Select(n => n.Id).ToHashSet();

            foreach (var zone in zones.EnumerateArray())
            {
                Assert.True(zone.TryGetProperty("zone_id", out var zid) && !string.IsNullOrEmpty(zid.GetString()));
                Assert.True(zone.TryGetProperty("total_fragments", out var tf) && tf.GetInt32() >= 2);
                Assert.True(zone.TryGetProperty("fragments", out var frags) && frags.GetArrayLength() == tf.GetInt32());
            }
        }

        [Fact]
        public void SaveRoundtrip_WaystationAndWastelandMapState_PreservesDeterminism()
        {
            string dataDir = GetDataDir();
            var mapSystem = WastelandMapCatalogLoader.CreateSystem(dataDir);
            mapSystem.Discover("loc_cut_abandoned_depot");
            mapSystem.Discover("loc_cut_merchant_caravanserai");
            mapSystem.Lock("loc_black_flotilla_outpost");

            var mapState = mapSystem.CaptureState();
            Assert.Contains("loc_cut_abandoned_depot", mapState.Discovered);
            Assert.Contains("loc_black_flotilla_outpost", mapState.Locked);

            var newMapSystem = new WastelandMapSystem(mapState, mapSystem.Nodes, mapSystem.Routes);
            Assert.True(newMapSystem.IsDiscovered("loc_cut_abandoned_depot"));
            Assert.True(newMapSystem.IsLocked("loc_black_flotilla_outpost"));

            // Waystation network state roundtrip
            var wsCatalog = WaystationCatalogLoader.Load(dataDir);
            var wsSystem = new WaystationNetworkSystem(wsCatalog);
            wsSystem.RepairFilter("waystation_alpha_cut");
            wsSystem.AssignWatch("waystation_verity", new[] { "survivor_medic_1" });

            var wsState = wsSystem.CaptureState();
            var newWsSystem = new WaystationNetworkSystem(wsCatalog, wsState);

            var verity = newWsSystem.GetStation("waystation_verity");
            Assert.NotNull(verity);
            Assert.Single(verity.assignedWatchSurvivorIds);
            Assert.Equal("survivor_medic_1", verity.assignedWatchSurvivorIds[0]);
        }
    }
}
