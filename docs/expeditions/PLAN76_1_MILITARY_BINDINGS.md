# Plan 76.1 — Military / Ammunition Family Scavenging-Table Bindings

Fourth family of the 42-destination `lootCategories`→Plan 46 table migration.

## Bindings (26 → 30 of 53)

| Destination | Binding | Rationale |
|---|---|---|
| `checkpoint_kilo_armory` | **`table_loot_checkpoint`** (new, shared) | Sealed armory lockers at a small post — 9mm/MRE/bandage/gas-mask ecology. `table_loot_military_depot` is a radiation-hazard deep depot (hazmat, lead cask, 5.56-heavy) — wrong identity and wrong tier. |
| `loc_garrison_checkpoint_gamma` | **`table_loot_checkpoint`** (new, shared) | Same checkpoint ecology (sandbags, boom barrier, martial-law levies). Table sharing is established Plan 46 practice (farm ×2, school ×2, substation ×2). |
| `loc_conscription_office` | **`table_loot_conscription_office`** (new) | Bureaucratic-military: a driving-licence bureau that changed its forms — records, ledgers, and a strongroom with service sidearm rounds. Paperwork identity the depot/police tables don't carry. |
| `loc_ordnance_shoulder` | **`table_loot_ordnance_shoulder`** (new) | The catalog's only bulk-ordnance site: 762/12g/brass/powder carried by no existing table (the depot's ammo entries are 9mm/5.56 with rad-depot dressing). |

No new item ids invented (Plan 76 §1.10). Codex refs reuse existing ids only
(`codex_patrol_order`, `codex_requisition_4471`, `codex_deployment_map`).

## New table signatures (Plan 76 §30)

| Table | Entries | Total weight | Common share | Base hazard | Signature |
|---|---:|---:|---:|---|---|
| `table_loot_checkpoint` | 9 | 139 | 79% | none 0.00 | post rounds (9mm modest), MREs, sandbag cloth, patrol orders |
| `table_loot_conscription_office` | 8 | 115 | 84% | none 0.00 | records/books first, sidearm rounds second — an office, not an armory |
| `table_loot_ordnance_shoulder` | 9 | 132 | 67% | chemical 0.10 | bulk 762/12g, brass, propellant (powder entry carries item-level chemical 0.15) |

Balance notes (§53): checkpoint and office keep ammunition **modest** (quantity
bands well under the depot's 15–40) — §14's rule that the depot must not become
a trivial ammo source applies doubly to its smaller cousins. The Ordnance
Shoulder is the deliberate exception: d7 approach, 8-tick haul, chemical
hazard, and the highest per-visit ammunition yield in the catalog — the
"might solve our ammunition problem, may not come back" destination.
*(Plan 76.2 post-sim trim applied: stack bands reduced ~60% after the balance
sim flagged a 3.5× economy outlier — see docs/balance/
BALANCE_SIM_EXPEDITION_DESTINATIONS.md; E[value] 216.9 → 114.4, best-ammo
identity retained.)*
Distinctiveness (§52): post ordnance ≠ bulk ordnance ≠ induction paperwork.

## Progression

11 original + 5 medical + 5 mechanical/fuel + 5 household/commercial + 4
military = **30 / 53** bound. Remaining unbound: **23** —
electrical/communications, agricultural remainder, water/chemical,
settlements, administrative/knowledge remainder, deep/endgame.
