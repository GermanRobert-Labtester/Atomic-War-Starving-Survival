# Plan 76.1 — Mechanical / Fuel Family Scavenging-Table Bindings

Second family of the 42-destination `lootCategories`→Plan 46 table migration.
Scope: the mechanical/fuel-signature unbound destinations.

## Correction to the suggested roster

`rural_gas_station` and `ruined_garage` were already bound in the original
Plan 46 eleven (`table_loot_industrial_district` / `table_loot_warehouse`) —
the actionable set was 5 destinations. Also, `loc_grain_silo` (The Grain
Exchange) is grouped under the **agriculture** signature in
`PLAN76_BALANCE_AUDIT.md`; it is bound by reuse here rather than given a
mechanical table, per its authored loot intent.

## Bindings (16 → 21 of 53)

| Destination | Binding | Rationale |
|---|---|---|
| `collapsed_building` | **`table_loot_collapsed_structure`** (new) | Structural-rubble salvage (planks, nails, concrete, rebar) — no existing table covers building-material wrecks; `table_loot_warehouse` is intact-goods flavour. |
| `loc_grain_silo` | `table_loot_farm` (existing) | The Grain Exchange's authored loot (`canned_food, seed_packets, cloth, scrap_metal`) sits inside the farm ecology; reuse-first. |
| `loc_weighbridge` | **`table_loot_weighbridge`** (new) | Its authored signature includes `diesel_fuel`, which **no existing table carries**; official weigh-station identity (ledgers, seals, fleet fuel) is distinct. |
| `loc_recovery_yard` | **`table_loot_recovery_yard`** (new) | Vehicle-parts yard with `engine` — no existing table carries engines; drains/fluids/numbered hoists identity. |
| `loc_diesel_tank_farm` | **`table_loot_tank_farm`** (new) | Bulk fuel storage is its whole identity; the only bulk-diesel table in the catalog. |

No new item ids invented (Plan 76 §1.10). Codex refs reuse existing ids only
(`codex_requisition_4471`, `codex_warehouse_discrepancy`).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_collapsed_structure` | 9 | 164 | 97% | none 0.00 | building materials: planks, nails, concrete, rebar |
| `table_loot_weighbridge` | 10 | 170 | 88% | chemical 0.10 | diesel + fleet fuel, mechanical parts, requisition ledgers |
| `table_loot_recovery_yard` | 11 | 198 | 81% | chemical 0.10 | scrap/steel volume with rare `engine`, lubricant, batteries |
| `table_loot_tank_farm` | 7 | 161 | 94% | chemical 0.15 | bulk diesel/petrol/canisters — the fuel depot table |

Balance notes (§53): `table_loot_tank_farm` is deliberately fuel-dominant — it
is the only place bulk diesel flows — but it is priced by danger 6, chemical
hazard 0.15 (vapour/bund risk, the only entry with an item-level chemical
hazard on cloth), and a 7-tick approach. No table overlaps another's signature
(§52): rubble ≠ fleet diesel ≠ engines ≠ bulk fuel.

## Progression

11 original + 5 medical + 5 mechanical/fuel = **21 / 53** bound.
Remaining unbound: **32** (household, administrative, military, agricultural
remainder, electrical, settlements, deep/endgame families).
