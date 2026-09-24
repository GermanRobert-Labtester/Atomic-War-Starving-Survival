# UNBLOCK — Expansion 35: The Habit / Chemical Dependency Taper & Withdrawal

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-35-the-habit-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `DependencyTaperWithdrawalEngine` (DEC-88) the live calculation authority
for clinical dependency taper programs and withdrawal management while preserving
the existing single owners (Rule 5):

- `ChemicalDependencySystem` remains the sole authority for survivor addiction
  levels, substance cravings, and tolerance buildup.
- `MedicalWardSystem` remains the clinical bed occupancy authority for supervised
  taper protocols.
- `SurvivorNeedsState` remains the authority for survivor biological needs and fatigue.
- `DependencyTaperLedger` wraps `DependencyTaperWithdrawalEngine` in pure Core to
  manage active taper programs, daily step-downs, substitute medicine stocks,
  care posture recommendation, and graduation records.
- `DependencyTaperWithdrawalHostSession` and `DependencyTaperWithdrawalSaveStore`
  provide host-level lifecycle, persistence (`dependency_taper_withdrawal` section,
  `dependency_taper_withdrawal_save.json`), and atomic operations.
- `DependencyTaperWithdrawalDayOwner` in `Main.CampaignOwners.cs` advances active
  taper programs during phase-5 daily simulation and emits `dependency_taper_ticked`.
- `--dependency-taper-selftest` validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Medical/DependencyTaperWithdrawalEngine.cs`: Pure static math engine.
  - `Assets/Ashfall.Core/Medical/DependencyTaperLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `dependency_taper_withdrawal` registered under `medical` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/DependencyTaperWithdrawalHostSession.cs`: Host session and checksummed save store.
  - `src/Main.DependencyTaperWithdrawal.cs`: Main partial with setup, save, flush, reset, and domain methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `DependencyTaperWithdrawalDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.DependencyTaperWithdrawal.cs`: 12-check diagnostic selftest probe.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/DependencyTaperWithdrawalEngineTests.cs`: 5/5 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/DependencyTaperLedgerTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --dependency-taper-selftest`: 12/12 PASS
- `bash scripts/ci/triad-drift-gate.sh`: GATE PASS (250 registered save sections)
- `python3 scripts/ci/generate-architecture-map.py --check`: 250 subsystems mapped (100%)
- `python3 scripts/ci/generate-save-store-matrix.py --check`: 252 save stores verified
- `python3 scripts/ci/generate-selftest-manifest.py --check`: 183 tests cataloged
- `python3 scripts/ci/generate-plan-integration-audit.py`: 47/47 plans INTEGRATED

## Non-Goals

No duplicate addiction ledger. No duplicate bed registry. No unseeded randomness.
No Unity dependencies or invocation.
