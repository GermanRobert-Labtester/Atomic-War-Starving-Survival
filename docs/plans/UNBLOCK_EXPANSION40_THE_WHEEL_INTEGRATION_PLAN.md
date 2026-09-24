# UNBLOCK — Expansion 40: The Wheel / Mechanical Power Driveline & Machine Tools

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-40-the-wheel-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `MechanicalPowerDrivelineEngine` (DEC-335) the live calculation authority
for rotational line shaft power transmission, friction drag, bearing wear,
spindle runout, machining tolerance evaluation, and millwright lubrication maintenance
while preserving existing single owners (Rule 5):

- `KineticStorageSystem` owns kinetic flywheel charging and surge release;
  `ShelterWorkshopSystem` owns recipe execution and workshop queues;
  `PrecisionMetrologySystem` owns calibration standards and master gauge blocks.
- `MechanicalDrivelineLedger` wraps `MechanicalPowerDrivelineEngine` in pure Core to manage
  line shaft branches, pillow block bearing wear, shaft alignment precision,
  machine tool runout drift, and lubricant reserve inventory.
- `MechanicalDrivelineHostSession` and `MechanicalDrivelineSaveStore` provide host-level
  lifecycle, persistence (`mechanical_driveline` section, `mechanical_driveline_save.json`),
  and atomic operations.
- `MechanicalDrivelineDayOwner` in `Main.CampaignOwners.cs` advances daily line shaft
  operating hours, monitors maintenance alerts, and updates alignment drift during phase-5
  daily simulation, emitting `mechanical_driveline_ticked`.
- `--mechanical-driveline-selftest` (alias `--the-wheel-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Shelter/MechanicalPowerDrivelineEngine.cs`: Pure static calculation engine.
  - `Assets/Ashfall.Core/Shelter/MechanicalDrivelineLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `mechanical_driveline` registered under `shelter` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/MechanicalDrivelineHostSession.cs`: Host session and checksummed save store.
  - `src/Main.MechanicalDriveline.cs`: Main partial with setup, save, flush, reset, power transmission, machining tolerance, millwright maintenance, lubricant restock, daily advance, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `MechanicalDrivelineDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.MechanicalDriveline.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/MechanicalPowerDrivelineEngineTests.cs`: PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/MechanicalDrivelineLedgerTests.cs`: 8/8 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1532/1532 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --mechanical-driveline-selftest`: 12/12 PASS
- `godot --headless --path . -- --the-wheel-selftest`: 12/12 PASS

## Non-Goals

No parallel energy/grid systems. No unseeded randomness.
No Unity dependencies or invocation.
