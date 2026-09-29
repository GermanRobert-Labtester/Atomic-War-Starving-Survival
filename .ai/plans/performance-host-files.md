# Plan 2 — Domain ownership and day-advance orchestration

STATUS: APPROVED BY USER

Editorial revision: 2026-09-28. Existing scope and approval retained.

## Outcome

Make host responsibilities traceable by domain while preserving gameplay,
deterministic ordering, save ownership and CLI entry points.

## Contract and sequence

1. Map historical `Main.Plans*.cs` bundles to their actual domains. Split mixed
   bundles; merge same-domain fragments. Preserve member bodies, method names,
   and Godot UID associations. Missing local callers alone do not prove dead code.
2. Keep cross-domain lifecycle composition in `Main.SubsystemComposition.cs`.
   Update source probes and references affected by renamed paths.
3. Route day advancement through `CampaignDayHostSession`, wrapping the existing
   `CampaignDayCoordinator`. Registration, ordering, rollback, RNG and persistence
   remain with the coordinator. Publish the committed day through `IEventBus`
   only after success; unsubscribe on disposal and reset during host teardown.
4. Split `HostCli.PanelTests.cs` by command into `HostCli.Command.*.cs` without
   changing dispatch or assertions. Coordinate export exclusions with Plan 1.

## Ownership

The [decomposition map](../../docs/architecture/MAIN_DECOMPOSITION_MAP.md)
records exact historical sources, destinations and current domain dependencies;
it replaces the duplicated filename inventory in this document.

Shared seams: `src/Main.Campaign.cs`, `src/Main.CampaignOwners.cs`,
`src/Main.Holdfast.cs`, `src/Main.Lifecycle.cs`, and
`src/Host/CampaignDayHostSession.cs`. The integrator owns their acceptance.

Reference adjustments also cover `src/Host/HostCli.RailTrackMaintenance.cs` and
the existing tests `Plan211InternalCommunicationHostWiringTests`,
`Plan218MuseumHostWiringTests`, and `Plan24NeedsSourceMigrationTests`.
Current claims remain governed by `WORKTREE_OWNERSHIP.md`.

## Acceptance

- Every historical bundle has a destination; live references resolve.
- Mechanical moves preserve member bodies. Any behavior repair is identified
  separately and receives its own focused verification.
- `MainTriadDriftGateTests`, affected source-path tests and relevant CLI commands
  pass; runtime checks cover day commit, failure and host reset/disposal.
- Both build configurations compile and retain the intended command boundary.

Run affected tests through `bin/run-scoped-tests`; do not repeat the full suite
after renames. A successful compile alone does not prove lifecycle integration.
