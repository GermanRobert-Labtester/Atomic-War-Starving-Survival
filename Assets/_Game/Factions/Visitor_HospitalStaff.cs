using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Factions
{
    [Serializable]
    public class HospitalStaffState
    {
        public string cardId = "visitor_hospital_staff";
        public string displayName = "Hospital Triage Staff";
        public bool isTaxDemanded = true;
        public bool isTaxPaid = false;
        public float medicalTaxRatio = 0.50f; // 50% of medical supplies
        public bool willHealPlayer = true;
    }

    /// <summary>
    /// Prompt #353: Visitor Event: Hospital - Doctors & Nurses.
    /// Desperate triage center. Friendly staff demands 50% of player's MedicalSupplies as a "Tax".
    /// Refusal makes them passive-aggressive and refuse healing services.
    /// </summary>
    /// <summary>DEMOTE-Visitor-batch — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Visitor_HospitalStaff
    {
        private HospitalStaffState _state = new HospitalStaffState();

        public event Action<HospitalStaffState, int> OnMedicalTaxPaid;
        public event Action<HospitalStaffState> OnMedicalTaxRefused;

        public HospitalStaffState State => _state;

        public bool PayMedicalTax(ref int playerMedsCount)
        {
            int tax = Mathf.CeilToInt(playerMedsCount * _state.medicalTaxRatio);
            if (playerMedsCount >= tax && tax > 0)
            {
                playerMedsCount -= tax;
                _state.isTaxPaid = true;
                _state.willHealPlayer = true;

                OnMedicalTaxPaid?.Invoke(_state, tax);
                return true;
            }
            return false;
        }

        public void RefuseMedicalTax()
        {
            _state.isTaxPaid = false;
            _state.willHealPlayer = false;

            OnMedicalTaxRefused?.Invoke(_state);
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public HospitalStaffState CaptureState() => _state;

        public void RestoreState(HospitalStaffState saved)
        {
            if (saved == null) return;
            _state = saved;
        }

}
}
