# Relic Restoration Runtime Contract

*Plan 87 — verified 2026-09-06 against current source. No code was changed.*

## Authoritative catalog (Task 87A resolution)

| Question | Answer |
|---|---|
| Authoritative workshop relic file | `Assets/StreamingAssets/Data/relic_recipes.json` |
| Does `relic_inks.json` exist? | **No.** The brief's "relic_inks.json has 6 entries" was a typo/phantom reference. No loader, test, or data file references it. |
| Loaded by | `Assets/Ashfall.Core/Crafting/RelicCatalogLoader.cs` (`FileName = "relic_recipes.json"`, classification: Required) |
| Consumed by | `WorkshopReverseEngineeringSystem.LoadCatalog` (Core), `CraftingHostSession` (host), `WorkshopPanel` (UI), `ContentUtilizationScanner` |
| Catalog size at Plan 87 start | 30 records: 6 cultural (`category: "relic"`) + 24 tech (`relic_tech_common` / `relic_tech_rare` / `relic_tech_military`) |

The Plan 87 brief's "six verified relic records" are exactly the six
`category: "relic"` records (gramophone, film_projector, ham_radio,
music_box, typewriter, camera). The 24 `relic_tech_*` records are a
later functional-salvage progression (Plan 166 era) with `morale_bonus: 0`.
Plan 87's "15 relics" target applies to the **cultural tier**: 6 existing + 9 new.
The tech tier is untouched.

## Schema (root)

```json
{ "schema_version": 1, "recipes": [ { ... } ] }
```

`CatalogLocator.LoadWrappedList` unwraps the `recipes` array.

## Record schema (`RelicJsonDto` → `RelicDefinition`)

| Field | Type | Required | Default | Notes |
|---|---|---|---|---|
| `relic_id` | string | yes | — | unique; first-wins on duplicate load; cultural tier uses bare descriptive ids (`gramophone`), tech tier uses `relic_*` prefix |
| `display_name` | string | no | falls back to `relic_id` | raw string (no localization keys) |
| `description` | string | no | `""` | raw string |
| `required_components` | `List<string>` | no | `[]` | **item IDs, exactly 1 unit each** — no quantity objects. Consumed atomically via `Inventory.TryConsumeBill(IEnumerable<string>)` at `StartRepair`; refunded on `CancelJob` |
| `repair_time_hours` | float | no | `8f` | loader coerces `<= 0` to 8; divided by survivor skill multiplier at `StartRepair` |
| `morale_bonus` | int | no | `0` | emitted once as `deltas["morale_bonus"]` on repair completion; host/UI applies. One-shot per relic (guarded by `completedRelicIds`) |
| `dialogue_event_id` | string | no | `""` | Tier-2 reference key in `CatalogIntegrityValidator` — value must resolve to a registered event ID in `events.json`. Not consumed by the workshop runtime itself; it is data-linked narrative wiring |
| `restoration_text` | string | no | `""` | raw prose; displayed by the workshop UI on completion |
| `world_flag` | string | no | `""` | emitted as `deltas["flag_" + world_flag] = 1` on repair completion. `world_flag` itself is a *definition* key (not a reference key) — no registry validation, so uniqueness is a data-author responsibility. Convention: `relic_restored_<relic_id>` |
| `research_unlock_id` | string | no | `""` | if non-empty, must exist in `research_knowledge.json` — pinned at exactly **16** resolved unlocks by `RelicResearchUnlockContractTests` |
| `dismantle_yield_item` / `dismantle_yield_amount` | string / int | no | `""` / `1` | dismantle path yield |
| `category` | string | no | `"relic"` | `"relic"` (cultural) vs `relic_tech_*` (functional) |

## Runtime semantics (`WorkshopReverseEngineeringSystem`)

- **Availability:** every loaded catalog entry is a candidate; duplicate IDs are first-wins (loader skips later duplicates).
- **Repair flow:** `StartRepair` (busy-check → completion-check → atomic component bill consume) → `TickProgress(hours)` until `progressHours >= hoursRequired` → `CompleteJob`.
- **Morale:** one-time delta at completion. Restoration is not repeatable — `IsRelicCompleted` blocks `StartRepair`/`StartDismantle`/`StartResearch` for the same ID.
- **Event dispatch:** `OnActionCompleted` carries the completion `ActionResult`; narrative events are wired by `dialogue_event_id` data reference into `events.json` (the narrative event engine's authority).
- **Persistence:** `WorkshopState.CaptureState/RestoreState` — `completedRelicIds` (restored relics), reserved components, and in-progress job state round-trip through `CraftingHostSession` (`save.WorkshopState`). **No new save fields needed for Plan 87.**

## Validation surface

- `required_components` → Tier-2 reference key; every ID must resolve in the item registry.
- `dialogue_event_id` → Tier-2 reference key; every ID must resolve to a registered event.
- `world_flag` → definition-only; uniqueness enforced by convention + review (all 15 cultural flags use `relic_restored_<relic_id>`).
- `research_unlock_id` → pinned contract test (16 unlocks).
- `minDay`/`maxDay` ranges on events validated by integrity Tier RANGES.

## Existing tests guarding this catalog

- `WorkshopReverseEngineeringSystemTests` — component consumption, refunds, completion, one-shot guard.
- `RelicResearchUnlockContractTests` — 16 research unlocks resolve; save round-trip.
- `RelicProvenanceCatalogTests` — separate 32-entry provenance dossier domain (`narrative/relic_provenance_dossiers.json`), self-pinned IDs, **not** keyed to `relic_recipes.json`.
- `CatalogIntegrityValidatorTests` / `--data-integrity-selftest`.
