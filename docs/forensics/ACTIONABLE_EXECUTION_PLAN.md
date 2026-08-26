# Actionable Execution Plan: ASHFALL Integration Phases

**Status:** P0-3 COMPLETE ✅ | P0-2, P0-1, P1-1, P1-2, P1-3, P2-1, P2-2 pending  
**Date:** 2026-08-23  
**Mode:** Exact file paths, exact commands, exact verification steps  

---

## P0-3 ✅ COMPLETE — 3 Zero-Test Catalogs

**Created:**
- `Ashfall.Core.Tests/GhostTransmissionCatalogTests.cs` (4 tests)
- `Ashfall.Core.Tests/OralLoreCatalogTests.cs` (4 tests)
- `Ashfall.Core.Tests/RadioScriptbookCatalogTests.cs` (4 tests)

**Verification:**
```bash
dotnet test Ashfall.Core.Tests --filter "FullyQualifiedName~GhostTransmissionCatalogTests|FullyQualifiedName~OralLoreCatalogTests|FullyQualifiedName~RadioScriptbookCatalogTests"
# Result: 9/9 passed
```

---

## P0-2 — Add schema_version to 317 JSON Files

### Strategy: Automated Script + Manual Verification

**Phase P0-2a: Core framework update (DONE)**
- `CatalogIntegrityValidator.cs` already validates schema_version presence
- 12 wrapper-first loaders already implemented in Task 4b

**Phase P0-2b: Expansion-specific files (~50 files)**

Target files:
```
Assets/StreamingAssets/Data/holdfast_quests.json
Assets/StreamingAssets/Data/duty_roster_locations.json
Assets/StreamingAssets/Data/duty_roster_quests.json
Assets/StreamingAssets/Data/standing_record_*.json
Assets/StreamingAssets/Data/greenhouse_*.json
Assets/StreamingAssets/Data/verdict_locations.json
Assets/StreamingAssets/Data/crossing_locations.json
... (50 expansion-specific files)
```

**Automation script:**
```bash
cd "/home/robertsrff/Music/Atomic_War_Straving_Survival/Atomic War"
python3 scripts/add_schema_version.py --target expansion --version 1
```

**Manual verification:**
```bash
# Check that no JSON file is missing schema_version
python3 -c "
import json, os
missing = []
for root, dirs, files in os.walk('Assets/StreamingAssets/Data'):
    for f in files:
        if f.endswith('.json'):
            path = os.path.join(root, f)
            with open(path) as fh:
                first = fh.read(200)
            if 'schema_version' not in first:
                missing.append(path)
print(f'Files without schema_version: {len(missing)}')
for m in missing[:10]:
    print(f'  {m}')
"
```

**Phase P0-2c: Remaining files (~255 files)**

Same script with `--target all` flag.

**Blast radius:** 317 JSON files + ~20 loader updates  
**Risk:** MEDIUM — bulk data mutation  
**Reversibility:** `git checkout -- Assets/StreamingAssets/Data/*.json Assets/Ashfall.Core/*Loader.cs`

---

## P0-1 — Fix SurvivorsHostSession H1 Duplication

### Current Problem

`src/Host/SurvivorsHostSession.cs:1-384` defines its own `SurvivorNeedsState` class (duplicate of Core), directly manages needs/radiation state, and re-implements save serialization.

### Exact Changes

**File 1: `src/Host/SurvivorsHostSession.cs`**

Remove duplicate state class (lines 10-35):
```csharp
// DELETE THIS DUPLICATE:
public class SurvivorNeedsState
{
    public string Id = string.Empty;
    public float Hunger;
    public float Thirst;
    public float Fatigue;
    public float Warmth = 100f;
    public float Morale = 50f;
    public float Health = 100f;
    public float Hygiene = 100f;
    public bool IsAliveState => !IsDead && IsAlive;
    // ... etc
}
```

Replace with Core import:
```csharp
using Ashfall.Core.Survivors; // Already imported
// Use SurvivorNeedsState from Core directly
```

Update `AddSurvivor` to create Core state:
```csharp
var state = new SurvivorNeedsState // From Core
{
    Id = id,
    // ... fields match Core exactly
};
```

Update `CaptureSave()` to delegate to Core:
```csharp
public SurvivorsSaveState CaptureSave()
{
    var save = new SurvivorsSaveState();
    foreach (var s in _coreNeeds.GetAll()) // Iterate Core's registrations
    {
        save.survivors.Add(new SurvivorSliceState { /* map from Core state */ });
    }
    return save;
}
```

