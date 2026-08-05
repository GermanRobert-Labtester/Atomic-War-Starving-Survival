using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Amputation & Phantom Pain (Prompt #56). When GunshotWound progresses to
    /// Sepsis in an extremity, the only cure is surgical amputation. Requires
    /// SurgicalTools, Morphine, and high Medical skill. Cures Sepsis but adds
    /// Amputee disability (halves carry weight/speed) and recurring PhantomPain.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class AmputationSystem
    {
        public const string AmputeeDisabilityId = "amputee";
        public const string SurgicalToolsItemId = "surgical_tools";
        public const string MorphineItemId = "morphine";

        /// <summary>Minimum Medical skill to attempt amputation.</summary>
        public const float RequiredMedicalSkill = 0.6f;

        /// <summary>Health lost by patient during surgery.</summary>
        public const float SurgeryHealthCost = 30f;

        /// <summary>Fatigue cost to the surgeon.</summary>
        public const float SurgeonFatigueCost = 25f;

        /// <summary>Morale hit to patient (losing a limb).</summary>
        public const float AmputationMoralePenalty = 35f;

        /// <summary>Morale hit to surgeon (even when necessary).</summary>
        public const float SurgeonMoralePenalty = 15f;

        /// <summary>Chance per day that PhantomPain fires (debilitating spike).</summary>
        public const float PhantomPainDailyChance = 0.25f;

        /// <summary>Fatigue spike from one PhantomPain episode.</summary>
        public const float PhantomPainFatigueSpike = 30f;

        /// <summary>Morale drain from one PhantomPain episode.</summary>
        public const float PhantomPainMoraleDrain = 10f;

        /// <summary>Carry weight multiplier for Amputee survivors.</summary>
        public const float AmputeeCarryWeightMultiplier = 0.5f;

        /// <summary>Speed multiplier for Amputee survivors (expedition ticks).</summary>
        public const float AmputeeSpeedMultiplier = 0.5f;

        private readonly HashSet<string> _amputees = new HashSet<string>();

        private Func<string, Survivors.Survivor> _findSurvivor;
        private Action<Survivors.Survivor, string> _inflictAffliction;
        private MedicalPerkSystem _medicalPerks;
        private Func<int> _getDay;

        // -- Events --
        public event Action<Survivors.Survivor, Survivors.Survivor> OnAmputationPerformed; // patient, surgeon
        public event Action<Survivors.Survivor> OnPhantomPainEpisode;

        public IReadOnlyCollection<string> Amputees => _amputees;

        public AmputationSystem() { }

        public void Bind(
            Func<string, Survivors.Survivor> findSurvivor,
            Action<Survivors.Survivor, string> inflictAffliction)
        {
            _findSurvivor = findSurvivor;
            _inflictAffliction = inflictAffliction;
        }

        /// <summary>Optional medical milestone perks (#204 Anatomist).</summary>
        public void BindMedicalPerks(MedicalPerkSystem perks, Func<int> getDay = null)
        {
            _medicalPerks = perks;
            _getDay = getDay;
        }

        /// <summary>Whether the survivor has the Amputee disability.</summary>
        public bool IsAmputee(string survivorId)
        {
            return !string.IsNullOrEmpty(survivorId) && _amputees.Contains(survivorId);
        }

        /// <summary>
        /// Attempt surgical amputation on a patient with Sepsis.
        /// Returns true if successful.
        /// </summary>
        public bool PerformAmputation(
            string patientId,
            string surgeonId,
            Func<string, int> countItem,
            Func<string, int, bool> consumeItem)
        {
            if (string.IsNullOrEmpty(patientId) || string.IsNullOrEmpty(surgeonId))
                return false;
            if (patientId == surgeonId) return false; // Cannot amputate self.

            var patient = _findSurvivor?.Invoke(patientId);
            var surgeon = _findSurvivor?.Invoke(surgeonId);
            if (patient == null || surgeon == null || !patient.IsAlive || !surgeon.IsAlive)
                return false;

            // Already an amputee of this limb.
            if (_amputees.Contains(patientId)) return false;

            // Surgeon skill check.
            if (surgeon.EffectiveMedicalSkill < RequiredMedicalSkill) return false;

            // Requires SurgicalTools and Morphine.
            // Prompt #215 — Miracle Worker: tools present optional; never consumed.
            if (countItem == null || consumeItem == null) return false;
            bool consumeTools = _medicalPerks == null || _medicalPerks.ConsumesSurgicalTools(surgeon);
            if (consumeTools && countItem(SurgicalToolsItemId) < 1) return false;
            if (countItem(MorphineItemId) < 1) return false;

            if (consumeTools)
                consumeItem(SurgicalToolsItemId, 1);
            consumeItem(MorphineItemId, 1);

            // Surgery costs.
            patient.Needs.Health = Mathf.Clamp(
                patient.Needs.Health - SurgeryHealthCost, 0f, patient.MaxHealthCap);
            patient.Needs.Morale = Mathf.Clamp(
                patient.Needs.Morale - AmputationMoralePenalty, 0f, 100f);
            surgeon.Needs.Fatigue = Mathf.Clamp(
                surgeon.Needs.Fatigue + SurgeonFatigueCost, 0f, 100f);
            surgeon.Needs.Morale = Mathf.Clamp(
                surgeon.Needs.Morale - SurgeonMoralePenalty, 0f, 100f);

            // Apply Amputee disability.
            _amputees.Add(patientId);
            if (patient.DisabilityIds == null)
                patient.DisabilityIds = new List<string>();
            if (!patient.HasDisability(AmputeeDisabilityId))
                patient.DisabilityIds.Add(AmputeeDisabilityId);

            // Prompt #204 — Anatomist: clean amputations (0% PhantomPain).
            int day = _getDay != null ? _getDay() : 0;
            _medicalPerks?.RecordAmputation(surgeon, patient, day);

            OnAmputationPerformed?.Invoke(patient, surgeon);
            return true;
        }

        /// <summary>
        /// Roll for PhantomPain episode on all amputees daily.
        /// Anatomist patients: 0% chance (Prompt #204).
        /// </summary>
        public void TickDaily(IReadOnlyList<Survivors.Survivor> survivors)
        {
            if (survivors == null) return;

            foreach (var id in _amputees)
            {
                var sv = _findSurvivor?.Invoke(id);
                if (sv == null || !sv.IsAlive) continue;

                float chance = _medicalPerks != null
                    ? _medicalPerks.GetPhantomPainDailyChance(id)
                    : PhantomPainDailyChance;
                if (chance <= 0f) continue;

                // Daily roll.
                if (UnityEngine.Random.value < chance)
                {
                    sv.Needs.Fatigue = Mathf.Clamp(
                        sv.Needs.Fatigue + PhantomPainFatigueSpike, 0f, 100f);
                    sv.Needs.Morale = Mathf.Clamp(
                        sv.Needs.Morale - PhantomPainMoraleDrain, 0f, 100f);
                    _inflictAffliction?.Invoke(sv, AfflictionSO.Ids.PhantomPain);
                    OnPhantomPainEpisode?.Invoke(sv);
                }
            }
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public AmputationSave CaptureState()
        {
            var ids = new string[_amputees.Count];
            _amputees.CopyTo(ids);
            return new AmputationSave { AmputeeIds = ids };
        }

        public void RestoreState(AmputationSave save)
        {
            _amputees.Clear();
            if (save?.AmputeeIds == null) return;
            for (int i = 0; i < save.AmputeeIds.Length; i++)
            {
                if (!string.IsNullOrEmpty(save.AmputeeIds[i]))
                    _amputees.Add(save.AmputeeIds[i]);
            }
        }
    }

    [Serializable]
    public class AmputationSave
    {
        public string[] AmputeeIds;
    }
}
