// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Achievements;

namespace Ashfall.Core.Tests.Plan149Achievements
{
    public class Plan149AchievementIntegrationTests
    {
        private static string GetCatalogJson()
        {
            string filename = "achievements.json";
            string[] candidates = new[]
            {
                Path.Combine(AppContext.BaseDirectory, "../../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(AppContext.BaseDirectory, "../../../Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "Assets/StreamingAssets/Data", filename),
                Path.Combine(Directory.GetCurrentDirectory(), "../Assets/StreamingAssets/Data", filename)
            };
            foreach (var c in candidates)
            {
                if (File.Exists(c)) return File.ReadAllText(c);
            }
            throw new FileNotFoundException($"Could not find {filename} in candidate paths.");
        }

        [Fact]
        public void CatalogLoad_LoadsAllAuthoredAchievements()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);

            Assert.NotNull(catalog);
            Assert.True(catalog.All.Count >= 16, $"Expected >= 16 achievements, found {catalog.All.Count}");

            // Verify canonical categories exist
            var survival = catalog.GetByCategory("survival").ToList();
            var combat = catalog.GetByCategory("combat").ToList();
            var social = catalog.GetByCategory("social").ToList();
            var exploration = catalog.GetByCategory("exploration").ToList();
            var economic = catalog.GetByCategory("economic").ToList();
            var moral = catalog.GetByCategory("moral").ToList();

            Assert.NotEmpty(survival);
            Assert.NotEmpty(combat);
            Assert.NotEmpty(social);
            Assert.NotEmpty(exploration);
            Assert.NotEmpty(economic);
            Assert.NotEmpty(moral);

            // Verify the six panel milestones migrated from AchievementsPanel literals
            Assert.True(catalog.TryGet("first_week_survivor", out var a1));
            Assert.Equal("First Week Survivor", a1.Name);
            Assert.Equal(7.0f, a1.TargetThreshold);

            Assert.True(catalog.TryGet("two_week_endurance", out var a2));
            Assert.Equal("Two-Week Endurance", a2.Name);
            Assert.Equal(14.0f, a2.TargetThreshold);

            Assert.True(catalog.TryGet("month_of_ash", out var a3));
            Assert.Equal("Month of Ash", a3.Name);
            Assert.Equal(30.0f, a3.TargetThreshold);

            Assert.True(catalog.TryGet("no_casualties", out _));
            Assert.True(catalog.TryGet("low_exposure", out _));
            Assert.True(catalog.TryGet("healthy_cohort", out _));
        }

        [Fact]
        public void ConditionEvaluation_SurvivalMilestones_TriggerUnlocks()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);
            var system = new AchievementSystem(catalog, "campaign_alpha");

            Assert.False(system.IsUnlocked("first_week_survivor"));

            // Day 5: Not yet unlocked
            system.SetFact("survive_days", 5.0f);
            Assert.False(system.IsUnlocked("first_week_survivor"));

            // Day 7: Unlocked!
            system.SetFact("survive_days", 7.0f);
            Assert.True(system.IsUnlocked("first_week_survivor"));
            Assert.False(system.IsUnlocked("two_week_endurance"));

            // Day 14: Next milestone unlocked
            system.SetFact("survive_days", 14.0f);
            Assert.True(system.IsUnlocked("two_week_endurance"));
        }

        [Fact]
        public void OnceOnlyUnlockEmission_SeamFiresExactlyOnce()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);
            var system = new AchievementSystem(catalog, "campaign_alpha");

            int eventFireCount = 0;
            string lastUnlockedId = string.Empty;
            string lastCampaignId = string.Empty;

            system.OnAchievementUnlockedSeam += (achId, campId) =>
            {
                eventFireCount++;
                lastUnlockedId = achId;
                lastCampaignId = campId;
            };

            // Unlock once
            system.SetFact("survive_days", 7.0f);
            Assert.Equal(1, eventFireCount);
            Assert.Equal("first_week_survivor", lastUnlockedId);
            Assert.Equal("campaign_alpha", lastCampaignId);

            // Redundant triggers should not fire again
            system.SetFact("survive_days", 8.0f);
            system.Unlock("first_week_survivor");
            Assert.Equal(1, eventFireCount);
        }

        [Fact]
        public void SnapshotEvaluation_ReplacesDerivedPanelLiterals()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);
            var system = new AchievementSystem(catalog, "campaign_test");

            // Snapshot: Day 30, 6/6 alive, avg health 85, avg dose 12 mSv, avg morale 80
            system.EvaluateRosterSnapshot(
                simDay: 30,
                totalRoster: 6,
                aliveCount: 6,
                avgHealth: 85.0f,
                avgDose: 12.0f,
                avgMorale: 80.0f);

            Assert.True(system.IsUnlocked("first_week_survivor"));
            Assert.True(system.IsUnlocked("two_week_endurance"));
            Assert.True(system.IsUnlocked("month_of_ash"));
            Assert.True(system.IsUnlocked("no_casualties"));
            Assert.True(system.IsUnlocked("low_exposure"));
            Assert.True(system.IsUnlocked("healthy_cohort"));
            Assert.True(system.IsUnlocked("harmonious_bunker"));
        }

        [Fact]
        public void Persistence_CaptureAndRestoreRoundtrip()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);
            var system1 = new AchievementSystem(catalog, "campaign_persist");

            system1.SetFact("survive_days", 7.0f);
            system1.SetFact("trades_completed", 5.0f);
            Assert.True(system1.IsUnlocked("first_week_survivor"));
            Assert.True(system1.IsUnlocked("thriving_caravan"));

            string savedState = system1.CaptureState();
            Assert.Contains("\"schema_version\":1", savedState);
            Assert.Contains("first_week_survivor", savedState);
            Assert.Contains("thriving_caravan", savedState);

            var system2 = new AchievementSystem(catalog, "campaign_persist");
            system2.RestoreState(savedState);

            Assert.True(system2.IsUnlocked("first_week_survivor"));
            Assert.True(system2.IsUnlocked("thriving_caravan"));
            Assert.False(system2.IsUnlocked("two_week_endurance"));
            Assert.Equal(7.0f, system2.GetProgressFact("survive_days"));
            Assert.Equal(5.0f, system2.GetProgressFact("trades_completed"));
        }

        [Fact]
        public void CrossRunProfileExport_ProvidesReadonlyIdsForPlan175()
        {
            string json = GetCatalogJson();
            var catalog = AchievementCatalog.LoadFromJson(json);
            var system = new AchievementSystem(catalog, "campaign_export");

            system.Unlock("first_week_survivor");
            system.Unlock("perimeter_secured");

            var exported = system.GetCompletedAchievementIds();
            Assert.Equal(2, exported.Count);
            Assert.Contains("first_week_survivor", exported);
            Assert.Contains("perimeter_secured", exported);
        }
    }
}
