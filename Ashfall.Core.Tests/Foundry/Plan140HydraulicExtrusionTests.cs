// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Foundry;
using Xunit;

namespace Ashfall.Core.Tests.Plan140Foundry
{
    /// <summary>
    /// Plan 140 Phase 1 — hydraulic extrusion contract: material/machine gating,
    /// energy/cooling consumption, deterministic defects and tool wear, quality
    /// grading, bounded downstream reliability, save/load, deterministic replay.
    /// Every grade remains fallible; no 100% reliability.
    /// </summary>
    public sealed class Plan140HydraulicExtrusionTests
    {
        private static HydraulicExtrusionCatalog CreateCatalog()
        {
            var catalog = new HydraulicExtrusionCatalog
            {
                product_profiles =
                {
                    new ExtrusionProductDef
                    {
                        id = "test_utility_tubing", display_name = "Utility", billet_material_tag = "high_grade_steel",
                        machine_class = "standard_press", energy_cost = 10, cooling_requirement = 4,
                        tool_wear = 6, base_quality = 60, defect_risk_bp = 1000, result_item_id = "tube_utility"
                    },
                    new ExtrusionProductDef
                    {
                        id = "test_premium_tubing", display_name = "Premium", billet_material_tag = "titanium_alloy",
                        machine_class = "precision_press", energy_cost = 30, cooling_requirement = 12,
                        tool_wear = 10, base_quality = 88, defect_risk_bp = 500, result_item_id = "tube_premium"
                    }
                },
                machine_profiles =
                {
                    new ExtrusionMachineDef { id = "standard_press", alignment_quality = 70, max_defect_reduction_bp = 400 },
                    new ExtrusionMachineDef { id = "precision_press", alignment_quality = 92, max_defect_reduction_bp = 1200 }
                },
                billet_materials =
                {
                    new ExtrusionBilletDef { id = "high_grade_steel", quality_bp = 60 },
                    new ExtrusionBilletDef { id = "titanium_alloy", quality_bp = 90 }
                }
            };
            catalog.Index();
            return catalog;
        }

        private static HydraulicExtrusionEngine CreateEngine(ISeededRng? rng = null)
        {
            var engine = new HydraulicExtrusionEngine(CreateCatalog());
            engine.Rng = rng;
            return engine;
        }

        private static void AdvanceToQa(HydraulicExtrusionEngine engine, string batchId)
        {
            for (int i = 0; i < HydraulicExtrusionEngine.Phases.Length + 1; i++)
                engine.AdvanceBatch(batchId);
        }

        private static SeededRngStub RngAlways(int value) => new SeededRngStub(value);

        [Fact]
        public void Unknown_product_is_typed_failure()
        {
            var engine = CreateEngine();
            engine.RegisterMachine("standard_press");
            var result = engine.StartBatch("nope", "standard_press", 1, 60, 100, 100, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("material_incompatible", result.FailureCode);
        }

        [Fact]
        public void Machine_class_mismatch_is_refused()
        {
            var engine = CreateEngine();
            engine.RegisterMachine("standard_press");
            var result = engine.StartBatch("test_premium_tubing", "standard_press", 1, 90, 100, 100, 1);
            Assert.True(result.IsFailure);
            Assert.Equal("machine_unavailable", result.FailureCode);
        }

        [Fact]
        public void Insufficient_power_and_cooling_are_refused()
        {
            var engine = CreateEngine();
            engine.RegisterMachine("standard_press");

            var noPower = engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 5, 100, 1);
            Assert.Equal("power_unavailable", noPower.FailureCode);

            var noCooling = engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 100, 1, 1);
            Assert.Equal("cooling_unavailable", noCooling.FailureCode);
        }

        [Fact]
        public void Batch_advances_through_declared_phases_then_completes()
        {
            var engine = CreateEngine(RngAlways(9999));
            engine.RegisterMachine("standard_press");
            Assert.True(engine.StartBatch("test_utility_tubing", "standard_press", 4, 70, 100, 100, 5).IsSuccess);
            var batch = engine.FindBatch("extrusion_5_1")!;

            Assert.Equal(HydraulicExtrusionEngine.Phases[0], batch.Phase);
            engine.AdvanceBatch(batch.BatchId);
            Assert.Equal(HydraulicExtrusionEngine.Phases[1], batch.Phase);

            // Completing before QA must be refused.
            Assert.Equal("quality_control_failed", engine.CompleteBatch(batch.BatchId, 0.5).FailureCode);

            AdvanceToQa(engine, batch.BatchId);
            Assert.True(engine.CompleteBatch(batch.BatchId, 0.5).IsSuccess);
            Assert.True(batch.Completed);
        }

        [Fact]
        public void High_inputs_produce_premium_grade()
        {
            var engine = CreateEngine(RngAlways(9999));
            engine.RegisterMachine("precision_press");
            Assert.True(engine.StartBatch("test_premium_tubing", "precision_press", 2, 95, 100, 100, 3).IsSuccess);
            var batch = engine.FindBatch("extrusion_3_1")!;
            AdvanceToQa(engine, batch.BatchId);
            Assert.True(engine.CompleteBatch(batch.BatchId, 1.0).IsSuccess);

            Assert.Equal(ExtrusionQuality.Premium, batch.QualityClass);
            Assert.Equal("tubing_quality:premium", engine.QualityTagFor(batch.BatchId));
        }

