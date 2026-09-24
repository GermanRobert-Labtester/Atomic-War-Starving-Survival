// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 167 (Underground Tunnel Network).

using System;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Underground;
using Ashfall.Core.World;

namespace AtomicWar.GodotApp
{
    public static class HostCliTunnelNetwork
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Underground Tunnel Network Self-Test (Plan 167) ===");
            int passed = 0;
            int total = 12;

            try
            {
                string path = Path.Combine(dataDir, "underground_tunnels.json");

                // Check 1: authored catalog exists and loads through the STRICT loader
                TunnelNetworkCatalogData? catalog = null;
                if (File.Exists(path))
                {
                    catalog = TunnelNetworkCatalogLoader.LoadFromJson(File.ReadAllText(path));
                }
                if (catalog != null && catalog.junctions.Count >= 3 && catalog.segments.Count >= 3)
                {
                    GD.Print($"[PASS] Check 1: Strict loader accepted authored catalog ({catalog.junctions.Count} junctions, {catalog.segments.Count} segments).");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 1: Authored tunnel catalog missing or did not load.");
                }

                // Check 2: strict loader rejects a duplicate segment id
                if (ExpectReject(
                    "{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}]," +
                    "\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\"}," +
                    "{\"id\":\"s1\",\"name\":\"S1b\",\"from\":\"a\",\"to\":\"c\"}]}"))
                {
                    GD.Print("[PASS] Check 2: Strict loader rejected a duplicate segment id.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 2: Duplicate segment id was not rejected.");
                }

                // Check 3: strict loader rejects an unknown hazard token
                if (ExpectReject(
                    "{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\"}]," +
                    "\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\",\"hazards\":[\"Dragonfire\"]}]}"))
                {
                    GD.Print("[PASS] Check 3: Strict loader rejected an unknown hazard token.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 3: Unknown hazard token was not rejected.");
                }

                // Check 4: strict loader rejects a junction -> missing segment reference
                if (ExpectReject(
                    "{\"schema_version\":1,\"junctions\":[{\"id\":\"j1\",\"name\":\"J1\",\"connected_segments\":[\"ghost\"]}]," +
                    "\"segments\":[{\"id\":\"s1\",\"name\":\"S1\",\"from\":\"a\",\"to\":\"b\"}]}"))
                {
                    GD.Print("[PASS] Check 4: Strict loader rejected a dangling junction segment reference.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 4: Dangling junction reference was not rejected.");
                }

                // Build the live authority from the authored catalog (or canonical fallback).
                var system = new TunnelNetworkSystem();
                if (catalog != null)
                {
                    system.LoadCatalog(catalog);
                }
                else
                {
                    system.RegisterJunction("loc_holdfast", "Holdfast Undercroft");
                    system.RegisterSegment("seg_fallback", "Fallback", "loc_holdfast", "loc_depot", 1.5f, 1, 90f);
                }

                var segments = system.CaptureState().Segments;
                var primary = segments.FirstOrDefault();

                // Check 5: catalog registered the authored topology
                if (system.TotalSegmentCount >= 3 && system.JunctionCount >= 3 && primary != null)
                {
                    GD.Print($"[PASS] Check 5: Live authority holds {system.TotalSegmentCount} segments and {system.JunctionCount} junctions.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Topology incomplete ({system.TotalSegmentCount} segments, {system.JunctionCount} junctions).");
                }

                // Check 6: discovery gates traversal (bidirectional)
                bool discovered = primary != null && system.DiscoverSegment(primary.SegmentId);
                var traverse = primary != null
                    ? system.CanTraverse(primary.ConnectsFrom, primary.ConnectsTo)
                    : (CanTraverse: false, TravelTimeHours: 0f, Reason: "no segment");
                if (discovered && traverse.CanTraverse && traverse.TravelTimeHours > 0f)
                {
                    GD.Print($"[PASS] Check 6: Discovered segment is traversable in {traverse.TravelTimeHours:0.0}h.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Discovery/traversal failed ({traverse.Reason}).");
                }

                // Check 7: reinforce raises integrity and clears collapse risk
                var livePrimary = primary != null ? system.FindSegment(primary.SegmentId) : null;
                if (livePrimary != null)
                {
                    livePrimary.StructuralIntegrity = 10f;
                    livePrimary.Status = TunnelStatus.Collapsed;
                    livePrimary.Hazards.Add(TunnelHazardType.CollapseRisk);
                    bool reinforced = system.ReinforceSegment(livePrimary.SegmentId, 40f);
                    if (reinforced && livePrimary.StructuralIntegrity >= 40f
                        && livePrimary.Status == TunnelStatus.Clear
                        && !livePrimary.Hazards.Contains(TunnelHazardType.CollapseRisk))
                    {
                        GD.Print("[PASS] Check 7: Reinforce raised integrity, cleared collapse, and reopened the segment.");
                        passed++;
                    }
                    else
                    {
                        GD.PrintErr($"[FAIL] Check 7: Reinforce failed (integrity={livePrimary.StructuralIntegrity}, status={livePrimary.Status}).");
                    }
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 7: No segment available for reinforce test.");
                }

                // Check 8: clear hazard on the first hazardous segment
                var hazardous = system.CaptureState().Segments.FirstOrDefault(s => s.Hazards.Count > 0);
                bool cleared = false;
                if (hazardous != null)
                {
                    var hazard = hazardous.Hazards[0];
                    cleared = system.ClearHazard(hazardous.SegmentId, hazard)
                        && !system.CaptureState().Segments.First(s => s.SegmentId == hazardous.SegmentId).Hazards.Contains(hazard);
                }
                if (cleared)
                {
                    GD.Print("[PASS] Check 8: Hazard cleared through the canonical authority.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 8: ClearHazard failed (no hazardous segment or removal rejected).");
                }

                // Check 9: daily tick degrades integrity and flags collapse risk
                var tickSegment = system.CaptureState().Segments.FirstOrDefault(s => s.Status != TunnelStatus.Collapsed);
                if (tickSegment != null)
                {
                    var live = system.FindSegment(tickSegment.SegmentId)!;
                    live.StructuralIntegrity = 20f;
                    live.Hazards.Remove(TunnelHazardType.CollapseRisk);
                    system.TickDay(5);
                    if (live.StructuralIntegrity < 20f && live.Hazards.Contains(TunnelHazardType.CollapseRisk))
                    {
                        GD.Print($"[PASS] Check 9: Daily tick degraded integrity to {live.StructuralIntegrity:0.00} and added CollapseRisk.");
                        passed++;
                    }
                    else
                    {
                        GD.PrintErr($"[FAIL] Check 9: Daily tick did not degrade/flag (integrity={live.StructuralIntegrity}).");
                    }
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 9: No non-collapsed segment for tick test.");
                }

                // Check 10: census reports truthful counts
                var census = system.GetCensus();
                if (census.TotalSegments == system.TotalSegmentCount
                    && census.TotalJunctions == system.JunctionCount
                    && census.DiscoveredSegments >= 1
                    && census.HazardousSegments >= 1
                    && census.AverageIntegrity > 0f)
                {
                    GD.Print($"[PASS] Check 10: Census accurate (segments={census.TotalSegments}, discovered={census.DiscoveredSegments}, avgIntegrity={census.AverageIntegrity:0.0}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Census mismatch (segments={census.TotalSegments}, discovered={census.DiscoveredSegments}, hazardous={census.HazardousSegments}).");
                }

                // Check 11: capture/restore round-trip
                var snapshot = system.CaptureState();
                var restored = new TunnelNetworkSystem();
                restored.RestoreState(snapshot);
                var restoredSegment = restored.FindSegment(primary?.SegmentId ?? string.Empty);
                var liveForCompare = primary != null ? system.FindSegment(primary.SegmentId) : null;
                if (restored.TotalSegmentCount == system.TotalSegmentCount
                    && restoredSegment != null
                    && liveForCompare != null
                    && Math.Abs(restoredSegment.StructuralIntegrity - liveForCompare.StructuralIntegrity) < 0.001f
                    && restoredSegment.Status == liveForCompare.Status)
                {
                    GD.Print("[PASS] Check 11: Capture/restore round-trip preserved segment state.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 11: Capture/restore did not preserve state.");
                }

                // Check 12: schema gate rejects a newer payload
                bool gated = false;
                try
                {
                    var newer = system.CaptureState();
                    newer.SchemaVersion = 99;
                    restored.RestoreState(newer);
                }
                catch (InvalidOperationException)
                {
                    gated = true;
                }
                if (gated)
                {
                    GD.Print("[PASS] Check 12: RestoreState rejected an unsupported future schema.");
                    passed++;
                }
                else
                {
                    GD.PrintErr("[FAIL] Check 12: RestoreState accepted an unsupported schema.");
                }

                // Extra host-integrity assertion: the map seeds the authored catalog.
                if (catalog != null)
                {
                    var map = new WastelandMapSystem(
                        new WastelandMapState(),
                        new[]
                        {
                            new MapNode { Id = "loc_holdfast", DisplayName = "Holdfast", Danger = MapNodeDanger.None, StartingUnlocked = true },
                            new MapNode { Id = "loc_cut_abandoned_depot", DisplayName = "Depot", Danger = MapNodeDanger.Low }
                        },
                        new[] { new MapRoute { From = "loc_holdfast", To = "loc_cut_abandoned_depot", DistanceKm = 5f } },
                        null,
                        catalog);
                    if (map.Tunnels.TotalSegmentCount == catalog.segments.Count)
                        GD.Print($"[PASS] (bonus) WastelandMapSystem seeded {map.Tunnels.TotalSegmentCount} authored tunnels.");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[FAIL] Exception in tunnel network self-test: {ex.Message}\n{ex.StackTrace}");
            }

            GD.Print($"=== Tunnel Network Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }

        private static bool ExpectReject(string json)
        {
            try
            {
                TunnelNetworkCatalogLoader.LoadFromJson(json);
                return false;
            }
            catch (InvalidOperationException)
            {
                return true;
            }
        }
    }
}
