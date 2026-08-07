#if UNITY_EDITOR
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEngine;
using UnityEngine.TextCore.LowLevel;
using UnityEngine.TextCore.Text;
using UnityEngine.UIElements;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Generates the *derived* assets the main menu needs — the ones that
    /// cannot sensibly be hand-authored or diffed in git: SDF font assets
    /// baked from the TTFs in Assets/Fonts, the PanelSettings the UIDocument
    /// renders through, the runtime theme stylesheet, and the 1x4 scanline
    /// tile that stands in for CSS repeating-linear-gradient (which USS has
    /// no equivalent for).
    ///
    /// Idempotent: existing assets are left alone unless <c>force</c> is set,
    /// so re-running it after a fresh clone only fills in what is missing.
    ///
    /// Entry points:
    ///   Editor menu:  ASHFALL > Generate Main Menu Assets
    ///   Batchmode:    -executeMethod AtomicWar._Game.Editor.MainMenuAssetGenerator.GenerateAll
    /// </summary>
    public static class MainMenuAssetGenerator
    {
        public const string FontSourceDir = "Assets/Fonts";
        public const string MenuDir = "Assets/_Game/UI/MainMenu";
        public const string FontAssetDir = MenuDir + "/Fonts";
        public const string PanelSettingsPath = MenuDir + "/MainMenuPanelSettings.asset";
        public const string ScanlinePath = MenuDir + "/Scanline_1x4.png";
        public const string ThemeDir = "Assets/UI Toolkit/UnityThemes";
        public const string ThemePath = ThemeDir + "/UnityDefaultRuntimeTheme.tss";

        /// <summary>
        /// SDF bake settings. 90pt sampling with 9px padding is the TMP
        /// default and gives clean edges at the sizes this menu uses
        /// (8px monospace captions up to 96px display type). A single
        /// 1024x1024 atlas holds far more than the Latin set the menu needs;
        /// multi-atlas support is on so a future localisation cannot silently
        /// drop glyphs.
        /// </summary>
        private const int SamplingPointSize = 90;
        private const int AtlasPadding = 9;
        private const int AtlasSize = 1024;

        /// <summary>The 5 Barlow weights + Share Tech Mono the UXML asks for.</summary>
        private static readonly string[] FontFiles =
        {
            "BarlowCondensed-Regular.ttf",
            "BarlowCondensed-Medium.ttf",
            "BarlowCondensed-MediumItalic.ttf",
            "BarlowCondensed-SemiBold.ttf",
            "BarlowCondensed-Bold.ttf",
            "ShareTechMono-Regular.ttf",
        };

        [MenuItem("ASHFALL/Generate Main Menu Assets")]
        public static void GenerateFromMenu() => Generate(force: false);

        /// <summary>Batchmode entry point.</summary>
        public static void GenerateAll() => Generate(force: false);

        /// <summary>Batchmode entry point that rebuilds even existing assets.</summary>
        public static void RegenerateAll() => Generate(force: true);

        private static void Generate(bool force)
        {
            EnsureFolder(MenuDir);
            EnsureFolder(FontAssetDir);
            EnsureFolder(ThemeDir);

            var fontAssets = new List<FontAsset>();
            foreach (string file in FontFiles)
            {
                FontAsset fa = GenerateFontAsset(file, force);
                if (fa != null) fontAssets.Add(fa);
            }

            GenerateTheme(force);
            GenerateScanline(force);
            GeneratePanelSettings(force);

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[MainMenuAssetGenerator] Done. {fontAssets.Count}/{FontFiles.Length} font assets present.");
        }

        // -----------------------------------------------------------------
        // SDF font assets
        // -----------------------------------------------------------------

        private static FontAsset GenerateFontAsset(string ttfFileName, bool force)
        {
            string sourcePath = $"{FontSourceDir}/{ttfFileName}";
            string assetName = Path.GetFileNameWithoutExtension(ttfFileName) + " SDF";
            string assetPath = $"{FontAssetDir}/{assetName}.asset";

            var existing = AssetDatabase.LoadAssetAtPath<FontAsset>(assetPath);
            if (existing != null && !force) return existing;

            var font = AssetDatabase.LoadAssetAtPath<Font>(sourcePath);
            if (font == null)
            {
                Debug.LogError($"[MainMenuAssetGenerator] Missing source font: {sourcePath}");
                return null;
            }

            var fontAsset = FontAsset.CreateFontAsset(
                font,
                SamplingPointSize,
                AtlasPadding,
                GlyphRenderMode.SDFAA,
                AtlasSize,
                AtlasSize,
                AtlasPopulationMode.Dynamic,
                enableMultiAtlasSupport: true);

            if (fontAsset == null)
            {
                Debug.LogError($"[MainMenuAssetGenerator] CreateFontAsset returned null for {sourcePath}");
                return null;
            }

            fontAsset.name = assetName;

            if (existing != null) AssetDatabase.DeleteAsset(assetPath);
            AssetDatabase.CreateAsset(fontAsset, assetPath);

            // The atlas texture and material are created in memory by
            // CreateFontAsset. They must be nested inside the .asset file or
            // they are discarded on domain reload and the font renders blank.
            if (fontAsset.atlasTextures != null && fontAsset.atlasTextures.Length > 0)
            {
                Texture2D atlas = fontAsset.atlasTextures[0];
                atlas.name = assetName + " Atlas";
                atlas.hideFlags = HideFlags.HideInHierarchy;
                AssetDatabase.AddObjectToAsset(atlas, fontAsset);
            }

            if (fontAsset.material != null)
            {
                fontAsset.material.name = assetName + " Material";
                fontAsset.material.hideFlags = HideFlags.HideInHierarchy;
                AssetDatabase.AddObjectToAsset(fontAsset.material, fontAsset);
            }

            EditorUtility.SetDirty(fontAsset);
            Debug.Log($"[MainMenuAssetGenerator] Baked SDF font asset: {assetPath}");
            return fontAsset;
        }

        // -----------------------------------------------------------------
        // Runtime theme
        // -----------------------------------------------------------------

        /// <summary>
        /// Without a theme stylesheet, PanelSettings logs a warning and every
        /// built-in control (Button, Slider, Toggle, DropdownField) renders
        /// with no styling at all. The one-line import pulls in Unity's
        /// default runtime theme, which our own USS then overrides.
        /// </summary>
        private static void GenerateTheme(bool force)
        {
            if (File.Exists(ThemePath) && !force) return;
            File.WriteAllText(ThemePath, "@import url(\"unity-theme://default\");\n");
            AssetDatabase.ImportAsset(ThemePath, ImportAssetOptions.ForceUpdate);
            Debug.Log($"[MainMenuAssetGenerator] Wrote theme: {ThemePath}");
        }

        // -----------------------------------------------------------------
        // Scanline tile
        // -----------------------------------------------------------------

        /// <summary>
        /// USS has no repeating-linear-gradient, so the CRT scanline overlay
        /// becomes a 1x4 tile repeated across the element: one lit row, three
        /// transparent. It is written at full opacity and dimmed in USS, so
        /// the strength stays tunable without regenerating the texture.
        /// </summary>
        private static void GenerateScanline(bool force)
        {
            if (File.Exists(ScanlinePath) && !force) return;

            var tex = new Texture2D(1, 4, TextureFormat.RGBA32, mipChain: false, linear: false);
            // Row 0 is the lit scanline; rows 1-3 are clear. Fully transparent
            // pixels keep white RGB so bilinear filtering at any future scale
            // cannot bleed black fringes into the lit row.
            tex.SetPixel(0, 0, new Color(1f, 1f, 1f, 1f));
            for (int y = 1; y < 4; y++) tex.SetPixel(0, y, new Color(1f, 1f, 1f, 0f));
            tex.Apply();

            File.WriteAllBytes(ScanlinePath, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
            AssetDatabase.ImportAsset(ScanlinePath, ImportAssetOptions.ForceUpdate);

            if (AssetImporter.GetAtPath(ScanlinePath) is TextureImporter ti)
            {
                ti.textureType = TextureImporterType.Default;
                ti.wrapMode = TextureWrapMode.Repeat;
                ti.filterMode = FilterMode.Point;   // crisp 1px lines, no blur
                ti.mipmapEnabled = false;
                ti.alphaIsTransparency = true;
                ti.sRGBTexture = true;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.textureCompression = TextureImporterCompression.Uncompressed;
                ti.SaveAndReimport();
            }

            Debug.Log($"[MainMenuAssetGenerator] Wrote scanline tile: {ScanlinePath}");
        }

        // -----------------------------------------------------------------
        // PanelSettings
        // -----------------------------------------------------------------

        private static void GeneratePanelSettings(bool force)
        {
            var existing = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (existing != null && !force) return;

            PanelSettings ps = existing != null
                ? existing
                : ScriptableObject.CreateInstance<PanelSettings>();

            // The whole layout is authored against 1920x1080 and scales with
            // the window. Matching on height keeps the menu column's vertical
            // rhythm intact on ultrawide displays, where matching on width
            // would blow the type up past the screen.
            ps.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            ps.referenceResolution = new Vector2Int(1920, 1080);
            ps.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            ps.match = 1f;
            ps.sortingOrder = 0f;
            // The menu's own root fills the screen with the backdrop image, so
            // the panel does not need to clear. Leaving clearColor off keeps
            // the camera in charge of the frame, as it is elsewhere.
            ps.clearColor = false;
            ps.colorClearValue = Color.black;

            var theme = AssetDatabase.LoadAssetAtPath<ThemeStyleSheet>(ThemePath);
            if (theme != null) ps.themeStyleSheet = theme;
            else Debug.LogWarning($"[MainMenuAssetGenerator] Theme not found at {ThemePath}; controls will render unstyled.");

            if (existing == null) AssetDatabase.CreateAsset(ps, PanelSettingsPath);
            EditorUtility.SetDirty(ps);
            Debug.Log($"[MainMenuAssetGenerator] Wrote panel settings: {PanelSettingsPath}");
        }

        // -----------------------------------------------------------------

        private static void EnsureFolder(string assetPath)
        {
            if (AssetDatabase.IsValidFolder(assetPath)) return;
            string parent = Path.GetDirectoryName(assetPath).Replace('\\', '/');
            string leaf = Path.GetFileName(assetPath);
            EnsureFolder(parent);
            AssetDatabase.CreateFolder(parent, leaf);
        }
    }
}
#endif
