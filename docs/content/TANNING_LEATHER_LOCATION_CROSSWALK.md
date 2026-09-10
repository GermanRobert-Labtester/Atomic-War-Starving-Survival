# TANNING LEATHER LOCATION CROSSWALK — Plan 159 (Workstreams D & F)

> Mapping of authored facility labels (`tannery_vat_id`, `beamhouse_pit_id`,
> `currying_workshop_id`, `mineral_tan_liquor_id`) onto canonical world
> identities. Facility labels are **equipment designations, never location
> IDs** (Plan 159 §5 Rule 8). Producers are restricted to EXISTING deep-lore
> sites (`Assets/StreamingAssets/Data/deep_lore_locations.json`, 25 sites),
> validated fail-closed by `LeatherworkArchiveSystem.TryRegisterProducer`.

## 1. Facility-label audit (Workstream D)

All 30 authored labels are machine/fixture designations (pits, drums, tubs,
benches, bays). None matches a `loc_*`/`location_*` identity, and none is
promoted to one. Labels remain provenance strings rendered in the archive
projection (`LeatherworkRecord.FacilityLabel`).

| Label family | Count | Examples | Interpretation |
|---|---|---|---|
| Tannery vats/pits | 8 | `LAY_AWAY_PIT_ROW_ALPHA_01`, `SUBTERRANEAN_RIVER_WILLOW_PIT` | vegetable-tanning fixtures |
| Mineral liquor drums/vats | 8 | `WHITE_TAWING_DRUM_UNIT_01`, `HEAVY_MINERAL_ARMOR_VAT` | chemical-bath fixtures |
| Beamhouse pits/stations | 7 | `DELIMING_WASHER_PADDLE_01`, `WARM_TRANSIT_HOLDING_CELLAR` | hide-prep fixtures |
| Currying benches/bays | 7 | `HEAVY_HARNESS_CURRYING_BENCH_01`, `POWER_TRANSMISSION_BELT_SHOP` | workshop fixtures |

Two labels carry explicit geographic signal and were matched accordingly:
`DRAINAGE_OUTFALL_SUMP_CANAL` (drainage canal) and
`POWER_TRANSMISSION_BELT_SHOP` (drive-belt manufacture). All other labels
carry no world geography — mapping relies on the *practice context* of the
record (bark → woodland sites; chemistry → chemical plant; hides → abattoir).

## 2. Producer map (Workstreams D & F)

29 of 30 records are assigned to **10 real deep-lore sites**; all four
process families are covered (≥5 producers required — 10 delivered).

| Producer (existing site) | Records | Family coverage | Contextual rationale |
|---|---|---|---|
| `location_upland_logging_camp` | #1–#4 | OakBarkVegetableTan | oak-bark sourcing woodland camp; pit-log provenance |
| `location_agricultural_research` | #5–#6 | OakBarkVegetableTan | botanical sumac/willow experiments |
| `location_steelworks` | #7, #26, #30 | Bark + Currying | spent tanbark burned in the record's own "foundry reverberatory furnace"; coal-fired steam (red rot); power-transmission drive context (rawhide belts) |
| `location_drainage_network` | #8 | OakBarkVegetableTan | record's own label: `DRAINAGE_OUTFALL_SUMP_CANAL` |
| `location_chemical_plant` | #9–#12, #15–#16 (6) | MineralTanLiquor | chromium/dichromate/syntan chemistry |
| `location_ammunition_depot` | #13, #24, #25 | Mineral + Currying | military armor vat; heavy draft-gear maintenance and cold vault storage |
| `location_automated_abattoir` | #17–#23 (7) | RawhideBatingFailure | the hide source; beamhouse/curing failures |
| `location_police_station` | #27 | HarnessCurrying | leather equipment-shed repair context |
| `location_metro_station` | #28 | HarnessCurrying | underground moisture / stitch-rot context |
| `location_frozen_wetland` | #29 | HarnessCurrying | mud-trudging boot-dressing context (record: "dry socks after twelve-hour shifts") |

**Deferred (1):** `mineral_tan_formaldehyde_synthetic_oil_tannage` (#14,
chamois aviation fuel-filter tannage) — no aviation/fuel-filter site exists;
left archival-depth rather than force a mismatch (`DeferredRecordIds()`
pins this; `ProducerMap_ValidLocations_NoInventedSites` pins real-site discipline).

## 3. Routes not taken (anti-invention record)

- **No tannery location invented.** The corpus implies a working tannery for
  ~20 years, but no canonical `loc_*` tannery exists and none is created.
  The `COAL_MINE_HAULAGE_DEPOT_02` label (red-rot record #26) has no coal-mine
  location — mapped to `location_steelworks` on the coal-fired-steam context
  and documented here as an approximation.
- **No faction ownership invented from prose** (Risk 16): no record names a
  faction; no faction is assigned.
- **Room producers** (`room_workshop`, per Plan 157) were considered and
  **not wired** in this plan: Plan 157's shelter-room producer hook is not
  currently invoked by any runtime path, so attaching leather records to it
  would create a dead producer. Deferred with a note in the completion report.

## 4. Discovery routing (Workstream F — producer routes implemented)

| Route | Mechanic | Records reachable |
|---|---|---|
| Expedition site discovery | `ExpeditionSystem.OnLocationDiscovered` → `DiscoverAtProducer(locationId)` | all 29 producer-assigned records |
| Item inspection | `Main.OnInventoryItemSelected` → `DiscoverForItem(itemId)` | 4 canonically-linked records (also discoverable at their sites) |
| First-discovery feedback | `_journal.TryAddRawEntry` with per-family banner (`TANNERY PIT LOG`, `TANNING LIQUOR ASSAY`, `BEAMHOUSE FAILURE REPORT`, `HARNESS CURRYING AUDIT`) | — |

This yields **5+ real discovery producers** (10 site producers + 3 item
producers: `gas_mask`, `item_preservation_salt`, `leather_strap`), each of
the four families reachable through at least one route.
