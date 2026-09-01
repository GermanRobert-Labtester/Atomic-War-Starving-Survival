# Plan 198 — Health History & Medical Records System

## Goal

Create a comprehensive health history and medical records system where each survivor maintains a persistent medical history — tracking illnesses, injuries, treatments, vaccinations, radiation exposure, and long-term health trends over time. Currently medical systems treat each condition as an isolated event with no memory of past health issues. A survivor who recovered from radiation sickness last month has no record of it. A medic treating a patient cannot see their medical history. This plan adds medical continuity and makes healthcare more strategic and realistic.

## Why

**Repository evidence:** Grep for `HealthHistory`, `MedicalRecord`, `HealthTracking`, `LongTermHealth`, `MedicalHistory`, `HealthLog` in Core returns ZERO matches. Medical systems (`MedicalPipelineCoordinator`, `DiseaseSystem`, `RadiationSystem`, `CombatTraumaSystem`) treat conditions as isolated events — diagnose, treat, resolve. No persistent health tracking, no medical history, no treatment records, no vaccination history, no long-term health trends. `DoseLedgerSystem.cs` tracks radiation dose history but this is radiation-specific, not general health tracking.

**What is missing:** No medical history system. No treatment records. No vaccination history. No long-term health tracking. No medical charts per survivor. Medics cannot see patient history. Players cannot track health trends. Each medical event is isolated with no memory.

**Why existing plans don't solve it:** Plan 193 (chronic conditions) adds permanent impairments but not medical history. Plan 179 (psychology) adds psychological profiles but not medical records. Plan 172 (radiation mutations) adds genetic changes but not health tracking. No plan addresses medical history as a system.

**Player value:** Creates strategic depth (medics can make better decisions with history), adds realism (health has continuity), generates emergent stories (survivor with extensive medical history), and makes healthcare more meaningful than just "treat current condition."

## Files / Systems to Inspect

- `Assets/Ashfall.Core/Medical/MedicalPipelineCoordinator.cs` — medical pipeline
- `Assets/Ashfall.Core/Disease/DiseaseSystem.cs` — disease system
- `Assets/Ashfall.Core/Radiation/RadiationSystem.cs` — radiation system
- `Assets/Ashfall.Core/Radiation/DoseLedgerSystem.cs` — radiation dose tracking
- NEW: `Assets/Ashfall.Core/Medical/HealthHistorySystem.cs`
- NEW: `Assets/StreamingAssets/Data/medical_record_templates.json`

## Main Task 1 — Foundation / System Contract

1. Create `HealthHistorySystem.cs` in `Assets/Ashfall.Core/Medical/`
2. Define `MedicalRecord` DTO: `recordId`, `survivorId`, `recordType` (illness/injury/treatment/vaccination/radiation_exposure/chronic_condition/checkup), `recordedDay`, `description`, `severity` (mild/moderate/severe/critical), `duration` (days), `outcome` (resolved/ongoing/chronic/fatal), `treatmentApplied` (list of treatment_ids), `treatingSurvivorId` (medic who treated), `notes` (additional details)
3. Define `HealthEvent` DTO: `eventId`, `survivorId`, `eventType` (diagnosis/treatment/recovery/relapse/complication/vaccination/checkup), `eventDay`, `description`, `relatedCondition` (condition_id if applicable), `outcome` (success/partial/failure), `notes`
4. Define `VaccinationRecord` DTO: `vaccinationId`, `survivorId`, `vaccineType` (disease_id or vaccine_id), `administeredDay`, `administeredBySurvivorId`, `immunityLevel` (0-100), `immunityDuration` (days), `boosterDue` (day)
5. Define `HealthTrend` DTO: `trendId`, `survivorId`, `healthMetric` (overall_health/radiation_dose/immune_strength/chronic_condition_count), `measurementDay`, `value` (0-100 or specific value), `trend` (improving/stable/declining)
6. Define `HealthHistoryState` DTO: list of medical records per survivor, list of health events, list of vaccination records, list of health trends, health history settings (auto-record treatments bool, show trends bool)
7. Implement `CaptureState/RestoreState` with schema versioning
8. Define medical record types (7+ types):
   - **Illness**: disease diagnosed, symptoms, duration, treatment, outcome
   - **Injury**: wound/fracture/trauma, severity, treatment, recovery time
   - **Treatment**: medical procedure performed, medications administered
   - **Vaccination**: vaccine administered, immunity level, booster schedule
   - **Radiation Exposure**: dose received, decontamination treatment, long-term effects
   - **Chronic Condition**: ongoing condition, management plan, progression
   - **Checkup**: routine medical examination, findings, recommendations
