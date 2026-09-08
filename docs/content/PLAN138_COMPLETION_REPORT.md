# Plan 138 Completion Report

Status: IMPLEMENTED; CURRENT HOST RECHECK BLOCKED BY UNRELATED WORKTREE CHANGES.

## Repository revision

`c3a5bf6a64a8c35284c689c44afbafd1cb517caf` plus the uncommitted Plan 138
working tree.

## Scope

Plan 138 adds deterministic fresh-campaign cohort selection while preserving
the exact Standard Holdfast opening. Cohort data is initialization input only;
the actual survivor aggregate becomes authoritative immediately after
composition.

## Gate record

## Implementation

- Added six-profile `starting_survivor_cohorts.json` authority while retaining
  the flat legacy `starting_survivors.json` file.
- Added Core validation for exact three-member profiles, canonical IDs,
  duplicate IDs, value ranges, active-questline exclusion, Standard parity,
  default selection, and invalid-alternate isolation.
- Added explicit `FreshInitialize` versus `Restore` lifecycle handling.
- New Game allocates the next free slot and resets only in-memory sessions.
- Restore uses the saved survivor payload, including an explicitly empty
  roster, and never falls back to cohort seeding.
- Added optional menu cohort selection with transient profile selection and
  direct Standard Holdfast fast start.
- Added deterministic first-30-day balance evidence in
  `docs/balance/BALANCE_SIM_STARTING_COHORTS.md`.

## Verification

| Gate | Result |
|---|---|
| `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --no-build` | PASS, 10,085/10,085 |
| focused cohort catalog/balance tests | PASS, 11/11 |
| `dotnet build Ashfall.csproj --no-restore` | BLOCKED by unrelated concurrent Plan 140/UI worktree changes (`Main.Medical.cs`, `Main.SurvivorFate.cs`, untracked `FeedbackPanel.cs`/`ConfirmationModal.cs`) |
| `--starting-cohort-lifecycle-selftest` | PASS in the prior host build; current source re-run is blocked by the unrelated host compile errors |
| `--survivors-uitest` | PASS in the prior host build |
| `--data-integrity-selftest` | PASS, 300 catalogs, 0 errors |
| `--bridge-selftest` | PASS |
| `--content-utilization-selftest` | Not rerun because the host build is blocked; scanner/runtime mappings are present |
| `godot --headless --path . --quit-after 2` | PASS in the prior host build |

The broader pre-existing `--real-campaign-journey-selftest` reaches the
cohort and survivor restore assertions, but later trade/radiation assertions
attempt to restore a slot after an Iron Man terminal defeat and fail. The
dedicated Plan 138 lifecycle gate avoids that unrelated scenario and passes
the required fresh/restore proof.
