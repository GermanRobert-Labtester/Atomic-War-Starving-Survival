using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Batch import and assign AI-generated sprites to their corresponding
    /// ItemDefinition, LocationDefinitionSO, and SurvivorArchetypeSO assets.
    ///
    /// Run via: Tools → ASHFALL → Import Art Assets → [Category]
    /// </summary>
    public class ArtAssetImporter : EditorWindow
    {
        private const string SpritesRoot = "Assets/_Game/Sprites";
        private const string ItemsPath = "Assets/_Game/Sprites/Items";
        private const string LocationsPath = "Assets/_Game/Sprites/Locations";
        private const string PortraitsPath = "Assets/_Game/Sprites/Portraits";
        private const string FactionsPath = "Assets/_Game/Sprites/Factions";
        private const string WeatherPath = "Assets/_Game/Sprites/Weather";

        private const string ItemsDataPath = "Assets/_Game/Data/Generated/Items";
        private const string LocationsDataPath = "Assets/_Game/Data/Generated/Locations";
        private const string ArchetypesDataPath = "Assets/_Game/Data/Generated/Archetypes";

        [MenuItem("Tools/ASHFALL/Import Art Assets/Set All Item Sprites to Correct Import Settings")]
        public static void FixItemImportSettings()
        {
            FixAllSpritesInDirectory(ItemsPath, 1024, SpriteMeshType.FullRect);
            FixAllSpritesInDirectory(LocationsPath, 2048, SpriteMeshType.FullRect);
            FixAllSpritesInDirectory(PortraitsPath, 1024, SpriteMeshType.FullRect);
            FixAllSpritesInDirectory(FactionsPath, 2048, SpriteMeshType.FullRect);
            FixAllSpritesInDirectory(WeatherPath, 2048, SpriteMeshType.FullRect);
            AssetDatabase.Refresh();
            Debug.Log("[ArtAssetImporter] All sprite import settings fixed.");
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Assign Item Icons from Sprites")]
        public static void AssignItemIcons()
        {
            int assigned = 0;
            int missing = 0;

            var itemGuids = AssetDatabase.FindAssets("t:ItemDefinition", new[] { ItemsDataPath });
            foreach (var guid in itemGuids)
            {
                string itemPath = AssetDatabase.GUIDToAssetPath(guid);
                var itemDef = AssetDatabase.LoadAssetAtPath<AtomicWar._Game.Inventory.ItemDefinition>(itemPath);
                if (itemDef == null) continue;

                string spriteName = string.IsNullOrEmpty(itemDef.id) ? itemDef.name : itemDef.id;
                var sprite = LoadSprite(ItemsPath, spriteName);

                if (sprite != null)
                {
                    var so = new SerializedObject(itemDef);
                    var iconProp = so.FindProperty("iconRef") ?? so.FindProperty("icon");
                    if (iconProp != null)
                    {
                        iconProp.objectReferenceValue = sprite;
                        so.ApplyModifiedProperties();
                        assigned++;
                    }
                    else
                    {
                        missing++;
                    }
                }
                else
                {
                    missing++;
                }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[ArtAssetImporter] Item icons: {assigned} assigned, {missing} sprites missing.");
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Assign Location Images from Sprites")]
        public static void AssignLocationImages()
        {
            int assigned = 0;
            int missing = 0;

            var locGuids = AssetDatabase.FindAssets("t:LocationDefinitionSO",
                new[] { LocationsDataPath, "Assets/_Game/Data/Generated" });
            foreach (var guid in locGuids)
            {
                string locPath = AssetDatabase.GUIDToAssetPath(guid);
                var locDef = AssetDatabase.LoadAssetAtPath<AtomicWar._Game.Data.LocationDefinitionSO>(locPath);
                if (locDef == null) continue;

                string spriteName = string.IsNullOrEmpty(locDef.id) ? locDef.name : locDef.id;
                var sprite = LoadSprite(LocationsPath, spriteName);

                if (sprite != null)
                {
                    var so = new SerializedObject(locDef);
                    var imgProp = so.FindProperty("establishingShot");
                    if (imgProp == null) imgProp = so.FindProperty("image");
                    if (imgProp != null)
                    {
                        imgProp.objectReferenceValue = sprite;
                        so.ApplyModifiedProperties();
                        assigned++;
                    }
                }
                else { missing++; }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[ArtAssetImporter] Location images: {assigned} assigned, {missing} sprites missing.");
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Assign Survivor Portraits from Sprites")]
        public static void AssignSurvivorPortraits()
        {
            int assigned = 0;
            int missing = 0;

            var archGuids = AssetDatabase.FindAssets("t:SurvivorArchetypeSO",
                new[] { ArchetypesDataPath, "Assets/_Game/Data/Generated" });
            foreach (var guid in archGuids)
            {
                string archPath = AssetDatabase.GUIDToAssetPath(guid);
                var archDef = AssetDatabase.LoadAssetAtPath<AtomicWar._Game.Data.SurvivorArchetypeSO>(archPath);
                if (archDef == null) continue;

                string spriteName = archDef.id ?? archDef.name;
                string spritePath = $"{PortraitsPath}/{spriteName}.png";
                var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(spritePath);

                if (sprite != null)
                {
                    var so = new SerializedObject(archDef);
                    var portraitProp = so.FindProperty("portrait");
                    if (portraitProp != null)
                    {
                        portraitProp.objectReferenceValue = sprite;
                        so.ApplyModifiedProperties();
                        assigned++;
                    }
                }
                else { missing++; }
            }

            AssetDatabase.SaveAssets();
            Debug.Log($"[ArtAssetImporter] Survivor portraits: {assigned} assigned, {missing} sprites missing.");
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Generate Placeholders And Assign")]
        public static void GeneratePlaceholdersAndAssign()
        {
            GeneratePlaceholders();
            FixItemImportSettings();
            AssignItemIcons();
            AssignLocationImages();
            AssignSurvivorPortraits();
            PrintMissingReport();
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Generate Missing Sprite Placeholders")]
        public static void GeneratePlaceholders()
        {
            string manifestPath = $"{SpritesRoot}/asset_manifest.json";
            if (!File.Exists(manifestPath))
            {
                Debug.LogError("[ArtAssetImporter] asset_manifest.json not found.");
                return;
            }

            // Create directory structure
            CreateDirectoryIfMissing(ItemsPath);
            CreateDirectoryIfMissing($"{ItemsPath}/Ammo");
            CreateDirectoryIfMissing($"{ItemsPath}/Weapons");
            CreateDirectoryIfMissing($"{ItemsPath}/Devices");
            CreateDirectoryIfMissing($"{ItemsPath}/Medical");
            CreateDirectoryIfMissing($"{ItemsPath}/Tools");
            CreateDirectoryIfMissing($"{ItemsPath}/Materials");
            CreateDirectoryIfMissing($"{ItemsPath}/Containers");
            CreateDirectoryIfMissing(LocationsPath);
            CreateDirectoryIfMissing(PortraitsPath);
            CreateDirectoryIfMissing(FactionsPath);
            CreateDirectoryIfMissing(WeatherPath);

            int created = 0;
            var manifest = JsonUtility.FromJson<AssetManifestJson>(
                File.ReadAllText(manifestPath));

            // Create item placeholders
            created += CreatePlaceholdersForCategory(
                manifest.categories.items_ammo_deprecated.sprites, ItemsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.items_ammo_military_boxes.sprites, ItemsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.items_weapons.sprites, ItemsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.items_containers.sprites, ItemsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.items_devices_medical_tools.sprites, ItemsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.locations.sprites, LocationsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.survivors.sprites, PortraitsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.factions.sprites, FactionsPath);
            created += CreatePlaceholdersForCategory(
                manifest.categories.weather.sprites, WeatherPath);

            created += CreatePlaceholdersForGeneratedIds(
                "t:ItemDefinition", ItemsDataPath, ItemsPath);
            created += CreatePlaceholdersForGeneratedIds(
                "t:LocationDefinitionSO", LocationsDataPath, LocationsPath);

            AssetDatabase.Refresh();
            Debug.Log($"[ArtAssetImporter] Created {created} placeholder sprites. " +
                "Replace with AI-generated assets when ready.");
        }

        [MenuItem("Tools/ASHFALL/Import Art Assets/Print Missing Asset Report")]
        public static void PrintMissingReport()
        {
            int total = 0, have = 0, missing = 0;
            var missingList = new List<string>();

            string[] categories = { ItemsPath, LocationsPath, PortraitsPath,
                FactionsPath, WeatherPath };
            foreach (var cat in categories)
            {
                if (!Directory.Exists(cat)) continue;
                var files = Directory.GetFiles(cat, "*.png", SearchOption.AllDirectories);
                foreach (var f in files)
                {
                    total++;
                    var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(f);
                    if (sprite != null) have++;
                    else { missing++; missingList.Add(f); }
                }
            }

            Debug.Log($"[ArtAssetImporter] Asset Report: {have}/{total} have sprites, " +
                $"{missing} missing.");
            if (missing > 0)
            {
                Debug.Log("Missing:\n" + string.Join("\n", missingList.GetRange(0,
                    Mathf.Min(missingList.Count, 20))));
                if (missingList.Count > 20)
                    Debug.Log($"... and {missingList.Count - 20} more.");
            }
        }

        private static void FixAllSpritesInDirectory(string dir, int maxSize,
            SpriteMeshType meshType)
        {
            if (!Directory.Exists(dir)) return;
            var files = Directory.GetFiles(dir, "*.png", SearchOption.AllDirectories);
            foreach (var f in files)
            {
                var importer = AssetImporter.GetAtPath(f) as TextureImporter;
                if (importer == null) continue;
                if (importer.textureType == TextureImporterType.Sprite &&
                    importer.maxTextureSize == maxSize) continue;

                importer.textureType = TextureImporterType.Sprite;
                importer.spriteImportMode = SpriteImportMode.Single;
                importer.spritePixelsPerUnit = 100;
                importer.maxTextureSize = maxSize;
                importer.filterMode = FilterMode.Bilinear;
                importer.textureCompression = TextureImporterCompression.Uncompressed;
                importer.SaveAndReimport();
            }
        }

        private static int CreatePlaceholdersForCategory(string[] spriteIds, string dir)
        {
            int created = 0;
            foreach (var id in spriteIds)
            {
                string path = $"{dir}/{id}.png";
                if (File.Exists(path)) continue;

                // Create a simple colored placeholder
                var tex = new Texture2D(128, 128, TextureFormat.RGBA32, false);
                var colors = new Color[128 * 128];
                Color tint = id.Contains("ammo") ? new Color(0.5f, 0.4f, 0.2f) :
                    id.Contains("weapon") || id.Contains("rifle") || id.Contains("pistol") ?
                    new Color(0.3f, 0.3f, 0.35f) :
                    id.Contains("location") ? new Color(0.2f, 0.25f, 0.3f) :
                    id.Contains("faction") ? new Color(0.3f, 0.2f, 0.2f) :
                    new Color(0.25f, 0.25f, 0.3f);

                for (int i = 0; i < colors.Length; i++) colors[i] = tint;
                tex.SetPixels(colors);
                tex.Apply();

                // Add text overlay
                var bytes = tex.EncodeToPNG();
                File.WriteAllBytes(path, bytes);
                Object.DestroyImmediate(tex);
                created++;
            }
            return created;
        }

        private static int CreatePlaceholdersForGeneratedIds(string filter, string dataPath, string spriteDir)
        {
            if (!Directory.Exists(dataPath)) return 0;
            var ids = new List<string>();
            var guids = AssetDatabase.FindAssets(filter, new[] { dataPath });
            foreach (var guid in guids)
            {
                string assetPath = AssetDatabase.GUIDToAssetPath(guid);
                var obj = AssetDatabase.LoadMainAssetAtPath(assetPath);
                if (obj == null) continue;
                var idField = obj.GetType().GetField("id");
                string id = idField != null ? idField.GetValue(obj) as string : null;
                if (string.IsNullOrEmpty(id)) id = obj.name;
                ids.Add(id);
            }
            return CreatePlaceholdersForCategory(ids.ToArray(), spriteDir);
        }

        private static Sprite LoadSprite(string dir, string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            string path = $"{dir}/{id}.png";
            var sprite = AssetDatabase.LoadAssetAtPath<Sprite>(path);
            if (sprite != null) return sprite;
            var assets = AssetDatabase.LoadAllAssetsAtPath(path);
            if (assets != null)
            {
                for (int i = 0; i < assets.Length; i++)
                    if (assets[i] is Sprite s) return s;
            }
            return null;
        }

        private static void CreateDirectoryIfMissing(string dir)
        {
            if (!Directory.Exists(dir))
                Directory.CreateDirectory(dir);
        }

        // JSON deserialization helper
        [System.Serializable]
        private class AssetManifestJson
        {
            public CategoryContainer categories;
        }

        [System.Serializable]
        private class CategoryContainer
        {
            public SpriteList items_ammo_deprecated;
            public SpriteList items_ammo_military_boxes;
            public SpriteList items_weapons;
            public SpriteList items_containers;
            public SpriteList items_devices_medical_tools;
            public SpriteList locations;
            public SpriteList survivors;
            public SpriteList factions;
            public SpriteList weather;
        }

        [System.Serializable]
        private class SpriteList
        {
            public string[] sprites;
        }
    }
}
