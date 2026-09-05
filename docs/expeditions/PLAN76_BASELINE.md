# Plan 76 — Baseline Reconnaissance Record

> Phase 0 evidence record. All findings verified against the working tree on the day of execution.

## 1. Baseline gate results (all PASS before any change)

| Gate | Command | Result |
|---|---|---|
| Godot host build | `dotnet build Ashfall.csproj` | 0 errors, 0 warnings |
| Core test suite | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | 6680 / 6680 PASS |
| Catalog integrity | `godot --headless --path . -- --data-integrity-selftest` | PASS — 0 findings, 208 catalogs, 10 640 ids authored |
| Expedition selftest | `godot --headless --path . -- --expedition-selftest` | PASS — 19/19 (headless demo + 9 vehicle gates) |

## 2. Repository truth vs. plan assumption

Plan 76 assumed a **2-destination** catalog. The verified baseline is:

| Surface | Count | Source |
|---|---:|---|
| Authored destinations in `expeditions.json` | **53** | counted at execution |
| Loader-merged expedition-capable ids (expeditions.json + locations_expansion3 + locations + year_of_ash_locations + holdfast_locations) | **263** | `ExpeditionCatalogLoader.Load` merge, first-seen dedupe |

The 2 → 15 expansion target is therefore **already exceeded 3.5×** by authored
records and 17× by the dispatchable merged surface. Per Plan 76 §1.1
(repository truth overrides the planning grammar), the quantitative expansion
objective is recorded as **superseded**, not executed as written.

## 3. Parity oracle — the original two destinations

Frozen from `expeditions.json` (also pinned by
`Plan32ExpeditionDestinationWiringTests.Expeditions_OriginalTwoRecordsArePreserved`):

| id | displayName | distanceTicks | dangerLevel | encounter/tick | stamina/hr | table | lootCategories |
|---|---|---:|---:|---:|---:|---|---|
| `loc_the_allotments` | The Works Allotment Commune | 5 | 2 | 0.12 | 2.0 | `table_loot_farm` | scrap_metal, clean_water, bandage*, dried_rations* |
| `loc_denial_cut_substation` | The Denial Cut Substation | 8 | 4 | 0.18 | 3.0 | `table_loot_power_substation` | dosimeter, copper_wire_10m_of_10m*, fuel, item_hydro_baron_queue_chit |

\* = values repaired by Plan 76 (were `bandages`, `food_rations`, `copper_wire`).
IDs, names, distance, danger, encounter chance and stamina drain are **unchanged**.

## 4. Loader semantics (read end-to-end)

`Assets/Ashfall.Core/Expeditions/ExpeditionCatalogLoader.cs`:

1. Loads `expeditions.json` first (primary authority), then merges
   `locations_expansion3.json`, `locations.json`, `year_of_ash_locations.json`,
   `holdfast_locations.json`. First-seen wins (`seen` set) → expeditions.json
   records take precedence on id collisions.
2. Every location entry is loaded as an expedition-capable destination with
   loader defaults: `distanceTicks` = `round(travelHours × 2)` (else 8),
   `encounterChancePerTick` = `Clamp(0.10 + danger × 0.02, 0.05, 0.50)`,
   `baseStaminaDrainPerHour` = `Clamp(1.5 + danger × 0.25, 1.0, 5.0)`.
3. Registered into the static `ExpeditionDefinitionRegistry` (last write wins).
4. Parse failures route through `CatalogDiagnostics.Warn` (path + shape + exception).

## 5. Runtime semantics verified

From `Assets/Ashfall.Core/Expeditions/ExpeditionSystem.cs`:

- **Dispatch:** `Start(def, survivorId, …)` — one active expedition per survivor;
  no world-topology routing. Reachability = presence in the registry
  (§34 resolved: the expedition system uses abstract distance; documented).
- **Encounters:** `RollEncounter` fires **every tick hour** (outbound, looting,
  inbound), chance = `encounterChancePerTick` × host multiplier × 0.5 in Stealth,
  clamped [0,1]. Round trip ≈ `2 × distanceTicks + 3` looting ticks.
- **Stamina:** `baseStaminaDrainPerHour × hours` + encumbrance penalty
  (≤15/tick at full load) × survivor multiplier; capacity 100.
- **Loot authority (dual-mode, Plan 46 live):**
  1. `scavenging_table_id` present + table in `ScavengingTableCatalog` →
     authoritative weighted table roll;
  2. else `lootCategories` strings are used **directly as item ids**
     (`PickLootCategory` → `AddLoot` → inventory).
- **Availability:** no discovery/unlock gate in core; the UI enumerates all
  `Definitions` (additive visibility). Host blocking hooks:
  `IsLocationBlocked` (Crossing gate + `ExtraBlocked`).
- **Save:** mid-expedition state persists `locationId` + travel progress;
  round-trip covered by `MidExpeditionSaveAndRestore_MaintainsDestinationIntegrity`.

## 6. Existing test gates preserved (untouched)

`Ashfall.Core.Tests/Expeditions/Plan32ExpeditionDestinationWiringTests.cs` pins:
count = 53; unique ids; ranges (distance ≥ 1, danger 1–10, encounter
0.05–0.50, stamina 1.0–5.0, non-empty lootCategories); original-two parity;
tier distribution 16/18/13/6; representative dispatch & completion;
mid-expedition save/restore.

## 7. Cross-catalog audits

- **Location identity (§33):** all 53 authored ids exist in the location
  catalogs (Model A — shared canonical identity). No new expedition-only ids.
- **Family coverage (§14):** every planned family role is covered by at least
  one distinct physical site in the merged surface — urban hospital
  (`abandoned_hospital`), metro (`location_flooded_subway_depot`), shopping
  (`loc_department_store`); industrial chemical/tank (`loc_diesel_tank_farm`),
  rail yard (`loc_sector_4_rail_switchyard`, `loc_cut_abandoned_depot`,
  `loc_railway_guild_roundhouse`), substation (`electrical_substation`);
  military depot (`loc_ordnance_shoulder`, `checkpoint_kilo_armory`), checkpoint
  (`loc_garrison_checkpoint_gamma`); scientific weather
  (`loc_granite_pass_weather_observatory`), survey/observatory
  (`location_silent_observatory`, `location_geo_thermal_plant_ruins`);
  wilderness burned woodland (`loc_ash_woodland`), radioactive wetland
  (`loc_black_thaw_drainage_basin`, `loc_poison_gas_culvert_marsh`), treeline
  (`loc_muster_treeline_camp`).
  → Adding the plan's 13 literal entries would duplicate these physical sites
  and violate §1.4 / §33 / §52 (no dead destination).
- **Weather gating (§37/§38):** no destination-level weather fields exist;
  weather blocking belongs to the host `ExtraBlocked` seam. No bindings authored
  (documented as deferred).
- **Micro-locations (§35/§36):** no destination-level binding field exists in
  the current schema; stable ids remain available for a future seam. No
  dangling refs authored.
