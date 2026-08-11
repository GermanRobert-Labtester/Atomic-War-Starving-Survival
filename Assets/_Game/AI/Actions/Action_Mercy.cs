using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class MercyActionState
    {
        public string actionId = "action_mercy";
        public string displayName = "Assisted Suicide (Mercy)";
        public int morphineCost = 1;
        public float somberClosureMoraleBuff = 15f;
    }

    /// <summary>
    /// Prompt #397: System: Assisted Suicide (Mercy).
    /// For survivors in incurable agony (Stage 4 Rads, Lockjaw, Rabies).
    /// Uses 1 Morphine; grants the shelter a SomberClosure morale buff instead of Despair.
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_Mercy
    {
        private MercyActionState _state = new MercyActionState();

        public event Action<MercyActionState, string, float> OnMercyExecutedWithClosure;

        public MercyActionState State => _state;

        public bool PerformMercyKill(string targetSurvivorId, bool isAgonyConditionMet, ref int morphineCount, out float moraleBuff)
        {
            moraleBuff = 0f;
            if (isAgonyConditionMet && morphineCount >= _state.morphineCost)
            {
                morphineCount -= _state.morphineCost;
                moraleBuff = _state.somberClosureMoraleBuff;

                OnMercyExecutedWithClosure?.Invoke(_state, targetSurvivorId, moraleBuff);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public MercyActionState CaptureState() => _state;

        public void RestoreState(MercyActionState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
