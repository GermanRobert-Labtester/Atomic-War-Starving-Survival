# Main.cs-Only Systems Audit (P3-1)

**Date:** 2026-08-23  
**Status:** Audit complete; extraction plan documented  

---

## 1. Scope Correction

The deep analysis cited **41 systems wired only through `Main.cs`**. After forensic verification:

- **35 systems** have dedicated `HostSession` classes in `src/Host/` and were extracted in P1-1.
- **9 systems** remain without dedicated `HostSession` files.
- The "41" figure was an overcount including systems that have HostSessions but are still instantiated in `Main.cs` partials.

---

## 2. Systems Without Dedicated HostSession Files

| # | System | Save State | Tests | Used By | Recommendation |
|---|--------|-----------|-------|---------|----------------|
| 1 | `ChemicalDependencySystem` | ✅ `CaptureState/RestoreState` | `ChemicalDependencySystemTests.cs` | Medical UI | Extract HostSession |
| 2 | `JournalSystem` | ✅ `JournalSave` | `JournalSystemTests.cs` | DeepCoast, DutyRoster, HostEventAdapter | Extract HostSession |
| 3 | `MedicalWardSystem` | ✅ `MedicalWardState` | None direct | Medical UI | Extract HostSession |
| 4 | `NeedsSystem` | ❌ No save state | `NeedsRadiationSystemTests.cs`, `SurvivorNeedsCharacterizationTests.cs` | ShelterThermal, KitchenNutrition, MentalHealthCrisis | Keep in Main (dependency) |
| 5 | `RadiationSystem` | ❌ No save state | `RadiationPhaseProgressionTests.cs`, `NeedsRadiationSystemTests.cs` | Autopsy, Decontamination | Keep in Main (dependency) |
| 6 | `SkillProgressionSystem` | ✅ `SkillProgressionSaveState` | `SkillProgressionSystemTests.cs` | ApprenticeshipHostSession | Keep in Main (dependency) |
| 7 | `VentilationSystem` | ✅ `VentilationState` | `VentilationSystemTests.cs` | Autopsy, ShelterThermal | Keep in Main (dependency) |
| 8 | `WeatherSystem` | ✅ `WorldWeatherState` | `WeatherSystemTests.cs` | WorldHostSession | Extract HostSession |
| 9 | `YearOfAshDeepFreezeSystem` | ✅ `YearOfAshDeepFreezeState` | None | ShelterThermalHostSession | Keep in Main (dependency) |

---

## 3. Extraction Candidates

### High Priority (standalone save state + UI)

| System | HostSession | Justification |
|--------|-------------|---------------|
| `JournalSystem` | `JournalHostSession` | Own save state, own UI panel, used by multiple sessions |
| `ChemicalDependencySystem` | `ChemicalDependencyHostSession` | Own save state, part of medical UI |
| `MedicalWardSystem` | `MedicalWardHostSession` | Own save state, part of medical UI |
| `WeatherSystem` | `WeatherHostSession` | Own save state, WorldHostSession delegates to it |

### Keep in Main (shared dependencies)

| System | Reason |
|--------|--------|
| `NeedsSystem` | Pure dependency; no save state; injected into 3+ HostSessions |
| `RadiationSystem` | Pure dependency; no save state; injected into 2+ HostSessions |
| `SkillProgressionSystem` | Pure dependency; injected into ApprenticeshipHostSession |
| `VentilationSystem` | Pure dependency; injected into 2+ HostSessions |
| `YearOfAshDeepFreezeSystem` | Pure dependency; injected into ShelterThermalHostSession |

---

## 4. Extraction Pattern

Follow the P1-1 pattern for each candidate:

1. Add `IsDirty`, `MarkDirty()`, `Save()` to new HostSession
2. Move construction from `Main.*.cs` to HostSession
3. Wire `StateChanged += () => _xxx.MarkDirty()` in Main
4. Add `_xxx?.Save()` in `Main.SaveOrchestrator.cs`
5. Add `--*-selftest` CLI verb with save/load round-trip

---

## 5. Blast Radius

- 4 new HostSession files
- 4 Main.cs partial updates
- 4 new selftest methods
- Estimated: ~200 lines per HostSession = 800 lines total

**Risk:** LOW — each extraction is isolated; pattern is proven by P1-1.

---

## 6. Recommendation

Execute P3-1 as a follow-up task extracting the 4 high-priority systems:
1. `JournalHostSession`
2. `ChemicalDependencyHostSession`
3. `MedicalWardHostSession`
4. `WeatherHostSession`

The 5 dependency systems remain in `Main.cs` as injected collaborators.
