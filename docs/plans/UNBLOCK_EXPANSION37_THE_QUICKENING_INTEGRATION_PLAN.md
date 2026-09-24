# UNBLOCK — Expansion 37: The Quickening / Antenatal & Neonatal Health

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-37-the-quickening-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `AntenatalMaternalHealthEngine` (DEC-332) the live calculation authority
for antenatal care, trimester progression, maternal physiological reserve management,
and neonatal birth resolution while preserving the existing single owners (Rule 5):

- `ChildDevelopmentSystem` and `GenerationalSystem` remain the sole authorities
  for registered children, childhood development milestones, education, and family lineage.
- `MedicalWardSystem` remains the clinical bed occupancy authority for supervised
  maternal and postpartum care.
- `SurvivorNeedsState` remains the authority for baseline survivor caloric demand and rest.
- `AntenatalMaternalCareLedger` wraps `AntenatalMaternalHealthEngine` in pure Core to
  manage active pregnancies, daily trimester progression, nutritional/rest deficit handling,
  clinical supervision tracking, postpartum recovery, and completed delivery records.
- `AntenatalMaternalHealthHostSession` and `AntenatalMaternalHealthSaveStore`
  provide host-level lifecycle, persistence (`antenatal_maternal_health` section,
  `antenatal_maternal_health_save.json`), and atomic operations.
- `AntenatalMaternalHealthDayOwner` in `Main.CampaignOwners.cs` advances active
  pregnancies and postpartum recoveries during phase-5 daily simulation and emits `antenatal_maternal_health_ticked`.
- `--antenatal-care-selftest` (alias `--the-quickening-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Survivors/AntenatalMaternalHealthEngine.cs`: Pure static math engine.
  - `Assets/Ashfall.Core/Survivors/AntenatalMaternalCareLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `antenatal_maternal_health` registered under `survivors` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/AntenatalMaternalHealthHostSession.cs`: Host session and checksummed save store.
  - `src/Main.AntenatalMaternalHealth.cs`: Main partial with setup, save, flush, reset, and domain methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `AntenatalMaternalHealthDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.AntenatalMaternalHealth.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/AntenatalMaternalHealthEngineTests.cs`: 5/5 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Survivors/AntenatalMaternalCareLedgerTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1508/1508 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --antenatal-care-selftest`: 12/12 PASS
- `bash scripts/ci/triad-drift-gate.sh`: GATE PASS (251 registered save sections)
- `python3 scripts/ci/generate-architecture-map.py --check`: 251 subsystems mapped (100%)
- `python3 scripts/ci/generate-save-store-matrix.py --check`: 253 save stores verified
- `python3 scripts/ci/generate-selftest-manifest.py --check`: 184 tests cataloged
- `python3 scripts/ci/generate-plan-integration-audit.py`: 49/49 plans INTEGRATED

## Non-Goals

No duplicate child development records or lineage trees. No duplicate clinical ward beds.
No unseeded randomness. No Unity dependencies or invocation.
