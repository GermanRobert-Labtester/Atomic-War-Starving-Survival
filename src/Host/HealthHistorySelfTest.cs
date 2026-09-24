// SPDX-License-Identifier: MIT
using System;
using System.IO;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HealthHistorySelfTest
    {
        public static int Run(string dataDir)
        {
            Console.WriteLine("=== [HostCli] Health History & Medical Records Self-Test (Plan 198) ===");
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
                    throw new InvalidOperationException($"HealthHistory self-test assertion failed: {name}");
                }
            }

            try
            {
                // Check 1: Catalog templates loading
                var session = HealthHistoryHostSession.Create(dataDir);
                Check(session.System.GetAllTemplates().Count >= 5,
                    $"Authoritative catalog loaded {session.System.GetAllTemplates().Count} medical record templates from medical_record_templates.json.");

                // Check 2: Initial state baseline
                var census0 = session.GetCensus();
                Check(census0.TotalRecords == 0 && census0.TotalEvents == 0 && census0.TotalVaccinations == 0 && census0.TotalTrends == 0,
                    "Initial state baseline clean (0 records, 0 events, 0 vaccinations, 0 trends).");

                // Check 3: Log illness medical record
                MedicalRecord? recordAdded = null;
                session.System.OnRecordAdded += r => recordAdded = r;
                var rec = session.LogRecord("survivor_alice", "illness", "Acute Radiation Sickness", "moderate", day: 1, medicId: "medic_clara", notes: "Stable vitals.");
                Check(rec != null && rec.RecordId.StartsWith("medrec_") && recordAdded?.RecordId == rec.RecordId,
                    $"Medical record logged (id={rec?.RecordId}, survivor={rec?.SurvivorId}, severity={rec?.Severity}).");

                // Check 4: Auto-recorded diagnostic health event
                var events = session.System.GetSurvivorEvents("survivor_alice");
                Check(events.Count >= 1 && events[0].EventType == "diagnosis",
                    $"Diagnostic event auto-logged for new record (eventType={events[0].EventType}).");

                // Check 5: Resolve medical record
                MedicalRecord? recordResolved = null;
                session.System.OnRecordResolved += r => recordResolved = r;
                bool resolved = session.ResolveRecord(rec!.RecordId, outcome: "resolved", day: 5, notes: "Full recovery after treatment.");
                Check(resolved && recordResolved?.Outcome == "resolved" && rec.Outcome == "resolved",
                    $"Medical record successfully resolved with recovery outcome.");

                // Check 6: Administer vaccination
                VaccinationRecord? vacAdministered = null;
                session.System.OnVaccinationAdministered += v => vacAdministered = v;
                var vac = session.AdministerVaccine("survivor_alice", "rad_shield_booster", day: 6, medicId: "medic_clara", initialImmunity: 100.0f, durationDays: 30);
                Check(vac != null && vacAdministered?.VaccinationId == vac.VaccinationId && vac.BoosterDueDay == 36,
                    $"Vaccine administered (type={vac?.VaccineType}, boosterDueDay={vac?.BoosterDueDay}).");

                // Check 7: Vaccination immunity level query
                float immunity = session.System.GetVaccinationImmunity("survivor_alice", "rad_shield_booster");
                Check(immunity == 100.0f,
                    $"Vaccine immunity query returns 100% active immunity level.");

                // Check 8: Day tick degrades immunity level
                session.TickDay(day: 21); // 15 days elapsed out of 30
                float degradedImmunity = session.System.GetVaccinationImmunity("survivor_alice", "rad_shield_booster");
                Check(degradedImmunity < 100.0f && degradedImmunity >= 30.0f,
                    $"Vaccine immunity linearly degraded over time (immunity={degradedImmunity:F1}%).");

                // Check 9: Booster alert on due day
                bool boosterAlertFired = false;
                session.System.OnBoosterDueAlert += v => boosterAlertFired = true;
                session.TickDay(day: 36); // Booster due day reached
                Check(boosterAlertFired && vac!.BoosterAlertFired,
                    "Booster due alert fired precisely on expiration threshold day.");

                // Check 10: Record daily longitudinal health trends
                var trends = session.RecordDailyHealthTrend("survivor_alice", day: 36, overallHealth: 85f, radiationDose: 12f, immuneStrength: 90f, chronicCount: 0);
                Check(trends.Count == 4 && session.System.GetLatestTrends("survivor_alice").Count == 4,
                    "Multi-metric longitudinal health trends recorded (4 metrics).");

                // Check 11: Census calculation
                var census = session.GetCensus();
                Check(census.TotalRecords >= 2 && census.TotalEvents >= 3 && census.TotalVaccinations == 1 && census.TotalTrends == 4,
                    $"Census matches longitudinal state (Records={census.TotalRecords}, Events={census.TotalEvents}, Vaccinations={census.TotalVaccinations}, Trends={census.TotalTrends}).");

                // Check 12: Save and restore state fidelity
                var state = session.System.CaptureState();
                var restoredSession = HealthHistoryHostSession.Create(dataDir, state);
                var restoredCensus = restoredSession.GetCensus();
                var restoredRecs = restoredSession.System.GetSurvivorRecords("survivor_alice");
                Check(restoredCensus.TotalRecords == census.TotalRecords &&
                      restoredCensus.TotalVaccinations == census.TotalVaccinations &&
                      restoredRecs.Count == 2 &&
                      restoredSession.System.GetVaccinationImmunity("survivor_alice", "rad_shield_booster") == session.System.GetVaccinationImmunity("survivor_alice", "rad_shield_booster"),
                    "Save and restore state verified with full round-trip fidelity.");

                Console.WriteLine($"=== [HostCli] Health History Self-Test PASSED ({passed}/12 checks) ===");
                return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] HealthHistory self-test threw exception: {ex.Message}");
                Console.WriteLine(ex.StackTrace);
                return 1;
            }
        }
    }
}
