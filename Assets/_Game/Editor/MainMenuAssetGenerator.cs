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
        public const string VignettePath = MenuDir + "/Vignette_256.png";
        public const string FadeRightPath = MenuDir + "/FadeRight_64x1.png";
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
            GenerateVignette(force);
            GenerateFadeRight(force);
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
            // Underscore, not a space: USS url() paths are far less
            // error-prone without characters that need percent-encoding.
            string assetName = Path.GetFileNameWithoutExtension(ttfFileName) + "_SDF";
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

            // Point filtering keeps the 1px line crisp instead of smearing it
            // across the 4px cell; Repeat is what makes the tile a pattern.
            WritePng(tex, ScanlinePath, FilterMode.Point, TextureWrapMode.Repeat);
            Debug.Log($"[MainMenuAssetGenerator] Wrote scanline tile: {ScanlinePath}");
        }

        // -----------------------------------------------------------------
        // Baked gradients
        // -----------------------------------------------------------------

        /// <summary>
        /// USS recognises linear-gradient as a value function but not the
        /// "to bottom" / "to right" direction keywords the prototype's CSS
        /// uses, and it has no repeating-linear-gradient at all. Rather than
        /// guess at the supported syntax, every gradient the menu needs is
        /// baked to a small texture here — exact, and cheap enough that the
        /// three of them together are under 200 KB.
        ///
        /// This one is the backdrop vignette: a horizontal ramp that crushes
        /// the left third to near-black so the menu column stays legible over
        /// the photo, composited over a shorter bottom-up darkening.
        /// </summary>
        private static void GenerateVignette(bool force)
        {
            const int size = 256;
            if (File.Exists(VignettePath) && !force) return;

            // Stops copied from the prototype's two stacked linear-gradients.
            var horizontal = new[]
            {
                (t: 0.00f, c: new Color32(1, 2, 2, 250)),
                (t: 0.23f, c: new Color32(2, 3, 3, 237)),
                (t: 0.48f, c: new Color32(5, 6, 6, 107)),
                (t: 0.80f, c: new Color32(3, 3, 3, 20)),
                (t: 1.00f, c: new Color32(3, 3, 3, 20)),
            };

            var tex = new Texture2D(size, size, TextureFormat.RGBA32, mipChain: false, linear: false);
            var pixels = new Color[size * size];

            for (int y = 0; y < size; y++)
            {
                // Bottom-up layer: opaque-ish at the very bottom, gone by 34%.
                float up = 1f - (y / (float)(size - 1));
                float bottomA = Mathf.Lerp(0.58f, 0f, Mathf.Clamp01(up / 0.34f));
                var bottom = new Color(1f / 255f, 2f / 255f, 2f / 255f, bottomA);

                for (int x = 0; x < size; x++)
                {
                    Color top = SampleRamp(horizontal, x / (float)(size - 1));
                    pixels[y * size + x] = OverStraightAlpha(top, bottom);
                }
            }

            tex.SetPixels(pixels);
            tex.Apply();
            WritePng(tex, VignettePath, FilterMode.Bilinear, TextureWrapMode.Clamp);
            Debug.Log($"[MainMenuAssetGenerator] Wrote vignette: {VignettePath}");
        }

        /// <summary>
        /// A plain white-to-transparent horizontal ramp. Tinted from USS with
        /// -unity-background-image-tint-color it stands in for every
        /// left-to-right fade in the design — the top rule, the menu row's
        /// backing panel, and its warm hover glow — so one 64x1 texture
        /// replaces three separate gradients.
        /// </summary>
        private static void GenerateFadeRight(bool force)
        {
            const int width = 64;
            if (File.Exists(FadeRightPath) && !force) return;

            var tex = new Texture2D(width, 1, TextureFormat.RGBA32, mipChain: false, linear: false);
            for (int x = 0; x < width; x++)
            {
                float a = 1f - (x / (float)(width - 1));
                tex.SetPixel(x, 0, new Color(1f, 1f, 1f, a));
            }
            tex.Apply();
            WritePng(tex, FadeRightPath, FilterMode.Bilinear, TextureWrapMode.Clamp);
            Debug.Log($"[MainMenuAssetGenerator] Wrote fade ramp: {FadeRightPath}");
        }

        /// <summary>Piecewise-linear colour ramp lookup, stops assumed sorted.</summary>
        private static Color SampleRamp((float t, Color32 c)[] stops, float t)
        {
            if (t <= stops[0].t) return stops[0].c;
            for (int i = 1; i < stops.Length; i++)
            {
                if (t > stops[i].t) continue;
                float span = stops[i].t - stops[i - 1].t;
                float k = span <= 0f ? 0f : (t - stops[i - 1].t) / span;
                return Color.Lerp(stops[i - 1].c, stops[i].c, k);
            }
            return stops[stops.Length - 1].c;
        }

        /// <summary>Source-over composite of two straight-alpha colours.</summary>
        private static Color OverStraightAlpha(Color src, Color dst)
        {
            float a = src.a + dst.a * (1f - src.a);
            if (a <= 0f) return new Color(0f, 0f, 0f, 0f);
            Vector3 rgb = (new Vector3(src.r, src.g, src.b) * src.a
                           + new Vector3(dst.r, dst.g, dst.b) * dst.a * (1f - src.a)) / a;
            return new Color(rgb.x, rgb.y, rgb.z, a);
        }

        private static void WritePng(Texture2D tex, string path, FilterMode filter, TextureWrapMode wrap)
        {
            File.WriteAllBytes(path, tex.EncodeToPNG());
            Object.DestroyImmediate(tex);
            AssetDatabase.ImportAsset(path, ImportAssetOptions.ForceUpdate);

            if (AssetImporter.GetAtPath(path) is TextureImporter ti)
            {
                ti.textureType = TextureImporterType.Default;
                ti.wrapMode = wrap;
                ti.filterMode = filter;
                ti.mipmapEnabled = false;
                ti.alphaIsTransparency = true;
                ti.sRGBTexture = true;
                ti.npotScale = TextureImporterNPOTScale.None;
                ti.textureCompression = TextureImporterCompression.Uncompressed;
                ti.SaveAndReimport();
            }
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
