# Trade Scenario Eligibility Matrix — Plan 61 (repository truth)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## Finding: there is no eligibility engine

`TradeScreenScenario` is a **static authored snapshot**. The runtime schema has no
`special_condition`, no producer registry, and no selection engine — a consumer
requests a scenario by ID (`TradeScreenScenarioLoader` → `CreateBinding`). Merely
loading the catalog never mutates any system (the presenter's zero-mutation
invariant is separately test-pinned).

Therefore the plan's provisional eligibility predicates (min reputation, min day,
required flag/item, debt threshold, settlement/patrol context) are **not
implementable without inventing new Core behavior**, which Plan 61 §6.1 forbids.
The truthful eligibility model is:

| Scenario | "Availability" model | Deferred predicate hook |
|---|---|---|
| all 15 | selected explicitly by ID by the consuming system (tests, skin track, future encounter producer) | a future producer may gate selection on world state without schema change |

## Impossibility audit (plan §61E.1)

Because there are no predicates, there are **no impossible compound conditions**:
no scenario can become unreachable through conflicting gates, and no predicate can
mutate faction/debt/settlement/patrol state on evaluation (plan §1.4 satisfied
trivially — eligibility evaluation does not exist).

## Documentation-only integrations (NOT authored as data)

| Plan | Status | Evidence |
|---|---|---|
| Plan 43 settlements | **DEFERRED** | no settlement→scenario default mechanism exists in the trade-screen seam; `settlements.json` is a separate authority with no consumer of `trade_screen_scenarios.json` |
| Plan 45 patrols | **DEFERRED** | no patrol→trade transition exists in this seam; smuggler (`border_runner`) and black-market scenarios are independently valid content |
| Plan 40 debt | **DEFERRED** | `settlement_of_accounts` presents an obligation table; the debt authority (outside this seam) owns all debt state. No repayment, interest, or in-kind logic was simulated through prices |

When a real producer lands, it can adopt these scenarios by ID and add eligibility
at its own layer; the catalog requires no migration.

## Fallback behavior (plan §61E.8)

- Unknown scenario ID → `LoadFromJson` yields no record; consumers find nothing
  (no silent generic substitution that could mutate terms).
- Loader tolerance for malformed records is documented in
  `TRADE_SCENARIO_SCHEMA_MAP.md`; data-contract tests fail loudly on invalid data
  before runtime ever sees it.
