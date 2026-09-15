// SPDX-License-Identifier: MIT
// ============================================================================
// HostCli Partial : Flagship Plans 139–141 selftests
// Plan 139        : --plans-139-141-selftest — InSAR repeat-pass classification
//                   from the authored catalog; extrusion energy/cooling gating,
//                   quality + tool wear; run-flat hazard reduction, heat, and
//                   rolling-resistance cost. Deterministic, no RNG required.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.IO;
using Ashfall.Core.World;
using Godot;

namespace AtomicWar.GodotApp
{
    public static partial class HostCli
    {
        public static int RunPlans139To141SelfTest(string dataDirectory)
        {
            int pass = 0, fail = 0;
            var details = new List<string>();

            void Check(string gate, bool ok, string note = "")
            {
                if (ok) { pass++; details.Add($"  PASS {gate}"); }
                else { fail++; details.Add($"  FAIL {gate}{(note.Length > 0 ? " — " + note : "")}"); }
                GD.Print($"{(ok ? "[PASS]" : "[FAIL]")} plans_139_141/{gate}");
            }

            var io = CatalogPath.CreateFileIOForDataDir(dataDirectory);

            // ── Plan 139: InSAR deformation intelligence ──────────────────────
            try
            {
                var catalog = InSarGeodesyCatalogLoader.Load(dataDirectory, io);
                Check("insar_catalog", catalog.All.Count >= 1, $"sensors={catalog.All.Count}");

                var engine = new InSarDeformationEngine(catalog);
                string sensorId = string.Empty;
                foreach (var s in catalog.All) { sensorId = s.id; break; }
                const string sector = "selftest_sector";

                engine.RecordSurveyPass(sensorId, sector, 1, 70, 100, sensorId);
                var onePass = engine.ProcessSector(sector, 0.5);
                Check("insar_single_pass_refused", onePass.FailureCode == "insufficient_survey_passes");

                engine.RecordSurveyPass(sensorId, sector, 21, 45, 100, sensorId);
                var processed = engine.ProcessSector(sector, 0.5);
                var summary = engine.GetSummary(sector);
                Check("insar_repeat_pass_processed", processed.IsSuccess && summary != null);
                Check("insar_subsidence_detected",
                    summary != null && summary.Classification == InSarClassification.SlowSubsidence,
                    summary?.Classification ?? "none");
                Check("insar_no_terrain_mutation", engine.GetTravelRisk("never_surveyed") == InSarClassification.Unsurveyed);
            }
            catch (Exception ex) { Check("insar_exception", false, ex.Message); }

            // ── Plan 140: hydraulic extrusion ─────────────────────────────────
            try
            {
                var catalog = HydraulicExtrusionCatalogLoader.Load(dataDirectory, io);
                Check("extrusion_catalog", catalog.AllProducts.Count >= 1, $"products={catalog.AllProducts.Count}");

                var engine = new HydraulicExtrusionEngine(catalog);
                ExtrusionProductDef? product = null;
                foreach (var p in catalog.AllProducts) { product = p; break; }
                if (product != null) engine.RegisterMachine(product.machine_class);

                var blockedPower = product != null
                    ? engine.StartBatch(product.id, product.machine_class, 1, 60, 0, 100, 1)
                    : default;
                Check("extrusion_power_gate", blockedPower.FailureCode == "power_unavailable");

                var started = product != null
                    ? engine.StartBatch(product.id, product.machine_class, 2, 90, 100, 100, 1)
                    : default;
                Check("extrusion_batch_started", started.IsSuccess);

                var batch = engine.State.Batches.Count > 0 ? engine.State.Batches[0] : null;
                if (batch != null)
                {
                    for (int i = 0; i < HydraulicExtrusionEngine.Phases.Length; i++) engine.AdvanceBatch(batch.BatchId);
                    var completed = engine.CompleteBatch(batch.BatchId, 1.0);
                    Check("extrusion_batch_completed", completed.IsSuccess && batch.Completed);
                    Check("extrusion_grade_emitted", !string.IsNullOrEmpty(batch.QualityClass),
                        batch.QualityClass);
                    Check("extrusion_tooling_wears",
                        engine.FindMachine(product!.machine_class)!.ToolingConditionBp < 100);
                }
                Check("extrusion_reliability_bounded",
                    HydraulicExtrusionEngine.ReliabilityBenefitBp(ExtrusionQuality.Premium)
                        <= HydraulicExtrusionEngine.MaxReliabilityBenefitBp);
            }
            catch (Exception ex) { Check("extrusion_exception", false, ex.Message); }

            // ── Plan 141: run-flat tires ──────────────────────────────────────
            try
            {
                var catalog = RunFlatTireCatalogLoader.Load(dataDirectory, io);
                Check("runflat_catalog", catalog.All.Count >= 1, $"profiles={catalog.All.Count}");

                var engine = new RunFlatTireEngine(catalog);
                RunFlatProfileDef? profile = null;
                foreach (var p in catalog.All) { profile = p; break; }
                string tag = profile != null && profile.compatible_vehicle_tags.Count > 0
                    ? profile.compatible_vehicle_tags[0] : "road_truck";
                const string vehicle = "selftest_vehicle";

                if (profile != null)
                    Check("runflat_install",
                        engine.Install(vehicle, tag, profile.id, true, true, 0.5).IsSuccess);

                var hazard = engine.ApplyHazard(vehicle, "glass", 40, 20);
                Check("runflat_hazard_applied", hazard.IsSuccess);
                Check("runflat_not_immune", engine.FindWheelSet(vehicle)!.IntegrityBp < 100);

                for (int i = 0; i < 5; i++) engine.TickHeat(vehicle, 90, 80, 25);
                int hot = engine.FindWheelSet(vehicle)!.HeatC;
                for (int i = 0; i < 8; i++) engine.TickHeat(vehicle, 0, 0, 20);
                Check("runflat_heat_cools", engine.FindWheelSet(vehicle)!.HeatC < hot);
                Check("runflat_fuel_cost", engine.GetFuelPenaltyPct(vehicle) >= 0);
            }
            catch (Exception ex) { Check("runflat_exception", false, ex.Message); }

            GD.Print("\n[HostCli] plans_139_141 self-test" + (fail == 0 ? " PASS" : " FAIL"));
            foreach (var line in details) GD.Print(line);
            GD.Print($"[HOST_SELFTEST_SUMMARY] test=plans_139_141 status={(fail == 0 ? "PASS" : "FAIL")} passed={pass} failed={fail}");
            return fail == 0 ? 0 : 1;
        }
    }
}
