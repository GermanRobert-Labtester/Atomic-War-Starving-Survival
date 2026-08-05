using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SlaversState
    {
        public string id = "npc_slavers";
        public string displayName = "The Slavers";
        public bool usesTearGas = true;
        public bool usesStunBatons = true;
        public bool isPlayerEnslaved = false;
        public string destinationNodeId = "labor_camp";
    }

    /// <summary>
    /// Prompt #335: NPC Encounter: The Slavers (Terrorist Sub-Group).
    /// Uses TearGas and StunBatons. Non-lethal capture: losing this encounter transports the scavenger
    /// to the Labor Camp node, stripped of gear, where they must escape via stealth.
    /// </summary>
    public class NPC_Slavers
    {
        private SlaversState _state = new SlaversState();

        public event Action<SlaversState, string> OnPlayerEnslaved;

        public SlaversState State => _state;

        public void HandlePlayerDefeat()
        {
            _state.isPlayerEnslaved = true;
            OnPlayerEnslaved?.Invoke(_state, _state.destinationNodeId);
        }
    }
}
