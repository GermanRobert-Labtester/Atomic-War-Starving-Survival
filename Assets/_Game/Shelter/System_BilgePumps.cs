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
        /// <summary>Default flood liters harvested when a room first floods (or per daily tick).</summary>
        public const float DefaultFloodLitersPerRoom = 50f;

        public event Action OnFloodingDetected;
        public event Action<float> OnWaterRouted;         // liters purified produced
        public event Action OnPurifierBoosted;

        private BilgePumpsState _state;

        public System_BilgePumps(BilgePumpsState state = null)
        {
            _state = state ?? new BilgePumpsState();
            if (string.IsNullOrEmpty(_state.systemId))
                _state.systemId = "system_bilge_pumps";
            if (_state.purificationEfficiency <= 0f)
                _state.purificationEfficiency = 0.7f;
        }

        public string SystemId => _state.systemId;
        public float PurificationEfficiency => _state.purificationEfficiency;
        public float TotalWaterRouted => _state.totalWaterRouted;

        public void Activate()
        {
            _state.isActive = true;
        }

        public void Deactivate()
        {
            _state.isActive = false;
        }

        public bool IsActive() => _state.isActive;

        public void SetPurificationEfficiency(float efficiency)
        {
            _state.purificationEfficiency = Mathf.Clamp01(efficiency);
        }

        /// <summary>
        /// Detect whether flooding is occurring. Fires <see cref="OnFloodingDetected"/>
        /// when flooding is present (regardless of pump power — routing still needs Activate).
        /// Returns true if flooding detected.
        /// </summary>
        public bool DetectFlooding(bool isFlooding)
        {
            if (!isFlooding) return false;
            OnFloodingDetected?.Invoke();
            return true;
        }

        /// <summary>
        /// Route flood water through the purifier. Returns liters of purified water produced.
        /// No-op when pumps are inactive or flood liters ≤ 0.
        /// </summary>
        public float RouteWater(float floodLiters)
        {
            if (!_state.isActive) return 0f;
            if (floodLiters <= 0f) return 0f;

            float efficiency = _state.purificationEfficiency;
            if (efficiency <= 0f) return 0f;

            float purifiedWater = floodLiters * efficiency;
            _state.totalWaterRouted += purifiedWater;

            OnWaterRouted?.Invoke(purifiedWater);
            OnPurifierBoosted?.Invoke();

            return purifiedWater;
        }

        /// <summary>
        /// Detect flooding from a room count and, if pumps are active, route
        /// <paramref name="floodLitersPerRoom"/> × count through the purifier.
        /// Returns purified liters produced (0 if inactive or no floods).
        /// </summary>
        public float ProcessFloodedRooms(int floodedRoomCount, float floodLitersPerRoom = DefaultFloodLitersPerRoom)
        {
            if (floodedRoomCount <= 0) return 0f;
            DetectFlooding(true);
            if (!_state.isActive) return 0f;
            return RouteWater(floodedRoomCount * floodLitersPerRoom);
        }

        public float GetTotalWaterRouted() => _state.totalWaterRouted;

        public BilgePumpsState CaptureState()
        {
            return new BilgePumpsState
            {
                systemId = string.IsNullOrEmpty(_state.systemId) ? "system_bilge_pumps" : _state.systemId,
                isActive = _state.isActive,
                purificationEfficiency = _state.purificationEfficiency,
                totalWaterRouted = _state.totalWaterRouted
            };
        }

        public void RestoreState(BilgePumpsState state)
        {
            if (state == null)
            {
                _state = new BilgePumpsState();
                return;
            }

            _state = new BilgePumpsState
            {
                systemId = string.IsNullOrEmpty(state.systemId) ? "system_bilge_pumps" : state.systemId,
                isActive = state.isActive,
                purificationEfficiency = state.purificationEfficiency > 0f ? state.purificationEfficiency : 0.7f,
                totalWaterRouted = Mathf.Max(0f, state.totalWaterRouted)
            };
        }
    }
}
