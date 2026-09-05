# Plan 76 ↔ Plan 85 Destination Reconciliation

Plan 76's expedition destination authority (`expeditions.json`, 53 authored destinations at baseline) was inventoried by concept against the Plan 85 candidate pool before authoring.

## Duplicates avoided (concept-level, not just id-level)

| Candidate concept | Existing Plan 76 content | Resolution |
|---|---|---|
| Municipal Archive Vault (planned Zone 2) | `loc_municipal_archive` — "Municipal Archive" | **dropped**; substituted with Court District / Evidence Sub-Basement (distinct facility: evidence custody, not records archive) |
| Cooperative Root Reserve (planned Zone 3, seeds) | `loc_seed_library_annex` + zone 2's `municipal_seed_vault` | **dropped**; substituted with Pasture Valley / Quarantine Barn (veterinary, not seeds-first) |
| Flood-control pump station (alternative pool) | `loc_pump_station_nine` (waterworks) | **avoided** |
| Hardened Signal Annex (planned Zone 9) | `loc_hidden_relay_bunker` + `loc_broadcast_bunker_echo` (comms/relay cluster) | **dropped**; substituted with Metro Service Ring / Electrical Maintenance Exchange (traction power, distinct from the pre-existing `electrical_substation` road substation) |
| Upland weather calibration vault (planned Zone 6) | Plan 76's named `weather_station` concept + `table_loot_observatory` overlap | **dropped** in favor of Materials Research Sublevel (observatory table reused as producer only) |
| Bonded Marine Warehouse (planned Zone 7) | maritime/dive-site layer (Plan 23) | **dropped** to keep the land/expedition split clean |
| Pre-war medical cache | `prewar_medical_cache` | triage annex kept — mass-casualty intake facility vs field cache; different function, table, and reward set |

## Integrations made

- **12 of 12** hidden installations became expedition destinations (plan minimum: 3), all pre-authored in `expeditions.json` with themed `scavenging_table_id` bindings and `lootCategories` = the zone's `revealed_items`.
- Visibility/access discipline: destinations always exist in the catalog; `ExpeditionSystem.Start` refuses dispatch until the installation is revealed via the damaged-map layer; the UI reports the block reason.
- Count pins updated with the same strictness: Plan 76 loot-reference tests (53→65), Plan 32 wiring/tier tests (65; tiers 21/23/15/6).
- `loc_hidden_relay_bunker` had a pre-existing map node but **no destination**; one was added (see reveal matrix note).

## Reuse, not replacement

No Plan 76 destination was renamed, re-tiered, or removed. Plan 85 destinations ride the same `ExpeditionDefinitionRegistry`, the same `DispatchSortie`/`StartExpedition` path, the same vehicle/stamina/encounter economics, and the same Plan 46 table bindings the source plan installed.
