using System;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ToyState
    {
        public string itemId = "item_toys";
        public float baseTradeValue = 1f;
        public float factionMultiplier = 5.0f;
        public bool cultDestroys = true;
    }

    public class Item_Toys
    {
        public event Action<string, string, float> OnToyTraded;
        public event Action<string, string> OnToyDestroyed;

        private ToyState _state;

        public Item_Toys(ToyState state = null)
        {
            _state = state ?? new ToyState();
        }

        public string ItemId => _state.itemId;

        /// <summary>
        /// Trades toys to a faction. Returns the total trade value.
        /// Factions with children pay 5x. Normal factions pay 1x. Cults destroy toys as "Old World Idols" (0 value).
        /// </summary>
        public float TradeToys(string traderId, string factionId, int toyCount)
        {
            if (string.IsNullOrEmpty(traderId) || string.IsNullOrEmpty(factionId))
            {
                Debug.LogWarning("[Item_Toys] TradeToys called with null/empty id.");
                return 0f;
            }

            if (toyCount <= 0)
            {
                return 0f;
            }

            float multiplier = GetFactionMultiplier(factionId);

            // Cult factions destroy the toys
            if (multiplier <= 0f)
            {
                OnToyDestroyed?.Invoke(traderId, factionId);
                return 0f;
            }

            float totalValue = _state.baseTradeValue * toyCount * multiplier;
            OnToyTraded?.Invoke(traderId, factionId, totalValue);

            return totalValue;
        }

        /// <summary>
        /// Returns the faction-specific trade multiplier for toys.
        /// 5.0f if faction has children, 1.0f if not, 0f if cult (destroys toys).
        /// </summary>
        public float GetFactionMultiplier(string factionId)
        {
            if (string.IsNullOrEmpty(factionId))
            {
                return 1.0f;
            }

            // Convention: faction IDs containing "_cult" are cult factions
            if (factionId.Contains("_cult"))
            {
                return _state.cultDestroys ? 0f : 1.0f;
            }

            // Convention: faction IDs containing "_family" or "_children" have children
            if (factionId.Contains("_family") || factionId.Contains("_children"))
            {
                return _state.factionMultiplier;
            }

            return 1.0f;
        }

        public ToyState CaptureState()
        {
            return new ToyState
            {
                itemId = _state.itemId,
                baseTradeValue = _state.baseTradeValue,
                factionMultiplier = _state.factionMultiplier,
                cultDestroys = _state.cultDestroys
            };
        }

        public void RestoreState(ToyState state)
        {
            _state = state ?? new ToyState();
        }
    }
}
