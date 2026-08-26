# Deep Forensic Analysis: 254-Subsystem Survey

**Date:** 2026-08-23  
**Input:** `docs/forensics/254_SUBSYSTEMS_CONSOLIDATED_REPORT.md`  
**Mode:** Read-only evidence synthesis — no code modified  

---

## 1. Executive Summary

The 254-subsystem forensic survey is **methodologically sound but architecturally shallow** in places. It correctly identifies 243 unique subsystems, classifies them, and flags orphan Core systems. However, the data reveals **three structural risks** that the flat table obscures:

1. **`Main.cs` is a god object** — 36 partial files, 44 Core references, 41 systems wired *only* through `Main.cs` with no dedicated `HostSession`
2. **Host session coverage is ~20%** — only 29 of 112 Core systems are directly referenced from `src/`; the rest are mediated through `Main.cs`
3. **Narrative catalogs are bulk debt** — 80 catalogs, 3 have zero tests, ~317 of 318 JSON files lack `schema_version`

The survey correctly flags **15 orphan Core systems** and **1 HIGH-risk** item (`SurvivorsHostSession` H1). The real risk is **not the orphans** — it is the **centralized wiring pattern** that makes any `Main.cs` change a blast-radius event.

---

## 2. Data Quality Issues in the Report

| Issue | Detail |
|-------|--------|
| **Duplicate entries** | Parsing yielded 258 rows for 243 unique subsystems; some appear twice with different risk strings |
| **Missing sequence** | 24 numbers missing from 1–267: `13, 21, 31, 51, 211, 213, 216, 217, 218, 220, 221, 223, 224, 225, 226, 227, 228, 231, 239, 241, 250, 251, 258, 260` |
| **Risk inflation** | Some systems appear as both `LOW` and `MEDIUM` in different batches; risk taxonomy is not normalized |
| **Classification drift** | `LIVE_CORE, PORTED_NOT_WIRED` appears 30 times in raw parse vs 15 in summary; deduplication needed |
| **`schema_version` gap** | 317 of 318 JSON files lack `schema_version`; only `questline_master.json` has it (v2) |

---

## 3. Architecture Deep Dive

### 3.1 The `Main.cs` God Object

`Main.cs` is split into **36 partial files**:

| Partial | Core References | Role |
|---------|----------------|------|
| `Main.ExpandedShelterSystems.cs` | **35** | Hub for shelter, thermal, autopsy, ventilation, etc. |
| `Main.Medical.cs` | 3 | Medical ward, bed saves |
| `Main.World.cs` | 3 | World state, memorial |
| `Main.Expeditions.cs` | 1 | Wasteland map |
| `Main.Narrative.cs` | 1 | Journal |
| `Main.UiPanels.cs` | 1 | Material shielding |
| All others | 0 | UI tests, panels, flow |

**Key finding:** `Main.ExpandedShelterSystems.cs` alone references 35 Core systems. This partial is the **central wiring hub** for the entire shelter/survivor/thermal stack.

### 3.2 Host Session Coverage

Only **29 Core systems** are directly referenced from `src/`:

```
MedicalWardSystem, FactionIconCatalog, JournalSystem, GuiltInsomniaSystem,
MedicalWardSave, MedicalBedSave, MemorialSystem, MemorialSave,
WastelandMapSystem, MaterialShieldingSystem, ShelterAssignmentSystem,
DailyBriefingSave, PowerGridSave, NeedsSystem, SilentFoundrySystem,
SilentFoundryCatalog, DiseaseSystem, DiseaseCatalog, BunkerBlueprintCatalog,
DeepLoreLocationCatalog, DiveSiteCatalog, ProceduralScavengeSystem,
PsychologicalContaminationSystem, SomaticFlashbackSystem, FinalWishSystem,
ChemicalDependencySystem, RespiratoryDegenerationSystem, WeatherSystem,
TacticalCombatSystem
```

**41 systems** are wired *only* through `Main.cs` partials — they have no dedicated `HostSession`.

### 3.3 The Real Orphan Problem

The report flags 15 orphan Core systems as `PORTED_NOT_WIRED`. But the deeper issue is **wiring concentration**:

