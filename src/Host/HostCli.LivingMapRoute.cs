// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : LivingMapRouteSelfTest
// Core Authority     : Ashfall.Core.World.WastelandMapSystem.ProjectLivingMapRoute (EN-02)
// Purpose            : canonical route -> living-map projection (hops/distance/hazards/tags)
// ============================================================================

using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliLivingMapRoute
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Living Map Route Projection Self-Test (EN-02) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var emptyNodes = new List<MapNode>
                {
                    new MapNode { Id = "loc_a", StartingUnlocked = true },
                    new MapNode { Id = "loc_b", StartingUnlocked = false }
                };
                var emptySystem = new WastelandMapSystem(new WastelandMapState(), emptyNodes, new List<MapRoute>());
                var empty = emptySystem.ProjectLivingMapRoute("loc_a", "loc_b");

                if (empty != null && !empty.IsValid && empty.NodeIds.Count == 0 && empty.TotalHops == 0 && empty.TotalDistanceKm == 0f)
                {
                    Console.WriteLine("[PASS] Check 1: a route with no canonical edge projects as invalid/empty.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 1: empty route projection invalid state."); }

                var singleNodes = new List<MapNode>
                {
                    new MapNode { Id = "loc_holdfast", StartingUnlocked = true },
                    new MapNode { Id = "loc_swamp", StartingUnlocked = true }
                };
                var flooded = new MapRoute
                {
                    From = "loc_holdfast",
                    To = "loc_swamp",
                    DistanceKm = 8.5f,
                    Tags = new List<string> { "flooded", "amphibious", "swamp_marsh" }
                };
                var singleSystem = new WastelandMapSystem(new WastelandMapState(), singleNodes, new List<MapRoute> { flooded });
                var single = singleSystem.ProjectLivingMapRoute("loc_holdfast", "loc_swamp");

                if (single.IsValid && single.NodeIds.Count == 2 && single.TotalHops == 1
                    && Math.Abs(single.TotalDistanceKm - 8.5f) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 2: single hop projects 2 nodes / 1 hop / 8.5 km ({single.TotalDistanceKm:F1}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: single hop = {single.NodeIds.Count}/{single.TotalHops}/{single.TotalDistanceKm}."); }

                if (single.HasFloodedEdges && single.HasAmphibiousEdges
                    && single.AllTags.Contains("flooded") && single.AllTags.Contains("amphibious") && single.AllTags.Contains("swamp_marsh"))
                {
                    Console.WriteLine("[PASS] Check 3: flooded/amphibious edge flags and all tags are surfaced.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 3: hazard flags/tags missing."); }

                if (single.OriginNodeId == "loc_holdfast" && single.DestinationNodeId == "loc_swamp")
                {
                    Console.WriteLine("[PASS] Check 4: origin and destination endpoints are preserved.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 4: endpoints = {single.OriginNodeId}->{single.DestinationNodeId}."); }

                var multiNodes = new List<MapNode>
                {
                    new MapNode { Id = "loc_a", StartingUnlocked = true },
                    new MapNode { Id = "loc_b", StartingUnlocked = true },
                    new MapNode { Id = "loc_c", StartingUnlocked = true }
                };
                var route1 = new MapRoute { From = "loc_a", To = "loc_b", DistanceKm = 10f, Tags = new List<string> { "flooded" } };
                var route2 = new MapRoute { From = "loc_b", To = "loc_c", DistanceKm = 15.5f, Tags = new List<string> { "rocky" } };
                var multiSystem = new WastelandMapSystem(new WastelandMapState(), multiNodes, new List<MapRoute> { route1, route2 });
                var multi = multiSystem.ProjectLivingMapRoute("loc_a", "loc_c");

                if (multi.IsValid && multi.NodeIds.Count == 3 && multi.TotalHops == 2
                    && Math.Abs(multi.TotalDistanceKm - 25.5f) < 0.001f)
                {
                    Console.WriteLine($"[PASS] Check 5: multi-hop aggregates 3 nodes / 2 hops / 25.5 km ({multi.TotalDistanceKm:F1}).");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: multi = {multi.NodeIds.Count}/{multi.TotalHops}/{multi.TotalDistanceKm}."); }

                if (multi.HasFloodedEdges && !multi.HasAmphibiousEdges
                    && multi.AllTags.Contains("flooded") && multi.AllTags.Contains("rocky"))
                {
                    Console.WriteLine("[PASS] Check 6: mixed-edge hazards aggregate correctly across the path.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 6: multi-edge hazard aggregation wrong."); }

                var direct = new LivingMapRouteProjection(null, null, null, -5f, 0, false, false, null, false);
                if (direct.OriginNodeId == string.Empty && direct.DestinationNodeId == string.Empty
                    && direct.TotalDistanceKm == 0f && direct.TotalHops == 0 && !direct.IsValid && direct.NodeIds.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 7: the direct constructor null/negative-safe path holds.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 7: direct constructor not clamped."); }

                var defaultValid = new LivingMapRouteProjection("x", "y", new[] { "x", "y" }, 3f, 1, false, false, Array.Empty<string>());
                if (defaultValid.IsValid && defaultValid.AllTags.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 8: the constructor defaults to valid with an empty tag list.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: default validity wrong."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Living map route projection: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
