// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Resolved message-stage view for one distress signal at one campaign day.
    /// TEST CONTRACT: A message stage is a deterministic presentation/intelligence
    /// view of one canonical distress signal. It is not quest progression and must
    /// not mutate quest, expedition, trust, or encounter state.
    /// </summary>
    public readonly struct DistressStageView
    {
        /// <summary>Index of the audible fragment in
        /// <see cref="DistressSignalDefinition.MessageFragments"/>.</summary>
        public int StageIndex { get; }

        /// <summary>The audible fragment. Never null on a successful resolve.</summary>
        public DistressMessageFragment Fragment { get; }

        public DistressStageView(int stageIndex, DistressMessageFragment fragment)
        {
            StageIndex = stageIndex;
            Fragment = fragment;
        }
    }

    /// <summary>
    /// Tasks 9–12 Wave 1: the single pure stage resolver for distress signals.
    ///
    /// <para><c>message_fragments</c> is the authoritative stage model (Wave 0
    /// decision gate, PC-2): each fragment is one presentation/intelligence stage
    /// of the same canonical signal identity. Stage selection is purely derivable
    /// from the current campaign day — no stage index is persisted anywhere.</para>
    ///
    /// <para>Anchor semantics (documented, both legacy consumers):
    /// fragment <c>day</c> is an ABSOLUTE campaign-day threshold. The audible
    /// stage is the first fragment index carrying the highest day that is
    /// <c>&lt;= campaignDay</c>. When no fragment threshold is satisfied yet,
    /// stage 0 is audible (legacy fallback: <c>fragments[0]</c>). Stage days must
    /// be strictly ascending and clarity non-decreasing — enforced by the
    /// data-integrity validator — so stages can never regress as campaign time
    /// is monotonic, and multi-day skips advance directly to the correct stage.</para>
    ///
    /// <para>Invariants: side-effect free, deterministic, no audio calls, no quest
    /// mutation, no trust mutation. Previously this selection loop was duplicated
    /// in <see cref="RadioTuner"/> and <see cref="RadioPropagation"/>; both now
    /// delegate here so the contract cannot drift.</para>
    /// </summary>
    public static class DistressStageResolver
    {
        /// <summary>
        /// Resolves the audible stage for a signal at a campaign day.
        /// Returns null when the signal (or its fragment list) is empty —
        /// callers keep their legacy no-fragment fallback (e.g. source name).
        /// </summary>
        public static DistressStageView? Resolve(DistressSignalDefinition? signal, int campaignDay)
        {
            if (signal?.MessageFragments == null || signal.MessageFragments.Count == 0)
                return null;
            int index = ResolveStageIndex(signal, campaignDay);
            return new DistressStageView(index, signal.MessageFragments[index]);
        }

        /// <summary>
        /// Resolves only the audible stage index. Returns -1 when the signal has
        /// no fragments (callers keep their legacy no-fragment fallback).
        /// Selection: first index of the highest fragment day that is
        /// <c>&lt;= campaignDay</c>; falls back to index 0 when no threshold is
        /// satisfied yet. Ties on equal day keep the first occurrence — this
        /// matches the legacy duplicated loops exactly; authored data is
        /// validator-enforced to strictly ascending days, so ties cannot occur
        /// in production catalogs.
        /// </summary>
        public static int ResolveStageIndex(DistressSignalDefinition? signal, int campaignDay)
        {
            var fragments = signal?.MessageFragments;
            if (fragments == null || fragments.Count == 0) return -1;

            int best = -1;
            for (int i = 0; i < fragments.Count; i++)
            {
                if (fragments[i].Day > campaignDay) continue;
                if (best == -1 || fragments[i].Day > fragments[best].Day) best = i;
            }
            // Legacy fallback: before the first authored threshold the opening
            // fragment is already audible.
            if (best == -1) best = 0;
            return best;
        }
    }
}
