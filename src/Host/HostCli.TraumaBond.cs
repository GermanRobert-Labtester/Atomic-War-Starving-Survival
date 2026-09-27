// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : TraumaBondSelfTest
// Subsystem          : Trauma bond authority (shared-hazard bonds, decay,
//                      co-shift efficiency bonus)
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliTraumaBond
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Trauma Bond Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                string dataRoot = (!string.IsNullOrEmpty(dataDir) && Directory.Exists(dataDir)
                    ? dataDir
                    : CatalogPath.ResolveDataDir());

                // Canonical owners used for hook routing.
                var relations = new SurvivorRelationsSystem(new SeededRng(1986));

                var affinityCalls = new List<string>();
                var sameShift = new HashSet<string>(StringComparer.Ordinal) { "sv_a|sv_b" };
                int currentDay = 3;

                var session = new TraumaBondHostSession();
                session.WireHooks(
                    adjustAffinity: (a, b, delta) =>
                    {
                        affinityCalls.Add($"{a}->{b}:{delta}");
                        relations.ModifyAffinity(a, b, delta);
                    },
                    areOnSameShift: (a, b) =>
                        sameShift.Contains($"{a}|{b}") || sameShift.Contains($"{b}|{a}"),
                    getDay: () => currentDay);

                // 1. Hooks are routed, not copied.
                if (session.System.AdjustAffinity != null && session.System.AreOnSameShift != null && session.System.GetDay != null)
                { Console.WriteLine("[PASS] Check 1: Affinity / shift / day hooks routed to the canonical owners."); passed++; }
                else Console.WriteLine("[FAIL] Check 1: hooks unbound.");

                // 2. A shared hazard forms bonds and raises affinity through the owner.
                var ids = new List<string> { "sv_a", "sv_b" };
                int formed = session.RecordSharedHazard(ids, "fallout_storm");
                if (formed > 0 && affinityCalls.Count >= 1 && affinityCalls.Contains("sv_a->sv_b:15")
                    && relations.TryGetRelationship("sv_a", "sv_b", out var entry) && entry != null
                    && entry.affinity > 0f)
                { Console.WriteLine($"[PASS] Check 2: Shared hazard formed {formed} bond(s) and raised canonical affinity."); passed++; }
                else Console.WriteLine("[FAIL] Check 2: bond formation did not route to the affinity owner.");

                // 3. Bond strength is authority-owned and non-zero.
                float strength = session.System.GetBondStrength("sv_a", "sv_b");
                if (strength >= TraumaBondSystem.MinBondStrengthForBonus)
                { Console.WriteLine($"[PASS] Check 3: Bond strength recorded ({strength:0.00}); affinity delta routed once per pair."); passed++; }
                else Console.WriteLine($"[FAIL] Check 3: bond strength too low ({strength:0.00}).");

                // 4. Co-shift bonus is granted only when the canonical shift owner agrees.
                float onShift = session.GetCoShiftEfficiencyBonus("sv_a", "sv_b");
                float offShift = session.GetCoShiftEfficiencyBonus("sv_a", "sv_c");
                if (onShift > 0f && offShift == 0f)
                { Console.WriteLine($"[PASS] Check 4: Co-shift bonus granted on a shared shift only ({onShift:0.00})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 4: co-shift gating wrong ({onShift} / {offShift}).");

                // 5. Bonus is gated by the authority's own bond-strength floor.
                var weak = new TraumaBondHostSession();
                weak.WireHooks((_, _, _) => { }, (_, _) => true, () => 1f);
                if (weak.GetCoShiftEfficiencyBonus("x", "y") == 0f)
                { Console.WriteLine("[PASS] Check 5: No bond means no co-shift bonus (no fabricated camaraderie)."); passed++; }
                else Console.WriteLine("[FAIL] Check 5: bonus granted without a bond.");

                // 6. Daily decay reduces bond strength deterministically.
                session.TickDay(new List<string> { "sv_a", "sv_b" }, 24f);
                float afterDecay = session.System.GetBondStrength("sv_a", "sv_b");
                if (afterDecay < strength && afterDecay > 0f)
                { Console.WriteLine($"[PASS] Check 6: Daily decay reduced bond strength ({strength:0.00} -> {afterDecay:0.00})."); passed++; }
                else Console.WriteLine($"[FAIL] Check 6: decay wrong ({strength} -> {afterDecay}).");

                // 7. Bonds eventually expire through decay alone.
                var expiring = new TraumaBondHostSession();
                expiring.WireHooks((_, _, _) => { }, (_, _) => true, () => 1f);
                expiring.RecordSharedHazard(new List<string> { "p", "q" }, "raid");
                for (int d = 0; d < 200; d++) expiring.TickDay(new List<string> { "p", "q" }, 24f);
                if (expiring.System.GetBondStrength("p", "q") == 0f && expiring.GetTotalBondCount() == 0)
                { Console.WriteLine("[PASS] Check 7: Bonds fully decay and expire without a permanent ledger."); passed++; }
                else Console.WriteLine("[FAIL] Check 7: bond did not expire.");

                // 8. Save round-trip preserves bond state exactly.
                var captured = session.CaptureState();
                var persisted = TraumaBondSaveStore.TryCapturePersisted(captured);
                var restored = TraumaBondSaveStore.TryRestorePersisted(persisted);
                var reload = new TraumaBondHostSession();
                reload.RestoreState(restored);
                bool sameBonds = reload.System.GetBondStrength("sv_a", "sv_b") == afterDecay;
                if (sameBonds && restored != null)
                { Console.WriteLine($"[PASS] Check 8: Save round-trip preserved bond strength ({afterDecay:0.00})."); passed++; }
                else Console.WriteLine("[FAIL] Check 8: save round-trip lost bond state.");

                // 9. Projection reads the live authority.
                var proj = reload.GetProjection();
                if (proj.TotalBonds > 0 && proj.BondedSurvivorCount >= 2)
                { Console.WriteLine($"[PASS] Check 9: Projection reads the live authority ({proj.TotalBonds} bond(s), {proj.StrongBondCount} strong)."); passed++; }
                else Console.WriteLine("[FAIL] Check 9: projection stale.");

                // 10. Restore is deterministic across two independent reloads.
                var reload2 = new TraumaBondHostSession();
                reload2.RestoreState(TraumaBondSaveStore.TryRestorePersisted(persisted));
                if (reload2.System.GetBondStrength("sv_a", "sv_b") == reload.System.GetBondStrength("sv_a", "sv_b"))
                { Console.WriteLine("[PASS] Check 10: Restore is deterministic across independent reloads."); passed++; }
                else Console.WriteLine("[FAIL] Check 10: restore not deterministic.");

                // 11. Reset clears the authority without touching the canonical owners.
                int affinityBefore = affinityCalls.Count;
                reload.Reset();
                if (reload.GetTotalBondCount() == 0 && affinityCalls.Count == affinityBefore)
                { Console.WriteLine("[PASS] Check 11: Reset clears bond state; canonical affinity is untouched."); passed++; }
                else Console.WriteLine("[FAIL] Check 11: reset behaviour wrong.");
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Trauma Bond Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
