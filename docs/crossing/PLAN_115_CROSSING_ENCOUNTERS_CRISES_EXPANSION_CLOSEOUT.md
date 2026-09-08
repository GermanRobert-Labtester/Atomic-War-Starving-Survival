# Plan 115 Crossing Encounters and Crises Expansion Closeout

## Status

**COMPLETE — Crossing content expansion with external hooks deferred.**

The final catalog contains:

- **25 encounters**: 14 existing records preserved plus 11 new records;
- **12 crises**: 5 existing records preserved plus 7 new records;
- 13 valid Crossing location IDs;
- existing `enc_nc_*` encounter grammar;
- existing `crisis_*` crisis grammar.

The plan's 10-encounter baseline was stale in the repository. Adding the
requested 15 records would have produced 29 encounters and violated the
required final count. The implementation therefore reconciles the live
14-entry baseline to the exact 25-entry target.

## New encounter records

### Route and trade

- `enc_nc_bonded_caravan_ambush`
- `enc_nc_smuggler_checkpoint`
- `enc_nc_frozen_barge`
- `enc_nc_toll_bridge_claim`

### Hazards

- `enc_nc_collapsed_crossing`
- `enc_nc_contaminated_ford`
- `enc_nc_ice_fracture`
- `enc_nc_uxo_field_crossing`

### Social and admission

- `enc_nc_refugee_blockade`
- `enc_nc_family_ledger_gate`
- `enc_nc_child_at_gate`

The new records use only `text`, `cost_items`, and `result` choice fields.
They contain no speculative consequence fields. New item references use
`crossing_items.json`; the preserved existing catalog also contains the
canonical global `water_filter` ID. Target locations are limited to
`crossing_locations.json`.

## Runtime contract

`CrossingCatalog` loads the wrapped JSON catalog. `CrossingSession` loads the
catalog and owns the vouch gate, but it does not select or resolve encounter
choices or advance crises. `cost_items` and `result` are therefore authored
content fields, not live inventory/effect commands. Crisis phases and
resolutions are authored prose and are not active state-machine inputs.

The authoritative schema and persistence boundary are documented in
`CROSSING_ENCOUNTER_CRISIS_RUNTIME_CONTRACT.md`.

## External hooks

| Plan/domain | Status | Reason |
|---|---|---|
| Plan 98 faction standing | deferred | no faction or standing field in the active DTO |
| Plan 76 expedition reveal | deferred | no location-reveal field or resolver |
| Plan 102 accords/treaties | deferred | no stable flag/effect field |
| Plan 112 disease | deferred | quarantine remains generic prose; no disease field |
| Plan 89 epilogue history | deferred | no persistent crisis outcome field |

No Core code, save schema, encounter executor, crisis engine, or political
subsystem was added.

## Persistence compatibility

Existing encounter/crisis IDs are unchanged. The active Crossing save state
does not store encounter or crisis IDs/phases/outcomes, so adding records
does not alter old save shape or restore behavior. Selection/distribution
behavior is unchanged because no active selector consumes this catalog.

## Validation target

The implementation validation checks:

- exactly 25 encounters and 12 crises;
- unique encounter and crisis IDs;
- all 25 target locations resolve;
- all choice arrays are non-empty and use only active fields;
- all `cost_items` resolve in the authoritative global or Crossing item
  catalogs;
- every crisis has 4 non-empty, distinct, ordered phase labels;
- JSON syntax and catalog loading;
- existing Crossing tests, data integrity, content utilization, builds, and
  the full regression suite.

## Verification results

- Contract/reference validator: **PASS** — 25 encounters, 12 crises, 13
  locations, unique IDs, preserved baseline IDs, valid choice fields, valid
  item references, and 4-phase crisis records.
- `godot --headless --path . -- --crossing-selftest`: **PASS** — 33/33.
- `godot --headless --path . -- --arbitration-selftest`: **PASS** — 58/58.
- `godot --headless --path . -- --expansion-depth-selftest`: **PASS** —
  23/23.
- Focused Crossing regressions: **PASS** — 83/83.
- `godot --headless --path . -- --data-integrity-selftest`: **PASS** —
  0 errors and 0 warnings across 298 catalogs.
- `godot --headless --path . -- --content-utilization-selftest`: **PASS** —
  0 orphaned catalogs.
- Core and Godot builds: **PASS** — 0 warnings and 0 errors.
- `godot --headless --path . -- --save-store-checksum-selftest`: **PASS** —
  21/21.
- Full `Ashfall.Core.Tests`: **PASS** — 9,891/9,891.
- Production headless boot: **PASS** — exits cleanly after catalog
  initialization. Existing unrelated duplicate-catalog, missing-icon, and
  shutdown-leak warnings remain.

No Core or save-schema changes were made for Plan 115.
