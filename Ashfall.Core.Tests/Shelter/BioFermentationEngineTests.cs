// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 126 — Biological Fermentation engine tests.
// Determinism convention: fixed RNG draw order (one contamination roll per
// active tick, one completion-quality roll on the completing tick). All
// stochastic outcomes stem from the injected ISeededRng; harvest performs
// zero draws.
// ============================================================================
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Inventory;
using Ashfall.Core.Shelter;
using Xunit;
using System.IO;

namespace Ashfall.Core.Tests.Shelter
{
    public sealed class BioFermentationEngineTests
    {
        private const string SugarFeed = "item_fermentation_sugar_feedstock";
        private const string StarchFeed = "item_fermentation_starch_feedstock";
        private const string Starter = "item_fermentation_culture_starter";
        private const string Kit = "item_fermentation_service_kit";
        private const string FilterModule = "item_fermentation_filter_module";
        private const string Concentrate = "item_fermentation_preservation_concentrate";
        private const string Reagent = "item_fermentation_cleaning_reagent";
        private const string Carboy = "item_fermented_organic_acid_carboy";
        private const string Pomace = "item_fermentation_waste_pomace";

        // ── Harness ─────────────────────────────────────────────────────────

        private static BioFermentationCatalog TestCatalog(
            float sensitivity = 0f,
            bool aerationRequired = false,
            string temperatureBand = "moderate",
            int durationDays = 4,
            int baseYield = 3,
            float filterWear = 20f,
            string processId = "bio_ferm_test_batch",
            string outputItemId = "item_fermentation_test_output")
        {
            return new BioFermentationCatalog
            {
                schema_version = 1,
                reactor = new BioFermentationReactorConfig
                {
                    build_cost_item_ids = new List<string> { "scrap_metal", "metal_pipe", FilterModule },
                    build_cost_amounts = new List<int> { 4, 2, 1 },
                    sanitize_cost_item_ids = new List<string> { "clean_water" },
                    sanitize_cost_amounts = new List<int> { 2 },
                    service_cost_item_ids = new List<string> { Kit },
                    service_cost_amounts = new List<int> { 1 },
                    filter_item_id = FilterModule,
                    filter_units_per_replace = 1,
                    service_filter_restore = 100f
                },
                processes = new List<BioFermentationProcessDef>
                {
                    new BioFermentationProcessDef
                    {
                        process_id = processId,
                        display_name = "Test Process",
                        process_family = "preservation",
                        aeration_required = aerationRequired,
                        temperature_band = temperatureBand,
                        acidity_target_band = "moderate",
                        base_duration_days = durationDays,
                        base_yield_units = baseYield,
                        contamination_sensitivity = sensitivity,
                        feedstock_item_ids = new List<string> { SugarFeed, StarchFeed },
                        feedstock_units_required = 2,
                        starter_item_id = Starter,
                        starter_quantity = 1,
                        output_item_id = outputItemId,
                        byproduct_item_ids = new List<string> { Pomace },
                        filter_wear_per_batch = filterWear
                    }
                }
            };
        }

        private static Inventory.Inventory StockedInventory()
        {
            var inv = new Inventory.Inventory { Capacity = 500, MaxWeight = 5000f };
            inv.AddById("scrap_metal", 40);
            inv.AddById("metal_pipe", 20);
            inv.AddById("clean_water", 40);
            inv.AddById(FilterModule, 10);
            inv.AddById(Kit, 10);
            inv.AddById(SugarFeed, 40);
            inv.AddById(StarchFeed, 40);
            inv.AddById(Starter, 10);
            inv.AddById("raw_meat", 20);
            return inv;
        }

        private static BioFermentationEngine NewEngine(
            out Inventory.Inventory inv,
            BioFermentationCatalog catalog,
            int seed = 4242,
            Func<float>? roomTemp = null)
        {
            inv = StockedInventory();
            var engine = new BioFermentationEngine(inv, new SeededRng(seed));
            engine.BindCatalog(catalog);
            if (roomTemp != null) engine.RoomTempC = roomTemp;
            return engine;
        }

        private static ActionResult BuildAndSanitize(BioFermentationEngine engine)
        {
            Assert.True(engine.BuildReactor().IsSuccess);
            return engine.SanitizeReactor();
        }

