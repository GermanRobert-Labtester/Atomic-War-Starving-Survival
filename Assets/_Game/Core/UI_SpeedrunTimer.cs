// UI_SpeedrunTimer.cs — Speedrun Timer UI (Prompt #863)
// Minimalist on-screen UI tracking real-time and in-game days.
// Records splits for community leaderboards.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Speedrun Timer (Prompt #863).
    /// Tracks real-world seconds and in-game days with named splits.
    /// </summary>
    [Serializable]
    public class SpeedrunTimerState
    {
        public string ui_id = "ui_speedrun_timer";
        public bool is_active;
        public float real_time_elapsed;
        public int in_game_days;
        public List<SpeedrunSplit> splits = new List<SpeedrunSplit>();
        public bool is_paused;
    }

    /// <summary>
    /// A single recorded split in a speedrun.
    /// </summary>
    [Serializable]
    public class SpeedrunSplit
    {
        public string name;
        public float real_time;
        public int in_game_day;

        public SpeedrunSplit() { }

        public SpeedrunSplit(string n, float rt, int day)
        {
            name = n;
            real_time = rt;
            in_game_day = day;
        }
    }

    /// <summary>
    /// Speedrun Timer (Prompt #863).
    /// Tracks real-world seconds and in-game days.
    /// Pre-defined splits: "Day 30 Flashpoint", "First Mega-Project",
    /// "First Death", "Bunker Level 5", "True Ending".
    /// Can export splits to clipboard.
    /// </summary>
    public class UI_SpeedrunTimer
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action OnTimerStarted;
        public event Action<string, float, int> OnSplitRecorded;
        public event Action OnTimerPaused;
        public event Action OnTimerResumed;
        public event Action<float, string[]> OnRunCompleted;

        // ── Pre-defined splits ─────────────────────────────────────────
        private static readonly string[] PreDefinedSplits =
        {
            "Day 30 Flashpoint",
            "First Mega-Project",
            "First Death",
            "Bunker Level 5",
            "True Ending"
        };

        // ── State ──────────────────────────────────────────────────────
        private SpeedrunTimerState _state = new SpeedrunTimerState();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Start (or restart) the speedrun timer.
        /// </summary>
        public void StartTimer()
        {
            _state.is_active = true;
            _state.is_paused = false;
            _state.real_time_elapsed = 0f;
            _state.in_game_days = 0;
            _state.splits.Clear();
            OnTimerStarted?.Invoke();
        }

        /// <summary>
        /// Called every second. Advances real time and in-game day counter.
        /// </summary>
        public void TickSecond(float realDelta, float inGameDayDelta)
        {
            if (!_state.is_active || _state.is_paused)
                return;

            _state.real_time_elapsed += realDelta;
            int prevDay = _state.in_game_days;
            _state.in_game_days += (int)inGameDayDelta;

            // Auto-record "Day 30 Flashpoint" split when day 30 is reached
            if (prevDay < 30 && _state.in_game_days >= 30)
            {
                RecordSplit(PreDefinedSplits[0], _state.in_game_days);
            }
        }

        /// <summary>
        /// Record a named split at the current timer position.
        /// </summary>
        public void RecordSplit(string name, int inGameDay)
        {
            var split = new SpeedrunSplit(name, _state.real_time_elapsed, inGameDay);
            _state.splits.Add(split);
            OnSplitRecorded?.Invoke(name, _state.real_time_elapsed, inGameDay);
        }

        /// <summary>
        /// Pause the timer.
        /// </summary>
        public void PauseTimer()
        {
            if (!_state.is_active || _state.is_paused)
                return;
            _state.is_paused = true;
            OnTimerPaused?.Invoke();
        }

        /// <summary>
        /// Resume the timer after a pause.
        /// </summary>
        public void ResumeTimer()
        {
            if (!_state.is_active || !_state.is_paused)
                return;
            _state.is_paused = false;
            OnTimerResumed?.Invoke();
        }

        /// <summary>
        /// Returns all recorded splits.
        /// </summary>
        public IReadOnlyList<SpeedrunSplit> GetSplits()
        {
            return _state.splits.AsReadOnly();
        }

        /// <summary>
        /// Returns real-world elapsed time in seconds.
        /// </summary>
        public float GetRealTime()
        {
            return _state.real_time_elapsed;
        }

        /// <summary>
        /// Returns current in-game day count.
        /// </summary>
        public int GetInGameDays()
        {
            return _state.in_game_days;
        }

        /// <summary>
        /// Signal that the run is complete. Fires OnRunCompleted with
        /// total time and split names for leaderboard export.
        /// </summary>
        public void CompleteRun()
        {
            if (!_state.is_active)
                return;

            string[] splitNames = new string[_state.splits.Count];
            for (int i = 0; i < _state.splits.Count; i++)
            {
                splitNames[i] = _state.splits[i].name;
            }

            _state.is_active = false;
            OnRunCompleted?.Invoke(_state.real_time_elapsed, splitNames);
        }

        /// <summary>
        /// Export splits as a formatted string suitable for clipboard / leaderboard paste.
        /// </summary>
        public string ExportSplitsToClipboard()
        {
            var sb = new System.Text.StringBuilder();
            sb.AppendLine($"Speedrun — {_state.real_time_elapsed:F1}s — {_state.in_game_days} days");
            for (int i = 0; i < _state.splits.Count; i++)
            {
                var s = _state.splits[i];
                sb.AppendLine($"  [{s.in_game_day}d] {s.name}: {s.real_time:F1}s");
            }
            return sb.ToString();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public SpeedrunTimerState CaptureState()
        {
            return _state;
        }

        public void RestoreState(SpeedrunTimerState state)
        {
            _state = state ?? new SpeedrunTimerState();
        }
    }
}
