# UNBLOCK — Expansion 39: The Reagent / Chemical Synthesis Safety

**Status:** SEALED — user-authorized full host integration completed
**Claim:** `claim-unblock-expansion-39-the-reagent-2026-09-24`
**Scope rule:** no `PARTIAL` closeout. The signed pure engine is fully reachable
from campaign composition, daily simulation, persistence, host CLI diagnostic probe,
and focused runtime verification.

## Outcome

Make `ChemicalReagentSynthesisEngine` (DEC-334) the live calculation authority
for chemical reactor vessel safety envelopes, thermal/pressure runaway mitigation,
stoichiometric mass balance evaluation, reagent purity grading, catalyst degradation,
and acidic waste effluent neutralization while preserving existing single owners (Rule 5):

- `ChlorAlkaliSynthesisEngine`, `PlasticPyrolysisSystem`, `FischerTropschSynthesisEngine`,
  `BioFermentationEngine`, and `PharmaLabSystem` remain authorities for specific recipes,
  specialized plant equipment, or biological cultures.
- `ChemicalReagentLedger` wraps `ChemicalReagentSynthesisEngine` in pure Core to manage
  synthesis reactor vessels, exothermic cycle execution, thermal/pressure runaways,
  catalyst bed activity and regeneration, reagent stocks graded from Crude to AnalyticalPharma,
  and acidic waste neutralization via alkali buffer.
- `ChemicalReagentSynthesisHostSession` and `ChemicalReagentSynthesisSaveStore` provide host-level
  lifecycle, persistence (`chemical_reagent_synthesis` section, `chemical_reagent_synthesis_save.json`),
  and atomic operations.
- `ChemicalReagentSynthesisDayOwner` in `Main.CampaignOwners.cs` advances daily ambient thermal
  cooling, checks acidic waste safety thresholds, and updates safety alerts during phase-5
  daily simulation, emitting `chemical_reagent_synthesis_ticked`.
- `--chemical-reagent-selftest` (alias `--the-reagent-selftest`) validates 12 end-to-end engine invariants.

## Authority and Boundaries

- **Pure Domain (`Ashfall.Core`):**
  - `Assets/Ashfall.Core/Shelter/ChemicalReagentSynthesisEngine.cs`: Pure static calculation engine.
  - `Assets/Ashfall.Core/Shelter/ChemicalReagentLedger.cs`: Stateful domain ledger, state model, and census read model.
- **Save Registration:**
  - `Assets/Ashfall.Core/Save/SaveSectionRegistry.cs`: Section `chemical_reagent_synthesis` registered under `shelter` domain with `ExpandedShelterLifecycleGroup`.
- **Godot Host (`src/`):**
  - `src/Host/ChemicalReagentSynthesisHostSession.cs`: Host session and checksummed save store.
  - `src/Main.ChemicalReagentSynthesis.cs`: Main partial with setup, save, flush, reset, cycle execution, mass balance, waste neutralization, alkali restock, catalyst regeneration, cooling adjustment, and census methods.
  - `src/Main.SaveOrchestrator.cs`: Wired into `RestoreAllSubsystemsFromDisk()` and `SaveAll()`.
  - `src/Main.Lifecycle.cs`: Wired into `ResetLateWaveIntegrationSessions()`.
  - `src/Main.CampaignOwners.cs`: Phase-5 `ChemicalReagentSynthesisDayOwner` with `IPreDaySnapshotRestore`.
  - `src/Host/HostCli.ChemicalReagentSynthesis.cs`: 12-check diagnostic selftest probe.
  - `src/Host/HostCli.cs`: Host CLI action and argument parsing.
  - `src/Main.Application.cs`: Host CLI dispatch wiring.

## Focused Verification

- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ChemicalReagentSynthesisEngineTests.cs`: 5/5 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Shelter/ChemicalReagentLedgerTests.cs`: 7/7 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Tooling/DayEventParitySourceGateTests.cs`: 2/2 PASS
- `bash scripts/run_test.sh Ashfall.Core.Tests/Save/ComprehensiveSaveStoreCorruptionAndMigrationTests.cs`: 1520/1520 PASS
- `dotnet build Ashfall.csproj`: 0 Warnings, 0 Errors
- `godot --headless --path . -- --chemical-reagent-selftest`: 12/12 PASS
- `bash scripts/ci/triad-drift-gate.sh`: GATE PASS (253 registered save sections, 0 defects)
- `python3 scripts/ci/generate-architecture-map.py --check`: 253 subsystems mapped (100%)
- `python3 scripts/ci/generate-save-store-matrix.py --check`: 255 save stores verified
- `python3 scripts/ci/generate-selftest-manifest.py --check`: 186 tests cataloged
- `python3 scripts/ci/generate-plan-integration-audit.py --check`: 51/51 plans INTEGRATED
- `python3 scripts/ci/agent-fast-verify.py`: 10/10 gates PASS

## Non-Goals

No parallel chemical machinery stores. No unseeded randomness.
No Unity dependencies or invocation.
