# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Repo-wide 6-loop find → repair → harden sweep (small-panel l10n + CSV integrity)

> **STATUS: APPROVED BY USER**

User-directed ("do 6 looping phases of find issues, repair them, harden spots
where issues found and repeat repo wide!").

## Bounded outcome

Six bounded find→repair→harden loops over the least-localized UI panels, plus the
cross-cutting defects the loops surfaced (CSV field integrity, gate registration,
prefix collisions). Presentation + tests/gates only: no save section, no mutable
state, no gameplay decision, no new authority.

## The 6 loops

1. `AirlockSecurityPanel` unbound placeholder → `ui.airlock.unbound`.
2. `ApprenticeshipPanel` → `ui.apprenticeship.unbound`.
3. `CaregivingPanel` → `ui.caregiving.unbound`.
4. `CenturySeedPanel` (4 strings) → `ui.century_seed.*`.
5. `DeepCoastPanel` → `ui.deep_coast.unavailable`.
6. `DutyRosterDetailPanel` → `ui.duty_roster_detail.no_marks`.

All six route through `AshfallLocalization` (the `ExpeditionPhaseText` precedent),
so they do not trip the registered-panel zero-tolerance gate while their remaining
`Make*(...)` chrome is still unlocalized.

## Exact paths

- `src/UI/AirlockSecurityPanel.cs`, `src/UI/ApprenticeshipPanel.cs`,
  `src/UI/CaregivingPanel.cs`, `src/UI/CenturySeedPanel.cs`,
  `src/UI/DeepCoastPanel.cs`, `src/UI/DutyRosterDetailPanel.cs`
- `assets/l10n/strings.csv` (the six prefixes named above, plus corrected
  `ui.barter.*` and `ui.greenhouse.*` CSV field quoting)
- `Ashfall.Core.Tests/Tooling/LocalizationRatchetTests.cs`
- `Ashfall.Core.Tests/UI/StatusPanelThresholdTests.cs`
- `.ai/state.md`, `WORKTREE_OWNERSHIP.md`, `INTEGRATION_PLANS.md`, this plan

## Cross-cutting repairs surfaced by the loops

- **5 malformed catalog rows** with unquoted commas
  (`ui.barter.requested_zero`, `ui.barter.offered_zero`,
  `ui.barter.balance_metrics_zero`, `ui.greenhouse.filter.greens_hint`,
  `ui.greenhouse.filter.legumes_hint`) — decimal-comma German and comma English
  fields quoted. `StringsCsvLocaleGateTests` now 4/4.
- **Key-prefix collision pin** 10 → 11: `ui.status.expedition_injury.unnamed`
  (used in `StatusPanel.cs`) is an intentional base/variant pair.
- **Missing drift-gate registration**: `GreenhousePanel.cs` uses
  `AshfallUiText.Tr` 85× with no raw chrome but was unregistered; the concurrent
  Wave-9 sweep registered it and the `LocalizedSurfaces_RegisteredInDriftGate`
  gate is green.

## Hardening

- Lowered the `Text=` UI-literal ratchet **528 → 510**.
- Added a new repo-wide **`Raw Make*(...) chrome literals` ratchet** (baseline
  1411) in `LocalizationRatchetTests`: registered panels are already held to zero
  by `RegisteredPanels_HaveNoRawChromeLiteral`, but the `Text=` ratchet could not
  see chrome passed straight into `MakeButton`/`MakeTitle`/`MakeDataRow`/… in the
  unregistered panels. Growth now fails.

## Deferred finding (authority decision required)

`AquiferTreatyConcessionPanel` renders fabricated telemetry (hardcoded siphon
depth, salinity, tribute amounts, lock-out timer) with **no Core authority** and
is routed in `Main.UiPanels.cs`. It is a fake operational route; repairing it
needs an authority decision, so it is recorded rather than improvised.

## Evidence

Host build 0 warnings / 0 errors; `StringsCsvLocaleGateTests` 4/4;
`StatusPanelThresholdTests` 211/211; `LocalizationRatchetTests` 3/3;
`ExpeditionLocaleKeysTests` 5/5; `l10n_drift_gate` PASS (1410 keys, German parity
verified); 0 duplicate catalog keys; `git diff --check` clean. No full suite; no
commit. Transient build/test failures during the sweep were concurrent-writer
artifacts and passed on re-run.
