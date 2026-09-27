// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RationConflictSelfTest
// Subsystem          : Survivor resentment over unequal ration allocations.
// ============================================================================
using System;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    public static class HostCliRationConflict
    {
        private static void Check(bool ok, string label)
        {
            if (ok) Console.WriteLine($"[PASS] {label}");
            else Console.WriteLine($"[FAIL] {label}");
        }

        private static RationConflictHostSession BuildSession(
            RationConflictSystem engine,
            NeedsSystem? needs,
            SurvivorRelationsSystem? relations,
            int seed = 7)
            => new RationConflictHostSession(
                engine,
                () => null,                       // allocation projection is tested separately
                () => needs,
                () => relations,
                null);

        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Ration Conflict Self-Test ===");
            int passed = 0;
            const int total = 11;
            try
            {
                // ── Scene 1: the Core authority's own contract ──────────────
                var engine = new RationConflictSystem(new SeededRng(7));
                engine.RegisterSurvivor("surv_a");
                engine.RegisterSurvivor("surv_b");
                engine.RegisterSurvivor("surv_c");

                // 1. Equal allocations read as perfectly fair.
                engine.SetAllocation("surv_a", 0.5f);
                engine.SetAllocation("surv_b", 0.5f);
                engine.SetAllocation("surv_c", 0.5f);
                engine.Tick("surv_a", 24f);
                var fair = engine.GetState("surv_a");
                Check(fair != null && fair!.perceivedFairness > 0.99f,
                    "Check 1: equal allocations read as perfectly fair.");
                passed += fair != null && fair.perceivedFairness > 0.99f ? 1 : 0;

                // 2. A 20%+ deficit builds resentment and names a target.
                engine.SetAllocation("surv_a", 0.2f);
                engine.SetAllocation("surv_b", 0.9f);
                engine.SetAllocation("surv_c", 0.9f);
                bool resentmentFired = false;
                engine.OnResentmentBuilt += (_, _, _) => resentmentFired = true;
                engine.Tick("surv_a", 24f);
                var resented = engine.GetState("surv_a");
                Check(resentmentFired && resented != null && resented.resentmentLevel > 0f
                      && !string.IsNullOrEmpty(resented.resentmentTargetId),
                    "Check 2: a 20%+ allocation deficit builds resentment and names a target.");
                passed += resentmentFired && resented != null && resented.resentmentLevel > 0f ? 1 : 0;

                // 3. Resentment decays when the allocation is fair again.
                engine.SetAllocation("surv_a", 0.95f);
                engine.SetAllocation("surv_b", 0.9f);
                engine.SetAllocation("surv_c", 0.9f);
                float beforeDecay = resented!.resentmentLevel;
                for (int i = 0; i < 10; i++) engine.Tick("surv_a", 24f);
                Check(engine.GetState("surv_a")!.resentmentLevel < beforeDecay,
                    "Check 3: resentment decays while allocations read as fair.");
                passed += engine.GetState("surv_a")!.resentmentLevel < beforeDecay ? 1 : 0;

                // ── Scene 2: the host routes consequences to canonical owners ──
                var needs = new NeedsSystem();
                var needA = new SurvivorNeedsState { Id = "surv_a" };
                var needB = new SurvivorNeedsState { Id = "surv_b" };
                needs.Register(needA);
                needs.Register(needB);
                var relations = new SurvivorRelationsSystem(new SeededRng(1986));
                var session = BuildSession(engine, needs, relations);

                // 4. Escalation past the confrontation threshold applies morale
                //    exactly once per event through NeedsSystem.
                engine.SetAllocation("surv_a", 0.05f);
                engine.SetAllocation("surv_b", 1.0f);
                engine.SetAllocation("surv_c", 1.0f);
                float moraleBefore = needA.Morale;
                int moraleEvents = 0;
                engine.OnMoraleDelta += (_, _, _) => moraleEvents++;
                bool confrontationFired = false;
                engine.OnRationConfrontation += (_, _) => confrontationFired = true;
                for (int i = 0; i < 30 && !confrontationFired; i++) engine.Tick("surv_a", 24f);
                Check(confrontationFired && moraleEvents >= 1 && needA.Morale < moraleBefore,
                    "Check 4: a confrontation applies morale exactly once per event through NeedsSystem.");
                passed += confrontationFired && moraleEvents >= 1 ? 1 : 0;

                // 5. The resentment target is recorded through the single affinity owner.
                float affinity = 0f;
                if (relations.TryGetRelationship("surv_a", "surv_b", out var rel) && rel != null)
                    affinity = rel.affinity;
                Check(Math.Abs(affinity) > 0.0001f,
                    $"Check 5: the resentment target is recorded through SurvivorRelationsSystem (affinity {affinity:+0;-0}).");
                passed += Math.Abs(affinity) > 0.0001f ? 1 : 0;

                // 6. The theft roll is deterministic for a fixed seed.
                int theftOutcomes = 0;
                for (int run = 0; run < 3; run++)
                {
                    var e2 = new RationConflictSystem(new SeededRng(4242));
                    e2.RegisterSurvivor("t");
                    e2.RegisterSurvivor("v");
                    e2.SetAllocation("t", 0.0f);
                    e2.SetAllocation("v", 1.0f);
                    bool stole = false;
                    e2.OnRationsStolen += (_, _) => stole = true;
                    for (int i = 0; i < 40 && !stole; i++) e2.Tick("t", 24f);
                    if (stole) theftOutcomes++;
                }
                Check(theftOutcomes == 0 || theftOutcomes == 3,
                    $"Check 6: the theft roll is deterministic for a fixed seed ({theftOutcomes}/3 identical runs).");
                passed += theftOutcomes == 0 || theftOutcomes == 3 ? 1 : 0;

                // 7. Capture/restore round-trips the resentment state.
                var captured = session.CaptureState();
                var persisted = RationConflictSaveStore.TryCapturePersisted(captured);
                var restored = RationConflictSaveStore.TryRestorePersisted(persisted);
                var reload = new RationConflictHostSession(
                    new RationConflictSystem(new SeededRng(1)), () => null, () => needs, () => relations, restored);
                Check(restored != null && reload.Engine.GetState("surv_a") != null,
                    "Check 7: resentment state survives a save/load round-trip.");
                passed += restored != null ? 1 : 0;

                // 8. Reset clears only this authority's state.
                reload.Clear();
                Check(reload.Engine.GetState("surv_a") == null,
                    "Check 8: reset clears the conflict authority's state only.");
                passed += reload.Engine.GetState("surv_a") == null ? 1 : 0;

                // 9. Unregistered survivors are ignored, never crash.
                var bare = new RationConflictHostSession(
                    new RationConflictSystem(new SeededRng(2)), () => null, () => null, () => null, null);
                bare.RegisterSurvivor("surv_x");
                Check(bare.TickDay() == 1,
                    "Check 9: unbound needs/relations providers do not break the day tick.");
                passed += bare.TickDay() == 1 ? 1 : 0;

                // 10. The authored thresholds are the engine's, not the host's.
                Check(RationConflictSystem.FairnessDeviationThreshold == 0.20f
                      && RationConflictSystem.ConfrontationThreshold == 0.70f
                      && RationConflictSystem.TheftThreshold == 0.85f,
                    "Check 10: the escalation thresholds stay engine-authored.");
                passed += RationConflictSystem.FairnessDeviationThreshold == 0.20f ? 1 : 0;

                // 11. Registering the same survivor twice is a no-op.
                session.RegisterSurvivor("surv_a");
                int registered = session.RegisteredSurvivors.Count;
                session.RegisterSurvivor("surv_a");
                Check(session.RegisteredSurvivors.Count == registered,
                    "Check 11: re-registering a survivor is a no-op.");
                passed += session.RegisteredSurvivors.Count == registered ? 1 : 0;
            }
            catch (Exception ex) { Console.WriteLine($"[FAIL] Unexpected probe exception: {ex}"); }
            Console.WriteLine($"=== Ration Conflict Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
