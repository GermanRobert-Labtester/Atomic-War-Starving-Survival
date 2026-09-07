// SPDX-License-Identifier: MIT
// Plan 117 — railway interlock engine: catalog load against canonical rail
// topology, route locks, conflicting reservations, signals, obstruction,
// maintenance, tamper, denial device, map/expedition bridge, determinism,
// save safety.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Xunit;

namespace Ashfall.Core.Tests.Expeditions
{
    public class RailwayInterlockEngineTests
    {
        private sealed class TestInventory
        {
            public Dictionary<string, int> Items = new Dictionary<string, int>(StringComparer.Ordinal);
            public int Get(string id) => Items.TryGetValue(id, out var v) ? v : 0;
            public void Consume(string id, int n)
            {
                var v = Get(id) - n;
                if (v < 0) throw new InvalidOperationException($"negative stock {id}");
                if (v == 0) Items.Remove(id); else Items[id] = v;
            }
        }

        private static RailwayInterlockCatalog CreateCatalog() => new RailwayInterlockCatalog
        {
            network_id = "rail_interlock_test",
            junctions = new List<RailwayJunctionDef>
            {
                new RailwayJunctionDef
                {
                    junction_id = "junction_alpha",
                    display_name = "Alpha Junction",
                    map_node_id = "rail_node_alpha",
                    connected_route_ids = new List<string> { "route_main", "route_siding" },
                    main_route_id = "route_main",
                    siding_route_ids = new List<string> { "route_siding" },
                    interlock_type = "relay",
                    maintenance_profile_id = "maint_test",
                    weather_sensitivity = 0.6f,
                    signal_profile_id = "signal_test",
                    unlock_requirements = new Dictionary<string, int> { ["item_switch_stand_module"] = 1 }
                },
                new RailwayJunctionDef
                {
                    junction_id = "junction_beta",
                    display_name = "Beta Junction",
                    map_node_id = "rail_node_beta",
                    connected_route_ids = new List<string> { "route_beta_leg" },
                    main_route_id = "route_beta_leg",
                    siding_route_ids = new List<string>(),
                    interlock_type = "mechanical",
                    maintenance_profile_id = "maint_test",
                    weather_sensitivity = 0.3f,
                    signal_profile_id = "signal_test",
                    unlock_requirements = new Dictionary<string, int> { ["item_switch_stand_module"] = 1 }
                }
            },
            maintenance_profiles = new List<RailwayMaintenanceProfile>
            {
                new RailwayMaintenanceProfile
                {
                    maintenance_profile_id = "maint_test",
                    required_items = new Dictionary<string, int> { ["item_track_maintenance_kit"] = 1 },
                    obstruction_per_severity_day = 0.12f,
                    obstruction_decay_per_day = 0.01f
                }
            }
        };

        private readonly Dictionary<RailwayInterlockEngine, TestInventory> _inventories =
            new Dictionary<RailwayInterlockEngine, TestInventory>();

        private TestInventory Inv(RailwayInterlockEngine engine) => _inventories[engine];

        private static string RepoPath(params string[] parts)
        {
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            return Path.Combine(new[] { root }.Concat(parts).ToArray());
        }

        private RailwayInterlockEngine CreateEngine(
            RailwayInterlockCatalog? catalog = null,
            int seed = 3307,
            float weather = 0f,
            float signalman = 0.5f,
            float inspector = 0.5f,
            bool restoreAll = true)
        {
            var inventory = new TestInventory();
            var engine = new RailwayInterlockEngine(new SeededRng(seed));
            engine.BindCatalog(catalog ?? CreateCatalog());
            engine.BindInventory(inventory.Get, inventory.Consume);
            engine.DayProvider = () => 200;
            engine.WeatherSeverityProvider = () => weather;
            engine.SignalmanSkillProvider = () => signalman;
            engine.TrackInspectorSkillProvider = () => inspector;
            _inventories[engine] = inventory;

            inventory.Items["item_switch_stand_module"] = 10;
            inventory.Items["item_track_maintenance_kit"] = 30;
            inventory.Items["item_signal_lamp_module"] = 10;
            inventory.Items["item_rail_control_component"] = 10;
            if (restoreAll)
            {
                foreach (var j in (catalog ?? CreateCatalog()).junctions)
                {
                    var r = engine.RestoreJunction(j.junction_id);
                    Assert.True(r.Status == ActionResult.StatusKind.Success, $"restore {j.junction_id}: {r.FailureCode}");
                }
            }
            return engine;
        }

        // ── 1. Catalog loads and references resolve against rail_network ─

