# Plan 201 — Shelter Sanitation & Waste Management System

## Goal

Create a shelter sanitation and waste management system where the shelter accumulates waste (sewage, trash, hazardous materials, biological waste), requires sanitation infrastructure (latrines, waste processing, water recycling), and suffers consequences when sanitation fails (disease outbreaks, contamination, morale collapse). Currently `VentilationSystem.cs` (270 lines) handles air quality, `SumpFloodingSystem.cs` (298 lines) handles water infiltration, and `KitchenNutritionSystem.cs` handles food spoilage — but there is no sanitation system, no waste accumulation, no sewage management, no trash disposal, no hygiene tracking. The shelter produces waste but has no waste management. This plan adds environmental hygiene as a survival layer.

## Why

**Repository evidence:** Grep for `SanitationSystem`, `WasteManagement`, `HygieneSystem`, `SewageSystem`, `LatrineSystem`, `WasteDisposal`, `SanitationLevel`, `HygieneLevel`, `WasteAccumulation` in Core returns ZERO matches. `VentilationSystem.cs` (270 lines) tracks smoke/soot, CO, filter saturation. `SumpFloodingSystem.cs` (298 lines) tracks water infiltration per node. `DiseaseSystem.cs` has `SetAirFiltration` for airborne disease. But no sanitation, no waste, no sewage, no hygiene. Plan 158 (Disaster Emergency Response) mentions "poor sanitation" as a disaster trigger but doesn't implement sanitation mechanics.

**What is missing:** No sanitation system. No waste accumulation tracking. No sewage management. No trash disposal. No hygiene levels. No waste processing infrastructure. No sanitation-related disease vectors. The shelter produces waste (human, kitchen, medical, industrial) but nothing tracks it or requires management.

**Why existing plans don't solve it:** Plan 158 (disaster response) mentions sanitation as a disaster trigger but doesn't implement sanitation mechanics. Plan 29 (shelter as character) covers wear/degradation but not waste. Plan 156 (shelter expansion) covers construction but not sanitation infrastructure. No plan addresses sanitation/waste as a system.

**Player value:** Creates survival realism (sanitation is critical in post-nuclear survival), adds strategic depth (invest in sanitation infrastructure or risk disease), generates emergent problems (sanitation failure cascades into disease/morale), and makes shelter management more complete (air, water, food, AND waste).

## Files / Systems to Inspect

- `Assets/Ashfall.Core/VentilationSystem.cs` — air quality (complementary)
- `Assets/Ashfall.Core/SumpFloodingSystem.cs` — water system (complementary)
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — disease vectors
- `Assets/Ashfall.Core/Shelter/ShelterThermalSystem.cs` — shelter environment
- `Assets/Ashfall.Core/Needs/NeedsSystem.cs` — survivor needs (hygiene)
- NEW: `Assets/Ashfall.Core/Shelter/SanitationSystem.cs`
- NEW: `Assets/StreamingAssets/Data/sanitation_infrastructure.json`

## Main Task 1 — Foundation / System Contract

1. Create `SanitationSystem.cs` in `Assets/Ashfall.Core/Shelter/`
2. Define `SanitationState` DTO: `sanitationLevel` (0-100, overall shelter hygiene), `wasteAccumulation` (0-100, unprocessed waste), `sewageLevel` (0-100, sewage tank fill), `trashLevel` (0-100, unprocessed trash), `hazardousWaste` (0-100, medical/chemical waste), `waterRecyclingEfficiency` (0-100), `sanitationInfrastructure` (list of installed facilities), `lastSanitationCheck` (day), `sanitationEvents` (list of sanitation events)
3. Define `SanitationFacility` DTO: `facilityId`, `facilityType` (latrine/septic_tank/composting_toilet/waste_processor/water_recycler/trash_incinerator/hazardous_waste_containment), `capacity` (waste units per day), `efficiency` (0-100), `condition` (0-100, degrades over time), `installedDay`, `lastMaintenanceDay`, `isActive` bool
4. Define `WasteCategory` enum: `HumanWaste`, `KitchenWaste`, `MedicalWaste`, `ChemicalWaste`, `GeneralTrash`, `HazardousMaterial`
5. Define `WasteAccumulation` DTO: `category` (WasteCategory), `amount` (units), `accumulationRate` (units/day), `processingRate` (units/day from facilities), `netAccumulation` (units/day after processing)
6. Define `SanitationEvent` DTO: `eventId`, `eventType` (facility_installed/facility_broke_down/waste_overflow/sewage_backup/disease_outbreak/sanitation_inspection/facility_repaired), `day`, `description`, `severity` (mild/moderate/severe/critical), `effects` (list of consequences)
7. Define `HygieneLevel` DTO: `survivorId`, `hygieneScore` (0-100), `lastWashedDay`, `exposureToWaste` (0-100), `diseaseRisk` (0-100), `moraleImpact` (float)
8. Implement `CaptureState/RestoreState` with schema versioning
9. Define waste categories (6 types):
   - **Human Waste**: produced by survivors daily (latrine/septic demand), high disease risk if unprocessed
   - **Kitchen Waste**: food scraps, spoiled food, cooking byproducts (composting/incineration)
   - **Medical Waste**: used bandages, syringes, biological samples (hazardous, requires special containment)
   - **Chemical Waste**: industrial byproducts, cleaning chemicals, battery acid (hazardous, environmental contamination)
   - **General Trash**: packaging, broken items, non-recyclables (incineration/landfill)
   - **Hazardous Material**: radioactive contamination, toxic substances (special containment, decontamination)
