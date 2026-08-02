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
| `UI/` | `AtomicWar._Game.UI` | Thin MonoBehaviours: `HUD` (root binder), `NeedsBar`, `DosimeterHUD`. No game logic. |

### `Assets/StreamingAssets/Data/` — authored JSON data

Empty (`[]`) placeholders, one per catalog. Imported by `Assets/_Game/Editor/`:

| File | Feeds |
| --- | --- |
| `items.json` | `Data/ItemCatalogSO` → `Inventory/ItemDefinition` |
| `recipes.json` | `Data/RecipeCatalogSO` → `Crafting/Recipe` |
| `survivors.json` | `Data/SurvivorCatalogSO` → `SurvivorArchetypeSO` |
| `locations.json` | `Data/LocationCatalogSO` → `LocationDefinitionSO` |
| `events.json` | `Data/GameEventCatalogSO` → `Events/GameEvent` |
| `radio.json` | `Data/RadioCatalogSO` → `RadioBroadcastSO` |

**Data flow:** JSON (authored) → Editor importer → ScriptableObject catalog (runtime source of truth) → systems.

### `Assets/Tests/` — automated tests

| Folder | Assembly | Runs where |
| --- | --- | --- |
| `EditMode/` | `AtomicWar.Tests.EditMode` | Editor only — pure-C# system tests (no scene). |
| `PlayMode/` | `AtomicWar.Tests.PlayMode` | In play mode — integration tests over frames. |

Both are Unity test assemblies (`.asmdef` referencing the Test Runner, gated by
`UNITY_INCLUDE_TESTS`) and currently contain passing stubs to be replaced as
systems are implemented.

> **Note on test location:** Unity only compiles and runs scripts under
> `Assets/` (or `Packages/`). A project-root `Tests/` folder would be silently
> ignored and never executed by `-runTests`, so the tests live at
> `Assets/Tests/` to remain runnable.

## Coexistence with `Assets/Scripts/`

`Assets/Scripts/` holds an earlier prototype under the `AtomicWar.Data`,
`AtomicWar.Core.*`, and `AtomicWar.Runtime.*` namespaces. The new
`Assets/_Game/` tree uses the distinct `AtomicWar._Game.*` namespaces, so the
two coexist without type collisions. Same-named types (e.g. `ItemType`,
`ItemDefinition`, `NeedsSystem`) are different types in different namespaces.

## Current state

Scaffold only: every file is a compiling stub — type shapes, member signatures,
and XML-doc intent. Method bodies are `throw new System.NotImplementedException();`.
No gameplay logic is implemented yet.

## Verify

```bash
# Compile (opens the project, compiles all assemblies, quits)
unity -batchmode -quit -nographics -projectPath . -logFile -

# Run EditMode + PlayMode tests
unity -batchmode -quit -nographics -projectPath . -runTests -testResults results.xml -logFile -
```