| Category | Count | Risk |
|----------|-------|------|
| Systems with dedicated `HostSession` | ~29 | Low — isolated wiring |
| Systems wired only via `Main.cs` | **41** | **Medium** — god-object coupling |
| True orphans (no host ref at all) | **15** | **Medium** — dead code or future work |
| **Total Core systems** | **112** | — |

**The 41 `Main.cs`-only systems are the real maintenance hazard.** A change to `Main.ExpandedShelterSystems.cs` touches 35 systems simultaneously.

---

## 4. Risk Cluster Analysis

### 4.1 HIGH Risk (1 system)

| # | System | Issue |
|---|--------|-------|
| 22 | `SurvivorsHostSession` | H1: Duplicates core survival mechanics in host session — violates Invariant 5 |

**Impact:** If `SurvivorsHostSession` diverges from Core, save compatibility and determinism break.

### 4.2 MEDIUM Risk Clusters

| Cluster | Count | Systems | Pattern |
|---------|-------|---------|---------|
| **Orphan Core** | 15 | Ballistics, Caregiving, ExpeditionVehicle, IdeologicalFriction, Leadership, PhantomMemory, RationConflict, MaritimeDive, OrbitalHarrowTelemetry, PharmaLab, SkillAtrophy, TraumaBond, WeaponCondition, WeatherStation, WorkshopReverseEngineering | Core logic exists but no host wiring; may be dead code or incomplete migration |
| **Save/Data Layer** | 1 | `ExpansionHubSave` | Phase 11 wiring stubs remain |
| **Core Systems** | 1 | `NeedsSystem` | Save/load round-trip coverage gap (H11) |
| **Unhosted Physics** | 1 | `BallisticsSystem` | No Godot host session; consumed only by `TacticalCombatSystem` |

### 4.3 LOW Risk but Bulk

| Category | Count | Issue |
|----------|-------|-------|
| Narrative catalogs | 80 | Bulk content; 3 have 0 tests; ~317/318 JSON files lack `schema_version` |
| Godot host sessions | 43 | Mostly thin wrappers; 40 have 0 tests |
| Core-only | 59 | Catalogs, no host session needed |

---

## 5. Test Coverage Gap Analysis

### 5.1 Narrative Catalog Gaps (3)

| # | Catalog | Risk |
|---|---------|------|
| 122 | `GhostTransmissionCatalog` | Content catalog, no tests |
| 168 | `OralLoreCatalog` | Content catalog, no tests |
| 184 | `RadioScriptbookCatalog` | Content catalog, no tests |

### 5.2 Host Session Gaps (40)

Most `HostSession` files lack tests. This is **expected** for thin wrappers, but problematic for:

| # | Host Session | Why It Matters |
|---|-------------|----------------|
| 137 | `InventoryHostSession` | Central hub — inventory is the most touched system |
| 263 | `WorldHostSession` | Central world-state wiring — weather, map, landmarks, radiation |
| 174 | `Phase0HostSession` | Central hub for expansion orchestration |

### 5.3 System Gaps (2)

| # | System | Gap |
|---|--------|-----|
| 55 | `PhantomMemorySystem` | 0 tests, orphan Core |
| 2 | `NeedsSystem` | Save/load round-trip gap (H11) |

---

## 6. Data Integrity Analysis

### 6.1 `schema_version` Coverage

| Metric | Value |
|--------|-------|
| Total JSON files | 318 |
| Files with `schema_version` | **1** (`questline_master.json` v2) |
| Files without `schema_version` | **317** |
| Files with wrapper-first loaders | 12 (Task 4b) |
| Files still bare-list | ~50+ expansion files |

**Risk:** Save migration and catalog validation rely on version detection. With 317/318 files unversioned, any schema change is a silent break.

### 6.2 ID Prefix Compliance

`CatalogIntegrityValidator` enforces snake_case prefixes. The survey shows **0 prefix violations**, which is good.

### 6.3 Content Gaps

| File | Gap |
|------|-----|
| `utility_actions.json` | 2 headroom actions added (6 total) |
| `disease_catalog.json` | 3 diseases with countermeasures added (7 total) |
| `expansion_item_tags.json` | 55 "missing" item IDs are narrative keepsakes — **correct by design** |

---

## 7. Cross-Cutting Concerns

### 7.1 Determinism

