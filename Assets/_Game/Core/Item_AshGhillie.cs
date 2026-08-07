using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AshGhillieState
    {
        public string itemId = "item_ash_ghillie";
        public string displayName = "Ash Ghillie Suit";
        public float stealthBonus = 0.95f;
        public bool fireVulnerability = true;
        public float burnDamageOnIgnition = 40f;
        public bool isEquipped = false;
        public float durability = 60f;
    }

    /// <summary>
    /// Prompt #599: Item: Ash Ghillie Suit.
    /// Crafted from Cloth+Ash. Provides 95% stealth in Wasteland terrain.
    /// If hit by Fire or Explosive damage, instantly ignites causing severe burns.
    /// </summary>
    public class Item_AshGhillie
    {
        private AshGhillieState _state = new AshGhillieState();

        public event Action<AshGhillieState> OnGhillieEquipped;
        public event Action<AshGhillieState, float> OnGhillieIgnited;
        public event Action<AshGhillieState, float> OnBurnDamageApplied;

        public AshGhillieState State => _state;

        public void Equip()
        {
            _state.isEquipped = true;
            OnGhillieEquipped?.Invoke(_state);
        }

        public float GetStealthMultiplier(string nodeType)
        {
            if (!_state.isEquipped)
                return 1f;

            if (nodeType == "wasteland")
                return _state.stealthBonus;

            return 1f;
        }

        public float CheckFireIgnition(string damageType)
        {
            if (!_state.isEquipped)
                return 0f;

            if (damageType == "fire" || damageType == "explosive")
            {
                float damage = _state.burnDamageOnIgnition;
                OnGhillieIgnited?.Invoke(_state, damage);
                OnBurnDamageApplied?.Invoke(_state, damage);
                return damage;
            }

            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public AshGhillieState CaptureState() => _state;

        public void RestoreState(AshGhillieState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
