using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class ShelterModule_DialysisState
    {
        public string moduleId = "dialysis";
        public float treatmentHours = 72f;
        public float hoursRemaining = 0f;
        public float waterConsumedPerHour = 5f;
        public bool isTreating = false;
        public string patientSurvivorId = string.Empty;
    }

    public class ShelterModule_Dialysis
    {
        public ShelterModule_DialysisState State { get; private set; }

        public event Action<string, float> OnTreatmentStarted;
        public event Action<string, bool> OnTreatmentTick;
        public event Action<string> OnTreatmentCompleted;
        public event Action<string> OnTreatmentFailed;

        public ShelterModule_Dialysis()
        {
            State = new ShelterModule_DialysisState();
        }

        public ShelterModule_Dialysis(ShelterModule_DialysisState state)
        {
            State = state ?? new ShelterModule_DialysisState();
        }

        public bool StartTreatment(string survivorId, int cleanWaterAvailable)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                return false;
            }

            if (State.isTreating)
            {
                return false;
            }

            float totalWaterNeeded = State.treatmentHours * State.waterConsumedPerHour;
            if (cleanWaterAvailable < totalWaterNeeded)
            {
                OnTreatmentFailed?.Invoke("Insufficient clean water");
                return false;
            }

            State.patientSurvivorId = survivorId;
            State.isTreating = true;
            State.hoursRemaining = State.treatmentHours;

            OnTreatmentStarted?.Invoke(survivorId, State.treatmentHours);

            return true;
        }

        public bool TickHour(ref int cleanWater)
        {
            if (!State.isTreating)
            {
                return false;
            }

            int waterNeededThisHour = (int)Math.Ceiling(State.waterConsumedPerHour);
            if (cleanWater < waterNeededThisHour)
            {
                State.isTreating = false;
                State.hoursRemaining = 0f;
                string failedId = State.patientSurvivorId;
                State.patientSurvivorId = string.Empty;
                OnTreatmentFailed?.Invoke(failedId);
                return false;
            }

            cleanWater -= waterNeededThisHour;
            State.hoursRemaining--;

            bool isComplete = State.hoursRemaining <= 0f;

            OnTreatmentTick?.Invoke(State.patientSurvivorId, isComplete);

            if (isComplete)
            {
                string completedId = State.patientSurvivorId;
                State.isTreating = false;
                State.hoursRemaining = 0f;
                State.patientSurvivorId = string.Empty;
                OnTreatmentCompleted?.Invoke(completedId);
            }

            return isComplete;
        }

        public bool IsTreating()
        {
            return State.isTreating;
        }

        public float GetHoursRemaining()
        {
            return State.hoursRemaining;
        }

        public string GetPatientId()
        {
            return State.patientSurvivorId;
        }
    
        public ShelterModule_DialysisState CaptureState()
        {
            return State;
        }

        public void RestoreState(ShelterModule_DialysisState saved)
        {
            State = saved ?? new ShelterModule_DialysisState();
        }
    }
}

