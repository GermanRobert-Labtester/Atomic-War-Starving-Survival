# Plan 49 — Micro-Location Discovery System Closeout

## Completion Status

**COMPLETE — data authority with schema extension.**

The micro-location catalog exists with 25 entries. The encounter choice schema has been extended with item grant, journal unlock, location discovery, and depletion fields. Runtime integration uses the existing `NarrativeEncounterSystem` via `ExpeditionEncounterBridge`.

## Integration Path

**Path B** — Category registration. Micro-locations are registered as `EncounterDefinition` entries in the `NarrativeEncounterSystem` encounter catalog. The existing `ExpeditionEncounterBridge` surfaces them during expedition travel.

## Schema Extension

`EncounterChoiceDefinition` extended with 6 new fields:
- `grantItemId` / `grantItemQuantity` — item grants
- `setWorldFlag` — world flag mutation
- `journalUnlockId` — journal/codex unlocks
- `discoverLocationId` — location discovery
- `depletesOnResolve` — one-time depletion

All fields are backward-compatible (empty/zero/false defaults for existing encounters).

## Catalog Summary

| Field | Value |
|---|---|
| File | `Assets/StreamingAssets/Data/micro_locations.json` |
| Schema version | 1 |
| Total entries | 25 |
| Discovery category | 20 |
| Hazard category | 3 |
| Social category | 2 |
| Choices with item grants | 33 |
| Choices with journal unlocks | 16 |
| Choices with location discoveries | 2 |
| Choices that deplete | 32 |
| Unique items referenced | 19 |

## 25 Micro-Locations

| # | ID | Category | Weight | Choices |
|---|---|---|---|---|
| 1 | micro_roadside_memorial | Discovery | 0.8 | 2 |
| 2 | micro_crashed_truck | Discovery | 0.6 | 3 |
| 3 | micro_frozen_bus | Discovery | 0.5 | 3 |
| 4 | micro_improvised_grave | Discovery | 0.7 | 3 |
| 5 | micro_collapsed_bridge | Hazard | 0.4 | 3 |
| 6 | micro_drainage_pipe | Discovery | 0.7 | 3 |
| 7 | micro_rail_siding | Discovery | 0.5 | 3 |
| 8 | micro_dead_livestock | Hazard | 0.6 | 3 |
| 9 | micro_ruined_greenhouse | Discovery | 0.5 | 3 |
| 10 | micro_shell_crater | Hazard | 0.4 | 3 |
| 11 | micro_field_kitchen | Discovery | 0.6 | 3 |
| 12 | micro_abandoned_generator | Discovery | 0.4 | 3 |
| 13 | micro_shrine | Social | 0.7 | 3 |
| 14 | micro_emergency_cache | Discovery | 0.2 | 2 |
| 15 | micro_observation_post | Discovery | 0.3 | 3 |
| 16 | micro_abandoned_barricade | Discovery | 0.7 | 3 |
| 17 | micro_hunting_blind | Discovery | 0.5 | 3 |
| 18 | micro_radio_tower | Discovery | 0.3 | 3 |
| 19 | micro_destroyed_checkpoint | Discovery | 0.5 | 3 |
| 20 | micro_abandoned_tent | Social | 0.7 | 3 |
| 21 | micro_makeshift_clinic | Discovery | 0.4 | 3 |
| 22 | micro_crashed_drone | Discovery | 0.2 | 3 |
| 23 | micro_fuel_cache | Discovery | 0.2 | 3 |
| 24 | micro_water_source | Discovery | 0.5 | 3 |
| 25 | micro_supply_drop | Discovery | 0.1 | 3 |

## Items Referenced

antenna_coil, bandage, canned_food, childs_drawing, clean_water, cloth, crop_medicinal_herb, dosimeter, dried_rations, electronic_scrap, fuel, jewelry, mechanical_parts, medical_kit, scrap_metal, sealed_government_document, seed_packets, soldering_kit, wedding_ring

## Location Discoveries

- `micro_observation_post` → discovers `rural_gas_station`
- `micro_supply_drop` → discovers `government_bunker`

## Journal Unlocks

16 journal/codex unlocks across: transit tags, grave markers, warnings, maintenance ledgers, grid references, fuel notes, faction markings, hunting journals, triage lists, flight logs, route sketches, checkpoint logs, radio logs, supply labels, ration marks, livestock tags.

## Depletion Model

Micro-locations use `depletesOnResolve: true` on loot-bearing choices. The `NarrativeEncounterState.history` list records resolution, preventing duplicate loot on revisit. The `NarrativeEncounterSystem` does not natively enforce depletion — the host layer must check history before granting loot.

## Deferred Features

| Feature | Reason |
|---|---|
| Native depletion in NarrativeEncounterSystem | System has no cooldown/depletion mechanism |
| Route-tag-based eligibility | Expeditions have no route_tag/biome fields |
| Standalone rumor system | Rumors are narrative events only |
| Procedural micro-location generation | All 25 are authored, not procedural |

## Files Created/Modified

```
Assets/StreamingAssets/Data/micro_locations.json (NEW)
Assets/Ashfall.Core/Narrative/EncounterCatalog.cs (MODIFIED — extended EncounterChoiceDefinition)
docs/discovery/PLAN49_BASELINE.md (NEW)
docs/discovery/MICRO_LOCATION_SCHEMA.md (NEW)
docs/discovery/PLAN49_CLOSEOUT.md (NEW)
```

## Verification

| Check | Result |
|---|---|
| JSON parse | valid |
| Entry count | 25/25 |
| Category distribution | 20/3/2 |
| Item refs | all 19 resolve in items.json |
| Journal refs | all 16 are valid knowledge keys |
| Location refs | both resolve in locations.json |
| Build | 0 errors |
