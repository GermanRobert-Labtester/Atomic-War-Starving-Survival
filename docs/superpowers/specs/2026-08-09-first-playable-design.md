# First Playable — Boot the Simulation from the Main Menu

Date: 2026-08-09
Status: Approved by user, ready for implementation planning.

## Problem

The shipped Linux player is a main menu that leads to a black screen.

`Assets/Scenes/StartScreen.unity` works — it carries `MainMenuController`
with real UI Toolkit assets (`MainMenu.uxml`/`.uss`, `MainMenuPanelSettings`,
Barlow Condensed + Share Tech Mono SDF fonts, vignette and scanline
textures). Choosing NEW EXPEDITION runs
`MainMenuController.cs:360` → `SceneManager.LoadScene(_gameplaySceneName)`,
where `_gameplaySceneName` defaults to `"SampleScene"`
(`MainMenuController.cs:29`).

`Assets/Scenes/SampleScene.unity` contains only a Main Camera and a Global
Light 2D. Both of its `MonoBehaviour` entries resolve to URP package
components, not game code. `GameBootstrap` — the composition root, spread
across ~50 partial files in `Assets/_Game/Core/` — is in **no scene**, and
there is **zero** `RuntimeInitializeOnLoadMethod` anywhere under
`Assets/_Game/`. Nothing boots the simulation in a player.

So the ~917 C# files and the ~1,100 passing tests are reachable only from
test code. This is unbuilt work, not a regression, but the green test count
makes the project look far more finished than it is.

### Why the tests did not catch it

The PlayMode suite constructs systems directly in C# and never loads a
scene. Scene and Inspector wiring is therefore entirely unverified by CI:
the suite would stay green against a completely broken scene. Closing that
gap is part of this work, not a follow-up.

## Current state of the inputs

### Data assets — 10 of 12 missing

`GameBootstrap` declares 12 `[SerializeField]` data references
(`GameBootstrap.cs:36-51`). Only two exist as assets:

| Field | Type | Asset |
| --- | --- | --- |
| `_flashpointSequence` | `FlashpointSequenceSO` | `Generated/Flashpoint/DefaultFlashpointSequence.asset` |
| `_mentalBreakCatalog` | `MentalBreakCatalogSO` | `Generated/Survivor/DefaultMentalBreakCatalog.asset` |
| `_needsProfile` | `NeedsProfile` | missing |
| `_lightProfile` | `LightProfile` | missing |
| `_seasonProfile` | `SeasonProfile` | missing |
| `_itemCatalog` | `ItemCatalogSO` | missing |
| `_recipeCatalog` | `RecipeCatalogSO` | missing |
| `_eventCatalog` | `GameEventCatalogSO` | missing |
| `_locationCatalog` | `LocationCatalogSO` | missing |
| `_radioCatalog` | `RadioCatalogSO` | missing |
| `_worldPhaseConfig` | `WorldPhaseConfigSO` | missing |
| `_lootTable` | `LootTableSO` | missing |

`JsonDataImporter.cs` creates individual `ItemDefinition`, `Recipe`,
`SurvivorArchetypeSO`, `LocationDefinitionSO` and `GameEvent` assets, but
**never creates or populates a catalog SO** — its only `AssetDatabase.
CreateAsset` call (line 640) writes individual entries. Every catalog type
is a plain `[CreateAssetMenu]` list that a human is expected to make and
fill by hand.

### Imported assets are stale, not filtered

`Assets/StreamingAssets/Data/` holds real authored data: 321 items, 72
survivors, 39 events, 16 recipes, 12 radio broadcasts, 5 locations, 15
echoes. `Generated/Items/` holds **19** assets.

This is staleness, not a filter: `ImportItemsMenu` passes the whole parsed
list to `ImportItems(items)` with no predicate
(`JsonDataImporter.cs:149-158`). The importer simply has not been re-run
since the JSON grew. No code change is required.

### Radio is not imported at all

`grep -n "radio" Assets/_Game/Editor/JsonDataImporter.cs` returns nothing.
The importer handles items, recipes, survivors, locations and events, but
has no radio path whatsoever, so `radio.json`'s 12 entries never become
`RadioBroadcastSO` assets and there is no `Generated/Radio/` folder for
`RadioCatalogSO` to aggregate.

