# Plan 186 — Shelter Maintenance & Degradation System

## Goal

Create a shelter maintenance and degradation system where bunker components (air filters, walls, power systems, water recyclers, blast doors) deteriorate over time and require regular maintenance, repairs, and part replacements. Currently `LandmarkDegradationSystem` handles external landmark decay, and narrative text references "air filters degrade" and "walls crack," but there is no shelter-specific maintenance system — no filter condition tracking, no wall integrity, no component wear, no maintenance scheduling, no repair mechanics. This plan makes the shelter a living, aging structure that requires care.

## Why

**Repository evidence:** Grep for `ShelterMaintenance`, `BuildingDegradation`, `AirFilterDegradation`, `WallIntegrity`, `BunkerCondition`, `FilterCondition`, `shelter_repair` in Core returns ZERO matches. `LandmarkDegradationSystem` exists but handles external locations, not the shelter itself. Narrative references to "air filters degrade" (in events and descriptions) have no mechanical backing. The shelter is treated as a static, indestructible box.

**What is missing:** No shelter component degradation. No air filter condition tracking. No wall integrity. No power system wear. No water recycler maintenance. No blast door degradation. No maintenance scheduling. No repair mechanics. The shelter never ages, never breaks down, never needs attention.

**Why existing plans don't solve it:** Plan 158 (disaster response) handles acute crises (earthquakes, floods) but not gradual degradation. Plan 135 (weather cascade) makes weather affect shelter through modifiers but not component wear. Plan 23 (power life support) tracks power consumption but not power system degradation. No plan addresses shelter maintenance as a system.

**Player value:** Creates strategic depth (schedule maintenance, stock spare parts), adds realism (buildings age and break), generates emergent stories (filter failure during storm, wall crack letting in radiation), and makes shelter management meaningful beyond just assigning survivors to duties.

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Shelter/` — shelter systems
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs` — thermal model
- `Assets/Ashfall.Core/Shelter/SkyLayerArmorSystem.cs` — sky-layer armor
- `Assets/Ashfall.Core/PowerGridSystem.cs` — power system
- `Assets/Ashfall.Core/WaterTreatmentSystem.cs` — water treatment
- `Assets/Ashfall.Core/VentilationSystem.cs` — ventilation
- NEW: `Assets/Ashfall.Core/Shelter/ShelterMaintenanceSystem.cs`
- NEW: `Assets/StreamingAssets/Data/shelter_components.json`

## Main Task 1 — Foundation / System Contract

