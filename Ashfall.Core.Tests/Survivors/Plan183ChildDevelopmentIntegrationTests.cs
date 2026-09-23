// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 183: Child Development Stages System — Integration Tests
// Verifies trait catalog loading, child profile registration, developmental
// stage progression, milestone history, caregiver passive growth, and state save/restore.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Plan183ChildDevelopment
{
    public sealed class Plan183ChildDevelopmentIntegrationTests
    {
        private static string ResolveDataPath(string filename)
        {
            var candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates) { if (File.Exists(c)) return Path.GetFullPath(c); }
            return Path.GetFullPath(Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename));
        }

        [Fact]
        public void LoadCatalog_LoadsTraitsFromJson()
        {
            var system = new ChildDevelopmentSystem();
            string path = ResolveDataPath("development_traits.json");
            Assert.True(File.Exists(path), $"development_traits.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var traits = system.GetAllTraits();
            Assert.NotEmpty(traits);
            Assert.NotNull(system.GetTrait("development_trait_resilient"));
            Assert.NotNull(system.GetTrait("development_trait_field_medic_instinct"));
        }

        [Fact]
        public void RegisterChild_InitializesAtInfantStage()
        {
            var system = new ChildDevelopmentSystem();
            var child = system.RegisterChild("child_leo", "Leo", birthDay: 5, new[] { "parent_maya" });

            Assert.NotNull(child);
            Assert.Equal("child_leo", child.ChildId);
            Assert.Equal("Leo", child.Name);
            Assert.Equal(5, child.BirthDay);
            Assert.Equal(DevelopmentStage.Infant, child.Stage);
            Assert.Contains("parent_maya", child.ParentIds);
        }

        [Fact]
        public void TickDay_AdvancesStageAndTriggersMilestoneEvent()
        {
            var system = new ChildDevelopmentSystem();
            system.RegisterChild("child_sarah", "Sarah", birthDay: 1);

            DevelopmentStage newStageObserved = DevelopmentStage.Infant;
            system.OnStageChanged += (profile, stage) => newStageObserved = stage;

            DevelopmentMilestoneEvent? eventObserved = null;
            system.OnMilestoneAchieved += ev => eventObserved = ev;

            // Day 65: age = 64 days -> Toddler threshold (>= 60)
            system.TickDay(currentDay: 65);

            var child = system.GetChild("child_sarah");
            Assert.NotNull(child);
            Assert.Equal(DevelopmentStage.Toddler, child!.Stage);
            Assert.Equal(DevelopmentStage.Toddler, newStageObserved);
            Assert.NotNull(eventObserved);
            Assert.Equal(DevelopmentStage.Toddler, eventObserved!.NewStage);
            Assert.Contains("Milestone_Toddler", child.Milestones);
        }

        [Fact]
        public void EducationAndCaregiver_ImprovesEducationScoreAndChoreCapacity()
        {
            var system = new ChildDevelopmentSystem();
            system.RegisterChild("child_toby", "Toby", birthDay: 1, caregiverId: "caregiver_elena");

            // Advance Toby to Child stage (>= 180 days, e.g. Day 200)
            system.TickDay(currentDay: 200);

            var child = system.GetChild("child_toby");
            Assert.NotNull(child);
            Assert.Equal(DevelopmentStage.Child, child!.Stage);

            // Record active education
            system.RecordEducation("child_toby", 15f);
            Assert.True(child.EducationScore >= 15f);

            // Chore work capacity is nonzero at Child stage
            float choreCapacity = system.GetChoreWorkCapacity("child_toby");
            Assert.True(choreCapacity > 0.3f);
        }

        [Fact]
        public void ResolveStage_AccuratelyMapsAgeThresholds()
        {
            Assert.Equal(DevelopmentStage.Infant, ChildDevelopmentSystem.ResolveStage(0));
            Assert.Equal(DevelopmentStage.Infant, ChildDevelopmentSystem.ResolveStage(59));
            Assert.Equal(DevelopmentStage.Toddler, ChildDevelopmentSystem.ResolveStage(60));
            Assert.Equal(DevelopmentStage.Toddler, ChildDevelopmentSystem.ResolveStage(179));
            Assert.Equal(DevelopmentStage.Child, ChildDevelopmentSystem.ResolveStage(180));
            Assert.Equal(DevelopmentStage.Child, ChildDevelopmentSystem.ResolveStage(499));
            Assert.Equal(DevelopmentStage.Adolescent, ChildDevelopmentSystem.ResolveStage(500));
            Assert.Equal(DevelopmentStage.Adolescent, ChildDevelopmentSystem.ResolveStage(719));
            Assert.Equal(DevelopmentStage.YoungAdult, ChildDevelopmentSystem.ResolveStage(720));
        }

        [Fact]
        public void SaveRestoreState_PreservesChildProfilesAndMilestoneHistory()
        {
            var system = new ChildDevelopmentSystem();
            system.RegisterChild("child_saved", "SavedChild", birthDay: 1, caregiverId: "guardian_alex");
            system.TickDay(currentDay: 70); // triggers Toddler milestone

            var state = system.CaptureState();

            var restored = new ChildDevelopmentSystem();
            restored.RestoreState(state);

            Assert.Equal(1, restored.ChildCount);
            var child = restored.GetChild("child_saved");
            Assert.NotNull(child);
            Assert.Equal(DevelopmentStage.Toddler, child!.Stage);
            Assert.Equal("guardian_alex", child.AssignedCaregiverId);
        }
    }
}
