// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Inventory;
using Ashfall.Core.Random;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class ChildDevelopmentSystemTests
    {
        [Fact]
        public void RegisterChild_InitializesProfileCorrectly()
        {
            var system = new ChildDevelopmentSystem();
            var profile = system.RegisterChild("child_1", "Leo", birthDay: 10, new[] { "parent_a", "parent_b" }, caregiverId: "parent_a");

            Assert.NotNull(profile);
            Assert.Equal("child_1", profile.ChildId);
            Assert.Equal("Leo", profile.Name);
            Assert.Equal(10, profile.BirthDay);
            Assert.Equal(DevelopmentStage.Infant, profile.Stage);
            Assert.Equal("parent_a", profile.AssignedCaregiverId);
            Assert.Equal(2, profile.ParentIds.Count);
            Assert.Equal(1, system.ChildCount);
        }

        [Theory]
        [InlineData(0, DevelopmentStage.Infant)]
        [InlineData(59, DevelopmentStage.Infant)]
        [InlineData(60, DevelopmentStage.Toddler)]
        [InlineData(179, DevelopmentStage.Toddler)]
        [InlineData(180, DevelopmentStage.Child)]
        [InlineData(499, DevelopmentStage.Child)]
        [InlineData(500, DevelopmentStage.Adolescent)]
        [InlineData(719, DevelopmentStage.Adolescent)]
        [InlineData(720, DevelopmentStage.YoungAdult)]
        [InlineData(1000, DevelopmentStage.YoungAdult)]
        public void ResolveStage_MapsAgeToCorrectStage(int ageDays, DevelopmentStage expected)
        {
            Assert.Equal(expected, ChildDevelopmentSystem.ResolveStage(ageDays));
        }

        [Fact]
        public void TickDay_AdvancesStageAndEmitsEvents()
        {
            var system = new ChildDevelopmentSystem();
            system.RegisterChild("child_2", "Maya", birthDay: 1);

            DevelopmentStage? stageChangedTo = null;
            DevelopmentMilestoneEvent? milestoneReceived = null;
            system.OnStageChanged += (child, stage) => stageChangedTo = stage;
            system.OnMilestoneAchieved += ev => milestoneReceived = ev;

            // Advance to day 65 (age 64 -> Toddler)
            system.TickDay(currentDay: 65);

            var child = system.GetChild("child_2");
            Assert.NotNull(child);
            Assert.Equal(DevelopmentStage.Toddler, child.Stage);
            Assert.Equal(DevelopmentStage.Toddler, stageChangedTo);
            Assert.NotNull(milestoneReceived);
            Assert.Equal("child_2", milestoneReceived.ChildId);
            Assert.Equal(DevelopmentStage.Toddler, milestoneReceived.NewStage);
        }

        [Fact]
        public void TickDay_WithCaregiver_IncreasesEducationAndChoreEfficiency()
        {
            var system = new ChildDevelopmentSystem();
            var child = system.RegisterChild("child_3", "Sam", birthDay: 1, caregiverId: "guardian_1");

            // Advance age to 200 (Child stage)
            system.TickDay(currentDay: 201);
            Assert.Equal(DevelopmentStage.Child, child.Stage);

            float initialEdu = child.EducationScore;
            float initialChore = child.ChoreEfficiency;

            // Tick another day
            system.TickDay(currentDay: 202);

            Assert.True(child.EducationScore > initialEdu);
            Assert.True(child.ChoreEfficiency > initialChore);
        }

        [Fact]
        public void GetChoreWorkCapacity_ReflectsStageAndEfficiency()
        {
            var system = new ChildDevelopmentSystem();
            system.RegisterChild("infant_1", "Baby", birthDay: 100);
            system.RegisterChild("child_work", "WorkerKid", birthDay: 1);

            // infant has 0 chore capacity
            Assert.Equal(0.0f, system.GetChoreWorkCapacity("infant_1"));

            // advance to child stage
            system.TickDay(currentDay: 250);
            float childCap = system.GetChoreWorkCapacity("child_work");
            Assert.True(childCap >= 0.35f);
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsAccurately()
        {
            var system1 = new ChildDevelopmentSystem();
            var c = system1.RegisterChild("child_4", "Nora", birthDay: 5, new[] { "p1" }, "cg1");
            c.EducationScore = 42f;
            c.ChoreEfficiency = 15f;
            system1.TickDay(currentDay: 70); // triggers Toddler

            var state = system1.CaptureState();
            Assert.Single(state.Profiles);
            Assert.Single(state.MilestoneHistory);

            var system2 = new ChildDevelopmentSystem();
            system2.RestoreState(state);

            Assert.Equal(1, system2.ChildCount);
            var restored = system2.GetChild("child_4");
            Assert.NotNull(restored);
            Assert.Equal("Nora", restored.Name);
            Assert.Equal(DevelopmentStage.Toddler, restored.Stage);
            Assert.Equal(42f, restored.EducationScore);
            Assert.Equal(15f, restored.ChoreEfficiency);
            Assert.Equal("cg1", restored.AssignedCaregiverId);
            Assert.Contains("p1", restored.ParentIds);
            Assert.Single(restored.Milestones);
        }

        [Fact]
        public void ProjectCanonicalChild_DerivesAgeStageWithoutOwningState()
        {
            var generational = new GenerationalSystem(new SeededRng(44), new Ashfall.Core.Inventory.Inventory());
            var canonical = generational.EnsureChild("canonical_child", birthDay: 10, initialPhase: DevelopmentPhase.OlderChild);
            canonical.educationXp = 27f;
            canonical.assignedGuardianId = "guardian_1";

            var projected = generational.GetCanonicalChildProfile("canonical_child", currentDay: 200);

            Assert.NotNull(projected);
            Assert.Equal(DevelopmentStage.Child, projected!.Stage); // age 190
            Assert.Equal(27f, projected.EducationScore);
            Assert.Equal("guardian_1", projected.AssignedCaregiverId);
            Assert.Equal(0, generational.State.formativeEvents.Count);
        }

        [Fact]
        public void GenerationalAgeBoundary_UsesExistingAdulthoodHandoffExactlyOnce()
        {
            var generational = new GenerationalSystem(new SeededRng(45), new Ashfall.Core.Inventory.Inventory());
            var canonical = generational.EnsureChild("adult_boundary", birthDay: 1, initialPhase: DevelopmentPhase.Adolescent);
            int handoffs = 0;
            generational.OnAdulthoodReached += (_, _, _) => handoffs++;

            generational.GrowthTick(currentDay: 721);
            generational.GrowthTick(currentDay: 721);

            Assert.True(canonical.adulthoodProcessed);
            Assert.Equal(DevelopmentPhase.AdultTransitioned, canonical.developmentPhase);
            Assert.Equal(1, handoffs);
            Assert.Equal(DevelopmentStage.YoungAdult, generational.GetCanonicalStage("adult_boundary", 721));
        }
    }
}
