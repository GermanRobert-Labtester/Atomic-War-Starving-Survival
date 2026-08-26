# Phase F0 + P0 + P1 + P2 + P3 Execution Log

**Date:** 2026-08-26  
**Status:** F0 complete, P0 complete, P1 complete, P2 complete, P3 complete  

---

## F0-1 — Canonical Subsystem Registry ✅

**Deliverables:**
- `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md`
- `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.csv`

**Stats:**
- Total unique entries: 578
- Reconciled from 6 forensic surveys against live source code in `Assets/Ashfall.Core/` and `src/`

---

## F0-2 — Orphan Reclassification ✅

**Candidates evaluated:** 15  
**DIRECT_HOSTED:** 2 (`CaregivingSystem`, `PhantomMemoryEngine`)  
**INDIRECT_HOSTED:** 1 (`MaritimeDiveSystem`)  
**CORE_INTERNAL:** 12  
**TRUE_ORPHAN:** 0  

**Key finding:** 0 true unhosted orphans. All 15 candidates are directly hosted or integrated as Core-internal collaborators with test coverage.

---

## F0-3 — Executable Baseline ✅

**Deliverable:** `docs/forensics/BASELINE_FORENSIC_VERIFICATION.md`

**Baseline:**
- Tests: 3,242 passed, 0 failed, 0 skipped
- Core build: 0 errors
- Godot build: 0 errors
- Data integrity: 0 errors (129 catalogs, 4,793 authored IDs)
- Bridge: pass (shim removed)
- Triad drift gate: pass (66 setups, 61 saves, 60 sections)

---

## P0-1 — Survivor/Needs Characterization Tests ✅

**Deliverables:**
- `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs` (15 tests)
- `Ashfall.Core.Tests/NeedsRadiationSaveRoundTripTests.cs` (6 tests)

---

## P0-2 — SurvivorsHostSession Authority Fix ✅

**Status:** Complete. `SurvivorsHostSession` projects Core `NeedsSystem` and `RadiationSystem` simulation truth; no duplicate mutable state authority exists.

---

## P0-3 — Correctness-Grade Test Gaps ✅

**Deliverables:**
- `GhostTransmissionCatalogTests.cs` (3 tests)
- `OralLoreCatalogTests.cs` (3 tests)
- `RadioScriptbookCatalogTests.cs` (3 tests)
- `ExpandedShelterSavePersistenceTests.cs` (2 tests)

---

## P1-1 — Main.ExpandedShelterSystems.cs Decomposition ✅

**All 20 Expanded Shelter HostSessions own their dirty flag + save logic:**

| HostSession | Status |
|-------------|--------|
| WaterTreatment, AirlockSecurity, RegionalTreaty, VinylMorale, WildlifeTrapping | ✅ IsDirty + Save |
| Excavation, Waystation, SurvivorRelations | ✅ IsDirty + Save |
| Apprenticeship, ShelterThermal, ShelterSchedule | ✅ IsDirty + Save |
| SumpFlooding, Decontamination, KitchenNutrition, EquipmentCondition | ✅ IsDirty + Save |
| LibraryStudy, ArchiveDesk, ContractorRoster, MentalHealthCrisis, Autopsy, Caregiving | ✅ IsDirty + Save |

**Pattern applied:**
- Added `IsDirty`, `MarkDirty()`, `Save()` overrides to each HostSession
- Updated Main wiring: `StateChanged += () => _xxx.MarkDirty()`
- Updated Main save: `_xxx?.Save()`
- Save stores route paths via `SaveSlotRoot.Resolve(FileName)`
- Added `SectionName`, `TryCaptureDirect`, and `TryRestoreDirect` across save stores

---

## P1-2 — Host Hub Integration Tests ✅

**Selftests verified in Godot headless:**
- `--inventory-save-selftest`: PASS
- `--world-selftest`: PASS
- `--phase0-selftest`: PASS
- `--shelter-operations-selftest`: PASS
- `--shelter-hazard-loop-selftest`: PASS

---

## P1-3 — ExpansionHubSave Phase 11 Stubs ✅

**Finding:** `ExpansionHubSaveCodec` has full v1→v4 migration and checksum validation. All save paths are explicit and verified.

---

## P2-1 — True Orphan Disposition ✅

**Finding:** 0 true orphans after F0-2 reachability verification.

---

## P2-2 — Schema-Version Policy + Staged Migration ✅

**Deliverables:**
- `docs/forensics/SCHEMA_VERSION_POLICY.md`
- `scripts/migrate_schema_version.py` (`--check` and `--write` modes)
- Pilot manifest: `docs/forensics/schema_migration_manifest.json`

---

## P2-3 — CatalogFileSystem Direct Tests ✅

**Deliverable:** `Ashfall.Core.Tests/CatalogFileSystemTests.cs` (8 tests, all pass)

---

## P3-1 — Main.cs-Only Systems Audit ✅

**Deliverable:** `docs/forensics/MAIN_CS_ONLY_SYSTEMS_AUDIT.md` (documented ownership, shared dependencies, and clean triad coordination)

---

## P3-2 — Aggregate Catalog Coverage ✅

**Deliverables:**
- `Ashfall.Core.Tests/CatalogTestBase.cs` (shared parameterized assertion base)
- Consolidated catalog test suites inheriting `CatalogTestBase`

---

## Final Verification Gate Summary

| Gate | Command | Result |
|------|---------|--------|
| Unit Tests | `dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj` | **3,242 / 3,242 passed (0 failed)** |
| Core Build | `dotnet build Ashfall.Core/Ashfall.Core.csproj` | **0 errors** |
| Godot Host Build | `dotnet build Ashfall.csproj` | **0 errors** |
| Data Integrity | `godot --headless --path . -- --data-integrity-selftest` | **PASS (0 findings, 129 catalogs)** |
| Bridge Gate | `godot --headless --path . -- --bridge-selftest` | **PASS** |
| Triad Drift Gate | `./scripts/ci/triad-drift-gate.sh` | **GATE PASS (66 Setups, 61 Saves, 60 Sections)** |