This does need a code change. It is small — the key union across all 12
entries is exactly `id`, `minDay`, `maxDay`, `message`, `triggerEventId`,
which maps 1:1 onto `RadioBroadcastSO`'s public fields. Extending the
importer is preferable to generating an empty `RadioCatalogSO`, which would
satisfy the non-null check while leaving the radio silently dead — the
precise failure mode this spec exists to eliminate.

### HUD is an unbuilt object graph

`HUD.cs` is a `MonoBehaviour` with **23** serialized child-widget
references, each itself a `MonoBehaviour` (`NeedsBar`, `DosimeterHUD`,
`EventModalUI`, `MapScreenUI`, …). The project contains **0 prefabs** and 4
images total.
The UI Toolkit foundation exists (`DiegeticHud.uxml`/`.uss`, two
`PanelSettings` assets, one mirrored into `Assets/Resources/UI/`); the
hierarchy that would mount it does not.

## Scope of this spec

A **minimal vertical slice**. Choosing NEW EXPEDITION loads a real gameplay
scene in which:

- `GameBootstrap` boots with every serialized reference assigned
- the clock advances
- needs decay
- the dosimeter reads
- one game event reaches a modal
- save → load round-trips

Four HUD widgets, not twenty-five. The remaining ~21 widgets are added later
against a working baseline.

## Approach

All catalogs and the gameplay scene are **generated by editor code**, not
hand-authored in the Inspector. Unity 6000.5.5f1 is installed locally
(`/home/robertsrff/Unity/Hub/Editor/6000.5.5f1/Editor/Unity`) and
`Assets/_Game/Editor/BuildScript.cs` already performs headless builds, so
`-executeMethod` automation is available.

Rejected alternatives:

- **Hand-authoring in the Editor.** `.unity`/`.asset` diffs are
  unreviewable YAML, the result cannot be verified from a headless session,
  and a broken scene could not be regenerated.
- **A runtime composition root** (catalogs from `Resources`, HUD built in
  code at `Awake`). Simplest to test, but it fights the `[SerializeField]`
  design already spread across ~50 `GameBootstrap` partials and forfeits
  Inspector tuning.

Generation matches the pattern already established in
`Assets/_Game/Editor/` and keeps the wiring reviewable as C# rather than
serialized YAML.

## Components

### 1. `Assets/_Game/Editor/CatalogGenerator.cs`

Menu: `Tools/ASHFALL/Generate Catalogs`. Also exposed as a static
`-executeMethod` entry point.

The 10 missing assets fall into two kinds, handled differently:

**JSON-backed catalogs** — `ItemCatalogSO`, `RecipeCatalogSO`,
`GameEventCatalogSO`, `LocationCatalogSO`, `RadioCatalogSO`. Aggregate the
individual assets the existing importer writes under
`Assets/_Game/Data/Generated/<Kind>/` into the catalog's list field
(`items`, `recipes`, `events`, `locations`, `broadcasts`). Pure aggregation
— `StreamingAssets/Data/*.json` remains the source of truth.

**Tuning profiles** — `NeedsProfile`, `LightProfile`, `SeasonProfile`,
`WorldPhaseConfigSO`, `LootTableSO`. These have no JSON source. Create them
at their declared C# field defaults, which are already balanced values
(`hungerPerHour = 2f`, `thirstPerHour = 3f`, `flashpointDay = 30`,
`campaignLengthDays = 90`, and `SeasonProfile`'s default
`AnimationCurve`s). `LootTableSO.entries` is seeded from the imported
`ItemDefinition` assets at uniform weight.

Written to `Assets/_Game/Data/Generated/Catalogs/`.

**Create-if-absent for tuning profiles; refresh-in-place for catalogs.**
Regeneration must never overwrite hand-tuned profile values, and must
preserve asset GUIDs so scene references survive.

### 2. Extend and re-run the importer

Add a radio path to `JsonDataImporter` (`RadioJson` DTO, `ImportRadio`,
`Tools/ASHFALL/Import Radio`, and a call inside `ImportAll()`), writing to
`Generated/Radio/`.

Then re-run `Tools/ASHFALL/Import All Data` to close the 19 → 321 item gap
and the equivalent gaps in events, survivors, locations and recipes. Must
run *before* `CatalogGenerator`, which aggregates its output.

### 3. `Assets/_Game/Editor/GameplaySceneBuilder.cs`

Menu: `Tools/ASHFALL/Build Gameplay Scene`. Also an `-executeMethod` entry
point.

Constructs `Assets/Scenes/Gameplay.unity` from nothing:

