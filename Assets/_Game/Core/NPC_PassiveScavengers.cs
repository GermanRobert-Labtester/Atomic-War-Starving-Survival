using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class PassiveScavengersState
    {
        public string id = "npc_passive_scavengers";
        public string displayName = "Passive Scavengers";
        public bool isFled = false;
        public float hourlyLootDepletionRate = 0.20f; // 20% per hour
        public float totalLootDepleted = 0f;
    }

    /// <summary>
    /// Prompt #358: NPC Encounter: Passive Scavengers.
    /// Competitors scavenging the same node. Runs away if player points a gun at them.
    /// If left alone, depletes the node's LootTable by 20% per hour.
    /// </summary>
    /// <summary>DEMOTE-NPC-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class NPC_PassiveScavengers
    {
        private PassiveScavengersState _state = new PassiveScavengersState();

        public event Action<PassiveScavengersState> OnFledFromGunpoint;
        public event Action<PassiveScavengersState, float> OnLootDepleted;

        public PassiveScavengersState State => _state;

        public bool PointGunAtScavenger()
        {
            if (_state.isFled) return false;
            _state.isFled = true;
            OnFledFromGunpoint?.Invoke(_state);
            return true;
        }

        public float TickHourlyScavenging(float hoursElapsed)
        {
            if (_state.isFled) return 0f;
            float depletedAmount = hoursElapsed * _state.hourlyLootDepletionRate;
            _state.totalLootDepleted = Mathf.Clamp01(_state.totalLootDepleted + depletedAmount);

            OnLootDepleted?.Invoke(_state, _state.totalLootDepleted);
            return _state.totalLootDepleted;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public PassiveScavengersState CaptureState() => _state;

        public void RestoreState(PassiveScavengersState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
