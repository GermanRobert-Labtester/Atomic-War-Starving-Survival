using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class EMPCascadeState
    {
        public string eventId = "event_emp_cascade";
        public bool isActive = false;
        // Track casualties
        public List<string> crushedSurvivorIds = new List<string>();
        public List<string> amputatedPatientIds = new List<string>();
        public List<string> amputatedLimbIds = new List<string>();
        public List<string> friedDeviceIds = new List<string>();
    }

    /// <summary>
    /// EMP Cascade — a solar flare that hard-fries all active electronic
    /// equipment. Exosuits crush their users as servos seize. Autodocs
    /// amputate whatever limb they were operating on when power cuts.
    /// Prompt #793: Event_EMPCascade
    /// </summary>
    public class Event_EMPCascade
    {
        // -- Events --
        public event Action OnCascadeTriggered;
        public event Action<string> OnExosuitCrushed;            // survivorId
        public event Action<string, string> OnAutodocAmputated;  // patientId, limbId
        public event Action<string> OnDeviceFried;               // deviceId

        // -- State --
        private bool _isActive = false;
        private readonly List<string> _crushedSurvivorIds = new List<string>();
        private readonly List<string> _amputatedPatientIds = new List<string>();
        private readonly List<string> _amputatedLimbIds = new List<string>();
        private readonly List<string> _friedDeviceIds = new List<string>();

        // -- Public API --

        /// <summary>
        /// Triggers the EMP cascade. All active tech is hard-fried.
        /// Each exosuit user is crushed by their seizing suit.
        /// Each autodoc patient has their current limb amputated.
        /// </summary>
        /// <param name="exosuitUsers">Survivor IDs currently wearing exosuits.</param>
        /// <param name="autodocPatients">
        /// Pairs of (patientId, limbId) currently in the autodoc.
        /// Pass as parallel lists: patientIds and limbIds must have equal length.
        /// </param>
        public void TriggerCascade(List<string> exosuitUsers, List<string> autodocPatientIds, List<string> autodocLimbIds = null)
        {
            _isActive = true;
            OnCascadeTriggered?.Invoke();

            // Exosuits crush their users
            if (exosuitUsers != null)
            {
                for (int i = 0; i < exosuitUsers.Count; i++)
                {
                    string survivorId = exosuitUsers[i];
                    if (string.IsNullOrEmpty(survivorId)) continue;
                    _crushedSurvivorIds.Add(survivorId);
                    _friedDeviceIds.Add("item_exosuit_" + survivorId);
                    OnExosuitCrushed?.Invoke(survivorId);
                    OnDeviceFried?.Invoke("item_exosuit_" + survivorId);
                }
            }

            // Autodoc amputates current limb on each patient
            if (autodocPatientIds != null)
            {
                for (int i = 0; i < autodocPatientIds.Count; i++)
                {
                    string patientId = autodocPatientIds[i];
                    if (string.IsNullOrEmpty(patientId)) continue;
                    string limbId = (autodocLimbIds != null && i < autodocLimbIds.Count)
                        ? autodocLimbIds[i]
                        : "limb_unknown";
                    _amputatedPatientIds.Add(patientId);
                    _amputatedLimbIds.Add(limbId);
                    OnAutodocAmputated?.Invoke(patientId, limbId);
                }
                // Autodoc device itself is fried
                _friedDeviceIds.Add("shelter_module_autodoc");
                OnDeviceFried?.Invoke("shelter_module_autodoc");
            }
        }

        /// <summary>Returns true if the cascade event has been triggered.</summary>
        public bool IsActive() => _isActive;

        // -- Save / Load --

        public EMPCascadeState CaptureState()
        {
            return new EMPCascadeState
            {
                eventId = "event_emp_cascade",
                isActive = _isActive,
                crushedSurvivorIds = new List<string>(_crushedSurvivorIds),
                amputatedPatientIds = new List<string>(_amputatedPatientIds),
                amputatedLimbIds = new List<string>(_amputatedLimbIds),
                friedDeviceIds = new List<string>(_friedDeviceIds)
            };
        }

        public void RestoreState(EMPCascadeState saved)
        {
            _crushedSurvivorIds.Clear();
            _amputatedPatientIds.Clear();
            _amputatedLimbIds.Clear();
            _friedDeviceIds.Clear();
            if (saved == null) return;
            _isActive = saved.isActive;
            _crushedSurvivorIds.AddRange(saved.crushedSurvivorIds);
            _amputatedPatientIds.AddRange(saved.amputatedPatientIds);
            _amputatedLimbIds.AddRange(saved.amputatedLimbIds);
            _friedDeviceIds.AddRange(saved.friedDeviceIds);
        }
    }
}
