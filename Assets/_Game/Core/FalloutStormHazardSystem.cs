using System;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Shelter;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Event data raised when a fallout storm breaches bunker environmental seals.
    /// </summary>
    public struct FalloutStormSealBreachedEvent
    {
        public WeatherKind Weather;
        public float StormIntensity;
        public float RadiationFloodRadsPerHour;
        public string EntryRoomId;
    }

    /// <summary>
    /// System linking fallout storms, bunker air filtration wear, entry room radiation surge,
    /// and emergency siren soundscapes (Prompt #40).
    /// </summary>
    
    [Serializable]
    public class FalloutStormHazardSystemSave
    {
        public string systemId = "fallout_storm_hazard_system";
    }
    public class FalloutStormHazardSystem
    {
        public const float RadiationFloodPerBreachRadsPerHour = 50f;

        private readonly WeatherSystem _weather;
        private AudioEventBus _audioBus;

        public event Action<FalloutStormSealBreachedEvent> OnSealBreached;

        public FalloutStormHazardSystem(WeatherSystem weather, AudioEventBus audioBus = null)
        {
            _weather = weather ?? throw new ArgumentNullException(nameof(weather));
            _audioBus = audioBus;
        }

        /// <summary>
        /// Late-bind a process-wide AudioEventBus after construction
        /// (used by GameBootstrap when the bus is created after this system).
        /// </summary>
        public void SetAudioBus(AudioEventBus audioBus)
        {
            if (audioBus != null && _audioBus == null)
                _audioBus = audioBus;
        }

        /// <summary>
        /// Calculate active air filter wear rate based on weather fallout storm intensity.
        /// When StormIntensity >= 0.7 during a FalloutStorm, degradation rate doubles (2.0x).
        /// </summary>
        public float CalculateFilterWearRate(float baseWearRatePerGameHour)
        {
            float multiplier = _weather != null ? _weather.AirFilterDegradationMultiplier : 1.0f;
            return baseWearRatePerGameHour * multiplier;
        }

        /// <summary>
        /// Process external hatch breach during weather state.
        /// If a breach occurs during an active FalloutStorm, floods entry room with +50 rads/hr
        /// and triggers emergency siren audio on AudioEventBus.
        /// </summary>
        public bool ProcessBreachedHatch(ShelterRoom entryRoom)
        {
            // FalloutStorm and BlackRain both flood the entry on hatch breach.
            if (_weather == null || !WeatherSystem.IsHyperHazardWeather(_weather.Current))
                return false;

            if (entryRoom != null)
            {
                entryRoom.AmbientRadiation += RadiationFloodPerBreachRadsPerHour;
            }

            if (_audioBus != null)
            {
                _audioBus.TriggerEmergencySiren(true);
            }

            var evt = new FalloutStormSealBreachedEvent
            {
                Weather = _weather.Current,
                StormIntensity = _weather.StormIntensity,
                RadiationFloodRadsPerHour = RadiationFloodPerBreachRadsPerHour,
                EntryRoomId = entryRoom?.RoomId ?? "entry"
            };

            OnSealBreached?.Invoke(evt);
            EventBus.Raise(evt);

            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public FalloutStormHazardSystemSave CaptureState() => new FalloutStormHazardSystemSave();

        public void RestoreState(FalloutStormHazardSystemSave saved) { _ = saved; }

}
}