9. Define health event types (7+ types):
   - **Diagnosis**: condition identified
   - **Treatment**: medical intervention applied
   - **Recovery**: condition resolved
   - **Relapse**: condition returned after recovery
   - **Complication**: secondary condition from primary
   - **Vaccination**: vaccine administered
   - **Checkup**: routine examination completed
10. Define medical record retention:
    - Medical records persist for entire survivor lifetime
    - Records organized chronologically
    - Records searchable by type, date, condition
    - Records displayable in UI
    - Records exportable (for reference)
11. Define health trend tracking:
    - Overall health metric (0-100, composite of all conditions)
    - Radiation dose tracking (cumulative, from `DoseLedgerSystem`)
    - Immune strength (0-100, based on vaccinations/illnesses)
    - Chronic condition count (number of active chronic conditions)
    - Trends calculated daily
    - Trends displayed as graphs in UI
12. Define medic integration:
    - Medics can view patient medical history
    - History informs treatment decisions
    - Previous treatments affect current treatment effectiveness
    - Allergies/contraindications tracked
    - Medic notes added to records
13. Define vaccination system:
    - Vaccines provide immunity to specific diseases
    - Immunity level (0-100) decreases over time
    - Booster shots restore immunity
    - Vaccination records tracked
    - Immunity affects disease susceptibility
14. Add deterministic seeding: health events use `ISeededRng`
15. Wire into `GameBootstrap`: `SetupHealthHistory`, `TickHealthHistory`, `SaveHealthHistory`

## Main Task 2 — Implementation / Records / Events / Trends / Vaccinations / UI

1. Implement medical record creation:
   - Auto-create records for illnesses, injuries, treatments
   - Manual creation for checkups, notes
   - Records include: type, date, description, severity, treatment, outcome
   - Records linked to survivor
   - Records stored in health history
2. Implement health event tracking:
   - Auto-log health events (diagnosis, treatment, recovery, etc.)
   - Events linked to medical records
   - Events include: type, date, description, outcome
   - Events stored in health history
3. Implement vaccination system:
   - Vaccines administered to survivors
   - Vaccination records created
   - Immunity level tracked
   - Immunity decreases over time
   - Booster shots restore immunity
   - Vaccination status displayed
4. Implement health trend tracking:
   - Calculate health metrics daily
   - Overall health (composite of conditions)
   - Radiation dose (from `DoseLedgerSystem`)
   - Immune strength (from vaccinations)
   - Chronic condition count
   - Trends stored and displayed
5. Implement medic integration:
   - Medics can view patient history
   - History displayed in medical panel
   - Previous treatments affect current treatment
   - Allergies/contraindications checked
   - Medic can add notes to records
6. Implement medical UI:
   - Health history panel: all records per survivor
   - Medical chart: chronological view of health events
   - Vaccination panel: vaccination status, boosters due
   - Health trends: graphs of health metrics over time
   - Medic panel: patient history, treatment options
7. Implement record search/filter:
   - Search records by type (illness, injury, treatment, etc.)
   - Filter by date range
   - Filter by condition
   - Filter by severity
   - Sort by date, severity, outcome
8. Implement record export:
   - Export medical records to text/JSON
   - Share records between medics
   - Archive records for deceased survivors
   - Export for reference
