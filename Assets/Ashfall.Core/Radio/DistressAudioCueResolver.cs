// SPDX-License-Identifier: MIT
using System;

namespace Ashfall.Core.Radio
{
    /// <summary>
    /// Tasks 9–12 Wave 4 — pure distress-audio cue resolver.
    ///
    /// <para>Audio is a PRESENTATION PROJECTION of authoritative radio state:
    /// <c>signal definition + current stage → cue ID</c>. Selection precedence
    /// (plan Option A — stage-level overrides, no second clarity-band system):
    /// </para>
    /// <list type="number">
    /// <item>the current stage's <c>audio_cue</c> override when authored;</item>
    /// <item>the signal's default <c>audio_cue</c>;</item>
    /// <item>empty string = text-only fallback (the transmission remains fully
    /// usable without audio — the radio is never audio-only).</item>
    /// </list>
    ///
    /// <para>Invariants: side-effect free, deterministic (explicit IDs — no
    /// RNG, no time, no hash ordering), no AudioManager calls, no gameplay
    /// mutation. Playback must never determine discovery, stage progression,
    /// trust, quest state, follow-up scheduling, or encounter outcomes.</para>
    /// </summary>
    public static class DistressAudioCueResolver
    {
        /// <summary>
        /// Resolves the audio cue for a signal at a stage index. Returns the
        /// empty string when no cue is authored (text-only fallback). Safe for
        /// unknown signals and out-of-range stage indices (falls back to the
        /// signal default, then empty).
        /// </summary>
        public static string Resolve(DistressSignalDefinition? signal, int stageIndex)
        {
            if (signal == null) return string.Empty;

            var fragments = signal.MessageFragments;
            if (fragments != null && stageIndex >= 0 && stageIndex < fragments.Count)
            {
                string stageCue = fragments[stageIndex].AudioCue;
                if (!string.IsNullOrWhiteSpace(stageCue)) return stageCue;
            }

            return string.IsNullOrWhiteSpace(signal.AudioCue) ? string.Empty : signal.AudioCue;
        }

        /// <summary>
        /// Convenience overload: resolves the stage via
        /// <see cref="DistressStageResolver"/> first, then the cue. Keeps the
        /// stage→cue pipeline in one call for host adapters.
        /// </summary>
        public static string ResolveForDay(DistressSignalDefinition? signal, int campaignDay)
        {
            if (signal == null) return string.Empty;
            int stageIndex = DistressStageResolver.ResolveStageIndex(signal, campaignDay);
            return Resolve(signal, stageIndex);
        }
    }
}
