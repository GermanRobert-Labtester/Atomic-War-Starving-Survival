// SPDX-License-Identifier: MIT
// Plan 116 — pharmaceutical tablet engine: catalog load, staging, quality
// model, tooling wear, rejects, specialists, packaging/shelf life, medical
// and trade bridges, save safety, content utilization.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using Ashfall.Core;
using Ashfall.Core.Medical;
using Xunit;

namespace Ashfall.Core.Tests.Medical
{
    public class PharmaceuticalTabletEngineTests
    {
        private sealed class TestInventory
        {
            public Dictionary<string, int> Items = new Dictionary<string, int>(StringComparer.Ordinal);
            public int Get(string id) => Items.TryGetValue(id, out var v) ? v : 0;
            public bool CanAdd(string id, int n) => true;
            public void Add(string id, int n) => Items[id] = Get(id) + n;
            public void Consume(string id, int n)
            {
                var v = Get(id) - n;
                if (v < 0) throw new InvalidOperationException($"negative stock {id}");
                if (v == 0) Items.Remove(id); else Items[id] = v;
            }
        }

        private static string RepoPath(params string[] parts)
        {
            string root = Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "..", "..", "..", ".."));
            return Path.Combine(new[] { root }.Concat(parts).ToArray());
        }

        private static TabletManufacturingCatalog CreateCatalog() => new TabletManufacturingCatalog
        {
            press = new TabletPressDef
            {
                press_id = "press_test",
                construction_required_items = new Dictionary<string, int> { ["scrap_metal"] = 4 },
                maintenance_required_items = new Dictionary<string, int> { ["scrap_metal"] = 1 },
                tooling_replacement_items = new Dictionary<string, int> { ["item_press_tooling_set"] = 1 },
                tooling_wear_per_batch = 2.5f
            },
            formulations = new List<TabletFormulationDef>
            {
                new TabletFormulationDef
                {
                    formulation_id = "form_test_antibiotic",
                    display_name = "Test Antibiotic Doses",
                    medical_effect_item_id = "antibiotics",
                    formulation_class = "antibiotic",
                    precursor_costs = new Dictionary<string, int> { ["item_medical_precursor_base"] = 2 },
                    binder_resource_id = "item_tablet_binder",
                    binder_amount = 1,
                    packaging_resource_id = "item_sealed_packaging_foil",
                    release_class_id = "immediate",
                    base_batch_size = 10,
                    quality_threshold = 0.5f,
                    trade_value_modifier = 1f
                },
                new TabletFormulationDef
                {
                    formulation_id = "form_test_coated",
                    display_name = "Test Coated Tablets",
                    medical_effect_item_id = "iodine_pills",
                    formulation_class = "supplement",
                    precursor_costs = new Dictionary<string, int> { ["item_medical_precursor_base"] = 1 },
                    binder_resource_id = "item_tablet_binder",
                    packaging_resource_id = "item_sealed_packaging_foil",
                    release_class_id = "delayed",
                    base_batch_size = 14,
                    quality_threshold = 0.5f,
                    trade_value_modifier = 0.8f
                }
            },
            release_classes = new List<TabletReleaseClassDef>
            {
                new TabletReleaseClassDef
                {
                    release_class_id = "immediate",
                    shelf_life_class = "standard",
                    coating_quality_floor = 1f
                },
                new TabletReleaseClassDef
                {
                    release_class_id = "delayed",
                    shelf_life_class = "extended",
                    coating_quality_floor = 0.7f,
                    trade_value_bonus = 0.1f,
                    extra_process_days = 1
                }
            }
        };

        private readonly Dictionary<PharmaceuticalTabletEngine, TestInventory> _inventories =
            new Dictionary<PharmaceuticalTabletEngine, TestInventory>();

        private TestInventory Inv(PharmaceuticalTabletEngine e) => _inventories[e];

        private PharmaceuticalTabletEngine CreateEngine(
            TabletManufacturingCatalog? catalog = null,
            int seed = 2209,
            float chemist = 0.8f,
            float technician = 0.8f,
            bool construct = true)
        {
            var inventory = new TestInventory();
            var engine = new PharmaceuticalTabletEngine(new SeededRng(seed));
            engine.BindCatalog(catalog ?? CreateCatalog());
            engine.BindInventory(inventory.Get, inventory.CanAdd, inventory.Add, inventory.Consume);
            engine.DayProvider = () => 300;
            engine.PharmaceuticalChemistSkillProvider = () => chemist;
            engine.FormulationTechnicianSkillProvider = () => technician;
            _inventories[engine] = inventory;

            if (construct)
            {
                inventory.Items["scrap_metal"] = 50;
                inventory.Items["item_medical_precursor_base"] = 100;
                inventory.Items["item_tablet_binder"] = 100;
                inventory.Items["item_sealed_packaging_foil"] = 100;
                inventory.Items["crop_medicinal_herb"] = 100;
                inventory.Items["item_press_tooling_set"] = 20;
                var r = engine.ConstructPress();
                Assert.True(r.Status == ActionResult.StatusKind.Success, $"construct failed: {r.FailureCode}");
            }
            return engine;
        }

        private static void RunFullBatch(PharmaceuticalTabletEngine e, string formulationId, int days = 4)
        {
            var r = e.StageBatch(formulationId);
            Assert.True(r.Status == ActionResult.StatusKind.Success, $"stage failed: {r.FailureCode}");
            for (int d = 0; d < days; d++) e.TickDay(300 + d);
        }

        // ── 1. Catalog loads and item references resolve ────────────────

        [Fact]
        public void Catalog_Loads_AndReferencesResolve()
        {
            string path = RepoPath("Assets", "StreamingAssets", "Data", "tablet_manufacturing_catalog.json");
            Assert.True(File.Exists(path), $"catalog not found at {path}");
            var catalog = JsonSerializer.Deserialize<TabletManufacturingCatalog>(File.ReadAllText(path));
            Assert.NotNull(catalog);
            Assert.Equal(1, catalog!.schema_version);
            Assert.Equal(3, catalog.formulations.Count);
            Assert.Equal(3, catalog.release_classes.Count);

            string itemsPath = RepoPath("Assets", "StreamingAssets", "Data", "items.json");
            using var doc = JsonDocument.Parse(File.ReadAllText(itemsPath));
            var ids = new HashSet<string>(doc.RootElement.GetProperty("items")
                .EnumerateArray().Select(i => i.GetProperty("id").GetString()!), StringComparer.Ordinal);

            Assert.All(catalog.formulations, f =>
            {
                Assert.True(ids.Contains(f.medical_effect_item_id), $"unknown output item {f.medical_effect_item_id}");
                Assert.True(ids.Contains(f.binder_resource_id), $"unknown binder {f.binder_resource_id}");
                Assert.True(ids.Contains(f.packaging_resource_id), $"unknown packaging {f.packaging_resource_id}");
                Assert.All(f.precursor_costs.Keys, p => Assert.True(ids.Contains(p), $"unknown precursor {p}"));
            });
            Assert.All(catalog.formulations, f =>
                Assert.True(string.IsNullOrEmpty(f.controlled_substance_tag),
                    "shipped formulations must be non-controlled (safety policy)"));
        }

        // ── 2. Missing inputs reject batch ──────────────────────────────

        [Fact]
        public void MissingInputs_RejectBatch()
        {
            var e = CreateEngine();
            Inv(e).Items.Remove("item_medical_precursor_base");
            var r = e.StageBatch("form_test_antibiotic");
            Assert.Equal(ActionResult.StatusKind.Blocked, r.Status);
            Assert.Equal(PharmaceuticalTabletFailures.MaterialsMissing, r.FailureCode);
            Assert.Null(e.ActiveBatch);
            Assert.Equal("idle", e.State.machine_state);
        }

        // ── 3. Valid formulation stages correctly ───────────────────────

        [Fact]
        public void ValidFormulation_StagesCorrectly()
        {
            var e = CreateEngine();
            var r = e.StageBatch("form_test_antibiotic");
            Assert.Equal(ActionResult.StatusKind.Success, r.Status);
            Assert.NotNull(e.ActiveBatch);
            Assert.Equal("form_test_antibiotic", e.ActiveBatch!.formulation_id);
            Assert.Equal("staging", e.State.machine_state);
            // Inputs consumed exactly once.
            Assert.Equal(98, Inv(e).Get("item_medical_precursor_base"));
            Assert.Equal(99, Inv(e).Get("item_tablet_binder"));
            Assert.Equal(99, Inv(e).Get("item_sealed_packaging_foil"));
        }

        // ── 4. Calibration affects quality ──────────────────────────────

        [Fact]
        public void Calibration_AffectsQuality()
        {
            var run = (float calibration) =>
            {
                var e = CreateEngine(seed: 41, chemist: 0.5f, technician: 0.5f);
                e.State.calibration_state = calibration;
                RunFullBatch(e, "form_test_antibiotic", 3);
                return e.State.uniformity_quality;
            };
            float low = run(0.3f);
            float high = run(1f);
            Assert.True(high > low, $"calibration should raise uniformity: low={low} high={high}");
        }

        // ── 5. Tooling wear increases deterministically ─────────────────

        [Fact]
        public void ToolingWear_IncreasesDeterministically()
        {
            var e = CreateEngine(seed: 42);
            float before = e.State.tooling_condition;
            RunFullBatch(e, "form_test_antibiotic", 3);
            Assert.Equal(before - 2.5f, e.State.tooling_condition, 3);
            Assert.Equal(before - 2.5f, RunAgain(), 3); // identical seed ⇒ identical wear
        }

        private float RunAgain()
        {
            var e = CreateEngine(seed: 42);
            RunFullBatch(e, "form_test_antibiotic", 3);
            return e.State.tooling_condition;
        }

        // ── 6. Poor tooling increases reject rate ───────────────────────

        [Fact]
        public void PoorTooling_IncreasesRejects()
        {
            int RejectsAt(float tooling)
            {
                var e = CreateEngine(seed: 77, chemist: 0.9f, technician: 0.9f);
                e.State.tooling_condition = tooling;
                RunFullBatch(e, "form_test_antibiotic", 3);
                var claimed = e.ClaimOutputs();
                if (claimed == null) return 10; // full reject
                return 10 - claimed.total_units;
            }

            int freshRejects = RejectsAt(100f);
            int wornRejects = RejectsAt(30f);
            Assert.True(wornRejects > freshRejects, $"worn tooling should reject more: fresh={freshRejects} worn={wornRejects}");
        }

        // ── 7. Specialist improves result within cap ────────────────────

        [Fact]
        public void Specialist_ImprovesResult_WithinCap()
        {
            var poor = CreateEngine(seed: 9, chemist: 0f, technician: 0f);
            var good = CreateEngine(seed: 9, chemist: 1f, technician: 1f);
            RunFullBatch(poor, "form_test_antibiotic", 3);
            RunFullBatch(good, "form_test_antibiotic", 3);

            float poorQ = poor.State.uniformity_quality * 0.5f + poor.State.tablet_integrity_quality * 0.5f;
            float goodQ = good.State.uniformity_quality * 0.5f + good.State.tablet_integrity_quality * 0.5f;
            Assert.True(goodQ > poorQ, $"specialists should raise quality: {poorQ} -> {goodQ}");
            // Composite quality is bounded — no trait reaches beyond 1.0.
            Assert.True(good.State.uniformity_quality <= 1f && good.State.tablet_integrity_quality <= 1f);
        }

        // ── 8. Packaging improves shelf-life class ──────────────────────

        [Fact]
        public void Packaging_ImprovesShelfLifeClass()
        {
            var sealedFoil = CreateEngine(seed: 5);
            RunFullBatch(sealedFoil, "form_test_coated", 4); // delayed release, sealed foil
            var claimed = sealedFoil.ClaimOutputs();
            Assert.NotNull(claimed);

            // Sealed foil (0.95) lifts extended → extended (already top of one step);
            // verify the coating batch reports extended-or-protected, never short.
            var e2 = CreateEngine(seed: 5);
            RunFullBatch(e2, "form_test_coated", 4);
            var result = e2.OutputBuffer.Single();
            Assert.Contains(result.shelf_life_class, new[] { "extended", "protected" });

            // Immediate-release with sealed foil: one step up (standard → extended).
            var e3 = CreateEngine(seed: 5);
            RunFullBatch(e3, "form_test_antibiotic", 3);
            Assert.Equal("extended", e3.OutputBuffer.Single().shelf_life_class);
        }

        // ── 9. Rejected batch creates no valid medicine output ──────────

        [Fact]
        public void RejectedBatch_CreatesNoMedicineOutput()
        {
            var e = CreateEngine(seed: 13, chemist: 0f, technician: 0f);
            e.State.tooling_condition = 0f; // maximal wear → guaranteed rejection
            e.State.calibration_state = 0f;
            var staged = e.StageBatch("form_test_antibiotic");
            Assert.Equal(ActionResult.StatusKind.Success, staged.Status);
            for (int d = 0; d < 3; d++) e.TickDay(300 + d);

            Assert.Equal("rejected", e.State.machine_state);
            Assert.Empty(e.OutputBuffer.Where(b => b.quantity > 0));
            Assert.Equal(0, Inv(e).Get("antibiotics")); // nothing entered inventory
            var claimed = e.ClaimOutputs();
            Assert.True(claimed == null || claimed.total_units == 0);
            Assert.Equal(0, Inv(e).Get("antibiotics"));
        }

        // ── 10. Successful batch creates exact inventory item/quantity ──

        [Fact]
        public void SuccessfulBatch_CreatesExactOutput()
        {
            var e = CreateEngine(seed: 21);
            RunFullBatch(e, "form_test_antibiotic", 3);
            var result = e.OutputBuffer.Single();
            Assert.Equal("antibiotics", result.result_item_id); // canonical medication id
            Assert.True(result.quantity > 0);
            Assert.Equal(10 - result.quantity, result.reject_quantity);

            var claimed = e.ClaimOutputs();
            Assert.NotNull(claimed);
            Assert.Equal(1, claimed!.batches_claimed);
            Assert.Equal(result.quantity, claimed.total_units);
            Assert.Equal(result.quantity, Inv(e).Get("antibiotics"));

            // Claim is idempotent — no duplicate inventory.
            Assert.Null(e.ClaimOutputs());
            Assert.Equal(result.quantity, Inv(e).Get("antibiotics"));
        }

        // ── 11. Medical system consumes output normally ─────────────────

        [Fact]
        public void MedicalAuthority_ConsumesOutput_AsCanonicalItems()
        {
            var e = CreateEngine(seed: 31);
            RunFullBatch(e, "form_test_antibiotic", 3);
            var result = e.OutputBuffer.Single();
            e.ClaimOutputs();

            // The output id must be the exact canonical id the medical
            // treatment pipeline declares as its item cost — the engine
            // never applies treatment effects itself.
            Assert.Equal(MedicalTreatmentCatalog.ItemAntibiotics, result.result_item_id);

            // The engine exposes no treatment API: consuming is the medical
            // authority's job (here: plain inventory removal, same port).
            int before = Inv(e).Get("antibiotics");
            Inv(e).Consume("antibiotics", 1);
            Assert.Equal(before - 1, Inv(e).Get("antibiotics"));
        }

        // ── 12. Trade reads value modifier; engine never touches currency ─

        [Fact]
        public void TradeReads_ValueModifier_EngineHasNoCurrencyPort()
        {
            var e = CreateEngine(seed: 51, chemist: 1f, technician: 1f);
            RunFullBatch(e, "form_test_antibiotic", 3);
            var result = e.OutputBuffer.Single();
            // Quality metadata for trade surfaces: better quality ⇒ higher
            // modifier; no currency field exists anywhere on the result.
            Assert.True(result.trade_value_modifier > 0.9f, $"expected strong modifier, got {result.trade_value_modifier}");
            Assert.True(result.trade_value_modifier <= 2f);

            var poor = CreateEngine(seed: 51, chemist: 0f, technician: 0f);
            RunFullBatch(poor, "form_test_antibiotic", 3);
            var poorResult = poor.OutputBuffer.Single();
            Assert.True(poorResult.trade_value_modifier < result.trade_value_modifier,
                "poorer quality must not command the same trade modifier");
        }

        // ── 13. Save/load resumes exact batch ───────────────────────────

        [Fact]
        public void SaveLoad_ResumesExactBatch()
        {
            var e = CreateEngine(seed: 61);
            e.StageBatch("form_test_coated");
            e.TickDay(300); // blending
            e.TickDay(301); // forming

            var saved = e.CaptureFullState();
            var json = JsonSerializer.Serialize(saved);
            var restored = JsonSerializer.Deserialize<TabletWorksFullState>(json);

            var fresh = CreateEngine(seed: 999, construct: false);
            fresh.RestoreFullState(restored);

            Assert.NotNull(fresh.ActiveBatch);
            Assert.Equal(e.ActiveBatch!.batch_id, fresh.ActiveBatch.batch_id);
            Assert.Equal(e.ActiveBatch.progress_days, fresh.ActiveBatch.progress_days);
            Assert.Equal(e.ActiveBatch.total_process_days, fresh.ActiveBatch.total_process_days);
            Assert.Equal(e.State.tooling_condition, fresh.State.tooling_condition, 4);
            Assert.Equal(e.State.uniformity_quality, fresh.State.uniformity_quality, 4);

            // Both continue identically after reload.
            var origResolved = new List<PharmaceuticalBatchResult>();
            var copyResolved = new List<PharmaceuticalBatchResult>();
            e.OnBatchResolved += (r, ok) => origResolved.Add(r);
            fresh.OnBatchResolved += (r, ok) => copyResolved.Add(r);
            e.TickDay(302);
            fresh.TickDay(302);
            e.TickDay(303);
            fresh.TickDay(303);
            Assert.Equal(origResolved[0].quantity, copyResolved[0].quantity);
            Assert.Equal(origResolved[0].quality_grade, copyResolved[0].quality_grade);
        }

        // ── 14. Completed batch does not duplicate on reload ────────────

        [Fact]
        public void CompletedBatch_DoesNotDuplicateOnReload()
        {
            var e = CreateEngine(seed: 71);
            RunFullBatch(e, "form_test_antibiotic", 3);
            var saved = e.CaptureFullState();
            var json = JsonSerializer.Serialize(saved);
            var restored = JsonSerializer.Deserialize<TabletWorksFullState>(json);

            var fresh = CreateEngine(seed: 71, construct: false);
            fresh.RestoreFullState(restored);

            // Buffer persists; claiming after reload adds exactly once.
            var claimed = fresh.ClaimOutputs();
            Assert.NotNull(claimed);
            int inInventory = Inv(fresh).Get("antibiotics");
            Assert.Equal(claimed!.total_units, inInventory);

            // Reload again post-claim: buffer empty, claim adds nothing.
            var saved2 = fresh.CaptureFullState();
            var fresh2 = CreateEngine(seed: 71, construct: false);
            fresh2.RestoreFullState(JsonSerializer.Deserialize<TabletWorksFullState>(JsonSerializer.Serialize(saved2)));
            Assert.Null(fresh2.ClaimOutputs());
            Assert.Equal(0, Inv(fresh2).Get("antibiotics")); // empty buffer never mints units

            // Continuing ticks after a completed batch never re-resolve it.
            for (int d = 0; d < 5; d++) fresh2.TickDay(400 + d);
            Assert.Equal(0, Inv(fresh2).Get("antibiotics"));
        }

        // ── 15. Content utilization reaches the engine ──────────────────

        [Fact]
        public void ContentUtilization_RegistersTabletCatalog()
        {
            string scannerPath = RepoPath("Assets", "Ashfall.Core", "Content", "ContentUtilizationScanner.cs");
            Assert.True(File.Exists(scannerPath), $"scanner not found at {scannerPath}");
            var source = File.ReadAllText(scannerPath);
            Assert.Contains("\"tablet_manufacturing_catalog.json\"", source);
            Assert.Contains("\"PharmaceuticalTabletEngine\"", source);
        }

        // ── Extra: faults and maintenance ────────────────────────────────

        [Fact]
        public void PressFault_LosesBatch_RequiresMaintenance()
        {
            // Deterministic seed sweep: with a dead machine some seeds fault.
            for (int seed = 1; seed <= 60; seed++)
            {
                var e = CreateEngine(seed: seed, chemist: 0f);
                e.State.machine_condition = 21f; // stageable but fault-prone
                var staged = e.StageBatch("form_test_antibiotic");
                Assert.Equal(ActionResult.StatusKind.Success, staged.Status);
                for (int d = 0; d < 3; d++)
                {
                    e.TickDay(300 + d);
                    if (e.State.machine_state == "faulted") break;
                }
                if (e.State.machine_state != "faulted") continue;

                // Faulted batch is lost with zero output.
                Assert.Null(e.ActiveBatch);
                Assert.Equal("press_jam", e.State.fault_state);
                Assert.Equal(0, Inv(e).Get("antibiotics"));
                // Maintenance restores the machine to idle.
                var m = e.MaintainPress();
                Assert.Equal(ActionResult.StatusKind.Success, m.Status);
                Assert.Equal("idle", e.State.machine_state);
                return;
            }
            Assert.True(false, "no faulted batch found in seed sweep — fault model broken?");
        }

        [Fact]
        public void OldSave_DefaultsToUnbuiltPress()
        {
            var e = CreateEngine(construct: false);
            e.RestoreFullState(null);
            Assert.False(e.IsConstructed);
            Assert.Null(e.ActiveBatch);
            e.TickDay(1); // safe no-op
            Assert.Equal(0, e.State.cycle_count);
        }
    }
}
