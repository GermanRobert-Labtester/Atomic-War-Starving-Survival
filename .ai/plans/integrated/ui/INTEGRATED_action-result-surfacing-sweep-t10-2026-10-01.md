# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**
> (Approved by user 2026-10-01 — T10 of the playability checklist; integrated in
> the same session. Original plan follows.)

## Goal

Close the remaining player-facing "why did nothing happen?" gaps left by the
2026-10-01 expedition/craft surfacing package. That package audited four
families (expedition, craft, cook, duty roster) and explicitly flagged the same
one-line pattern in `WorkshopPanel`/`PharmaLabPanel` plus "outside the four
audited families" panels. This package sweeps those, surfaces the refusal, and
records the projection surfaces in `docs/ACTION_RESULT_SURFACING_MATRIX.md`.

## Verification (commands and results)

- `dotnet build Ashfall.csproj --no-restore -v:minimal` — Build succeeded, 0
  errors / 6 pre-existing CS0162 warnings in untouched `HostCli.*` probes.
- New `Ashfall.Core.Tests/UI/ActionResultSurfacingGateTests.cs` — 3/3 PASS
  (host LastEvent failure branch, host-backed panels render LastEvent, and
  direct-panel shared formatter).
- `--ui-layout-selftest` — Failures: 0, UiControllerParity 61/61.
- `--workshop-relic-uitest` — PASS, Errors: 0.
- `--expedition-panel-uitest` — PASS.
- `--player-panels-uitest` — PASS 22/22.
- `--cloud-seeding-selftest` — PASS 7/7 checks.
- `--plans-122-125-selftest` — PASS 41/41; `--precision-metrology-selftest` —
  PASS 12/12.
- `LocalizationRatchetTests` — GREEN at 611 ≤ baseline 612 after a ratchet-down
  pass localized the 8 foreign-grown UI literals into `assets/l10n/strings.csv`
  and removed a dead `FeedbackPanel` tooltip assignment; `StringsCsvLocaleGateTests`
  4/4 and `scripts/ci/l10n_drift_gate.py` PASS.
- `git diff --check` on changed files — clean.

## Limitations / open

- `RadioProgramProductionHostSession.TryDeliver` / `ResolveFollowUp` now also
  publish `LastEvent` (string-returning, not on the current player-facing
  RadioPanel path).
- **Convention fully hardened (same session):** every host method that
  publishes a success `LastEvent` now publishes a failure `LastEvent` too — 73
  methods across 26 host sessions (autopsy, chemical dependency, duty roster,
  excavation, greenhouse, kitchen nutrition, library study, mental-health
  crisis, night watch, regional treaty, water treatment, fluid logistics,
  salvage, and the original swept set), pinned by `ActionResultSurfacingGateTests`.
  `Refuse()` helpers cover the early-return paths
  (`WildlifeTrappingHostSession`, `NightWatchHostSession`). The host-session
  convention scanner now reports **zero** remaining hits. Two real fixes fell
  out of it: `FluidLogisticsHostSession.AdvanceDay` no longer overwrites a
  transfer refusal with the tick line (the refusal is folded into the tick
  message), and `SalvageHostSession.TryTeardown` raises `StateChanged` on
  refusal and names the `SalvageOutcome`.
- No panel-specific headless probe exists for `PharmaLabPanel`; it is covered by
  the shared `ActionRefusalText` source gate and `--ui-layout-selftest`.
- The matrix lists representative codes, not an exhaustive per-system catalog.

## Non-goals (unchanged)

- No Core gameplay change, no new catalog, save section, or determinism surface.
- No change to the four already-audited families.
- No mass rewrite of host-session architecture; only the existing
  `LastEvent`/`RefreshView` seam is extended for the affected owners.
- No commit; no full test suite.

## Premise evidence (read-only, verified before editing)

- `ActionResult` (Assets/Ashfall.Core/ActionResult.cs) already carries stable
  `FailureCode`; the matrix says Core owns codes and host/UI owns wording.
- Many host sessions set `LastEvent` **only** inside `if (res.IsSuccess)`, so a
  runtime refusal leaves the previous success text on screen. The panel then
  discards the returned `ActionResult` and calls `RefreshView()`, which renders
  the stale `LastEvent` (e.g. `DecontaminationHostSession.ProcessQueue`).
- Several host sessions that own a `LastEvent`-displaying panel emit a
  successful-sounding message on the failure branch (e.g.
  `AirlockSecurityHostSession.RepairDoor` → `"Blast door repaired: {FailureCode}"`).
- `WorkshopPanel` and `PharmaLabPanel` bind Core systems directly (no host
  session, no `LastEvent` label) and discard every result:
  `WorkshopPanel` (`TryStartJob`, `TryCancelJob`, `TryCollectCompletedJob`,
  `TryOverhaulTooling`, `StartRepair`, `CancelJob`) and `PharmaLabPanel`
  (`StartBatch`, `CancelBatch`).
- `WeatherForecastPanel` discards `CloudSeedingSystem.Install` (ActionResult)
  and `Deploy` (CloudSeedingResult).

## Plan / changes

1. New shared UI formatter `src/UI/ActionRefusalText.cs` (Core-code → prose,
   with a humanizing default) for panels that have no host `LastEvent` seam.
2. Host sessions: always assign `LastEvent` on both branches for the affected
   ActionResult methods so the existing panel `LastEvent` surface tells the
   truth.
3. Direct-system panels: capture the result and render a refusal line via the
   shared formatter.
4. Extend `docs/ACTION_RESULT_SURFACING_MATRIX.md` with the swept surfaces.
5. Add a focused source gate pinning the swept host methods' failure surfacing.

## Acceptance / verification

- `dotnet build Ashfall.csproj` — 0 errors.
- Focused `--ui-layout-selftest` / panel self-tests unaffected.
- New gate test passes and fails on the pre-fix source (TDD).
- `git diff --check` clean.
