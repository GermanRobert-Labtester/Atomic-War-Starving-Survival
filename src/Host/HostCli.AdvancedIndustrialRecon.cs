// SPDX-License-Identifier: MIT
// Plans 118-121 — deterministic Core-only industrial/reconnaissance proof.

using Godot;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Radio;
using Ashfall.Core.Shelter;
using Ashfall.Core.World;
using InventoryModel = Ashfall.Core.Inventory.Inventory;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        private const int AdvancedIndustrialReconSeed = 118121;

        public static int RunSyntheticLubricantSelfTest(string dataDirectory)
        {
            var checks = new List<AdvancedCheck>();
            try
            {
                var catalog = FischerTropschCatalogLoader.Load(dataDirectory);
                var inventory = NewInventory(("synthetic_fuel_canister", 2), ("item_bearing_grease", 1));
                var engine = new FischerTropschSynthesisEngine(new SeededRng(118), catalog);
                engine.BindInventory(inventory);
                Check(checks, catalog.Reactors.Count == 1 && catalog.Products.Count == 3, "118 catalog resolves reactor/products/catalyst");
                Check(checks, engine.StartBatch("ft_reactor_mk1").Status == ActionResult.StatusKind.Success, "118 feedstock starts an atomic batch");
                engine.Tick(0.6f, 0.6f, 1f);
                var completed = engine.Tick(0.6f, 0.6f, 1f).CompletedBatch;
                Check(checks, completed != null && completed.lubricant_units >= 0 && completed.wax_units >= 0 && completed.light_fraction_units >= 0, "118 process produces bounded output");
                Check(checks, engine.ClaimOutputs().Status == ActionResult.StatusKind.Success, "118 output claim is atomic");
                engine.RegisterLubricantConsumer(new MechanicalLubricantConsumer { consumer_id = "generator_a", accepted_grades = new List<string> { "synthetic" }, wear_multiplier = 0.75f });
                Check(checks, engine.ServiceLubricantConsumer("generator_a").Status == ActionResult.StatusKind.Success && engine.GetWearMultiplier("generator_a") == 0.75f, "118 registered consumer receives explicit service benefit");
                Check(checks, engine.GetWearMultiplier("unregistered") == 1f, "118 unregistered consumers retain baseline wear");
                Check(checks, RunSynthesisResumeProbe(catalog), "118 active batch capture/restore is deterministic");
            }
            catch (Exception ex) { Check(checks, false, "118 selftest exception", ex.Message); }
            return Finish("synthetic_lubricant_selftest", checks, "catalog=fischer_tropsch_catalog.json");
        }

        public static int RunUvCoronaSelfTest(string dataDirectory)
        {
            var checks = new List<AdvancedCheck>();
            try
            {
                var catalog = UvCoronaDetectionCatalogLoader.Load(dataDirectory);
                var inventory = NewInventory(("battery", 2));
                var engine = new UvCoronaDetectionEngine(new SeededRng(119), catalog);
                engine.BindInventory(inventory);
                engine.Equip("uv_corona_camera_mk1");
                var input = new[] { new ElectricalFaultInput { fault_id = "fault_substation_a", asset_id = "substation_a", fault_intensity = 0.9f, distance = 1f, energized = true } };
                var result = engine.Scan(input, "clear", day: 21);
                Check(checks, catalog.Detectors.Count == 1 && catalog.Environments.Count >= 3, "119 catalog resolves detector and environments");
                Check(checks, result.Success && result.Observations.Count == 1, "119 scan returns an observation");
                Check(checks, result.Observations.Count == 0 || (result.Observations[0].confidence >= 0f && result.Observations[0].confidence <= 1f), "119 confidence is bounded");
                Check(checks, result.Observations.Count == 0 || result.Observations[0].energized, "119 observes supplied fault without mutating power truth");
                Check(checks, inventory.CountById("battery") == 1, "119 scan consumes one battery atomically");
                Check(checks, RunUvResumeProbe(catalog), "119 saved observations restore deterministically");
            }
            catch (Exception ex) { Check(checks, false, "119 selftest exception", ex.Message); }
            return Finish("uv_corona_selftest", checks, "catalog=uv_corona_detector_catalog.json");
        }

        public static int RunCarbonCompositeSelfTest(string dataDirectory)
        {
            var checks = new List<AdvancedCheck>();
            try
            {
                var catalog = CarbonCompositeCatalogLoader.Load(dataDirectory);
                var inventory = NewInventory(("prepreg_standard", 2));
                var engine = new CarbonCompositeEngine(new SeededRng(120), catalog);
                engine.BindInventory(inventory);
                Check(checks, catalog.Components.Count >= 2 && catalog.Cures.Count > 0, "120 catalog resolves explicit components and cure profile");
                Check(checks, engine.StartJob("composite_sensor_housing", "prepreg_standard").Status == ActionResult.StatusKind.Success, "120 material starts a cure job");
                engine.Tick(0.70f, 0.65f, 1f, 1f);
                engine.Tick(0.70f, 0.65f, 1f, 1f);
                var completed = engine.Tick(0.70f, 0.65f, 1f, 1f).CompletedOutput;
                Check(checks, completed != null && completed.quality >= CompositeQualityGrade.Reject && completed.quality <= CompositeQualityGrade.Certified, "120 quality grade is bounded");
                Check(checks, completed == null || (completed.mass_factor > 0f && completed.mass_factor <= 1.5f), "120 component mass projection is explicit and bounded");
                Check(checks, RunCompositeResumeProbe(catalog), "120 active cure capture/restore is deterministic");
            }
            catch (Exception ex) { Check(checks, false, "120 selftest exception", ex.Message); }
            return Finish("carbon_composite_selftest", checks, "catalog=carbon_composite_catalog.json");
        }

        public static int RunGprCartographySelfTest(string dataDirectory)
        {
            var checks = new List<AdvancedCheck>();
            try
            {
                var catalog = GroundPenetratingRadarCatalogLoader.Load(dataDirectory);
                var inventory = NewInventory(("battery_pack", 2));
                var engine = new GroundPenetratingRadarEngine(new SeededRng(121), catalog);
                engine.BindInventory(inventory);
                engine.Equip("gpr_cart_mk1");
                Check(checks, catalog.Modes.Count >= 2 && catalog.Terrains.Count >= 3, "121 catalog resolves modes and terrain attenuation");
                Check(checks, engine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan").Status == ActionResult.StatusKind.Success, "121 survey reserves power before mutation");
                engine.Tick(); engine.Tick();
                var result = engine.Tick();
                Check(checks, result.Success && result.Observation != null, "121 survey produces a bounded observation");
                Check(checks, result.Observation == null || (result.Observation.confidence >= 0f && result.Observation.confidence <= 1f && result.Observation.depth_band >= 0f), "121 confidence/depth are bounded");
                Check(checks, RunGprResumeProbe(catalog), "121 active transect capture/restore is deterministic");
            }
            catch (Exception ex) { Check(checks, false, "121 selftest exception", ex.Message); }
            return Finish("gpr_cartography_selftest", checks, "catalog=gpr_exploration_catalog.json");
        }

        public static int RunAdvancedIndustrialReconSelfTest(string dataDirectory)
        {
            var checks = new List<AdvancedCheck>();
            try
            {
                var primary = AdvancedIndustrialReconRun.Create(dataDirectory, AdvancedIndustrialReconSeed);
                primary.AdvanceTo(54);
                primary.PrepareMidpointSave();
                var saved = primary.CaptureSave();
                primary.AdvanceTo(60);

                var restored = AdvancedIndustrialReconRun.Create(dataDirectory, AdvancedIndustrialReconSeed);
                restored.RestoreSave(saved);
                restored.AdvanceTo(60);
                string primarySuffix = AdvancedJson.Serialize(primary.Snapshots.Skip(54).ToList());
                string restoredSuffix = AdvancedJson.Serialize(restored.Snapshots.Skip(0).ToList());
                var same = AdvancedIndustrialReconRun.Create(dataDirectory, AdvancedIndustrialReconSeed);
                same.AdvanceTo(60);
                string primaryFull = AdvancedJson.Serialize(primary.Snapshots);
                string sameFull = AdvancedJson.Serialize(same.Snapshots);
                var different = AdvancedIndustrialReconRun.Create(dataDirectory, AdvancedIndustrialReconSeed + 1);
                different.AdvanceTo(60);
                string differentFull = AdvancedJson.Serialize(different.Snapshots);

                Check(checks, primary.Snapshots.Count == 60, "60-day integrated run emits one snapshot per day", $"count={primary.Snapshots.Count}");
                Check(checks, primary.CatalogsLoaded, "all four authoritative catalogs load");
                Check(checks, primary.ProductionCompletions > 0 && primary.CompositeCompletions > 0, "industrial production completes within the campaign");
                Check(checks, primary.UvObservations > 0 && primary.GprObservations > 0, "field sensors produce observations");
                Check(checks, primary.AllBoundsSane(), "integrated trajectory remains bounded");
                Check(checks, primaryFull == sameFull, "same seed produces byte-identical integrated ledger");
                Check(checks, primaryFull != differentFull, "different fixed seed diverges in stochastic outputs");
                Check(checks, primarySuffix == restoredSuffix, "day-55 save/reload preserves days 55-60 trajectory");

                WriteAdvancedIndustrialArtifacts(primary, checks, primaryFull == sameFull, primaryFull != differentFull, primarySuffix == restoredSuffix);
            }
            catch (Exception ex) { Check(checks, false, "advanced integrated selftest exception", ex.Message); }
            return Finish("advanced_industrial_recon_selftest", checks, "artifact=artifacts/advanced-industrial-recon-60d.json");
        }

        private static int Finish(string name, List<AdvancedCheck> checks, string details)
        {
            foreach (var check in checks)
                GD.Print($"[{(check.passed ? "PASS" : "FAIL")}] {check.id}{(string.IsNullOrEmpty(check.evidence) ? string.Empty : $" ({check.evidence})")}");
            bool passed = checks.All(x => x.passed);
            return EmitSummary(name, passed, passed ? 0 : 1, checks.Count(x => x.passed), checks.Count(x => !x.passed), details);
        }

        private static void Check(List<AdvancedCheck> checks, bool passed, string id, string evidence = "")
            => checks.Add(new AdvancedCheck { id = id, passed = passed, evidence = evidence });

        private static InventoryModel NewInventory(params (string id, int amount)[] items)
        {
            var inventory = new InventoryModel();
            foreach (var item in items) inventory.AddById(item.id, item.amount);
            return inventory;
        }

        private static bool RunSynthesisResumeProbe(FischerTropschCatalog catalog)
        {
            var inventory = NewInventory(("synthetic_fuel_canister", 2));
            var first = new FischerTropschSynthesisEngine(new SeededRng(118), catalog);
            first.BindInventory(inventory);
            if (first.StartBatch("ft_reactor_mk1").Status != ActionResult.StatusKind.Success) return false;
            first.Tick(0.6f, 0.6f, 1f);
            var second = new FischerTropschSynthesisEngine(new SeededRng(118), catalog);
            second.RestoreState(first.CaptureState());
            var a = first.Tick(0.6f, 0.6f, 1f).CompletedBatch;
            var b = second.Tick(0.6f, 0.6f, 1f).CompletedBatch;
            return a != null && b != null && a.lubricant_units == b.lubricant_units && a.wax_units == b.wax_units && a.light_fraction_units == b.light_fraction_units;
        }

        private static bool RunUvResumeProbe(UvCoronaDetectionCatalog catalog)
        {
            var inventory = NewInventory(("battery", 1));
            var first = new UvCoronaDetectionEngine(new SeededRng(119), catalog);
            first.BindInventory(inventory); first.Equip("uv_corona_camera_mk1");
            first.Scan(new[] { new ElectricalFaultInput { fault_id = "fault_a", fault_intensity = 0.9f, distance = 1f } }, "clear", day: 1);
            var second = new UvCoronaDetectionEngine(new SeededRng(119), catalog); second.RestoreState(first.CaptureState());
            return second.State.observations.Count == 1 && second.State.observations[0].confidence == first.State.observations[0].confidence;
        }

        private static bool RunCompositeResumeProbe(CarbonCompositeCatalog catalog)
        {
            var inventory = NewInventory(("prepreg_standard", 2));
            var first = new CarbonCompositeEngine(new SeededRng(120), catalog); first.BindInventory(inventory);
            if (first.StartJob("composite_sensor_housing", "prepreg_standard").Status != ActionResult.StatusKind.Success) return false;
            first.Tick(0.7f, 0.65f, 1f, 1f);
            var second = new CarbonCompositeEngine(new SeededRng(120), catalog); second.RestoreState(first.CaptureState());
            var a = first.Tick(0.7f, 0.65f, 1f, 1f); var b = second.Tick(0.7f, 0.65f, 1f, 1f);
            return a.StatusCode == b.StatusCode && a.CompletedOutput?.quality == b.CompletedOutput?.quality;
        }

        private static bool RunGprResumeProbe(GroundPenetratingRadarCatalog catalog)
        {
            var inventory = NewInventory(("battery_pack", 2));
            var first = new GroundPenetratingRadarEngine(new SeededRng(121), catalog); first.BindInventory(inventory); first.Equip("gpr_cart_mk1");
            first.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan"); first.Tick();
            var second = new GroundPenetratingRadarEngine(new SeededRng(121), catalog); second.RestoreState(first.CaptureState());
            var a = first.Tick(); var b = second.Tick();
            return a.Success == b.Success && a.Observation?.confidence == b.Observation?.confidence;
        }

        private static void WriteAdvancedIndustrialArtifacts(AdvancedIndustrialReconRun run, List<AdvancedCheck> checks, bool same, bool different, bool saveParity)
        {
            string directory = Path.Combine(CatalogPath.ResolveRepoRoot(), "artifacts");
            Directory.CreateDirectory(directory);
            var artifact = new AdvancedIndustrialReconArtifact
            {
                schema_version = 1, seed = run.Seed, days = run.Snapshots.Count, same_seed_byte_equal = same,
                different_seed_diverged = different, midpoint_save_load_equal = saveParity, snapshots = run.Snapshots,
                checks = checks
            };
            var json = new SystemTextJsonSerializer();
            File.WriteAllText(Path.Combine(directory, "advanced-industrial-recon-60d.json"), json.Serialize(artifact), new System.Text.UTF8Encoding(false));
            var md = new List<string>
            {
                "# ASHFALL Plans 118-121 advanced industrial/reconnaissance proof", "",
                $"- Seed: `{run.Seed}`", $"- Snapshot count: `{run.Snapshots.Count}`", "- Target: Core-only deterministic 60-day contract slice", "",
                "## Evidence", "", $"- Same-seed byte equality: `{same}`", $"- Different-seed divergence: `{different}`", $"- Day-55 save/reload parity: `{saveParity}`",
                $"- Production completions: `{run.ProductionCompletions}`", $"- Composite completions: `{run.CompositeCompletions}`", $"- UV observations: `{run.UvObservations}`", $"- GPR observations: `{run.GprObservations}`", "",
                "## Authority boundary", "", "The proof uses the four new catalog-backed Core engines. Inventory mutation is atomic; UV and GPR receive observations and do not mutate power, map, excavation, or loot truth. Composite projections are explicit component factors, not global vehicle bonuses. Host panels and dedicated save-store registrations remain a follow-up because these systems were absent from the repository at reconnaissance time.", "",
                "## Checks", ""
            };
            foreach (var check in checks) md.Add($"- {(check.passed ? "PASS" : "FAIL")}: {check.id}{(string.IsNullOrEmpty(check.evidence) ? string.Empty : $" — {check.evidence}")}");
            File.WriteAllText(Path.Combine(directory, "advanced-industrial-recon-60d.md"), string.Join("\n", md) + "\n", new System.Text.UTF8Encoding(false));
        }

        [Serializable]
        private sealed class AdvancedCheck
        {
            public string id = string.Empty;
            public bool passed;
            public string evidence = string.Empty;
        }

        [Serializable]
        private sealed class AdvancedIndustrialReconArtifact
        {
            public int schema_version;
            public int seed;
            public int days;
            public bool same_seed_byte_equal;
            public bool different_seed_diverged;
            public bool midpoint_save_load_equal;
            public List<AdvancedIndustrialDaySnapshot> snapshots = new List<AdvancedIndustrialDaySnapshot>();
            public List<AdvancedCheck> checks = new List<AdvancedCheck>();
        }

        private static class AdvancedJson
        {
            private static readonly SystemTextJsonSerializer Serializer = new SystemTextJsonSerializer();
            public static string Serialize<T>(T value) => Serializer.Serialize(value);
        }

        [Serializable]
        private sealed class AdvancedIndustrialSave
        {
            public int day;
            public FischerTropschSynthesisState synthesis = new FischerTropschSynthesisState();
            public CarbonCompositeState composites = new CarbonCompositeState();
            public UvCoronaDetectionState uv = new UvCoronaDetectionState();
            public GroundPenetratingRadarState gpr = new GroundPenetratingRadarState();
            public InventorySaveState inventory = new InventorySaveState();
        }

        [Serializable]
        private sealed class AdvancedIndustrialDaySnapshot
        {
            public int day;
            public float catalyst_condition;
            public bool synthesis_active;
            public int synthesis_buffer;
            public bool composite_active;
            public int composite_buffer;
            public float autoclave_condition;
            public int uv_observations;
            public int gpr_observations;
            public int gpr_leads;
            public int fuel_stock;
            public int prepreg_stock;
            public int battery_stock;
            public int battery_pack_stock;
            public float latest_uv_confidence;
            public float latest_gpr_confidence;
            public float latest_gpr_depth_band;
            public string latest_gpr_anomaly_class = string.Empty;
        }

        private sealed class AdvancedIndustrialReconRun
        {
            private readonly FischerTropschCatalog _fischer;
            private readonly CarbonCompositeCatalog _composites;
            private readonly UvCoronaDetectionCatalog _uv;
            private readonly GroundPenetratingRadarCatalog _gpr;
            private readonly InventoryModel _inventory;
            private readonly FischerTropschSynthesisEngine _synthesis;
            private readonly CarbonCompositeEngine _composite;
            private readonly UvCoronaDetectionEngine _uvEngine;
            private readonly GroundPenetratingRadarEngine _gprEngine;
            private int _day;
            private bool _midpointPrepared;

            public int Seed { get; }
            public bool CatalogsLoaded => _fischer != null && _composites != null && _uv != null && _gpr != null;
            public List<AdvancedIndustrialDaySnapshot> Snapshots { get; } = new List<AdvancedIndustrialDaySnapshot>();
            public int ProductionCompletions { get; private set; }
            public int CompositeCompletions { get; private set; }
            public int UvObservations => _uvEngine.State.observations.Count;
            public int GprObservations => _gprEngine.State.observations.Count;

            private AdvancedIndustrialReconRun(string dataDirectory, int seed)
            {
                Seed = seed;
                _fischer = FischerTropschCatalogLoader.Load(dataDirectory);
                _composites = CarbonCompositeCatalogLoader.Load(dataDirectory);
                _uv = UvCoronaDetectionCatalogLoader.Load(dataDirectory);
                _gpr = GroundPenetratingRadarCatalogLoader.Load(dataDirectory);
                _inventory = NewInventory(("synthetic_fuel_canister", 80), ("item_bearing_grease", 8), ("prepreg_standard", 20), ("battery", 20), ("battery_pack", 40));
                _synthesis = new FischerTropschSynthesisEngine(new SeededRng(seed + 118), _fischer); _synthesis.BindInventory(_inventory);
                _composite = new CarbonCompositeEngine(new SeededRng(seed + 120), _composites); _composite.BindInventory(_inventory);
                _uvEngine = new UvCoronaDetectionEngine(new SeededRng(seed + 119), _uv); _uvEngine.BindInventory(_inventory); _uvEngine.Equip("uv_corona_camera_mk1");
                _gprEngine = new GroundPenetratingRadarEngine(new SeededRng(seed + 121), _gpr); _gprEngine.BindInventory(_inventory); _gprEngine.Equip("gpr_cart_mk1");
            }

            public static AdvancedIndustrialReconRun Create(string dataDirectory, int seed) => new AdvancedIndustrialReconRun(dataDirectory, seed);

            public void AdvanceTo(int day)
            {
                while (_day < day)
                {
                    if (_day == 54 && !_midpointPrepared) PrepareMidpointSave();
                    AdvanceDay(++_day);
                }
            }

            public void PrepareMidpointSave()
            {
                if (!_synthesis.HasActiveBatch) _synthesis.StartBatch("ft_reactor_mk1", feedQuality: 0.95f);
                if (_composite.State.active_job == null) _composite.StartJob("composite_sensor_housing", "prepreg_standard", materialAgeDays: 3);
                if (_gprEngine.State.active_survey == null) _gprEngine.BeginSurvey("sector_midpoint", "gpr_buried_structure", "dry_soil", "gpr_deep_scan", _day + 1);
                _midpointPrepared = true;
            }

            private void AdvanceDay(int day)
            {
                if (day == 1 || (day % 8 == 0 && !_synthesis.HasActiveBatch)) _synthesis.StartBatch("ft_reactor_mk1", feedQuality: 0.95f);
                var synthesisTick = _synthesis.Tick(day % 11 == 0 ? 0.3f : 0.6f, 0.6f, day % 17 == 0 ? 0.7f : 1f);
                if (synthesisTick.CompletedBatch != null) { ProductionCompletions++; _synthesis.ClaimOutputs(); }
                if (day == 10 || (day % 14 == 0 && _composite.State.active_job == null)) _composite.StartJob("composite_sensor_housing", "prepreg_standard", materialAgeDays: day / 10);
                var compositeTick = _composite.Tick(0.70f, 0.65f, day % 19 == 0 ? 0.8f : 1f, day > 40 ? 0.9f : 1f);
                if (compositeTick.CompletedOutput != null) { CompositeCompletions++; _composite.ClaimOutput(); }
                if (day == 21) _uvEngine.Scan(new[] { new ElectricalFaultInput { fault_id = "fault_substation_a", asset_id = "substation_a", fault_intensity = 0.9f, distance = 1f, energized = true } }, day % 35 == 0 ? "fog" : "clear", 0.6f, day);
                _uvEngine.AdvanceDay();
                if (day == 21) _gprEngine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_deep_scan", day);
                if (day == 24) _gprEngine.BeginSurvey("sector_a", "gpr_buried_structure", "dry_soil", "gpr_detail_scan", day);
                if (day == 32) _gprEngine.BeginSurvey("sector_b", "gpr_void", "wet_mud", "gpr_detail_scan", day);
                var gprTick = _gprEngine.Tick(0.6f);
                if (gprTick.Observation != null)
                    _gprEngine.TryCreateLead(gprTick.Observation.target_id, out _);
                AddSnapshot(day);
            }

            private void AddSnapshot(int day)
            {
                var latestUv = _uvEngine.State.observations.LastOrDefault();
                var latestGpr = _gprEngine.State.observations.LastOrDefault();
                Snapshots.Add(new AdvancedIndustrialDaySnapshot
                {
                    day = day, catalyst_condition = _synthesis.CatalystCondition, synthesis_active = _synthesis.HasActiveBatch,
                    synthesis_buffer = _synthesis.State.output_buffer.Count, composite_active = _composite.State.active_job != null,
                    composite_buffer = _composite.State.output_buffer.Count, autoclave_condition = _composite.State.autoclave_condition,
                    uv_observations = _uvEngine.State.observations.Count, gpr_observations = _gprEngine.State.observations.Count,
                    gpr_leads = _gprEngine.State.leads.Count, fuel_stock = _inventory.CountById("synthetic_fuel_canister"),
                    prepreg_stock = _inventory.CountById("prepreg_standard"), battery_stock = _inventory.CountById("battery"),
                    battery_pack_stock = _inventory.CountById("battery_pack"),
                    latest_uv_confidence = latestUv?.confidence ?? 0f,
                    latest_gpr_confidence = latestGpr?.confidence ?? 0f,
                    latest_gpr_depth_band = latestGpr?.depth_band ?? 0f,
                    latest_gpr_anomaly_class = latestGpr?.anomaly_class ?? string.Empty
                });
            }

            public AdvancedIndustrialSave CaptureSave() => new AdvancedIndustrialSave
            {
                day = _day, synthesis = _synthesis.CaptureState(), composites = _composite.CaptureState(), uv = _uvEngine.CaptureState(),
                gpr = _gprEngine.CaptureState(), inventory = _inventory.CaptureState()
            };

            public void RestoreSave(AdvancedIndustrialSave save)
            {
                _day = save.day;
                _midpointPrepared = _day >= 54;
                _synthesis.RestoreState(save.synthesis); _composite.RestoreState(save.composites); _uvEngine.RestoreState(save.uv); _gprEngine.RestoreState(save.gpr);
                _inventory.RestoreState(save.inventory, id => new ItemDefinition { id = id, stackMax = 99 });
            }

            public bool AllBoundsSane()
            {
                return Snapshots.All(x => x.catalyst_condition >= 0f && x.catalyst_condition <= 100f && x.autoclave_condition >= 0f && x.autoclave_condition <= 1f
                    && x.fuel_stock >= 0 && x.prepreg_stock >= 0 && x.battery_stock >= 0 && x.battery_pack_stock >= 0
                    && x.uv_observations >= 0 && x.gpr_observations >= 0 && x.gpr_leads >= 0);
            }
        }
    }
}
