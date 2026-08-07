using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class VisionLossState
    {
        public string survivorId;
        public bool hasCataracts = false;
        public bool isFlashBlinded = false;
        public float uiBlurIntensity = 0.80f; // High UI blur intensity
    }

    /// <summary>
    /// Prompt #396: System: Cataracts & Flash Blindness.
    /// Flashpoint or BioFog exposure causes Cataracts.
    /// Permanently blurs the UI screen whenever that specific survivor is selected.
    /// </summary>
    
    [Serializable]
    public class VisionLossSystemSave
    {
        public string systemId = "vision_loss_system";
    }
public class VisionLossSystem
    {
        private readonly Dictionary<string, VisionLossState> _visionMap = new Dictionary<string, VisionLossState>();

        public event Action<string, float> OnCataractsBlurApplied;

        public IReadOnlyDictionary<string, VisionLossState> VisionMap => _visionMap;

        public void InflictCataracts(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId)) return;
            var state = new VisionLossState { survivorId = survivorId, hasCataracts = true };
            _visionMap[survivorId] = state;
        }

        public float EvaluateUIBlurForSelectedSurvivor(string selectedSurvivorId)
        {
            if (!string.IsNullOrEmpty(selectedSurvivorId) && _visionMap.TryGetValue(selectedSurvivorId, out var state) && state.hasCataracts)
            {
                OnCataractsBlurApplied?.Invoke(selectedSurvivorId, state.uiBlurIntensity);
                return state.uiBlurIntensity;
            }
            return 0f;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public VisionLossSystemSave CaptureState() => new VisionLossSystemSave();

        public void RestoreState(VisionLossSystemSave saved) { _ = saved; }

}
}
