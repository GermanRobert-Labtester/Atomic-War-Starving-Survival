// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Ashfall.Core.Needs;
using Xunit;

namespace Ashfall.Core.Tests.Needs
{
    public sealed class SurvivorMentalHealthTests
    {
        private static PsychologicalTraumaCatalog CreateSampleCatalog()
        {
            return new PsychologicalTraumaCatalog
            {
                schema_version = 1,
                trauma_types = new List<TraumaTypeDefinition>
                {
                    new TraumaTypeDefinition
                    {
                        id = "trauma_test_shock",
                        display_name = "Test Shock",
                        stress_floor_permille = 250,
                        insomnia_chance_permille = 300,
                        trigger_tags = new List<string> { "combat" },
                        crisis_affinity = "crisis_test_panic"
                    },
                    new TraumaTypeDefinition
                    {
                        id = "trauma_test_guilt",
                        display_name = "Test Guilt",
                        stress_floor_permille = 200,
                        insomnia_chance_permille = 200,
                        trigger_tags = new List<string> { "loss" },
                        crisis_affinity = "crisis_test_withdrawal"
                    }
                },
                recovery_actions = new List<RecoveryActionDefinition>
                {
                    new RecoveryActionDefinition
                    {
                        id = "therapy_test_quiet",
                        display_name = "Quiet Meditation",
                        required_room = "room_bunks",
                        stress_reduction_permille = 100,
                        daily_resolution_chance_permille = 500,
                        counselor_bonus_permille = 50
                    }
                },
                crisis_events = new List<CrisisEventDefinition>
                {
                    new CrisisEventDefinition
                    {
                        id = "crisis_test_panic",
                        display_name = "Panic Attack",
                        threshold_stress_permille = 800,
                        duration_days = 2,
                        productivity_penalty_permille = 700
                    },
                    new CrisisEventDefinition
                    {
                        id = "crisis_test_withdrawal",
                        display_name = "Withdrawal",
                        threshold_stress_permille = 800,
                        duration_days = 3,
                        productivity_penalty_permille = 600
                    }
                }
            };
        }

        [Fact]
        public void CatalogAndRecordQueries_AreDetachedSnapshots()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            var record = system.GetOrCreateRecord("survivor_snapshot");
            record.stressPermille = 700;
            record.activeTraumaIds.Add("trauma_test_shock");

            system.Traumas["trauma_test_shock"].stress_floor_permille = 1;

            var state = system.CaptureState();
            Assert.Equal(200, state.survivorRecords["survivor_snapshot"].stressPermille);
            Assert.Empty(state.survivorRecords["survivor_snapshot"].activeTraumaIds);

            Assert.True(system.InflictTrauma("survivor_catalog", "trauma_test_shock", out _));
            Assert.Equal(250, system.GetStressFloor("survivor_catalog"));
        }

        [Fact]
        public void LoadCatalog_NullEntriesAndWhitespaceIds_AreFiltered()
        {
            var catalog = new PsychologicalTraumaCatalog
            {
                trauma_types = new List<TraumaTypeDefinition>
                {
                    null!,
                    new TraumaTypeDefinition { id = " TRAUMA_CANONICAL ", stress_floor_permille = 200 }
                },
                recovery_actions = null!,
                crisis_events = new List<CrisisEventDefinition> { null! }
            };
            var system = new SurvivorMentalHealthSystem();

            system.LoadCatalog(catalog);

            Assert.True(system.Traumas.ContainsKey("trauma_canonical"));
            Assert.Empty(system.Therapies);
            Assert.Empty(system.Crises);
        }

        [Fact]
        public void PrescribeTherapy_OverflowingChance_IsClampedAndCanResolve()
        {
            var catalog = CreateSampleCatalog();
            catalog.recovery_actions[0].stress_reduction_permille = int.MaxValue;
            catalog.recovery_actions[0].daily_resolution_chance_permille = int.MaxValue;
            catalog.recovery_actions[0].counselor_bonus_permille = int.MaxValue;
            var system = new SurvivorMentalHealthSystem(catalog, new SeededRng(42));
            system.GetOrCreateRecord("survivor_therapy_overflow");
            Assert.True(system.InflictTrauma("survivor_therapy_overflow", "trauma_test_guilt", out _));

            Assert.True(system.PrescribeTherapy(
                "survivor_therapy_overflow", "therapy_test_quiet", hasCounselor: true, out _));

            Assert.Empty(system.GetOrCreateRecord("survivor_therapy_overflow").activeTraumaIds);
            Assert.Equal(1, system.CaptureState().totalCatharsisBreakthroughs);
        }

