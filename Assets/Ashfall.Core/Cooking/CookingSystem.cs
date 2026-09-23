// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;
using Ashfall.Core.Inventory;

namespace Ashfall.Core.Cooking
{
    public enum FoodQuality
    {
        Raw,
        Cooked,
        WellCooked,
        Burnt
    }

    public struct CookingCensus
    {
        public readonly int ActiveOperationsCount;
        public readonly int CompletedOperationsCount;
        public readonly int TotalMealsPrepared;
        public readonly float CookingSkillLevel;
        public readonly float TotalFoodDecontaminated;
        public readonly int DiscoveredRecipesCount;

        public CookingCensus(
            int activeOps,
            int completedOps,
            int meals,
            float skill,
            float decontaminated,
            int discovered)
        {
            ActiveOperationsCount = activeOps;
            CompletedOperationsCount = completedOps;
            TotalMealsPrepared = meals;
            CookingSkillLevel = skill;
            TotalFoodDecontaminated = decontaminated;
            DiscoveredRecipesCount = discovered;
        }
    }

    [Serializable]
    public sealed class RecipeIngredient
    {
        public string itemId { get; set; } = string.Empty;
        public int quantity { get; set; } = 1;
    }

    [Serializable]
    public sealed class CookingRecipe
    {
        public string id { get; set; } = string.Empty;
        public string displayName { get; set; } = string.Empty;
        public string description { get; set; } = string.Empty;
        public string requiredEquipment { get; set; } = "improvised_stove";
        public float cookTimeMinutes { get; set; } = 20f;
        public float nutritionValue { get; set; } = 30f;
        public float radiationRemoval { get; set; } = 0.5f; // 0..1 fraction removed
        public float shelfLifeDays { get; set; } = 5f;
        public float moraleBonus { get; set; } = 3f;
        public List<RecipeIngredient> inputItems { get; set; } = new List<RecipeIngredient>();
        public string outputItemId { get; set; } = string.Empty;
        public int outputQuantity { get; set; } = 1;
    }

    [Serializable]
    public sealed class CookingOperation
    {
        public string operationId { get; set; } = string.Empty;
        public string recipeId { get; set; } = string.Empty;
        public string assignedCookId { get; set; } = string.Empty;
        public float startMinute { get; set; }
        public float progressMinutes { get; set; }
        public float totalMinutesRequired { get; set; }
        public string equipmentType { get; set; } = "improvised_stove";
        public bool isCompleted { get; set; }
        public bool isCancelled { get; set; }
        public FoodQuality foodQuality { get; set; } = FoodQuality.Cooked;
        public float radiationRemainingFraction { get; set; } = 0.5f;
        public string outputItemId { get; set; } = string.Empty;
        public int outputQuantity { get; set; } = 1;
        public float finalNutritionValue { get; set; }
        public float finalMoraleBonus { get; set; }
    }

    [Serializable]
    public sealed class CookingState
    {
        public int schema_version { get; set; } = 1;
        public List<CookingOperation> activeOperations { get; set; } = new List<CookingOperation>();
        public List<CookingOperation> completedOperations { get; set; } = new List<CookingOperation>();
        public int totalMealsPrepared { get; set; }
        public float cookingSkillLevel { get; set; } // 0..100
        public float totalFoodDecontaminated { get; set; }
        public List<string> discoveredRecipeIds { get; set; } = new List<string>();
    }

    /// <summary>
    /// Source interface allowing CookingSystem to inspect, consume, and deliver items to an inventory or pantry.
    /// </summary>
    public interface ICookingSource
    {
        bool HasIngredients(IEnumerable<RecipeIngredient> ingredients);
        bool TryConsumeIngredients(IEnumerable<RecipeIngredient> ingredients);
        bool DeliverCookedFood(string itemId, int quantity, float radiationFraction, float nutritionValue);
    }

    /// <summary>
    /// Default ICookingSource adapter bridging CookingSystem directly to Ashfall.Core.Inventory without shadow stores.
    /// </summary>
    public sealed class InventoryCookingSource : ICookingSource
    {
        private readonly Inventory.Inventory _inventory;

