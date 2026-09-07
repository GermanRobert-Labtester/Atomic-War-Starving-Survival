// SPDX-License-Identifier: MIT
// Plan 115 — aquifer piezometer engine: catalog load, drawdown, contamination
// fronts, warnings, isolation, fouling/maintenance, water-advisory bridge,
// determinism, save safety, data integrity, content utilization.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Shelter
{
    public class AquiferPiezometerEngineTests
    {
        // ── Fixture helpers ─────────────────────────────────────────────

        /// <summary>Deterministic counting inventory (test-only port).</summary>
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

        private static AquiferPiezometerCatalog CreateCatalog() => new AquiferPiezometerCatalog
        {
            network_id = "aquifer_network_test",
            sensor_item_id = "item_groundwater_sensor",
            isolation_module_item_id = "item_aquifer_isolation_module",
            maintenance_kit_item_id = "item_well_maintenance_kit",
            strata = new List<PiezometerStrataDef>
            {
                new PiezometerStrataDef
                {
                    strata_id = "strata_shallow",
                    display_name = "Shallow Bench",
                    baseline_recharge_class = "high",
                    storage_capacity_class = "small",
                    fracture_susceptibility = 0.2f,
                    contamination_transport_class = "fast",
                    sensor_install_cost = new Dictionary<string, int> { ["item_groundwater_sensor"] = 2 },
                    monitoring_accuracy = 0.8f,
                    maintenance_profile_id = "maint_standard",
                    monitored_source_ids = new List<string> { "source_shallow_well" }
                },
                new PiezometerStrataDef
                {
                    strata_id = "strata_deep",
                    display_name = "Deep Bedrock",
                    baseline_recharge_class = "low",
                    storage_capacity_class = "large",
                    fracture_susceptibility = 0.8f,
                    contamination_transport_class = "slow",
                    sensor_install_cost = new Dictionary<string, int> { ["item_groundwater_sensor"] = 1 },
                    monitoring_accuracy = 0.6f,
                    maintenance_profile_id = "maint_standard",
                    monitored_source_ids = new List<string> { "source_deep_borehole" }
                }
            },
            maintenance_profiles = new List<PiezometerMaintenanceProfile>
            {
                new PiezometerMaintenanceProfile
                {
                    maintenance_profile_id = "maint_standard",
                    required_items = new Dictionary<string, int> { ["item_well_maintenance_kit"] = 1 },
                    fouling_per_day = 0.02f,
                    calibration_drift_per_day = 0.02f
                }
            },
            forecast = new PiezometerForecastTuning
            {
                warning_signal_threshold = 0.45f,
                critical_head_index = 0.25f,
                stable_head_index = 0.55f,
                minimum_warning_confidence = 0.2f
            }
        };

        private AquiferPiezometerEngine CreateConstructed(
            AquiferPiezometerCatalog? catalog = null,
            int seed = 4102,
            float pumpDemand = 0.4f,
            float seasonal = 0.5f,
            float hydrogeologist = 0f,
            float driller = 0.8f)
        {
            var inventory = new TestInventory();
            var warnings = new List<AquiferWarningRecord>();
            var engine = new AquiferPiezometerEngine(new SeededRng(seed));
            engine.BindCatalog(catalog ?? CreateCatalog());
            engine.BindInventory(inventory.Get, inventory.Consume);
            engine.DayProvider = () => 100;
            engine.PumpDemandProvider = () => pumpDemand;
            engine.SeasonalRechargeModifierProvider = () => seasonal;
            engine.HydrogeologistSkillProvider = () => hydrogeologist;
            engine.WellDrillerSkillProvider = () => driller;
            engine.OnWarningIssued += warnings.Add;

            _inventories[engine] = inventory;
            _warnings[engine] = warnings;

            inventory.Items["item_groundwater_sensor"] = 10;
            inventory.Items["item_well_maintenance_kit"] = 20;
            inventory.Items["item_aquifer_isolation_module"] = 5;
            var result = engine.ConstructNetwork();
            Assert.True(result.Status == ActionResult.StatusKind.Success, $"construct failed: {result.FailureCode}");
            return engine;
        }

        private readonly Dictionary<AquiferPiezometerEngine, TestInventory> _inventories =
            new Dictionary<AquiferPiezometerEngine, TestInventory>();

        private readonly Dictionary<AquiferPiezometerEngine, List<AquiferWarningRecord>> _warnings =
            new Dictionary<AquiferPiezometerEngine, List<AquiferWarningRecord>>();

        private TestInventory Inv(AquiferPiezometerEngine engine) => _inventories[engine];

        private List<AquiferWarningRecord> Warnings(AquiferPiezometerEngine engine) => _warnings[engine];

        private static string RepoPath(params string[] parts)
        {
            // bin/Debug/net9.0 → up 3 = Ashfall.Core.Tests, up 4 = repo root.
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            return Path.Combine(new[] { root }.Concat(parts).ToArray());
        }

        // ── 1. Catalog loads from the data authority ────────────────────

        [Fact]
        public void Catalog_LoadsFromDataAuthority()
        {
            string path = RepoPath("Assets", "StreamingAssets", "Data", "piezometer_network_catalog.json");
            Assert.True(File.Exists(path), $"catalog not found at {path}");

            var json = File.ReadAllText(path);
            var catalog = JsonSerializer.Deserialize<AquiferPiezometerCatalog>(json);
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.Equal(3, catalog.strata.Count);
            Assert.All(catalog.strata, s => Assert.False(string.IsNullOrEmpty(s.strata_id)));
            Assert.All(catalog.strata, s => Assert.NotEmpty(s.monitored_source_ids));
            Assert.Equal(2, catalog.maintenance_profiles.Count);
        }

        // ── 2. Stable aquifer reports stable trend ──────────────────────

        [Fact]
        public void StableAquifer_ReportsStableDrawdown()
        {
            var h = CreateConstructed(pumpDemand: 0.2f, seasonal: 0.8f);
            for (int d = 0; d < 20; d++) h.TickDay(100 + d);
            Assert.Equal("stable", h.State.drawdown_state);
        }

        // ── 3. High demand worsens drawdown state ───────────────────────

        [Fact]
        public void HighDemand_WorsensDrawdownState()
        {
            var low = CreateConstructed(pumpDemand: 0.05f, seasonal: 0.9f, seed: 77);
            var high = CreateConstructed(pumpDemand: 0.95f, seasonal: 0.1f, seed: 77);
            for (int d = 0; d < 60; d++)
            {
                low.TickDay(100 + d);
                high.TickDay(100 + d);
            }
            var order = new[] { "stable", "declining", "critical" };
            Assert.True(
                Array.IndexOf(order, high.State.drawdown_state)
                > Array.IndexOf(order, low.State.drawdown_state),
                $"expected worse drawdown under high demand: low={low.State.drawdown_state}, high={high.State.drawdown_state}");
        }

        // ── 4. Recharge improves forecast ───────────────────────────────

        [Fact]
        public void HighRecharge_ImprovesDrawdownAndRechargeState()
        {
            var dry = CreateConstructed(pumpDemand: 0.5f, seasonal: 0.05f, seed: 31);
            var wet = CreateConstructed(pumpDemand: 0.5f, seasonal: 0.95f, seed: 31);
            for (int d = 0; d < 40; d++)
            {
                dry.TickDay(100 + d);
                wet.TickDay(100 + d);
            }
            var order = new[] { "stable", "declining", "critical" };
            Assert.True(
                Array.IndexOf(order, wet.State.drawdown_state)
                <= Array.IndexOf(order, dry.State.drawdown_state),
                "better recharge should not worsen drawdown");
            Assert.True(wet.State.aquifer_health >= dry.State.aquifer_health - 0.001f,
                "better recharge should not reduce aquifer health");
        }

        // ── 5. Contamination front progresses deterministically ─────────

        [Fact]
        public void ContaminationFront_ProgressesDeterministically()
        {
            AquiferContaminationForecast? Run()
            {
                var h = CreateConstructed(seed: 99, pumpDemand: 0.5f, seasonal: 0.5f);
                h.ReportSurfaceContaminationEvent(0.8f);
                for (int d = 0; d < 30; d++) h.TickDay(100 + d);
                return h.ForecastContamination();
            }

            var a = Run();
            var b = Run();
            Assert.Equal(a.risk_level, b.risk_level);
            Assert.Equal(a.estimated_arrival_window_min, b.estimated_arrival_window_min);
            Assert.Equal(a.estimated_arrival_window_max, b.estimated_arrival_window_max);
            Assert.Equal(a.affected_source_ids, b.affected_source_ids);
            Assert.Equal(a.confidence, b.confidence, 4);

            // Same event history → same contamination signal per node.
            var run = () =>
            {
                var h = CreateConstructed(seed: 99, pumpDemand: 0.5f, seasonal: 0.5f);
                h.ReportSurfaceContaminationEvent(0.8f);
                for (int d = 0; d < 30; d++) h.TickDay(100 + d);
                return h.State.nodes.Select(n => n.contamination_signal).ToList();
            };
            var sigA = run();
            var sigB = run();
            Assert.Equal(sigA, sigB);
        }

        // ── 6/7. Fouling reduces confidence; maintenance restores it ────

        [Fact]
        public void Fouling_ReducesConfidence_AndMaintenanceRestoresIt()
        {
            var h = CreateConstructed(seed: 5, hydrogeologist: 0f);
            float fresh = h.State.monitoring_confidence;

            for (int d = 0; d < 120; d++) h.TickDay(100 + d);
            float degraded = h.State.monitoring_confidence;
            Assert.True(degraded < fresh, $"expected fouling to reduce confidence: {fresh} -> {degraded}");

            var node = h.State.nodes[0];
            var result = h.MaintainNode(node.node_id);
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);
            Assert.True(h.State.monitoring_confidence > degraded,
                "maintenance should raise confidence above the degraded value");
        }

        // ── 8. Isolation removes affected source from intake advisory ───

        [Fact]
        public void Isolation_RemovesAffectedSourceFromAdvisory()
        {
            var h = CreateConstructed(seed: 7);
            // Drive a severe contamination front on the shallow stratum.
            h.ReportSurfaceContaminationEvent(1f);
            for (int d = 0; d < 40; d++) h.TickDay(100 + d);

            var before = h.BuildAdvisory();
            Assert.Contains("source_shallow_well", before.contaminated_source_ids);

            var result = h.IsolateAquiferZone("strata_shallow");
            Assert.Equal(ActionResult.StatusKind.Success, result.Status);

            var after = h.BuildAdvisory();
            Assert.DoesNotContain("source_shallow_well", after.contaminated_source_ids);
        }

        // ── 9. Isolation reduces available-source set ───────────────────

        [Fact]
        public void Isolation_ReducesAvailableSourceSet()
        {
            var h = CreateConstructed(seed: 7);
            var all = h.AvailableSourceIds();
            Assert.Equal(2, all.Count);

            h.IsolateAquiferZone("strata_shallow");
            var remaining = h.AvailableSourceIds();
            Assert.DoesNotContain("source_shallow_well", remaining);
            Assert.Contains("source_deep_borehole", remaining);
            Assert.Single(remaining);

            Assert.True(h.IsSourceIsolated("source_shallow_well"));
            Assert.False(h.IsSourceIsolated("source_deep_borehole"));

            // Isolation resolves the open warning for that zone.
            Assert.All(Warnings(h), w => Assert.NotEqual("strata_shallow", w.strata_id));

            // Reconnect restores the set.
            h.ReconnectAquiferZone("strata_shallow");
            Assert.Equal(2, h.AvailableSourceIds().Count);
        }

        // ── 10. Specialist modifier applied exactly once ────────────────

        [Fact]
        public void SpecialistModifier_AppliedExactlyOnce()
        {
            // Compared on freshly constructed networks: node state is
            // identical, so the confidence ratio must be exactly the single
            // documented interpretation factor (1 + 0.15). Drift-rate skill
            // benefits only appear after ticks and are a separate mechanism.
            var noSkill = CreateConstructed(seed: 11, hydrogeologist: 0f);
            var fullSkill = CreateConstructed(seed: 11, hydrogeologist: 1f);
            float expected = noSkill.State.monitoring_confidence * 1.15f;
            Assert.Equal(expected, fullSkill.State.monitoring_confidence, 3);

            // The specialist factor must not compound: full skill must not
            // exceed the single-factor ceiling on identical node state.
            Assert.True(fullSkill.State.monitoring_confidence
                <= noSkill.State.monitoring_confidence * 1.15f + 0.0001f);
        }

        // ── 11. Warning persists through save/load ──────────────────────

        [Fact]
        public void Warning_PersistsThroughSaveLoad()
        {
            var h = CreateConstructed(seed: 23);
            h.ReportSurfaceContaminationEvent(1f);
            for (int d = 0; d < 60; d++) h.TickDay(100 + d);
            Assert.NotEmpty(Warnings(h));

            var saved = h.CaptureState();
            var json = JsonSerializer.Serialize(saved);
            var restored = JsonSerializer.Deserialize<HydrogeologyNetworkState>(json);

            var fresh = new AquiferPiezometerEngine(new SeededRng(23));
            fresh.BindCatalog(CreateCatalog());
            fresh.RestoreState(restored);

            int warningsBefore = Warnings(h).Count;
            // Continue ticking after reload — no re-fire, no reroll.
            for (int d = 0; d < 10; d++) fresh.TickDay(200 + d);
            Assert.Equal(saved.warnings.Count, fresh.State.warnings.Count);
            for (int i = 0; i < saved.warnings.Count; i++)
            {
                Assert.Equal(saved.warnings[i].warning_id, fresh.State.warnings[i].warning_id);
                Assert.Equal(saved.warnings[i].issued_day, fresh.State.warnings[i].issued_day);
            }
            Assert.Equal(warningsBefore, Warnings(h).Count); // original not re-fired either
        }

        // ── 12. Old save defaults to unbuilt/empty network ──────────────

        [Fact]
        public void OldSave_DefaultsToUnbuiltNetwork()
        {
            var engine = new AquiferPiezometerEngine(new SeededRng(1));
            engine.BindCatalog(CreateCatalog());
            engine.RestoreState(null); // pre-Plan-115 save: nothing persisted
            Assert.False(engine.IsConstructed);
            Assert.Empty(engine.State.nodes);

            // Ticks before construction are safe no-ops.
            engine.TickDay(1);
            Assert.Equal(0, engine.State.days_monitored);

            // Null fields inside a legacy state are repaired on restore.
            var legacy = new HydrogeologyNetworkState { constructed = false, nodes = null! };
            engine.RestoreState(legacy);
            Assert.False(engine.IsConstructed);
            Assert.NotNull(engine.State.nodes);
        }

        // ── 13. Water system consumes advisory exactly once ─────────────

        [Fact]
        public void WaterSystem_ConsumesAdvisoryOnce()
        {
            var water = new WaterTreatmentSystem();

            bool first = water.RegisterContaminationAdvisory(
                "advisory_aquifer_network_test_100", "warning",
                new List<string> { "source_shallow_well" });
            Assert.True(first);
            Assert.True(water.HasContaminationAdvisory);
            Assert.True(water.IsIntakeSourceBlocked("source_shallow_well"));
            Assert.False(water.IsIntakeSourceBlocked("source_deep_borehole"));

            // Same advisory id → consumed once, never re-applied.
            bool second = water.RegisterContaminationAdvisory(
                "advisory_aquifer_network_test_100", "warning",
                new List<string> { "source_shallow_well" });
            Assert.False(second);
            Assert.True(water.HasContaminationAdvisory);

            // Water stock untouched by advisory registration (intelligence-only).
            Assert.Equal(0f, water.CleanWater);
            Assert.Equal(0f, water.RawWater);
        }

        // ── 14. Data references resolve against the items authority ─────

        [Fact]
        public void CatalogReferences_ResolveAgainstItemsAuthority()
        {
            string path = RepoPath("Assets", "StreamingAssets", "Data", "items.json");
            Assert.True(File.Exists(path), $"items authority not found at {path}");

            var json = File.ReadAllText(path);
            using var doc = JsonDocument.Parse(json);
            var ids = new HashSet<string>(doc.RootElement.GetProperty("items")
                .EnumerateArray()
                .Select(i => i.GetProperty("id").GetString()!), StringComparer.Ordinal);

            var catalog = CreateCatalog();
            var referenced = new List<string> { catalog.sensor_item_id, catalog.isolation_module_item_id, catalog.maintenance_kit_item_id };
            foreach (var s in catalog.strata)
                referenced.AddRange(s.sensor_install_cost.Keys);
            foreach (var p in catalog.maintenance_profiles)
                referenced.AddRange(p.required_items.Keys);

            Assert.All(referenced, id => Assert.True(ids.Contains(id), $"item id not in items.json: {id}"));

            // Authored catalog file also references scrap_metal + bedrock rig.
            string catPath = RepoPath("Assets", "StreamingAssets", "Data", "piezometer_network_catalog.json");
            var catJson = File.ReadAllText(catPath);
            Assert.Contains("item_bedrock_sensor_rig", catJson);
            Assert.Contains("scrap_metal", catJson);
        }

        // ── 15. Content utilization reaches the engine ──────────────────

        [Fact]
        public void ContentUtilization_RegistersPiezometerCatalog()
        {
            // The scanner's consumer mapping must route the new catalog to
            // the engine so --content-utilization-selftest can prove liveness.
            string scannerPath = RepoPath("Assets", "Ashfall.Core", "Content", "ContentUtilizationScanner.cs");
            Assert.True(File.Exists(scannerPath), $"scanner not found at {scannerPath}");
            var source = File.ReadAllText(scannerPath);
            Assert.Contains("\"piezometer_network_catalog.json\"", source);
            Assert.Contains("\"AquiferPiezometerEngine\"", source);
        }

        // ── Extra: idempotent outputs / no duplicate warnings ───────────

        [Fact]
        public void Construction_IsIdempotent_AndConsumesOnce()
        {
            var h = CreateConstructed(seed: 3);
            int sensorsBefore = Inv(h).Get("item_groundwater_sensor");
            var again = h.ConstructNetwork();
            Assert.Equal(ActionResult.StatusKind.Blocked, again.Status);
            Assert.Equal(sensorsBefore, Inv(h).Get("item_groundwater_sensor"));
        }

        [Fact]
        public void SaveLoad_PreservesIsolationAndBoundedHistory()
        {
            var h = CreateConstructed(seed: 41);
            h.IsolateAquiferZone("strata_deep");
            for (int d = 0; d < 30; d++) h.TickDay(100 + d);

            var saved = h.CaptureState();
            Assert.Contains("strata_deep", saved.active_isolation_zone_ids);
            Assert.All(saved.nodes, n => Assert.True(n.head_trend.Count <= 8, "trend window must stay bounded"));

            var json = JsonSerializer.Serialize(saved);
            var restored = JsonSerializer.Deserialize<HydrogeologyNetworkState>(json);
            var fresh = new AquiferPiezometerEngine(new SeededRng(41));
            fresh.BindCatalog(CreateCatalog());
            fresh.RestoreState(restored);

            Assert.Equal(saved.active_isolation_zone_ids, fresh.State.active_isolation_zone_ids);
            Assert.True(fresh.IsSourceIsolated("source_deep_borehole"));
            Assert.Equal(saved.monitoring_confidence, fresh.State.monitoring_confidence, 5);
            Assert.Single(fresh.AvailableSourceIds());
        }

        [Fact]
        public void MaintenanceRequiresMaterials()
        {
            var h = CreateConstructed(seed: 8);
            Inv(h).Items.Remove("item_well_maintenance_kit");
            var result = h.MaintainNode("node_strata_shallow");
            Assert.Equal(ActionResult.StatusKind.Blocked, result.Status);
            Assert.Equal(AquiferPiezometerFailures.MaterialsMissing, result.FailureCode);
        }

        [Fact]
        public void IsolationWithoutModuleOrSpecialist_Blocked()
        {
            var h = CreateConstructed(seed: 9, driller: 0.8f);
            Inv(h).Items.Remove("item_aquifer_isolation_module");
            var noModule = h.IsolateAquiferZone("strata_shallow");
            Assert.Equal(ActionResult.StatusKind.Blocked, noModule.Status);

            var h2 = CreateConstructed(seed: 9, driller: 0.1f);
            var noSpecialist = h2.IsolateAquiferZone("strata_shallow");
            Assert.Equal(ActionResult.StatusKind.Blocked, noSpecialist.Status);
        }
    }
}