9. Implement health alerts:
   - Alert when vaccination booster due
   - Alert when health trend declining
   - Alert when chronic condition worsening
   - Alert when radiation dose high
   - Alerts displayed in UI
10. Create health events:
    - "The Diagnosis" — condition diagnosed
    - "The Treatment" — treatment administered
    - "The Recovery" — condition resolved
    - "The Relapse" — condition returned
    - "The Vaccination" — vaccine administered
    - "The Checkup" — routine examination
    - "The Trend" — health trend changed
    - "The History" — medical history completed
11. Add health quest hooks:
    - "The Medic" — treat 20 patients
    - "The Historian" — maintain complete medical records for 10 survivors
    - "The Vaccinator" — vaccinate all survivors against common diseases
    - "The Trend" — improve health trend for 5 survivors
    - "The Checkup" — perform 50 routine checkups
    - "The Record" — maintain medical records for 100 days
    - "The Prevention" — prevent 10 illnesses through vaccination
12. Implement health tutorial: first medical record explains system
13. Add health tooltips: hover over record shows details
14. Create medical record templates in data file
15. Implement health persistence: records saved with survivor state

## Main Task 3 — Integration / Consequences / Validation

1. Wire into `MedicalPipelineCoordinator`: medical records created for treatments
2. Connect to `DiseaseSystem`: illness records created for diseases
3. Integrate with `RadiationSystem`: radiation exposure records created
4. Connect to `DoseLedgerSystem`: radiation dose integrated into health trends
5. Wire into `CombatTraumaSystem`: injury records created for combat wounds
6. Connect to `ChronicConditionSystem` (Plan 193): chronic condition records created
7. Implement old-save compatibility: existing saves get empty health history
8. Add deterministic seeding: health events use `ISeededRng`
9. Create exploit prevention: health records are automatic, can't be gamed
10. Add tests: medical records, health events, vaccinations, trends, medic integration, save round-trip
11. Verify all record types work correctly
12. Test edge cases: no records (healthy survivor), extensive records (chronically ill)
13. Verify headless behavior: health history processes correctly without UI
14. Add data-integrity-selftest: health records validate against medical catalogs
15. Create `--health-history-selftest` verb for CI validation

## Verification

```bash
dotnet build Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet test Ashfall.Core.Tests/Ashfall.Core.Tests.csproj
dotnet build Ashfall.csproj
godot --headless --path . -- --data-integrity-selftest
godot --headless --path . -- --health-history-selftest
```

## Risk

**LOW** — Health history is straightforward with clear inputs (medical events) and outputs (records, trends). Risk of health tracking feeling like record-keeping chore. Mitigation: auto-generate records, show clear trends, provide useful alerts, and ensure history informs gameplay (better treatment decisions).

## Definition of Done

- `HealthHistorySystem.cs` exists with full `CaptureState/RestoreState`
- 7+ medical record types (illness, injury, treatment, vaccination, radiation, chronic, checkup)
- 7+ health event types (diagnosis, treatment, recovery, relapse, complication, vaccination, checkup)
- Vaccination system (immunity levels, boosters, duration)
- Health trend tracking (overall health, radiation dose, immune strength, chronic count)
- Medic integration (view patient history, inform treatment decisions)
- Medical UI (history panel, medical chart, vaccination panel, trends)
- Record search/filter (by type, date, condition, severity)
- Health alerts (booster due, trend declining, condition worsening)
- Health events and quest hooks
- Save/load round-trip tested
- Deterministic health events verified
- Old saves load with empty health history
- Medical record templates in data authority
- Cross-system integration (medical pipeline, disease, radiation, dose ledger, combat trauma, chronic conditions)

## Follow-On Opportunities

- Health specialization (survivors become expert medics with better record-keeping)
- Health legacy (famous medical cases remembered)
- Health quests (specific health goals)
- Health events (medical breakthroughs, epidemics)
- Health trading (share medical knowledge between settlements)
