using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class KevlarVestState
    {
        public string itemId = "item_kevlar_vest";
        public string displayName = "Riot / Kevlar Vest";
        public float ballisticDamageReductionRatio = 0.70f; // 70% reduction
        public float radiationProtection = 0f;
        public float coldProtection = 0f;
        public float animalBiteProtection = 0f;
    }

    /// <summary>
    /// Prompt #422: Gear: Riot / Kevlar Vest.
    /// High-tier BallisticArmor that reduces incoming human gunfire damage by 70%.
    /// Useless against Radiation, Cold, or FeralDogs.
    /// </summary>
    public class Item_KevlarVest
    {
        private KevlarVestState _state = new KevlarVestState();

        public event Action<KevlarVestState, float, float> OnBallisticDamageMitigated;

        public KevlarVestState State => _state;

        public float ApplyDamageMitigation(float incomingDamage, string damageType)
        {
            if (damageType == "human_gunfire" || damageType == "ballistic")
            {
                float mitigated = incomingDamage * (1.0f - _state.ballisticDamageReductionRatio);
                OnBallisticDamageMitigated?.Invoke(_state, incomingDamage, mitigated);
                return mitigated;
            }
            return incomingDamage;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public KevlarVestState CaptureState() => _state;

        public void RestoreState(KevlarVestState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
