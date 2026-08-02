using System.Collections.Generic;
using AtomicWar.Core.Events;
using AtomicWar.Data;
using AtomicWar.Runtime.Inventory;
using AtomicWar.Runtime.Survivors;
using UnityEngine;

namespace AtomicWar.Runtime.Crafting
{
    public class CraftingTask
    {
        public RecipeData Recipe { get; }
        public SurvivorModel AssignedSurvivor { get; }
        public float RemainingHours { get; set; }

        public CraftingTask(RecipeData recipe, SurvivorModel survivor)
        {
            Recipe = recipe;
            AssignedSurvivor = survivor;
            RemainingHours = recipe.CraftingTimeInHours / (survivor?.Data.CraftingSpeedMultiplier ?? 1.0f);
        }
    }

    public struct CraftingCompletedEvent
    {
        public RecipeData Recipe;
        public SurvivorModel Survivor;
    }

    /// <summary>
    /// Pure C# system executing recipe checks, crafting tasks, and inventory updates.
    /// </summary>
    public class CraftingSystem
    {
        private readonly InventorySystem _inventorySystem;
        private readonly List<CraftingTask> _activeTasks = new List<CraftingTask>();

        public IReadOnlyList<CraftingTask> ActiveTasks => _activeTasks;

        public CraftingSystem(InventorySystem inventorySystem)
        {
            _inventorySystem = inventorySystem;
        }

        public bool CanCraft(RecipeData recipe)
        {
            if (recipe == null) return false;
            foreach (var ing in recipe.Ingredients)
            {
                if (!_inventorySystem.HasItemAmount(ing.Item.Id, ing.Amount))
                    return false;
            }
            return true;
        }

        public bool StartCrafting(RecipeData recipe, SurvivorModel survivor)
        {
            if (!CanCraft(recipe)) return false;

            // Consume ingredients upfront
            foreach (var ing in recipe.Ingredients)
            {
                _inventorySystem.RemoveItem(ing.Item, ing.Amount);
            }

            var task = new CraftingTask(recipe, survivor);
            if (survivor != null) survivor.CurrentState = SurvivorState.Crafting;

            _activeTasks.Add(task);
            Debug.Log($"[CraftingSystem] Started crafting: {recipe.RecipeName}");
            return true;
        }

        public void TickCrafting(float hoursPassed)
        {
            for (int i = _activeTasks.Count - 1; i >= 0; i--)
            {
                var task = _activeTasks[i];
                task.RemainingHours -= hoursPassed;

                if (task.RemainingHours <= 0f)
                {
                    _inventorySystem.AddItem(task.Recipe.ResultItem, task.Recipe.ResultAmount);
                    if (task.AssignedSurvivor != null && task.AssignedSurvivor.CurrentState == SurvivorState.Crafting)
                    {
                        task.AssignedSurvivor.CurrentState = SurvivorState.Idle;
                    }

                    EventBus.Raise(new CraftingCompletedEvent
                    {
                        Recipe = task.Recipe,
                        Survivor = task.AssignedSurvivor
                    });

                    Debug.Log($"[CraftingSystem] Completed crafting: {task.Recipe.RecipeName}");
                    _activeTasks.RemoveAt(i);
                }
            }
        }
    }
}