        public InventoryCookingSource(Inventory.Inventory inventory)
        {
            _inventory = inventory ?? throw new ArgumentNullException(nameof(inventory));
        }

        public bool HasIngredients(IEnumerable<RecipeIngredient> ingredients)
        {
            if (ingredients == null) return true;
            foreach (var ing in ingredients)
            {
                if (string.IsNullOrEmpty(ing.itemId) || ing.quantity <= 0) continue;
                if (!_inventory.HasSufficient(ing.itemId, ing.quantity))
                    return false;
            }
            return true;
        }

        public bool TryConsumeIngredients(IEnumerable<RecipeIngredient> ingredients)
        {
            if (!HasIngredients(ingredients)) return false;
            if (ingredients == null) return true;
            foreach (var ing in ingredients)
            {
                if (string.IsNullOrEmpty(ing.itemId) || ing.quantity <= 0) continue;
                if (!_inventory.TryConsume(ing.itemId, ing.quantity))
                    return false;
            }
            return true;
        }

        public bool DeliverCookedFood(string itemId, int quantity, float radiationFraction, float nutritionValue)
        {
            if (string.IsNullOrEmpty(itemId) || quantity <= 0) return true;
            return _inventory.AddById(itemId, quantity);
        }
    }

    /// <summary>
    /// Pure domain cooking and food safety subsystem (Plan 136 / DEC-140).
    /// Transforms raw trapped game and cultivated crops into safe, nutritious meals while decontaminating radioactive meat.
    /// </summary>
    public sealed class CookingSystem
    {
        public const string SystemId = "cooking_system";
        public const string DefaultCatalogFileName = "recipes_cooking.json";

        private readonly Dictionary<string, CookingRecipe> _recipes = new Dictionary<string, CookingRecipe>(StringComparer.OrdinalIgnoreCase);
        private readonly ISeededRng? _rng;
        private CookingState _state;

        public Action<CookingOperation>? OnCookingCompletedSeam { get; set; }
        public Action<float>? OnSkillLevelUpSeam { get; set; }
        public Action<string>? OnRecipeDiscoveredSeam { get; set; }

        public CookingSystem(CookingState? state = null, ISeededRng? rng = null)
        {
            _state = state ?? new CookingState();
            _rng = rng;
        }

        public CookingState State => _state;
        public IReadOnlyCollection<CookingRecipe> Recipes => _recipes.Values;

        public CookingCensus GetCensus()
        {
            return new CookingCensus(
                _state.activeOperations.Count,
                _state.completedOperations.Count,
                _state.totalMealsPrepared,
                _state.cookingSkillLevel,
                _state.totalFoodDecontaminated,
                _state.discoveredRecipeIds.Count);
        }

        public void BindValidatedRecipes(IEnumerable<CookingRecipe>? recipes)
        {
            if (recipes == null) return;
            foreach (var r in recipes)
            {
                RegisterRecipe(r);
            }
        }

        public void RegisterRecipe(CookingRecipe recipe)
        {
            if (recipe == null || string.IsNullOrWhiteSpace(recipe.id)) return;
            _recipes[recipe.id] = recipe;
            if (!_state.discoveredRecipeIds.Contains(recipe.id))
            {
                _state.discoveredRecipeIds.Add(recipe.id);
                OnRecipeDiscoveredSeam?.Invoke(recipe.id);
            }
        }

        public bool TryGetRecipe(string recipeId, out CookingRecipe? recipe)
        {
            return _recipes.TryGetValue(recipeId, out recipe);
        }

        public void LoadCatalog(string json)
        {
            if (string.IsNullOrWhiteSpace(json)) return;
            try
            {
                var list = CatalogLocator.LoadWrappedList<CookingRecipe>(json, SystemTextJsonSerializer.Options);
                if (list != null)
                {
                    foreach (var r in list)
                    {
                        RegisterRecipe(r);
                    }
                }
            }
            catch
            {
                // Catalog parse fallback
            }
        }

