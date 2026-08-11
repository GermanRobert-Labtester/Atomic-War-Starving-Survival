using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class SabotageMissionState
    {
        public string actionId = "action_sabotage";
        public string targetFactionId;
        public bool isMissionSuccessful = false;
        public bool isAgentCaughtAndKilled = false;
        public int globalRaidLevelOverride = 1; // Reduces raids to Level 1
    }

    /// <summary>
    /// Prompt #413: System: Sabotage Missions.
    /// Send a high-stealth survivor to a Faction Base node to poison water or blow the armory.
    /// Weakens the faction globally (lowers Raid levels to 1). If caught, the agent is killed.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_Sabotage
    {
        private SabotageMissionState _state = new SabotageMissionState();

        public event Action<SabotageMissionState, string> OnFactionWeakenedBySabotage;
        public event Action<SabotageMissionState, string> OnSaboteurCaughtAndKilled;

        public SabotageMissionState State => _state;

        public bool ExecuteSabotageMission(string factionId, string agentSurvivorId, int agentStealthSkill, System.Random rng)
        {
            _state.targetFactionId = factionId;
            bool success = agentStealthSkill >= 12 && rng.NextDouble() > 0.30;

            if (success)
            {
                _state.isMissionSuccessful = true;
                _state.isAgentCaughtAndKilled = false;
                OnFactionWeakenedBySabotage?.Invoke(_state, factionId);
                return true;
            }

            _state.isMissionSuccessful = false;
            _state.isAgentCaughtAndKilled = true;
            OnSaboteurCaughtAndKilled?.Invoke(_state, agentSurvivorId);
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SabotageMissionState CaptureState() => _state;

        public void RestoreState(SabotageMissionState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
