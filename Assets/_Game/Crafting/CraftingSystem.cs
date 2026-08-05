using System;
using System.Collections.Generic;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

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
        private SurvivalPerkSystem _survivalPerks;
        private PersonalQuestSystem _personalQuests;
        private Func<int> _getDay;
        private System.Random _rng = new System.Random(42);

        /// <summary>When true, Tick advances no crafts (game paused).</summary>
        public bool IsPaused { get; set; }

        /// <summary>Fired when a craft starts (ingredients already consumed).</summary>
        public event Action<Recipe> OnCraftStarted;
        /// <summary>Fired when a craft completes and its result is produced.</summary>
        public event Action<Recipe> OnCraftCompleted;
        /// <summary>Fired when a craft completes with the assigned crafter (may be null).</summary>
        public event Action<Recipe, Survivor> OnCraftCompletedBy;

        public CraftingSystem(Inventory.Inventory inventory)
        {
            _inventory = inventory != null ? inventory : throw new ArgumentNullException(nameof(inventory));
        }

        /// <summary>Prompt #191–#193 — moonshine unlock + high-yield medical crafts.</summary>
        public void BindSurvivalPerks(SurvivalPerkSystem perks, Func<int> getDay = null)
        {
            _survivalPerks = perks;
            _getDay = getDay ?? (() => 0);
        }

        /// <summary>Prompt #216 — Alchemist double yield + mold antibiotics.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests, System.Random rng = null)
        {
            _personalQuests = personalQuests;
            if (rng != null) _rng = rng;
        }

        /// <summary>
        /// Prompt #216 — Alchemist can craft antibiotics from mold + dirty water
        /// even without a formal recipe (host supplies item defs).
        /// </summary>
        public bool TryCraftAntibioticsFromMold(
            Survivor crafter,
            ItemDefinition mold,
            ItemDefinition dirtyWater,
            ItemDefinition antibiotics)
        {
            if (crafter == null || !crafter.IsAlive) return false;
            if (_personalQuests == null || !_personalQuests.CanCraftAntibioticsFromMold(crafter))
                return false;
            if (mold == null || dirtyWater == null || antibiotics == null) return false;
            if (_inventory.Count(mold) < 1 || _inventory.Count(dirtyWater) < 1) return false;
            if (!_inventory.Remove(mold, 1)) return false;
            if (!_inventory.Remove(dirtyWater, 1))
            {
                _inventory.Add(mold, 1);
                return false;
            }
            int amount = _personalQuests.ApplyAlchemistYield(crafter, 1, _rng);
            _inventory.Add(antibiotics, amount);
            return true;
        }

        /// <summary>
        /// Prompt #229 — Synthesizer crafts AntiRad from pure ChemicalScrap.
        /// </summary>
        public bool TryCraftAntiRadFromChemicalScrap(
            Survivor crafter,
            ItemDefinition chemicalScrap,
            ItemDefinition antiRad)
        {
            if (crafter == null || !crafter.IsAlive) return false;
            if (_personalQuests == null || !_personalQuests.CanCraftAntiRadFromChemicalScrap(crafter))
                return false;
            if (chemicalScrap == null || antiRad == null) return false;
            if (_inventory.Count(chemicalScrap) < 1) return false;
            if (!_inventory.Remove(chemicalScrap, 1)) return false;
            _inventory.Add(antiRad, 1);
            return true;
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
        public bool StartCraft(Recipe recipe, Survivor crafter = null)
        {
            if (!CanCraft(recipe))
            {
                return false;
            }

            // Prompt #191 — moonshine requires Wasteland Brewer
            if (IsMoonshineRecipe(recipe)
                && (_survivalPerks == null || !_survivalPerks.CanCraftMoonshine(crafter)))
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

            // Prompt #193 — high-yield meds craft twice as fast when result is antibiotics/iodine
            if (crafter != null && _survivalPerks != null
                && _survivalPerks.CanProduceHighYieldMeds(crafter)
                && recipe.result != null
                && (SurvivalPerkSystem.IsAntibioticId(recipe.result.id)
                    || SurvivalPerkSystem.IsIodineId(recipe.result.id)))
            {
                duration *= SurvivalPerkSystem.HighYieldTreatmentSpeedMult;
            }

            _active.Add(new ActiveCraft
            {
                Recipe = recipe,
                HoursRemaining = duration,
                Crafter = crafter
            });
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
                    CompleteCraft(craft);
                    _active.RemoveAt(i);
                }
            }
        }

        private void CompleteCraft(ActiveCraft craft)
        {
            if (craft?.Recipe == null) return;

            var recipe = craft.Recipe;
            var crafter = craft.Crafter;
            int day = _getDay != null ? _getDay() : 0;

            ItemDefinition result = recipe.result;
            int amount = recipe.resultAmount;

            // Prompt #193 — Pharmacologist upgrades antibiotics/iodine to high-yield variants
            if (result != null && crafter != null && _survivalPerks != null)
            {
                string resolvedId = _survivalPerks.ResolveMedicalCraftResultId(crafter, result.id);
                if (!string.Equals(resolvedId, result.id, StringComparison.OrdinalIgnoreCase))
                {
                    result = CreateHighYieldItem(result, resolvedId);
                }
            }

            // Prompt #216 — Alchemist: 30% chance to double med craft yield.
            if (result != null && amount > 0 && crafter != null && _personalQuests != null
                && IsMedicalCraftResult(recipe))
            {
                amount = _personalQuests.ApplyAlchemistYield(crafter, amount, _rng);
            }

            if (result != null && amount > 0)
                _inventory.Add(result, amount);

            var station = GetStation(recipe.requiredStationId);
            if (station != null)
                station.Degrade(StationWearPerCraft);

            // Milestone counters
            if (crafter != null && _survivalPerks != null)
            {
                if (IsMedicalCraftResult(recipe))
                    _survivalPerks.RecordMedicalCraft(crafter, 1, day);
            }

            OnCraftCompleted?.Invoke(recipe);
            OnCraftCompletedBy?.Invoke(recipe, crafter);
        }

        private static bool IsMedicalCraftResult(Recipe recipe)
        {
            if (recipe?.result == null) return false;
            if (recipe.result.type == ItemType.Medical) return true;
            string id = recipe.result.id;
            return SurvivalPerkSystem.IsAntibioticId(id)
                   || SurvivalPerkSystem.IsIodineId(id)
                   || string.Equals(id, "bandage", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(id, "morphine", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(id, "anti_rad", StringComparison.OrdinalIgnoreCase)
                   || string.Equals(id, "rad_away", StringComparison.OrdinalIgnoreCase);
        }

        private static bool IsMoonshineRecipe(Recipe recipe)
        {
            if (recipe == null) return false;
            if (string.Equals(recipe.id, "recipe_moonshine", StringComparison.OrdinalIgnoreCase))
                return true;
            return recipe.result != null
                   && string.Equals(recipe.result.id, SurvivalPerkSystem.MoonshineId,
                       StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>Clone base med definition with high-yield id / display name.</summary>
        public static ItemDefinition CreateHighYieldItem(ItemDefinition baseItem, string highYieldId)
        {
            var item = UnityEngine.ScriptableObject.CreateInstance<ItemDefinition>();
            if (baseItem != null)
            {
                item.displayName = "High-Yield " + (baseItem.displayName ?? highYieldId);
                item.description = (baseItem.description ?? "")
                    + " Concentrated. Acts twice as fast. Ignores resistance.";
                item.type = baseItem.type;
                item.stackMax = baseItem.stackMax;
                item.weight = baseItem.weight;
                item.tradeValue = baseItem.tradeValue * 1.5f;
                item.healthEffect = baseItem.healthEffect;
                item.thirstRestore = baseItem.thirstRestore;
                item.hungerRestore = baseItem.hungerRestore;
            }
            else
            {
                item.displayName = highYieldId;
                item.type = ItemType.Medical;
                item.stackMax = 10;
                item.weight = 0.1f;
            }
            item.id = highYieldId;
            return item;
        }
    }

    /// <summary>An in-progress craft: the recipe and the game-hours left until it completes.</summary>
    [Serializable]
    public class ActiveCraft
    {
        public Recipe Recipe;
        public float HoursRemaining;
        [NonSerialized] public Survivor Crafter;
    }
}
