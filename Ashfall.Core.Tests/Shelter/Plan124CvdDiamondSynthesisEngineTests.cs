// SPDX-License-Identifier: MIT
using System;
using Ashfall.Core;
using Ashfall.Core.Shelter;
using Xunit;

namespace Ashfall.Core.Tests.Plan124CvdDiamond
{
    /// <summary>
    /// Plan 124 Core contract: growth batches, bounded multiplicative model,
    /// seeded defects, grade ladder, certification gate, consumer registry
    /// (no unregistered benefit, no zero-wear), reactor faults, idempotent
    /// output consumption, and the 120-day tool-economy soak.
    /// </summary>
    public sealed class Plan124CvdDiamondSynthesisEngineTests
    {
        private static CvdDiamondCatalog CreateCatalog()
        {
            var catalog = new CvdDiamondCatalog
            {
                growth_grades =
                {
                    new DiamondGrowthGrade { id = "grade_rejected", display_name = "Rejected", rank = 0, wear_factor_bp = 0 },
                    new DiamondGrowthGrade { id = "grade_utility", display_name = "Utility", rank = 1, wear_factor_bp = 7000 },
                    new DiamondGrowthGrade { id = "grade_industrial", display_name = "Industrial", rank = 2, wear_factor_bp = 4000 },
                    new DiamondGrowthGrade { id = "grade_master", display_name = "Master", rank = 3, wear_factor_bp = 2500, requires_certification = true }
                },
                defect_profiles =
                {
                    new DiamondDefectProfile { id = "defect_poor_crystal", display_name = "Poor Crystal", grade_penalty_bp = 2000 },
                    new DiamondDefectProfile { id = "defect_surface_pitting", display_name = "Pitting", grade_penalty_bp = 800 }
                },
                reactor_profiles =
                {
                    new CvdReactorProfile
                    {
                        id = "test_reactor", display_name = "Test Chamber",
                        growth_rate_per_tick_bp = 6000, plasma_stability_baseline_bp = 8000,
                        magnetron_wear_per_tick_bp = 25, chamber_wear_per_tick_bp = 18,
                        power_demand_kw = 12f, cooling_requirement = 6,
                        install_item_ids = { "item_high_vacuum_pump" },
                        repair_item_ids = { "item_vacuum_pump_oil" },
                        maintenance_profile_id = "maint_cvd"
                    }
                },
                feed_profiles =
                {
                    new DiamondFeedProfile { id = "feed_low", display_name = "Low Purity", purity_bp = 6000, feedstock_item_ids = { "item_biofuel_generator_grade" } },
                    new DiamondFeedProfile { id = "feed_high", display_name = "High Purity", purity_bp = 9500, feedstock_item_ids = { "item_biofuel_high_grade" } }
                },
                substrate_profiles =
                {
                    new DiamondSubstrateProfile { id = "substrate_low", display_name = "Glass", quality_bp = 5500, input_item_id = "item_cast_borosilicate_glass_blank" },
                    new DiamondSubstrateProfile { id = "substrate_high", display_name = "Superalloy", quality_bp = 8500, input_item_id = "item_superalloy_turbine_blade_blank" }
                },
                tool_components =
                {
                    new DiamondToolComponent { id = "insert_utility", display_name = "Utility Insert", min_grade_id = "grade_utility", accepted_consumer_tags = { "consumer_excavation" } },
                    new DiamondToolComponent { id = "insert_industrial", display_name = "Industrial Insert", min_grade_id = "grade_industrial", accepted_consumer_tags = { "consumer_excavation", "consumer_lathe" } },
                    new DiamondToolComponent { id = "insert_master", display_name = "Master Insert", min_grade_id = "grade_master", requires_certification = true, accepted_consumer_tags = { "consumer_excavation", "consumer_lathe" } }
                },
                consumer_mappings =
                {
                    new DiamondConsumerMapping { id = "consumer_excavation", display_name = "Excavation Cutter", accepted_component_ids = { "insert_utility", "insert_industrial", "insert_master" } },
                    new DiamondConsumerMapping { id = "consumer_lathe", display_name = "Precision Lathe", accepted_component_ids = { "insert_industrial", "insert_master" } }
                },
                maintenance_profiles =
                {
                    new DiamondMaintenanceProfile { id = "maint_cvd", display_name = "Lab Upkeep", inspection_interval_days = 10, repair_skill_minimum = 50 }
                }
            };
            catalog.Index();
            return catalog;
        }

