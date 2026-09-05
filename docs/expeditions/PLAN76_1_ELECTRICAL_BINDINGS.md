# Plan 76.1 — Electrical / Communications Family Scavenging-Table Bindings

Fifth family of the 42-destination `lootCategories`→Plan 46 table migration.

## Bindings (30 → 36 of 53)

| Destination | Binding | Rationale |
|---|---|---|
| `location_silent_observatory` | `table_loot_observatory` (existing) | Exact location-type and signature match — mountain-top scientific station: electronic scrap, wire, batteries, lead glass, dosimetry. |
| `loc_radio_relay_mast` | **`table_loot_relay_mast`** (new) | Comm-relay identity — `vacuum_tube`/`handheld_radio`/`aa_batteries` carried by no existing table; the powered-hut mystery drives the rare radio logs. |
| `loc_transit_authority_hq` | **`table_loot_transit_depot`** (new) | Road-fleet depot + dispatch identity (route maps under glass) — rail_yard is rail; this is road transit with evacuation-route documents. |
| `location_geo_thermal_plant_ruins` | **`table_loot_geo_thermal_plant`** (new) | Plant-works identity — heavy pipe/steel salvage with thermal-paste rarity; hazard chemical 0.15 for the breathing vapor and boiling mud. |
| `location_arcology_sector_4` | **`table_loot_arcology_sector_4`** (new) | Sealed-habitat identity — solar cells, filters, stocked stores; the door is the danger, not the loot (hazard none). Casualty-list record reuses existing `codex_sector4_casualties`. |
| `loc_settlement_tinkers_notch` | **`table_loot_tinkers_notch`** (new, **renewable**) | §42 boundary handled explicitly: a living market's loot is *trade stock*, not scavenged debris — first table in the catalog with `depletion_model: renewable` to model stock that turns over. |

No new item ids invented (Plan 76 §1.10). Codex refs reuse existing ids only
(`codex_civil_defense`, `codex_sector7_evacuation`, `codex_sector4_casualties`,
`codex_debt_default`).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_relay_mast` | 9 | 168 | 85% | none 0.00 | tubes, radios, AA cells, radio logs |
| `table_loot_transit_depot` | 8 | 136 | 93% | none 0.00 | fleet parts + dispatch paperwork |
| `table_loot_geo_thermal_plant` | 9 | 176 | 88% | chemical 0.15 | pipe/steel plant salvage, thermal paste |
| `table_loot_arcology_sector_4` | 9 | 146 | 82% | none 0.00 | sealed-store habitat stock (cells, filters, MREs) |
| `table_loot_tinkers_notch` | 7 | 135 | 92% | none 0.00 | market stock: copper, cells, chips (renewable) |

Distinctiveness (§52): relay comm-parts ≠ fleet depot ≠ plant works ≠ sealed
habitat stock ≠ market stock. The settlements precedent is recorded: the two
remaining settlements (`loc_settlement_pilgrim_hearth`, `loc_settlement_brine_pans`)
should get the same renewable trade-stock treatment in the settlements family
pass for consistency.

## Progression

11 original + 5 medical + 5 mechanical/fuel + 5 household/commercial + 4
military + 6 electrical = **36 / 53** bound. Remaining unbound: **17** —
water/chemical (5), agricultural remainder (4), administrative/knowledge (3),
settlements (2), deep/endgame (3).
