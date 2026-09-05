# Plan 61 Completion Report — Trade Screen Scenarios Expansion (3 → 15)

**Implementation Date:** 2026-09-03
**Status:** COMPLETE & VERIFIED
**Author:** Antigravity (ASHFALL Core Engineering)

---

## 14.1 Summary

- **Baseline Scenario Count:** 3 (`fair_deal`, `offer_short`, `empty_table`).
- **Final Scenario Count:** 15 (3 baseline preserved + 12 new expansion scenarios).
- **Primary Files Changed:**
  - `Assets/StreamingAssets/Data/trade_screen_scenarios.json` — Expanded from 3 to 15 scenarios.
  - `Ashfall.Core.Tests/TradeScreenSeamTests.cs` — Added `Scenarios_LoadAllFifteenFromData`, `Scenarios_AllScenariosMatchExpectedFairness`, `Scenarios_AllItemReferencesResolveInEconomyGoods`, `Scenarios_AllFactionReferencesResolveInRadioCorpus`, `Scenarios_NewScenarios_CharacterizationProbes`.
  - `docs/data/CATALOG_REGISTRY.md` — Updated definition count from 3 to 15.
  - Fixes to pre-existing compile blocker in test suite: `Plan10CatalogCoverageTests.cs` (disambiguated Dictionary type & fixed plan 60 zero-fuel count assertion) and `src/Main.CampaignOwners.cs` (qualified `_m._dataDir`).
  - Documentation Suite:
    - `docs/economy/PLAN61_BASELINE.md`
    - `docs/economy/TRADE_SCENARIO_SCHEMA_MAP.md`
    - `docs/economy/TRADE_PRICE_AUTHORITY_MAP.md`
    - `docs/economy/TRADE_SCENARIO_MATRIX.md`
    - `docs/economy/TRADE_SCENARIO_ELIGIBILITY_MATRIX.md`
    - `docs/economy/TRADE_SCENARIO_STOCK_MATRIX.md`
    - `docs/economy/TRADE_NEGOTIATION_INTEGRATION.md`
    - `docs/economy/TRADE_SETTLEMENT_PATROL_INTEGRATION.md`
    - `docs/economy/TRADE_ARBITRAGE_AUDIT.md`
    - `docs/economy/PLAN61_SAVE_COMPATIBILITY.md`
    - `docs/economy/PLAN61_REGRESSION_MATRIX.md`
    - `docs/economy/PLAN61_COMPLETION_REPORT.md`
- **Architectural Purity:** Pure data-layer catalog expansion. Core architecture remained untouched; no new engine dependencies, no parallel economy authorities, and no duplicate state stores were introduced.

---

## 14.2 Existing Scenario Preservation

All three baseline scenarios remain byte-level and semantically preserved:

| ID | Stance | Player Offer Value | Faction Ask Value | Expected Fairness | Confirm Succeeds | Test Contract |
|---|---|---|---|---|---|---|
| `fair_deal` | `Trade` | 94.0 | 44.0 | `fair` | `true` | `Scenario_FairDeal_ComputedFairnessMatchesDataExpectation` |
| `offer_short` | `Trade` | 18.0 | 80.0 | `short` | `false` | `Scenario_OfferShort_BlocksConfirmAndKeepsStanceLegible` |
| `empty_table` | `Refuse` | 0.0 | 0.0 | `empty` | `false` | `Scenario_EmptyTable_IsDeliberateNotBroken` |

---

## 14.3 New Scenario Roster (12 Expansion Entries)

1. **`last_vials` (`desperate_survivor`):**
   - *Producer:* Safe Haven Community Infirmary (`safe_haven_community`, Dr. Aris Thorne).
   - *Stock:* Sacrificing exploration tools (`crowbar`, `solar_cell`) for urgently needed `antibiotics`.
   - *Valuation:* Offer 80.0 vs Ask 80.0 (Fair).
   - *Decision:* Part with durable high-tier exploration tools to cure critical disease.

