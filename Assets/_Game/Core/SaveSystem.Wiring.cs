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
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Medical;
using AtomicWar._Game.AI; // HallucinationSystem (audit wiring fix)
using AtomicWar._Game.Economy;
using AtomicWar._Game.Crafting; // CraftingSystem, WorkbenchSystem (audit wiring fix)
using AtomicWar._Game.Events;

namespace AtomicWar._Game.Core
{
    public partial class SaveSystem
    {
        /// <summary>Inject a PhotoperiodSystem after construction (optional; safe to skip in tests).</summary>
        public void SetPhotoPeriodSystem(PhotoperiodSystem photoPeriodSystem)
        {
            _photoPeriodSystem = photoPeriodSystem;
        }

        /// <summary>Inject radiation fog-of-war map (optional; safe to skip in tests).</summary>
        public void SetKnowledgeMap(RadiationKnowledgeMap knowledgeMap)
        {
            _knowledgeMap = knowledgeMap;
        }

        /// <summary>Inject inventory so device battery/calibration/broken persist across save/load.</summary>
        public void SetInventory(Inventory.Inventory inventory)
        {
            _inventory = inventory;
        }

        /// <summary>Inject expedition system (optional; safe to skip in tests).</summary>
        public void SetExpeditionSystem(ExpeditionSystem expeditionSystem)
        {
            _expeditionSystem = expeditionSystem;
        }

        /// <summary>Inject medical triage pipeline so afflictions persist across save/load.</summary>
        public void SetMedicalSystem(MedicalSystem medicalSystem)
        {
            _medicalSystem = medicalSystem;
        }

        /// <summary>Inject Prompt #55 blood transfusion for save/load.</summary>
        public void SetBloodTransfusionSystem(BloodTransfusionSystem sys)
        {
            _bloodTransfusion = sys;
        }

        /// <summary>Inject Prompt #56 amputation for save/load.</summary>
        public void SetAmputationSystem(AmputationSystem sys)
        {
            _amputationSystem = sys;
        }

        /// <summary>Inject Prompt #57 scurvy for save/load.</summary>
        public void SetScurvySystem(ScurvySystem sys)
        {
            _scurvySystem = sys;
        }

        /// <summary>Inject Prompt #60 mutagenesis for save/load.</summary>
        public void SetMutagenesisSystem(RadiationMutagenesisSystem sys)
        {
            _mutagenesisSystem = sys;
        }

        /// <summary>Inject world phase system so CurrentPhase/HasTriggeredExchange persist across save/load.</summary>
        public void SetWorldPhaseSystem(WorldPhaseSystem worldPhaseSystem)
        {
            _worldPhaseSystem = worldPhaseSystem;
        }

        /// <summary>Inject dynamic economy / faction trust matrix for save/load.</summary>
        public void SetEconomySystem(DynamicEconomySystem economySystem)
        {
            _economySystem = economySystem;
        }

        /// <summary>Inject shelter power grid for save/load.</summary>
        public void SetPowerNetwork(PowerNetwork powerNetwork)
        {
            _powerNetwork = powerNetwork;
        }

        /// <summary>Inject hatch defense / raid state for save/load.</summary>
        public void SetHatchDefense(HatchDefenseSystem hatchDefense)
        {
            _hatchDefense = hatchDefense;
        }

        /// <summary>Inject faction radio intercept log for save/load.</summary>
        public void SetFactionRadioIntercepts(FactionRadioInterceptSystem radioIntercepts)
        {
            _factionRadioIntercepts = radioIntercepts;
        }

        /// <summary>Inject diegetic journal / knowledge base for save/load.</summary>
        public void SetJournalSystem(JournalSystem journalSystem)
        {
            _journalSystem = journalSystem;
        }

        /// <summary>Inject campaign win/loss victory project for save/load.</summary>
        public void SetVictoryProjectManager(VictoryProjectManager victoryProject)
        {
            _victoryProject = victoryProject;
        }

        /// <summary>
        /// Inject EventRunner so the scheduled narrative-chain queue
        /// (Prompt #43) persists across save/load.
        /// </summary>
        public void SetEventRunner(EventRunner eventRunner)
        {
            _eventRunner = eventRunner;
        }

