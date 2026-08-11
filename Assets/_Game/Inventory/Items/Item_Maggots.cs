using System;

namespace AtomicWar._Game.Inventory
{
    [Serializable]
    public class Item_MaggotsState
    {
        public string itemId = "maggots";
        public float cureRate = 1.0f;
        public float horrorDebuff = -25f;
        public float painDebuff = -20f;
        public int stackMax = 5;
        public int currentStack = 0;
    }

    public class Item_Maggots
    {
        /// <summary>
        /// MISC-005: seeded stream so this system's rolls replay identically. The
        /// call site below previously used wall-clock UnityEngine.Random, which made
        /// the same save produce different outcomes on each load.
        /// </summary>
    private static System.Random FallbackRng =>
        AtomicWar._Game.Utilities.SeededRandom.Stream("item_maggots");

        public Item_MaggotsState State { get; private set; }

        public event Action<string, bool, float> OnMaggotsApplied;
        public event Action<string> OnApplicationFailed;

        public Item_Maggots()
        {
            State = new Item_MaggotsState();
        }

        public Item_Maggots(Item_MaggotsState state)
        {
            State = state ?? new Item_MaggotsState();
        }

        public (bool cured, float moraleDelta) Apply(string afflictionId)
        {
            if (string.IsNullOrEmpty(afflictionId))
            {
                OnApplicationFailed?.Invoke("Invalid affliction ID");
                return (false, 0f);
            }

            if (State.currentStack <= 0)
            {
                OnApplicationFailed?.Invoke("No maggots available");
                return (false, 0f);
            }

            bool isValidAffliction = afflictionId == "sepsis" || afflictionId == "necrosis";
            if (!isValidAffliction)
            {
                OnApplicationFailed?.Invoke("Maggots only work on sepsis or necrosis");
                return (false, 0f);
            }

            float moraleDelta = State.horrorDebuff + State.painDebuff;
            bool cured = FallbackRng.NextDouble() <= State.cureRate;

            State.currentStack--;

            OnMaggotsApplied?.Invoke(afflictionId, cured, moraleDelta);

            return (cured, moraleDelta);
        }

        public bool AddToStack(int amount)
        {
            if (State.currentStack + amount > State.stackMax)
            {
                return false;
            }

            State.currentStack += amount;
            return true;
        }

        public int GetStackCount()
        {
            return State.currentStack;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public Item_MaggotsState CaptureState() => State;

        public void RestoreState(Item_MaggotsState saved)
        {
            if (saved == null) return;
            State = saved;
        }

}
}
