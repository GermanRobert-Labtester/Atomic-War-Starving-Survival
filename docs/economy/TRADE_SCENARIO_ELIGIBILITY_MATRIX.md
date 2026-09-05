# Trade Scenario Eligibility Matrix

**Document Version:** 1.0.0
**Authority:** `Assets/StreamingAssets/Data/trade_screen_scenarios.json`

---

## 1. Scenario Eligibility & Gating Rules

| Scenario ID | Primary Producer | Faction Gate | Trust / Stance Gate | World Phase / Day Window | Fallback Scenario |
|---|---|---|---|---|---|
| `fair_deal` | Scavenger Camp Trade Post | `scavenger_camp` | Trust $\ge -40$ (`Trade`) | `CivilWar` / Day 14+ | `empty_table` |
| `offer_short` | Militia Roadblock / Encounter | `upland_militia` | Trust $< 0$ (`Trade`) | `CivilWar` / Day 20+ | `empty_table` |
| `empty_table` | Hostile / Closed Settlement | `rot_farmers` | Trust $< -20$ (`Refuse`) | `LongWinter` / Day 40+ | Closed table UI |
| `last_vials` | Clinic / Shelter Medical Visit | `safe_haven_community` | Trust $\ge 0$ (`Trade`) | Any / Day 1+ | `fair_deal` |
| `winter_cart` | Ice Road Traveler Encounter | `rot_farmers` | Trust $\ge -20$ (`Trade`) | `LongWinter` / Day 30+ | `empty_table` |
| `depot_window` | Military Stronghold Post | `military_remnants` | Trust $\ge 40$ (`Trade`) | Any / Day 15+ | `emergency_requisition` |
| `emergency_requisition` | Military Checkpoint Decree | `military_remnants` | Active Decree (`Trade`) | `CivilWar` / Day 30+ | `empty_table` |
| `back_room_exchange` | Clandestine Broker Venue | `wire_heads` | Trust $\ge 0$ (`Trade`) | `PostWar` / Day 10+ | `long_road_caravan` |
| `ledgerless_broker` | Canal Ruins Ambush / Shakedown | `sump_dredgers` | Hostile (`Rob`) | Any / Day 20+ | Hostile encounter |
| `long_road_caravan` | Settlement Trade Route Hub | `scavenger_camp` | Trust $\ge 0$ (`Trade`) | Any / Day 5+ | `fair_deal` |
| `salvage_caravan` | Industrial Quarry / Rail Siding | `custodians` | Trust $\ge 0$ (`Trade`) | Any / Day 15+ | `long_road_caravan` |
| `settlement_of_accounts` | Overdue Credit Enforcement | `hydro_barons` | Active Delinquency (`Refuse`)| Day 30+ | Debt enforcement HUD |
| `crate_lot` | Agricultural Community Silo | `safe_haven_community` | Trust $\ge 20$ (`Trade`) | Any / Day 10+ | `long_road_caravan` |
| `border_runner` | Wasteland Perimeter Transit | `echo_bats` | Neutral / Warm (`ShareIntel`)| Day 25+ | `back_room_exchange` |
| `road_knowledge` | Displaced Wanderer Camp | `doomsday_preppers` | Non-hostile (`Trade`) | Any / Day 1+ | `fair_deal` |

---

## 2. Selection Precedence

When multiple trade scenarios match the active world and encounter state, selection follows this strict deterministic hierarchy:

1. **Active Debt / Delinquency Override:** If the player has a delinquent debt contract with the visiting faction creditor, `settlement_of_accounts` takes absolute precedence.
2. **Faction War / Emergency Requisition:** If a faction war decree is active and military standing is insufficient for depot privileges, `emergency_requisition` is selected.
3. **Standing-Gated Faction Quartermaster:** If the trader is a stronghold logistics officer and player standing exceeds 40, `depot_window` takes precedence over generic caravans.
4. **Specialty / Route Context:** Route caravans (`salvage_caravan`, `long_road_caravan`), bulk dealers (`crate_lot`), or clandestine runners (`border_runner`, `back_room_exchange`) selected according to route node type.
5. **Atmospheric / Survival Encounters:** Desperate survivor trades (`last_vials`, `winter_cart`) and refugee barter (`road_knowledge`).
6. **Generic Fallback:** Baseline `fair_deal` (if trade is allowed) or `empty_table` (if trade is refused).

Ties are broken deterministically by ordinal sorting on scenario ID.
