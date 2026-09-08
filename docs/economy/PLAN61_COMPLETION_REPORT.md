# Plan 61 Completion Report — Trade Screen Scenarios Expansion (3 → 15)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## 14.1 Summary

- **Implementation date:** 2026-02
- **Baseline scenario count:** 3 (`fair_deal`, `offer_short`, `empty_table`)
- **Final scenario count:** **15**
- **Work remained pure data:** **YES** — zero Core/runtime source changes. The runtime
  schema already expressed every required dimension; the plan's provisional fields
  (`trader_type`, `available_goods`, `price_modifier`, `special_condition`) do not
  exist in the loader and were **not invented** (plan §6.1 data-first discipline).
- **Justified deviations from the source plan:**
  1. "Price posture via `price_modifier`" → expressed as **authored per-line
     `unit_price` with a global per-item consistency rule** (the only price field the
     runtime reads; direction ambiguity is structurally impossible — see
     `TRADE_PRICE_AUTHORITY_MAP.md`).
  2. "Trader archetype" → expressed through **faction identity + stance + trust band +
     table composition** (archetype is a documentation/test mapping, not a field).
  3. Refugee location clue → expressed as **real map items + `ShareIntel` stance**
     (plan §1.8 option 1); no `loc_*` hack, no new reward primitive.
  4. Bulk dealer → expressed as **unit-count profile at canonical worths**, not a
     volume-pricing engine (plan §61B.10 fallback explicitly taken).
  5. Settlement/patrol/debt producers → **deferred**, not faked (§14.4).
  6. Contextual prose rides the **consumed `radio_ticker`** field; no dead
     `description` field was added (plan §5.2).

## 14.2 Existing scenario preservation

All three originals preserved **byte-identically in content**; locked by
`Catalog_OriginalThreeScenariosKeepLockedContracts` (IDs, factions, stances, trust,
fairness, confirm semantics) plus all 30 pre-existing seam tests, updated only in one
count assertion (3 → ≥15). Before/after semantics: **unchanged**.

## 14.3 New scenario roster

Full matrix in `TRADE_SCENARIO_MATRIX.md`. Roster:

| ID | Archetype | Producer (current truth) | Stock role | Price behavior | Negotiation role | Eligibility | Faction/world links | Player decision |
|---|---|---|---|---|---|---|---|---|
| `last_vials` | desperate_survivor | requested by ID (skin track/tests; future producer hook) | personal medicine+aid barter | canonical worths, fair/confirm | Trade/neutral tell | none (static) | sump_dredgers, CivilWar d18 | trade aid goods for a household's last medicine |
| `winter_cart` | desperate_survivor | by ID | cold-weather stock-up | short/blocked; WinterDeepens ×1.6, fuel ×2.2 | Trade/wary | none | cult_of_the_glow, LongWinter d63 | urgency: their ask outweighs your cart |
| `depot_window` | faction_quartermaster | by ID | curated military stock | fair/confirm; FactionWar ×1.4 | Trade/warm (only warm scenario) | none | military_remnants, d33 | spend trust on protected stock |
| `emergency_requisition` | faction_quartermaster | by ID | crisis war materiel | short/blocked; FactionWar ×1.9, ammo ×1.7 | Trade/neutral | none | upland_militia, d48 | wartime terms cannot be met fairly |
| `back_room_exchange` | black_market | by ID | scarce rad-medicine premium | fair/confirm; Convoy ×2.2, scarcity ×1.9/1.7 | Trade/neutral | none | faction_black_flotilla, d39 | pay the dark-market premium |
| `ledgerless_broker` | black_market | by ID | favors/cipher/paperwork | fair/confirm; bio drawer BoneMarrow | **ShareIntel**/neutral | none | wire_heads, d26 | trade favors — or the drawer — for paperwork |
| `long_road_caravan` | caravan_merchant | by ID | broad staples | fair/confirm; no shocks (dependable) | Trade/neutral | none | doomsday_preppers, d21 | reliable breadth at standard terms |
| `salvage_caravan` | caravan_merchant | by ID | industrial inputs | fair/confirm; Convoy ×1.5, rail ×1.6 | Trade/neutral | none | faction_silent_foundry, d57 | scrap into industrial stock |
| `settlement_of_accounts` | debt_collector | by ID | **demands-only ledger table** | short/blocked by design | Trade/wary, aggression 0.5 | none | custodians, d44 | face the obligation (presentation only — no debt math) |
| `crate_lot` | bulk_dealer | by ID | highest unit count (9 ask units) | fair/confirm at canonical worths | Trade/neutral | none | hydro_barons, d71 | volume over margin |
| `border_runner` | smuggler | by ID | rare comms single-units | fair/confirm; Convoy ×2.4, module ×1.8 | Trade/neutral | none | echo_bats, d52 | premium for route-run stock |
| `road_knowledge` | refugee_barter | by ID | necessities + real map items | fair/confirm; WinterDeepens ×1.3 | **ShareIntel**/neutral | none | safe_haven_community, d35 | small charity goods for route knowledge |

