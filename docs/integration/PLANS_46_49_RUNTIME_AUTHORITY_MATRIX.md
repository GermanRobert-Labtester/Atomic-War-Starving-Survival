# Plans 46–49 Runtime Authority & Integration Matrix

**Document ID:** DOC-AUTH-46-49
**Plan Reference:** OPS-INT-05-08
**Scope:** Shelter Operations — Precision Workshop, Wasteland Radio Intelligence, Shelter Social Dynamics, Subterranean Hazards

---

## 1. Executive Summary

This matrix establishes the definitive architectural authorities, save ownership, random number generation (RNG) stream allocation, event surfaces, and UI bindings for the four shelter-operation pillars specified in Plans 46–49.

All systems adhere strictly to the ASHFALL core architectural invariants:
- Zero engine coupling in Core (`Assets/Ashfall.Core/`).
- JSON as data authority (`Assets/StreamingAssets/Data/`).
- Deterministic cross-host simulation using injected `ISeededRng`.
- Single versioned atomic campaign envelope persistence via `SaveStoreHub` and `SaveSectionRegistry`.

---

## 2. Runtime Authority Matrix

| Feature | Task-Proposed Type | Live Type | System Owner | Save Owner (Section) | RNG Owner (Stream) | Events Currently Emitted | Events Required / Target | UI Surface | Readiness Status |
|---|---|---|---|---|---|---|---|---|---|
| **Workshop Jobs** (Ammo / Refurbish) | `ShelterWorkshopSystem` | `ShelterWorkshopSystem` | `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | `ShelterWorkshopSaveStore` (`shelter_workshop`) | `shelter_workshop` (Campaign Day fork) | `OnJobStarted`, `OnJobCompleted`, `OnJobCancelled` | `workshop_job_completed`, `workshop_job_started` | `WorkshopPanel.cs` (`assets/ui/panels/WorkshopPanel.tscn`) | **Authoritative & Ready** |
| **Machine Tooling & Calibration** | `ShelterWorkshopSystem` | `ShelterWorkshopSystem` | `Assets/Ashfall.Core/Shelter/ShelterWorkshopSystem.cs` | `ShelterWorkshopSaveStore` (`shelter_workshop`) | Deterministic wear calculation | `OnMachineStateChanged`, `OnWorkshopChanged` | `workshop_machine_degraded`, `workshop_machine_overhauled` | `WorkshopPanel.cs` | **Authoritative & Ready** |
| **Weapon Servicing** | `EquipmentConditionSystem` / `ShelterWorkshopSystem` | `ShelterWorkshopSystem` + `EquipmentConditionSystem` | `Ashfall.Core.Combat.EquipmentConditionSystem` + `ShelterWorkshopSystem` | `EquipmentConditionSaveStore` (`equipment_condition`) + `ShelterWorkshopSaveStore` | Deterministic repair formulas | `OnConditionChanged`, `OnWeaponJam` | `weapon_serviced`, `condition_restored` | `WorkshopPanel.cs` | **Authoritative & Ready** |
| **Radio Frequency Tuning & Lock** | `ShelterRadioStationSystem` | `ShelterRadioStationSystem` | `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs` | `RadioStationSaveStore` (`radio_station`) | `radio_station` | `OnInterceptDetected`, `OnRadioStateChanged` | `radio_tuning_changed`, `radio_signal_locked` | `RadioPanel.cs` | **Authoritative & Ready** |
| **Radio Decryption** | `ShelterRadioStationSystem` | `ShelterRadioStationSystem` | `Assets/Ashfall.Core/Radio/ShelterRadioStationSystem.cs` | `RadioStationSaveStore` (`radio_station`) | Operator skill formulas | `OnInterceptDecrypted` | `radio_intercept_decrypted`, `radio_distress_active` | `RadioPanel.cs` | **Authoritative & Ready** |
| **Radio Triangulation** | `SignalTriangulationSystem` / `ShelterRadioStationSystem` | `ShelterRadioStationSystem` + `SignalTriangulationSystem` | `Ashfall.Core.Radio.ShelterRadioStationSystem` | `RadioStationSaveStore` (`radio_station`) | Stable bearing math | `OnLocationTriangulated`, `OnLocationRevealed` | `radio_location_triangulated` → `WastelandMapSystem.Discover` | `TriangulationPanel.cs` | **Authoritative & Ready** |
| **Survivor Social Incidents** | `ShelterSocialDynamicsSystem` | `ShelterSocialDynamicsSystem` | `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` | `ShelterSocialSaveStore` (`shelter_social_dynamics`) | `shelter_social` | `OnIncidentTriggered`, `OnSocialStateChanged` | `social_dispute_unresolved`, `social_incident_triggered` | `ShelterSocialModal` / `StatusPanel.cs` | **Authoritative & Ready** |
| **Social Mediation** | `ShelterSocialDynamicsSystem` | `ShelterSocialDynamicsSystem` | `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` | `ShelterSocialSaveStore` (`shelter_social_dynamics`) | Mediator skill roll | `OnIncidentMediated` | `social_dispute_mediated`, `affinity_adjusted` | Social incident dialog | **Authoritative & Ready** |
| **Privacy Fatigue** | `ShelterSocialDynamicsSystem` | `ShelterSocialDynamicsSystem` | `Assets/Ashfall.Core/Shelter/ShelterSocialDynamicsSystem.cs` | `ShelterSocialSaveStore` (`shelter_social_dynamics`) | Deterministic room crowding accumulation | `OnSocialStateChanged` | `social_privacy_warning` | Quarters overview | **Authoritative & Ready** |
| **Excavation Cave-In** | `ExcavationHazardSystem` | `ExcavationHazardSystem` | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | `ExcavationHazardSaveStore` (`excavation_hazards`) | `excavation_hazards` | `OnRescueStarted`, `OnHazardStateChanged` | `subterranean_cave_in`, `subterranean_rescue_active` | `ExcavationPanel.cs` | **Authoritative & Ready** |
| **Methane Accumulation & Ignition** | `ExcavationHazardSystem` | `ExcavationHazardSystem` | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | `ExcavationHazardSaveStore` (`excavation_hazards`) | `excavation_hazards` | `OnMethaneIgnition`, `OnHazardStateChanged` | `subterranean_methane_warning`, `methane_alarm` | `ExcavationPanel.cs` | **Authoritative & Ready** |
| **Flood & Dewatering Sump** | `ExcavationHazardSystem` / `SumpFloodingSystem` | `ExcavationHazardSystem` + `SumpFloodingSystem` | `Ashfall.Core.Excavation.ExcavationHazardSystem` | `ExcavationHazardSaveStore` (`excavation_hazards`) | Inflow vs pumping rate | `OnSectorFlooded` | `subterranean_flood_warning` | `SlurryDewateringSumpPanel.cs` | **Authoritative & Ready** |
| **Shoring Degradation** | `ExcavationHazardSystem` | `ExcavationHazardSystem` | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | `ExcavationHazardSaveStore` (`excavation_hazards`) | `excavation_hazards` | `OnHazardStateChanged` | `subterranean_shoring_warning` | `ExcavationPanel.cs` | **Authoritative & Ready** |
| **Trapped Miners & Rescue Operations** | `ExcavationHazardSystem` | `ExcavationHazardSystem` | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | `ExcavationHazardSaveStore` (`excavation_hazards`) | Deterministic labor allocation | `OnRescueStarted`, `OnRescueSucceeded`, `OnRescueFailed` | `subterranean_rescue_completed`, `subterranean_rescue_failed` | `ExcavationPanel.cs` | **Authoritative & Ready** |
| **Hydraulic Bulkheads** | `ExcavationHazardSystem` | `ExcavationHazardSystem` | `Assets/Ashfall.Core/Excavation/ExcavationHazardSystem.cs` | `ExcavationHazardSaveStore` (`excavation_hazards`) | State toggle validation | `OnHazardStateChanged` | `subterranean_bulkhead_changed` | `ExcavationPanel.cs` | **Authoritative & Ready** |
| **Pumps & Dewatering** | `ExcavationHazardSystem` / `SumpFloodingSystem` | `ExcavationHazardSystem` + `SumpFloodingSystem` | `Ashfall.Core.Excavation.ExcavationHazardSystem` | `ExcavationHazardSaveStore` (`excavation_hazards`) | Power grid integration | `OnMitigationInstalled`, `OnHazardStateChanged` | `subterranean_pump_toggled` | `SlurryDewateringSumpPanel.cs` | **Authoritative & Ready** |
| **Dynamic Quests** | `DynamicQuestlineSystem` | `DynamicQuestlineSystem` (New) | `Assets/Ashfall.Core/Quests/DynamicQuestlines.cs` | `DynamicQuestSaveStore` (`dynamic_quests`) | `campaign_day` | `OnQuestTriggered`, `OnQuestCompleted`, `OnQuestFailed` | `quest_rescue_trapped_miners`, `quest_investigate_radio_depot` | `QuestsPanel.cs` | **Scaffolded in Phase D** |

---

## 3. Cross-Domain Handoff Contracts

1. **Radio Triangulation → Wasteland Map**:
   - `ShelterRadioStationSystem.OnLocationTriangulated(interceptId, revealedLocationId)` resolves `revealedLocationId` from `radio_intercepts.json`.
   - `WastelandMapSystem.Discover(revealedLocationId)` un-fogs the canonical map node.
   - Dynamic quest `quest_investigate_radio_depot` triggers with target `revealedLocationId`.

2. **Cave-In → Dynamic Rescue Quest → Fatal Outcome Pipeline**:
   - `ExcavationHazardSystem.TriggerCaveInRescue(sectorId, trappedSurvivorIds, deadlineDays, requiredLabor)` fires `OnRescueStarted`.
   - `DynamicQuestlineSystem` receives event and instantiates `quest_rescue_trapped_miners`.
   - If rescued before deadline: `OnRescueSucceeded` frees miners, applies solidarity, completes quest.
   - If deadline expires: `OnRescueFailed` marks trapped miners deceased in `SurvivorFateLedger` / `MemorialSystem`, emits `survivor_perished` day event, and fails quest.

3. **Social Friction → Daily Briefing & Morale**:
   - Living quarters crowding raises `PrivacyFatiguePermille`.
   - Above threshold (e.g. 700‰), `ShelterSocialDynamicsSystem` raises `social_privacy_warning` and triggers eligible incident.
   - Mediation modifies `SurvivorRelationsSystem` and records persistent `SocialIncidentRecord`.

4. **Workshop Production → Audio & Daily Briefing**:
   - Job completion consumes inputs, awards output, wears tooling, and emits `workshop_job_completed` day event.
   - Morning Daily Briefing presents completed munitions and refurbishment counts.
