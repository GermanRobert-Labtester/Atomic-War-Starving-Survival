// SPDX-License-Identifier: MIT
// xUnit tests for Plan 188: RoutineTemplateCatalogLoader and SurvivorRoutineSystem
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Records
{
    public sealed class RoutineTemplateCatalogLoaderTests
    {
        private static string GetStreamingAssetsDataPath()
        {
            string baseDir = AppDomain.CurrentDomain.BaseDirectory;
            string path = Path.GetFullPath(Path.Combine(baseDir, "..", "..", "..", "..", "Assets", "StreamingAssets", "Data"));
            if (Directory.Exists(path)) return path;

            path = Path.GetFullPath(Path.Combine(Directory.GetCurrentDirectory(), "Assets", "StreamingAssets", "Data"));
            return path;
        }

        [Fact]
        public void Load_CanonicalCatalog_SucceedsWithExpectedTemplates()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);

            Assert.True(result.Success, $"Catalog load failed: {string.Join("; ", result.Errors)}");
            Assert.NotNull(result.Catalog);
            Assert.True(result.Catalog!.templates.Count >= 4,
                $"Expected at least 4 templates; found {result.Catalog.templates.Count}");
        }

        [Fact]
        public void Load_CanonicalCatalog_AllTemplatesHaveNonEmptyDefaultBlocks()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            foreach (var t in result.Catalog!.templates)
            {
                Assert.False(string.IsNullOrEmpty(t.template_id), "Template must have non-empty template_id.");
                Assert.False(string.IsNullOrEmpty(t.display_name), $"Template '{t.template_id}' must have non-empty display_name.");
                Assert.NotEmpty(t.default_blocks);
            }
        }

        [Fact]
        public void Load_CanonicalCatalog_AllFourCanonicalTemplatesPresent()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var ids = result.Catalog!.templates.Select(t => t.template_id).ToHashSet(StringComparer.OrdinalIgnoreCase);
            Assert.Contains("routine_standard", ids);
            Assert.Contains("routine_night_shift", ids);
            Assert.Contains("routine_early_riser", ids);
            Assert.Contains("routine_night_owl", ids);
        }

        [Fact]
        public void Load_CanonicalCatalog_NoduplicateTemplateIds()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var ids = result.Catalog!.templates.Select(t => t.template_id).ToList();
            var uniqueIds = ids.Distinct(StringComparer.OrdinalIgnoreCase).ToList();
            Assert.Equal(uniqueIds.Count, ids.Count);
        }

        [Fact]
        public void Load_MissingFile_ReturnsErrorResult()
        {
            var result = RoutineTemplateCatalogLoader.Load(Path.GetTempPath() + Guid.NewGuid().ToString());
            Assert.False(result.Success);
            Assert.NotEmpty(result.Errors);
        }

        [Fact]
        public void SurvivorRoutineSystem_AssignRoutine_TracksCorrectly()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var system = new SurvivorRoutineSystem();
            system.BindValidatedCatalog(result.Catalog!);

            var record = system.AssignRoutine("survivor_alpha", "routine_standard");
            Assert.Equal("survivor_alpha", record.SurvivorId);
            Assert.Equal("routine_standard", record.TemplateId);
            Assert.NotEmpty(record.TimeBlocks);
            Assert.Equal(1, system.TrackedRoutineCount);
        }

        [Fact]
        public void SurvivorRoutineSystem_HourlyActivityResolution_ReturnsExpectedTypes()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var system = new SurvivorRoutineSystem();
            system.BindValidatedCatalog(result.Catalog!);
            system.AssignRoutine("survivor_alpha", "routine_standard");

            // routine_standard: sleep ~23-7, work ~9-17, meal ~13-14
            string sleepAct = system.GetActivityAtHour("survivor_alpha", 2);
            string workAct = system.GetActivityAtHour("survivor_alpha", 11);
            string mealAct = system.GetActivityAtHour("survivor_alpha", 13);

            Assert.Equal("Sleep", sleepAct);
            Assert.Equal("Work", workAct);
            Assert.Equal("Meal", mealAct);
        }

        [Fact]
        public void SurvivorRoutineSystem_SatisfactionEvaluation_ScoresCorrectly()
        {
            var system = new SurvivorRoutineSystem();

            // Ideal inputs
            var satIdeal = system.EvaluateDailySatisfaction("survivor_a", day: 1,
                hoursWorked: 8, hoursSlept: 8, mealsHad: 3, socialHours: 3);
            Assert.True(satIdeal.OverallSatisfaction >= 90f,
                $"Ideal satisfaction too low: {satIdeal.OverallSatisfaction}");

            // Exhausted/starving inputs
            var satPoor = system.EvaluateDailySatisfaction("survivor_b", day: 1,
                hoursWorked: 16, hoursSlept: 2, mealsHad: 0, socialHours: 0);
            Assert.True(satPoor.OverallSatisfaction < 50f,
                $"Penalized satisfaction too high: {satPoor.OverallSatisfaction}");

            Assert.True(satIdeal.OverallSatisfaction > satPoor.OverallSatisfaction);
        }

        [Fact]
        public void SurvivorRoutineSystem_ConflictDetectionAndResolution_Works()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var system = new SurvivorRoutineSystem();
            system.BindValidatedCatalog(result.Catalog!);

            system.AssignRoutine("survivor_alpha", "routine_standard");  // sleeps 23-7
            system.AssignRoutine("survivor_beta", "routine_night_shift"); // sleeps during day

            var roomAssignments = new Dictionary<string, string>
            {
                { "survivor_alpha", "bunk_room_1" },
                { "survivor_beta", "bunk_room_1" }
            };

            var conflicts = system.DetectConflicts(day: 1, roomAssignments: roomAssignments);
            var sleepConflict = conflicts.FirstOrDefault(c => c.ConflictType == "sleep_disturbance");
            Assert.NotNull(sleepConflict);
            Assert.False(sleepConflict!.IsResolved);

            bool resolved = system.ResolveConflict(sleepConflict.ConflictId);
            Assert.True(resolved);
            Assert.Empty(system.GetActiveConflicts());
        }

        [Fact]
        public void SurvivorRoutineSystem_SaveRestoreRoundtrip_PreservesState()
        {
            string dataDir = GetStreamingAssetsDataPath();
            var result = RoutineTemplateCatalogLoader.Load(dataDir);
            Assert.True(result.Success);

            var system1 = new SurvivorRoutineSystem();
            system1.BindValidatedCatalog(result.Catalog!);
            system1.AssignRoutine("survivor_alpha", "routine_standard");
            system1.SetPreference("survivor_alpha", "early_riser", "morning", "extrovert");
            system1.SetEnforcementLevel("strict");
            system1.EvaluateDailySatisfaction("survivor_alpha", 1, 8, 8, 3, 2);

            var captured = system1.CaptureState();
            var census1 = system1.GetCensus();

            var system2 = new SurvivorRoutineSystem();
            system2.RestoreState(captured);
            system2.BindValidatedCatalog(result.Catalog!);

            var census2 = system2.GetCensus();
            Assert.Equal(census1.TotalRoutines, census2.TotalRoutines);
            Assert.Equal(census1.TotalPreferences, census2.TotalPreferences);
            Assert.Equal("strict", census2.EnforcementLevel);

            var restoredRoutine = system2.GetRoutine("survivor_alpha");
            Assert.NotNull(restoredRoutine);
            Assert.Equal("routine_standard", restoredRoutine!.TemplateId);
        }
    }
}
