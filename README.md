# ASHFALL (working title) — 2D Atomic-War Survival

Original 2D survival-management game set after a nuclear exchange.
Unity 6 LTS · 2D · URP · C#. Data-driven (ScriptableObjects + JSON).
Thin MonoBehaviours; logic in plain C# systems; event bus; Utility AI (no LLM at runtime).

This document maps the project folders to their responsibilities.

## Stack & conventions

- **Namespace scheme:** root `AtomicWar`; the gameplay code under `Assets/_Game/`
  uses `AtomicWar._Game.<Folder>` (e.g. `AtomicWar._Game.Radiation`). Namespaces
  mirror folders.
- **Ids:** snake_case everywhere (`item_clean_water`, `recipe_iodine`).
- **State changes:** every public system raises C# events on the EventBus (for UI + save).
- **Save/load safe:** system state is serializable primitives only.
- **Tone:** cold, exhausted, human, restrained. No magic/fantasy, no real
  countries/wars/people, no glorified violence.

## Folder map

### `Assets/_Game/` — gameplay code

| Folder | Namespace | Responsibility |
| --- | --- | --- |
| `Core/` | `AtomicWar._Game.Core` | Session lifecycle (`GameState`), the publish/subscribe `EventBus`, the `TimeSystem` clock (hours/days + tick events), and the `SaveSystem` (JSON slot persistence). |
| `Survivors/` | `AtomicWar._Game.Survivors` | The `Survivor` runtime model, its `Needs` (hunger, thirst, fatigue, warmth, morale, health), and the `NeedsSystem` that decays/restores them and raises threshold events. |
| `Radiation/` | `AtomicWar._Game.Radiation` | `RadiationSystem` (dose accumulation, iodine/anti-rad, chronic illness), `Contamination` (zones/items/survivors), and the personal `Dosimeter` reading. |
| `Environment/` | `AtomicWar._Game.Environment` | `FalloutMap` (spatial dose field), `WeatherSystem` (incl. fallout storms), and `TemperatureSystem` (nuclear-winter cold → Warmth need). |
| `Inventory/` | `AtomicWar._Game.Inventory` | `ItemType` enum, `ItemDefinition` ScriptableObject, and the runtime `Inventory` container (stacks). |
| `Crafting/` | `AtomicWar._Game.Crafting` | `Recipe` ScriptableObject, `CraftingSystem` (validation, timers, consume/produce), and `CraftingStation` (gating + wear). |
| `Shelter/` | `AtomicWar._Game.Shelter` | The bunker aggregate `Shelter` plus its `Shielding` (rad attenuation) and `AirFiltration` (degrading filters) sub-systems. |
| `AI/` | `AtomicWar._Game.AI` | Utility AI: `UtilityAI` engine, `ActionScorer` (0..1 scoring), and `SurvivorAction` ScriptableObject candidates. |
| `Events/` | `AtomicWar._Game.Events` | `GameEvent` ScriptableObject (scripted/narrative events) and `EventRunner` (weighted/scheduled selection + cooldowns). |
| `Data/` | `AtomicWar._Game.Data` | ScriptableObject **catalogs** that hold collections and are the runtime source of truth, imported from the JSON below (items, recipes, survivors, locations, events, radio). |
| `Editor/` | `AtomicWar._Game.Editor` | Editor-only importers/validators (`Tools/ASHFALL/...` menu) that turn JSON into ScriptableObject catalogs and check snake_case ids. |
| `UI/` | `AtomicWar._Game.UI` | Thin MonoBehaviours over UI Toolkit: `HUD` (root binder), the widgets it binds (`NeedsBar`, `DosimeterHUD`, `EventModalUI`, `DiegeticHudController`, …), and `MainMenu/`. No game logic. |

### `Assets/StreamingAssets/Data/` — authored JSON data

One file per catalog, imported by `Assets/_Game/Editor/`:

| File | Feeds |
| --- | --- |
| `items.json` | `Data/ItemCatalogSO` → `Inventory/ItemDefinition` |
| `recipes.json` | `Data/RecipeCatalogSO` → `Crafting/Recipe` |
| `survivors.json` | `Data/SurvivorCatalogSO` → `SurvivorArchetypeSO` |
| `locations.json` | `Data/LocationCatalogSO` → `LocationDefinitionSO` |
| `events.json` | `Data/GameEventCatalogSO` → `Events/GameEvent` |
| `radio.json` | `Data/RadioCatalogSO` → `RadioBroadcastSO` |

`echoes.json` also lives here. It has no importer and no catalog yet, and no
`GameBootstrap` field references it.

**Data flow:** JSON (authored) → Editor importer → per-entry ScriptableObject →
`Tools/ASHFALL/Generate Catalogs` → catalog asset (runtime source of truth) → systems.

### `Assets/Tests/` — automated tests

| Folder | Assembly | Runs where |
| --- | --- | --- |
| `EditMode/` | `AtomicWar.Tests.EditMode` | Editor only — pure-C# system tests (no scene). |
| `PlayMode/` | `AtomicWar.Tests.PlayMode` | In play mode — integration tests over frames. |

Both are Unity test assemblies (`.asmdef` referencing the Test Runner, gated by
`UNITY_INCLUDE_TESTS`). Almost all of them construct systems directly in C# and
never load a scene, which is why the suite stayed green while the shipped player
booted into an empty scene. `PlayMode/GameplaySceneSmokeTests.cs` is the fixture
that loads the real scene and catches that class of failure.

> **Note on test location:** Unity only compiles and runs scripts under
> `Assets/` (or `Packages/`). A project-root `Tests/` folder would be silently
> ignored and never executed by `-runTests`, so the tests live at
> `Assets/Tests/` to remain runnable.

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

## Verify

```bash
# Compile (opens the project, compiles all assemblies, quits)
unity -batchmode -quit -nographics -projectPath . -logFile -

# Run the tests. Note: no -quit. Combining -quit with -runTests kills the
# editor before the run finishes, and it exits 0 with an empty result file.
unity -batchmode -nographics -projectPath . -runTests -testPlatform EditMode \
  -testResults "$(pwd)/em.xml" -logFile -
unity -batchmode -nographics -projectPath . -runTests -testPlatform PlayMode \
  -testResults "$(pwd)/pm.xml" -logFile -
```
# Atomic-War-Starving-Survival
