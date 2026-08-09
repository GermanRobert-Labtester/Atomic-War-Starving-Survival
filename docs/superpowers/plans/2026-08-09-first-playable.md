# First Playable Implementation Plan

> **For agentic workers:** REQUIRED SUB-SKILL: Use superpowers:subagent-driven-development (recommended) or superpowers:executing-plans to implement this plan task-by-task. Steps use checkbox (`- [ ]`) syntax for tracking.

**Goal:** Make NEW EXPEDITION on the main menu load a real gameplay scene in which `GameBootstrap` boots fully wired, the clock advances, needs decay, the dosimeter reads, and save/load round-trips.

**Architecture:** Everything is produced by editor code, not hand-authoring. Two new `Tools/ASHFALL/` commands — `CatalogGenerator` and `GameplaySceneBuilder` — create the 10 missing catalog/profile assets and construct `Assets/Scenes/Gameplay.unity` with all 17 `GameBootstrap` serialized fields assigned, refusing to save if a field that should be wired is null. A new PlayMode test loads that scene for real, closing the gap that let an empty scene ship green.

**Tech Stack:** Unity 6000.5.5f1, C#, URP 2D, UI Toolkit, NUnit + Unity Test Framework, ScriptableObject + JSON data pipeline.

## Global Constraints

- Unity version is pinned to **6000.5.5f1** at `/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity`. Do not change `ProjectSettings/ProjectVersion.txt`; CI asserts this exact string.
- **Never pass `-quit` together with `-runTests`.** Unity exits before the runner starts and silently writes no results XML. `-quit` is fine for compile-only runs.
- Finish all `.cs` edits before launching a test run. Editing source mid-run triggers a recompile that can hang the editor with no log output; kill and re-run instead of waiting.
- Delete `*.xml` / `*.log` run artifacts afterwards. The repo root already holds ~150 MB of stale ones; do not add more.
- All ids are **snake_case**. Never invent an id that is not already in `Assets/StreamingAssets/Data/*.json`.
- Editor-only code lives under `Assets/_Game/Editor/` in namespace `AtomicWar._Game.Editor` and must be guarded so it never ships in a player build.
- Every new editor command must be callable both from the `Tools/ASHFALL/` menu and as a static `-executeMethod` entry point.
- Generated assets are written under `Assets/_Game/Data/Generated/`. Regeneration must preserve asset GUIDs so scene references survive.
- Tone rules for any user-facing string: cold, exhausted, human, restrained. No magic, no real countries/wars/people.

---

### Task 1: Import radio broadcasts

`JsonDataImporter` handles items, recipes, survivors, locations and events. It has **no radio handling at all** — `grep -n "radio" Assets/_Game/Editor/JsonDataImporter.cs` returns nothing. So `radio.json`'s 12 entries never become assets, and `RadioCatalogSO` would have nothing to aggregate in Task 2.

`radio.json` maps 1:1 onto `RadioBroadcastSO`: the key union across all 12 entries is exactly `id`, `minDay`, `maxDay`, `message`, `triggerEventId`.

**Files:**
- Modify: `Assets/_Game/Editor/JsonDataImporter.cs`
- Test: `Assets/Tests/EditMode/RadioImportTests.cs` (create)

**Interfaces:**
- Consumes: `FindOrCreate<T>(string folder, string id)` and `EnsureDirectory(string relativePath)`, the existing private static helpers at `JsonDataImporter.cs:632` and `:645`.
- Produces: `public static List<RadioBroadcastSO> ImportRadio(List<RadioJson> broadcasts)` and the `[MenuItem("Tools/ASHFALL/Import Radio")] public static void ImportRadioMenu()` entry point. Task 2 relies on the output assets existing at `Assets/_Game/Data/Generated/Radio/<id>.asset`.

- [ ] **Step 1: Write the failing test**

Create `Assets/Tests/EditMode/RadioImportTests.cs`:

```csharp
using System.Collections.Generic;
using NUnit.Framework;
using AtomicWar._Game.Data;
using AtomicWar._Game.Editor;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class RadioImportTests
    {
        [Test]
        public void ImportRadio_MapsEveryJsonFieldOntoTheScriptableObject()
        {
            var json = new List<JsonDataImporter.RadioJson>
            {
                new JsonDataImporter.RadioJson
                {
                    id = "radio_test_signal",
                    minDay = 4,
                    maxDay = 9,
                    message = "Static. Then nothing.",
                    triggerEventId = "filter_failure"
                }
            };

            List<RadioBroadcastSO> assets = JsonDataImporter.ImportRadio(json);

            Assert.AreEqual(1, assets.Count);
            Assert.AreEqual("radio_test_signal", assets[0].id);
            Assert.AreEqual(4, assets[0].minDay);
            Assert.AreEqual(9, assets[0].maxDay);
            Assert.AreEqual("Static. Then nothing.", assets[0].message);
            Assert.AreEqual("filter_failure", assets[0].triggerEventId);
        }
    }
}
```

- [ ] **Step 2: Run the test to verify it fails**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testFilter "AtomicWar.Tests.EditMode.RadioImportTests" \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log"
```

Expected: FAIL — compile error, `JsonDataImporter` has no `RadioJson` and no `ImportRadio`.

- [ ] **Step 3: Add the DTO and importer**

The existing JSON DTOs are **nested private** classes inside
`JsonDataImporter` (`ItemJson` at line 231, `RecipeJson` at 272, `EventJson`
at 309). Add `RadioJson` in the same block, but **public** — the test
references it as `JsonDataImporter.RadioJson`:

```csharp
        [Serializable]
        public class RadioJson
        {
            public string id;
            public int minDay;
            public int maxDay = -1;
            public string message;
            public string triggerEventId;
        }
