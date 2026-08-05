using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SnowshoesState
    {
        public string itemId = "item_snowshoes";
        public string displayName = "Snowshoes";
        public bool negatesBlizzardAshDriftPenalty = true;
        public float currentDurability = 100f;
        public float asphaltDurabilityDrainPerKm = 25f; // Fast wear on bare asphalt
    }

    /// <summary>
    /// Prompt #421: Gear: Snowshoes.
    /// Negates travel time penalties of Blizzard and AshDrifts,
    /// but degrades rapidly when walking on bare asphalt routes.
    /// </summary>
    public class Item_Snowshoes
    {
        private SnowshoesState _state = new SnowshoesState();

        public event Action<SnowshoesState, float> OnAsphaltDegradationApplied;

        public SnowshoesState State => _state;

        public bool TravelWithSnowshoes(bool isBlizzardOrAshDrift, bool isBareAsphalt, float kmTraveled)
        {
            if (_state.currentDurability <= 0f) return false;

            if (isBareAsphalt)
            {
                float drain = kmTraveled * _state.asphaltDurabilityDrainPerKm;
                _state.currentDurability = Mathf.Max(0f, _state.currentDurability - drain);
                OnAsphaltDegradationApplied?.Invoke(_state, _state.currentDurability);
            }

            return _state.negatesBlizzardAshDriftPenalty;
        }
    }
}
