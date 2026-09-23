// SPDX-License-Identifier: MIT
using Ashfall.Core.Radio;
using Xunit;

namespace Ashfall.Core.Tests.Radio
{
    public sealed class RescuedArcProjectionTests
    {
        [Fact]
        public void Project_NullMission_ReturnsDefaultTerminal()
        {
            var projection = RescuedArcProjection.Project(null!, currentDay: 10);

            Assert.Equal(RescuedArcPhase.None, projection.Phase);
            Assert.True(projection.IsTerminal);
            Assert.Equal("rescued_arc:none", projection.JournalKey);
        }

        [Fact]
        public void Project_Dispatched_ReturnsEnRoute()
        {
            var mission = new DistressRescueMission
            {
                SignalId = "distress_signal_01",
                QuestId = "quest_rescue_01",
                DestinationId = "loc_bunker_alpha",
                Stage = DistressRescueMissionStage.Dispatched,
                InterceptedDay = 5
            };

            var projection = RescuedArcProjection.Project(mission, currentDay: 6);

            Assert.Equal(RescuedArcPhase.EnRoute, projection.Phase);
            Assert.False(projection.IsTerminal);
            Assert.Equal("rescued_arc:distress_signal_01:enroute", projection.JournalKey);
        }

        [Fact]
        public void Project_TerminalRescued_RecentArrival_ReturnsHospitalized()
        {
            var mission = new DistressRescueMission
            {
                SignalId = "distress_signal_02",
                Stage = DistressRescueMissionStage.TerminalRescued,
                SenderAlive = true,
                InterceptedDay = 10,
                DaysToTrace = 2 // Arrival at day 12
            };

            // Current day 13 -> 1 day since arrival, recoveryDaysTotal = 5 -> 4 days left
            var projection = RescuedArcProjection.Project(mission, currentDay: 13, recoveryDaysTotal: 5);

            Assert.Equal(RescuedArcPhase.Hospitalized, projection.Phase);
            Assert.False(projection.IsTerminal);
            Assert.Equal(4, projection.RecoveryDaysLeft);
            Assert.Contains("4 days remaining", projection.StatusSummary);
            Assert.Equal("rescued_arc:distress_signal_02:hospitalized", projection.JournalKey);
        }

        [Fact]
        public void Project_TerminalRescued_PastRecovery_ReturnsIntegrated()
        {
            var mission = new DistressRescueMission
            {
                SignalId = "distress_signal_03",
                Stage = DistressRescueMissionStage.TerminalRescued,
                SenderAlive = true,
                InterceptedDay = 10,
                DaysToTrace = 2 // Arrival at day 12
            };

            // Current day 20 -> 8 days since arrival, recoveryDaysTotal = 5 -> integrated
            var projection = RescuedArcProjection.Project(mission, currentDay: 20, recoveryDaysTotal: 5);

            Assert.Equal(RescuedArcPhase.Integrated, projection.Phase);
            Assert.True(projection.IsTerminal);
            Assert.Equal(0, projection.RecoveryDaysLeft);
            Assert.Contains("integrated", projection.StatusSummary);
            Assert.Equal("rescued_arc:distress_signal_03:integrated", projection.JournalKey);
        }

        [Fact]
        public void Project_TerminalAmbush_ReturnsAmbushed()
        {
            var mission = new DistressRescueMission
            {
                SignalId = "distress_signal_trap",
                Stage = DistressRescueMissionStage.TerminalAmbush
            };

            var projection = RescuedArcProjection.Project(mission, currentDay: 15);

            Assert.Equal(RescuedArcPhase.Ambushed, projection.Phase);
            Assert.True(projection.IsTerminal);
            Assert.Contains("ambush", projection.StatusSummary);
            Assert.Equal("rescued_arc:distress_signal_trap:ambushed", projection.JournalKey);
        }
    }
}
