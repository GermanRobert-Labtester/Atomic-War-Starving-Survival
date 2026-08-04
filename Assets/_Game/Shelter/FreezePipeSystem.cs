using System;
using UnityEngine;

namespace AtomicWar._Game.Shelter
{
    /// <summary>
    /// Freezing Pipes & Water Loss (Prompt #53). Links TemperatureSystem to
    /// WaterStorage. If the room containing water drops below 0°C, pipes freeze —
    /// water cannot be used. After 48 hours frozen, pipes burst, deleting 50% of
    /// stored water. Players must heat critical infrastructure, not just beds.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class FreezePipeSystem
    {
        /// <summary>Temperature (°C) at/below which pipes begin freezing.</summary>
        public const float FreezeThresholdC = 0f;

        /// <summary>Hours frozen before pipes burst.</summary>
        public const float BurstThresholdHours = 48f;

        /// <summary>Fraction of stored water destroyed on burst.</summary>
        public const float BurstWaterLossFraction = 0.5f;

        private float _frozenHours;
        private bool _isFrozen;
        private bool _hasBurst;

        // Delegates — injected after construction.
        private Func<float> _getWaterRoomTempC;
        private Func<WaterStorage> _getWaterStorage;

        // -- Public state --
        public bool IsFrozen => _isFrozen;
        public bool HasBurst => _hasBurst;
        public float FrozenHours => _frozenHours;
        public float BurstCountdownHours => _isFrozen ? Mathf.Max(0f, BurstThresholdHours - _frozenHours) : BurstThresholdHours;

        // -- Events --
        public event Action OnPipesFroze;
        public event Action OnPipesThawed;
        public event Action<float> OnPipesBurst;     // waterLost

        public FreezePipeSystem() { }

        /// <summary>Wire temperature and water storage delegates.</summary>
        public void Bind(
            Func<float> getWaterRoomTempC,
            Func<WaterStorage> getWaterStorage)
        {
            _getWaterRoomTempC = getWaterRoomTempC;
            _getWaterStorage = getWaterStorage;
        }

        // -----------------------------------------------------------------
        // Tick
        // -----------------------------------------------------------------

        /// <summary>Check temperature and advance freeze/burst state.</summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            float temp = _getWaterRoomTempC?.Invoke() ?? 20f;
            bool shouldFreeze = temp <= FreezeThresholdC;

            if (shouldFreeze && !_isFrozen)
            {
                _isFrozen = true;
                _frozenHours = 0f;
                OnPipesFroze?.Invoke();
            }
            else if (!shouldFreeze && _isFrozen)
            {
                _isFrozen = false;
                _frozenHours = 0f;
                OnPipesThawed?.Invoke();
            }

            if (_isFrozen && !_hasBurst)
            {
                _frozenHours += gameHours;
                if (_frozenHours >= BurstThresholdHours)
                {
                    BurstPipes();
                }
            }
        }

        private void BurstPipes()
        {
            _hasBurst = true;
            _isFrozen = false;

            var storage = _getWaterStorage?.Invoke();
            if (storage == null) return;

            float lost = (storage.CleanWater + storage.DirtyWater + storage.IrradiatedWater)
                * BurstWaterLossFraction;

            // Drain from clean first, then dirty, then irradiated.
            float remaining = lost;
            remaining -= storage.ConsumeClean(remaining);
            if (remaining > 0f) remaining -= storage.ConsumeDirty(remaining);
            if (remaining > 0f) storage.ConsumeIrradiated(remaining);

            OnPipesBurst?.Invoke(lost);
        }

        /// <summary>Whether water can currently be used (not frozen, not burst-drained).</summary>
        public bool CanUseWater => !_isFrozen;

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public FreezePipeSave CaptureState()
        {
            return new FreezePipeSave
            {
                FrozenHours = _frozenHours,
                IsFrozen = _isFrozen,
                HasBurst = _hasBurst
            };
        }

        public void RestoreState(FreezePipeSave save)
        {
            if (save == null)
            {
                _frozenHours = 0f;
                _isFrozen = false;
                _hasBurst = false;
                return;
            }
            _frozenHours = save.FrozenHours;
            _isFrozen = save.IsFrozen;
            _hasBurst = save.HasBurst;
        }
    }

    [Serializable]
    public class FreezePipeSave
    {
        public float FrozenHours;
        public bool IsFrozen;
        public bool HasBurst;
    }
}
