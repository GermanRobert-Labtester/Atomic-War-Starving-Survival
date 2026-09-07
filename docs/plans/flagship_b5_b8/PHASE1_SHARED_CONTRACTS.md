# Phase 1 — Shared Contracts (B5–B8) — landed

> Contracts added in Phase 1. No gameplay balance changed; nothing in
> production consumes them yet. Phases 3–7 must consume these shapes instead
> of inventing parallel mechanisms.

## 1. Capability query — `ResearchSystem.HasCapability(knowledgeId)`

- File: `Assets/Ashfall.Core/Research/ResearchSystem.cs`
- Thin, documented alias over `IsManualUnlocked`. Behaviorally identical.
- Rules for consumers (greenhouse, power builds, defense builds, water sources):
  - `HasCapability == true` **gates what may be built** — it never grants the
    built instance, never applies an invisible multiplier, never substitutes
    for a canonical item/recipe cost.
  - Do not cache unlock truth locally; query live.
- Tests: `Phase1SharedContractsTests.HasCapability_*` (parity, null/empty, unknown id).

## 2. Water request seam — `WaterRequest` / `WaterRequestPreview` / `WaterRequestContracts`

- File: `Assets/Ashfall.Core/WaterRequest.cs` (extension methods over
  `WaterTreatmentSystem` — the single spendable water authority).
- Shapes:
  - `WaterRequest { ConsumerId, Quality (live `WaterType`), RequestedAmount, Purpose }`
  - `WaterRequestPreview { CanCommit, AvailableAmount, RequestedAmount, Quality, ReasonCode }`
  - Reason codes: `no_authority`, `invalid_amount`, `unknown_type`, `insufficient_water`.
- Behavior:
  - `PreviewWaterRequest` — pure; mutates nothing; never rolls RNG.
  - `CommitWaterRequest` — all-or-nothing; consumes the exact amount exactly
    once via `RemoveWater` (the same mutation path all existing consumers use);
    zero mutation on failure.
  - Quality is per-pool: requesting `Clean` never silently substitutes `Raw`/`Irradiated`.
- Plan 189 boundary: this is the consumer-facing seam only. Source routing,
  network topology and aquifer simulation stay out (see
  `PLAN66_PLAN189_BOUNDARY.md`). The quarantined `WaterAuthorityMassBalanceTests`
  suite (`Ashfall.Core.Tests/Water/`, removed from compilation — targets an
  in-flight `DrawWater`/`IOutputSink` API) is the conservation contract that
  will govern this authority once its stream lands it.
- Tests: `WaterPreview_*`, `WaterCommit_*` (purity, atomicity, exact-once, quality enforcement).

## 3. Power tick-summary subscription — `PowerGridSystem.OnTickSummary`

- Already live (`event Action<PowerGridTickSummary>?`), raised exactly once per
  `TickDay` with a payload identical to the returned summary; raised even with
  no subscribers. Existing subscribers: `PowerGridHostSession` (state-changed
  fan-out), `ShelterAudioController` (brownout cue).
- Rules for new consumers (greenhouse controlled-environment, sump pump,
  perimeter sentries, vinyl/radio):
  - Subscribe to `OnTickSummary`; never call `SetBrownout`, never mutate
    generation, never read a private power counter.
  - Derive consequences from the typed summary (served/shed/brownout/critical
    deficit as those fields land in Phase 2) inside your own authority.
- Tests: `OnTickSummary_*` (exactly-once, payload parity, no-subscriber safety,
  same-seed determinism).

## 4. Preview/commit standardization

The canonical pattern for **all** multi-authority actions is the existing Core
`Ashfall.Core.PlayerCommand` family:

- `CommandPreview` (side-effect-free, `ProjectedDeltas`, `StateVersion` for
  stale detection) + `CommandResult` (+ `PlayerCommandCode`), as already used
  by greenhouse treat-blight and water-treatment start.
- The `WaterRequest` seam deliberately reuses this philosophy (preview →
  authoritative commit, stable reason codes) without forcing state-version
  plumbing into the water pools in Phase 1.
- New commands in Phases 3–7 (nutrient dosing, source builds, defense builds,
  priority changes) must either extend `PlayerCommandCode` with preview/commit
  or use a preview/commit extension pair like `WaterRequestContracts` — never
  consume-then-discover-failure.

## Test entry point

`Ashfall.Core.Tests/Phase1SharedContractsTests.cs` — 13 tests, all green.
