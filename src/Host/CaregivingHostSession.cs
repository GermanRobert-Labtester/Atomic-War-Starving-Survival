using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.Survivors;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// Host session for CaregivingSystem.
    /// Manages caregiver → patient assignments, bond deepening, fatigue and recovery hooks.
    /// Thin host: Core owns all gameplay rules; this session only wires events and exposes actions.
    /// </summary>
    public sealed class CaregivingHostSession : HostSessionBase
    {
        public CaregivingSystem System { get; }
        public string LastEvent { get; private set; } = string.Empty;

        public CaregivingHostSession(CaregivingSystem? system = null)
        {
            System = system ?? new CaregivingSystem();

            System.OnCaregivingStarted += (caregiverId, patientId) =>
            {
                LastEvent = $"[Caregiving] {caregiverId} → tending {patientId}";
            };

            System.OnCaregivingBondDeepened += (caregiverId, patientId, bond) =>
            {
                LastEvent = $"[Caregiving] Bond deepened: {caregiverId} ↔ {patientId} ({bond:F2})";
            };

            System.OnCaregivingEnded += (caregiverId, patientId) =>
            {
                LastEvent = $"[Caregiving] Ended: {caregiverId} ↔ {patientId}";
            };

            System.OnCaregivingDialogueUnlocked += (caregiverId, patientId) =>
            {
                LastEvent = $"[Caregiving] Dialogue unlocked: {caregiverId} ↔ {patientId}";
            };

            System.OnStateChanged += () => RaiseStateChanged();
        }

        public bool AssignCaregiver(string caregiverId, string patientId)
        {
            return System.AssignCaregiver(caregiverId, patientId);
        }

        public void UnassignCaregiver(string patientId)
        {
            System.UnassignCaregiver(patientId);
        }

        public void UnassignCaregiverByCaregiver(string caregiverId)
        {
            System.UnassignCaregiverByCaregiver(caregiverId);
        }

        public void TickDay(int day)
        {
            // Caregiving tick is hour-based; host ticks one full day.
            System.Tick(24f);
        }

        public int ActiveAssignmentCount => System.ActiveAssignmentCount;
        public string? GetPatientForCaregiver(string caregiverId) => System.GetPatientForCaregiver(caregiverId);
        public string? GetCaregiverForPatient(string patientId) => System.GetCaregiverForPatient(patientId);
        public float GetBondStrength(string patientId) => System.GetBondStrength(patientId);

        public override void Save()
        {
            if (!IsDirty) return;
            CaregivingSaveStore.TrySave(System.CaptureState());
            base.Save();
        }

        protected override void UnsubscribeSystemEvents()
        {
            // Events are anonymous lambdas; clearing is handled via Dispose base.
            // No explicit unsubscribe needed beyond base clearing StateChanged.
        }
    }
}
