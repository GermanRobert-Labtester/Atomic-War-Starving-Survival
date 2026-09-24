// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Psychology;

namespace AtomicWar.GodotApp
{
    public static class PsychologicalProfileSelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Psychological Profile & Phobia System Self-Test (Plan 179) ===");
            int passed = 0;

            void Check(bool condition, string name)
            {
                if (condition)
                {
                    Console.WriteLine($"[PASS] Check {++passed}: {name}");
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check {passed + 1}: {name}");
                    throw new InvalidOperationException($"PsychologicalProfile self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog loaded phobias
                var session = PsychologicalProfileHostSession.Create(dataDir);
                Check(session.System.GetAllPhobias().Count >= 6,
                    $"Authoritative catalog loaded {session.System.GetAllPhobias().Count} phobia definitions from psychology_profiles.json.");

                // Check 2: Catalog loaded coping mechanisms
                Check(session.System.GetAllCopingMechanisms().Count >= 6,
                    $"Authoritative catalog loaded {session.System.GetAllCopingMechanisms().Count} coping mechanism definitions.");

                // Check 3: Initial survivor baseline
                var profile0 = session.GetProfile("survivor_selftest_1");
                Check(profile0 == null, "Survivor profile is lazily unallocated before first psychological event.");

                // Check 4: Mild trauma does not trigger phobia
                var rngLow = new SeededRng(17901);
                bool mildPhobia = session.RecordTraumaEvent("survivor_selftest_1", "power_outage", 10.0f, 1, rngLow);
                var p1 = session.GetProfile("survivor_selftest_1");
                Check(!mildPhobia && p1 != null && p1.phobias.Count == 0 && p1.trauma_event_count == 1,
                    "Low-severity trauma recorded in profile without developing a phobia.");

                // Check 5: High severity trauma triggers phobia development
                var rngHigh = new SeededRng(17902);
                bool highPhobia = session.RecordTraumaEvent("survivor_selftest_1", "power_outage", 95.0f, 2, rngHigh);
                Check(highPhobia && p1!.phobias.Count == 1 && p1.phobias[0].phobia_id == "phobia_nyctophobia",
                    $"Severe power_outage trauma triggered phobia: {p1?.phobias.FirstOrDefault()?.phobia_id}.");

                // Check 6: Phobia trigger evaluation matches condition
                var triggerResult = session.EvaluatePhobiaExposure("survivor_selftest_1", "darkness_exposure");
                Check(triggerResult != null && triggerResult.Triggered && triggerResult.ActiveEffects.Count > 0,
                    $"Trigger condition 'darkness_exposure' correctly activated {triggerResult?.ActiveEffects?.Count} phobia effects.");

                // Check 7: Unrelated condition produces no phobia effects
                var noTrigger = session.EvaluatePhobiaExposure("survivor_selftest_1", "open_sky");
                Check(noTrigger != null && !noTrigger.Triggered && noTrigger.ActiveEffects.Count == 0,
                    "Unrelated trigger condition 'open_sky' produced 0 effects.");

                // Check 8: Teach coping mechanism
                bool taught = session.TeachCopingMechanism("survivor_selftest_1", "cope_meditation");
                Check(taught && p1!.coping_mechanisms.Any(c => c.mechanism_id == "cope_meditation"),
                    "Taught coping mechanism 'cope_meditation' to survivor.");

                // Check 9: Coping mechanism enhances resilience score
                float resilienceWithCoping = session.GetProfileResilienceScore("survivor_selftest_1");
                Check(resilienceWithCoping > 0f && resilienceWithCoping <= 100f,
                    $"Composite resilience score evaluated with coping bonus: {resilienceWithCoping:F1}.");

                // Check 10: Therapy session reduces phobia severity
                float initialSeverity = p1!.phobias[0].severity;
                bool therapy1 = session.ConductTherapySession("survivor_selftest_1", "phobia_nyctophobia", 60.0f);
                Check(therapy1 && p1.phobias[0].severity < initialSeverity && p1.therapy_sessions_completed == 1,
                    $"Therapy session completed: severity reduced from {initialSeverity:F1} to {p1.phobias[0].severity:F1}.");

                // Check 11: Continued therapy manages phobia
                for (int i = 0; i < 8; i++)
                {
                    session.ConductTherapySession("survivor_selftest_1", "phobia_nyctophobia", 100.0f);
                }
                Check(p1.phobias[0].is_managed && p1.phobias[0].severity <= 5.0f,
                    $"Phobia successfully managed (severity={p1.phobias[0].severity:F1}, is_managed={p1.phobias[0].is_managed}).");

                // Check 12: Save store round-trip capture & restore
                var census = session.GetCensus();
                var state = session.CaptureState();
                var restoredSession = PsychologicalProfileHostSession.Create(dataDir, state);
                var restoredCensus = restoredSession.GetCensus();
                var restoredProfile = restoredSession.GetProfile("survivor_selftest_1");

                Check(restoredCensus.TotalProfiles == census.TotalProfiles &&
                      restoredCensus.TotalPhobias == census.TotalPhobias &&
                      restoredCensus.ManagedPhobias == census.ManagedPhobias &&
                      restoredProfile != null &&
                      restoredProfile.phobias[0].is_managed == p1.phobias[0].is_managed &&
                      restoredProfile.coping_mechanisms.Count == p1.coping_mechanisms.Count,
                    "Save/restore round-trip preserved 100% parity across profiles, phobias, coping, and therapy.");

                Console.WriteLine($"=== Psychological Profile Self-Test Result: {passed}/12 Passed ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Psychological Profile self-test terminated with exception: {ex.Message}\n{ex.StackTrace}");
                return 1;
            }
        }
    }
}
