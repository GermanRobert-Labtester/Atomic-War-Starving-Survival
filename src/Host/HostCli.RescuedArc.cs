// SPDX-License-Identifier: MIT
// ============================================================================
// Host CLI Self-Test : RescuedArcSelfTest
// Core Authority     : Ashfall.Core.Radio.RescuedArcProjection (EN-05)
// Purpose            : distress rescue mission state -> truthful arc read model
// ============================================================================

using System;
using Ashfall.Core.Radio;

namespace AtomicWar.GodotApp
{
    public static class HostCliRescuedArc
    {
        public static int RunSelfTest(string? dataDir = null)
        {
            Console.WriteLine("=== [HostCli] Rescued Survivor Arc Projection Self-Test (EN-05) ===");
            int passed = 0;
            const int total = 8;

            try
            {
                var nullProjection = RescuedArcProjection.Project(null!, currentDay: 10);
                if (nullProjection.Phase == RescuedArcPhase.None && nullProjection.IsTerminal
                    && nullProjection.JournalKey == "rescued_arc:none")
                {
                    Console.WriteLine("[PASS] Check 1: a null mission renders a terminal None arc.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 1: null arc = {nullProjection.Phase}/{nullProjection.JournalKey}."); }

                var dispatched = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_01",
                    QuestId = "quest_rescue_01",
                    DestinationId = "loc_bunker_alpha",
                    Stage = DistressRescueMissionStage.Dispatched,
                    InterceptedDay = 5
                }, currentDay: 6);
                if (dispatched.Phase == RescuedArcPhase.EnRoute && !dispatched.IsTerminal
                    && dispatched.JournalKey == "rescued_arc:distress_signal_01:enroute"
                    && dispatched.QuestId == "quest_rescue_01" && dispatched.DestinationId == "loc_bunker_alpha")
                {
                    Console.WriteLine("[PASS] Check 2: a dispatched mission is non-terminal EnRoute with bound identities.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 2: dispatched arc = {dispatched.Phase}/{dispatched.JournalKey}."); }

                var hospitalized = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_02",
                    Stage = DistressRescueMissionStage.TerminalRescued,
                    SenderAlive = true,
                    InterceptedDay = 10,
                    DaysToTrace = 2
                }, currentDay: 13, recoveryDaysTotal: 5);
                if (hospitalized.Phase == RescuedArcPhase.Hospitalized && !hospitalized.IsTerminal
                    && hospitalized.RecoveryDaysLeft == 4 && hospitalized.StatusSummary.Contains("4 days remaining"))
                {
                    Console.WriteLine("[PASS] Check 3: a recent rescue is Hospitalized with a truthful recovery countdown.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 3: hospitalized = {hospitalized.Phase}/{hospitalized.RecoveryDaysLeft}."); }

                var integrated = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_03",
                    Stage = DistressRescueMissionStage.TerminalRescued,
                    SenderAlive = true,
                    InterceptedDay = 10,
                    DaysToTrace = 2
                }, currentDay: 20, recoveryDaysTotal: 5);
                if (integrated.Phase == RescuedArcPhase.Integrated && integrated.IsTerminal
                    && integrated.RecoveryDaysLeft == 0 && integrated.StatusSummary.Contains("integrated"))
                {
                    Console.WriteLine("[PASS] Check 4: a rescue past the recovery window is terminal Integrated.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 4: integrated = {integrated.Phase}/{integrated.RecoveryDaysLeft}."); }

                var ambushed = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_trap",
                    Stage = DistressRescueMissionStage.TerminalAmbush
                }, currentDay: 15);
                if (ambushed.Phase == RescuedArcPhase.Ambushed && ambushed.IsTerminal
                    && ambushed.JournalKey == "rescued_arc:distress_signal_trap:ambushed")
                {
                    Console.WriteLine("[PASS] Check 5: a terminal ambush is a distinct terminal phase.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 5: ambush = {ambushed.Phase}/{ambushed.JournalKey}."); }

                var failed = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_lost",
                    Stage = DistressRescueMissionStage.TerminalFailed
                }, currentDay: 15);
                if (failed.Phase == RescuedArcPhase.Perished && failed.IsTerminal)
                {
                    Console.WriteLine("[PASS] Check 6: a terminal failure is Perished and terminal.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 6: failed = {failed.Phase}."); }

                var deadSender = RescuedArcProjection.Project(new DistressRescueMission
                {
                    SignalId = "distress_signal_dead",
                    Stage = DistressRescueMissionStage.TerminalRescued,
                    SenderAlive = false,
                    InterceptedDay = 10,
                    DaysToTrace = 2
                }, currentDay: 13);
                if (deadSender.Phase == RescuedArcPhase.Perished && deadSender.IsTerminal)
                {
                    Console.WriteLine("[PASS] Check 7: a rescued stage with a dead sender is Perished, not Hospitalized.");
                    passed++;
                }
                else { Console.WriteLine($"[FAIL] Check 7: dead sender = {deadSender.Phase}."); }

                var direct = new RescuedArcProjection(null, null, null, RescuedArcPhase.EnRoute, -3, false, null, null);
                if (direct.SignalId == string.Empty && direct.QuestId == string.Empty && direct.DestinationId == string.Empty
                    && direct.RecoveryDaysLeft == 0 && direct.StatusSummary == string.Empty && direct.JournalKey == string.Empty)
                {
                    Console.WriteLine("[PASS] Check 8: the constructor null/negative-safe path holds.");
                    passed++;
                }
                else { Console.WriteLine("[FAIL] Check 8: constructor not clamped."); }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[FAIL] Unhandled: {ex.Message}");
            }

            Console.WriteLine($"Rescued arc projection: {passed}/{total} checks passed");
            return passed == total ? 0 : 1;
        }
    }
}