```

Add the import method next to `ImportItems` (line 490). Note that
`ImportItems` is private but `ImportRadio` is **public static** — this is
deliberate, so the test can call it directly without going through the
filesystem. Do not "tidy" it back to private.

```csharp
public static List<RadioBroadcastSO> ImportRadio(List<RadioJson> broadcasts)
{
    var result = new List<RadioBroadcastSO>();
    foreach (var json in broadcasts)
    {
        var so = FindOrCreate<RadioBroadcastSO>(Path.Combine(OutputRoot, "Radio"), json.id);
        so.id             = json.id;
        so.minDay         = json.minDay;
        so.maxDay         = json.maxDay;
        so.message        = json.message;
        so.triggerEventId = json.triggerEventId;
        EditorUtility.SetDirty(so);
        result.Add(so);
    }
    return result;
}
```

Add the menu entry alongside `ImportItemsMenu`:

```csharp
[MenuItem("Tools/ASHFALL/Import Radio")]
public static void ImportRadioMenu()
{
    var errors = new List<string>();
    var broadcasts = LoadAndValidate<RadioJson>(Path.Combine(DataRoot, "radio.json"), "radio", errors);
    if (errors.Count > 0) { LogErrors(errors); return; }
    EnsureDirectory(Path.Combine(OutputRoot, "Radio"));
    ImportRadio(broadcasts);
    AssetDatabase.SaveAssets();
    AssetDatabase.Refresh();
    Debug.Log($"[ASHFALL] Imported {broadcasts.Count} radio broadcasts.");
}
```

- [ ] **Step 4: Wire radio into `ImportAll`**

The menu entry `ImportAllMenu` (line 35) delegates to `ImportAll()` (line
61) — edit `ImportAll()`, not the menu wrapper. Add the load alongside the
other five:

```csharp
var radio = LoadAndValidate<RadioJson>(Path.Combine(DataRoot, "radio.json"), "radio", errors);
```

Add `EnsureDirectory(Path.Combine(OutputRoot, "Radio"));` to the directory block at lines 94-98, and `ImportRadio(radio);` to the import block that begins at line 101.

- [ ] **Step 5: Run the test to verify it passes**

Same command as Step 2. Expected: PASS, 1 test.

- [ ] **Step 6: Commit**

```bash
rm -f em.xml em.log
git add Assets/_Game/Editor/JsonDataImporter.cs Assets/Tests/EditMode/RadioImportTests.cs
git commit -m "feat(data): import radio.json into RadioBroadcastSO assets"
```

---

### Task 2: Generate the catalog and profile assets

Ten of `GameBootstrap`'s twelve `[SerializeField]` data references have no asset. This task creates them.

**Files:**
- Create: `Assets/_Game/Editor/CatalogGenerator.cs`
- Test: `Assets/Tests/EditMode/CatalogGeneratorTests.cs`

**Interfaces:**
- Consumes: the individual assets under `Assets/_Game/Data/Generated/{Items,Recipes,Events,Locations,Radio}/` written by `JsonDataImporter` (Task 1 added `Radio/`).
- Produces: `public static void GenerateAll()` — the `-executeMethod` entry point — writing assets to `Assets/_Game/Data/Generated/Catalogs/`. Task 3 runs it; Task 4 loads its output by path.

The ten assets split into two kinds, handled differently:

| Kind | Assets | Rule |
| --- | --- | --- |
| JSON-backed catalogs | `ItemCatalogSO`, `RecipeCatalogSO`, `GameEventCatalogSO`, `LocationCatalogSO`, `RadioCatalogSO` | Refresh list in place on every run |
| Tuning profiles | `NeedsProfile`, `LightProfile`, `SeasonProfile`, `WorldPhaseConfigSO`, `LootTableSO` | Create at C# defaults **only if absent** — never overwrite |

- [ ] **Step 1: Write the failing test**

Create `Assets/Tests/EditMode/CatalogGeneratorTests.cs`:

```csharp
using NUnit.Framework;
using UnityEditor;
using AtomicWar._Game.Data;
using AtomicWar._Game.Editor;
using AtomicWar._Game.Survivors;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class CatalogGeneratorTests
    {
        const string Root = "Assets/_Game/Data/Generated/Catalogs";

        [Test]
        public void GenerateAll_CreatesEveryCatalogAndProfileAsset()
        {
            CatalogGenerator.GenerateAll();

            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RecipeCatalogSO>($"{Root}/RecipeCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<GameEventCatalogSO>($"{Root}/GameEventCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LocationCatalogSO>($"{Root}/LocationCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<RadioCatalogSO>($"{Root}/RadioCatalog.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<NeedsProfile>($"{Root}/NeedsProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LightProfile>($"{Root}/LightProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<SeasonProfile>($"{Root}/SeasonProfile.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<WorldPhaseConfigSO>($"{Root}/WorldPhaseConfig.asset"));
            Assert.IsNotNull(AssetDatabase.LoadAssetAtPath<LootTableSO>($"{Root}/LootTable.asset"));
        }

        [Test]
        public void GenerateAll_PopulatesItemCatalogFromImportedAssets()
        {
            CatalogGenerator.GenerateAll();

            var catalog = AssetDatabase.LoadAssetAtPath<ItemCatalogSO>($"{Root}/ItemCatalog.asset");
            Assert.Greater(catalog.items.Count, 0, "item catalog should aggregate imported ItemDefinition assets");
            Assert.IsNotNull(catalog.GetById("clean_water"), "known seed item should resolve by id");
        }

        [Test]
        public void GenerateAll_DoesNotOverwriteHandTunedProfileValues()
        {
            CatalogGenerator.GenerateAll();

            var profile = AssetDatabase.LoadAssetAtPath<NeedsProfile>($"{Root}/NeedsProfile.asset");
            profile.hungerPerHour = 99f;
            EditorUtility.SetDirty(profile);
            AssetDatabase.SaveAssets();

            CatalogGenerator.GenerateAll();

            var reloaded = AssetDatabase.LoadAssetAtPath<NeedsProfile>($"{Root}/NeedsProfile.asset");
            Assert.AreEqual(99f, reloaded.hungerPerHour, "regeneration must not clobber tuned profile values");

            reloaded.hungerPerHour = 2f;
            EditorUtility.SetDirty(reloaded);
            AssetDatabase.SaveAssets();
        }

        [Test]
        public void GenerateAll_SeedsLootTableValidFromTheEarliestPhase()
        {
            CatalogGenerator.GenerateAll();

            var loot = AssetDatabase.LoadAssetAtPath<LootTableSO>($"{Root}/LootTable.asset");
            Assert.Greater(loot.GetValidEntries(WorldPhase.PreWar).Count, 0,
                "seeded loot must be reachable in the earliest phase");
        }
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testFilter "AtomicWar.Tests.EditMode.CatalogGeneratorTests" \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log"
```

Expected: FAIL — compile error, `CatalogGenerator` does not exist.

- [ ] **Step 3: Implement `CatalogGenerator`**

Create `Assets/_Game/Editor/CatalogGenerator.cs`:

```csharp
#if UNITY_EDITOR
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
    /// requires. JSON-backed catalogs aggregate the individual assets written by
    /// JsonDataImporter and are refreshed on every run; tuning profiles have no
    /// JSON source and are created once at their C# defaults, never overwritten.
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

            Refresh<ItemCatalogSO>("ItemCatalog",           c => c.items      = items);
            Refresh<RecipeCatalogSO>("RecipeCatalog",       c => c.recipes    = recipes);
            Refresh<GameEventCatalogSO>("GameEventCatalog", c => c.events     = events);
            Refresh<LocationCatalogSO>("LocationCatalog",   c => c.locations  = locations);
            Refresh<RadioCatalogSO>("RadioCatalog",         c => c.broadcasts = radio);

            // Tuning profiles: C# field defaults are the balanced values.
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
                    // PreWar is the lowest WorldPhase value and GetValidEntries tests
                    // `currentPhase >= phaseRequirement`, so this stays valid in every
                    // phase. The C# default (CivilWar) would hide all loot in PreWar.
                    phaseRequirement = WorldPhase.PreWar
                }).ToList();
            });

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log($"[ASHFALL] Catalogs generated: {items.Count} items, {recipes.Count} recipes, " +
                      $"{events.Count} events, {locations.Count} locations, {radio.Count} broadcasts.");
        }

        static void Refresh<T>(string assetName, System.Action<T> fill) where T : ScriptableObject
        {
            var asset = LoadOrCreate<T>(assetName);
            fill(asset);
            EditorUtility.SetDirty(asset);
        }

        static void CreateIfAbsent<T>(string assetName, System.Action<T> seed = null)
            where T : ScriptableObject
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
                Debug.LogWarning($"[ASHFALL] {folder} does not exist — run Tools/ASHFALL/Import All Data first.");
                return new List<T>();
            }

            return AssetDatabase.FindAssets($"t:{typeof(T).Name}", new[] { folder })
                .Select(AssetDatabase.GUIDToAssetPath)
                .Select(AssetDatabase.LoadAssetAtPath<T>)
                .Where(a => a != null)
                .OrderBy(a => a.name)               // stable order → stable YAML diffs
                .ToList();
        }

        static void EnsureFolder(string path)
        {
            if (AssetDatabase.IsValidFolder(path))
                return;

            var parts = path.Split('/');
            var current = parts[0];
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
```

- [ ] **Step 4: Run the tests to verify they pass**

Same command as Step 2. Expected: PASS, 4 tests.

If `PopulatesItemCatalogFromImportedAssets` fails on `GetById("clean_water")`, the importer has not been run yet — that is Task 3. Run `Tools/ASHFALL/Import All Data` first, then re-run.

- [ ] **Step 5: Commit**

```bash
rm -f em.xml em.log
git add Assets/_Game/Editor/CatalogGenerator.cs Assets/Tests/EditMode/CatalogGeneratorTests.cs
git commit -m "feat(data): generate catalog and tuning-profile assets from imported data"
```

---

### Task 3: Re-import all data and commit the generated assets

`items.json` holds 321 entries; `Generated/Items/` holds 19. This is staleness, not a filter — `ImportItemsMenu` passes the whole parsed list to `ImportItems(items)` with no predicate. Same for events (39), survivors (72), locations (5), recipes (16), radio (12).

**Files:**
- Create: `Assets/_Game/Data/Generated/{Items,Recipes,Survivors,Locations,Events,Radio}/*.asset` (generated)
- Create: `Assets/_Game/Data/Generated/Catalogs/*.asset` (generated)

**Interfaces:**
- Consumes: `JsonDataImporter.ImportAll` (Task 1 extended it with radio) and `CatalogGenerator.GenerateAll` (Task 2).
- Produces: on-disk assets at the paths Task 4 loads.

- [ ] **Step 1: Run the importer headlessly**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . \
  -executeMethod AtomicWar._Game.Editor.JsonDataImporter.ImportAllMenu \
  -logFile "$(pwd)/import.log"
```

`-quit` is correct here — this is not a test run.

- [ ] **Step 2: Verify the counts match the JSON**

```bash
ls Assets/_Game/Data/Generated/Items/*.asset     | wc -l   # expect 321
ls Assets/_Game/Data/Generated/Events/*.asset    | wc -l   # expect 39
ls Assets/_Game/Data/Generated/Survivors/*.asset | wc -l   # expect 72
ls Assets/_Game/Data/Generated/Recipes/*.asset   | wc -l   # expect 16
ls Assets/_Game/Data/Generated/Locations/*.asset | wc -l   # expect 5
ls Assets/_Game/Data/Generated/Radio/*.asset     | wc -l   # expect 12
```

If any count is short, read `import.log` for the validation errors — `LoadAndValidate` aborts the whole import when it collects any error, so a single bad id blocks everything.

- [ ] **Step 3: Generate the catalogs**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . \
  -executeMethod AtomicWar._Game.Editor.CatalogGenerator.GenerateAll \
  -logFile "$(pwd)/catalogs.log"
```

- [ ] **Step 4: Verify all ten assets exist**

```bash
ls Assets/_Game/Data/Generated/Catalogs/
```

Expect exactly: `GameEventCatalog.asset`, `ItemCatalog.asset`, `LightProfile.asset`, `LocationCatalog.asset`, `LootTable.asset`, `NeedsProfile.asset`, `RadioCatalog.asset`, `RecipeCatalog.asset`, `SeasonProfile.asset`, `WorldPhaseConfig.asset` (plus `.meta` files).

- [ ] **Step 5: Commit**

```bash
rm -f import.log catalogs.log
git add Assets/_Game/Data/Generated
git commit -m "chore(data): re-import all authored data and generate catalogs"
```

---

### Task 4: Build the gameplay scene

**Files:**
- Create: `Assets/_Game/Editor/GameplaySceneBuilder.cs`
- Create: `Assets/Scenes/Gameplay.unity` (generated)
- Modify: `Assets/_Game/UI/MainMenu/MainMenuController.cs:29`
- Modify: `Assets/Scenes/StartScreen.unity` (serialized field value)
- Delete: `Assets/Scenes/SampleScene.unity`
- Test: `Assets/Tests/EditMode/GameplaySceneBuilderTests.cs`

**Interfaces:**
- Consumes: the ten assets under `Assets/_Game/Data/Generated/Catalogs/` (Task 3), plus `DefaultFlashpointSequence.asset` and `DefaultMentalBreakCatalog.asset` which already exist.
- Produces: `public static void BuildGameplayScene()` — the `-executeMethod` entry point — and the scene at `Assets/Scenes/Gameplay.unity`. Task 5 loads that scene by name.

**The null-check gate.** `GameBootstrap` has 17 serialized fields (all in `GameBootstrap.cs:36-58`); every one must be non-null. `HUD` has 23 serialized fields — 21 object references at `HUD.cs:21-41`, plus `_debugToggleKey` and `_debugModeEnabled`, which are value types and cannot be null. This slice wires 4 of the 21: `_needsBar`, `_dosimeterHud`, `_eventModalUi`, `_diegeticHud`. The other 17 go in an explicit `ExpectedUnwired` set so the gate stays meaningful — as each widget lands it moves out of that set, which empties when the HUD is complete.

This matters because `GameBootstrap` uses null-conditional access on its catalogs throughout (`_itemCatalog?.GetById(...)` at `GameBootstrap.InitLate.cs:81`, `InitFoundation.cs:250` and `:322-323`). A missing catalog degrades into a silently empty game rather than an exception, so the runtime will never tell you.

- [ ] **Step 1: Write the failing test**

Create `Assets/Tests/EditMode/GameplaySceneBuilderTests.cs`:

```csharp
using System.Linq;
using System.Reflection;
using NUnit.Framework;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using AtomicWar._Game.Core;
using AtomicWar._Game.Editor;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.EditMode
{
    [TestFixture]
    public class GameplaySceneBuilderTests
    {
        const string ScenePath = "Assets/Scenes/Gameplay.unity";

        [Test]
        public void BuildGameplayScene_WritesASceneWithAFullyWiredBootstrap()
        {
            GameplaySceneBuilder.BuildGameplayScene();

            Assert.IsTrue(System.IO.File.Exists(ScenePath), "scene file should exist on disk");

            var scene = EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            Assert.IsNotNull(bootstrap, "scene must contain a GameBootstrap");

            foreach (var field in SerializedObjectFields(bootstrap))
                Assert.IsNotNull(field.Value, $"GameBootstrap.{field.Key} must be assigned");

            Assert.IsTrue(scene.isLoaded);
        }

        [Test]
        public void BuildGameplayScene_WiresTheFourSliceHudWidgets()
        {
            GameplaySceneBuilder.BuildGameplayScene();
            EditorSceneManager.OpenScene(ScenePath, OpenSceneMode.Single);

            var hud = Object.FindAnyObjectByType<HUD>();
            Assert.IsNotNull(hud, "scene must contain a HUD");

            var fields = SerializedObjectFields(hud);
            foreach (var name in GameplaySceneBuilder.HudSliceAllowlist)
                Assert.IsNotNull(fields[name], $"HUD.{name} is in the slice and must be assigned");
        }

        [Test]
        public void BuildGameplayScene_RegistersTheSceneAndRetiresSampleScene()
        {
            GameplaySceneBuilder.BuildGameplayScene();

            var registered = EditorBuildSettings.scenes.Select(s => s.path).ToList();
            CollectionAssert.Contains(registered, ScenePath);
            CollectionAssert.DoesNotContain(registered, "Assets/Scenes/SampleScene.unity");
            Assert.IsFalse(System.IO.File.Exists("Assets/Scenes/SampleScene.unity"));
        }

        static System.Collections.Generic.Dictionary<string, Object> SerializedObjectFields(Object target)
        {
            var result = new System.Collections.Generic.Dictionary<string, Object>();
            var so = new SerializedObject(target);
            var it = so.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.propertyType == SerializedPropertyType.ObjectReference)
                    result[it.name] = it.objectReferenceValue;
            }
            return result;
        }
    }
}
```

- [ ] **Step 2: Run the tests to verify they fail**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testFilter "AtomicWar.Tests.EditMode.GameplaySceneBuilderTests" \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log"
```

Expected: FAIL — compile error, `GameplaySceneBuilder` does not exist.

- [ ] **Step 3: Implement `GameplaySceneBuilder`**

Create `Assets/_Game/Editor/GameplaySceneBuilder.cs`:

```csharp
#if UNITY_EDITOR
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using UnityEngine.UIElements;
using AtomicWar._Game.Core;
using AtomicWar._Game.UI;

namespace AtomicWar._Game.Editor
{
    /// <summary>
    /// Constructs Assets/Scenes/Gameplay.unity from scratch: camera, 2D light,
    /// a fully wired GameBootstrap, and the vertical slice's HUD widgets.
    /// Refuses to save a scene in which a field that should be wired is null.
    /// </summary>
    public static class GameplaySceneBuilder
    {
        const string ScenePath   = "Assets/Scenes/Gameplay.unity";
        const string SamplePath  = "Assets/Scenes/SampleScene.unity";
        const string CatalogRoot = "Assets/_Game/Data/Generated/Catalogs";

        /// <summary>HUD widgets this slice wires. Every other object reference on
        /// HUD is in <see cref="HudExpectedUnwired"/>.</summary>
        public static readonly string[] HudSliceAllowlist =
        {
            "_needsBar", "_dosimeterHud", "_eventModalUi", "_diegeticHud"
        };

        /// <summary>Deliberately unwired until their widgets land. Listing them
        /// explicitly (rather than skipping all nulls) keeps the gate meaningful:
        /// a widget that silently fails to wire is still caught.</summary>
        public static readonly string[] HudExpectedUnwired =
        {
            "_healthTrajectoryHud", "_geigerAudioHook", "_environmentStatusHud",
            "_mapKnowledgeHud", "_tradeScreenUi", "_powerGridHud", "_mapScreenUi",
            "_workbenchUi", "_hatchDefenseHud", "_roomAssignmentHud",
            "_radioInterceptHud", "_factionRadioVoHook", "_journalBookUi",
            "_inventoryStripUi", "_endgameSummaryUi", "_internalHorrorHud",
            "_expeditionEncounterLogHud"
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
            BuildGlobalLight();

            var hud = BuildHud();
            var overlay = new GameObject("Diagnostics").AddComponent<DiagnosticsOverlay>();
            var bootstrap = new GameObject("GameBootstrap").AddComponent<GameBootstrap>();

            WireBootstrap(bootstrap, hud, overlay);

            AssertWired(bootstrap, expectedUnwired: new string[0]);
            AssertWired(hud, expectedUnwired: HudExpectedUnwired);

            EditorSceneManager.SaveScene(scene, ScenePath);

            RegisterInBuildSettings();
            RetireSampleScene();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();
            Debug.Log($"[ASHFALL] Built {ScenePath} with a fully wired GameBootstrap.");
        }

        static void BuildCamera()
        {
            var go = new GameObject("Main Camera");
            var cam = go.AddComponent<Camera>();
            cam.orthographic = true;
            cam.orthographicSize = 5f;
            cam.clearFlags = CameraClearFlags.SolidColor;
            cam.backgroundColor = new Color(0.035f, 0.043f, 0.047f); // --ink #090b0c
            go.AddComponent<UniversalAdditionalCameraData>();
            go.tag = "MainCamera";
            go.transform.position = new Vector3(0f, 0f, -10f);
        }

        static void BuildGlobalLight()
        {
            var go = new GameObject("Global Light 2D");
            var light = go.AddComponent<Light2D>();
            light.lightType = Light2D.LightType.Global;
            light.intensity = 1f;
        }

        static HUD BuildHud()
        {
            var root = new GameObject("HUD");
            var doc = root.AddComponent<UIDocument>();
            doc.panelSettings = AssetDatabase.LoadAssetAtPath<PanelSettings>(
                "Assets/_Game/UI/DiegeticHudPanelSettings.asset");
            doc.visualTreeAsset = AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(
                "Assets/_Game/UI/DiegeticHud.uxml");

            var hud = root.AddComponent<HUD>();

            var needsBar    = Child<NeedsBar>(root, "NeedsBar");
            var dosimeter   = Child<DosimeterHUD>(root, "DosimeterHUD");
            var eventModal  = Child<EventModalUI>(root, "EventModalUI");
            var diegetic    = Child<DiegeticHudController>(root, "DiegeticHud");

            var so = new SerializedObject(hud);
            so.FindProperty("_needsBar").objectReferenceValue     = needsBar;
            so.FindProperty("_dosimeterHud").objectReferenceValue = dosimeter;
            so.FindProperty("_eventModalUi").objectReferenceValue = eventModal;
            so.FindProperty("_diegeticHud").objectReferenceValue  = diegetic;
            so.ApplyModifiedPropertiesWithoutUndo();

            return hud;
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

            Assign(so, "_needsProfile",       $"{CatalogRoot}/NeedsProfile.asset");
            Assign(so, "_lightProfile",       $"{CatalogRoot}/LightProfile.asset");
            Assign(so, "_seasonProfile",      $"{CatalogRoot}/SeasonProfile.asset");
            Assign(so, "_itemCatalog",        $"{CatalogRoot}/ItemCatalog.asset");
            Assign(so, "_recipeCatalog",      $"{CatalogRoot}/RecipeCatalog.asset");
            Assign(so, "_eventCatalog",       $"{CatalogRoot}/GameEventCatalog.asset");
            Assign(so, "_locationCatalog",    $"{CatalogRoot}/LocationCatalog.asset");
            Assign(so, "_radioCatalog",       $"{CatalogRoot}/RadioCatalog.asset");
            Assign(so, "_worldPhaseConfig",   $"{CatalogRoot}/WorldPhaseConfig.asset");
            Assign(so, "_lootTable",          $"{CatalogRoot}/LootTable.asset");
            Assign(so, "_flashpointSequence",
                "Assets/_Game/Data/Generated/Flashpoint/DefaultFlashpointSequence.asset");
            Assign(so, "_mentalBreakCatalog",
                "Assets/_Game/Data/Generated/Survivor/DefaultMentalBreakCatalog.asset");

            so.FindProperty("_hud").objectReferenceValue = hud;
            so.FindProperty("_diagnosticsOverlay").objectReferenceValue = overlay;

            so.ApplyModifiedPropertiesWithoutUndo();
        }

        static void Assign(SerializedObject so, string fieldName, string assetPath)
        {
            var asset = AssetDatabase.LoadAssetAtPath<Object>(assetPath);
            if (asset == null)
                throw new SceneBuildException(
                    $"Missing asset for {fieldName}: {assetPath}. " +
                    "Run Tools/ASHFALL/Import All Data then Tools/ASHFALL/Generate Catalogs first.");

            so.FindProperty(fieldName).objectReferenceValue = asset;
        }

        static void AssertWired(Object target, string[] expectedUnwired)
        {
            var skip = new HashSet<string>(expectedUnwired);
            var missing = new List<string>();

            var so = new SerializedObject(target);
            var it = so.GetIterator();
            while (it.NextVisible(true))
            {
                if (it.propertyType != SerializedPropertyType.ObjectReference) continue;
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
            if (AssetDatabase.LoadAssetAtPath<Object>(SamplePath) != null)
                AssetDatabase.DeleteAsset(SamplePath);
        }
    }

    public class SceneBuildException : System.Exception
    {
        public SceneBuildException(string message) : base(message) { }
    }
}
#endif
```

- [ ] **Step 4: Repoint the main menu at the new scene**

Two places, both required. Changing only the C# default leaves the menu silently loading the old scene, because `StartScreen.unity` carries its own serialized value.

Edit `Assets/_Game/UI/MainMenu/MainMenuController.cs:29`:

```csharp
[SerializeField] private string _gameplaySceneName = "Gameplay";
```

Then update the serialized value in the scene:

```bash
grep -n "SampleScene" Assets/Scenes/StartScreen.unity
sed -i 's/_gameplaySceneName: SampleScene/_gameplaySceneName: Gameplay/' Assets/Scenes/StartScreen.unity
grep -n "_gameplaySceneName" Assets/Scenes/StartScreen.unity
```

The final `grep` must show `Gameplay`. If the field is absent from the scene YAML entirely, the scene is using the C# default and the edit to line 29 is sufficient.

- [ ] **Step 5: Run the builder**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . \
  -executeMethod AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene \
  -logFile "$(pwd)/scene.log"
```

Expected in `scene.log`: `[ASHFALL] Built Assets/Scenes/Gameplay.unity with a fully wired GameBootstrap.`

If it throws `SceneBuildException`, the message names the exact unassigned fields. Do not weaken the gate to get past it — fix the missing asset.

- [ ] **Step 6: Run the tests to verify they pass**

Same command as Step 2. Expected: PASS, 3 tests.

- [ ] **Step 7: Commit**

```bash
rm -f em.xml em.log scene.log
git add Assets/_Game/Editor/GameplaySceneBuilder.cs \
        Assets/Tests/EditMode/GameplaySceneBuilderTests.cs \
        Assets/Scenes/Gameplay.unity Assets/Scenes/Gameplay.unity.meta \
        Assets/_Game/UI/MainMenu/MainMenuController.cs \
        Assets/Scenes/StartScreen.unity \
        ProjectSettings/EditorBuildSettings.asset
git rm -f Assets/Scenes/SampleScene.unity Assets/Scenes/SampleScene.unity.meta
git commit -m "feat(scene): generate a fully wired Gameplay scene and retire SampleScene"
```

---

### Task 5: PlayMode smoke test over the real scene

Your ~1,100 tests all construct systems directly in C# and never load a scene, so they would stay green against a completely empty one. That is exactly how the current build shipped. This task closes the gap.

`Assets/Tests/PlayMode/GameBootstrapStubTests.cs` is a placeholder whose own doc comment asks for precisely this ("bootstrap wiring, time/needs tick over frames, save/load round-trip"). Replace it.

**Files:**
- Create: `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`
- Delete: `Assets/Tests/PlayMode/GameBootstrapStubTests.cs`

**Interfaces:**
- Consumes: `Assets/Scenes/Gameplay.unity` (Task 4). Relevant runtime API: `GameBootstrap.TimeSystem`, `GameBootstrap.Survivors` (`List<Survivor>`, `GameBootstrap.cs:486`), `GameBootstrap.SaveSystem`; `TimeSystem.TotalElapsedHours` (float) and `TimeSystem.CurrentDay` (int); `Survivor.Needs` (`Needs`, get-only) with public float fields `Hunger`, `Thirst`, `Fatigue`, `Warmth`, `Morale`, `Health`, `Hygiene`; `SaveSystem.Save(string slotId)` and `SaveSystem.Load(string slotId)`, both returning `bool`.
- Produces: nothing consumed downstream.

- [ ] **Step 1: Write the failing test**

Create `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`:

```csharp
using System.Collections;
using NUnit.Framework;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.TestTools;
using AtomicWar._Game.Core;
using AtomicWar._Game.UI;

namespace AtomicWar.Tests.PlayMode
{
    /// <summary>
    /// Loads the real gameplay scene and asserts the simulation actually runs.
    /// The rest of the suite constructs systems directly in C#, so this is the
    /// only test that can catch a broken scene or a null Inspector reference.
    /// </summary>
    [TestFixture]
    public class GameplaySceneSmokeTests
    {
        const string SceneName = "Gameplay";

        [UnitySetUp]
        public IEnumerator LoadGameplayScene()
        {
            yield return SceneManager.LoadSceneAsync(SceneName, LoadSceneMode.Single);
            yield return null; // let Awake/Start run
        }

        [UnityTest]
        public IEnumerator Scene_BootsWithEveryBootstrapReferenceAssigned()
        {
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            Assert.IsNotNull(bootstrap, "Gameplay scene must contain a GameBootstrap");

            Assert.IsNotNull(bootstrap.TimeSystem,  "TimeSystem should be constructed in Awake");
            Assert.IsNotNull(bootstrap.NeedsSystem, "NeedsSystem should be constructed in Awake");
            Assert.IsNotNull(bootstrap.SaveSystem,  "SaveSystem should be constructed in Awake");
            Assert.IsNotNull(Object.FindAnyObjectByType<HUD>(), "Gameplay scene must contain a HUD");

            Assert.IsNotNull(bootstrap.Survivors);
            Assert.Greater(bootstrap.Survivors.Count, 0, "a new game should start with survivors");

            yield return null;
        }

        [UnityTest]
        public IEnumerator Clock_AdvancesOverFrames()
        {
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            float before = bootstrap.TimeSystem.TotalElapsedHours;

            for (int i = 0; i < 120; i++)
                yield return null;

            Assert.Greater(bootstrap.TimeSystem.TotalElapsedHours, before,
                "the clock must advance while the scene is playing");
        }

        [UnityTest]
        public IEnumerator Needs_DecayOverFrames()
        {
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();
            var survivor = bootstrap.Survivors[0];
            float thirstBefore = survivor.Needs.Thirst;

            for (int i = 0; i < 120; i++)
                yield return null;

            Assert.Greater(survivor.Needs.Thirst, thirstBefore,
                "thirst accumulates upward as time passes");
        }

        [UnityTest]
        public IEnumerator SaveAndLoad_RoundTripsClockAndNeeds()
        {
            var bootstrap = Object.FindAnyObjectByType<GameBootstrap>();

            for (int i = 0; i < 60; i++)
                yield return null;

            Assert.IsTrue(bootstrap.SaveSystem.Save("smoke_test"), "save should succeed");

            int dayAtSave = bootstrap.TimeSystem.CurrentDay;
            float hoursAtSave = bootstrap.TimeSystem.TotalElapsedHours;
            float thirstAtSave = bootstrap.Survivors[0].Needs.Thirst;

            for (int i = 0; i < 60; i++)
                yield return null;

            Assert.IsTrue(bootstrap.SaveSystem.Load("smoke_test"), "load should succeed");

            Assert.AreEqual(dayAtSave, bootstrap.TimeSystem.CurrentDay);
            Assert.AreEqual(hoursAtSave, bootstrap.TimeSystem.TotalElapsedHours, 0.001f);
            Assert.AreEqual(thirstAtSave, bootstrap.Survivors[0].Needs.Thirst, 0.001f);

            bootstrap.SaveSystem.Delete("smoke_test");
        }
    }
}
```

- [ ] **Step 2: Add the scene to the test build settings**

PlayMode tests can only load scenes registered in `EditorBuildSettings`. Task 4 Step 5 already registered `Gameplay.unity`. Confirm:

```bash
grep -A3 "m_Scenes" ProjectSettings/EditorBuildSettings.asset
```

Expect `Assets/Scenes/Gameplay.unity` present with `enabled: 1`, and no `SampleScene` entry.

- [ ] **Step 3: Run the tests to verify they fail**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testFilter "AtomicWar.Tests.PlayMode.GameplaySceneSmokeTests" \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
```

Expected before Task 4 has run: FAIL, scene not found. After Task 4: these should mostly pass — if any fail, that is a real wiring defect. Read `pm.log` and fix the wiring, not the assertion.

- [ ] **Step 4: Delete the stub it replaces**

```bash
git rm -f Assets/Tests/PlayMode/GameBootstrapStubTests.cs \
          Assets/Tests/PlayMode/GameBootstrapStubTests.cs.meta
```

- [ ] **Step 5: Run the full PlayMode suite**

```bash
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"
```

Expected: all PlayMode tests pass, count up by 4 from the stub's 1.

- [ ] **Step 6: Commit**

```bash
rm -f pm.xml pm.log
git add Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs
git commit -m "test(scene): smoke-test the real gameplay scene over frames"
```

---

### Task 6: Gate regeneration in CI and refresh the README

Without this, the generators drift into decoration while the committed assets are edited by hand.

**Files:**
- Modify: `.github/workflows/ci.yml`
- Modify: `README.md`

**Interfaces:**
- Consumes: `CatalogGenerator.GenerateAll` and `GameplaySceneBuilder.BuildGameplayScene` as `-executeMethod` targets.
- Produces: nothing consumed downstream.

- [ ] **Step 1: Add the regeneration job**

In `.github/workflows/ci.yml`, add a job after `validate` and before `test`:

```yaml
  regenerate:
    name: Generated Assets Are Current
    needs: validate
    runs-on: ubuntu-latest
    steps:
      - uses: actions/checkout@v4
        with:
          lfs: true

      - uses: actions/cache@v4
        with:
          path: Library
          key: Library-regen-${{ env.UNITY_VERSION }}-${{ hashFiles('Assets/**', 'Packages/manifest.json', 'Packages/packages-lock.json', 'ProjectSettings/**') }}
          restore-keys: |
            Library-regen-${{ env.UNITY_VERSION }}-

      - uses: game-ci/unity-builder@v4
        with:
          projectPath: .
          unityVersion: 6000.5.5f1
          buildMethod: AtomicWar._Game.Editor.CatalogGenerator.GenerateAll
          allowDirtyBuild: true

      - uses: game-ci/unity-builder@v4
        with:
          projectPath: .
          unityVersion: 6000.5.5f1
          buildMethod: AtomicWar._Game.Editor.GameplaySceneBuilder.BuildGameplayScene
          allowDirtyBuild: true

      - name: Fail if regenerated output differs from committed
        run: |
          if ! git diff --quiet -- Assets/_Game/Data/Generated Assets/Scenes/Gameplay.unity; then
            echo "Regenerated assets differ from what is committed."
            echo "Re-run Tools/ASHFALL/Generate Catalogs and Build Gameplay Scene, then commit."
            git diff --stat -- Assets/_Game/Data/Generated Assets/Scenes/Gameplay.unity
            exit 1
          fi
          echo "OK: generated assets are current."
```

- [ ] **Step 2: Refresh the stale README section**

`README.md`'s "Current state" section predates the main-menu work. It claims there is "no rendering or UI layer: no sprites, prefabs, materials, animations, Canvas, uGUI, UI Toolkit or TextMeshPro", which is no longer true — `Assets/_Game/UI/MainMenu/` holds UXML, USS, `PanelSettings` and SDF fonts, and this plan adds a booting gameplay scene.

Replace the "Current state" section with:

```markdown
## Current state

**The simulation runs from the main menu.**

- `Assets/Scenes/StartScreen.unity` is the boot scene; NEW EXPEDITION loads
  `Assets/Scenes/Gameplay.unity`, where `GameBootstrap` initializes every
  system, the clock advances, and needs decay.
- Both scenes are generated or authored through `Tools/ASHFALL/` editor
  commands. `Gameplay.unity` is built by
  `Tools/ASHFALL/Build Gameplay Scene` and must be regenerated rather than
  hand-edited; CI fails if the committed scene differs from a fresh build.
- Data assets come from `Assets/StreamingAssets/Data/*.json` via
  `Tools/ASHFALL/Import All Data`, then
  `Tools/ASHFALL/Generate Catalogs`.
- UI is UI Toolkit (UXML/USS + `PanelSettings`). The main menu is complete;
  the in-game HUD wires 4 of its 21 widgets — the rest are tracked in
  `GameplaySceneBuilder.HudExpectedUnwired` and land incrementally.
- There is no localization; all user-facing strings are inline literals.
```

- [ ] **Step 3: Verify the workflow parses**

```bash
python3 -c "import yaml; yaml.safe_load(open('.github/workflows/ci.yml')); print('ci.yml OK')"
```

- [ ] **Step 4: Commit**

```bash
git add .github/workflows/ci.yml README.md
git commit -m "ci: fail when generated assets drift, and refresh the README state"
```

---

## Verification

After all six tasks, confirm the whole thing end to end:

```bash
# Full suites, both platforms, sequentially in one command
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform EditMode \
  -testResults "$(pwd)/em.xml" -logFile "$(pwd)/em.log" && \
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics \
  -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile "$(pwd)/pm.log"

# Build and run the player headlessly; the log must show game activity
/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity -batchmode -nographics -quit \
  -projectPath . -executeMethod AtomicWar._Game.Editor.BuildScript.PerformBuildPipeline \
  -logFile "$(pwd)/build.log"

rm -f em.xml em.log pm.xml pm.log build.log
```

Expected: EditMode up by 8 tests (1 radio + 4 catalog + 3 scene builder), PlayMode up by 4 net, and a Linux player that boots into a running simulation instead of idling.

## Known gaps left open

- The other 17 HUD widgets remain unwired, tracked explicitly in
  `GameplaySceneBuilder.HudExpectedUnwired`.
- `echoes.json` (15 entries) has no importer and no catalog. It is not
  referenced by any `GameBootstrap` serialized field, so it does not block
  this work.
- `LootTableSO` is seeded from every imported item at uniform weight. If
  loot is meant to be curated, it should become authored JSON with its own
  importer rather than a generated default.
- Windows and macOS build targets are not installed on this Editor, so CI
  builds Linux only.
