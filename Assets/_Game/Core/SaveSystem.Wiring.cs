using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Simulation; // CompostSystem, SterilizationSystem, etc. (audit C-3 split)
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.AI; // HallucinationSystem (audit wiring fix)
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Crafting; // CraftingSystem, WorkbenchSystem (audit wiring fix)
using AtomicWar._Game.Events;

using AtomicWar._Game.Endgame;

using AtomicWar._Game.Encounters;

using AtomicWar._Game.World;

using AtomicWar._Game.Narrative;

using AtomicWar._Game.Factions;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {
        /// <summary>Inject a PhotoperiodSystem after construction (optional; safe to skip in tests).</summary>
        public void SetPhotoPeriodSystem(PhotoperiodSystem photoPeriodSystem) =>
            RegisterSystem(ref _photoPeriodSystem, photoPeriodSystem, "photoperiod",
                () => photoPeriodSystem.GetState(),
                o => photoPeriodSystem.RestoreState((PhotoperiodState)o));

        /// <summary>Inject radiation fog-of-war map (optional; safe to skip in tests).</summary>
        public void SetKnowledgeMap(RadiationKnowledgeMap knowledgeMap) =>
            RegisterSystem(ref _knowledgeMap, knowledgeMap, "radiation_knowledge",
                () => knowledgeMap.CaptureState(),
                o => knowledgeMap.RestoreState((RadiationKnowledgeSave)o));

        /// <summary>Inject inventory so device battery/calibration/broken persist across save/load.</summary>
        public void SetInventory(Inventory.Inventory inventory) =>
            RegisterSystem(ref _inventory, inventory, "inventory",
                () => inventory.CaptureState(),
                o => inventory.RestoreState((InventorySaveState)o, _itemLookup));

        /// <summary>Persistent bunker overflow crate shared by crafting and scavenging returns.</summary>
        public void SetOverflowStash(Inventory.Inventory overflowStash) =>
            RegisterSystem(ref _overflowStash, overflowStash, "overflow_stash",
                () => overflowStash.CaptureState(),
                o => overflowStash.RestoreState((InventorySaveState)o, _itemLookup));

        /// <summary>Persist the daily bunker food/water policy separately from store contents.</summary>
        public void SetBunkerRationingSystem(BunkerRationingSystem system) =>
            RegisterSystem(ref _bunkerRationingSystem, system, "bunker_rationing",
                () => system.CaptureState(),
                o => system.RestoreState((BunkerRationingSave)o));

        /// <summary>Persist the assigned repair worker and urgency; condition remains on existing assets.</summary>
        public void SetBunkerMaintenanceSystem(BunkerMaintenanceSystem system) =>
            RegisterSystem(ref _bunkerMaintenanceSystem, system, "bunker_maintenance",
                () => system.CaptureState(),
                o => system.RestoreState((BunkerMaintenanceSave)o));

        /// <summary>Persist the in-flight maintenance task separately from terminal assignment.</summary>
        public void SetRepairWorkOrderSystem(RepairWorkOrderSystem system) =>
            RegisterSystem(ref _repairWorkOrderSystem, system, "repair_work_order",
                () => system.CaptureState(),
                o => system.RestoreState((RepairWorkOrderSave)o));

        /// <summary>Persist continuous bunker staffing independently from task-board presentation.</summary>
        public void SetSurvivorWorkShiftSystem(SurvivorWorkShiftSystem system) =>
            RegisterSystem(ref _survivorWorkShiftSystem, system, "survivor_work_shifts",
                () => system.CaptureState(),
                o => system.RestoreState((SurvivorWorkShiftSave)o));

        /// <summary>Persist board feedback; live work ownership remains in repair_work_order.</summary>
        public void SetSurvivorTaskBoardSystem(SurvivorTaskBoardSystem system) =>
            RegisterSystem(ref _survivorTaskBoardSystem, system, "survivor_task_board",
                () => system.CaptureState(),
                o => system.RestoreState((SurvivorTaskBoardSave)o));

        /// <summary>Inject expedition system (optional; safe to skip in tests).</summary>
        public void SetExpeditionSystem(ExpeditionSystem expeditionSystem)
        {
            _expeditionSystem = expeditionSystem;
        }

        /// <summary>Inject medical triage pipeline so afflictions persist across save/load.</summary>
        public void SetMedicalSystem(MedicalSystem medicalSystem) =>
            RegisterSystem(ref _medicalSystem, medicalSystem, "medical",
                () => medicalSystem.CaptureState(),
                o => medicalSystem.RestoreState((MedicalSystemSave)o));

        /// <summary>Inject Prompt #55 blood transfusion for save/load.</summary>
        public void SetBloodTransfusionSystem(BloodTransfusionSystem sys) =>
            RegisterSystem(ref _bloodTransfusion, sys, "blood_transfusion",
                () => sys.CaptureState(),
                o => sys.RestoreState((BloodTransfusionSave)o));

        /// <summary>Inject Prompt #56 amputation for save/load.</summary>
        public void SetAmputationSystem(AmputationSystem sys) =>
            RegisterSystem(ref _amputationSystem, sys, "amputation",
                () => sys.CaptureState(),
                o => sys.RestoreState((AmputationSave)o));

        /// <summary>Inject Prompt #57 scurvy for save/load.</summary>
        public void SetScurvySystem(ScurvySystem sys) =>
            RegisterSystem(ref _scurvySystem, sys, "scurvy",
                () => sys.CaptureState(),
                o => sys.RestoreState((ScurvySave)o));

        /// <summary>Inject Prompt #60 mutagenesis for save/load.</summary>
        public void SetMutagenesisSystem(RadiationMutagenesisSystem sys) =>
            RegisterSystem(ref _mutagenesisSystem, sys, "mutagenesis",
                () => sys.CaptureState(),
                o => sys.RestoreState((MutagenesisSave)o));

        /// <summary>Inject world phase system so CurrentPhase/HasTriggeredExchange persist across save/load.</summary>
        public void SetWorldPhaseSystem(WorldPhaseSystem worldPhaseSystem) =>
            RegisterSystem(ref _worldPhaseSystem, worldPhaseSystem, "world_phase",
                () => worldPhaseSystem.CaptureState(),
                o => worldPhaseSystem.RestoreState((WorldPhaseSave)o));

        /// <summary>Inject dynamic economy / faction trust matrix for save/load.</summary>
        public void SetEconomySystem(DynamicEconomySystem economySystem) =>
            RegisterSystem(ref _economySystem, economySystem, "economy",
                () => economySystem.CaptureState(),
                o => economySystem.RestoreState((DynamicEconomySave)o));

        /// <summary>Inject shelter power grid for save/load.</summary>
        public void SetPowerNetwork(PowerNetwork powerNetwork) =>
            RegisterSystem(ref _powerNetwork, powerNetwork, "power_network",
                () => powerNetwork.CaptureState(),
                o => powerNetwork.RestoreState((PowerNetworkSave)o));

        /// <summary>Inject hatch defense / raid state for save/load.</summary>
        public void SetHatchDefense(HatchDefenseSystem hatchDefense) =>
            RegisterSystem(ref _hatchDefense, hatchDefense, "hatch_defense",
                () => hatchDefense.CaptureState(),
                o => hatchDefense.RestoreState((HatchDefenseSave)o));

        /// <summary>Inject faction radio intercept log for save/load.</summary>
        public void SetFactionRadioIntercepts(FactionRadioInterceptSystem radioIntercepts) =>
            RegisterSystem(ref _factionRadioIntercepts, radioIntercepts, "faction_radio_intercepts",
                () => radioIntercepts.CaptureState(),
                o => radioIntercepts.RestoreState((FactionRadioInterceptSave)o));

        /// <summary>Inject diegetic journal / knowledge base for save/load.</summary>
        public void SetJournalSystem(JournalSystem journalSystem) =>
            RegisterSystem(ref _journalSystem, journalSystem, "journal",
                () => journalSystem.CaptureState(),
                o => journalSystem.RestoreState((JournalSave)o));

        /// <summary>Inject campaign win/loss victory project for save/load.</summary>
        public void SetVictoryProjectManager(VictoryProjectManager victoryProject) =>
            RegisterSystem(ref _victoryProject, victoryProject, "victory_project",
                () => victoryProject.CaptureState(),
                o => victoryProject.RestoreState((VictoryProjectSave)o));

        /// <summary>
        /// Inject EventRunner so the scheduled narrative-chain queue
        /// (Prompt #43) persists across save/load.
        /// </summary>
        public void SetEventRunner(EventRunner eventRunner)
        {
            _eventRunner = eventRunner;
        }

        /// <summary>Inject internal-mystery SuspicionTracker for save/load.</summary>
        public void SetSuspicionTracker(SuspicionTracker suspicionTracker) =>
            RegisterSystem(ref _suspicionTracker, suspicionTracker, "suspicion",
                () => suspicionTracker.CaptureState(),
                o => suspicionTracker.RestoreState((SuspicionTrackerSave)o));

        /// <summary>Inject weather-driven hatch entrapment for save/load (Prompt #48).</summary>
        public void SetHatchEntrapment(HatchEntrapmentSystem hatchEntrapment) =>
            RegisterSystem(ref _hatchEntrapment, hatchEntrapment, "hatch_entrapment",
                () => hatchEntrapment.CaptureState(),
                o => hatchEntrapment.RestoreState((HatchEntrapmentSave)o));

        /// <summary>Inject Internal Horror room atmosphere (O2/CO/fire/humidity).</summary>
        public void SetAtmosphereSystem(ShelterAtmosphereSystem atmosphereSystem) =>
            RegisterSystem(ref _atmosphereSystem, atmosphereSystem, "atmosphere",
                () => atmosphereSystem.CaptureState(),
                o => atmosphereSystem.RestoreState((ShelterAtmosphereSave)o));

        /// <summary>Inject Internal Horror corpse management for save/load.</summary>
        public void SetCorpseSystem(CorpseManagementSystem corpseSystem) =>
            RegisterSystem(ref _corpseSystem, corpseSystem, "corpses",
                () => corpseSystem.CaptureState(),
                o => corpseSystem.RestoreState((CorpseManagementSave)o));

        /// <summary>Inject Internal Horror pantry rust system for save/load.</summary>
        public void SetPantrySystem(PantryContaminationSystem pantrySystem) =>
            RegisterSystem(ref _pantrySystem, pantrySystem, "pantry",
                () => pantrySystem.CaptureState(),
                o => pantrySystem.RestoreState((PantryContaminationSave)o));

        /// <summary>Inject Prompt #13 sabotaged-cache habit / plant counters for save/load.</summary>
        public void SetSabotagedCacheSystem(SabotagedCacheSystem sabotagedCaches) =>
            RegisterSystem(ref _sabotagedCaches, sabotagedCaches, "sabotaged_caches",
                () => sabotagedCaches.CaptureState(),
                o => sabotagedCaches.RestoreState((SabotagedCacheSave)o));

        /// <summary>Inject Prompt #14 shifting death-zone windstorms for save/load.</summary>
        public void SetShiftingHotspotSystem(ShiftingHotspotSystem shiftingHotspots)
        {
            _shiftingHotspots = shiftingHotspots;
        }

        /// <summary>Inject Prompt #17 inter-faction raid plan wiretaps for save/load.</summary>
        public void SetFactionRaidPlanSystem(FactionRaidPlanSystem factionRaidPlans)
        {
            _factionRaidPlans = factionRaidPlans;
        }

        /// <summary>Inject Prompt #18 delayed faction dig-out debt collector for save/load.</summary>
        public void SetDebtCollectorSystem(DebtCollectorSystem debtCollector) =>
            RegisterSystem(ref _debtCollector, debtCollector, "debt_collector",
                () => debtCollector.CaptureState(),
                o => debtCollector.RestoreState((DebtCollectorSave)o));

        /// <summary>Inject Prompt #19 post-EMP ghost station dial unlock for save/load.</summary>
        public void SetGhostStationSystem(GhostStationSystem ghostStations) =>
            RegisterSystem(ref _ghostStations, ghostStations, "ghost_stations",
                () => ghostStations.CaptureState(),
                o => ghostStations.RestoreState((GhostStationSave)o));

        /// <summary>Inject Child Dependent system so child state persists across save/load.</summary>
        public void SetChildDependentSystem(ChildDependentSystem childSystem) =>
            RegisterSystem(ref _childSystem, childSystem, "child_dependent",
                () => childSystem.CaptureState(),
                o => childSystem.RestoreState((ChildDependentSystem.SaveState)o, _getSurvivors?.Invoke()));

        /// <summary>Inject Prompt #49 structural integrity for save/load.</summary>
        public void SetStructuralIntegritySystem(StructuralIntegritySystem sys) =>
            RegisterSystem(ref _structuralIntegrity, sys, "structural_integrity",
                () => sys.CaptureState(),
                o => sys.RestoreState((StructuralIntegritySave)o));

        /// <summary>Inject Prompt #50 waste/hygiene for save/load.</summary>
        public void SetWasteSystem(WasteSystem sys) =>
            RegisterSystem(ref _wasteSystem, sys, "waste",
                () => sys.CaptureState(),
                o => sys.RestoreState((WasteSystemSave)o));

        /// <summary>Inject Prompt #51 vermin for save/load.</summary>
        public void SetVerminSystem(VerminSystem sys) =>
            RegisterSystem(ref _verminSystem, sys, "vermin",
                () => sys.CaptureState(),
                o => sys.RestoreState((VerminSave)o));

        /// <summary>Inject Prompt #52 jury-rig for save/load.</summary>
        public void SetJuryRigSystem(JuryRigSystem sys) =>
            RegisterSystem(ref _juryRigSystem, sys, "jury_rig",
                () => sys.CaptureState(),
                o => sys.RestoreState((JuryRigSave)o));

        /// <summary>Inject Prompt #53 freeze-pipe for save/load.</summary>
        public void SetFreezePipeSystem(FreezePipeSystem sys) =>
            RegisterSystem(ref _freezePipeSystem, sys, "freeze_pipe",
                () => sys.CaptureState(),
                o => sys.RestoreState((FreezePipeSave)o));

        /// <summary>Inject Prompt #67 cartography for save/load.</summary>
        public void SetCartographySystem(CartographySystem sys) =>
            RegisterSystem(ref _cartographySystem, sys, "cartography",
                () => sys.CaptureState(),
                o => sys.RestoreState((CartographySave)o));

        /// <summary>Inject Prompt #69 flooded map nodes for save/load.</summary>
        public void SetFloodedNodeSystem(FloodedNodeSystem sys) =>
            RegisterSystem(ref _floodedNodeSystem, sys, "flooded_node",
                () => sys.CaptureState(), o => sys.RestoreState((FloodedNodeSave)o));

        /// <summary>Bicycle logistics (Prompt #68) — currently stateless; CR reserved.</summary>
        public void SetBicycleSystem(BicycleSystem s) =>
            RegisterSystem(ref _bicycleSystem, s, "bicycle",
                () => s.CaptureState(), o => s.RestoreState((BicycleSystemSave)o));

        /// <summary>Stove cooking (Prompt #189) — currently stateless; CR reserved.</summary>
        public void SetCookingSystem(CookingSystem s) =>
            RegisterSystem(ref _cookingSystem, s, "cooking",
                () => s.CaptureState(), o => s.RestoreState((CookingSystemSave)o));

        /// <summary>Catchment + purifier water economy — currently stateless; CR reserved.</summary>
        public void SetWaterEconomySystem(WaterEconomySystem s) =>
            RegisterSystem(ref _waterEconomySystem, s, "water_economy",
                () => s.CaptureState(), o => s.RestoreState((WaterEconomySystemSave)o));

        /// <summary>Black Rain hazard (Prompt #11) — currently stateless; CR reserved.</summary>
        public void SetBlackRainHazardSystem(BlackRainHazardSystem s) =>
            RegisterSystem(ref _blackRainHazardSystem, s, "black_rain",
                () => s.CaptureState(), o => s.RestoreState((BlackRainHazardSystemSave)o));

        /// <summary>Inject Prompt #71 tracker for save/load.</summary>
        public void SetTrackerSystem(TrackerSystem sys) =>
            RegisterSystem(ref _trackerSystem, sys, "tracker",
                () => sys.CaptureState(),
                o => sys.RestoreState((TrackerSave)o));

        /// <summary>Inject Prompt #72 dead-drop for save/load.</summary>
        public void SetDeadDropSystem(DeadDropSystem sys) =>
            RegisterSystem(ref _deadDropSystem, sys, "dead_drops",
                () => sys.CaptureState(),
                o => sys.RestoreState((DeadDropSave)o));

        /// <summary>Inject Prompt #73 hostage for save/load.</summary>
        public void SetHostageSystem(HostageSystem sys) => RegisterSystem(ref _hostageSystem, sys, "hostage", () => sys.CaptureState(), o => sys.RestoreState((HostageSave)o));
        /// <summary>Inject Prompt #74 propaganda for save/load.</summary>
        public void SetPropagandaSystem(PropagandaSystem sys) => RegisterSystem(ref _propagandaSystem, sys, "propaganda", () => sys.CaptureState(), o => sys.RestoreState((PropagandaSave)o));
        /// <summary>Inject Prompt #75 deserter for save/load.</summary>
        public void SetDeserterSystem(DeserterSystem sys) => RegisterSystem(ref _deserterSystem, sys, "deserter", () => sys.CaptureState(), o => sys.RestoreState((DeserterSave)o));
        /// <summary>Inject Prompt #76 scapegoat for save/load.</summary>
        public void SetScapegoatSystem(WeatherScapegoatSystem sys) => RegisterSystem(ref _scapegoatSystem, sys, "scapegoat", () => sys.CaptureState(), o => sys.RestoreState((ScapegoatSave)o));
        /// <summary>Inject Prompt #77 labor camp for save/load.</summary>
        public void SetLaborCampSystem(LaborCampSystem sys) => RegisterSystem(ref _laborCampSystem, sys, "labor_camp", () => sys.CaptureState(), o => sys.RestoreState((LaborCampSave)o));
        /// <summary>Inject Prompt #78 cult moral for save/load.</summary>
        public void SetCultMoralSystem(CultMoralDisgustSystem sys) => RegisterSystem(ref _cultMoralSystem, sys, "cult_moral", () => sys.CaptureState(), o => sys.RestoreState((CultMoralSave)o));
        /// <summary>Inject Prompt #79 mutated ecosystem for save/load.</summary>
        public void SetEcosystemSystem(MutatedEcosystemSystem sys) => RegisterSystem(ref _ecosystemSystem, sys, "ecosystem", () => sys.CaptureState(), o => sys.RestoreState((EcosystemSave)o));
        /// <summary>Inject Prompt #79 house-to-bunker for save/load.</summary>
        public void SetHouseToBunkerSystem(HouseToBunkerSystem sys) => RegisterSystem(ref _houseToBunkerSystem, sys, "house_to_bunker", () => sys.CaptureState(), o => sys.RestoreState((HouseToBunkerSave)o));
        /// <summary>Inject Prompt #901 dead letter office for save/load.</summary>
        public void SetDeadLetterOffice(Encounter_DeadLetterOffice enc) => RegisterSystem(ref _deadLetterOffice, enc, "enc_dead_letter_office", () => enc.CaptureState(), o => enc.RestoreState((DeadLetterOfficeState)o));
        /// <summary>Inject Prompt #903 weather station for save/load.</summary>
        public void SetWeatherStation(Encounter_WeatherStation enc) => RegisterSystem(ref _weatherStation, enc, "enc_weather_station", () => enc.CaptureState(), o => enc.RestoreState((WeatherStationState)o));
        /// <summary>Inject Prompt #904 the pianist for save/load.</summary>
        public void SetPianist(Encounter_Pianist enc) => RegisterSystem(ref _pianist, enc, "enc_pianist", () => enc.CaptureState(), o => enc.RestoreState((PianistState)o));
        /// <summary>Inject Prompt #85-94 location quests for save/load.</summary>
        public void SetLocationQuestSystem(LocationQuestSystem sys) => RegisterSystem(ref _locationQuestSystem, sys, "location_quest", () => sys.CaptureState(), o => sys.RestoreState((LocationQuestSave)o));
        public void SetExcavationSystem(ExcavationSystem s) => RegisterSystem(ref _excavationSystem, s, "excavation", () => s.CaptureState(), o => s.RestoreState((ExcavationSave)o));
        public void SetFloodingSystem(RoomFloodingSystem s) => RegisterSystem(ref _floodingSystem, s, "flooding", () => s.CaptureState(), o => s.RestoreState((FloodingSave)o));
        public void SetHiddenStorageSystem(HiddenStorageSystem s) => RegisterSystem(ref _hiddenStorageSystem, s, "hidden_storage", () => s.CaptureState(), o => s.RestoreState((HiddenStorageSave)o));
        public void SetCeilingCollapseSystem(CeilingCollapseSystem s) => RegisterSystem(ref _ceilingCollapseSystem, s, "ceiling_collapse", () => s.CaptureState(), o => s.RestoreState((CeilingCollapseSave)o));
        public void SetPerimeterTrapSystem(PerimeterTrapSystem s) => RegisterSystem(ref _perimeterTrapSystem, s, "perimeter_trap", () => s.CaptureState(), o => s.RestoreState((PerimeterTrapSave)o));
        public void SetTunnelingSystem(TunnelingSystem s) => RegisterSystem(ref _tunnelingSystem, s, "tunneling", () => s.CaptureState(), o => s.RestoreState((TunnelingSave)o));
        public void SetHatchVisibilitySystem(HatchVisibilitySystem s) => RegisterSystem(ref _hatchVisibilitySystem, s, "hatch_visibility", () => s.CaptureState(), o => s.RestoreState((HatchVisibilitySave)o));
        public void SetEscapeHatchSystem(EscapeHatchSystem s) => RegisterSystem(ref _escapeHatchSystem, s, "escape_hatch", () => s.CaptureState(), o => s.RestoreState((EscapeHatchSave)o));
        public void SetMaterialShieldingSystem(MaterialShieldingSystem s) => RegisterSystem(ref _materialShieldingSystem, s, "material_shielding", () => s.CaptureState(), o => s.RestoreState((MaterialShieldingSave)o));
        public void SetAirlockSystem(AirlockSystem s) => RegisterSystem(ref _airlockSystem, s, "airlock", () => s.CaptureState(), o => s.RestoreState((AirlockSave)o));
        public void SetNoiseSystem(NoiseSystem s) => RegisterSystem(ref _noiseSystem, s, "noise", () => s.CaptureState(), o => s.RestoreState((NoiseSave)o));
        public void SetClothingSystem(ClothingDegradationSystem s) { _clothingSystem = s; }
        public void SetResilienceSystem(ResilienceSystem s) => RegisterSystem(ref _resilienceSystem, s, "resilience", () => s.CaptureState(), o => s.RestoreState((ResilienceSave)o));
        public void SetCompostSystem(CompostSystem s) => RegisterSystem(ref _compostSystem, s, "compost", () => s.CaptureState(), o => s.RestoreState((CompostSave)o));
        public void SetScrapWeaponSystem(ScrapWeaponSystem s) { _scrapWeaponSystem = s; }
        public void SetSterilizationSystem(SterilizationSystem s) => RegisterSystem(ref _sterilizationSystem, s, "sterilization", () => s.CaptureState(), o => s.RestoreState((SterilizationSave)o));
        public void SetChelationSystem(ChelationSystem s) => RegisterSystem(ref _chelationSystem, s, "chelation", () => s.CaptureState(), o => s.RestoreState((ChelationSave)o));
        public void SetWindTurbineSystem(WindTurbineSystem s) => RegisterSystem(ref _windTurbineSystem, s, "wind_turbine", () => s.CaptureState(), o => s.RestoreState((WindTurbineSave)o));
        public void SetAntibioticResistSystem(AntibioticResistanceSystem s) => RegisterSystem(ref _antibioticResistSystem, s, "antibiotic_resist", () => s.CaptureState(), o => s.RestoreState((AntibioticResistSave)o));
        public void SetHaulingSystem(InternalHaulingSystem s) => RegisterSystem(ref _haulingSystem, s, "hauling", () => s.CaptureState(), o => s.RestoreState((HaulingSave)o));
        public void SetWeaponMaintenanceSystem(WeaponMaintenanceSystem s) => RegisterSystem(ref _weaponMaintenanceSystem, s, "weapon_maint", () => s.CaptureState(), o => s.RestoreState((WeaponMaintSave)o));
        public void SetAestheticsSystem(RoomAestheticsSystem s) => RegisterSystem(ref _aestheticsSystem, s, "aesthetics", () => s.CaptureState(), o => s.RestoreState((AestheticsSave)o));
        public void SetHamRadioSystem(HamRadioSystem s) => RegisterSystem(ref _hamRadioSystem, s, "ham_radio", () => s.CaptureState(), o => s.RestoreState((HamRadioSave)o));
        public void SetTriageSystem(TriageBoardSystem s) => RegisterSystem(ref _triageSystem, s, "triage", () => s.CaptureState(), o => s.RestoreState((TriageSave)o));
        public void SetPolypharmacySystem(PolypharmacySystem s) => RegisterSystem(ref _polypharmacySystem, s, "polypharmacy", () => s.CaptureState(), o => s.RestoreState((PolypharmSave)o));

        /// <summary>Inject Prompts #179–#181 skill progression for save/load.</summary>
        public void SetSkillProgressionSystem(SkillProgressionSystem s) =>
            RegisterSystem(ref _skillProgression, s, "skill_progression",
                () => s.CaptureState(),
                o => s.RestoreState((SkillProgressionSave)o, _getSurvivors?.Invoke()));

        public void SetCombatPerkSystem(CombatPerkSystem s) =>
            RegisterSystem(ref _combatPerkSystem, s, "combat_perks",
                () => s.CaptureState(),
                o => s.RestoreState((CombatPerkSave)o));

        public void SetSurvivalPerkSystem(SurvivalPerkSystem s) =>
            RegisterSystem(ref _survivalPerkSystem, s, "survival_perks",
                () => s.CaptureState(),
                o => s.RestoreState((SurvivalPerkSave)o));

        public void SetShelterPerkSystem(ShelterPerkSystem s) =>
            RegisterSystem(ref _shelterPerkSystem, s, "shelter_perks",
                () => s.CaptureState(),
                o => s.RestoreState((ShelterPerkSave)o));

        public void SetMedicalPerkSystem(MedicalPerkSystem s) =>
            RegisterSystem(ref _medicalPerkSystem, s, "medical_perks",
                () => s.CaptureState(),
                o => s.RestoreState((MedicalPerkSave)o));

        public void SetExpeditionPerkSystem(ExpeditionPerkSystem s) =>
            RegisterSystem(ref _expeditionPerkSystem, s, "expedition_perks",
                () => s.CaptureState(),
                o => s.RestoreState((ExpeditionPerkSave)o));

        public void SetSocialPerkSystem(SocialPerkSystem s) =>
            RegisterSystem(ref _socialPerkSystem, s, "social_perks",
                () => s.CaptureState(),
                o => s.RestoreState((SocialPerkSave)o));

        public void SetPersonalQuestSystem(PersonalQuestSystem s) =>
            RegisterSystem(ref _personalQuestSystem, s, "personal_quests",
                () => s.CaptureState(),
                o => s.RestoreState((PersonalQuestSave)o));

        /// <summary>Prompt #65 — hallucination phantom item state (audit wiring fix).</summary>
        public void SetHallucinationSystem(HallucinationSystem s) =>
            RegisterSystem(ref _hallucinationSystem, s, "hallucination",
                () => s.CaptureState(),
                o => s.RestoreState((HallucinationSave)o));

        /// <summary>Prompt #62 — internal door locks + guard assignments (audit wiring fix).</summary>
        public void SetInternalLockSystem(InternalLockSystem s) =>
            RegisterSystem(ref _internalLockSystem, s, "internal_lock",
                () => s.CaptureState(),
                o => s.RestoreState((InternalLockSave)o));

        /// <summary>Prompt #61 — survivor diary entries (audit wiring fix).</summary>
        public void SetSurvivorDiariesSystem(SurvivorDiariesSystem s) =>
            RegisterSystem(ref _survivorDiariesSystem, s, "survivor_diaries",
                () => s.CaptureState(),
                o => s.RestoreState((DiarySystemSave)o));

        /// <summary>Radio broadcasts played-set (audit wiring fix).</summary>
        public void SetRadioBroadcastSystem(RadioBroadcastSystem s) =>
            RegisterSystem(ref _radioBroadcastSystem, s, "radio_broadcast",
                () => s.CaptureState(),
                o => s.RestoreState((RadioSave)o));

        /// <summary>Active crafting queue (audit wiring fix).</summary>
        public void SetCraftingSystem(CraftingSystem s) =>
            RegisterSystem(ref _craftingSystem, s, "crafting",
                () => s.CaptureState(),
                o => s.RestoreState((CraftingSystemSave)o));

        /// <summary>
        /// Workbench system. Genuinely stateless for save purposes — station wear
        /// lives on CraftingSystem, which is captured separately — so this registers
        /// an empty slot to reserve the save id for future mutable state.
        /// </summary>
        public void SetWorkbenchSystem(WorkbenchSystem s)
        {
            if (s != null)
                Register(new SaveableAdapter("workbench",
                    () => new WorkbenchSystemSave(),
                    _ => { }));
        }

        /// <summary>Active scavenging missions (audit wiring fix).</summary>
        public void SetScavengingSystem(LocationScavengingSystem s) =>
            RegisterSystem(ref _scavengingSystem, s, "scavenging",
                () => s.CaptureState(),
                o => s.RestoreState((ScavengingSystemSave)o));

        /// <summary>Animal companions roster (prompt #380 follow-up / save audit).</summary>
        public void SetPetSystem(PetSystem s) =>
            RegisterSystem(ref _petSystem, s, "pets",
                () => s.CaptureState(),
                o => s.RestoreState((PetSystemSave)o));

        /// <summary>Pre-war fuel varnish / biofuel stills (prompt #380).</summary>
        public void SetFuelDecaySystem(FuelDecaySystem s) =>
            RegisterSystem(ref _fuelDecaySystem, s, "fuel_decay",
                () => s.CaptureState(),
                o => s.RestoreState((FuelDecayState)o));

        /// <summary>Radio tuner power/tuning + extracted intel nodes (had CaptureState, never registered).</summary>
        public void SetRadioTunerSystem(RadioTunerSystem s) =>
            RegisterSystem(ref _radioTunerSystem, s, "radio_tuner",
                () => s.CaptureState(),
                o => s.RestoreState((RadioTunerSave)o));

        /// <summary>Addiction recovery-hour progress (survivor flags already on SurvivorSave).</summary>
        public void SetAddictionSystem(AddictionSystem s) =>
            RegisterSystem(ref _addictionSystem, s, "addiction",
                () => s.CaptureState(),
                o => s.RestoreState((AddictionSave)o));

        /// <summary>Chem-abuse blood toxicity (had CaptureState, never constructed/registered).</summary>
        public void SetBloodToxicitySystem(BloodToxicitySystem s) =>
            RegisterSystem(ref _bloodToxicitySystem, s, "blood_toxicity",
                () => s.CaptureState(),
                o => s.RestoreState((BloodToxicitySave)o));

        /// <summary>Graft/prosthetic rejection timers (had CaptureState, never constructed/registered).</summary>
        public void SetGraftRejectionSystem(GraftRejectionSystem s) =>
            RegisterSystem(ref _graftRejectionSystem, s, "graft_rejection",
                () => s.CaptureState(),
                o => s.RestoreState((GraftRejectionSave)o));

        /// <summary>Mutant pheromone camo hours (had CaptureState, never constructed/registered).</summary>
        public void SetPheromoneMaskingSystem(PheromoneMaskingSystem s) =>
            RegisterSystem(ref _pheromoneMaskingSystem, s, "pheromone_masking",
                () => s.CaptureState(),
                o => s.RestoreState((PheromoneMaskingSave)o));

        /// <summary>Chem tolerance (Prompt #833) — morphine / amphetamines / anti_rad use counts.</summary>
        public void SetChemToleranceSystem(System_Tolerance s) =>
            RegisterSystem(ref _chemToleranceSystem, s, "tolerance",
                () => s.CaptureState(),
                o => s.RestoreState((ToleranceState)o));

        /// <summary>Rogue-lite grave site from prior wipe (had CaptureState, never constructed/registered).</summary>
        public void SetLastWillSystem(LastWillSystem s) =>
            RegisterSystem(ref _lastWillSystem, s, "last_will",
                () => s.CaptureState(),
                o => s.RestoreState((LastWillSave)o));

        /// <summary>Prompt #859 — legacy ruined-bunker start seeded from Last Will.</summary>
        public void SetLegacyStartSystem(System_LegacyStart s) =>
            RegisterSystem(ref _legacyStartSystem, s, "legacy_start",
                () => s.CaptureState(),
                o => s.RestoreState((LegacyStartState)o));

        /// <summary>Prompt #829 — A/B/AB/O types + bag transfusion hemolytic shock.</summary>
        public void SetBloodTypesSystem(System_BloodTypes s) =>
            RegisterSystem(ref _bloodTypesSystem, s, "blood_types",
                () => s.CaptureState(),
                o => s.RestoreState((BloodTypesState)o));

        /// <summary>Prompt #768 — empty-bunker epilogue stats (meals / bullets / death rooms).</summary>
        public void SetEpilogueStatsSystem(System_EpilogueStats s) =>
            RegisterSystem(ref _epilogueStatsSystem, s, "epilogue_stats",
                () => s.CaptureState(),
                o => s.RestoreState((EpilogueStatsState)o));

        /// <summary>Prompt #839 — bunker gossip rumors + affinity decay totals.</summary>
        public void SetGossipSystem(System_Gossip s) =>
            RegisterSystem(ref _gossipSystem, s, "gossip",
                () => s.CaptureState(),
                o => s.RestoreState((GossipSystemState)o));

        /// <summary>Prompt #861 — adaptive warlord counters across playthroughs.</summary>
        public void SetAdaptiveWarlordsSystem(System_AdaptiveWarlords s) =>
            RegisterSystem(ref _adaptiveWarlordsSystem, s, "adaptive_warlords",
                () => s.CaptureState(),
                o => s.RestoreState((AdaptiveWarlordsState)o));

        // Expansion II — The Weight of Factions

        /// <summary>Central Garrison compliance ledger (strike / non-compliant / 4-week reinstatement).</summary>
        public void SetGarrisonComplianceLedgerSystem(System_GarrisonComplianceLedger s) =>
            RegisterSystem(ref _garrisonComplianceLedgerSystem, s, "garrison_compliance_ledger",
                () => s.CaptureState(),
                o => s.RestoreState((GarrisonComplianceLedgerState)o));

        /// <summary>Upland Militia tithe book (10% base, 5% escalation, 3-day refusal grace).</summary>
        public void SetMilitiaContributionTaxSystem(System_MilitiaContributionTax s) =>
            RegisterSystem(ref _militiaContributionTaxSystem, s, "militia_contribution_tax",
                () => s.CaptureState(),
                o => s.RestoreState((MilitiaContributionTaxState)o));

        /// <summary>Cult of the Glow "leash" (3 visits → blessed; 1 miss warned, 2+ forbidden).</summary>
        public void SetCultLeashSystem(System_CultLeash s) =>
            RegisterSystem(ref _cultLeashSystem, s, "cult_leash",
                () => s.CaptureState(),
                o => s.RestoreState((CultLeashState)o));

        /// <summary>Scavenger Warlord tribute book (1.5x short escalation, 8x cap, leave-one-thing).</summary>
        public void SetWarlordTributeSystem(System_WarlordTribute s) =>
            RegisterSystem(ref _warlordTributeSystem, s, "warlord_tribute",
                () => s.CaptureState(),
                o => s.RestoreState((WarlordTributeState)o));

        /// <summary>Prompt #806 — automated bilge pumps (flood → purified water).</summary>
        public void SetBilgePumpsSystem(System_BilgePumps s) =>
            RegisterSystem(ref _bilgePumpsSystem, s, "bilge_pumps",
                () => s.CaptureState(),
                o => s.RestoreState((BilgePumpsState)o));

        /// <summary>Prompt #658 — outdoor carrion birds (hatch visibility / map danger / morale).</summary>
        public void SetCarrionBirdsSystem(System_CarrionBirds s) =>
            RegisterSystem(ref _carrionBirdsSystem, s, "carrion_birds",
                () => s.CaptureState(),
                o => s.RestoreState((CarrionBirdsState)o));

        /// <summary>Prompt #799 — IF/THEN power-grid logic gates.</summary>
        public void SetLogicGatesSystem(System_LogicGates s) =>
            RegisterSystem(ref _logicGatesSystem, s, "logic_gates",
                () => s.CaptureState(),
                o => s.RestoreState((LogicGatesState)o));

        /// <summary>Prompt #864 — community JSON mod loader (path + loaded names).</summary>
        public void SetModLoaderSystem(System_ModLoader s) =>
            RegisterSystem(ref _modLoaderSystem, s, "mod_loader",
                () => s.CaptureState(),
                o => s.RestoreState((ModLoaderState)o));

        /// <summary>Prompt #865 — Twitch chat polls (connection, polls, cooldown).</summary>
        public void SetTwitchApiSystem(System_TwitchAPI s) =>
            RegisterSystem(ref _twitchApiSystem, s, "twitch_api",
                () => s.CaptureState(),
                o => s.RestoreState((TwitchApiState)o));

        // ── Batch CaptureState systems (previously unconstructed) ──────────

        public void SetDiseaseExpansionSystem(DiseaseSystem_Expansion s) =>
            RegisterSystem(ref _diseaseExpansionSystem, s, "disease_expansion",
                () => s.CaptureState(),
                o => s.RestoreState((DiseaseSystemExpansionState)o));

        public void SetDynamicScapegoatSystem(Dynamic_Scapegoat s) =>
            RegisterSystem(ref _dynamicScapegoatSystem, s, "dynamic_scapegoat",
                () => s.CaptureState(),
                o => s.RestoreState((ScapegoatState)o));

        public void SetIronManMode(Mode_IronMan s) =>
            RegisterSystem(ref _ironManMode, s, "mode_iron_man",
                () => s.CaptureState(),
                o => s.RestoreState((IronManState)o));

        /// <summary>Persist victory/defeat state (H-2); previously reset to in-progress on every load.</summary>
        public void SetEndgameEngine(EndgameEngine s) =>
            RegisterSystem(ref _endgameEngine, s, "endgame",
                () => s.CaptureState(),
                o => s.RestoreState((CampaignResult)o));

        public void SetAndroidNpcSystem(NPC_Android s) =>
            RegisterSystem(ref _androidNpcSystem, s, "npc_android",
                () => s.CaptureState(),
                o => s.RestoreState((AndroidState)o));

        public void SetSheriffRoleSystem(Role_Sheriff s) =>
            RegisterSystem(ref _sheriffRoleSystem, s, "role_sheriff",
                () => s.CaptureState(),
                o => s.RestoreState((SheriffState)o));

        public void SetScenarioGenSystem(UI_ScenarioGen s) =>
            RegisterSystem(ref _scenarioGenSystem, s, "ui_scenario_gen",
                () => s.CaptureState(),
                o => s.RestoreState((ScenarioGenState)o));

        public void SetSpeedrunTimerSystem(UI_SpeedrunTimer s) =>
            RegisterSystem(ref _speedrunTimerSystem, s, "ui_speedrun_timer",
                () => s.CaptureState(),
                o => s.RestoreState((SpeedrunTimerState)o));

        public void SetTrueEndingSystem(Victory_TrueEnding s) =>
            RegisterSystem(ref _trueEndingSystem, s, "victory_true_ending",
                () => s.CaptureState(),
                o => s.RestoreState((TrueEndingState)o));

        public void SetVictoryAirliftSystem(Victory_Airlift s) =>
            RegisterSystem(ref _victoryAirliftSystem, s, "victory_airlift",
                () => s.CaptureState(),
                o => s.RestoreState((AirliftState)o));

        public void SetVictoryAscendancySystem(Victory_Ascendancy s) =>
            RegisterSystem(ref _victoryAscendancySystem, s, "victory_ascendancy",
                () => s.CaptureState(),
                o => s.RestoreState((AscendancyState)o));

        public void SetVictoryBuriedAliveSystem(Victory_BuriedAlive s) =>
            RegisterSystem(ref _victoryBuriedAliveSystem, s, "victory_buried_alive",
                () => s.CaptureState(),
                o => s.RestoreState((BuriedAliveState)o));

        public void SetVictoryCannibalKingSystem(Victory_CannibalKing s) =>
            RegisterSystem(ref _victoryCannibalKingSystem, s, "victory_cannibal_king",
                () => s.CaptureState(),
                o => s.RestoreState((CannibalKingState)o));

        public void SetVictoryDefectionSystem(Victory_Defection s) =>
            RegisterSystem(ref _victoryDefectionSystem, s, "victory_defection",
                () => s.CaptureState(),
                o => s.RestoreState((DefectionState)o));

        public void SetVictoryIcebreakerSystem(Victory_Icebreaker s) =>
            RegisterSystem(ref _victoryIcebreakerSystem, s, "victory_icebreaker",
                () => s.CaptureState(),
                o => s.RestoreState((IcebreakerState)o));

        public void SetVictoryLoneSurvivorSystem(Victory_LoneSurvivor s) =>
            RegisterSystem(ref _victoryLoneSurvivorSystem, s, "victory_lone_survivor",
                () => s.CaptureState(),
                o => s.RestoreState((LoneSurvivorState)o));

        public void SetVictoryMadSystem(Victory_MAD s) =>
            RegisterSystem(ref _victoryMadSystem, s, "victory_mad",
                () => s.CaptureState(),
                o => s.RestoreState((MADState)o));

        public void SetVictoryMigrationSystem(Victory_Migration s) =>
            RegisterSystem(ref _victoryMigrationSystem, s, "victory_migration",
                () => s.CaptureState(),
                o => s.RestoreState((MigrationState)o));

        public void SetVictoryTheBroadcastSystem(Victory_TheBroadcast s) =>
            RegisterSystem(ref _victoryTheBroadcastSystem, s, "victory_the_broadcast",
                () => s.CaptureState(),
                o => s.RestoreState((BroadcastState)o));

        public void SetVictoryTheCureSystem(Victory_TheCure s) =>
            RegisterSystem(ref _victoryTheCureSystem, s, "victory_the_cure",
                () => s.CaptureState(),
                o => s.RestoreState((TheCureState)o));

        public void SetVictoryTheMartianSystem(Victory_TheMartian s) =>
            RegisterSystem(ref _victoryTheMartianSystem, s, "victory_the_martian",
                () => s.CaptureState(),
                o => s.RestoreState((TheMartianState)o));

        public void SetVictoryUndergroundCitySystem(Victory_UndergroundCity s) =>
            RegisterSystem(ref _victoryUndergroundCitySystem, s, "victory_underground_city",
                () => s.CaptureState(),
                o => s.RestoreState((UndergroundCityState)o));

        public void SetVictoryUnifierSystem(Victory_Unifier s) =>
            RegisterSystem(ref _victoryUnifierSystem, s, "victory_unifier",
                () => s.CaptureState(),
                o => s.RestoreState((UnifierState)o));

        public void SetMapHazardAcidGeyser(MapHazard_AcidGeyser s) =>
            RegisterSystem(ref _mapHazardAcidGeyser, s, "map_hazard_acid_geyser",
                () => s.CaptureState(),
                o => s.RestoreState((AcidGeyserState)o));

        public void SetMapHazardAshlanche(MapHazard_Ashlanche s) =>
            RegisterSystem(ref _mapHazardAshlanche, s, "map_hazard_ashlanche",
                () => s.CaptureState(),
                o => s.RestoreState((AshlancheState)o));

        public void SetMapHazardBiometricDoor(MapHazard_BiometricDoor s) =>
            RegisterSystem(ref _mapHazardBiometricDoor, s, "map_hazard_biometric_door",
                () => s.CaptureState(),
                o => s.RestoreState((BiometricDoorState)o));

        public void SetMapHazardCraterWall(MapHazard_CraterWall s) =>
            RegisterSystem(ref _mapHazardCraterWall, s, "map_hazard_crater_wall",
                () => s.CaptureState(),
                o => s.RestoreState((CraterWallState)o));

        public void SetMapHazardCrevice(MapHazard_Crevice s) =>
            RegisterSystem(ref _mapHazardCrevice, s, "map_hazard_crevice",
                () => s.CaptureState(),
                o => s.RestoreState((CreviceState)o));

        public void SetMapHazardFlammableGas(MapHazard_FlammableGas s) =>
            RegisterSystem(ref _mapHazardFlammableGas, s, "map_hazard_flammable_gas",
                () => s.CaptureState(),
                o => s.RestoreState((FlammableGasState)o));

        public void SetMapHazardGasPockets(MapHazard_GasPockets s) =>
            RegisterSystem(ref _mapHazardGasPockets, s, "map_hazard_gas_pockets",
                () => s.CaptureState(),
                o => s.RestoreState((GasPocketState)o));

        public void SetMapHazardMagneticAnomaly(MapHazard_MagneticAnomaly s) =>
            RegisterSystem(ref _mapHazardMagneticAnomaly, s, "map_hazard_magnetic_anomaly",
                () => s.CaptureState(),
                o => s.RestoreState((MagneticAnomalyState)o));

        public void SetMapHazardSinkholeCollapse(MapHazard_SinkholeCollapse s) =>
            RegisterSystem(ref _mapHazardSinkholeCollapse, s, "map_hazard_sinkhole_collapse",
                () => s.CaptureState(),
                o => s.RestoreState((UrbanSinkholeState)o));

        public void SetMapHazardVenusTrap(MapHazard_VenusTrap s) =>
            RegisterSystem(ref _mapHazardVenusTrap, s, "map_hazard_venus_trap",
                () => s.CaptureState(),
                o => s.RestoreState((VenusTrapState)o));

        public void SetMapHazardFrozenSurvivor(MapHazard_FrozenSurvivor s) =>
            RegisterSystem(ref _mapHazardFrozenSurvivor, s, "map_hazard_frozen_survivor",
                () => s.CaptureState(),
                o => s.RestoreState((FrozenSurvivorState)o));

        public void SetMapAnomalyAshDunes(MapAnomaly_AshDunes s) =>
            RegisterSystem(ref _mapAnomalyAshDunes, s, "map_anomaly_ash_dunes",
                () => s.CaptureState(), o => s.RestoreState((AshDunesState)o));

        public void SetMapAnomalyBoilingLake(MapAnomaly_BoilingLake s) =>
            RegisterSystem(ref _mapAnomalyBoilingLake, s, "map_anomaly_boiling_lake",
                () => s.CaptureState(), o => s.RestoreState((BoilingLakeState)o));

        public void SetMapAnomalyCherenkov(MapAnomaly_Cherenkov s) =>
            RegisterSystem(ref _mapAnomalyCherenkov, s, "map_anomaly_cherenkov",
                () => s.CaptureState(), o => s.RestoreState((CherenkovState)o));

        public void SetMapAnomalyDogDen(MapAnomaly_DogDen s) =>
            RegisterSystem(ref _mapAnomalyDogDen, s, "map_anomaly_dog_den",
                () => s.CaptureState(), o => s.RestoreState((DogDenState)o));

        public void SetMapAnomalyDontLook(MapAnomaly_DontLook s) =>
            RegisterSystem(ref _mapAnomalyDontLook, s, "map_anomaly_dont_look",
                () => s.CaptureState(), o => s.RestoreState((DontLookState)o));

        public void SetMapAnomalyDryCoral(MapAnomaly_DryCoral s) =>
            RegisterSystem(ref _mapAnomalyDryCoral, s, "map_anomaly_dry_coral",
                () => s.CaptureState(), o => s.RestoreState((DryCoralState)o));

        public void SetMapAnomalyFloodedSubway(MapAnomaly_FloodedSubway s) =>
            RegisterSystem(ref _mapAnomalyFloodedSubway, s, "map_anomaly_flooded_subway",
                () => s.CaptureState(), o => s.RestoreState((FloodedSubwayState)o));

        public void SetMapAnomalyGlassCrater(MapAnomaly_GlassCrater s) =>
            RegisterSystem(ref _mapAnomalyGlassCrater, s, "map_anomaly_glass_crater",
                () => s.CaptureState(), o => s.RestoreState((GlassCraterState)o));

        public void SetMapAnomalyMassGrave(MapAnomaly_MassGrave s) =>
            RegisterSystem(ref _mapAnomalyMassGrave, s, "map_anomaly_mass_grave",
                () => s.CaptureState(), o => s.RestoreState((MassGraveState)o));

        public void SetMapAnomalyMirage(MapAnomaly_Mirage s) =>
            RegisterSystem(ref _mapAnomalyMirage, s, "map_anomaly_mirage",
                () => s.CaptureState(), o => s.RestoreState((MirageState)o));

        public void SetMapAnomalyPetrifiedForest(MapAnomaly_PetrifiedForest s) =>
            RegisterSystem(ref _mapAnomalyPetrifiedForest, s, "map_anomaly_petrified_forest",
                () => s.CaptureState(), o => s.RestoreState((PetrifiedForestState)o));

        public void SetMapAnomalyQuietZone(MapAnomaly_QuietZone s) =>
            RegisterSystem(ref _mapAnomalyQuietZone, s, "map_anomaly_quiet_zone",
                () => s.CaptureState(), o => s.RestoreState((QuietZoneState)o));

        public void SetMapAnomalyRustedTank(MapAnomaly_RustedTank s) =>
            RegisterSystem(ref _mapAnomalyRustedTank, s, "map_anomaly_rusted_tank",
                () => s.CaptureState(), o => s.RestoreState((RustedTankState)o));

        public void SetMapAnomalyServerFarm(MapAnomaly_ServerFarm s) =>
            RegisterSystem(ref _mapAnomalyServerFarm, s, "map_anomaly_server_farm",
                () => s.CaptureState(), o => s.RestoreState((ServerFarmState)o));

        public void SetMapAnomalySinkhole(MapAnomaly_Sinkhole s) =>
            RegisterSystem(ref _mapAnomalySinkhole, s, "map_anomaly_sinkhole",
                () => s.CaptureState(), o => s.RestoreState((SinkholeState)o));

        public void SetMapAnomalyTangledDrop(MapAnomaly_TangledDrop s) =>
            RegisterSystem(ref _mapAnomalyTangledDrop, s, "map_anomaly_tangled_drop",
                () => s.CaptureState(), o => s.RestoreState((TangledDropState)o));

        public void SetMapAnomalyTireFire(MapAnomaly_TireFire s) =>
            RegisterSystem(ref _mapAnomalyTireFire, s, "map_anomaly_tire_fire",
                () => s.CaptureState(), o => s.RestoreState((TireFireState)o));

        public void SetMapAnomalyUxoNuke(MapAnomaly_UXO_Nuke s) =>
            RegisterSystem(ref _mapAnomalyUxoNuke, s, "map_anomaly_uxo_nuke",
                () => s.CaptureState(), o => s.RestoreState((UXONukeState)o));

        public void SetBiomeAshSwamp(Biome_AshSwamp s) =>
            RegisterSystem(ref _biomeAshSwamp, s, "biome_ash_swamp",
                () => s.CaptureState(), o => s.RestoreState((AshSwampState)o));

        public void SetBiomeGlassDesert(Biome_GlassDesert s) =>
            RegisterSystem(ref _biomeGlassDesert, s, "biome_glass_desert",
                () => s.CaptureState(), o => s.RestoreState((GlassDesertState)o));

        public void SetBiomeHighwayTunnel(Biome_HighwayTunnel s) =>
            RegisterSystem(ref _biomeHighwayTunnel, s, "biome_highway_tunnel",
                () => s.CaptureState(), o => s.RestoreState((HighwayTunnelState)o));

        public void SetBiomeSaltFlats(Biome_SaltFlats s) =>
            RegisterSystem(ref _biomeSaltFlats, s, "biome_salt_flats",
                () => s.CaptureState(), o => s.RestoreState((SaltFlatsState)o));

        public void SetBiomeSkyscraperTops(Biome_SkyscraperTops s) =>
            RegisterSystem(ref _biomeSkyscraperTops, s, "biome_skyscraper_tops",
                () => s.CaptureState(), o => s.RestoreState((SkyscraperTopsState)o));

        public void SetBiomeSuburbs(Biome_Suburbs s) =>
            RegisterSystem(ref _biomeSuburbs, s, "biome_suburbs",
                () => s.CaptureState(), o => s.RestoreState((SuburbsState)o));

        public void SetWeatherAcidSnow(Weather_AcidSnow s) =>
            RegisterSystem(ref _weatherAcidSnow, s, "weather_acid_snow",
                () => s.CaptureState(), o => s.RestoreState((AcidSnowState)o));

        // Prompts #319–#325 — Section X new weather events
        public void SetWeatherAshLightning(Weather_AshLightning s) =>
            RegisterSystem(ref _weatherAshLightning, s, "weather_ash_lightning",
                () => s.CaptureState(), o => s.RestoreState((AshLightningState)o));

        public void SetWeatherFogOfParticulate(Weather_FogOfParticulate s) =>
            RegisterSystem(ref _weatherFogOfParticulate, s, "weather_fog_of_particulate",
                () => s.CaptureState(), o => s.RestoreState((ParticulateFogState)o));

        public void SetWeatherThermalInversion(Weather_ThermalInversion s) =>
            RegisterSystem(ref _weatherThermalInversion, s, "weather_thermal_inversion",
                () => s.CaptureState(), o => s.RestoreState((ThermalInversionState)o));

        public void SetWeatherIceStorm(Weather_IceStorm s) =>
            RegisterSystem(ref _weatherIceStorm, s, "weather_ice_storm",
                () => s.CaptureState(), o => s.RestoreState((IceStormState)o));

        public void SetWeatherSilence(Weather_Silence s) =>
            RegisterSystem(ref _weatherSilence, s, "weather_silence",
                () => s.CaptureState(), o => s.RestoreState((SilenceState)o));

        public void SetWeatherBioFog(Weather_BioFog s) =>
            RegisterSystem(ref _weatherBioFog, s, "weather_bio_fog",
                () => s.CaptureState(), o => s.RestoreState((BioFogState)o));

        public void SetWeatherBlackSnow(Weather_BlackSnow s) =>
            RegisterSystem(ref _weatherBlackSnow, s, "weather_black_snow",
                () => s.CaptureState(), o => s.RestoreState((BlackSnowState)o));

        public void SetWeatherBloodRain(Weather_BloodRain s) =>
            RegisterSystem(ref _weatherBloodRain, s, "weather_blood_rain",
                () => s.CaptureState(), o => s.RestoreState((BloodRainState)o));

        public void SetWeatherDeadWind(Weather_DeadWind s) =>
            RegisterSystem(ref _weatherDeadWind, s, "weather_dead_wind",
                () => s.CaptureState(), o => s.RestoreState((DeadWindState)o));

        public void SetWeatherDeepFreeze(Weather_DeepFreeze s) =>
            RegisterSystem(ref _weatherDeepFreeze, s, "weather_deep_freeze",
                () => s.CaptureState(), o => s.RestoreState((DeepFreezeState)o));

        public void SetWeatherDustDevil(Weather_DustDevil s) =>
            RegisterSystem(ref _weatherDustDevil, s, "weather_dust_devil",
                () => s.CaptureState(), o => s.RestoreState((DustDevilState)o));

        public void SetWeatherEmpStorm(Weather_EMPStorm s) =>
            RegisterSystem(ref _weatherEmpStorm, s, "weather_emp_storm",
                () => s.CaptureState(), o => s.RestoreState((EMPStormState)o));

        public void SetWeatherFalseSpring(Weather_FalseSpring s) =>
            RegisterSystem(ref _weatherFalseSpring, s, "weather_false_spring",
                () => s.CaptureState(), o => s.RestoreState((FalseSpringState)o));

        public void SetWeatherGlassStorm(Weather_GlassStorm s) =>
            RegisterSystem(ref _weatherGlassStorm, s, "weather_glass_storm",
                () => s.CaptureState(), o => s.RestoreState((GlassStormState)o));

        public void SetWeatherOzoneHole(Weather_OzoneHole s) =>
            RegisterSystem(ref _weatherOzoneHole, s, "weather_ozone_hole",
                () => s.CaptureState(), o => s.RestoreState((OzoneHoleState)o));

        public void SetWeatherRadHail(Weather_RadHail s) =>
            RegisterSystem(ref _weatherRadHail, s, "weather_rad_hail",
                () => s.CaptureState(), o => s.RestoreState((RadHailState)o));

        public void SetWeatherSilentSpring(Weather_SilentSpring s) =>
            RegisterSystem(ref _weatherSilentSpring, s, "weather_silent_spring",
                () => s.CaptureState(), o => s.RestoreState((SilentSpringState)o));

        public void SetWeatherSolarFlare(Weather_SolarFlare s) =>
            RegisterSystem(ref _weatherSolarFlare, s, "weather_solar_flare",
                () => s.CaptureState(), o => s.RestoreState((SolarFlareState)o));

        public void SetWeatherStaticCharge(Weather_StaticCharge s) =>
            RegisterSystem(ref _weatherStaticCharge, s, "weather_static_charge",
                () => s.CaptureState(), o => s.RestoreState((StaticChargeState)o));

        public void SetEncounterAmalgamation(Encounter_Amalgamation s) =>
            RegisterSystem(ref _encounterAmalgamation, s, "encounter_amalgamation",
                () => s.CaptureState(), o => s.RestoreState((AmalgamationState)o));

        public void SetEncounterBurrowers(BurrowersSystem s) =>
            RegisterSystem(ref _encounterBurrowers, s, "encounter_burrowers",
                () => s.CaptureState(), o => s.RestoreState((BurrowersState)o));

        public void SetEncounterFloodedMaze(Encounter_FloodedMaze s) =>
            RegisterSystem(ref _encounterFloodedMaze, s, "encounter_flooded_maze",
                () => s.CaptureState(), o => s.RestoreState((FloodedMazeState)o));

        public void SetEncounterGlowingDead(Encounter_GlowingDead s) =>
            RegisterSystem(ref _encounterGlowingDead, s, "encounter_glowing_dead",
                () => s.CaptureState(), o => s.RestoreState((GlowingDeadState)o));

        public void SetEncounterGlowingStag(Encounter_GlowingStag s) =>
            RegisterSystem(ref _encounterGlowingStag, s, "encounter_glowing_stag",
                () => s.CaptureState(), o => s.RestoreState((GlowingStagState)o));

        public void SetEncounterHitAndRun(Encounter_HitAndRun s) =>
            RegisterSystem(ref _encounterHitAndRun, s, "encounter_hit_and_run",
                () => s.CaptureState(), o => s.RestoreState((HitAndRunState)o));

        public void SetEncounterLeeches(Encounter_Leeches s) =>
            RegisterSystem(ref _encounterLeeches, s, "encounter_leeches",
                () => s.CaptureState(), o => s.RestoreState((LeechesSaveState)o));

        public void SetEncounterMirelurker(Encounter_Mirelurker s) =>
            RegisterSystem(ref _encounterMirelurker, s, "encounter_mirelurker",
                () => s.CaptureState(), o => s.RestoreState((MirelurkerState)o));

        public void SetEncounterPressurePlate(Encounter_PressurePlate s) =>
            RegisterSystem(ref _encounterPressurePlate, s, "encounter_pressure_plate",
                () => s.CaptureState(), o => s.RestoreState((PressurePlateState)o));

        public void SetEncounterRiverPirates(Encounter_RiverPirates s) =>
            RegisterSystem(ref _encounterRiverPirates, s, "encounter_river_pirates",
                () => s.CaptureState(), o => s.RestoreState((RiverPiratesState)o));

        public void SetEncounterRoadblock(Encounter_Roadblock s) =>
            RegisterSystem(ref _encounterRoadblock, s, "encounter_roadblock",
                () => s.CaptureState(), o => s.RestoreState((RoadblockState)o));

        public void SetEncounterRobotDog(Encounter_RobotDog s) =>
            RegisterSystem(ref _encounterRobotDog, s, "encounter_robot_dog",
                () => s.CaptureState(), o => s.RestoreState((RobotDogState)o));

        public void SetEncounterSleepingCamp(Encounter_SleepingCamp s) =>
            RegisterSystem(ref _encounterSleepingCamp, s, "encounter_sleeping_camp",
                () => s.CaptureState(), o => s.RestoreState((SleepingCampState)o));

        public void SetEncounterTripwireMaze(Encounter_TripwireMaze s) =>
            RegisterSystem(ref _encounterTripwireMaze, s, "encounter_tripwire_maze",
                () => s.CaptureState(), o => s.RestoreState((TripwireMazeState)o));

        public void SetEncounterWarlordTank(Encounter_WarlordTank s) =>
            RegisterSystem(ref _encounterWarlordTank, s, "encounter_warlord_tank",
                () => s.CaptureState(), o => s.RestoreState((WarlordTankState)o));

        public void SetShelterModuleAcidTrap(ShelterModule_AcidTrap s) =>
            RegisterSystem(ref _shelterModuleAcidTrap, s, "shelter_module_acid_trap",
                () => s.CaptureState(), o => s.RestoreState((AcidTrapState)o));

        public void SetShelterModuleAutodoc(ShelterModule_Autodoc s) =>
            RegisterSystem(ref _shelterModuleAutodoc, s, "shelter_module_autodoc",
                () => s.CaptureState(), o => s.RestoreState((AutodocState)o));

        public void SetShelterModuleCctv(ShelterModule_CCTV s) =>
            RegisterSystem(ref _shelterModuleCctv, s, "shelter_module_cctv",
                () => s.CaptureState(), o => s.RestoreState((CCTVState)o));

        public void SetShelterModuleClassroom(ShelterModule_Classroom s) =>
            RegisterSystem(ref _shelterModuleClassroom, s, "shelter_module_classroom",
                () => s.CaptureState(), o => s.RestoreState((ClassroomState)o));

        public void SetShelterModuleConfessional(ShelterModule_Confessional s) =>
            RegisterSystem(ref _shelterModuleConfessional, s, "shelter_module_confessional",
                () => s.CaptureState(), o => s.RestoreState((ConfessionalModuleState)o));

        public void SetShelterModuleConveyor(ShelterModule_Conveyor s) =>
            RegisterSystem(ref _shelterModuleConveyor, s, "shelter_module_conveyor",
                () => s.CaptureState(), o => s.RestoreState((ConveyorState)o));

        public void SetShelterModuleDaylightSensor(ShelterModule_DaylightSensor s) =>
            RegisterSystem(ref _shelterModuleDaylightSensor, s, "shelter_module_daylight_sensor",
                () => s.CaptureState(), o => s.RestoreState((DaylightSensorState)o));

        public void SetShelterModuleDroneStation(ShelterModule_DroneStation s) =>
            RegisterSystem(ref _shelterModuleDroneStation, s, "shelter_module_drone_station",
                () => s.CaptureState(), o => s.RestoreState((DroneStationState)o));

        public void SetShelterModuleHoloEmitter(ShelterModule_HoloEmitter s) =>
            RegisterSystem(ref _shelterModuleHoloEmitter, s, "shelter_module_holo_emitter",
                () => s.CaptureState(), o => s.RestoreState((HoloEmitterState)o));

        public void SetShelterModuleInsectFarm(ShelterModule_InsectFarm s) =>
            RegisterSystem(ref _shelterModuleInsectFarm, s, "shelter_module_insect_farm",
                () => s.CaptureState(), o => s.RestoreState((InsectFarmState)o));

        public void SetShelterModuleLathe(ShelterModule_Lathe s) =>
            RegisterSystem(ref _shelterModuleLathe, s, "shelter_module_lathe",
                () => s.CaptureState(), o => s.RestoreState((LatheState)o));

        public void SetShelterModuleMortar(ShelterModule_Mortar s) =>
            RegisterSystem(ref _shelterModuleMortar, s, "shelter_module_mortar",
                () => s.CaptureState(), o => s.RestoreState((MortarState)o));

        public void SetShelterModulePanicButton(ShelterModule_PanicButton s) =>
            RegisterSystem(ref _shelterModulePanicButton, s, "shelter_module_panic_button",
                () => s.CaptureState(), o => s.RestoreState((PanicButtonState)o));

        public void SetShelterModulePitfall(ShelterModule_Pitfall s) =>
            RegisterSystem(ref _shelterModulePitfall, s, "shelter_module_pitfall",
                () => s.CaptureState(), o => s.RestoreState((PitfallSave)o));

        public void SetShelterModuleReloader(ShelterModule_Reloader s) =>
            RegisterSystem(ref _shelterModuleReloader, s, "shelter_module_reloader",
                () => s.CaptureState(), o => s.RestoreState((ReloaderState)o));

        public void SetShelterModuleSorter(ShelterModule_Sorter s) =>
            RegisterSystem(ref _shelterModuleSorter, s, "shelter_module_sorter",
                () => s.CaptureState(), o => s.RestoreState((SorterState)o));

        public void SetShelterModuleThermostat(ShelterModule_Thermostat s) =>
            RegisterSystem(ref _shelterModuleThermostat, s, "shelter_module_thermostat",
                () => s.CaptureState(), o => s.RestoreState((ThermostatState)o));

        public void SetShelterModuleWasteChute(ShelterModule_WasteChute s) =>
            RegisterSystem(ref _shelterModuleWasteChute, s, "shelter_module_waste_chute",
                () => s.CaptureState(), o => s.RestoreState((WasteChuteState)o));

        public void SetShelterModuleAutopsy(ShelterModule_Autopsy s) =>
            RegisterSystem(ref _shelterModuleAutopsy, s, "shelter_module_autopsy",
                () => s.CaptureState(), o => s.RestoreState((AutopsyTableState)o));

        public void SetShelterModuleBatteryBank(ShelterModule_BatteryBank s) =>
            RegisterSystem(ref _shelterModuleBatteryBank, s, "shelter_module_battery_bank",
                () => s.CaptureState(), o => s.RestoreState((BatteryBankState)o));

        public void SetShelterModuleBioLatrine(ShelterModule_BioLatrine s) =>
            RegisterSystem(ref _shelterModuleBioLatrine, s, "shelter_module_bio_latrine",
                () => s.CaptureState(), o => s.RestoreState((BioLatrineState)o));

        public void SetShelterModuleChoreBoard(ShelterModule_ChoreBoard s) =>
            RegisterSystem(ref _shelterModuleChoreBoard, s, "shelter_module_chore_board",
                () => s.CaptureState(), o => s.RestoreState((ChoreBoardState)o));

        public void SetShelterModuleDeadManSwitch(ShelterModule_DeadManSwitch s) =>
            RegisterSystem(ref _shelterModuleDeadManSwitch, s, "shelter_module_dead_man_switch",
                () => s.CaptureState(), o => s.RestoreState((DeadManSwitchState)o));

        public void SetShelterModuleDeconShower(ShelterModule_DeconShower s) =>
            RegisterSystem(ref _shelterModuleDeconShower, s, "shelter_module_decon_shower",
                () => s.CaptureState(), o => s.RestoreState((DeconShowerState)o));

        public void SetShelterModuleDialysis(ShelterModule_Dialysis s) =>
            RegisterSystem(ref _shelterModuleDialysis, s, "shelter_module_dialysis",
                () => s.CaptureState(), o => s.RestoreState((ShelterModule_DialysisState)o));

        public void SetShelterModuleDistressBeacon(ShelterModule_DistressBeacon s) =>
            RegisterSystem(ref _shelterModuleDistressBeacon, s, "shelter_module_distress_beacon",
                () => s.CaptureState(), o => s.RestoreState((DistressBeaconState)o));

        public void SetShelterModuleDronePad(ShelterModule_DronePad s) =>
            RegisterSystem(ref _shelterModuleDronePad, s, "shelter_module_drone_pad",
                () => s.CaptureState(), o => s.RestoreState((DronePadState)o));

        public void SetShelterModuleGarage(ShelterModule_Garage s) =>
            RegisterSystem(ref _shelterModuleGarage, s, "shelter_module_garage",
                () => s.CaptureState(), o => s.RestoreState((GarageModuleState)o));

        public void SetShelterModuleGunRack(ShelterModule_GunRack s) =>
            RegisterSystem(ref _shelterModuleGunRack, s, "shelter_module_gun_rack",
                () => s.CaptureState(), o => s.RestoreState((GunRackState)o));

        public void SetShelterModuleHammock(ShelterModule_Hammock s) =>
            RegisterSystem(ref _shelterModuleHammock, s, "shelter_module_hammock",
                () => s.CaptureState(), o => s.RestoreState((HammockModuleState)o));

        public void SetShelterModuleHandCrank(ShelterModule_HandCrank s) =>
            RegisterSystem(ref _shelterModuleHandCrank, s, "shelter_module_hand_crank",
                () => s.CaptureState(), o => s.RestoreState((HandCrankState)o));

        public void SetShelterModuleHotShower(ShelterModule_HotShower s) =>
            RegisterSystem(ref _shelterModuleHotShower, s, "shelter_module_hot_shower",
                () => s.CaptureState(), o => s.RestoreState((HotShowerState)o));

        public void SetShelterModuleIncinerator(ShelterModule_Incinerator s) =>
            RegisterSystem(ref _shelterModuleIncinerator, s, "shelter_module_incinerator",
                () => s.CaptureState(), o => s.RestoreState((IncineratorState)o));

        public void SetShelterModuleMagmaTap(MagmaTapSystem s) =>
            RegisterSystem(ref _shelterModuleMagmaTap, s, "shelter_module_magma_tap",
                () => s.CaptureState(), o => s.RestoreState((MagmaTapState)o));

        public void SetShelterModuleMotionSensor(ShelterModule_MotionSensor s) =>
            RegisterSystem(ref _shelterModuleMotionSensor, s, "shelter_module_motion_sensor",
                () => s.CaptureState(), o => s.RestoreState((MotionSensorState)o));

        public void SetShelterModulePanicRoom(PanicRoomSystem s) =>
            RegisterSystem(ref _shelterModulePanicRoom, s, "shelter_module_panic_room",
                () => s.CaptureState(), o => s.RestoreState((PanicRoomState)o));

        public void SetShelterModulePrintingPress(ShelterModule_PrintingPress s) =>
            RegisterSystem(ref _shelterModulePrintingPress, s, "shelter_module_printing_press",
                () => s.CaptureState(), o => s.RestoreState((PrintingPressState)o));

        public void SetShelterModulePunchingBag(ShelterModule_PunchingBag s) =>
            RegisterSystem(ref _shelterModulePunchingBag, s, "shelter_module_punching_bag",
                () => s.CaptureState(), o => s.RestoreState((PunchingBagState)o));

        public void SetShelterModuleRainBarrel(ShelterModule_RainBarrel s) =>
            RegisterSystem(ref _shelterModuleRainBarrel, s, "shelter_module_rain_barrel",
                () => s.CaptureState(), o => s.RestoreState((RainBarrelState)o));

        public void SetShelterModuleRecordPlayer(ShelterModule_RecordPlayer s) =>
            RegisterSystem(ref _shelterModuleRecordPlayer, s, "shelter_module_record_player",
                () => s.CaptureState(), o => s.RestoreState((RecordPlayerState)o));

        public void SetShelterModuleSprinklers(ShelterModule_Sprinklers s) =>
            RegisterSystem(ref _shelterModuleSprinklers, s, "shelter_module_sprinklers",
                () => s.CaptureState(), o => s.RestoreState((SprinklersState)o));

        public void SetShelterModuleThumper(ThumperSystem s) =>
            RegisterSystem(ref _shelterModuleThumper, s, "shelter_module_thumper",
                () => s.CaptureState(), o => s.RestoreState((ThumperState)o));

        public void SetShelterModuleTreadmillGen(ShelterModule_TreadmillGen s) =>
            RegisterSystem(ref _shelterModuleTreadmillGen, s, "shelter_module_treadmill_gen",
                () => s.CaptureState(), o => s.RestoreState((TreadmillGenState)o));

        public void SetShelterModuleTurret(ShelterModule_Turret s) =>
            RegisterSystem(ref _shelterModuleTurret, s, "shelter_module_turret",
                () => s.CaptureState(), o => s.RestoreState((TurretModuleState)o));

        public void SetShelterModuleVaultDoor(ShelterModule_VaultDoor s) =>
            RegisterSystem(ref _shelterModuleVaultDoor, s, "shelter_module_vault_door",
                () => s.CaptureState(), o => s.RestoreState((VaultDoorState)o));

        public void SetShelterModuleWoodStove(ShelterModule_WoodStove s) =>
            RegisterSystem(ref _shelterModuleWoodStove, s, "shelter_module_wood_stove",
                () => s.CaptureState(), o => s.RestoreState((WoodStoveState)o));

        public void SetEventBrawl(Event_Brawl s) =>
            RegisterSystem(ref _eventBrawl, s, "event_brawl",
                () => s.CaptureState(), o => s.RestoreState((BrawlState)o));

        public void SetEventComingOfAge(Event_ComingOfAge s) =>
            RegisterSystem(ref _eventComingOfAge, s, "event_coming_of_age",
                () => s.CaptureState(), o => s.RestoreState((ComingOfAgeState)o));

        public void SetEventCultBlessing(Event_CultBlessing s) =>
            RegisterSystem(ref _eventCultBlessing, s, "event_cult_blessing",
                () => s.CaptureState(), o => s.RestoreState((CultBlessingState)o));

        public void SetEventCultInitiation(Event_CultInitiation s) =>
            RegisterSystem(ref _eventCultInitiation, s, "event_cult_initiation",
                () => s.CaptureState(), o => s.RestoreState((CultInitiationState)o));

        public void SetEventCultOfAi(Event_CultOfAI s) =>
            RegisterSystem(ref _eventCultOfAi, s, "event_cult_of_ai",
                () => s.CaptureState(), o => s.RestoreState((CultOfAISave)o));

        public void SetEventEmpCascade(Event_EMPCascade s) =>
            RegisterSystem(ref _eventEmpCascade, s, "event_emp_cascade",
                () => s.CaptureState(), o => s.RestoreState((EMPCascadeState)o));

        public void SetEventFeralRescue(Event_FeralRescue s) =>
            RegisterSystem(ref _eventFeralRescue, s, "event_feral_rescue",
                () => s.CaptureState(), o => s.RestoreState((FeralRescueState)o));

        public void SetEventFoundDiary(Event_FoundDiary s) =>
            RegisterSystem(ref _eventFoundDiary, s, "event_found_diary",
                () => s.CaptureState(), o => s.RestoreState((FoundDiaryState)o));

        public void SetEventGriefCascade(Event_GriefCascade s) =>
            RegisterSystem(ref _eventGriefCascade, s, "event_grief_cascade",
                () => s.CaptureState(), o => s.RestoreState((GriefCascadeState)o));

        public void SetEventHungerStrike(Event_HungerStrike s) =>
            RegisterSystem(ref _eventHungerStrike, s, "event_hunger_strike",
                () => s.CaptureState(), o => s.RestoreState((HungerStrikeState)o));

        public void SetEventNodeCollapse(Event_NodeCollapse s) =>
            RegisterSystem(ref _eventNodeCollapse, s, "event_node_collapse",
                () => s.CaptureState(), o => s.RestoreState((NodeCollapseState)o));

        public void SetEventRansomNote(Event_RansomNote s) =>
            RegisterSystem(ref _eventRansomNote, s, "event_ransom_note",
                () => s.CaptureState(), o => s.RestoreState((RansomNoteState)o));

        public void SetEventSchism(Event_Schism s) =>
            RegisterSystem(ref _eventSchism, s, "event_schism",
                () => s.CaptureState(), o => s.RestoreState((SchismState)o));

        public void SetEventSecretSociety(Event_SecretSociety s) =>
            RegisterSystem(ref _eventSecretSociety, s, "event_secret_society",
                () => s.CaptureState(), o => s.RestoreState((SecretSocietyState)o));

        public void SetEventSiblingFeud(Event_SiblingFeud s) =>
            RegisterSystem(ref _eventSiblingFeud, s, "event_sibling_feud",
                () => s.CaptureState(), o => s.RestoreState((SiblingFeudState)o));

        public void SetEventSpontaneousMurder(Event_SpontaneousMurder s) =>
            RegisterSystem(ref _eventSpontaneousMurder, s, "event_spontaneous_murder",
                () => s.CaptureState(), o => s.RestoreState((SpontaneousMurderState)o));

        public void SetEventTeenRebellion(Event_TeenRebellion s) =>
            RegisterSystem(ref _eventTeenRebellion, s, "event_teen_rebellion",
                () => s.CaptureState(), o => s.RestoreState((TeenRebellionState)o));

        public void SetEventWitchHunt(Event_WitchHunt s) =>
            RegisterSystem(ref _eventWitchHunt, s, "event_witch_hunt",
                () => s.CaptureState(), o => s.RestoreState((WitchHuntState)o));

        public void SetEventEuthanasiaPact(Event_EuthanasiaPact s) =>
            RegisterSystem(ref _eventEuthanasiaPact, s, "event_euthanasia_pact",
                () => s.CaptureState(), o => s.RestoreState((Event_EuthanasiaPactState)o));

        public void SetEventFactionMerger(Event_FactionMerger s) =>
            RegisterSystem(ref _eventFactionMerger, s, "event_faction_merger",
                () => s.CaptureState(), o => s.RestoreState((FactionMergerState)o));

        public void SetEventMudslide(Event_Mudslide s) =>
            RegisterSystem(ref _eventMudslide, s, "event_mudslide",
                () => s.CaptureState(), o => s.RestoreState((MudslideState)o));

        public void SetEventNumbersStation(Event_NumbersStation s) =>
            RegisterSystem(ref _eventNumbersStation, s, "event_numbers_station",
                () => s.CaptureState(), o => s.RestoreState((NumbersStationState)o));

        public void SetEventProjectSabotage(Event_ProjectSabotage s) =>
            RegisterSystem(ref _eventProjectSabotage, s, "event_project_sabotage",
                () => s.CaptureState(), o => s.RestoreState((ProjectSabotageState)o));

        public void SetEventSinkhole(FoundationSinkholeSystem s) =>
            RegisterSystem(ref _eventSinkhole, s, "event_sinkhole",
                () => s.CaptureState(), o => s.RestoreState((FoundationSinkholeState)o));

        public void SetEventTriangulation(Event_Triangulation s) =>
            RegisterSystem(ref _eventTriangulation, s, "event_triangulation",
                () => s.CaptureState(), o => s.RestoreState((TriangulationState)o));

        public void SetEventVaultCollision(VaultCollisionSystem s) =>
            RegisterSystem(ref _eventVaultCollision, s, "event_vault_collision",
                () => s.CaptureState(), o => s.RestoreState((VaultCollisionState)o));

        public void SetEventWarlordSuccession(Event_WarlordSuccession s) =>
            RegisterSystem(ref _eventWarlordSuccession, s, "event_warlord_succession",
                () => s.CaptureState(), o => s.RestoreState((WarlordSuccessionState)o));

        public void SetSiegeArtillerySystem(Siege_Artillery s) =>
            RegisterSystem(ref _siegeArtillerySystem, s, "siege_artillery",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeArtilleryState)o));

        public void SetSiegeBiowarfareSystem(Siege_Biowarfare s) =>
            RegisterSystem(ref _siegeBiowarfareSystem, s, "siege_biowarfare",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeBiowarfareState)o));

        public void SetSiegeBlockadeSystem(Siege_Blockade s) =>
            RegisterSystem(ref _siegeBlockadeSystem, s, "siege_blockade",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeBlockadeState)o));

        public void SetSiegeHostageShieldSystem(Siege_HostageShield s) =>
            RegisterSystem(ref _siegeHostageShieldSystem, s, "siege_hostage_shield",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeHostageShieldState)o));

        public void SetSiegeNightRaidSystem(Siege_NightRaid s) =>
            RegisterSystem(ref _siegeNightRaidSystem, s, "siege_night_raid",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeNightRaidState)o));

        public void SetSiegeSappersSystem(Siege_Sappers s) =>
            RegisterSystem(ref _siegeSappersSystem, s, "siege_sappers",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeSappersState)o));

        public void SetSiegeSmokeOutSystem(Siege_SmokeOut s) =>
            RegisterSystem(ref _siegeSmokeOutSystem, s, "siege_smoke_out",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeSmokeOutState)o));

        public void SetSiegeVehicleRamSystem(Siege_VehicleRam s) =>
            RegisterSystem(ref _siegeVehicleRamSystem, s, "siege_vehicle_ram",
                () => s.CaptureState(),
                o => s.RestoreState((SiegeVehicleRamState)o));

        /// <summary>River crossings / bridges / blockades (had CaptureState, never constructed/registered).</summary>
        public void SetRiverNodeSystem(RiverNodeSystem s) =>
            RegisterSystem(ref _riverNodeSystem, s, "river_nodes",
                () => s.CaptureState(),
                o => s.RestoreState((RiverNodeSave)o));

        public void SetLifeboatTransmissionSystem(LifeboatTransmissionSystem lifeboat)
        {
            _lifeboat = lifeboat;
            if (lifeboat != null)
                Register(new SaveableAdapter("lifeboat", () => lifeboat.CaptureState(), o => lifeboat.RestoreState((LifeboatTransmissionSave)o)));
        }

        /// <summary>Inject proc-gen wasteland map (reveal/visit flags + seed).</summary>
        public void SetGeneratedMap(GeneratedMap generatedMap)
        {
            _generatedMap = generatedMap;
        }

        /// <summary>Inject bunker water cisterns (clean/dirty/irradiated) for save/load.</summary>
        public void SetWaterStorage(WaterStorage waterStorage) =>
            RegisterSystem(ref _waterStorage, waterStorage, "water_storage",
                () => waterStorage.CaptureState(),
                o => waterStorage.RestoreState((WaterStorageSave)o));

        /// <summary>Inject mental-break system so affinity matrix persists across save/load.</summary>
        public void SetMentalBreakSystem(MentalBreakSystem mentalBreakSystem)
        {
            _mentalBreakSystem = mentalBreakSystem;
        }

        /// <summary>Inject phantom-intruder system so cooldowns persist across save/load.</summary>
        public void SetPhantomIntruderSystem(PhantomIntruderSystem phantomIntruderSystem)
        {
            _phantomIntruderSystem = phantomIntruderSystem;
        }

        /// <summary>Inject Day-30 Flashpoint Choreographer adapter so the
        /// choreography checkpoint (buildup days processed, current step)
        /// persists across save/load. The Capture delegate returns the
        /// current state; the Restore delegate applies a loaded snapshot.
        /// Optional; safe to skip if no choreographer is wired.</summary>
        public void SetFlashpointChoreographer(
            Func<FlashpointChoreographerSave> capture,
            Action<FlashpointChoreographerSave> restore)
        {
            _captureChoreographer = capture;
            _restoreChoreographer = restore;
        }

        /// <summary>
        /// Hook invoked immediately before building a save snapshot (quicksave,
        /// autosave, named slots). Used to flush live HUD presentation into
        /// systems (radio open/unread/tuner).
        /// </summary>
        public void SetPreCaptureHook(Action preCapture)
        {
            _preCaptureHook = preCapture;
        }

        // ── CoreFamilies bulk Set methods (auto) ────────────────────────
        public void SetFalloutStormHazard(FalloutStormHazardSystem s) =>
            RegisterSystem(ref _falloutStormHazard, s, "fallout_storm_hazard_system",
                () => s.CaptureState(), o => s.RestoreState((FalloutStormHazardSystemSave)o));

        public void SetActionCrawlspace(Action_Crawlspace s) =>
            RegisterSystem(ref _actionCrawlspace, s, "action_crawlspace",
                () => s.CaptureState(), o => s.RestoreState((CrawlspaceState)o));

        public void SetActionPlay(Action_Play s) =>
            RegisterSystem(ref _actionPlay, s, "action_play",
                () => s.CaptureState(), o => s.RestoreState((PlayState)o));

        public void SetActionSlaughterPet(Action_SlaughterPet s) =>
            RegisterSystem(ref _actionSlaughterPet, s, "action_slaughter_pet",
                () => s.CaptureState(), o => s.RestoreState((SlaughterPetState)o));

        public void SetActionTeachChild(Action_TeachChild s) =>
            RegisterSystem(ref _actionTeachChild, s, "action_teach_child",
                () => s.CaptureState(), o => s.RestoreState((TeachChildState)o));

        public void SetActionTellStories(Action_TellStories s) =>
            RegisterSystem(ref _actionTellStories, s, "action_tell_stories",
                () => s.CaptureState(), o => s.RestoreState((TellStoriesState)o));

        public void SetItemAshGoat(Item_AshGoat s) =>
            RegisterSystem(ref _itemAshGoat, s, "item_ash_goat",
                () => s.CaptureState(), o => s.RestoreState((AshGoatState)o));

        public void SetItemBoots(Item_Boots s) =>
            RegisterSystem(ref _itemBoots, s, "item_boots",
                () => s.CaptureState(), o => s.RestoreState((ItemBootsSave)o));

        public void SetItemLiveTrap(Item_LiveTrap s) =>
            RegisterSystem(ref _itemLiveTrap, s, "item_live_trap",
                () => s.CaptureState(), o => s.RestoreState((LiveTrapState)o));

        public void SetItemMutantChicken(Item_MutantChicken s) =>
            RegisterSystem(ref _itemMutantChicken, s, "item_mutant_chicken",
                () => s.CaptureState(), o => s.RestoreState((MutantChickenState)o));

        public void SetItemToys(Item_Toys s) =>
            RegisterSystem(ref _itemToys, s, "item_toys",
                () => s.CaptureState(), o => s.RestoreState((ToyState)o));

        public void SetTraitAshTongue(Trait_AshTongue s) =>
            RegisterSystem(ref _traitAshTongue, s, "trait_ash_tongue",
                () => s.CaptureState(), o => s.RestoreState((TraitAshTongueSave)o));

        public void SetTraitKleptomaniac(Trait_Kleptomaniac s) =>
            RegisterSystem(ref _traitKleptomaniac, s, "trait_kleptomaniac",
                () => s.CaptureState(), o => s.RestoreState((TraitKleptomaniacSave)o));

        public void SetTraitMascot(Trait_Mascot s) =>
            RegisterSystem(ref _traitMascot, s, "trait_mascot",
                () => s.CaptureState(), o => s.RestoreState((MascotState)o));

        public void SetTraitStuntedEmpathy(Trait_StuntedEmpathy s) =>
            RegisterSystem(ref _traitStuntedEmpathy, s, "trait_stunted_empathy",
                () => s.CaptureState(), o => s.RestoreState((StuntedEmpathyState)o));

        public void SetTraitSuperstitious(Trait_Superstitious s) =>
            RegisterSystem(ref _traitSuperstitious, s, "trait_superstitious",
                () => s.CaptureState(), o => s.RestoreState((TraitSuperstitiousSave)o));

        public void SetAfflictionBunkerFever(Affliction_BunkerFever s) =>
            RegisterSystem(ref _afflictionBunkerFever, s, "affliction_bunker_fever",
                () => s.CaptureState(), o => s.RestoreState((AfflictionBunkerFeverSave)o));

        public void SetAfflictionZoonoticFlu(Affliction_ZoonoticFlu s) =>
            RegisterSystem(ref _afflictionZoonoticFlu, s, "affliction_zoonotic_flu",
                () => s.CaptureState(), o => s.RestoreState((ZoonoticFluState)o));

        public void SetModuleRationLock(Module_RationLock s) =>
            RegisterSystem(ref _moduleRationLock, s, "module_ration_lock",
                () => s.CaptureState(), o => s.RestoreState((RationLockState)o));

        public void SetNodeOrphanage(Node_Orphanage s) =>
            RegisterSystem(ref _nodeOrphanage, s, "node_orphanage",
                () => s.CaptureState(), o => s.RestoreState((OrphanageState)o));

        public void SetPetGuardDog(Pet_GuardDog s) =>
            RegisterSystem(ref _petGuardDog, s, "pet_guard_dog",
                () => s.CaptureState(), o => s.RestoreState((GuardDogState)o));

        public void SetActionAdministerPlacebo(Action_AdministerPlacebo s) =>
            RegisterSystem(ref _actionAdministerPlacebo, s, "action_administer_placebo",
                () => s.CaptureState(), o => s.RestoreState((AdministerPlaceboState)o));

        public void SetActionBarricadeDoor(Action_BarricadeDoor s) =>
            RegisterSystem(ref _actionBarricadeDoor, s, "action_barricade_door",
                () => s.CaptureState(), o => s.RestoreState((BarricadeState)o));

        public void SetActionBoilBatteries(Action_BoilBatteries s) =>
            RegisterSystem(ref _actionBoilBatteries, s, "action_boil_batteries",
                () => s.CaptureState(), o => s.RestoreState((BoilBatteriesState)o));

        public void SetActionBroadcastPropaganda(Action_BroadcastPropaganda s) =>
            RegisterSystem(ref _actionBroadcastPropaganda, s, "action_broadcast_propaganda",
                () => s.CaptureState(), o => s.RestoreState((PropagandaState)o));

        public void SetActionBurnCharcoal(Action_BurnCharcoal s) =>
            RegisterSystem(ref _actionBurnCharcoal, s, "action_burn_charcoal",
                () => s.CaptureState(), o => s.RestoreState((BurnCharcoalState)o));

        public void SetActionBuryTimeCapsule(Action_BuryTimeCapsule s) =>
            RegisterSystem(ref _actionBuryTimeCapsule, s, "action_bury_time_capsule",
                () => s.CaptureState(), o => s.RestoreState((TimeCapsuleState)o));

        public void SetActionCallCaravan(Action_CallCaravan s) =>
            RegisterSystem(ref _actionCallCaravan, s, "action_call_caravan",
                () => s.CaptureState(), o => s.RestoreState((CallCaravanState)o));

        public void SetActionCoverTracks(Action_CoverTracks s) =>
            RegisterSystem(ref _actionCoverTracks, s, "action_cover_tracks",
                () => s.CaptureState(), o => s.RestoreState((CoverTracksSave)o));

        public void SetActionCrackMainframe(Action_CrackMainframe s) =>
            RegisterSystem(ref _actionCrackMainframe, s, "action_crack_mainframe",
                () => s.CaptureState(), o => s.RestoreState((CrackMainframeState)o));

        public void SetActionDecrypt(Action_Decrypt s) =>
            RegisterSystem(ref _actionDecrypt, s, "action_decrypt",
                () => s.CaptureState(), o => s.RestoreState((DecryptState)o));

        public void SetActionDemandTribute(Action_DemandTribute s) =>
            RegisterSystem(ref _actionDemandTribute, s, "action_demand_tribute",
                () => s.CaptureState(), o => s.RestoreState((DemandTributeState)o));

        public void SetActionEstablishRoute(Action_EstablishRoute s) =>
            RegisterSystem(ref _actionEstablishRoute, s, "action_establish_route",
                () => s.CaptureState(), o => s.RestoreState((EstablishRouteState)o));

        public void SetActionExile(Action_Exile s) =>
            RegisterSystem(ref _actionExile, s, "action_exile",
                () => s.CaptureState(), o => s.RestoreState((ExileActionState)o));

        public void SetActionFish(Action_Fish s) =>
            RegisterSystem(ref _actionFish, s, "action_fish",
                () => s.CaptureState(), o => s.RestoreState((FishActionState)o));

        public void SetActionHarvestOrgans(Action_HarvestOrgans s) =>
            RegisterSystem(ref _actionHarvestOrgans, s, "harvest_organs",
                () => s.CaptureState(), o => s.RestoreState((Action_HarvestOrgansState)o));

        public void SetActionInfectSelf(Action_InfectSelf s) =>
            RegisterSystem(ref _actionInfectSelf, s, "infect_self",
                () => s.CaptureState(), o => s.RestoreState((Action_InfectSelfState)o));

        public void SetActionIsotopeTrace(Action_IsotopeTrace s) =>
            RegisterSystem(ref _actionIsotopeTrace, s, "action_isotope_trace",
                () => s.CaptureState(), o => s.RestoreState((IsotopeTraceState)o));

        public void SetActionMercy(Action_Mercy s) =>
            RegisterSystem(ref _actionMercy, s, "action_mercy",
                () => s.CaptureState(), o => s.RestoreState((MercyActionState)o));

        public void SetActionMixCement(Action_MixCement s) =>
            RegisterSystem(ref _actionMixCement, s, "action_mix_cement",
                () => s.CaptureState(), o => s.RestoreState((MixCementSave)o));

        public void SetActionMixChems(Action_MixChems s) =>
            RegisterSystem(ref _actionMixChems, s, "mix_chems",
                () => s.CaptureState(), o => s.RestoreState((Action_MixChemsState)o));

        public void SetActionOverwatch(Action_Overwatch s) =>
            RegisterSystem(ref _actionOverwatch, s, "action_overwatch",
                () => s.CaptureState(), o => s.RestoreState((OverwatchState)o));

        public void SetActionPhysicalTherapy(Action_PhysicalTherapy s) =>
            RegisterSystem(ref _actionPhysicalTherapy, s, "action_physical_therapy",
                () => s.CaptureState(), o => s.RestoreState((PhysicalTherapyState)o));

        public void SetActionPirateRadio(Action_PirateRadio s) =>
            RegisterSystem(ref _actionPirateRadio, s, "action_pirate_radio",
                () => s.CaptureState(), o => s.RestoreState((PirateRadioState)o));

        public void SetActionPlaceBait(Action_PlaceBait s) =>
            RegisterSystem(ref _actionPlaceBait, s, "action_place_bait",
                () => s.CaptureState(), o => s.RestoreState((BaitStationState)o));

        public void SetActionPullTooth(Action_PullTooth s) =>
            RegisterSystem(ref _actionPullTooth, s, "action_pull_tooth",
                () => s.CaptureState(), o => s.RestoreState((PullToothState)o));

        public void SetActionRigCorpse(Action_RigCorpse s) =>
            RegisterSystem(ref _actionRigCorpse, s, "action_rig_corpse",
                () => s.CaptureState(), o => s.RestoreState((RigCorpseState)o));

        public void SetActionRoutePower(Action_RoutePower s) =>
            RegisterSystem(ref _actionRoutePower, s, "action_route_power",
                () => s.CaptureState(), o => s.RestoreState((RoutePowerState)o));

        public void SetActionSabotage(Action_Sabotage s) =>
            RegisterSystem(ref _actionSabotage, s, "action_sabotage",
                () => s.CaptureState(), o => s.RestoreState((SabotageMissionState)o));

        public void SetActionScorchedEarth(Action_ScorchedEarth s) =>
            RegisterSystem(ref _actionScorchedEarth, s, "action_scorched_earth",
                () => s.CaptureState(), o => s.RestoreState((ScorchedEarthState)o));

        public void SetActionSealRoom(Action_SealRoom s) =>
            RegisterSystem(ref _actionSealRoom, s, "action_seal_room",
                () => s.CaptureState(), o => s.RestoreState((SealRoomState)o));

        public void SetActionSelfSurgery(Action_SelfSurgery s) =>
            RegisterSystem(ref _actionSelfSurgery, s, "self_surgery",
                () => s.CaptureState(), o => s.RestoreState((Action_SelfSurgeryState)o));

        public void SetActionSilentTakedown(Action_SilentTakedown s) =>
            RegisterSystem(ref _actionSilentTakedown, s, "action_silent_takedown",
                () => s.CaptureState(), o => s.RestoreState((Action_SilentTakedownSave)o));

        public void SetActionSiphonGas(Action_SiphonGas s) =>
            RegisterSystem(ref _actionSiphonGas, s, "action_siphon_gas",
                () => s.CaptureState(), o => s.RestoreState((SiphonGasState)o));

        public void SetActionStabilizeDNA(Action_StabilizeDNA s) =>
            RegisterSystem(ref _actionStabilizeDNA, s, "action_stabilize_dna",
                () => s.CaptureState(), o => s.RestoreState((StabilizeDNAState)o));

        public void SetActionStargazing(Action_Stargazing s) =>
            RegisterSystem(ref _actionStargazing, s, "action_stargazing",
                () => s.CaptureState(), o => s.RestoreState((Action_StargazingSave)o));

        public void SetActionWorshipIdol(Action_WorshipIdol s) =>
            RegisterSystem(ref _actionWorshipIdol, s, "action_worship_idol",
                () => s.CaptureState(), o => s.RestoreState((WorshipIdolState)o));

        public void SetAfflictionAdrenalineCrash(Affliction_AdrenalineCrash s) =>
            RegisterSystem(ref _afflictionAdrenalineCrash, s, "affliction_adrenaline_crash",
                () => s.CaptureState(), o => s.RestoreState((AdrenalineCrashState)o));

        public void SetAfflictionAmnesia(AmnesiaSystem s) =>
            RegisterSystem(ref _afflictionAmnesia, s, "affliction_amnesia",
                () => s.CaptureState(), o => s.RestoreState((AmnesiaSystemSave)o));

        public void SetAfflictionBrainwashed(Affliction_Brainwashed s) =>
            RegisterSystem(ref _afflictionBrainwashed, s, "affliction_brainwashed",
                () => s.CaptureState(), o => s.RestoreState((BrainwashedState)o));

        public void SetAfflictionBrittleBones(BrittleBonesSystem s) =>
            RegisterSystem(ref _afflictionBrittleBones, s, "affliction_brittle_bones",
                () => s.CaptureState(), o => s.RestoreState((BrittleBonesSystemSave)o));

        public void SetAfflictionCaveMadness(CaveMadnessSystem s) =>
            RegisterSystem(ref _afflictionCaveMadness, s, "affliction_cave_madness",
                () => s.CaptureState(), o => s.RestoreState((CaveMadnessState)o));

        public void SetAfflictionFeralRegression(FeralRegressionSystem s) =>
            RegisterSystem(ref _afflictionFeralRegression, s, "affliction_feral_regression",
                () => s.CaptureState(), o => s.RestoreState((FeralRegressionSystemSave)o));

        public void SetAfflictionImaginaryFriend(ImaginaryFriendSystem s) =>
            RegisterSystem(ref _afflictionImaginaryFriend, s, "affliction_imaginary_friend",
                () => s.CaptureState(), o => s.RestoreState((ImaginaryFriendSystemSave)o));

        public void SetAfflictionNerveDamage(Affliction_NerveDamage s) =>
            RegisterSystem(ref _afflictionNerveDamage, s, "affliction_nerve_damage",
                () => s.CaptureState(), o => s.RestoreState((Affliction_NerveDamageState)o));

        public void SetAfflictionOldAge(Affliction_OldAge s) =>
            RegisterSystem(ref _afflictionOldAge, s, "affliction_old_age",
                () => s.CaptureState(), o => s.RestoreState((OldAgeState)o));

        public void SetAfflictionPhantomLimb(Affliction_PhantomLimb s) =>
            RegisterSystem(ref _afflictionPhantomLimb, s, "affliction_phantom_limb",
                () => s.CaptureState(), o => s.RestoreState((PhantomLimbState)o));

        public void SetAfflictionRadHallucinations(Affliction_RadHallucinations s) =>
            RegisterSystem(ref _afflictionRadHallucinations, s, "affliction_rad_hallucinations",
                () => s.CaptureState(), o => s.RestoreState((RadHallucinationState)o));

        public void SetAfflictionRadiationBlindness(RadiationBlindnessSystem s) =>
            RegisterSystem(ref _afflictionRadiationBlindness, s, "affliction_radiation_blindness",
                () => s.CaptureState(), o => s.RestoreState((RadiationBlindnessSystemSave)o));

        public void SetAfflictionScurvyDegeneration(Affliction_ScurvyDegeneration s) =>
            RegisterSystem(ref _afflictionScurvyDegeneration, s, "affliction_scurvy_degeneration",
                () => s.CaptureState(), o => s.RestoreState((Affliction_ScurvyDegenerationState)o));

        public void SetAfflictionSporeLung(SporeLungSystem s) =>
            RegisterSystem(ref _afflictionSporeLung, s, "affliction_spore_lung",
                () => s.CaptureState(), o => s.RestoreState((SporeLungSystemSave)o));

        public void SetAfflictionSterile(Affliction_Sterile s) =>
            RegisterSystem(ref _afflictionSterile, s, "affliction_sterile",
                () => s.CaptureState(), o => s.RestoreState((SterileState)o));

        public void SetAfflictionSurvivorsGuilt(SurvivorsGuiltSystem s) =>
            RegisterSystem(ref _afflictionSurvivorsGuilt, s, "affliction_survivors_guilt",
                () => s.CaptureState(), o => s.RestoreState((SurvivorsGuiltSystemSave)o));

        public void SetAfflictionTBI(Affliction_TBI s) =>
            RegisterSystem(ref _afflictionTBI, s, "affliction_tbi",
                () => s.CaptureState(), o => s.RestoreState((TBIState)o));

        public void SetAfflictionThyroidCancer(Affliction_ThyroidCancer s) =>
            RegisterSystem(ref _afflictionThyroidCancer, s, "affliction_thyroid_cancer",
                () => s.CaptureState(), o => s.RestoreState((ThyroidCancerState)o));

        public void SetAfflictionTrenchFoot(TrenchFootSystem s) =>
            RegisterSystem(ref _afflictionTrenchFoot, s, "affliction_trench_foot",
                () => s.CaptureState(), o => s.RestoreState((TrenchFootSystemSave)o));

        public void SetAudioEventDeafening(AudioEvent_Deafening s) =>
            RegisterSystem(ref _audioEventDeafening, s, "audio_event_deafening",
                () => s.CaptureState(), o => s.RestoreState((DeafeningState)o));

        public void SetAudioEventHeartbeat(AudioEvent_Heartbeat s) =>
            RegisterSystem(ref _audioEventHeartbeat, s, "audio_event_heartbeat",
                () => s.CaptureState(), o => s.RestoreState((HeartbeatState)o));

        public void SetCombatBleedOut(Combat_BleedOut s) =>
            RegisterSystem(ref _combatBleedOut, s, "combat_bleed_out",
                () => s.CaptureState(), o => s.RestoreState((BleedOutState)o));

        public void SetCombatFlanking(Combat_Flanking s) =>
            RegisterSystem(ref _combatFlanking, s, "combat_flanking",
                () => s.CaptureState(), o => s.RestoreState((FlankingState)o));

        public void SetCombatSuppression(Combat_Suppression s) =>
            RegisterSystem(ref _combatSuppression, s, "combat_suppression",
                () => s.CaptureState(), o => s.RestoreState((SuppressionState)o));

        public void SetCombatStanceLastStand(CombatStance_LastStand s) =>
            RegisterSystem(ref _combatStanceLastStand, s, "combat_stance_last_stand",
                () => s.CaptureState(), o => s.RestoreState((LastStandState)o));

        public void SetCrisisFeralFlora(Crisis_FeralFlora s) =>
            RegisterSystem(ref _crisisFeralFlora, s, "crisis_feral_flora",
                () => s.CaptureState(), o => s.RestoreState((FeralFloraState)o));

        public void SetCrisisStructuralFailure(Crisis_StructuralFailure s) =>
            RegisterSystem(ref _crisisStructuralFailure, s, "crisis_structural_failure",
                () => s.CaptureState(), o => s.RestoreState((StructuralFailureState)o));

        public void SetDurabilitySuppressor(Durability_Suppressor s) =>
            RegisterSystem(ref _durabilitySuppressor, s, "durability_suppressor",
                () => s.CaptureState(), o => s.RestoreState((SuppressorSave)o));

        public void SetEndgameUltimatum(Endgame_Ultimatum s) =>
            RegisterSystem(ref _endgameUltimatum, s, "endgame_ultimatum",
                () => s.CaptureState(), o => s.RestoreState((EndgameUltimatumState)o));

        public void SetHazardCookOff(Hazard_CookOff s) =>
            RegisterSystem(ref _hazardCookOff, s, "hazard_cook_off",
                () => s.CaptureState(), o => s.RestoreState((CookOffState)o));

        public void SetHazardExplosiveCrafting(Hazard_ExplosiveCrafting s) =>
            RegisterSystem(ref _hazardExplosiveCrafting, s, "hazard_explosive_crafting",
                () => s.CaptureState(), o => s.RestoreState((ExplosiveCraftingState)o));

        public void SetHazardFriendlyFire(Hazard_FriendlyFire s) =>
            RegisterSystem(ref _hazardFriendlyFire, s, "hazard_friendly_fire",
                () => s.CaptureState(), o => s.RestoreState((FriendlyFireState)o));

        public void SetHazardMethane(MethaneSystem s) =>
            RegisterSystem(ref _hazardMethane, s, "hazard_methane",
                () => s.CaptureState(), o => s.RestoreState((MethaneState)o));

        public void SetHazardMimicCrate(Hazard_MimicCrate s) =>
            RegisterSystem(ref _hazardMimicCrate, s, "hazard_mimic_crate",
                () => s.CaptureState(), o => s.RestoreState((MimicCrateState)o));

        public void SetHazardSurgicalBotch(Hazard_SurgicalBotch s) =>
            RegisterSystem(ref _hazardSurgicalBotch, s, "hazard_surgical_botch",
                () => s.CaptureState(), o => s.RestoreState((SurgicalBotchState)o));

        public void SetHazardWeaponBurst(Hazard_WeaponBurst s) =>
            RegisterSystem(ref _hazardWeaponBurst, s, "hazard_weapon_burst",
                () => s.CaptureState(), o => s.RestoreState((WeaponBurstState)o));

        public void SetHiddenStatUnseen(HiddenStat_Unseen s) =>
            RegisterSystem(ref _hiddenStatUnseen, s, "hidden_stat_unseen",
                () => s.CaptureState(), o => s.RestoreState((UnseenState)o));

        public void SetItemAICoreData(Item_AICoreData s) =>
            RegisterSystem(ref _itemAICoreData, s, "item_ai_core_data",
                () => s.CaptureState(), o => s.RestoreState((AICoreDataState)o));

        public void SetItemAmmoTypes(Item_AmmoTypes s) =>
            RegisterSystem(ref _itemAmmoTypes, s, "item_ammo_types",
                () => s.CaptureState(), o => s.RestoreState((AmmoTypeState)o));

        public void SetItemAmmonia(Item_Ammonia s) =>
            RegisterSystem(ref _itemAmmonia, s, "item_ammonia",
                () => s.CaptureState(), o => s.RestoreState((AmmoniaState)o));

        public void SetItemAmphetamines(Item_Amphetamines s) =>
            RegisterSystem(ref _itemAmphetamines, s, "item_amphetamines",
                () => s.CaptureState(), o => s.RestoreState((AmphetaminesState)o));

        public void SetItemAshGhillie(Item_AshGhillie s) =>
            RegisterSystem(ref _itemAshGhillie, s, "item_ash_ghillie",
                () => s.CaptureState(), o => s.RestoreState((AshGhillieState)o));

        public void SetItemAutoDoc(Item_AutoDoc s) =>
            RegisterSystem(ref _itemAutoDoc, s, "item_auto_doc",
                () => s.CaptureState(), o => s.RestoreState((AutoDocState)o));

        public void SetItemBioPlastic(Item_BioPlastic s) =>
            RegisterSystem(ref _itemBioPlastic, s, "item_bio_plastic",
                () => s.CaptureState(), o => s.RestoreState((BioPlasticState)o));

        public void SetItemBloodBag(Item_BloodBag s) =>
            RegisterSystem(ref _itemBloodBag, s, "item_blood_bag",
                () => s.CaptureState(), o => s.RestoreState((BloodBagState)o));

        public void SetItemBoneSaw(Item_BoneSaw s) =>
            RegisterSystem(ref _itemBoneSaw, s, "bone_saw",
                () => s.CaptureState(), o => s.RestoreState((Item_BoneSawState)o));

        public void SetItemC4(Item_C4 s) =>
            RegisterSystem(ref _itemC4, s, "item_c4",
                () => s.CaptureState(), o => s.RestoreState((C4State)o));

        public void SetItemCaltrops(Item_Caltrops s) =>
            RegisterSystem(ref _itemCaltrops, s, "item_caltrops",
                () => s.CaptureState(), o => s.RestoreState((CaltropsState)o));

        public void SetItemCarrierBird(Item_CarrierBird s) =>
            RegisterSystem(ref _itemCarrierBird, s, "item_carrier_bird",
                () => s.CaptureState(), o => s.RestoreState((CarrierBirdState)o));

        public void SetItemChildsDrawing(Item_ChildsDrawing s) =>
            RegisterSystem(ref _itemChildsDrawing, s, "item_childs_drawing",
                () => s.CaptureState(), o => s.RestoreState((ChildsDrawingState)o));

        public void SetItemCigarettes(Item_Cigarettes s) =>
            RegisterSystem(ref _itemCigarettes, s, "item_cigarettes",
                () => s.CaptureState(), o => s.RestoreState((CigarettesState)o));

        public void SetItemClimbingGear(Item_ClimbingGear s) =>
            RegisterSystem(ref _itemClimbingGear, s, "item_climbing_gear",
                () => s.CaptureState(), o => s.RestoreState((ClimbingGearState)o));

        public void SetItemDecoy(Item_Decoy s) =>
            RegisterSystem(ref _itemDecoy, s, "item_decoy",
                () => s.CaptureState(), o => s.RestoreState((DecoyState)o));

        public void SetItemDogTags(Item_DogTags s) =>
            RegisterSystem(ref _itemDogTags, s, "item_dog_tags",
                () => s.CaptureState(), o => s.RestoreState((DogTagsState)o));

        public void SetItemEMPGrenade(Item_EMPGrenade s) =>
            RegisterSystem(ref _itemEMPGrenade, s, "item_emp_grenade",
                () => s.CaptureState(), o => s.RestoreState((EMPGrenadeState)o));

        public void SetItemEncryptedDrive(Item_EncryptedDrive s) =>
            RegisterSystem(ref _itemEncryptedDrive, s, "item_encrypted_drive",
                () => s.CaptureState(), o => s.RestoreState((EncryptedDriveState)o));

        public void SetItemEpiPen(Item_EpiPen s) =>
            RegisterSystem(ref _itemEpiPen, s, "item_epipen",
                () => s.CaptureState(), o => s.RestoreState((EpiPenState)o));

        public void SetItemExosuit(Item_Exosuit s) =>
            RegisterSystem(ref _itemExosuit, s, "item_exosuit",
                () => s.CaptureState(), o => s.RestoreState((ExosuitState)o));

        public void SetItemFaradayPack(Item_FaradayPack s) =>
            RegisterSystem(ref _itemFaradayPack, s, "item_faraday_pack",
                () => s.CaptureState(), o => s.RestoreState((FaradayPackState)o));

        public void SetItemForeignBook(Item_ForeignBook s) =>
            RegisterSystem(ref _itemForeignBook, s, "item_foreign_book",
                () => s.CaptureState(), o => s.RestoreState((ForeignBookState)o));

        public void SetItemGeigerCalibrator(Item_GeigerCalibrator s) =>
            RegisterSystem(ref _itemGeigerCalibrator, s, "item_geiger_calibrator",
                () => s.CaptureState(), o => s.RestoreState((GeigerCalibratorState)o));

        public void SetItemGlowingMushroom(GlowingMushroomSystem s) =>
            RegisterSystem(ref _itemGlowingMushroom, s, "item_glowing_mushroom",
                () => s.CaptureState(), o => s.RestoreState((GlowingMushroomState)o));

        public void SetItemGoldBars(Item_GoldBars s) =>
            RegisterSystem(ref _itemGoldBars, s, "item_gold_bars",
                () => s.CaptureState(), o => s.RestoreState((GoldBarsState)o));

        public void SetItemGuitar(Item_Guitar s) =>
            RegisterSystem(ref _itemGuitar, s, "item_guitar",
                () => s.CaptureState(), o => s.RestoreState((GuitarState)o));

        public void SetItemHeirloom(Item_Heirloom s) =>
            RegisterSystem(ref _itemHeirloom, s, "item_heirloom",
                () => s.CaptureState(), o => s.RestoreState((HeirloomState)o));

        public void SetItemIBeam(Item_IBeam s) =>
            RegisterSystem(ref _itemIBeam, s, "item_i_beam",
                () => s.CaptureState(), o => s.RestoreState((IBeamState)o));

        public void SetItemImpureIodine(Item_ImpureIodine s) =>
            RegisterSystem(ref _itemImpureIodine, s, "item_impure_iodine",
                () => s.CaptureState(), o => s.RestoreState((ImpureIodineState)o));

        public void SetItemJuggernautArmor(Item_JuggernautArmor s) =>
            RegisterSystem(ref _itemJuggernautArmor, s, "item_juggernaut_armor",
                () => s.CaptureState(), o => s.RestoreState((JuggernautArmorState)o));

        public void SetItemKevlarVest(Item_KevlarVest s) =>
            RegisterSystem(ref _itemKevlarVest, s, "item_kevlar_vest",
                () => s.CaptureState(), o => s.RestoreState((KevlarVestState)o));

        public void SetItemKeycards(Item_Keycards s) =>
            RegisterSystem(ref _itemKeycards, s, "item_keycards",
                () => s.CaptureState(), o => s.RestoreState((KeycardState)o));

        public void SetItemLandmine(Item_Landmine s) =>
            RegisterSystem(ref _itemLandmine, s, "item_landmine",
                () => s.CaptureState(), o => s.RestoreState((LandmineState)o));

        public void SetItemLeadApron(Item_LeadApron s) =>
            RegisterSystem(ref _itemLeadApron, s, "item_lead_apron",
                () => s.CaptureState(), o => s.RestoreState((LeadApronState)o));

        public void SetItemLiquidStitches(Item_LiquidStitches s) =>
            RegisterSystem(ref _itemLiquidStitches, s, "item_liquid_stitches",
                () => s.CaptureState(), o => s.RestoreState((LiquidStitchesState)o));

        public void SetItemMaggots(Item_Maggots s) =>
            RegisterSystem(ref _itemMaggots, s, "maggots",
                () => s.CaptureState(), o => s.RestoreState((Item_MaggotsState)o));

        public void SetItemMilGasMask(Item_MilGasMask s) =>
            RegisterSystem(ref _itemMilGasMask, s, "item_mil_gas_mask",
                () => s.CaptureState(), o => s.RestoreState((MilGasMaskState)o));

        public void SetItemMutantGland(Item_MutantGland s) =>
            RegisterSystem(ref _itemMutantGland, s, "item_mutant_gland",
                () => s.CaptureState(), o => s.RestoreState((MutantGlandState)o));

        public void SetItemNanites(Item_Nanites s) =>
            RegisterSystem(ref _itemNanites, s, "item_nanites",
                () => s.CaptureState(), o => s.RestoreState((NanitesState)o));

        public void SetItemNightVision(Item_NightVision s) =>
            RegisterSystem(ref _itemNightVision, s, "item_night_vision",
                () => s.CaptureState(), o => s.RestoreState((NightVisionState)o));

        public void SetItemPackMule(Item_PackMule s) =>
            RegisterSystem(ref _itemPackMule, s, "item_pack_mule",
                () => s.CaptureState(), o => s.RestoreState((PackMuleState)o));

        public void SetItemPasswordNote(Item_PasswordNote s) =>
            RegisterSystem(ref _itemPasswordNote, s, "item_password_note",
                () => s.CaptureState(), o => s.RestoreState((PasswordNoteState)o));

        public void SetItemPhotoAlbum(Item_PhotoAlbum s) =>
            RegisterSystem(ref _itemPhotoAlbum, s, "item_photo_album",
                () => s.CaptureState(), o => s.RestoreState((PhotoAlbumState)o));

        public void SetItemPotassiumIodide(Item_PotassiumIodide s) =>
            RegisterSystem(ref _itemPotassiumIodide, s, "item_potassium_iodide",
                () => s.CaptureState(), o => s.RestoreState((PotassiumIodideState)o));

        public void SetItemPresidentialSeal(Item_PresidentialSeal s) =>
            RegisterSystem(ref _itemPresidentialSeal, s, "item_presidential_seal",
                () => s.CaptureState(), o => s.RestoreState((PresidentialSealState)o));

        public void SetItemPrussianBlue(Item_PrussianBlue s) =>
            RegisterSystem(ref _itemPrussianBlue, s, "item_prussian_blue",
                () => s.CaptureState(), o => s.RestoreState((PrussianBlueState)o));

        public void SetItemRTGBattery(Item_RTGBattery s) =>
            RegisterSystem(ref _itemRTGBattery, s, "item_rtg_battery",
                () => s.CaptureState(), o => s.RestoreState((RTGBatteryState)o));

        public void SetItemSeedLedger(Item_SeedLedger s) =>
            RegisterSystem(ref _itemSeedLedger, s, "item_seed_ledger",
                () => s.CaptureState(), o => s.RestoreState((SeedLedgerState)o));

        public void SetItemShockCollar(Item_ShockCollar s) =>
            RegisterSystem(ref _itemShockCollar, s, "item_shock_collar",
                () => s.CaptureState(), o => s.RestoreState((ShockCollarState)o));

        public void SetItemSnowshoes(Item_Snowshoes s) =>
            RegisterSystem(ref _itemSnowshoes, s, "item_snowshoes",
                () => s.CaptureState(), o => s.RestoreState((SnowshoesState)o));

        public void SetItemSurgicalTubing(Item_SurgicalTubing s) =>
            RegisterSystem(ref _itemSurgicalTubing, s, "item_surgical_tubing",
                () => s.CaptureState(), o => s.RestoreState((SurgicalTubingState)o));

        public void SetItemTearGas(Item_TearGas s) =>
            RegisterSystem(ref _itemTearGas, s, "item_tear_gas",
                () => s.CaptureState(), o => s.RestoreState((TearGasState)o));

        public void SetItemTeddyBear(Item_TeddyBear s) =>
            RegisterSystem(ref _itemTeddyBear, s, "item_teddy_bear",
                () => s.CaptureState(), o => s.RestoreState((TeddyBearState)o));

        public void SetItemTrashHazmat(Item_TrashHazmat s) =>
            RegisterSystem(ref _itemTrashHazmat, s, "item_trash_hazmat",
                () => s.CaptureState(), o => s.RestoreState((TrashHazmatState)o));

        public void SetItemUndeliveredMail(Item_UndeliveredMail s) =>
            RegisterSystem(ref _itemUndeliveredMail, s, "item_undelivered_mail",
                () => s.CaptureState(), o => s.RestoreState((UndeliveredMailState)o));

        public void SetItemVacuumTubes(Item_VacuumTubes s) =>
            RegisterSystem(ref _itemVacuumTubes, s, "item_vacuum_tubes",
                () => s.CaptureState(), o => s.RestoreState((VacuumTubesState)o));

        public void SetItemVinylCollection(Item_VinylCollection s) =>
            RegisterSystem(ref _itemVinylCollection, s, "item_vinyl_collection",
                () => s.CaptureState(), o => s.RestoreState((VinylCollectionState)o));

        public void SetItemVitamins(Item_Vitamins s) =>
            RegisterSystem(ref _itemVitamins, s, "item_vitamins",
                () => s.CaptureState(), o => s.RestoreState((VitaminsState)o));

        public void SetItemWalkieTalkie(Item_WalkieTalkie s) =>
            RegisterSystem(ref _itemWalkieTalkie, s, "item_walkie_talkie",
                () => s.CaptureState(), o => s.RestoreState((WalkieTalkieState)o));

        public void SetItemWastelandSoap(Item_WastelandSoap s) =>
            RegisterSystem(ref _itemWastelandSoap, s, "item_wasteland_soap",
                () => s.CaptureState(), o => s.RestoreState((WastelandSoapState)o));

        public void SetItemWaterTabs(Item_WaterTabs s) =>
            RegisterSystem(ref _itemWaterTabs, s, "item_water_tabs",
                () => s.CaptureState(), o => s.RestoreState((WaterTabsState)o));

        public void SetItemWeldingGoggles(Item_WeldingGoggles s) =>
            RegisterSystem(ref _itemWeldingGoggles, s, "item_welding_goggles",
                () => s.CaptureState(), o => s.RestoreState((WeldingGogglesState)o));

        public void SetItemWristDosimeter(Item_WristDosimeter s) =>
            RegisterSystem(ref _itemWristDosimeter, s, "item_wrist_dosimeter",
                () => s.CaptureState(), o => s.RestoreState((WristDosimeterState)o));

        public void SetLocationArcade(Location_Arcade s) =>
            RegisterSystem(ref _locationArcade, s, "location_arcade",
                () => s.CaptureState(), o => s.RestoreState((ArcadeState)o));

        public void SetLocationSlaveMarket(Location_SlaveMarket s) =>
            RegisterSystem(ref _locationSlaveMarket, s, "location_slave_market",
                () => s.CaptureState(), o => s.RestoreState((SlaveMarketState)o));

        public void SetLocationStrandedYacht(Location_StrandedYacht s) =>
            RegisterSystem(ref _locationStrandedYacht, s, "location_stranded_yacht",
                () => s.CaptureState(), o => s.RestoreState((StrandedYachtState)o));

        public void SetMapAquifer(AquiferSystem s) =>
            RegisterSystem(ref _mapAquifer, s, "map_aquifer",
                () => s.CaptureState(), o => s.RestoreState((AquiferState)o));

        public void SetAshDriftSystem(AshDriftSystem s) =>
            RegisterSystem(ref _ashDriftSystem, s, "ash_drift_system",
                () => s.CaptureState(), o => s.RestoreState((AshDriftSystemSave)o));

        public void SetBurnWardSystem(BurnWardSystem s) =>
            RegisterSystem(ref _burnWardSystem, s, "burn_ward_system",
                () => s.CaptureState(), o => s.RestoreState((BurnWardSystemSave)o));

        public void SetCognitiveDecaySystem(CognitiveDecaySystem s) =>
            RegisterSystem(ref _cognitiveDecaySystem, s, "cognitive_decay_system",
                () => s.CaptureState(), o => s.RestoreState((CognitiveDecaySystemSave)o));

        public void SetLightningStrikesSystem(LightningStrikesSystem s) =>
            RegisterSystem(ref _lightningStrikesSystem, s, "lightning_strikes_system",
                () => s.CaptureState(), o => s.RestoreState((LightningStrikeState)o));

        public void SetLocationStateRuinSystem(LocationStateRuinSystem s) =>
            RegisterSystem(ref _locationStateRuinSystem, s, "location_state_ruin_system",
                () => s.CaptureState(), o => s.RestoreState((LocationStateRuinSystemSave)o));

        public void SetMobileCampSystem(MobileCampSystem s) =>
            RegisterSystem(ref _mobileCampSystem, s, "mobile_camp_system",
                () => s.CaptureState(), o => s.RestoreState((MobileCampState)o));

        public void SetMoralDilemmaSystem(MoralDilemmaSystem s) =>
            RegisterSystem(ref _moralDilemmaSystem, s, "moral_dilemma_system",
                () => s.CaptureState(), o => s.RestoreState((MoralDilemmaSystemSave)o));

        public void SetNeedleSterilizationSystem(NeedleSterilizationSystem s) =>
            RegisterSystem(ref _needleSterilizationSystem, s, "needle_sterilization_system",
                () => s.CaptureState(), o => s.RestoreState((NeedleSterilizationSystemSave)o));

        public void SetNightScavengeSystem(NightScavengeSystem s) =>
            RegisterSystem(ref _nightScavengeSystem, s, "night_scavenge_system",
                () => s.CaptureState(), o => s.RestoreState((NightScavengeSystemSave)o));

        public void SetProstheticCraftingSystem(ProstheticCraftingSystem s) =>
            RegisterSystem(ref _prostheticCraftingSystem, s, "prosthetic_crafting_system",
                () => s.CaptureState(), o => s.RestoreState((ProstheticCraftingSystemSave)o));

        public void SetSeismicVentsSystem(SeismicVentsSystem s) =>
            RegisterSystem(ref _seismicVentsSystem, s, "seismic_vents_system",
                () => s.CaptureState(), o => s.RestoreState((SeismicVentsSystemSave)o));

        public void SetSevereFrostbiteSystem(SevereFrostbiteSystem s) =>
            RegisterSystem(ref _severeFrostbiteSystem, s, "severe_frostbite_system",
                () => s.CaptureState(), o => s.RestoreState((SevereFrostbiteSystemSave)o));

        public void SetTetanusAfflictionSystem(TetanusAfflictionSystem s) =>
            RegisterSystem(ref _tetanusAfflictionSystem, s, "tetanus_affliction_system",
                () => s.CaptureState(), o => s.RestoreState((TetanusAfflictionSystemSave)o));

        /// <summary>
        /// Register the live game clock. This must be the same instance Update()
        /// advances — it was previously pointed at a second, never-ticked TimeSystem,
        /// so the save file recorded a clock that had never left day 1.
        /// </summary>
        public void SetTimeSystem(TimeSystem s) =>
            RegisterSystem(ref _timeSystem, s, "time_system",
                () => s.CaptureState(), o => s.RestoreState((TimeSystemSave)o));

        public void SetToothDecaySystem(ToothDecaySystem s) =>
            RegisterSystem(ref _toothDecaySystem, s, "tooth_decay_system",
                () => s.CaptureState(), o => s.RestoreState((ToothDecaySystemSave)o));

        public void SetVehicleStrandingSystem(VehicleStrandingSystem s) =>
            RegisterSystem(ref _vehicleStrandingSystem, s, "vehicle_stranding_system",
                () => s.CaptureState(), o => s.RestoreState((VehicleStrandingSystemSave)o));

        public void SetVehicleSystem(VehicleSystem s) =>
            RegisterSystem(ref _vehicleSystem, s, "vehicle_system",
                () => s.CaptureState(), o => s.RestoreState((VehicleSystemSave)o));

        public void SetVisionLossSystem(VisionLossSystem s) =>
            RegisterSystem(ref _visionLossSystem, s, "vision_loss_system",
                () => s.CaptureState(), o => s.RestoreState((VisionLossSystemSave)o));

        public void SetVisitorRNGSystem(VisitorRNGSystem s) =>
            RegisterSystem(ref _visitorRNGSystem, s, "visitor_rngsystem",
                () => s.CaptureState(), o => s.RestoreState((VisitorRNGSystemSave)o));

        public void SetNPCAddictsPassive(NPC_AddictsPassive s) =>
            RegisterSystem(ref _nPCAddictsPassive, s, "npc_addicts_passive",
                () => s.CaptureState(), o => s.RestoreState((AddictsPassiveState)o));

        public void SetNPCAggroScavengers(NPC_AggroScavengers s) =>
            RegisterSystem(ref _nPCAggroScavengers, s, "npc_aggro_scavengers",
                () => s.CaptureState(), o => s.RestoreState((AggroScavengersState)o));

        public void SetNPCAggroTrader(NPC_AggroTrader s) =>
            RegisterSystem(ref _nPCAggroTrader, s, "npc_aggro_trader",
                () => s.CaptureState(), o => s.RestoreState((AggroTraderState)o));

        public void SetNPCBandits(NPC_Bandits s) =>
            RegisterSystem(ref _nPCBandits, s, "npc_bandits",
                () => s.CaptureState(), o => s.RestoreState((BanditsState)o));

        public void SetNPCBlackOps(NPC_BlackOps s) =>
            RegisterSystem(ref _nPCBlackOps, s, "npc_black_ops",
                () => s.CaptureState(), o => s.RestoreState((BlackOpsState)o));

        public void SetNPCBroker(NPC_Broker s) =>
            RegisterSystem(ref _nPCBroker, s, "npc_broker",
                () => s.CaptureState(), o => s.RestoreState((BrokerState)o));

        public void SetNPCCannibals(NPC_Cannibals s) =>
            RegisterSystem(ref _nPCCannibals, s, "npc_cannibals",
                () => s.CaptureState(), o => s.RestoreState((CannibalsState)o));

        public void SetNPCChemScientists(NPC_ChemScientists s) =>
            RegisterSystem(ref _nPCChemScientists, s, "npc_chem_scientists",
                () => s.CaptureState(), o => s.RestoreState((ChemScientistsState)o));

        public void SetNPCCityResidents(NPC_CityResidents s) =>
            RegisterSystem(ref _nPCCityResidents, s, "npc_city_residents",
                () => s.CaptureState(), o => s.RestoreState((CityResidentsState)o));

        public void SetNPCCollaborators(NPC_Collaborators s) =>
            RegisterSystem(ref _nPCCollaborators, s, "npc_collaborators",
                () => s.CaptureState(), o => s.RestoreState((CollaboratorsState)o));

        public void SetNPCConscripts(NPC_Conscripts s) =>
            RegisterSystem(ref _nPCConscripts, s, "npc_conscripts",
                () => s.CaptureState(), o => s.RestoreState((ConscriptsState)o));

        public void SetNPCDesperateFamily(NPC_DesperateFamily s) =>
            RegisterSystem(ref _nPCDesperateFamily, s, "npc_desperate_family",
                () => s.CaptureState(), o => s.RestoreState((DesperateFamilyState)o));

        public void SetNPCDrunksAggro(NPC_DrunksAggro s) =>
            RegisterSystem(ref _nPCDrunksAggro, s, "npc_drunks_aggro",
                () => s.CaptureState(), o => s.RestoreState((DrunksAggroState)o));

        public void SetNPCHomeless(NPC_Homeless s) =>
            RegisterSystem(ref _nPCHomeless, s, "npc_homeless",
                () => s.CaptureState(), o => s.RestoreState((HomelessEncampmentState)o));

        public void SetNPCLonePsychopath(NPC_LonePsychopath s) =>
            RegisterSystem(ref _nPCLonePsychopath, s, "npc_lone_psychopath",
                () => s.CaptureState(), o => s.RestoreState((LonePsychopathState)o));

        public void SetNPCLooters(NPC_Looters s) =>
            RegisterSystem(ref _nPCLooters, s, "npc_looters",
                () => s.CaptureState(), o => s.RestoreState((LootersState)o));

        public void SetNPCMercenaries(NPC_Mercenaries s) =>
            RegisterSystem(ref _nPCMercenaries, s, "npc_mercenaries",
                () => s.CaptureState(), o => s.RestoreState((MercenariesState)o));

        public void SetNPCMilitaryPatrol(NPC_MilitaryPatrol s) =>
            RegisterSystem(ref _nPCMilitaryPatrol, s, "npc_military_patrol",
                () => s.CaptureState(), o => s.RestoreState((MilitaryPatrolState)o));

        public void SetNPCPassiveScavengers(NPC_PassiveScavengers s) =>
            RegisterSystem(ref _nPCPassiveScavengers, s, "npc_passive_scavengers",
                () => s.CaptureState(), o => s.RestoreState((PassiveScavengersState)o));

        public void SetNPCPassiveTrader(NPC_PassiveTrader s) =>
            RegisterSystem(ref _nPCPassiveTrader, s, "npc_passive_trader",
                () => s.CaptureState(), o => s.RestoreState((PassiveTraderState)o));

        public void SetNPCPsychopathPair(NPC_PsychopathPair s) =>
            RegisterSystem(ref _nPCPsychopathPair, s, "npc_psychopath_pair",
                () => s.CaptureState(), o => s.RestoreState((PsychopathPairState)o));

        public void SetNPCRebelMilitia(NPC_RebelMilitia s) =>
            RegisterSystem(ref _nPCRebelMilitia, s, "npc_rebel_militia",
                () => s.CaptureState(), o => s.RestoreState((RebelMilitiaState)o));

        public void SetNPCRebelModerates(NPC_RebelModerates s) =>
            RegisterSystem(ref _nPCRebelModerates, s, "npc_rebel_moderates",
                () => s.CaptureState(), o => s.RestoreState((RebelModeratesState)o));

        public void SetNPCRebelSnipers(NPC_RebelSnipers s) =>
            RegisterSystem(ref _nPCRebelSnipers, s, "npc_rebel_snipers",
                () => s.CaptureState(), o => s.RestoreState((RebelSnipersState)o));

        public void SetNPCRebelZealots(NPC_RebelZealots s) =>
            RegisterSystem(ref _nPCRebelZealots, s, "npc_rebel_zealots",
                () => s.CaptureState(), o => s.RestoreState((RebelZealotsState)o));

        public void SetNPCSlavers(NPC_Slavers s) =>
            RegisterSystem(ref _nPCSlavers, s, "npc_slavers",
                () => s.CaptureState(), o => s.RestoreState((SlaversState)o));

        public void SetNPCSpecOps(NPC_SpecOps s) =>
            RegisterSystem(ref _nPCSpecOps, s, "npc_spec_ops",
                () => s.CaptureState(), o => s.RestoreState((SpecOpsState)o));

        public void SetNPCSurvivalists(NPC_Survivalists s) =>
            RegisterSystem(ref _nPCSurvivalists, s, "npc_survivalists",
                () => s.CaptureState(), o => s.RestoreState((SurvivalistsState)o));

        public void SetNPCTaxCollector(NPC_TaxCollector s) =>
            RegisterSystem(ref _nPCTaxCollector, s, "npc_tax_collector",
                () => s.CaptureState(), o => s.RestoreState((TaxCollectorState)o));

        public void SetNPCTerrorists(NPC_Terrorists s) =>
            RegisterSystem(ref _nPCTerrorists, s, "npc_terrorists",
                () => s.CaptureState(), o => s.RestoreState((TerroristState)o));

        public void SetNPCTheNegotiator(NPC_TheNegotiator s) =>
            RegisterSystem(ref _nPCTheNegotiator, s, "npc_the_negotiator",
                () => s.CaptureState(), o => s.RestoreState((NegotiatorState)o));

        public void SetNPCTheOld(NPC_TheOld s) =>
            RegisterSystem(ref _nPCTheOld, s, "npc_the_old",
                () => s.CaptureState(), o => s.RestoreState((TheOldState)o));

        public void SetNPCTheParents(NPC_TheParents s) =>
            RegisterSystem(ref _nPCTheParents, s, "npc_the_parents",
                () => s.CaptureState(), o => s.RestoreState((TheParentsState)o));

        public void SetNPCTravelingCouple(NPC_TravelingCouple s) =>
            RegisterSystem(ref _nPCTravelingCouple, s, "npc_traveling_couple",
                () => s.CaptureState(), o => s.RestoreState((TravelingCoupleState)o));

        public void SetNodeAutomatedArmory(Node_AutomatedArmory s) =>
            RegisterSystem(ref _nodeAutomatedArmory, s, "node_automated_armory",
                () => s.CaptureState(), o => s.RestoreState((AutomatedArmoryState)o));

        public void SetNodeGhostShip(Node_GhostShip s) =>
            RegisterSystem(ref _nodeGhostShip, s, "node_ghost_ship",
                () => s.CaptureState(), o => s.RestoreState((GhostShipState)o));

        public void SetNodeMutantHive(Node_MutantHive s) =>
            RegisterSystem(ref _nodeMutantHive, s, "node_mutant_hive",
                () => s.CaptureState(), o => s.RestoreState((MutantHiveState)o));

        public void SetNodePlayerBank(Node_PlayerBank s) =>
            RegisterSystem(ref _nodePlayerBank, s, "node_player_bank",
                () => s.CaptureState(), o => s.RestoreState((PlayerBankState)o));

        public void SetNodeSector7G(Node_Sector7G s) =>
            RegisterSystem(ref _nodeSector7G, s, "node_sector_7g",
                () => s.CaptureState(), o => s.RestoreState((Sector7GState)o));

        public void SetNodeSporeHive(Node_SporeHive s) =>
            RegisterSystem(ref _nodeSporeHive, s, "node_spore_hive",
                () => s.CaptureState(), o => s.RestoreState((SporeHiveState)o));

        public void SetPetFeralCat(Pet_FeralCat s) =>
            RegisterSystem(ref _petFeralCat, s, "pet_feral_cat",
                () => s.CaptureState(), o => s.RestoreState((FeralCatState)o));

        public void SetProjectBioReactor(Project_BioReactor s) =>
            RegisterSystem(ref _projectBioReactor, s, "project_bio_reactor",
                () => s.CaptureState(), o => s.RestoreState((BioReactorState)o));

        public void SetProjectDeepWell(Project_DeepWell s) =>
            RegisterSystem(ref _projectDeepWell, s, "project_deep_well",
                () => s.CaptureState(), o => s.RestoreState((DeepWellState)o));

        public void SetProjectElevator(Project_Elevator s) =>
            RegisterSystem(ref _projectElevator, s, "project_elevator",
                () => s.CaptureState(), o => s.RestoreState((ElevatorState)o));

        public void SetProjectMinecart(Project_Minecart s) =>
            RegisterSystem(ref _projectMinecart, s, "project_minecart",
                () => s.CaptureState(), o => s.RestoreState((MinecartState)o));

        public void SetProjectRadioArray(Project_RadioArray s) =>
            RegisterSystem(ref _projectRadioArray, s, "project_radio_array",
                () => s.CaptureState(), o => s.RestoreState((RadioArrayState)o));

        public void SetProjectSurfaceDome(Project_SurfaceDome s) =>
            RegisterSystem(ref _projectSurfaceDome, s, "project_surface_dome",
                () => s.CaptureState(), o => s.RestoreState((SurfaceDomeState)o));

        public void SetShelterEventCaravanAmbush(ShelterEvent_CaravanAmbush s) =>
            RegisterSystem(ref _shelterEventCaravanAmbush, s, "shelter_event_caravan_ambush",
                () => s.CaptureState(), o => s.RestoreState((CaravanAmbushState)o));

        public void SetShelterEventFalseCure(ShelterEvent_FalseCure s) =>
            RegisterSystem(ref _shelterEventFalseCure, s, "event_false_cure",
                () => s.CaptureState(), o => s.RestoreState((FalseCureState)o));

        public void SetShelterEventRansom(ShelterEvent_Ransom s) =>
            RegisterSystem(ref _shelterEventRansom, s, "shelter_event_ransom",
                () => s.CaptureState(), o => s.RestoreState((RansomEventState)o));

        public void SetShelterEventRefugees(ShelterEvent_Refugees s) =>
            RegisterSystem(ref _shelterEventRefugees, s, "shelter_event_refugees",
                () => s.CaptureState(), o => s.RestoreState((RefugeeWaveState)o));

        public void SetShelterEventTheMirror(ShelterEvent_TheMirror s) =>
            RegisterSystem(ref _shelterEventTheMirror, s, "shelter_event_the_mirror",
                () => s.CaptureState(), o => s.RestoreState((MirrorEventSave)o));

        public void SetShelterEventTribute(ShelterEvent_Tribute s) =>
            RegisterSystem(ref _shelterEventTribute, s, "shelter_event_tribute",
                () => s.CaptureState(), o => s.RestoreState((TributeSystemState)o));

        public void SetSkirmishBandit_vs_Terror(Skirmish_Bandit_vs_Terror s) =>
            RegisterSystem(ref _skirmishBandit_vs_Terror, s, "skirmish_bandit_vs_terror",
                () => s.CaptureState(), o => s.RestoreState((BanditVsTerrorState)o));

        public void SetSkirmishMil_vs_Rebel(Skirmish_Mil_vs_Rebel s) =>
            RegisterSystem(ref _skirmishMil_vs_Rebel, s, "skirmish_mil_vs_rebel",
                () => s.CaptureState(), o => s.RestoreState((MilVsRebelState)o));

        public void SetSkirmishMil_vs_Terror(Skirmish_Mil_vs_Terror s) =>
            RegisterSystem(ref _skirmishMil_vs_Terror, s, "skirmish_mil_vs_terror",
                () => s.CaptureState(), o => s.RestoreState((MilVsTerrorState)o));

        public void SetSkirmishRebel_vs_Bandit(Skirmish_Rebel_vs_Bandit s) =>
            RegisterSystem(ref _skirmishRebel_vs_Bandit, s, "skirmish_rebel_vs_bandit",
                () => s.CaptureState(), o => s.RestoreState((RebelVsBanditState)o));

        public void SetSkirmishRebel_vs_Terror(Skirmish_Rebel_vs_Terror s) =>
            RegisterSystem(ref _skirmishRebel_vs_Terror, s, "skirmish_rebel_vs_terror",
                () => s.CaptureState(), o => s.RestoreState((RebelVsTerrorState)o));

        public void SetTraderPlagueConvoy(Trader_PlagueConvoy s) =>
            RegisterSystem(ref _traderPlagueConvoy, s, "trader_plague_convoy",
                () => s.CaptureState(), o => s.RestoreState((PlagueConvoyState)o));

        public void SetTraitAnthropophobia(Trait_Anthropophobia s) =>
            RegisterSystem(ref _traitAnthropophobia, s, "trait_anthropophobia",
                () => s.CaptureState(), o => s.RestoreState((Trait_AnthropophobiaSave)o));

        public void SetTraitClairvoyant(ClairvoyantSystem s) =>
            RegisterSystem(ref _traitClairvoyant, s, "trait_clairvoyant",
                () => s.CaptureState(), o => s.RestoreState((ClairvoyantSystemSave)o));

        public void SetTraitGenerationalTrauma(Trait_GenerationalTrauma s) =>
            RegisterSystem(ref _traitGenerationalTrauma, s, "trait_generational_trauma",
                () => s.CaptureState(), o => s.RestoreState((GenerationalTraumaState)o));

        public void SetTraitInheritedGenetics(Trait_InheritedGenetics s) =>
            RegisterSystem(ref _traitInheritedGenetics, s, "trait_inherited_genetics",
                () => s.CaptureState(), o => s.RestoreState((InheritedGeneticsState)o));

        public void SetTraitMatriarch(Trait_Matriarch s) =>
            RegisterSystem(ref _traitMatriarch, s, "trait_matriarch",
                () => s.CaptureState(), o => s.RestoreState((MatriarchState)o));

        public void SetTraitPTSD(Trait_PTSD s) =>
            RegisterSystem(ref _traitPTSD, s, "trait_ptsd",
                () => s.CaptureState(), o => s.RestoreState((PTSDState)o));

        public void SetUIEventBlurredVision(UIEvent_BlurredVision s) =>
            RegisterSystem(ref _uIEventBlurredVision, s, "ui_event_blurred_vision",
                () => s.CaptureState(), o => s.RestoreState((BlurredVisionState)o));

        public void SetUIEventCorruptionScare(UIEvent_CorruptionScare s) =>
            RegisterSystem(ref _uIEventCorruptionScare, s, "ui_event_corruption_scare",
                () => s.CaptureState(), o => s.RestoreState((CorruptionScareState)o));

        public void SetUIEventFalseInventory(UIEvent_FalseInventory s) =>
            RegisterSystem(ref _uIEventFalseInventory, s, "ui_event_false_inventory",
                () => s.CaptureState(), o => s.RestoreState((FalseInventoryState)o));

        public void SetUIEventGhostRadio(UIEvent_GhostRadio s) =>
            RegisterSystem(ref _uIEventGhostRadio, s, "ui_event_ghost_radio",
                () => s.CaptureState(), o => s.RestoreState((GhostRadioState)o));

        public void SetUIEventHacking(UIEvent_Hacking s) =>
            RegisterSystem(ref _uIEventHacking, s, "ui_event_hacking",
                () => s.CaptureState(), o => s.RestoreState((HackingState)o));

        public void SetUIEventLowPower(UIEvent_LowPower s) =>
            RegisterSystem(ref _uIEventLowPower, s, "ui_event_low_power",
                () => s.CaptureState(), o => s.RestoreState((LowPowerUIState)o));

        public void SetUIEventMapRot(UIEvent_MapRot s) =>
            RegisterSystem(ref _uIEventMapRot, s, "ui_event_map_rot",
                () => s.CaptureState(), o => s.RestoreState((MapRotSaveData)o));

        public void SetUIEventPhantomBlip(PhantomBlipSystem s) =>
            RegisterSystem(ref _uIEventPhantomBlip, s, "ui_event_phantom_blip",
                () => s.CaptureState(), o => s.RestoreState((PhantomBlipState)o));

        public void SetVehicleArmoredTruck(Vehicle_ArmoredTruck s) =>
            RegisterSystem(ref _vehicleArmoredTruck, s, "vehicle_armored_truck",
                () => s.CaptureState(), o => s.RestoreState((ArmoredTruckState)o));

        public void SetVehicleMotorcycle(Vehicle_Motorcycle s) =>
            RegisterSystem(ref _vehicleMotorcycle, s, "vehicle_motorcycle",
                () => s.CaptureState(), o => s.RestoreState((MotorcycleState)o));

        public void SetVehicleRowboat(Vehicle_Rowboat s) =>
            RegisterSystem(ref _vehicleRowboat, s, "vehicle_rowboat",
                () => s.CaptureState(), o => s.RestoreState((RowboatState)o));

        public void SetVisitorAbandonedState(Visitor_AbandonedState s) =>
            RegisterSystem(ref _visitorAbandonedState, s, "visitor_abandoned",
                () => s.CaptureState(), o => s.RestoreState((Visitor_AbandonedStateSave)o));

        public void SetVisitorChurchHostile(Visitor_ChurchHostile s) =>
            RegisterSystem(ref _visitorChurchHostile, s, "visitor_church_hostile",
                () => s.CaptureState(), o => s.RestoreState((ChurchHostileState)o));

        public void SetVisitorChurchSanctuary(Visitor_ChurchSanctuary s) =>
            RegisterSystem(ref _visitorChurchSanctuary, s, "visitor_church_sanctuary",
                () => s.CaptureState(), o => s.RestoreState((ChurchSanctuaryState)o));

        public void SetVisitorExplodedState(Visitor_ExplodedState s) =>
            RegisterSystem(ref _visitorExplodedState, s, "visitor_exploded_state",
                () => s.CaptureState(), o => s.RestoreState((Visitor_ExplodedStateSave)o));

        public void SetVisitorFleeingHorde(Visitor_FleeingHorde s) =>
            RegisterSystem(ref _visitorFleeingHorde, s, "visitor_fleeing_horde",
                () => s.CaptureState(), o => s.RestoreState((FleeingHordeState)o));

        public void SetVisitorHospitalPatients(Visitor_HospitalPatients s) =>
            RegisterSystem(ref _visitorHospitalPatients, s, "visitor_hospital_patients",
                () => s.CaptureState(), o => s.RestoreState((HospitalPatientsState)o));

        public void SetVisitorHospitalStaff(Visitor_HospitalStaff s) =>
            RegisterSystem(ref _visitorHospitalStaff, s, "visitor_hospital_staff",
                () => s.CaptureState(), o => s.RestoreState((HospitalStaffState)o));

        public void SetVisitorMilTrainingYard(Visitor_MilTrainingYard s) =>
            RegisterSystem(ref _visitorMilTrainingYard, s, "visitor_mil_training_yard",
                () => s.CaptureState(), o => s.RestoreState((MilTrainingYardState)o));

        public void SetVisitorQuestFaction(Visitor_QuestFaction s) =>
            RegisterSystem(ref _visitorQuestFaction, s, "visitor_quest_faction",
                () => s.CaptureState(), o => s.RestoreState((QuestFactionState)o));

        public void SetVisitorRebelTrainingYard(Visitor_RebelTrainingYard s) =>
            RegisterSystem(ref _visitorRebelTrainingYard, s, "visitor_rebel_training_yard",
                () => s.CaptureState(), o => s.RestoreState((RebelTrainingYardState)o));

        public void SetWeaponChainsaw(Weapon_Chainsaw s) =>
            RegisterSystem(ref _weaponChainsaw, s, "weapon_chainsaw",
                () => s.CaptureState(), o => s.RestoreState((ChainsawState)o));

        public void SetWeaponFlamethrower(Weapon_Flamethrower s) =>
            RegisterSystem(ref _weaponFlamethrower, s, "weapon_flamethrower",
                () => s.CaptureState(), o => s.RestoreState((FlamethrowerState)o));

        public void SetWeaponHMG(Weapon_HMG s) =>
            RegisterSystem(ref _weaponHMG, s, "weapon_hmg",
                () => s.CaptureState(), o => s.RestoreState((HMGState)o));

        public void SetWeaponRPG(Weapon_RPG s) =>
            RegisterSystem(ref _weaponRPG, s, "weapon_rpg",
                () => s.CaptureState(), o => s.RestoreState((RPGState)o));

        public void SetWorldEventDeforestation(WorldEvent_Deforestation s) =>
            RegisterSystem(ref _worldEventDeforestation, s, "world_event_deforestation",
                () => s.CaptureState(), o => s.RestoreState((DeforestationState)o));

        public void SetWorldEventFinalWinter(WorldEvent_FinalWinter s) =>
            RegisterSystem(ref _worldEventFinalWinter, s, "world_event_final_winter",
                () => s.CaptureState(), o => s.RestoreState((FinalWinterState)o));

        public void SetWorldEventFissure(WorldEvent_Fissure s) =>
            RegisterSystem(ref _worldEventFissure, s, "world_event_fissure",
                () => s.CaptureState(), o => s.RestoreState((FissureState)o));

        public void SetWorldEventGreatFamine(WorldEvent_GreatFamine s) =>
            RegisterSystem(ref _worldEventGreatFamine, s, "world_event_great_famine",
                () => s.CaptureState(), o => s.RestoreState((GreatFamineState)o));

        public void SetWorldEventMegafauna(WorldEvent_Megafauna s) =>
            RegisterSystem(ref _worldEventMegafauna, s, "world_event_megafauna",
                () => s.CaptureState(), o => s.RestoreState((MegafaunaState)o));

    }
}
