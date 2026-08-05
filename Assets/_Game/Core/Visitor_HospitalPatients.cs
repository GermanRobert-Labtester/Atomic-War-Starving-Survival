using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class HospitalPatientsState
    {
        public string cardId = "visitor_hospital_patients";
        public string displayName = "Bedridden Patients";
        public bool isDespairAuraActive = true;
        public float maxStaminaMultiplier = 0.50f; // 50% max stamina reduction
    }

    /// <summary>
    /// Prompt #354: Visitor Event: Hospital - The Patients.
    /// Staff fled leaving hundreds of bedridden patients to die.
    /// Free looting, but ambient Despair reduces the scavenger's max Stamina by 50% for the trip.
    /// </summary>
    public class Visitor_HospitalPatients
    {
        private HospitalPatientsState _state = new HospitalPatientsState();

        public event Action<HospitalPatientsState, float> OnDespairAuraApplied;

        public HospitalPatientsState State => _state;

        public float ApplyDespairAura(float currentMaxStamina)
        {
            float effectiveMax = currentMaxStamina * _state.maxStaminaMultiplier;
            OnDespairAuraApplied?.Invoke(_state, effectiveMax);
            return effectiveMax;
        }
    }
}
