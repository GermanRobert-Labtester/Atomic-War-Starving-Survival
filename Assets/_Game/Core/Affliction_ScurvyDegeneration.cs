using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Affliction_ScurvyDegenerationState
    {
        public string survivorId = string.Empty;
        public int daysSinceScurvy = 0;
        public int degenerationThresholdDays = 30;
        public bool isDegenerating = false;
        public bool bleedingFromScars = false;
    }

    public class Affliction_ScurvyDegeneration
    {
        public Affliction_ScurvyDegenerationState State { get; private set; }

        public event Action<string, int> OnScurvyDayTick;
        public event Action<string> OnDegenerationStarted;
        public event Action<string> OnBleedingFromScars;

        public Affliction_ScurvyDegeneration()
        {
            State = new Affliction_ScurvyDegenerationState();
        }

        public Affliction_ScurvyDegeneration(Affliction_ScurvyDegenerationState state)
        {
            State = state ?? new Affliction_ScurvyDegenerationState();
        }

        public void TickDay(int scurvyDurationDays)
        {
            State.daysSinceScurvy = scurvyDurationDays;

            OnScurvyDayTick?.Invoke(State.survivorId, State.daysSinceScurvy);

            CheckDegeneration();
        }

        public void CheckDegeneration()
        {
            if (State.daysSinceScurvy >= State.degenerationThresholdDays && !State.isDegenerating)
            {
                State.isDegenerating = true;
                State.bleedingFromScars = true;

                OnDegenerationStarted?.Invoke(State.survivorId);
                OnBleedingFromScars?.Invoke(State.survivorId);
            }
        }

        public string GetBleedingAfflictionId()
        {
            return State.bleedingFromScars ? "bleeding_from_scurvy_scars" : string.Empty;
        }

        public void Afflict(string survivorId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                return;
            }

            State.survivorId = survivorId;
            State.daysSinceScurvy = 0;
            State.isDegenerating = false;
            State.bleedingFromScars = false;
        }

        public void Cure()
        {
            State.daysSinceScurvy = 0;
            State.isDegenerating = false;
            State.bleedingFromScars = false;
        }

        public bool IsDegenerating()
        {
            return State.isDegenerating;
        }

        public int GetDaysSinceScurvy()
        {
            return State.daysSinceScurvy;
        }
    }
}
