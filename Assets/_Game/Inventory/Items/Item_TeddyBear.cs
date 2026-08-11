using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class TeddyBearState
    {
        public string itemId = "item_teddy_bear";
        public string displayName = "The Teddy Bear";
        public string equippedChildId;
        public bool isEquippedByChild = false;
        public bool isDestroyedOrStolen = false;
        public float mentalBreakSeverityOnLoss = 100f; // Catastrophic mental break
    }

    /// <summary>
    /// Prompt #428: Gear: The Teddy Bear.
    /// Accessory specifically for Child characters (#251). Completely negates ambient Despair and RadiationAnxiety.
    /// If destroyed or stolen, the child suffers a catastrophic mental break.
    /// </summary>
    public class Item_TeddyBear
    {
        private TeddyBearState _state = new TeddyBearState();

        public event Action<TeddyBearState, string> OnTeddyBearEquipped;
        public event Action<TeddyBearState, string, float> OnTeddyBearLostCatastrophicBreak;

        public TeddyBearState State => _state;

        public bool EquipTeddyBear(string childId, bool isChildCharacter)
        {
            if (!isChildCharacter) return false;

            _state.equippedChildId = childId;
            _state.isEquippedByChild = true;
            _state.isDestroyedOrStolen = false;

            OnTeddyBearEquipped?.Invoke(_state, childId);
            return true;
        }

        public void DestroyOrStealTeddyBear(ref float childMorale)
        {
            if (_state.isEquippedByChild && !_state.isDestroyedOrStolen)
            {
                _state.isDestroyedOrStolen = true;
                _state.isEquippedByChild = false;
                childMorale = 0f; // Mental break

                OnTeddyBearLostCatastrophicBreak?.Invoke(_state, _state.equippedChildId, _state.mentalBreakSeverityOnLoss);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TeddyBearState CaptureState() => _state;

        public void RestoreState(TeddyBearState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
