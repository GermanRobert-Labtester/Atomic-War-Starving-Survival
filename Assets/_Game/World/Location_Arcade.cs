using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class ArcadeState
    {
        public string locationId = "location_arcade";
        public string displayName = "The Arcade";
        public int tokensAvailable = 0;
        public bool childScavengersPresent = true;
        public string acceptedCurrency = "tokens";
        public float tradeValuePerToken = 50f;
    }

    /// <summary>
    /// Prompt #612: Location: The Arcade.
    /// Broken machines that can be searched for Tokens. Child-scavengers use Tokens
    /// as their only accepted currency, refusing PreWarMoney or Meds for trade.
    /// </summary>
    /// <summary>DEMOTE-Location-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Location_Arcade
    {
        private ArcadeState _state = new ArcadeState();

        private static readonly List<string> _childScavengerInventory = new List<string>
        {
            "clean_water",
            "iodine_pills",
            "gas_mask_filter",
            "scrap_metal"
        };

        public event Action<ArcadeState, int> OnTokensFound;
        public event Action<ArcadeState, string, int> OnTradeCompleted;
        public event Action<ArcadeState, string> OnTradeRejected;

        public ArcadeState State => _state;

        /// <summary>
        /// Searches broken arcade machines for tokens.
        /// </summary>
        /// <param name="rng">Random number generator.</param>
        /// <returns>Number of tokens found (0–5).</returns>
        public int SearchForTokens(System.Random rng)
        {
            if (rng == null)
                return 0;

            int tokensFound = rng.Next(0, 6); // 0 to 5 inclusive
            _state.tokensAvailable += tokensFound;

            OnTokensFound?.Invoke(_state, tokensFound);
            return tokensFound;
        }

        /// <summary>
        /// Attempts to trade with child-scavengers. Only tokens are accepted.
        /// </summary>
        /// <param name="itemType">The type of item/currency offered for trade.</param>
        /// <param name="tokenCount">Number of tokens available to spend.</param>
        /// <returns>True if the trade was accepted and completed.</returns>
        public bool TryTrade(string itemType, int tokenCount)
        {
            if (string.IsNullOrEmpty(itemType) || !_state.childScavengersPresent)
            {
                OnTradeRejected?.Invoke(_state, itemType ?? string.Empty);
                return false;
            }

            // Only tokens are accepted as currency
            if (itemType != _state.acceptedCurrency)
            {
                OnTradeRejected?.Invoke(_state, itemType);
                return false;
            }

            int requiredTokens = Mathf.CeilToInt(1f); // Minimum 1 token per trade
            if (tokenCount < requiredTokens)
            {
                OnTradeRejected?.Invoke(_state, itemType);
                return false;
            }

            OnTradeCompleted?.Invoke(_state, itemType, requiredTokens);
            return true;
        }

        /// <summary>
        /// Returns the child-scavengers' current tradeable inventory.
        /// </summary>
        public IReadOnlyList<string> GetChildScavengerInventory()
        {
            return _childScavengerInventory.AsReadOnly();
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ArcadeState CaptureState() => _state;

        public void RestoreState(ArcadeState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
