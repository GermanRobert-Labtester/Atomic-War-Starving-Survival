# Trade Settlement, Patrol & Debt Integration

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/settlements.json`, `Assets/Ashfall.Core/Economy/TradeCreditCoordinator.cs`

---

## 1. Plan 43 Settlement Trade Integration

The 15 trade scenarios link naturally to settlement archetypes established in `settlements.json`:

| Settlement Archetype | Representative Settlement | Default Scenario | Archetype Role | Mechanics & Narrative Context |
|---|---|---|---|---|
| **Trade Post / Crossing** | `settlement_iron_siding` | `long_road_caravan` | `caravan_merchant` | Reliable general exchange hub along main transit corridors; food/water/tools. |
| **Stronghold / Depot** | `settlement_garrison_redoubt` | `depot_window` | `faction_quartermaster` | Military ordnance depot; requires high standing ($\ge 40$ trust) for munitions and kits. |
| **Refugee Camp / Wanderer Hub** | `settlement_ash_verge` | `road_knowledge` | `refugee_barter` | Impoverished barter; small scale trade of seeds, preserved meat, and route advice. |
| **Agricultural Community / Silo** | `settlement_grain_reach` | `crate_lot` | `bulk_dealer` | High-volume staple distributor; wholesale exchange of water/fuel for food and scrap. |

---

## 2. Plan 45 Patrol Encounter Integration

Mobile wasteland encounters interface with the trade screen when non-hostile contact occurs:

1. **Smuggler (`border_runner`):**
   - Triggered when encountering mobile border scouts or independent couriers (`echo_bats`).
   - Uses `ShareIntel` stance: offers both high-tier anti-radiation medicine and route advice.
   - Non-hostile; does not require illegal crime meters.

2. **Clandestine Black Market (`back_room_exchange`):**
   - Triggered at contested boundary checkpoints or black-box relay nodes (`wire_heads`).
   - Sells specialized filter gear and ammunition away from faction surveillance.

3. **Coercive Roadblock (`ledgerless_broker`):**
   - Hostile shakedown encounter (`sump_dredgers`) using `Rob` stance.
   - Demonstrates that hostile encounters cannot confirm legitimate trades.

---

## 3. Plan 40 Debt & Credit Integration

1. **Encounter Producer:**
   - Incurred debt with a faction creditor (such as `hydro_barons`) is tracked in `TradeCreditCoordinator`.
   - When repayment is delinquent, routine merchant visits from that faction are intercepted by `settlement_of_accounts`.

2. **Boundary Invariant:**
   - `TradeScreenScenario` does **not** own the debt ledger, interest rate, or principal balance.
   - The scenario provides the dramatic framing (`Refuse` stance, heavy water demand) and closes routine trade until the debt is satisfied through the authoritative debt ledger.