10. Define sanitation facilities (7+ types):
    - **Latrine**: basic human waste processing, low capacity, requires manual emptying
    - **Septic Tank**: medium-capacity human waste processing, underground, requires periodic pumping
    - **Composting Toilet**: human waste → compost (can be used in greenhouse), medium capacity
    - **Waste Processor**: general waste → reduced volume, requires power
    - **Water Recycler**: greywater → clean water, requires power + filters
    - **Trash Incinerator**: burns general trash, reduces volume 90%, requires fuel
    - **Hazardous Waste Containment**: isolates hazardous materials, prevents contamination
11. Define sanitation mechanics:
    - Waste accumulates daily based on survivor count and activity
    - Facilities process waste (reduce accumulation)
    - Facilities degrade over time, require maintenance
    - Unprocessed waste: sanitation level drops
    - Low sanitation: disease risk increases, morale decreases
    - Critical sanitation: disease outbreaks, contamination events
12. Define sanitation consequences:
    - **High sanitation (80-100)**: clean shelter, low disease risk, morale bonus
    - **Moderate sanitation (50-79)**: acceptable conditions, minor morale penalty
    - **Low sanitation (20-49)**: visible waste, odor, disease risk, morale penalty
    - **Critical sanitation (0-19)**: disease outbreaks, contamination, morale collapse, vermin
13. Define hygiene per survivor:
    - Each survivor has hygiene score (0-100)
    - Hygiene decreases daily (sweat, dirt, exposure)
    - Hygiene increases with access to washing facilities
    - Low hygiene: morale penalty, disease risk, social penalty
    - Very low hygiene: visible filth, social ostracism, high disease risk
14. Define water recycling:
    - Greywater (from washing, cooking) can be recycled
    - Water recycler: greywater → clean water
    - Recycling efficiency affects water consumption
    - Failed recycling: water waste, contamination risk
15. Add deterministic seeding: sanitation events use `ISeededRng`
16. Wire into `GameBootstrap`: `SetupSanitation`, `TickSanitation`, `SaveSanitation`

## Main Task 2 — Implementation / Facilities / Waste / Hygiene / Disease / UI

1. Implement waste accumulation:
   - Each survivor produces waste daily (human, kitchen, general)
   - Medical system produces medical waste
   - Industrial jobs produce chemical waste
   - Waste categorized and accumulated
   - Accumulation logged
2. Implement sanitation facilities:
   - Facilities installed in shelter rooms
   - Each facility processes specific waste category
   - Facilities have capacity, efficiency, condition
   - Facilities degrade with use
   - Facilities require maintenance/repair
   - Facility breakdown: waste backs up
3. Implement sanitation level:
   - Overall shelter sanitation calculated from waste accumulation
   - Waste overflow: sanitation drops
   - Sewage backup: sanitation drops severely
   - Sanitation level displayed in UI
   - Sanitation alerts when low
4. Implement hygiene per survivor:
   - Each survivor has hygiene score
   - Hygiene decreases daily
   - Washing facilities restore hygiene
   - Low hygiene: morale penalty, disease risk
   - Hygiene displayed in survivor panel
5. Implement disease vectors:
   - Low sanitation: increased disease transmission
   - Contaminated water: waterborne disease
   - Poor hygiene: contact disease
   - Medical waste: biohazard exposure
   - Disease outbreaks from sanitation failure
6. Implement water recycling:
   - Greywater produced from washing/cooking
   - Water recycler processes greywater
   - Recycled water added to water supply
   - Recycler efficiency affects yield
   - Failed recycler: water loss
