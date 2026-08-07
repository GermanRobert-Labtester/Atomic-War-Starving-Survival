using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class FeralFloraState
    {
        public string crisisId = "crisis_feral_flora";
        public bool isOvergrown = false;
        public int daysSinceLastHarvest = 0;
        public int overgrowthThresholdDays = 10;
        public float airVentClogPercent = 0f;
        public float plantHealthPool = 200f;
        public bool requiresMachete = true;
    }

    /// <summary>
    /// Prompt #553: Crisis — Feral Flora (Greenhouse Rebellion).
    /// If MutatedFungi are left unharvested in Hydroponics bay for too long,
    /// they overgrow. Vines actively attack the room's AirVents, clogging them.
    /// Player must send a survivor with Machete to clear the room (melee combat
    /// against plants). Save/load safe. Plain C#.
    /// </summary>
    public class Crisis_FeralFlora
    {
        private FeralFloraState _state = new FeralFloraState();

        // -- Events --
        public event Action<FeralFloraState> OnFloraOvergrown;
        public event Action<FeralFloraState, float> OnAirVentClogged;
        public event Action<FeralFloraState> OnFloraCleared;

        public FeralFloraState State => _state;

        /// <summary>
        /// Called once per game-day. If unharvested fungi are present (count &gt; 0),
        /// increments the days counter. Triggers overgrowth when the threshold is reached.
        /// </summary>
        public void TickDay(float unharvestedFungiCount)
        {
            if (_state.plantHealthPool <= 0f) return;

            if (unharvestedFungiCount > 0f)
            {
                _state.daysSinceLastHarvest++;

                if (!_state.isOvergrown
                    && _state.daysSinceLastHarvest >= _state.overgrowthThresholdDays)
                {
                    _state.isOvergrown = true;
                    OnFloraOvergrown?.Invoke(_state);
                }

                if (_state.isOvergrown)
                {
                    // Clog grows ~10% per day after overgrowth, capped at 100%.
                    float clogDelta = 10f;
                    _state.airVentClogPercent = Math.Min(100f,
                        _state.airVentClogPercent + clogDelta);
                }
            }
            else
            {
                // Harvesting resets the counter.
                _state.daysSinceLastHarvest = 0;
            }
        }

        /// <summary>
        /// Applies air-quality degradation proportional to the current clog percent.
        /// Returns the effective air-quality reduction (higher = worse).
        /// </summary>
        public float ApplyVentClogDamage(float shelterAirQuality)
        {
            if (!_state.isOvergrown || _state.airVentClogPercent <= 0f)
                return shelterAirQuality;

            float reductionFactor = _state.airVentClogPercent / 100f;
            float newQuality = shelterAirQuality * (1f - reductionFactor);

            OnAirVentClogged?.Invoke(_state, _state.airVentClogPercent);
            return newQuality;
        }

        /// <summary>
        /// Melee combat resolution against overgrown plants. Requires a Machete.
        /// Returns the damage dealt to the plant health pool.
        /// </summary>
        public float TryClearWithMachete(float meleeSkill, System.Random rng)
        {
            if (!_state.isOvergrown) return 0f;
            if (_state.plantHealthPool <= 0f) return 0f;

            // Base damage 30, scaled by melee skill (0..1 → 0.5×..1.5×), ±20% RNG.
            float skillMultiplier = 0.5f + meleeSkill;
            float variance = 0.8f + (float)(rng.NextDouble() * 0.4);
            float damage = 30f * skillMultiplier * variance;

            _state.plantHealthPool = Math.Max(0f, _state.plantHealthPool - damage);

            // As plant health drops, clog clears proportionally.
            float healthFraction = _state.plantHealthPool / 200f;
            _state.airVentClogPercent = Math.Max(0f, 100f * healthFraction);

            if (IsResolved())
            {
                _state.isOvergrown = false;
                _state.airVentClogPercent = 0f;
                _state.daysSinceLastHarvest = 0;
                OnFloraCleared?.Invoke(_state);
            }

            return damage;
        }

        /// <summary>Returns true when the plant health pool is depleted.</summary>
        public bool IsResolved() => _state.plantHealthPool <= 0f;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public FeralFloraState GetState() => _state;

        // ── Save / Load ────────────────────────────────────────────────


        public FeralFloraState CaptureState() => _state;



        public void RestoreState(FeralFloraState state)
        {
            _state = state ?? new FeralFloraState();
        }

}
}
