// SPDX-License-Identifier: MIT
// Host CLI self-test probe for Plan 188 (Individual Survivor Daily Routines).

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Godot;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliSurvivorRoutines
    {
        public static int RunSelfTest(string dataDir)
        {
            GD.Print("=== [HostCli] Individual Survivor Daily Routines Self-Test (Plan 188) ===");
            int passed = 0;
            int total = 12;

            try
            {
                // Check 1: Catalog loading
                var loadResult = RoutineTemplateCatalogLoader.Load(dataDir);
                if (loadResult.Success && loadResult.Catalog != null &&
                    loadResult.Catalog.templates.Count >= 4)
                {
                    GD.Print($"[PASS] Check 1: Catalog loaded successfully ({loadResult.Catalog.templates.Count} templates).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 1: Catalog load failed: {string.Join("; ", loadResult.Errors)}");
                }

                // Check 2: Host session instantiation & catalog binding
                var host = SurvivorRoutineHostSession.Create(dataDir);
                if (host.GetAllTemplates().Count >= 4)
                {
                    GD.Print($"[PASS] Check 2: Host session initialized with {host.GetAllTemplates().Count} routine templates.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 2: Host session template count mismatch: {host.GetAllTemplates().Count}");
                }

                // Check 3: Canonical template definitions verification
                var templates = host.GetAllTemplates();
                bool hasStandardWorker = templates.Any(t => t.template_id == "standard_worker" && t.default_blocks.Count > 0);
                bool hasNightGuard = templates.Any(t => t.template_id == "night_guard" && t.default_blocks.Count > 0);
                bool hasScavenger = templates.Any(t => t.template_id == "scavenger_flexible" && t.default_blocks.Count > 0);
                bool hasCaretaker = templates.Any(t => t.template_id == "infirmary_caretaker" && t.default_blocks.Count > 0);

                if (hasStandardWorker && hasNightGuard && hasScavenger && hasCaretaker)
                {
                    GD.Print("[PASS] Check 3: All 4 canonical routine templates (standard_worker, night_guard, scavenger_flexible, infirmary_caretaker) verified.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 3: Template verification failed: worker={hasStandardWorker}, night={hasNightGuard}, scav={hasScavenger}, care={hasCaretaker}");
                }

                // Check 4: Routine assignment
                var alphaRoutine = host.AssignRoutine("survivor_alpha", "standard_worker");
                if (alphaRoutine != null && host.TrackedRoutineCount == 1 &&
                    alphaRoutine.SurvivorId == "survivor_alpha" && alphaRoutine.TemplateId == "standard_worker" &&
                    alphaRoutine.TimeBlocks.Count > 0)
                {
                    GD.Print($"[PASS] Check 4: Routine 'standard_worker' assigned to 'survivor_alpha' ({alphaRoutine.TimeBlocks.Count} time blocks).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 4: Routine assignment failed.");
                }

                // Check 5: Hourly activity resolution
                string sleepAct = host.GetActivityAtHour("survivor_alpha", 2); // 2 AM should be Sleep
                string workAct = host.GetActivityAtHour("survivor_alpha", 11); // 11 AM should be Work
                string mealAct = host.GetActivityAtHour("survivor_alpha", 12); // 12 PM should be Meal
                if (sleepAct == "Sleep" && workAct == "Work" && mealAct == "Meal")
                {
                    GD.Print($"[PASS] Check 5: Hourly activity resolution verified: 2h={sleepAct}, 11h={workAct}, 12h={mealAct}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 5: Hourly activity mismatch: 2h={sleepAct}, 11h={workAct}, 12h={mealAct}");
                }

                // Check 6: Chronotype preference assignment
                host.SetPreference("survivor_alpha", chronotype: "early_riser", workShift: "morning", social: "extrovert");
                var pref = host.GetPreference("survivor_alpha");
                if (pref != null && pref.Chronotype == "early_riser" && pref.SocialPreference == "extrovert")
                {
                    GD.Print($"[PASS] Check 6: Survivor preference set: Chronotype={pref.Chronotype}, Shift={pref.WorkShiftPreference}, Social={pref.SocialPreference}.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 6: Survivor preference assignment failed.");
                }

                // Check 7: Ideal daily satisfaction evaluation
                var satIdeal = host.EvaluateDailySatisfaction(
                    survivorId: "survivor_alpha",
                    day: 1,
                    hoursWorked: 8,
                    hoursSlept: 8,
                    mealsHad: 3,
                    socialHours: 4 // Close to extrovert target 4.5
                );
                if (satIdeal.OverallSatisfaction >= 90.0f && satIdeal.SleepSatisfaction >= 95.0f && satIdeal.MealSatisfaction >= 95.0f)
                {
                    GD.Print($"[PASS] Check 7: Ideal satisfaction evaluated: Overall {satIdeal.OverallSatisfaction:F1}%, Sleep {satIdeal.SleepSatisfaction:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 7: Ideal satisfaction lower than expected: Overall={satIdeal.OverallSatisfaction}");
                }

                // Check 8: Penalized daily satisfaction (exhaustion / overwork)
                var satPoor = host.EvaluateDailySatisfaction(
                    survivorId: "survivor_alpha",
                    day: 2,
                    hoursWorked: 16,
                    hoursSlept: 2,
                    mealsHad: 1,
                    socialHours: 0
                );
                if (satPoor.OverallSatisfaction < 50.0f && satPoor.SleepSatisfaction <= 30.0f)
                {
                    GD.Print($"[PASS] Check 8: Penalized satisfaction evaluated: Overall {satPoor.OverallSatisfaction:F1}%, Sleep {satPoor.SleepSatisfaction:F1}%.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 8: Penalty satisfaction evaluation mismatch: Overall={satPoor.OverallSatisfaction}");
                }

                // Check 9: Enforcement level configuration & census
                host.SetEnforcementLevel("strict");
                var censusStrict = host.Census;
                if (host.EnforcementLevel == "strict" && censusStrict.EnforcementLevel == "strict")
                {
                    GD.Print($"[PASS] Check 9: Routine enforcement level updated to 'strict' and reflected in census.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 9: Enforcement level update failed: {host.EnforcementLevel}");
                }

                // Check 10: Conflict detection (roommate sleep schedule clash)
                // survivor_beta assigned to night_guard (sleeps daytime, wake night)
                host.AssignRoutine("survivor_beta", "night_guard");
                var roomAssignments = new Dictionary<string, string>
                {
                    { "survivor_alpha", "bunk_room_1" },
                    { "survivor_beta", "bunk_room_1" }
                };

                var conflicts = host.DetectConflicts(day: 3, roomAssignments: roomAssignments);
                var sleepConflict = conflicts.FirstOrDefault(c => c.ConflictType == "sleep_disturbance");
                if (sleepConflict != null && !sleepConflict.IsResolved && host.GetActiveConflicts().Count > 0)
                {
                    GD.Print($"[PASS] Check 10: Roommate sleep clash detected: {sleepConflict.ConflictId} ({sleepConflict.SurvivorA} vs {sleepConflict.SurvivorB}, Severity={sleepConflict.Severity}).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 10: Sleep disturbance conflict not detected: count={conflicts.Count}");
                }

                // Check 11: Conflict resolution
                bool resolved = host.ResolveConflict(sleepConflict!.ConflictId);
                var activeAfter = host.GetActiveConflicts();
                if (resolved && !activeAfter.Any(c => c.ConflictId == sleepConflict.ConflictId))
                {
                    GD.Print($"[PASS] Check 11: Routine conflict '{sleepConflict.ConflictId}' resolved successfully.");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 11: Conflict resolution failed: ok={resolved}");
                }

                // Check 12: Save/restore roundtrip via SurvivorRoutineSaveStore
                var captured = host.CaptureState();
                bool saveOk = SurvivorRoutineSaveStore.TrySave(captured);
                var loaded = SurvivorRoutineSaveStore.TryLoad();

                var restoredHost = SurvivorRoutineHostSession.Create(dataDir);
                restoredHost.RestoreState(loaded);
                var restoredCensus = restoredHost.Census;

                if (saveOk && loaded != null &&
                    restoredCensus.TotalRoutines == 2 &&
                    restoredCensus.TotalPreferences == 1 &&
                    restoredCensus.ResolvedConflicts == 1 &&
                    restoredCensus.EnforcementLevel == "strict")
                {
                    GD.Print($"[PASS] Check 12: SurvivorRoutineSaveStore save/load/restore roundtrip verified (2 routines, 1 pref, 1 resolved conflict).");
                    passed++;
                }
                else
                {
                    GD.PrintErr($"[FAIL] Check 12: Save/restore roundtrip failed: saveOk={saveOk}, routines={restoredCensus.TotalRoutines}, level={restoredCensus.EnforcementLevel}");
                }
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[EXCEPTION] Exception during survivor routines self-test: {ex}");
            }

            GD.Print($"=== Individual Survivor Daily Routines Self-Test Result: {passed}/{total} Passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
