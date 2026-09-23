// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 54 — The Seven-Day Slice playtest instrument in the running game.
//
// The instrument is loaded from the authored scenario and its evidence (the
// frozen scenario hash plus the last scorecard) is captured through the
// registered "seven_day_slice" section. It never simulates the campaign: beat
// verification is a measurement against the named required systems.
// ============================================================================
using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Campaign;

namespace AtomicWar.GodotApp
{
    public partial class Main : Control
    {
        private SliceScenarioHostSession? _sliceScenario;
        private bool _sliceScenarioDirty;

        public SliceScenarioHostSession? SliceScenario => _sliceScenario;

        /// <summary>
        /// Load the authored seven-day slice instrument. A missing or invalid
        /// scenario leaves the instrument absent (logged) rather than blocking a
        /// live campaign: the data authority owns the playtest, not the game.
        /// </summary>
        private SliceScenarioHostSession? EnsureSliceScenario()
        {
            if (_sliceScenario != null) return _sliceScenario;
            try
            {
                var session = SliceScenarioHostSession.Load(_dataDir, new FileSystemIO());
                session.RestoreEvidence(SliceScenarioSaveStore.TryLoad());
                _sliceScenario = session;
            }
            catch (Exception ex)
            {
                GD.PrintErr($"[Slice] seven-day slice unavailable: {ex.Message}");
                _sliceScenario = null;
            }
            return _sliceScenario;
        }

        private void SetupSevenDaySlice()
        {
            var session = EnsureSliceScenario();
            if (session == null) return;
            GD.Print($"[Slice] {session.Scenario.Data.Title} loaded "
                + $"({session.Scenario.Data.TargetDays} day(s), hash {session.ContentHash.Substring(0, 12)}…).");
        }

        private void SaveSevenDaySlice()
        {
            if (_sliceScenario?.LastScorecard == null) return;
            CaptureSection(
                SliceScenarioSaveStore.SectionName,
                SliceScenarioSaveStore.TryCapturePersisted(
                    SliceScenarioSaveStore.From(_sliceScenario.LastScorecard, _sliceScenario.ContentHash)));
            _sliceScenarioDirty = false;
        }

        private void FlushSevenDaySliceIfDirty()
        {
            if (_sliceScenarioDirty) SaveSevenDaySlice();
        }

        private void ResetSevenDaySlice()
        {
            _sliceScenario?.Dispose();
            _sliceScenario = null;
            _sliceScenarioDirty = false;
        }

        /// <summary>Record one measured beat and finalize the slice evidence.</summary>
        internal bool RecordSliceBeat(int day, string actionKey, string systemOutcome, string notes = "")
            => _sliceScenario?.RecordBeat(day, new SliceBeatMeasurement(actionKey, systemOutcome, notes)) ?? false;

        /// <summary>Finalize the playtest slice and persist its scorecard.</summary>
        internal SliceScorecard? CompleteSlicePlaytest(int retainedSurvivors)
        {
            if (_sliceScenario == null) return null;
            var scorecard = _sliceScenario.Complete(retainedSurvivors);
            _sliceScenarioDirty = true;
            SaveSevenDaySlice();

            _journal?.TryAddRawEntry(
                "seven_day_slice_report",
                $"Seven-day slice {scorecard.ScenarioId}: {scorecard.CompletedBeats}/{scorecard.TotalBeats} beat(s) verified, "
                + $"{scorecard.RetainedSurvivors}/{scorecard.InitialSurvivors} survivor(s) retained — "
                + (scorecard.Passed ? "slice passed." : "slice did not pass."),
                null!, Math.Max(1, _simDay));
            return scorecard;
        }
    }
}
