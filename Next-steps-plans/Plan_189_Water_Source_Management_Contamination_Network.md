# Plan 189 — Water Source Management & Contamination Network

## Goal

Create a unified water source management and contamination tracking system where players can discover, monitor, and manage multiple water sources (wells, rivers, rain collectors, underground springs) with individual contamination levels, flow rates, and maintenance requirements. Currently `WaterTreatmentSystem.cs` (634 lines) handles water purification with a single `incomingContaminationLevel`, `LocationEvolutionSystem` tracks per-location contamination, and `HydroGeologyCatalog` has static well contamination data, but there is no unified water source network — no individual well tracking, no water source discovery, no contamination propagation between sources, no water infrastructure management, no source switching. This plan adds strategic water management to the survival loop.

## Why

**Repository evidence:** Grep for `WaterSource`, `WaterNetwork`, `WaterInfrastructure`, `WellManagement`, `WaterSourceManagement` in Core returns ZERO system matches. `WaterTreatmentSystem.cs` (634 lines) has `incomingContaminationLevel` (single float) but no source tracking. `LocationEvolutionSystem` has `contaminationLevel` per location but not per water source. `HydroGeologyCatalog` has `WellContaminationEntry` (static data) but no dynamic tracking. `District8DeepCoastSystem` has `contaminationLevel` for coastal nodes. `SumpFloodingSystem` has `contaminationLevel` for sump nodes. Contamination is scattered across multiple systems with no unified water source management.

**What is missing:** No unified water source network. No individual well/river/spring tracking. No water source discovery system. No contamination propagation between connected sources. No water infrastructure (pipes, pumps, storage tanks). No source switching (use well A instead of well B). No water quality monitoring. No water source maintenance. Players have one "water supply" with no strategic choices.

**Why existing plans don't solve it:** Plan 158 (disaster response) handles water contamination crises but not source management. Plan 135 (weather cascade) affects water through weather but not source tracking. Plan 23 (power life support) handles water recycler but not external sources. No plan addresses water source management as a system.