1. Create `ShelterMaintenanceSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `ShelterComponent` DTO: `componentId`, `componentName` (air_filter/wall_section/power_generator/water_recycler/blast_door/ventilation_unit/structural_beam), `componentType` (life_support/structural/power/water/air), `condition` (0-100, 100=perfect), `maxCondition` (100), `degradationRate` (per day), `lastMaintained` (day), `repairCost` (list of item_ids), `repairTime` (hours), `failureThreshold` (condition where component fails), `warningThreshold` (condition where warnings start)
3. Define `MaintenanceAction` DTO: `actionId`, `componentId`, `actionType` (inspect/clean/repair/replace), `requiredSkill` (skill_id), `requiredItems` (list of item_ids with quantities), `duration` (hours), `conditionRestored` (amount), `successChance` (0-100), `assignedSurvivorId`
4. Define `DegradationEvent` DTO: `eventId`, `componentId`, `eventType` (warning/failure/critical_failure/cascade), `day`, `description`, `effects` (list)
5. Define `ShelterMaintenanceState` DTO: list of shelter components, list of active maintenance actions, list of degradation events, last maintenance day, shelter overall condition
6. Implement `CaptureState/RestoreState` with schema versioning
7. Define shelter components (12+ components):
   - **Air Filters** (3 stages): pre-filter, main filter, HEPA filter — each degrades at different rates
   - **Wall Sections** (4-6 sections): north/south/east/west walls, ceiling, floor — structural integrity
   - **Power Generator**: main power source, degrades with use
   - **Water Recycler**: water purification system, filter degradation
   - **Blast Doors**: main entrance, emergency exits — mechanical wear
   - **Ventilation Units**: air circulation, fan wear, duct integrity
   - **Structural Beams**: support structure, slow degradation
   - **Radiation Shielding**: lead/concrete shielding, erosion over time
8. Define degradation mechanics:
   - Each component degrades daily based on usage and environmental factors
   - Air filters degrade faster during storms/high radiation
   - Walls degrade from temperature cycling, radiation, moisture
   - Power generator degrades with runtime hours
   - Water recycler degrades with contamination load
   - Blast doors degrade with open/close cycles
   - Ventilation degrades from dust/particulate load
9. Define maintenance mechanics:
   - **Inspect**: check component condition (no skill required, reveals condition)
   - **Clean**: basic maintenance (low skill, small condition restore)
   - **Repair**: fix damaged component (medium skill, moderate restore, requires parts)
   - **Replace**: full component replacement (high skill, full restore, requires new component)
10. Define failure consequences:
    - Air filter failure: radiation ingress, air quality drop
    - Wall breach: radiation leak, thermal loss, structural risk
    - Power generator failure: blackout, life support failure
    - Water recycler failure: no clean water, contamination risk
    - Blast door failure: security breach, radiation ingress
    - Ventilation failure: air quality drop, CO2 buildup
    - Structural failure: collapse risk, shelter integrity crisis
11. Define cascade failures:
    - Power failure → ventilation stops → air quality drops → survivors suffer
    - Wall breach → radiation ingress → survivors irradiated → medical crisis
    - Water recycler failure → dehydration → needs crisis
    - Multiple failures compound
12. Add deterministic seeding: degradation uses `ISeededRng`
13. Wire into `GameBootstrap`: `SetupShelterMaintenance`, `TickShelterMaintenance`, `SaveShelterMaintenance`
14. Create `ShelterComponentCatalogLoader` for component definitions
15. Implement shelter maintenance UI: component status panel, maintenance scheduling

## Main Task 2 — Implementation / Degradation / Maintenance / Failures / Repairs

1. Implement daily degradation:
   - Each day, check all shelter components
   - Apply degradation based on usage and environment
   - Weather modifiers (storms accelerate filter/wall degradation)
   - Radiation modifiers (high rad accelerates shielding erosion)
   - Usage modifiers (power generator degrades with load)
   - Age modifiers (older components degrade faster)
   - Degradation logged
2. Implement condition tracking:
   - Each component has 0-100 condition
   - 100-75: good (green)
   - 74-50: worn (yellow) — warnings start
   - 49-25: damaged (orange) — failure risk
   - 24-0: critical/failed (red) — immediate action required
3. Implement maintenance scheduling:
   - Player assigns survivor to maintenance task
   - Task requires specific skill level
   - Task requires specific items (parts, tools)
   - Task has duration (hours/days)
   - Task restores condition based on action type
   - Success chance based on survivor skill
4. Implement failure events:
   - Component reaches failure threshold: failure event
   - Failure effects applied (radiation ingress, power loss, etc.)
   - Failure logged in journal
   - Cascade failures trigger if dependencies fail
5. Implement repair mechanics:
   - Player assigns survivor to repair task
   - Repair requires specific items (spare parts, tools)
   - Repair has duration
   - Repair restores condition
   - Full replacement requires new component item
6. Implement spare parts system:
   - Spare parts are craftable or salvageable items
   - Filter cartridges, wall patches, generator coils, recycler membranes
   - Parts stockpiled in inventory
   - Parts consumed during repairs
7. Implement inspection system:
   - Survivors can inspect components
   - Inspection reveals exact condition
   - Inspection takes time
   - Inspection skill affects accuracy
8. Implement emergency repairs:
   - Failed components can be emergency-repaired
   - Emergency repair is faster but less effective
   - Emergency repair uses improvised materials
   - Permanent repair still required
9. Create degradation events:
   - "The Warning" — component condition dropping
   - "The Failure" — component failed
   - "The Breach" — wall breach detected
   - "The Blackout" — power failure
   - "The Repair" — component repaired
   - "The Replacement" — component replaced
   - "The Cascade" — multiple failures compounding
   - "The Crisis" — shelter integrity at risk
10. Add degradation quest hooks:
    - "The Engineer" — maintain all components above 50%
    - "The Repair" — fix failed component before cascade
    - "The Parts" — stockpile 10 spare parts
    - "The Inspection" — inspect all components
    - "The Crisis" — deal with shelter integrity failure
    - "The Upgrade" — upgrade component to reduce degradation
    - "The Maintenance" — complete 50 maintenance tasks
11. Implement shelter maintenance UI:
    - Component status panel: all components with condition bars
    - Maintenance scheduling: assign survivors to tasks
    - Repair panel: select component, assign repair, consume parts
    - Failure alerts: warnings when components at risk
    - Degradation log: history of failures and repairs
12. Add degradation journal: automatic log of degradation events
13. Implement degradation tutorial: first warning explains system
14. Add degradation tooltips: hover over component shows condition, degradation rate, last maintained
15. Create component definitions in data file (12+ components)

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `ShelterThermalSystem`: wall integrity affects thermal loss
2. Connect to `PowerGridSystem`: power generator degradation
3. Integrate with `WaterTreatmentSystem`: water recycler degradation
4. Connect to `VentilationSystem`: ventilation unit degradation
5. Wire into `RadiationSystem`: air filter/wall integrity affects radiation ingress
6. Connect to `WeatherSystem`: weather accelerates degradation
7. Implement old-save compatibility: existing saves get default component states
8. Add deterministic seeding: degradation uses `ISeededRng`
9. Create exploit prevention: degradation is time-based, can't be gamed
10. Add tests: degradation rates, maintenance effects, failures, repairs, cascade, save round-trip
11. Verify all components degrade correctly
12. Test edge cases: no degradation (constant maintenance), rapid degradation (neglect)
13. Verify headless behavior: degradation processes correctly without UI
14. Add data-integrity-selftest: components validate against item/skill catalogs
15. Create `--shelter-maintenance-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --shelter-maintenance-selftest
```

## Risk

**LOW** — Shelter maintenance is straightforward with clear inputs (time, usage, environment) and outputs (condition changes, failures). Risk of maintenance feeling like a chore rather than meaningful management. Mitigation: make inspections easy, show clear warnings, allow strategic prioritization, and ensure failures are dramatic not frequent.

## Definition of Done

- `ShelterMaintenanceSystem.cs` exists with full `CaptureState/RestoreState`
- 12+ shelter components (air filters, walls, power, water, blast doors, ventilation, structural, shielding)
- Daily degradation applied to all components
- Condition tracking (0-100, color-coded)
- Maintenance scheduling (inspect/clean/repair/replace)
- Failure events with consequences (radiation ingress, power loss, etc.)
- Cascade failure mechanics
- Repair mechanics with spare parts
- Emergency repair option
- Degradation events and quest hooks
- Save/load round-trip tested
- Deterministic degradation verified
- Old saves load without error
- Component definitions in data authority
- UI component status panel with maintenance scheduling
- Cross-system integration (thermal, power, water, ventilation, radiation, weather)

## Follow-On Opportunities

- Component upgrades (better filters, reinforced walls)
- Component modularization (swap components between shelters)
- Maintenance specialization (engineer trait reduces degradation)
- Maintenance legacy (famous repairs remembered)
- Maintenance quests (specific component goals)
