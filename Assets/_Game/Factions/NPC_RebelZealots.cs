using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class RebelZealotsState
    {
        public string id = "npc_rebel_zealots";
        public string displayName = "Rebel Zealots";
        public bool executeOnSight = false;
        public int daysSinceMilitaryTrade = -1;
        public int maxDaysTradeMemory = 10;
    }

    /// <summary>
    /// Prompt #330: Faction: Rebel Zealots (Sub-Faction).
    /// Sub-faction of rebels who execute anyone suspected of collaborating.
    /// If player traded with the Military in the last 10 days, Zealots execute on sight.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_RebelZealots
    {
        private RebelZealotsState _state = new RebelZealotsState();

        public event Action<RebelZealotsState, bool> OnCollaboratorCheckEvaluated;

        public RebelZealotsState State => _state;

        public bool CheckTradeHistory(int daysSinceLastMilitaryTrade)
        {
            _state.daysSinceMilitaryTrade = daysSinceLastMilitaryTrade;
            bool isCollaborator = daysSinceLastMilitaryTrade >= 0 && daysSinceLastMilitaryTrade <= _state.maxDaysTradeMemory;
            _state.executeOnSight = isCollaborator;

            OnCollaboratorCheckEvaluated?.Invoke(_state, isCollaborator);
            return isCollaborator;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public RebelZealotsState CaptureState() => _state;

        public void RestoreState(RebelZealotsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
