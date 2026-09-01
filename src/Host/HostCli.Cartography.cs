using Godot;
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Economy;
using Ashfall.Core.Narrative;
using Ashfall.Core.Waystation;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        /// <summary>
        /// --cartography-selftest / --plan16-selftest:
        /// Verifies Plan 16 physical & institutional geography:
        /// 60-node wasteland map graph, 6 macro-regions, 6 waystations,
        /// 4 caravan circuits, 12-treaty accord web, and damaged map zones.
        /// </summary>
        public static int RunCartographySelfTest(string dataDirectory)
        {
            CatalogLocator.UseInvariantCulture();
            int failures = 0;
            int totalAssertions = 0;

            void Check(bool ok, string label)
            {
                totalAssertions++;
                GD.Print($"[{(ok ? "PASS" : "FAIL")}] {label}");
                if (!ok) failures++;
            }

            GD.Print("[CartographyHeadlessDemo] begin Plan 16 verification...");

            var json = new SystemTextJsonSerializer();
            var files = new FileSystemIO();

            // 1. Map Graph Densification & Reachability (60 nodes, 202 routes)
            var (nodes, routes) = WastelandMapCatalogLoader.Load(dataDirectory, files, json);
            Check(nodes != null && nodes.Count >= 60, $"Map nodes loaded (expected >= 60, got {nodes?.Count ?? 0})");
            Check(routes != null && routes.Count >= 200, $"Map routes loaded (expected >= 200, got {routes?.Count ?? 0})");

            var mapState = new WastelandMapState();
            var map = new WastelandMapSystem(mapState, nodes, routes);

            // Starting unlocked nodes
            Check(map.IsDiscovered("loc_holdfast"), "Holdfast starts discovered");
            Check(map.IsDiscovered("loc_cut_merchant_caravanserai"), "Merchant Caravanserai starts discovered");

            // Verify full reachability to all nodes
            int reachableCount = 0;
            if (nodes != null)
            {
                foreach (var node in nodes)
                {
                    map.Discover(node.Id);
                    map.Unlock(node.Id);
                }

                foreach (var node in nodes)
                {
                    var path = map.PlanRoute("loc_holdfast", node.Id);
                    if (path.Count > 0 && path.First() == "loc_holdfast" && path.Last() == node.Id)
                    {
                        reachableCount++;
                    }
                }
            }
            Check(reachableCount == (nodes?.Count ?? 0), $"All {nodes?.Count ?? 0} nodes reachable from loc_holdfast ({reachableCount}/{nodes?.Count ?? 0})");

            // 2. Waystation Infrastructure Network
            var waystations = WaystationCatalogLoader.Load(dataDirectory, files, json);
            Check(waystations != null && waystations.Count >= 6, $"Waystation catalog loaded (expected >= 6, got {waystations?.Count ?? 0})");

            var wsSystem = new WaystationNetworkSystem(waystations);
            var stationAlpha = wsSystem.GetStation("waystation_alpha_cut");
            Check(stationAlpha != null, "Waystation A (The Cut) instance initialized");

            float prevFilter = stationAlpha?.filterHealth ?? 100f;
            wsSystem.TickDay();
            Check((stationAlpha?.filterHealth ?? 100f) < prevFilter, "Waystation filter health degrades on day tick");

            wsSystem.RepairFilter("waystation_alpha_cut");
            Check(stationAlpha?.filterHealth == 100f, "Waystation filter repair restores to 100%");

            // 3. Caravan Circuit Network
            var caravans = CaravanCatalogLoader.Load(dataDirectory, files, json);
            Check(caravans != null && caravans.Count >= 4, $"Caravan catalog loaded (expected >= 4, got {caravans?.Count ?? 0})");

            var caravanSystem = new TravelingCaravanSystem();
            if (caravans != null)
            {
                foreach (var c in caravans)
                {
                    caravanSystem.SpawnCaravan(c.caravan_id, c.name, c.faction_id, c.route_node_ids, c.origin_region);
                }
            }
            Check(caravanSystem.CaravanCount >= 4, $"TravelingCaravanSystem initialized with {caravanSystem.CaravanCount} active caravans");

            // 4. Regional Treaty & Accord Web
            string treatyPath = System.IO.Path.Combine(dataDirectory, "foundry_accords.json");
            var treatyCatalog = new RegionalTreatyCatalog();
            if (files.FileExists(treatyPath))
            {
                treatyCatalog.Load(files.ReadAllText(treatyPath), json);
            }
            Check(treatyCatalog.AllTreaties.Count >= 12, $"Regional treaty catalog loaded (expected >= 12, got {treatyCatalog.AllTreaties.Count})");

            // 5. Damaged Map Regional Zones
            string damagedMapPath = System.IO.Path.Combine(dataDirectory, "damaged_map_zones.json");
            Check(files.FileExists(damagedMapPath), "damaged_map_zones.json exists on disk");

            var damagedDoc = System.Text.Json.JsonDocument.Parse(files.ReadAllText(damagedMapPath));
            var zones = damagedDoc.RootElement.GetProperty("zones");
            Check(zones.GetArrayLength() >= 6, $"Damaged map zones catalog has >= 6 regional zones (got {zones.GetArrayLength()})");

            // 6. Save Roundtrip Determinism
            var capturedMap = map.CaptureState();
            var restoredMap = new WastelandMapSystem(capturedMap, nodes, routes);
            Check(restoredMap.IsDiscovered("loc_cut_abandoned_depot"), "Restored map retains discovered nodes");

            GD.Print($"[CartographyHeadlessDemo] completed with {failures} failures across {totalAssertions} assertions.");
            return failures == 0 ? 0 : 1;
        }
    }
}
