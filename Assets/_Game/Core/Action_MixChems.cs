using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Action_MixChemsState
    {
        public string actionId = "mix_chems";
        public float durationHours = 24f;
        public float cardiacArrestChance = 0.60f;
        public bool isActive = false;
        public float hoursRemaining = 0f;
        public bool hasMorphine = false;
        public bool hasAdrenaline = false;
        public bool hasAntiRad = false;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_MixChems
    {
        public Action_MixChemsState State { get; private set; }

        public event Action<float> OnWonderDrugActivated;
        public event Action<bool> OnCardiacArrestRoll;
        public event Action OnWonderDrugExpired;

        public Action_MixChems()
        {
            State = new Action_MixChemsState();
        }

        public Action_MixChems(Action_MixChemsState state)
        {
            State = state ?? new Action_MixChemsState();
        }

        public bool Mix(bool hasMorphine, bool hasAdrenaline, bool hasAntiRad)
        {
            if (!hasMorphine || !hasAdrenaline || !hasAntiRad)
            {
                return false;
            }

            if (State.isActive)
            {
                return false;
            }

            State.hasMorphine = hasMorphine;
            State.hasAdrenaline = hasAdrenaline;
            State.hasAntiRad = hasAntiRad;
            State.isActive = true;
            State.hoursRemaining = State.durationHours;

            OnWonderDrugActivated?.Invoke(State.durationHours);

            return true;
        }

        public bool TickHour(System.Random rng)
        {
            if (!State.isActive)
            {
                return false;
            }

            if (rng == null)
            {
                return false;
            }

            State.hoursRemaining--;

            double roll = rng.NextDouble();
            bool cardiacArrest = roll < State.cardiacArrestChance;

            OnCardiacArrestRoll?.Invoke(cardiacArrest);

            if (cardiacArrest || State.hoursRemaining <= 0f)
            {
                State.isActive = false;
                State.hoursRemaining = 0f;
                OnWonderDrugExpired?.Invoke();
            }

            return cardiacArrest;
        }

        public bool IsActive()
        {
            return State.isActive;
        }

        public float GetHoursRemaining()
        {
            return State.hoursRemaining;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_MixChemsState CaptureState() => State;

        public void RestoreState(Action_MixChemsState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
