using System;

namespace AtomicWar._Game.AI.Actions
{
    [Serializable]
    public class Action_SelfSurgeryState
    {
        public string actionId = "self_surgery";
        public float hoursRequired = 4f;
        public float deathChance = 0.50f;
        public bool isAloneRequired = true;
        public bool hasBeenAttempted = false;
        public float hoursElapsed = 0f;
    }

    /// <summary>DEMOTE-Action-remaining — dormant ghost; not Boot/Save wired until a host calls APIs.</summary>
    public class Action_SelfSurgery
    {
        public Action_SelfSurgeryState State { get; private set; }

        public event Action<bool, bool> OnSelfSurgeryCompleted;
        public event Action<string> OnSurgeryFailed;

        public Action_SelfSurgery()
        {
            State = new Action_SelfSurgeryState();
        }

        public Action_SelfSurgery(Action_SelfSurgeryState state)
        {
            State = state ?? new Action_SelfSurgeryState();
        }

        public (bool survived, bool hasTraumaScar) Attempt(bool isAlone, System.Random rng)
        {
            if (State.isAloneRequired && !isAlone)
            {
                OnSurgeryFailed?.Invoke("Must be alone to attempt self-surgery");
                return (false, false);
            }

            if (rng == null)
            {
                OnSurgeryFailed?.Invoke("Random number generator required");
                return (false, false);
            }

            double roll = rng.NextDouble();
            bool survived = roll > State.deathChance;
            bool hasTraumaScar = survived;

            State.hasBeenAttempted = true;
            State.hoursElapsed = State.hoursRequired;

            OnSelfSurgeryCompleted?.Invoke(survived, hasTraumaScar);

            return (survived, hasTraumaScar);
        }

        public float GetHoursRequired()
        {
            return State.hoursRequired;
        }

        public bool HasBeenAttempted()
        {
            return State.hasBeenAttempted;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_SelfSurgeryState CaptureState() => State;

        public void RestoreState(Action_SelfSurgeryState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
