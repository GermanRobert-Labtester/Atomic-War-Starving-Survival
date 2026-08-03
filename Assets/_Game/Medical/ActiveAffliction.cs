using System;

namespace AtomicWar._Game.Medical
{
    /// <summary>
    /// Runtime instance of an affliction on one survivor. Save/load safe (ids + floats).
    /// </summary>
    [Serializable]
    public class ActiveAffliction
    {
        public string AfflictionId;
        public float HoursActive;
        public float HoursUntilProgression;
        public bool ProgressionHalted;
        public bool IsTreating;
        public float TreatmentHoursRemaining;
        public string ActiveTreatmentRecipeId;

        public static ActiveAffliction Create(AfflictionSO def)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            return new ActiveAffliction
            {
                AfflictionId = def.id,
                HoursActive = 0f,
                HoursUntilProgression = def.progressionHours > 0f ? def.progressionHours : float.MaxValue,
                ProgressionHalted = false,
                IsTreating = false,
                TreatmentHoursRemaining = 0f,
                ActiveTreatmentRecipeId = null
            };
        }
    }

    [Serializable]
    public class ActiveAfflictionSave
    {
        public string AfflictionId;
        public float HoursActive;
        public float HoursUntilProgression;
        public bool ProgressionHalted;
        public bool IsTreating;
        public float TreatmentHoursRemaining;
        public string ActiveTreatmentRecipeId;
    }

    [Serializable]
    public class SurvivorAfflictionsSave
    {
        public string SurvivorId;
        public ActiveAfflictionSave[] Afflictions;
    }

    [Serializable]
    public class MedicalSystemSave
    {
        public SurvivorAfflictionsSave[] BySurvivor;
    }
}
