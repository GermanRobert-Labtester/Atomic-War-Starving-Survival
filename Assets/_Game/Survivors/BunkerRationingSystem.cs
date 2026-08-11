using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Survivors
{
    /// <summary>Resource governed by the bunker’s daily ration policy.</summary>
    public enum RationResource
    {
        Food,
        Water
    }

    /// <summary>Daily issue level. Lower levels preserve stores but carry a human cost.</summary>
    public enum RationLevel
    {
        Restricted,
        Standard,
        Full
    }

    /// <summary>
    /// Plain-C# daily ration policy. Storage is accessed through narrow count/remove
    /// delegates supplied by Core, keeping survivor logic independent of Inventory
    /// while preserving atomic stock mutations and their UI/save events.
    /// </summary>
    public sealed class BunkerRationingSystem
    {
        public const float RestrictedUnitsPerSurvivor = 0.5f;
        public const float StandardUnitsPerSurvivor = 1f;
        public const float FullUnitsPerSurvivor = 1.5f;
        public const float RestrictedFoodRestore = 20f;
        public const float StandardFoodRestore = 40f;
        public const float FullFoodRestore = 55f;
        public const float RestrictedWaterRestore = 25f;
        public const float StandardWaterRestore = 50f;
        public const float FullWaterRestore = 70f;
        public const float RestrictedMoraleDelta = -2f;
        public const float FullMoraleDelta = 2f;
        public const float ShortageMoralePenalty = 8f;

        private readonly Func<RationResource, int> _getAvailable;
        private readonly Func<RationResource, int, int> _consume;
        // The cistern stays owned by Shelter/Core. These narrow delegates allow
        // only whole, purified units to join the daily drinking pool without
        // coupling the survivor assembly to WaterStorage.
        private readonly Func<int> _getCleanCisternWater;
        private readonly Func<int, int> _consumeCleanCisternWater;
        private Func<float> _getRationRestoreMultiplier;

        public RationLevel FoodLevel { get; private set; } = RationLevel.Standard;
        public RationLevel WaterLevel { get; private set; } = RationLevel.Standard;
        public int LastAppliedDay { get; private set; }
        public BunkerRationingReport LastReport { get; private set; }

        /// <summary>Raised whenever policy, daily report, or restored state changes.</summary>
        public event Action OnChanged;
        /// <summary>Raised once for each completed daily issue.</summary>
        public event Action<BunkerRationingReport> OnDailyRationsApplied;

        public BunkerRationingSystem(
            Func<RationResource, int> getAvailable,
            Func<RationResource, int, int> consume,
            Func<int> getCleanCisternWater = null,
            Func<int, int> consumeCleanCisternWater = null)
        {
            _getAvailable = getAvailable ?? throw new ArgumentNullException(nameof(getAvailable));
            _consume = consume ?? throw new ArgumentNullException(nameof(consume));
            _getCleanCisternWater = getCleanCisternWater;
            _consumeCleanCisternWater = consumeCleanCisternWater;
        }

        /// <summary>Bind a transient multiplier granted by an active ration-preparation shift.</summary>
        public void SetRationRestoreMultiplierProvider(Func<float> provider)
        {
            _getRationRestoreMultiplier = provider;
            OnChanged?.Invoke();
        }

        /// <summary>Move a single food/water setting one step, clamped to valid policy levels.</summary>
        public bool AdjustLevel(RationResource resource, int direction)
        {
            if (direction == 0) return false;
            RationLevel current = resource == RationResource.Food ? FoodLevel : WaterLevel;
            int nextValue = Mathf.Clamp((int)current + (direction > 0 ? 1 : -1),
                (int)RationLevel.Restricted, (int)RationLevel.Full);
            var next = (RationLevel)nextValue;
            if (next == current) return false;

            if (resource == RationResource.Food) FoodLevel = next;
            else WaterLevel = next;
            OnChanged?.Invoke();
            return true;
        }

        public bool SetLevel(RationResource resource, RationLevel level)
        {
            level = ClampLevel(level);
            RationLevel current = resource == RationResource.Food ? FoodLevel : WaterLevel;
            if (current == level) return false;
            if (resource == RationResource.Food) FoodLevel = level;
            else WaterLevel = level;
            OnChanged?.Invoke();
            return true;
        }

        /// <summary>
        /// Issue the configured food and clean-water allotment once for a day.
        /// Short stock is shared proportionally so a missing unit never silently
        /// favors whichever survivor happened to be iterated first.
        /// </summary>
        public bool ApplyDailyRations(int day, IReadOnlyList<Survivor> survivors, NeedsSystem needsSystem)
        {
            if (day <= 0 || day <= LastAppliedDay || needsSystem == null) return false;

            int survivorCount = CountLivingSurvivors(survivors);
            int foodRequested = RequiredUnits(survivorCount, FoodLevel);
            int waterRequested = RequiredUnits(survivorCount, WaterLevel);
            int foodTaken = ConsumeUpTo(RationResource.Food, foodRequested);
            int waterTaken = ConsumeUpTo(RationResource.Water, waterRequested);
            float foodCoverage = Coverage(foodTaken, foodRequested);
            float waterCoverage = Coverage(waterTaken, waterRequested);
            float moraleDelta = GetMoraleDelta(FoodLevel) * foodCoverage
                + GetMoraleDelta(WaterLevel) * waterCoverage
                - ShortageMoralePenalty * ((1f - foodCoverage) + (1f - waterCoverage)) * 0.5f;

            if (survivors != null)
            {
                for (int i = 0; i < survivors.Count; i++)
                {
                    var survivor = survivors[i];
                    if (survivor == null || !survivor.IsAlive) continue;
                    needsSystem.Modify(survivor, NeedKind.Hunger,
                        -GetEffectiveRestore(RationResource.Food, FoodLevel) * foodCoverage);
                    needsSystem.Modify(survivor, NeedKind.Thirst,
                        -GetEffectiveRestore(RationResource.Water, WaterLevel) * waterCoverage);
                    if (!Mathf.Approximately(moraleDelta, 0f))
                        needsSystem.Modify(survivor, NeedKind.Morale, moraleDelta);
                }
            }

            LastAppliedDay = day;
            LastReport = new BunkerRationingReport
            {
                Day = day,
                SurvivorCount = survivorCount,
                FoodLevel = FoodLevel,
                WaterLevel = WaterLevel,
                FoodRequested = foodRequested,
                FoodIssued = foodTaken,
                WaterRequested = waterRequested,
                WaterIssued = waterTaken,
                FoodCoverage = foodCoverage,
                WaterCoverage = waterCoverage,
                HungerReductionPerSurvivor = GetEffectiveRestore(RationResource.Food, FoodLevel) * foodCoverage,
                ThirstReductionPerSurvivor = GetEffectiveRestore(RationResource.Water, WaterLevel) * waterCoverage,
                MoraleDeltaPerSurvivor = moraleDelta
            };
            OnChanged?.Invoke();
            OnDailyRationsApplied?.Invoke(LastReport);
            return true;
        }

        /// <summary>Build a display-only policy, stock, and next-issue projection.</summary>
        public BunkerRationingSnapshot GetSnapshot(IReadOnlyList<Survivor> survivors)
        {
            int survivorCount = CountLivingSurvivors(survivors);
            int foodRequired = RequiredUnits(survivorCount, FoodLevel);
            int waterRequired = RequiredUnits(survivorCount, WaterLevel);
            int foodOnHand = Mathf.Max(0, _getAvailable(RationResource.Food));
            int inventoryWaterOnHand = Mathf.Max(0, _getAvailable(RationResource.Water));
            int cisternWaterOnHand = GetCleanCisternWater();
            int waterOnHand = inventoryWaterOnHand + cisternWaterOnHand;
            float foodCoverage = Coverage(Mathf.Min(foodOnHand, foodRequired), foodRequired);
            float waterCoverage = Coverage(Mathf.Min(waterOnHand, waterRequired), waterRequired);
            return new BunkerRationingSnapshot
            {
                FoodLevel = FoodLevel,
                WaterLevel = WaterLevel,
                SurvivorCount = survivorCount,
                FoodOnHand = foodOnHand,
                WaterOnHand = waterOnHand,
                InventoryWaterOnHand = inventoryWaterOnHand,
                CleanCisternWaterOnHand = cisternWaterOnHand,
                FoodRequired = foodRequired,
                WaterRequired = waterRequired,
                ProjectedFoodCoverage = foodCoverage,
                ProjectedWaterCoverage = waterCoverage,
                ProjectedHungerReduction = GetEffectiveRestore(RationResource.Food, FoodLevel) * foodCoverage,
                ProjectedThirstReduction = GetEffectiveRestore(RationResource.Water, WaterLevel) * waterCoverage,
                ProjectedMoraleDelta = GetMoraleDelta(FoodLevel) * foodCoverage
                    + GetMoraleDelta(WaterLevel) * waterCoverage
                    - ShortageMoralePenalty * ((1f - foodCoverage) + (1f - waterCoverage)) * 0.5f,
                LastAppliedDay = LastAppliedDay,
                LastReport = LastReport
            };
        }

        public BunkerRationingSave CaptureState()
        {
            return new BunkerRationingSave
            {
                foodLevel = (int)FoodLevel,
                waterLevel = (int)WaterLevel,
                lastAppliedDay = LastAppliedDay
            };
        }

        public void RestoreState(BunkerRationingSave state)
        {
            if (state == null) return;
            FoodLevel = ClampLevel((RationLevel)state.foodLevel);
            WaterLevel = ClampLevel((RationLevel)state.waterLevel);
            LastAppliedDay = Mathf.Max(0, state.lastAppliedDay);
            LastReport = null;
            OnChanged?.Invoke();
        }

        private int ConsumeUpTo(RationResource resource, int requested)
        {
            if (requested <= 0) return 0;
            int available = Mathf.Max(0, _getAvailable(resource));
            if (resource == RationResource.Water)
                available += GetCleanCisternWater();
            if (available <= 0) return 0;
            int target = Mathf.Min(requested, available);
            int taken = Mathf.Clamp(_consume(resource, target), 0, target);
            if (resource != RationResource.Water || taken >= target || _consumeCleanCisternWater == null)
                return taken;

            int fromCistern = Mathf.Clamp(_consumeCleanCisternWater(target - taken), 0, target - taken);
            return taken + fromCistern;
        }

        private int GetCleanCisternWater()
        {
            return _getCleanCisternWater != null ? Mathf.Max(0, _getCleanCisternWater()) : 0;
        }

        private static int CountLivingSurvivors(IReadOnlyList<Survivor> survivors)
        {
            int count = 0;
            if (survivors == null) return count;
            for (int i = 0; i < survivors.Count; i++)
            {
                if (survivors[i] != null && survivors[i].IsAlive)
                    count++;
            }
            return count;
        }

        private static int RequiredUnits(int survivorCount, RationLevel level)
        {
            if (survivorCount <= 0) return 0;
            return Mathf.CeilToInt(survivorCount * GetUnitsPerSurvivor(level));
        }

        private static float Coverage(int issued, int requested)
        {
            return requested <= 0 ? 1f : Mathf.Clamp01(issued / (float)requested);
        }

        public static float GetUnitsPerSurvivor(RationLevel level)
        {
            switch (ClampLevel(level))
            {
                case RationLevel.Restricted: return RestrictedUnitsPerSurvivor;
                case RationLevel.Full: return FullUnitsPerSurvivor;
                default: return StandardUnitsPerSurvivor;
            }
        }

        public static float GetRestore(RationResource resource, RationLevel level)
        {
            bool food = resource == RationResource.Food;
            switch (ClampLevel(level))
            {
                case RationLevel.Restricted: return food ? RestrictedFoodRestore : RestrictedWaterRestore;
                case RationLevel.Full: return food ? FullFoodRestore : FullWaterRestore;
                default: return food ? StandardFoodRestore : StandardWaterRestore;
            }
        }

        private float GetEffectiveRestore(RationResource resource, RationLevel level)
        {
            float multiplier = _getRationRestoreMultiplier != null
                ? _getRationRestoreMultiplier()
                : 1f;
            return GetRestore(resource, level) * Mathf.Clamp(multiplier, 1f, 1.5f);
        }

        public static float GetMoraleDelta(RationLevel level)
        {
            switch (ClampLevel(level))
            {
                case RationLevel.Restricted: return RestrictedMoraleDelta;
                case RationLevel.Full: return FullMoraleDelta;
                default: return 0f;
            }
        }

        private static RationLevel ClampLevel(RationLevel level)
        {
            return (RationLevel)Mathf.Clamp((int)level, (int)RationLevel.Restricted, (int)RationLevel.Full);
        }
    }

    /// <summary>Serializable policy state; stock remains in Inventory’s existing save.</summary>
    [Serializable]
    public sealed class BunkerRationingSave
    {
        public string systemId = "bunker_rationing";
        public int foodLevel = (int)RationLevel.Standard;
        public int waterLevel = (int)RationLevel.Standard;
        public int lastAppliedDay;
    }

    /// <summary>Immutable-style data transfer object for the ration terminal.</summary>
    [Serializable]
    public sealed class BunkerRationingSnapshot
    {
        public RationLevel FoodLevel;
        public RationLevel WaterLevel;
        public int SurvivorCount;
        public int FoodOnHand;
        public int WaterOnHand;
        public int InventoryWaterOnHand;
        public int CleanCisternWaterOnHand;
        public int FoodRequired;
        public int WaterRequired;
        public float ProjectedFoodCoverage;
        public float ProjectedWaterCoverage;
        public float ProjectedHungerReduction;
        public float ProjectedThirstReduction;
        public float ProjectedMoraleDelta;
        public int LastAppliedDay;
        public BunkerRationingReport LastReport;
    }

    /// <summary>Record of a resolved daily issue for HUD feedback and event consumers.</summary>
    [Serializable]
    public sealed class BunkerRationingReport
    {
        public int Day;
        public int SurvivorCount;
        public RationLevel FoodLevel;
        public RationLevel WaterLevel;
        public int FoodRequested;
        public int FoodIssued;
        public int WaterRequested;
        public int WaterIssued;
        public float FoodCoverage;
        public float WaterCoverage;
        public float HungerReductionPerSurvivor;
        public float ThirstReductionPerSurvivor;
        public float MoraleDeltaPerSurvivor;
    }
}