Coverage: all eight requested archetypes in the exact 2/2/2/2/1/1/1/1 distribution
(test-gated). Differentiation: every pair ≥2 dimensions (test-gated).

## 14.4 Cross-plan integration status

| Plan | Status | Evidence |
|---|---|---|
| Plan 40 debt | **DEFERRED** | no debt producer/consumer in the trade-screen seam; `settlement_of_accounts` is presentation-only (demands-only table, confirm blocked); debt authority untouched |
| Plan 43 settlements | **DEFERRED** | no settlement→scenario default mechanism exists; intended mapping documented in `TRADE_SETTLEMENT_PATROL_INTEGRATION.md` |
| Plan 45 patrols | **DEFERRED** | no patrol→trade transition exists; smuggler/black-market scenarios independently valid |
| Plan 56 economy goods | **NOT APPLICABLE in this seam** | scenario goods reference `items.json` directly (test-verified); reconciliation map in `TRADE_SCENARIO_STOCK_MATRIX.md` |
| Plan 62 trade tells | **LIVE — integrated** | `TradeTellEngine` + `trade_tell_lines.json` consumed via stance × trust band; seed-deterministic; zero tell IDs authored in scenarios |

## 14.5 Economy balance

- **Final "modifiers":** none exist — per-item canonical worths are the price authority
  (`canned_food` 18, `clean_water` 22, `fuel` 40, `filter_pack` 28, …), globally
  consistent across all 15 tables and both edges (test-locked, spread < 0.01).
- **Representative verdicts:** fair/confirm on 9 of 12 new scenarios; short/blocked on
  `winter_cart`, `emergency_requisition`, `settlement_of_accounts` — the urgency,
  crisis, and obligation postures.
- **Bulk quantity ranges:** `crate_lot` ask edge = 9 units over 2 lines (catalog max);
  all at canonical worths — volume, not discount.
- **Scarce-goods limits:** rare comms/medicine appear as single units at high worth;
  bulk scenarios carry only common staples.
- **Arbitrage findings:** structurally impossible (one worth per item, no buy/sell
  spread, static tables, no currency loop) — full audit in `TRADE_ARBITRAGE_AUDIT.md`.
- **Adjustments made:** table arithmetic tuned during authoring so every
  `expected_fairness` matches the computed verdict; no runtime changes needed.

## 14.6 Persistence / determinism

- **Scenario save policy:** static content — nothing persisted; old saves unaffected;
  all three original IDs locked (test-pinned).
- **Stock save policy:** N/A (fixed tables; no generation/depletion).
- **Negotiation save policy:** N/A (tells resolved per binding from seed).
- **Determinism evidence:** `Catalog_TellSelectionIsSeedDeterministic` — same seed +
  scenario ⇒ same tell ID and line; scenario selection itself has no RNG.
- **Reroll-prevention evidence:** no selection/stock RNG exists to reroll; presenter
  zero-mutation invariant test-pinned.

## 14.7 Verification

See `PLAN61_REGRESSION_MATRIX.md` — all gates green:

- `dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — 0 errors
- `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` — **9426/9426 PASS** (3 consecutive runs; trade subset 49/49)
- `dotnet build Ashfall.csproj` — 0 errors, 0 warnings
- `godot --headless --path . -- --data-integrity-selftest` — PASS (298 catalogs, 0 findings)
- `godot --headless --path . -- --bridge-selftest` — PASS
- `godot --headless --path . -- --economy-selftest` — PASS
- `godot --headless --path . -- --content-utilization-selftest` — PASS (exit 0)
- `godot --headless --path . -- --save-store-checksum-selftest` — PASS (21/21)
- `godot --headless --path . -- --7-day-smoke-selftest` — PASS
- `godot --headless --path . -- --real-campaign-journey-selftest` — PASS

## 14.8 Remaining risks / deferred work

1. **Settlement defaults (Plan 43):** four scenario→settlement-type mappings are
   documented and ready; wiring requires the future settlement→trade producer.
2. **Patrol trade (Plan 45):** `border_runner` + black-market scenarios await a
   patrol-encounter producer that respects disposition checks.
3. **Debt interaction (Plan 40):** real in-seam repayment needs a
   `ITradeExecutionSink`-routed debt action; scenario data must not change.
4. **Tell depth per archetype (Plan 62 follow-on):** archetype-flavored tell entries
   belong in the tell corpus/engine, not in scenarios.
5. **Suite flakiness (owned elsewhere):** two transient full-suite runs showed
   order-dependent failures in catalog-sweep classes touching the concurrent stream's
   modified catalogs; three consecutive final-state runs are fully green. Flagged in
   the regression matrix for the owning stream.
