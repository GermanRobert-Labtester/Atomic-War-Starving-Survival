// SPDX-License-Identifier: MIT
// Task #132 — Exhaustive lifecycle transition table.
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.Survivors;
using Xunit;

namespace Ashfall.Core.Tests.Survivors
{
    public class SurvivorLifecycleTableTests
    {
        private static readonly SurvivorLifecycleState[] AllStates =
            (SurvivorLifecycleState[])Enum.GetValues(typeof(SurvivorLifecycleState));

        private static readonly SurvivorTransition[] AllTransitions =
            (SurvivorTransition[])Enum.GetValues(typeof(SurvivorTransition));

        /// <summary>
        /// The state set is deliberately closed. Adding Candidate or Missing without
        /// backing mechanics would be inventing gameplay, so this pins the decision.
        /// </summary>
        [Fact]
        public void StateSet_IsExactlyTheStatesTheGameBacks()
        {
            Assert.Equal(
                new[] { "Unknown", "Resident", "Away", "Dead", "Memorialized" },
                AllStates.Select(s => s.ToString()).ToArray());

            Assert.Equal(
                new[]
                {
                    SurvivorLifecycleState.Resident,
                    SurvivorLifecycleState.Away,
                    SurvivorLifecycleState.Dead,
                    SurvivorLifecycleState.Memorialized
                },
                SurvivorLifecycle.LegalStates);

            Assert.False(SurvivorLifecycle.IsLegalState(SurvivorLifecycleState.Unknown));
            Assert.All(SurvivorLifecycle.LegalStates, s => Assert.True(SurvivorLifecycle.IsLegalState(s)));
        }

        /// <summary>Persisted numeric values are a save contract; they must not drift.</summary>
        [Fact]
        public void StateValues_ArePinnedForPersistence()
        {
            Assert.Equal(0, (int)SurvivorLifecycleState.Unknown);
            Assert.Equal(1, (int)SurvivorLifecycleState.Resident);
            Assert.Equal(2, (int)SurvivorLifecycleState.Away);
            Assert.Equal(3, (int)SurvivorLifecycleState.Dead);
            Assert.Equal(4, (int)SurvivorLifecycleState.Memorialized);
        }

