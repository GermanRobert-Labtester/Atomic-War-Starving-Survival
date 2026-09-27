# FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED

> **STATUS: FULLY INTEGRATED — FULLY INTEGRATED — FULLY INTEGRATED**

# Plan 253: Voluntary Register — Full Host Integration

**Status:** APPROVED BY USER — **FULLY INTEGRATED**
**Date:** 2026-09-26
**Claim:** `claim-triple-j-voluntaryregister-worldevolution-2026-09-26`

## Scope

`VoluntaryRegisterSystem` (148 lines) was a stateful Core authority with
`CaptureState`/`RestoreState` and **zero** host references. It manages high-dose
surface-work volunteer signatures: who signed away the front of the day, for
which task, and how much dose they banked on completion.

## What was integrated

- **Host session** — `src/Host/VoluntaryRegisterHostSession.cs`
  - `VoluntaryRegisterHostSession` (host lifecycle bind, event forwarding to `RaiseStateChanged`)
  - `VoluntaryRegisterSaveStore` (`SaveStoreHub.Checksummed<VoluntaryRegisterSystemState>`)
- **Host wiring** — `src/Main.VoluntaryRegister.cs`
  - `SetupVoluntaryRegister` / `SaveVoluntaryRegister` / `TickVoluntaryRegister`
  - `FlushVoluntaryRegisterIfDirty` / `ResetVoluntaryRegister`
- **Day owner** — `src/Main.CampaignOwners.cs`
  - `VoluntaryRegisterDayOwner` (ownerId `voluntary_register`, phase 5) with
    `IPreDaySnapshotRestore` pre-day snapshot rollback
  - emits `voluntary_register_ticked` heartbeat
- **Registry**
  - `SaveSectionRegistry`: section `voluntary_register`, `voluntary_register_save.json`
  - `DayEventVocabulary`: `voluntary_register_ticked` → `SemanticKind.Heartbeat`
  - `docs/campaign/EVENT_SEMANTIC_PARITY_MATRIX.md`: heartbeat row
  - `scripts/ci/generate-architecture-map.py`: graph entry
- **Lifecycle** — `src/Main.SaveOrchestrator.cs` (setup + save),
  `src/Main.Lifecycle.cs` (reset)
- **CLI probe** — `--voluntary-register-selftest` / `--volunteers-selftest`
  - registered in `Assets/Ashfall.Core/HostCliRegistry.cs`, `src/Host/HostCli.cs`,
    dispatched in `src/Main.Application.cs`, implemented in
    `src/Host/HostCli.VoluntaryRegister.cs`
- **Tests** — `Ashfall.Core.Tests/PlanTriplePackageJCoreTests.cs` (consolidated suite)

## Rule-5 authority boundary

No parallel ledger. The Core system remains the sole owner of volunteer entries,
exactly-once task completion and banked dose. The host supplies only the day,
dirty tracking and persistence. Dose banking to the campaign dose ledger is left
to the existing dose owner — no second dose store was created, because no dose
owner was found bound in the host at integration time and inventing one would
have been a duplicate authority.

## Verification

- Host build: **0 errors / 0 warnings in owned files**
- Core test build: **0 errors / 0 warnings**
- `--voluntary-register-selftest`: **11/11 PASS**
- `PlanTriplePackageJCoreTests`: **9/9 PASS** (consolidated with world evolution)
- Adjacent gates: `SaveSectionRegistryTests`, `PersistentFilenameRegistryGateTests`,
  `DayEventVocabularyTests`, `DayEventParitySourceGateTests`,
  `HostCliActionParityGateTests`, `MainTriadDriftGateTests`,
  `ComprehensiveSaveStoreCorruptionAndMigrationTests` — **1929/1929 PASS**
- Generators: selftest manifest **312 tests** (310 headless), architecture map
  **314 subsystems**