**File 2: `src/Host/SurvivorsHostSession.cs`** (continued)

Make `RosterState` a read-only view:
```csharp
public IReadOnlyList<SurvivorNeedsState> RosterState => _coreNeeds.GetAll();
```

Remove `_radStates` dictionary — delegate to `RadiationSystem` state.

**Blast radius:** 1 file (384 lines → ~200 lines)  
**Risk:** MEDIUM — touches the most-used host session  
**Reversibility:** `git checkout -- src/Host/SurvivorsHostSession.cs`

---

## P1-1 — Extract Main.ExpandedShelterSystems.cs God Object

### Current Problem

`src/Main.ExpandedShelterSystems.cs` is 699 lines with 20 sessions, 20 panels, 20 dirty flags, 20 Setup methods, 20 Save methods.

### Extraction Pattern

For each system, create a self-contained HostSession:

```csharp
// BEFORE: Main.ExpandedShelterSystems.cs owns everything
private WaterTreatmentHostSession _waterTreatment = null!;
private bool _waterTreatmentDirty;
private void SetupWaterTreatment() { ... }
private void SaveWaterTreatment() { ... }

// AFTER: WaterTreatmentHostSession is self-contained
public sealed class WaterTreatmentHostSession
{
    public WaterTreatmentSystem System { get; }
    public WaterTreatmentPanel Panel { get; }
    public bool IsDirty { get; private set; }
    public event Action StateChanged;
    
    public WaterTreatmentHostSession(string dataDir) { ... }
    public void TickDay(int day) => System.Tick(day);
    public void Save() => WaterTreatmentSaveStore.TrySave(System.CaptureState());
    public void BindPanel(WaterTreatmentPanel panel) { ... }
}
```

### Extraction Order (dependency-aware)

| Order | System | Dependencies | Est. Lines |
|-------|--------|--------------|------------|
| 1 | WaterTreatmentHostSession | None | 80 |
| 2 | AirlockSecurityHostSession | None | 80 |
| 3 | RegionalTreatyHostSession | None | 80 |
| 4 | VinylMoraleHostSession | None | 80 |
| 5 | WildlifeTrappingHostSession | None | 80 |
| 6 | ExcavationHostSession | None | 80 |
| 7 | WaystationHostSession | None | 80 |
| 8 | SurvivorRelationsHostSession | Needs Core instance | 100 |
| 9 | ApprenticeshipHostSession | Roster, SurvivorRelations | 120 |
| 10 | ShelterThermalHostSession | Needs, StartingLevel, DeepFreeze | 150 |
| 11 | ShelterScheduleHostSession | PowerGrid | 100 |
| 12 | SumpFloodingHostSession | Weather, Power, DeepFreeze | 120 |
| 13 | DecontaminationHostSession | Inventory, Radiation, Airlock, StartingLevel | 150 |
| 14 | KitchenNutritionHostSession | Inventory, Needs | 100 |
| 15 | EquipmentConditionHostSession | Inventory, Crafting | 100 |
| 16 | LibraryStudyHostSession | Skills, Research, Journal, Roster | 150 |
| 17 | ArchiveDeskHostSession | Journal, Knowledge, Inventory, Roster | 150 |
| 18 | ContractorRosterHostSession | Inventory, Roster, Expedition | 120 |
| 19 | MentalHealthCrisisHostSession | Needs, Medical, Dependency, Roster | 150 |
| 20 | AutopsyHostSession | Inventory, Radiation, Ventilation, Research, Medical | 200 |

**Blast radius:** 33+ files  
**Risk:** HIGH — refactoring central orchestration  
**Reversibility:** `git checkout -- src/Main.ExpandedShelterSystems.cs src/Host/*HostSession.cs`

---

## P1-2 — Wire 12 Orphan Core Systems

### Classification

| Category | Count | Systems | Action |
|----------|-------|---------|--------|
| Dead code | 12 | Caregiving, ExpeditionVehicle, IdeologicalFriction, Leadership, PhantomMemory, RationConflict, MaritimeDive, OrbitalHarrowTelemetry, PharmaLab, TraumaBond, WeatherStation, WorkshopReverseEngineering | Create thin HostSession OR remove |
| Core-internal | 3 | Ballistics, SkillAtrophy, WeaponCondition | No host session needed |

### For Each Dead-Code Orphan

1. **Verify it's truly unused:**
   ```bash
   grep -rn "CaregivingSystem" src/ Assets/Ashfall.Core/ | grep -v "CaregivingSystem.cs"
   # If only 1 result (the file itself), it's dead code
   ```

