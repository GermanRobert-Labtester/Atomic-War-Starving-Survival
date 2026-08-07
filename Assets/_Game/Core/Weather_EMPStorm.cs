using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EMPStormState
    {
        public string weatherId = "weather_emp_storm";
        public string displayName = "EMP Aftershocks";
        public bool isPowerGridTripped = false;
        public int blackoutHoursRemaining = 0;
        public bool isRadioStaticOnly = false;
    }

    /// <summary>
    /// Prompt #370: System: EMP Aftershocks.
    /// Rolling EMP bursts caused by ash cloud friction. Trips PowerNetwork offline for 1-4 hours at a time
    /// and renders RadioSystem outputs as static.
    /// </summary>
    public class Weather_EMPStorm
    {
        private EMPStormState _state = new EMPStormState();

        public event Action<EMPStormState, int> OnEMPBurstTrippedPower;
        public event Action<EMPStormState> OnPowerRestored;

        public EMPStormState State => _state;

        public void TriggerEMPBurst(System.Random rng)
        {
            _state.isPowerGridTripped = true;
            _state.blackoutHoursRemaining = rng.Next(1, 5); // 1-4 hours
            _state.isRadioStaticOnly = true;

            OnEMPBurstTrippedPower?.Invoke(_state, _state.blackoutHoursRemaining);
        }

        public void TickHourly(int hoursPassed = 1)
        {
            if (_state.isPowerGridTripped)
            {
                _state.blackoutHoursRemaining -= hoursPassed;
                if (_state.blackoutHoursRemaining <= 0)
                {
                    _state.blackoutHoursRemaining = 0;
                    _state.isPowerGridTripped = false;
                    _state.isRadioStaticOnly = false;

                    OnPowerRestored?.Invoke(_state);
                }
            }
        }

        public EMPStormState CaptureState() => _state;

        public void RestoreState(EMPStormState saved)
        {
            _state = saved ?? new EMPStormState();
        }
    }
}
