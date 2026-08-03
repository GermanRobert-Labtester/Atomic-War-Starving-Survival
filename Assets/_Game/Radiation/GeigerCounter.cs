using System;
using UnityEngine;

namespace AtomicWar._Game.Radiation
{
    /// <summary>
    /// Pure-C# Geiger counter logic: converts a zone dose-rate into a click rate
    /// and schedules clicks over time. Optionally gated by a DeviceState — a broken
    /// or dead instrument is silent. The audio layer subscribes to OnClick.
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

        /// <summary>Optional reliability gate. Null = always working (legacy/tests).</summary>
        public DeviceState Device { get; set; }

        private float _accumulator;

        /// <summary>Click rate for a dose-rate: proportional, clamped to [0, MaxTicksPerSecond].</summary>
        public static float ComputeTicksPerSecond(float zoneRadLevel)
        {
            return Mathf.Clamp(Mathf.Max(0f, zoneRadLevel) * TicksPerSecondPerRad, 0f, MaxTicksPerSecond);
        }

        /// <summary>Point the counter at a new zone dose-rate (raw; bias applied by caller if needed).</summary>
        public void SetRadLevel(float zoneRadLevel)
        {
            CurrentRadLevel = Mathf.Max(0f, zoneRadLevel);
        }

        /// <summary>
        /// Point the counter at true ambient rad, applying device bias when Device is set.
        /// Broken / dead battery → silent (rad level 0).
        /// </summary>
        public void SetTrueRadLevel(float trueRadLevel)
        {
            if (Device != null && !InstrumentDevice.CanMeasure(Device))
            {
                CurrentRadLevel = 0f;
                return;
            }

            if (Device != null && InstrumentDevice.TryRead(Device, trueRadLevel, out float biased))
            {
                CurrentRadLevel = biased;
            }
            else
            {
                CurrentRadLevel = Mathf.Max(0f, trueRadLevel);
            }
        }

        /// <summary>Advance the click scheduler by a real-time delta, firing OnClick per due click.</summary>
        public void Tick(float deltaTimeSeconds)
        {
            if (Device != null && !InstrumentDevice.CanMeasure(Device))
            {
                _accumulator = 0f;
                return;
            }

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
