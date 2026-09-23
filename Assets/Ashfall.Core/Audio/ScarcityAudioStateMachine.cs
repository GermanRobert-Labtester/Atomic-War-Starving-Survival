// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.Audio
{
    /// <summary>
    /// Discrete ambience soundscape beds representing distinct environmental states.
    /// </summary>
    public enum ScarcityAmbienceBed
    {
        /// <summary>Baseline bunker hum, generator rumble, and low air circulation.</summary>
        BunkerAmbience = 0,

        /// <summary>Wasteland surface wind, dust whistling, distant barren plains.</summary>
        SurfaceAmbience = 1,

        /// <summary>Surface blizzard, ash squall, or violent atmospheric storm.</summary>
        SurfaceStormAmbience = 2,

        /// <summary>Subterranean transit, train tunnels, or narrow sewer passages.</summary>
        TransitAmbience = 3,

        /// <summary>Crowded shelter bunks with murmurs, creaking floorboards, and muffled breath.</summary>
        CrowdedShelterAmbience = 4,

        /// <summary>Dead silence: total lack of ambient beds (Silence, SilentSpring, FalseSpring).</summary>
        AbsoluteSilence = 5
    }

    /// <summary>
    /// Geiger click rate bands mapped to current radiation dose rate.
    /// </summary>
    public enum GeigerRateBand
    {
        Off = 0,
        Low = 1,       // 0.1 - 0.5 rads/h
        Medium = 2,    // 0.5 - 2.0 rads/h
        High = 3,      // 2.0 - 10.0 rads/h
        Lethal = 4     // > 10.0 rads/h
    }

    /// <summary>
    /// Audio profile mapping a specific weather kind to its ambient bed, cues, and pitch.
    /// </summary>
    public sealed class ScarcityWeatherAudioProfile
    {
        public string WeatherKind { get; set; } = string.Empty;
        public ScarcityAmbienceBed Bed { get; set; } = ScarcityAmbienceBed.SurfaceAmbience;
        public string AudioCueName { get; set; } = "amb_surface";
        public float PitchScale { get; set; } = 1.0f;
        public float VolumeOffsetDb { get; set; } = 0.0f;
        public bool IsAbsoluteSilence { get; set; }

        public ScarcityWeatherAudioProfile() { }

        public ScarcityWeatherAudioProfile(
            string weatherKind,
            ScarcityAmbienceBed bed,
            string audioCueName,
            float pitchScale = 1.0f,
            float volumeOffsetDb = 0.0f,
            bool isAbsoluteSilence = false)
        {
            WeatherKind = weatherKind ?? string.Empty;
            Bed = bed;
            AudioCueName = audioCueName ?? string.Empty;
            PitchScale = pitchScale;
            VolumeOffsetDb = volumeOffsetDb;
            IsAbsoluteSilence = isAbsoluteSilence;
        }
    }

    /// <summary>
    /// Mix ducking policy to prevent alert pile-up and preserve intelligibility.
    /// </summary>
    public sealed class ScarcityDuckingPolicy
    {
        public float AlertDuckingAttenuationDb { get; set; } = -7.0f;
        public float DuckAttackSeconds { get; set; } = 0.05f;
        public float DuckReleaseSeconds { get; set; } = 0.80f;
        public int MaxConcurrentAlerts { get; set; } = 2;
    }

    /// <summary>
    /// Radiation audio exposure state ensuring clean exposure-end termination.
    /// </summary>
    public sealed class ScarcityRadiationAudioState
    {
        public bool IsExposed { get; private set; }
        public float DoseRateRadsPerHour { get; private set; }
        public bool GeigerLoopActive { get; private set; }
        public GeigerRateBand RateBand { get; private set; } = GeigerRateBand.Off;

        public void SetExposure(float doseRate)
        {
            DoseRateRadsPerHour = Math.Max(0.0f, doseRate);
            IsExposed = DoseRateRadsPerHour > 0.05f;
            GeigerLoopActive = IsExposed;

            if (DoseRateRadsPerHour <= 0.05f)
            {
                RateBand = GeigerRateBand.Off;
                GeigerLoopActive = false;
            }
            else if (DoseRateRadsPerHour < 0.5f)
            {
                RateBand = GeigerRateBand.Low;
            }
            else if (DoseRateRadsPerHour < 2.0f)
            {
                RateBand = GeigerRateBand.Medium;
            }
            else if (DoseRateRadsPerHour < 10.0f)
            {
                RateBand = GeigerRateBand.High;
            }
            else
            {
                RateBand = GeigerRateBand.Lethal;
            }
        }

        /// <summary>
        /// Explicitly ends radiation exposure and halts the geiger audio loop.
        /// Resolves the orphan loop defect.
        /// </summary>
        public void EndExposure()
        {
            IsExposed = false;
            DoseRateRadsPerHour = 0.0f;
            GeigerLoopActive = false;
            RateBand = GeigerRateBand.Off;
        }
    }

    /// <summary>
    /// State machine controlling game ambience, mix ducking, and silence state transitions.
    /// Pure domain authority without engine dependencies.
    /// </summary>
    public sealed class ScarcityAudioStateMachine
    {
        private static readonly Dictionary<string, ScarcityWeatherAudioProfile> WeatherProfiles =
            new Dictionary<string, ScarcityWeatherAudioProfile>(StringComparer.OrdinalIgnoreCase)
            {
                // All 22 canonical weather kinds:
                { "clear", new ScarcityWeatherAudioProfile("clear", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 1.0f, 0.0f) },
                { "overcast", new ScarcityWeatherAudioProfile("overcast", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 0.95f, -1.0f) },
                { "fog", new ScarcityWeatherAudioProfile("fog", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 0.9f, -2.0f) },
                { "ash_fall", new ScarcityWeatherAudioProfile("ash_fall", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_ash", 0.85f, 1.0f) },
                { "ash_storm", new ScarcityWeatherAudioProfile("ash_storm", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.05f, 3.0f) },
                { "acid_rain", new ScarcityWeatherAudioProfile("acid_rain", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_rain", 1.0f, 2.0f) },
                { "acid_snow", new ScarcityWeatherAudioProfile("acid_snow", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_snow", 0.88f, 0.0f) },
                { "bio_fog", new ScarcityWeatherAudioProfile("bio_fog", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_bio", 0.80f, -1.0f) },
                { "black_snow", new ScarcityWeatherAudioProfile("black_snow", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_snow", 0.85f, 0.0f) },
                { "blood_rain", new ScarcityWeatherAudioProfile("blood_rain", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 0.92f, 2.5f) },
                { "emp_storm", new ScarcityWeatherAudioProfile("emp_storm", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.20f, 3.0f) },
                { "glass_storm", new ScarcityWeatherAudioProfile("glass_storm", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.15f, 4.0f) },
                { "rad_hail", new ScarcityWeatherAudioProfile("rad_hail", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.10f, 3.5f) },
                { "algae_bloom", new ScarcityWeatherAudioProfile("algae_bloom", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 0.75f, -2.0f) },
                { "ash_lightning", new ScarcityWeatherAudioProfile("ash_lightning", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.10f, 3.0f) },
                { "particulate_fog", new ScarcityWeatherAudioProfile("particulate_fog", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 0.82f, -1.5f) },
                { "thermal_inversion", new ScarcityWeatherAudioProfile("thermal_inversion", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface", 0.90f, -2.0f) },
                { "ice_storm", new ScarcityWeatherAudioProfile("ice_storm", ScarcityAmbienceBed.SurfaceStormAmbience, "amb_surface_storm", 1.0f, 3.5f) },
                { "nuclear_winter", new ScarcityWeatherAudioProfile("nuclear_winter", ScarcityAmbienceBed.SurfaceAmbience, "amb_surface_snow", 0.80f, -1.0f) },
                // Authoritative Silence states:
                { "silence", new ScarcityWeatherAudioProfile("silence", ScarcityAmbienceBed.AbsoluteSilence, "(none)", 1.0f, -80.0f, isAbsoluteSilence: true) },
                { "silent_spring", new ScarcityWeatherAudioProfile("silent_spring", ScarcityAmbienceBed.AbsoluteSilence, "(none)", 1.0f, -80.0f, isAbsoluteSilence: true) },
                { "false_spring", new ScarcityWeatherAudioProfile("false_spring", ScarcityAmbienceBed.AbsoluteSilence, "(none)", 1.0f, -80.0f, isAbsoluteSilence: true) }
            };

        public ScarcityAmbienceBed CurrentBed { get; private set; } = ScarcityAmbienceBed.BunkerAmbience;
        public string CurrentWeatherKind { get; private set; } = "clear";
        public bool IsSurvivingOnSurface { get; private set; }
        public bool IsGeneratorPowered { get; private set; } = true;
        public int ShelterOccupantCount { get; private set; } = 4;
        public int ActiveAlertCount { get; private set; }
        public float CurrentDuckAttenuationDb { get; private set; }

        public ScarcityDuckingPolicy DuckingPolicy { get; } = new ScarcityDuckingPolicy();
        public ScarcityRadiationAudioState RadiationState { get; } = new ScarcityRadiationAudioState();

        /// <summary>Delegate seam fired whenever the active ambience bed transitions.</summary>
        public Action<ScarcityAmbienceBed, ScarcityAmbienceBed>? OnAmbienceBedTransitionSeam { get; set; }

        /// <summary>Delegate seam fired whenever alert ducking attenuation changes.</summary>
        public Action<float>? OnDuckingChangedSeam { get; set; }

        /// <summary>Delegate seam fired whenever geiger state or rate band changes.</summary>
        public Action<bool, GeigerRateBand>? OnGeigerLoopStateChangedSeam { get; set; }

        public void UpdateContext(bool onSurface, string weatherKind, bool isGeneratorPowered = true, int occupantCount = 4)
        {
            IsSurvivingOnSurface = onSurface;
            CurrentWeatherKind = weatherKind ?? "clear";
            IsGeneratorPowered = isGeneratorPowered;
            ShelterOccupantCount = Math.Max(0, occupantCount);

            EvaluateActiveBed();
        }

        public void TriggerAlert(string alertCueName)
        {
            ActiveAlertCount++;
            if (ActiveAlertCount > DuckingPolicy.MaxConcurrentAlerts)
            {
                ActiveAlertCount = DuckingPolicy.MaxConcurrentAlerts;
            }

            CurrentDuckAttenuationDb = DuckingPolicy.AlertDuckingAttenuationDb;
            OnDuckingChangedSeam?.Invoke(CurrentDuckAttenuationDb);
        }

        public void ReleaseAlert()
        {
            ActiveAlertCount = Math.Max(0, ActiveAlertCount - 1);
            if (ActiveAlertCount == 0)
            {
                CurrentDuckAttenuationDb = 0.0f;
            }
            OnDuckingChangedSeam?.Invoke(CurrentDuckAttenuationDb);
        }

        public void SetRadiationExposure(float doseRate)
        {
            var oldBand = RadiationState.RateBand;
            var oldActive = RadiationState.GeigerLoopActive;

            RadiationState.SetExposure(doseRate);

            if (RadiationState.RateBand != oldBand || RadiationState.GeigerLoopActive != oldActive)
            {
                OnGeigerLoopStateChangedSeam?.Invoke(RadiationState.GeigerLoopActive, RadiationState.RateBand);
            }
        }

        public void EndRadiationExposure()
        {
            var oldActive = RadiationState.GeigerLoopActive;
            var oldBand = RadiationState.RateBand;

            RadiationState.EndExposure();

            if (oldActive || oldBand != GeigerRateBand.Off)
            {
                OnGeigerLoopStateChangedSeam?.Invoke(false, GeigerRateBand.Off);
            }
        }

        public ScarcityWeatherAudioProfile GetWeatherAudioProfile(string weatherKind)
        {
            if (WeatherProfiles.TryGetValue(weatherKind, out var profile))
                return profile;

            return WeatherProfiles["clear"];
        }

        /// <summary>
        /// True when the authority has a profile for this weather key. Lets the
        /// host verify its WeatherKind mapping instead of silently relying on
        /// the clear-weather fallback.
        /// </summary>
        public static bool HasWeatherProfile(string weatherKind)
            => !string.IsNullOrEmpty(weatherKind) && WeatherProfiles.ContainsKey(weatherKind);

        public static int TotalMappedWeatherKinds => WeatherProfiles.Count;

        private void EvaluateActiveBed()
        {
            var weatherProfile = GetWeatherAudioProfile(CurrentWeatherKind);

            ScarcityAmbienceBed nextBed;

            // 1. Silence weather overrides everything
            if (weatherProfile.IsAbsoluteSilence)
            {
                nextBed = ScarcityAmbienceBed.AbsoluteSilence;
            }
            // 2. Surface Ambience / Storm
            else if (IsSurvivingOnSurface)
            {
                nextBed = weatherProfile.Bed == ScarcityAmbienceBed.SurfaceStormAmbience
                    ? ScarcityAmbienceBed.SurfaceStormAmbience
                    : ScarcityAmbienceBed.SurfaceAmbience;
            }
            // 3. Indoors Shelter Ambience
            else
            {
                if (ShelterOccupantCount >= 20)
                {
                    nextBed = ScarcityAmbienceBed.CrowdedShelterAmbience;
                }
                else
                {
                    nextBed = ScarcityAmbienceBed.BunkerAmbience;
                }
            }

            if (nextBed != CurrentBed)
            {
                var old = CurrentBed;
                CurrentBed = nextBed;
                OnAmbienceBedTransitionSeam?.Invoke(old, CurrentBed);
            }
        }
    }
}
