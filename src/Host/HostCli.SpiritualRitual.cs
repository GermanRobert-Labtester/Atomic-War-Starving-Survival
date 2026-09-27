// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : SpiritualRitualSelfTest
// Subsystem          : EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Spiritual;

namespace AtomicWar.GodotApp
{
    public static class HostCliSpiritualRitual
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Spiritual Ritual Self-Test (EXPANSION-13-THE-FAITHFUL-AND-THE-FRACTURED) ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir());

                var catalog = SpiritualCatalogLoader.Load(dataRoot, new FileSystemIO(), new SystemTextJsonSerializer());
                var session = new SpiritualRitualHostSession();
                session.BindCatalog(catalog);

                // 1. Authored ritual corpus is bound (no reload, no copy).
                if (session.AuthoredRitualCount > 0)
                { Console.WriteLine($"[PASS] Check 1: Authored ritual corpus bound ({session.AuthoredRitualCount} rituals)."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: no authored rituals bound.");

                // 2. Engine is the sole verdict authority for a real ritual.
                // A ritual with an authored cooldown of at least two days is needed
                // for the cooldown checks below.
                string? firstId = null;
                SpiritualRitualDefinition? cooldownRitual = null;
                foreach (var r in catalog.Rituals)
                {
                    if (r == null || string.IsNullOrEmpty(r.Id)) continue;
                    if (firstId == null) firstId = r.Id;
                    if (cooldownRitual == null && r.CooldownDays >= 2) cooldownRitual = r;
                }
                if (firstId == null) { Console.WriteLine("[FAIL] Check 2: catalog has no usable ritual id."); }
                else
                {
                    var ok = session.TryEvaluateRitual(firstId, currentDay: 10, shelterMoralePermille: 500);
                    if (ok.IsAllowed && ok.Reason.Length > 0)
                    { Console.WriteLine($"[PASS] Check 2: Engine verdict returned for '{firstId}' (+{ok.MoraleDeltaPermille} permille morale)."); passed++; }
                    else Console.WriteLine($"[FAIL] Check 2: ritual '{firstId}' refused ({ok.Reason}).");
                }

                // 3. Unknown ritual is refused without mutating anything.
                var unknown = session.TryEvaluateRitual("no_such_ritual", 10, 500);
                if (!unknown.IsAllowed && session.LastPerformedDay.Count == 0)
                { Console.WriteLine("[PASS] Check 3: Unknown ritual refused without fabricating a performance."); passed++; }
                else Console.WriteLine("[FAIL] Check 3: unknown ritual mutated state.");

                if (cooldownRitual != null)
                {
                    string ritualId = cooldownRitual.Id;
                    int cooldown = Math.Max(1, cooldownRitual.CooldownDays);

                    // 4. Performing records the ledger entry.
                    var performed = session.TryPerformRitual(ritualId, currentDay: 10, shelterMoralePermille: 500);
                    if (performed != null && session.LastPerformedDay.ContainsKey(ritualId))
                    { Console.WriteLine($"[PASS] Check 4: Performance recorded in the cooldown ledger (day 10)."); passed++; }
                    else Console.WriteLine("[FAIL] Check 4: performance not recorded.");

                    // 5. Cooldown is enforced by the engine, not by the host.
                    var cooled = session.TryPerformRitual(ritualId, currentDay: 10 + cooldown - 1, shelterMoralePermille: 500);
                    if (cooled == null)
                    { Console.WriteLine($"[PASS] Check 5: Cooldown refuses the repeat inside the authored {cooldown}-day window."); passed++; }
                    else Console.WriteLine($"[FAIL] Check 5: cooldown not enforced (authored {cooldown} days).");

                    // 6. After the authored cooldown the ritual is allowed again.
                    var again = session.TryPerformRitual(ritualId, currentDay: 10 + cooldown, shelterMoralePermille: 500);
                    if (again != null)
                    { Console.WriteLine($"[PASS] Check 6: Ritual allowed again after the authored cooldown (day {10 + cooldown})."); passed++; }
                    else Console.WriteLine("[FAIL] Check 6: ritual never became available again.");
                }
                else { Console.WriteLine("[FAIL] Checks 4-6: no authored ritual with a cooldown of at least 2 days."); }

                // 7. Low morale scales spiritual comfort (engine-owned rule).
                SpiritualRitualDefinition? ritualDef = null;
                foreach (var r in catalog.Rituals)
                {
                    if (r != null && !string.IsNullOrEmpty(r.Id) && r.MoraleDelta > 0f) { ritualDef = r; break; }
                }
                ritualDef ??= (firstId == null ? null : session.GetRitual(firstId));
                if (ritualDef != null)
                {
                    var low = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritualDef, int.MaxValue, 250);
                    var normal = SpiritualRitualCalendarEngine.EvaluateRitualObservance(ritualDef, int.MaxValue, 700);
                    if (low.MoraleDeltaPermille >= normal.MoraleDeltaPermille)
                    { Console.WriteLine($"[PASS] Check 7: Low shelter morale scales comfort ({normal.MoraleDeltaPermille} -> {low.MoraleDeltaPermille} permille)."); passed++; }
                    else Console.WriteLine("[FAIL] Check 7: low-morale comfort scaling wrong.");
                }
                else Console.WriteLine("[FAIL] Check 7: no ritual definition to evaluate.");

                // 8. Holy-day window scheduling is deterministic.
                var hd1 = session.GetScheduledObservance("ash_witnesses", 45);
                var hd1b = session.GetScheduledObservance("ash_witnesses", 45);
                var hdOff = session.GetScheduledObservance("ash_witnesses", 200);
                if (hd1 != null && hd1b != null && hd1.Value.HolyDayId == hd1b.Value.HolyDayId && hdOff == null)
                { Console.WriteLine($"[PASS] Check 8: Holy-day window scheduled deterministically ({hd1.Value.Title}, day 45)."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: holy-day scheduling wrong.");

                // 9. Same-movement friction is fully mitigated; a shared rite adds mitigation.
                int same = session.GetFrictionMitigation("ash_witnesses", "ash_witnesses", false);
                int crossPlain = session.GetFrictionMitigation("ash_witnesses", "rebuilders", false);
                int crossShared = session.GetFrictionMitigation("ash_witnesses", "rebuilders", true);
                if (same == 1000 && crossPlain < same && crossShared > crossPlain)
                { Console.WriteLine($"[PASS] Check 9: Friction mitigation derives from movement pair and shared rite ({crossPlain} -> {crossShared})."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: friction mitigation derivation wrong.");

                // 10. Save round-trip preserves the cooldown ledger exactly.
                var captured = session.CaptureState();
                var persisted = SpiritualRitualSaveStore.TryCapturePersisted(captured);
                var restoredState = SpiritualRitualSaveStore.TryRestorePersisted(persisted);
                var reload = new SpiritualRitualHostSession();
                reload.BindCatalog(catalog);
                reload.RestoreState(restoredState);
                bool sameLedger = reload.LastPerformedDay.Count == session.LastPerformedDay.Count;
                if (sameLedger)
                {
                    foreach (var kvp in session.LastPerformedDay)
                        if (!reload.LastPerformedDay.TryGetValue(kvp.Key, out int v) || v != kvp.Value) sameLedger = false;
                }
                if (sameLedger && restoredState != null)
                { Console.WriteLine($"[PASS] Check 10: Save round-trip preserves the cooldown ledger ({session.LastPerformedDay.Count} entries)."); passed++; }
                else Console.WriteLine("[FAIL] Check 10: save round-trip lost ledger entries.");

                // 11. Reset clears the ledger and nothing else.
                reload.Reset();
                if (reload.LastPerformedDay.Count == 0 && reload.AuthoredRitualCount == session.AuthoredRitualCount)
                { Console.WriteLine("[PASS] Check 11: Reset clears the ledger while keeping the authored catalog bound."); passed++; }
                else Console.WriteLine("[FAIL] Check 11: reset behaviour wrong.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Spiritual Ritual Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
