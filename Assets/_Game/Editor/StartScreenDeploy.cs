#if UNITY_EDITOR
using System;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;
using AtomicWar._Game.UI.MainMenu;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Builds a RAM-lean, deployable starting screen and a standalone
    /// executable that boots straight into it — no Unity Editor required to
    /// run the game.
    ///
    /// What it does:
    ///   1. Imports Assets/UI_StyleReference_01.jpg (from the repo root copy)
    ///      and downscales + Crunch-compresses it so the 5504x3072 source
    ///      (~63 MB RGBA) ships as a small, low-memory texture.
    ///   2. Generates Assets/Scenes/StartScreen.unity: a camera + a UIDocument
    ///      running MainMenu.uxml under MainMenuController.
    ///   3. Registers StartScreen as scene 0 and the gameplay scene as scene 1
    ///      in Build Settings, so the menu can actually load a game.
    ///   4. Builds a standalone Linux64 executable with low-memory settings.
    ///
    /// Entry points:
    ///   Editor menu:  ASHFALL > Deploy Start Screen
    ///                 ASHFALL > Rebuild Start Screen Scene Only
    ///   Batchmode:    -executeMethod AtomicWar._Game.Editor.StartScreenDeploy.Deploy
    ///                 -executeMethod AtomicWar._Game.Editor.StartScreenDeploy.DeploySceneOnly
    /// </summary>
    public static class StartScreenDeploy
    {
        public const string SourceImage = "UI_StyleReference_01.jpg";          // repo root
        public const string TargetImage = "Assets/UI_StyleReference_01.jpg";   // imported copy
        public const string ScenePath = "Assets/Scenes/StartScreen.unity";
        public const string GameplayScenePath = "Assets/Scenes/SampleScene.unity";
        public const string BuildPath = "Builds/StartScreen/ASHFALL_Start.x86_64";

        public const string MenuUxmlPath = "Assets/_Game/UI/MainMenu/MainMenu.uxml";
        public const string PanelSettingsPath = "Assets/_Game/UI/MainMenu/MainMenuPanelSettings.asset";

        /// <summary>Max texture dimension for the start screen (RAM saving).</summary>
        private const int MaxTextureSize = 2048;

        [MenuItem("ASHFALL/Deploy Start Screen")]
        public static void DeployFromMenu()
        {
            Deploy();
        }

        [MenuItem("ASHFALL/Rebuild Start Screen Scene Only")]
        public static void DeploySceneOnlyFromMenu()
        {
            DeploySceneOnly();
        }

        /// <summary>
        /// Regenerate the scene and refresh Build Settings without producing a
        /// player. The full <see cref="Deploy"/> spends minutes on the
        /// standalone build, which is dead time while iterating on the menu.
        /// </summary>
        public static void DeploySceneOnly()
        {
            ImportOptimisedTexture();

            if (!BuildStartScreenScene())
            {
                Debug.LogError("[StartScreenDeploy] Scene build failed.");
                return;
            }

            RegisterAsEntryScene();
            Debug.Log($"[StartScreenDeploy] Scene rebuilt: {ScenePath} (no player build).");
        }

        /// <summary>Full pipeline: import+optimise image, build scene, register, build binary.</summary>
        public static void Deploy()
        {
            Debug.Log("[StartScreenDeploy] Importing + optimising style-reference image...");
            Texture2D tex = ImportOptimisedTexture();
            if (tex == null)
            {
                Debug.LogError("[StartScreenDeploy] Could not import UI_StyleReference_01.jpg. Aborting.");
                return;
            }

            Debug.Log("[StartScreenDeploy] Building StartScreen scene...");
            if (!BuildStartScreenScene())
            {
                Debug.LogError("[StartScreenDeploy] Scene build failed. Aborting.");
                return;
            }

            Debug.Log("[StartScreenDeploy] Registering StartScreen as entry scene...");
            RegisterAsEntryScene();

            Debug.Log("[StartScreenDeploy] Building standalone Linux64 (RAM-lean)...");
            bool ok = BuildLinux();
            Debug.Log(ok
                ? $"[StartScreenDeploy] DONE. Run: {Path.GetFullPath(BuildPath)}"
                : "[StartScreenDeploy] FAILED. See console.");
        }

        // -----------------------------------------------------------------
        // 1. Import + optimise the image
        // -----------------------------------------------------------------

        private static Texture2D ImportOptimisedTexture()
        {
            // Copy from repo root into Assets if not already there.
            if (!File.Exists(TargetImage) && File.Exists(SourceImage))
            {
                File.Copy(SourceImage, TargetImage, overwrite: true);
                AssetDatabase.ImportAsset(TargetImage, ImportAssetOptions.ForceUpdate);
            }

            AssetImporter imp = AssetImporter.GetAtPath(TargetImage);
            if (imp is TextureImporter ti)
            {
                // Downscale + Crunch: biggest RAM saver (63MB -> ~ a few MB).
                ti.maxTextureSize = MaxTextureSize;
                ti.textureCompression = TextureImporterCompression.CompressedHQ;
                ti.crunchedCompression = true;
                ti.compressionQuality = 50;
                ti.mipmapEnabled = false;          // fullscreen backdrop needs no mips
                ti.alphaIsTransparency = false;
                ti.sRGBTexture = true;
                ti.textureType = TextureImporterType.Default;
                ti.SaveAndReimport();
                Debug.Log($"[StartScreenDeploy] Texture imported @ {MaxTextureSize}px, Crunch-compressed.");
            }
            else
            {
                Debug.LogWarning("[StartScreenDeploy] No importer found for target image; using as-is.");
            }

            return AssetDatabase.LoadAssetAtPath<Texture2D>(TargetImage);
        }

        // -----------------------------------------------------------------
        // 2. Build the StartScreen scene programmatically
        // -----------------------------------------------------------------

        private static bool BuildStartScreenScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            // --- Main camera (solid black clear) ---
            var camGo = new GameObject("Main Camera");
            var cam = camGo.AddComponent<Camera>();
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = Color.black;
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.nearClipPlane = 0.3f;
            cam.farClipPlane = 1000f;
            camGo.tag = "MainCamera";
            camGo.AddComponent<AudioListener>();

            // --- Main menu (UI Toolkit) ---
            //
            // Deliberately no EventSystem: with one active but no
            // PanelRaycaster/PanelEventHandler present, a UI Toolkit panel
            // receives no input at all. Without an EventSystem the panel falls
            // back to UI Toolkit's own default event system, which reads the
            // legacy Input Manager -- satisfied by this project's
            // activeInputHandler: 2 (Both).
            var menuGo = new GameObject("MainMenu");
            var document = menuGo.AddComponent<UIDocument>();

            var panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            var uxml = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(MenuUxmlPath);
            if (panelSettings == null || uxml == null)
            {
                Debug.LogError(
                    "[StartScreenDeploy] Main menu assets missing. Run " +
                    "ASHFALL > Generate Main Menu Assets first.");
                return false;
            }

            document.panelSettings = panelSettings;
            document.visualTreeAsset = uxml;
            menuGo.AddComponent<MainMenuController>();

            // --- Save ---
            Directory.CreateDirectory(Path.GetDirectoryName(ScenePath));
            bool saved = EditorSceneManager.SaveScene(scene, ScenePath);
            if (!saved)
            {
                Debug.LogError($"[StartScreenDeploy] Failed to save scene {ScenePath}");
                return false;
            }
            Debug.Log($"[StartScreenDeploy] Scene saved: {ScenePath}");
            return true;
        }

        // -----------------------------------------------------------------
        // 3. Register StartScreen as the entry (scene 0)
        // -----------------------------------------------------------------

        private static void RegisterAsEntryScene()
        {
            var list = new System.Collections.Generic.List<EditorBuildSettingsScene>();
            // Put StartScreen first so the standalone boots into it, and the
            // gameplay scene immediately after -- SceneManager.LoadScene from
            // the menu fails outright if the target is not in this list.
            list.Add(new EditorBuildSettingsScene(ScenePath, true));
            if (File.Exists(GameplayScenePath))
                list.Add(new EditorBuildSettingsScene(GameplayScenePath, true));
            else
                Debug.LogWarning($"[StartScreenDeploy] Gameplay scene not found at {GameplayScenePath}.");

            foreach (var s in EditorBuildSettings.scenes)
            {
                if (s == null || string.IsNullOrEmpty(s.path)) continue;
                if (s.path == ScenePath || s.path == GameplayScenePath) continue;
                list.Add(new EditorBuildSettingsScene(s.path, s.enabled));
            }
            EditorBuildSettings.scenes = list.ToArray();
            Debug.Log("[StartScreenDeploy] StartScreen registered as scene 0, gameplay scene as scene 1.");
        }

        // -----------------------------------------------------------------
        // 4. RAM-lean standalone build
        // -----------------------------------------------------------------

        private static bool BuildLinux()
        {
            if (!BuildPipeline.IsBuildTargetSupported(BuildTargetGroup.Standalone, BuildTarget.StandaloneLinux64))
            {
                Debug.LogError("[StartScreenDeploy] Linux build support not installed. Install it first.");
                return false;
            }

            // Both scenes: a start screen that cannot start a game is not a
            // deployable build.
            string[] scenes = File.Exists(GameplayScenePath)
                ? new[] { ScenePath, GameplayScenePath }
                : new[] { ScenePath };

            // RAM-lean PlayerSettings captured to a snapshot so we can restore them.
            using var restore = new PlayerSettingsSnapshot();

            // Lowest-quality asset streaming / memory profile for the deploy.
            PlayerSettings.stripEngineCode = true;
            PlayerSettings.runInBackground = false;
            PlayerSettings.usePlayerLog = false;
            PlayerSettings.SetUseDefaultGraphicsAPIs(BuildTarget.StandaloneLinux64, true);

            var options = new BuildPlayerOptions
            {
                scenes = scenes,
                locationPathName = BuildPath,
                target = BuildTarget.StandaloneLinux64,
                options = BuildOptions.None
            };

            var report = BuildPipeline.BuildPlayer(options);
            if (report.summary.result == UnityEditor.Build.Reporting.BuildResult.Succeeded)
            {
                Debug.Log($"[StartScreenDeploy] Build OK: {BuildPath} ({report.summary.totalSize} bytes)");
                return true;
            }
            Debug.LogError($"[StartScreenDeploy] Build failed: {report.summary.result}");
            return false;
        }
    }

    /// <summary>Restores globally-changed PlayerSettings after a build, so the
    /// main (full) game build keeps its own intended settings.</summary>
    internal sealed class PlayerSettingsSnapshot : IDisposable
    {
        private readonly bool _stripEngineCode;
        private readonly bool _runInBackground;
        private readonly bool _usePlayerLog;

        public PlayerSettingsSnapshot()
        {
            _stripEngineCode = PlayerSettings.stripEngineCode;
            _runInBackground = PlayerSettings.runInBackground;
            _usePlayerLog = PlayerSettings.usePlayerLog;
        }

        public void Dispose()
        {
            PlayerSettings.stripEngineCode = _stripEngineCode;
            PlayerSettings.runInBackground = _runInBackground;
            PlayerSettings.usePlayerLog = _usePlayerLog;
        }
    }
}
#endif