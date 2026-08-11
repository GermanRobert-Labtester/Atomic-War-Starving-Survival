using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.World
{
    [Serializable]
    public class StrandedYachtState
    {
        public string locationId = "location_stranded_yacht";
        public string displayName = "The Billionaire's Yacht";
        public int mercenaryCount = 6;
        public float mercenaryCombatPower = 70f;
        public List<string> luxuryItems = new List<string> { "caviar", "champagne", "gold_watch", "silk_scarf" };
        public float moraleBonusPerItem = 25f;
        public float nutritionValue = 0f;
        public bool isCleared = false;
    }

    /// <summary>
    /// Prompt #618: Location: The Stranded Yacht.
    /// A billionaire's yacht dropped in the middle of a ruined city. Contains luxury items
    /// (Caviar, Champagne) that boost morale but provide zero nutrition.
    /// Guarded by mercenaries.
    /// </summary>
    /// <summary>DEMOTE-Location-batch — dormant ghost. Re-promote with Boot+Save+host.</summary>
    public class Location_StrandedYacht
    {
        private StrandedYachtState _state = new StrandedYachtState();

        public event Action<StrandedYachtState> OnYachtDiscovered;
        public event Action<StrandedYachtState, bool> OnMercenaryCombat;
        public event Action<StrandedYachtState, List<string>> OnLuxuryLooted;
        public event Action<StrandedYachtState> OnInfiltrationSuccess;

        public StrandedYachtState State => _state;

        /// <summary>
        /// Notifies that the yacht has been discovered.
        /// </summary>
        public void DiscoverYacht()
        {
            OnYachtDiscovered?.Invoke(_state);
        }

        /// <summary>
        /// Attempts to infiltrate the yacht past the mercenaries using stealth.
        /// </summary>
        /// <param name="stealthSkill">The survivor's stealth skill.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True if infiltration succeeded.</returns>
        public bool TryInfiltrate(float stealthSkill, System.Random rng)
        {
            if (_state.isCleared || rng == null)
                return false;

            // Stealth must overcome mercenary awareness
            float successChance = Mathf.Clamp01(stealthSkill / (_state.mercenaryCombatPower + stealthSkill));

            if (rng.NextDouble() < successChance)
            {
                OnInfiltrationSuccess?.Invoke(_state);
                return true;
            }

            return false;
        }

        /// <summary>
        /// Engages the mercenaries in direct combat.
        /// </summary>
        /// <param name="combatPower">The player's total combat power.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>True if mercenaries are defeated and the yacht is cleared.</returns>
        public bool EngageMercenaries(float combatPower, System.Random rng)
        {
            if (_state.isCleared || rng == null)
                return false;

            float totalMercenaryPower = _state.mercenaryCount * _state.mercenaryCombatPower;
            float roll = (float)(rng.NextDouble() * totalMercenaryPower);

            bool victory = combatPower > roll;

            OnMercenaryCombat?.Invoke(_state, victory);

            if (victory)
            {
                _state.isCleared = true;
            }

            return victory;
        }

        /// <summary>
        /// Loots luxury items from the yacht. Items provide morale but zero nutrition.
        /// </summary>
        /// <param name="count">Number of luxury items to take.</param>
        /// <returns>List of looted luxury item names.</returns>
        public List<string> LootLuxury(int count)
        {
            if (count <= 0 || _state.luxuryItems.Count == 0)
                return new List<string>();

            int toTake = Math.Min(count, _state.luxuryItems.Count);
            List<string> looted = new List<string>();

            for (int i = 0; i < toTake; i++)
            {
                looted.Add(_state.luxuryItems[i]);
            }

            // Remove looted items from available pool
            _state.luxuryItems.RemoveRange(0, toTake);

            OnLuxuryLooted?.Invoke(_state, looted);
            return looted;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public StrandedYachtState CaptureState() => _state;

        public void RestoreState(StrandedYachtState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
