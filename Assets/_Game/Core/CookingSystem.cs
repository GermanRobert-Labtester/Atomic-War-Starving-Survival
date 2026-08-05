using System;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Survivors;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Stove cooking (Prompt #189). Consumes CleanWater (+ optional food ingredients)
    /// to produce a cooked meal. Survivors with Ration Stretcher have a 25% chance
    /// the recipe skips CleanWater entirely.
    /// </summary>
    public class CookingSystem
    {
        public const string StoveStationId = "stove";
        public const string CookedMealId = "cooked_meal";
        public const float CleanWaterPerMeal = 1f;
        public const float CookHours = 0.5f;

        private readonly Inventory.Inventory _inventory;
        private readonly WaterStorage _water;
        private SurvivalPerkSystem _survivalPerks;
        private readonly System.Random _rng;
        private Func<int> _getDay;
        private ItemDefinition _mealDef;
        private ItemDefinition _cleanWaterItem; // optional inventory clean_water packs

        public event Action<Survivor, bool> OnMealCooked; // cook, freeWater
        public event Action OnCookingStateChanged;

        public CookingSystem(
            Inventory.Inventory inventory,
            WaterStorage water = null,
            System.Random rng = null)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
            _water = water;
            _rng = rng ?? new System.Random(189);
        }

        public void BindSurvivalPerks(SurvivalPerkSystem perks, Func<int> getDay = null)
        {
            _survivalPerks = perks;
            _getDay = getDay ?? (() => 0);
        }

        public void SetMealDefinition(ItemDefinition meal) => _mealDef = meal;
        public void SetCleanWaterItem(ItemDefinition cleanWater) => _cleanWaterItem = cleanWater;

        public static ItemDefinition CreateCookedMealDefinition()
        {
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = CookedMealId;
            item.displayName = "Cooked Meal";
            item.description = "Boiled, salted, almost edible. Warmth in a tin.";
            item.type = ItemType.Food;
            item.stackMax = 20;
            item.weight = 0.8f;
            item.hungerRestore = 45f;
            item.moraleEffect = 2f;
            item.tradeValue = 4f;
            return item;
        }

        /// <summary>
        /// Cook one meal at the stove. Consumes CleanWater unless Ration Stretcher
        /// free-water roll succeeds. Returns false if water/capacity insufficient.
        /// </summary>
        public bool CookMeal(Survivor cook, float waterCost = CleanWaterPerMeal)
        {
            if (cook == null || !cook.IsAlive) return false;
            EnsureMealDef();
            if (_mealDef == null || !_inventory.CanAdd(_mealDef, 1)) return false;

            bool skipWater = _survivalPerks != null
                && _survivalPerks.RollSkipCleanWater(cook, _rng);

            if (!skipWater && waterCost > 0f)
            {
                if (!TryConsumeCleanWater(waterCost))
                    return false;
            }

            if (!_inventory.Add(_mealDef, 1))
            {
                // Refund water if we took it
                if (!skipWater && waterCost > 0f && _water != null)
                    _water.AddClean(waterCost);
                return false;
            }

            int day = _getDay != null ? _getDay() : 0;
            _survivalPerks?.RecordMealCooked(cook, day);

            OnMealCooked?.Invoke(cook, skipWater);
            OnCookingStateChanged?.Invoke();
            return true;
        }

        private bool TryConsumeCleanWater(float amount)
        {
            if (_water != null && _water.CleanWater >= amount)
            {
                return _water.ConsumeClean(amount) >= amount - 0.0001f;
            }

            // Fallback: inventory clean_water packs (1 unit each)
            if (_cleanWaterItem != null && _inventory.Count(_cleanWaterItem) >= Mathf.CeilToInt(amount))
                return _inventory.Remove(_cleanWaterItem, Mathf.CeilToInt(amount));

            var slot = _inventory.FindSlot(SurvivalPerkSystem.CleanWaterItemId);
            if (slot?.Item != null && slot.Amount >= Mathf.CeilToInt(amount))
                return _inventory.Remove(slot.Item, Mathf.CeilToInt(amount));

            return false;
        }

        private void EnsureMealDef()
        {
            if (_mealDef != null) return;
            var slot = _inventory.FindSlot(CookedMealId);
            if (slot?.Item != null)
            {
                _mealDef = slot.Item;
                return;
            }
            _mealDef = CreateCookedMealDefinition();
        }
    }
}
