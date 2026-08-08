using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class BlackOpsState
    {
        public string id = "npc_black_ops";
        public string displayName = "Black Ops (Ex-Military Rebels)";
        public bool isHostileToEveryone = true;
        public int perceptionCheckThreshold = 14;
        public float boobyTrapBleedDamage = 25f;
    }

    /// <summary>
    /// Prompt #325: NPC Encounter: Black Ops (Ex-Military Rebels).
    /// Rogue spec-ops who use booby traps. Player must pass a Perception check upon entering
    /// or start the encounter Bleeding from a trap. Hostile to all factions.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_BlackOps
    {
        private BlackOpsState _state = new BlackOpsState();

        public event Action<BlackOpsState, bool> OnAmbushTriggered; // state, perceptionPassed
        public event Action<BlackOpsState, float> OnPlayerBleedingFromTrap;

        public BlackOpsState State => _state;

        public bool CheckPreEncounterAmbush(int playerPerceptionSkill)
        {
            bool passed = playerPerceptionSkill >= _state.perceptionCheckThreshold;
            OnAmbushTriggered?.Invoke(_state, passed);

            if (!passed)
            {
                OnPlayerBleedingFromTrap?.Invoke(_state, _state.boobyTrapBleedDamage);
            }

            return passed;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public BlackOpsState CaptureState() => _state;

        public void RestoreState(BlackOpsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
