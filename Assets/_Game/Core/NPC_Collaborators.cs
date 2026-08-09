using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class CollaboratorsState
    {
        public string id = "npc_collaborators";
        public string displayName = "The Collaborators";
        public bool isSaved = false;
        public bool isExecuted = false;
        public float moraleDropOnWatch = 15f;
        public string rewardItem = "collaborator_hidden_stash_map";
    }

    /// <summary>
    /// Prompt #333: NPC Encounter: The Collaborators.
    /// Civilians being lined up against a wall by Rebels. Player can Intervene (fight Rebels, save them for reward)
    /// or Watch (causes morale drop due to guilt, but safe).
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_Collaborators
    {
        private CollaboratorsState _state = new CollaboratorsState();

        public event Action<CollaboratorsState, string> OnCollaboratorsSaved;
        public event Action<CollaboratorsState, float> OnFiringSquadWatched;

        public CollaboratorsState State => _state;

        public string InterveneAndSave()
        {
            if (_state.isExecuted || _state.isSaved) return null;
            _state.isSaved = true;

            OnCollaboratorsSaved?.Invoke(_state, _state.rewardItem);
            return _state.rewardItem;
        }

        public float WatchExecution()
        {
            if (_state.isExecuted || _state.isSaved) return 0f;
            _state.isExecuted = true;

            OnFiringSquadWatched?.Invoke(_state, _state.moraleDropOnWatch);
            return _state.moraleDropOnWatch;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CollaboratorsState CaptureState() => _state;

        public void RestoreState(CollaboratorsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
