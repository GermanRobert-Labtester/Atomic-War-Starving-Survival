// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 176: Aging & Elderly Survivor System — Integration Tests
// Verifies catalog loading, survivor registration, age calculation,
// life-stage progression, dignified retirement, milestone celebrations,
// and save/restore persistence.
// ============================================================================
using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Plan176Aging
{
    public sealed class Plan176AgingSystemIntegrationTests
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
        public void LoadCatalog_LoadsLifeStagesAndMilestones()
        {
            var system = new AgingSystem();
            string path = ResolveDataPath("life_stages.json");
            Assert.True(File.Exists(path), $"life_stages.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            Assert.Equal(30, system.DaysPerYear);
            Assert.Equal(65, system.MinRetirementAgeYears);

            var stages = system.GetAllLifeStages();
            Assert.Equal(5, stages.Count);
            Assert.Contains(stages, s => s.stage_id == "stage_young_adult");
            Assert.Contains(stages, s => s.stage_id == "stage_elderly");

            var milestones = system.GetAllMilestones();
            Assert.True(milestones.Count >= 6);
            Assert.Contains(milestones, m => m.age == 65);
        }

        [Fact]
        public void EvaluateSurvivor_CalculatesAgeAndStageCorrectly()
        {
            var system = new AgingSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));

            system.RegisterSurvivor("survivor_young", baseAgeYears: 20, joinedDay: 1);
            system.RegisterSurvivor("survivor_middle", baseAgeYears: 48, joinedDay: 1);

            // Day 1
            var profileYoung = system.EvaluateSurvivor("survivor_young", currentDay: 1);
            Assert.Equal(20, profileYoung.EffectiveAgeYears);
            Assert.Equal(SurvivorLifeStage.YoungAdult, profileYoung.Stage);
            Assert.False(profileYoung.IsRetirementEligible);

            // Day 61 (2 years in at 30 days/year) -> age 50 -> Prime, but day 91 -> age 51 -> MiddleAge
            var profileMiddle = system.EvaluateSurvivor("survivor_middle", currentDay: 91);
            Assert.Equal(51, profileMiddle.EffectiveAgeYears);
            Assert.Equal(SurvivorLifeStage.MiddleAge, profileMiddle.Stage);
            Assert.True(profileMiddle.MentorshipXpBonus > 0f);
        }

        [Fact]
        public void RetireSurvivor_EnforcesEligibilityAndAppliesRetirementProfile()
        {
            var system = new AgingSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));

            system.RegisterSurvivor("survivor_senior", baseAgeYears: 64, joinedDay: 1);

            // Day 1: age 64 -> not yet 65, cannot retire
            bool retiredEarly = system.RetireSurvivor("survivor_senior", currentDay: 1);
            Assert.False(retiredEarly);
            Assert.False(system.IsRetired("survivor_senior"));

            // Day 61: 2 years pass (30 days/yr) -> age 66 -> stage is Elderly and eligible
            string? retiredId = null;
            system.OnSurvivorRetired += (id, day) => retiredId = id;

            bool retired = system.RetireSurvivor("survivor_senior", currentDay: 61);
            Assert.True(retired);
            Assert.True(system.IsRetired("survivor_senior"));
            Assert.Equal("survivor_senior", retiredId);

            var profile = system.EvaluateSurvivor("survivor_senior", currentDay: 61);
            Assert.True(profile.IsRetired);
            Assert.Equal(SurvivorLifeStage.Elderly, profile.Stage);
            Assert.Equal(SurvivorAgingProgressionEngine.RetiredLightDutyMultiplier, profile.PhysicalLaborMultiplier);
        }

        [Fact]
        public void TickDay_DetectsStageTransitionsAndCelebratesMilestones()
        {
            var system = new AgingSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));

            // Start at age 49 on Day 1
            system.RegisterSurvivor("survivor_advancing", baseAgeYears: 49, joinedDay: 1);

            int milestoneAgeReached = 0;
            system.OnMilestoneReached += (id, age, label) => milestoneAgeReached = age;

            SurvivorLifeStage oldStageRecorded = SurvivorLifeStage.Prime;
            SurvivorLifeStage newStageRecorded = SurvivorLifeStage.Prime;
            system.OnStageTransitioned += (id, oldS, newS) =>
            {
                oldStageRecorded = oldS;
                newStageRecorded = newS;
            };

            // Advance 60 days (2 years -> age 51: hits age 50 milestone and transitions from Prime to MiddleAge)
            system.TickDay(currentDay: 61, new[] { "survivor_advancing" });

            Assert.Equal(50, milestoneAgeReached);
            Assert.Equal(SurvivorLifeStage.Prime, oldStageRecorded);
            Assert.Equal(SurvivorLifeStage.MiddleAge, newStageRecorded);
        }

        [Fact]
        public void HasLivingElderMentor_DetectsActiveElderPresence()
        {
            var system = new AgingSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));

            system.RegisterSurvivor("survivor_young", baseAgeYears: 22, joinedDay: 1);
            system.RegisterSurvivor("survivor_elder", baseAgeYears: 68, joinedDay: 1);

            bool onlyYoung = system.HasLivingElderMentor(currentDay: 1, new[] { "survivor_young" });
            Assert.False(onlyYoung);

            bool withElder = system.HasLivingElderMentor(currentDay: 1, new[] { "survivor_young", "survivor_elder" });
            Assert.True(withElder);
        }

        [Fact]
        public void SaveRestoreState_PreservesTrackingAndRetirementStatus()
        {
            var system = new AgingSystem();
            system.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));

            system.RegisterSurvivor("survivor_saved", baseAgeYears: 66, joinedDay: 1);
            system.RetireSurvivor("survivor_saved", currentDay: 10);
            system.TickDay(currentDay: 30, new[] { "survivor_saved" });

            var state = system.CaptureState();

            var restored = new AgingSystem();
            restored.LoadCatalog(File.ReadAllText(ResolveDataPath("life_stages.json")));
            restored.RestoreState(state);

            Assert.Equal(1, restored.TrackedSurvivorCount);
            Assert.Equal(1, restored.RetiredSurvivorCount);
            Assert.True(restored.IsRetired("survivor_saved"));

            var profile = restored.EvaluateSurvivor("survivor_saved", currentDay: 30);
            Assert.True(profile.IsRetired);
            Assert.Equal(SurvivorLifeStage.Elderly, profile.Stage);
        }
    }
}