        private void RunDays(BioFermentationEngine engine, int days)
        {
            for (int i = 1; i <= days; i++) engine.TickDay(i);
        }

        // ── 1. Catalog ──────────────────────────────────────────────────────

        [Fact]
        public void BindCatalog_LoadsValidProcesses_AndRejectsBrokenOnes()
        {
            var catalog = TestCatalog();
            catalog.processes.Add(new BioFermentationProcessDef
            {
                process_id = "bio_ferm_broken",
                display_name = "Broken",
                process_family = "industrial",
                feedstock_item_ids = new List<string>(), // no feedstock → invalid
                starter_item_id = Starter,
                base_duration_days = 0,
                base_yield_units = 0,
                contamination_sensitivity = 2.5f, // out of range
                output_item_id = ""
            });

            var engine = NewEngine(out _, catalog);
            Assert.Equal(1, engine.Processes.Count);
            Assert.Contains(engine.CatalogErrors, e => e.StartsWith("bio_ferm_broken:"));
            Assert.Null(engine.GetProcess("bio_ferm_broken"));
            Assert.NotNull(engine.GetProcess("bio_ferm_test_batch"));
        }

        // ── 2/3. Feedstock staging ─────────────────────────────────────────

        [Fact]
        public void ValidFeedstock_Stages_AndStartBatchConsumesExactBill()
        {
            var engine = NewEngine(out var inv, TestCatalog());
            Assert.True(BuildAndSanitize(engine).IsSuccess);

            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StageFeedstock(SugarFeed, 1).IsSuccess); // accumulates

            int beforeSugar = inv.CountById(SugarFeed);
            int beforeStarter = inv.CountById(Starter);

            var start = engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 3);
            Assert.True(start.IsSuccess, start.ToString());

