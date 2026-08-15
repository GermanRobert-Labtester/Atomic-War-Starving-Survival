using System;
using System.Collections.Generic;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Survivors
{
    /// <summary>
    /// Hoarding Behavior System (#44) — survivors who survived severe
    /// starvation gain a permanent compulsion to hoard food rations in
    /// their personal quarters, reducing shared pantry counts unless
    /// discovered during room inspections.
    ///
    /// Owns: Survivor.HasHoardingCompulsion, Survivor.HiddenFoodCount,
    /// Survivor.HoardWasDiscovered.
    /// </summary>
    public class HoardingBehaviorSystem
    {
        public const int StarvationDaysToTrigger = 5;
        public const float HoardChancePerDay = 0.30f;
        public const int MaxHiddenFood = 10;
        public const int FoodPerHoard = 1;
        public const float DiscoveryMoraleHit = -10f;
        public const float DiscoveryTrustPenalty = -15f;

        public event Action<Survivor> OnHoardingStarted;
        public event Action<Survivor, int> OnFoodHidden;
        // sv, count
        public event Action<Survivor, int> OnHoardDiscovered;
        // sv, totalHidden

        public Func<Survivor, int> GetDaysStarved;
        public Func<string, int, bool> TryRemoveFromPantry;
        // itemId, count → bool success
        public Action<Survivor, float> ApplyMoraleDelta;
        public Action<string, string, float> AdjustAffinity;
        public System.Random Rng;

        public void TickDaily(IReadOnlyList<Survivor> survivors, int currentDay)
        {
            if (survivors == null) return;
            for (int i = 0; i < survivors.Count; i++)
            {
                var sv = survivors[i];
                if (sv == null || !sv.IsAlive) continue;

                int daysStarved = GetDaysStarved?.Invoke(sv) ?? 0;

                // Trigger hoarding compulsion if starved enough
                if (daysStarved >= StarvationDaysToTrigger && !sv.HasHoardingCompulsion)
                {
                    sv.HasHoardingCompulsion = true;
                    OnHoardingStarted?.Invoke(sv);
                }

                // Hoard food if compulsive
                if (sv.HasHoardingCompulsion && sv.HiddenFoodCount < MaxHiddenFood)
                {
                    if ((Rng?.NextDouble() ?? 0.5) < HoardChancePerDay)
                    {
                        bool taken = TryRemoveFromPantry?.Invoke("food_ration",
                            FoodPerHoard) ?? false;
                        if (taken)
                        {
                            sv.HiddenFoodCount += FoodPerHoard;
                            OnFoodHidden?.Invoke(sv, sv.HiddenFoodCount);
                        }
                    }
                }
            }
        }

        public int DiscoverHoard(Survivor sv)
        {
            if (sv == null || !sv.HasHoardingCompulsion) return 0;
            if (sv.HiddenFoodCount <= 0) return 0;

            int found = sv.HiddenFoodCount;
            sv.HiddenFoodCount = 0;
            sv.HoardWasDiscovered = true;

            ApplyMoraleDelta?.Invoke(sv, DiscoveryMoraleHit);
            OnHoardDiscovered?.Invoke(sv, found);

            return found;
        }
    }
}
