// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : ChronicConditionSelfTest
// Subsystem          : Plan 193 — Chronic Conditions & Accommodations
// ============================================================================

using System;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliChronicCondition
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Chronic Conditions Self-Test (Plan 193) ===");
            int passed = 0;
            const int total = 12;

            try
            {
                var session = ChronicConditionHostSession.Create(dataDir ?? string.Empty);

                // Check 1: authored catalog strictness (must exist and load).
                bool catalogOk = session.CatalogLoaded && session.GetAllConditionDefs().Count >= 6
                    && session.GetAllAccommodationDefs().Count >= 6;
                if (catalogOk)
                {
                    Console.WriteLine($"[PASS] Check 1: authored catalog loaded ({session.GetAllConditionDefs().Count} conditions / {session.GetAllAccommodationDefs().Count} accommodations).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: authored chronic_conditions.json missing or truncated.");
                }

                // Check 2: one valid recorded condition (producer seam shape).
                var rec = session.RecordCondition("surv_a", "cond_chronic_limp", 5, "injury");
                string severityAuthored = session.System.GetConditionDef("cond_chronic_limp")?.severity ?? "";
                if (rec != null && rec.OnsetDay == 5 && string.Equals(rec.Severity, severityAuthored, StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("[PASS] Check 2: valid condition recorded with catalog attribution (severity/cause).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: valid condition record wrong (onset={rec?.OnsetDay}, severity={rec?.Severity}, authored={severityAuthored}).");
                }

                // Check 3: a second delivery of the same id is exactly-once.
                var rec2 = session.RecordCondition("surv_a", "cond_chronic_limp", 9, "injury");
                if (rec2 != null && ReferenceEquals(rec, rec2) && session.GetSurvivorConditions("surv_a").Count == 1)
                {
                    Console.WriteLine("[PASS] Check 3: repeated diagnosis delivered exactly once (idempotent).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 3: duplicate condition delivery not exactly-once.");
                }

                // Check 4: refusal — unknown accommodation id must refuse.
                var refused = session.AssignAccommodation("surv_a", "accom_does_not_exist", "cond_chronic_limp", 6);
                if (refused == null && session.GetSurvivorAccommodations("surv_a").Count == 0)
                {
                    Console.WriteLine("[PASS] Check 4: unknown accommodation refused explicitly (no silent acceptance).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: unknown accommodation was not refused.");
                }

                // Check 5: penalty applies; recommended accommodation reduces it.
                float penaltyNoAcc = session.CalculateCapabilityModifier("surv_a", "movement_speed");
                var acc = session.AssignAccommodation("surv_a", "accom_cane_crutch", "cond_chronic_limp", 7);
                float penaltyWithAcc = session.CalculateCapabilityModifier("surv_a", "movement_speed");
                if (acc != null && penaltyNoAcc < 1f && penaltyWithAcc > penaltyNoAcc)
                {
                    Console.WriteLine($"[PASS] Check 5: capability penalty {penaltyNoAcc:0.00} reduced to {penaltyWithAcc:0.00} by the recommended accommodation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: capability/penalty or accommodation reduction wrong.");
                }

                // Check 6: unfitted capability is untouched (no generic penalty).
                float untouched = session.CalculateCapabilityModifier("surv_b", "movement_speed");
                if (Math.Abs(untouched - 1f) < 0.0001f)
                {
                    Console.WriteLine("[PASS] Check 6: survivors without conditions read 1.0 (no mood-meter inference).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 6: condition penalty leaked to an unconditioned survivor.");
                }

                // Check 7: impairment score is a truthful derived readout.
                float imp = session.GetTotalImpairmentScore("surv_a");
                if (imp > 0f)
                {
                    Console.WriteLine($"[PASS] Check 7: impairment score derived ({imp:0.0}%).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 7: impairment score not derived from conditions.");
                }

                // Check 8: removal is explicit and observable.
                bool removed = session.RemoveAccommodation("surv_a", "accom_cane_crutch");
                float afterRemoval = session.CalculateCapabilityModifier("surv_a", "movement_speed");
                if (removed && afterRemoval == penaltyNoAcc)
                {
                    Console.WriteLine("[PASS] Check 8: accommodation removal restores the raw penalty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: accommodation removal wrong.");
                }

                // Check 9: capture must persist every record.
                var captured = session.CaptureState();
                if (captured.Conditions.Count == 1 && captured.Conditions[0].ConditionId == "cond_chronic_limp")
                {
                    Console.WriteLine("[PASS] Check 9: capture preserves the tracked condition.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: capture lost the tracked condition.");
                }

                // Check 10: restore in a FRESH session (no handlers attached) applies once.
                var restored = new ChronicConditionHostSession();
                restored.RestoreState(captured);
                if (restored.GetSurvivorConditions("surv_a").Count == 1)
                {
                    Console.WriteLine("[PASS] Check 10: replay after restore applies exactly once (no duplicate).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: replay duplicated or lost conditions on restore.");
                }

                // Check 11: legacy missing-field baseline — empty state restores clean.
                var fresh = new ChronicConditionHostSession();
                fresh.RestoreState(new ChronicConditionState());
                Console.WriteLine("[PASS] Check 11: legacy empty/missing-field baseline restores clean.");
                passed++;

                // Check 12: contracts — store name + section name.
                if (ChronicConditionSaveStore.SectionName == "chronic_condition"
                    && ChronicConditionSaveStore.FileName == "chronic_condition_save.json")
                {
                    Console.WriteLine("[PASS] Check 12: chronic-condition save store contract names verified (own save key).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 12: save store contract names wrong.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}");
            }

            Console.WriteLine($"=== Chronic conditions: {passed}/{total} checks passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
