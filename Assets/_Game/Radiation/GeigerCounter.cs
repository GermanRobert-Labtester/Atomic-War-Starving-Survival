using System;
using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Pure-C# Geiger counter logic: converts a zone dose-rate into a click rate
    /// and schedules clicks over time. The actual audio playback is a hook — the
    /// audio layer subscribes to OnClick and plays a tick sample (a thin MonoBehaviour
    /// bridges OnClick to an AudioSource). Keeps all timing logic EditMode-testable.
    /// </summary>
    public class GeigerCounter
    {
        /// <summary>Clicks per second contributed per unit of dose-rate.</summary>
        public const float TicksPerSecondPerRad = 0.05f;

        /// <summary>Upper bound on click rate so a hot zone doesn't become a solid tone.</summary>
        public const float MaxTicksPerSecond = 30f;

        /// <summary>Audio hook: raised once per scheduled click.</summary>
        public event Action OnClick;

        public float CurrentRadLevel { get; private set; }

        private float _accumulator;

        /// <summary>Click rate for a dose-rate: proportional, clamped to [0, MaxTicksPerSecond].</summary>
        public static float ComputeTicksPerSecond(float zoneRadLevel)
        {
            return Mathf.Clamp(Mathf.Max(0f, zoneRadLevel) * TicksPerSecondPerRad, 0f, MaxTicksPerSecond);
        }

        /// <summary>Point the counter at a new zone dose-rate.</summary>
        public void SetRadLevel(float zoneRadLevel)
        {
            CurrentRadLevel = Mathf.Max(0f, zoneRadLevel);
        }

        /// <summary>Advance the click scheduler by a real-time delta, firing OnClick per due click.</summary>
        public void Tick(float deltaTimeSeconds)
        {
            float rate = ComputeTicksPerSecond(CurrentRadLevel);
            if (rate <= 0f || deltaTimeSeconds <= 0f)
            {
                _accumulator = 0f;
                return;
            }

            _accumulator += deltaTimeSeconds * rate;
            while (_accumulator >= 1f)
            {
                _accumulator -= 1f;
                OnClick?.Invoke();
            }
        }
    }
}
