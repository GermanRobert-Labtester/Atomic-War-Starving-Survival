# Expansion XI — "The Glass Orchard" (greenhouse agriculture)

**Goal (2 lines):** Add a save-safe `GreenhouseSystem` so the bunker can grow food under lead-glass + grow-lights in nuclear winter — completing the dangling `Item_SeedLedger`/`NutrientDripAutomation` hooks ("yields ultimate Hydroponics… pre-war Wheat"). Ship it with items, locations, narrative events, lore, wiring, and an EditMode test. Tone: hope that corrodes.

It follows the live codebase pattern exactly (plain C# in `AtomicWar._Game.Core`, `using UnityEngine;`, `[Serializable]` state, `ISaveable`, `SystemRegistry` daily tick, `event Action<…>`), matching siblings `MutatedEcosystemSystem` / `NutrientDripAutomation` / `VehicleMaintenanceSystem`. (Note: the engine-agnostic `Ashfall.Core` is the stated target but is currently orphaned/unconsumed per the 2026-08-14 audit; matching live siblings is what makes this *integrate* rather than become another orphan.)

---

## System design — `GreenhouseSystem` (new, plain C#, save-safe)

State (`[Serializable]`, JsonUtility-safe — public fields, no dicts):
- `GreenhouseState { string saveId="greenhouse"; List<PlotState> plots; bool preWarWheatUnlocked; int totalHarvests; }`
- `PlotState { int plotIndex; string cropId; int stage; float growth; float water; float soilContamination; float blight; int plantedDay; }`
  - `stage`: 0 Fallow, 1 Sprouting, 2 Growing, 3 Mature, 4 Failed.

Crop defs — `GreenhouseCropCatalog` (C# const ids + `Get(cropId)`), tuning as consts (mirrors `MutatedEcosystemSystem` consts):

| seed id | clean yield | tainted yield | light h/day | water/day | yield | blight resist | unlock |
|---|---|---|---|---|---|---|---|
| `item_seed_mushroom` | `crop_mushroom` | `tainted_food` | 4 | low | 2 | high | always |
| `item_seed_tuber` | `crop_tuber` | `tainted_food` | 8 | med | 3 | med | always |
| `item_seed_grain` | `crop_grain` | `tainted_food` | 12 | high | 5 | low | always |
| `item_seed_wheat` | `crop_wheat` | `tainted_food` | 10 | high | 6 | med | **seed-ledger only** |

Operations (system owns the simulation; **host mediates inventory** so the system stays pure/testable — returns what to consume/grant, like `NutrientDripAutomation`'s `out`):
- `EnsurePlots(int planterBoxCount)` — grows plots list to match owned `item_planter_box`.
- `Plant(plot, cropId, out string seedConsumed)` — gate on fallow plot + crop def + unlock; returns seed id to consume.
- `Water(plot, waterUnits, bool tainted)` — raises `water`; tainted adds contamination.
- `Harvest(plot) → {yieldItemId, amount, contaminated}` — if Mature, picks clean vs tainted yield by `soilContamination` vs crop tolerance; resets plot (retains residual contamination); returns items for host to grant.
- `TreatBlight(plot, out string treatmentConsumed)` — reduces blight (consumes `item_blight_treatment`).
- `UnlockPreWarWheat()` — called when the Svalbard Seed Ledger decrypts.
- `TickDay(currentDay, growLightHours, ashContaminationRate, Random rng)` — per planted plot: consume water (drought stalls growth + blight risk), accrue growth from light factor, drift soil contamination from ash, roll blight (seeded rng), advance stage → Mature. Deterministic via seeded rng.

Events raised: `OnCropPlanted`, `OnCropMatured`, `OnCropHarvested(yieldId, amount, contaminated, plot)`, `OnBlightOutbreak`, `OnPlotDriedOut`, `OnCropFailed`. Implements `ISaveable` (`SaveId="greenhouse"`, `CaptureState`/`RestoreState` round-trip).

## Content (data-driven)

**`greenhouse_items.json`** (~14 new items, items.json schema): seeds (`item_seed_mushroom/tuber/grain/wheat`), `item_planter_box`, `item_grow_lamp`, `item_lead_glass_pane`, `item_blight_treatment`, `item_grow_medium`, yields (`crop_mushroom`, `crop_tuber`, `crop_grain`, `crop_wheat`), and `tainted_food` (fills the dangling `NutrientDripAutomation` reference — sanctioned id, just never defined).

**Locations** appended to `locations.json` (~4): `location_glasshouse_ruins`, `location_seed_vault`, `location_hydro_barons_aquaponics`, `location_rot_farmers_compost_yard` (only references existing factions `hydro_barons`/`rot_farmers` — no new faction ids invented).

**Narrative + mechanic events** via `GreenhouseEventFactory.CreateAll()` (C# `GameEvent`s with choices/effects/delayed consequences — canonical `EventPoolBuilder` path): `greenhouse_first_sprout` (echo: share/hoard/gift-to-rot-farmers), `greenhouse_blight_outbreak` (burn/treat/ignore), `greenhouse_tainted_harvest` (feed/compost/discard), `greenhouse_the_offering` (rot_farmers trade seed stock), `greenhouse_dead_gardener` (corpse + seed tin + lore), `greenhouse_glass_breaks` (ozone/UV tie-in). These events call the system's Plant/Water/Harvest/TreatBlight through the host, making the greenhouse **playable through the existing event + journal loop**.

**Lore** appended to `world_history.json` (~4 entries): the pre-war municipal feeding program, the seed vault's purpose, the first post-exchange gardener, the lead-glass works.

## Files

**Create (7):**
1. `Assets/StreamingAssets/Data/greenhouse_items.json`
2. `Assets/_Game/Data/GreenhouseItemsCatalogLoader.cs` (mirror `HoldfastItemsCatalogLoader`)
3. `Assets/_Game/Core/GreenhouseExpansionCatalog.cs` (id consts + `GreenhouseCropCatalog`)
4. `Assets/_Game/Core/GreenhouseSystem.cs` (state + ops + tick + `ISaveable`)
5. `Assets/_Game/Events/GreenhouseEventFactory.cs` (`CreateAll() → List<GameEvent>`)
6. `Assets/_Game/Core/GameBootstrap.Greenhouse.cs` (construct, `RegisterDaily`, `SaveSystem.Register`, merge item catalog, wire events → `TriggerEventById`/journal, seed-ledger → `UnlockPreWarWheat`)
7. `Assets/Tests/EditMode/GreenhouseSystemTests.cs`

**Edit (4):**
8. `Assets/_Game/Core/EventPoolBuilder.cs` — `AddRangeWithDedup(pool, GreenhouseEventFactory.CreateAll())`.
9. `Assets/StreamingAssets/Data/locations.json` — append ~4.
10. `Assets/StreamingAssets/Data/world_history.json` — append ~4.
11. Boot call-site — invoke `InitGreenhouse()` in the init sequence (near other expansion inits) + seed-ledger hook.

## Scope boundary (stated honestly)
No new HUD panel/widget — Phase 11 is explicitly the HUD-integration workstream. The greenhouse is **player-reachable via the event modal + journal** (events drive Plant/Water/Harvest/TreatBlight), and the system auto-simulates on the daily tick. The host action methods are exposed for a future Phase-11 widget. All ids are snake_case and either new-and-self-consistent or reference existing sanctioned ids; no faction/location/lore id is invented outside existing lists.

## Verification + CROSS-TOOL QA (per AGENTS.md)
- **EditMode test** (`GreenhouseSystemTests.cs`, sibling-style): mature-from-sprout; tainted irrigation → `tainted_food`; drought → `OnPlotDriedOut` + stall; forced blight → outbreak/fail; save round-trip (`CaptureState`/`RestoreState`); unlock gate (wheat before/after). Seeded rng for determinism.
- **JSON validity**: `json.load` on `greenhouse_items.json` + appended `locations.json`/`world_history.json`.
- **Compile**: attempt a Unity batch compile / `dotnet build` of the Unity-generated solution if available; **report exactly what ran and what didn't** (Unity may not be installed in this environment — I will say so explicitly rather than claim PASS without running it).
- **CROSS-TOOL QA rule** (system has ≥2 coupled variables): I implement, then a **separate review agent** reviews the `GreenhouseSystem` diff **against only the spec above** (not my reasoning) — reviewing the code, not the story.

## Next prompt to run after delivery
> "Review the Glass Orchard expansion: have a fresh agent audit `GreenhouseSystem.cs` + `GreenhouseEventFactory.cs` against the design spec for save-safety, determinism, and id consistency; then wire a Phase-11 greenhouse HUD panel that surfaces Plant/Water/Harvest/TreatBlight."