        internal sealed class SeededRngStub : ISeededRng
        {
            public SeededRngStub(int seed, int alwaysValue = 9999) { Seed = seed; _alwaysValue = alwaysValue; }
            private readonly int _alwaysValue;
            public int Seed { get; }
            public int Next(int minInclusive, int maxExclusive) => Math.Clamp(_alwaysValue, minInclusive, maxExclusive - 1);
            public float NextFloat() => _alwaysValue / 10000f;
            public double NextDouble() => _alwaysValue / 10000.0;
        }

        private static CvdDiamondSynthesisEngine CreateIdleEngine(ISeededRng? rng = null)
        {
            var engine = new CvdDiamondSynthesisEngine(CreateCatalog()) { Rng = rng };
            Assert.True(engine.Install("test_reactor", partsAvailable: true).IsSuccess);
            Assert.Equal(CvdReactorMode.Idle, engine.State.Mode);
            return engine;
        }

        private static ActionResult StartBatch(CvdDiamondSynthesisEngine engine, string batchId,
            string componentId = "insert_industrial", string feed = "feed_high", string substrate = "substrate_high",
            float skill = 80f)
            => engine.StartBatch(batchId, componentId, feed, substrate,
                powerAvailable: true, coolingAvailable: true, feedstockAvailable: true, substrateItemAvailable: true,
                operatorSkillLevel: skill);

        private static void RunToCompletion(CvdDiamondSynthesisEngine engine, int maxTicks = 12)
        {
            for (int i = 0; i < maxTicks && engine.State.ActiveBatch != null; i++)
                engine.AdvanceBatch();
            if (engine.State.ActiveBatch != null)
                throw new Xunit.Sdk.XunitException(
                    $"batch still active after {maxTicks} ticks: progress={engine.State.ActiveBatch.GrowthProgressBp} mode={engine.State.Mode}");
            Assert.Null(engine.State.ActiveBatch);
        }

        [Fact]
        public void Missing_feedstock_is_typed_failure()
        {
            var engine = CreateIdleEngine();
            var r = engine.StartBatch("b1", "insert_industrial", "feed_high", "substrate_high",
                powerAvailable: true, coolingAvailable: true, feedstockAvailable: false, substrateItemAvailable: true,
                operatorSkillLevel: 50f);
            Assert.True(r.IsFailure);
            Assert.Equal(CvdFailureCodes.InputMissing, r.FailureCode);
        }

        [Fact]
        public void Missing_substrate_item_is_typed_failure()
        {
            var engine = CreateIdleEngine();
            var r = engine.StartBatch("b1", "insert_industrial", "feed_high", "substrate_high",
                powerAvailable: true, coolingAvailable: true, feedstockAvailable: true, substrateItemAvailable: false,
                operatorSkillLevel: 50f);
            Assert.True(r.IsFailure);
            Assert.Equal(CvdFailureCodes.InputMissing, r.FailureCode);
        }

        [Fact]
        public void Missing_power_or_cooling_is_typed_failure()
        {
            var engine = CreateIdleEngine();
            var noPower = engine.StartBatch("b1", "insert_industrial", "feed_high", "substrate_high",
                powerAvailable: false, coolingAvailable: true, feedstockAvailable: true, substrateItemAvailable: true,
                operatorSkillLevel: 50f);
            Assert.Equal(CvdFailureCodes.ReactorUnavailable, noPower.FailureCode);

            var noCooling = engine.StartBatch("b1", "insert_industrial", "feed_high", "substrate_high",
                powerAvailable: true, coolingAvailable: false, feedstockAvailable: true, substrateItemAvailable: true,
                operatorSkillLevel: 50f);
            Assert.Equal(CvdFailureCodes.PlasmaUnstable, noCooling.FailureCode);
        }

        [Fact]
        public void Degraded_equipment_blocks_batch_start()
        {
            var engine = CreateIdleEngine();
            engine.State.MagnetronConditionBp = CvdDiamondSynthesisEngineTests_MagnetronWorn;
            var r = StartBatch(engine, "b1");
            Assert.True(r.IsFailure);
            Assert.Equal(CvdFailureCodes.EquipmentDegraded, r.FailureCode);
        }

        private const int CvdDiamondSynthesisEngineTests_MagnetronWorn = 2000;

