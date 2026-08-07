#if UNITY_EDITOR
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Editor helpers: sync diegetic HUD assets into Resources for play mode,
    /// and mount a UIDocument on a selected HUD / scene object.
    /// Mirrors the MainMenu StartScreenDeploy pattern without rewriting scenes.
    /// </summary>
    public static class DiegeticHudDeploy
    {
        public const string CanonicalUxml = DiegeticHudController.UxmlResourcePath;
        public const string CanonicalUss = DiegeticHudController.UssResourcePath;
        public const string CanonicalPanel = DiegeticHudController.PanelSettingsAssetPath;

        public const string ResourcesDir = "Assets/Resources/UI";
        public const string ResourcesUxml = ResourcesDir + "/DiegeticHud.uxml";
        public const string ResourcesUss = ResourcesDir + "/DiegeticHud.uss";
        public const string ResourcesPanel = ResourcesDir + "/DiegeticHudPanelSettings.asset";

        [MenuItem("ASHFALL/Diegetic HUD/Sync Resources Copies")]
        public static void SyncResourcesCopies()
        {
            EnsureFolder("Assets/Resources");
            EnsureFolder(ResourcesDir);

            CopyAsset(CanonicalUxml, ResourcesUxml);
            CopyAsset(CanonicalUss, ResourcesUss);
            CopyAsset(CanonicalPanel, ResourcesPanel);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log("[DiegeticHudDeploy] Synced UXML/USS/PanelSettings into Resources/UI for play-mode load.");
        }

        [MenuItem("ASHFALL/Diegetic HUD/Mount UIDocument On Selection")]
        public static void MountOnSelection()
        {
            var go = Selection.activeGameObject;
            if (go == null)
            {
                Debug.LogError("[DiegeticHudDeploy] Select a GameObject (HUD or child) first.");
                return;
            }

            SyncResourcesCopies();

            var hud = go.GetComponent<HUD>() ?? go.GetComponentInParent<HUD>();
            var host = hud != null ? hud.gameObject : go;

            var controller = host.GetComponent<DiegeticHudController>()
                ?? host.GetComponentInChildren<DiegeticHudController>()
                ?? host.AddComponent<DiegeticHudController>();

            var document = controller.GetComponent<UIDocument>()
                ?? controller.gameObject.AddComponent<UIDocument>();

            var panel = AssetDatabase.LoadAssetAtPath<PanelSettings>(CanonicalPanel)
                ?? AssetDatabase.LoadAssetAtPath<PanelSettings>(DiegeticHudController.SharedPanelSettingsPath);
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(CanonicalUxml);
            var uss = AssetDatabase.LoadAssetAtPath<StyleSheet>(CanonicalUss);

            if (panel == null || uxml == null)
            {
                Debug.LogError(
                    "[DiegeticHudDeploy] Missing DiegeticHud UXML or PanelSettings. " +
                    "Expected under Assets/_Game/UI/.");
                return;
            }

            document.panelSettings = panel;
            document.visualTreeAsset = uxml;

            // Assign serialized fields via SerializedObject so they stick on the prefab/scene.
            var so = new SerializedObject(controller);
            so.FindProperty("_document").objectReferenceValue = document;
            so.FindProperty("_panelSettings").objectReferenceValue = panel;
            so.FindProperty("_uxml").objectReferenceValue = uxml;
            so.FindProperty("_uss").objectReferenceValue = uss;
            so.FindProperty("_createDocumentIfMissing").boolValue = true;
            so.ApplyModifiedPropertiesWithoutUndo();

            EditorUtility.SetDirty(controller);
            EditorUtility.SetDirty(document);
            if (hud != null) EditorUtility.SetDirty(hud);

            controller.EnsureDocumentMounted();
            controller.EnsureBuilt();

            Debug.Log($"[DiegeticHudDeploy] Mounted UIDocument on '{host.name}' (panel sort={panel.sortingOrder}).");
        }

        private static void CopyAsset(string from, string to)
        {
            if (!File.Exists(from))
            {
                Debug.LogWarning($"[DiegeticHudDeploy] Missing source: {from}");
                return;
            }

            // Prefer AssetDatabase.CopyAsset so .meta stays consistent when possible.
            if (File.Exists(to))
            {
                // Overwrite content in place (keeps destination GUID).
                File.Copy(from, to, overwrite: true);
                AssetDatabase.ImportAsset(to, ImportAssetOptions.ForceUpdate);
            }
            else if (!AssetDatabase.CopyAsset(from, to))
            {
                File.Copy(from, to, overwrite: true);
                AssetDatabase.ImportAsset(to, ImportAssetOptions.ForceUpdate);
            }
        }

        private static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath)) return;
            string parent = Path.GetDirectoryName(assetPath)?.Replace('\\', '/');
            string leaf = Path.GetFileName(assetPath);
            if (string.IsNullOrEmpty(parent) || string.IsNullOrEmpty(leaf)) return;
            if (!AssetDatabase.IsValidFolder(parent))
                EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }
    }
}
#endif
