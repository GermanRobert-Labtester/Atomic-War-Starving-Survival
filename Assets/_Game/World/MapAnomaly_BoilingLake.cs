using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class BoilingLakeState
    {
        public string anomalyId = "map_anomaly_boiling_lake";
        public string displayName = "The Boiling Lake";
        public float heatOutputPerTick = 50f;
        public float hullDamagePerTick = 15f;
        public float centerIslandLootValue = 300f;
        public float reactorMeltdownRadLevel = 500f;
    }

    /// <summary>
    /// Prompt #578: Anomaly: The Boiling Lake.
    /// Lake heated by a submerged melting nuclear reactor. Generates infinite Heat.
    /// Crossing by boat damages hull every tick. Looting the center island requires
    /// perfect vehicle durability management.
    /// </summary>
    public class MapAnomaly_BoilingLake
    {
        private BoilingLakeState _state = new BoilingLakeState();

        public event Action<BoilingLakeState, float> OnBoilingLakeCrossed;
        public event Action<BoilingLakeState, float> OnHullDamaged;
        public event Action<BoilingLakeState, float> OnCenterIslandLooted;
        public event Action<BoilingLakeState> OnBoatSunk;

        public BoilingLakeState State => _state;

        public float CrossByBoat(float currentHullDurability, float ticks)
        {
            float totalDamage = _state.hullDamagePerTick * ticks;
            float remainingDurability = currentHullDurability - totalDamage;

            OnHullDamaged?.Invoke(_state, totalDamage);

            if (remainingDurability <= 0f)
            {
                OnBoatSunk?.Invoke(_state);
                return 0f; // Boat sunk
            }

            OnBoilingLakeCrossed?.Invoke(_state, remainingDurability);
            return remainingDurability;
        }

        public float CollectHeat(float ticks)
        {
            return _state.heatOutputPerTick * ticks;
        }

        public float TryLootCenterIsland(float hullDurability, System.Random rng)
        {
            if (hullDurability <= 0f)
            {
                OnBoatSunk?.Invoke(_state);
                return 0f;
            }

            // Partial loot based on remaining hull condition
            float conditionFactor = Math.Min(hullDurability / 80f, 1f);
            float lootGained = _state.centerIslandLootValue * conditionFactor;

            OnCenterIslandLooted?.Invoke(_state, lootGained);
            return lootGained;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public BoilingLakeState CaptureState()
        {
            return new BoilingLakeState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                heatOutputPerTick = _state.heatOutputPerTick,
                hullDamagePerTick = _state.hullDamagePerTick,
                centerIslandLootValue = _state.centerIslandLootValue,
                reactorMeltdownRadLevel = _state.reactorMeltdownRadLevel,
            };
        }

        public void RestoreState(BoilingLakeState saved)
        {
            if (saved == null)
            {
                _state = new BoilingLakeState();
                return;
            }
            _state = new BoilingLakeState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                heatOutputPerTick = saved.heatOutputPerTick,
                hullDamagePerTick = saved.hullDamagePerTick,
                centerIslandLootValue = saved.centerIslandLootValue,
                reactorMeltdownRadLevel = saved.reactorMeltdownRadLevel,
            };
        }
    }
}
