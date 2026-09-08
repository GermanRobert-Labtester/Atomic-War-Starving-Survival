# Plan 61 Save Compatibility

> **Provenance (Plan 61).** This document supersedes the 2026-09-03 v1.0.0 planning
> version of the same deliverable (authored before the catalog expansion landed).
> The planning roster was never landed in `trade_screen_scenarios.json` (baseline
> remained 3 scenarios) and its table design referenced item IDs absent from
> `items.json` with per-item prices violating the global unit-price consistency
> gate. This version is the implementation record for the landed 15-scenario
> catalog; unique correct authority facts from the planning version are merged.

## What is saved today: nothing in this seam

Trade scenarios are **static content**. The trade-screen seam persists no scenario
ID, no generated stock (none exists), no negotiated terms, and no transaction
history (plan §61E.4 findings). Player inventory, faction trust, and any future
debt/settlement state remain owned by their existing campaign save sections.

## Consequences of the expansion

| Concern | Verdict |
|---|---|
| Old saves referencing original 3 scenario IDs | **SAFE** — all three IDs preserved with locked contracts (`Catalog_OriginalThreeScenariosKeepLockedContracts`) |
| Saves from before Plan 61 loading the expanded catalog | **SAFE** — additive JSON; loader reads whatever records exist; no schema_version change (still 1) |
| Active scenario state round-trip | **N/A** — nothing to persist |
| Stock depletion/refresh round-trip | **N/A** — static tables |
| Unknown scenario IDs in future/corrupt saves | Loader yields no record; no silent generic substitution (plan §61E.5 honored) |
| Determinism | Same seed + same scenario ⇒ same tell selection and identical table math (test-pinned) |

## ID stability policy (plan §6.4)

All 15 IDs are stable content names from this point. Removal of any shipped
scenario ID requires a documented migration/fallback policy in a future plan.
No ID was renamed, renumbered, or re-prefixed (`fair_deal`-style plain snake_case
retained; no `trade_*`/`scenario_*` prefix invented).

## UI rebinding discipline

Presenter binding/unbinding cannot reroll scenarios, regenerate stock, consume RNG
beyond the documented tell selection, or mutate faction/debt state — pinned by the
existing `Presenter_ZeroMutation_InvariantHolds` and the new binding/determinism gates.
