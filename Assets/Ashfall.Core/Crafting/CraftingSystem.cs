using System;
using System.Collections.Generic;
using Ashfall.Core.Inventory;
using InventoryContainer = Ashfall.Core.Inventory.Inventory;

namespace Ashfall.Core.Crafting
{
    /// <summary>
    /// Engine-agnostic port of Unity's CraftingSystem (craft queue, ingredient
    /// consumption, timed completion with overflow/rollback, station wear).
    /// Operates on the shared Ashfall.Core.Inventory container.
    /// </summary>
    public class CraftingSystem
    {
        public const float StationWearPerCraft = 5f;

        public bool IsPaused { get; set; }

        private readonly InventoryContainer _inventory;
        private readonly List<CraftingStation> _stations = new List<CraftingStation>();
        private readonly List<ActiveCraft> _active = new List<ActiveCraft>();
        private Func<string, bool> _isCraftResultAllowed;
        private Func<int> _getDay;
        private Func<string, Recipe?> _recipeLookup;
        private Func<string, float> _crafterCostMultiplier; // crafterId -> material cost mult
        private Func<string, float> _crafterCraftTimeMultiplier; // crafterId -> duration mult
        private Func<string, bool> _canCraftMoonshine;

        public InventoryContainer OverflowStash { get; set; }

        public event Action<Recipe> OnCraftStarted;
        public event Action<Recipe> OnCraftCompleted;
        public event Action<Recipe, string, int> OnCraftResultOverflow; // recipe, itemId, amount

        public CraftingSystem(InventoryContainer inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }

        public void BindCraftResultGate(Func<string, bool> isResultAllowed)
            => _isCraftResultAllowed = isResultAllowed;

        public void SetDayProvider(Func<int> getDay) => _getDay = getDay;

        public void SetCrafterCostMultiplier(Func<string, float> mult) => _crafterCostMultiplier = mult;
        public void SetCrafterCraftTimeMultiplier(Func<string, float> mult) => _crafterCraftTimeMultiplier = mult;
        public void SetMoonshineGate(Func<string, bool> canCraftMoonshine) => _canCraftMoonshine = canCraftMoonshine;

        public int ActiveCraftCount => _active.Count;
        public IReadOnlyList<ActiveCraft> ActiveCrafts => _active;

        public void AddStation(CraftingStation station)
        {
            if (station != null && !_stations.Contains(station))
                _stations.Add(station);
        }

        public void RemoveStation(CraftingStation station) => _stations.Remove(station);

        public CraftingStation? GetStation(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            for (int i = 0; i < _stations.Count; i++)
                if (_stations[i] != null && _stations[i].id == id) return _stations[i];
            return null;
        }

        public bool CanCraft(Recipe recipe) => CanCraft(recipe, null!);

        public bool CanCraft(Recipe recipe, string crafterId)
        {
            if (recipe == null) return false;

            if (!string.IsNullOrEmpty(recipe.requiredStationId))
            {
                var station = GetStation(recipe.requiredStationId);
                if (station == null || !station.IsOperational) return false;
            }

            if (_isCraftResultAllowed != null
                && recipe.result != null
                && !string.IsNullOrEmpty(recipe.result.id)
                && !_isCraftResultAllowed(recipe.result.id))
            {
                return false;
            }

            float costMult = GetCraftCostMultiplier(crafterId);
            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    if (ingredient == null || ingredient.item == null) return false;
                    int need = ScaleIngredientAmount(ingredient.amount, costMult);
                    if (_inventory.Count(ingredient.item) < need) return false;
                }
            }

            if (recipe.result != null && recipe.resultAmount > 0 && !_inventory.CanAdd(recipe.result, recipe.resultAmount))
                return false;