2. **Decision gate:**
   - If 0 external references → mark for removal (ask user)
   - If 1-2 references → create thin HostSession

3. **Thin HostSession pattern:**
   ```csharp
   public sealed class XxxHostSession
   {
       public XxxSystem System { get; }
       public event Action StateChanged;
       
       public XxxHostSession() 
       {
           System = new XxxSystem(...);
       }
       
       public void TickDay(int day) => System.Tick(day);
       public XxxState CaptureState() => System.CaptureState();
       public void RestoreState(XxxState s) => System.RestoreState(s);
   }
   ```

**Files to create:** 12 `src/Host/*HostSession.cs`  
**Files to touch:** 5 `src/Main.*.cs` partials  
**Blast radius:** MEDIUM

---

## P1-3 — Add Tests for 3 Central Hub Sessions

### Targets

| Session | Why It Matters | Test Strategy |
|---------|----------------|---------------|
| InventoryHostSession | Most touched system | Test add/remove/equip cycles, save/load |
| WorldHostSession | World-state wiring | Test weather/map/radiation propagation |
| Phase0HostSession | Expansion orchestration | Test expansion gating, save/load |

**Files to create:**
- `Ashfall.Core.Tests/InventoryHostSessionTests.cs`
- `Ashfall.Core.Tests/WorldHostSessionTests.cs`
- `Ashfall.Core.Tests/Phase0HostSessionTests.cs`

**Blast radius:** LOW — new test files only

---

## P2-1 — Audit 41 Main.cs-Only Systems

### Audit Criteria

For each of the 41 systems wired only through Main.cs:

1. **Does it have a dedicated HostSession?** → Already done
2. **Does it have UI?** → Extract if yes
3. **Does it have save state?** → Extract if yes
4. **Is it only ticked?** → Keep in Main.cs

### Priority Order

1. Systems with save state + UI → Extract immediately
2. Systems with save state only → Extract soon
3. Systems with UI only → Extract soon
4. Systems with neither → Keep in Main.cs

**Blast radius:** HIGH — 41 systems across multiple Main partials

---

## P2-2 — Add CatalogFileSystem Direct Tests

### Test Strategy

```csharp
[Fact]
public void CatalogFileSystem_DiscoverAllJsonFiles()
{
    var files = CatalogFileSystem.DiscoverAll("Assets/StreamingAssets/Data");
    Assert.NotEmpty(files);
    Assert.All(files, f => Assert.EndsWith(".json", f));
}

[Fact]
public void CatalogFileSystem_ResolvesSchemaVersion()
{
    var version = CatalogFileSystem.GetSchemaVersion("path/to/file.json");
    Assert.True(version >= 1);
}
```

**Files to create:** `Ashfall.Core.Tests/CatalogFileSystemTests.cs`  
**Blast radius:** LOW

---

## Verification Gates (Run After EVERY Phase)

```bash
# 1. Core tests
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj

# 2. Godot host build
dotnet build Ashfall.csproj

# 3. Data integrity
godot --headless --path . -- --data-integrity-selftest

# 4. Bridge selftest
godot --headless --path . -- --bridge-selftest
```

**Current baseline:**
- Tests: 2554/2554 pass ✅
- Core build: 0 errors ✅
- Data integrity: 0 errors ✅
- Bridge: pass ✅

---

## Rollback Commands

```bash
# If anything breaks:
git checkout -- Ashfall.Core.Tests/*CatalogTests.cs  # P0-3 rollback
git checkout -- Assets/StreamingAssets/Data/*.json   # P0-2 rollback
git checkout -- src/Host/SurvivorsHostSession.cs      # P0-1 rollback
git checkout -- src/Main.ExpandedShelterSystems.cs    # P1-1 rollback
git checkout -- src/Host/*HostSession.cs              # P1-2 rollback
```

---

## Next Steps

1. **P0-2:** Run schema_version sweep script on 50 expansion-specific files
2. **P0-1:** Fix SurvivorsHostSession H1 duplication
3. **P1-1:** Extract Main.ExpandedShelterSystems.cs (start with WaterTreatmentHostSession)
4. **P1-2:** Wire 12 orphan Core systems
5. **P1-3:** Add central hub session tests
6. **P2-1:** Audit 41 Main.cs-only systems
7. **P2-2:** Add CatalogFileSystem tests

**Recommended next prompt:**
```bash
# Start P0-2: schema_version sweep
python3 scripts/add_schema_version.py --target expansion --version 1
```
