# Plan 76.1 — Water / Chemical Family Scavenging-Table Bindings

Sixth family of the 42-destination `lootCategories`→Plan 46 table migration.

## Bindings (36 → 41 of 53)

| Destination | Binding | Rationale |
|---|---|---|
| `loc_terrace_pumphouse` | **`table_loot_waterworks`** (new, shared) | Working irrigation-head pump hall — metal/filter/clean-water ecology. |
| `loc_pump_station_nine` | **`table_loot_waterworks`** (new, shared) | Six drainage pumps drowned in the basin they were built to keep dry — same municipal machinery identity. |
| `loc_lock_gate_four` | **`table_loot_waterworks`** (new, shared) | Seized lock-gate mechanism — gates, pipes, and the water they held. Three-way sharing avoids three near-duplicate tables (§52). |
| `loc_public_swimming_baths` | **`table_loot_swimming_baths`** (new) | Distinct identity: chlorine treatment room + the mattress-floor shelter. Chemical hazard 0.10 (treatment chemicals carry item-level chemical 0.15). |
| `location_flooded_subway_depot` | `table_loot_metro_station` (existing) | Drowned transit depot, 40 rads dissolved in the water, wader suit required — the metro table's radon/filters/dirty-water ecology is exact. |

No new item ids invented (Plan 76 §1.10). No codex refs (the water notice
document carries no unlock in existing tables' convention).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_waterworks` | 9 | 182 | 94% | disease 0.05 | filters, pipe, clean water; `dirty_water` entry carries item-level disease 0.20 |
| `table_loot_swimming_baths` | 7 | 125 | 94% | chemical 0.10 | purification tablets, treatment chemicals, shelter bedding |

Balance notes (§53): waterworks sites yield working-infrastructure parts at
d5–d7 with a modest stagnation-disease hazard; the baths are the safe,
shelter-adjacent water source (d6 but none of the flood exposure). Distinct
from `table_loot_chemical_plant` (industrial feedstock, chemical 0.30) —
water treatment is not chemical manufacturing.

## Progression

11 original + 5 medical + 5 mechanical/fuel + 5 household/commercial + 4
military + 6 electrical + 5 water/chemical = **41 / 53** bound.
Remaining unbound: **12** — agricultural remainder (4), administrative/knowledge
(3), settlements (2, renewable trade-stock precedent), deep/endgame (3).
