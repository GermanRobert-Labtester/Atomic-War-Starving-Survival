// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 188: Individual Survivor Daily Routines — Integration Tests
// Verifies routine template catalog loading, routine assignment, hourly activity
// querying across day/night boundaries, satisfaction evaluation, conflict
// detection/resolution, and save/restore state persistence.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Xunit;
using Ashfall.Core.Survivors;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class Plan188SurvivorRoutineIntegrationTests : CatalogTestBase
    {
        [Fact]
        public void LoadCatalog_LoadsRoutineTemplates()
        {
            var system = new SurvivorRoutineSystem();
            string path = Path.Combine(DataDirectory, "routine_templates.json");
            Assert.True(File.Exists(path), $"routine_templates.json must exist at {path}");

            system.LoadCatalog(File.ReadAllText(path));

            var templates = system.GetAllTemplates();
            Assert.Equal(4, templates.Count);

            var earlyRiser = system.GetTemplate("routine_early_riser");
            Assert.NotNull(earlyRiser);
            Assert.Equal(5, earlyRiser.wake_hour);
            Assert.Equal(21, earlyRiser.sleep_hour);
            Assert.Equal(3, earlyRiser.meal_hours.Count);
            Assert.Equal(7, earlyRiser.default_blocks.Count);

            var nightOwl = system.GetTemplate("routine_night_owl");
            Assert.NotNull(nightOwl);
            Assert.Equal(10, nightOwl.wake_hour);
            Assert.Equal(2, nightOwl.sleep_hour);
        }

        [Fact]
        public void AssignRoutine_PopulatesTimeBlocksAndRespectsTemplate()
        {
            var system = new SurvivorRoutineSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));

            string? assignedSurvivor = null;
            string? assignedTemplate = null;
            system.OnRoutineAssigned += (s, t) =>
            {
                assignedSurvivor = s;
                assignedTemplate = t;
            };

            var routine = system.AssignRoutine("surv_01", "routine_early_riser");

            Assert.NotNull(routine);
            Assert.Equal("surv_01", assignedSurvivor);
            Assert.Equal("routine_early_riser", assignedTemplate);
            Assert.Equal(1, system.TrackedRoutineCount);
            Assert.Equal(5, routine.WakeHour);
            Assert.Equal(21, routine.SleepHour);
            Assert.Equal(7, routine.TimeBlocks.Count);
        }

        [Fact]
        public void GetActivityAtHour_ReturnsCorrectActivityThroughoutDay()
        {
            var system = new SurvivorRoutineSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));
            system.AssignRoutine("surv_01", "routine_early_riser");

            // Routine early riser:
            // 21 - 5: Sleep (wraps midnight)
            // 5 - 6: Meal
            // 6 - 14: Work
            // 14 - 17: Social
            // 18 - 19: Meal
            // 19 - 21: Leisure
            Assert.Equal("Sleep", system.GetActivityAtHour("surv_01", 4));
            Assert.Equal("Sleep", system.GetActivityAtHour("surv_01", 22));
            Assert.Equal("Meal", system.GetActivityAtHour("surv_01", 5));
            Assert.Equal("Work", system.GetActivityAtHour("surv_01", 8));
            Assert.Equal("Social", system.GetActivityAtHour("surv_01", 15));
            Assert.Equal("Leisure", system.GetActivityAtHour("surv_01", 20));
        }

        [Fact]
        public void EvaluateDailySatisfaction_CalculatesCategoryAndOverallScores()
        {
            var system = new SurvivorRoutineSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));
            system.AssignRoutine("surv_02", "routine_standard");
            system.SetPreference("surv_02", "intermediate", "morning", "introvert");

            RoutineSatisfactionRecord? evaluated = null;
            system.OnSatisfactionEvaluated += r => evaluated = r;

            // Ideal sleep = 8h, ideal meals = 3, ideal work = 8h, introvert ideal social = 1.5h
            var record = system.EvaluateDailySatisfaction(
                "surv_02", day: 1, hoursWorked: 8, hoursSlept: 8, mealsHad: 3, socialHours: 2);

            Assert.NotNull(evaluated);
            Assert.Equal("surv_02", record.SurvivorId);
            Assert.Equal(100.0f, record.SleepSatisfaction);
            Assert.Equal(100.0f, record.MealSatisfaction);
            Assert.Equal(100.0f, record.WorkSatisfaction);
            Assert.True(record.SocialSatisfaction >= 85.0f);
            Assert.True(record.OverallSatisfaction >= 95.0f);
        }

        [Fact]
        public void DetectAndResolveConflicts_IdentifiesSleepDisturbanceAndResolves()
        {
            var system = new SurvivorRoutineSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));

            system.AssignRoutine("surv_early", "routine_early_riser"); // sleep 21
            system.AssignRoutine("surv_night", "routine_night_owl");   // sleep 2 (diff = 5 hours >= 4)

            var roomAssignments = new Dictionary<string, string>
            {
                { "surv_early", "dorm_alpha" },
                { "surv_night", "dorm_alpha" }
            };

            RoutineConflictRecord? detected = null;
            system.OnConflictDetected += c => detected = c;

            var conflicts = system.DetectConflicts(day: 2, roomAssignments: roomAssignments);

            Assert.Single(conflicts);
            Assert.NotNull(detected);
            Assert.Equal("sleep_disturbance", detected.ConflictType);
            Assert.Equal("major", detected.Severity);
            Assert.Single(system.GetActiveConflicts());

            // Resolve conflict
            bool resolved = system.ResolveConflict(detected.ConflictId);
            Assert.True(resolved);
            Assert.Empty(system.GetActiveConflicts());
        }

        [Fact]
        public void SaveRestoreState_PreservesRoutinesPreferencesAndConflicts()
        {
            var system = new SurvivorRoutineSystem();
            system.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));

            system.SetEnforcementLevel("strict");
            system.AssignRoutine("surv_alex", "routine_night_shift");
            system.SetPreference("surv_alex", "night_owl", "night", "extrovert");
            system.EvaluateDailySatisfaction("surv_alex", 3, 7, 7, 3, 4);

            var state = system.CaptureState();

            var restoredSystem = new SurvivorRoutineSystem();
            restoredSystem.LoadCatalog(File.ReadAllText(Path.Combine(DataDirectory, "routine_templates.json")));
            restoredSystem.RestoreState(state);

            Assert.Equal("strict", restoredSystem.EnforcementLevel);
            Assert.Equal(1, restoredSystem.TrackedRoutineCount);

            var r = restoredSystem.GetRoutine("surv_alex");
            Assert.NotNull(r);
            Assert.Equal("routine_night_shift", r.TemplateId);
            Assert.Equal(18, r.WakeHour);
            Assert.Equal(10, r.SleepHour);

            var p = restoredSystem.GetPreference("surv_alex");
            Assert.NotNull(p);
            Assert.Equal("night_owl", p.Chronotype);
            Assert.Equal("extrovert", p.SocialPreference);
        }
    }
}