2. **`winter_cart` (`desperate_survivor`):**
   - *Producer:* Wanderer push-cart encounter (`rot_farmers`, Harlan Frost).
   - *Stock:* Scrap metal vs fuel in freezing conditions.
   - *Valuation:* Offer 20.0 vs Ask 70.0 (Short).
   - *Decision:* Demonstrates that low-value scrap cannot secure heating fuel during a permafrost front.

3. **`depot_window` (`faction_quartermaster`):**
   - *Producer:* Military Stronghold Depot (`military_remnants`, Captain Kroll).
   - *Stock:* Munitions (`ammo_556`) and subsidized `medical_kit` in exchange for water and scrap.
   - *Valuation:* Offer 88.0 vs Ask 88.0 (Fair).
   - *Decision:* Allied standing ($\ge 40$ trust) grants access to military-grade firepower.

4. **`emergency_requisition` (`faction_quartermaster`):**
   - *Producer:* Military Checkpoint (`military_remnants`, Logistics Officer Brand).
   - *Stock:* Military demanding `diesel_fuel`, offering basic bandages.
   - *Valuation:* Offer 30.0 vs Ask 100.0 (Short).
   - *Decision:* Depicts emergency requisition pressures where player cannot afford garrison demands.

5. **`back_room_exchange` (`black_market`):**
   - *Producer:* Contested Relay Station (`wire_heads`, Nix the Solderer).
   - *Stock:* Clandestine exchange of `electronic_scrap` and biological offering (`PintOfBlood`) for `gas_mask` and `9mm_ammo`.
   - *Valuation:* Offer 136.0 vs Ask 125.0 (Fair).
   - *Decision:* Willingness to trade blood in the grim drawer to obtain sealed filter protection.

6. **`ledgerless_broker` (`black_market`):**
   - *Producer:* Sump Canal Shakedown (`sump_dredgers`, Corvo the Blind).
   - *Stock:* Coercive demand for `weapon_sidearm` with `Rob` stance.
   - *Valuation:* Offer 20.0 vs Ask 120.0 (Short).
   - *Decision:* Hostile encounters cannot confirm legitimate trades.

7. **`long_road_caravan` (`caravan_merchant`):**
   - *Producer:* Trade Post Hub (`scavenger_camp`, Mistress Janna).
   - *Stock:* Overland general staples: food rations and tobacco for water, rope, and duct tape.
   - *Valuation:* Offer 104.0 vs Ask 104.0 (Fair).
   - *Decision:* Dependable, standard-spread trade post exchange for travel supplies.

8. **`salvage_caravan` (`caravan_merchant`):**
   - *Producer:* Industrial Quarry Siding (`custodians`, Surveyor Vane).
   - *Stock:* Industrial chemicals and electronic salvage for `mechanical_parts` and `water_filter`.
   - *Valuation:* Offer 90.0 vs Ask 90.0 (Fair).
   - *Decision:* Industrial component trade for shelter technical upgrades.

9. **`settlement_of_accounts` (`debt_collector`):**
   - *Producer:* Hydro Barons Credit Enforcement (`hydro_barons`, Enforcer Malik).
   - *Stock:* Aggressive water collection with `Refuse` stance.
   - *Valuation:* Offer 10.0 vs Ask 125.0 (Short).
   - *Decision:* Highlights unresolved credit delinquency closing routine commerce.

10. **`crate_lot` (`bulk_dealer`):**
    - *Producer:* Agricultural Silo Clearance (`safe_haven_community`, Orin).
    - *Stock:* Wholesale volume exchange: 6× water + 3× fuel for 8× canned food + 8× scrap metal.
    - *Valuation:* Offer 120.0 vs Ask 120.0 (Fair).
    - *Decision:* High-volume liquidation of surplus resources for survival stockpiles.

11. **`border_runner` (`smuggler`):**
    - *Producer:* Wasteland Perimeter Transit (`echo_bats`, Silt-Runner Kira).
    - *Stock:* Fuel and luxury tobacco for `anti_rad` and `iodine_pills` with `ShareIntel` stance.
    - *Valuation:* Offer 90.0 vs Ask 90.0 (Fair).
    - *Decision:* Mobile runner offering valuable anti-rad pharmaceuticals and route intel.

