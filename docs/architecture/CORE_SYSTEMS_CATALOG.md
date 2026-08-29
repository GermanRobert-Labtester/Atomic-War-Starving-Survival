# ASHFALL Core Domain Subsystems & Host Seams Catalog

**Authoritative Architecture Map** | **Generated:** 2026-08-29 | **Systems Documented:** 34

> [!IMPORTANT]
> **ARCHITECTURE INVARIANTS (Invariants 1 & 5):**
> 1. `Assets/Ashfall.Core/` contains **zero engine coupling** (`UnityEngine`, `Godot`, `JsonUtility`). All gameplay logic lives here.
> 2. `src/Host/` contains **thin host sessions** inheriting from `HostSessionBase` (`StatefulSessionBase`) that handle only presentation and wiring.
> 3. Save persistence is owned by `SaveStore<T>` via `SaveStoreHub.cs` and packed into the single atomic `campaign.json` envelope.

---

## Subsystem Seam Matrix Table

| Domain | Core System Class | Owning Host Session | Save Section Key | Data Feed | CLI Diagnostic Verb |
|---|---|---|---|---|---|
| Shelter & Thermal | `ShelterThermalSystem` | `ShelterThermalHostSession.cs` | `shelter_thermal` | `shelter_schedules.json` | `--shelter-thermal-selftest` |
| Shelter & Flooding | `SumpFloodingSystem` | `SumpFloodingHostSession.cs` | `sump_flooding` | `sump_flooding.json` | `--sump-flooding-selftest` |
| Shelter & Security | `AirlockSecuritySystem` | `AirlockSecurityHostSession.cs` | `airlock_security` | `airlock_protocols.json` | `--airlock-security-selftest` |
| Shelter & Ventilation | `VentilationSystem` | `VentilationHostSession.cs` | `ventilation` | `ventilation_grid.json` | `--ventilation-selftest` |
| Water & Sanitation | `WaterTreatmentSystem` | `WaterTreatmentHostSession.cs` | `water_treatment` | `water_treatment.json` | `--water-treatment-selftest` |
| Water & Chemistry | `BrineWaterSystem` | `BrineWaterHostSession.cs` | `brine_water` | `brine_recipes.json` | `--brine-water-selftest` |
| Medical & Dosimetry | `DoseLedgerSystem` | `DoseLedgerHostSession.cs` | `dose_ledger` | `dose_registers.json` | `--dose-ledger-selftest` |
| Medical & Chemical | `ChemicalDependencySystem` | `ChemicalDependencyHostSession.cs` | `chemical_dependency` | `chemical_dependency.json` | `--chemical-dependency-selftest` |
| Medical & Pharmaceuticals | `PharmaLabSystem` | `PharmaLabHostSession.cs` | `pharma_lab` | `pharma_recipes.json` | `--pharma-lab-selftest` |
| Medical & Pathology | `AutopsySystem` | `AutopsyHostSession.cs` | `autopsy` | `autopsy_procedures.json` | `--autopsy-selftest` |
| Medical & Hospital | `SickListSystem` | `MedicalWardHostSession.cs` | `medical_ward` | `disease_catalog.json` | `--medical-ward-selftest` |
| Survivors & Caregiving | `CaregivingSystem` | `CaregivingHostSession.cs` | `caregiving` | `survivors.json` | `--caregiving-selftest` |
| Survivors & Apprenticeship | `ApprenticeshipSystem` | `ApprenticeshipHostSession.cs` | `apprenticeship` | `skills.json` | `--apprenticeship-selftest` |
| Survivors & Relations | `SurvivorRelationsSystem` | `SurvivorRelationsHostSession.cs` | `survivor_relations` | `survivors.json` | `--survivor-relations-selftest` |
| Survivors & Psychology | `MentalHealthCrisisSystem` | `MentalHealthCrisisHostSession.cs` | `mental_health` | `psychological_traits.json` | `--mental-health-selftest` |
| Expeditions & Vehicles | `ExpeditionVehicleSystem` | `ExpeditionHostSession.cs` | `expedition` | `vehicles.json` | `--expedition-selftest` |
| Expeditions & Logistics | `IceRoadSystem` | `IceRoadHostSession.cs` | `ice_road` | `ice_roads.json` | `--ice-road-selftest` |
| Expeditions & Outposts | `WaystationSystem` | `WaystationHostSession.cs` | `waystation` | `waystations.json` | `--waystation-selftest` |
| Expeditions & Deep Coast | `District8DeepCoastSystem` | `DeepCoastHostSession.cs` | `deep_coast` | `deep_coast_nodes.json` | `--deep-coast-selftest` |
| Economy & Trade | `TravelingCaravanSystem` | `TravelingCaravanHostSession.cs` | `traveling_caravan` | `caravan_routes.json` | `--traveling-caravan-selftest` |
| Economy & Finance | `LedgerDebtSystem` | `EconomyHostSession.cs` | `economy` | `market_goods.json` | `--economy-selftest` |
| Crafting & Industry | `WorkshopReverseEngineeringSystem` | `CraftingHostSession.cs` | `crafting` | `recipes.json` | `--crafting-selftest` |
| Research & Archives | `LibraryStudySystem` | `LibraryStudyHostSession.cs` | `library_study` | `library_manuals.json` | `--library-study-selftest` |
| Research & Scribes | `ArchiveDeskSystem` | `ArchiveDeskHostSession.cs` | `archive_desk` | `archive_inks.json` | `--archive-desk-selftest` |
| World & Meteorology | `WeatherStationSystem` | `WeatherHostSession.cs` | `weather` | `weather_events.json` | `--weather-selftest` |
| World & Wildlife | `WildlifeTrappingSystem` | `WildlifeTrappingHostSession.cs` | `wildlife_trapping` | `wildlife_species.json` | `--wildlife-trapping-selftest` |
| World & Ecology | `WildlifeMigrationSystem` | `WorldHostSession.cs` | `world` | `wildlife_species.json` | `--world-selftest` |
| World & Landmarks | `LandmarkDegradationSystem` | `WorldHostSession.cs` | `world` | `landmarks.json` | `--world-selftest` |
| Factions & Treaties | `RegionalTreatySystem` | `RegionalTreatyHostSession.cs` | `regional_treaty` | `faction_treaties.json` | `--regional-treaty-selftest` |
| Expansion 01 (Holdfast) | `HoldfastQuestSystem` | `HoldfastRuntimeSession.cs` | `holdfast` | `holdfast_quests.json` | `--holdfast-selftest` |
| Expansion 02 (Duty Roster) | `DutyRosterSystem` | `DutyRosterHostSession.cs` | `duty_roster` | `duty_roster_shifts.json` | `--duty-roster-selftest` |
| Expansion 03 (Standing Record) | `StandingRecordSystem` | `StandingRecordHostSession.cs` | `standing_record` | `standing_records.json` | `--standing-record-selftest` |
| Expansion 04 (Crossing) | `CrossingArbitrationSystem` | `ExpansionHostSession.cs` | `expansion_hub` | `crossing_quests.json` | `--crossing-selftest` |
| Expansion 08 (Verdict) | `VerdictSystem` | `VerdictHostSession.cs` | `verdict` | `verdict_trials.json` | `--verdict-selftest` |

