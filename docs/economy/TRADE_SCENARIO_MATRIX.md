# Trade Scenario Matrix — Plan 61 final catalog (15 scenarios)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

Archetype is the economic role expressed through **repository-native dimensions**
(faction identity, stance, trust band, table composition, shock/scarcity context).
No `trader_type` field exists in the runtime schema (see `TRADE_SCENARIO_SCHEMA_MAP.md`).

| # | Scenario ID | Archetype | Faction | Stance | Trust band | Shocks | Scarcity | Table shape (offer/ask lines) | Fairness | Confirm | Player decision created |
|---|---|---|---|---|---|---|---|---|---|---|---|
| 1 | `fair_deal` ★ | baseline | scavenger_camp | Trade | neutral (22) | Plume ×2.5 | clean_water ×2.0 | 2 (+1 bio) / 1 | fair | ✔ | straightforward barter anchor |
| 2 | `offer_short` ★ | baseline | upland_militia | Trade | wary (−5) | Convoy ×1.8 | fuel ×1.8 | 1 / 1 | short | ✘ | sweeten the offer or walk |
| 3 | `empty_table` ★ | baseline | rot_farmers | Refuse | wary (−25) | — | — | 0 / 0 | empty | ✘ | read a closed posture, don't trade |
| 4 | `last_vials` | desperate_survivor | sump_dredgers | Trade | neutral (12) | — | iodine_pills ×1.6 | 2 / 2 | fair | ✔ | pay aid goods for a dying household's last medicine |
| 5 | `winter_cart` | desperate_survivor | cult_of_the_glow | Trade | wary (−15) | WinterDeepens ×1.6 | fuel ×2.2 | 2 / 3 | short | ✘ | urgency trade — their warm goods outweigh what you brought |
| 6 | `depot_window` | faction_quartermaster | military_remnants | Trade | warm (55) | FactionWar ×1.4 | filter_pack ×1.5 | 3 / 2 | fair | ✔ | spend institutional trust on curated military stock |
| 7 | `emergency_requisition` | faction_quartermaster | upland_militia | Trade | neutral (18) | FactionWar ×1.9 | ammo_762 ×1.7 | 2 / 2 | short | ✘ | wartime terms — the militia cannot trade fairly mid-crisis |
| 8 | `back_room_exchange` | black_market | faction_black_flotilla | Trade | neutral (8) | Convoy ×2.2 | rad_away ×1.9, anti_rad ×1.7 | 2 / 3 | fair | ✔ | pay a premium for rad-medicine you can't buy openly |
| 9 | `ledgerless_broker` | black_market | wire_heads | ShareIntel | neutral (30) | — | — | 3 (+1 bio BoneMarrow) / 2 | fair | ✔ | trade favors, cipher goods — or the drawer — for paperwork |
| 10 | `long_road_caravan` | caravan_merchant | doomsday_preppers | Trade | neutral (35) | — | clean_water ×1.4 | 2 / 2 | fair | ✔ | dependable broad staples at standard terms |
| 11 | `salvage_caravan` | caravan_merchant | faction_silent_foundry | Trade | neutral (22) | Convoy ×1.5 | steel_rail_segment ×1.6 | 3 / 2 | fair | ✔ | low-value scrap into industrial stock — different strategic class |
| 12 | `settlement_of_accounts` | debt_collector | custodians | Trade | wary (−20) | — | — | 0 / 3 (demands only) | short | ✘ | face the ledger: bare edge, heavy obligation |
| 13 | `crate_lot` | bulk_dealer | hydro_barons | Trade | neutral (28) | — | — | 2 (6 units) / 2 (9 units) | fair | ✔ | volume trade — highest unit count in the catalog |
| 14 | `border_runner` | smuggler | echo_bats | Trade | neutral (15) | Convoy ×2.4 | item_military_radio_module ×1.8 | 2 / 2 | fair | ✔ | premium for route-run comms stock that posts don't carry |
| 15 | `road_knowledge` | refugee_barter | safe_haven_community | ShareIntel | neutral (40) | WinterDeepens ×1.3 | — | 2 / 3 | fair | ✔ | small necessities for maps and route knowledge (ShareIntel) |

★ = original baseline scenario (preserved byte-identically in content).

## Differentiation proof

Mechanical gate: `Catalog_EveryScenarioDiffersFromNearestNeighborInTwoDimensions`
requires ≥2 differing dimensions across every pair among: stance, trust band, table
composition, shock kinds, scarcity items, fairness outcome. Notable pairs:

- `offer_short` vs `emergency_requisition` (same faction): different shock kind, trust band, day, goods, aggression, scarcity, table size.
- `back_room_exchange` vs `ledgerless_broker`: ShareIntel vs Trade, cipher/favor goods vs rad-medicine, bio drawer vs none, shocks vs none.
- `long_road_caravan` vs `salvage_caravan`: staples vs industrial goods, different scarcity/shock, different price band.
- `back_room_exchange` vs `border_runner`: fixed clandestine venue (flotilla) vs mobile route runner (echo bats), medicine vs comms stock — distinguished by producer fiction and goods class, not a risk meter.
- `last_vials` vs `winter_cart`: fair/confirm vs short/blocked, different shocks/scarcity/goods/bands.
- `empty_table` vs `settlement_of_accounts`: both bare-vs-heavy asymmetry, but Refuse/both-bare vs Trade/demands-only — a closed posture vs a called-in obligation.

## Anti-exploit notes

- No scenario selection RNG exists; no reroll surface exists (static content).
- Global unit-price consistency per `item_id` (test-gated) makes cross-table
  buy-low/sell-high structurally impossible.
- `settlement_of_accounts` is presentation of an outstanding demand only; no debt
  math exists in this seam and none was added.
- `crate_lot` expresses bulk via unit count, not a discount engine; per-unit worths
  are the canonical values.
