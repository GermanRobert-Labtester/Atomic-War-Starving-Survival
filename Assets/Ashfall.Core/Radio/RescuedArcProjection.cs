// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Radio
{
    public enum RescuedArcPhase
    {
        None = 0,
        EnRoute = 1,
        Hospitalized = 2,
        Integrated = 3,
        Perished = 4,
        Ambushed = 5,
    }

    /// <summary>
    /// EN-05: Rescued Survivor Arc Projection Read Model.
    /// Pure projection over DistressRescueMission state providing truthful
    /// UI-ready status summaries, recovery timelines, and exactly-once journal keys.
    /// Does not mutate state or duplicate survivor records.
    /// </summary>
    public sealed class RescuedArcProjection
    {
        public string SignalId { get; }
        public string QuestId { get; }
        public string DestinationId { get; }
        public RescuedArcPhase Phase { get; }
        public int RecoveryDaysLeft { get; }
        public bool IsTerminal { get; }
        public string StatusSummary { get; }
        public string JournalKey { get; }

        public RescuedArcProjection(
            string signalId,
            string questId,
            string destinationId,
            RescuedArcPhase phase,
            int recoveryDaysLeft,
            bool isTerminal,
            string statusSummary,
            string journalKey)
        {
            SignalId = signalId ?? string.Empty;
            QuestId = questId ?? string.Empty;
            DestinationId = destinationId ?? string.Empty;
            Phase = phase;
            RecoveryDaysLeft = Math.Max(0, recoveryDaysLeft);
            IsTerminal = isTerminal;
            StatusSummary = statusSummary ?? string.Empty;
            JournalKey = journalKey ?? string.Empty;
        }

        public static RescuedArcProjection Project(DistressRescueMission mission, int currentDay, int recoveryDaysTotal = 5)
        {
            if (mission == null)
            {
                return new RescuedArcProjection(
                    string.Empty,
                    string.Empty,
                    string.Empty,
                    RescuedArcPhase.None,
                    0,
                    true,
                    "No rescue mission record.",
                    "rescued_arc:none");
            }

            RescuedArcPhase phase;
            bool isTerminal;
            int recoveryDaysLeft = 0;
            string summary;

            switch (mission.Stage)
            {
                case DistressRescueMissionStage.Dispatched:
                case DistressRescueMissionStage.Reached:
                    phase = RescuedArcPhase.EnRoute;
                    isTerminal = false;
                    summary = "Rescue team en route to survivor coordinates.";
                    break;

                case DistressRescueMissionStage.TerminalAmbush:
                    phase = RescuedArcPhase.Ambushed;
                    isTerminal = true;
                    summary = "Distress signal was an ambush; recovery team repelled hostiles.";
                    break;

                case DistressRescueMissionStage.TerminalFailed:
                    phase = RescuedArcPhase.Perished;
                    isTerminal = true;
                    summary = "Survivor coordinates found abandoned; sender did not survive.";
                    break;

                case DistressRescueMissionStage.TerminalRescued:
                case DistressRescueMissionStage.TerminalSurvived:
                    if (!mission.SenderAlive)
                    {
                        phase = RescuedArcPhase.Perished;
                        isTerminal = true;
                        summary = "Survivor succumbed before extraction could be completed.";
                    }
                    else
                    {
                        int arrivalDay = mission.InterceptedDay + mission.DaysToTrace;
                        int daysSinceArrival = Math.Max(0, currentDay - arrivalDay);
                        if (daysSinceArrival < recoveryDaysTotal)
                        {
                            phase = RescuedArcPhase.Hospitalized;
                            isTerminal = false;
                            recoveryDaysLeft = recoveryDaysTotal - daysSinceArrival;
                            summary = $"Survivor undergoing medical stabilization; {recoveryDaysLeft} days remaining.";
                        }
                        else
                        {
                            phase = RescuedArcPhase.Integrated;
                            isTerminal = true;
                            recoveryDaysLeft = 0;
                            summary = "Survivor fully stabilized and integrated into shelter operations.";
                        }
                    }
                    break;

                default:
                    if (mission.Expired || !mission.SenderAlive)
                    {
                        phase = RescuedArcPhase.Perished;
                        isTerminal = true;
                        summary = "Signal expired without response; sender presumed lost.";
                    }
                    else
                    {
                        phase = RescuedArcPhase.None;
                        isTerminal = false;
                        summary = "Distress signal intercepted; awaiting expedition dispatch.";
                    }
                    break;
            }

            string journalKey = $"rescued_arc:{mission.SignalId}:{phase.ToString().ToLowerInvariant()}";

            return new RescuedArcProjection(
                mission.SignalId,
                mission.QuestId,
                mission.DestinationId,
                phase,
                recoveryDaysLeft,
                isTerminal,
                summary,
                journalKey);
        }
    }
}
