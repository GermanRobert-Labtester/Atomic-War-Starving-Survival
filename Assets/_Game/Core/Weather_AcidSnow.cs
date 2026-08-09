using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AcidSnowState
    {
        public string weatherId = "weather_acid_snow";
        public string displayName = "Acid Snow";
        public bool isActive = false;
        public float airFilterDegradationMultiplier = 10.0f; // 10x speed
        public float hazmatSuitDegradationMultiplier = 5.0f;
    }

    /// <summary>
    /// Prompt #369: System: Acid Snow (Filter Killer).
    /// Rare Phase 3 weather event. Low radiation, highly corrosive. Drains AirFilter durability at 10x speed
    /// and damages HazmatSuits outside, forcing player to shut off ventilation and endure CO2 buildup.
    /// </summary>
    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_AcidSnow
    {
        private AcidSnowState _state = new AcidSnowState();

        public event Action<AcidSnowState, float> OnAcidCorrosionApplied;

        public AcidSnowState State => _state;

        public float TickCorrosion(float deltaHours, bool isVentilationActive, ref float airFilterDurability)
        {
            if (!_state.isActive) return 0f;

            float drain = 0f;
            if (isVentilationActive)
            {
                drain = deltaHours * _state.airFilterDegradationMultiplier;
                airFilterDurability = Mathf.Max(0f, airFilterDurability - drain);
                OnAcidCorrosionApplied?.Invoke(_state, drain);
            }

            return drain;
        }

        public void SetActive(bool active)
        {
            _state.isActive = active;
        }

        public AcidSnowState CaptureState() => _state;

        public void RestoreState(AcidSnowState saved)
        {
            _state = saved ?? new AcidSnowState();
        }
    }
}
