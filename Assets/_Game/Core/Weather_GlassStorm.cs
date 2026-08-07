using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GlassStormState
    {
        public string weatherId = "weather_glass_storm";
        public string displayName = "Glass Storm";
        public float durationHours = 12f;
        public float hoursRemaining = 0f;
        public float hazmatDurabilityDamage = 50f;
        public string bleedAfflictionId = "laceration";
        public float hatchSecurityReduction = 5f;
    }

    /// <summary>
    /// Prompt #649: Weather — Glass Storm.
    /// High winds pick up vitrified sand, shredding HazmatSuits instantly and inflicting
    /// Bleeding. Sandblasts the shelter hatch, causing permanent ShelterSecurity reduction.
    /// </summary>
    public class Weather_GlassStorm
    {
        private GlassStormState _state = new GlassStormState();

        // -- Events --
        public event Action<GlassStormState> OnGlassStormTriggered;
        public event Action<GlassStormState> OnGlassStormEnded;
        public event Action<GlassStormState, float> OnHazmatShredded;
        public event Action<GlassStormState, float> OnHatchSandblasted;

        public GlassStormState State => _state;

        public bool IsActive => _state.hoursRemaining > 0f;

        /// <summary>
        /// Triggers the glass storm for its full configured duration.
        /// </summary>
        public void Trigger()
        {
            _state.hoursRemaining = _state.durationHours;
            OnGlassStormTriggered?.Invoke(_state);
        }

        /// <summary>
        /// Per-hour tick. Damages HazmatSuit durability and reduces hatch security.
        /// Returns the bleed affliction id if the survivor is outside without
        /// adequate protection, or null otherwise.
        /// </summary>
        public string TickHour(ref float hazmatDurability, ref float hatchSecurity)
        {
            if (!IsActive) return null;

            _state.hoursRemaining = Mathf.Max(0f, _state.hoursRemaining - 1f);

            // Shred HazmatSuit durability
            float suitDamage = _state.hazmatDurabilityDamage;
            hazmatDurability = Mathf.Max(0f, hazmatDurability - suitDamage);
            OnHazmatShredded?.Invoke(_state, suitDamage);

            // Sandblast hatch — permanent security reduction
            float secLoss = _state.hatchSecurityReduction;
            hatchSecurity = Mathf.Max(0f, hatchSecurity - secLoss);
            OnHatchSandblasted?.Invoke(_state, secLoss);

            string affliction = null;
            // If the suit is destroyed, survivor takes bleeding
            if (hazmatDurability <= 0f)
            {
                affliction = _state.bleedAfflictionId;
            }

            if (!IsActive)
            {
                OnGlassStormEnded?.Invoke(_state);
            }

            return affliction;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public GlassStormState GetState() => _state;

        public GlassStormState CaptureState() => GetState();

        public void RestoreState(GlassStormState state)
        {
            _state = state ?? new GlassStormState();
        }
    }
}
