// Node_Sector7G.cs — Dev Room Easter Egg (Prompt #866)
// Hidden node requiring complex code to enter. Skeletal remains wearing
// developer name tags. Pristine Coffee and Pizza. Easter egg.
using System;
using System.Collections.Generic;

namespace AtomicWar._Game.World
{
    /// <summary>
    /// Serializable state for the Sector 7G dev room (Prompt #866).
    /// One-time visit per playthrough.
    /// </summary>
    [Serializable]
    public class Sector7GState
    {
        public string node_id = "node_sector_7g";
        public bool is_discovered;
        public string access_code = "7G-OMEGA-ASHFALL-42";
        public string code_entered = string.Empty;
        public bool is_unlocked;
        public List<string> dev_names = new List<string>();
        public bool loot_available;
    }

    /// <summary>
    /// Sector 7G dev room (Prompt #866).
    /// Access code = "7G-OMEGA-ASHFALL-42".
    /// Room contains developer skeleton names.
    /// Loot: 10x Coffee (energy +30), 5x Pizza (hunger +50).
    /// One-time visit per playthrough.
    /// </summary>
    public class Node_Sector7G
    {
        // ── Events ─────────────────────────────────────────────────────
        public event Action OnDiscovered;
        public event Action<string, bool> OnCodeAttempted;
        public event Action OnRoomUnlocked;
        public event Action<string[]> OnDevRemainsFound;
        public event Action<string> OnLootCollected;
        public event Action OnEasterEggTriggered;

        // ── Constants ──────────────────────────────────────────────────
        private const string AccessCode = "7G-OMEGA-ASHFALL-42";
        private const int CoffeeCount = 10;
        private const int PizzaCount = 5;
        private const int CoffeeEnergyBonus = 30;
        private const int PizzaHungerBonus = 50;

        // ── State ──────────────────────────────────────────────────────
        private Sector7GState _state;
        private bool _visited;

        public Node_Sector7G()
        {
            _state = new Sector7GState();
            // Default developer skeleton names (Prompt #866)
            _state.dev_names = new List<string>
            {
                "Lead Architect",
                "Systems Designer",
                "Narrative Writer",
                "Pixel Artist",
                "Sound Engineer"
            };
        }

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Discover the hidden Sector 7G node on the map.
        /// </summary>
        public void Discover()
        {
            if (_state.is_discovered)
                return;

            _state.is_discovered = true;
            OnDiscovered?.Invoke();
        }

        /// <summary>
        /// Attempt to enter the access code. Returns true if correct.
        /// </summary>
        public bool AttemptCode(string inputCode)
        {
            _state.code_entered = inputCode;
            bool correct = (inputCode == AccessCode);

            OnCodeAttempted?.Invoke(inputCode, correct);

            if (correct && !_state.is_unlocked)
            {
                _state.is_unlocked = true;
                _state.loot_available = true;
                OnRoomUnlocked?.Invoke();
                OnDevRemainsFound?.Invoke(_state.dev_names.ToArray());
                OnEasterEggTriggered?.Invoke();
            }

            return correct;
        }

        /// <summary>
        /// Returns true when the room has been unlocked.
        /// </summary>
        public bool IsUnlocked()
        {
            return _state.is_unlocked;
        }

        /// <summary>
        /// Returns the developer skeleton names found in the room.
        /// </summary>
        public IReadOnlyList<string> GetDevNames()
        {
            return _state.dev_names.AsReadOnly();
        }

        /// <summary>
        /// Collect a loot item by id. One-time visit — loot can only
        /// be collected once per playthrough.
        /// Valid item ids: "coffee" (10x, energy +30), "pizza" (5x, hunger +50).
        /// </summary>
        public void CollectLoot(string itemId)
        {
            if (!_state.loot_available || _visited)
                return;

            if (itemId == "coffee" || itemId == "pizza")
            {
                _state.loot_available = false;
                _visited = true;
                OnLootCollected?.Invoke(itemId);
            }
        }

        /// <summary>
        /// Returns available loot descriptions if the room is unlocked
        /// and not yet visited.
        /// </summary>
        public string[] GetAvailableLoot()
        {
            if (!_state.is_unlocked || _visited || !_state.loot_available)
                return Array.Empty<string>();

            return new string[]
            {
                $"Coffee x{CoffeeCount} (energy +{CoffeeEnergyBonus} each)",
                $"Pizza x{PizzaCount} (hunger +{PizzaHungerBonus} each)"
            };
        }

        // ── Save / Load ────────────────────────────────────────────────

        public Sector7GState CaptureState()
        {
            return _state;
        }

        public void RestoreState(Sector7GState state)
        {
            _state = state ?? new Sector7GState();
        }
    }
}
