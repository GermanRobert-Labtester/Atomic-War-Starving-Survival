// UI_ScenarioGen.cs — Custom Scenario Generator (Prompt #867)
// Menu before starting. Toggle rules: "Start Day 100", "Zero Radiation",
// "Oops All Mutants", "Constant Winter." Infinite replayability.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Serializable state for the Scenario Generator UI (Prompt #867).
    /// Stores all selected modifiers and their values.
    /// </summary>
    [Serializable]
    public class ScenarioGenState
    {
        public string ui_id = "ui_scenario_gen";
        public List<string> selected_modifiers = new List<string>();
        public int start_day;
        public float radiation_multiplier = 1f;
        public float mutant_chance;
        public string weather_override = "normal";
        public float difficulty_multiplier = 1f;
    }

    /// <summary>
    /// Custom Scenario Generator (Prompt #867).
    /// Modifiers: start_day (1–200), radiation (0–3x), mutant_chance (0–1),
    /// weather (constant_winter / summer / normal), starting_survivors (1–8),
    /// difficulty (0.5–3x). Invalid combos flagged.
    /// </summary>
    public class UI_ScenarioGen
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, object> OnModifierChanged;
        public event Action<bool> OnScenarioValidated;
        public event Action<string> OnScenarioApplied;
        public event Action OnScenarioReset;

        // ── State ──────────────────────────────────────────────────────
        private ScenarioGenState _state = new ScenarioGenState();

        // Additional modifier storage for non-state fields
        private int _startingSurvivors = 4;

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Set a modifier value. Valid modifier ids:
        /// "start_day", "radiation", "mutant_chance", "weather",
        /// "starting_survivors", "difficulty".
        /// </summary>
        public void SetModifier(string modifierId, object value)
        {
            switch (modifierId)
            {
                case "start_day":
                    _state.start_day = Clamp(Convert.ToInt32(value), 1, 200);
                    break;
                case "radiation":
                    _state.radiation_multiplier = Clamp(Convert.ToSingle(value), 0f, 3f);
                    break;
                case "mutant_chance":
                    _state.mutant_chance = Clamp(Convert.ToSingle(value), 0f, 1f);
                    break;
                case "weather":
                    string w = value.ToString();
                    if (w == "constant_winter" || w == "summer" || w == "normal")
                        _state.weather_override = w;
                    break;
                case "starting_survivors":
                    _startingSurvivors = Clamp(Convert.ToInt32(value), 1, 8);
                    break;
                case "difficulty":
                    _state.difficulty_multiplier = Clamp(Convert.ToSingle(value), 0.5f, 3f);
                    break;
            }

            if (!_state.selected_modifiers.Contains(modifierId))
                _state.selected_modifiers.Add(modifierId);

            OnModifierChanged?.Invoke(modifierId, value);
        }

        /// <summary>
        /// Returns the list of active modifier ids.
        /// </summary>
        public IReadOnlyList<string> GetActiveModifiers()
        {
            return _state.selected_modifiers.AsReadOnly();
        }

        /// <summary>
        /// Apply current modifiers to the game state.
        /// Fires OnScenarioApplied with the generated scenario name.
        /// </summary>
        public void ApplyToGameState()
        {
            bool valid = ValidateCombos();
            if (!valid)
                return;

            string name = GetScenarioName();
            OnScenarioApplied?.Invoke(name);
        }

        /// <summary>
        /// Reset all modifiers to their default values.
        /// </summary>
        public void ResetToDefaults()
        {
            _state.selected_modifiers.Clear();
            _state.start_day = 0;
            _state.radiation_multiplier = 1f;
            _state.mutant_chance = 0f;
            _state.weather_override = "normal";
            _state.difficulty_multiplier = 1f;
            _startingSurvivors = 4;
            OnScenarioReset?.Invoke();
        }

        /// <summary>
        /// Validate modifier combinations. Returns true if valid.
        /// Flags invalid combos (e.g., zero radiation + constant winter).
        /// </summary>
        public bool ValidateCombos()
        {
            bool isValid = true;

            // Invalid: zero radiation with constant winter makes no thematic sense
            if (_state.radiation_multiplier <= 0f && _state.weather_override == "constant_winter")
                isValid = false;

            // Invalid: max mutants + difficulty below 1 is contradictory
            if (_state.mutant_chance >= 1f && _state.difficulty_multiplier < 1f)
                isValid = false;

            // Invalid: starting on day 200 with only 1 survivor at 0.5x difficulty
            if (_state.start_day >= 200 && _startingSurvivors <= 1 &&
                _state.difficulty_multiplier <= 0.5f)
                isValid = false;

            OnScenarioValidated?.Invoke(isValid);
            return isValid;
        }

        /// <summary>
        /// Generate a descriptive scenario name from active modifiers.
        /// </summary>
        public string GetScenarioName()
        {
            var parts = new List<string>();

            if (_state.start_day > 1)
                parts.Add($"Day {_state.start_day}");
            if (_state.radiation_multiplier <= 0f)
                parts.Add("Zero Rad");
            else if (_state.radiation_multiplier > 1f)
                parts.Add($"Rad x{_state.radiation_multiplier:F1}");
            if (_state.mutant_chance >= 1f)
                parts.Add("All Mutants");
            else if (_state.mutant_chance > 0f)
                parts.Add($"Mutants {_state.mutant_chance:P0}");
            if (_state.weather_override != "normal")
                parts.Add(_state.weather_override == "constant_winter"
                    ? "Constant Winter"
                    : "Eternal Summer");
            if (_state.difficulty_multiplier > 1f)
                parts.Add($"Hard x{_state.difficulty_multiplier:F1}");
            else if (_state.difficulty_multiplier < 1f)
                parts.Add($"Easy x{_state.difficulty_multiplier:F1}");

            return parts.Count > 0
                ? string.Join(" / ", parts)
                : "Default Scenario";
        }

        /// <summary>
        /// Returns the current starting survivors count.
        /// </summary>
        public int GetStartingSurvivors()
        {
            return _startingSurvivors;
        }

        // ── Internals ──────────────────────────────────────────────────

        private static int Clamp(int val, int min, int max)
        {
            return val < min ? min : (val > max ? max : val);
        }

        private static float Clamp(float val, float min, float max)
        {
            return val < min ? min : (val > max ? max : val);
        }

        // ── Save / Load ────────────────────────────────────────────────

        public ScenarioGenState CaptureState()
        {
            var cap = new ScenarioGenState
            {
                ui_id = string.IsNullOrEmpty(_state.ui_id) ? "ui_scenario_gen" : _state.ui_id,
                selected_modifiers = new List<string>(),
                start_day = _state.start_day,
                radiation_multiplier = _state.radiation_multiplier,
                mutant_chance = _state.mutant_chance,
                weather_override = _state.weather_override ?? "normal",
                difficulty_multiplier = _state.difficulty_multiplier
            };
            if (_state.selected_modifiers != null)
            {
                for (int i = 0; i < _state.selected_modifiers.Count; i++)
                    cap.selected_modifiers.Add(_state.selected_modifiers[i]);
            }
            return cap;
        }

        public void RestoreState(ScenarioGenState state)
        {
            if (state == null)
            {
                _state = new ScenarioGenState();
                return;
            }
            _state = new ScenarioGenState
            {
                ui_id = string.IsNullOrEmpty(state.ui_id) ? "ui_scenario_gen" : state.ui_id,
                selected_modifiers = new List<string>(),
                start_day = state.start_day,
                radiation_multiplier = state.radiation_multiplier,
                mutant_chance = state.mutant_chance,
                weather_override = state.weather_override ?? "normal",
                difficulty_multiplier = state.difficulty_multiplier
            };
            if (state.selected_modifiers != null)
            {
                for (int i = 0; i < state.selected_modifiers.Count; i++)
                    _state.selected_modifiers.Add(state.selected_modifiers[i]);
            }
        }
    }
}
