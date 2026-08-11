using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Encounters
{
    [Serializable]
    public class AcidGeyserState
    {
        public string hazard_id = "map_hazard_acid_geyser";
        public string node_id = "";
        public float hours_since_eruption = 0f;
        public float eruption_interval_hours = 3f;
        public bool is_erupting = false;
        public float eruption_duration_minutes = 10f;
        public float eruption_timer_minutes = 0f;
    }

    /// <summary>
    /// Prompt #850: Acid Geysers — Erupts every 3 in-game hours.
    /// Caught = HazmatSuit instantly melts to 0% + SevereChemicalBurns.
    /// 30-second warning before eruption.
    /// </summary>
    public sealed class MapHazard_AcidGeyser
    {
        private AcidGeyserState _state;

        private const float WarningLeadSeconds = 30f;
        private const float ChemicalBurnHealthLoss = 40f;

        public event Action<float> OnEruptionWarning;                  // minutes_until
        public event Action<string> OnEruptionStarted;                 // node_id
        public event Action<string> OnHazmatMelted;                    // survivor_id
        public event Action<string, float> OnChemicalBurnsApplied;     // survivor_id, severity
        public event Action<string> OnEruptionEnded;                   // node_id

        public string HazardId => _state.hazard_id;

        public MapHazard_AcidGeyser()
        {
            _state = new AcidGeyserState();
        }

        /// <summary>
        /// Advances the geyser clock by one in-game hour. Checks for eruption
        /// trigger and issues a 30-second warning when the next eruption is imminent.
        /// </summary>
        public void TickHour()
        {
            _state.hours_since_eruption += 1f;

            if (_state.is_erupting)
            {
                _state.eruption_timer_minutes -= 60f; // 1 game-hour elapsed
                if (_state.eruption_timer_minutes <= 0f)
                {
                    _state.is_erupting = false;
                    _state.eruption_timer_minutes = 0f;
                    _state.hours_since_eruption = 0f;
                    OnEruptionEnded?.Invoke(_state.node_id);
                    GameLog.Log($"[MapHazard_AcidGeyser] Eruption ended at node '{_state.node_id}'.");
                }
                return;
            }

            float hours_remaining = _state.eruption_interval_hours - _state.hours_since_eruption;
            float minutes_remaining = hours_remaining * 60f;

            // 30-second warning
            if (minutes_remaining > 0f && minutes_remaining <= (WarningLeadSeconds / 60f))
            {
                OnEruptionWarning?.Invoke(minutes_remaining);
                GameLog.Log($"[MapHazard_AcidGeyser] Warning — eruption in {minutes_remaining:F1} minutes.");
            }

            if (_state.hours_since_eruption >= _state.eruption_interval_hours)
            {
                CheckEruption();
            }
        }

        /// <summary>
        /// Triggers an eruption if the interval has elapsed and the geyser
        /// is not already erupting.
        /// </summary>
        public void CheckEruption()
        {
            if (_state.is_erupting)
                return;

            _state.is_erupting = true;
            _state.eruption_timer_minutes = _state.eruption_duration_minutes;

            OnEruptionStarted?.Invoke(_state.node_id);
            GameLog.Log($"[MapHazard_AcidGeyser] Eruption started at node '{_state.node_id}' " +
                      $"for {_state.eruption_duration_minutes:F0} minutes.");
        }

        /// <summary>
        /// Resolves a survivor caught in the eruption. HazmatSuit melts to 0%.
        /// Survivor takes SevereChemicalBurns (health -40).
        /// </summary>
        public void OnSurvivorCaught(string survivor_id, bool has_hazmat)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[MapHazard_AcidGeyser] survivor_id is null or empty.");
                return;
            }

            if (has_hazmat)
            {
                OnHazmatMelted?.Invoke(survivor_id);
                GameLog.Log($"[MapHazard_AcidGeyser] Survivor '{survivor_id}' hazmat suit melted to 0%.");
            }

            OnChemicalBurnsApplied?.Invoke(survivor_id, ChemicalBurnHealthLoss);
            GameLog.Log($"[MapHazard_AcidGeyser] Survivor '{survivor_id}' suffered severe chemical burns " +
                      $"(-{ChemicalBurnHealthLoss:F0} health).");
        }

        /// <summary>
        /// Returns minutes until the next eruption. 0 if currently erupting.
        /// </summary>
        public float GetTimeToNextEruption()
        {
            if (_state.is_erupting)
                return 0f;

            float hours_remaining = _state.eruption_interval_hours - _state.hours_since_eruption;
            return Mathf.Max(0f, hours_remaining * 60f);
        }

        /// <summary>
        /// Returns whether the geyser is currently erupting.
        /// </summary>
        public bool IsErupting() => _state.is_erupting;

        public AcidGeyserState CaptureState()
        {
            return new AcidGeyserState
            {
                hazard_id = _state.hazard_id,
                node_id = _state.node_id,
                hours_since_eruption = _state.hours_since_eruption,
                eruption_interval_hours = _state.eruption_interval_hours,
                is_erupting = _state.is_erupting,
                eruption_duration_minutes = _state.eruption_duration_minutes,
                eruption_timer_minutes = _state.eruption_timer_minutes
            };
        }

        public void RestoreState(AcidGeyserState saved)
        {
            _state = saved ?? new AcidGeyserState();
        }
    }
}
