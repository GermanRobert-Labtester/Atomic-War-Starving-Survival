using System;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class Item_BoneSawState
    {
        public string itemId = "bone_saw";
        public bool requiresGangrene = true;
        public float traumaInflicted = 50f;
        public bool infectionGuaranteed = true;
        public float hoursRequired = 1f;
        public bool hasBeenUsed = false;
    }

    public class Item_BoneSaw
    {
        public Item_BoneSawState State { get; private set; }

        public event Action<string, string, bool, string> OnAmputationPerformed;
        public event Action<string> OnAmputationFailed;

        public Item_BoneSaw()
        {
            State = new Item_BoneSawState();
        }

        public Item_BoneSaw(Item_BoneSawState state)
        {
            State = state ?? new Item_BoneSawState();
        }

        public (bool survived, string infectionAfflictionId) Amputate(string survivorId, string gangrenousLimbId)
        {
            if (string.IsNullOrEmpty(survivorId))
            {
                OnAmputationFailed?.Invoke("Invalid survivor ID");
                return (false, string.Empty);
            }

            if (string.IsNullOrEmpty(gangrenousLimbId))
            {
                OnAmputationFailed?.Invoke("Invalid gangrenous limb ID");
                return (false, string.Empty);
            }

            if (State.requiresGangrene && !gangrenousLimbId.Contains("gangrene"))
            {
                OnAmputationFailed?.Invoke("Limb must be gangrenous");
                return (false, string.Empty);
            }

            bool survived = true;
            string infectionAfflictionId = State.infectionGuaranteed ? "infection_post_amputation" : string.Empty;

            State.hasBeenUsed = true;

            OnAmputationPerformed?.Invoke(survivorId, gangrenousLimbId, survived, infectionAfflictionId);

            return (survived, infectionAfflictionId);
        }

        public float GetTraumaInflicted()
        {
            return State.traumaInflicted;
        }

        public float GetHoursRequired()
        {
            return State.hoursRequired;
        }

        public bool HasBeenUsed()
        {
            return State.hasBeenUsed;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Item_BoneSawState CaptureState() => State;

        public void RestoreState(Item_BoneSawState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
