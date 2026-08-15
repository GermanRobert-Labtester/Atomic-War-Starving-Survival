using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Caregiving System — assigning a healthy survivor to tend a bedridden,
    /// irradiated companion builds deep mutual trust, speeds up recovery,
    /// and unlocks unique dialogue.
    ///
    /// Owns: Survivor.CaregivingTargetId, Survivor.CaregiverId,
    /// Survivor.CaregivingBondStrength.
    /// </summary>
    public class CaregivingSystem
    {
        public const float RecoverySpeedBonus = 0.30f;
        public const float AffinityGainPerDay = 5f;
        public const float CaregiverFatigueDrain = 0.15f;
        public const float MinBondForDialogue = 0.5f;

        public event Action<Survivor, Survivor> OnCaregivingStarted;
        // caregiver, patient
        public event Action<Survivor, Survivor, float> OnCaregivingBondDeepened;
        public event Action<Survivor, Survivor> OnCaregivingEnded;
        public event Action<Survivor, Survivor> OnCaregivingDialogueUnlocked;

        // Host hooks
        public Action<string, string, float> AdjustAffinity;
        public Action<Survivor, float> ApplyFatigueDelta;
        public Action<Survivor, float> ApplyHealthRecoveryBonus;

        public bool AssignCaregiver(Survivor caregiver, Survivor patient)
        {
            if (caregiver == null || patient == null || !caregiver.IsAlive ||
                !patient.IsAlive || caregiver.Id == patient.Id)
                return false;

            // Only care for bedridden/incapacitated
            if (patient.State != SurvivorState.Incapacitated &&
                patient.State != SurvivorState.Sick)
                return false;

            // Release previous caregiver if any
            if (!string.IsNullOrEmpty(patient.CaregiverId))
                UnassignCaregiver(patient);

            caregiver.CaregivingTargetId = patient.Id;
            patient.CaregiverId = caregiver.Id;
            OnCaregivingStarted?.Invoke(caregiver, patient);
            return true;
        }

        public void UnassignCaregiver(Survivor patient)
        {
            if (patient == null || string.IsNullOrEmpty(patient.CaregiverId))
                return;
            var survivors = GetSurvivorsForLookup?.Invoke();
            var caregiver = SurvivorById(patient.CaregiverId, survivors);
            if (caregiver != null)
                caregiver.CaregivingTargetId = null;
            OnCaregivingEnded?.Invoke(caregiver, patient);
            patient.CaregiverId = null;
        }

        public void Tick(float gameHours, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null) return;

            for (int i = 0; i < survivors.Count; i++)
            {
                var caregiver = survivors[i];
                if (caregiver == null || !caregiver.IsAlive) continue;
                if (string.IsNullOrEmpty(caregiver.CaregivingTargetId)) continue;

                var patient = SurvivorById(caregiver.CaregivingTargetId, survivors);
                if (patient == null || !patient.IsAlive)
                {
                    caregiver.CaregivingTargetId = null;
                    continue;
                }

                // Recovery speed bonus
                ApplyHealthRecoveryBonus?.Invoke(patient,
                    RecoverySpeedBonus * (gameHours / 24f));

                // Caregiver fatigue cost
                ApplyFatigueDelta?.Invoke(caregiver,
                    CaregiverFatigueDrain * gameHours);

                // Bond growth
                patient.CaregivingBondStrength = Math.Min(1f,
                    patient.CaregivingBondStrength + 0.02f * (gameHours / 24f));
                AdjustAffinity?.Invoke(caregiver.Id, patient.Id,
                    AffinityGainPerDay * (gameHours / 24f));

                OnCaregivingBondDeepened?.Invoke(caregiver, patient,
                    patient.CaregivingBondStrength);

                if (patient.CaregivingBondStrength >= MinBondForDialogue &&
                    caregiver.CaregivingTargetId == patient.Id)
                {
                    OnCaregivingDialogueUnlocked?.Invoke(caregiver, patient);
                }
            }
        }

        // Public hook for looking up survivors
        public Func<IReadOnlyList<Survivor>> GetSurvivorsForLookup;

        private Survivor SurvivorById(string id, IReadOnlyList<Survivor> survivors)
        {
            if (survivors == null || string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].Id == id)
                    return survivors[i];
            }
            return null;
        }
    }
}
