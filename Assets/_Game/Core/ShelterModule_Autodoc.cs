using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class AutodocState
    {
        public string moduleId = "shelter_module_autodoc";
        public float traumaDebuff = 0.5f;
        public float paranoiaDebuff = 0.4f;
        // Track treated patients and their afflictions
        public List<string> treatedPatientIds = new List<string>();
        public List<string> treatedAfflictionIds = new List<string>();
    }

    /// <summary>
    /// Autodoc (Surgery Pod) — heals Phase 2 afflictions perfectly, but the
    /// expired anesthesia leaves the patient conscious during surgery,
    /// inflicting massive Trauma and Paranoia debuffs.
    /// Prompt #791: ShelterModule_Autodoc
    /// </summary>
    public class ShelterModule_Autodoc
    {
        // -- Constants --
        public const float TraumaDebuff = 0.5f;
        public const float ParanoiaDebuff = 0.4f;

        // -- Events --
        public event Action<string, string> OnSurgeryCompleted; // patientId, afflictionId
        public event Action<string, float> OnTraumaApplied;     // patientId, trauma
        public event Action<string, float> OnParanoiaApplied;   // patientId, paranoia

        // -- State --
        private readonly List<string> _treatedPatientIds = new List<string>();
        private readonly List<string> _treatedAfflictionIds = new List<string>();

        // -- Public API --

        /// <summary>
        /// Performs surgery on a patient to heal a specific affliction.
        /// The surgery always succeeds (affliction healed perfectly), but
        /// the expired anesthesia means the patient is conscious — applying
        /// Trauma and Paranoia debuffs.
        /// Returns true if surgery was performed.
        /// </summary>
        public bool PerformSurgery(string patientId, string afflictionId)
        {
            if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(afflictionId))
            {
                Debug.LogWarning("[Autodoc] Invalid patient or affliction id.");
                return false;
            }

            // Heal the affliction perfectly
            _treatedPatientIds.Add(patientId);
            _treatedAfflictionIds.Add(afflictionId);
            OnSurgeryCompleted?.Invoke(patientId, afflictionId);

            // Patient was conscious — apply debuffs
            OnTraumaApplied?.Invoke(patientId, TraumaDebuff);
            OnParanoiaApplied?.Invoke(patientId, ParanoiaDebuff);

            return true;
        }

        /// <summary>Returns the trauma debuff magnitude applied by this autodoc.</summary>
        public float GetTraumaDebuff() => TraumaDebuff;

        /// <summary>Returns the paranoia debuff magnitude applied by this autodoc.</summary>
        public float GetParanoiaDebuff() => ParanoiaDebuff;

        // -- Save / Load --

        public AutodocState CaptureState()
        {
            return new AutodocState
            {
                moduleId = "shelter_module_autodoc",
                traumaDebuff = TraumaDebuff,
                paranoiaDebuff = ParanoiaDebuff,
                treatedPatientIds = new List<string>(_treatedPatientIds),
                treatedAfflictionIds = new List<string>(_treatedAfflictionIds)
            };
        }

        public void RestoreState(AutodocState saved)
        {
            _treatedPatientIds.Clear();
            _treatedAfflictionIds.Clear();
            if (saved == null) return;
            if (saved.treatedPatientIds != null)
                _treatedPatientIds.AddRange(saved.treatedPatientIds);
            if (saved.treatedAfflictionIds != null)
                _treatedAfflictionIds.AddRange(saved.treatedAfflictionIds);
        }
    }
}
