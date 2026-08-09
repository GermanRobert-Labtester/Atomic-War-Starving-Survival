using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class NegotiatorState
    {
        public string id = "npc_the_negotiator";
        public string displayName = "The Negotiator";
        public bool isCoveredByPlayerSniper = false;
        public bool isPeaceBrokered = false;
        public List<string> factionRewards = new List<string> { "rebel_intel_cache", "military_supply_crate" };
    }

    /// <summary>
    /// Prompt #366: NPC Encounter: The Negotiator.
    /// Spawns during Skirmish Events trying to broker peace in the crossfire.
    /// If player uses a Sniper to cover the Negotiator, both factions cease fire and player is rewarded by both sides.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_TheNegotiator
    {
        private NegotiatorState _state = new NegotiatorState();

        public event Action<NegotiatorState, List<string>> OnPeaceBrokered;

        public NegotiatorState State => _state;

        public List<string> ProvideSniperCover(bool playerHasSniperRifle, SkirmishState skirmishState)
        {
            if (playerHasSniperRifle && !_state.isPeaceBrokered)
            {
                _state.isCoveredByPlayerSniper = true;
                _state.isPeaceBrokered = true;

                if (skirmishState != null)
                {
                    skirmishState.isResolved = true;
                    skirmishState.winningFaction = "Peace Treaty (Both Factions)";
                }

                OnPeaceBrokered?.Invoke(_state, _state.factionRewards);
                return _state.factionRewards;
            }
            return new List<string>();
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public NegotiatorState CaptureState() => _state;

        public void RestoreState(NegotiatorState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
