// Action_BuryTimeCapsule.cs — Time Capsule Action (Prompt #860)
// Before bunker falls, place ONE item in titanium box.
// Next playthrough, find it on Day 1. Pass endgame items to new timeline.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializable state for the Time Capsule action (Prompt #860).
    /// Persists across saves in a separate file so the buried item
    /// survives playthrough wipes.
    /// </summary>
    [Serializable]
    public class TimeCapsuleState
    {
        public string action_id = "action_bury_time_capsule";
        public string item_id = string.Empty;
        public string item_data = string.Empty;
        public bool is_buried;
        public string capsule_location = string.Empty;
        public bool retrieved_in_new_game;
    }

    /// <summary>
    /// Time Capsule action (Prompt #860).
    /// Only 1 item per playthrough. Must be done when bunker is falling.
    /// Item persists across saves in a separate file.
    /// Retrieved on Day 1 of next game.
    /// </summary>
    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_BuryTimeCapsule
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action<string, string> OnCapsuleBuried;
        public event Action<string, string> OnCapsuleRetrieved;
        public event Action OnCapsuleLost;

        // ── State ──────────────────────────────────────────────────────
        private TimeCapsuleState _state = new TimeCapsuleState();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Bury a single item in the titanium capsule.
        /// Only allowed when the bunker is falling and no item is already buried.
        /// </summary>
        public void BuryItem(string itemId, string itemData, string location)
        {
            if (_state.is_buried)
                return; // Only 1 item per playthrough

            _state.item_id = itemId;
            _state.item_data = itemData;
            _state.capsule_location = location;
            _state.is_buried = true;

            OnCapsuleBuried?.Invoke(itemId, location);
        }

        /// <summary>
        /// Returns true when the capsule can be buried (bunker is falling
        /// and nothing is buried yet).
        /// </summary>
        public bool CanBury(bool bunkerFalling)
        {
            return bunkerFalling && !_state.is_buried;
        }

        /// <summary>
        /// Retrieve the buried capsule on Day 1 of a new playthrough.
        /// </summary>
        public void RetrieveCapsule(string newGameId)
        {
            if (!_state.is_buried || _state.retrieved_in_new_game)
            {
                OnCapsuleLost?.Invoke();
                return;
            }

            _state.retrieved_in_new_game = true;
            OnCapsuleRetrieved?.Invoke(_state.item_id, newGameId);
        }

        /// <summary>
        /// Returns the buried item data, or null if nothing is buried.
        /// </summary>
        public string GetBuriedItem()
        {
            return _state.is_buried ? _state.item_data : null;
        }

        // ── Save / Load ────────────────────────────────────────────────

        public TimeCapsuleState CaptureState()
        {
            return _state;
        }

        public void RestoreState(TimeCapsuleState state)
        {
            _state = state ?? new TimeCapsuleState();
        }
    }
}
