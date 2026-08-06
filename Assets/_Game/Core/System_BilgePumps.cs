using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BilgePumpsState
    {
        public string systemId = "system_bilge_pumps";
        public bool isActive = false;
        public float purificationEfficiency = 0.7f; // 70% of flood water becomes purified
        public float totalWaterRouted = 0f;
    }

    /// <summary>
    /// Prompt #806: Automated Bilge Pumps.
    /// Detects flooding and routes floodwater to WaterPurifier.
    /// Disaster becomes water boon — flood water is converted to purified water.
    /// </summary>
    public class System_BilgePumps
    {
        public event Action OnFloodingDetected;
        public event Action<float> OnWaterRouted;         // litersRouted
        public event Action OnPurifierBoosted;

        private BilgePumpsState _state;

        public System_BilgePumps(BilgePumpsState state = null)
        {
            _state = state ?? new BilgePumpsState();
        }

        public string SystemId => _state.systemId;

        public void Activate()
        {
            _state.isActive = true;
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        public bool IsActive() => _state.isActive;

        /// <summary>
        /// Detect whether flooding is occurring. Triggers event if flooding detected and pumps active.
        /// Returns true if flooding detected.
        /// </summary>
        public bool DetectFlooding(bool isFlooding)
        {
            if (isFlooding)
            {
                OnFloodingDetected?.Invoke();
                return true;
            }

            return false;
        }

        /// <summary>
        /// Route flood water to the purifier. Converts flood water to purified water.
        /// Returns liters of purified water produced.
        /// </summary>
        public float RouteWater(float floodLiters)
        {
            if (!_state.isActive)
            {
                Debug.LogWarning("[System_BilgePumps] RouteWater called but pumps are not active.");
                return 0f;
            }

            if (floodLiters <= 0f)
                return 0f;

            float purifiedWater = floodLiters * _state.purificationEfficiency;
            _state.totalWaterRouted += purifiedWater;

            OnWaterRouted?.Invoke(purifiedWater);
            OnPurifierBoosted?.Invoke();

            return purifiedWater;
        }

        public float GetTotalWaterRouted() => _state.totalWaterRouted;

        public BilgePumpsState CaptureState()
        {
            return new BilgePumpsState
            {
                systemId = _state.systemId,
                isActive = _state.isActive,
                purificationEfficiency = _state.purificationEfficiency,
                totalWaterRouted = _state.totalWaterRouted
            };
        }

        public void RestoreState(BilgePumpsState state)
        {
            _state = state ?? new BilgePumpsState();
        }
    }
}
