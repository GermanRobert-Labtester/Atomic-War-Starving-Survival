using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class WaterTabsState
    {
        public string itemId = "item_water_tabs";
        public string displayName = "Water Purification Tablets";
        public int tabletCount = 10;
    }

    /// <summary>
    /// Prompt #435: Item: Water Purification Tablets.
    /// Converts DirtyWater to CleanWater while out on the map without needing the bunker Purifier.
    /// Does NOT work on IrradiatedWater.
    /// </summary>
    public class Item_WaterTabs
    {
        private WaterTabsState _state = new WaterTabsState();

        public event Action<WaterTabsState, int> OnFieldPurificationExecuted;

        public WaterTabsState State => _state;

        public bool PurifyFieldWater(string waterType, ref int dirtyWater, ref int cleanWater)
        {
            if (waterType == "irradiated_water") return false; // Irradiated water cannot be purified with tabs

            if (waterType == "dirty_water" && dirtyWater > 0 && _state.tabletCount > 0)
            {
                dirtyWater--;
                cleanWater++;
                _state.tabletCount--;

                OnFieldPurificationExecuted?.Invoke(_state, _state.tabletCount);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public WaterTabsState CaptureState() => _state;

        public void RestoreState(WaterTabsState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
