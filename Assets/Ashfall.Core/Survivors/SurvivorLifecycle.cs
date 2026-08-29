// SPDX-License-Identifier: MIT
// Task #132 — Survivor lifecycle state machine and transaction result.
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Survivors
{
    /// <summary>
    /// The lifecycle of one campaign survivor. Exactly one state at a time —
    /// this replaces the boolean soup (<c>isAlive</c> on the roster,
    /// <c>IsAlive</c>/<c>IsDead</c> on needs, <c>IsAlive</c> on radiation,
    /// <c>MedicalAdmissionStatus.Deceased</c>) with a single value that cannot
    /// express a contradiction.
    ///
    /// <para><b>Every state here is backed by mechanics that already exist.</b>
    /// The task brief suggested <c>Candidate</c> and <c>Missing</c> as well; both
    /// were rejected on evidence:</para>
    /// <list type="bullet">
    /// <item><description><c>Candidate</c> — there is no recruitment pool. A survivor
    /// before <c>Join</c> is a <see cref="SurvivorDefinition"/> in the catalog with no
    /// campaign aggregate at all, so "candidate" is the absence of an aggregate
    /// rather than a state of one.</description></item>
    /// <item><description><c>Missing</c> — no missing-survivor concept exists anywhere
    /// in Core or the host. Adding one would be inventing gameplay.</description></item>
    /// <item><description><c>Incapacitated</c> — real, but it is
    /// <c>MedicalAdmissionStatus.Active</c> and needs no cross-domain lifecycle
    /// authority, so it stays owned by the medical domain.</description></item>
    /// </list>
    /// </summary>
    public enum SurvivorLifecycleState
    {
        /// <summary>
        /// Not a legal state for a live aggregate. Reserved so a save written by
        /// an older or newer build deserializes to something obviously wrong
        /// rather than silently to <c>Resident</c>.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// In the shelter and available. Backed by <c>SurvivorRosterEntry.isAlive == true</c>
        /// with no active expedition.
        /// </summary>
        Resident = 1,

        /// <summary>
        /// Out on an expedition. Backed by an entry in <c>ExpeditionSystem._active</c>,
        /// which is keyed by survivor id and therefore permits at most one.
        /// </summary>
        Away = 2,

        /// <summary>
        /// Dead. Backed by <c>SurvivorRosterEntry.isAlive == false</c> plus a
        /// <see cref="SurvivorFateEvent"/>. The fate ledger keeps the cause and
        /// day; the aggregate keeps only the fact, so death has one owner.
        /// </summary>
        Dead = 3,

        /// <summary>
        /// Dead and remembered. Backed by <c>MemorialSystem</c>'s idempotent
        /// post-death record. Terminal: a memorialized survivor never re-enters play.
        /// </summary>
        Memorialized = 4
    }

    /// <summary>The named lifecycle operations. One per legal reason to change state.</summary>
    public enum SurvivorTransition
    {
        /// <summary>A definition becomes a campaign survivor.</summary>
        Join = 0,
        /// <summary>A resident is sent out on an expedition.</summary>
        Deploy = 1,
        /// <summary>An away survivor comes home.</summary>
        Return = 2,
        /// <summary>A living survivor dies.</summary>
        Die = 3,
        /// <summary>A dead survivor is memorialized.</summary>
        Memorialize = 4,
        /// <summary>A resident is removed from the campaign while still alive.</summary>
        Leave = 5
    }

    /// <summary>Stable failure codes. Snake_case so UI and tests can branch without string parsing.</summary>
    public static class SurvivorLifecycleFailure
    {
        public const string IdInvalid = "survivor_id_invalid";
        public const string Unknown = "survivor_unknown";
        public const string AlreadyExists = "survivor_already_exists";
        public const string DefinitionRequired = "survivor_definition_required";
        public const string TransitionIllegal = "lifecycle_transition_illegal";
        public const string AlreadyInState = "lifecycle_already_in_state";
        public const string ExpeditionIdRequired = "expedition_id_required";
        public const string ExpeditionMismatch = "expedition_mismatch";
        public const string ComponentsAttached = "survivor_components_attached";
    }

    /// <summary>
    /// Outcome of one lifecycle transaction. Follows the house result shape
    /// (<see cref="ActionResult"/>, <c>CommandResult</c>): a readonly struct with a
    /// status, a stable failure code, and a precise message.
    ///
    /// <para><b>A non-committed result guarantees no state changed.</b> Transactions
    /// validate every precondition before the first mutation, so a blocked
    /// transaction cannot leave a survivor half-joined or half-dead.</para>
    /// </summary>
    public readonly struct SurvivorLifecycleResult
    {
        public enum StatusKind
        {
            /// <summary>State changed and post-commit events fired.</summary>
            Committed = 0,
            /// <summary>A precondition failed. Nothing changed.</summary>
            Blocked = 1,
            /// <summary>
            /// The survivor was already in the requested state. Nothing changed and
            /// nothing is wrong — this is how repeated death reports stay idempotent
            /// without re-running a cascade.
            /// </summary>
            AlreadyInState = 2
        }

        public StatusKind Status { get; }
        public SurvivorId SurvivorId { get; }
        public SurvivorTransition Transition { get; }
        public SurvivorLifecycleState From { get; }
        public SurvivorLifecycleState To { get; }
        public string FailureCode { get; }
        public string Message { get; }

        /// <summary>
        /// Aggregate revision after the transaction, or the unchanged current
        /// revision when nothing committed. Lets a caller detect that the survivor
        /// moved underneath a stale read (Phase 34).
        /// </summary>
        public long Revision { get; }

        private SurvivorLifecycleResult(
            StatusKind status,
            SurvivorId survivorId,
            SurvivorTransition transition,
            SurvivorLifecycleState from,
            SurvivorLifecycleState to,
            string failureCode,
            string message,
            long revision)
        {
            Status = status;
            SurvivorId = survivorId;
            Transition = transition;
            From = from;
            To = to;
            FailureCode = failureCode ?? string.Empty;
            Message = message ?? string.Empty;
            Revision = revision;
        }

        /// <summary>True when state changed.</summary>
        public bool IsCommitted => Status == StatusKind.Committed;

        /// <summary>True when nothing changed because a precondition failed.</summary>
        public bool IsBlocked => Status == StatusKind.Blocked;

        /// <summary>
        /// True when the caller's intent already holds — committed, or already in
        /// the requested state. The right check for idempotent callers such as the
        /// death cascade.
        /// </summary>
        public bool IsSatisfied => Status == StatusKind.Committed || Status == StatusKind.AlreadyInState;

        public static SurvivorLifecycleResult Committed(
            SurvivorId id,
            SurvivorTransition transition,
            SurvivorLifecycleState from,
            SurvivorLifecycleState to,
            long revision)
            => new SurvivorLifecycleResult(
                StatusKind.Committed, id, transition, from, to, string.Empty, string.Empty, revision);

        public static SurvivorLifecycleResult Blocked(
            SurvivorId id,
            SurvivorTransition transition,
            SurvivorLifecycleState from,
            string failureCode,
            string message,
            long revision = 0L)
            => new SurvivorLifecycleResult(
                StatusKind.Blocked, id, transition, from, from, failureCode, message, revision);

        public static SurvivorLifecycleResult AlreadyIn(
            SurvivorId id,
            SurvivorTransition transition,
            SurvivorLifecycleState state,
            long revision)
            => new SurvivorLifecycleResult(
                StatusKind.AlreadyInState, id, transition, state, state,
                SurvivorLifecycleFailure.AlreadyInState,
                $"Survivor '{id}' is already {state}.",
                revision);

        public override string ToString()
            => Status == StatusKind.Committed
                ? $"[Committed] {SurvivorId}: {From} -> {To} via {Transition} (rev {Revision})"
                : $"[{Status}] {SurvivorId}: {Transition} refused from {From} — {FailureCode}: {Message}";
    }

    /// <summary>
    /// The survivor lifecycle transition table and the eligibility questions other
    /// domains need to ask. Pure, allocation-free, and the single place the rules live.
    /// </summary>
    public static class SurvivorLifecycle
    {
        /// <summary>Every state a live aggregate may hold, in declaration order.</summary>
        public static readonly SurvivorLifecycleState[] LegalStates =
        {
            SurvivorLifecycleState.Resident,
            SurvivorLifecycleState.Away,
            SurvivorLifecycleState.Dead,
            SurvivorLifecycleState.Memorialized
        };

        /// <summary>
        /// True when <paramref name="state"/> is a legal state for a stored
        /// aggregate. <see cref="SurvivorLifecycleState.Unknown"/> is not.
        /// </summary>
        public static bool IsLegalState(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Resident
            || state == SurvivorLifecycleState.Away
            || state == SurvivorLifecycleState.Dead
            || state == SurvivorLifecycleState.Memorialized;

        /// <summary>
        /// True when the survivor is alive. The one question that used to be
        /// answered independently by the roster, needs, radiation, and the
        /// medical ward.
        /// </summary>
        public static bool IsAlive(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Resident || state == SurvivorLifecycleState.Away;

        /// <summary>True once the survivor is dead, memorialized or not.</summary>
        public static bool IsDeceased(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Dead || state == SurvivorLifecycleState.Memorialized;

        /// <summary>True when the survivor is physically in the shelter.</summary>
        public static bool IsInShelter(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Resident;

        /// <summary>True when the survivor is out of the shelter on an expedition.</summary>
        public static bool IsDeployed(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Away;

        /// <summary>
        /// True when the survivor may hold an active shelter duty (Phase 17).
        /// Only a resident can: someone away on an expedition is not present to
        /// work a shift, and the dead hold no duties.
        ///
        /// <para><b>Declared, not yet enforced.</b> <c>DutyRosterSystem</c> has no
        /// expedition awareness today, so an away survivor can still hold a duty
        /// row. <see cref="SurvivorIntegrityValidator"/> reports that as a
        /// <see cref="SurvivorIntegritySeverity.Warning"/> rather than an error until
        /// the assignment domain is migrated.</para>
        /// </summary>
        public static bool IsAssignmentEligible(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Resident;

        /// <summary>True when the survivor may be sent on an expedition.</summary>
        public static bool IsDeploymentEligible(SurvivorLifecycleState state)
            => state == SurvivorLifecycleState.Resident;

        /// <summary>
        /// True when the survivor should still be ticked by the survival
        /// simulation (needs decay, radiation dose). Deployed survivors keep
        /// accumulating both, so this is exactly <see cref="IsAlive"/> — named
        /// separately because the intent differs at call sites.
        /// </summary>
        public static bool IsSimulated(SurvivorLifecycleState state) => IsAlive(state);

        // ── Transition table ───────────────────────────────────────────

        /// <summary>
        /// The state a transition leads to from <paramref name="from"/>, or null
        /// when the transition is illegal there. <see cref="SurvivorTransition.Join"/>
        /// has no source state and always answers <see cref="SurvivorLifecycleState.Resident"/>;
        /// <see cref="SurvivorTransition.Leave"/> removes the aggregate entirely and
        /// therefore has no destination state.
        /// </summary>
        public static SurvivorLifecycleState? Destination(SurvivorLifecycleState from, SurvivorTransition transition)
        {
            switch (transition)
            {
                case SurvivorTransition.Join:
                    return SurvivorLifecycleState.Resident;

                case SurvivorTransition.Deploy:
                    return from == SurvivorLifecycleState.Resident
                        ? SurvivorLifecycleState.Away
                        : (SurvivorLifecycleState?)null;

                case SurvivorTransition.Return:
                    return from == SurvivorLifecycleState.Away
                        ? SurvivorLifecycleState.Resident
                        : (SurvivorLifecycleState?)null;

                case SurvivorTransition.Die:
                    // A survivor can die at home or in the field. Both are common.
                    return (from == SurvivorLifecycleState.Resident || from == SurvivorLifecycleState.Away)
                        ? SurvivorLifecycleState.Dead
                        : (SurvivorLifecycleState?)null;

                case SurvivorTransition.Memorialize:
                    return from == SurvivorLifecycleState.Dead
                        ? SurvivorLifecycleState.Memorialized
                        : (SurvivorLifecycleState?)null;

                case SurvivorTransition.Leave:
                    // Removes the aggregate; no destination state.
                    return null;

                default:
                    return null;
            }
        }

        /// <summary>True when <paramref name="transition"/> is legal from <paramref name="from"/>.</summary>
        public static bool IsLegalTransition(SurvivorLifecycleState from, SurvivorTransition transition)
            => transition == SurvivorTransition.Leave
                ? from == SurvivorLifecycleState.Resident
                : Destination(from, transition).HasValue;

        /// <summary>
        /// Every legal transition out of <paramref name="from"/>. Enumerable so tests
        /// can assert the table exhaustively rather than sampling it.
        /// </summary>
        public static IEnumerable<SurvivorTransition> LegalTransitionsFrom(SurvivorLifecycleState from)
        {
            foreach (SurvivorTransition t in new[]
            {
                SurvivorTransition.Deploy,
                SurvivorTransition.Return,
                SurvivorTransition.Die,
                SurvivorTransition.Memorialize,
                SurvivorTransition.Leave
            })
            {
                if (IsLegalTransition(from, t)) yield return t;
            }
        }

        /// <summary>
        /// Why a transition is refused, phrased for a log line a human has to read
        /// at 2am (Phase 71).
        /// </summary>
        public static string DescribeIllegal(SurvivorId id, SurvivorLifecycleState from, SurvivorTransition transition)
        {
            string reason = transition switch
            {
                SurvivorTransition.Deploy when from == SurvivorLifecycleState.Away
                    => "already deployed",
                SurvivorTransition.Deploy when IsDeceased(from)
                    => "the dead cannot be deployed",
                SurvivorTransition.Return when from == SurvivorLifecycleState.Resident
                    => "not deployed",
                SurvivorTransition.Return when IsDeceased(from)
                    => "the dead do not return",
                SurvivorTransition.Die when IsDeceased(from)
                    => "already dead",
                SurvivorTransition.Memorialize when IsAlive(from)
                    => "cannot memorialize a living survivor",
                SurvivorTransition.Memorialize when from == SurvivorLifecycleState.Memorialized
                    => "already memorialized",
                SurvivorTransition.Leave when from == SurvivorLifecycleState.Away
                    => "cannot leave the campaign while deployed; recall or resolve the expedition first",
                SurvivorTransition.Leave when IsDeceased(from)
                    => "the dead have already left",
                _ => "not a legal transition from this state"
            };

            return $"Survivor '{id}' is {from}; {transition} refused — {reason}.";
        }
    }
}