        [Fact]
        public void Growth_completes_with_progress_and_wear()
        {
            var engine = CreateIdleEngine();
            Assert.True(StartBatch(engine, "b1").IsSuccess);
            int magnetronBefore = engine.State.MagnetronConditionBp;
            int chamberBefore = engine.State.ChamberConditionBp;
            RunToCompletion(engine);

            var batch = engine.GetFinishedBatch("b1");
            Assert.NotNull(batch);
            Assert.True(batch!.GrowthProgressBp >= 10000);
            Assert.True(engine.State.MagnetronConditionBp < magnetronBefore, "magnetron must wear per tick");
            Assert.True(engine.State.ChamberConditionBp < chamberBefore, "chamber must wear per tick");
            Assert.True(engine.State.Mode == CvdReactorMode.Idle || engine.State.Mode == CvdReactorMode.AwaitingCertification,
                $"completed batch must leave the reactor Idle or AwaitingCertification, got {engine.State.Mode}");
        }

        [Fact]
        public void High_purity_inputs_outgrade_low_purity_inputs()
        {
            var engineHigh = CreateIdleEngine(new SeededRngStub(31, 9999)); // no defects
            Assert.True(StartBatch(engineHigh, "bh", feed: "feed_high").IsSuccess);
            RunToCompletion(engineHigh);

            var engineLow = CreateIdleEngine(new SeededRngStub(31, 9999));
            Assert.True(StartBatch(engineLow, "bl", feed: "feed_low").IsSuccess);
            RunToCompletion(engineLow);

            Assert.True(engineHigh.GetFinishedBatch("bh")!.ConformityScoreBp
                > engineLow.GetFinishedBatch("bl")!.ConformityScoreBp,
                "higher feed purity must score higher conformity");
        }

        [Fact]
        public void Defects_lower_the_grade_score()
        {
            // Stub always rolls 0: every tick passes the defect risk check,
            // so a long growth accumulates defects deterministically.
            var defective = CreateIdleEngine(new SeededRngStub(8, 0));
            Assert.True(StartBatch(defective, "bd", feed: "feed_low", substrate: "substrate_low").IsSuccess);
            RunToCompletion(defective);
            var batch = defective.GetFinishedBatch("bd");
            Assert.NotNull(batch);
            // With guaranteed defect rolls the batch must carry defects and
            // cannot reach master threshold cleanliness.
            Assert.True(batch!.DefectIds.Count > 0, "guaranteed defect rolls must record defects");
            Assert.True(batch.ConformityScoreBp < CvdDiamondSynthesisEngine.MasterGradeThresholdBp);
        }

        [Fact]
        public void Same_seed_same_commands_identical_outcome()
        {
            var a = CreateIdleEngine(new SeededRngStub(77, 4400));
            var b = CreateIdleEngine(new SeededRngStub(77, 4400));
            for (int i = 0; i < 3; i++)
            {
                Assert.Equal(StartBatch(a, $"b{i}").IsSuccess, StartBatch(b, "b" + i).IsSuccess);
                for (int t = 0; t < 12; t++)
                {
                    var ra = a.AdvanceBatch();
                    var rb = b.AdvanceBatch();
                    Assert.Equal(ra.IsSuccess, rb.IsSuccess);
                    if (a.State.ActiveBatch == null) break;
                }
                Assert.Equal(a.State.PlasmaStabilityBp, b.State.PlasmaStabilityBp);
                Assert.Equal(a.State.MagnetronConditionBp, b.State.MagnetronConditionBp);
                Assert.Equal(a.GetFinishedBatch("b" + i)?.DefectIds.Count, b.GetFinishedBatch("b" + i)?.DefectIds.Count);
            }
        }

        [Fact]
        public void Master_grade_requires_certification()
        {
            var engine = CreateIdleEngine(new SeededRngStub(3, 9999)); // no defects, best inputs
            Assert.True(StartBatch(engine, "bm", componentId: "insert_master", feed: "feed_high", substrate: "substrate_high", skill: 100f).IsSuccess);
            RunToCompletion(engine);

            var batch = engine.GetFinishedBatch("bm")!;
            Assert.True(batch.ConformityScoreBp >= CvdDiamondSynthesisEngine.MasterGradeThresholdBp,
                $"best-input batch should reach master threshold, scored {batch.ConformityScoreBp}");

            var blocked = engine.ConsumeOutput("bm");
            Assert.NotNull(blocked);
            Assert.True(blocked!.IsFailure);
            Assert.Equal(CvdFailureCodes.CertificationUnavailable, blocked.FailureCode);

            Assert.True(engine.CertifyBatch("bm", metrologyPassed: true).IsSuccess);
            var released = engine.ConsumeOutput("bm");
            Assert.NotNull(released);
            Assert.True(released!.IsSuccess);
            Assert.Equal("grade_master", released.Value!.GradeId);
            Assert.True(released.Value.WearFactorBp > 0 && released.Value.WearFactorBp < 10000);
        }

