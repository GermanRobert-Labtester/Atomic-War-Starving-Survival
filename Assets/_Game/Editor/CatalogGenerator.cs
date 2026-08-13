#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Survivors;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Builds the catalog and tuning-profile ScriptableObjects that GameBootstrap
    /// requires. JsonDataImporter writes individual assets but never a catalog, so
    /// ten of GameBootstrap's twelve data references had no asset at all.
    ///
    /// JSON-backed catalogs aggregate the imported assets and are refreshed on every
    /// run. Tuning profiles have no JSON source, so they are created once at their
    /// C# defaults and never overwritten -- regeneration must not clobber tuning.
    /// </summary>
    public static class CatalogGenerator
    {
        const string GeneratedRoot = "Assets/_Game/Data/Generated";
        const string CatalogRoot   = GeneratedRoot + "/Catalogs";

        /// <summary>
        /// Command-line / CI batchmode entry point:
        /// -executeMethod AtomicWar._Game.Editor.CatalogGenerator.GenerateAll
        /// </summary>
        [MenuItem("Tools/ASHFALL/Generate Catalogs")]
        public static void GenerateAll()
        {
            EnsureFolder(CatalogRoot);

            var items     = LoadAll<ItemDefinition>(GeneratedRoot + "/Items");
            var recipes   = LoadAll<Recipe>(GeneratedRoot + "/Recipes");
            var events    = LoadAll<GameEvent>(GeneratedRoot + "/Events");
            var locations = LoadAll<LocationDefinitionSO>(GeneratedRoot + "/Locations");
            var radio     = LoadAll<RadioBroadcastSO>(GeneratedRoot + "/Radio");
            var survivors = LoadAll<SurvivorArchetypeSO>(GeneratedRoot + "/Survivors");

            Refresh<ItemCatalogSO>("ItemCatalog",           c => c.items      = items);
            Refresh<RecipeCatalogSO>("RecipeCatalog",       c => c.recipes    = recipes);
            Refresh<GameEventCatalogSO>("GameEventCatalog", c => c.events     = events);
            Refresh<LocationCatalogSO>("LocationCatalog",   c => c.locations  = locations);
            Refresh<RadioCatalogSO>("RadioCatalog",         c => c.broadcasts = radio);
            Refresh<SurvivorCatalogSO>("SurvivorCatalog",   c => c.archetypes = survivors);

            // Tuning profiles: the C# field defaults are the balanced values.
            CreateIfAbsent<NeedsProfile>("NeedsProfile");
            CreateIfAbsent<LightProfile>("LightProfile");
            CreateIfAbsent<SeasonProfile>("SeasonProfile");
            CreateIfAbsent<WorldPhaseConfigSO>("WorldPhaseConfig");

            CreateIfAbsent<LootTableSO>("LootTable", loot =>
            {
                loot.entries = items.Select(i => new LootEntry
                {
                    item = i,
                    weight = 1f,
                    // PreWar is the lowest WorldPhase and GetValidEntries tests
                    // `currentPhase >= phaseRequirement`, so this stays valid in every
                    // phase. LootEntry's own default (CivilWar) would hide all loot
                    // while the campaign is still in PreWar.
                    phaseRequirement = WorldPhase.PreWar
                }).ToList();
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[ASHFALL] Catalogs generated: {items.Count} items, {recipes.Count} recipes, " +
                      $"{events.Count} events, {locations.Count} locations, {radio.Count} broadcasts, " +
                      $"{survivors.Count} survivor archetypes.");
        }

        static void Refresh<T>(string assetName, Action<T> fill) where T : ScriptableObject
        {
            var asset = LoadOrCreate<T>(assetName);
            fill(asset);
            EditorUtility.SetDirty(asset);
        }

        static void CreateIfAbsent<T>(string assetName, Action<T> seed = null) where T : ScriptableObject
        {
            var path = $"{CatalogRoot}/{assetName}.asset";
            if (AssetDatabase.LoadAssetAtPath<T>(path) != null)
                return;

            var asset = ScriptableObject.CreateInstance<T>();
            seed?.Invoke(asset);
            AssetDatabase.CreateAsset(asset, path);
            EditorUtility.SetDirty(asset);
        }

        static T LoadOrCreate<T>(string assetName) where T : ScriptableObject
        {
            var path = $"{CatalogRoot}/{assetName}.asset";
            var existing = AssetDatabase.LoadAssetAtPath<T>(path);
            if (existing != null)
                return existing;

            var asset = ScriptableObject.CreateInstance<T>();
            AssetDatabase.CreateAsset(asset, path);
            return asset;
        }

        static List<T> LoadAll<T>(string folder) where T : ScriptableObject
        {
            if (!AssetDatabase.IsValidFolder(folder))
            {
                Debug.LogWarning($"[ASHFALL] {folder} does not exist -- run Tools/ASHFALL/Import All Data first.");
                return new List<T>();
            }

            return AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(a => a != null)
                .OrderBy(a => a.name, StringComparer.Ordinal) // stable order -> stable YAML diffs
                .ToList();
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parts = path.Split('/');
            var current = parts[0]; // "Assets"
            for (int i = 1; i < parts.Length; i++)
            {
                var next = current + "/" + parts[i];
                if (!AssetDatabase.IsValidFolder(next))
                    AssetDatabase.CreateFolder(current, parts[i]);
                current = next;
            }
        }
    }
}
#endif
