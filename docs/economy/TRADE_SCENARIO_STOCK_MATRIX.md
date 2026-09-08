# Trade Scenario Stock Matrix — Plan 61 (repository truth)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## Stock model: fixed authored tables

There is **no stock generator, no merchant inventory, and no `available_goods`**
mechanism in the trade-screen seam. A scenario's "stock" is its authored barter
table (`player_offers` / `faction_demands` / `biological_offers`) — fixed, static,
never generated, never depleted, never persisted. Plan §61A.4's stock-lifecycle
question is answered: **fixed**, with no seed/state persistence needed.

## Goods register (all `item_id`s verified against `items.json` by test)

| Item | Worth | Scenarios | Category role | Notes |
|---|---|---|---|---|
| `canned_food` | 18 | fair_deal, settlement_of_accounts, road_knowledge | staple | baseline item |
| `duct_tape` | 15 | fair_deal | utility | baseline item |
| `clean_water` | 22 | fair_deal, last_vials | staple | baseline item |
| `fuel` | 40 | offer_short, emergency_requisition, settlement_of_accounts, border_runner | fuel | scarcity-flagged in 3 scenarios |
| `item_salted_meat` | 16 | last_vials, depot_window | food |
| `antibiotics` | 45 | last_vials | medicine |
| `splint` | 10 | last_vials | medical |
| `item_grain_flour` | 14 | winter_cart, long_road_caravan, crate_lot | staple |
| `battery` | 14 | winter_cart | component |
| `item_heavy_wool_coat` | 34 | winter_cart | cold-weather |
| `item_fur_mittens` | 14 | winter_cart | cold-weather |
| `charcoal` | 8 | winter_cart | fuel |
| `item_trade_salt_sack` | 10 | depot_window, long_road_caravan | staple |
| `battery_pack` | 20 | depot_window, settlement_of_accounts | component |
| `gas_mask` | 55 | depot_window | protection |
| `filter_pack` | 28 | depot_window, back_room_exchange | protection |
| `scrap_chemical` | 7 | emergency_requisition | industrial |
| `military_radio` | 70 | emergency_requisition | comms |
| `item_radio_cipher_rotor` | 45 | emergency_requisition | comms |
| `clean_water_jug` | 30 | back_room_exchange, crate_lot | staple |
| `item_honey_pot` | 18 | back_room_exchange, crate_lot | food |
| `rad_away` / `anti_rad` | 38 / 35 | back_room_exchange | scarce medicine |
| `item_logistics_cipher_sheet` | 30 | ledgerless_broker | paperwork |
| `electronic_scrap` | 10 | ledgerless_broker | component |
| `cigarette_lighter` | 8 | ledgerless_broker | personal |
| `item_comm_codebook_alpha` | 40 | ledgerless_broker | paperwork |
| `radio_headset` | 25 | ledgerless_broker | comms |
| `item_canned_grain_stew` | 15 | long_road_caravan, crate_lot | staple |
| `scrap_metal` | 4 | salvage_caravan | industrial |
| `box_of_nails_10` | 5 | salvage_caravan | industrial |
| `copper_wire_10m_of_10m` | 12 | salvage_caravan | industrial |
| `steel_rail_segment` | 15 | salvage_caravan | industrial (scarcity-flagged) |
| `item_metallurgy_steel_billet` | 18 | salvage_caravan | industrial |
| `item_military_radio_module` | 60 | border_runner | rare comms (scarcity-flagged) |
| `item_radio_vacuum_tube` | 18 | border_runner | comms |
| `item_battery_reconditioned` | 25 | border_runner | component |
| `wool_blanket` | 24 | road_knowledge | cold-weather |
| `item_collectible_road_map` | 20 | road_knowledge | intel-as-item (real item, no `loc_*` hack) |
| `item_document_evacuation_route_map` | 25 | road_knowledge | intel-as-item |
| `bandage` | 12 | road_knowledge | medical |

Biological offers: `PintOfBlood ×1` (fair_deal), `BoneMarrow ×1` (ledgerless_broker)
— priced only by `TradePricing.BioUnitValue`.

## Stock rules compliance (plan §61C.5)

- No quest-critical/unique items used; all referenced items are ordinary goods in `items.json`.
- No `loc_*` IDs or arbitrary reward strings — the refugee clue is expressed as **real map items + ShareIntel stance**, per plan §1.8 option 1.
- No bypass of progression gates: rare comms modules (`border_runner`) appear as single units at high worth; no advanced crafting inputs (billets beyond ordinary trade grade) are discounted.
- Bulk (`crate_lot`) moves common staples only — no bulk medicine/ammo.
- No empty stock sets among tradable scenarios (only the deliberate `empty_table` is bare, and the demands-only debt table has a heavy ask edge).

## Scarcity interaction

Scenario tables are fixed; `scarcity[]` entries are news-strip presentation in this
seam and do not replenish, deplete, or reprice anything. Rare-goods access is
therefore bounded by authored worth and single-unit quantities — documented as
intentional seam behavior (no economy simulation owns scenario stock).
