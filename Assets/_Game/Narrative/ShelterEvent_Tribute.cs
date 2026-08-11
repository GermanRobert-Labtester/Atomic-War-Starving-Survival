using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Narrative
{
    [Serializable]
    public class TributeSystemState
    {
        public string eventId = "shelter_event_tribute";
        public string warlordFactionId = "military_remnants";
        public int fuelDemand = 20;
        public int foodDemand = 50;
        public int intervalDays = 15;
        public int lastPaidDay = 0;
        public bool isProtectionActive = false;
        public bool isLevel5SiegeTriggered = false;
    }

    /// <summary>
    /// Prompt #412: Event: The Warlord's Tribute.
    /// Endgame dominant faction demands 20 Fuel and 50 Food every 15 days.
    /// Paying grants protection from all other Raids. Refusing triggers a Level 5 Siege raid.
    /// </summary>
    public class ShelterEvent_Tribute
    {
        private TributeSystemState _state = new TributeSystemState();

        public event Action<TributeSystemState> OnTributePaidProtectionGranted;
        public event Action<TributeSystemState> OnTributeRefusedSiegeTriggered;

        public TributeSystemState State => _state;

        public bool PayTribute(ref int fuelStorage, ref int foodStorage, int currentDay)
        {
            if (fuelStorage >= _state.fuelDemand && foodStorage >= _state.foodDemand)
            {
                fuelStorage -= _state.fuelDemand;
                foodStorage -= _state.foodDemand;
                _state.lastPaidDay = currentDay;
                _state.isProtectionActive = true;
                _state.isLevel5SiegeTriggered = false;

                OnTributePaidProtectionGranted?.Invoke(_state);
                return true;
            }

            RefuseTribute();
            return false;
        }

        public void RefuseTribute()
        {
            _state.isProtectionActive = false;
            _state.isLevel5SiegeTriggered = true;
            OnTributeRefusedSiegeTriggered?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public TributeSystemState CaptureState() => _state;

        public void RestoreState(TributeSystemState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