        /// <summary>
        /// Every (state, transition) pair, asserted explicitly. The table is small
        /// enough to enumerate, so there is no reason to sample it.
        /// </summary>
        [Theory]
        // From Resident
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Deploy, true)]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Return, false)]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Die, true)]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Memorialize, false)]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Leave, true)]
        // From Away
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Deploy, false)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Return, true)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Die, true)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Memorialize, false)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Leave, false)]
        // From Dead
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Deploy, false)]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Return, false)]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Die, false)]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Memorialize, true)]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Leave, false)]
        // From Memorialized — terminal
        [InlineData(SurvivorLifecycleState.Memorialized, SurvivorTransition.Deploy, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, SurvivorTransition.Return, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, SurvivorTransition.Die, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, SurvivorTransition.Memorialize, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, SurvivorTransition.Leave, false)]
        public void TransitionTable_IsExhaustivelyPinned(
            SurvivorLifecycleState from, SurvivorTransition transition, bool legal)
        {
            Assert.Equal(legal, SurvivorLifecycle.IsLegalTransition(from, transition));
        }

        [Theory]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Deploy, SurvivorLifecycleState.Away)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Return, SurvivorLifecycleState.Resident)]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Die, SurvivorLifecycleState.Dead)]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Die, SurvivorLifecycleState.Dead)]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Memorialize, SurvivorLifecycleState.Memorialized)]
        public void Destination_IsCorrectForLegalTransitions(
            SurvivorLifecycleState from, SurvivorTransition transition, SurvivorLifecycleState expected)
        {
            Assert.Equal(expected, SurvivorLifecycle.Destination(from, transition));
        }

        [Fact]
        public void Join_AlwaysLeadsToResident()
        {
            foreach (var state in AllStates)
                Assert.Equal(SurvivorLifecycleState.Resident, SurvivorLifecycle.Destination(state, SurvivorTransition.Join));
        }

        /// <summary>Leave removes the aggregate, so it has no destination state.</summary>
        [Fact]
        public void Leave_HasNoDestinationState()
        {
            foreach (var state in AllStates)
                Assert.Null(SurvivorLifecycle.Destination(state, SurvivorTransition.Leave));

            Assert.True(SurvivorLifecycle.IsLegalTransition(SurvivorLifecycleState.Resident, SurvivorTransition.Leave));
        }

        [Fact]
        public void LegalTransitionsFrom_MatchesTheTable()
        {
            Assert.Equal(
                new[] { SurvivorTransition.Deploy, SurvivorTransition.Die, SurvivorTransition.Leave },
                SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Resident).ToArray());

            Assert.Equal(
                new[] { SurvivorTransition.Return, SurvivorTransition.Die },
                SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Away).ToArray());

            Assert.Equal(
                new[] { SurvivorTransition.Memorialize },
                SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Dead).ToArray());

            Assert.Empty(SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Memorialized));
        }

        /// <summary>No transition leads out of Unknown, so a corrupt state cannot be played on.</summary>
        [Fact]
        public void UnknownState_HasNoLegalTransitionsExceptJoin()
        {
            Assert.Empty(SurvivorLifecycle.LegalTransitionsFrom(SurvivorLifecycleState.Unknown));
        }

        // ── Eligibility predicates ─────────────────────────────────────

        [Theory]
        [InlineData(SurvivorLifecycleState.Resident, true)]
        [InlineData(SurvivorLifecycleState.Away, true)]
        [InlineData(SurvivorLifecycleState.Dead, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, false)]
        [InlineData(SurvivorLifecycleState.Unknown, false)]
        public void IsAlive_CoversResidentAndAway(SurvivorLifecycleState state, bool alive)
        {
            Assert.Equal(alive, SurvivorLifecycle.IsAlive(state));
            // A survivor is simulated exactly while alive: deployed survivors keep
            // accumulating hunger and dose.
            Assert.Equal(alive, SurvivorLifecycle.IsSimulated(state));
        }

        [Theory]
        [InlineData(SurvivorLifecycleState.Resident, false)]
        [InlineData(SurvivorLifecycleState.Away, false)]
        [InlineData(SurvivorLifecycleState.Dead, true)]
        [InlineData(SurvivorLifecycleState.Memorialized, true)]
        public void IsDeceased_CoversDeadAndMemorialized(SurvivorLifecycleState state, bool deceased)
        {
            Assert.Equal(deceased, SurvivorLifecycle.IsDeceased(state));
            Assert.False(SurvivorLifecycle.IsAlive(state) && SurvivorLifecycle.IsDeceased(state));
        }

        [Fact]
        public void AliveAndDeceased_ArePartitionsOfTheLegalStates()
        {
            foreach (var state in SurvivorLifecycle.LegalStates)
                Assert.True(SurvivorLifecycle.IsAlive(state) ^ SurvivorLifecycle.IsDeceased(state));
        }

        [Theory]
        [InlineData(SurvivorLifecycleState.Resident, true)]
        [InlineData(SurvivorLifecycleState.Away, false)]
        [InlineData(SurvivorLifecycleState.Dead, false)]
        [InlineData(SurvivorLifecycleState.Memorialized, false)]
        public void AssignmentAndDeployment_RequireAResident(SurvivorLifecycleState state, bool eligible)
        {
            Assert.Equal(eligible, SurvivorLifecycle.IsAssignmentEligible(state));
            Assert.Equal(eligible, SurvivorLifecycle.IsDeploymentEligible(state));
            Assert.Equal(eligible, SurvivorLifecycle.IsInShelter(state));
        }

        [Fact]
        public void IsDeployed_IsAwayOnly()
        {
            Assert.True(SurvivorLifecycle.IsDeployed(SurvivorLifecycleState.Away));
            foreach (var state in AllStates.Where(s => s != SurvivorLifecycleState.Away))
                Assert.False(SurvivorLifecycle.IsDeployed(state));
        }

        // ── Diagnostics ────────────────────────────────────────────────

        /// <summary>
        /// Every illegal pair must produce a specific reason, not a generic one.
        /// A log line nobody can act on is a log line nobody reads.
        /// </summary>
        [Fact]
        public void DescribeIllegal_NamesTheSurvivorStateAndReason()
        {
            var id = new SurvivorId("the_surveyor");

            var checkedPairs = 0;
            foreach (var from in SurvivorLifecycle.LegalStates)
            {
                foreach (var transition in AllTransitions)
                {
                    if (transition == SurvivorTransition.Join) continue;
                    if (SurvivorLifecycle.IsLegalTransition(from, transition)) continue;

                    string message = SurvivorLifecycle.DescribeIllegal(id, from, transition);
                    Assert.Contains("the_surveyor", message);
                    Assert.Contains(from.ToString(), message);
                    Assert.Contains(transition.ToString(), message);
                    checkedPairs++;
                }
            }

            // 4 legal states x 5 transitions (Join excluded) = 20 pairs, of which
            // 6 are legal (Resident: Deploy/Die/Leave, Away: Return/Die, Dead: Memorialize).
            Assert.Equal(14, checkedPairs);
        }

        [Theory]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Deploy, "already deployed")]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Return, "not deployed")]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Die, "already dead")]
        [InlineData(SurvivorLifecycleState.Resident, SurvivorTransition.Memorialize, "cannot memorialize a living survivor")]
        [InlineData(SurvivorLifecycleState.Away, SurvivorTransition.Leave, "while deployed")]
        [InlineData(SurvivorLifecycleState.Dead, SurvivorTransition.Deploy, "the dead cannot be deployed")]
        public void DescribeIllegal_GivesTheSpecificReason(
            SurvivorLifecycleState from, SurvivorTransition transition, string fragment)
        {
            string message = SurvivorLifecycle.DescribeIllegal(new SurvivorId("the_surveyor"), from, transition);
            Assert.Contains(fragment, message);
        }

        // ── Result type ────────────────────────────────────────────────

        [Fact]
        public void Result_SatisfiedCoversCommittedAndAlreadyInState()
        {
            var id = new SurvivorId("the_surveyor");

            var committed = SurvivorLifecycleResult.Committed(
                id, SurvivorTransition.Die, SurvivorLifecycleState.Resident, SurvivorLifecycleState.Dead, 2L);
            var already = SurvivorLifecycleResult.AlreadyIn(
                id, SurvivorTransition.Die, SurvivorLifecycleState.Dead, 2L);
            var blocked = SurvivorLifecycleResult.Blocked(
                id, SurvivorTransition.Deploy, SurvivorLifecycleState.Dead,
                SurvivorLifecycleFailure.TransitionIllegal, "no");

            Assert.True(committed.IsCommitted);
            Assert.True(committed.IsSatisfied);
            Assert.False(committed.IsBlocked);

            Assert.False(already.IsCommitted);
            Assert.True(already.IsSatisfied);
            Assert.False(already.IsBlocked);

            Assert.False(blocked.IsCommitted);
            Assert.False(blocked.IsSatisfied);
            Assert.True(blocked.IsBlocked);
        }

        [Fact]
        public void Result_BlockedKeepsFromAndToIdentical()
        {
            var blocked = SurvivorLifecycleResult.Blocked(
                new SurvivorId("the_surveyor"), SurvivorTransition.Deploy,
                SurvivorLifecycleState.Dead, SurvivorLifecycleFailure.TransitionIllegal, "no");

            Assert.Equal(blocked.From, blocked.To);
        }

        [Fact]
        public void FailureCodes_AreDistinctSnakeCase()
        {
            var codes = new[]
            {
                SurvivorLifecycleFailure.IdInvalid,
                SurvivorLifecycleFailure.Unknown,
                SurvivorLifecycleFailure.AlreadyExists,
                SurvivorLifecycleFailure.DefinitionRequired,
                SurvivorLifecycleFailure.TransitionIllegal,
                SurvivorLifecycleFailure.AlreadyInState,
                SurvivorLifecycleFailure.ExpeditionIdRequired,
                SurvivorLifecycleFailure.ExpeditionMismatch,
                SurvivorLifecycleFailure.ComponentsAttached
            };

            Assert.Equal(codes.Length, new HashSet<string>(codes).Count);
            Assert.All(codes, c => Assert.Matches("^[a-z][a-z0-9_]*$", c));
        }
    }
}
