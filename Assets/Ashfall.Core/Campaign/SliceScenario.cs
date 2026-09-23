#nullable enable
using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Ashfall.Core.Campaign
{
    /// <summary>
    /// Definition of an authored beat within the 7-day playtest slice.
    /// </summary>
    public sealed class SliceBeatDef
    {
        public int Day { get; set; }
        public string BeatId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string RequiredSystem { get; set; } = string.Empty;
        public string ActionKey { get; set; } = string.Empty;
        public string ExpectedOutcome { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
    }

    /// <summary>
    /// Authored catalog data for the seven-day slice scenario.
    /// </summary>
    public sealed class SliceScenarioData
    {
        public int SchemaVersion { get; set; } = 1;
        public string ScenarioId { get; set; } = string.Empty;
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public int TargetDays { get; set; } = 7;
        public int DefaultSeed { get; set; } = 1337;
        public string DifficultyPreset { get; set; } = "normal";
        public Dictionary<string, int> StartingResources { get; set; } = new Dictionary<string, int>(StringComparer.OrdinalIgnoreCase);
        public int InitialSurvivorCount { get; set; } = 4;
        public List<SliceBeatDef> Beats { get; set; } = new List<SliceBeatDef>();
    }

    /// <summary>
    /// Individual outcome record for an evaluated slice beat.
    /// </summary>
    public sealed class SliceBeatResult
    {
        public int Day { get; set; }
        public string BeatId { get; set; } = string.Empty;
        public bool Success { get; set; }
        public string ActionKey { get; set; } = string.Empty;
        public string SystemOutcome { get; set; } = string.Empty;
        public string Notes { get; set; } = string.Empty;
    }

    /// <summary>
    /// Comprehensive evaluation scorecard for a completed slice playtest.
    /// </summary>
    public sealed class SliceScorecard
    {
        public string ScenarioId { get; set; } = string.Empty;
        public int CompletedDays { get; set; }
        public int TotalBeats { get; set; }
        public int CompletedBeats { get; set; }
        public int InitialSurvivors { get; set; }
        public int RetainedSurvivors { get; set; }
        public string ScenarioContentHash { get; set; } = string.Empty;
        public bool Passed { get; set; }
        public List<SliceBeatResult> BeatResults { get; set; } = new List<SliceBeatResult>();
    }

    /// <summary>
    /// Pure domain engine for Plan 54: The Seven-Day Slice playtest instrument.
    /// Governs beat verification, scenario integrity hashing, and scorecard generation.
    /// Zero engine references (Godot/UnityEngine free).
    /// </summary>
    public sealed class SliceScenario
    {
        private readonly SliceScenarioData _data;
        private readonly Dictionary<int, SliceBeatDef> _beatsByDay = new Dictionary<int, SliceBeatDef>();
        private readonly List<SliceBeatResult> _evaluatedResults = new List<SliceBeatResult>();

        // Seam delegates
        public Action<SliceBeatResult>? OnBeatEvaluatedSeam { get; set; }
        public Action<SliceScorecard>? OnSliceCompletedSeam { get; set; }
        public Action<string, string>? OnBeatValidationFailedSeam { get; set; }

        public SliceScenarioData Data => _data;
        public IReadOnlyList<SliceBeatResult> EvaluatedResults => _evaluatedResults;

        public SliceScenario(SliceScenarioData data)
        {
            _data = data ?? throw new ArgumentNullException(nameof(data));
            foreach (var beat in _data.Beats)
            {
                if (beat != null && beat.Day > 0)
                {
                    _beatsByDay[beat.Day] = beat;
                }
            }
        }

        /// <summary>
        /// Computes a deterministic SHA-256 content hash of the scenario configuration.
        /// </summary>
        public string ComputeScenarioHash()
        {
            var sb = new StringBuilder();
            sb.Append(_data.ScenarioId).Append('|')
              .Append(_data.TargetDays).Append('|')
              .Append(_data.DefaultSeed).Append('|')
              .Append(_data.DifficultyPreset).Append('|')
              .Append(_data.InitialSurvivorCount).Append('|');

            foreach (var kvp in _data.StartingResources)
            {
                sb.Append(kvp.Key).Append(':').Append(kvp.Value).Append(';');
            }
            sb.Append('|');

            foreach (var beat in _data.Beats)
            {
                sb.Append(beat.Day).Append(':')
                  .Append(beat.BeatId).Append(':')
                  .Append(beat.RequiredSystem).Append(':')
                  .Append(beat.ActionKey).Append(':')
                  .Append(beat.ExpectedOutcome).Append(';');
            }

            using var sha256 = SHA256.Create();
            byte[] bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(sb.ToString()));
            var hashSb = new StringBuilder();
            foreach (byte b in bytes)
            {
                hashSb.Append(b.ToString("x2"));
            }
            return hashSb.ToString();
        }

        /// <summary>
        /// Verifies whether the current scenario definition matches the expected frozen hash.
        /// </summary>
        public bool IsScenarioFrozen(string expectedHash)
        {
            if (string.IsNullOrEmpty(expectedHash)) return false;
            return string.Equals(ComputeScenarioHash(), expectedHash, StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Evaluates a beat for the specified day against authored requirements.
        /// </summary>
        public bool EvaluateDayBeat(int day, string actionKey, string systemOutcome, out SliceBeatResult result)
        {
            if (!_beatsByDay.TryGetValue(day, out var beatDef))
            {
                result = new SliceBeatResult
                {
                    Day = day,
                    BeatId = "unknown",
                    Success = false,
                    ActionKey = actionKey ?? string.Empty,
                    SystemOutcome = systemOutcome ?? string.Empty,
                    Notes = $"No beat defined for day {day}"
                };
                OnBeatValidationFailedSeam?.Invoke(day.ToString(), result.Notes);
                _evaluatedResults.Add(result);
                OnBeatEvaluatedSeam?.Invoke(result);
                return false;
            }

            bool actionMatch = string.Equals(beatDef.ActionKey, actionKey, StringComparison.OrdinalIgnoreCase);
            bool outcomeMatch = string.Equals(beatDef.ExpectedOutcome, systemOutcome, StringComparison.OrdinalIgnoreCase);
            bool success = actionMatch && outcomeMatch;

            string notes = success
                ? $"Beat {beatDef.BeatId} passed on day {day}"
                : $"Mismatch: expected Action={beatDef.ActionKey}, Outcome={beatDef.ExpectedOutcome}; got Action={actionKey}, Outcome={systemOutcome}";

            result = new SliceBeatResult
            {
                Day = day,
                BeatId = beatDef.BeatId,
                Success = success,
                ActionKey = actionKey ?? string.Empty,
                SystemOutcome = systemOutcome ?? string.Empty,
                Notes = notes
            };

            if (!success)
            {
                OnBeatValidationFailedSeam?.Invoke(beatDef.BeatId, notes);
            }

            _evaluatedResults.Add(result);
            OnBeatEvaluatedSeam?.Invoke(result);
            return success;
        }

        /// <summary>
        /// Finalizes the seven-day slice and constructs the conclusive scorecard.
        /// </summary>
        public SliceScorecard CompleteSlice(int retainedSurvivors)
        {
            int completedBeats = 0;
            foreach (var r in _evaluatedResults)
            {
                if (r.Success) completedBeats++;
            }

            bool passed = completedBeats == _data.TargetDays && retainedSurvivors > 0;

            var scorecard = new SliceScorecard
            {
                ScenarioId = _data.ScenarioId,
                CompletedDays = _evaluatedResults.Count,
                TotalBeats = _data.TargetDays,
                CompletedBeats = completedBeats,
                InitialSurvivors = _data.InitialSurvivorCount,
                RetainedSurvivors = retainedSurvivors,
                ScenarioContentHash = ComputeScenarioHash(),
                Passed = passed,
                BeatResults = new List<SliceBeatResult>(_evaluatedResults)
            };

            OnSliceCompletedSeam?.Invoke(scorecard);
            return scorecard;
        }
    }
}