12. **`road_knowledge` (`refugee_barter`):**
    - *Producer:* Displaced Wanderer Camp (`doomsday_preppers`, Old Sela).
    - *Stock:* Agricultural `seed_packets` and `item_smoked_meat` for water and food.
    - *Valuation:* Offer 50.0 vs Ask 50.0 (Fair).
    - *Decision:* Humane barter supporting displaced families in exchange for seeds.

---

## 14.4 Cross-Plan Integration Status

| Cross-Plan Feature | Status | Evidence in Repository |
|---|---|---|
| **Plan 40 (Debt & Credit)** | `LIVE` | `TradeCreditCoordinator.cs` models delinquent debt; `settlement_of_accounts` provides context without duplicating debt state. |
| **Plan 43 (Settlement Defaults)** | `LIVE` | `settlements.json` archetypes map to 4 default scenario roles (`long_road_caravan`, `depot_window`, `road_knowledge`, `crate_lot`). |
| **Plan 45 (Patrol Encounters)** | `LIVE` | `border_runner` (`ShareIntel`), `back_room_exchange`, and `ledgerless_broker` (`Rob`) provide distinct non-settlement encounter contexts. |
| **Plan 56 (Economy Goods)** | `LIVE` | All 25 referenced item IDs resolve directly to `economy_goods.json`. |
| **Plan 62 (Trade Tell Lines)** | `LIVE` | `trade_tell_lines.json` and `TradeTellEngine` select tells deterministically based on scenario stance and trust. |

---

## 14.5 Economy & Balance Findings

- **Spread & Multipliers:** Scenarios employ thematic price shocks ($1.3\times - 2.5\times$) consistent with world conditions (`PlumePassing`, `WinterDeepens`, `FactionWar`, `ConvoyAmbush`).
- **Arbitrage Proof:** No risk-free same-day buy-low/sell-high cycle is introduced; price differentials reflect legitimate transit freight costs and regional scarcity.
- **Progression Safety:** Advanced unique quest items and high-end tech schematics remain excluded from scenario stock.

---

## 14.6 Persistence & Determinism

- **Zero-Mutation Invariant:** Presentation reads providers and builds view models without side-effects on simulation state.
- **Snapshot Round-Trip:** Selection states round-trip deterministically through `TradeSelectionSnapshot`.
- **Deterministic Tells:** `ISeededRng` ensures reproducible tell selection given seed and world day.

---

## 14.7 Verification Matrix Results

```bash
# 1. Data Integrity Self-Test
godot --headless --path . -- --data-integrity-selftest
# Output: DATA_INTEGRITY_SELFTEST PASS — 0 findings (10307 ids authored, 3475 reuses reserved) — 0 errors across 208 catalogs

# 2. xUnit Core Suite
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
# Output: Passed! - Failed: 0, Passed: 6616, Skipped: 0, Total: 6616, Duration: 34 s

# 3. Host Application Build
dotnet build Ashfall.csproj
# Output: Build succeeded. 0 Warning(s), 0 Error(s).

# 4. Content Utilization Self-Test
godot --headless --path . -- --content-utilization-selftest
# Output: CI gate: PASS (490 catalogs scanned, 0 orphaned, 0 broken)

# 5. Scene Binding Self-Test
godot --headless --path . -- --scene-binding-selftest
# Output: Summary: 22 passed, 0 failed (of 22)

# 6. Production Scene Lint
python3 scripts/ci/scene-lint.py
# Output: scene-lint: 27 production scenes checked; 0 errors; 0 warning(s)
```

---

## 14.8 Remaining Risks & Deferred Work

- **Dynamic Negotiation Minigame (Plan 62 Phase 2):** In-screen player bargaining actions (pushing for concessions) remain deferred to Plan 62's interactive phase; static posture tells are fully operational.
- **Settlement Route AI Integration (Plan 43 Phase 3):** Dynamic migration of caravan entities along wasteland routes will continue to be expanded in world-simulation updates.
