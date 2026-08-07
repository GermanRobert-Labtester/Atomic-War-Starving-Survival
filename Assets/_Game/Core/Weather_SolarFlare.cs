using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SolarFlareState
    {
        public string weatherId = "weather_solar_flare";
        public string displayName = "Solar Flare (CME)";
        public float durationHours = 72f;
        public float hoursRemaining = 0f;
        public float moraleBoost = 30f;
        public bool electronicsDisabled = false;
    }

    /// <summary>
    /// Prompt #635: Weather: Solar Flare (Coronal Mass Ejection).
    /// A CME paints the sky with beautiful auroras (massive Morale boost) but
    /// hard-disables ALL electronics, Radios, and Walkie-Talkies for 72 hours.
    /// The Expedition Map goes dark.
    /// </summary>
    public class Weather_SolarFlare
    {
        private SolarFlareState _state = new SolarFlareState();

        public event Action<SolarFlareState> OnSolarFlareTriggered;
        public event Action<SolarFlareState> OnSolarFlareEnded;
        public event Action<SolarFlareState, float> OnTick;

        public SolarFlareState State => _state;

        public void Trigger()
        {
            _state.hoursRemaining = _state.durationHours;
            _state.electronicsDisabled = true;
            OnSolarFlareTriggered?.Invoke(_state);
        }

        public void TickHour()
        {
            if (_state.hoursRemaining <= 0f) return;

            _state.hoursRemaining -= 1f;
            OnTick?.Invoke(_state, _state.hoursRemaining);

            if (_state.hoursRemaining <= 0f)
            {
                _state.hoursRemaining = 0f;
                _state.electronicsDisabled = false;
                OnSolarFlareEnded?.Invoke(_state);
            }
        }

        public bool AreElectronicsDisabled()
        {
            return _state.electronicsDisabled;
        }

        public float GetMoraleBonus()
        {
            return _state.hoursRemaining > 0f ? _state.moraleBoost : 0f;
        }

        public SolarFlareState CaptureState() => _state;

        public void RestoreState(SolarFlareState saved)
        {
            _state = saved ?? new SolarFlareState();
        }
    }
}
