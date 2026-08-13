using System;
using System.Collections.Generic;
using UnityEngine;

namespace UnityEditor
{
    [AttributeUsage(AttributeTargets.Method)]
    public sealed class MenuItemAttribute : Attribute
    {
        public string menuItem { get; }
        public bool validate { get; }
        public int priority { get; }
        public MenuItemAttribute(string itemName) { menuItem = itemName; }
        public MenuItemAttribute(string itemName, bool isValidateFunction) { menuItem = itemName; validate = isValidateFunction; }
        public MenuItemAttribute(string itemName, bool isValidateFunction, int priority) { menuItem = itemName; validate = isValidateFunction; this.priority = priority; }
    }

    [AttributeUsage(AttributeTargets.Method)]
    public sealed class InitializeOnLoadMethodAttribute : Attribute { }

    [AttributeUsage(AttributeTargets.Class)]
    public sealed class InitializeOnLoadAttribute : Attribute { }

    public class EditorWindow : ScriptableObject
    {
        public static T GetWindow<T>(string title = "", bool focus = true) where T : EditorWindow => ScriptableObject.CreateInstance<T>();
        public static T GetWindow<T>(bool utility, string title = "", bool focus = true) where T : EditorWindow => ScriptableObject.CreateInstance<T>();
        public void Show() { }
        public void Close() { }
        public void Repaint() { }
    }

    public class AssetPostprocessor
    {
        public string assetPath { get; set; } = "";
        public AssetImporter? assetImporter => null;
    }

    public enum MessageType { None, Info, Warning, Error }
    public enum ImportAssetOptions { Default = 0, ForceUpdate = 1 }
    public enum SpriteMeshType { FullRect, Tight }
    public enum SpriteImportMode { None, Single, Multiple, Polygon }
    public enum TextureImporterType { Default, Sprite, GUI, Cookie }
    public enum TextureImporterCompression { Uncompressed, Compressed }
    public enum TextureImporterNPOTScale { None, ToNearest, ToLarger, ToSmaller }

    public class AssetImporter : UnityEngine.Object
    {
        public static AssetImporter? GetAtPath(string path) => null;
    }

    public class TextureImporter : AssetImporter
    {
        public TextureImporterType textureType { get; set; }
        public SpriteImportMode spriteImportMode { get; set; }
        public float spritePixelsPerUnit { get; set; }
        public int maxTextureSize { get; set; }
        public FilterMode filterMode { get; set; }
        public TextureImporterCompression textureCompression { get; set; }
        public TextureWrapMode wrapMode { get; set; }
        public bool mipmapEnabled { get; set; }
        public bool alphaIsTransparency { get; set; }
        public bool sRGBTexture { get; set; }
        public TextureImporterNPOTScale npotScale { get; set; }
        public void SaveAndReimport() { }
    }

    public static class AssetDatabase
    {
        public static string[] FindAssets(string filter) => Array.Empty<string>();
        public static string[] FindAssets(string filter, string[] searchInFolders) => Array.Empty<string>();
        public static string GUIDToAssetPath(string guid) => "";
        public static string AssetPathToGUID(string path) => "";
        public static T? LoadAssetAtPath<T>(string assetPath) where T : UnityEngine.Object => null;
        public static UnityEngine.Object? LoadMainAssetAtPath(string assetPath) => null;
        public static UnityEngine.Object[] LoadAllAssetsAtPath(string assetPath) => Array.Empty<UnityEngine.Object>();
        public static void CreateAsset(UnityEngine.Object asset, string path) { }
        public static void AddObjectToAsset(UnityEngine.Object objectToAdd, UnityEngine.Object assetObject) { }
        public static void DeleteAsset(string path) { }
        public static void SaveAssets() { }
        public static void Refresh() { }
        public static void ImportAsset(string path, ImportAssetOptions options = ImportAssetOptions.Default) { }
        public static bool IsValidFolder(string path) => true;
        public static string CreateFolder(string parentFolder, string newFolderName) => $"{parentFolder}/{newFolderName}";
    }

    public static class EditorUtility
    {
        public static void SetDirty(UnityEngine.Object target) { }
        public static bool DisplayDialog(string title, string message, string ok, string cancel = "") => true;
    }

    public static class EditorGUIUtility
    {
        public static string systemCopyBuffer { get; set; } = "";
    }

    public static class EditorStyles
    {
        public static GUIStyle boldLabel { get; } = new GUIStyle();
        public static GUIStyle label { get; } = new GUIStyle();
        public static GUIStyle helpBox { get; } = new GUIStyle();
    }

    public static class EditorGUILayout
    {
        public static void LabelField(string label, GUIStyle? style = null) { }
        public static void LabelField(string label, string label2) { }
        public static void HelpBox(string message, MessageType type) { }
        public static void Space(float pixels = 6f) { }
        public static bool Button(string text) => false;
        public static Vector2 BeginScrollView(Vector2 scrollPosition, params GUILayoutOption[] options) => scrollPosition;
        public static void EndScrollView() { }
        public static string TextArea(string text, params GUILayoutOption[] options) => text;
        public static string TextField(string label, string text) => text;
        public static int IntField(string label, int value) => value;
        public static float FloatField(string label, float value) => value;
        public static bool Toggle(string label, bool value) => value;
        public static UnityEngine.Object? ObjectField(string label, UnityEngine.Object? obj, Type objType, bool allowSceneObjects) => obj;
    }

    public class SerializedProperty
    {
        public string stringValue { get; set; } = "";
        public int intValue { get; set; }
        public float floatValue { get; set; }
        public bool boolValue { get; set; }
        public UnityEngine.Object? objectReferenceValue { get; set; }
    }

    public class SerializedObject
    {
        public UnityEngine.Object targetObject { get; }
        public SerializedObject(UnityEngine.Object obj) { targetObject = obj; }
        public SerializedProperty? FindProperty(string propertyPath) => new SerializedProperty();
        public void ApplyModifiedProperties() { }
        public void Update() { }
    }

    public static class AssemblyReloadEvents
    {
        public static event Action? beforeAssemblyReload;
    }

    public enum PlayModeStateChange { EnteredEditMode, ExitingEditMode, EnteredPlayMode, ExitingPlayMode }

    public static class EditorApplication
    {
        public static bool isPlaying { get; set; }
        public static event Action<PlayModeStateChange>? playModeStateChanged;
        public static event Action? quitting;
        public static void Exit(int exitCode) { }
    }
}

namespace UnityEditor.SceneManagement
{
    public static class EditorSceneManager
    {
        public static void MarkSceneDirty(UnityEngine.SceneManagement.Scene scene) { }
    }
}
