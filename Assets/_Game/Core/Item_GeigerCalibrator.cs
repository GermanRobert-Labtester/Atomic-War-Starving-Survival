using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class GeigerCalibratorState
    {
        public string itemId = "item_geiger_calibrator";
        public string displayName = "Broken Geiger Calibrator";
        public bool isRepaired = false;
        public int earlyWarningHours = 24;
    }

    /// <summary>
    /// Prompt #464: Artifact: Broken Geiger Calibrator.
    /// Key component that when repaired at the Workbench upgrades all Dosimeters and Geiger counters
    /// to detect Fallout Storms 24 hours before they hit the region.
    /// </summary>
    public class Item_GeigerCalibrator
    {
        private GeigerCalibratorState _state = new GeigerCalibratorState();

        public event Action<GeigerCalibratorState, int> OnGeigerCalibratorRepaired;

        public GeigerCalibratorState State => _state;

        public bool RepairCalibrator(int electronicsParts, ref int electronicStorage)
        {
            if (!_state.isRepaired && electronicStorage >= electronicsParts)
            {
                electronicStorage -= electronicsParts;
                _state.isRepaired = true;

                OnGeigerCalibratorRepaired?.Invoke(_state, _state.earlyWarningHours);
                return true;
            }
            return false;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public GeigerCalibratorState CaptureState() => _state;

        public void RestoreState(GeigerCalibratorState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
