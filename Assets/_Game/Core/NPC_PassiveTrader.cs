using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PassiveTraderState
    {
        public string id = "npc_passive_trader";
        public string displayName = "Passive Traders";
        public int guardCount = 6;
        public float basePriceMultiplier = 1.0f;
    }

    /// <summary>
    /// Prompt #362: NPC Encounter: Passive Traders.
    /// Heavily guarded caravans offering dynamic exchange rates based on active WeatherSystem conditions
    /// (e.g., selling water for 5x price during a Fallout Storm).
    /// </summary>
    public class NPC_PassiveTrader
    {
        private PassiveTraderState _state = new PassiveTraderState();

        public event Action<PassiveTraderState, string, float> OnWeatherExchangeRateApplied;

        public PassiveTraderState State => _state;

        public float GetPriceMultiplierForItem(string itemId, string activeWeatherKind)
        {
            float mult = _state.basePriceMultiplier;

            if (itemId == "clean_water" || itemId == "water_bottle")
            {
                if (activeWeatherKind == "FalloutStorm" || activeWeatherKind == "Fallout_Storm")
                {
                    mult *= 5.0f; // 5x price during Fallout Storm
                }
                else if (activeWeatherKind == "DeepFreeze" || activeWeatherKind == "Deep_Freeze")
                {
                    mult *= 2.5f;
                }
            }

            OnWeatherExchangeRateApplied?.Invoke(_state, itemId, mult);
            return mult;
        }
    }
}
