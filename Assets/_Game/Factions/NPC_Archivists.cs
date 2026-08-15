using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class NPC_ArchivistsState
    {
        public string id = "faction_archivists";
        public string displayName = "The Archivists of the Before";
        public bool isActive = true;
        public float ancestralReputation = 50f;
        /// <summary>Lore bible 05_FACTIONS — reachable only by boat; the chart ends it.</summary>
        public bool isolated = true;
    }

    /// <summary>
    /// Expansion IV — Chapter 40: Factions of the Long Dark.
    /// The Archivists of the Before: Monastic order of Bunker-Born hoarding pre-war media as ancestral spirits.
    /// Offers morale buffs and item_encrypted_drive data, demanding tithes of destroyed pre-war photo albums and cassette tapes.
    /// </summary>
    public class NPC_Archivists
    {
        private NPC_ArchivistsState _state = new NPC_ArchivistsState();

        public event Action<NPC_ArchivistsState, float> OnTitheOffered;
        /// <summary>Raised once when the Kittiwake chart ends the Archivists' isolation.</summary>
        public event Action<NPC_ArchivistsState> OnIsolationEnded;

        public NPC_ArchivistsState State => _state;

        /// <summary>GameBootstrap bridge: applies the currents.json entry at construction.</summary>
        public void Initialise(string displayName)
        {
            if (!string.IsNullOrEmpty(displayName)) _state.displayName = displayName;
            _state.isActive = true;
        }

        /// <summary>
        /// Lore bible 05_FACTIONS interlocks — distributing the Kittiwake chart
        /// makes the Memory Vault reachable for everyone. The Archivists stop
        /// being isolated, and their safety ends at the same moment.
        /// </summary>
        public void EndIsolation()
        {
            if (!_state.isolated) return;
            _state.isolated = false;
            OnIsolationEnded?.Invoke(_state);
        }

        public bool SubmitAncestralTithe(string relicItemId, out float moraleBonus, out string rewardItemId)
        {
            moraleBonus = 0f;
            rewardItemId = null;

            if (relicItemId == "item_pre_war_photo_album")
            {
                moraleBonus = 25f;
                rewardItemId = "item_encrypted_drive";
                _state.ancestralReputation += 10f;
                OnTitheOffered?.Invoke(_state, moraleBonus);
                return true;
            }
            else if (relicItemId == "item_cassette_tape" || relicItemId == "item_vinyl_collection")
            {
                moraleBonus = 15f;
                rewardItemId = "item_encrypted_drive";
                _state.ancestralReputation += 5f;
                OnTitheOffered?.Invoke(_state, moraleBonus);
                return true;
            }

            return false;
        }

        public NPC_ArchivistsState CaptureState() => _state;
        public void RestoreState(NPC_ArchivistsState saved) { _state = saved ?? new NPC_ArchivistsState(); }
    }
}