- **0 `System.Random` leaks** in the survey scope
- All stateful systems implement `CaptureState/RestoreState`
- `ISeededRng` is used consistently in Core
- **Risk:** `HoldfastRuntimeSession` duplicates Core survival mechanics (H1-adjacent)

### 7.2 Save Compatibility

- 5 Godot save stores lacked checksum — **fixed** in this workspace
- `SaveChecksum` pins JSON shape and hash
- Cross-host save compatibility is now **Godot-only** (Unity host deleted)

### 7.3 Engine Coupling

- **0** `UnityEngine.*` references in Core
- **0** `Godot.*` references in Core
- All engine coupling is in `src/Host/` — correct per Invariant 1

---

## 8. Actionable Insights

### 8.1 Immediate (P0)

| Action | Target | Rationale |
|--------|--------|-----------|
| Fix `SurvivorsHostSession` duplication | H1 | Violates Invariant 5; risks save/load divergence |
| Add `schema_version` to remaining JSON files | 317 files | Enables safe schema migration |
| Add tests for `GhostTransmissionCatalog`, `OralLoreCatalog`, `RadioScriptbookCatalog` | 3 catalogs | Zero-test content catalogs are invisible to regression |

### 8.2 Near-term (P1)

| Action | Target | Rationale |
|--------|--------|-----------|
| Wire 15 orphan Core systems to host sessions | 15 systems | Unlock gameplay features currently in dead code |
| Add tests for `InventoryHostSession`, `WorldHostSession`, `Phase0HostSession` | 3 sessions | Central hubs with 0 tests |
| Reduce `Main.ExpandedShelterSystems.cs` from 35 refs to <10 | 1 partial | God-object risk; one change = 35 systems affected |
| Add `schema_version` to 50 expansion-specific JSON files | ~50 files | `holdfast_quests.json`, `duty_roster_*`, etc. |

### 8.3 Strategic (P2)

| Action | Target | Rationale |
|--------|--------|-----------|
| Extract `Main.ExpandedShelterSystems.cs` into dedicated `HostSession` classes | 35 systems | Reduce god-object blast radius |
| Create `BallisticsHostSession` (even if thin) | `BallisticsSystem` | Currently orphaned; needed for combat UI |
| Audit 41 `Main.cs`-only systems for host-session extraction | 41 systems | Improve testability and isolation |
| Add `CatalogFileSystem` direct tests | 1 infrastructure | Currently only tested indirectly |

---

## 9. Contradictions with Prior AGENTS.md Claims

| Claim in AGENTS.md | Evidence from Survey |
|--------------------|----------------------|
| "588 DEMOTE ghost markers resolved" | Confirmed: 0 markers remain in Core/src |
| "All stateful systems implement CaptureState/RestoreState" | **Confirmed:** 0 silent data loss |
| "0 UnityEngine references in Core" | **Confirmed:** 0 violations of Invariant 1 |
| "29 catalog loaders use JsonUtility" | **RESOLVED:** All migrated to Core; 0 `JsonUtility` references |
| "124 compiler warnings in tests" | **RESOLVED:** 0 errors, 3 minor analyzer warnings |
| "`HoldfastRuntimeSession` duplicates core survival mechanics" | **Confirmed:** H1-adjacent risk; not fixed yet |

---

## 10. Final Verdict

| Dimension | Rating | Evidence |
|-----------|--------|----------|
| **Coverage** | 98% | 243/254 subsystems analyzed; 11 missing numbers |
| **Classification accuracy** | High | 224 LOW, 15 MEDIUM orphan, 1 HIGH — matches code evidence |
| **Risk identification** | High | Correctly flags `SurvivorsHostSession` H1 and 15 orphans |
| **Architectural insight** | **Medium** | Under-reports `Main.cs` god-object risk; 41 systems wired only through partials |
| **Actionability** | High | Clear P0/P1/P2 roadmap with file targets |

**Bottom line:** The survey is a solid **inventory**, but it is not a **risk model**. The real danger is not the 15 orphan Core systems — it is the **36-partial `Main.cs` god object** that concentrates 35 system wirings into one file. Any change to `Main.ExpandedShelterSystems.cs` is a simultaneous change to 35 systems with no isolation, no per-system tests, and no rollback granularity.

---

*Analysis complete. No code modified. Next step: review actionable insights with project owner for prioritization.*