        [Fact]
        public void Consuming_twice_yields_no_second_reward()
        {
            var engine = CreateIdleEngine(new SeededRngStub(3, 9999));
            Assert.True(StartBatch(engine, "b1", feed: "feed_high", substrate: "substrate_high", skill: 100f).IsSuccess);
            RunToCompletion(engine);
            // Best inputs reach master grade: certify before release.
            if (engine.GetFinishedBatch("b1")!.AchievedGradeId == "grade_master")
                Assert.True(engine.CertifyBatch("b1", metrologyPassed: true).IsSuccess);
            var first = engine.ConsumeOutput("b1");
            Assert.True(first!.IsSuccess);
            var second = engine.ConsumeOutput("b1");
            Assert.True(second!.IsFailure);
            Assert.Equal(CvdFailureCodes.BatchRejected, second.FailureCode);
        }

        [Fact]
        public void Low_conformity_batch_is_rejected_without_output()
        {
            var engine = CreateIdleEngine(new SeededRngStub(11, 0)); // every tick rolls 0: defect fires (deduped per id)
            // Worst inputs + no skill + guaranteed defect roll.
            Assert.True(StartBatch(engine, "br", componentId: "insert_utility", feed: "feed_low", substrate: "substrate_low", skill: 0f).IsSuccess);
            RunToCompletion(engine);
            var batch = engine.GetFinishedBatch("br")!;
            Assert.True(batch.ConformityScoreBp < CvdDiamondSynthesisEngine.UtilityGradeThresholdBp,
                $"worst-input batch should fall below utility, scored {batch.ConformityScoreBp}");
            var consumed = engine.ConsumeOutput("br");
            Assert.True(consumed!.IsFailure);
        }

        [Fact]
        public void Registered_consumer_gets_wear_reduction_never_zero()
        {
            var engine = CreateIdleEngine();
            Assert.True(engine.RegisterConsumer("consumer_excavation").IsSuccess);
            Assert.True(engine.TryGetWearFactor("consumer_excavation", "insert_industrial", "grade_industrial", out int wearBp));
            Assert.True(wearBp > 0, "no zero-wear benefit may exist");
            Assert.True(wearBp < 10000, "industrial grade must extend service life vs baseline 10000");
            // Service-interval multiplier is 10000/wear: target 2x-3x band.
            double intervalMultiplier = 10000.0 / wearBp;
            Assert.True(intervalMultiplier >= 2.0 && intervalMultiplier <= 3.5,
                $"industrial insert interval {intervalMultiplier:0.00}x outside the 2x-3x design band");
        }

        [Fact]
        public void Unregistered_consumer_receives_no_benefit()
        {
            var engine = CreateIdleEngine();
            Assert.False(engine.IsConsumerRegistered("consumer_lathe"));
            Assert.False(engine.TryGetWearFactor("consumer_lathe", "insert_industrial", "grade_industrial", out _),
                "unregistered consumer must not receive the wear factor");
        }

        [Fact]
        public void Lathe_rejects_utility_grade_component()
        {
            var engine = CreateIdleEngine();
            Assert.True(engine.RegisterConsumer("consumer_lathe").IsSuccess);
            // Utility insert is below the lathe's accepted component list.
            Assert.False(engine.TryGetWearFactor("consumer_lathe", "insert_utility", "grade_industrial", out _),
                "precision lathe must not accept utility-only inserts");
        }

        [Fact]
        public void Plasma_collapse_faults_reactor_and_loses_batch()
        {
            // Collapse roll always hits; stability recomputes from the
            // magnetron each tick, so inject accelerated wear mid-batch
            // (wear-out below the start gate happens during long batches).
            var engine = CreateIdleEngine(new SeededRngStub(5, 0));
            Assert.True(StartBatch(engine, "bf").IsSuccess);
            engine.State.MagnetronConditionBp = 1500; // wear-out injection
            var r = engine.AdvanceBatch();
            Assert.True(r.IsFailure);
            Assert.Equal(CvdReactorMode.Faulted, engine.State.Mode);
            Assert.Null(engine.State.ActiveBatch); // batch lost
            Assert.True(engine.State.ChamberConditionBp < 10000, "fault must damage the chamber");
        }

