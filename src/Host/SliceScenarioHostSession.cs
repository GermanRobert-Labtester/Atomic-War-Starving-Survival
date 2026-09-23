// SPDX-License-Identifier: MIT
// ============================================================================
// Plan 54 / C2[20]-family — The Seven-Day Slice playtest instrument.
//
// Authority boundary (deliberate): SliceScenario is the beat-verification,
// freeze-hash and scorecard authority. This adapter binds it to the authored
// scenario file and to the live canonical owner that each beat names, then
// reports the scorecard. It adds no simulation of its own: a beat passes only
// when the named required system produced the expected outcome.
//
// Beat verification is driven by a per-beat *measurement* the host takes from
// the running campaign, so a green scorecard is real evidence and not a
// self-reported constant.
// ============================================================================
using System;
using System.Collections.Generic;
using System.IO;
using Ashfall.Core;
using Ashfall.Core.Campaign;
using Ashfall.Core.Save;

namespace AtomicWar.GodotApp
{
    /// <summary>One beat's live measurement, taken from a canonical owner.</summary>
    public readonly struct SliceBeatMeasurement
    {
        public readonly string ActionKey;
        public readonly string SystemOutcome;
        public readonly string Notes;

        public SliceBeatMeasurement(string actionKey, string systemOutcome, string notes = "")
        {
            ActionKey = actionKey ?? string.Empty;
            SystemOutcome = systemOutcome ?? string.Empty;
            Notes = notes ?? string.Empty;
        }
    }

    public sealed class SliceScenarioHostSession : HostSessionBase
    {
        /// <summary>Authored scenario file in the data authority.</summary>
        public const string ScenarioFile = "slice_seven_days.json";

        private readonly List<SliceBeatMeasurement> _measurements = new();

        public SliceScenario Scenario { get; }
        public SliceScorecard? LastScorecard { get; private set; }

        public SliceScenarioHostSession(SliceScenario scenario)
        {
            Scenario = scenario ?? throw new ArgumentNullException(nameof(scenario));
        }

        /// <summary>
        /// Load the authored scenario through the strict data loader path. A
        /// missing or invalid file is a hard failure: the instrument never
        /// invents beats to make itself pass.
        /// </summary>
        public static SliceScenarioHostSession Load(string dataDir, IFileIO files)
        {
            var loaded = SliceScenarioCatalogLoader.Load(dataDir, files);
            if (loaded.HasErrors || loaded.Scenario == null)
                throw new InvalidDataException(string.Join("; ", loaded.Errors));

            return new SliceScenarioHostSession(new SliceScenario(loaded.Scenario));
        }

        /// <summary>
        /// Verify the authored content hash so a modified scenario cannot be
        /// silently replayed as the canonical slice.
        /// </summary>
        public bool IsFrozen(string expectedHash)
            => !string.IsNullOrEmpty(expectedHash) && Scenario.IsScenarioFrozen(expectedHash);

        /// <summary>Record one beat's live measurement and let the authority verify it.</summary>
        public bool RecordBeat(int day, SliceBeatMeasurement measurement)
        {
            _measurements.Add(measurement);
            bool ok = Scenario.EvaluateDayBeat(day, measurement.ActionKey, measurement.SystemOutcome, out _);
            RaiseStateChanged();
            return ok;
        }

        public IReadOnlyList<SliceBeatMeasurement> Measurements => _measurements;

        /// <summary>Finalize the slice and publish the scorecard.</summary>
        public SliceScorecard Complete(int retainedSurvivors)
        {
            var scorecard = Scenario.CompleteSlice(Math.Max(0, retainedSurvivors));
            LastScorecard = scorecard;
            RaiseStateChanged();
            return scorecard;
        }

        /// <summary>
        /// Restore the last captured scorecard so a campaign that resumes after a
        /// slice run still carries its evidence.
        /// </summary>
        public void RestoreEvidence(SliceScorecardState? state)
        {
            if (state == null || string.IsNullOrWhiteSpace(state.scenario_id)) return;
            if (!string.Equals(state.scenario_id, Scenario.Data.ScenarioId, StringComparison.Ordinal)) return;
            if (state.content_hash != ContentHash) return;

            LastScorecard = new SliceScorecard
            {
                ScenarioId = state.scenario_id,
                CompletedDays = state.beat_results?.Count ?? 0,
                TotalBeats = state.total_beats,
                CompletedBeats = state.completed_beats,
                InitialSurvivors = state.initial_survivors,
                RetainedSurvivors = state.retained_survivors,
                ScenarioContentHash = state.content_hash,
                Passed = state.passed,
                BeatResults = state.beat_results ?? new List<SliceBeatResult>()
            };
            RaiseStateChanged();
        }

        /// <summary>Scenario hash of the loaded content (for reports and freezes).</summary>
        public string ContentHash => Scenario.ComputeScenarioHash();

        public string Describe()
        {
            if (LastScorecard == null)
                return $"slice {Scenario.Data.ScenarioId}: {Scenario.Data.TargetDays} beat(s), not completed";
            return $"slice {LastScorecard.ScenarioId}: {LastScorecard.CompletedBeats}/{LastScorecard.TotalBeats} beat(s) "
                + $"passed, {LastScorecard.RetainedSurvivors}/{LastScorecard.InitialSurvivors} survivor(s) retained, "
                + "verdict " + (LastScorecard.Passed ? "PASS" : "FAIL");
        }
    }

    /// <summary>Strict JSON projection used by the slice probe's save store.</summary>
    public sealed class SliceScorecardState
    {
        public int schema_version { get; set; } = 1;
        public string scenario_id { get; set; } = string.Empty;
        public string content_hash { get; set; } = string.Empty;
        public bool passed { get; set; }
        public int completed_beats { get; set; }
        public int total_beats { get; set; }
        public int initial_survivors { get; set; }
        public int retained_survivors { get; set; }
        public List<SliceBeatResult> beat_results { get; set; } = new();
    }

    public static class SliceScenarioSaveStore
    {
        public const string FileName = "seven_day_slice_save.json";
        public const string SectionName = "seven_day_slice";

        private static readonly SaveStore<SliceScorecardState> s_store =
            SaveStoreHub.Checksummed<SliceScorecardState>(FileName, nameof(SliceScenarioSaveStore));

        public static bool TrySave(SliceScorecardState state) => s_store.TrySave(state);
        public static SliceScorecardState? TryLoad() => s_store.TryLoad();
        public static string TryCapturePersisted(SliceScorecardState state) => s_store.CapturePersisted(state);
        public static SliceScorecardState? TryRestore(string json) => s_store.RestoreBare(json);

        /// <summary>Project a scorecard for persistence (evidence artifact).</summary>
        public static SliceScorecardState From(SliceScorecard scorecard, string contentHash) => new()
        {
            schema_version = 1,
            scenario_id = scorecard.ScenarioId,
            content_hash = contentHash,
            passed = scorecard.Passed,
            completed_beats = scorecard.CompletedBeats,
            total_beats = scorecard.TotalBeats,
            initial_survivors = scorecard.InitialSurvivors,
            retained_survivors = scorecard.RetainedSurvivors,
            beat_results = scorecard.BeatResults ?? new List<SliceBeatResult>()
        };
    }
}
