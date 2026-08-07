using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Action_InfectSelfState
    {
        public string actionId = "infect_self";
        public bool curesRadiationSickness = true;
        public float maxHealthCap = 40f;
        public bool isInfected = false;
        public string infectedSurvivorId = string.Empty;
    }

    public class Action_InfectSelf
    {
        public Action_InfectSelfState State { get; private set; }

        public event Action<string, float, float> OnSelfInfectionApplied;
        public event Action<string> OnInfectionFailed;

        public Action_InfectSelf()
        {
            State = new Action_InfectSelfState();
        }

        public Action_InfectSelf(Action_InfectSelfState state)
        {
            State = state ?? new Action_InfectSelfState();
        }

        public void InfectSelf(string survivorId, ref float currentRadSickness, ref float maxHealth)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                OnInfectionFailed?.Invoke("Invalid survivor ID");
                return;
            }

            if (State.isInfected)
            {
                OnInfectionFailed?.Invoke("Already infected");
                return;
            }

            if (State.curesRadiationSickness)
            {
                currentRadSickness = 0f;
            }

            maxHealth = Math.Min(maxHealth, State.maxHealthCap);

            State.isInfected = true;
            State.infectedSurvivorId = survivorId;

            OnSelfInfectionApplied?.Invoke(survivorId, currentRadSickness, maxHealth);
        }

        public bool IsInfected()
        {
            return State.isInfected;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Action_InfectSelfState CaptureState() => State;

        public void RestoreState(Action_InfectSelfState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
