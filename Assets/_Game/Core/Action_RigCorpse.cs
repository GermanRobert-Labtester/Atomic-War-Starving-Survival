using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RigCorpseState
    {
        public string actionId = "action_rig_corpse";
        public bool requiresGrenade = true;
        public float karmaPenalty = -20f;
        public float trapDamage = 80f;
        public float passiveLootChance = 0.40f;
        public bool isTrapPlaced = false;
        public string riggedNodeId;
    }

    /// <summary>
    /// Prompt #600: Action: Rig Corpse.
    /// Place a grenade under a dead body. When Factions investigate the corpse,
    /// the trap kills them. Generates passive loot drops but costs Karma/Trust.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_RigCorpse
    {
        private RigCorpseState _state = new RigCorpseState();

        public event Action<RigCorpseState, string> OnCorpseRigged;
        public event Action<RigCorpseState, string> OnFactionKilledByTrap;
        public event Action<RigCorpseState, float> OnKarmaLost;
        public event Action<RigCorpseState> OnPassiveLootGenerated;

        public RigCorpseState State => _state;

        /// <summary>
        /// Places a grenade trap under a corpse at the given node.
        /// </summary>
        /// <param name="hasGrenade">Whether the player has a grenade available.</param>
        /// <param name="nodeId">The map node where the corpse is located.</param>
        /// <returns>True if the trap was successfully placed.</returns>
        public bool RigCorpse(bool hasGrenade, string nodeId)
        {
            if (!hasGrenade || string.IsNullOrEmpty(nodeId))
                return false;

            _state.isTrapPlaced = true;
            _state.riggedNodeId = nodeId;

            OnCorpseRigged?.Invoke(_state, nodeId);
            return true;
        }

        /// <summary>
        /// Checks whether an investigating faction triggers the trap.
        /// </summary>
        /// <param name="factionType">The type of faction investigating.</param>
        /// <param name="rng">Random number generator.</param>
        /// <returns>Tuple indicating whether the faction was killed and whether loot dropped.</returns>
        public (bool killed, bool lootDropped) CheckFactionTrigger(string factionType, System.Random rng)
        {
            if (!_state.isTrapPlaced || string.IsNullOrEmpty(factionType))
                return (false, false);

            bool killed = true; // Trap always kills on investigation
            bool lootDropped = rng.NextDouble() < _state.passiveLootChance;

            OnFactionKilledByTrap?.Invoke(_state, factionType);
            OnKarmaLost?.Invoke(_state, _state.karmaPenalty);

            if (lootDropped)
            {
                OnPassiveLootGenerated?.Invoke(_state);
            }

            _state.isTrapPlaced = false; // Trap is consumed
            return (killed, lootDropped);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RigCorpseState CaptureState() => _state;

        public void RestoreState(RigCorpseState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
