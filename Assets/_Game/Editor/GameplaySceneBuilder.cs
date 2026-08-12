#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Core;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Constructs Assets/Scenes/Gameplay.unity from scratch: a camera, a fully wired
    /// GameBootstrap, and the vertical slice's HUD widgets.
    ///
    /// The scene is generated rather than hand-authored so the wiring is reviewable
    /// as C# instead of serialized YAML, and so it can be rebuilt if it breaks.
    /// Refuses to save a scene in which a field that should be wired is null.
    /// </summary>
    public static class GameplaySceneBuilder
    {
        const string ScenePath   = "Assets/Scenes/Gameplay.unity";
        const string SamplePath  = "Assets/Scenes/SampleScene.unity";
        const string CatalogRoot = "Assets/_Game/Data/Generated/Catalogs";
        const string UiRoot      = "Assets/_Game/UI";

        /// <summary>HUD widgets this slice wires.</summary>
        public static readonly string[] HudSliceAllowlist =
        {
            "_needsBar", "_dosimeterHud", "_eventModalUi", "_diegeticHud",
            "_radiationPhaseIndicator", "_phantomMemoryVignette",
            "_hypervigilanceIndicator", "_moralBranchDisplay",
            "_keepsakeSlotUi", "_memorialWallUi",
            "_terminalPrognosisBanner", "_addictionDetoxIndicator"
        };

        /// <summary>
        /// Deliberately unwired until their widgets land. Listed explicitly rather
        /// than skipping all nulls, so the gate stays meaningful: a widget that
        /// silently fails to wire is still caught. As each lands it moves to
        /// <see cref="HudSliceAllowlist"/>, and this empties when the HUD is done.
        /// </summary>
        public static readonly string[] HudExpectedUnwired =
        {
            "_healthTrajectoryHud", "_geigerAudioHook", "_environmentStatusHud",
            "_mapKnowledgeHud", "_tradeScreenUi", "_powerGridHud", "_mapScreenUi",
            "_workbenchUi", "_hatchDefenseHud", "_roomAssignmentHud",
            "_radioInterceptHud", "_factionRadioVoHook", "_journalBookUi",
            "_inventoryStripUi", "_endgameSummaryUi", "_internalHorrorHud",
            "_expeditionEncounterLogHud",
            "_scavengeDispatchHud", "_overflowCrateHud", "_fieldGearLoadoutHud",
            "_bunkerRationingHud", "_waterPurificationHud", "_airHeatManagementHud",
            "_bunkerMaintenanceHud", "_survivorTaskBoardHud",
            "_moralChronicleUi", "_tutorialOverlay",
            // Batch-20 widgets — painted via DiegeticHud; component refs land later
            "_radiationDosimeterWidget", "_geigerSweepGauge", "_airFilterIntegrityBar",
            "_falloutStormWarningBanner", "_survivorPortraitCard", "_moralDecayMeter",
            "_rationAllocationDial", "_waterPurityGauge", "_temperatureReadoutWidget",
            "_powerFlowSchematic", "_factionPressureRing", "_expeditionCountdownTimer",
            "_radioSignalStrengthBar", "_craftQueueStrip", "_alertToastNotification",
            "_bunkerFloorMapMiniature", "_dayNightArcClock", "_bloodTypeIndicator",
            "_lootHaulTicker", "_endgameVictoryPathTracker"
        };

        /// <summary>
        /// Command-line / CI batchmode entry point:
        /// -executeMethod AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene
        /// </summary>
        [MenuItem("Tools/ASHFALL/Build Gameplay Scene")]
        public static void BuildGameplayScene()
        {
            var scene = EditorSceneManager.NewScene(NewSceneSetup.EmptyScene, NewSceneMode.Single);

            BuildCamera();

            var hud = BuildHud();
            var overlay = new GameObject("Diagnostics").AddComponent<DiagnosticsOverlay>();
            var bootstrap = new GameObject("GameBootstrap").AddComponent<GameBootstrap>();

            // Same GameObject, not a child: PlayerInputHandler.Awake resolves its
            // bootstrap with GetComponent, and Update early-returns when that came
            // back null. On a child it would sit there consuming nothing, silently.
            bootstrap.gameObject.AddComponent<PlayerInputHandler>();

            WireBootstrap(bootstrap, hud, overlay);
            WireDiagnosticsOverlay(overlay, bootstrap);

            AssertWired(bootstrap, Array.Empty<string>());
            AssertWired(overlay, Array.Empty<string>());
            AssertWired(hud, HudExpectedUnwired);

            EditorSceneManager.MarkSceneDirty(scene);
            if (!EditorSceneManager.SaveScene(scene, ScenePath))
                throw new SceneBuildException($"Failed to save {ScenePath}");

            RegisterInBuildSettings();
            RetireSampleScene();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ASHFALL] Built {ScenePath} with a fully wired GameBootstrap.");
        }

        // No Global Light 2D: the slice renders no sprites, and UI Toolkit draws
        // independently of the 2D light system. Adding one would pull a URP package
        // reference into this editor assembly for no visible effect. Add it with the
        // sprite layer.
        static void BuildCamera()
        {
            var go = new GameObject("Main Camera");
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.035f, 0.043f, 0.047f); // --ink #090b0c
            go.tag = "MainCamera";
            go.transform.position = new Vector3(0f, 0f, -10f);
        }

        static HUD BuildHud()
        {
            var root = new GameObject("HUD");
            var hud = root.AddComponent<HUD>();

            var needsBar   = Child<NeedsBar>(root, "NeedsBar");
            var dosimeter  = Child<DosimeterHUD>(root, "DosimeterHUD");
            var eventModal = Child<EventModalUI>(root, "EventModalUI");
            var diegetic   = Child<DiegeticHudController>(root, "DiegeticHud");

            WireDiegeticHud(diegetic);
            var diegeticDoc = diegetic.GetComponent<UIDocument>();

            // Phase 11 — expansion HUD widgets share the DiegeticHud UIDocument
            var radPhase     = Child<RadiationPhaseIndicator>(root, "RadiationPhaseIndicator");
            var phantom      = Child<PhantomMemoryVignette>(root, "PhantomMemoryVignette");
            var hyper        = Child<HypervigilanceIndicator>(root, "HypervigilanceIndicator");
            var moralBranch  = Child<MoralBranchDisplay>(root, "MoralBranchDisplay");
            var keepsake     = Child<KeepsakeSlotUI>(root, "KeepsakeSlotUI");
            var memorial     = Child<MemorialWallUI>(root, "MemorialWallUI");
            var terminal     = Child<TerminalPrognosisBanner>(root, "TerminalPrognosisBanner");
            var addiction    = Child<AddictionDetoxIndicator>(root, "AddictionDetoxIndicator");

            BindPhase11Document(radPhase, diegeticDoc);
            BindPhase11Document(phantom, diegeticDoc);
            BindPhase11Document(hyper, diegeticDoc);
            BindPhase11Document(moralBranch, diegeticDoc);
            BindPhase11Document(keepsake, diegeticDoc);
            BindPhase11Document(memorial, diegeticDoc);
            BindPhase11Document(terminal, diegeticDoc);
            BindPhase11Document(addiction, diegeticDoc);

            var so = new SerializedObject(hud);
            so.FindProperty("_needsBar").objectReferenceValue     = needsBar;
            so.FindProperty("_dosimeterHud").objectReferenceValue = dosimeter;
            so.FindProperty("_eventModalUi").objectReferenceValue = eventModal;
            so.FindProperty("_diegeticHud").objectReferenceValue  = diegetic;
            so.FindProperty("_radiationPhaseIndicator").objectReferenceValue = radPhase;
            so.FindProperty("_phantomMemoryVignette").objectReferenceValue   = phantom;
            so.FindProperty("_hypervigilanceIndicator").objectReferenceValue = hyper;
            so.FindProperty("_moralBranchDisplay").objectReferenceValue      = moralBranch;
            so.FindProperty("_keepsakeSlotUi").objectReferenceValue          = keepsake;
            so.FindProperty("_memorialWallUi").objectReferenceValue          = memorial;
            so.FindProperty("_terminalPrognosisBanner").objectReferenceValue = terminal;
            so.FindProperty("_addictionDetoxIndicator").objectReferenceValue = addiction;
            so.ApplyModifiedPropertiesWithoutUndo();

            return hud;
        }

        static void BindPhase11Document(Component widget, UIDocument document)
        {
            if (widget == null || document == null) return;
            var so = new SerializedObject(widget);
            var prop = so.FindProperty("_document");
            if (prop == null) return;
            prop.objectReferenceValue = document;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>The UIDocument lives on the DiegeticHud child, which is where
        /// HUD.EnsureDiegeticDocument looks for it first.</summary>
        static void WireDiegeticHud(DiegeticHudController diegetic)
        {
            var document = diegetic.gameObject.AddComponent<UIDocument>();
            var panel = Load<PanelSettings>($"{UiRoot}/DiegeticHudPanelSettings.asset");
            var uxml  = Load<VisualTreeAsset>($"{UiRoot}/DiegeticHud.uxml");
            var uss   = Load<StyleSheet>($"{UiRoot}/DiegeticHud.uss");

            document.panelSettings = panel;
            document.visualTreeAsset = uxml;

            var so = new SerializedObject(diegetic);
            so.FindProperty("_document").objectReferenceValue      = document;
            so.FindProperty("_panelSettings").objectReferenceValue = panel;
            so.FindProperty("_uxml").objectReferenceValue          = uxml;
            so.FindProperty("_uss").objectReferenceValue           = uss;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>The overlay holds a back-reference to the bootstrap. Left null it
        /// still renders, but only to say "Bootstrap not assigned" -- which would make
        /// the slice's main readout useless without failing anything.</summary>
        static void WireDiagnosticsOverlay(DiagnosticsOverlay overlay, GameBootstrap bootstrap)
        {
            var so = new SerializedObject(overlay);
            so.FindProperty("_bootstrap").objectReferenceValue = bootstrap;
            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static T Child<T>(GameObject parent, string name) where T : Component
        {
            var go = new GameObject(name);
            go.transform.SetParent(parent.transform, false);
            return go.AddComponent<T>();
        }

        static void WireBootstrap(GameBootstrap bootstrap, HUD hud, DiagnosticsOverlay overlay)
        {
            var so = new SerializedObject(bootstrap);

            Assign(so, "_needsProfile",     $"{CatalogRoot}/NeedsProfile.asset");
            Assign(so, "_lightProfile",     $"{CatalogRoot}/LightProfile.asset");
            Assign(so, "_seasonProfile",    $"{CatalogRoot}/SeasonProfile.asset");
            Assign(so, "_itemCatalog",      $"{CatalogRoot}/ItemCatalog.asset");
            Assign(so, "_recipeCatalog",    $"{CatalogRoot}/RecipeCatalog.asset");
            Assign(so, "_eventCatalog",     $"{CatalogRoot}/GameEventCatalog.asset");
            Assign(so, "_locationCatalog",  $"{CatalogRoot}/LocationCatalog.asset");
            Assign(so, "_radioCatalog",     $"{CatalogRoot}/RadioCatalog.asset");
            Assign(so, "_worldPhaseConfig", $"{CatalogRoot}/WorldPhaseConfig.asset");
            Assign(so, "_lootTable",        $"{CatalogRoot}/LootTable.asset");
            Assign(so, "_flashpointSequence",
                "Assets/_Game/Data/Generated/Flashpoint/DefaultFlashpointSequence.asset");
            Assign(so, "_mentalBreakCatalog",
                "Assets/_Game/Data/Generated/Survivor/DefaultMentalBreakCatalog.asset");

            so.FindProperty("_hud").objectReferenceValue = hud;
            so.FindProperty("_diagnosticsOverlay").objectReferenceValue = overlay;

            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static T Load<T>(string assetPath) where T : UnityEngine.Object
        {
            var asset = AssetDatabase.LoadAssetAtPath<T>(assetPath);
            if (asset == null)
                throw new SceneBuildException($"Missing {typeof(T).Name} at {assetPath}");
            return asset;
        }

        static void Assign(SerializedObject so, string fieldName, string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(assetPath);
            if (asset == null)
                throw new SceneBuildException(
                    $"Missing asset for {fieldName}: {assetPath}. Run Tools/ASHFALL/Import All Data " +
                    "then Tools/ASHFALL/Generate Catalogs first.");

            var property = so.FindProperty(fieldName);
            if (property == null)
                throw new SceneBuildException($"No serialized field named {fieldName}");

            property.objectReferenceValue = asset;
        }

        /// <summary>Fail before writing rather than leaving a half-wired scene on disk.
        /// GameBootstrap null-conditionals its catalogs (_itemCatalog?.GetById(...)),
        /// so a missing one degrades into a silently empty game with no exception --
        /// the runtime will never tell you. This is the only place that will.</summary>
        static void AssertWired(UnityEngine.Object target, string[] expectedUnwired)
        {
            var skip = new HashSet<string>(expectedUnwired);
            var missing = new List<string>();

            var so = new SerializedObject(target);
            var it = so.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.propertyType != SerializedPropertyType.ObjectReference) continue;
                // Unity internals (m_Script, m_PrefabInstance, m_CorrespondingSourceObject...).
                // The prefab ones are legitimately null on a plain scene object.
                if (it.name.StartsWith("m_", StringComparison.Ordinal)) continue;
                if (skip.Contains(it.name)) continue;
                if (it.objectReferenceValue == null) missing.Add(it.name);
            }

            if (missing.Count > 0)
                throw new SceneBuildException(
                    $"{target.GetType().Name} has unassigned fields, refusing to save the scene: " +
                    string.Join(", ", missing));
        }

        static void RegisterInBuildSettings()
        {
            var scenes = EditorBuildSettings.scenes
                .Where(s => s.path != SamplePath && s.path != ScenePath)
                .ToList();
            scenes.Add(new EditorBuildSettingsScene(ScenePath, true));
            EditorBuildSettings.scenes = scenes.ToArray();
        }

        static void RetireSampleScene()
        {
            if (AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(SamplePath) != null)
                AssetDatabase.DeleteAsset(SamplePath);
        }
    }

    public class SceneBuildException : Exception
    {
        public SceneBuildException(string message) : base(message) { }
    }
}
#endif
