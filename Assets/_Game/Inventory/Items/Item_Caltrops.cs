using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class CaltropsState
    {
        public string itemId = "item_caltrops";
        public string displayName = "Caltrops";
        public float delayHours = 12f;
        public float damageToRaiders = 15f;
        public int maxUses = 3;
        public int usesRemaining = 3;
        public string deployedNodeId = "";
    }

    /// <summary>
    /// Prompt #607: Item: Caltrops.
    /// Dropped on Expedition Map nodes. Faction Caravan/Raid passing through
    /// the node is delayed 12h and takes light damage. Buys time during siege.
    /// </summary>
    public class Item_Caltrops
    {
        private CaltropsState _state = new CaltropsState();

        public event Action<CaltropsState, string> OnCaltropsDeployed;
        public event Action<CaltropsState, string, float> OnFactionDelayed;
        public event Action<CaltropsState> OnCaltropsExhausted;

        public CaltropsState State => _state;

        public bool Deploy(string nodeId)
        {
            if (_state.usesRemaining <= 0)
                return false;

            _state.deployedNodeId = nodeId;
            OnCaltropsDeployed?.Invoke(_state, nodeId);
            return true;
        }

        public (bool delayed, float damage) ApplyToPassingFaction(string factionType)
        {
            if (string.IsNullOrEmpty(_state.deployedNodeId) || _state.usesRemaining <= 0)
                return (false, 0f);

            ConsumeUse();
            OnFactionDelayed?.Invoke(_state, factionType, _state.damageToRaiders);
            return (true, _state.damageToRaiders);
        }

        public void ConsumeUse()
        {
            _state.usesRemaining = Mathf.Max(0, _state.usesRemaining - 1);

            if (_state.usesRemaining <= 0)
            {
                _state.deployedNodeId = "";
                OnCaltropsExhausted?.Invoke(_state);
            }
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public CaltropsState CaptureState() => _state;

        public void RestoreState(CaltropsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
