#if UNITY_EDITOR
using UnityEditor;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor tooling that imports the JSON data in StreamingAssets/Data into
    /// ScriptableObject catalogs (items, recipes, survivors, locations, events,
    /// radio) and validates that every id is snake_case and present in the master
    /// list. Editor-only; no runtime dependency.
    /// </summary>
    public static class JsonDataImporter
    {
        /// <summary>Import every JSON data file into its ScriptableObject catalog.</summary>
        [MenuItem("Tools/ASHFALL/Import All Data (JSON -> ScriptableObjects)")]
        public static void ImportAll() => throw new System.NotImplementedException();

        /// <summary>Import StreamingAssets/Data/items.json into the item catalog.</summary>
        [MenuItem("Tools/ASHFALL/Import Items")]
        public static void ImportItems() => throw new System.NotImplementedException();

        /// <summary>Import StreamingAssets/Data/recipes.json into the recipe catalog.</summary>
        [MenuItem("Tools/ASHFALL/Import Recipes")]
        public static void ImportRecipes() => throw new System.NotImplementedException();

        /// <summary>Validate that all data ids are snake_case and cross-references resolve.</summary>
        [MenuItem("Tools/ASHFALL/Validate Data Ids (snake_case)")]
        public static void ValidateIds() => throw new System.NotImplementedException();
    }
}
#endif
