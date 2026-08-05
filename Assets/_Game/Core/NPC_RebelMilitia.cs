using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RebelMilitiaState
    {
        public string id = "npc_rebel_militia";
        public string displayName = "Rebel Militias";
        public bool isHostile = false;
        public float requiredKarmaThreshold = 20f;
        public List<string> weaponsEquipped = new List<string> { "pipe_weapon", "molotov_cocktail" };
        public bool caughtStealing = false;
    }

    /// <summary>
    /// Prompt #329: NPC Encounter: Rebel Militias.
    /// Neighborhood watch gone extreme. Friendly if player has high Karma/Reputation (>= 20);
    /// turns hostile if player is caught stealing. Fights with PipeWeapons and Molotovs.
    /// </summary>
    public class NPC_RebelMilitia
    {
        private RebelMilitiaState _state = new RebelMilitiaState();

        public event Action<RebelMilitiaState, bool> OnStanceDetermined;
        public event Action<RebelMilitiaState> OnCaughtStealingHostile;

        public RebelMilitiaState State => _state;

        public bool EvaluateStance(float playerKarma)
        {
            if (_state.caughtStealing)
            {
                _state.isHostile = true;
            }
            else
            {
                _state.isHostile = playerKarma < _state.requiredKarmaThreshold;
            }

            OnStanceDetermined?.Invoke(_state, _state.isHostile);
            return _state.isHostile;
        }

        public void NotifyCaughtStealing()
        {
            _state.caughtStealing = true;
            _state.isHostile = true;
            OnCaughtStealingHostile?.Invoke(_state);
        }
    }
}
