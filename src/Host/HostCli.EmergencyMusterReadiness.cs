// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : EmergencyMusterReadinessSelfTest
// Subsystem          : Emergency Muster Readiness (Expansion 23 — The Alarm)
// ============================================================================

using System;
using Ashfall.Core.Shelter;

namespace AtomicWar.GodotApp
{
    public static class HostCliEmergencyMusterReadiness
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Emergency Muster Readiness Self-Test ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var session = EmergencyMusterReadinessHostSession.Create();
                session.SetShelterCensus(40, 6);

                if (session.DrillCount == 0 && session.CurrentDay == 0)
                {
                    Console.WriteLine("[PASS] Check 1: Muster ledger starts empty.");
                    passed++;
                }
                else
                {
                    Console.WriteLine("[FAIL] Check 1: Ledger was not empty.");
                }

                // An untrained population with blocked routes must fail certification.
                var untrained = session.ConductDrill(1, EmergencyDrillType.None, 200, 300);
                if (!untrained.IsReadinessCertified
                    && untrained.CompositeReadinessScorePermille < 650
                    && untrained.EstimatedEvacuationMinutes > 4)
                {
                    Console.WriteLine($"[PASS] Check 2: Untrained blocked shelter fails certification ({untrained.CompositeReadinessScorePermille} permille, {untrained.EstimatedEvacuationMinutes} min).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 2: certified={untrained.IsReadinessCertified}, score={untrained.CompositeReadinessScorePermille}.");
                }

                // A full-alarm evacuation on clear routes must be the strongest drill.
                var evac = session.ConductDrill(2, EmergencyDrillType.FullAlarmEvacuation, 1000, 900);
                if (evac.CompositeReadinessScorePermille > untrained.CompositeReadinessScorePermille
                    && evac.CascadeInterventionMarginMinutes >= untrained.CascadeInterventionMarginMinutes)
                {
                    Console.WriteLine($"[PASS] Check 3: Full-alarm evacuation beats an untrained baseline.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 3: {evac.CompositeReadinessScorePermille} vs {untrained.CompositeReadinessScorePermille}.");
                }

                // Clear routes must shorten evacuation and lower missing-survivor risk.
                session.SetShelterCensus(40, 8);
                var cleared = session.ConductDrill(3, EmergencyDrillType.FullAlarmEvacuation, 1000, 1000);
                var blocked = session.ConductDrill(4, EmergencyDrillType.FullAlarmEvacuation, 0, 0);
                if (cleared.EstimatedEvacuationMinutes < blocked.EstimatedEvacuationMinutes
                    && cleared.MissingSurvivorRiskPermille < blocked.MissingSurvivorRiskPermille)
                {
                    Console.WriteLine($"[PASS] Check 4: Clear routes speed evacuation and cut missing risk.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 4: {cleared.EstimatedEvacuationMinutes} min vs {blocked.EstimatedEvacuationMinutes} min.");
                }

                // Over-drilling must trigger compliance fatigue without adding readiness.
                var fatigued = session.ConductDrill(5, EmergencyDrillType.TabletopWalkthrough, 1000, 1000);
                var rested = EmergencyMusterReadinessEngine.Evaluate(
                    40, 8, 60, EmergencyDrillType.TabletopWalkthrough, 1000, 1000, 0);
                if (fatigued.ComplianceFatiguePermille > 0
                    && fatigued.CompositeReadinessScorePermille <= rested.CompositeReadinessScorePermille)
                {
                    Console.WriteLine($"[PASS] Check 5: Over-drilling causes compliance fatigue ({fatigued.ComplianceFatiguePermille} permille).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 5: fatigue {fatigued.ComplianceFatiguePermille}.");
                }

                // Drill recency must decay: 60 days since a tabletop walkthrough scores lower than day 0.
                var fresh = EmergencyMusterReadinessEngine.Evaluate(
                    40, 6, 0, EmergencyDrillType.TabletopWalkthrough, 900, 900, 0);
                var stale = EmergencyMusterReadinessEngine.Evaluate(
                    40, 6, 60, EmergencyDrillType.TabletopWalkthrough, 900, 900, 0);
                if (stale.CompositeReadinessScorePermille < fresh.CompositeReadinessScorePermille)
                {
                    Console.WriteLine($"[PASS] Check 6: Drill decay lowers readiness over time ({fresh.CompositeReadinessScorePermille} -> {stale.CompositeReadinessScorePermille}).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 6: {fresh.CompositeReadinessScorePermille} -> {stale.CompositeReadinessScorePermille}.");
                }

                session.AdvanceDay(10);
                if (session.CurrentDay == 10 && session.DrillCount == 5)
                {
                    Console.WriteLine($"[PASS] Check 7: Readiness clock advanced ({session.DrillCount} drills, {session.CertedDrillCount} certified).");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 7: day={session.CurrentDay}, drills={session.DrillCount}.");
                }

                bool saved = session.TrySave();
                var reloaded = EmergencyMusterReadinessHostSession.Create();
                bool loaded = reloaded.TryLoad();
                if (saved && loaded
                    && reloaded.DrillCount == session.DrillCount
                    && reloaded.LastDrillType == EmergencyDrillType.TabletopWalkthrough
                    && EmergencyMusterReadinessSaveStore.SectionName.Equals("emergency_muster_readiness", StringComparison.Ordinal)
                    && EmergencyMusterReadinessSaveStore.FileName.Equals("emergency_muster_readiness_save.json", StringComparison.Ordinal)
                    && session.DrillCount == 5)
                {
                    Console.WriteLine($"[PASS] Check 8: Save/restore round-trip verified.");
                    passed++;
                }
                else
                {
                    Console.WriteLine($"[FAIL] Check 8: saved={saved}, loaded={loaded}, drills={reloaded.DrillCount}.");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Self-test crashed: {ex.Message}");
            }

            Console.WriteLine($"=== Emergency Muster Readiness Self-Test: {passed}/{total} passed ===");
            return passed == total ? 0 : 1;
        }
    }
}
