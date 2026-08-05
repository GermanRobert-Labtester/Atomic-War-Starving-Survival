using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SevereFrostbiteState
    {
        public string survivorId;
        public float hoursAtZeroWarmth = 0f;
        public bool hasLostDigits = false;
        public float permanentCraftingSpeedPenalty = 0.10f; // 10% penalty
        public float permanentAgilityPenalty = 0.10f;        // 10% penalty
    }

    /// <summary>
    /// Prompt #394: System: Severe Frostbite.
    /// Staying at 0 Warmth for 4 hours causes severe frostbite, leading to loss of fingers/toes.
    /// Permanently reduces Crafting Speed and Agility by 10% (irreversible).
    /// </summary>
    public class SevereFrostbiteSystem
    {
        private readonly Dictionary<string, SevereFrostbiteState> _frostbiteMap = new Dictionary<string, SevereFrostbiteState>();

        public event Action<string> OnPermanentDigitLossOccurred;

        public IReadOnlyDictionary<string, SevereFrostbiteState> FrostbiteMap => _frostbiteMap;

        public void TickWarmthExposure(string survivorId, float currentWarmth, float deltaHours)
        {
            if (string.IsNullOrEmpty(survivorId)) return;

            if (!_frostbiteMap.TryGetValue(survivorId, out var state))
            {
                state = new SevereFrostbiteState { survivorId = survivorId };
                _frostbiteMap[survivorId] = state;
            }

            if (currentWarmth <= 0f && !state.hasLostDigits)
            {
                state.hoursAtZeroWarmth += deltaHours;
                if (state.hoursAtZeroWarmth >= 4.0f)
                {
                    state.hasLostDigits = true;
                    OnPermanentDigitLossOccurred?.Invoke(survivorId);
                }
            }
            else if (currentWarmth > 0f)
            {
                state.hoursAtZeroWarmth = 0f;
            }
        }
    }
}
