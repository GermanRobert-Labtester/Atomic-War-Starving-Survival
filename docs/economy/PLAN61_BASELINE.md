# Plan 61 — Baseline Reconnaissance Report

**Document Version:** 1.0.0
**Date:** 2026-09-03
**Target:** Expansion of `trade_screen_scenarios.json` from 3 to 15 scenarios.

---

## 1. Catalog Baseline Inventory

The file `Assets/StreamingAssets/Data/trade_screen_scenarios.json` contains exactly 3 verified baseline scenarios before expansion:

| Scenario ID | Faction ID | Faction Name | Leader Name | Stance | Expected Fairness | Confirm Succeeds | Shocks / Scarcity | Offers / Demands |
|---|---|---|---|---|---|---|---|---|
| `fair_deal` | `scavenger_camp` | Scavenger Camp | Varek | `Trade` | `fair` | `true` | PlumePassing (2.5×) / Clean Water (2.0×) | Offer: 3× Canned Food (18), 1× Duct Tape (15), 1× Blood (25) = 94. Ask: 2× Water (22) = 44 |
| `offer_short` | `upland_militia` | Upland Militia | Sergeant Oduya | `Trade` | `short` | `false` | ConvoyAmbush (1.8×) / Fuel (1.8×) | Offer: 1× Canned Food (18) = 18. Ask: 2× Fuel (40) = 80 |
| `empty_table` | `rot_farmers` | Rot Farmers | Mother Ilde | `Refuse` | `empty` | `false` | None | Offer: None. Ask: None |

---

## 2. Runtime Schema & Serialization Analysis

From `Assets/Ashfall.Core/Economy/TradeScreenScenarios.cs`:

- **Root Structure:**
  ```json
  {
    "schema_version": 1,
    "$schema": "./schema/trade_screen_scenarios.schema.json",
    "version": 1,
    "description": "...",
    "scenarios": [ ... ]
  }
  ```
- **Scenario Record Fields:**
  - `id` (string): Stable scenario identifier.
  - `faction_id` (string): Maps to `faction_radio_corpus.json` and faction runtime.
  - `faction_name` (string): Display name for the faction in UI headers.
  - `leader_name` (string): Faction leader name.
  - `succession_generation` (int, default 1): Succession generation number.
  - `stance` (string): Parsed via `ParseStance` to `TradeStance` (`Trade`, `Refuse`, `ShareIntel`, `Rob`, `HostileRaid`).
  - `trust` (float): Meter on the table edge (-100 to 100). Drives `TradeTellEngine` trust band (`hostile`, `wary`, `neutral`, `warm`).
  - `aggression` (float): Raid aggression meter (0.0 to 1.0).
  - `consecutive_repels` (int): Faction presence meter.
  - `has_surrendered` (bool): Faction state flag.
  - `can_demand_parley` (bool): Gating flag for parley intent.
  - `world_phase` (string): Display label for world phase (e.g. `CivilWar`, `LongWinter`, `PostWar`).
  - `world_day` (int): Campaign day of the encounter context.
  - `price_shocks` (array of `{ kind, multiplier, note }`): Shock badges in news strip. Kinds: `PlumePassing`, `ConvoyAmbush`, `FactionWar`, `WinterDeepens`.
  - `scarcity` (array of `{ item_id, display_name, multiplier }`): Scarcity badges in news strip.
  - `player_offers` (array of `{ item_id, display_name, quantity, unit_price }`): Player barter line items.
  - `biological_offers` (dictionary of `{ PintOfBlood, BoneMarrow, Plasma, Organ : int }`): Grim drawer biological trade.
  - `faction_demands` (array of `{ item_id, display_name, quantity, unit_price }`): Faction ask line items.
  - `expected_fairness` (string): `"fair"`, `"short"`, or `"empty"`.
  - `confirm_succeeds` (bool): Mock intent sink outcome.
  - `radio_ticker` (string): Diegetic atmospheric line displayed in the radio news ticker.

---

## 3. Baseline Verification Results

- `godot --headless --path . -- --data-integrity-selftest`: PASS (0 errors, 208 catalogs).
- `dotnet test Ashfall.Core.Tests`: PASS (6,612 tests passing).
- `dotnet build Ashfall.csproj`: PASS (0 warnings, 0 errors).
- `python3 scripts/ci/scene-lint.py`: PASS (27 production scenes checked, 0 errors).
- `godot --headless --path . -- --content-utilization-selftest`: PASS (CI gate PASS).
- `godot --headless --path . -- --scene-binding-selftest`: PASS (22/22 passed).

---

## 4. Architectural Boundaries

1. **Base Price Authority:** `Assets/StreamingAssets/Data/economy_goods.json` and `GoodsCatalog.cs` own base item value.
2. **Faction Stance Authority:** `FactionStanceEngine.cs` and `faction_radio_corpus.json` own faction trust, aggression, and radio intercepts.
3. **Debt Authority:** `TradeCreditCoordinator.cs` and `Main.DebtCredit.cs` own debt contracts, ledgers, and credit limits.
4. **Settlement Authority:** `settlements.json` owns settlements and default trade specialties.
5. **Trade Screen Seam:** `TradeScreenSeam.cs`, `TradeScreenPresenter.cs`, and `TradeScreenViewModel.cs` manage presentation and transaction arbitration without mutating simulation state directly.
