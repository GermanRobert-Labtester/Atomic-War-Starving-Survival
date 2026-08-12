using System;
using UnityEngine;
using AtomicWar._Game.Medical;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Expansion IV — Chapter 45 UI Additions.
    /// LetheDripGauge: Physical glass sight-gauge on WaterPurifier UI showing amber level of the amnestic reservoir.
    /// When it hits the red line (< 20%), water droplets in UI visually slow down and ambient audio shifts to ragged breathing.
    /// </summary>
    public class LetheDripGauge : MonoBehaviour
    {
        private LetheProtocolSystem _letheSystem;
        private bool _isRedLineWarning;

        public bool IsRedLineWarning => _isRedLineWarning;

        public event Action OnPanickedBreathingAudioTriggered;

        public void BindLetheSystem(LetheProtocolSystem letheSystem)
        {
            _letheSystem = letheSystem;
            if (_letheSystem != null)
            {
                _letheSystem.OnReservoirLevelChanged += OnReservoirLevelChanged;
                OnReservoirLevelChanged(_letheSystem.ReservoirLevel);
            }
        }

        private void OnDestroy()
        {
            if (_letheSystem != null)
            {
                _letheSystem.OnReservoirLevelChanged -= OnReservoirLevelChanged;
            }
        }

        private void OnReservoirLevelChanged(float level)
        {
            bool wasRedLine = _isRedLineWarning;
            _isRedLineWarning = level <= LetheProtocolSystem.CriticalRedLineLevel;

            if (!wasRedLine && _isRedLineWarning)
            {
                OnPanickedBreathingAudioTriggered?.Invoke();
            }
        }

        public float GetDropletSpeedMultiplier()
        {
            if (_letheSystem == null) return 1.0f;
            if (_isRedLineWarning) return 0.25f; // Slow down water droplets
            return 1.0f;
        }
    }
}
