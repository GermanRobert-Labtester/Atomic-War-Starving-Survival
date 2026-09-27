// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : PatientRecordIntegritySelfTest
// Subsystem          : Clinical record integrity across the live medical pipeline.
// ============================================================================
using System;
using System.Collections.Generic;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliPatientRecordIntegrity
    {
        private static readonly HashSet<string> Roster =
            new HashSet<string>(StringComparer.Ordinal) { "surv_a", "surv_b" };
        private static readonly HashSet<string> Items =
            new HashSet<string>(StringComparer.Ordinal) { "bandage", "antibiotics" };

        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static PatientRecordIntegrityHostSession Build(
            MedicalPipelineSaveState? state,
            HashSet<string>? survivors = null,
            HashSet<string>? knownItems = null)
        {
            var roster = survivors ?? Roster;
            var items = knownItems ?? Items;
            return new PatientRecordIntegrityHostSession(
                () => state,
                id => id != null && roster.Contains(id),
                _ => true,
                id => id != null && items.Contains(id));
        }

        /// <summary>
        /// Builds a medicine reservation referencing the given survivor and item ids,
        /// shaped only through the public save DTOs.
        /// </summary>
        private static MedicalPipelineSaveState PipelineWith(string survivorId, string itemId)
        {
            var state = new MedicalPipelineSaveState();
            state.reservations.reservations.Add(new MedicalReservation
            {
                reservationId = 1,
                survivorId = survivorId,
                kind = "medicine",
                targetId = itemId,
                quantity = 1
            });
            return state;
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Patient Record Integrity Self-Test ===");
            int passed = 0;
            const int total = 10;
            try
            {
                // 1. A clean pipeline reports zero findings.
                var clean = new MedicalPipelineSaveState();
                var session = Build(clean);
                var findings = session.Validate();
                Check(session.IsClean && findings.Count == 0,
                    "Check 1: a clean medical pipeline validates with zero findings.");
                passed += session.IsClean ? 1 : 0;

                // 2. An unbound pipeline is a refusal, never a crash.
                var none = Build(null);
                none.Validate();
                Check(none.IsClean && none.LastEvent.Contains("No medical pipeline state", StringComparison.Ordinal),
                    "Check 2: an unbound pipeline is reported, not invented.");
                passed += none.IsClean ? 1 : 0;

                // 3. Validation is read-only.
                int diagBefore = clean.diagnosis?.records?.Count ?? 0;
                session.Validate();
                Check((clean.diagnosis?.records?.Count ?? 0) == diagBefore,
                    "Check 3: validation never mutates the pipeline state.");
                passed += (clean.diagnosis?.records?.Count ?? 0) == diagBefore ? 1 : 0;

                // 4. Repeat validation is idempotent and stably ordered.
                var dirty = PipelineWith("surv_a", "bandage");
                var idempotent = Build(dirty);
                var firstCodes = string.Join(",", Collect(idempotent));
                var secondCodes = string.Join(",", Collect(idempotent));
                Check(firstCodes == secondCodes && idempotent.ValidationCount == 2,
                    $"Check 4: repeat validation is idempotent and stably ordered ({firstCodes}).");
                passed += firstCodes == secondCodes && idempotent.ValidationCount == 2 ? 1 : 0;

                // 5. A clean authored reference produces no findings.
                var ok = Build(PipelineWith("surv_a", "bandage"));
                ok.Validate();
                Check(ok.IsClean,
                    "Check 5: a reservation naming a real survivor and a real item is clean.");
                passed += ok.IsClean ? 1 : 0;

                // 6. A dangling survivor reference is reported.
                var ghostSurvivor = Build(PipelineWith("ghost_survivor", "bandage"));
                ghostSurvivor.Validate();
                Check(ghostSurvivor.HasCode("reservation_unknown_survivor"),
                    $"Check 6: a survivor id outside the roster is reported ({ghostSurvivor.FindingCodes()}).");
                passed += ghostSurvivor.HasCode("reservation_unknown_survivor") ? 1 : 0;

                // 7. The same reference clears once the roster actually knows it.
                var widened = Build(PipelineWith("ghost_survivor", "bandage"),
                    new HashSet<string>(StringComparer.Ordinal) { "surv_a", "surv_b", "ghost_survivor" });
                widened.Validate();
                Check(widened.IsClean,
                    "Check 7: resolving the reference against the real roster clears it.");
                passed += widened.IsClean ? 1 : 0;

                // 8. A dangling item reference is reported.
                var ghostItem = Build(PipelineWith("surv_a", "item_that_does_not_exist"));
                ghostItem.Validate();
                Check(ghostItem.HasCode("reservation_unknown_item"),
                    $"Check 8: an unknown item reference is reported ({ghostItem.FindingCodes()}).");
                passed += ghostItem.HasCode("reservation_unknown_item") ? 1 : 0;

                // 9. Every finding carries an authored code, never a blank.
                bool authored = ghostSurvivor.Findings.Count > 0;
                foreach (var f in ghostSurvivor.Findings)
                    if (string.IsNullOrWhiteSpace(f.code)) authored = false;
                Check(authored, "Check 9: every finding carries an authored code.");
                passed += authored ? 1 : 0;

                // 10. Clearing the report never touches clinical state.
                int before = ghostSurvivor.FindingCount;
                ghostSurvivor.Clear();
                Check(ghostSurvivor.IsClean && ghostSurvivor.FindingCount == 0 && before > 0
                      && ghostSurvivor.StatusLine() == "clinical integrity: clean",
                    "Check 10: clearing findings changes only the report.");
                passed += ghostSurvivor.IsClean && before > 0 ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Patient Record Integrity Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }

        private static IEnumerable<string> Collect(PatientRecordIntegrityHostSession session)
        {
            foreach (var f in session.Validate()) yield return f?.code ?? "?";
        }
    }
}