        [Fact]
        public void Catalog_Loads_AndReferencesResolveAgainstRailNetwork()
        {
            string path = RepoPath("Assets", "StreamingAssets", "Data", "railway_interlock_catalog.json");
            Assert.True(File.Exists(path), $"catalog not found at {path}");
            var json = File.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<RailwayInterlockCatalog>(json);
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.Equal(3, catalog.junctions.Count);

            // Cross-reference against the canonical topology authority.
            string netPath = RepoPath("Assets", "StreamingAssets", "Data", "rail_network.json");
            var net = JsonSerializer.Deserialize<RailwayNetworkCatalog>(File.ReadAllText(netPath));
            Assert.NotNull(net);
            var nodeIds = net!.nodes.Select(n => n.node_id).ToHashSet();
            var segIds = net.segments.Select(s => s.segment_id).ToHashSet();

            Assert.All(catalog.junctions, j =>
            {
                Assert.True(nodeIds.Contains(j.map_node_id), $"unknown map node {j.map_node_id}");
                Assert.Contains(j.main_route_id, j.connected_route_ids);
                Assert.True(segIds.Contains(j.main_route_id), $"unknown route {j.main_route_id}");
                Assert.All(j.connected_route_ids, r => Assert.True(segIds.Contains(r), $"unknown route {r}"));
                Assert.All(j.siding_route_ids, r => Assert.Contains(r, j.connected_route_ids));
            });
        }

        // ── 2. Valid route locks ────────────────────────────────────────

        [Fact]
        public void ValidRoute_LocksJunctionAndSetsSignalClear()
        {
            var e = CreateEngine();
            var r = e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);

            var s = e.State.junctions["junction_alpha"];
            Assert.Equal("route_main", s.selected_route_id);
            Assert.Equal("locked", s.lock_state);
            Assert.Equal("clear", s.signal_state);
            Assert.Equal("exp_1", s.route_reserved_by_expedition_id);
        }

        // ── 3. Conflicting route rejected ───────────────────────────────

