#if UNITY_EDITOR
using System;
using System.IO;
using AtomicWar.Data;
using UnityEditor;
using UnityEngine;

namespace AtomicWar.EditorTools
{
    public static class ItemImporterEditor
    {
        [Serializable]
        private class ItemJsonWrapper
        {
            public string id;
            public string itemName;
            public string itemType;
            public int stackSize = 1;
            public float weight;
            public int hungerRestored;
            public int healthEffect;
            public int moraleEffect;
            public string description;
        }

        [Serializable]
        private class ItemListWrapper
        {
            public ItemJsonWrapper[] items;
        }

        [MenuItem("Tools/Import Items from JSON")]
        public static void ImportItemsFromJson()
        {
            string jsonPath = Path.Combine(Application.dataPath, "StreamingAssets", "items.json");
            if (!File.Exists(jsonPath))
            {
                Debug.LogError($"[ItemImporter] items.json not found at: {jsonPath}");
                return;
            }

            string rawJson = File.ReadAllText(jsonPath);

            // Wrap array for JsonUtility parsing
            string wrappedJson = $"{{\"items\":{rawJson}}}";
            ItemListWrapper wrapper = JsonUtility.FromJson<ItemListWrapper>(wrappedJson);

            if (wrapper == null || wrapper.items == null || wrapper.items.Length == 0)
            {
                Debug.LogError("[ItemImporter] Failed to parse items from JSON.");
                return;
            }

            string outputFolder = "Assets/Game/Items";
            if (!AssetDatabase.IsValidFolder("Assets/Game"))
            {
                AssetDatabase.CreateFolder("Assets", "Game");
            }
            if (!AssetDatabase.IsValidFolder(outputFolder))
            {
                AssetDatabase.CreateFolder("Assets/Game", "Items");
            }

            int importedCount = 0;

            foreach (var jsonItem in wrapper.items)
            {
                string assetPath = $"{outputFolder}/{jsonItem.id}.asset";
                ItemDefinition itemDef = AssetDatabase.LoadAssetAtPath<ItemDefinition>(assetPath);
                bool isNew = false;

                if (itemDef == null)
                {
                    itemDef = ScriptableObject.CreateInstance<ItemDefinition>();
                    isNew = true;
                }

                itemDef.id = jsonItem.id;
                itemDef.itemName = jsonItem.itemName;
                
                if (Enum.TryParse<ItemType>(jsonItem.itemType, true, out var parsedType))
                {
                    itemDef.itemType = parsedType;
                }
                else
                {
                    itemDef.itemType = ItemType.Material;
                }

                itemDef.stackSize = jsonItem.stackSize;
                itemDef.weight = jsonItem.weight;
                itemDef.hungerRestored = jsonItem.hungerRestored;
                itemDef.healthEffect = jsonItem.healthEffect;
                itemDef.moraleEffect = jsonItem.moraleEffect;
                itemDef.description = jsonItem.description;

                if (isNew)
                {
                    AssetDatabase.CreateAsset(itemDef, assetPath);
                }
                else
                {
                    EditorUtility.SetDirty(itemDef);
                }

                importedCount++;
            }

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[ItemImporter] Successfully imported/updated {importedCount} ItemDefinition ScriptableObject assets into '{outputFolder}'.");
        }
    }
}
#endif
