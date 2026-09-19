// SPDX-License-Identifier: MIT
using System.Collections.Generic;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public sealed class LeadershipSuccessionTests
    {
        private static readonly List<string> AliveIds = new List<string> { "leader_1", "successor_1", "deputy_1", "challenger_1" };

        private static LeadershipSystem CreateSystem()
        {
            var sys = new LeadershipSystem();
            sys.GetAliveSurvivorIds = () => AliveIds;
            return sys;
        }

        [Fact]
        public void DesignateSuccessor_SetsSuccessor_WhenAlive()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");

            bool designated = sys.DesignateSuccessor("successor_1");

            Assert.True(designated);
            Assert.Equal("successor_1", sys.DesignatedSuccessorId);
        }

        [Fact]
        public void DesignateSuccessor_RejectsLeaderAndDeadSurvivor()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");

            Assert.False(sys.DesignateSuccessor("leader_1"));
            Assert.False(sys.DesignateSuccessor("not_alive"));
            Assert.True(string.IsNullOrEmpty(sys.DesignatedSuccessorId));
        }

        [Fact]
        public void AppointDeputy_SetsDeputy_WhenAlive()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");

            bool appointed = sys.AppointDeputy("deputy_1");

            Assert.True(appointed);
            Assert.Equal("deputy_1", sys.DeputyLeaderId);
        }

        [Fact]
        public void OnSurvivorDied_LeaderFallen_TriggersSuccessionToDesignatedSuccessor()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");
            sys.DesignateSuccessor("successor_1");

            string? triggeredDead = null;
            string? triggeredNew = null;
            sys.OnSuccessionTriggered += (d, n) =>
            {
                triggeredDead = d;
                triggeredNew = n;
            };

            // Leader dies
            sys.OnSurvivorDied("leader_1");

            Assert.Equal("leader_1", triggeredDead);
            Assert.Equal("successor_1", triggeredNew);
            Assert.Equal("successor_1", sys.CurrentLeaderId);
            Assert.True(sys.IsDesignatedLeader("successor_1"));
        }

        [Fact]
        public void OnSurvivorDied_LeaderFallen_TriggersSuccessionToDeputy_WhenNoSuccessor()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");
            sys.AppointDeputy("deputy_1");

            sys.OnSurvivorDied("leader_1");

            Assert.Equal("deputy_1", sys.CurrentLeaderId);
            Assert.True(sys.IsDesignatedLeader("deputy_1"));
        }

        [Fact]
        public void InitiateChallenge_And_ResolveChallenge_ReplacesLeaderOnVictory()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");

            var ch = sys.InitiateChallenge("challenger_1", "Dispute over food rations");
            Assert.NotNull(ch);
            Assert.Equal("challenger_1", ch.challenger_id);
            Assert.Equal("leader_1", ch.challenged_leader_id);
            Assert.False(ch.is_resolved);

            bool resolved = sys.ResolveChallenge(ch.challenge_id, challengerWon: true);

            Assert.True(resolved);
            Assert.True(ch.is_resolved);
            Assert.True(ch.challenger_won);
            Assert.Equal("challenger_1", sys.CurrentLeaderId);
            Assert.False(sys.IsDesignatedLeader("leader_1"));
            Assert.True(sys.IsDesignatedLeader("challenger_1"));
        }

        [Fact]
        public void InitiateChallenge_RejectsLeaderDeadAndDuplicateChallengers()
        {
            var sys = CreateSystem();
            sys.DesignateLeader("leader_1");

            Assert.Null(sys.InitiateChallenge("leader_1", "self"));
            Assert.Null(sys.InitiateChallenge("not_alive", "invalid"));
            Assert.NotNull(sys.InitiateChallenge("challenger_1", "first"));
            Assert.Null(sys.InitiateChallenge("challenger_1", "duplicate"));
        }

        [Fact]
        public void CaptureState_And_RestoreState_RoundTripsSuccessorDeputyAndChallenges()
        {
            var sys1 = CreateSystem();
            sys1.DesignateLeader("leader_1");
            sys1.DesignateSuccessor("successor_1");
            sys1.AppointDeputy("deputy_1");
            sys1.InitiateChallenge("challenger_1", "Test challenge");

            var state = sys1.CaptureState();
            Assert.Equal("successor_1", state.designated_successor_id);
            Assert.Equal("deputy_1", state.deputy_leader_id);
            Assert.Single(state.challenges);

            var sys2 = CreateSystem();
            sys2.RestoreState(state);

            Assert.Equal("leader_1", sys2.CurrentLeaderId);
            Assert.Equal("successor_1", sys2.DesignatedSuccessorId);
            Assert.Equal("deputy_1", sys2.DeputyLeaderId);
            Assert.Single(sys2.Challenges);
            Assert.Equal("challenger_1", sys2.Challenges[0].challenger_id);

            sys2.ResolveChallenge("chl_1", challengerWon: false);
            var next = sys2.InitiateChallenge("challenger_1", "Second challenge");
            Assert.NotNull(next);
            Assert.Equal("chl_2", next.challenge_id);
        }

        [Fact]
        public void OnSurvivorDied_DeadSuccessorIsClearedAndDeputyStillSucceeds()
        {
            var alive = new List<string>(AliveIds);
            var sys = new LeadershipSystem { GetAliveSurvivorIds = () => alive };
            sys.DesignateLeader("leader_1");
            sys.DesignateSuccessor("successor_1");
            sys.AppointDeputy("deputy_1");

            alive.Remove("successor_1");
            sys.OnSurvivorDied("successor_1");
            Assert.True(string.IsNullOrEmpty(sys.DesignatedSuccessorId));

            alive.Remove("leader_1");
            sys.OnSurvivorDied("leader_1");
            Assert.Equal("deputy_1", sys.CurrentLeaderId);
        }

        [Fact]
        public void ResolveChallenge_DeadChallengerCannotTakeLeadership()
        {
            var alive = new List<string>(AliveIds);
            var sys = new LeadershipSystem { GetAliveSurvivorIds = () => alive };
            sys.DesignateLeader("leader_1");
            var challenge = sys.InitiateChallenge("challenger_1", "Test");
            Assert.NotNull(challenge);

            alive.Remove("challenger_1");

            Assert.False(sys.ResolveChallenge(challenge.challenge_id, challengerWon: true));
            Assert.Equal("leader_1", sys.CurrentLeaderId);
        }
    }
}
