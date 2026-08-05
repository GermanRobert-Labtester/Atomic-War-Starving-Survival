using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HomelessEncampmentState
    {
        public string id = "npc_homeless";
        public string displayName = "The Homeless Encampment";
        public bool isPassive = true;
        public float diseaseRiskChance = 0.70f;
        public string phase1Affliction = "cholera_phase_1";
        public string intelNodeSold = "subway_tunnel_schematics";
    }

    /// <summary>
    /// Prompt #348: NPC Encounter: The Homeless Encampment.
    /// Found in Subway nodes. Passive, high disease vector. Interacting without a HazmatSuit
    /// contracts Phase 1 Afflictions, but enables trading for rare IntelNodes.
    /// </summary>
    public class NPC_Homeless
    {
        private HomelessEncampmentState _state = new HomelessEncampmentState();

        public event Action<HomelessEncampmentState, string> OnDiseaseContracted;
        public event Action<HomelessEncampmentState, string> OnIntelNodeTraded;

        public HomelessEncampmentState State => _state;

        public string InteractAndTrade(bool hasHazmatSuit, System.Random rng)
        {
            if (!hasHazmatSuit && rng.NextDouble() < _state.diseaseRiskChance)
            {
                OnDiseaseContracted?.Invoke(_state, _state.phase1Affliction);
            }

            OnIntelNodeTraded?.Invoke(_state, _state.intelNodeSold);
            return _state.intelNodeSold;
        }
    }
}
