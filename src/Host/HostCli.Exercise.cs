// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ExerciseSelfTest
// Subsystem          : Plan 216 — Survivor Exercise & Physical Training
// ============================================================================

using System;
using System.IO;
using System.Linq;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliExercise
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Exercise & Physical Training System Self-Test (Plan 216) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                // Check 1: Catalog loading from exercise_routines.json
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir) ? dataDir : CatalogPath.ResolveDataDir());
                string catPath = Path.Combine(dataRoot, "exercise_routines.json");

                var session = ExerciseHostSession.Create();
                if (File.Exists(catPath))
                {
                    session.LoadCatalog(File.ReadAllText(catPath));
                }

                if (session.Census.AuthoredRoutinesCount >= 3)
                {
                    Console.WriteLine($"[PASS] Check 1: Catalog loaded {session.Census.AuthoredRoutinesCount} exercise routines.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 1: Exercise routines catalog failed to load (count={session.Census.AuthoredRoutinesCount}).");
                }

                // Check 2: Routine definition query and gains verification
                var calisthenics = session.System.GetRoutine("routine_calisthenics_basic");
                var jog = session.System.GetRoutine("routine_perimeter_jog");
                if (calisthenics != null && jog != null && calisthenics.min_fitness_level == 0f && jog.min_fitness_level == 15f)
                {
                    Console.WriteLine("[PASS] Check 2: Routine definitions verified (Calisthenics: 0 min fitness, Jog: 15 min fitness).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: Routine definition query verification failed.");
                }

                // Check 3: Fitness profile creation and baseline values
                var profile = session.GetOrCreateProfile("surv_boris", initialBase: 30f);
                if (profile != null && profile.SurvivorId == "surv_boris" && profile.Cardio == 30f && profile.OverallConditioning == 30f)
                {
                    Console.WriteLine($"[PASS] Check 3: Created fitness profile for surv_boris (Overall Conditioning: {profile.OverallConditioning:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: Fitness profile initialization failed.");
                }

                // Check 4: Prerequisite gating for advanced routines
                var noviceProfile = session.GetOrCreateProfile("surv_novice", initialBase: 10f);
                var blockedResult = session.ExecuteRoutine("surv_novice", "routine_scrap_weightlifting", currentDay: 1); // requires 25
                if (blockedResult == null)
                {
                    Console.WriteLine("[PASS] Check 4: Prerequisite gating blocked underweight survivor from heavy scrap weightlifting.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: Prerequisite gating failed to block ineligible survivor.");
                }

                // Check 5: Workout execution and conditioning gains
                float preCardio = profile?.Cardio ?? 30f;
                var workoutResult = session.ExecuteRoutine("surv_boris", "routine_calisthenics_basic", currentDay: 1, intensityMultiplier: 1.0f);
                if (workoutResult != null && profile != null && profile.Cardio > preCardio)
                {
                    Console.WriteLine($"[PASS] Check 5: Calisthenics completed (+{workoutResult.CardioGain:F1} cardio, +{workoutResult.StrengthGain:F1} strength).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: Workout execution failed.");
                }

                // Check 6: Consecutive workout streak tracking
                session.ExecuteRoutine("surv_boris", "routine_calisthenics_basic", currentDay: 2);
                if (profile != null && profile.WorkoutStreak == 2 && profile.TotalWorkoutsCompleted == 2)
                {
                    Console.WriteLine($"[PASS] Check 6: Consecutive workout streak incremented to {profile.WorkoutStreak}.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: Workout streak tracking failed (streak={profile?.WorkoutStreak}).");
                }

                // Check 7: Fatigue resistance multiplier calculation
                float multiplier = session.System.GetFatigueResistanceMultiplier("surv_boris");
                if (multiplier > 0f && multiplier <= 1.25f)
                {
                    Console.WriteLine($"[PASS] Check 7: Fatigue resistance multiplier computed ({multiplier:F2}x).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: Fatigue resistance multiplier calculation failed.");
                }

                // Check 8: Workout injury check evaluation (deterministic)
                var heavyResult = session.ExecuteRoutine("surv_boris", "routine_scrap_weightlifting", currentDay: 3, intensityMultiplier: 1.5f);
                if (heavyResult != null)
                {
                    Console.WriteLine($"[PASS] Check 8: Heavy workout evaluated with injury probability check (Injury: {heavyResult.InjuryOccurred}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: Heavy workout evaluation failed.");
                }

                // Check 9: Daily deconditioning decay after inactivity (3+ inactive days)
                float preDecay = profile?.OverallConditioning ?? 30f;
                session.TickDay(10); // currentDay 10 (last workout was Day 3, so 7 inactive days)
                if (profile != null && profile.OverallConditioning < preDecay && profile.WorkoutStreak == 0)
                {
                    Console.WriteLine($"[PASS] Check 9: Deconditioning decay reduced conditioning ({preDecay:F1} -> {profile.OverallConditioning:F1}) and reset streak.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: Deconditioning decay failed.");
                }

                // Check 10: Baseline fitness floor enforcement
                session.TickDay(100);
                if (profile != null &&
                    profile.Cardio >= session.Census.BaselineFitnessFloor &&
                    profile.Strength >= session.Census.BaselineFitnessFloor)
                {
                    Console.WriteLine($"[PASS] Check 10: Fitness attributes respected baseline floor ({session.Census.BaselineFitnessFloor:F0}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: Baseline fitness floor was breached.");
                }


                // Check 11: Census reporting
                var census = session.Census;
                if (census.TrackedProfilesCount >= 2 && census.AuthoredRoutinesCount >= 3)
                {
                    Console.WriteLine($"[PASS] Check 11: Census reported {census.TrackedProfilesCount} profiles, {census.AuthoredRoutinesCount} routines (Avg Conditioning: {census.AverageConditioningScore:F1}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 11: Census reporting failed (Profiles={census.TrackedProfilesCount}).");
                }

                // Check 12: SaveStore persistence round-trip
                var captured = session.CaptureState();
                var restoreSession = ExerciseHostSession.Create();
                restoreSession.RestoreState(captured);
                var restoredCensus = restoreSession.Census;
                if (restoredCensus.TrackedProfilesCount == census.TrackedProfilesCount &&
                    Math.Abs(restoredCensus.AverageConditioningScore - census.AverageConditioningScore) < 0.01f)
                {
                    Console.WriteLine("[PASS] Check 12: SaveStore captured and restored exercise state cleanly.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: SaveStore state restore mismatch.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected exception during ExerciseSelfTest: {ex.Message}");
            }

            Console.WriteLine($"=== ExerciseSelfTest: {passed}/{total} checks passed. ===");
            return passed == total ? 0 : 1;
        }
    }
}
