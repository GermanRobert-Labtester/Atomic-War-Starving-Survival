// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan176AgingHostIntegrationTests
    {
        private static string GetStreamingAssetsDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (Directory.Exists(path)) return path;

            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data"));
            return path;
        }

        private static LifeStagesCatalog LoadCanonicalCatalog()
        {
            var result = LifeStagesCatalogLoader.Load(GetStreamingAssetsDataPath());
            Assert.True(result.Success, $"Catalog load failed: {string.Join("; ", result.Errors)}");
            Assert.NotNull(result.Catalog);
            return result.Catalog!;
        }

        [Fact]
        public void AgingSystem_BindValidatedCatalog_AppliesConfiguredDaysAndStages()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            Assert.Equal(30, system.DaysPerYear);
            Assert.Equal(65, system.MinRetirementAgeYears);
            Assert.Equal(5, system.GetAllLifeStages().Count);
            Assert.Equal(6, system.GetAllMilestones().Count);
        }

        [Fact]
        public void AgingSystem_RegisterSurvivor_StoresInitialRecord()
        {
            var system = new AgingSystem();
            var rec = system.RegisterSurvivor("survivor_01", 28, 1);

            Assert.NotNull(rec);
            Assert.Equal("survivor_01", rec.SurvivorId);
            Assert.Equal(28, rec.BaseAgeYears);
            Assert.Equal(1, rec.JoinedDay);
            Assert.False(rec.IsRetired);
            Assert.Equal(1, system.TrackedSurvivorCount);
        }

        [Fact]
        public void AgingSystem_EvaluateSurvivor_CalculatesAgeAndStageDeterministic()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_young", 24, 1);

            // Day 1: tenure 0 days -> age 24, YoungAdult
            var profileDay1 = system.EvaluateSurvivor("survivor_young", 1);
            Assert.Equal(24, profileDay1.EffectiveAgeYears);
            Assert.Equal(SurvivorLifeStage.YoungAdult, profileDay1.Stage);
            Assert.False(profileDay1.IsRetirementEligible);
            Assert.False(profileDay1.IsRetired);
            Assert.True(profileDay1.PhysicalLaborMultiplier > 1.0f);

            // Day 181: tenure 180 days / 30 = 6 years -> age 30, YoungAdult
            var profileDay181 = system.EvaluateSurvivor("survivor_young", 181);
            Assert.Equal(30, profileDay181.EffectiveAgeYears);
            Assert.Equal(SurvivorLifeStage.YoungAdult, profileDay181.Stage);

            // Day 211: tenure 210 days / 30 = 7 years -> age 31, Prime
            var profileDay211 = system.EvaluateSurvivor("survivor_young", 211);
            Assert.Equal(31, profileDay211.EffectiveAgeYears);
            Assert.Equal(SurvivorLifeStage.Prime, profileDay211.Stage);
        }

        [Fact]
        public void AgingSystem_TickDay_AdvancesChronologicalAgeAndTransitionsStage()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_edge", 30, 1);

            string transitionedSurvivor = string.Empty;
            SurvivorLifeStage oldStage = SurvivorLifeStage.Child;
            SurvivorLifeStage newStage = SurvivorLifeStage.Child;

            system.OnStageTransitioned += (id, oldS, newS) =>
            {
                transitionedSurvivor = id;
                oldStage = oldS;
                newStage = newS;
            };

            // Advance to day 31 -> tenure 30 days = 1 year -> age 31 -> YoungAdult to Prime
            system.TickDay(31);

            Assert.Equal("survivor_edge", transitionedSurvivor);
            Assert.Equal(SurvivorLifeStage.YoungAdult, oldStage);
            Assert.Equal(SurvivorLifeStage.Prime, newStage);
        }

        [Fact]
        public void AgingSystem_TickDay_CelebratesAndRecordsMilestones()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_milestone", 39, 1);

            int milestoneAgeReached = 0;
            string milestoneLabel = string.Empty;

            system.OnMilestoneReached += (id, age, label) =>
            {
                milestoneAgeReached = age;
                milestoneLabel = label;
            };

            // Day 31 -> tenure 30 days = 1 year -> age 40 -> Fortieth Year milestone
            system.TickDay(31);

            Assert.Equal(40, milestoneAgeReached);
            Assert.Equal("Fortieth Year", milestoneLabel);

            var rec = system.RegisterSurvivor("survivor_milestone", 39, 1);
            Assert.Contains(40, rec.CelebratedMilestoneAges);
        }

        [Fact]
        public void AgingSystem_RetireSurvivor_EnforcesMinimumRetirementAge()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_young", 40, 1);

            // Attempt to retire under 65 -> false
            bool retiredEarly = system.RetireSurvivor("survivor_young", 10);
            Assert.False(retiredEarly);
            Assert.False(system.IsRetired("survivor_young"));

            // Register elder (64 + 1 year = 65)
            system.RegisterSurvivor("survivor_elder", 64, 1);
            system.TickDay(31); // day 31 = age 65
            bool retiredLegal = system.RetireSurvivor("survivor_elder", 31);
            Assert.True(retiredLegal);
            Assert.True(system.IsRetired("survivor_elder"));
        }

        [Fact]
        public void AgingSystem_RetireSurvivor_AppliesRetiredFlagsAndModifiers()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_elder", 66, 1);
            system.RetireSurvivor("survivor_elder", 5);

            var profile = system.EvaluateSurvivor("survivor_elder", 5);
            Assert.True(profile.IsRetired);
            Assert.Equal(SurvivorLifeStage.Elderly, profile.Stage);
            Assert.Equal(SurvivorAgingProgressionEngine.RetiredLightDutyMultiplier, profile.PhysicalLaborMultiplier);
            Assert.True(profile.FatigueAccumulationMultiplier < 1.0f);
            Assert.True(profile.MentorshipXpBonus > 0f);
        }

        [Fact]
        public void AgingSystem_HasLivingElderMentor_DetectsLivingElderInParty()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("survivor_a", 25, 1);
            system.RegisterSurvivor("survivor_b", 40, 1);

            var rosterOnlyYoung = new[] { "survivor_a", "survivor_b" };
            Assert.False(system.HasLivingElderMentor(1, rosterOnlyYoung));

            system.RegisterSurvivor("survivor_c", 68, 1);
            var rosterWithElder = new[] { "survivor_a", "survivor_b", "survivor_c" };
            Assert.True(system.HasLivingElderMentor(1, rosterWithElder));
        }

        [Fact]
        public void AgingSystem_Census_ReflectsAccurateDemographicCounts()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("s_child", 12, 1);
            system.RegisterSurvivor("s_adult", 35, 1);
            system.RegisterSurvivor("s_elder1", 67, 1);
            system.RegisterSurvivor("s_elder2", 70, 1);

            system.TickDay(1); // update evaluated stages
            system.RetireSurvivor("s_elder2", 1);

            var census = system.GetCensus();
            Assert.Equal(4, census.TotalTrackedSurvivors);
            Assert.Equal(1, census.RetiredSurvivors);
            Assert.Equal(2, census.ElderlySurvivors);
            Assert.Equal(30, census.DaysPerYear);
            Assert.Equal(65, census.MinRetirementAge);
        }

        [Fact]
        public void AgingSystem_CaptureAndRestoreState_RoundtripsFaithfully()
        {
            var system = new AgingSystem();
            var catalog = LoadCanonicalCatalog();
            system.BindValidatedCatalog(catalog);

            system.RegisterSurvivor("s1", 29, 1);
            system.RegisterSurvivor("s2", 66, 1);
            system.TickDay(31); // s1 turns 30 (milestone), s2 turns 67
            system.RetireSurvivor("s2", 31);

            var captured = system.CaptureState();
            Assert.Equal(1, captured.SchemaVersion);
            Assert.Equal(2, captured.Records.Count);

            var restoredSystem = new AgingSystem();
            restoredSystem.BindValidatedCatalog(catalog);
            restoredSystem.RestoreState(captured);

            var census = restoredSystem.GetCensus();
            Assert.Equal(2, census.TotalTrackedSurvivors);
            Assert.Equal(1, census.RetiredSurvivors);
            Assert.Equal(1, census.ElderlySurvivors);
            Assert.Equal(6, census.TotalMilestonesCelebrated);
            Assert.True(restoredSystem.IsRetired("s2"));
            Assert.False(restoredSystem.IsRetired("s1"));
        }
    }
}
