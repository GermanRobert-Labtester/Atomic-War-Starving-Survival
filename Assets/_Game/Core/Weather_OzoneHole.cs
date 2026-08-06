using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class OzoneHoleState
    {
        public string weatherId = "weather_ozone_hole";
        public string displayName = "Ozone Hole";
        public bool isActive = false;
        public int durationDays = 5;
        public int daysRemaining = 0;
        public float radPerHourDaylight = 20f;
        public float burnDamagePerHour = 10f;
        public bool scavengingLockedDuringDay = true;
    }

    /// <summary>
    /// Prompt #651: Weather — Ozone Hole.
    /// Ozone layer stripped. Summer daylight delivers severe Radiation and SevereBurns.
    /// Scavenging is limited to the night cycle while the event is active.
    /// </summary>
    public class Weather_OzoneHole
    {
        private OzoneHoleState _state = new OzoneHoleState();

        // -- Events --
        public event Action<OzoneHoleState> OnOzoneHoleTriggered;
        public event Action<OzoneHoleState> OnOzoneHoleEnded;
        public event Action<OzoneHoleState, float, float> OnDaylightExposure;

        public OzoneHoleState State => _state;

        /// <summary>
        /// Triggers the ozone hole event. Only activates during summer (season == 2).
        /// Season mapping: 0=Spring, 1=Autumn, 2=Summer, 3=Winter (or caller-defined).
        /// </summary>
        public void Trigger(int season)
        {
            // Only activates during summer
            const int summerSeason = 2;
            if (season != summerSeason) return;

            _state.isActive = true;
            _state.daysRemaining = _state.durationDays;

            OnOzoneHoleTriggered?.Invoke(_state);
        }

        /// <summary>
        /// Per-hour tick. During daylight, applies radiation and burn damage.
        /// Returns (radDamage, burnDamage) applied this hour.
        /// </summary>
        public (float radDamage, float burnDamage) TickHour(bool isDaylight)
        {
            if (!_state.isActive) return (0f, 0f);

            if (!isDaylight) return (0f, 0f);

            float rad = _state.radPerHourDaylight;
            float burn = _state.burnDamagePerHour;

            OnDaylightExposure?.Invoke(_state, rad, burn);
            return (rad, burn);
        }

        /// <summary>
        /// Daily tick. Decrements remaining days and deactivates when expired.
        /// </summary>
        public void TickDay()
        {
            if (!_state.isActive) return;

            _state.daysRemaining = Math.Max(0, _state.daysRemaining - 1);
            if (_state.daysRemaining <= 0)
            {
                _state.isActive = false;
                OnOzoneHoleEnded?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns whether scavenging is allowed given the current time of day.
        /// During an active ozone hole, daytime scavenging is blocked.
        /// </summary>
        public bool IsScavengingAllowed(bool isDaylight)
        {
            if (!_state.isActive) return true;
            if (!_state.scavengingLockedDuringDay) return true;
            return !isDaylight;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public OzoneHoleState GetState() => _state;

        public void RestoreState(OzoneHoleState state)
        {
            _state = state ?? new OzoneHoleState();
        }
    }
}