        [Fact]
        public void AddStress_IntMax_DoesNotOverflow()
        {
            var system = new SurvivorMentalHealthSystem();
            system.RestoreState(new SurvivorMentalHealthState
            {
                survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>
                {
                    ["survivor_overflow"] = new SurvivorMentalHealthRecord
                    {
                        survivorId = "survivor_overflow",
                        stressPermille = 1000
                    }
                }
            });

            system.AddStress("survivor_overflow", int.MaxValue);

            Assert.Equal(1000, system.GetOrCreateRecord("survivor_overflow").stressPermille);
        }

        [Fact]
        public void TickDay_DuplicateAndBackwardTicks_AreIdempotent()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.RestoreState(new SurvivorMentalHealthState
            {
                survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>
                {
                    ["survivor_tick"] = new SurvivorMentalHealthRecord
                    {
                        survivorId = "survivor_tick",
                        stressPermille = 800,
                        currentCrisisId = "crisis_test_panic",
                        crisisDaysRemaining = 2,
                        activeTraumaIds = new List<string> { "trauma_test_shock" }
                    }
                }
            });

            system.TickDay(10);
            var afterFirst = system.CaptureState();
            system.TickDay(10);
            system.TickDay(9);

            var afterDuplicate = system.CaptureState();
            Assert.Equal(afterFirst.survivorRecords["survivor_tick"].crisisDaysRemaining,
                afterDuplicate.survivorRecords["survivor_tick"].crisisDaysRemaining);
            Assert.Equal(afterFirst.survivorRecords["survivor_tick"].insomniaDaysRemaining,
                afterDuplicate.survivorRecords["survivor_tick"].insomniaDaysRemaining);
            Assert.Equal(10, afterDuplicate.lastTickDay);
            Assert.Throws<ArgumentOutOfRangeException>(() => system.TickDay(-1));
        }

        [Fact]
        public void Restore_MalformedState_IsNormalized()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.RestoreState(new SurvivorMentalHealthState
            {
                systemId = "  ",
                survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>
                {
                    [" SURVIVOR_BAD "] = new SurvivorMentalHealthRecord
                    {
                        survivorId = " survivor_bad ",
                        stressPermille = -50,
                        activeTraumaIds = new List<string> { " TRAUMA_TEST_SHOCK ", "trauma_test_shock", null!, "unknown" },
                        insomniaDaysRemaining = -2,
                        currentCrisisId = " CRISIS_TEST_PANIC ",
                        crisisDaysRemaining = -3,
                        therapySessionCount = -4
                    },
                    ["survivor_bad"] = new SurvivorMentalHealthRecord { survivorId = "survivor_bad" }
                },
                totalCatharsisBreakthroughs = -1
            });

            var state = system.CaptureState();
            Assert.Equal(SurvivorMentalHealthSystem.SystemId, state.systemId);
            Assert.Single(state.survivorRecords);
            var record = state.survivorRecords["survivor_bad"];
            Assert.Equal(0, record.stressPermille);
            Assert.Equal(new[] { "trauma_test_shock", "unknown" }, record.activeTraumaIds);
            Assert.Equal(0, record.insomniaDaysRemaining);
            Assert.Equal("crisis_test_panic", record.currentCrisisId);
            Assert.Equal(0, record.crisisDaysRemaining);
            Assert.Equal(0, record.therapySessionCount);
            Assert.Equal(0, state.totalCatharsisBreakthroughs);
        }

        [Fact]
        public void CountersSaturate_InsteadOfWrapping()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.GetOrCreateRecord("survivor_counter", 100);
            system.InflictTrauma("survivor_counter", "trauma_test_shock", out _);
            system.RestoreState(new SurvivorMentalHealthState
            {
                totalCatharsisBreakthroughs = int.MaxValue,
                survivorRecords = new Dictionary<string, SurvivorMentalHealthRecord>
                {
                    ["survivor_counter"] = new SurvivorMentalHealthRecord
                    {
                        survivorId = "survivor_counter",
                        activeTraumaIds = new List<string> { "trauma_test_shock" }
                    }
                }
            });

            Assert.True(system.ResolveTrauma("survivor_counter", "TRAUMA_TEST_SHOCK", out _));
            Assert.Equal(int.MaxValue, system.CaptureState().totalCatharsisBreakthroughs);
        }

        [Fact]
        public void CaptureRestore_AreDeepCopies()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.GetOrCreateRecord("survivor_copy", 300);
            system.InflictTrauma("survivor_copy", "trauma_test_shock", out _);
            var state = system.CaptureState();
            state.survivorRecords["survivor_copy"].activeTraumaIds.Clear();

            Assert.Single(system.GetOrCreateRecord("survivor_copy").activeTraumaIds);

            var restored = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            restored.RestoreState(state);
            state.survivorRecords["survivor_copy"].stressPermille = 0;
            Assert.NotEqual(0, restored.GetOrCreateRecord("survivor_copy").stressPermille);
        }

        [Fact]
        public void InflictTrauma_RaisesStressFloorAndStress()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.GetOrCreateRecord("survivor_alpha", 200);

            Assert.Equal(0, system.GetStressFloor("survivor_alpha"));

            bool inflicted = system.InflictTrauma("survivor_alpha", "trauma_test_shock", out string reason);
            Assert.True(inflicted, reason);

            Assert.Equal(250, system.GetStressFloor("survivor_alpha"));
            Assert.True(system.GetOrCreateRecord("survivor_alpha").stressPermille >= 450);

            // Attempting to reduce stress below floor is clamped
            system.ReduceStress("survivor_alpha", 500);
            Assert.Equal(250, system.GetOrCreateRecord("survivor_alpha").stressPermille);
        }

        [Fact]
        public void ExtremeStress_TriggersAffiliatedCrisisAndPenalizesProductivity()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));
            system.GetOrCreateRecord("survivor_beta", 400);
            system.InflictTrauma("survivor_beta", "trauma_test_shock", out _);

            // Push stress past 800
            system.AddStress("survivor_beta", 500);

            var rec = system.GetOrCreateRecord("survivor_beta");
            Assert.Equal("crisis_test_panic", rec.currentCrisisId);
            Assert.Equal(2, rec.crisisDaysRemaining);

            int penalty = system.GetProductivityPenaltyPermille("survivor_beta");
            Assert.Equal(700, penalty);

            // Tick 2 days to clear crisis
            system.TickDay(1);
            Assert.Equal(1, system.GetOrCreateRecord("survivor_beta").crisisDaysRemaining);
            system.TickDay(2);
            Assert.Empty(system.GetOrCreateRecord("survivor_beta").currentCrisisId);
            Assert.Equal(0, system.GetProductivityPenaltyPermille("survivor_beta"));
        }

        [Fact]
        public void PrescribeTherapy_CanAchieveCatharsisBreakthrough()
        {
            // Use SeededRng with fixed roll ensuring breakthrough (< 500)
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(42));
            system.GetOrCreateRecord("survivor_gamma", 600);
            system.InflictTrauma("survivor_gamma", "trauma_test_guilt", out _);

            bool ok = system.PrescribeTherapy("survivor_gamma", "therapy_test_quiet", hasCounselor: true, out string outcome);
            Assert.True(ok, outcome);

            // Therapy should have reduced stress and processed trauma
            var rec = system.GetOrCreateRecord("survivor_gamma");
            Assert.Equal(1, rec.therapySessionCount);
        }

        [Fact]
        public void MutatingTherapies_StillRequiresAValidSurvivorId()
        {
            var system = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(101));

            Assert.Throws<ArgumentException>(() =>
                system.InflictTrauma("   ", "trauma_test_shock", out _));
            Assert.Throws<ArgumentException>(() =>
                system.PrescribeTherapy(string.Empty, "therapy_test_quiet", false, out _));
        }

        [Fact]
        public void LegacySaveWithoutDayCursor_RestoresSentinel()
        {
            const string legacy = "{\"systemId\":\"survivor_mental_health\",\"survivorRecords\":{},\"totalCatharsisBreakthroughs\":0}";
            var saved = new SystemTextJsonSerializer().Deserialize<SurvivorMentalHealthState>(legacy);

            Assert.NotNull(saved);
            Assert.Equal(-1, saved!.lastTickDay);
            var restored = new SurvivorMentalHealthSystem(CreateSampleCatalog(), new SeededRng(202));
            restored.RestoreState(saved);
            Assert.Equal(-1, restored.CaptureState().lastTickDay);
        }

        [Fact]
        public void StateRoundtrip_PreservesTraumaAndStress()
        {
            var catalog = CreateSampleCatalog();
            var system = new SurvivorMentalHealthSystem(catalog, new SeededRng(202));
            system.GetOrCreateRecord("survivor_delta", 300);
            system.InflictTrauma("survivor_delta", "trauma_test_shock", out _);
            system.AddStress("survivor_delta", 150);

            var state = system.CaptureState();

            var restored = new SurvivorMentalHealthSystem(catalog, new SeededRng(202));
            restored.RestoreState(state);

            Assert.True(restored.HasRecord("survivor_delta"));
            var rec = restored.GetOrCreateRecord("survivor_delta");
            Assert.Contains("trauma_test_shock", rec.activeTraumaIds);
            Assert.Equal(250, restored.GetStressFloor("survivor_delta"));
            Assert.Equal(system.GetOrCreateRecord("survivor_delta").stressPermille, rec.stressPermille);
        }
    }
}
