using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class SeismicVentState
    {
        public string locationId;
        public bool hasToxicGas = false;
        public string afflictionOnNoMask = "lung_damage_affliction";
        public float staminaCapMultiplier = 0.60f; // Caps stamina at 60%
    }

    /// <summary>
    /// Prompt #372: System: Toxic Ground Ruptures.
    /// Fault line instability causes random map nodes to gain ToxicGas modifiers.
    /// Scavenging without a GasMask guarantees LungDamage, capping max stamina.
    /// </summary>
    
    [Serializable]
    public class SeismicVentsSystemSave
    {
        public string systemId = "seismic_vents_system";
    }
public class SeismicVentsSystem
    {
        private readonly Dictionary<string, SeismicVentState> _vents = new Dictionary<string, SeismicVentState>();

        public event Action<string> OnToxicRuptureSpawned;
        public event Action<string, string> OnLungDamageContracted;

        public IReadOnlyDictionary<string, SeismicVentState> Vents => _vents;

        public void SpawnRuptureAtLocation(string locationId)
        {
            if (string.IsNullOrEmpty(locationId)) return;
            var vent = new SeismicVentState { locationId = locationId, hasToxicGas = true };
            _vents[locationId] = vent;

            OnToxicRuptureSpawned?.Invoke(locationId);
        }

        public bool CheckGasMaskRequirement(string locationId, bool hasGasMask, out string affliction)
        {
            affliction = null;
            if (_vents.TryGetValue(locationId, out var vent) && vent.hasToxicGas)
            {
                if (!hasGasMask)
                {
                    affliction = vent.afflictionOnNoMask;
                    OnLungDamageContracted?.Invoke(locationId, affliction);
                    return false;
                }
            }
            return true;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public SeismicVentsSystemSave CaptureState() => new SeismicVentsSystemSave();

        public void RestoreState(SeismicVentsSystemSave saved) { _ = saved; }

}
}
