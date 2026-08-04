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
using AtomicWar._Game.Economy;
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
