using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ConscriptsState
    {
        public string id = "npc_conscripts";
        public string displayName = "The Conscripts";
        public int totalCount = 4;
        public int aliveCount = 4;
        public bool isSurrendered;
        public float fleeChance = 0.65f;
        public int riflesDroppedCount;
        public float playerMoralePenaltyOnExecution = 30f;
    }

    /// <summary>
    /// Prompt #323: NPC Encounter: The Conscripts.
    /// Drafted teenagers. Killing one causes the rest to surrender and drop StandardIssueRifles;
    /// executing surrendered conscripts imposes a heavy morale penalty.
    /// </summary>
    public class NPC_Conscripts
    {
        private ConscriptsState _state = new ConscriptsState();

        public event Action<ConscriptsState> OnConscriptKilled;
        public event Action<ConscriptsState> OnGroupSurrendered;
        public event Action<ConscriptsState, float> OnSurrenderedConscriptExecuted;

        public ConscriptsState State => _state;

        public void KillOneConscript()
        {
            if (_state.aliveCount <= 0) return;
            _state.aliveCount--;
            OnConscriptKilled?.Invoke(_state);

            if (_state.aliveCount > 0 && !_state.isSurrendered)
            {
                // Remaining conscripts surrender immediately upon seeing a comrade die
                _state.isSurrendered = true;
                _state.riflesDroppedCount = _state.aliveCount;
                OnGroupSurrendered?.Invoke(_state);
            }
        }

        public float ExecuteSurrenderedConscript()
        {
            if (!_state.isSurrendered || _state.aliveCount <= 0) return 0f;
            _state.aliveCount--;
            OnSurrenderedConscriptExecuted?.Invoke(_state, _state.playerMoralePenaltyOnExecution);
            return _state.playerMoralePenaltyOnExecution;
        }

        public bool TryFlee(System.Random rng)
        {
            if (_state.isSurrendered || _state.aliveCount <= 0) return false;
            if (rng.NextDouble() < _state.fleeChance)
            {
                _state.aliveCount = 0;
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ConscriptsState CaptureState() => _state;

        public void RestoreState(ConscriptsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
