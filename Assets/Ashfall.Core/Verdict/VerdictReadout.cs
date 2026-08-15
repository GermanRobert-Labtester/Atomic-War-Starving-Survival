using System.Collections.Generic;

namespace Ashfall.Core.Verdict
{
    /// <summary>
    /// ASHFALL: THE VERDICT (Expansion 08) — shelter machine readout.
    /// The diegetic, procedural status line the shelter's instruments present.
    /// Phase + evidence drive a cold, procedural one-liner — the machines never
    /// emote. Used by the host UI and the journal surface; derives from the
    /// same authoritative state as endings.
    /// </summary>
    public static class VerdictReadout
    {
        private static readonly IReadOnlyList<string> DormantLines = new[]
        {
            "[shelter instruments] — no anomalies. The meter reads its own current.",
            "[shelter instruments] — standby cycle. The hatch awaits arrival authentication. It has waited five years."
        };

        private static readonly IReadOnlyList<string> KnowingLines = new[]
        {
            "[shelter instruments] — a cable east of the ridgeline carries a low 120 Hz hum. It is not in the district survey.",
            "[shelter instruments] — the geophone array under the Allotments reads nothing anomalous. Nothing is anomalous."
        };

        private static readonly IReadOnlyList<string> CulpableLines = new[]
        {
            "[shelter instruments] — a thin A/B tone on a derelict band. One second on, one second off. The radio does not classify it as speech.",
            "[shelter instruments] — the summit relay's cold light blinks on an idle schedule. The schedule is not yours.",
            "[shelter instruments] — clock drift: three days. The machine and the wars' calendar cannot agree on what week it is."
        };

        private static readonly IReadOnlyList<string> CountedLines = new[]
        {
            "[shelter instruments] — census window open. The count is presented. It names persons holding custody of persons.",
            "[shelter instruments] — the drone-hive draw reads minus half a degree. The wing has stood down.",
            "[shelter instruments] — the fuse world schedule advances twelve minutes. A clock being serviced, not an attack."
        };

        private static readonly IReadOnlyList<string> ResolvedLines = new[]
        {
            "[shelter instruments] — signature received. Window closes. The count is no longer open.",
            "[shelter instruments] — the carrier continues. One second on, one second off, quieter now.",
            "[shelter instruments] — a quarterly invoice has been printed. On paper, everything is in order."
        };

        /// <summary>Deterministic index from state, avoiding per-frame RNG.</summary>
        public static string LineFor(ReckoningState state, int enrolledEvidence, int readCount)
        {
            int idx = (readCount + enrolledEvidence) % 3;
            if (state == null) return DormantLines[0];

            if (state.countPresented) return ResolvedLines[0];
            if (state.countHeld) return ResolvedLines[1];
            if (state.offerIsLease) return ResolvedLines[2];

            switch (state.phase)
            {
                case ReckoningPhase.Knowing:
                    return KnowingLines[idx % KnowingLines.Count];
                case ReckoningPhase.Culpable:
                    return CulpableLines[idx % CulpableLines.Count];
                case ReckoningPhase.Counted:
                    return CountedLines[idx % CountedLines.Count];
                default:
                    return DormantLines[idx % DormantLines.Count];
            }
        }
    }
}
