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
            Assert.Equal(1, rec.crisisDaysRemaining);
            system.TickDay(2);
            Assert.Empty(rec.currentCrisisId);
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
