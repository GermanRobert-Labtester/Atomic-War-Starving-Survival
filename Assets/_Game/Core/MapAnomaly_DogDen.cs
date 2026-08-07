using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class DogDenState
    {
        public string anomalyId = "map_anomaly_dog_den";
        public string displayName = "The Feral Dog Den";
        public float hostileEncounterRate = 1.0f; // 100% hostile encounter
        public List<string> hoarderLootPool = new List<string> { "animal_bones", "dropped_survivor_backpack", "old_leather_shoes", "scrap_leather" };
    }

    /// <summary>
    /// Prompt #451: Anomaly: The Feral Dog Den.
    /// Collapsed subway entrance occupied by feral dog packs.
    /// Has a 100% HostileEncounter rate, but contains hoarded survivor loot at the center.
    /// </summary>
    public class MapAnomaly_DogDen
    {
        private DogDenState _state = new DogDenState();

        public event Action<DogDenState, List<string>> OnHoarderLootClaimed;

        public DogDenState State => _state;

        public List<string> ClearDenAndLoot(bool survivedPackAttack)
        {
            if (survivedPackAttack)
            {
                OnHoarderLootClaimed?.Invoke(_state, _state.hoarderLootPool);
                return new List<string>(_state.hoarderLootPool);
            }
            return new List<string>();
        }

        // ── Save / Load ────────────────────────────────────────────────

        public DogDenState CaptureState()
        {
            return new DogDenState
            {
                anomalyId = _state.anomalyId,
                displayName = _state.displayName,
                hostileEncounterRate = _state.hostileEncounterRate,
                hoarderLootPool = _state.hoarderLootPool != null ? new System.Collections.Generic.List<string>(_state.hoarderLootPool) : new System.Collections.Generic.List<string>(),
            };
        }

        public void RestoreState(DogDenState saved)
        {
            if (saved == null)
            {
                _state = new DogDenState();
                return;
            }
            _state = new DogDenState
            {
                anomalyId = saved.anomalyId,
                displayName = saved.displayName,
                hostileEncounterRate = saved.hostileEncounterRate,
                hoarderLootPool = saved.hoarderLootPool != null ? new System.Collections.Generic.List<string>(saved.hoarderLootPool) : new System.Collections.Generic.List<string>(),
            };
        }
    }
}