        /// <summary>Inject internal-mystery SuspicionTracker for save/load.</summary>
        public void SetSuspicionTracker(SuspicionTracker suspicionTracker)
        {
            _suspicionTracker = suspicionTracker;
        }

        /// <summary>Inject weather-driven hatch entrapment for save/load (Prompt #48).</summary>
        public void SetHatchEntrapment(HatchEntrapmentSystem hatchEntrapment)
        {
            _hatchEntrapment = hatchEntrapment;
        }

        /// <summary>Inject Internal Horror room atmosphere (O2/CO/fire/humidity).</summary>
        public void SetAtmosphereSystem(ShelterAtmosphereSystem atmosphereSystem)
        {
            _atmosphereSystem = atmosphereSystem;
        }

        /// <summary>Inject Internal Horror corpse management for save/load.</summary>
        public void SetCorpseSystem(CorpseManagementSystem corpseSystem)
        {
            _corpseSystem = corpseSystem;
        }

        /// <summary>Inject Internal Horror pantry rust system for save/load.</summary>
        public void SetPantrySystem(PantryContaminationSystem pantrySystem)
        {
            _pantrySystem = pantrySystem;
        }

        /// <summary>Inject Prompt #13 sabotaged-cache habit / plant counters for save/load.</summary>
        public void SetSabotagedCacheSystem(SabotagedCacheSystem sabotagedCaches)
        {
            _sabotagedCaches = sabotagedCaches;
        }

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
        public void SetDebtCollectorSystem(DebtCollectorSystem debtCollector)
        {
            _debtCollector = debtCollector;
        }

        /// <summary>Inject Prompt #19 post-EMP ghost station dial unlock for save/load.</summary>
        public void SetGhostStationSystem(GhostStationSystem ghostStations)
        {
            _ghostStations = ghostStations;
        }

        /// <summary>Inject Prompt #20 Lifeboat Transmission endgame dilemma for save/load.</summary>
        /// <summary>Inject Child Dependent system so child state persists across save/load.</summary>
        public void SetChildDependentSystem(ChildDependentSystem childSystem)
        {
            _childSystem = childSystem;
        }

        /// <summary>Inject Prompt #49 structural integrity for save/load.</summary>
        public void SetStructuralIntegritySystem(StructuralIntegritySystem sys)
        {
            _structuralIntegrity = sys;
        }

        /// <summary>Inject Prompt #50 waste/hygiene for save/load.</summary>
        public void SetWasteSystem(WasteSystem sys)
        {
            _wasteSystem = sys;
        }

        /// <summary>Inject Prompt #51 vermin for save/load.</summary>
        public void SetVerminSystem(VerminSystem sys)
        {
            _verminSystem = sys;
        }

        /// <summary>Inject Prompt #52 jury-rig for save/load.</summary>
        public void SetJuryRigSystem(JuryRigSystem sys)
        {
            _juryRigSystem = sys;
        }

        /// <summary>Inject Prompt #53 freeze-pipe for save/load.</summary>
        public void SetFreezePipeSystem(FreezePipeSystem sys)
        {
            _freezePipeSystem = sys;
        }

        /// <summary>Inject Prompt #67 cartography for save/load.</summary>
        public void SetCartographySystem(CartographySystem sys)
        {
            _cartographySystem = sys;
        }

        /// <summary>Inject Prompt #69 flooded map nodes for save/load.</summary>
        public void SetFloodedNodeSystem(FloodedNodeSystem sys) =>
            RegisterSystem(ref _floodedNodeSystem, sys, "flooded_node",
                () => sys.CaptureState(), o => sys.RestoreState((FloodedNodeSave)o));

        /// <summary>Inject Prompt #71 tracker for save/load.</summary>
        public void SetTrackerSystem(TrackerSystem sys)
        {
            _trackerSystem = sys;
        }

        /// <summary>Inject Prompt #72 dead-drop for save/load.</summary>
        public void SetDeadDropSystem(DeadDropSystem sys)
        {
            _deadDropSystem = sys;
        }

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

        /// <summary>Workbench system (near-stateless; captured for RNG state & future-proofing).</summary>
        public void SetWorkbenchSystem(WorkbenchSystem s)
        {
            _workbenchSystem = s;
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
        public void SetWaterStorage(WaterStorage waterStorage)
        {
            _waterStorage = waterStorage;
        }

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
    }
}
