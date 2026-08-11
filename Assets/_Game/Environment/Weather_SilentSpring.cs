using System;
using UnityEngine;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class SilentSpringState
    {
        public string weatherId = "weather_silent_spring";
        public string displayName = "Silent Spring";
        public float durationHours = 6f;
        public float hoursRemaining = 0f;
        public float paranoiaDebuff = -30f;
        public bool hordeGuaranteed = true;
        public bool audioSilenced = false;
    }

    /// <summary>
    /// Prompt #656: Weather — Silent Spring.
    /// All animal sounds stop. The AudioMixer drops to dead quiet. Applies a massive
    /// Paranoia debuff. A guaranteed Level 5 Mutant Horde follows when the silence ends.
    /// </summary>
    public class Weather_SilentSpring
    {
        private SilentSpringState _state = new SilentSpringState();

        // -- Events --
        public event Action<SilentSpringState> OnSilentSpringTriggered;
        public event Action<SilentSpringState> OnSilentSpringEnded;
        public event Action<SilentSpringState> OnHordeImminent;

        public SilentSpringState State => _state;

        public bool IsActive => _state.hoursRemaining > 0f;

        /// <summary>
        /// Triggers the Silent Spring event. Silences all audio and begins
        /// the paranoia countdown.
        /// </summary>
        public void Trigger()
        {
            _state.hoursRemaining = _state.durationHours;
            _state.audioSilenced = true;

            OnSilentSpringTriggered?.Invoke(_state);
        }

        /// <summary>
        /// Per-hour tick. Decrements remaining time. When the silence ends,
        /// audio is restored and the horde signal fires.
        /// </summary>
        public void TickHour()
        {
            if (!IsActive) return;

            _state.hoursRemaining = Mathf.Max(0f, _state.hoursRemaining - 1f);

            if (!IsActive)
            {
                _state.audioSilenced = false;
                OnSilentSpringEnded?.Invoke(_state);

                if (_state.hordeGuaranteed)
                {
                    OnHordeImminent?.Invoke(_state);
                }
            }
        }

        /// <summary>
        /// Returns the paranoia debuff value while active. Returns 0 when inactive.
        /// </summary>
        public float GetParanoiaDebuff()
        {
            if (!IsActive) return 0f;
            return _state.paranoiaDebuff;
        }

        /// <summary>
        /// Returns true if the horde is guaranteed and the silence has ended
        /// (i.e., the horde is imminent or arriving).
        /// </summary>
        public bool IsHordeImminent()
        {
            return _state.hordeGuaranteed && !IsActive && _state.hoursRemaining <= 0f;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public SilentSpringState GetState() => _state;

        public SilentSpringState CaptureState() => GetState();

        public void RestoreState(SilentSpringState state)
        {
            _state = state ?? new SilentSpringState();
        }
    }
}