            return true;
        }

        public bool StartCraft(Recipe recipe, string crafterId = null!)
        {
            if (!CanCraft(recipe, crafterId)) return false;

            if (IsMoonshineRecipe(recipe)
                && _canCraftMoonshine != null
                && !_canCraftMoonshine(crafterId))
            {
                return false;
            }

            float costMult = GetCraftCostMultiplier(crafterId);
            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    int need = ScaleIngredientAmount(ingredient.amount, costMult);
                    _inventory.Remove(ingredient.item, need);
                }
            }

            float duration = recipe.craftingTimeHours;
            if (crafterId != null && _crafterCraftTimeMultiplier != null)
                duration *= _crafterCraftTimeMultiplier(crafterId);

            _active.Add(new ActiveCraft
            {
                Recipe = recipe,
                HoursRemaining = duration,
                CrafterId = crafterId ?? string.Empty
            });
            OnCraftStarted?.Invoke(recipe);
            return true;
        }

        public void Tick(float gameHours)
        {
            if (IsPaused || gameHours <= 0f) return;
            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var craft = _active[i];
                float elapsed = MathfCompat.Min(gameHours, MathfCompat.Max(0f, craft.HoursRemaining));
                craft.HoursRemaining -= elapsed;
                if (craft.HoursRemaining <= 0f)
                {
                    CompleteCraft(craft);
                    _active.RemoveAt(i);
                }
            }
        }

        private void CompleteCraft(ActiveCraft craft)
        {
            if (craft?.Recipe == null) return;
            var recipe = craft.Recipe;
            int day = _getDay != null ? _getDay() : 0;

            var result = recipe.result;
            int amount = recipe.resultAmount;

            if (result != null && amount > 0)
            {
                bool placed = _inventory.Add(result, amount);
                if (!placed)
                {
                    OnCraftResultOverflow?.Invoke(recipe, result.id, amount);
                    if (OverflowStash != null)
                    {
                        OverflowStash.Add(result, amount);
                    }
                    else
                    {
                        RefundIngredients(recipe);
                        var overflowStation = GetStation(recipe.requiredStationId);
                        if (overflowStation != null) overflowStation.Repair(StationWearPerCraft);
                        return;
                    }
                }
            }

            var station = GetStation(recipe.requiredStationId);
            if (station != null)
                station.Degrade(StationWearPerCraft);

            OnCraftCompleted?.Invoke(recipe);
        }

        private void RefundIngredients(Recipe recipe)
        {
            if (recipe?.ingredients == null) return;
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ingredient = recipe.ingredients[i];
                if (ingredient?.item != null)
                    _inventory.Add(ingredient.item, ingredient.amount);
            }
        }

        private float GetCraftCostMultiplier(string crafterId)
        {
            if (crafterId != null && _crafterCostMultiplier != null)
                return _crafterCostMultiplier(crafterId);
            return 1f;
        }

        private static int ScaleIngredientAmount(int amount, float costMult)
        {
            if (costMult <= 1f || amount <= 0) return amount;
            return (int)Math.Ceiling(amount * costMult);
        }

        private static bool IsMedicalCraftResult(Recipe recipe)
        {
            if (recipe?.result == null) return false;
            if (recipe.result.type == ItemType.Medical) return true;
            string id = recipe.result.id;
            return id == "bandage" || id == "morphine" || id == "anti_rad"
                || id == "rad_away" || id == "antibiotics" || id == "iodine_pills";
        }

        public static bool IsMedicalRecipe(Recipe recipe) => IsMedicalCraftResult(recipe);

        private static bool IsMoonshineRecipe(Recipe recipe)
        {
            if (recipe == null) return false;
            if (recipe.id == "recipe_moonshine") return true;
            return recipe.result != null && recipe.result.id == "moonshine";
        }

        // ── Save / Load ────────────────────────────────────────────────

        public CraftingSystemSave CaptureState()
        {
            var crafts = new ActiveCraftSave[_active.Count];
            for (int i = 0; i < _active.Count; i++)
            {
                var c = _active[i];
                crafts[i] = new ActiveCraftSave
                {
                    RecipeId = c.Recipe != null ? c.Recipe.id : string.Empty,
                    HoursRemaining = c.HoursRemaining,
                    CrafterId = c.CrafterId ?? string.Empty
                };
            }
            return new CraftingSystemSave { ActiveCrafts = crafts };
        }

        public void SetRecipeLookup(Func<string, Recipe?> lookup) => _recipeLookup = lookup;

        public void RestoreState(CraftingSystemSave save)
        {
            _active.Clear();
            if (save == null || save.ActiveCrafts == null) return;
            for (int i = 0; i < save.ActiveCrafts.Length; i++)
            {
                var c = save.ActiveCrafts[i];
                if (c == null || string.IsNullOrEmpty(c.RecipeId)) continue;
                var recipe = _recipeLookup != null ? _recipeLookup(c.RecipeId) : null;
                if (recipe == null) continue;
                _active.Add(new ActiveCraft
                {
                    Recipe = recipe,
                    HoursRemaining = c.HoursRemaining,
                    CrafterId = c.CrafterId ?? string.Empty
                });
            }
        }
    }

    /// <summary>Engine-agnostic port of Unity's Recipe ScriptableObject.</summary>
    public class Recipe
    {
        public string id = string.Empty;
        public string recipeName = string.Empty;
        public List<Ingredient> ingredients = new List<Ingredient>();
        public ItemDefinition result;
        public int resultAmount = 1;
        public float craftingTimeHours = 1f;
        public string requiredStationId = string.Empty;
    }

    /// <summary>A quantity of an item required (or produced) by a recipe.</summary>
    public class Ingredient
    {
        public ItemDefinition item;
        public int amount = 1;
    }

    /// <summary>Engine-agnostic port of Unity's CraftingStation.</summary>
    public class CraftingStation
    {
        public string id = string.Empty;
        public string displayName = string.Empty;
        public float condition = 100f;
        public bool IsOperational => condition > 0f;

        public void Degrade(float amount)
        {
            condition = MathfCompat.Max(0f, condition - MathfCompat.Max(0f, amount));
        }

        public void Repair(float amount)
        {
            condition = MathfCompat.Min(100f, condition + MathfCompat.Max(0f, amount));
        }
    }

    /// <summary>An in-progress craft (mirrors Unity's ActiveCraft).</summary>
    public class ActiveCraft
    {
        public Recipe Recipe;
        public float HoursRemaining;
        public string CrafterId = string.Empty;
    }

    public class CraftingSystemSave
    {
        public ActiveCraftSave[] ActiveCrafts = Array.Empty<ActiveCraftSave>();
    }

    public class ActiveCraftSave
    {
        public string RecipeId;
        public float HoursRemaining;
        public string CrafterId;
    }
}
