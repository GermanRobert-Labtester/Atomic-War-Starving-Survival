using System;
using System.Collections.Generic;
using System.Threading;

namespace Ashfall.Core
{
    /// <summary>
    /// Deterministic event ID counter for <see cref="ActionResult"/>.
    /// Uses a thread-safe incrementing counter seeded from ticks so IDs are
    /// stable within a session and unique across actions.
    /// </summary>
    internal static class ActionEventIdCounter
    {
        private static long _counter = Environment.TickCount64 & 0x3FFFFFFF;
        internal static long Next() => Interlocked.Increment(ref _counter);
        internal static string NextString() => $"aev{Next():x}";
    }

    /// <summary>
    /// Typed result for every player-initiated action in ASHFALL.
    ///
    /// All new player commands must return one of these subtypes so that
    /// the host can branch on <see cref="Status"/> without parsing strings.
    /// Existing string-returning host methods are compatibility wrappers;
    /// live campaign UI moves to typed results.
    ///
    /// Every action validates before mutating. Inventory consumption,
    /// equipment reservation, research progress, resource production,
    /// and reward delivery commit atomically. Repeated commands, reloads,
    /// and modal reopenings must not duplicate rewards or consume inputs twice.
    /// </summary>
    public readonly struct ActionResult
    {
        /// <summary>Enumeration of possible action outcomes.</summary>
        public enum StatusKind
        {
            /// <summary>Action succeeded and state was mutated.</summary>
            Success,
            /// <summary>Action was blocked by a known precondition failure.</summary>
            Blocked,
            /// <summary>Action failed due to an unexpected error or invalid state.</summary>
            Failed,
            /// <summary>Action was cancelled by the player before mutation.</summary>
            Cancelled,
            /// <summary>Action partially completed (e.g. partial refund on dismantle).</summary>
            Partial
        }

        /// <summary>The outcome kind.</summary>
        public StatusKind Status { get; }

        /// <summary>
        /// Stable error/failure code for UI branching.
        /// Empty on <see cref="StatusKind.Success"/>.
        /// </summary>
        public string FailureCode { get; }

        /// <summary>Player-facing message key for localization.</summary>
        public string MessageKey { get; }

        /// <summary>
        /// Resource/state deltas produced by this action.
        /// Key is a snake_case delta identifier; value is the numeric change.
        /// Example: "scrap" => -5, "clean_water" => 10, "research_progress" => 0.25
        /// </summary>
        public IReadOnlyDictionary<string, double> Deltas { get; }

        /// <summary>Stable event identifier for audit, journal, and replay.</summary>
        public string EventId { get; }

        /// <summary>Optional inner result event ID for composite action tracking.</summary>
        public string InnerEventId { get; }

        private ActionResult(
            StatusKind status,
            string failureCode,
            string messageKey,
            IReadOnlyDictionary<string, double>? deltas,
            string eventId,
            string innerEventId)
        {
            Status = status;
            FailureCode = failureCode ?? string.Empty;
            MessageKey = messageKey ?? string.Empty;
            Deltas = deltas ?? new Dictionary<string, double>();
            EventId = eventId ?? string.Empty;
            InnerEventId = innerEventId ?? string.Empty;
        }

        /// <summary>Create a success result.</summary>
        public static ActionResult Success(
            string messageKey,
            IReadOnlyDictionary<string, double>? deltas = null,
            string? eventId = null,
            string? innerEventId = null)
        {
            return new ActionResult(
                StatusKind.Success,
                string.Empty,
                messageKey,
                deltas,
                eventId ?? ActionEventIdCounter.NextString(),
                innerEventId ?? string.Empty);
        }

        /// <summary>Create a blocked result (known precondition failure).</summary>
        public static ActionResult Blocked(
            string failureCode,
            string messageKey,
            IReadOnlyDictionary<string, double>? deltas = null,
            string? eventId = null)
        {
            return new ActionResult(
                StatusKind.Blocked,
                failureCode,
                messageKey,
                deltas,
                eventId ?? string.Empty,
                string.Empty);
        }

        /// <summary>Create a failed result (unexpected error).</summary>
        public static ActionResult Failed(
            string failureCode,
            string messageKey,
            string? eventId = null)
        {
            return new ActionResult(
                StatusKind.Failed,
                failureCode,
                messageKey,
                null,
                eventId ?? string.Empty,
                string.Empty);
        }

        /// <summary>Create a cancelled result.</summary>
        public static ActionResult Cancelled(
            string messageKey,
            string? eventId = null)
        {
            return new ActionResult(
                StatusKind.Cancelled,
                string.Empty,
                messageKey,
                null,
                eventId ?? string.Empty,
                string.Empty);
        }

        /// <summary>Create a partial success result.</summary>
        public static ActionResult Partial(
            string messageKey,
            IReadOnlyDictionary<string, double>? deltas = null,
            string? failureCode = null,
            string? eventId = null)
        {
            return new ActionResult(
                StatusKind.Partial,
                failureCode ?? string.Empty,
                messageKey,
                deltas,
                eventId ?? string.Empty,
                string.Empty);
        }

        /// <summary>True if the action succeeded or partially succeeded.</summary>
        public bool IsSuccessOrPartial => Status == StatusKind.Success || Status == StatusKind.Partial;

        public override string ToString()
        {
            return $"[{Status}] {MessageKey} (event={EventId}, failure={FailureCode})";
        }
    }
}