7. Implement sanitation events:
   - Facility breakdown: waste backs up
   - Waste overflow: visible waste, odor
   - Sewage backup: critical sanitation failure
   - Disease outbreak: from poor sanitation
   - Vermin infestation: from accumulated waste
   - Contamination event: hazardous waste leak
   - Sanitation inspection: assessment of facilities
8. Implement sanitation UI:
   - Sanitation panel: overall level, waste categories, facilities
   - Facility detail: capacity, efficiency, condition, maintenance
   - Waste overview: accumulation rates, processing rates
   - Hygiene panel: survivor hygiene scores
   - Alerts: low sanitation, facility breakdown, disease risk
   - Sanitation map: show facilities and waste flows
9. Create sanitation events:
    - "The Backup" — sewage system backed up
    - "The Overflow" — waste overflowing
    - "The Outbreak" — disease from poor sanitation
    - "The Inspection" — sanitation assessment
    - "The Breakdown" — facility broke down
    - "The Cleanup" — major sanitation effort
    - "The Recycler" — water recycler online
    - "The Crisis" — critical sanitation failure
10. Add sanitation quest hooks:
    - "The Plumber" — maintain 5 sanitation facilities
    - "The Clean" — keep sanitation above 80 for 100 days
    - "The Recycler" — recycle 1000 units of water
    - "The Hygienist" — keep all survivors above 70 hygiene
    - "The Engineer" — build complete sanitation infrastructure
    - "The Crisis Manager" — recover from critical sanitation
    - "The Zero Waste" — process all waste categories for 50 days
11. Implement sanitation tutorial: first waste overflow explains system
12. Add sanitation tooltips: hover over facility shows details
13. Create sanitation infrastructure definitions in data file
14. Implement sanitation persistence: facilities/waste saved with shelter state
15. Integrate with `DiseaseSystem`: sanitation affects disease transmission

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `VentilationSystem`: coordinate air quality and sanitation
2. Connect to `SumpFloodingSystem`: water contamination from poor sanitation
3. Integrate with `DiseaseSystem`: sanitation vectors for disease
4. Connect to `NeedsSystem`: hygiene need (if exists) or morale impact
5. Wire into `KitchenNutritionSystem`: kitchen waste production
6. Connect to `MedicalPipelineCoordinator`: medical waste production
7. Wire into `GreenhouseSystem`: composting toilet → fertilizer
8. Connect to `PowerGridSystem`: waste processors/recyclers need power
9. Implement old-save compatibility: existing saves get basic latrine, moderate sanitation
10. Add deterministic seeding: sanitation events use `ISeededRng`
11. Create exploit prevention: waste is automatic, can't be gamed
12. Add tests: waste accumulation, facility processing, sanitation level, hygiene, disease vectors, save round-trip
13. Verify all waste categories work correctly
14. Test edge cases: no facilities (current behavior), full infrastructure (clean shelter)
15. Verify headless behavior: sanitation processes correctly without UI
16. Add data-integrity-selftest: sanitation validates against shelter/room catalogs
17. Create `--sanitation-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --sanitation-selftest
```

## Risk

**LOW** — Sanitation is straightforward with clear inputs (waste production) and outputs (sanitation level, disease risk). Risk of sanitation feeling like another meter to manage. Mitigation: make consequences visible (disease outbreaks, morale collapse), show clear cause-effect, and ensure infrastructure investment pays off.

## Definition of Done

- `SanitationSystem.cs` exists with full `CaptureState/RestoreState`
- 6 waste categories (human, kitchen, medical, chemical, general, hazardous)
- 7+ sanitation facilities (latrine, septic, composting, processor, recycler, incinerator, containment)
- Waste accumulation and processing mechanics
- Sanitation level (0-100) with consequences
- Per-survivor hygiene tracking
- Disease vectors from poor sanitation
- Water recycling mechanics
- Sanitation events and quest hooks
- Save/load round-trip tested
- Deterministic sanitation events verified
- Old saves load with basic latrine, moderate sanitation
- Sanitation infrastructure in data authority
- UI sanitation panel, facility detail, waste overview, hygiene panel, alerts
- Cross-system integration (ventilation, sump, disease, needs, kitchen, medical, greenhouse, power)

## Follow-On Opportunities

- Sanitation specialization (survivors become expert plumbers/sanitation engineers)
- Sanitation legacy (famous sanitation crises remembered)
- Sanitation quests (specific sanitation goals)
- Sanitation events (massive waste spill, sewage catastrophe)
- Sanitation trading (trade compost/fertilizer with other settlements)
