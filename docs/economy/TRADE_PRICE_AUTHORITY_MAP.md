# Trade Price Authority Map — Plan 61

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

One owner per mutable fact. Where repository truth contradicted the provisional
plan mapping, the row was corrected (2026-02, against current source).

| Fact / behavior | Authority (verified) | Plan 61 role |
|---|---|---|
| scenario definitions | `trade_screen_scenarios.json` + `TradeScreenScenarioLoader` | authored content (data-only expansion) |
| scenario "selection" | consumer requests scenario by ID (tests / skin track) — no eligibility engine exists | deterministic ID lookup; no RNG over scenarios |
| trade presentation | `TradeScreenPresenter` → `TradeScreenViewModel` | paints context/terms; zero-mutation vs providers |
| table line value (mock/skin track) | **authored `unit_price` per line** (scenario data) | the only price field that exists in a scenario |
| table line value (live track) | host-supplied `unitPriceLookup` (host adapters project `economy_goods.json` / `GoodsCatalog` + `HardcoreEconomyTuning` scarcity/shock composition) | base value remains outside scenario data |
| qualitative worth labels | `TradeWorthLabels.Format` (ECON-002: no raw digits at the offer edges) | presentation |
| biological offer value | `TradePricing.BioUnitValue` = `((int)kind + 1) × 25` | core rule; never duplicated in data |
| fairness verdict | computed in `TradeScreenViewModel.SetTable` | `expected_fairness` is a test-pinned expectation, not a second engine |
| price shock badges | scenario `price_shocks[]` (mock binding) / `IPriceShockProvider` (live track) | presentation only — **no price math in this seam** |
| scarcity bands | scenario `scarcity[]` (mock binding) / `IPriceShockProvider` (live track) | presentation only |
| base item existence | `items.json` | every `item_id` must resolve (test-enforced) |
| faction identity | `faction_radio_corpus.json` canonical set | every `faction_id` must resolve (test-enforced) |
| faction standing / stance engine | `FactionStanceEngine` + `FactionThresholds` (live track) | untouched; scenarios are snapshots, not stance writes |
| tell lines | `trade_tell_lines.json` + `TradeTellEngine` (Plan 62 — LIVE) | auto-selected from stance × trust band |
| intent execution | `ITradeIntentSink` / `ITradeExecutionSink` | mock sink records; live sink executes; presenter never mutates |
| save state | campaign save stores (separate systems) | scenarios are static content — nothing to persist |
| debt / settlement / patrol state | `TradeCreditCoordinator` + `ledger_debt_templates.json` (debt); `settlements.json` (settlement defaults); patrol owners (outside this seam) | **no linkage to scenario data exists** — deferred, not faked |

## Price-semantics findings (plan §1.2 disposition)

1. There is **no `price_modifier` field** and no composed price formula in this seam.
   The plan's "desperate = 1.5 / bulk = 0.8" multipliers are **not implementable**;
   price posture is authored per line.
2. Buy/sell direction does not exist: a table line's `unit_price` is the item's
   *worth* on either edge of the barter. Fairness compares the two edges' totals.
3. Because worth is authored per item, the anti-arbitrage rule is **global
   unit-price consistency**: the same `item_id` must carry the same `unit_price`
   in every table and on both edges (matches the three baseline scenarios:
   `canned_food` = 18 everywhere). Enforced by test.
4. Bio offers have one price authority (`TradePricing`); no scenario may encode a
   bio equivalent value.
5. Rounding/clamps: not applicable — all authored prices are integers; the test
   gate rejects `unit_price <= 0` and non-integer values.
6. Live-track composition note (merged from the 2026-09-03 planning version): on the
   live track, the host's unit-price lookup composes base value with scarcity/shock
   tuning **before** the seam; the scenario seam itself never composes multipliers
   into prices. Both tracks agree that scenario data owns no price math.
