# Trade Scenario Stock Matrix

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/economy_goods.json`
**Consumer:** `Assets/StreamingAssets/Data/trade_screen_scenarios.json`

This matrix cross-references every item referenced in the 15 trade scenarios against the authoritative goods registry in `economy_goods.json`.

| Item ID | Display Name | Category | Base Price | Stack | Referenced In Scenarios | Role | Scenario Price (Unit) |
|---|---|---|---|---|---|---|---|
| `clean_water` | Clean Water | water | 8.0 | 5 | `fair_deal`, `depot_window`, `long_road_caravan`, `crate_lot`, `settlement_of_accounts`, `road_knowledge` | Offer, Demand, Scarcity | 10.0 – 25.0 |
| `scrap_metal` | Scrap Metal | materials | 3.0 | 20 | `winter_cart`, `depot_window`, `ledgerless_broker`, `crate_lot`, `settlement_of_accounts` | Offer, Demand | 3.0 – 5.0 |
| `bandages` | Bandages | medical | 15.0 | 10 | `emergency_requisition` | Offer | 15.0 |
| `antibiotics` | Antibiotics | medical | 40.0 | 5 | `last_vials` | Demand, Scarcity | 80.0 (scarcity 2.5×) |
| `iodine_pills` | Iodine Pills | medical | 25.0 | 10 | `border_runner` | Demand | 20.0 |
| `fuel` | Fuel | fuel | 20.0 | 5 | `offer_short`, `winter_cart`, `crate_lot`, `border_runner` | Offer, Demand, Scarcity | 20.0 – 40.0 |
| `diesel_fuel` | Diesel Fuel | fuel | 25.0 | 5 | `emergency_requisition` | Demand, Scarcity | 50.0 (scarcity 2.5×) |
| `9mm_ammo` | 9mm Ammo | ammo | 12.0 | 30 | `back_room_exchange` | Demand | 25.0 |
| `ammo_556` | 5.56mm Ammo | ammo | 18.0 | 30 | `depot_window` | Demand, Scarcity | 30.0 |
| `crowbar` | Crowbar | tools | 25.0 | 1 | `last_vials` | Offer | 35.0 |
| `gas_mask` | Gas Mask | gear | 45.0 | 1 | `back_room_exchange` | Demand, Scarcity | 75.0 |
| `canned_food` | Canned Food | food | 10.0 | 10 | `fair_deal`, `offer_short`, `back_room_exchange`, `long_road_caravan`, `crate_lot`, `road_knowledge` | Offer, Demand, Scarcity | 12.0 – 20.0 |
| `item_smoked_meat`| Smoked Meat | food | 14.0 | 5 | `road_knowledge` | Offer | 20.0 |
| `water_filter` | Water Filter | materials | 20.0 | 5 | `salvage_caravan` | Demand | 30.0 |
| `seed_packets` | Seed Packets | materials | 12.0 | 10 | `road_knowledge` | Offer | 15.0 |
| `chemicals` | Industrial Chemicals | materials | 15.0 | 10 | `salvage_caravan` | Offer | 20.0 |
| `electronic_scrap`| Electronic Scrap | materials | 18.0 | 10 | `back_room_exchange`, `salvage_caravan` | Offer | 25.0 |
| `mechanical_parts`| Mechanical Parts | materials | 22.0 | 10 | `salvage_caravan` | Demand, Scarcity | 30.0 |
| `solar_cell` | Solar Cell Component | materials | 35.0 | 2 | `last_vials` | Offer | 45.0 |
| `medical_kit` | Medical Kit | medical | 45.0 | 3 | `depot_window` | Demand | 28.0 (allied subsidy) |
| `anti_rad` | Anti-Rad Injector | medical | 35.0 | 5 | `border_runner` | Demand, Scarcity | 50.0 |
| `weapon_sidearm`| Service Sidearm | weapons | 85.0 | 1 | `ledgerless_broker` | Demand, Scarcity | 120.0 |
| `duct_tape` | Duct Tape | materials | 8.0 | 10 | `fair_deal`, `long_road_caravan` | Offer, Demand | 15.0 – 16.0 |
| `rope` | Braided Rope | materials | 10.0 | 5 | `long_road_caravan` | Demand | 15.0 |
| `tobacco_pouch` | Tobacco Pouch | luxury | 15.0 | 5 | `long_road_caravan`, `border_runner` | Offer | 20.0 |

---

## 2. Validation & Progression Guarantees

1. **No Invented IDs:** 100% of referenced items resolve directly to `Assets/StreamingAssets/Data/economy_goods.json`.
2. **Progression Safety:** Advanced late-game blueprints, unique relics, and faction-exclusive documents are excluded from open scenario barter.
3. **Scarcity Alignment:** High-tier medical (`antibiotics`), fuel (`diesel_fuel`), and firearms (`weapon_sidearm`) carry steep scenario multipliers (1.5× to 2.5×) consistent with active price shocks.
4. **Volume Restraint:** Bulk dealer demands high-liquidity staples (`clean_water`, `fuel`) in exchange for bulk lots, avoiding infinite stockpiling loops.
