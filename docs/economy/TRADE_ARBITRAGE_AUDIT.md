# Trade Arbitrage Audit

**Document Version:** 1.0.0
**Domain:** Cross-Scenario Barter & Market Stability

---

## 1. Cross-Scenario Price Comparison for Key Commodities

| Item ID | Base Price (`economy_goods`) | `crate_lot` (Bulk) | `long_road_caravan` (Caravan) | `depot_window` (Quartermaster) | `back_room_exchange` (Black Market) | `last_vials` (Desperate) |
|---|---|---|---|---|---|---|
| `clean_water` | 8.0 | 10.0 | 14.0 | 12.0 | N/A | N/A |
| `canned_food` | 10.0 | 12.0 | 16.0 | N/A | 18.0 | N/A |
| `fuel` | 20.0 | 20.0 | N/A | N/A | N/A | N/A |
| `scrap_metal` | 3.0 | 3.0 | N/A | 4.0 | N/A | N/A |
| `ammo_556` | 18.0 | N/A | N/A | 30.0 | N/A | N/A |
| `antibiotics` | 40.0 | N/A | N/A | N/A | N/A | 80.0 (Scarcity) |

---

## 2. Connected-Market Pair Analysis

### Pair A: Bulk Dealer (`crate_lot`) ↔ Caravan Merchant (`long_road_caravan`)
- **Action:** Player trades in `crate_lot` to acquire `canned_food` at unit valuation 12.0 by offering `clean_water` valued at 10.0.
- **Potential Arbitrage:** Player carries `canned_food` to `long_road_caravan`, where `canned_food` is accepted as a player offer at 16.0 unit valuation.
- **Net Margin:** $16.0 - 12.0 = +4.0$ valuation gain per unit.
- **Exploit Mitigation:**
  1. `crate_lot` demands 6× Clean Water + 3× Fuel (heavy survival essentials) to clear 8× Canned Food.
  2. Overland transit between the agricultural silo (`settlement_grain_reach`) and the trade post (`settlement_iron_siding`) consumes rations and water in expedition simulation.
  3. Stock is finite and does not immediately refresh upon screen exit.
  4. The spread represents intentional merchant freight margin, not risk-free infinite currency duplication.

### Pair B: Allied Quartermaster (`depot_window`) ↔ Black Market (`back_room_exchange`)
- **Action:** Player uses allied standing with `military_remnants` to acquire subsidized `medical_kit` (28.0) or `ammo_556` (30.0) in exchange for `clean_water` (12.0) and `scrap_metal` (4.0).
- **Potential Arbitrage:** Reselling munitions to black-market dealers.
- **Exploit Mitigation:**
  1. High trust threshold ($\ge 40$) is required to open the depot window.
  2. Quartermaster stock is strictly quantity-capped per campaign cycle.
  3. Black market brokers demand high premiums on their own sales ($1.6\times - 2.2\times$) while taking steep discounts on incoming player offers.

### Pair C: Desperate Survivor (`last_vials`) ↔ Any Merchant
- **Action:** Player brings `antibiotics` to a desperate survivor in exchange for tools.
- **Analysis:** The desperate survivor is demanding medicine and offering high-value tools (`crowbar`, `solar_cell`).
- **Exploit Mitigation:**
  1. Antibiotics are strictly scarce (world shock multiplier $2.5\times$).
  2. The survivor's inventory is tiny (one-off relief exchange).
  3. No infinite stock loop exists.

---

## 3. Conclusion
No scenario pairing introduces an infinite, risk-free, same-location buy-low/sell-high loop. Price differentials reflect legitimate regional scarcity, freight risk, and standing privileges.