        [Fact]
        public void A_forced_defect_yields_out_of_spec_quality()
        {
            // Rng stub rolls 0, which is below any positive defect risk.
            var engine = CreateEngine(RngAlways(0));
            engine.RegisterMachine("standard_press");
            Assert.True(engine.StartBatch("test_utility_tubing", "standard_press", 2, 60, 100, 100, 3).IsSuccess);
            var batch = engine.FindBatch("extrusion_3_1")!;
            AdvanceToQa(engine, batch.BatchId);
            Assert.True(engine.CompleteBatch(batch.BatchId, 0.5).IsSuccess);

            Assert.Equal("dimensional_out_of_spec", batch.DefectCode);
        }

        [Fact]
        public void Rejected_grade_grants_no_reliability_and_no_tag_benefit()
        {
            Assert.Equal(0, HydraulicExtrusionEngine.ReliabilityBenefitBp(ExtrusionQuality.Rejected));
            Assert.Equal("tubing_quality:none", CreateEngine().QualityTagFor("missing_batch"));
        }

        [Fact]
        public void Reliability_benefit_is_bounded_and_never_absolute()
        {
            int premium = HydraulicExtrusionEngine.ReliabilityBenefitBp(ExtrusionQuality.Premium);
            Assert.True(premium > 0);
            Assert.True(premium <= HydraulicExtrusionEngine.MaxReliabilityBenefitBp);
            Assert.True(premium < 10000, "No grade may eliminate failure risk entirely.");
            Assert.True(HydraulicExtrusionEngine.ReliabilityBenefitBp(ExtrusionQuality.HighPressure)
                        < HydraulicExtrusionEngine.ReliabilityBenefitBp(ExtrusionQuality.Premium));
        }

        [Fact]
        public void Completion_wears_tooling()
        {
            var engine = CreateEngine(RngAlways(9999));
            engine.RegisterMachine("standard_press");
            var machine = engine.FindMachine("standard_press")!;
            int before = machine.ToolingConditionBp;

            engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 100, 100, 1);
            var batch = engine.FindBatch("extrusion_1_1")!;
            AdvanceToQa(engine, batch.BatchId);
            engine.CompleteBatch(batch.BatchId, 0.5);

            Assert.True(machine.ToolingConditionBp < before);
            Assert.True(machine.ToolingConditionBp >= 0);
        }

        [Fact]
        public void Worn_tooling_reduces_quality()
        {
            int FreshScore() => ScoreWithTooling(100);
            int WornScore() => ScoreWithTooling(10);
            Assert.True(FreshScore() > WornScore());
        }

        private static int ScoreWithTooling(int tooling)
        {
            var engine = CreateEngine(RngAlways(9999));
            engine.RegisterMachine("standard_press");
            engine.FindMachine("standard_press")!.ToolingConditionBp = tooling;
            engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 100, 100, 1);
            var batch = engine.FindBatch("extrusion_1_1")!;
            AdvanceToQa(engine, batch.BatchId);
            engine.CompleteBatch(batch.BatchId, 0.5);
            return batch.FinalQualityScore;
        }

        [Fact]
        public void Busy_machine_refuses_second_batch()
        {
            var engine = CreateEngine();
            engine.RegisterMachine("standard_press");
            Assert.True(engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 100, 100, 1).IsSuccess);
            var second = engine.StartBatch("test_utility_tubing", "standard_press", 1, 60, 100, 100, 1);
            Assert.True(second.IsFailure);
            Assert.Equal("machine_unavailable", second.FailureCode);
        }

        [Fact]
        public void Save_load_round_trip_preserves_batches_and_tooling()
        {
            var engine = CreateEngine(RngAlways(9999));
            engine.RegisterMachine("standard_press");
            engine.StartBatch("test_utility_tubing", "standard_press", 3, 70, 100, 100, 4);
            var batch = engine.FindBatch("extrusion_4_1")!;
            AdvanceToQa(engine, batch.BatchId);
            engine.CompleteBatch(batch.BatchId, 0.6);

            var restored = CreateEngine();
            restored.RestoreState(engine.CaptureState());

            Assert.NotNull(restored.FindBatch(batch.BatchId));
            Assert.Equal(batch.QualityClass, restored.FindBatch(batch.BatchId)!.QualityClass);
            Assert.Equal(engine.FindMachine("standard_press")!.ToolingConditionBp,
                         restored.FindMachine("standard_press")!.ToolingConditionBp);
        }

        [Fact]
        public void Old_save_baseline_has_no_fabricated_batches()
        {
            var restored = CreateEngine();
            restored.RestoreState(new HydraulicExtrusionState());
            Assert.Empty(restored.State.Batches);
            Assert.Empty(restored.State.Machines);
        }

        [Fact]
        public void Completion_is_deterministic_without_rng()
        {
            int RunOnce()
            {
                var engine = CreateEngine();
                engine.RegisterMachine("standard_press");
                engine.StartBatch("test_utility_tubing", "standard_press", 2, 66, 100, 100, 7);
                var batch = engine.FindBatch("extrusion_7_1")!;
                AdvanceToQa(engine, batch.BatchId);
                engine.CompleteBatch(batch.BatchId, 0.5);
                return batch.FinalQualityScore;
            }

            Assert.Equal(RunOnce(), RunOnce());
        }

        internal sealed class SeededRngStub : ISeededRng
        {
            private readonly int _value;
            public SeededRngStub(int value) { _value = value; }
            public int Seed => _value;
            public int Next(int minInclusive, int maxExclusive) => _value;
            public float NextFloat() => _value / 10000f;
            public double NextDouble() => _value / 10000.0;
            public ISeededRng Fork(int day = 0, int actionIndex = 0) => this;
        }
    }
}
