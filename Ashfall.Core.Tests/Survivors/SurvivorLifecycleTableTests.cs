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
        [Fact]
        public void TransitionTable_IsExhaustivelyPinned()
        {
            var cases = new[]
            {
                (SurvivorLifecycleState.Resident, SurvivorTransition.Deploy, true),
                (SurvivorLifecycleState.Resident, SurvivorTransition.Return, false),
                (SurvivorLifecycleState.Resident, SurvivorTransition.Die, true),
                (SurvivorLifecycleState.Resident, SurvivorTransition.Memorialize, false),
                (SurvivorLifecycleState.Resident, SurvivorTransition.Leave, true),
                (SurvivorLifecycleState.Away, SurvivorTransition.Deploy, false),
                (SurvivorLifecycleState.Away, SurvivorTransition.Return, true),
                (SurvivorLifecycleState.Away, SurvivorTransition.Die, true),
                (SurvivorLifecycleState.Away, SurvivorTransition.Memorialize, false),
                (SurvivorLifecycleState.Away, SurvivorTransition.Leave, false),
                (SurvivorLifecycleState.Dead, SurvivorTransition.Deploy, false),
                (SurvivorLifecycleState.Dead, SurvivorTransition.Return, false),
                (SurvivorLifecycleState.Dead, SurvivorTransition.Die, false),
                (SurvivorLifecycleState.Dead, SurvivorTransition.Memorialize, true),
                (SurvivorLifecycleState.Dead, SurvivorTransition.Leave, false),
                (SurvivorLifecycleState.Memorialized, SurvivorTransition.Deploy, false),
                (SurvivorLifecycleState.Memorialized, SurvivorTransition.Return, false),
                (SurvivorLifecycleState.Memorialized, SurvivorTransition.Die, false),
                (SurvivorLifecycleState.Memorialized, SurvivorTransition.Memorialize, false),
                (SurvivorLifecycleState.Memorialized, SurvivorTransition.Leave, false)
            };
            var failures = new List<string>();

            foreach (var (from, transition, expected) in cases)
            {
                bool actual = SurvivorLifecycle.IsLegalTransition(from, transition);
                if (actual != expected)
                    failures.Add($"{from}/{transition}: expected legal={expected}, got {actual}");
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void Destination_IsCorrectForLegalTransitions()
        {
            var cases = new[]
            {
                (From: SurvivorLifecycleState.Resident, Transition: SurvivorTransition.Deploy, Expected: SurvivorLifecycleState.Away),
                (From: SurvivorLifecycleState.Away, Transition: SurvivorTransition.Return, Expected: SurvivorLifecycleState.Resident),
                (From: SurvivorLifecycleState.Resident, Transition: SurvivorTransition.Die, Expected: SurvivorLifecycleState.Dead),
                (From: SurvivorLifecycleState.Away, Transition: SurvivorTransition.Die, Expected: SurvivorLifecycleState.Dead),
                (From: SurvivorLifecycleState.Dead, Transition: SurvivorTransition.Memorialize, Expected: SurvivorLifecycleState.Memorialized)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                var actual = SurvivorLifecycle.Destination(test.From, test.Transition);
                if (actual != test.Expected)
                {
                    failures.Add(
                        $"{test.From}/{test.Transition}: expected {test.Expected}, got {actual}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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

        [Fact]
        public void IsAlive_CoversResidentAndAway()
        {
            var cases = new[]
            {
                (State: SurvivorLifecycleState.Resident, Alive: true),
                (State: SurvivorLifecycleState.Away, Alive: true),
                (State: SurvivorLifecycleState.Dead, Alive: false),
                (State: SurvivorLifecycleState.Memorialized, Alive: false),
                (State: SurvivorLifecycleState.Unknown, Alive: false)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                bool actualAlive = SurvivorLifecycle.IsAlive(test.State);
                bool actualSimulated = SurvivorLifecycle.IsSimulated(test.State);
                if (actualAlive != test.Alive || actualSimulated != test.Alive)
                {
                    failures.Add(
                        $"{test.State}: expected alive/simulated={test.Alive}, " +
                        $"got alive={actualAlive}, simulated={actualSimulated}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void IsDeceased_CoversDeadAndMemorialized()
        {
            var cases = new[]
            {
                (State: SurvivorLifecycleState.Resident, Deceased: false),
                (State: SurvivorLifecycleState.Away, Deceased: false),
                (State: SurvivorLifecycleState.Dead, Deceased: true),
                (State: SurvivorLifecycleState.Memorialized, Deceased: true)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                bool actualDeceased = SurvivorLifecycle.IsDeceased(test.State);
                bool actualAlive = SurvivorLifecycle.IsAlive(test.State);
                if (actualDeceased != test.Deceased || (actualAlive && actualDeceased))
                {
                    failures.Add(
                        $"{test.State}: expected deceased={test.Deceased}, " +
                        $"got deceased={actualDeceased}, alive={actualAlive}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
        }

        [Fact]
        public void AliveAndDeceased_ArePartitionsOfTheLegalStates()
        {
            foreach (var state in SurvivorLifecycle.LegalStates)
                Assert.True(SurvivorLifecycle.IsAlive(state) ^ SurvivorLifecycle.IsDeceased(state));
        }

        [Fact]
        public void AssignmentAndDeployment_RequireAResident()
        {
            var cases = new[]
            {
                (State: SurvivorLifecycleState.Resident, Eligible: true),
                (State: SurvivorLifecycleState.Away, Eligible: false),
                (State: SurvivorLifecycleState.Dead, Eligible: false),
                (State: SurvivorLifecycleState.Memorialized, Eligible: false)
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                bool assignment = SurvivorLifecycle.IsAssignmentEligible(test.State);
                bool deployment = SurvivorLifecycle.IsDeploymentEligible(test.State);
                bool inShelter = SurvivorLifecycle.IsInShelter(test.State);
                if (assignment != test.Eligible ||
                    deployment != test.Eligible ||
                    inShelter != test.Eligible)
                {
                    failures.Add(
                        $"{test.State}: expected all eligibility flags={test.Eligible}, " +
                        $"got assignment={assignment}, deployment={deployment}, shelter={inShelter}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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

        [Fact]
        public void DescribeIllegal_GivesTheSpecificReason()
        {
            var cases = new[]
            {
                (From: SurvivorLifecycleState.Away, Transition: SurvivorTransition.Deploy, Fragment: "already deployed"),
                (From: SurvivorLifecycleState.Resident, Transition: SurvivorTransition.Return, Fragment: "not deployed"),
                (From: SurvivorLifecycleState.Dead, Transition: SurvivorTransition.Die, Fragment: "already dead"),
                (From: SurvivorLifecycleState.Resident, Transition: SurvivorTransition.Memorialize, Fragment: "cannot memorialize a living survivor"),
                (From: SurvivorLifecycleState.Away, Transition: SurvivorTransition.Leave, Fragment: "while deployed"),
                (From: SurvivorLifecycleState.Dead, Transition: SurvivorTransition.Deploy, Fragment: "the dead cannot be deployed")
            };
            var failures = new List<string>();

            foreach (var test in cases)
            {
                try
                {
                    string message = SurvivorLifecycle.DescribeIllegal(
                        new SurvivorId("the_surveyor"), test.From, test.Transition);
                    if (!message.Contains("the_surveyor", StringComparison.Ordinal) ||
                        !message.Contains(test.From.ToString(), StringComparison.Ordinal) ||
                        !message.Contains(test.Transition.ToString(), StringComparison.Ordinal) ||
                        !message.Contains(test.Fragment, StringComparison.Ordinal))
                    {
                        failures.Add(
                            $"{test.From}/{test.Transition}: message did not contain the required " +
                            $"state, transition, and fragment '{test.Fragment}': {message}");
                    }
                }
                catch (Exception exception)
                {
                    failures.Add(
                        $"{test.From}/{test.Transition}: {exception.GetType().Name}: {exception.Message}");
                }
            }

            Assert.True(failures.Count == 0, string.Join(Environment.NewLine, failures));
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
