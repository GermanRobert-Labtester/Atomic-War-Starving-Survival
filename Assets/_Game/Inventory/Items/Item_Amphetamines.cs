using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class AmphetaminesState
    {
        public string itemId = "item_amphetamines";
        public string displayName = "Military Amphetamines";
        public float durationHoursRemaining = 48f;
        public float actionSpeedMultiplier = 1.50f; // +50% speed
        public bool isFatigueLockedAtZero = true;
        public float heartAttackRiskPerStormHour = 0.25f;
    }

    /// <summary>
    /// Prompt #433: Item: Military Amphetamines.
    /// Locks Fatigue at 0% and increases ActionSpeed by 50% for 48 hours.
    /// Highly addictive (#59); forcing work through storms risks a lethal HeartAttack.
    /// </summary>
    public class Item_Amphetamines
    {
        private AmphetaminesState _state = new AmphetaminesState();

        public event Action<AmphetaminesState, string> OnAmphetaminesConsumed;
        public event Action<AmphetaminesState, string> OnHeartAttackTriggered;

        public AmphetaminesState State => _state;

        public void ConsumeAmphetamines(string survivorId)
        {
            _state.durationHoursRemaining = 48f;
            _state.isFatigueLockedAtZero = true;

            OnAmphetaminesConsumed?.Invoke(_state, survivorId);
        }

        public bool CheckHeartAttackRisk(string survivorId, bool isWorkingInStorm, System.Random rng)
        {
            if (_state.durationHoursRemaining > 0f && isWorkingInStorm)
            {
                if (rng.NextDouble() < _state.heartAttackRiskPerStormHour)
                {
                    OnHeartAttackTriggered?.Invoke(_state, survivorId);
                    return true;
                }
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public AmphetaminesState CaptureState() => _state;

        public void RestoreState(AmphetaminesState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
