using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class FalseSpringState
    {
        public string weatherId = "weather_false_spring";
        public string displayName = "The False Spring";
        public bool isActive = false;
        public float durationHoursRemaining = 0f;
        public float ambientTemperature = 15f;
        public bool isCatchmentOverflowing = false;
        public bool isLowestRoomFlooded = false;
    }

    /// <summary>
    /// Prompt #373: System: The False Spring (Flash Floods).
    /// A sudden 48-hour temperature spike to 15°C. Rapid snow/ash melt causes CatchmentSurfaces
    /// to overflow (wasting water) and instantly floods the lowest room in the shelter.
    /// </summary>
    public class Weather_FalseSpring
    {
        private FalseSpringState _state = new FalseSpringState();

        public event Action<FalseSpringState> OnFalseSpringStarted;
        public event Action<FalseSpringState> OnFalseSpringEnded;

        public FalseSpringState State => _state;

        public void TriggerFalseSpring()
        {
            _state.isActive = true;
            _state.durationHoursRemaining = 48f;
            _state.isCatchmentOverflowing = true;
            _state.isLowestRoomFlooded = true;

            OnFalseSpringStarted?.Invoke(_state);
        }

        public void TickHourly(float hoursElapsed)
        {
            if (_state.isActive)
            {
                _state.durationHoursRemaining -= hoursElapsed;
                if (_state.durationHoursRemaining <= 0f)
                {
                    _state.durationHoursRemaining = 0f;
                    _state.isActive = false;
                    _state.isCatchmentOverflowing = false;

                    OnFalseSpringEnded?.Invoke(_state);
                }
            }
        }

        public FalseSpringState CaptureState() => _state;

        public void RestoreState(FalseSpringState saved)
        {
            _state = saved ?? new FalseSpringState();
        }
    }
}
