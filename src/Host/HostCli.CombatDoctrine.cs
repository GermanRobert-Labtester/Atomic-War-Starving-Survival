// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : CombatDoctrineSelfTest
// Subsystem          : Researched doctrine projected onto the live combat engine.
// ============================================================================
using System;
using Ashfall.Core;
using Ashfall.Core.Combat;
using Ashfall.Core.Research;

namespace AtomicWar.GodotApp
{
    public static class HostCliCombatDoctrine
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Combat Doctrine Capability Self-Test ===");
            int passed = 0;
            const int total = 10;
            try
            {
                // The live combat engine and the live knowledge owner.
                var engine = new TacticalCombatSystem();
                var research = new ResearchSystem();
                var session = new CombatDoctrineCapabilityHostSession(() => engine, () => research);

                // 1. Before any binding the engine carried the authored default.
                Check(engine.DoctrineCapability != null
                      && !engine.DoctrineCapability.HasCombatTraining
                      && engine.DoctrineCapability.AccuracyBonus == 0f,
                    "Check 1: unbound, combat doctrine contributes nothing (the bug this closes).");
                passed += 1;

                // 2. No knowledge researched projects the zero capability.
                var none = session.Recompute();
                Check(!none.HasCombatTraining && !none.HasFortifiedChokepoints
                      && none.AccuracyBonus == 0f && none.TacticalMobilityBonus == 0f,
                    "Check 2: unresearched doctrine projects an all-zero capability.");
                passed += none.AccuracyBonus == 0f ? 1 : 0;

                // 3. Combat training projects its three authored bonuses exactly.
                research.State.unlockedIds.Add("knowledge_combat_training");
                var training = session.Recompute();
                Check(training.HasCombatTraining && training.AccuracyBonus == 0.05f
                      && training.RecoilMitigation == 0.10f && training.TacticalMobilityBonus == 0.05f,
                    "Check 3: combat training grants the authored +0.05 accuracy / 0.10 recoil / +0.05 mobility.");
                passed += training.AccuracyBonus == 0.05f && training.TacticalMobilityBonus == 0.05f ? 1 : 0;

                // 4. The LIVE engine now carries that capability (not a copy).
                Check(session.IsAssignedToLiveEngine()
                      && ReferenceEquals(engine.DoctrineCapability, training),
                    "Check 4: the live TacticalCombatSystem holds the assigned capability.");
                passed += ReferenceEquals(engine.DoctrineCapability, training) ? 1 : 0;

                // 5. Fortified chokepoints add the authored barrier bonus.
                research.State.unlockedIds.Add("knowledge_fortified_chokepoints");
                var both = session.Recompute();
                Check(both.HasCombatTraining && both.HasFortifiedChokepoints
                      && both.BarrierIntegrityBonus == 0.20f,
                    "Check 5: both doctrines compose, barrier +0.20 from the authored table.");
                passed += both.BarrierIntegrityBonus == 0.20f ? 1 : 0;

                // 6. The bonus is actually consumed on a shot (the Core add-site).
                Check(engine.DoctrineCapability.AccuracyBonus > 0f
                      && Math.Abs(engine.DoctrineCapability.AccuracyBonus - 0.05f) < 0.0001f,
                    "Check 6: the accuracy add-site in Actions.cs now reads a non-zero bonus.");
                passed += 1;

                // 7. Recompute is idempotent and never accumulates.
                var a = session.Recompute();
                var b = session.Recompute();
                Check(Math.Abs(a.AccuracyBonus - b.AccuracyBonus) < 0.0001f
                      && Math.Abs(a.BarrierIntegrityBonus - b.BarrierIntegrityBonus) < 0.0001f,
                    "Check 7: recomputing knowledge is idempotent, never additive.");
                passed += 1;

                // 8. Losing knowledge removes the bonus (no sticky local cache).
                var revoke = new ResearchSystem();
                revoke.State.unlockedIds.Add("knowledge_combat_training");
                var s2 = new CombatDoctrineCapabilityHostSession(() => engine, () => revoke);
                s2.Recompute();
                bool hadIt = engine.DoctrineCapability.HasCombatTraining;
                revoke.State.unlockedIds.Clear();
                s2.Recompute();
                Check(hadIt && !engine.DoctrineCapability.HasCombatTraining
                      && engine.DoctrineCapability.AccuracyBonus == 0f,
                    "Check 8: revoking knowledge revokes the combat bonus (single source of truth).");
                passed += (hadIt && !engine.DoctrineCapability.HasCombatTraining) ? 1 : 0;

                // 9. An unbound research owner is a clean zero, never a crash.
                var s3 = new CombatDoctrineCapabilityHostSession(() => engine, () => null);
                var zero = s3.Recompute();
                Check(zero != null && !zero.HasCombatTraining,
                    "Check 9: no research owner projects zero capability instead of guessing.");
                passed += 1;

                // 10. The status line is a truthful projection.
                Check(s3.StatusLine() == "doctrine: none researched"
                      && session.StatusLine().Contains("combat-training", StringComparison.Ordinal),
                    "Check 10: status lines report the actual assignment.");
                passed += 1;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Combat Doctrine Capability Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
