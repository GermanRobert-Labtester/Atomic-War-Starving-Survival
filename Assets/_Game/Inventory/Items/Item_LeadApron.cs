using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class LeadApronState
    {
        public string itemId = "item_lead_apron";
        public string displayName = "Lead-Lined Apron";
        public float torsoRadiationProtection = 0.90f; // 90% torso rad protection
        public float movementSpeedPenalty = 0.25f;    // 25% speed penalty
        public float thermalProtection = 0f;
        public float ballisticProtection = 0f;
    }

    /// <summary>
    /// Prompt #419: Gear: Lead-Lined Apron.
    /// Provides torso RadiationProtection, but slows MovementSpeed by 25% due to its weight,
    /// offering zero Thermal or Ballistic protection.
    /// </summary>
    public class Item_LeadApron
    {
        private LeadApronState _state = new LeadApronState();

        public event Action<LeadApronState, float> OnLeadApronEquipped;

        public LeadApronState State => _state;

        public float EquipApron(out float speedMultiplier)
        {
            speedMultiplier = 1.0f - _state.movementSpeedPenalty;
            OnLeadApronEquipped?.Invoke(_state, speedMultiplier);
            return _state.torsoRadiationProtection;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public LeadApronState CaptureState() => _state;

        public void RestoreState(LeadApronState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
