using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DeadWindState
    {
        public string weatherId = "weather_dead_wind";
        public string displayName = "Dead Wind (Stagnation)";
        public bool isActive = false;
        public float windSpeed = 0f;
        public float airFilterEfficiencyMultiplier = 0.50f; // 50% drop
        public bool areWindTurbinesStopped = true;
    }

    /// <summary>
    /// Prompt #376: System: Dead Wind (Stagnation).
    /// Wind speed drops to absolute zero. Stops WindTurbines and cuts AirFilter efficiency by 50%
    /// due to lack of draft pressure, leaving the shelter stuffy and sweltering.
    /// </summary>
    /// <summary>DEMOTE-Weather-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Weather_DeadWind
    {
        private DeadWindState _state = new DeadWindState();

        public event Action<DeadWindState> OnDeadWindStarted;
        public event Action<DeadWindState> OnDeadWindEnded;

        public DeadWindState State => _state;

        public void ActivateDeadWind()
        {
            _state.isActive = true;
            _state.windSpeed = 0f;
            _state.areWindTurbinesStopped = true;
            _state.airFilterEfficiencyMultiplier = 0.50f;

            OnDeadWindStarted?.Invoke(_state);
        }

        public void DeactivateDeadWind()
        {
            _state.isActive = false;
            _state.areWindTurbinesStopped = false;
            _state.airFilterEfficiencyMultiplier = 1.0f;

            OnDeadWindEnded?.Invoke(_state);
        }

        public DeadWindState CaptureState() => _state;

        public void RestoreState(DeadWindState saved)
        {
            _state = saved ?? new DeadWindState();
        }
    }
}
