// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 177/179 — sleep / nightmare narrative beat
// Subsystems   : reads SurvivorMentalHealthRecord + insomnia and writes
//                journal presentation only.
// Contract     : no DreamSystem, no profile ledger, no state mutation. The
//                journal's knowledge-key dedup bounds this to one entry per
//                survivor per severity.
// ============================================================================
using Ashfall.Core.Needs;
using Ashfall.Core.Random;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>
        /// Daily sleep beat: one read-only projection per living survivor.
        /// Restful sleep stays out of the journal; trouble is recorded once per
        /// survivor (journal knowledge-key dedup), so this never becomes a log.
        /// </summary>
        private void TickSleepNarrative(int day)
        {
            var mental = EnsureSurvivorMentalHealth();
            var roster = _survivors?.RosterState;
            if (mental == null || roster == null || _journal == null) return;

            // Day-keyed fork: the same day and seed always pick the same line.
            var rng = _campaignDay != null
                ? _campaignDay.Rng.Fork(CampaignStreamIds.Psychology, day, 11)
                : null;
            var projection = new SleepNarrativeProjection(mental, rng);

            for (int i = 0; i < roster.Count; i++)
            {
                var survivor = roster[i];
                if (survivor == null || !survivor.IsAliveState) continue;

                var beat = projection.Project(survivor.Id, day);
                if (!beat.Emitted) continue;

                _journal.TryAddRawEntry(beat.JournalKey, beat.Text, null!, day);
            }
        }
    }
}
