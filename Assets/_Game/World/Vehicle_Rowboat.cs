using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class RowboatState
    {
        public string vehicleId = "vehicle_rowboat";
        public int maxPassengers = 2;
        public float speedMultiplier = 1.8f;
        public float fuelConsumption = 0f; // Human-powered
        public float staminaCostPerHour = 15f;
        public float sniperVulnerability = 0.30f;
        public bool isCrafted = false;
        public float hullDurability = 80f;
    }

    /// <summary>
    /// Prompt #570: Vehicle Type: The Rowboat.
    /// Crafted using Wood and Sealant. Fast safe travel along RiverNodes.
    /// Bypasses land blockades. Requires high Stamina survivor to row.
    /// Completely exposed to Sniper fire from riverbanks.
    /// </summary>
    public class Vehicle_Rowboat
    {
        private RowboatState _state = new RowboatState();

        public event Action<RowboatState> OnRowboatCrafted;
        public event Action<RowboatState, float> OnRowingFatigue;
        public event Action<RowboatState, float> OnSniperHit;

        public RowboatState State => _state;

        public bool TryCraft(bool hasWood, bool hasSealant)
        {
            if (!hasWood || !hasSealant || _state.isCrafted) return false;

            _state.isCrafted = true;
            _state.hullDurability = 80f;
            OnRowboatCrafted?.Invoke(_state);
            return true;
        }

        public float RowHour(float staminaPercent, System.Random rng)
        {
            if (!_state.isCrafted) return 0f;

            float staminaCost = _state.staminaCostPerHour;

            // Low stamina survivors row slower
            if (staminaPercent < 30f)
            {
                staminaCost *= 1.5f;
            }

            OnRowingFatigue?.Invoke(_state, staminaCost);
            return GetEffectiveSpeed(staminaPercent);
        }

        public bool CheckSniperFire(System.Random rng)
        {
            if (!_state.isCrafted) return false;

            if (rng.NextDouble() < _state.sniperVulnerability)
            {
                float damage = (float)(rng.NextDouble() * 20.0 + 10.0);
                _state.hullDurability -= damage;
                OnSniperHit?.Invoke(_state, damage);
                return true;
            }

            return false;
        }

        public float GetEffectiveSpeed(float staminaPercent = 100f)
        {
            if (!_state.isCrafted) return 0f;

            float speedFactor = staminaPercent >= 50f ? 1f : staminaPercent / 50f;
            return _state.speedMultiplier * speedFactor;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RowboatState CaptureState() => _state;

        public void RestoreState(RowboatState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
