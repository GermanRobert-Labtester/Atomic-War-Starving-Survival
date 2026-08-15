using System;
using UnityEngine;
using AtomicWar._Game.Core;

namespace AtomicWar._Game.Shelter.Modules
{
    [Serializable]
    public class SubBayModuleState
    {
        public string moduleId = "shelter_module_sub_bay";
        public string displayName = "Flooded Submarine Bay";
        public bool isBuilt = false;
        public float waterDepthMeters = 4.5f;       // 0m = fully dry, >3m = flooded
        public float maxDepthMeters = 6.0f;
        public float structuralIntegrity = 100f;    // 0..100%
        public float sealIntegrity = 100f;          // 0..100%
        public bool isDivingUnlocked = false;       // Unlocked when depth <= 1.0m or diving gear equipped
        public int completedDivesCount = 0;
    }

    /// <summary>
    /// Expansion V / Spec §3.3: Module: Submarine Bay.
    /// Manages the flooded sub-pen bay room: water-depth state, structural pressure,
    /// seal-failure risk, and consumes bilge-pump throughput from System_BilgePumps
    /// to unlock underwater scavenging and diving expeditions.
    /// </summary>
    public class ShelterModule_SubBay
    {
        private SubBayModuleState _state = new SubBayModuleState();

        public event Action<SubBayModuleState> OnSubBayDrained;
        public event Action<SubBayModuleState> OnSealBreached;
        public event Action<SubBayModuleState, int> OnDiveCompleted;

        public SubBayModuleState State => _state;
        public bool IsBuilt => _state.isBuilt;
        public float WaterDepthMeters => _state.waterDepthMeters;
        public bool IsDivingUnlocked => _state.isDivingUnlocked;

        public void BuildModule()
        {
            _state.isBuilt = true;
        }

        /// <summary>
        /// Applies bilge pumping throughput to reduce water depth.
        /// </summary>
        public float PumpWater(System_BilgePumps pumps, float pumpRateMetersPerTick = 0.5f)
        {
            if (!_state.isBuilt || pumps == null || !pumps.IsActive() || _state.waterDepthMeters <= 0f)
                return 0f;

            float drained = Mathf.Min(_state.waterDepthMeters, pumpRateMetersPerTick);
            _state.waterDepthMeters = Mathf.Max(0f, _state.waterDepthMeters - drained);

            // Water converted to liters (1 meter depth ≈ 200 liters in sub pen)
            pumps.RouteWater(drained * 200f);

            if (_state.waterDepthMeters <= 1.0f && !_state.isDivingUnlocked)
            {
                _state.isDivingUnlocked = true;
                OnSubBayDrained?.Invoke(_state);
            }

            return drained;
        }

        /// <summary>
        /// Daily/hourly simulation tick: calculates hydrostatic pressure damage on seals if deep water persists.
        /// </summary>
        public void Tick(float hours, float stormPressureMultiplier = 1.0f)
        {
            if (!_state.isBuilt) return;

            // Deep water exerts pressure on blast seals
            if (_state.waterDepthMeters > 3.0f)
            {
                float wear = (hours / 24f) * (_state.waterDepthMeters / 6.0f) * 2.0f * stormPressureMultiplier;
                _state.sealIntegrity = Mathf.Clamp(_state.sealIntegrity - wear, 0f, 100f);

                if (_state.sealIntegrity <= 0f)
                {
                    _state.waterDepthMeters = Mathf.Min(_state.maxDepthMeters, _state.waterDepthMeters + (hours * 0.2f));
                    OnSealBreached?.Invoke(_state);
                }
            }
        }

        public bool PerformDive()
        {
            if (!_state.isBuilt || !_state.isDivingUnlocked)
                return false;

            _state.completedDivesCount++;
            OnDiveCompleted?.Invoke(_state, _state.completedDivesCount);
            return true;
        }

        public void RepairSeals(float repairAmount)
        {
            _state.sealIntegrity = Mathf.Clamp(_state.sealIntegrity + repairAmount, 0f, 100f);
        }

        public SubBayModuleState CaptureState() => _state;

        public void RestoreState(SubBayModuleState saved)
        {
            _state = saved ?? new SubBayModuleState();
        }
    }
}