        public static CookingSystem LoadFromDirectory(string dataDir, IFileIO fileIO, ISeededRng? rng = null)
        {
            var system = new CookingSystem(null, rng);
            if (fileIO != null && !string.IsNullOrEmpty(dataDir))
            {
                string path = fileIO.Combine(dataDir, DefaultCatalogFileName);
                if (fileIO.FileExists(path))
                {
                    system.LoadCatalog(fileIO.ReadAllText(path));
                }
            }
            return system;
        }

        /// <summary>
        /// Starts a cooking batch, consuming ingredients from source if provided.
        /// </summary>
        public ActionResult StartCooking(
            string recipeId,
            string cookId = "cook_survivor",
            string equipmentType = "improvised_stove",
            ICookingSource? source = null,
            float currentMinute = 0f)
        {
            if (string.IsNullOrWhiteSpace(recipeId) || !_recipes.TryGetValue(recipeId, out var recipe))
                return ActionResult.Blocked("unknown_recipe", "cooking.unknown_recipe");

            if (source != null && !source.HasIngredients(recipe.inputItems))
                return ActionResult.Blocked("missing_ingredients", "cooking.missing_ingredients");

            if (source != null && !source.TryConsumeIngredients(recipe.inputItems))
                return ActionResult.Blocked("consume_failed", "cooking.consume_failed");

            // Skill modifies cook time (up to 30% reduction at level 100)
            float skillReductionFactor = 1.0f - Math.Min(0.30f, _state.cookingSkillLevel * 0.003f);
            float totalMinutes = Math.Max(5f, recipe.cookTimeMinutes * skillReductionFactor);

            var op = new CookingOperation
            {
                operationId = $"cook_op_{recipe.id}_{_state.totalMealsPrepared + _state.activeOperations.Count + 1}",
                recipeId = recipe.id,
                assignedCookId = cookId ?? string.Empty,
                startMinute = currentMinute,
                progressMinutes = 0f,
                totalMinutesRequired = totalMinutes,
                equipmentType = equipmentType ?? "improvised_stove",
                isCompleted = false,
                isCancelled = false,
                outputItemId = recipe.outputItemId,
                outputQuantity = recipe.outputQuantity
            };

            _state.activeOperations.Add(op);
            return ActionResult.Success("cooking.started", new Dictionary<string, double>
            {
                { "totalMinutes", totalMinutes },
                { "outputQuantity", recipe.outputQuantity }
            });
        }

        /// <summary>
        /// Progresses active cooking operations by deltaMinutes, completing those that reach total required time.
        /// </summary>
        public int ProgressCooking(float deltaMinutes, ICookingSource? destination = null)
        {
            if (deltaMinutes <= 0f) return 0;
            int completedCount = 0;

            var active = _state.activeOperations.ToList();
            foreach (var op in active)
            {
                op.progressMinutes += deltaMinutes;
                if (op.progressMinutes >= op.totalMinutesRequired && !op.isCompleted)
                {
                    CompleteOperation(op, destination);
                    completedCount++;
                }
            }

            return completedCount;
        }

