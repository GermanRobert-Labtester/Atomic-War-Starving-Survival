using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SurvivalistsState
    {
        public string id = "npc_survivalists";
        public string displayName = "Hardcore Survivalists";
        public bool isPassive = true;
        public float lethalityRating = 300f; // Extremely lethal
        public bool wearsHazmatSuit = true;
        public List<string> acceptedTradeItems = new List<string> { "military_ammunition_crate", "fuel_canister" };
    }

    /// <summary>
    /// Prompt #360: NPC Encounter: Hardcore Survivalists.
    /// Preppers in HazmatSuits carrying AssaultRifles. Extremely passive, highly lethal.
    /// Only trades for Ammunition or Fuel. Wipes out early-game players if attacked.
    /// </summary>
    public class NPC_Survivalists
    {
        private SurvivalistsState _state = new SurvivalistsState();

        public event Action<SurvivalistsState, string> OnTradeCompleted;
        public event Action<SurvivalistsState, float> OnPlayerWiped;

        public SurvivalistsState State => _state;

        public bool TradeForAmmoOrFuel(string offeredItem, out string receivedItem)
        {
            receivedItem = null;
            if (_state.acceptedTradeItems.Contains(offeredItem))
            {
                receivedItem = "medical_trauma_kit_pristine";
                OnTradeCompleted?.Invoke(_state, offeredItem);
                return true;
            }
            return false;
        }

        public float DefendAgainstAttack(float playerPowerLevel)
        {
            _state.isPassive = false;
            float retaliatoryDamage = _state.lethalityRating;

            if (playerPowerLevel < 150f)
            {
                OnPlayerWiped?.Invoke(_state, retaliatoryDamage);
            }
            return retaliatoryDamage;
        }
    }
}
