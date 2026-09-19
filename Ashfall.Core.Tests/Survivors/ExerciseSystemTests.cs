// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class ExerciseSystemTests
    {
        [Fact]
        public void GetOrCreateProfile_InitializesDefaultProfile()
        {
            var system = new ExerciseSystem();

            var profile = system.GetOrCreateProfile("dweller_alpha", initialBase: 35f);

            Assert.NotNull(profile);
            Assert.Equal("dweller_alpha", profile.SurvivorId);
            Assert.Equal(35f, profile.Cardio);
            Assert.Equal(35f, profile.Strength);
            Assert.Equal(35f, profile.Flexibility);
            Assert.Equal(35f, profile.Endurance);
            Assert.Equal(35f, profile.OverallConditioning);
            Assert.Equal(1, system.ProfileCount);
        }

        [Fact]
        public void ExecuteWorkout_ImprovesRelevantAttributes_AndIncursFatigue()
        {
            var system = new ExerciseSystem();
            WorkoutResult? observed = null;
            system.OnWorkoutCompleted += res => observed = res;

            var result = system.ExecuteWorkout("dweller_bravo", WorkoutRoutineType.StrengthTraining, currentDay: 1, intensity: 1.0f);

            Assert.NotNull(result);
            Assert.Equal(observed, result);
            Assert.Equal(WorkoutRoutineType.StrengthTraining, result.RoutineType);
            Assert.Equal(3.0f, result.StrengthGain);
            Assert.Equal(25f, result.FatigueIncurred);

            var profile = system.GetOrCreateProfile("dweller_bravo");
            Assert.Equal(33.0f, profile.Strength); // initial 30 + 3.0
            Assert.Equal(1, profile.WorkoutStreak);
            Assert.Equal(1, profile.TotalWorkoutsCompleted);
        }

        [Fact]
        public void ExecuteWorkout_IncrementsStreak_WhenExercisedOnConsecutiveDays()
        {
            var system = new ExerciseSystem();

            system.ExecuteWorkout("dweller_charlie", WorkoutRoutineType.Calisthenics, currentDay: 1);
            var profile = system.GetOrCreateProfile("dweller_charlie");
            Assert.Equal(1, profile.WorkoutStreak);

            system.ExecuteWorkout("dweller_charlie", WorkoutRoutineType.Calisthenics, currentDay: 2);
            Assert.Equal(2, profile.WorkoutStreak);

            // Skipped day 3, workout on day 4
            system.ExecuteWorkout("dweller_charlie", WorkoutRoutineType.Calisthenics, currentDay: 4);
            Assert.Equal(1, profile.WorkoutStreak);
        }

        [Fact]
        public void TickDay_DeconditionsAttributes_AfterInactivity()
        {
            var system = new ExerciseSystem();
            system.GetOrCreateProfile("dweller_delta", initialBase: 50f);
            system.ExecuteWorkout("dweller_delta", WorkoutRoutineType.CardioDrill, currentDay: 1);

            float deconditionedAmount = 0f;
            system.OnDeconditioned += (p, diff) => deconditionedAmount += diff;

            // Inactive days 2, 3, 4, 5 (inactiveDays > 3 starting day 5)
            system.TickDay(currentDay: 2);
            system.TickDay(currentDay: 3);
            system.TickDay(currentDay: 4);

            var profile = system.GetOrCreateProfile("dweller_delta");
            float condDay4 = profile.OverallConditioning;

            system.TickDay(currentDay: 5); // Inactive 4 days -> deconditioning occurs
            Assert.True(profile.OverallConditioning < condDay4);
            Assert.True(deconditionedAmount > 0f);
        }

        [Fact]
        public void GetFatigueResistanceMultiplier_ScalesWithOverallConditioning()
        {
            var system = new ExerciseSystem();
            system.GetOrCreateProfile("low_fitness", initialBase: 10f);
            system.GetOrCreateProfile("mid_fitness", initialBase: 50f);
            system.GetOrCreateProfile("high_fitness", initialBase: 90f);

            float lowMult = system.GetFatigueResistanceMultiplier("low_fitness");
            float midMult = system.GetFatigueResistanceMultiplier("mid_fitness");
            float highMult = system.GetFatigueResistanceMultiplier("high_fitness");

            // Higher conditioning gives lower fatigue multiplier (better resistance)
            Assert.True(lowMult > midMult);
            Assert.True(midMult > highMult);
            Assert.Equal(1.0f, midMult, 2);
            Assert.True(highMult < 1.0f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ExerciseSystem();
            system1.GetOrCreateProfile("dweller_echo", initialBase: 42f);
            system1.ExecuteWorkout("dweller_echo", WorkoutRoutineType.CombatDrill, currentDay: 3);

            var state = system1.CaptureState();
            Assert.Single(state.Profiles);

            var system2 = new ExerciseSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ProfileCount);
            var restored = system2.GetOrCreateProfile("dweller_echo");
            Assert.Equal(3, restored.LastWorkoutDay);
            Assert.Equal(1, restored.WorkoutStreak);
            Assert.Equal(1, restored.TotalWorkoutsCompleted);
        }

        [Fact]
        public void FitnessModel_UsesConditioningProjectionWithoutReplacingNeedsFacts()
        {
            var thresholds = new FitnessThresholds(
                fatigueImpaired: 60f, fatigueUnfit: 90f,
                healthImpaired: 50f, healthUnfit: 20f,
                hungerImpaired: 70f, hungerUnfit: 95f,
                thirstImpaired: 70f, thirstUnfit: 95f,
                warmthImpaired: 30f, warmthUnfit: 10f,
                daysWithoutSleepImpaired: 2, daysWithoutSleepUnfit: 4,
                doseImpairedMsv: 100f, doseUnfitMsv: 200f);
            var model = new FitnessForDutyModel(thresholds);
            var verdict = model.Evaluate(new FitnessEvaluationFacts
            {
                SurvivorId = "conditioned_dweller",
                Conditioning = 20f,
                Fatigue = 0f,
                Health = 100f,
                Warmth = 100f
            });

            Assert.Equal(FitnessLevel.Unfit, verdict.Level);
            Assert.Contains(FitnessReasonIds.LowConditioning, verdict.DegradedFactors);
            Assert.Contains(NeedKind.Fatigue, verdict.AffectedNeeds);
        }
    }
}
