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

## Current state

**The simulation is implemented; the game is not yet wired into a scene.**

- `Assets/_Game/` holds ~500 implemented C# files. There are **zero**
  `NotImplementedException` bodies left.
- Tests are green: **EditMode 1037 / PlayMode 61**, and the suites construct
  every system directly in code.
- A Linux player **builds** successfully (~100 MB).

The gap is presentation and scene wiring:

- `Assets/Scenes/SampleScene.unity` is the only scene in Build Settings, and it
  contains just `Main Camera` and `Global Light 2D`. **`GameBootstrap` is not in
  it**, and nothing else instantiates it (there is no
  `RuntimeInitializeOnLoadMethod`). A built player therefore launches and idles —
  verified by running it with `-batchmode -nographics`, whose log shows no game
  activity at all.
- To boot the game, `GameBootstrap` must be added to a scene GameObject and its
  **12 `[SerializeField]` catalog/profile references** assigned in the Inspector
  (`NeedsProfile`, `ItemCatalogSO`, `GameEventCatalogSO`, … — the matching assets
  live in `Assets/_Game/Data/`). It will `NullReferenceException` on `Start` if
  the component is added without them.
- There is **no rendering or UI layer**: no sprites, prefabs, materials,
  animations, Canvas, uGUI, UI Toolkit or TextMeshPro exist. The `*HUD` classes
  are data/formatting models with no draw code; the only `OnGUI` is IMGUI debug
  overlay. There is also no localization — all user-facing strings are inline
  literals.

None of the above is a regression; it is unbuilt work. It is recorded here
because the passing test count makes the project look more finished than it is.

## Verify

```bash
# Compile (opens the project, compiles all assemblies, quits)
unity -batchmode -quit -nographics -projectPath . -logFile -

# Run EditMode + PlayMode tests
unity -batchmode -quit -nographics -projectPath . -runTests -testResults results.xml -logFile -
```
# Atomic-War-Starving-Survival