**Player value:** Creates strategic depth (choose cleanest source, manage infrastructure), adds realism (water sources degrade, require maintenance), generates emergent stories (well contaminated, emergency switch to river), and makes water management meaningful beyond just "purify everything."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/WaterTreatmentSystem.cs` — water purification
- `Assets/Ashfall.Core/LocationEvolutionSystem.cs` — location contamination
- `Assets/Ashfall.Core/Narrative/HydroGeologyCatalog.cs` — well data
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — sump contamination
- `Assets/Ashfall.Core/District8DeepCoastSystem.cs` — coastal contamination
- NEW: `Assets/Ashfall.Core/Water/WaterSourceSystem.cs`
- NEW: `Assets/StreamingAssets/Data/water_sources.json`

## Main Task 1 — Foundation / System Contract

1. Create `WaterSourceSystem.cs` in `Assets/Ashfall.Core/Water/`
2. Define `WaterSource` DTO: `sourceId`, `sourceName`, `sourceType` (well/river/spring/rain_collector/underground_spring/municipal), `locationId`, `discoveredDay`, `flowRate` (liters/day), `currentContamination` (0-1), `baseContamination` (0-1, natural level), `contaminationTrend` (improving/stable/worsening), `lastTestedDay`, `lastTestedContamination` (0-1), `isActive` bool, `infrastructureLevel` (none/basic/advanced), `maintenanceRequired` bool, `lastMaintainedDay`
3. Define `WaterSourceConnection` DTO: `connectionId`, `sourceA`, `sourceB`, `connectionType` (underground_flow/surface_runoff/pipe_connection), `flowRate` (liters/day between sources), `contaminationTransferRate` (0-1, how much contamination propagates)
4. Define `WaterInfrastructure` DTO: `infrastructureId`, `infrastructureType` (pipe/pump/storage_tank/filtration_unit/water_test_kit), `locationId`, `condition` (0-100), `capacity` (liters for storage), `efficiency` (0-1 for filtration), `installedDay`, `lastMaintainedDay`
5. Define `WaterTestResult` DTO: `testId`, `sourceId`, `testedDay`, `testedBySurvivorId`, `contaminationLevel` (0-1), `contaminantType` (radiation/biological/chemical/mixed), `testAccuracy` (0-1, based on test kit quality)
6. Define `WaterSourceState` DTO: list of water sources, list of source connections, list of infrastructure, list of test results, active source id, total available water (liters), water demand (liters/day)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define water source types:
   - **Well**: drilled into groundwater, moderate flow, contamination risk from surface runoff
   - **River**: surface water, high flow, high contamination risk (upstream sources)
   - **Spring**: natural groundwater seep, low flow, low contamination (filtered through rock)
   - **Rain Collector**: captures rainfall, variable flow (weather-dependent), low contamination
   - **Underground Spring**: deep groundwater, low flow, very low contamination
   - **Municipal**: pre-war water system, moderate flow, unknown contamination (needs testing)
9. Define contamination mechanics:
   - Each source has base contamination (natural level)
   - Contamination changes daily based on:
     - Weather (rain increases runoff contamination)
     - Location evolution (nearby contamination spreads)
     - Infrastructure (filtration reduces contamination)
     - Maintenance (poor maintenance increases contamination)
     - Connection to other sources (contamination propagates)
   - Contamination affects water quality
   - High contamination water causes illness (disease risk)
10. Define contamination propagation:
    - Connected sources share contamination
    - Underground flow: contamination moves slowly between connected wells/aquifers
    - Surface runoff: rain carries contamination from higher to lower elevation
    - Pipe connections: contaminated source pollutes connected clean sources
    - Propagation rate based on connection type and flow rate
11. Define water infrastructure:
    - **Pipes**: connect sources to shelter, can leak (contamination ingress)
    - **Pumps**: move water from source to shelter, require power, can break
    - **Storage Tanks**: store water, can be contaminated if not sealed
    - **Filtration Units**: reduce contamination, require maintenance, filter degradation
    - **Water Test Kits**: test source contamination, accuracy based on kit quality
12. Define water source discovery:
    - Sources discovered through exploration (expeditions)
    - Some sources known at game start (shelter well)
    - Sources can be lost (well runs dry, river diverted)
    - New sources can appear (spring discovered during construction)
13. Define water source maintenance:
    - Wells require periodic cleaning (sediment removal)
    - Filters require replacement
    - Pipes require leak checks
    - Pumps require mechanical maintenance
    - Storage tanks require sealing checks
    - Poor maintenance: increased contamination, reduced flow
14. Add deterministic seeding: contamination changes use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupWaterSources`, `TickWaterSources`, `SaveWaterSources`

## Main Task 2 — Implementation / Sources / Contamination / Infrastructure / Testing

1. Implement water source tracking:
   - Each source has flow rate, contamination, status
   - Sources update daily (contamination changes, flow varies)
   - Active source provides water to shelter
   - Inactive sources available but not used
   - Source list viewable in UI
2. Implement contamination dynamics:
   - Daily contamination update based on factors
   - Weather modifiers (rain increases contamination)
   - Location modifiers (nearby hazards increase contamination)
   - Infrastructure modifiers (filtration reduces contamination)
   - Connection modifiers (contamination propagates)
   - Contamination trend calculated (improving/stable/worsening)
3. Implement contamination propagation:
   - Connected sources share contamination
   - Propagation calculated daily
   - Underground flow: slow propagation
   - Surface runoff: fast propagation during rain
   - Pipe connections: immediate propagation
   - Propagation logged
4. Implement water infrastructure:
   - Infrastructure installed at sources or shelter
   - Infrastructure has condition (degrades over time)
   - Infrastructure requires maintenance
   - Infrastructure affects water quality/flow
   - Infrastructure failures reduce water supply
5. Implement water testing:
   - Survivors can test water sources
   - Test requires water test kit item
   - Test reveals contamination level
   - Test accuracy based on kit quality
   - Test results logged
   - Test results shown in UI
6. Implement source switching:
   - Player can switch active water source
   - Switch takes time (plumbing reconfiguration)
   - Switch may require infrastructure (pipes/pumps)
   - Switch logged
   - Multiple sources can be active (blending)