        private void CompleteOperation(CookingOperation op, ICookingSource? destination)
        {
            op.isCompleted = true;
            _state.activeOperations.Remove(op);

            if (_recipes.TryGetValue(op.recipeId, out var recipe))
            {
                // Determine food quality and safety
                // High skill reduces burn chance; critical success yields WellCooked
                double roll = _rng != null ? _rng.NextDouble() : 0.5;
                float burnThreshold = Math.Max(0.01f, 0.15f - (_state.cookingSkillLevel * 0.0015f));
                float wellCookedThreshold = Math.Min(0.95f, 0.70f + (_state.cookingSkillLevel * 0.002f));

                if (roll < burnThreshold)
                {
                    op.foodQuality = FoodQuality.Burnt;
                    op.finalNutritionValue = recipe.nutritionValue * 0.4f;
                    op.finalMoraleBonus = -5.0f;
                    op.radiationRemainingFraction = Math.Max(0.05f, 1.0f - recipe.radiationRemoval);
                }
                else if (roll >= wellCookedThreshold)
                {
                    op.foodQuality = FoodQuality.WellCooked;
                    op.finalNutritionValue = recipe.nutritionValue * 1.25f;
                    op.finalMoraleBonus = recipe.moraleBonus + 3.0f;
                    op.radiationRemainingFraction = Math.Max(0.02f, 1.0f - (recipe.radiationRemoval * 1.2f));
                }
                else
                {
                    op.foodQuality = FoodQuality.Cooked;
                    float skillNutrBonus = 1.0f + Math.Min(0.20f, _state.cookingSkillLevel * 0.002f);
                    op.finalNutritionValue = recipe.nutritionValue * skillNutrBonus;
                    op.finalMoraleBonus = recipe.moraleBonus;
                    op.radiationRemainingFraction = Math.Max(0.05f, 1.0f - recipe.radiationRemoval);
                }

                // Deliver to inventory if destination bound
                destination?.DeliverCookedFood(op.outputItemId, op.outputQuantity, op.radiationRemainingFraction, op.finalNutritionValue);

                // Update skill & metrics
                float decontaminatedUnits = (1.0f - op.radiationRemainingFraction) * op.outputQuantity;
                _state.totalFoodDecontaminated += decontaminatedUnits;
                _state.totalMealsPrepared += op.outputQuantity;

                float prevSkill = _state.cookingSkillLevel;
                _state.cookingSkillLevel = Math.Min(100f, _state.cookingSkillLevel + 1.5f);
                if ((int)_state.cookingSkillLevel > (int)prevSkill)
                {
                    OnSkillLevelUpSeam?.Invoke(_state.cookingSkillLevel);
                }
            }

            _state.completedOperations.Add(op);
            OnCookingCompletedSeam?.Invoke(op);
        }

        public ActionResult CancelCooking(string operationId)
        {
            var op = _state.activeOperations.Find(o => o.operationId == operationId);
            if (op == null)
                return ActionResult.Blocked("not_found", "cooking.not_found");

            op.isCancelled = true;
            _state.activeOperations.Remove(op);
            return ActionResult.Success("cooking.cancelled");
        }

        public CookingState CaptureState()
        {
            return new CookingState
            {
                schema_version = _state.schema_version,
                totalMealsPrepared = _state.totalMealsPrepared,
                cookingSkillLevel = _state.cookingSkillLevel,
                totalFoodDecontaminated = _state.totalFoodDecontaminated,
                activeOperations = _state.activeOperations.Select(CloneOperation).ToList(),
                completedOperations = _state.completedOperations.Select(CloneOperation).ToList(),
                discoveredRecipeIds = new List<string>(_state.discoveredRecipeIds)
            };
        }

        public void RestoreState(CookingState? state)
        {
            if (state == null)
            {
                _state = new CookingState();
                return;
            }
            _state = new CookingState
            {
                schema_version = state.schema_version,
                totalMealsPrepared = state.totalMealsPrepared,
                cookingSkillLevel = state.cookingSkillLevel,
                totalFoodDecontaminated = state.totalFoodDecontaminated,
                activeOperations = state.activeOperations != null ? state.activeOperations.Select(CloneOperation).ToList() : new List<CookingOperation>(),
                completedOperations = state.completedOperations != null ? state.completedOperations.Select(CloneOperation).ToList() : new List<CookingOperation>(),
                discoveredRecipeIds = state.discoveredRecipeIds != null ? new List<string>(state.discoveredRecipeIds) : new List<string>()
            };
        }

        private static CookingOperation CloneOperation(CookingOperation op)
        {
            return new CookingOperation
            {
                operationId = op.operationId,
                recipeId = op.recipeId,
                assignedCookId = op.assignedCookId,
                startMinute = op.startMinute,
                progressMinutes = op.progressMinutes,
                totalMinutesRequired = op.totalMinutesRequired,
                equipmentType = op.equipmentType,
                isCompleted = op.isCompleted,
                isCancelled = op.isCancelled,
                foodQuality = op.foodQuality,
                radiationRemainingFraction = op.radiationRemainingFraction,
                outputItemId = op.outputItemId,
                outputQuantity = op.outputQuantity,
                finalNutritionValue = op.finalNutritionValue,
                finalMoraleBonus = op.finalMoraleBonus
            };
        }
    }
}
