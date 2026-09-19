// SPDX-License-Identifier: MIT
using Ashfall.Core;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan216_202CoordinatorIntegrationTests
    {
        [Fact]
        public void ExerciseCoordinator_RoutesFatigueThroughNeeds_AndPersistsProfile()
        {
            var needs = new NeedsSystem();
            var survivor = new SurvivorNeedsState { Id = "dweller", IsAlive = true, Fatigue = 3f };
            needs.Register(survivor);
            var coordinator = new SurvivorSocialCoordinator(
                new SeededRng(216), needs,
                new SurvivorRelationsSystem(new SeededRng(217)),
                new DutyRosterSystem(), () => 1);

            var result = coordinator.ExecuteWorkout(
                "dweller", WorkoutRoutineType.StrengthTraining, currentDay: 1);

            Assert.NotNull(result);
            Assert.Equal(28f, survivor.Fatigue);
            Assert.Equal(1, coordinator.Exercise.GetOrCreateProfile("dweller").TotalWorkoutsCompleted);
            var save = coordinator.CaptureState();
            Assert.Single(save.exercise.Profiles);
        }

        [Fact]
        public void ExerciseCoordinator_RejectsDeadSurvivorWithoutCreatingWorkout()
        {
            var needs = new NeedsSystem();
            needs.Register(new SurvivorNeedsState { Id = "dead", IsAlive = false, IsDead = true });
            var coordinator = new SurvivorSocialCoordinator(
                new SeededRng(216), needs,
                new SurvivorRelationsSystem(new SeededRng(217)),
                new DutyRosterSystem(), () => 1);

            Assert.Null(coordinator.ExecuteWorkout(
                "dead", WorkoutRoutineType.Calisthenics, currentDay: 1));
            Assert.Equal(0, coordinator.Exercise.ProfileCount);
        }
    }
}
