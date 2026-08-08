#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor tooling that imports JSON data from StreamingAssets/Data/ into
    /// ScriptableObject assets under Assets/_Game/Data/Generated/.
    /// Validates schema, enforces referential integrity, and is idempotent.
    /// </summary>
    public static class JsonDataImporter
    {
        const string DataRoot   = "Assets/StreamingAssets/Data";
        const string OutputRoot = "Assets/_Game/Data/Generated";

        static readonly Regex SnakeCaseRe = new Regex(@"^[a-z][a-z0-9_]*$", RegexOptions.Compiled);

        // -------------------------------------------------------------------
        // Menu items
        // -------------------------------------------------------------------

        [MenuItem("Tools/ASHFALL/Import All Data (JSON -> ScriptableObjects)")]
        public static void ImportAllMenu()
        {
            var errors = ImportAll();
            if (errors.Count == 0)
                Debug.Log("[ASHFALL] Import complete -- zero errors.");
            else
                Debug.LogError($"[ASHFALL] Import FAILED with {errors.Count} error(s):\n" +
                               string.Join("\n", errors));
        }

        [MenuItem("Tools/ASHFALL/Validate Data (no import)")]
        public static void ValidateDataMenu()
        {
            var errors = ValidateAll();
            if (errors.Count == 0)
                Debug.Log("[ASHFALL] Validation passed -- zero errors.");
            else
                Debug.LogError($"[ASHFALL] Validation FAILED with {errors.Count} error(s):\n" +
                               string.Join("\n", errors));
        }

        // -------------------------------------------------------------------
        // CI-friendly entry points (return error list)
        // -------------------------------------------------------------------

        /// <summary>Import all JSON data files. Returns empty list on success.</summary>
        public static List<string> ImportAll()
        {
            var errors = new List<string>();

            // Phase 1: parse + schema validate every file
            var items      = LoadAndValidate<ItemJson>(Path.Combine(DataRoot, "items.json"),      "item",      errors);
            var recipes    = LoadAndValidate<RecipeJson>(Path.Combine(DataRoot, "recipes.json"),    "recipe",    errors);
            var survivors  = LoadAndValidate<SurvivorJson>(Path.Combine(DataRoot, "survivors.json"), "survivor",  errors);
            var locations  = LoadAndValidate<LocationJson>(Path.Combine(DataRoot, "locations.json"), "location",  errors);
            var events     = LoadAndValidate<EventJson>(Path.Combine(DataRoot, "events.json"),     "event",     errors);

            // Phase 2: referential integrity
            var itemIds = new HashSet<string>(items.Select(i => i.id));

            foreach (var r in recipes)
            {
                if (r.ingredients != null)
                {
                    foreach (var ing in r.ingredients)
                    {
                        if (!string.IsNullOrEmpty(ing.itemId) && !itemIds.Contains(ing.itemId))
                            errors.Add($"[recipe:{r.id}] ingredient references unknown item '{ing.itemId}'");
                    }
                }
                if (!string.IsNullOrEmpty(r.resultItemId) && !itemIds.Contains(r.resultItemId))
                    errors.Add($"[recipe:{r.id}] resultItemId references unknown item '{r.resultItemId}'");
            }

            // If any errors accumulated, abort import
            if (errors.Count > 0)
                return errors;

            // Phase 3: ensure output directories exist
            EnsureDirectory(Path.Combine(OutputRoot, "Items"));
            EnsureDirectory(Path.Combine(OutputRoot, "Recipes"));
            EnsureDirectory(Path.Combine(OutputRoot, "Survivors"));
            EnsureDirectory(Path.Combine(OutputRoot, "Locations"));
            EnsureDirectory(Path.Combine(OutputRoot, "Events"));

            // Phase 4: import (idempotent -- create or update by id)
            var itemAssets = ImportItems(items);
            ImportRecipes(recipes, itemAssets);
            ImportSurvivors(survivors);
            ImportLocations(locations);
            ImportEvents(events);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            return errors;
        }

        /// <summary>Validate all JSON data without importing. Returns error list.</summary>
        public static List<string> ValidateAll()
        {
            var errors = new List<string>();

            var items      = LoadAndValidate<ItemJson>(Path.Combine(DataRoot, "items.json"),      "item",      errors);
            var recipes    = LoadAndValidate<RecipeJson>(Path.Combine(DataRoot, "recipes.json"),    "recipe",    errors);
            var survivors  = LoadAndValidate<SurvivorJson>(Path.Combine(DataRoot, "survivors.json"), "survivor",  errors);
            var locations  = LoadAndValidate<LocationJson>(Path.Combine(DataRoot, "locations.json"), "location",  errors);
            var events     = LoadAndValidate<EventJson>(Path.Combine(DataRoot, "events.json"),     "event",     errors);

            // Referential integrity: recipe ingredient/result ids must exist in items
            var itemIds = new HashSet<string>(items.Select(i => i.id));

            foreach (var r in recipes)
            {
                if (r.ingredients != null)
                {
                    foreach (var ing in r.ingredients)
                    {
                        if (!string.IsNullOrEmpty(ing.itemId) && !itemIds.Contains(ing.itemId))
                            errors.Add($"[recipe:{r.id}] ingredient references unknown item '{ing.itemId}'");
                    }
                }
                if (!string.IsNullOrEmpty(r.resultItemId) && !itemIds.Contains(r.resultItemId))
                    errors.Add($"[recipe:{r.id}] resultItemId references unknown item '{r.resultItemId}'");
            }

            return errors;
        }

        // -------------------------------------------------------------------
        // Per-type import (individual menu items)
        // -------------------------------------------------------------------

        [MenuItem("Tools/ASHFALL/Import Items")]
        public static void ImportItemsMenu()
        {
            var errors = new List<string>();
            var items = LoadAndValidate<ItemJson>(Path.Combine(DataRoot, "items.json"), "item", errors);
            if (errors.Count > 0) { LogErrors(errors); return; }
            EnsureDirectory(Path.Combine(OutputRoot, "Items"));
            ImportItems(items);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ASHFALL] Imported {items.Count} items.");
        }

        [MenuItem("Tools/ASHFALL/Import Recipes")]
        public static void ImportRecipesMenu()
        {
            var errors = new List<string>();
            var recipes = LoadAndValidate<RecipeJson>(Path.Combine(DataRoot, "recipes.json"), "recipe", errors);
            var items = LoadAndValidate<ItemJson>(Path.Combine(DataRoot, "items.json"), "item", errors);
            var itemIds = new HashSet<string>(items.Select(i => i.id));
            foreach (var r in recipes)
            {
                if (r.ingredients != null)
                {
                    foreach (var ing in r.ingredients)
                    {
                        if (!string.IsNullOrEmpty(ing.itemId) && !itemIds.Contains(ing.itemId))
                            errors.Add($"[recipe:{r.id}] ingredient references unknown item '{ing.itemId}'");
                    }
                }
            }
            if (errors.Count > 0) { LogErrors(errors); return; }
            EnsureDirectory(Path.Combine(OutputRoot, "Recipes"));
            var itemAssets = ImportItems(items);
            ImportRecipes(recipes, itemAssets);
            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ASHFALL] Imported {recipes.Count} recipes.");
        }

        [MenuItem("Tools/ASHFALL/Validate Data Ids (snake_case)")]
        public static void ValidateIds()
        {
            var errors = new List<string>();

            var items     = LoadAndValidate<ItemJson>(Path.Combine(DataRoot, "items.json"),      "item",      errors);
            var recipes   = LoadAndValidate<RecipeJson>(Path.Combine(DataRoot, "recipes.json"),    "recipe",    errors);
            var survivors = LoadAndValidate<SurvivorJson>(Path.Combine(DataRoot, "survivors.json"), "survivor",  errors);
            var locations = LoadAndValidate<LocationJson>(Path.Combine(DataRoot, "locations.json"), "location",  errors);
            var events    = LoadAndValidate<EventJson>(Path.Combine(DataRoot, "events.json"),     "event",     errors);

            // Check all ids are snake_case
            foreach (var i in items)     CheckSnakeCase(i.id, "item", errors);
            foreach (var r in recipes)   CheckSnakeCase(r.id, "recipe", errors);
            foreach (var s in survivors) CheckSnakeCase(s.id, "survivor", errors);
            foreach (var l in locations) CheckSnakeCase(l.id, "location", errors);
            foreach (var e in events)    CheckSnakeCase(e.id, "event", errors);

            // Cross-ref check
            var itemIds = new HashSet<string>(items.Select(i => i.id));
            foreach (var r in recipes)
            {
                if (r.ingredients != null)
                {
                    foreach (var ing in r.ingredients)
                    {
                        if (!string.IsNullOrEmpty(ing.itemId) && !itemIds.Contains(ing.itemId))
                            errors.Add($"[recipe:{r.id}] ingredient references unknown item '{ing.itemId}'");
                    }
                }
            }

            if (errors.Count == 0)
                Debug.Log("[ASHFALL] All ids are valid snake_case and cross-references resolve.");
            else
                LogErrors(errors);
        }

        // -------------------------------------------------------------------
        // JSON DTOs
        // -------------------------------------------------------------------

        [Serializable]
        class ItemJson
        {
            public string id;
            public string displayName;
            public string description;
            public string type;
            public int    stackMax = 1;
            public float  weight;
            public float  radProtection;
            public float  durability;
            public float  contamination;
            public float  hungerRestore;
            public float  thirstRestore;
            public float  healthEffect;
            public float  radCleanse;
            public float  moraleEffect;
            public bool   isEquipable;
            public string equipSlot;
            public float  tradeValue;
            public string tradeTier;
            public bool   empShielded;
            public ScrapYieldJson[] scrapValue;
            public ScrapYieldJson[] repairCosts;
            public float  disassembleYieldFraction = 0.5f;
        }

        [Serializable]
        class ScrapYieldJson
        {
            public string materialId;
            public int    amount = 1;
        }

        [Serializable]
        class IngredientJson
        {
            public string itemId;
            public int    amount = 1;
        }

        [Serializable]
        class RecipeJson
        {
            public string id;
            public string recipeName;
            public IngredientJson[] ingredients;
            public string resultItemId;
            public int    resultAmount = 1;
            public float  craftingTimeHours = 1f;
            public string requiredStationId;
        }

        [Serializable]
        class SurvivorJson
        {
            public string id;
            public string displayName;
            public string profession;
            public string bio;
            public float  baseHealth = 100f;
            /// <summary>Prompt #214 — predetermined latent expert trait id.</summary>
            public string latentExpertTrait;
            /// <summary>Prompt #214 — personal questline id.</summary>
            public string activeQuestlineId;
        }

        [Serializable]
        class LocationJson
        {
            public string id;
            public string displayName;
            public string description;
            public float  dangerLevel;
            public float  travelHours;
            public float  baseRadsPerHour;
        }

        [Serializable]
        class EventJson
        {
            public string id;
            public string title;
            public string bodyText;
            public float  weight = 1f;
            public int    minDay;
        }

        // -------------------------------------------------------------------
        // Loading + schema validation
        // -------------------------------------------------------------------

        static List<T> LoadAndValidate<T>(string relativePath, string typeName, List<string> errors)
        {
            var fullPath = Path.Combine(Application.dataPath, "..", relativePath);
            // Prompt #864 — prefer Active mod-loader override (file stem, e.g. "items")
            // when community mods are loaded; otherwise fall back to base JSON on disk.
            string fileStem = Path.GetFileNameWithoutExtension(relativePath);
            var json = System_ModLoader.ResolveJsonText(fileStem, fullPath);
            if (json == null)
            {
                errors.Add($"[{typeName}] File not found: {relativePath}");
                return new List<T>();
            }

            if (string.IsNullOrWhiteSpace(json) || json.Trim() == "[]")
            {
                errors.Add($"[{typeName}] File is empty: {relativePath}");
                return new List<T>();
            }

            // Wrap in a container for JsonUtility
            string wrapped = WrapJson<T>(json);
            var list = JsonUtility.FromJson<TempList<T>>(wrapped);
            if (list == null || list.items == null)
            {
                // Try direct array parse via container
                errors.Add($"[{typeName}] Failed to parse JSON in {relativePath}");
                return new List<T>();
            }

            var result = list.items;

            // Schema validation per entry
            for (int i = 0; i < result.Count; i++)
            {
                var entry = result[i];
                ValidateEntry(entry, typeName, i, errors);
            }

            // Duplicate id check
            var seen = new HashSet<string>();
            for (int i = 0; i < result.Count; i++)
            {
                var id = GetId(result[i]);
                if (string.IsNullOrEmpty(id))
                    errors.Add($"[{typeName}#{i}] missing id");
                else if (!seen.Add(id))
                    errors.Add($"[{typeName}:{id}] duplicate id at index {i}");
            }

            return result;
        }

        static void ValidateItemJson(ItemJson item, string typeName, string id, List<string> errors)
        {
            if (string.IsNullOrEmpty(item.displayName))
                errors.Add($"[{typeName}:{id}] displayName is null or empty");
            if (!Enum.TryParse<ItemType>(item.type, true, out _))
                errors.Add($"[{typeName}:{id}] invalid type '{item.type}' (valid: {string.Join(", ", Enum.GetNames(typeof(ItemType)))})");
            if (item.stackMax < 1)
                errors.Add($"[{typeName}:{id}] stackMax must be >= 1");
            if (item.contamination < 0f || item.contamination > 1f)
                errors.Add($"[{typeName}:{id}] contamination must be within 0..1");
            if (!EquipSlots.IsCanonicalName(item.equipSlot))
            {
                // Distinguish "known legacy spelling" from "typo": the first is
                // mechanically fixable, the second means the slot was silently lost.
                string canonical = EquipSlots.CanonicalNameForAlias(item.equipSlot);
                errors.Add(canonical != null
                    ? $"[{typeName}:{id}] equipSlot '{item.equipSlot}' is a legacy alias; use '{canonical}'"
                    : $"[{typeName}:{id}] invalid equipSlot '{item.equipSlot}' (valid: {string.Join(", ", EquipSlots.CanonicalNames)})");
            }
        }

        static void ValidateRecipeJson(RecipeJson recipe, string typeName, string id, List<string> errors)
        {
            if (string.IsNullOrEmpty(recipe.recipeName))
                errors.Add($"[{typeName}:{id}] recipeName is null or empty");
            if (recipe.ingredients == null || recipe.ingredients.Length == 0)
                errors.Add($"[{typeName}:{id}] ingredients is null or empty");
            else
            {
                for (int j = 0; j < recipe.ingredients.Length; j++)
                {
                    if (string.IsNullOrEmpty(recipe.ingredients[j].itemId))
                        errors.Add($"[{typeName}:{id}] ingredients[{j}].itemId is null or empty");
                    if (recipe.ingredients[j].amount < 1)
                        errors.Add($"[{typeName}:{id}] ingredients[{j}].amount must be >= 1");
                }
            }
            if (string.IsNullOrEmpty(recipe.resultItemId))
                errors.Add($"[{typeName}:{id}] resultItemId is null or empty");
        }

        static void ValidateSurvivorJson(SurvivorJson survivor, string typeName, string id, List<string> errors)
        {
            if (string.IsNullOrEmpty(survivor.displayName))
                errors.Add($"[{typeName}:{id}] displayName is null or empty");
            if (string.IsNullOrEmpty(survivor.profession))
                errors.Add($"[{typeName}:{id}] profession is null or empty");
            if (survivor.baseHealth <= 0)
                errors.Add($"[{typeName}:{id}] baseHealth must be > 0");
        }

        static void ValidateLocationJson(LocationJson location, string typeName, string id, List<string> errors)
        {
            if (string.IsNullOrEmpty(location.displayName))
                errors.Add($"[{typeName}:{id}] displayName is null or empty");
            if (location.travelHours <= 0)
                errors.Add($"[{typeName}:{id}] travelHours must be > 0");
            if (location.baseRadsPerHour < 0)
                errors.Add($"[{typeName}:{id}] baseRadsPerHour must be >= 0");
        }

        static void ValidateEventJson(EventJson evt, string typeName, string id, List<string> errors)
        {
            if (string.IsNullOrEmpty(evt.title))
                errors.Add($"[{typeName}:{id}] title is null or empty");
            if (string.IsNullOrEmpty(evt.bodyText))
                errors.Add($"[{typeName}:{id}] bodyText is null or empty");
            // weight 0 is valid: scheduled-only / tracker-fired chain steps
            // (must not enter the random pool — see EventRunner.SelectEvent).
            if (evt.weight < 0)
                errors.Add($"[{typeName}:{id}] weight must be >= 0");
        }

        static void ValidateEntry<T>(T entry, string typeName, int index, List<string> errors)
        {
            string id = GetId(entry);

            if (string.IsNullOrEmpty(id))
                errors.Add($"[{typeName}#{index}] id is null or empty");
            else if (!SnakeCaseRe.IsMatch(id))
                errors.Add($"[{typeName}:{id}] id is not snake_case");

            // Type-specific validation
            switch (entry)
            {
                case ItemJson item: ValidateItemJson(item, typeName, id, errors); break;
                case RecipeJson recipe: ValidateRecipeJson(recipe, typeName, id, errors); break;
                case SurvivorJson survivor: ValidateSurvivorJson(survivor, typeName, id, errors); break;
                case LocationJson location: ValidateLocationJson(location, typeName, id, errors); break;
                case EventJson evt: ValidateEventJson(evt, typeName, id, errors); break;
            }
        }

        static string GetId<T>(T entry)
        {
            return entry switch
            {
                ItemJson i     => i.id,
                RecipeJson r   => r.id,
                SurvivorJson s => s.id,
                LocationJson l => l.id,
                EventJson e    => e.id,
                _ => null
            };
        }

        static void CheckSnakeCase(string id, string typeName, List<string> errors)
        {
            if (!SnakeCaseRe.IsMatch(id))
                errors.Add($"[{typeName}:{id}] id is not snake_case");
        }

        // -------------------------------------------------------------------
        // Import helpers
        // -------------------------------------------------------------------

        static Dictionary<string, ItemDefinition> ImportItems(List<ItemJson> items)
        {
            var map = new Dictionary<string, ItemDefinition>();
            foreach (var json in items)
            {
                var so = FindOrCreate<ItemDefinition>(Path.Combine(OutputRoot, "Items"), json.id);
                so.id              = json.id;
                so.displayName     = json.displayName;
                so.description     = json.description;
                so.type            = Enum.Parse<ItemType>(json.type, true);
                so.stackMax        = json.stackMax;
                so.weight          = json.weight;
                so.radProtection   = json.radProtection;
                so.durability      = json.durability;
                so.contamination   = json.contamination;
                so.hungerRestore   = json.hungerRestore;
                so.thirstRestore   = json.thirstRestore;
                so.healthEffect    = json.healthEffect;
                so.radCleanse      = json.radCleanse;
                so.moraleEffect    = json.moraleEffect;
                so.isEquipable     = json.isEquipable;
                so.equipSlot       = EquipSlots.Parse(json.equipSlot);
                so.tradeValue      = json.tradeValue;
                if (!string.IsNullOrEmpty(json.tradeTier) && Enum.TryParse<AtomicWar._Game.Inventory.ItemTradeTier>(json.tradeTier, true, out var parsedTier))
                    so.tradeTier = parsedTier;
                else
                    so.tradeTier = AtomicWar._Game.Inventory.Item_TradeValues.InferTier(so.type);
                so.empShielded     = json.empShielded;
                so.disassembleYieldFraction = json.disassembleYieldFraction > 0f
                    ? Mathf.Clamp01(json.disassembleYieldFraction)
                    : 0.5f;

                so.scrapValue = new System.Collections.Generic.List<AtomicWar._Game.Inventory.ScrapYield>();
                if (json.scrapValue != null)
                {
                    for (int i = 0; i < json.scrapValue.Length; i++)
                    {
                        var row = json.scrapValue[i];
                        if (row == null || string.IsNullOrEmpty(row.materialId) || row.amount <= 0) continue;
                        so.scrapValue.Add(new AtomicWar._Game.Inventory.ScrapYield(row.materialId, row.amount));
                    }
                }

                so.repairRecipe = new AtomicWar._Game.Inventory.RepairRecipe { hours = 0.5f, requiresTools = true };
                if (json.repairCosts != null)
                {
                    for (int i = 0; i < json.repairCosts.Length; i++)
                    {
                        var row = json.repairCosts[i];
                        if (row == null || string.IsNullOrEmpty(row.materialId) || row.amount <= 0) continue;
                        so.repairRecipe.costs.Add(new AtomicWar._Game.Inventory.ScrapYield(row.materialId, row.amount));
                    }
                }

                EditorUtility.SetDirty(so);
                map[json.id] = so;
            }
            return map;
        }

        static void ImportRecipes(List<RecipeJson> recipes, Dictionary<string, ItemDefinition> itemAssets)
        {
            foreach (var json in recipes)
            {
                var so = FindOrCreate<Recipe>(Path.Combine(OutputRoot, "Recipes"), json.id);
                so.id               = json.id;
                so.recipeName       = json.recipeName;
                so.resultAmount     = json.resultAmount;
                so.craftingTimeHours = json.craftingTimeHours;
                so.requiredStationId = json.requiredStationId;

                // Resolve result item ref
                if (itemAssets.TryGetValue(json.resultItemId, out var resultItem))
                    so.result = resultItem;

                // Resolve ingredient refs
                so.ingredients.Clear();
                if (json.ingredients != null)
                {
                    foreach (var ing in json.ingredients)
                    {
                        var ingredient = new Ingredient { amount = ing.amount };
                        if (itemAssets.TryGetValue(ing.itemId, out var itemDef))
                            ingredient.item = itemDef;
                        so.ingredients.Add(ingredient);
                    }
                }

                EditorUtility.SetDirty(so);
            }
        }

        static void ImportSurvivors(List<SurvivorJson> survivors)
        {
            foreach (var json in survivors)
            {
                var so = FindOrCreate<SurvivorArchetypeSO>(Path.Combine(OutputRoot, "Survivors"), json.id);
                so.id           = json.id;
                so.displayName  = json.displayName;
                so.profession   = json.profession;
                so.bio          = json.bio;
                so.baseHealth   = json.baseHealth;
                so.latentExpertTrait = json.latentExpertTrait;
                so.activeQuestlineId = json.activeQuestlineId;
                EditorUtility.SetDirty(so);
            }
        }

        static void ImportLocations(List<LocationJson> locations)
        {
            foreach (var json in locations)
            {
                var so = FindOrCreate<LocationDefinitionSO>(Path.Combine(OutputRoot, "Locations"), json.id);
                so.id              = json.id;
                so.displayName     = json.displayName;
                so.description     = json.description;
                so.dangerLevel     = json.dangerLevel;
                so.travelHours     = json.travelHours;
                so.baseRadsPerHour = json.baseRadsPerHour;
                EditorUtility.SetDirty(so);
            }
        }

        static void ImportEvents(List<EventJson> events)
        {
            foreach (var json in events)
            {
                var so = FindOrCreate<GameEvent>(Path.Combine(OutputRoot, "Events"), json.id);
                so.id       = json.id;
                so.title    = json.title;
                so.bodyText = json.bodyText;
                so.weight   = json.weight;
                so.minDay   = json.minDay;
                EditorUtility.SetDirty(so);
            }
        }

        // -------------------------------------------------------------------
        // Asset management (idempotent: find by id or create)
        // -------------------------------------------------------------------

        static T FindOrCreate<T>(string folder, string id) where T : ScriptableObject
        {
            var assetPath = $"{folder}/{id}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (existing != null)
                return existing;

            var so = ScriptableObject.CreateInstance<T>();
            EnsureDirectory(folder);
            AssetDatabase.CreateAsset(so, assetPath);
            return so;
        }

        static void EnsureDirectory(string relativePath)
        {
            if (AssetDatabase.IsValidFolder(relativePath))
                return;

            var parts = relativePath.Replace("\\", "/").Split('/');
            var current = parts[0]; // "Assets"
            for (int i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }

        // -------------------------------------------------------------------
        // JsonUtility wrapper (needs {"items":[...]} format)
        // -------------------------------------------------------------------

        [Serializable] class TempList<T> { public List<T> items; }

        static string WrapJson<T>(string jsonArray)
        {
            // TempList<T> always has field "items" -- wrap accordingly
            return $"{{\"items\":{jsonArray}}}";
        }

        // -------------------------------------------------------------------
        // Logging
        // -------------------------------------------------------------------

        static void LogErrors(List<string> errors)
        {
            Debug.LogError($"[ASHFALL] {errors.Count} validation error(s):\n" +
                           string.Join("\n", errors));
        }
    }
}
#endif
