# Deep Integration Plan: 254-Subsystem Forensic Actionables

**Date:** 2026-08-23  
**Input:** `docs/forensics/DEEP_ANALYSIS_254_SUBSYSTEMS.md`  
**Mode:** Evidence-first integration planning — no code modified  

---

## 1. Dependency Graph & Blast Radius

### 1.1 Critical Path

```
P0-1: Fix SurvivorsHostSession H1 duplication
    ├── Blast radius: src/Host/SurvivorsHostSession.cs (384 lines)
    ├── Depends on: NeedsSystem, RadiationSystem, SurvivorNeedsState
    └── Unlocks: Clean host-core separation for all survivor panels

P0-2: Add schema_version to remaining JSON files
    ├── Blast radius: 317 JSON files in Assets/StreamingAssets/Data/
    ├── Depends on: CatalogIntegrityValidator, all *CatalogLoader.cs files
    └── Unlocks: Safe schema migration path

P0-3: Add tests for 3 zero-test narrative catalogs
    ├── Blast radius: GhostTransmissionCatalog, OralLoreCatalog, RadioScriptbookCatalog
    ├── Depends on: CatalogLocator, IFileIO, IJsonSerializer
    └── Unlocks: Regression safety for narrative content
```

### 1.2 Near-Term Path

```
P1-1: Extract Main.ExpandedShelterSystems.cs god object
    ├── Blast radius: src/Main.ExpandedShelterSystems.cs (699 lines)
    ├── Depends on: P0-1 (clean SurvivorsHostSession pattern)
    └── Unlocks: 20 HostSession classes become self-contained

P1-2: Wire 15 orphan Core systems to host sessions
    ├── Blast radius: 15 Core systems + new HostSession files
    ├── Depends on: P1-1 (established pattern)
    └── Unlocks: Dead code becomes live gameplay

P1-3: Add tests for central hub sessions
    ├── Blast radius: InventoryHostSession, WorldHostSession, Phase0HostSession
    ├── Depends on: P1-1 (extracted sessions are testable)
    └── Unlocks: Regression safety for critical paths
```

### 1.3 Strategic Path

```
P2-1: Audit 41 Main.cs-only systems for HostSession extraction
    ├── Blast radius: src/Main.*.cs partials
    ├── Depends on: P1-1 (pattern established)
    └── Unlocks: Full host-core separation

P2-2: Add CatalogFileSystem direct tests
    ├── Blast radius: Ashfall.Core.Tests/CatalogFileSystemTests.cs
    ├── Depends on: P0-2 (schema_version work complete)
    └── Unlocks: Infrastructure test coverage
```

---

## 2. Phase-by-Phase Integration Plan

### P0-1: Fix SurvivorsHostSession H1 Duplication

**Problem:** `SurvivorsHostSession` duplicates Core survival mechanics:
- Defines its own `SurvivorNeedsState` class (Core already has this)
- Directly manages needs/radiation state instead of delegating to `NeedsSystem`/`RadiationSystem`
- `CaptureSave()`/`RestoreSave()` re-implements state serialization

**Evidence:**
```csharp
// Host has its own copy:
public class SurvivorNeedsState { ... }  // DUPLICATE of Ashfall.Core.Survivors.SurvivorNeedsState

// Host directly manipulates state:
public string TickHour(float gameHours = 1f)
{
    Needs.Tick(gameHours);      // Delegates to Core ✓
    Radiation.Tick(gameHours);  // Delegates to Core ✓
    // BUT: RosterState is host-owned list of host-defined SurvivorNeedsState
}

// Host re-implements save:
public SurvivorsSaveState CaptureSave()
{
    // Manually maps host state to save DTO
}
```

**Fix Strategy:**
1. Remove `SurvivorNeedsState` class from host — use `Ashfall.Core.Survivors.SurvivorNeedsState`
2. Make `RosterState` a read-only view over Core `NeedsSystem` registrations
3. `CaptureSave()` delegates to `NeedsSystem` + `RadiationSystem` state
4. `RestoreSave()` re-registers survivors into Core systems

**Files to touch:**
- `src/Host/SurvivorsHostSession.cs` (384 lines → ~200 lines)
- `src/Host/SurvivorsHostSession.cs` — remove duplicate state class

