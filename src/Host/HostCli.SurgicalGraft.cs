// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SurgicalGraftSelfTest
// Subsystem          : PLAN-SURGICAL-WARD-TRUTH-213 — Ward grafts
// ============================================================================
using System;
using System.Linq;
using Ashfall.Core.Medical;

namespace AtomicWar.GodotApp
{
    public static class HostCliSurgicalGraft
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Surgical Graft Rejection Self-Test (PLAN-SURGICAL-WARD-TRUTH-213) ===");
            int passed = 0;
            const int total = 10;

            try
            {
                var session = new SurgicalGraftHostSession();

                // 1 placement
                var graft = session.RecordGraft("surv_a", "left_arm", "donor_1", GraftBiocompatibilityTier.AllograftUnmatched, 1);
                if (graft != null && session.Engine.Grafts.Count == 1) { Console.WriteLine("[PASS] Check 1: Graft placed."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: graft placement failed.");

                // 2 daily integration progress advances
                int before = graft!.IntegrationProgressPermille;
                session.TickDay(2, (id, risk) => 999); // high roll = no rejection
                if (graft.IntegrationProgressPermille > before) { Console.WriteLine("[PASS] Check 2: Daily tick advanced integration progress."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: integration progress did not advance.");

                // 3 immunosuppressant raises level
                graft.ImmunosuppressantLevelPermille = 100; // driven low to prove dosing
                bool dose = session.AdministerImmunosuppressant(graft.GraftId, 400, 2);
                if (dose && graft.ImmunosuppressantLevelPermille == 500) { Console.WriteLine("[PASS] Check 3: Immunosuppressant administered (100+400)."); passed++; }
                else Console.WriteLine($"[FAIL] Check 3: immunosuppressant failed (level={graft.ImmunosuppressantLevelPermille}, dose={dose}).");

                // 4 immunosuppressant decay on tick
                session.TickDay(3, (id, risk) => 999);
                if (graft.ImmunosuppressantLevelPermille < 500) { Console.WriteLine("[PASS] Check 4: Immunosuppressant decayed on tick."); passed++; }
                else Console.WriteLine("[FAIL] Check 4: immunosuppressant did not decay.");

                // 5 low immunosuppressant + bad roll -> rejection risk / rejection
                bool rejected = false;
                session.Engine.OnGraftRejected += _ => rejected = true;
                for (int d = 4; d < 40 && !rejected; d++) session.TickDay(d, (id, risk) => 0);
                if (rejected || session.Engine.Grafts.Any(g => g.Status == GraftStatus.Rejected))
                { Console.WriteLine("[PASS] Check 5: Untreated graft eventually rejected under low immunosuppression."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: graft never rejected.");

                // 6 determinism: same seed/rolls produce same result
                var a = new SurgicalGraftHostSession(); a.RecordGraft("s", "l", "d", GraftBiocompatibilityTier.AllograftMatched, 1);
                var b = new SurgicalGraftHostSession(); b.RecordGraft("s", "l", "d", GraftBiocompatibilityTier.AllograftMatched, 1);
                for (int d = 2; d < 12; d++) { a.TickDay(d, (i, r) => 500); b.TickDay(d, (i, r) => 500); }
                if (a.Engine.Grafts[0].IntegrationProgressPermille == b.Engine.Grafts[0].IntegrationProgressPermille
                    && a.Engine.Grafts[0].Status == b.Engine.Grafts[0].Status)
                { Console.WriteLine("[PASS] Check 6: Same rolls produce identical graft state (deterministic)."); passed++; }
                else Console.WriteLine("[FAIL] Check 6: determinism broken.");

                // 7 snapshot
                var snap = a.GetSnapshot();
                if (snap.TotalGrafts == 1) { Console.WriteLine("[PASS] Check 7: Snapshot projection reads live grafts."); passed++; }

                // 8 save/restore round-trip
                var saved = a.CaptureState();
                var restored = new SurgicalGraftHostSession();
                restored.RestoreState(saved);
                if (restored.Engine.Grafts.Count == a.Engine.Grafts.Count
                    && restored.Engine.Grafts[0].Status == a.Engine.Grafts[0].Status
                    && restored.Engine.Grafts[0].IntegrationProgressPermille == a.Engine.Grafts[0].IntegrationProgressPermille)
                { Console.WriteLine("[PASS] Check 8: Save/restore round-trip preserved graft records."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: restore round-trip broken.");

                // 9 replay guard: no same-day double integration
                int progressAfterRestore = restored.Engine.Grafts[0].IntegrationProgressPermille;
                restored.TickDay(restored.Engine.Grafts[0].LastUpdateDay, (i, r) => 500);
                if (restored.Engine.Grafts[0].IntegrationProgressPermille == progressAfterRestore)
                { Console.WriteLine("[PASS] Check 9: Same-day replay does not double-advance."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: same-day replay advanced state.");

                // 10 contract names (local bool keeps the drift guard reachable)
                bool surgicalContractOk = SurgicalGraftSaveStore.SectionName == "surgical_graft"
                    && SurgicalGraftSaveStore.FileName == "surgical_graft_save.json";
                if (surgicalContractOk)
                { Console.WriteLine("[PASS] Check 10: Save store contract names verified."); passed++; }
                else Console.WriteLine("[FAIL] Check 10: save store contract names wrong.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}"); }

            Console.WriteLine($"=== Surgical Graft Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
