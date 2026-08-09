using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Utilities;

namespace AtomicWar._Game.Core
{
    public enum KeycardColor
    {
        Red,
        Blue,
        Green
    }

    [Serializable]
    public class KeycardState
    {
        public string item_id_prefix = "item_keycard_";
        public List<string> found_card_ids = new List<string>();
        public List<string> unlocked_door_ids = new List<string>();
    }

    /// <summary>
    /// REPROMOTE-Item-001 — Boot/Save live. Expedition looting on keycard_door-tagged
    /// nodes finds cards and attempts TryOpenDoor (secure hangar / command doors).
    /// </summary>
    public sealed class Item_Keycards
    {
        private KeycardState _state;

        public event Action<string, KeycardColor> OnKeycardFound;   // (survivor_id, color)
        public event Action<string, KeycardColor> OnDoorUnlocked;    // (survivor_id, color)

        public string EventId => _state.item_id_prefix;
        public IReadOnlyList<string> FoundCardIds => _state.found_card_ids;
        public IReadOnlyList<string> UnlockedDoorIds => _state.unlocked_door_ids;

        public Item_Keycards()
        {
            _state = new KeycardState();
        }

        /// <summary>True if this door id was already unlocked via <see cref="TryOpenDoor"/>.</summary>
        public bool IsDoorUnlocked(string doorId)
        {
            if (string.IsNullOrEmpty(doorId) || _state.unlocked_door_ids == null) return false;
            return _state.unlocked_door_ids.Contains(doorId);
        }

        /// <summary>
        /// Returns the canonical item id for a given keycard color.
        /// </summary>
        public string GetKeycardId(KeycardColor color)
        {
            switch (color)
            {
                case KeycardColor.Red:   return "item_keycard_red";
                case KeycardColor.Blue:  return "item_keycard_blue";
                case KeycardColor.Green: return "item_keycard_green";
                default:
                    Debug.LogError($"[Item_Keycards] Unknown keycard color: {color}");
                    return "item_keycard_unknown";
            }
        }

        /// <summary>
        /// Reports that a survivor found a keycard.
        /// </summary>
        public void ReportKeycardFound(string survivor_id, KeycardColor color)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Item_Keycards] survivor_id is null or empty.");
                return;
            }

            string card_id = GetKeycardId(color);
            if (!_state.found_card_ids.Contains(card_id))
            {
                _state.found_card_ids.Add(card_id);
            }

            OnKeycardFound?.Invoke(survivor_id, color);
            GameLog.Log($"[Item_Keycards] Survivor '{survivor_id}' found {card_id}.");
        }

        /// <summary>
        /// Checks whether the player's collected keycards can open a door
        /// that requires the specified color.
        /// </summary>
        public bool CanOpenDoor(KeycardColor required_color, List<KeycardColor> player_cards)
        {
            if (player_cards == null)
                return false;

            for (int i = 0; i < player_cards.Count; i++)
            {
                if (player_cards[i] == required_color)
                    return true;
            }

            return false;
        }

        /// <summary>
        /// Attempts to open a door with the required keycard color.
        /// Returns true if the survivor has the matching card.
        /// </summary>
        public bool TryOpenDoor(string survivor_id, KeycardColor required, List<KeycardColor> owned)
        {
            if (string.IsNullOrEmpty(survivor_id))
            {
                Debug.LogError("[Item_Keycards] survivor_id is null or empty.");
                return false;
            }

            if (!CanOpenDoor(required, owned))
            {
                GameLog.Log($"[Item_Keycards] Survivor '{survivor_id}' cannot open door " +
                          $"(requires {required}, not in owned cards).");
                return false;
            }

            string door_id = $"{_state.item_id_prefix}{required.ToString().ToLowerInvariant()}_door";
            if (!_state.unlocked_door_ids.Contains(door_id))
            {
                _state.unlocked_door_ids.Add(door_id);
            }

            OnDoorUnlocked?.Invoke(survivor_id, required);
            GameLog.Log($"[Item_Keycards] Survivor '{survivor_id}' unlocked {required} door.");
            return true;
        }

        public KeycardState CaptureState()
        {
            return new KeycardState
            {
                item_id_prefix = _state.item_id_prefix,
                found_card_ids = new List<string>(_state.found_card_ids),
                unlocked_door_ids = new List<string>(_state.unlocked_door_ids)
            };
        }

        public void RestoreState(KeycardState saved)
        {
            _state = saved ?? new KeycardState();
        }
    }
}
