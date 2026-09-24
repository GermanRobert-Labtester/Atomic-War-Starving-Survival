# UNBLOCK — Plan 202: InterpersonalConflictSystem / Interpersonal Conflict & Grievance

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-plan-202-interpersonal-conflict-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The Interpersonal Conflict & Grievance system is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `InterpersonalConflictSystem` (DEC-342) the live survivor relationship conflict, grievance accumulation,
and dispute resolution authority while preserving single-owner domain boundaries (Rule 5):

- `SurvivorRelationsState` / `SurvivorSocialSystem` own baseline relationship affinities, trust, and romance.
- `InterpersonalConflictSystem` owns active dispute states, grievances between pairs of survivors, escalations, grievance decay, and mediation outcomes (Direct, Mediation, Separation, Punishment).
- `InterpersonalConflictSystem` in pure Core manages conflict generation from templates (`Assets/StreamingAssets/Data/conflict_templates.json`), grievances, resolution mechanics, and census statistics (`InterpersonalConflictCensus`).
- `InterpersonalConflictHostSession` and `InterpersonalConflictSaveStore` provide host-level lifecycle, persistence (`interpersonal_conflict` section, `interpersonal_conflict_save.json`, section #259), and atomic state operations.
- `InterpersonalConflictDayOwner` in `Main.CampaignOwners.cs` advances daily grievance decay and conflict checks during phase-5 daily simulation, emitting `interpersonal_conflict_ticked`.
- `--interpersonal-conflict-selftest` validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Survivors/InterpersonalConflictSystem.cs`: Pure domain system, state model, disputes, grievances, resolutions, and `InterpersonalConflictCensus`.
  - `Assets/StreamingAssets/Data/conflict_templates.json`: Authoritative conflict templates catalog.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `interpersonal_conflict` registered under `survivors` domain with `ExpandedSurvivorLifecycleGroup` (file: `interpersonal_conflict_save.json`).
- **Godot Host (`src/`):**
  - `src/Host/InterpersonalConflictHostSession.cs`: Host session and checksummed save store.
  - `src/Main.InterpersonalConflict.cs`: Main partial with setup, save, reset, record, resolve, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAllDirect()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `InterpersonalConflictDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.InterpersonalConflict.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/Plan202InterpersonalConflictIntegrationTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/InterpersonalConflictSystemTests.cs`: 7/7 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/ArchitectureTestMapGateTests.cs`: PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --interpersonal-conflict-selftest`: 12/12 PASS

## Non-Goals

No duplicate social affinity matrices. Pure Core remains engine-free. Deterministic simulation.
