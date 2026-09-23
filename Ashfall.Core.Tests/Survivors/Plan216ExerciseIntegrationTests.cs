// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan216ExerciseIntegrationTests
    {
        private const string SampleCatalogJson = @"{
  ""schema_version"": 1,
  ""routines"": [
    {
      ""id"": ""routine_calisthenics_basic"",
      ""name"": ""Shelter Calisthenics"",
      ""routine_type"": ""calisthenics"",
      ""duration_hours"": 1.0,
      ""intensity"": ""light"",
      ""required_equipment"": ""none"",
      ""min_fitness_level"": 0.0,
      ""cardio_gain"": 1.0,
      ""strength_gain"": 1.0,
      ""flexibility_gain"": 1.0,
      ""endurance_gain"": 1.0,
      ""fatigue_cost"": 12.0,
      ""injury_chance_base"": 0.01,
      ""description"": ""Standard daily push-ups, squats, and jumping jacks.""
    },
    {
      ""id"": ""routine_scrap_weightlifting"",
      ""name"": ""Scrap Iron Weightlifting"",
      ""routine_type"": ""strength_training"",
      ""duration_hours"": 1.5,
      ""intensity"": ""heavy"",
      ""required_equipment"": ""weights"",
      ""min_fitness_level"": 25.0,
      ""cardio_gain"": 0.5,
      ""strength_gain"": 3.0,
      ""flexibility_gain"": 0.0,
      ""endurance_gain"": 1.0,
      ""fatigue_cost"": 25.0,
      ""injury_chance_base"": 0.04,
      ""description"": ""Lifting weighted gear assemblies.""
    }
  ]
}";

        [Fact]
        public void LoadCatalog_ParsesRoutinesAndExposesThem()
        {
            var system = new ExerciseSystem();
            system.LoadCatalog(SampleCatalogJson);

            Assert.Equal(2, system.AvailableRoutineCount);
            var routine = system.GetRoutine("routine_scrap_weightlifting");
            Assert.NotNull(routine);
            Assert.Equal("Scrap Iron Weightlifting", routine.name);
            Assert.Equal(WorkoutRoutineType.StrengthTraining, routine.ParseRoutineType());
            Assert.Equal(25.0f, routine.min_fitness_level);
        }

        [Fact]
        public void ExecuteRoutine_AppliesCatalogGainsAndFatigue()
        {
            var system = new ExerciseSystem();
            system.LoadCatalog(SampleCatalogJson);

            // Establish baseline profile above prerequisite (25)
            system.GetOrCreateProfile("survivor_strong", initialBase: 40f);

            var result = system.ExecuteRoutine("survivor_strong", "routine_scrap_weightlifting", currentDay: 2, intensityMultiplier: 1.0f);

            Assert.NotNull(result);
            Assert.Equal(3.0f, result.StrengthGain);
            Assert.Equal(25.0f, result.FatigueIncurred);
            Assert.Equal("survivor_strong", result.SurvivorId);
            Assert.Equal(2, result.Day);

            var profile = system.GetOrCreateProfile("survivor_strong");
            Assert.True(profile.Strength > 40f);
            Assert.Equal(1, profile.WorkoutStreak);
            Assert.Equal(1, profile.TotalWorkoutsCompleted);
        }

        [Fact]
        public void ExecuteRoutine_RejectsSurvivorBelowMinFitnessLevel()
        {
            var system = new ExerciseSystem();
            system.LoadCatalog(SampleCatalogJson);

            // Establish weak profile below prerequisite (25)
            system.GetOrCreateProfile("survivor_frail", initialBase: 15f);

            var result = system.ExecuteRoutine("survivor_frail", "routine_scrap_weightlifting", currentDay: 1);

            Assert.Null(result); // Prerequisite not met
            var profile = system.GetOrCreateProfile("survivor_frail");
            Assert.Equal(0, profile.TotalWorkoutsCompleted);
        }

        [Fact]
        public void StreakProgression_And_Deconditioning_Cycle()
        {
            var system = new ExerciseSystem();
            system.LoadCatalog(SampleCatalogJson);
            system.GetOrCreateProfile("survivor_cadence", initialBase: 50f);

            // Day 1 workout
            system.ExecuteRoutine("survivor_cadence", "routine_calisthenics_basic", currentDay: 1);
            var profile = system.GetOrCreateProfile("survivor_cadence");
            Assert.Equal(1, profile.WorkoutStreak);

            // Day 2 consecutive workout -> streak increments
            system.ExecuteRoutine("survivor_cadence", "routine_calisthenics_basic", currentDay: 2);
            Assert.Equal(2, profile.WorkoutStreak);

            float condDay2 = profile.OverallConditioning;

            // Inactive days 3, 4, 5, 6
            system.TickDay(3);
            system.TickDay(4);
            system.TickDay(5);
            system.TickDay(6); // > 3 inactive days -> deconditioning occurs

            Assert.True(profile.OverallConditioning < condDay2);
            Assert.Equal(0, profile.WorkoutStreak);
        }

        [Fact]
        public void FatigueResistanceMultiplier_CalculatesExpectedBounds()
        {
            var system = new ExerciseSystem();
            system.GetOrCreateProfile("unfit", initialBase: 10f);
            system.GetOrCreateProfile("fit", initialBase: 80f);

            float unfitMultiplier = system.GetFatigueResistanceMultiplier("unfit");
            float fitMultiplier = system.GetFatigueResistanceMultiplier("fit");

            Assert.True(unfitMultiplier > 1.0f, "Low fitness should accumulate more fatigue.");
            Assert.True(fitMultiplier < 1.0f, "High fitness should accumulate less fatigue.");
        }

        [Fact]
        public void CaptureState_And_RestoreState_PreservesProfilesAndConditioning()
        {
            var system = new ExerciseSystem();
            system.GetOrCreateProfile("survivor_alpha", initialBase: 45f);
            system.ExecuteWorkout("survivor_alpha", WorkoutRoutineType.Calisthenics, currentDay: 1);

            var saved = system.CaptureState();
            Assert.Single(saved.Profiles);

            var restoredSystem = new ExerciseSystem();
            restoredSystem.RestoreState(saved);

            Assert.Equal(1, restoredSystem.ProfileCount);
            var profile = restoredSystem.GetOrCreateProfile("survivor_alpha");
            Assert.Equal(1, profile.WorkoutStreak);
            Assert.Equal(1, profile.TotalWorkoutsCompleted);
        }
    }
}
