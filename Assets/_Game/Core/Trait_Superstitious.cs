using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SuperstitiousTraitState
    {
        public string traitId = "trait_superstitious";
        public string survivorId;
        public string luckyItemId;
        public bool hasItem;
        public bool statBoostActive;
        public bool statPenaltyActive;
        public bool panicSearching;
        public float panicHoursRemaining;
    }

    /// <summary>
    /// Prompt #847: Superstitions — Survivor attaches sanity to a junk item
    /// (LuckyCoin). Has item = +10% all stats. Loses item = -50% stats until
    /// found. Survivor panic-searches for lost item.
    /// </summary>
    public class Trait_Superstitious
    {
        private readonly Dictionary<string, SuperstitiousTraitState> _states =
            new Dictionary<string, SuperstitiousTraitState>();

        private const float StatBoostMultiplier = 1.1f;
        private const float StatPenaltyMultiplier = 0.5f;
        private const float PanicSearchDurationHours = 4f;

        public event Action<string, string> OnTraitAssigned;           // survivorId, itemId
        public event Action<string, string> OnItemLost;                // survivorId, itemId
        public event Action<string, float> OnStatPenaltyApplied;       // survivorId, multiplier
        public event Action<string, string> OnItemFound;               // survivorId, itemId
        public event Action<string> OnStatBoostRestored;               // survivorId

        public IReadOnlyDictionary<string, SuperstitiousTraitState> States => _states;

        /// <summary>
        /// Assigns the Superstitious trait to a survivor with their lucky item.
        /// </summary>
        public void AssignTrait(string survivorId, string luckyItemId)
        {
            var state = new SuperstitiousTraitState
            {
                survivorId = survivorId,
                luckyItemId = luckyItemId,
                hasItem = true,
                statBoostActive = true,
                statPenaltyActive = false,
                panicSearching = false,
                panicHoursRemaining = 0f
            };
            _states[survivorId] = state;

            OnTraitAssigned?.Invoke(survivorId, luckyItemId);
        }

        /// <summary>
        /// Checks whether the survivor currently holds their lucky item.
        /// Updates boost/penalty state accordingly.
        /// </summary>
        public void CheckInventory(string survivorId, bool hasLuckyItem)
        {
            if (!_states.TryGetValue(survivorId, out var state)) return;

            if (state.hasItem && !hasLuckyItem)
            {
                HandleItemLost(survivorId);
            }
            else if (!state.hasItem && hasLuckyItem)
            {
                HandleItemFound(survivorId);
            }
        }

        /// <summary>
        /// Called when the lucky item is lost. Applies stat penalty and starts panic search.
        /// </summary>
        public void HandleItemLost(string survivorId = null)
        {
            // If called via CheckInventory, find the right state
            if (survivorId != null && _states.TryGetValue(survivorId, out var specificState))
            {
                ApplyLoss(specificState);
                return;
            }

            // Parameterless version: update all states that lost their item
            foreach (var kvp in _states)
            {
                if (kvp.Value.hasItem == false && !kvp.Value.statPenaltyActive)
                {
                    ApplyLoss(kvp.Value);
                }
            }
        }

        private void ApplyLoss(SuperstitiousTraitState state)
        {
            state.hasItem = false;
            state.statBoostActive = false;
            state.statPenaltyActive = true;
            state.panicSearching = true;
            state.panicHoursRemaining = PanicSearchDurationHours;

            OnItemLost?.Invoke(state.survivorId, state.luckyItemId);
            OnStatPenaltyApplied?.Invoke(state.survivorId, StatPenaltyMultiplier);
        }

        /// <summary>
        /// Called when the lucky item is found/recovered. Restores stat boost.
        /// </summary>
        public void HandleItemFound(string survivorId = null)
        {
            if (survivorId != null && _states.TryGetValue(survivorId, out var specificState))
            {
                ApplyFound(specificState);
                return;
            }

            foreach (var kvp in _states)
            {
                if (!kvp.Value.hasItem && kvp.Value.statPenaltyActive)
                {
                    ApplyFound(kvp.Value);
                }
            }
        }

        private void ApplyFound(SuperstitiousTraitState state)
        {
            state.hasItem = true;
            state.statPenaltyActive = false;
            state.statBoostActive = true;
            state.panicSearching = false;
            state.panicHoursRemaining = 0f;

            OnItemFound?.Invoke(state.survivorId, state.luckyItemId);
            OnStatBoostRestored?.Invoke(state.survivorId);
        }

        /// <summary>
        /// Returns the stat multiplier for a superstitious survivor.
        /// 1.1x with item, 0.5x without, 1.0x if no trait.
        /// </summary>
        public float GetStatMultiplier(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state)) return 1.0f;

            if (state.statBoostActive) return StatBoostMultiplier;
            if (state.statPenaltyActive) return StatPenaltyMultiplier;
            return 1.0f;
        }

        /// <summary>
        /// Advances panic search timer. Returns true if survivor is still searching
        /// (cannot do other tasks).
        /// </summary>
        public bool TickHour(string survivorId)
        {
            if (!_states.TryGetValue(survivorId, out var state)) return false;
            if (!state.panicSearching) return false;

            state.panicHoursRemaining -= 1f;
            if (state.panicHoursRemaining <= 0f)
            {
                state.panicSearching = false;
                state.panicHoursRemaining = 0f;
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns true if the survivor is panic-searching and cannot do other tasks.
        /// </summary>
        public bool IsPanicSearching(string survivorId)
        {
            return _states.TryGetValue(survivorId, out var state) && state.panicSearching;
        }

        /// <summary>
        /// Captures all trait states for save.
        /// </summary>
        public Dictionary<string, SuperstitiousTraitState> CaptureState() =>
            new Dictionary<string, SuperstitiousTraitState>(_states);

        /// <summary>
        /// Restores trait states from save.
        /// </summary>
        public void RestoreState(Dictionary<string, SuperstitiousTraitState> states)
        {
            _states.Clear();
            foreach (var kvp in states)
                _states[kvp.Key] = kvp.Value;
        }
    }
}
