using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AutopsyTableState
    {
        public string moduleId = "shelter_module_autopsy";
        public string displayName = "Autopsy Table";
        public bool isBuilt = false;
        public float anatomyXPGain = 150f;
        public float roomEntryMoralePenalty = 15f;
    }

    /// <summary>
    /// Prompt #398: System: The Autopsy Table.
    /// Allows players to bring back dead Raiders for dissection.
    /// Yields AnatomyXP for the doctor and reveals hidden IntelNodes, but severely damages morale of room entrants.
    /// </summary>
    public class ShelterModule_Autopsy
    {
        private AutopsyTableState _state = new AutopsyTableState();

        public event Action<AutopsyTableState, string, float, string> OnAutopsyPerformed;
        public event Action<AutopsyTableState, string, float> OnRoomEnteredDisgustMoraleDropped;

        public AutopsyTableState State => _state;

        public bool PerformAutopsy(string doctorId, string corpseId, System.Random rng, out string revealedIntelNode)
        {
            revealedIntelNode = null;
            if (!_state.isBuilt) return false;

            if (rng.NextDouble() < 0.40)
            {
                revealedIntelNode = "swallowed_bunker_keycard_map";
            }

            OnAutopsyPerformed?.Invoke(_state, doctorId, _state.anatomyXPGain, revealedIntelNode);
            return true;
        }

        public void TriggerRoomEntryMoralePenalty(string entrantSurvivorId)
        {
            if (!_state.isBuilt) return;
            OnRoomEnteredDisgustMoraleDropped?.Invoke(_state, entrantSurvivorId, _state.roomEntryMoralePenalty);
        }
    }
}
