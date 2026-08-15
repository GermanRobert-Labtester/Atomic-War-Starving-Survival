using System;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_LamplightersState
    {
        public string id = "faction_lamplighters";
        public string displayName = "The Lamplighters";
        public bool isActive;
        /// <summary>Access is granted, not trust-tracked — a Current has no standing to lose.</summary>
        public bool accessGranted = true;
        /// <summary>Times the player has asked for the dark night. Two asks end it.</summary>
        public int darkNightRequests;
        /// <summary>After withdrawal, the lamps in the player's region go out one at a time.</summary>
        public int lampsOutCountdown;
        public string badgeAssetId = "faction_leader_1";
    }

    /// <summary>
    /// Lore bible 05_FACTIONS §2 — The Lamplighters (peaceful Current).
    /// They maintain the route beacons and do not ask who is walking.
    /// The rule does not have an exception and they did not invent one:
    /// a beacon is lit for whoever is walking.
    /// </summary>
    public class NPC_Lamplighters
    {
        private NPC_LamplightersState _state = new NPC_LamplightersState();

        /// <summary>Raised once when access is withdrawn permanently (the second ask).</summary>
        public event Action<NPC_LamplightersState> OnAccessWithdrawn;
        /// <summary>Raised when the last lamp in the player's region goes out.</summary>
        public event Action<NPC_LamplightersState> OnLampsOut;

        public NPC_LamplightersState State => _state;

        /// <summary>GameBootstrap bridge: applies the currents.json entry at construction.</summary>
        public void Initialise(string displayName, string badgeAssetId)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            if (!string.IsNullOrEmpty(badgeAssetId)) _state.badgeAssetId = badgeAssetId;
            _state.isActive = true;
        }

        /// <summary>
        /// The player may offer an exception — fuel on condition the lamps go dark
        /// for a night. The refusal is not negotiable and never becomes negotiable.
        /// Pressed twice, access is withdrawn permanently.
        /// </summary>
        public bool RequestDarkNight()
        {
            _state.darkNightRequests++;
            if (_state.darkNightRequests >= 2 && _state.accessGranted)
            {
                _state.accessGranted = false;
                _state.lampsOutCountdown = 11;
                OnAccessWithdrawn?.Invoke(_state);
            }
            return false; // The answer was never yes.
        }

        /// <summary>Daily tick: the lamps go out one at a time over eleven days.</summary>
        public void Tick(float gameHours = 0f)
        {
            if (_state.accessGranted || _state.lampsOutCountdown <= 0) return;
            _state.lampsOutCountdown--;
            if (_state.lampsOutCountdown == 0)
                OnLampsOut?.Invoke(_state);
        }

        public NPC_LamplightersState CaptureState() => _state;
        public void RestoreState(NPC_LamplightersState saved) { _state = saved ?? new NPC_LamplightersState(); }
    }
}
