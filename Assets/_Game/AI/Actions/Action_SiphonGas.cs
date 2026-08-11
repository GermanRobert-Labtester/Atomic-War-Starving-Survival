using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class SiphonGasState
    {
        public string actionId = "action_siphon_gas";
        public float hoursRequired = 2.0f;
        public float poisoningChance = 0.30f;
        public int fuelYieldUnits = 10;
        public string poisoningAffliction = "gasoline_poisoning";
    }

    /// <summary>
    /// Prompt #385: System: Siphoning Wrecks.
    /// Siphons gasoline from old car wrecks. Requires a Hose, takes 2 hours,
    /// with a high risk of swallowing gas (inflicting Gasoline Poisoning).
    /// </summary>
    /// <summary>DEMOTE-Action-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_SiphonGas
    {
        private SiphonGasState _state = new SiphonGasState();

        public event Action<SiphonGasState, int> OnGasSiphoned;
        public event Action<SiphonGasState, string> OnGasolineSwallowedPoisoned;

        public SiphonGasState State => _state;

        public int ExecuteSiphon(bool hasHose, System.Random rng, out string poisonAffliction)
        {
            poisonAffliction = null;
            if (!hasHose) return 0;

            int yield = _state.fuelYieldUnits;
            OnGasSiphoned?.Invoke(_state, yield);

            if (rng.NextDouble() < _state.poisoningChance)
            {
                poisonAffliction = _state.poisoningAffliction;
                OnGasolineSwallowedPoisoned?.Invoke(_state, poisonAffliction);
            }

            return yield;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SiphonGasState CaptureState() => _state;

        public void RestoreState(SiphonGasState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
