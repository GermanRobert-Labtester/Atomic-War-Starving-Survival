# Plan 76 — Baseline Reconnaissance

## Stale-premise reconciliation

The plan brief stated a "verified baseline of 2 destinations." **Repository truth at implementation time: 55 destinations** in `expeditions.json`, with Plan 46's `scavenging_table_id` binding live on every entry, `requiresDiscovery` gating on 2 entries, and danger up to 10. Per §1.1 (repository truth overrides the planning grammar), the plan was executed as a **gap-closure expansion (55 → 63)**, not a 2 → 15 build-out.

## Concept-coverage audit (plan's 13 proposed concepts vs. the existing 55)

| Plan concept | Existing coverage | Gap? |
|---|---|---|
| Urban hospital | `abandoned_hospital`, `hospital_pharmacy`, `prewar_medical_cache`, `loc_st_brigids_almshouse` | no |
| Urban metro | `location_flooded_subway_depot` | no |
| Urban shopping center | `loc_department_store` (Vansen's) | no |
| Industrial chemical plant | `table_loot_chemical_plant` exists (bound to water_station) but **no true plant destination** | **YES** |
| Industrial rail yard | `table_loot_rail_yard` exists and is **completely unused** | **YES** |
| Industrial substation | `electrical_substation`, `loc_denial_cut_substation` | no |
| Military ammunition depot | `loc_ordnance_shoulder` (ammo); **`table_loot_military_depot` unused** | **YES** (depot-scale, distinct from ordnance shoulder) |
| Military checkpoint | `checkpoint_kilo_armory`, `loc_garrison_checkpoint_gamma` | no |
| Scientific weather station | none (observatory is astronomy) | **YES** |
| Scientific geological survey | none | **YES** |
| Wilderness irradiated forest | none (`loc_forestry_compound` is industrial) | **YES** |
| Wilderness frozen wetland | none | **YES** |
| Wilderness burned woodland | none | **YES** |

**Result: 8 genuine gaps closed (55 → 63); 5 concepts skipped as already covered (often multiple times).** Adding the skipped 5 would duplicate existing sites, violating §33/§52.

## Loader contract (read end-to-end)

`ExpeditionCatalogLoader` — wrapped list, `ExpeditionJsonDto` (id, displayName, distanceTicks, travelHours fallback, dangerLevel, encounterChancePerTick, baseStaminaDrainPerHour, lootCategories, scavenging_table_id, requiresDiscovery). Loads `expeditions.json` then 4 location catalogs (first-seen ID wins). Defaults/clamps: encounterChance `clamp(0.10 + danger*0.02, 0.05, 0.50)` when absent; drain `clamp(1.5 + danger*0.25, 1.0, 5.0)`; distance fallback `travelHours * 2`. Registers into `ExpeditionDefinitionRegistry`.

## Range semantics (from tests + runtime)

- `distanceTicks` ≥ 1 (existing spread 2–18)
- `dangerLevel` 1–10 (int-rounded)
- `encounterChancePerTick` ∈ (0, 1), clamped ≤ 0.50
- `baseStaminaDrainPerHour` > 0, clamped ≤ 5.0
- All 8 new entries carry explicit values within these bounds.

## Baseline health (55-state, pre-change)

- All 55 `scavenging_table_id` refs resolved; 0 range violations; 0 duplicate IDs.
- 8 unused Plan 46 tables — `rail_yard`, `military_depot`, `chemical_plant` (partially), `clinic`, `fire_station`, `greenhouse`, `hunting_cabin`, `monastery`, `police_station` — reuse-first bindings chosen for 3 of them.

## Loot authority (reconciled with Plan 46 — LIVE)

Case C/D hybrid: `scavenging_table_id` is the authoritative weighted-loot binding (Plan 46 `scavenging_tables.json`, 49 → 54 tables); `lootCategories` remains a display/signature field validated by `ExpeditionLootValidator` through `ExpeditionLootReferenceResolver` (item IDs across merged item catalogs, or semantic category tokens). Both were kept consistent for every new entry. No new items were created (§32 gate).

## Cross-plan seams audited

- **Plan 49 micro-locations:** `micro_locations.json` binds by location identity; no destination-ID→approach hook is wired in the loader. The three target concepts (hospital, metro, checkpoint) already have destinations; new stable IDs are binding-ready. **Deferred, no dangling refs.**
- **Plan 48 weather gates:** weather gating has no destination-eligibility field in the current schema. **Deferred; no authored weather locks.**
- **Plan 58/50/59:** stable `loc_*` IDs are the delivered hook; no narrative content authored here.

## Baseline gates (55-state, all green before edits)

`--data-integrity-selftest` PASS · `--expedition-selftest` PASS · full xUnit green · build clean.
