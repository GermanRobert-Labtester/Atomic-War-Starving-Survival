# Trade Settlement / Patrol / Debt Integration — Plan 61 (all DEFERRED)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

Producer/consumer graph as actually implemented. **No fabricated linkage was
committed**; every row states the repository evidence.

## Producer/consumer graph (as implemented)

| Scenario ID | Producer (verified) | Eligibility inputs | Consumer | Stock source | Price source | Feedback | Persistence owner |
|---|---|---|---|---|---|---|---|
| all 15 | consumer requests scenario **by ID** (tests, skin track `CreateBinding`, future encounter producers) | none — static content | `TradeScreenScenarioLoader` → `TradeScreenViewModel` / `TradeScreenPresenter` | authored table | authored `unit_price` + `TradePricing` bio rule | VM `Changed` event, radio ticker, fairness label | none required (static content) |

A scenario with no producer is content dead code **only if nothing can ever request
it**; the skin-track/tests request every scenario by ID today, and the catalog is
the documented surface for future encounter producers. The mechanical
differentiation and binding gates prove each record loads, binds, and behaves.

## Plan 43 — settlements: DEFERRED

Requested defaults (trade post → caravan, stronghold → quartermaster, refugee camp →
refugee barter, community → bulk dealer) **cannot be wired**: no settlement→scenario
default mechanism, override chain, or precedence rule exists in the trade-screen
seam. `settlements.json` was checked; it has no consumer of
`trade_screen_scenarios.json`. The intended mapping is preserved here for the future
implementer:

- trade post → `long_road_caravan`
- stronghold → `depot_window`
- refugee camp → `road_knowledge`
- community → `crate_lot`

Precedence note for that future work: encounter/debt-specific context should
outrank settlement-type defaults (plan §61C.2); no cached-scenario risk exists
while scenarios are requested per open.

## Plan 45 — patrols: DEFERRED

No patrol-encounter → trade transition exists in this seam. `border_runner`
(smuggler) and the black-market scenarios are independently valid content reachable
by ID. When patrol trade lands, the producer must respect existing disposition
checks (plan §61C.3) — no scenario data change is required or permitted to fake it.

## Plan 40 — debt: DEFERRED (presentation-only)

`settlement_of_accounts` models the *moment* of being called to account: a bare
player edge against a heavy ledger edge (`expected_fairness: short`,
`confirm_succeeds: false`). The plan §61C.4 invariant holds by construction:

1. no debt state exists in this seam to duplicate;
2. opening the scenario mutates nothing (zero-mutation presenter invariant, test-pinned);
3. no price multiplier pretends to repay anything;
4. save/load of real debt state belongs to the debt authority, untouched.

If the debt system later grows a trade-seam repayment action, this scenario is the
natural presentation context — wired through `ITradeExecutionSink`, not through
scenario data.

## Plan 56 — economy goods: NOT APPLICABLE in this seam

No goods/stock-profile registry feeds scenario tables. All references are to
`items.json` (verified). If an economy-goods catalog later becomes the stock
authority, the stock matrix above is the reconciliation map.
