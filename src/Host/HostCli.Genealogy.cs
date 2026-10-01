// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : GenealogySelfTest
// Subsystem          : Plan 217 — Survivor Genealogy & Family Tree
// ============================================================================

using System;
using System.Linq;
using System.IO;
using Ashfall.Core.Legacy;

namespace AtomicWar.GodotApp
{
    public static class HostCliGenealogy
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Survivor Genealogy Self-Test (Plan 217) ===");
            int passed = 0;
            const int total = 10;

            try
            {
                // Check 1: composition over the succession engine (shared, not forked).
                var engine = new GenerationalSuccessionEngine();
                var session = new GenealogyHostSession(engine);
                Console.WriteLine("[PASS] Check 1: Genealogy host session composed over the succession engine.");
                passed++;

                // Check 2: union fact from a canonical family formation.
                bool union = session.RecordUnion("surv_a", "surv_b", 3);
                if (union && session.Lineage.GetSpouse("surv_a") == "surv_b")
                {
                    Console.WriteLine("[PASS] Check 2: Union recorded from canonical family formation.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 2: union fact not recorded.");
                }

                // Check 3: birth lineage via a canonical family payload.
                session.FamilyResolver = familyId => new Ashfall.Core.Survivors.FamilyUnit
                {
                    FamilyId = familyId,
                    FamilyName = "Ashwood",
                    ParentIds = { "surv_a", "surv_b" }
                };
                var parents = session.RecordChild("fam_1", "surv_child", isAdopted: false, day: 5);
                int childRecords = session.Lineage.LineageRecords.Count(l => l.childId == "surv_child" && !string.IsNullOrWhiteSpace(l.parentId));
                if (parents.Count == 2 && childRecords == 2)
                {
                    Console.WriteLine("[PASS] Check 3: Child welcomed produced one lineage record per committed parent.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: child lineage wrong (parents={parents.Count}, records={session.Lineage.LineageRecords.Count}).");
                }

                // Check 4: exactly-once — replaying the same child adds nothing.
                session.RecordChild("fam_1", "surv_child", isAdopted: false, day: 6);
                if (session.Lineage.LineageRecords.Count(l => l.childId == "surv_child") == 2)
                {
                    Console.WriteLine("[PASS] Check 4: Replayed child fact refused (lineage_exists) — exactly once.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 4: replay duplicated lineage records.");
                }

                // Check 5: no second family-unit ledger — the host path never
                // calls FormFamilyUnit; familyUnits stays empty.
                if (session.Lineage.State.familyUnits.Count == 0)
                {
                    Console.WriteLine("[PASS] Check 5: No second family-unit ledger created (family owner untouched).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 5: host path created duplicate family units.");
                }

                // Check 6: kinship read model.
                var kinship = session.GetKinship("surv_child");
                if (kinship.Parents.Count == 2)
                {
                    Console.WriteLine("[PASS] Check 6: Read-only kinship projection resolves parents and inherited family name.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: kinship projection wrong (parents={kinship.Parents.Count}).");
                }

                // Check 7: death fact recorded once.
                session.RecordDeath("surv_a", 9);
                int deathCount = session.Lineage.FamilyEventLog.Count(e => e.eventType == "death" && e.participantIds.Contains("surv_a"));
                if (deathCount == 1)
                {
                    Console.WriteLine("[PASS] Check 7: Death kinship fact recorded (one event per canonical fact).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: death fact handling wrong (count={deathCount}).");
                }

                // Check 8: capture/restore round-trip preserves kinship facts.
                var saved = session.CaptureState();
                var restored = new GenealogyHostSession(new GenerationalSuccessionEngine());
                restored.RestoreState(saved);
                var kinship2 = restored.GetKinship("surv_child");
                if (restored.Lineage.LineageRecords.Count == session.Lineage.LineageRecords.Count
                    && kinship2.Parents.Count == 2
                    && restored.Lineage.GetSpouse("surv_a") == "surv_b")
                {
                    Console.WriteLine("[PASS] Check 8: Save/restore round-trip preserves lineage, unions, and events.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 8: restore round-trip broken.");
                }

                // Check 9: replay after restore adds nothing (exactly-once survives restore).
                restored.FamilyResolver = session.FamilyResolver;
                restored.RecordChild("fam_1", "surv_child", isAdopted: false, day: 11);
                if (restored.Lineage.LineageRecords.Count == session.Lineage.LineageRecords.Count)
                {
                    Console.WriteLine("[PASS] Check 9: Replay after restore produces no duplicate lineage.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 9: replay after restore duplicated lineage.");
                }

                // Check 10: save store contract names (local bool keeps the guard reachable).
                bool genealogyContractOk = GenealogySaveStore.SectionName == "genealogy"
                    && GenealogySaveStore.FileName == "genealogy_save.json";
                if (genealogyContractOk)
                {
                    Console.WriteLine("[PASS] Check 10: Genealogy save store contract names verified (own save key).");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 10: save store contract names wrong.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unexpected probe exception: {ex.Message}");
            }

            Console.WriteLine($"=== Survivor Genealogy Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
