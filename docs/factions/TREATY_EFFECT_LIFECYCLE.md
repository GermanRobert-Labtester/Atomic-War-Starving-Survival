# Treaty Effect Lifecycle (Plan VIII · Task 21)

Status: implemented (Slices 1–3, 2026-09-05). This is the lifecycle/effect
reference for `RegionalTreatySystem` world consequences.

## Authorities touched (plan §21.1 mapping to real code)

The plan was written against aspirational names; the implementation binds to
the existing typed authorities:

| Plan concept | Actual authority |
|---|---|
| "price factor / ExplainPrice stack" | `CaravanTradeNetworkSystem.CalculateItemBuyPrice` multiplier stack (no `ExplainPrice` exists in the repo) |
| "raid pressure authority" | `Muster.IronRaidersSystem.EvaluateRaidChance` + additive composition at read sites (same pattern as `DebtConsequenceHostBridge` bounty boost) |
| "escalation spine (Plan 25C)" | `FactionWarSystem.ModifyStanding` (canonical, clamped, event-raising); `RegionalTreatySystem.CountByStatus` feeds `MusterPathInput.ActiveTreatyCount`/`ViolatedTreatyCount` |
| "radio/news/briefing surfacing" | `RadioScheduleCoordinator.InjectTreatyAlert` → "Regional Compact Wire" bulletin on `station_open_classroom` / `station_automated_relay` |

## Lifecycle states and transitions

```
Proposed ──Ratify──► Ratified ──TickDay(term_days elapsed)──► Expired
   │                     │                                 (effects end, no breach)
   │                     └──BreakTreaty (betrayal)──┐
   │                     └──compliance decay → 0───┤
   │                                               ▼
   └──────────────────────────────────────────► Violated
                                             (effects end + breach consequences)
```

- `Propose` mutates state and raises `OnTreatyStatusChanged` only — no world
  effects start, so no `TreatyTransition` is emitted.
- `Ratify` / `BreakTreaty` / compliance violation / term expiry mutate state
  and then emit exactly one typed `TreatyTransition` on `OnTreatyTransition`
  with `StartedEffects` (ratify) or `EndedEffects` (every exit path).
- `TreatyViolationCause`: `Betrayal` (player-initiated `BreakTreaty`),
  `ComplianceFailure` (interval decay reached zero), `None` (expiry).

## Typed effect contract

Data authority keeps authored `TreatyEffect.effect_type` strings;
`TreatyEffectTable.TryMapKind` is the single interpretation point:

| data string | kind | default value |
|---|---|---|
| `economy_discount` | TradeDiscount | 0.10 |
| `raid_pressure_relief` / `security` | RaidPressureRelief | 0.05 |
| `supply_relief` | SupplyPriceRelief | 0.10 |
| `water_quota` | WaterQuota (informational) | — |
| `power` | PowerQuota (informational) | — |
| anything else | skipped (never guessed) | — |

`RegionalTreatyFeed.Map` derives `raid_pressure_relief` / `economy_discount`
effects from authored tags (`security`/`peace`/`sky_defense` → security;
`trade`/`economy`/`market`/`barter` → trade), preserves the full
`signatory_factions` list, and maps the optional `term_days` field
(only `treaty_01_lock_4_sluice_and_brine_concession` carries `term_days: 180`
today; all other accords are indefinite and exit via compliance decay).

Stable effect identity (§21.7): `SourceId = treaty:{treatyId}:effect:{kind}`.
Descriptor ordering is ordinal by (treatyId, kind, targetId) — deterministic
across runs and hosts (§21.11).

## Consumers (all pull-derived — the restore-safety mechanism)

**There are no granted-and-persisted modifiers anywhere in this pipeline.**
Every consumer derives the treaty term from treaty state at read time, so
`RestoreState` (which emits no transitions and mutates only treaty state)
cannot double-apply, and expiry/breach removal is inherently symmetric.

| Consumer | Read | Path |
|---|---|---|
| Caravan buy prices | `GetTradeDiscount(faction)` (any-signatory match, best-of, clamped 0..0.5) | `CaravanTradeNetworkSystem.SetTreatyPriceReliefProvider` — applied inside the multiplier stack after favored status |
| Raid pressure | `GetRaidPressureModifier()` (+`BreachRaidPressure` 0.15 per Violated, −relief per active security pact, clamped ±0.5) | composed additively with `IronRaidersSystem.EvaluateRaidChance()` at read sites (host display wired; same composition pattern as the debt-bounty accumulator) |
| Escalation | transition `IsBreach` → standing penalty `violation_penalty_affinity` (−20) | host consumer calls `FactionWarSystem.ModifyStanding` — the canonical API; counters are never hand-edited |
| Radio | every transition → `TreatyBulletins.Compose` | host injects via `RadioScheduleCoordinator.InjectTreatyAlert`; copy states the concrete gain/loss and never renders raw ids |

## Save/load idempotency (§21.8)

- `RegionalTreatyState` persists only `TreatyInstance` rows (status, days,
  compliance). Effect activity is a pure function of (status × catalog).
- Save during a ratified accord → restore → discount/relief are active exactly
  once (derived, not re-granted).
- Save after a breach → restore → the standing penalty is **not** re-applied
  (transitions don't fire during restore; the penalty lives in
  `FactionWarSystem`'s own persisted state, applied exactly once at transition
  time). The Violated status still yields the derived raid-pressure term.
- Old saves without `term_days`/signatory fields: fields default to
  0/empty = indefinite, faction matching falls back to `faction_id`.

## Known limits / non-goals

- `MusterPathEvaluator` production wiring remains a demo-only path (pre-existing
  gap); `CountByStatus` now exposes the inputs it needs.
- `CaravanTradeNetworkSystem` has no host mount yet (Core + tests only), so the
  trade discount is exercised at Core level; the panel/radio surface the terms
  to players today.
- `WaterQuota`/`PowerQuota` are displayed as consequences but drive no shelter
  system yet (pre-existing state, unchanged by Task 21).
- The treaty panel displays consequences and honest compliance; Propose/Ratify
  buttons remain unbuilt (the session APIs exist and are tested; scrap payment
  is validated but not deducted — wiring a real payment path is a separate task).

## Tests

`Ashfall.Core.Tests/RegionalTreatyConsequenceTests.cs` — typed mapping,
transitions (ratify/break/compliance/expiry), restore-silence +
derived-state identity, raid modifier math + clamp, signatory matching,
`CountByStatus`, descriptor ordering, caravan price relief/removal/clamp,
bulletin copy, and the full propose→ratify→save→restore→breach→save→restore
arc with exactly-once escalation and surfacing.