        [Fact]
        public void Maintenance_restores_condition_and_clears_fault()
        {
            var engine = CreateIdleEngine();
            engine.State.ChamberConditionBp = 3000;
            engine.State.MagnetronConditionBp = 3000;
            Assert.True(engine.PerformMaintenance(partsAvailable: true).IsSuccess);
            Assert.True(engine.State.ChamberConditionBp > 3000);
            Assert.True(engine.State.MagnetronConditionBp > 3000);

            engine.State.Mode = CvdReactorMode.Faulted;
            engine.State.FaultCode = CvdFailureCodes.PlasmaUnstable;
            Assert.True(engine.PerformMaintenance(partsAvailable: true).IsSuccess);
            Assert.Equal(CvdReactorMode.Idle, engine.State.Mode);
            Assert.Equal(string.Empty, engine.State.FaultCode);
        }

        [Fact]
        public void Soak_120_day_tool_economy_stays_bounded()
        {
            var engine = CreateIdleEngine(new SeededRngStub(2026, 4300));
            Assert.True(engine.RegisterConsumer("consumer_excavation").IsSuccess);
            int batchesCompleted = 0;
            string? activeBatchId = null;
            var rng = new System.Random(2026); // test-only profile variation
            for (int day = 0; day < 120; day++)
            {
                if (engine.State.ActiveBatch == null)
                {
                    if (activeBatchId != null && engine.GetFinishedBatch(activeBatchId) != null)
                        batchesCompleted++; // finished last tick
                    activeBatchId = null;
                    if (engine.State.MagnetronConditionBp < 3000 || engine.State.ChamberConditionBp < 3000)
                        engine.PerformMaintenance(partsAvailable: true);
                }
                if (engine.State.Mode == CvdReactorMode.Faulted)
                {
                    Assert.True(engine.PerformMaintenance(partsAvailable: true).IsSuccess);
                    activeBatchId = null;
                }
                if (engine.State.Mode == CvdReactorMode.Idle && engine.State.ActiveBatch == null && activeBatchId == null)
                {
                    string batchId = "soak_" + day;
                    if (StartBatch(engine, batchId, feed: day % 3 == 0 ? "feed_low" : "feed_high",
                        substrate: "substrate_high", skill: 50f + (float)(rng!.NextDouble() * 30.0)).IsSuccess)
                        activeBatchId = batchId;
                }
                if (engine.State.ActiveBatch != null)
                    engine.AdvanceBatch();
                Assert.True(engine.State.MagnetronConditionBp >= 0 && engine.State.MagnetronConditionBp <= 10000, "magnetron out of bounds");
                Assert.True(engine.State.ChamberConditionBp >= 0 && engine.State.ChamberConditionBp <= 10000, "chamber out of bounds");
                Assert.True(engine.State.PlasmaStabilityBp >= 0 && engine.State.PlasmaStabilityBp <= 10000, "plasma out of bounds");
            }
            Assert.True(batchesCompleted > 0, "120-day soak must complete batches");
            // Determinism: replay with a fresh engine and identical stub.
            var replay = CreateIdleEngine(new SeededRngStub(2026, 4300));
            Assert.Equal(batchesCompleted, CountReplayBatches(replay));
        }

        private static int CountReplayBatches(CvdDiamondSynthesisEngine engine)
        {
            int completed = 0;
            string? activeBatchId = null;
            var rng = new System.Random(2026);
            for (int day = 0; day < 120; day++)
            {
                if (engine.State.ActiveBatch == null)
                {
                    if (activeBatchId != null && engine.GetFinishedBatch(activeBatchId) != null)
                        completed++;
                    activeBatchId = null;
                    if (engine.State.MagnetronConditionBp < 3000 || engine.State.ChamberConditionBp < 3000)
                        engine.PerformMaintenance(partsAvailable: true);
                }
                if (engine.State.Mode == CvdReactorMode.Faulted)
                {
                    engine.PerformMaintenance(partsAvailable: true);
                    activeBatchId = null;
                }
                if (engine.State.Mode == CvdReactorMode.Idle && engine.State.ActiveBatch == null && activeBatchId == null)
                {
                    string batchId = "soak_" + day;
                    if (engine.StartBatch(batchId, "insert_industrial", day % 3 == 0 ? "feed_low" : "feed_high",
                        "substrate_high", true, true, true, true, 50f + (float)(rng!.NextDouble() * 30.0)).IsSuccess)
                        activeBatchId = batchId;
                }
                if (engine.State.ActiveBatch != null)
                    engine.AdvanceBatch();
            }
            return completed;
        }
    }
}
