using System;

namespace AtomicWar._Game.Environment
{
    [Serializable]
    public class AlgaeBloomState
    {
        public string weatherId = "weather_algae_bloom";
        public bool isActive = false;
        public int durationDays = 7;
        public int daysRemaining = 7;
        public float toxicGasPerRiverNode = 20f;
        public bool waterLethallyPoisonous = false;
        public bool requiresCharcoalBoil = true;
    }

    /// <summary>
    /// Prompt #571: Weather — Toxic Algae Bloom.
    /// Warmer temperatures cause rivers to bloom. RiverNodes generate massive
    /// ambient ToxicGas. Any DirtyWater harvested during this week is fatally
    /// poisonous and bypasses standard purifiers — only boiling with Charcoal
    /// makes it safe. Save/load safe. Plain C#.
    /// </summary>
    public class Weather_AlgaeBloom
    {
        /// <summary>Damage dealt when lethally-poisonous water is consumed un-boiled.</summary>
        public const float PoisoningDamage = 80f;

        /// <summary>Minimum ambient temperature (°C) required for activation.</summary>
        public const float ActivationTemperatureThreshold = 15f;

        private AlgaeBloomState _state = new AlgaeBloomState();

        // -- Events --
        public event Action<AlgaeBloomState> OnAlgaeBloomStarted;
        public event Action<AlgaeBloomState> OnAlgaeBloomEnded;
        public event Action<AlgaeBloomState, float> OnToxicWaterConsumed;

        public AlgaeBloomState State => _state;

        /// <summary>
        /// Checks whether conditions for a toxic algae bloom are met.
        /// Activates when ambient temperature exceeds 15°C and a river node is nearby.
        /// </summary>
        public void CheckActivation(float ambientTemperature, bool isRiverNearby)
        {
            if (_state.isActive) return;

            if (ambientTemperature > ActivationTemperatureThreshold && isRiverNearby)
            {
                _state.isActive = true;
                _state.daysRemaining = _state.durationDays;
                _state.waterLethallyPoisonous = true;
                _state.requiresCharcoalBoil = true;

                OnAlgaeBloomStarted?.Invoke(_state);
            }
        }

        /// <summary>
        /// Called once per game-day. Decrements the remaining duration and
        /// deactivates the bloom when it expires.
        /// </summary>
        public void TickDay()
        {
            if (!_state.isActive) return;

            _state.daysRemaining = Math.Max(0, _state.daysRemaining - 1);

            if (_state.daysRemaining <= 0)
            {
                _state.isActive = false;
                _state.waterLethallyPoisonous = false;

                OnAlgaeBloomEnded?.Invoke(_state);
            }
        }

        /// <summary>
        /// Returns the toxic gas level emitted by a given map node.
        /// River nodes (identified by "river" in the id) produce
        /// <see cref="AlgaeBloomState.toxicGasPerRiverNode"/> during a bloom.
        /// </summary>
        public float GetToxicGasLevel(string nodeId)
        {
            if (!_state.isActive) return 0f;
            if (string.IsNullOrEmpty(nodeId)) return 0f;

            if (nodeId.IndexOf("river", StringComparison.OrdinalIgnoreCase) >= 0)
                return _state.toxicGasPerRiverNode;

            return 0f;
        }

        /// <summary>
        /// Determines whether water is safe to drink. During an algae bloom,
        /// standard purifiers FAIL — only boiling with charcoal makes water safe.
        /// </summary>
        public bool IsWaterSafeToDrink(bool wasBoiledWithCharcoal, bool hasStandardPurifier)
        {
            if (!_state.isActive || !_state.waterLethallyPoisonous)
            {
                // No bloom — standard purifier is sufficient.
                return hasStandardPurifier || wasBoiledWithCharcoal;
            }

            // During bloom: only charcoal boil works.
            return wasBoiledWithCharcoal;
        }

        /// <summary>
        /// Returns the poisoning damage dealt when lethally-poisonous water is
        /// consumed without proper charcoal boiling. Fires
        /// <see cref="OnToxicWaterConsumed"/>.
        /// </summary>
        public float GetWaterPoisoningDamage()
        {
            if (!_state.isActive || !_state.waterLethallyPoisonous) return 0f;

            OnToxicWaterConsumed?.Invoke(_state, PoisoningDamage);
            return PoisoningDamage;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public AlgaeBloomState GetState() => _state;

        public void RestoreState(AlgaeBloomState state)
        {
            _state = state ?? new AlgaeBloomState();
        }
    }
}
