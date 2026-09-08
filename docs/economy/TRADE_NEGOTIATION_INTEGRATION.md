# Trade Negotiation Integration — Plan 61 × Plan 62 (LIVE)

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## Plan 62 status: LIVE

`trade_tell_lines.json` + `TradeTellEngine` exist and are consumed by both
`TradeScreenScenarioLoader.CreateBinding` (skin track) and `TradeScreenPresenter`
(live track). Pools exist for **all 5 stances × 4 trust bands**, so every scenario
resolves a tell line (test-gated: `Catalog_EveryScenarioBindsWithTellAndLegibleState`).

## How scenarios integrate with negotiation (repository-native)

Scenarios do **not** reference tell IDs. Integration is entirely through the two
inputs `TradeTellEngine.TrySelectTell(stance, trust, rng, out tell)` reads:

1. **Stance** — chosen per scenario to give the negotiation surface its identity
   (`Trade` = barter available; `ShareIntel` = barter + road knowledge / favors —
   used by `ledgerless_broker` and `road_knowledge` where the fiction is knowledge,
   not goods).
2. **Trust band** — `hostile ≤ −40`, `wary −39..0`, `neutral 1..40`, `warm 41..100`.
   The 12 new scenarios spread across wary (winter_cart −15, settlement_of_accounts
   −20), neutral (9 scenarios), and warm (depot_window 55).

Selection is **seed-deterministic** through `ISeededRng`
(test: `Catalog_TellSelectionIsSeedDeterministic`). Rotation rebinds never consume
state; the same seed + scenario always yields the same line.

## Contract discipline (plan §61C.6)

- Every resolved tell comes from the live tell corpus — **no scenario-owned tell
  text, no duplicate text ownership, no scenario-specific copy of the catalog**.
- Option ordering is deterministic (corpus pools + seeded selection).
- No unresolved forward references were committed; the corpus predates Plan 61 and
  required zero changes.
- Disabled-option UX: nothing to disable — scenarios expose no option IDs.

## Post-Plan-61 depth (deferred, not owed)

Tell-line depth per *archetype* (e.g., smuggler-specific phrasing) is a Plan 62
follow-on: it belongs in the tell corpus as new stance×band entries or a future
archetype selector owned by the tell engine — never as scenario-resident text.