        [Fact]
        public void ConflictingRouteRequest_IsRejected()
        {
            var e = CreateEngine();
            Assert.Equal(ActionResult.StatusKind.Success, e.RequestRoute("junction_alpha", "route_main", "exp_1").Status);

            var conflict = e.RequestRoute("junction_alpha", "route_siding", "exp_2");
            Assert.Equal(ActionResult.StatusKind.Blocked, conflict.Status);
            Assert.Equal(RailwayInterlockFailures.ConflictingReservation, conflict.FailureCode);

            // Owner may re-request the same reservation (idempotent, no dup).
            var owner = e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Success, owner.Status);
            Assert.Equal("exp_1", e.State.junctions["junction_alpha"].route_reserved_by_expedition_id);
        }

        // ── 4. Signal cannot clear without valid lock ───────────────────

        [Fact]
        public void Signal_CannotClear_WithoutValidLock()
        {
            var e = CreateEngine(restoreAll: false);

            // Unrestored junction: request blocked, availability blocked.
            var denied = e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, denied.Status);
            Assert.Equal(RailwayInterlockFailures.JunctionNotRestored, denied.FailureCode);
            Assert.Contains("route_main", e.BuildAvailability().blocked_route_ids);

            // Tamper hold also blocks clearing after restoration.
            e.RestoreJunction("junction_alpha");
            e.State.junctions["junction_alpha"].tamper_risk_state = "detected";
            var tampered = e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, tampered.Status);
            Assert.Equal(RailwayInterlockFailures.TamperDetected, tampered.FailureCode);
            Assert.False(e.CanClearRoute("junction_alpha", "route_main"));
        }

        // ── 5. Reservation persists ─────────────────────────────────────

        [Fact]
        public void Reservation_PersistsThroughSaveLoad()
        {
            var e = CreateEngine();
            e.RequestRoute("junction_alpha", "route_main", "exp_7");
            e.RequestRoute("junction_beta", "route_beta_leg", "exp_8");

            var saved = e.CaptureState();
            var json = JsonSerializer.Serialize(saved);
            var restored = JsonSerializer.Deserialize<RailwayInterlockState>(json);

            var fresh = new RailwayInterlockEngine(new SeededRng(3307));
            fresh.BindCatalog(CreateCatalog());
            fresh.RestoreState(restored);

            var alpha = fresh.State.junctions["junction_alpha"];
            Assert.Equal("locked", alpha.lock_state);
            Assert.Equal("exp_7", alpha.route_reserved_by_expedition_id);
            Assert.Equal("route_main", alpha.selected_route_id);
            // Reservation survives: a conflicting request is still rejected.
            var conflict = fresh.RequestRoute("junction_alpha", "route_siding", "exp_9");
            Assert.Equal(ActionResult.StatusKind.Blocked, conflict.Status);
        }

        // ── 6. Route releases after passage ─────────────────────────────

        [Fact]
        public void Route_ReleasesAfterPassage()
        {
            var e = CreateEngine();
            e.RequestRoute("junction_alpha", "route_main", "exp_1");

            // Only the owner may release.
            var foreign = e.ReleaseRoute("junction_alpha", "exp_2");
            Assert.Equal(ActionResult.StatusKind.Blocked, foreign.Status);
            Assert.Equal(RailwayInterlockFailures.NotReservationOwner, foreign.FailureCode);

            var release = e.ReleaseRoute("junction_alpha", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Success, release.Status);
            Assert.Equal("unlocked", e.State.junctions["junction_alpha"].lock_state);
            Assert.Equal(string.Empty, e.State.junctions["junction_alpha"].route_reserved_by_expedition_id);
            // Released junction accepts a new reservation.
            Assert.Equal(ActionResult.StatusKind.Success, e.RequestRoute("junction_alpha", "route_siding", "exp_2").Status);
        }

        // ── 7. Siding route opens expected map edge ─────────────────────

        [Fact]
        public void SidingRoute_OpensExpectedEdge_WhenCleared()
        {
            // Authored catalog: delta junction siding = delta→dam segment.
            string path = RepoPath("Assets", "StreamingAssets", "Data", "railway_interlock_catalog.json");
            var catalog = JsonSerializer.Deserialize<RailwayInterlockCatalog>(File.ReadAllText(path));
            var e = CreateEngine(catalog, restoreAll: false);
            Inv(e).Items["item_switch_stand_module"] = 5;
            Inv(e).Items["item_signal_lamp_module"] = 5;
            Inv(e).Items["item_rail_control_component"] = 5;
            // Before restoration the whole junction mesh sits at stop: the
            // delta→dam edge is blocked by the uncontrolled junction.
            var unrestored = e.BuildAvailability();
            Assert.Contains("rail_segment_delta_to_dam", unrestored.blocked_route_ids);

            var restoreResult = e.RestoreJunction("junction_delta");
            Assert.Equal(ActionResult.StatusKind.Success, restoreResult.Status);

            Assert.Equal(ActionResult.StatusKind.Success,
                e.RequestRoute("junction_delta", "rail_segment_delta_to_dam", "exp_siding").Status);
            var after = e.BuildAvailability();
            Assert.Contains("rail_segment_delta_to_dam", after.open_route_ids);
            Assert.DoesNotContain("rail_segment_delta_to_dam", after.blocked_route_ids);
        }

        // ── 8. Obstruction closes/restricts route ───────────────────────

        [Fact]
        public void Obstruction_RestrictsThenClosesRoutes()
        {
            var e = CreateEngine(weather: 1f); // max severity every day
            // With signalman 0.5, tamper threshold ~18bp/day — drain suspicion
            // deterministically by inspecting when it appears.
            for (int d = 0; d < 8; d++)
            {
                e.TickDay(200 + d);
                foreach (var j in e.State.junctions.Values)
                    if (j.tamper_risk_state != "none") e.InspectJunction(j.junction_id);
            }

            var s = e.State.junctions["junction_alpha"]; // weather_sensitivity 0.6
            Assert.True(s.obstruction_level > 0.4f, $"expected heavy obstruction, got {s.obstruction_level}");
            Assert.Equal("obstructed", e.State.junctions["junction_alpha"].weather_obstruction_state);

            // Siding is refused while obstructed; mainline still clearable but restricted.
            var siding = e.RequestRoute("junction_alpha", "route_siding", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, siding.Status);
            Assert.Equal(RailwayInterlockFailures.RouteObstructed, siding.FailureCode);

            var snapshot = e.BuildAvailability();
            Assert.Contains("route_siding", snapshot.restricted_route_ids);

            // Sustained severity eventually closes the junction outright.
            for (int d = 8; d < 40; d++)
            {
                e.TickDay(200 + d);
                foreach (var j in e.State.junctions.Values)
                    if (j.tamper_risk_state != "none") e.InspectJunction(j.junction_id);
            }
            Assert.Equal("closed", e.State.junctions["junction_alpha"].weather_obstruction_state);
            Assert.False(e.CanClearRoute("junction_alpha", "route_main"));
            Assert.Contains("route_main", e.BuildAvailability().blocked_route_ids);
        }

        // ── 9. Maintenance restores route deterministically ─────────────

        [Fact]
        public void Maintenance_RestoresRouteDeterministically()
        {
            var run = () =>
            {
                var e = CreateEngine(weather: 1f, seed: 55);
                for (int d = 0; d < 40; d++)
                {
                    e.TickDay(200 + d);
                    foreach (var j in e.State.junctions.Values)
                        if (j.tamper_risk_state != "none") e.InspectJunction(j.junction_id);
                }
                Assert.Equal("closed", e.State.junctions["junction_alpha"].weather_obstruction_state);
                var m = e.MaintainJunction("junction_alpha");
                Assert.Equal(ActionResult.StatusKind.Success, m.Status);
                return (e.State.junctions["junction_alpha"], e.BuildAvailability());
            };

            var (stateA, snapA) = run();
            var (stateB, snapB) = run();
            Assert.Equal("clear", stateA.weather_obstruction_state);
            Assert.Equal(0, stateA.days_since_maintenance);
            Assert.Equal("nominal", stateA.maintenance_state);
            Assert.Contains("route_main", snapA.open_route_ids);
            // Deterministic: identical seed + identical history ⇒ identical recovery.
            Assert.Equal(snapA.open_route_ids, snapB.open_route_ids);
            Assert.Equal(snapA.blocked_route_ids, snapB.blocked_route_ids);
        }

        // ── 10. Tamper state requires inspection/closure ────────────────

        [Fact]
        public void TamperState_RequiresInspection()
        {
            var e = CreateEngine();
            var s = e.State.junctions["junction_alpha"];
            s.tamper_risk_state = "detected";
            s.signal_state = "fault";

            var request = e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.Equal(ActionResult.StatusKind.Blocked, request.Status);
            Assert.Contains("route_main", e.BuildAvailability().blocked_route_ids);

            var inspect = e.InspectJunction("junction_alpha");
            Assert.Equal(ActionResult.StatusKind.Success, inspect.Status);
            Assert.Equal("none", e.State.junctions["junction_alpha"].tamper_risk_state);
            Assert.Equal(200, e.State.junctions["junction_alpha"].last_inspection_day);

            // After inspection, route request succeeds again.
            Assert.Equal(ActionResult.StatusKind.Success, e.RequestRoute("junction_alpha", "route_main", "exp_1").Status);
        }

        // ── 11. Specialist modifier applied once ────────────────────────

        [Fact]
        public void SpecialistModifier_AppliedExactlyOnce()
        {
            // Track-inspector reduces obstruction accumulation by exactly
            // 30% (single factor) under identical seed and weather.
            var run = (float inspector) =>
            {
                var e = CreateEngine(weather: 1f, inspector: inspector, seed: 66, restoreAll: true, signalman: 0f);
                // Zero out tamper noise impact by reading before tamper fires.
                e.TickDay(200);
                return e.State.junctions["junction_alpha"].obstruction_level;
            };

            float noSkill = run(0f);
            float fullSkill = run(1f);
            float expected = noSkill * 0.7f; // single 30% reduction factor
            Assert.Equal(expected, fullSkill, 4);
        }

        // ── 12. Map consumes availability snapshot ──────────────────────

        [Fact]
        public void AvailabilitySnapshot_IsConsumableMapProjection()
        {
            var e = CreateEngine();
            // Open main, block beta by leaving it unreserved-but-faulted.
            e.RequestRoute("junction_alpha", "route_main", "exp_1");
            e.State.junctions["junction_beta"].signal_state = "fault";

            var snap = e.BuildAvailability();
            Assert.Contains("route_main", snap.open_route_ids);
            Assert.Contains("route_beta_leg", snap.blocked_route_ids);
            Assert.True(snap.grid_confidence > 0f && snap.grid_confidence <= 1f);
            // Deterministic projection: identical state ⇒ identical snapshot.
            var snap2 = e.BuildAvailability();
            Assert.Equal(snap.open_route_ids, snap2.open_route_ids);
            Assert.Equal(snap.blocked_route_ids, snap2.blocked_route_ids);
            Assert.Equal(snap.restricted_route_ids, snap2.restricted_route_ids);
        }

        // ── 13. Expedition path uses legal open route ───────────────────

        [Fact]
        public void ExpeditionPath_UsesLegalOpenRoute()
        {
            var e = CreateEngine();
            // Path through alpha main + beta leg — legal when both restored.
            List<string> legal = new List<string> { "route_main", "route_beta_leg" };
            Assert.True(e.IsExpeditionPathLegal(legal));

            // Beta faulted → its leg blocked → whole path illegal.
            e.State.junctions["junction_beta"].signal_state = "fault";
            Assert.False(e.IsExpeditionPathLegal(legal));

            // Alpha closed by reservation conflict? No — reservation blocks
            // *clearing*, but the path check reads availability: reserved
            // route stays open for its owner; beta is still the blocker.
            e.State.junctions["junction_beta"].signal_state = "stop";
            e.RequestRoute("junction_alpha", "route_main", "exp_1");
            Assert.True(e.IsExpeditionPathLegal(legal));
        }

        // ── 14. (folded into #1: data references resolve) ───────────────

        // ── 15. Content utilization reaches the engine ──────────────────

        [Fact]
        public void ContentUtilization_RegistersInterlockCatalog()
        {
            string scannerPath = RepoPath("Assets", "Ashfall.Core", "Content", "ContentUtilizationScanner.cs");
            Assert.True(File.Exists(scannerPath), $"scanner not found at {scannerPath}");
            var source = File.ReadAllText(scannerPath);
            Assert.Contains("\"railway_interlock_catalog.json\"", source);
            Assert.Contains("\"RailwayInterlockEngine\"", source);
        }

        // ── Extra: denial device lifecycle + no duplicate locks ─────────

        [Fact]
        public void DenialDevice_ArmedTriggeredSpent_ClosesRouteViaEncounterEvent()
        {
            var e = CreateEngine();
            string? triggeredRoute = null;
            e.OnRouteDenialTriggered += (_, route) => triggeredRoute = route;

            e.RequestRoute("junction_alpha", "route_main", "raider_band");
            Assert.Equal(ActionResult.StatusKind.Success, e.ArmRouteDenial("junction_alpha").Status);
            // Triggering releases the hold and stops the signal.
            Assert.Equal(ActionResult.StatusKind.Success, e.TriggerRouteDenial("junction_alpha").Status);
            Assert.Equal("triggered", e.State.junctions["junction_alpha"].denial_device_state);
            Assert.Equal("route_main", triggeredRoute);
            Assert.False(e.CanClearRoute("junction_alpha", "route_main"));
            Assert.Contains("route_main", e.BuildAvailability().blocked_route_ids);

            // Inspection spends the device; re-arm is then possible.
            e.InspectJunction("junction_alpha");
            Assert.Equal("spent", e.State.junctions["junction_alpha"].denial_device_state);
            Assert.Equal(ActionResult.StatusKind.Success, e.ArmRouteDenial("junction_alpha").Status);
            Assert.Equal("armed", e.State.junctions["junction_alpha"].denial_device_state);
        }

        [Fact]
        public void UnrestoredJunction_RoutesBlocked_NoInventedFaults()
        {
            var e = CreateEngine(restoreAll: false);
            for (int d = 0; d < 30; d++) e.TickDay(200 + d);
            // Old-save default: nothing restored, nothing degraded, all blocked.
            Assert.Empty(e.State.junctions);
            var snap = e.BuildAvailability();
            Assert.Equal(3, snap.blocked_route_ids.Count);
            Assert.Empty(snap.open_route_ids);
            Assert.Empty(e.State.junctions.Where(j => j.Value.signal_state == "fault"));
        }

        [Fact]
        public void RestoreJunction_PaysUnlockRequirements_Once()
        {
            var e = CreateEngine(restoreAll: false);
            int before = Inv(e).Get("item_switch_stand_module");
            Assert.Equal(ActionResult.StatusKind.Success, e.RestoreJunction("junction_alpha").Status);
            Assert.Equal(before - 1, Inv(e).Get("item_switch_stand_module"));

            var again = e.RestoreJunction("junction_alpha");
            Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
            Assert.Equal(before - 1, Inv(e).Get("item_switch_stand_module")); // no double charge
        }

        [Fact]
        public void SameSeedSameHistory_ProducesIdenticalGridState()
        {
            RailwayInterlockState Run()
            {
                var e = CreateEngine(weather: 0.7f, seed: 88);
                e.RequestRoute("junction_alpha", "route_main", "exp_1");
                for (int d = 0; d < 20; d++)
                {
                    e.TickDay(200 + d);
                    foreach (var j in e.State.junctions.Values)
                        if (j.tamper_risk_state == "detected") e.InspectJunction(j.junction_id);
                }
                e.ReleaseRoute("junction_alpha", "exp_1");
                return e.CaptureState();
            }

            var a = Run();
            var b = Run();
            Assert.Equal(
                JsonSerializer.Serialize(a),
                JsonSerializer.Serialize(b));
        }
    }
}
