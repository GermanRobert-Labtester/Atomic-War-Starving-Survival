# Hardcore Faction Preference Contract

## 1. Faction Architecture & Resolution

The 8 faction preference profiles in `hardcore_economy_tuning.json` correspond to the authoritative factions established across Plan 23, Plan 98, and the Core Standing system:

| Faction ID | Display Identity | Primary Premium Goods | Refused Goods | Canonical Trade Medium |
|:---|:---|:---|:---|:---|
| `central_garrison_remnants` | Central Garrison Remnants | Munitions (`ammo_*`), military armor, fuel, MREs | Jewelry, books, luxury cigarettes | Fuel vouchers, ammunition crates, military command chits |
| `faction_black_flotilla` | The Black Flotilla | Marine sealant, dive lines, dive lamps, rebreathers, fittings | Keepsakes, old books, photographs, luxury toys | Dry cloth, medical stores, fuel, stamped marine salvage |
| `faction_the_scale` | The Scale | Water filters, clean water, valve bodies, fittings, sealant | Luxury jewelry, cigarettes, keepsake photos | Volumetric water chits, calibrated valves, pipe sealant |
| `faction_the_compact` | The Compact | Paper scrap, books, legal records, dosimeters, geiger counters | Ammunition, radioactive dust drums, tailings | Archival deeds, boundary warrants, arbitration chits |
| `faction_the_underwrite` | The Underwrite | Heavy fuel, ammunition, trauma medkits, calibration kits | Sludge cakes, toxic hot-dust drums, tailings | Fuel debentures, risk insurance policies, escort chits |
| `faction_the_cutters` | The Cutters | Heavy fuel, engine blocks, mechanical scrap, canned food | Fine jewelry, paper archives, legal books | Black coal, haulage sledges, cleared pass transit chits |
| `faction_the_rebuilders` | The Rebuilders | Viable seed packets, ash grain, clean water, antibiotics | Hot-dust drums, tailings drums, sludge cakes | Grain bushels, heirloom seed credits, communal silo chits |
| `faction_the_overlay` | The Overlay | Brass fittings, dosimeters, geiger counters, paper scrap | Luxury jewelry, cigarettes, sludge cakes | Cadastral keys, triangulation data, correction warrants |

---

## 2. Invariants & Guarantees

1. **Exact Count:** Exactly 8 unique canonical faction IDs are registered.
2. **Zero Collision:** For each faction, `BuysAtPremium` and `Refuses` are strictly disjoint sets. A faction never buys and refuses the same item.
3. **Identity Stability:** The legacy `central_garrison_remnants` baseline and the Plan 23 `faction_black_flotilla` baseline are byte-preserved and backward-compatible with all existing unit and scenario tests.
