using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Expansion III — Mega-Location: The White Desert.
    /// A central government armored convoy caught in the open on the salt flats
    /// when the first high-altitude detonations occurred. The flash baked the salt
    /// into glass. Vehicles fused to the ground. Crews are shadows burned into
    /// the white earth. The npc_burned_patrol still guards the convoy — blind,
    /// but they hear footsteps on the glass.
    /// </summary>
    public class Location_SaltFlatsConvoy
    {
        public const string LocationId = "location_salt_flats_convoy";
        public const string DisplayName = "The White Desert";
        public const int TravelHours = 18;
        public const int DangerLevel = 9;
        public const float BaseRads = 40f; // mSv/h (doubled by glass reflection)

        // ── Required gear ─────────────────────────────────────────────
        public const string RequiredGear_Engine = "engine";
        public const string RequiredGear_Vehicle = "vehicle_system";

        // ── Unique loot ───────────────────────────────────────────────
        public const string Loot_TungstenBar = "tungsten_bar";
        public const string Loot_Ammo50BMG = "ammo_50bmg_jhp_ap";
        public const string Loot_GeneratorAlternator = "generator_alternator";

        // ── Hazard constants ──────────────────────────────────────────
        public const float GlassReflectionRadMultiplier = 2.0f;
        public const float PatrolHearingRadius = 50f;     // meters — a dropped crowbar rings like a bell
        public const float NoiseDetectionChance = 0.80f;  // if any noise made
        public const float CoverTracksSuccessRate = 0.60f;

        // ── Photoperiod: move only during darkest hours ───────────────
        public const float SafeTravelStartHour = 22f;  // 10 PM
        public const float SafeTravelEndHour = 4f;     // 4 AM

        // ── Events ────────────────────────────────────────────────────
        public event Action<string> OnPatrolDetected;         // survivorId
        public event Action<string> OnNoiseMade;              // survivorId
        public event Action<string> OnLootRecovered;
        public event Action OnConvoyApproached;
        public event Action<string> OnCoverTracksResult;      // "success" or "failure"

        private readonly System.Random _rng;
        private bool _tungstenRecovered;
        private bool _ammoRecovered;
        private bool _alternatorRecovered;
        private bool _patrolAlerted;
        private float _noiseLevel; // 0..1, accumulates

        public bool IsTungstenRecovered => _tungstenRecovered;
        public bool IsAmmoRecovered => _ammoRecovered;
        public bool IsAlternatorRecovered => _alternatorRecovered;
        public bool IsPatrolAlerted => _patrolAlerted;
        public float NoiseLevel => _noiseLevel;

        public Location_SaltFlatsConvoy(System.Random rng = null)
        {
            _rng = rng ?? new System.Random(9999);
        }

        /// <summary>
        /// Check if current hour is safe for travel (dark hours only).
        /// UV + Rad synergy doubles exposure during daylight.
        /// </summary>
        public bool IsSafeTravelHour(float currentHour)
        {
            // Wrap-around window (const: SafeTravelStartHour=22, SafeTravelEndHour=4).
            // 22f > 4f is compile-time true, so the non-wrapping branch was unreachable.
            return currentHour >= SafeTravelStartHour || currentHour < SafeTravelEndHour;
        }

        /// <summary>
        /// Get effective radiation dose, accounting for glass reflection.
        /// Daytime = 2× dose from reflection.
        /// </summary>
        public float GetEffectiveRads(float currentHour)
        {
            bool isDaytime = !IsSafeTravelHour(currentHour);
            return BaseRads * (isDaytime ? GlassReflectionRadMultiplier : 1f);
        }

        /// <summary>
        /// Make noise on the glass flats. Accumulates noise level.
        /// A dropped tool or loud action can alert the burned patrol.
        /// </summary>
        public bool MakeNoise(string survivorId, float noiseAmount)
        {
            _noiseLevel = Mathf.Clamp01(_noiseLevel + noiseAmount);
            OnNoiseMade?.Invoke(survivorId);

            if (!_patrolAlerted && _noiseLevel > 0.5f && _rng.NextDouble() < NoiseDetectionChance)
            {
                _patrolAlerted = true;
                OnPatrolDetected?.Invoke(survivorId);
                return true; // Detected
            }
            return false;
        }

        /// <summary>
        /// Attempt to cover tracks on the glass flats. Reduces noise
        /// and chance of patrol detection.
        /// </summary>
        public bool AttemptCoverTracks(string survivorId)
        {
            if (_rng.NextDouble() < CoverTracksSuccessRate)
            {
                _noiseLevel = Mathf.Max(0f, _noiseLevel - 0.3f);
                OnCoverTracksResult?.Invoke("success");
                return true;
            }
            OnCoverTracksResult?.Invoke("failure");
            return false;
        }

        /// <summary>
        /// Approach the convoy. Alerts patrol if noise is high.
        /// </summary>
        public bool ApproachConvoy(string survivorId)
        {
            OnConvoyApproached?.Invoke();
            if (_patrolAlerted)
            {
                OnPatrolDetected?.Invoke(survivorId);
                return false; // Must deal with patrol first
            }
            return true;
        }

        /// <summary>
        /// Search a convoy vehicle. Returns loot found.
        /// </summary>
        public List<string> SearchVehicle(string vehicleId, string survivorId)
        {
            var loot = new List<string>();

            // Each vehicle searched adds noise
            MakeNoise(survivorId, 0.15f);

            if (!_tungstenRecovered && _rng.NextDouble() < 0.45f)
            {
                _tungstenRecovered = true;
                loot.Add(Loot_TungstenBar);
                OnLootRecovered?.Invoke(Loot_TungstenBar);
            }

            if (!_ammoRecovered && _rng.NextDouble() < 0.35f)
            {
                _ammoRecovered = true;
                loot.Add(Loot_Ammo50BMG);
                OnLootRecovered?.Invoke(Loot_Ammo50BMG);
            }

            if (!_alternatorRecovered && _rng.NextDouble() < 0.25f)
            {
                _alternatorRecovered = true;
                loot.Add(Loot_GeneratorAlternator);
                OnLootRecovered?.Invoke(Loot_GeneratorAlternator);
            }

            return loot;
        }

        // ── Save / Load ───────────────────────────────────────────────

        public SaltFlatsSave CaptureState()
        {
            return new SaltFlatsSave
            {
                TungstenRecovered = _tungstenRecovered,
                AmmoRecovered = _ammoRecovered,
                AlternatorRecovered = _alternatorRecovered,
                PatrolAlerted = _patrolAlerted,
                NoiseLevel = _noiseLevel
            };
        }

        public void RestoreState(SaltFlatsSave save)
        {
            _tungstenRecovered = false;
            _ammoRecovered = false;
            _alternatorRecovered = false;
            _patrolAlerted = false;
            _noiseLevel = 0f;
            if (save == null) return;
            _tungstenRecovered = save.TungstenRecovered;
            _ammoRecovered = save.AmmoRecovered;
            _alternatorRecovered = save.AlternatorRecovered;
            _patrolAlerted = save.PatrolAlerted;
            _noiseLevel = save.NoiseLevel;
        }
    }

    [Serializable]
    public class SaltFlatsSave
    {
        public bool TungstenRecovered;
        public bool AmmoRecovered;
        public bool AlternatorRecovered;
        public bool PatrolAlerted;
        public float NoiseLevel;
    }
}