**Blast radius:** LOW — only this file and its panel (`SurvivorsPanel`)

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj --filter "FullyQualifiedName~SurvivorsHostSession"
godot --headless -- --survivors-selftest
```

---

### P0-2: Add schema_version to Remaining JSON Files

**Problem:** 317 of 318 JSON files lack `schema_version`. Only `questline_master.json` has it (v2).

**Evidence:**
```
Total JSON files: 318
Files with schema_version: 1
Files without: 317
```

**Fix Strategy:**
1. Wrap bare-list files with `{"schema_version": 1, "key": [...]}` (Task 4b pattern)
2. Add `schema_version` to object-root files as a top-level field
3. Update loaders to prefer `schema_version`-aware deserialization

**Files to touch:**
- `Assets/StreamingAssets/Data/*.json` (317 files)
- All `*CatalogLoader.cs` files (loaders)
- `CatalogIntegrityValidator.cs` (validate schema_version presence)

**Blast radius:** HIGH — touches every catalog loader and every JSON data file

**Implementation order:**
1. Core framework files first (`CatalogIntegrityValidator`, base loader helpers)
2. Wrapper-first loaders (12 already done in Task 4b)
3. Remaining ~50 expansion-specific files
4. Bulk sweep for remaining files

**Verification:**
```bash
godot --headless -- --data-integrity-selftest  # Must pass with 0 errors
dotnet test Ashfall.Core.Tests --filter "FullyQualifiedName~Catalog"
```

---

### P0-3: Add Tests for 3 Zero-Test Narrative Catalogs

**Problem:** 3 content catalogs have 0 tests:
- `GhostTransmissionCatalog`
- `OralLoreCatalog`
- `RadioScriptbookCatalog`

**Fix Strategy:**
1. Create `*CatalogTests.cs` for each following existing pattern
2. Test: load from data dir, verify count, verify IDs resolve, verify no duplicates

**Files to create:**
- `Ashfall.Core.Tests/GhostTransmissionCatalogTests.cs`
- `Ashfall.Core.Tests/OralLoreCatalogTests.cs`
- `Ashfall.Core.Tests/RadioScriptbookCatalogTests.cs`

**Blast radius:** LOW — new test files only

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
```

---

### P1-1: Extract Main.ExpandedShelterSystems.cs God Object

**Problem:** `Main.ExpandedShelterSystems.cs` is 699 lines with:
- 20 HostSession fields
- 20 UI Panel fields
- 20 dirty flag fields
- 20 SetupXxx() methods
- 20 SaveXxx() methods
- 1 TickAll() method
- 1 OpenPanel() switch

**Evidence:**
```csharp
// Current: 699-line monolith
private WaterTreatmentHostSession _waterTreatment = null!;
private WaterTreatmentPanel _waterTreatmentPanel = null!;
private bool _waterTreatmentDirty;
// ... repeated 20x

private void SetupWaterTreatment() { ... }
private void SaveWaterTreatment() { ... }
// ... repeated 20x
```

**Fix Strategy:**
Extract each system into a self-contained `HostSession` that owns:
- Its Core system instance
- Its save/load logic
- Its dirty flag
- Its UI panel binding
- Its `TickDay()` method

**Pattern (from existing clean sessions):**
```csharp
// BEFORE: Main.ExpandedShelterSystems.cs owns everything
private void SetupWaterTreatment() { ... }
private void SaveWaterTreatment() { ... }

// AFTER: WaterTreatmentHostSession is self-contained
public sealed class WaterTreatmentHostSession
{
    public WaterTreatmentSystem System { get; }
    public WaterTreatmentPanel Panel { get; }
    public bool IsDirty { get; private set; }
    
    public WaterTreatmentHostSession(string dataDir) { ... }
    public void TickDay(int day) { ... }
    public void Save() { ... }
    public void BindPanel(WaterTreatmentPanel panel) { ... }
}
```

**Implementation order (dependency-aware):**
1. `WaterTreatmentHostSession` (no deps on other expanded systems)
2. `AirlockSecurityHostSession` (no deps)
3. `SurvivorRelationsHostSession` (needs `_survivorRelationsCore` — already extracted in some places)
4. `RegionalTreatyHostSession` (no deps)
5. `VinylMoraleHostSession` (no deps)
6. `WildlifeTrappingHostSession` (no deps)
7. `ExcavationHostSession` (no deps)
8. `ApprenticeshipHostSession` (needs `_expandedShelterRoster`, `_survivorRelationsCore`)
9. `ShelterThermalHostSession` (needs `_shelterAssignment`)
10. `ShelterScheduleHostSession` (needs PowerGridSystem)
11. `AutopsyHostSession` (complex: needs Inventory, Radiation, Ventilation, Research, MedicalWard)
12. `WaystationHostSession` (no deps)
13. `SumpFloodingHostSession` (needs WeatherSystem, PowerGridSystem, DeepFreeze)
14. `DecontaminationHostSession` (needs Inventory, Radiation, AirlockSecurity, StartingLevel)
15. `KitchenNutritionHostSession` (needs Inventory, NeedsSystem)
16. `EquipmentConditionHostSession` (needs Inventory, CraftingSystem)
17. `LibraryStudyHostSession` (needs SkillProgression, Research, Journal, Roster)
18. `ArchiveDeskHostSession` (needs Journal, KnowledgeBase, Inventory, Roster)
19. `ContractorRosterHostSession` (needs Inventory, Roster, ExpeditionSystem)
20. `MentalHealthCrisisHostSession` (needs NeedsSystem, MedicalWard, ChemicalDependency, Roster)

**Files to touch:**
- `src/Main.ExpandedShelterSystems.cs` (reduce from 699 → ~50 lines of orchestration)
- 20 `Host/*HostSession.cs` files (expand with self-contained save/load/tick)
- 12 `UI/*Panel.cs` files (bind to self-contained sessions)

**Blast radius:** MEDIUM — touches 33+ files but each change is isolated

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless -- --data-integrity-selftest
# Manual: verify each panel opens and saves correctly
```

---

### P1-2: Wire 15 Orphan Core Systems to Host Sessions

**Problem:** 15 Core systems have no Godot host session. 12 are truly dead code (no external references), 3 are Core-internal utilities.

**Classification of orphans:**

| Category | Count | Systems | Action |
|----------|-------|---------|--------|
| **Dead code** | 12 | Caregiving, ExpeditionVehicle, IdeologicalFriction, Leadership, PhantomMemory, RationConflict, MaritimeDive, OrbitalHarrowTelemetry, PharmaLab, TraumaBond, WeatherStation, WorkshopReverseEngineering | Create thin HostSession OR remove if truly unused |
| **Core-internal utility** | 3 | BallisticsSystem, SkillAtrophySystem, WeaponConditionSystem | No host session needed — pure Core dependencies |

**Fix Strategy for dead-code orphans:**
1. For each, create a minimal `XxxHostSession` following the pattern:
   ```csharp
   public sealed class XxxHostSession
   {
       public XxxSystem System { get; }
       public event Action StateChanged;
       
       public XxxHostSession() { System = new XxxSystem(...); }
       public void TickDay(int day) { System.Tick(day); }
       public XxxSaveState CaptureState() => System.CaptureState();
       public void RestoreState(XxxSaveState state) => System.RestoreState(state);
   }
   ```
2. Wire into `Main.cs` or relevant expansion host session
3. Add save/load to appropriate save store

**Files to create:**
- `src/Host/CaregivingHostSession.cs`
- `src/Host/ExpeditionVehicleHostSession.cs`
- `src/Host/IdeologicalFrictionHostSession.cs`
- `src/Host/LeadershipHostSession.cs`
- `src/Host/PhantomMemoryHostSession.cs`
- `src/Host/RationConflictHostSession.cs`
- `src/Host/MaritimeDiveHostSession.cs`
- `src/Host/OrbitalHarrowTelemetryHostSession.cs`
- `src/Host/PharmaLabHostSession.cs`
- `src/Host/TraumaBondHostSession.cs`
- `src/Host/WeatherStationHostSession.cs`
- `src/Host/WorkshopReverseEngineeringHostSession.cs`

**Files to touch:**
- `src/Main.Survivors.cs` (Caregiving, TraumaBond, Leadership, RationConflict, SkillAtrophy)
- `src/Main.Expeditions.cs` (ExpeditionVehicle, MaritimeDive)
- `src/Main.YearOfAsh.cs` (OrbitalHarrowTelemetry, PharmaLab, WeatherStation)
- `src/Main.ExpandedShelterSystems.cs` (WorkshopReverseEngineering)
- Various `*SaveStore.cs` files (add new state fields)

**Blast radius:** MEDIUM — 12 new files + 5 modified files

**Verification:**
```bash
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
godot --headless -- --data-integrity-selftest
```

---

### P1-3: Add Tests for Central Hub Sessions

**Problem:** 3 central hub sessions have 0 tests:
- `InventoryHostSession` — most touched system
- `WorldHostSession` — world-state wiring
- `Phase0HostSession` — expansion orchestration

**Fix Strategy:**
1. Create integration tests that:
   - Construct the host session
   - Verify it wires Core systems correctly
   - Verify state changes propagate to UI events
   - Verify save/load round-trip

**Files to create:**
- `Ashfall.Core.Tests/InventoryHostSessionTests.cs`
- `Ashfall.Core.Tests/WorldHostSessionTests.cs`
- `Ashfall.Core.Tests/Phase0HostSessionTests.cs`

**Blast radius:** LOW — new test files only

---

### P2-1: Audit 41 Main.cs-Only Systems for HostSession Extraction

**Problem:** 41 systems are wired only through `Main.cs` partials with no dedicated `HostSession`.

**Audit Strategy:**
1. For each system, determine if it needs a dedicated session:
   - Does it have UI? → YES, extract
   - Is it only ticked? → Maybe keep in Main.cs
   - Does it have save state? → YES, extract
2. Prioritize by: save state > UI > tick-only

**Implementation:**
- Extract highest-priority systems first (those with save state + UI)
- Leave tick-only systems in Main.cs if they have no independent lifecycle

**Blast radius:** HIGH — touches 41 systems across multiple Main partials

---

### P2-2: Add CatalogFileSystem Direct Tests

**Problem:** `CatalogFileSystem` has 0 direct tests.

**Fix Strategy:**
1. Create `CatalogFileSystemTests.cs` testing:
   - File discovery
   - Path resolution
   - Error handling for missing files
   - schema_version detection

**Files to create:**
- `Ashfall.Core.Tests/CatalogFileSystemTests.cs`

---

## 3. Cross-Cutting Concerns

### 3.1 schema_version Migration Path

| Phase | Files | Strategy |
|-------|-------|----------|
| P0-2a | Core framework | Update `CatalogIntegrityValidator` to require `schema_version` |
| P0-2b | Already-wrapped (12) | Verify loaders handle wrapper-first + bare-list fallback |
| P0-2c | Expansion-specific (~50) | Wrap with `schema_version: 1` |
| P0-2d | Remaining (~255) | Bulk wrap + loader updates |

### 3.2 Save Compatibility

| System | Current State | Action |
|--------|---------------|--------|
| `ExpansionHubSave` | v1-v4 migration complete | No action needed |
| `NeedsSystem` | No save state (stateless) | Host session owns save |
| `SurvivorsHostSession` | Custom save format | Align with Core state after H1 fix |
| 5 Godot save stores | Checksum added | Already fixed |

### 3.3 Determinism

| Concern | Status |
|---------|--------|
| `System.Random` leaks | 0 found |
| `ISeededRng` usage | Consistent in Core |
| `Guid.NewGuid()` | Documented in ProceduralItemInstance |
| Save checksums | All stores ship checksummed envelopes |

---

## 4. Implementation Order (Dependency-Sorted)

| Order | Phase | Action | Blast Radius | Risk |
|-------|-------|--------|--------------|------|
| 1 | P0-3 | Add tests for 3 zero-test catalogs | LOW | LOW |
| 2 | P0-2 | Add schema_version to JSON files | HIGH | MEDIUM |
| 3 | P0-1 | Fix SurvivorsHostSession H1 | LOW | MEDIUM |
| 4 | P1-1 | Extract Main.ExpandedShelterSystems.cs | MEDIUM | HIGH |
| 5 | P1-2 | Wire 12 orphan Core systems | MEDIUM | MEDIUM |
| 6 | P1-3 | Add tests for central hub sessions | LOW | LOW |
| 7 | P2-1 | Audit 41 Main.cs-only systems | HIGH | MEDIUM |
| 8 | P2-2 | Add CatalogFileSystem tests | LOW | LOW |

**Critical path:** P0-3 → P0-2 → P0-1 → P1-1 → P1-2

---

## 5. Verification Gates (Per Phase)

```bash
# After EVERY phase:
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj   # 0 errors
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj     # All pass
dotnet build Ashfall.csproj                                  # 0 errors
godot --headless -- --data-integrity-selftest               # 0 errors
godot --headless -- --bridge-selftest                       # Pass
```

---

## 6. Rollback Strategy

| Phase | Rollback Method |
|-------|-----------------|
| P0-3 | Delete new test files |
| P0-2 | Git revert JSON mutations + loader changes |
| P0-1 | Git revert `SurvivorsHostSession.cs` |
| P1-1 | Git revert extracted HostSession files + Main.cs changes |
| P1-2 | Git revert new HostSession files + Main.cs wiring |
| P1-3 | Delete new test files |
| P2-1 | Git revert extracted HostSession files |
| P2-2 | Delete new test file |

**Note:** Because the workspace has 2,932 uncommitted changes, use `git stash` cautiously (broken symlink blocks it). Prefer `git worktree` or branch-based rollback.

---

## 7. Final Integration Checklist

- [ ] P0-1: SurvivorsHostSession H1 fixed
- [ ] P0-2: schema_version added to all JSON files
- [x] P0-3: 3 narrative catalog tests added
- [ ] P1-1: Main.ExpandedShelterSystems.cs extracted
- [ ] P1-2: 12 orphan Core systems wired
- [ ] P1-3: 3 central hub session tests added
- [ ] P2-1: 41 Main.cs-only systems audited
- [ ] P2-2: CatalogFileSystem tests added
- [ ] All 5 verification gates pass after each phase
- [ ] No new `System.Random` or `Godot.*` leaks in Core
- [ ] All stateful systems still implement `CaptureState/RestoreState`

---

*Integration plan complete. Ready for project owner review and prioritization.*
