#pragma warning disable CS0067 // Public API event surface; subscribers arrive with feature wiring
using System;
using System.Collections.Generic;
#pragma warning disable CS8618

namespace Ashfall.Core.Survivors
{
    // ── Save-state DTOs ────────────────────────────────────────────────

    [Serializable]
    public class CaregivingSaveState
    {
        public List<CaregivingAssignmentState> Assignments
            = new List<CaregivingAssignmentState>();
    }

    [Serializable]
    public class CaregivingAssignmentState
    {
        public string CaregiverId;
        public string PatientId;
        public float BondStrength;
    }

    // ── System ─────────────────────────────────────────────────────────

    /// <summary>
    /// Caregiving System — assigning a healthy survivor to tend a bedridden,
    /// irradiated companion builds deep mutual trust, speeds up recovery,
    /// and unlocks unique dialogue.
    ///
    /// Engine-agnostic port. Survivors are identified by string IDs.
    /// </summary>
    public class CaregivingSystem
    {
        // ── Constants ──────────────────────────────────────────────────
        public const float RecoverySpeedBonus = 0.30f;
        public const float AffinityGainPerDay = 5f;
        public const float CaregiverFatigueDrain = 0.15f;
        public const float MinBondForDialogue = 0.5f;
        public const float BondGrowthPerDay = 0.02f;

        // ── Events ─────────────────────────────────────────────────────

        /// <summary>Fired when a caregiver is assigned: (caregiverId, patientId).</summary>
        public event Action<string, string> OnCaregivingStarted;

        /// <summary>Fired when bond deepens during tick: (caregiverId, patientId, bondStrength).</summary>
        public event Action<string, string, float> OnCaregivingBondDeepened;

        /// <summary>Fired when caregiving is unassigned: (caregiverId, patientId).</summary>
        public event Action<string, string> OnCaregivingEnded;

        /// <summary>Fired when bond reaches dialogue threshold: (caregiverId, patientId).</summary>
        public event Action<string, string> OnCaregivingDialogueUnlocked;

        /// <summary>Generic state-changed event for save/UI.</summary>
        public event Action OnStateChanged;

        // ── Host hooks ─────────────────────────────────────────────────

        /// <summary>Check if a survivor is alive.</summary>
        public Func<string, bool> IsAlive;

        /// <summary>Check if a survivor is in a state that allows them to provide care
        /// (i.e., not incapacitated themselves).</summary>
        public Func<string, bool> CanProvideCare;

        /// <summary>Check if a patient is in a care-receiving state
        /// (bedridden / incapacitated / sick).</summary>
        public Func<string, bool> NeedsCare;

        /// <summary>Adjust affinity between two survivors: (caregiver, patient, delta).</summary>
        public Action<string, string, float> AdjustAffinity;

        /// <summary>Apply fatigue delta to a survivor: (survivorId, delta).</summary>
        public Action<string, float> ApplyFatigueDelta;

        /// <summary>Apply health recovery bonus to a patient: (patientId, amount).</summary>
        public Action<string, float> ApplyHealthRecoveryBonus;

        // ── Internal state ─────────────────────────────────────────────
        // caregiverId → patientId
        readonly Dictionary<string, string> _caregiverToPatient
            = new Dictionary<string, string>();
        // patientId → caregiverId
        readonly Dictionary<string, string> _patientToCaregiver
            = new Dictionary<string, string>();
        // patientId → bond strength [0..1]
        readonly Dictionary<string, float> _bondStrengths
            = new Dictionary<string, float>();

        // ── Public API ─────────────────────────────────────────────────

