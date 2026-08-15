using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Environment
{
    /// <summary>
    /// Ash Drift Burial System (#52) — heavy ash storms gradually block
    /// surface vents and hatch doors. Neglecting shovel work causes carbon
    /// dioxide spikes inside the bunker and triggers claustrophobic
    /// emergency events.
    ///
    /// Plain C#, save-safe. Ties into existing AshAccumulationSystem.
    /// </summary>
    public class AshDriftBurialSystem
    {
        public const float AshAccumulationRatePerStorm = 15f;
        public const float AshNaturalDecayPerDay = 2f;
        public const float AshClearingRatePerHour = 10f;
        public const float HatchBlockedThreshold = 80f;
        public const float VentBlockedThreshold = 60f;
        public const float CO2SpikeThreshold = 90f;
        public const float CO2MoraleDrainPerHour = 3f;
        public const float CO2HealthDrainPerHour = 0.5f;

        public event Action<float> OnAshLevelChanged;
        // currentAshLevel
        public event Action OnHatchBlocked;
        public event Action OnVentsBlocked;
        public event Action OnCO2Spike;
        public event Action OnHatchCleared;
        public event Action OnVentsCleared;

        private float _ashAccumulation;

        public float AshAccumulation => _ashAccumulation;

        public void OnAshStorm(float severity)
        {
            _ashAccumulation = Math.Min(100f,
                _ashAccumulation + AshAccumulationRatePerStorm * severity);
            OnAshLevelChanged?.Invoke(_ashAccumulation);

            if (_ashAccumulation >= VentBlockedThreshold)
                OnVentsBlocked?.Invoke();
            if (_ashAccumulation >= HatchBlockedThreshold)
                OnHatchBlocked?.Invoke();
            if (_ashAccumulation >= CO2SpikeThreshold)
                OnCO2Spike?.Invoke();
        }

        public float ClearAsh(float workHours)
        {
            float cleared = Math.Min(_ashAccumulation,
                AshClearingRatePerHour * workHours);
            _ashAccumulation -= cleared;

            if (_ashAccumulation < HatchBlockedThreshold)
                OnHatchCleared?.Invoke();
            if (_ashAccumulation < VentBlockedThreshold)
                OnVentsCleared?.Invoke();

            OnAshLevelChanged?.Invoke(_ashAccumulation);
            return cleared;
        }

        public void Tick(float gameHours)
        {
            _ashAccumulation = Math.Max(0f,
                _ashAccumulation - AshNaturalDecayPerDay * (gameHours / 24f));

            if (_ashAccumulation >= CO2SpikeThreshold)
            {
                // CO2 effects applied by host via event
            }
        }

        public bool IsHatchBlocked => _ashAccumulation >= HatchBlockedThreshold;
        public bool AreVentsBlocked => _ashAccumulation >= VentBlockedThreshold;
        public bool IsCO2Critical => _ashAccumulation >= CO2SpikeThreshold;
    }
}
