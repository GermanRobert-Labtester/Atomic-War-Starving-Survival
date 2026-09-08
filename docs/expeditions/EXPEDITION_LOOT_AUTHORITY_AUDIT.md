# Plan 76 — Loot Authority Audit & Cross-Plan Bindings

## Loot authority decision (Plan 46 reconciled)

**Plan 46 is implemented and is the loot authority.** Every destination — all 63 — binds `scavenging_table_id` to a weighted location-specific table in `scavenging_tables.json` (54 tables). `lootCategories` is a secondary signature field validated by `ExpeditionLootValidator` via `ExpeditionLootReferenceResolver` (item IDs from merged catalogs, or semantic category tokens — Case C mixed namespace).

- **0 unresolved `scavenging_table_id` refs** (63/63 bound — pinned by `AllScavengingTableReferences_ResolveAgainstPlan46Authority`).
- **0 unresolved `lootCategories` tokens** across the full multi-catalog item merge (verified programmatically; pinned by `AllLootCategories_ResolveAgainstMergedItemCatalog`).
- **0 new items created** (§32 reuse-first gate held — e.g., `mechanic_gloves` reused instead of inventing a gloves item; `rubber_gloves` was caught and rejected during authoring).
- Table-integrity contract (≥6 entries, positive weights, min ≤ max quantities) holds for all 5 new tables.

## Plan 49 micro-location bindings — DEFERRED (binding-ready)

Audit found no destination-ID → approach-discovery seam in the current loader/runtime (`micro_locations.json` binds through `NarrativeEncounterSystem.TryResolve` by its own location identity). Therefore **no production refs were authored**. Binding-ready stable IDs for the plan's three micro-location targets already exist: `abandoned_hospital` (hospital), `location_flooded_subway_depot` (metro), `checkpoint_kilo_armory` / `loc_garrison_checkpoint_gamma` (checkpoint). Future Plan 49 work binds against these IDs; nothing dangling ships today.

## Plan 48 weather gates — DEFERRED (no seam)

The destination schema has no weather-eligibility field, and the loader implements no weather checks (per §13/§38, none were invented). The two target concepts are represented by `loc_marsh_hollow` (frozen wetland — blizzard/whiteout candidate) and `loc_mirrim_forest_edge` (irradiated forest — fallout-storm candidate) with stable IDs ready for a future eligibility seam. Weather authority (`WeatherSystem` / Plan 48) remains untouched.

## Plan 58 / 50 / 59 hooks

Stable `loc_*` IDs are the delivered integration surface. No encounter pools, distress signals, or quest targets were authored here. Natural future anchors: `loc_north_freight_yard` (caravan/route investigation), `loc_blackridge_ammunition_depot` + `loc_birchline_weather_station` (remote signal/quest sources), `loc_vulcan_works_chemical` (engineer/chemistry questlines).

## Settle boundary (Plan 43)

The three `loc_settlement_*` destinations pre-date Plan 76 and follow the existing social/trade destination convention; no new settlement entries were added.