            Assert.Equal(beforeSugar - 3, inv.CountById(SugarFeed));
            Assert.Equal(beforeStarter - 1, inv.CountById(Starter));
            Assert.Equal("Inoculated", engine.State.phase == (int)BioFermentationPhase.Inoculated ? "Inoculated" : "wrong");
            Assert.Equal(3, engine.State.committed_feedstock.Sum(c => c.units));
            Assert.Empty(engine.State.staged_feedstock);
        }

        [Fact]
        public void InvalidFeedstock_IsRejected_WithoutMovingInventory()
        {
            var engine = NewEngine(out var inv, TestCatalog());
            Assert.True(BuildAndSanitize(engine).IsSuccess);

            var blocked = engine.StageFeedstock("item_gold_ingot", 1);
            Assert.True(blocked.IsFailure);
            Assert.Equal("invalid_item", blocked.FailureCode);

            var negative = engine.StageFeedstock(SugarFeed, 0);
            Assert.True(negative.IsFailure);
            Assert.Equal("invalid_units", negative.FailureCode);

            Assert.Empty(engine.State.staged_feedstock);
            Assert.Equal(40, inv.CountById(SugarFeed));
        }

        [Fact]
        public void StartBatch_WithoutEnoughStagedFeedstock_IsBlocked()
        {
            var engine = NewEngine(out var inv, TestCatalog());
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            Assert.True(engine.StageFeedstock(SugarFeed, 1).IsSuccess);

            var blocked = engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1);
            Assert.True(blocked.IsFailure);
            Assert.Equal("needs_feedstock", blocked.FailureCode);

            Assert.Equal(40, inv.CountById(SugarFeed));   // nothing consumed
            Assert.Equal(10, inv.CountById(Starter));      // nothing consumed
        }

        // ── 4/5. Process health drives yield ───────────────────────────────

        [Fact]
        public void HealthyProcess_YieldsMoreThanDegradedProcess()
        {
            var catalog = TestCatalog(aerationRequired: true, temperatureBand: "cool");

            var healthy = NewEngine(out var invH, catalog, seed: 11, roomTemp: () => 14f); // cool band ok
            Assert.True(BuildAndSanitize(healthy).IsSuccess);
            healthy.SetAeration(true);
            Assert.True(healthy.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(healthy.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);

            var degraded = NewEngine(out var invD, catalog, seed: 11, roomTemp: () => 26f); // warm room vs cool band
            Assert.True(BuildAndSanitize(degraded).IsSuccess);
            degraded.SetAeration(false); // aeration off on an aerated process
            Assert.True(degraded.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(degraded.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);

            RunDays(healthy, 4);
            RunDays(degraded, 4);

            Assert.True(healthy.State.process_health > degraded.State.process_health);

            var harvestH = healthy.HarvestBatch();
            var harvestD = degraded.HarvestBatch();
            Assert.True(harvestH.IsSuccess);
            Assert.True(harvestD.IsSuccess);

            int unitsH = invH.CountById("item_fermentation_test_output");
            int unitsD = invD.CountById("item_fermentation_test_output");
            Assert.True(unitsH > unitsD, $"healthy {unitsH} should exceed degraded {unitsD}");
            Assert.True(healthy.State.yield_quality > degraded.State.yield_quality);
        }

        // ── 6. Contamination determinism ───────────────────────────────────

        [Fact]
        public void ContaminationOutcome_IsDeterministic_SameSeedSameOrder()
        {
            var catalog = TestCatalog(sensitivity: 0.4f, aerationRequired: true);

            var a = NewEngine(out _, catalog, seed: 777, roomTemp: () => 16f);
            var b = NewEngine(out _, catalog, seed: 777, roomTemp: () => 16f);

            foreach (var engine in new[] { a, b })
            {
                Assert.True(BuildAndSanitize(engine).IsSuccess);
                engine.SetAeration(true);
                Assert.True(engine.StageFeedstock(StarchFeed, 2).IsSuccess);
                Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
                RunDays(engine, 4);
            }

            Assert.Equal(a.State.contamination_state, b.State.contamination_state);
            Assert.Equal(a.State.culture_health, b.State.culture_health);
            Assert.Equal(a.State.process_health, b.State.process_health);
            Assert.Equal(a.State.yield_quality, b.State.yield_quality);
            Assert.Equal(a.State.phase, b.State.phase);
        }

        // ── 7. Contaminated batches cannot produce premium output ───────────

        [Fact]
        public void ContaminatedBatch_NeverProducesPremiumOutput()
        {
            // Scan a seed where the guaranteed risk roll lands "contaminated"
            // (severity in the mid band) before completion.
            int seed = -1;
            string found = string.Empty;
            for (int s = 1; s < 200 && seed < 0; s++)
            {
                var probe = NewEngine(out _, TestCatalog(sensitivity: 1.0f, aerationRequired: true), seed: s, roomTemp: () => 5f);
                Assert.True(BuildAndSanitize(probe).IsSuccess);
                probe.SetAeration(false);
                Assert.True(probe.StageFeedstock(SugarFeed, 2).IsSuccess);
                Assert.True(probe.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
                RunDays(probe, 4);
                if (probe.State.contamination_state == "contaminated"
                    && probe.State.phase == (int)BioFermentationPhase.Complete)
                {
                    seed = s;
                    found = "contaminated";
                }
            }
            Assert.True(seed > 0, "no eligible contaminated seed found in scan");

            var engine = NewEngine(out var inv, TestCatalog(sensitivity: 1.0f, aerationRequired: true), seed: seed, roomTemp: () => 5f);
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            engine.SetAeration(false);
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(engine, 4);

            Assert.Equal("contaminated", engine.State.contamination_state);
            var harvest = engine.HarvestBatch();
            Assert.True(harvest.IsSuccess);

            int units = inv.CountById("item_fermentation_test_output");
            Assert.True(units <= 3, $"contaminated batch yielded {units}; premium output must be impossible");
            Assert.True(engine.State.yield_quality <= BioFermentationEngine.ContaminatedQualityCap);
        }

        [Fact]
        public void SpoiledBatch_TerminatesRun_AndRequiresServiceToRecover()
        {
            // Find a seed where the guaranteed risk roll lands "spoiled".
            int seed = -1;
            for (int s = 1; s < 120 && seed < 0; s++)
            {
                var probe = NewEngine(out _, TestCatalog(sensitivity: 1.0f, aerationRequired: true), seed: s, roomTemp: () => 5f);
                Assert.True(BuildAndSanitize(probe).IsSuccess);
                probe.SetAeration(false);
                Assert.True(probe.StageFeedstock(SugarFeed, 2).IsSuccess);
                Assert.True(probe.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
                RunDays(probe, 4);
                if (probe.State.phase == (int)BioFermentationPhase.Contaminated) seed = s;
            }
            Assert.True(seed > 0, "no spoiled seed found in scan");

            var engine = NewEngine(out var inv, TestCatalog(sensitivity: 1.0f, aerationRequired: true), seed: seed, roomTemp: () => 5f);
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            engine.SetAeration(false);
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(engine, 4);

            Assert.Equal((int)BioFermentationPhase.Contaminated, engine.State.phase);
            var blockedHarvest = engine.HarvestBatch();
            Assert.True(blockedHarvest.IsFailure);

            int kitsBefore = inv.CountById(Kit);
            var service = engine.ServiceReactor(day: 9);
            Assert.True(service.IsSuccess, service.ToString());
            Assert.Equal(kitsBefore - 1, inv.CountById(Kit));
            Assert.Equal((int)BioFermentationPhase.Idle, engine.State.phase);
            Assert.Equal("clear", engine.State.contamination_state);
        }

        // ── 8. Specialist modifiers apply once ─────────────────────────────

        [Fact]
        public void SpecialistTraits_SnapshotAtStart_MidBatchDelegateChangeIsIgnored()
        {
            var catalog = TestCatalog(sensitivity: 0.4f, aerationRequired: true);

            // Control: trait present for the whole run.
            var control = NewEngine(out _, catalog, seed: 91, roomTemp: () => 16f);
            control.TraitsOf = _ => new[] { BioFermentationEngine.TraitBioprocessEngineer };
            Assert.True(BuildAndSanitize(control).IsSuccess);
            control.SetAeration(true);
            Assert.True(control.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(control.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(control, 4);

            // Subject: trait removed mid-batch — outcome must match control.
            int calls = 0;
            var subject = NewEngine(out _, catalog, seed: 91, roomTemp: () => 16f);
            subject.TraitsOf = _ =>
            {
                calls++;
                if (calls > 1) return Array.Empty<string>(); // delegate changes after StartBatch
                return new[] { BioFermentationEngine.TraitBioprocessEngineer };
            };
            Assert.True(BuildAndSanitize(subject).IsSuccess);
            subject.SetAeration(true);
            Assert.True(subject.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(subject.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(subject, 4);

            Assert.True(calls == 1, "trait delegate must be queried exactly once (at StartBatch)");
            Assert.Equal(control.State.contamination_state, subject.State.contamination_state);
            Assert.Equal(control.State.culture_health, subject.State.culture_health);
            Assert.Equal(control.State.process_health, subject.State.process_health);
            Assert.Equal(control.State.yield_quality, subject.State.yield_quality);
            Assert.Contains(BioFermentationEngine.TraitBioprocessEngineer, subject.State.operator_trait_ids);

            // And the trait genuinely improves outcomes vs no trait.
            var untraited = NewEngine(out _, catalog, seed: 91, roomTemp: () => 16f);
            untraited.TraitsOf = _ => Array.Empty<string>();
            Assert.True(BuildAndSanitize(untraited).IsSuccess);
            untraited.SetAeration(true);
            Assert.True(untraited.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(untraited.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(untraited, 4);
            Assert.True(control.State.yield_quality >= untraited.State.yield_quality);
        }

        // ── 9. Food-preservation bridge (real food_preservation.json) ───────

        [Fact]
        public void PreservationConcentrate_IsConsumedByTheFoodPreservationAuthority()
        {
            string dataDir = CatalogLocator.ResolveDataDirectory();
            var catalog = FoodPreservationCatalogLoader.Load(dataDir, new FileSystemIO());
            var recipe = catalog.GetRecipe("recipe_cure_ferment_concentrate");
            Assert.NotNull(recipe);
            Assert.Equal(Concentrate, recipe.preservative_item_id);

            var inv = new Inventory.Inventory { Capacity = 500, MaxWeight = 5000f };
            inv.AddById("raw_meat", 10);
            inv.AddById(Concentrate, 4);

            var preservation = new FoodPreservationSystem(new SeededRng(7), inv, catalog);
            var start = preservation.StartCuringJob("recipe_cure_ferment_concentrate", "cook_everton", 5);
            Assert.True(start.IsSuccess, start.ToString());

            Assert.Equal(8, inv.CountById("raw_meat"));     // 2 consumed
            Assert.Equal(3, inv.CountById(Concentrate));    // 1 concentrate consumed

            // Run to completion: canonical output enters the pantry cohort.
            preservation.TickDay(6);
            preservation.TickDay(7);
            preservation.TickDay(8);
            Assert.Equal(2, inv.CountById("dried_rations")); // recipe output pair
        }

        // ── 10. Workshop bridge (real workshop_recipes.json) ────────────────

        [Fact]
        public void CleaningReagent_IsConsumedByTheWorkshopAuthority()
        {
            var inv = new Inventory.Inventory { Capacity = 500, MaxWeight = 5000f };
            inv.AddById("scrap_metal", 10);
            inv.AddById(Reagent, 3);

            var workshop = new ShelterWorkshopSystem(inv, new SeededRng(5));
            workshop.LoadCatalog(File.ReadAllText(System.IO.Path.Combine(
                CatalogLocator.ResolveDataDirectory(), "workshop_recipes.json")));

            bool can = workshop.CanStartJob("recipe_workshop_rust_treatment", "room_workshop_heavy", null, null, out string reason);
            Assert.True(can, reason);
            Assert.Empty(reason);
        }

        // ── 11. Soil-cleanup bridge: documented, disabled by design ─────────

        [Fact]
        public void SoilCleanupBridge_IsExplicitlyDisabled_OrganicAcidIsTradeOnly()
        {
            // Load the REAL authored catalog from the data authority.
            string dataDir = CatalogLocator.ResolveDataDirectory();
            string json = File.ReadAllText(Path.Combine(dataDir, "bio_fermentation_catalog.json"));
            var catalog = new SystemTextJsonSerializer().Deserialize<BioFermentationCatalog>(json);
            Assert.NotNull(catalog);

            var acid = catalog!.processes.Find(p => p.process_id == "bio_ferm_organic_acid");
            Assert.NotNull(acid);
            Assert.Equal(Carboy, acid!.output_item_id);
            // Documented decision (docs/shelter/PLANS_126_129_OWNERSHIP_DECISIONS.md §5):
            // the soil-wash bridge is DISABLED — no greenhouse soil-wash mechanic
            // exists, and this engine adds none. The carboy waits as the future
            // recipe input, marked in data so the intent is visible.
            Assert.Contains("soil_wash_input_pending", acid.tags);

            // Boundary proof: the engine's only outward mutation surface is the
            // canonical Inventory (constructor argument) — it cannot touch soil,
            // greenhouse, or food state because it receives none of them.
            var engine = new BioFermentationEngine(new Inventory.Inventory(), new SeededRng(1));
            Assert.True(engine.BindCatalog(catalog) >= 3);
        }

        // ── 12. Save/load resumes the exact batch ───────────────────────────

        [Fact]
        public void SaveLoad_ResumesExactBatch_AndCompletesOnce()
        {
            var catalog = TestCatalog(durationDays: 4);

            var original = NewEngine(out var invA, catalog, seed: 33);
            Assert.True(BuildAndSanitize(original).IsSuccess);
            Assert.True(original.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(original.StartBatch("bio_ferm_test_batch", "op_ferrier", 2).IsSuccess);
            original.TickDay(3);
            original.TickDay(4);

            var snapshot = original.CaptureState();

            var resumed = NewEngine(out var invB, catalog, seed: 33);
            resumed.RestoreState(snapshot);

            // Continuity from the restored point (fresh rng of the same seed).
            resumed.TickDay(5);
            resumed.TickDay(6);

            Assert.Equal(4, resumed.State.phase_days_elapsed);
            Assert.Equal("bio_ferm_test_batch", resumed.State.process_id);
            Assert.Equal("ferm_batch_0_d2", resumed.State.current_feedstock_batch_id);
            Assert.Equal(2, resumed.State.committed_feedstock.Sum(c => c.units));

            var assertHarvestOnce = (BioFermentationEngine engine, Inventory.Inventory inv) =>
            {
                var h1 = engine.HarvestBatch();
                Assert.True(h1.IsSuccess, h1.ToString());
                int afterFirst = inv.CountById("item_fermentation_test_output");

                var h2 = engine.HarvestBatch();
                Assert.True(h2.IsFailure);
                Assert.Equal("not_complete", h2.FailureCode);
                Assert.Equal(afterFirst, inv.CountById("item_fermentation_test_output"));
            };
            assertHarvestOnce(resumed, invB);
        }

        // ── 13. Completed batch does not duplicate (covered above, plus) ────

        [Fact]
        public void CompletedBatch_GrantsOutputExactlyOnce_AcrossReload()
        {
            var catalog = TestCatalog();

            var run = NewEngine(out var inv, catalog, seed: 44);
            Assert.True(BuildAndSanitize(run).IsSuccess);
            Assert.True(run.StageFeedstock(StarchFeed, 2).IsSuccess);
            Assert.True(run.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(run, 4);
            Assert.Equal((int)BioFermentationPhase.Complete, run.State.phase);

            // Reload from save BEFORE harvesting: the completed batch must
            // not re-complete or duplicate its grant.
            var snapshot = run.CaptureState();
            var reloaded = NewEngine(out var inv2, catalog, seed: 44);
            reloaded.RestoreState(snapshot);
            RunDays(reloaded, 4); // extra ticks on a completed batch are inert

            Assert.Equal((int)BioFermentationPhase.Complete, reloaded.State.phase);
            Assert.Equal(0, inv2.CountById("item_fermentation_test_output"));

            var h1 = reloaded.HarvestBatch();
            Assert.True(h1.IsSuccess);
            int granted = inv2.CountById("item_fermentation_test_output");

            var h2 = reloaded.HarvestBatch();
            Assert.True(h2.IsFailure);
            Assert.Equal(granted, inv2.CountById("item_fermentation_test_output"));
            Assert.Equal(0, inv.CountById("item_fermentation_test_output")); // original timeline untouched
        }

        // ── 14. Old saves default to unbuilt ────────────────────────────────

        [Fact]
        public void OldSave_MissingSection_DefaultsToUnbuiltAndEmpty()
        {
            var engine = NewEngine(out var inv, TestCatalog());
            engine.RestoreState(null); // legacy path: nothing persisted

            Assert.Equal((int)BioFermentationPhase.Unbuilt, engine.State.phase);
            Assert.Empty(engine.State.staged_feedstock);
            Assert.Empty(engine.State.committed_feedstock);
            Assert.Equal(100f, engine.State.filter_condition);
            Assert.Equal(-1, engine.State.last_service_day);

            // A fresh engine refuses to run anything until built.
            var stage = engine.StageFeedstock(SugarFeed, 2);
            Assert.True(stage.IsFailure);
            Assert.Equal("reactor_unbuilt", stage.FailureCode);
        }

        [Fact]
        public void RestoreState_RejectsCorruptPhaseValue_WithoutInventingWork()
        {
            var engine = NewEngine(out _, TestCatalog());
            var corrupt = new BioFermentationState { phase = 712 };
            engine.RestoreState(corrupt);
            Assert.Equal((int)BioFermentationPhase.Unbuilt, engine.State.phase);
        }

        // ── 15. Mini flagship journey: production → consumption chains ──────

        [Fact]
        public void FlagshipJourney_BiologicalResourceLoop_PreservesAcrossReload()
        {
            var catalog = TestCatalog(sensitivity: 0f, durationDays: 4, baseYield: 4, outputItemId: Concentrate);

            // 1. agriculture/kitchen feedstock staging → fermentation batch.
            var engine = NewEngine(out var inv, catalog, seed: 55);
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_hassim", 1).IsSuccess);

            // 2. process quality resolves; save/load mid-batch; nothing rerolls.
            RunDays(engine, 4);
            var snapshot = engine.CaptureState();

            var reloaded = NewEngine(out var inv2, catalog, seed: 55);
            reloaded.RestoreState(snapshot);
            Assert.Equal((int)BioFermentationPhase.Complete, reloaded.State.phase);
            Assert.Equal(0, inv2.CountById(Concentrate));

            // 3. output enters inventory exactly once.
            var harvest = reloaded.HarvestBatch();
            Assert.True(harvest.IsSuccess);
            int granted = inv2.CountById(Concentrate);
            Assert.True(granted >= 4, "healthy run should grant base+ yield");
            Assert.Equal(0, inv.CountById(Concentrate)); // original timeline never harvested

            // 4. kitchen/preservation authority consumes the concentrate via the
            //    authored curing recipe (real food_preservation.json).
            string dataDir = CatalogLocator.ResolveDataDirectory();
            var foodCatalog = FoodPreservationCatalogLoader.Load(dataDir, new FileSystemIO());
            inv2.AddById("raw_meat", 4);
            var preservation = new FoodPreservationSystem(new SeededRng(7), inv2, foodCatalog);
            int before = inv2.CountById(Concentrate);
            Assert.True(preservation.StartCuringJob("recipe_cure_ferment_concentrate", "cook_belsy", 6).IsSuccess);
            Assert.Equal(before - 1, inv2.CountById(Concentrate));
        }

        // ── Extras: power, filter wear, full-loop determinism ───────────────

        [Fact]
        public void PowerLoss_StallsBatch_NoProgressNoRolls()
        {
            var catalog = TestCatalog(sensitivity: 0.4f, aerationRequired: true);
            var engine = NewEngine(out _, catalog, seed: 12, roomTemp: () => 16f);
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            engine.SetAeration(true);
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);

            engine.IsRoomPowered = () => false;
            engine.TickDay(2);
            Assert.Equal("power_loss", engine.State.fault_state);
            Assert.Equal(0, engine.State.phase_days_elapsed); // stalled: no progress
            Assert.Equal((int)BioFermentationPhase.Inoculated, engine.State.phase);

            engine.IsRoomPowered = () => true;
            engine.TickDay(3);
            Assert.Equal("", engine.State.fault_state);
            Assert.Equal(1, engine.State.phase_days_elapsed);
            Assert.Equal((int)BioFermentationPhase.Fermenting, engine.State.phase);
        }

        [Fact]
        public void FilterWear_ClogsAfterHeavyBatches_ServiceRestores()
        {
            var catalog = TestCatalog(filterWear: 60f, durationDays: 1, baseYield: 2);
            var engine = NewEngine(out var inv, catalog, seed: 21);

            Assert.True(BuildAndSanitize(engine).IsSuccess);

            // Batch 1: 60 wear → filter at 40.
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 1).IsSuccess);
            RunDays(engine, 1);
            Assert.True(engine.HarvestBatch().IsSuccess);
            Assert.Equal(40f, engine.State.filter_condition);

            // Batch 2: another 60 wear → filter hits 0 → MaintenanceRequired.
            Assert.True(engine.SanitizeReactor().IsSuccess);
            Assert.True(engine.StageFeedstock(SugarFeed, 2).IsSuccess);
            Assert.True(engine.StartBatch("bio_ferm_test_batch", "op_ferrier", 2).IsSuccess);
            RunDays(engine, 2);
            Assert.True(engine.HarvestBatch().IsSuccess);
            Assert.Equal((int)BioFermentationPhase.MaintenanceRequired, engine.State.phase);
            Assert.Equal("filter_clogged", engine.State.fault_state);

            // Service restores; a new batch can start again.
            int kits = inv.CountById(Kit);
            int filters = inv.CountById(FilterModule);
            var service = engine.ServiceReactor(day: 12);
            Assert.True(service.IsSuccess, service.ToString());
            Assert.Equal(kits - 1, inv.CountById(Kit));
            Assert.Equal(filters - 1, inv.CountById(FilterModule));
            Assert.Equal(100f, engine.State.filter_condition);
            Assert.Equal((int)BioFermentationPhase.Idle, engine.State.phase);
        }

        [Fact]
        public void ServiceReactor_RestoresFilterFromCloggedFault()
        {
            var engine = NewEngine(out var inv, TestCatalog());
            Assert.True(BuildAndSanitize(engine).IsSuccess);
            engine.RestoreState(new BioFermentationState
            {
                phase = (int)BioFermentationPhase.MaintenanceRequired,
                filter_condition = 0f,
                fault_state = "filter_clogged",
                last_service_day = 2
            });

            int kits = inv.CountById(Kit);
            int filters = inv.CountById(FilterModule);

            var service = engine.ServiceReactor(day: 10);
            Assert.True(service.IsSuccess, service.ToString());
            Assert.Equal(kits - 1, inv.CountById(Kit));
            Assert.Equal(filters - 1, inv.CountById(FilterModule)); // clogged → new filter consumed
            Assert.Equal(100f, engine.State.filter_condition);
            Assert.Equal("", engine.State.fault_state);
            Assert.Equal((int)BioFermentationPhase.Idle, engine.State.phase);
            Assert.Equal(10, engine.State.last_service_day);
        }
    }
}