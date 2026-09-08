# Faction War Override Day Window Contract

> **Catalog Authority:** `Assets/StreamingAssets/Data/faction_war_location_overrides.json`
> **Query System:** `FactionWarContentCatalog.GetActiveLocationOverride`
> **Test Gate:** `Ashfall.Core.Tests.FactionWarLocationOverridesExpansionTests.AllOverrides_DayWindowsAreOrderedAndBounded`

---

## 1. Day Window Rules & Invariants

Each location override defines a temporal activation window bounded by two integer values:
1. `activeFromDay >= 1`: The campaign day when the temporal state begins (inclusive).
2. `activeUntilDay`: The campaign day when the temporal state expires (inclusive).
   - If `activeUntilDay == 0`, the override is permanent / unbounded from `activeFromDay` onward.
   - If `activeUntilDay > 0`, it must satisfy `activeUntilDay >= activeFromDay`.

### 1.1 In-Range Evaluation Predicate
In `FactionWarContentCatalog.cs`:
```csharp
if (day < o.activeFromDay) continue;
if (o.activeUntilDay > 0 && day > o.activeUntilDay) continue;
```
Thus:
- At `day = activeFromDay - 1`, the override is **inactive**.
- At `day = activeFromDay`, the override is **active**.
- At `day = activeUntilDay`, the override is **active**.
- At `day = activeUntilDay + 1`, the override is **inactive** (reverting to either an earlier base state or another active override).

---

## 2. Chronological Ordering of All 20 Overrides

| Override ID | Target Location ID | From Day | Until Day | Duration (Days) | Campaign Phase |
|---|---|---|---|---|---|
| `loc_override_almshouse_pre_strike` | `loc_almshouse` | 1 | 100 | 100 | Early Survival |
| `loc_override_plaza_cleared` | `loc_ration_queue_plaza` | 1 | 50 | 50 | Early Survival |
| `loc_override_almshouse_post_strike` | `loc_almshouse` | 101 | 300 | 200 | Mid-Campaign War |
| `loc_override_waystation_refugee` | `loc_shrine_switchback_waystation` | 150 | 280 | 131 | Refugee Movement |
| `loc_override_silo_fortified` | `loc_grain_silo` | 200 | 350 | 151 | Escalation |
| `loc_override_checkpoint_occupied` | `loc_garrison_checkpoint_gamma` | 200 | 250 | 51 | Garrison Mobilization |
| `loc_override_conscription_burned` | `loc_conscription_office` | 220 | 340 | 121 | Civil Collapse |
| `loc_override_granary_burned` | `loc_crossing_granary_pledge` | 220 | 260 | 41 | Border Raid |
| `loc_override_well_contaminated` | `location_municipal_water_reservoir` | 240 | 280 | 41 | Resource Shock |
| `loc_override_weighbridge_barricaded` | `loc_weighbridge` | 250 | 380 | 131 | Transit Control |
| `loc_override_rail_yard_fortified` | `loc_sector_4_rail_switchyard` | 260 | 320 | 61 | Logistics Defense |
| `loc_override_village_abandoned` | `loc_settlement_iron_siding` | 280 | 340 | 61 | Depopulation |
| `loc_override_cache_looted` | `loc_d9_cache_bunker_delta` | 300 | 450 | 151 | Cache Raiding |
| `loc_override_factory_occupied` | `location_chemical_plant` | 300 | 350 | 51 | Industrial Seizure |
| `loc_override_bridge_destroyed` | `loc_bridge_seven` | 310 | 360 | 51 | Chokepoint Denial |
| `loc_override_roadblock_liberated` | `loc_ash_militia_deadfall_barrier` | 330 | 370 | 41 | Militia Repulsion |
| `loc_override_camp_overrun` | `loc_crossing_petition_tent` | 340 | 380 | 41 | Refugee Dispersion |
| `loc_override_station_reclaimed` | `location_metro_station` | 350 | 400 | 51 | Reconstruction Attempt |
| `loc_override_field_scorched` | `location_burned_woodland` | 360 | 400 | 41 | Scorched Earth Campaign |
| `loc_override_understory_transmitter_ambient` | `loc_understory_transmitter` | 480 | 600 | 121 | Late Year of Ash War |

---

## 3. Horizon Distribution

- **Days 1–100 (Early Post-Exchange):** 2 baseline overrides representing pre-strike civil structure.
- **Days 101–200 (Shelter Crisis & Displacement):** 2 baseline overrides showing initial ruins and overcrowding.
- **Days 200–400 (Faction War Mid-Campaign Surge):** 15 overrides (4 baseline + 11 Plan 124 expansion), creating a dense, dynamic tapestry of conflict, occupation, sabotage, and recovery.
- **Days 480–600+ (Late Year of Ash):** 1 baseline override covering deep SIGINT operations.
