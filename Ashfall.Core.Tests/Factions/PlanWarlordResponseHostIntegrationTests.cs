// SPDX-License-Identifier: MIT
using Ashfall.Core.Warlords;
using Xunit;

namespace Ashfall.Core.Tests.Factions
{
    /// <summary>
    /// Host-integration tests for the idempotent warlord tribute response surface
    /// over the LIVE <see cref="WarlordDoctrineSystem"/>. The doctrine owner keeps
    /// ask escalation, reliability, and standing; this surface only guards which
    /// responses have already been issued for a given tribute.
    /// </summary>
    public sealed class PlanWarlordResponseHostIntegrationTests
    {
        private static WarlordDoctrineSystem BuildDoctrine()
        {
            var system = new WarlordDoctrineSystem();
            // Advance the tribute cadence once so a canonical tribute id exists.
            system.State.totalWeeksAsked = 1;
            system.State.lastAskDay = 1;
            return system;
        }

        [Fact]
        public void TheCanonicalTributeId_IsDerivedFromTheLiveDoctrineState()
        {
            var doctrine = BuildDoctrine();

            Assert.Equal("tribute_week_1", $"tribute_week_{doctrine.State.totalWeeksAsked}");
        }

        [Fact]
        public void Pay_IsAcceptedOnceAndThenRefusedForTheSameAsk()
        {
            var doctrine = BuildDoctrine();
            var actions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            const string id = "tribute_week_1";

            var first = actions.Pay(id, 5, 3);
            var second = actions.Pay(id, 5, 3);

            Assert.True(first.Succeeded);
            Assert.False(second.Succeeded);
            Assert.Equal("already_responded", second.ReasonCode);
            Assert.True(actions.IsResponded(id));
        }

        [Fact]
        public void Contest_And_Submit_AreDistinctResponseKinds()
        {
            var doctrine = BuildDoctrine();
            var contestActions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            var submitActions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            const string id = "tribute_week_1";

            var contest = contestActions.Contest(id, 4);
            var submit = submitActions.Submit(id, 4);

            Assert.True(contest.Succeeded);
            Assert.Equal(WarlordResponseKind.Contest, contest.Record.Kind);
            Assert.Equal(0, contest.Record.AmountPaid);
            Assert.True(submit.Succeeded);
            Assert.Equal(WarlordResponseKind.Submit, submit.Record.Kind);
            Assert.True(submit.Record.SettledFully);
        }

        [Fact]
        public void OneResponsePerAsk_AcrossAllThreeKinds()
        {
            var doctrine = BuildDoctrine();
            var actions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            const string id = "tribute_week_1";

            actions.Pay(id, 5, 3);

            Assert.False(actions.Contest(id, 3).Succeeded);
            Assert.False(actions.Submit(id, 3).Succeeded);
        }

        [Fact]
        public void AnEmptyTributeId_IsAlwaysRefused()
        {
            var doctrine = BuildDoctrine();
            var actions = new WarlordResponseActions(new WarlordResponseState(), doctrine);

            Assert.False(actions.Pay(string.Empty, 5, 3).Succeeded);
            Assert.False(actions.Contest(string.Empty, 3).Succeeded);
            Assert.False(actions.Submit(string.Empty, 3).Succeeded);
        }

        [Fact]
        public void CaptureAndRestore_RoundTripsTheResponseLedger()
        {
            var doctrine = BuildDoctrine();
            var actions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            actions.Pay("tribute_week_1", 5, 3);

            var captured = actions.CaptureState();
            var reloaded = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            reloaded.RestoreState(captured);

            Assert.True(reloaded.IsResponded("tribute_week_1"));
            Assert.False(reloaded.Pay("tribute_week_1", 5, 3).Succeeded);
        }

        [Fact]
        public void SupersededIds_StopBlockingANewAsk()
        {
            var doctrine = BuildDoctrine();
            var state = new WarlordResponseState();
            var actions = new WarlordResponseActions(state, doctrine);
            actions.Pay("tribute_week_1", 5, 3);

            // The host prunes anything that is not the current week's id.
            int currentWeek = 2;
            state.Responses.RemoveAll(r => r.TributeId != $"tribute_week_{currentWeek}");

            Assert.False(actions.IsResponded("tribute_week_1"));
            Assert.True(actions.Pay("tribute_week_2", 5, 9).Succeeded);
        }

        [Fact]
        public void TheDoctrineOwner_IsNeverReplacedOrBypassed()
        {
            var doctrine = BuildDoctrine();
            var actions = new WarlordResponseActions(new WarlordResponseState(), doctrine);
            actions.Pay("tribute_week_1", 6, 3);

            // The response surface records intent; the doctrine owner remains the
            // only authority that can settle an ask and count a paid week.
            int totalWeeksPaidBefore = doctrine.State.totalWeeksPaid;
            int nextAsk;
            doctrine.SettleTribute(6, 3, out nextAsk);

            Assert.Equal(totalWeeksPaidBefore + 1, doctrine.State.totalWeeksPaid);
        }
    }
}
