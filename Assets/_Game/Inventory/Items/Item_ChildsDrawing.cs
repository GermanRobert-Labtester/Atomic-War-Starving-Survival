using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class ChildsDrawingState
    {
        public string itemId = "item_childs_drawing";
        public string displayName = "Child's Crayon Drawing";
        public bool isPinnedOnWall = false;
        public float parentMoraleBonus = 15f;
        public float parentTraumaPenaltyOnChildDeath = 40f;
    }

    /// <summary>
    /// Prompt #463: Artifact: Child's Crayon Drawing.
    /// Pinned to the shelter wall, granting +15 global Morale to parent survivors.
    /// If the child dies, the drawing becomes a severe Trauma trigger for parents.
    /// </summary>
    public class Item_ChildsDrawing
    {
        private ChildsDrawingState _state = new ChildsDrawingState();

        public event Action<ChildsDrawingState, float> OnDrawingPinnedParentMoraleBoosted;
        public event Action<ChildsDrawingState, string, float> OnChildDeathTraumaTriggered;

        public ChildsDrawingState State => _state;

        public void PinDrawingOnWall(ref float parentMorale)
        {
            _state.isPinnedOnWall = true;
            parentMorale = Mathf.Min(100f, parentMorale + _state.parentMoraleBonus);
            OnDrawingPinnedParentMoraleBoosted?.Invoke(_state, _state.parentMoraleBonus);
        }

        public void HandleChildDeath(string parentId, ref float parentSanity)
        {
            if (_state.isPinnedOnWall)
            {
                parentSanity = Mathf.Max(0f, parentSanity - _state.parentTraumaPenaltyOnChildDeath);
                OnChildDeathTraumaTriggered?.Invoke(_state, parentId, _state.parentTraumaPenaltyOnChildDeath);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public ChildsDrawingState CaptureState() => _state;

        public void RestoreState(ChildsDrawingState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
