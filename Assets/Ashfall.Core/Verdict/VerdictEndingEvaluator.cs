using System.Collections.Generic;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — authoritative ending selection.
    /// Derives the verdict ending from real persisted state (ReckoningState
    /// resolution flags + evidence ledger count), never from fragile text-ID
    /// scans. Mirrors EpilogueMatrixRuntime.EvaluateRegionalFate semantics so
    /// the base-game epilogue also sees tempestDecommissioned.
    /// </summary>
    public static class VerdictEndingEvaluator
    {
        /// <summary>The three Verdict ending keys, in selection priority order.</summary>
        public static readonly string[] EndingKeys =
        {
            "ending_verdict_the_sector_recounts",
            "ending_verdict_the_count_is_held",
            "ending_verdict_the_offer_is_a_lease"
        };

        public const string EndingKeyCounted = "ending_verdict_the_sector_recounts";
        public const string EndingKeyHeld = "ending_verdict_the_count_is_held";
        public const string EndingKeyLease = "ending_verdict_the_offer_is_a_lease";

        /// <summary>Evidence count below which the 'held' ending is the weak-but-honest outcome.</summary>
        public const int MinimumEvidenceForRecount = 4;

        /// <summary>
        /// Returns the currently-resolved verdict ending key (from ReckoningState),
        /// or the enforced fallback if state is contradictory. Null if none resolved.
        /// </summary>
        public static string ResolvedEnding(ReckoningState state)
        {
            if (state == null) return null;
            if (state.countPresented) return EndingKeyCounted;
            if (state.countHeld) return EndingKeyHeld;
            if (state.offerIsLease) return EndingKeyLease;
            return null;
        }

        /// <summary>
        /// Decides the ending a player would get at this point (used for the final
        /// choice and epilogue matrix). Prefers the player's explicit selection;
        /// falls back by evidence sufficiency. Idempotent and deterministic.
        /// </summary>
        public static string DecideEnding(ReckoningState state, int enrolledEvidence, int day)
        {
            string resolved = ResolvedEnding(state);
            if (!string.IsNullOrEmpty(resolved)) return resolved;

            if (state == null || state.phase < ReckoningPhase.Counted)
                return null; // the count has not been presented; no ending yet

            // The count is presented (Counted) but no explicit choice was made:
            // derive from evidence sufficiency — the sector that read enough evidence
            // honors the count; a sparse ledger holds it; a lease is never chosen unseen.
            return enrolledEvidence >= MinimumEvidenceForRecount
                ? EndingKeyCounted
                : EndingKeyHeld;
        }

        /// <summary>Base-game integration: the Tempest was decommissioned exactly when the sector recounted.</summary>
        public static bool IsTempestDecommissioned(ReckoningState state)
        {
            string resolved = ResolvedEnding(state);
            return resolved == EndingKeyCounted;
        }
    }
}
