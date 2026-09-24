# UNBLOCK — Expansion 41: The Quiet / Sleep Quality, Soundproofing & Shelter Crowding

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-41-the-quiet-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `SleepAcousticRestEngine` (DEC-336) the live calculation authority
for shelter sleeping quarters, acoustic soundproofing attenuation, bunk decibels,
crowding density area penalties, quiet hours compliance, sensory relief kit deployment,
and restorative fatigue/morale sleep modifiers while preserving existing single owners (Rule 5):

- `ShelterNoiseSystem` owns broad ambient noise sources and propagation;
  `NeedsSystem` owns survivor Fatigue and Numbness;
  `ShelterAssignmentSystem` owns survivor bedroom assignments.
- `SleepAcousticLedger` wraps `SleepAcousticRestEngine` in pure Core to manage
  sleeping quarters, wall/door acoustic attenuation, quiet hours schedules,
  sleep disturbance alerts, and sensory relief kit inventory.
- `SleepAcousticRestHostSession` and `SleepAcousticRestSaveStore` provide host-level
  lifecycle, persistence (`sleep_acoustic_rest` section, `sleep_acoustic_rest_save.json`),
  and atomic operations.
- `SleepAcousticRestDayOwner` in `Main.CampaignOwners.cs` advances daily nocturnal
  rest evaluations across quarters, records quiet hours compliance, and tracks
  restorative sleep bands during phase-5 daily simulation, emitting `sleep_acoustic_rest_ticked`.
- `--sleep-acoustic-selftest` (alias `--the-quiet-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Needs/SleepAcousticRestEngine.cs`: Pure static calculation engine.
  - `Assets/Ashfall.Core/Needs/SleepAcousticLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `sleep_acoustic_rest` registered under `needs` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/SleepAcousticRestHostSession.cs`: Host session and checksummed save store.
  - `src/Main.SleepAcousticRest.cs`: Main partial with setup, save, flush, reset, sleep evaluation, sensory kit installation, sensory kit restock, soundproofing updates, quiet hours configuration, daily advance, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `SleepAcousticRestDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.SleepAcousticRest.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Needs/SleepAcousticRestEngineTests.cs`: PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Needs/SleepAcousticLedgerTests.cs`: 8/8 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1532/1532 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --sleep-acoustic-selftest`: 12/12 PASS
- `godot --headless --path . -- --the-quiet-selftest`: 12/12 PASS

## Non-Goals

No parallel fatigue/need systems. No unseeded randomness.
No Unity dependencies or invocation.
