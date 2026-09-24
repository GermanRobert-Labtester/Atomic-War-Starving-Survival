# UNBLOCK — Expansion 38: The Ward / Clinical Triage & Sterile Supply

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-38-the-ward-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `ClinicalWardTriageEngine` (DEC-333) the live calculation authority
for clinical triage routing, surgical preflight readiness checks, sterile supply depletion/autoclave turnover,
and nosocomial infection risk modeling while preserving the existing single owners (Rule 5):

- `MedicalWardSystem` and `AdvancedSurgicalWardSystem` remain the authorities for
  clinical bed occupancy, physical inpatient admissions, and long-term procedure progression.
- `InventorySystem` remains the authority for raw medical consumables and fabric.
- `ClinicalWardLedger` wraps `ClinicalWardTriageEngine` in pure Core to manage
  triage admission prioritization (Immediate, Delayed, Minimal, Expectant), contagious patient isolation routing,
  surgical preflight readiness gating, sterile supply inventory, autoclave sterilization cycles, and nosocomial infection tracking.
- `ClinicalWardTriageHostSession` and `ClinicalWardTriageSaveStore` provide host-level
  lifecycle, persistence (`clinical_ward_triage` section, `clinical_ward_triage_save.json`), and atomic operations.
- `ClinicalWardTriageDayOwner` in `Main.CampaignOwners.cs` advances inpatient clinical
  tenure, daily sterile supply baseline consumption, and nosocomial risk calculations during phase-5 daily simulation and emits `clinical_ward_triage_ticked`.
- `--clinical-ward-selftest` (alias `--the-ward-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Medical/ClinicalWardTriageEngine.cs`: Pure static calculation engine.
  - `Assets/Ashfall.Core/Medical/ClinicalWardLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `clinical_ward_triage` registered under `medical` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/ClinicalWardTriageHostSession.cs`: Host session and checksummed save store.
  - `src/Main.ClinicalWardTriage.cs`: Main partial with setup, save, flush, reset, triage, surgery preflight, discharge, restock, autoclave, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `ClinicalWardTriageDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.ClinicalWardTriage.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/ClinicalWardTriageEngineTests.cs`: 5/5 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Medical/ClinicalWardLedgerTests.cs`: 6/6 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1514/1514 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --clinical-ward-selftest`: 12/12 PASS
- `bash scripts/ci/triad-drift-gate.sh`: GATE PASS (252 registered save sections, 0 defects)
- `python3 scripts/ci/generate-architecture-map.py --check`: 252 subsystems mapped (100%)
- `python3 scripts/ci/generate-save-store-matrix.py --check`: 254 save stores verified
- `python3 scripts/ci/generate-selftest-manifest.py --check`: 185 tests cataloged
- `python3 scripts/ci/generate-plan-integration-audit.py --check`: 50/50 plans INTEGRATED

## Non-Goals

No duplicate patient entities or parallel medical bed registries. No unseeded randomness.
No Unity dependencies or invocation.