- Main Camera and Global Light 2D matching the existing URP 2D setup
- a `GameBootstrap` GameObject
- a HUD GameObject carrying `UIDocument` + `DiegeticHudPanelSettings` and
  the slice's four child widgets: `NeedsBar`, `DosimeterHUD`,
  `EventModalUI`, `DiegeticHudController`
- a `DiagnosticsOverlay` GameObject for `_diagnosticsOverlay`

All 17 `GameBootstrap` serialized fields are assigned through
`SerializedObject`/`FindProperty`. The builder then **refuses to save if a
field that should be wired is null**, logging the offending field names, so
a half-wired scene never reaches disk.

"Should be wired" is defined precisely, because `HUD` has 23 serialized
fields — 21 object references to widgets (`HUD.cs:21-41`) plus
`_debugToggleKey` and `_debugModeEnabled`, which are value types and cannot
be null — and this slice deliberately wires only 4 of the 21:

- **All 17** `GameBootstrap` fields must be non-null. No exceptions.
- On `HUD`, the builder holds an explicit **slice allowlist** —
  `_needsBar`, `_dosimeterHud`, `_eventModalUi`, `_diegeticHud` — which
  must be non-null. The other 17 widget references are recorded in a named
  `ExpectedUnwired` set and skipped.

Keeping the unwired widgets in an explicit list rather than skipping all
nulls means the gate stays meaningful: as each widget lands, it moves from
`ExpectedUnwired` to the allowlist, and the set shrinks to empty when the
HUD is complete. A widget that silently fails to wire is still caught.

It also:

- registers `Gameplay.unity` in `EditorBuildSettings`
- repoints `MainMenuController._gameplaySceneName` in **both** places: the
  C# default at `MainMenuController.cs:29` *and* the value serialized into
  `StartScreen.unity`. Changing only the default would leave the menu
  silently loading the old scene.
- deletes `Assets/Scenes/SampleScene.unity` and removes it from
  `EditorBuildSettings`

`SampleScene` is deleted rather than built into: it exists only as the thing
the menu currently fails into, and a from-scratch generator is simpler to
reason about than one that reconciles with existing scene contents.

### 4. `Assets/Tests/PlayMode/GameplaySceneSmokeTests.cs`

Loads `Gameplay.unity` for real via `SceneManager.LoadSceneAsync` and
asserts:

- `GameBootstrap` is present and all 17 of its serialized references are
  non-null, as are the four allowlisted `HUD` widgets
- after N frames the `TimeSystem` clock has advanced
- at least one need has decayed from its starting value
- `SaveSystem` save → load preserves clock and needs state

This is the regression gate for the entire class of bug described under
"Why the tests did not catch it".

## Data flow

```
StreamingAssets/Data/*.json
  → JsonDataImporter          (existing)
  → individual SO assets       Generated/{Items,Recipes,Survivors,Locations,Events}/
  → CatalogGenerator           (new)
  → catalog + profile SOs      Generated/Catalogs/
  → GameplaySceneBuilder       (new)
  → serialized refs in Gameplay.unity
  → GameBootstrap.Awake        (runtime)
```

## Error handling

Both generators fail loudly with the list of missing or unassigned items
rather than writing partial output.

This matters more than usual here: `GameBootstrap` uses null-conditional
access on its catalogs throughout (`_itemCatalog?.GetById(...)` at
`GameBootstrap.InitLate.cs:81`, `InitFoundation.cs:250,322-323`, and
elsewhere). A missing catalog therefore degrades into a silently empty game
rather than an exception. The null check must live in the generator and the
smoke test, because the runtime will not raise one.

## Testing

- `GameplaySceneSmokeTests` joins the existing PlayMode CI job.
- A new CI step regenerates catalogs and the scene headlessly and fails if
  the output differs from what is committed. This keeps the generators the
  real source of truth instead of drifting into decoration while the
  committed assets are edited by hand.

Run the suites per the project convention — never pass `-quit` together with
`-runTests`, which exits before the runner starts and writes no results XML.

## Out of scope

- The remaining ~21 HUD widgets
- Sprites, prefabs, and art beyond what the slice needs
- Localization (all user-facing strings remain inline literals)
- Windows and macOS build targets (not installed on this Editor)

## Repository note

`README.md`'s "Current state" section predates the main-menu work and
claims the project has no UI Toolkit, Canvas, or rendering layer at all.
It should be refreshed once this lands.
