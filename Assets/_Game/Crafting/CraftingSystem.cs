using System;
using System.Collections.Generic;
using UnityEngine;
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
        /// <summary>
        /// Optional craft gate: result item id → allowed. Host binds
        /// Item_AmmoTypes.IsWorkbenchCraftAllowed so military AP/API/battle-rifle
        /// exclusives cannot be pressed at the civilian workbench.
        /// </summary>
        private Func<string, bool> _isCraftResultAllowed;

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

        /// <summary>
        /// CRAFT-003 hardened: optional overflow stash for completed crafts that
        /// cannot fit in the main inventory. The host wires this from
        /// GameBootstrap so a successful craft always places its result SOMEWHERE
        /// (a "hot bar", a crate in the bunker, a post office box at the entrance).
        /// Without this, a full inventory silently eats the result. Null is allowed
        /// for tests / partial hosts; in that case the craft rolls back instead
        /// of losing the result.
        /// </summary>
        public Inventory.Inventory OverflowStash { get; set; }

        /// <summary>
        /// CRAFT-003 rollback: return the consumed ingredients to the inventory
        /// when a craft cannot place its result. Used only when OverflowStash is
        /// not wired (the no-stash fallback path).
        /// </summary>
        private void RefundIngredients(Recipe recipe)
        {
            if (recipe?.ingredients == null || _inventory == null) return;
            for (int i = 0; i < recipe.ingredients.Count; i++)
            {
                var ing = recipe.ingredients[i];
                if (ing?.item == null || ing.amount <= 0) continue;
                _inventory.Add(ing.item, ing.amount);
            }
        }

        /// <summary>Prompt #191–#193 — moonshine unlock + high-yield medical crafts.</summary>
        public void BindSurvivalPerks(SurvivalPerkSystem perks, Func<int> getDay = null)
        {
            _survivalPerks = perks;
            _getDay = getDay ?? (() => 0);
        }

        /// <summary>
        /// Gate recipe results (e.g. only civilian ammo loads are workbench-craftable).
        /// Null keeps all results allowed (legacy / tests without ammo catalog).
        /// </summary>
        public void BindCraftResultGate(Func<string, bool> isResultAllowed)
        {
            _isCraftResultAllowed = isResultAllowed;
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

        /// <summary>In-progress crafts (mirrors ExpeditionSystem.ActiveExpeditions).</summary>
        public IReadOnlyList<ActiveCraft> ActiveCrafts => _active;

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
        public bool CanCraft(Recipe recipe) => CanCraft(recipe, crafter: null);

        /// <summary>
        /// Whether ingredients + station allow a craft. Optional crafter applies
        /// #260 Supply Chain Master material cost mult.
        /// </summary>
        public bool CanCraft(Recipe recipe, Survivor crafter)
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

            // Civilian workbench only: reject military/rebel exclusive ammo results.
            if (_isCraftResultAllowed != null
                && recipe.result != null
                && !string.IsNullOrEmpty(recipe.result.id)
                && !_isCraftResultAllowed(recipe.result.id))
            {
                return false;
            }

            float costMult = GetCraftCostMultiplier(crafter);
            if (recipe.ingredients != null)
            {
                for (int i = 0; i < recipe.ingredients.Count; i++)
                {
                    var ingredient = recipe.ingredients[i];
                    if (ingredient == null || ingredient.item == null)
                    {
                        return false;
                    }
                    int need = ScaleIngredientAmount(ingredient.amount, costMult);
                    if (_inventory.Count(ingredient.item) < need)
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
            if (!CanCraft(recipe, crafter))
            {
                return false;
            }

            // Prompt #191 — moonshine requires Wasteland Brewer
            if (IsMoonshineRecipe(recipe)
                && (_survivalPerks == null || !_survivalPerks.CanCraftMoonshine(crafter)))
            {
                return false;
            }

            float costMult = GetCraftCostMultiplier(crafter);
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

            // CRAFT-003 hardened: Add() can fail on a full inventory (capacity, weight,
            // or stack overflow). Pre-fix, the crafted item was silently lost: ingredients
            // had been consumed at craft start, station wear was applied, and the
            // completion events fired — all while the result was dropped on the floor.
            // Now the result is preserved: if the bag can't take it, it sits in the
            // overflow stash for later retrieval. If the stash is unavailable (test
            // host), the craft is rolled back and no events fire — the player keeps
            // their ingredients and station wear.
            if (result != null && amount > 0)
            {
                bool placed = _inventory.Add(result, amount);
                if (!placed)
                {
                    if (OverflowStash != null)
                    {
                        OverflowStash.Add(result, amount);
                        Debug.LogWarning($"[CraftingSystem] '{recipe.id}' craft produced {amount}× '{result.id}' " +
                                         $"but inventory is full. Stashed in overflow for later retrieval.");
                    }
                    else
                    {
                        // Test/host without overflow: refund the ingredients and the
                        // station wear so the craft cleanly failed. This avoids the
                        // silent-loss class of bug.
                        Debug.LogWarning($"[CraftingSystem] '{recipe.id}' craft produced {amount}× '{result.id}' " +
                                         $"but inventory is full and no overflow stash is wired; ingredients refunded.");
                        RefundIngredients(recipe);
                        if (station != null)
                            station.Repair(StationWearPerCraft);
                        return; // skip station wear, skill counter, and event
                    }
                }
            }

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

        // -----------------------------------------------------------------
        // Save / Load (audit wiring fix)
        // Recipes are ScriptableObjects — we save recipe IDs and restore
        // by lookup from the recipe catalog.
        // -----------------------------------------------------------------
        public CraftingSystemSave CaptureState()
        {
            var crafts = new ActiveCraftSave[_active.Count];
            for (int i = 0; i < _active.Count; i++)
            {
                var c = _active[i];
                crafts[i] = new ActiveCraftSave
                {
                    RecipeId = c.Recipe != null ? c.Recipe.id : "",
                    HoursRemaining = c.HoursRemaining,
                    CrafterId = c.Crafter != null ? c.Crafter.Id : ""
                };
            }
            return new CraftingSystemSave { ActiveCrafts = crafts };
        }

        /// <summary>
        /// Restore active crafts. Requires a recipe-lookup function (injected after
        /// construction so CraftingSystem stays agnostic of the catalog). Call
        /// <see cref="SetRecipeLookup"/> before <see cref="RestoreState"/>.
        /// </summary>
        private Func<string, Recipe> _recipeLookup;

        public void SetRecipeLookup(Func<string, Recipe> lookup) => _recipeLookup = lookup;

        /// <summary>
        /// Survivor lookup used to rebind <see cref="ActiveCraft.Crafter"/> on restore.
        /// CrafterId is persisted, but Crafter itself is [NonSerialized]; without this
        /// a craft saved mid-run completes with a null crafter and silently loses the
        /// crafter's Pharmacologist / Alchemist yield perks.
        /// </summary>
        private Func<string, Survivor> _survivorLookup;

        public void SetSurvivorLookup(Func<string, Survivor> lookup) => _survivorLookup = lookup;

        public void RestoreState(CraftingSystemSave save)
        {
            _active.Clear();
            if (save?.ActiveCrafts == null) return;

            // Without a recipe lookup every craft below is dropped. That is a wiring
            // bug (SetRecipeLookup must run before restore), not a content change, and
            // it silently empties the player's craft queue — say so once.
            if (_recipeLookup == null && save.ActiveCrafts.Length > 0)
                Debug.LogWarning(
                    $"[CraftingSystem] Restoring {save.ActiveCrafts.Length} craft(s) with no recipe lookup wired; " +
                    "all of them will be dropped. Call SetRecipeLookup before RestoreState.");

            for (int i = 0; i < save.ActiveCrafts.Length; i++)
            {
                var sc = save.ActiveCrafts[i];
                if (sc == null || string.IsNullOrEmpty(sc.RecipeId)) continue;
                Recipe recipe = _recipeLookup?.Invoke(sc.RecipeId);
                if (recipe == null)
                {
                    // Recipe genuinely gone from the catalog: dropping is correct, but
                    // the player loses queued work, so leave a trace.
                    if (_recipeLookup != null)
                        Debug.LogWarning(
                            $"[CraftingSystem] Dropping active craft '{sc.RecipeId}': recipe not in catalog.");
                    continue;
                }
                _active.Add(new ActiveCraft
                {
                    Recipe = recipe,
                    HoursRemaining = Mathf.Max(0f, sc.HoursRemaining),
                    // Crafter is [NonSerialized]; rebind it from the persisted
                    // CrafterId so completion-time perks survive a save/load.
                    Crafter = string.IsNullOrEmpty(sc.CrafterId)
                        ? null
                        : _survivorLookup?.Invoke(sc.CrafterId)
                });
            }
        }

        /// <summary>#260 Supply Chain Master: 20% fewer materials when crafter has the latent.</summary>
        private float GetCraftCostMultiplier(Survivor crafter)
        {
            if (crafter == null || _personalQuests == null) return 1f;
            return _personalQuests.GetCraftMaterialCostMultiplier(crafter);
        }

        private static int ScaleIngredientAmount(int baseAmount, float costMult)
        {
            if (baseAmount <= 0) return 0;
            if (costMult >= 0.999f) return baseAmount;
            return Mathf.Max(1, Mathf.RoundToInt(baseAmount * costMult));
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

    [Serializable]
    public class CraftingSystemSave
    {
        public ActiveCraftSave[] ActiveCrafts;
    }

    [Serializable]
    public class ActiveCraftSave
    {
        public string RecipeId;
        public float HoursRemaining;
        public string CrafterId;
    }
}
