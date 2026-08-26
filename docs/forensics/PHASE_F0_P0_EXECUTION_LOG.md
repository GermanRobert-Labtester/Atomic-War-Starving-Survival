# Phase F0 + P0 + P1-1 Execution Log

**Date:** 2026-08-23  
**Status:** F0 complete, P0 complete, P1-1 slice 1 complete (20/20 systems)  

---

## F0-1 — Canonical Subsystem Registry ✅

**Deliverables:**
- `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.md`
- `docs/forensics/CANONICAL_SUBSYSTEM_REGISTRY.csv`

**Stats:**
- Total entries: 391
- Kinds: gameplay system (112), other (101), catalog (92), host session (48), demo/selftest (26), save store (12)

---

## F0-2 — Orphan Reclassification ✅

**Candidates evaluated:** 15  
**CORE_INTERNAL:** 14  
**TRUE_ORPHAN:** 1 (`PhantomMemorySystem`)

**Key finding:** Previous reports overstated orphan count by 14x. Most "orphans" are Core-internal collaborators with test coverage.

---

## F0-3 — Executable Baseline ✅

**Deliverable:** `docs/forensics/BASELINE_FORENSIC_VERIFICATION.md`

**Baseline:**
- Tests: 2554 passed, 0 failed
- Core build: 0 errors
- Godot build: 0 errors
- Data integrity: 0 errors
- Bridge: pass

---

## P0-1 — Survivor/Needs Characterization Tests ✅

**Deliverable:** `Ashfall.Core.Tests/SurvivorNeedsCharacterizationTests.cs`

**Tests added:** 15 (all pass)

---

## P0-2 — SurvivorsHostSession Authority Fix — SKIPPED

**Reason:** Code inspection revealed `SurvivorsHostSession` already delegates to Core. No duplicate state class exists.

**Actual H1 issue:** `HoldfastRuntimeSession` duplicates survival mechanics.

---

## P0-3 — Correctness-Grade Test Gaps ✅

**Deliverables:**
- `GhostTransmissionCatalogTests.cs` (3 tests)
- `OralLoreCatalogTests.cs` (3 tests)
- `RadioScriptbookCatalogTests.cs` (3 tests)

**Total:** 9/9 pass

---

## P1-1 — Main.ExpandedShelterSystems.cs Decomposition — COMPLETE ✅

**All 20 systems extracted:**

| # | HostSession | Status |
|---|-------------|--------|
| 1 | WaterTreatmentHostSession | ✅ IsDirty + Save |
| 2 | AirlockSecurityHostSession | ✅ IsDirty + Save |
| 3 | SurvivorRelationsHostSession | ✅ IsDirty + Save |
| 4 | RegionalTreatyHostSession | ✅ IsDirty + Save |
| 5 | VinylMoraleHostSession | ✅ IsDirty + Save |
| 6 | WildlifeTrappingHostSession | ✅ IsDirty + Save |
| 7 | ExcavationHostSession | ✅ IsDirty + Save |
| 8 | WaystationHostSession | ✅ IsDirty + Save |
| 9 | ApprenticeshipHostSession | ✅ IsDirty + Save |
| 10 | ShelterThermalHostSession | ✅ IsDirty + Save |
| 11 | ShelterScheduleHostSession | ✅ IsDirty + Save |
| 12 | SumpFloodingHostSession | ✅ IsDirty + Save |
| 13 | DecontaminationHostSession | ✅ IsDirty + Save |
| 14 | KitchenNutritionHostSession | ✅ IsDirty + Save |
| 15 | EquipmentConditionHostSession | ✅ IsDirty + Save |
| 16 | LibraryStudyHostSession | ✅ IsDirty + Save |
| 17 | ArchiveDeskHostSession | ✅ IsDirty + Save |
| 18 | ContractorRosterHostSession | ✅ IsDirty + Save |
| 19 | MentalHealthCrisisHostSession | ✅ IsDirty + Save |
| 20 | AutopsyHostSession | ✅ IsDirty + Save |

**Pattern applied:**
- Added `IsDirty`, `MarkDirty()`, `Save()` to each HostSession
- Updated Main wiring: `StateChanged += () => _xxx.MarkDirty()`
- Updated Main save: `_xxx?.Save()`

**Files modified:**
- 20 `src/Host/*HostSession.cs` files
- `src/Main.ExpandedShelterSystems.cs`

**Verification:**
- `dotnet build Ashfall.csproj`: 0 errors
- `dotnet test Ashfall.Core.Tests`: 2569/2569 passed
- `godot --headless -- --data-integrity-selftest`: 0 errors
- `godot --headless -- --bridge-selftest`: pass

---

## Current Test Count

**2569/2569** xUnit tests pass (up from 2554)

---

## Remaining Work

| Phase | Status | Notes |
|-------|--------|-------|
| P1-2 | ⏳ PENDING | Host hub integration tests |
| P1-3 | ⏳ PENDING | ExpansionHubSave stub disposition |
| P2-1 | ⏳ PENDING | True orphan disposition (1: PhantomMemorySystem) |
| P2-2 | ⏳ PENDING | Schema-version policy + staged migration |
| P2-3 | ⏳ PENDING | CatalogFileSystem direct tests |
| P3-1 | ⏳ PENDING | Re-audit 41 Main.cs-only systems |
| P3-2 | ⏳ PENDING | Aggregate catalog coverage |

---

## Next Action

**P1-2:** Add host-aware integration tests for InventoryHostSession, WorldHostSession, Phase0HostSession.
