using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.Crafting
{
    /// <summary>
    /// Tick-based crafting: validates recipes against an inventory and the available
    /// stations, consumes ingredients on start, runs a craft timer, and produces the
    /// result on completion (wearing the station used). Pause-aware. Raises events on
    /// start and completion. Decoupled from where stations come from -- register them
    /// via AddStation (the composition root owns the station list).
    /// </summary>
    public class CraftingSystem
    {
        /// <summary>Condition lost by a station each time a craft completes there.</summary>
        public const float StationWearPerCraft = 5f;

        private readonly Inventory.Inventory _inventory;
        private readonly List<CraftingStation> _stations = new List<CraftingStation>();
        private readonly List<ActiveCraft> _active = new List<ActiveCraft>();

        /// <summary>When true, Tick advances no crafts (game paused).</summary>
        public bool IsPaused { get; set; }

        /// <summary>Fired when a craft starts (ingredients already consumed).</summary>
        public event Action<Recipe> OnCraftStarted;
        /// <summary>Fired when a craft completes and its result is produced.</summary>
        public event Action<Recipe> OnCraftCompleted;

        public CraftingSystem(Inventory.Inventory inventory)
        {
            _inventory = inventory != null ? inventory : throw new ArgumentNullException(nameof(inventory));
        }

        /// <summary>Number of crafts currently in progress.</summary>
        public int ActiveCraftCount => _active.Count;

        /// <summary>Register a station as available for crafting.</summary>
        public void AddStation(CraftingStation station)
        {
            if (station != null && !_stations.Contains(station))
            {
                _stations.Add(station);
            }
        }

        /// <summary>Unregister a station.</summary>
        public void RemoveStation(CraftingStation station)
        {
            _stations.Remove(station);
        }

        /// <summary>Find a registered station by id, or null.</summary>
        public CraftingStation GetStation(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return null;
            }
            for (int i = 0; i < _stations.Count; i++)
            {
                if (_stations[i] != null && _stations[i].id == id)
                {
                    return _stations[i];
                }
            }
            return null;
        }

        /// <summary>
        /// Whether a recipe can be started right now: required station present and
        /// operational, all ingredients held, and the result can fit the inventory.
        /// </summary>
        public bool CanCraft(Recipe recipe)
        {
            if (recipe == null)
            {
                return false;
            }

            if (!string.IsNullOrEmpty(recipe.requiredStationId))
            {
                var station = GetStation(recipe.requiredStationId);
                if (station == null || !station.IsOperational)
                {
                    return false;
                }
            }

            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    if (ingredient == null || ingredient.item == null)
                    {
                        return false;
                    }
                    if (_inventory.Count(ingredient.item) < ingredient.amount)
                    {
                        return false;
                    }
                }
            }

            if (recipe.result != null && recipe.resultAmount > 0 && !_inventory.CanAdd(recipe.result, recipe.resultAmount))
            {
                return false;
            }

            return true;
        }

        /// <summary>Start crafting: consume the ingredients and queue the craft. False if it can't start.</summary>
        public bool StartCraft(Recipe recipe, AtomicWar._Game.Survivors.Survivor crafter = null)
        {
            if (!CanCraft(recipe))
            {
                return false;
            }

            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    _inventory.Remove(ingredient.item, ingredient.amount);
                }
            }

            float duration = recipe.craftingTimeHours;
            if (crafter != null && crafter.HasDisability("tremors"))
            {
                duration *= 2.0f; // 50% action speed penalty
            }

            _active.Add(new ActiveCraft { Recipe = recipe, HoursRemaining = duration });
            OnCraftStarted?.Invoke(recipe);
            return true;
        }

        /// <summary>Advance in-progress crafts over elapsed game hours; completed crafts produce their result.</summary>
        public void Tick(float gameHours)
        {
            if (IsPaused || gameHours <= 0f)
            {
                return;
            }

            for (int i = _active.Count - 1; i >= 0; i--)
            {
                var craft = _active[i];
                craft.HoursRemaining -= gameHours;
                if (craft.HoursRemaining <= 0f)
                {
                    if (craft.Recipe.result != null && craft.Recipe.resultAmount > 0)
                    {
                        _inventory.Add(craft.Recipe.result, craft.Recipe.resultAmount);
                    }

                    var station = GetStation(craft.Recipe.requiredStationId);
                    if (station != null)
                    {
                        station.Degrade(StationWearPerCraft);
                    }

                    _active.RemoveAt(i);
                    OnCraftCompleted?.Invoke(craft.Recipe);
                }
            }
        }
    }

    /// <summary>An in-progress craft: the recipe and the game-hours left until it completes.</summary>
    [Serializable]
    public class ActiveCraft
    {
        public Recipe Recipe;
        public float HoursRemaining;
    }
}