7. Implement water supply/demand:
   - Total available water calculated from active sources
   - Water demand based on survivor count
   - Supply < demand: water shortage
   - Water shortage: dehydration risk, rationing
   - Supply > demand: surplus stored
8. Implement water quality effects:
   - High contamination water causes illness
   - Contaminant type affects disease (radiation/biological/chemical)
   - Boiling/purification reduces contamination
   - WaterTreatmentSystem integrates with source contamination
   - Contaminated water increases disease risk
9. Implement source maintenance:
   - Sources require periodic maintenance
   - Maintenance reduces contamination
   - Maintenance restores flow rate
   - Maintenance requires items/tools
   - Maintenance logged
10. Create water source events:
    - "The Discovery" — new water source found
    - "The Contamination" — source contamination detected
    - "The Shortage" — water supply below demand
    - "The Failure" — infrastructure failure
    - "The Test" — water test completed
    - "The Switch" — active source changed
    - "The Maintenance" — source maintained
    - "The Crisis" — all sources contaminated
11. Add water source quest hooks:
    - "The Hydrologist" — discover 5 water sources
    - "The Engineer" — install water infrastructure
    - "The Tester" — test all water sources
    - "The Manager" — maintain all sources above 80% quality
    - "The Crisis" — deal with total water contamination
    - "The Network" — connect 3 sources to shelter
    - "The Purifier" — reduce source contamination to 0
12. Implement water source UI:
    - Source list: all discovered sources with status
    - Source detail: contamination, flow, infrastructure, test history
    - Infrastructure panel: installed infrastructure, condition
    - Contamination map: show sources and connections
    - Water supply/demand indicator
13. Add water source journal: automatic log of water events
14. Implement water source tutorial: first water shortage explains system
15. Add water source tooltips: hover over source shows contamination, flow, last test

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `WaterTreatmentSystem`: source contamination feeds into treatment
2. Connect to `LocationEvolutionSystem`: location contamination affects sources
3. Integrate with `DiseaseSystem`: contaminated water causes disease
4. Connect to `WeatherSystem`: weather affects contamination
5. Wire into `ExpeditionSystem`: expeditions discover new sources
6. Connect to `GreenhouseSystem`: water source affects irrigation
7. Implement old-save compatibility: existing saves get default well source
8. Add deterministic seeding: contamination uses `ISeededRng`
9. Create exploit prevention: contamination is time/environment-based, can't be gamed
10. Add tests: source tracking, contamination dynamics, propagation, infrastructure, testing, save round-trip
11. Verify all source types work correctly
12. Test edge cases: no sources (water crisis), all sources contaminated, infrastructure failure
13. Verify headless behavior: water sources process correctly without UI
14. Add data-integrity-selftest: water sources validate against location/item catalogs
15. Create `--water-source-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --water-source-selftest
```

## Risk

**LOW** — Water source management is straightforward with clear inputs (contamination, flow, infrastructure) and outputs (water quality, supply). Risk of water management becoming tedious rather than strategic. Mitigation: make testing easy, show clear contamination trends, allow automation (infrastructure), and ensure source switching is meaningful.

## Definition of Done

- `WaterSourceSystem.cs` exists with full `CaptureState/RestoreState`
- 6 water source types (well, river, spring, rain collector, underground spring, municipal)
- Contamination tracking per source (0-1)
- Contamination propagation between connected sources
- Water infrastructure (pipes, pumps, storage tanks, filtration units, test kits)
- Water testing mechanics
- Source switching
- Water supply/demand tracking
- Water quality effects (disease risk)
- Source maintenance
- Water source events and quest hooks
- Save/load round-trip tested
- Deterministic contamination verified
- Old saves get default well source
- Water source definitions in data authority
- UI source list, detail view, infrastructure panel, contamination map
- Cross-system integration (water treatment, location evolution, disease, weather, expedition, greenhouse)

## Follow-On Opportunities

- Water source upgrades (deeper wells, better pumps)
- Water trading (sell/buy water from other settlements)
- Water purification research (advanced filtration)
- Water source legacy (famous wells remembered)
- Water source quests (specific source goals)