---

## Detailed Domain Seam Specifications

### ShelterThermalSystem (Shelter & Thermal)

- **Source File:** [`Assets/Ashfall.Core/ShelterThermalSystem.cs`](../../Assets/Ashfall.Core/ShelterThermalSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ShelterThermalHostSession.cs`](../../src/Host/ShelterThermalHostSession.cs)
- **Save Store Façade:** [`src/Host/ShelterThermalSaveStore.cs`](../../src/Host/ShelterThermalSaveStore.cs)
- **Persisted State DTO:** `ShelterThermalSaveState` (Section: `shelter_thermal`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/shelter_schedules.json`
- **Key Domain Events:** `OnTemperatureCritical, OnHeaterStateChanged`
- **CLI Verification Command:** `godot --headless --path . -- --shelter-thermal-selftest`

### SumpFloodingSystem (Shelter & Flooding)

- **Source File:** [`Assets/Ashfall.Core/SumpFloodingSystem.cs`](../../Assets/Ashfall.Core/SumpFloodingSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/SumpFloodingHostSession.cs`](../../src/Host/SumpFloodingHostSession.cs)
- **Save Store Façade:** [`src/Host/SumpFloodingSaveStore.cs`](../../src/Host/SumpFloodingSaveStore.cs)
- **Persisted State DTO:** `SumpFloodingState` (Section: `sump_flooding`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/sump_flooding.json`
- **Key Domain Events:** `OnWaterLevelChanged, OnPumpFailure`
- **CLI Verification Command:** `godot --headless --path . -- --sump-flooding-selftest`

### AirlockSecuritySystem (Shelter & Security)

- **Source File:** [`Assets/Ashfall.Core/AirlockSecuritySystem.cs`](../../Assets/Ashfall.Core/AirlockSecuritySystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/AirlockSecurityHostSession.cs`](../../src/Host/AirlockSecurityHostSession.cs)
- **Save Store Façade:** [`src/Host/AirlockSecuritySaveStore.cs`](../../src/Host/AirlockSecuritySaveStore.cs)
- **Persisted State DTO:** `AirlockSecurityState` (Section: `airlock_security`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/airlock_protocols.json`
- **Key Domain Events:** `OnBreachAlert, OnCycleComplete`
- **CLI Verification Command:** `godot --headless --path . -- --airlock-security-selftest`

### VentilationSystem (Shelter & Ventilation)

- **Source File:** [`Assets/Ashfall.Core/VentilationSystem.cs`](../../Assets/Ashfall.Core/VentilationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/VentilationHostSession.cs`](../../src/Host/VentilationHostSession.cs)
- **Save Store Façade:** [`src/Host/VentilationSaveStore.cs`](../../src/Host/VentilationSaveStore.cs)
- **Persisted State DTO:** `VentilationState` (Section: `ventilation`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/ventilation_grid.json`
- **Key Domain Events:** `OnFilterDegraded, OnAirflowBlocked`
- **CLI Verification Command:** `godot --headless --path . -- --ventilation-selftest`

### WaterTreatmentSystem (Water & Sanitation)

- **Source File:** [`Assets/Ashfall.Core/WaterTreatmentSystem.cs`](../../Assets/Ashfall.Core/WaterTreatmentSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WaterTreatmentHostSession.cs`](../../src/Host/WaterTreatmentHostSession.cs)
- **Save Store Façade:** [`src/Host/WaterTreatmentSaveStore.cs`](../../src/Host/WaterTreatmentSaveStore.cs)
- **Persisted State DTO:** `WaterTreatmentState` (Section: `water_treatment`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/water_treatment.json`
- **Key Domain Events:** `OnContaminationAlert, OnOutputProcessed`
- **CLI Verification Command:** `godot --headless --path . -- --water-treatment-selftest`

### BrineWaterSystem (Water & Chemistry)

- **Source File:** [`Assets/Ashfall.Core/BrineWaterSystem.cs`](../../Assets/Ashfall.Core/BrineWaterSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/BrineWaterHostSession.cs`](../../src/Host/BrineWaterHostSession.cs)
- **Save Store Façade:** [`src/Host/BrineWaterSaveStore.cs`](../../src/Host/BrineWaterSaveStore.cs)
- **Persisted State DTO:** `BrineWaterState` (Section: `brine_water`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/brine_recipes.json`
- **Key Domain Events:** `OnSalinityChanged, OnMineralHarvested`
- **CLI Verification Command:** `godot --headless --path . -- --brine-water-selftest`

### DoseLedgerSystem (Medical & Dosimetry)

- **Source File:** [`Assets/Ashfall.Core/DoseLedgerSystem.cs`](../../Assets/Ashfall.Core/DoseLedgerSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/DoseLedgerHostSession.cs`](../../src/Host/DoseLedgerHostSession.cs)
- **Save Store Façade:** [`src/Host/DoseLedgerSaveStore.cs`](../../src/Host/DoseLedgerSaveStore.cs)
- **Persisted State DTO:** `DoseLedgerSaveState` (Section: `dose_ledger`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/dose_registers.json`
- **Key Domain Events:** `OnRadiationTierChanged, OnRadDoseLogged`
- **CLI Verification Command:** `godot --headless --path . -- --dose-ledger-selftest`

### ChemicalDependencySystem (Medical & Chemical)

- **Source File:** [`Assets/Ashfall.Core/ChemicalDependencySystem.cs`](../../Assets/Ashfall.Core/ChemicalDependencySystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ChemicalDependencyHostSession.cs`](../../src/Host/ChemicalDependencyHostSession.cs)
- **Save Store Façade:** [`src/Host/ChemicalDependencySaveStore.cs`](../../src/Host/ChemicalDependencySaveStore.cs)
- **Persisted State DTO:** `ChemicalDependencySaveState` (Section: `chemical_dependency`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/chemical_dependency.json`
- **Key Domain Events:** `OnWithdrawalOnset, OnToleranceShift`
- **CLI Verification Command:** `godot --headless --path . -- --chemical-dependency-selftest`

### PharmaLabSystem (Medical & Pharmaceuticals)

- **Source File:** [`Assets/Ashfall.Core/PharmaLabSystem.cs`](../../Assets/Ashfall.Core/PharmaLabSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/PharmaLabHostSession.cs`](../../src/Host/PharmaLabHostSession.cs)
- **Save Store Façade:** [`src/Host/PharmaLabSaveStore.cs`](../../src/Host/PharmaLabSaveStore.cs)
- **Persisted State DTO:** `PharmaLabSaveState` (Section: `pharma_lab`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/pharma_recipes.json`
- **Key Domain Events:** `OnCompoundSynthesized, OnReagentDepleted`
- **CLI Verification Command:** `godot --headless --path . -- --pharma-lab-selftest`

### AutopsySystem (Medical & Pathology)

- **Source File:** [`Assets/Ashfall.Core/AutopsySystem.cs`](../../Assets/Ashfall.Core/AutopsySystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/AutopsyHostSession.cs`](../../src/Host/AutopsyHostSession.cs)
- **Save Store Façade:** [`src/Host/AutopsySaveStore.cs`](../../src/Host/AutopsySaveStore.cs)
- **Persisted State DTO:** `AutopsyState` (Section: `autopsy`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/autopsy_procedures.json`
- **Key Domain Events:** `OnPathologyDiscovered, OnBiohazardFlagged`
- **CLI Verification Command:** `godot --headless --path . -- --autopsy-selftest`

### SickListSystem (Medical & Hospital)

- **Source File:** [`Assets/Ashfall.Core/SickListSystem.cs`](../../Assets/Ashfall.Core/SickListSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/MedicalWardHostSession.cs`](../../src/Host/MedicalWardHostSession.cs)
- **Save Store Façade:** [`src/Host/MedicalWardSaveStore.cs`](../../src/Host/MedicalWardSaveStore.cs)
- **Persisted State DTO:** `SickListState` (Section: `medical_ward`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/disease_catalog.json`
- **Key Domain Events:** `OnPatientAdmitted, OnTriageUpdated`
- **CLI Verification Command:** `godot --headless --path . -- --medical-ward-selftest`

### CaregivingSystem (Survivors & Caregiving)

- **Source File:** [`Assets/Ashfall.Core/CaregivingSystem.cs`](../../Assets/Ashfall.Core/CaregivingSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/CaregivingHostSession.cs`](../../src/Host/CaregivingHostSession.cs)
- **Save Store Façade:** [`src/Host/CaregivingSaveStore.cs`](../../src/Host/CaregivingSaveStore.cs)
- **Persisted State DTO:** `CaregivingState` (Section: `caregiving`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/survivors.json`
- **Key Domain Events:** `OnCaregiverAssigned, OnMoraleBoosted`
- **CLI Verification Command:** `godot --headless --path . -- --caregiving-selftest`

### ApprenticeshipSystem (Survivors & Apprenticeship)

- **Source File:** [`Assets/Ashfall.Core/ApprenticeshipSystem.cs`](../../Assets/Ashfall.Core/ApprenticeshipSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ApprenticeshipHostSession.cs`](../../src/Host/ApprenticeshipHostSession.cs)
- **Save Store Façade:** [`src/Host/ApprenticeshipSaveStore.cs`](../../src/Host/ApprenticeshipSaveStore.cs)
- **Persisted State DTO:** `ApprenticeshipState` (Section: `apprenticeship`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/skills.json`
- **Key Domain Events:** `OnSkillMastered, OnMentorshipFormed`
- **CLI Verification Command:** `godot --headless --path . -- --apprenticeship-selftest`

### SurvivorRelationsSystem (Survivors & Relations)

- **Source File:** [`Assets/Ashfall.Core/SurvivorRelationsSystem.cs`](../../Assets/Ashfall.Core/SurvivorRelationsSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/SurvivorRelationsHostSession.cs`](../../src/Host/SurvivorRelationsHostSession.cs)
- **Save Store Façade:** [`src/Host/SurvivorRelationsSaveStore.cs`](../../src/Host/SurvivorRelationsSaveStore.cs)
- **Persisted State DTO:** `SurvivorRelationsState` (Section: `survivor_relations`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/survivors.json`
- **Key Domain Events:** `OnAffinityChanged, OnRivalryTriggered`
- **CLI Verification Command:** `godot --headless --path . -- --survivor-relations-selftest`

### MentalHealthCrisisSystem (Survivors & Psychology)

- **Source File:** [`Assets/Ashfall.Core/MentalHealthCrisisSystem.cs`](../../Assets/Ashfall.Core/MentalHealthCrisisSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/MentalHealthCrisisHostSession.cs`](../../src/Host/MentalHealthCrisisHostSession.cs)
- **Save Store Façade:** [`src/Host/MentalHealthSaveStore.cs`](../../src/Host/MentalHealthSaveStore.cs)
- **Persisted State DTO:** `MentalHealthState` (Section: `mental_health`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/psychological_traits.json`
- **Key Domain Events:** `OnBreakdownOccurred, OnStabilizationAchieved`
- **CLI Verification Command:** `godot --headless --path . -- --mental-health-selftest`

### ExpeditionVehicleSystem (Expeditions & Vehicles)

- **Source File:** [`Assets/Ashfall.Core/ExpeditionVehicleSystem.cs`](../../Assets/Ashfall.Core/ExpeditionVehicleSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ExpeditionHostSession.cs`](../../src/Host/ExpeditionHostSession.cs)
- **Save Store Façade:** [`src/Host/ExpeditionSaveStore.cs`](../../src/Host/ExpeditionSaveStore.cs)
- **Persisted State DTO:** `ExpeditionVehicleState` (Section: `expedition`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/vehicles.json`
- **Key Domain Events:** `OnVehicleBreakdown, OnFuelDepleted, OnDispatch`
- **CLI Verification Command:** `godot --headless --path . -- --expedition-selftest`

### IceRoadSystem (Expeditions & Logistics)

- **Source File:** [`Assets/Ashfall.Core/IceRoadSystem.cs`](../../Assets/Ashfall.Core/IceRoadSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/IceRoadHostSession.cs`](../../src/Host/IceRoadHostSession.cs)
- **Save Store Façade:** [`src/Host/IceRoadSaveStore.cs`](../../src/Host/IceRoadSaveStore.cs)
- **Persisted State DTO:** `IceRoadSaveState` (Section: `ice_road`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/ice_roads.json`
- **Key Domain Events:** `OnRouteThawed, OnConvoyAmbushed`
- **CLI Verification Command:** `godot --headless --path . -- --ice-road-selftest`

### WaystationSystem (Expeditions & Outposts)

- **Source File:** [`Assets/Ashfall.Core/WaystationSystem.cs`](../../Assets/Ashfall.Core/WaystationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WaystationHostSession.cs`](../../src/Host/WaystationHostSession.cs)
- **Save Store Façade:** [`src/Host/WaystationSaveStore.cs`](../../src/Host/WaystationSaveStore.cs)
- **Persisted State DTO:** `WaystationState` (Section: `waystation`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/waystations.json`
- **Key Domain Events:** `OnOutpostUpgraded, OnCacheReplenished`
- **CLI Verification Command:** `godot --headless --path . -- --waystation-selftest`

### District8DeepCoastSystem (Expeditions & Deep Coast)

- **Source File:** [`Assets/Ashfall.Core/District8DeepCoastSystem.cs`](../../Assets/Ashfall.Core/District8DeepCoastSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/DeepCoastHostSession.cs`](../../src/Host/DeepCoastHostSession.cs)
- **Save Store Façade:** [`src/Host/DeepCoastSaveStore.cs`](../../src/Host/DeepCoastSaveStore.cs)
- **Persisted State DTO:** `DeepCoastSaveState` (Section: `deep_coast`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/deep_coast_nodes.json`
- **Key Domain Events:** `OnTideShift, OnWreckSalvaged`
- **CLI Verification Command:** `godot --headless --path . -- --deep-coast-selftest`

### TravelingCaravanSystem (Economy & Trade)

- **Source File:** [`Assets/Ashfall.Core/TravelingCaravanSystem.cs`](../../Assets/Ashfall.Core/TravelingCaravanSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/TravelingCaravanHostSession.cs`](../../src/Host/TravelingCaravanHostSession.cs)
- **Save Store Façade:** [`src/Host/TravelingCaravanSaveStore.cs`](../../src/Host/TravelingCaravanSaveStore.cs)
- **Persisted State DTO:** `TravelingCaravanState` (Section: `traveling_caravan`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/caravan_routes.json`
- **Key Domain Events:** `OnCaravanArrived, OnTradeCompleted`
- **CLI Verification Command:** `godot --headless --path . -- --traveling-caravan-selftest`

### LedgerDebtSystem (Economy & Finance)

- **Source File:** [`Assets/Ashfall.Core/LedgerDebtSystem.cs`](../../Assets/Ashfall.Core/LedgerDebtSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/EconomyHostSession.cs`](../../src/Host/EconomyHostSession.cs)
- **Save Store Façade:** [`src/Host/EconomySaveStore.cs`](../../src/Host/EconomySaveStore.cs)
- **Persisted State DTO:** `LedgerDebtState` (Section: `economy`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/market_goods.json`
- **Key Domain Events:** `OnDebtDefaulted, OnInterestCompounded`
- **CLI Verification Command:** `godot --headless --path . -- --economy-selftest`

### WorkshopReverseEngineeringSystem (Crafting & Industry)

- **Source File:** [`Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs`](../../Assets/Ashfall.Core/WorkshopReverseEngineeringSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/CraftingHostSession.cs`](../../src/Host/CraftingHostSession.cs)
- **Save Store Façade:** [`src/Host/CraftingSaveStore.cs`](../../src/Host/CraftingSaveStore.cs)
- **Persisted State DTO:** `WorkshopState` (Section: `crafting`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/recipes.json`
- **Key Domain Events:** `OnSchematicUnlocked, OnPrototypeCrafted`
- **CLI Verification Command:** `godot --headless --path . -- --crafting-selftest`

### LibraryStudySystem (Research & Archives)

- **Source File:** [`Assets/Ashfall.Core/LibraryStudySystem.cs`](../../Assets/Ashfall.Core/LibraryStudySystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/LibraryStudyHostSession.cs`](../../src/Host/LibraryStudyHostSession.cs)
- **Save Store Façade:** [`src/Host/LibraryStudySaveStore.cs`](../../src/Host/LibraryStudySaveStore.cs)
- **Persisted State DTO:** `LibraryStudyState` (Section: `library_study`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/library_manuals.json`
- **Key Domain Events:** `OnKnowledgeGained, OnManualDecoded`
- **CLI Verification Command:** `godot --headless --path . -- --library-study-selftest`

### ArchiveDeskSystem (Research & Scribes)

- **Source File:** [`Assets/Ashfall.Core/ArchiveDeskSystem.cs`](../../Assets/Ashfall.Core/ArchiveDeskSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ArchiveDeskHostSession.cs`](../../src/Host/ArchiveDeskHostSession.cs)
- **Save Store Façade:** [`src/Host/ArchiveDeskSaveStore.cs`](../../src/Host/ArchiveDeskSaveStore.cs)
- **Persisted State DTO:** `ArchiveDeskState` (Section: `archive_desk`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/archive_inks.json`
- **Key Domain Events:** `OnRecordCataloged, OnMapDrawn`
- **CLI Verification Command:** `godot --headless --path . -- --archive-desk-selftest`

### WeatherStationSystem (World & Meteorology)

- **Source File:** [`Assets/Ashfall.Core/WeatherStationSystem.cs`](../../Assets/Ashfall.Core/WeatherStationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WeatherHostSession.cs`](../../src/Host/WeatherHostSession.cs)
- **Save Store Façade:** [`src/Host/WeatherSaveStore.cs`](../../src/Host/WeatherSaveStore.cs)
- **Persisted State DTO:** `WeatherStationState` (Section: `weather`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/weather_events.json`
- **Key Domain Events:** `OnStormApproaching, OnFalloutPlumeDetected`
- **CLI Verification Command:** `godot --headless --path . -- --weather-selftest`

### WildlifeTrappingSystem (World & Wildlife)

- **Source File:** [`Assets/Ashfall.Core/WildlifeTrappingSystem.cs`](../../Assets/Ashfall.Core/WildlifeTrappingSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WildlifeTrappingHostSession.cs`](../../src/Host/WildlifeTrappingHostSession.cs)
- **Save Store Façade:** [`src/Host/WildlifeTrappingSaveStore.cs`](../../src/Host/WildlifeTrappingSaveStore.cs)
- **Persisted State DTO:** `WildlifeTrappingState` (Section: `wildlife_trapping`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/wildlife_species.json`
- **Key Domain Events:** `OnTrapTriggered, OnBaitSpoiled`
- **CLI Verification Command:** `godot --headless --path . -- --wildlife-trapping-selftest`

### WildlifeMigrationSystem (World & Ecology)

- **Source File:** [`Assets/Ashfall.Core/WildlifeMigrationSystem.cs`](../../Assets/Ashfall.Core/WildlifeMigrationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
- **Save Store Façade:** [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs)
- **Persisted State DTO:** `WildlifeMigrationState` (Section: `world`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/wildlife_species.json`
- **Key Domain Events:** `OnHerdMigrated, OnPredatorPressureChanged`
- **CLI Verification Command:** `godot --headless --path . -- --world-selftest`

### LandmarkDegradationSystem (World & Landmarks)

- **Source File:** [`Assets/Ashfall.Core/LandmarkDegradationSystem.cs`](../../Assets/Ashfall.Core/LandmarkDegradationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/WorldHostSession.cs`](../../src/Host/WorldHostSession.cs)
- **Save Store Façade:** [`src/Host/WorldSaveStore.cs`](../../src/Host/WorldSaveStore.cs)
- **Persisted State DTO:** `LandmarkDegradationState` (Section: `world`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/landmarks.json`
- **Key Domain Events:** `OnLandmarkCollapsed, OnStructuralDecay`
- **CLI Verification Command:** `godot --headless --path . -- --world-selftest`

### RegionalTreatySystem (Factions & Treaties)

- **Source File:** [`Assets/Ashfall.Core/RegionalTreatySystem.cs`](../../Assets/Ashfall.Core/RegionalTreatySystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/RegionalTreatyHostSession.cs`](../../src/Host/RegionalTreatyHostSession.cs)
- **Save Store Façade:** [`src/Host/RegionalTreatySaveStore.cs`](../../src/Host/RegionalTreatySaveStore.cs)
- **Persisted State DTO:** `RegionalTreatyState` (Section: `regional_treaty`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/faction_treaties.json`
- **Key Domain Events:** `OnPactSigned, OnTreatyViolated`
- **CLI Verification Command:** `godot --headless --path . -- --regional-treaty-selftest`

### HoldfastQuestSystem (Expansion 01 (Holdfast))

- **Source File:** [`Assets/Ashfall.Core/HoldfastQuestSystem.cs`](../../Assets/Ashfall.Core/HoldfastQuestSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/HoldfastRuntimeSession.cs`](../../src/Host/HoldfastRuntimeSession.cs)
- **Save Store Façade:** [`src/Host/HoldfastSaveStore.cs`](../../src/Host/HoldfastSaveStore.cs)
- **Persisted State DTO:** `HoldfastQuestSaveState` (Section: `holdfast`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/holdfast_quests.json`
- **Key Domain Events:** `OnProtocolCompleted, OnBroadcastReceived`
- **CLI Verification Command:** `godot --headless --path . -- --holdfast-selftest`

### DutyRosterSystem (Expansion 02 (Duty Roster))

- **Source File:** [`Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs`](../../Assets/Ashfall.Core/DutyRoster/DutyRosterSystem.cs)
- **Namespace:** `Ashfall.Core.DutyRoster`
- **Host Presentation Session:** [`src/Host/DutyRosterHostSession.cs`](../../src/Host/DutyRosterHostSession.cs)
- **Save Store Façade:** [`src/Host/DutyRosterSaveStore.cs`](../../src/Host/DutyRosterSaveStore.cs)
- **Persisted State DTO:** `DutyRosterSaveState` (Section: `duty_roster`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/duty_roster_shifts.json`
- **Key Domain Events:** `OnShiftCompleted, OnFatigueAccumulated`
- **CLI Verification Command:** `godot --headless --path . -- --duty-roster-selftest`

### StandingRecordSystem (Expansion 03 (Standing Record))

- **Source File:** [`Assets/Ashfall.Core/StandingRecord/StandingRecordSystem.cs`](../../Assets/Ashfall.Core/StandingRecord/StandingRecordSystem.cs)
- **Namespace:** `Ashfall.Core.StandingRecord`
- **Host Presentation Session:** [`src/Host/StandingRecordHostSession.cs`](../../src/Host/StandingRecordHostSession.cs)
- **Save Store Façade:** [`src/Host/StandingRecordSaveStore.cs`](../../src/Host/StandingRecordSaveStore.cs)
- **Persisted State DTO:** `StandingRecordSaveState` (Section: `standing_record`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/standing_records.json`
- **Key Domain Events:** `OnRecordInscribed, OnRemembranceHeld`
- **CLI Verification Command:** `godot --headless --path . -- --standing-record-selftest`

### CrossingArbitrationSystem (Expansion 04 (Crossing))

- **Source File:** [`Assets/Ashfall.Core/CrossingArbitrationSystem.cs`](../../Assets/Ashfall.Core/CrossingArbitrationSystem.cs)
- **Namespace:** `Ashfall.Core`
- **Host Presentation Session:** [`src/Host/ExpansionHostSession.cs`](../../src/Host/ExpansionHostSession.cs)
- **Save Store Façade:** [`src/Host/ExpansionHubSaveStore.cs`](../../src/Host/ExpansionHubSaveStore.cs)
- **Persisted State DTO:** `CrossingArbitrationState` (Section: `expansion_hub`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/crossing_quests.json`
- **Key Domain Events:** `OnDisputeArbitrated, OnTollEnforced`
- **CLI Verification Command:** `godot --headless --path . -- --crossing-selftest`

### VerdictSystem (Expansion 08 (Verdict))

- **Source File:** [`Assets/Ashfall.Core/Verdict/VerdictSystem.cs`](../../Assets/Ashfall.Core/Verdict/VerdictSystem.cs)
- **Namespace:** `Ashfall.Core.Verdict`
- **Host Presentation Session:** [`src/Host/VerdictHostSession.cs`](../../src/Host/VerdictHostSession.cs)
- **Save Store Façade:** [`src/Host/VerdictSaveStore.cs`](../../src/Host/VerdictSaveStore.cs)
- **Persisted State DTO:** `VerdictSaveState` (Section: `verdict`)
- **Authoritative JSON Feed:** `Assets/StreamingAssets/Data/verdict_trials.json`
- **Key Domain Events:** `OnVerdictDelivered, OnExileExecuted`
- **CLI Verification Command:** `godot --headless --path . -- --verdict-selftest`

---

## Ports & Adapters Interface Registry

| Port Interface | Purpose | Godot Adapter | Core Fallback |
|---|---|---|---|
| `IJsonSerializer` | JSON serialization / deserialization | `SystemTextJsonSerializer` | `SystemTextJsonSerializer` |
| `IFileIO` | File system read/write/delete | `GodotFileIO` | `FileSystemIO` |
| `ILog` | Logging (Info, Warn, Error) | `GodotLog` | `ConsoleLog` |
| `IClock` / `ISimClock` | Simulation day & tick clock | `SimClock` | `SimClock` |
| `ISeededRng` | Deterministic PRNG (xorshift64*) | `CoreSeededRng` | `SeededRng` |
