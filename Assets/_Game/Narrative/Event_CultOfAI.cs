using System;

namespace AtomicWar._Game.Narrative
{
    /// <summary>
    /// Event — Cult of AI (Prompt #595). When the AI Core achieves near-perfect
    /// automation efficiency, civilian survivors begin worshipping it. They
    /// sacrifice food to the servers each day. Attempting to shut down the Core
    /// while the cult is active triggers mutiny if cultists outnumber guards.
    /// Save/load safe. Plain C#.
    /// </summary>
    public class Event_CultOfAI
    {
        public const string EventId = "event_cult_of_ai";
        public const float ActivationEfficiencyThreshold = 0.95f;
        public const int MinCiviliansForCult = 3;
        public const int DefaultFoodSacrificedPerDay = 5;

        // -- Runtime state --
        public bool IsActive { get; private set; }
        public int CultistCount { get; private set; }
        public int FoodSacrificedPerDay { get; private set; } = DefaultFoodSacrificedPerDay;
        public float AiCoreEfficiency { get; private set; }
        public bool MutinyTriggered { get; private set; }

        // -- Events --
        public event Action<int> OnCultFormed;          // cultistCount
        public event Action<int> OnFoodSacrificed;      // amount
        public event Action OnMutinyTriggered;
        public event Action OnCultDisbanded;

        public Event_CultOfAI() { }

        /// <summary>
        /// Check whether conditions are met for the cult to form. Activates
        /// when AI efficiency exceeds 95 % and there are more than 2 civilians.
        /// </summary>
        public void CheckActivation(float aiEfficiency, int civilianCount)
        {
            AiCoreEfficiency = aiEfficiency;

            if (IsActive || MutinyTriggered) return;

            if (aiEfficiency > ActivationEfficiencyThreshold && civilianCount >= MinCiviliansForCult)
            {
                IsActive = true;
                CultistCount = civilianCount;
                OnCultFormed?.Invoke(CultistCount);
            }
        }

        /// <summary>
        /// Called once per game-day. Deducts sacrificed food from the supply.
        /// </summary>
        public void TickDay(ref int foodSupply)
        {
            if (!IsActive) return;

            int sacrifice = Math.Min(FoodSacrificedPerDay, foodSupply);
            foodSupply -= sacrifice;
            OnFoodSacrificed?.Invoke(sacrifice);
        }

        /// <summary>
        /// Attempt to shut down the AI Core. If cultists outnumber the guards
        /// a mutiny is triggered. Returns true if shutdown succeeds (no mutiny).
        /// </summary>
        public bool TryShutdownCore(int guardCount, Random rng)
        {
            if (!IsActive) return true; // nothing to shut down

            if (CultistCount > guardCount)
            {
                MutinyTriggered = true;
                OnMutinyTriggered?.Invoke();
                return false;
            }

            // Guards suppress the cult; core shuts down safely.
            IsActive = false;
            CultistCount = 0;
            OnCultDisbanded?.Invoke();
            return true;
        }

        // -----------------------------------------------------------------
        // Save / Load
        // -----------------------------------------------------------------

        public CultOfAISave CaptureState()
        {
            return new CultOfAISave
            {
                IsActive = IsActive,
                CultistCount = CultistCount,
                FoodSacrificedPerDay = FoodSacrificedPerDay,
                AiCoreEfficiency = AiCoreEfficiency,
                MutinyTriggered = MutinyTriggered
            };
        }

        public void RestoreState(CultOfAISave save)
        {
            if (save == null) return;
            IsActive = save.IsActive;
            CultistCount = save.CultistCount;
            FoodSacrificedPerDay = save.FoodSacrificedPerDay;
            AiCoreEfficiency = save.AiCoreEfficiency;
            MutinyTriggered = save.MutinyTriggered;
        }
    }

    [Serializable]
    public class CultOfAISave
    {
        public bool IsActive;
        public int CultistCount;
        public int FoodSacrificedPerDay;
        public float AiCoreEfficiency;
        public bool MutinyTriggered;
    }
}