        /// <summary>
        /// Assign a caregiver to a patient. Returns false if invalid.
        /// Automatically unassigns any previous caregiver for the patient.
        /// </summary>
        public bool AssignCaregiver(string caregiverId, string patientId)
        {
            if (string.IsNullOrEmpty(caregiverId) || string.IsNullOrEmpty(patientId))
                return false;
            if (caregiverId == patientId)
                return false;
            if (IsAlive != null && (!IsAlive(caregiverId) || !IsAlive(patientId)))
                return false;
            if (CanProvideCare != null && !CanProvideCare(caregiverId))
                return false;
            if (NeedsCare != null && !NeedsCare(patientId))
                return false;

            // Release previous caregiver if any
            if (_patientToCaregiver.TryGetValue(patientId, out var prevCaregiver))
            {
                _caregiverToPatient.Remove(prevCaregiver);
                OnCaregivingEnded?.Invoke(prevCaregiver, patientId);
            }

            // If caregiver was already caring for someone else, unassign that patient
            if (_caregiverToPatient.TryGetValue(caregiverId, out var prevPatient))
            {
                _patientToCaregiver.Remove(prevPatient);
                // Don't fire ended for the old patient here — the new assignment
                // is replacing it. But we should clean up.
                OnCaregivingEnded?.Invoke(caregiverId, prevPatient);
            }

            _caregiverToPatient[caregiverId] = patientId;
            _patientToCaregiver[patientId] = caregiverId;
            if (!_bondStrengths.ContainsKey(patientId))
                _bondStrengths[patientId] = 0f;

            OnCaregivingStarted?.Invoke(caregiverId, patientId);
            OnStateChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Unassign the caregiver from a patient.
        /// </summary>
        public void UnassignCaregiver(string patientId)
        {
            if (string.IsNullOrEmpty(patientId)) return;
            if (!_patientToCaregiver.TryGetValue(patientId, out var caregiverId))
                return;

            _caregiverToPatient.Remove(caregiverId);
            _patientToCaregiver.Remove(patientId);
            // Keep bond strength for historical/dialogue purposes

            OnCaregivingEnded?.Invoke(caregiverId, patientId);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Unassign a caregiver from whatever patient they are tending.
        /// </summary>
        public void UnassignCaregiverByCaregiver(string caregiverId)
        {
            if (string.IsNullOrEmpty(caregiverId)) return;
            if (!_caregiverToPatient.TryGetValue(caregiverId, out var patientId))
                return;

            _patientToCaregiver.Remove(patientId);
            _caregiverToPatient.Remove(caregiverId);

            OnCaregivingEnded?.Invoke(caregiverId, patientId);
            OnStateChanged?.Invoke();
        }

        /// <summary>
        /// Tick all active caregiving assignments over elapsed game hours.
        /// </summary>
        public void Tick(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Snapshot keys to allow mutation during iteration
            var caregiverIds = new List<string>(_caregiverToPatient.Keys);
            bool changed = false;

            for (int i = 0; i < caregiverIds.Count; i++)
            {
                var caregiverId = caregiverIds[i];
                if (!_caregiverToPatient.TryGetValue(caregiverId, out var patientId))
                    continue;

                // Check if caregiver is still able to provide care
                if (IsAlive != null && !IsAlive(caregiverId))
                {
                    _caregiverToPatient.Remove(caregiverId);
                    _patientToCaregiver.Remove(patientId);
                    OnCaregivingEnded?.Invoke(caregiverId, patientId);
                    changed = true;
                    continue;
                }

                // Check if patient is still alive
                if (IsAlive != null && !IsAlive(patientId))
                {
                    _caregiverToPatient.Remove(caregiverId);
                    _patientToCaregiver.Remove(patientId);
                    OnCaregivingEnded?.Invoke(caregiverId, patientId);
                    changed = true;
                    continue;
                }

                float dayFraction = gameHours / 24f;

                // Recovery speed bonus for patient
                ApplyHealthRecoveryBonus?.Invoke(patientId,
                    RecoverySpeedBonus * dayFraction);

                // Caregiver fatigue cost
                ApplyFatigueDelta?.Invoke(caregiverId,
                    CaregiverFatigueDrain * gameHours);

                // Bond growth
                if (!_bondStrengths.ContainsKey(patientId))
                    _bondStrengths[patientId] = 0f;

                float prevBond = _bondStrengths[patientId];
                float newBond = MathfCompat.Min(1f,
                    prevBond + BondGrowthPerDay * dayFraction);
                _bondStrengths[patientId] = newBond;

                // Affinity gain
                AdjustAffinity?.Invoke(caregiverId, patientId,
                    AffinityGainPerDay * dayFraction);

                OnCaregivingBondDeepened?.Invoke(caregiverId, patientId, newBond);

                // Dialogue unlock (fire once when crossing threshold)
                if (prevBond < MinBondForDialogue && newBond >= MinBondForDialogue)
                    OnCaregivingDialogueUnlocked?.Invoke(caregiverId, patientId);

                changed = true;
            }

            if (changed)
                OnStateChanged?.Invoke();
        }

        // ── Queries ────────────────────────────────────────────────────

        /// <summary>Get the patient ID for a caregiver, or null if not caregiving.</summary>
        public string? GetPatientForCaregiver(string caregiverId)
        {
            if (string.IsNullOrEmpty(caregiverId)) return null;
            return _caregiverToPatient.TryGetValue(caregiverId, out var patientId)
                ? patientId : null;
        }

        /// <summary>Get the caregiver ID for a patient, or null if no caregiver assigned.</summary>
        public string? GetCaregiverForPatient(string patientId)
        {
            if (string.IsNullOrEmpty(patientId)) return null;
            return _patientToCaregiver.TryGetValue(patientId, out var caregiverId)
                ? caregiverId : null;
        }

        /// <summary>Get the bond strength for a patient. Returns 0 if no caregiving has occurred.</summary>
        public float GetBondStrength(string patientId)
        {
            if (string.IsNullOrEmpty(patientId)) return 0f;
            return _bondStrengths.TryGetValue(patientId, out var bond) ? bond : 0f;
        }

        /// <summary>Check if a caregiver is currently assigned to any patient.</summary>
        public bool IsCaregiver(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId)
                && _caregiverToPatient.ContainsKey(survivorId);
        }

        /// <summary>Check if a patient currently has a caregiver assigned.</summary>
        public bool HasCaregiver(string patientId)
        {
            return !string.IsNullOrEmpty(patientId)
                && _patientToCaregiver.ContainsKey(patientId);
        }

        /// <summary>Get the total number of active caregiving assignments.</summary>
        public int ActiveAssignmentCount => _caregiverToPatient.Count;

        // ── Save / Load ────────────────────────────────────────────────

        /// <summary>
        /// Capture a deep copy of all caregiving state for serialization.
        /// </summary>
        public CaregivingSaveState CaptureState()
        {
            var save = new CaregivingSaveState();
            foreach (var kvp in _caregiverToPatient)
            {
                save.Assignments.Add(new CaregivingAssignmentState
                {
                    CaregiverId = kvp.Key,
                    PatientId = kvp.Value,
                    BondStrength = _bondStrengths.TryGetValue(kvp.Value, out var b) ? b : 0f
                });
            }
            return save;
        }

        /// <summary>
        /// Restore state from a previously captured save. Deep copies all data.
        /// Pass null to clear all state.
        /// </summary>
        public void RestoreState(CaregivingSaveState save)
        {
            _caregiverToPatient.Clear();
            _patientToCaregiver.Clear();
            _bondStrengths.Clear();

            if (save == null || save.Assignments == null) return;

            foreach (var a in save.Assignments)
            {
                if (a == null) continue;
                if (string.IsNullOrEmpty(a.CaregiverId) || string.IsNullOrEmpty(a.PatientId))
                    continue;

                _caregiverToPatient[a.CaregiverId] = a.PatientId;
                _patientToCaregiver[a.PatientId] = a.CaregiverId;
                _bondStrengths[a.PatientId] = MathfCompat.Clamp01(a.BondStrength);
            }

            OnStateChanged?.Invoke();
        }
    }
}
