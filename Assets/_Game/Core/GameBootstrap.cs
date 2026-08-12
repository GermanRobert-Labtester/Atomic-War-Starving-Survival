using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
// WorkbenchSystem lives in Crafting
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Simulation; // CompostSystem, ChelationSystem, HamRadioSystem, etc. (audit C-3 split)
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Utilities;

using AtomicWar._Game.Endgame;

using AtomicWar._Game.Encounters;

using AtomicWar._Game.World;

using AtomicWar._Game.Narrative;

using AtomicWar._Game.Factions;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Composition root: initializes every system, owns the game loop, and wires
    /// the HUD, input, save/load, and win/lose detection. Drop on a GameObject in
    /// the main scene; no other MonoBehaviour needs to know about system wiring.
    /// </summary>
    public partial class GameBootstrap : MonoBehaviour
    {
        // -----------------------------------------------------------------
        // Inspector references
        // -----------------------------------------------------------------

        [Header("Data Assets")]
        [SerializeField] private NeedsProfile _needsProfile;
        [SerializeField] private LightProfile _lightProfile;
        [SerializeField] private SeasonProfile _seasonProfile;
        [SerializeField] private ItemCatalogSO _itemCatalog;
        [SerializeField] private RecipeCatalogSO _recipeCatalog;
        [SerializeField] private GameEventCatalogSO _eventCatalog;
        [SerializeField] private LocationCatalogSO _locationCatalog;
        [SerializeField] private RadioCatalogSO _radioCatalog;
        [SerializeField] private WorldPhaseConfigSO _worldPhaseConfig;
        [SerializeField] private FlashpointSequenceSO _flashpointSequence;
        [SerializeField] private MentalBreakCatalogSO _mentalBreakCatalog;
        [SerializeField] private LootTableSO _lootTable;

        [Header("UI")]
        [SerializeField] private HUD _hud;

        [Header("Diagnostics (M-1)")]
        [SerializeField] private DiagnosticsOverlay _diagnosticsOverlay;

        [Header("Tuning")]
        [SerializeField] private int _worldSeed = 42;
        [SerializeField] private float _secondsPerGameHour = 10f;
        [SerializeField] private int _campaignLengthDays = 90;

        // -----------------------------------------------------------------
        // Public system accessors
        // -----------------------------------------------------------------

        public GameState GameState { get; private set; }
        public TimeSystem TimeSystem { get; private set; }
        public WeatherSystem WeatherSystem { get; private set; }
        public TemperatureSystem TemperatureSystem { get; private set; }
        public PhotoperiodSystem PhotoperiodSystem { get; private set; }
        public NeedsSystem NeedsSystem { get; private set; }
        public RadiationSystem RadiationSystem { get; private set; }
        public Shelter.Shelter Shelter { get; private set; }
        /// <summary>Climate-terminal adapter for existing filter, heater, and power-grid state.</summary>
        public AirHeatManagementSystem AirHeatManagementSystem { get; private set; }
        /// <summary>Repair-order terminal for existing module and generator condition.</summary>
        public BunkerMaintenanceSystem BunkerMaintenanceSystem { get; private set; }
        /// <summary>Timed, Utility-AI claimed repair task issued by the maintenance terminal.</summary>
        public RepairWorkOrderSystem RepairWorkOrderSystem { get; private set; }
        /// <summary>Continuous crew reservations for existing bunker duties.</summary>
        public SurvivorWorkShiftSystem SurvivorWorkShiftSystem { get; private set; }
        /// <summary>Roster-wide view of active work orders and their survivor reservations.</summary>
        public SurvivorTaskBoardSystem SurvivorTaskBoardSystem { get; private set; }
        public Inventory.Inventory Inventory { get; private set; }
        /// <summary>Daily food/water policy and its shared survivor consequences.</summary>
        public BunkerRationingSystem BunkerRationingSystem { get; private set; }
        /// <summary>Persistent unlimited-capacity bunker stash for returns and craft results that do not fit in the main bag.</summary>
        public Inventory.Inventory OverflowStash { get; private set; }
        /// <summary>Safe transfer bridge between the persistent bunker crate and the field bag.</summary>
        public OverflowCrateSystem OverflowCrateSystem { get; private set; }
        /// <summary>Safe face/body equipment bridge with receiving-crate fallback.</summary>
        public FieldGearLoadoutSystem FieldGearLoadoutSystem { get; private set; }
        /// <summary>Compatibility alias for existing craft integrations.</summary>
        public Inventory.Inventory CraftingOverflowStash => OverflowStash;
        public CraftingSystem CraftingSystem { get; private set; }
        public WorkbenchSystem WorkbenchSystem { get; private set; }
        public UtilityAI UtilityAI { get; private set; }
        public EventRunner EventRunner { get; private set; }
        /// <summary>Internal mysteries — resource-starved Missing Rations chain.</summary>
        public SuspicionTracker SuspicionTracker { get; private set; }
        public SaveSystem SaveSystem { get; private set; }
        public LocationScavengingSystem ScavengingSystem { get; private set; }
        public ExpeditionSystem ExpeditionSystem { get; private set; }
        /// <summary>Weather-driven hatch seal / DigOut / suffocation (Prompt #48).</summary>
        public HatchEntrapmentSystem HatchEntrapmentSystem { get; private set; }
        /// <summary>Entry room used for DigOut CO2 spikes (atmospheric engineering).</summary>
        private ShelterRoom _entryRoom;
        /// <summary>Internal Horror — room O2/CO/humidity/fire.</summary>
        public ShelterAtmosphereSystem AtmosphereSystem { get; private set; }
        /// <summary>Internal Horror — death → corpse item, rot, bury/fertilizer.</summary>
        public CorpseManagementSystem CorpseSystem { get; private set; }
        /// <summary>Internal Horror — humidity rusts food → botulism risk.</summary>
        public PantryContaminationSystem PantrySystem { get; private set; }
        private ShelterRoom _storesRoom;
        public RadiationKnowledgeMap KnowledgeMap { get; private set; }
        /// <summary>Seeded wasteland node graph for expeditions (#23).</summary>
        public GeneratedMap GeneratedMap { get; private set; }
        public RadioBroadcastSystem RadioSystem { get; private set; }
        public RadioTunerSystem RadioTunerSystem { get; private set; }
        public BeliefSystem BeliefSystem { get; private set; }
        public MedicalSystem MedicalSystem { get; private set; }
        public WorldPhaseSystem WorldPhaseSystem { get; private set; }
        public DynamicEconomySystem EconomySystem { get; private set; }
        public HatchDefenseSystem HatchDefenseSystem { get; private set; }
        public PowerNetwork PowerNetwork { get; private set; }
        public WaterStorage WaterStorage { get; private set; }
        public WaterEconomySystem WaterEconomySystem { get; private set; }
        /// <summary>Prompt #11 — Black Rain dread + hazmat melt helpers.</summary>
        public BlackRainHazardSystem BlackRainHazardSystem { get; private set; }
        /// <summary>Prompt #13 — hostile factions plant poisoned medical caches.</summary>
        public SabotagedCacheSystem SabotagedCacheSystem { get; private set; }
        /// <summary>Prompt #14 — windstorms move lethal death-zone rad pockets.</summary>
        public ShiftingHotspotSystem ShiftingHotspotSystem { get; private set; }
        public FlashpointChoreographer FlashpointChoreographer { get; private set; }
        public MentalBreakSystem MentalBreakSystem { get; private set; }
        public HatchDilemmaPrompt HatchDilemmaPromptField { get; private set; }
        /// <summary>Post-repel offer: open trade / demand parley without hunting the UI.</summary>
        public ParleyOfferPrompt ParleyOfferPromptField { get; private set; }
        /// <summary>Faction succession / parley / hatch-bounce radio chatter.</summary>
        public FactionRadioInterceptSystem FactionRadioIntercepts { get; private set; }
        /// <summary>Prompt #17 — inter-faction raid plan wiretaps (antenna required).</summary>
        public FactionRaidPlanSystem FactionRaidPlanSystem { get; private set; }
        /// <summary>Prompt #18 — delayed dig-out debt collector (day + 20).</summary>
        public DebtCollectorSystem DebtCollectorSystem { get; private set; }
        /// <summary>Prompt #19 — post-EMP ghost stations (pre-war loops, no live intel).</summary>
        public GhostStationSystem GhostStationSystem { get; private set; }
        /// <summary>Prompt #49 — structural integrity + cave-ins.</summary>
        public StructuralIntegritySystem StructuralIntegrity { get; private set; }
        /// <summary>Prompt #50 — waste management + hygiene.</summary>
        public WasteSystem WasteSystem { get; private set; }
        /// <summary>Prompt #51 — vermin infestations.</summary>
        public VerminSystem VerminSystem { get; private set; }
        /// <summary>Prompt #52 — module jury-rigging.</summary>
        public JuryRigSystem JuryRigSystem { get; private set; }
        /// <summary>Prompt #53 — freezing pipes + water loss.</summary>
        public FreezePipeSystem FreezePipeSystem { get; private set; }
        /// <summary>Prompt #20 — Lifeboat Transmission (one seat; rest condemned).</summary>
        public LifeboatTransmissionSystem LifeboatTransmissionSystem { get; private set; }
        /// <summary>Diegetic journal book + discovery knowledge (immersive tutorial).</summary>
        public JournalSystem JournalSystem { get; private set; }
        /// <summary>Campaign win/loss projects (radio extraction + vehicle escape).</summary>
        public VictoryProjectManager VictoryProject { get; private set; }
        /// <summary>Campaign endgame evaluation engine (Prompt #41).</summary>
        public EndgameEngine EndgameEngine { get; private set; }

        // Prompt #10 — Skill Atrophy: morale < 20 for 14 days → permanent skill downgrade.
        public SkillAtrophySystem SkillAtrophy { get; private set; }

        // Prompts #179–#181 — action-driven XP, dormant perks, stress epiphany.
        public SkillProgressionSystem SkillProgression { get; private set; }

        // Prompts #182–#188 — combat milestone perks (jams, stealth, ammo, CQ, traps, flee, desensitized).
        public CombatPerkSystem CombatPerks { get; private set; }

        // Prompts #189–#194 — survival cooking / illness / brew / butcher / pharma / mycology.
        public SurvivalPerkSystem SurvivalPerks { get; private set; }
        public CookingSystem CookingSystem { get; private set; }

        // Prompts #195–#200 — shelter engineering perks (jury-rig, struts, HVAC, scrap, dig, fuel).
        public ShelterPerkSystem ShelterPerks { get; private set; }

        // Prompts #201–#205 — medical milestone perks (surgery, triage, radiologist, anatomist, paramedic).
        public MedicalPerkSystem MedicalPerks { get; private set; }

        // Prompts #206–#210 — expedition / wasteland perks (Pack Mule … Forager).
        public ExpeditionPerkSystem ExpeditionPerks { get; private set; }

        // Prompts #211–#213 — social / leadership perks (De-Escalator, Quartermaster, Taskmaster).
        public SocialPerkSystem SocialPerks { get; private set; }

        // Prompts #214–#219 — personal questlines + latent expert traits.
        public PersonalQuestSystem PersonalQuests { get; private set; }

        // Prompt #8 — Empath & Sociopath trait variance.
        public EmpathSystem EmpathSystem { get; private set; }
        // Prompt #61 — Survivor diaries & privacy violations.
        public SurvivorDiariesSystem SurvivorDiaries { get; private set; }
        // Prompt #62 — Internal door locks & guard assignments.
        public InternalLockSystem InternalLockSystem { get; private set; }
        // Prompt #63 — Spatial psychology traits (Claustrophobia / Agoraphobia).
        public SpatialPsychologySystem SpatialPsychology { get; private set; }
        // Prompt #64 — Grief Keepsakes & inventory locking.
        public GriefKeepsakeSystem GriefKeepsakes { get; private set; }
        // Prompt #65 — UI hallucinations & phantom utility actions.
        public AI.HallucinationSystem HallucinationSystem { get; private set; }
        // Prompt #66 — Skill mentorship action.
        public MentorshipSystem MentorshipSystem { get; private set; }

        // Prompt #7 — Addiction & Withdrawal pipeline.
        public AddictionSystem Addiction { get; private set; }
        // Prompt #551 — chem-abuse blood toxicity (bite retaliation).
        public BloodToxicitySystem BloodToxicity { get; private set; }
        /// <summary>Prompt #833 — morphine / amphetamines / anti_rad tolerance.</summary>
        public System_Tolerance ChemTolerance { get; private set; }
        /// <summary>Shared chem consumption side-effects (addiction, toxicity, polypharmacy, tolerance).</summary>
        public ChemUseRouter ChemUse { get; private set; }
        // Prompt #556 — graft/prosthetic rejection + immunosuppressants.
        public GraftRejectionSystem GraftRejection { get; private set; }
        // Prompt #558 — mutant pheromone camo (24h animal friendliness).
        public PheromoneMaskingSystem PheromoneMasking { get; private set; }
        // Prompt #563 — last-will grave site (rogue-lite wipe → next run discovery).
        public LastWillSystem LastWill { get; private set; }
        // Prompt #859 — start next run in ruined prior bunker (from Last Will wipe data).
        public System_LegacyStart LegacyStart { get; private set; }
        // Prompt #829 — A/B/AB/O typing + bag transfusion hemolytic shock.
        public System_BloodTypes BloodTypes { get; private set; }
        // Prompt #768 — empty-bunker epilogue (meals / bullets / death rooms).
        public System_EpilogueStats EpilogueStats { get; private set; }
        // Prompt #839 — bunker gossip: crime witness → rumor spread → affinity rot.
        public System_Gossip Gossip { get; private set; }
        // Prompt #861 — cross-wipe warlord counters from dominant player strategies.
        public System_AdaptiveWarlords AdaptiveWarlords { get; private set; }
        // Prompt #806 — bilge pumps: flood water → purified cistern water.
        public System_BilgePumps BilgePumps { get; private set; }
        // Prompt #658 — outdoor corpses attract vultures (hatch vis + map danger + morale).
        public System_CarrionBirds CarrionBirds { get; private set; }
        // Prompt #799 — IF/THEN module automation on the power grid.
        public System_LogicGates LogicGates { get; private set; }
        // Prompt #864 — community JSON mods under StreamingAssets/Mods or persistentDataPath/Mods.
        public System_ModLoader ModLoader { get; private set; }
        // Prompt #865 — Twitch chat polls (offline-safe stubs; weather / raid / supply).
        public System_TwitchAPI TwitchAPI { get; private set; }
        // Batch wiring — CaptureState systems previously unconstructed (disease/siege/mode/ui/victory).
        public DiseaseSystem_Expansion DiseaseExpansion { get; private set; }
        public Dynamic_Scapegoat Scapegoat { get; private set; }
        public Mode_IronMan IronMan { get; private set; }
        public NPC_Android AndroidNpcs { get; private set; }
        public Role_Sheriff Sheriff { get; private set; }
        public UI_ScenarioGen ScenarioGen { get; private set; }
        public UI_SpeedrunTimer SpeedrunTimer { get; private set; }
        public Victory_TrueEnding TrueEnding { get; private set; }
        // Victory paths (endgame condition trackers) — all except TrueEnding which is in batch.
        public Victory_Airlift VictoryAirlift { get; private set; }
        public Victory_Ascendancy VictoryAscendancy { get; private set; }
        public Victory_BuriedAlive VictoryBuriedAlive { get; private set; }
        public Victory_CannibalKing VictoryCannibalKing { get; private set; }
        public Victory_Defection VictoryDefection { get; private set; }
        public Victory_Icebreaker VictoryIcebreaker { get; private set; }
        public Victory_LoneSurvivor VictoryLoneSurvivor { get; private set; }
        public Victory_MAD VictoryMAD { get; private set; }
        public Victory_Migration VictoryMigration { get; private set; }
        public Victory_TheBroadcast VictoryTheBroadcast { get; private set; }
        public Victory_TheCure VictoryTheCure { get; private set; }
        public Victory_TheMartian VictoryTheMartian { get; private set; }
        public Victory_UndergroundCity VictoryUndergroundCity { get; private set; }
        public Victory_Unifier VictoryUnifier { get; private set; }
        // Map hazards — expedition/map node hazard trackers.
        public MapHazard_AcidGeyser MapHazardAcidGeyser { get; private set; }
        public MapHazard_Ashlanche MapHazardAshlanche { get; private set; }
        public MapHazard_BiometricDoor MapHazardBiometricDoor { get; private set; }
        public MapHazard_CraterWall MapHazardCraterWall { get; private set; }
        public MapHazard_Crevice MapHazardCrevice { get; private set; }
        public MapHazard_FlammableGas MapHazardFlammableGas { get; private set; }
        public MapHazard_GasPockets MapHazardGasPockets { get; private set; }
        public MapHazard_MagneticAnomaly MapHazardMagneticAnomaly { get; private set; }
        public MapHazard_SinkholeCollapse MapHazardSinkholeCollapse { get; private set; }
        public MapHazard_VenusTrap MapHazardVenusTrap { get; private set; }
        public MapHazard_FrozenSurvivor MapHazardFrozenSurvivor { get; private set; }
        // Map anomalies — expedition/map node anomaly trackers.
        public MapAnomaly_AshDunes MapAnomalyAshDunes { get; private set; }
        public MapAnomaly_BoilingLake MapAnomalyBoilingLake { get; private set; }
        public MapAnomaly_Cherenkov MapAnomalyCherenkov { get; private set; }
        public MapAnomaly_DogDen MapAnomalyDogDen { get; private set; }
        public MapAnomaly_DontLook MapAnomalyDontLook { get; private set; }
        public MapAnomaly_DryCoral MapAnomalyDryCoral { get; private set; }
        public MapAnomaly_FloodedSubway MapAnomalyFloodedSubway { get; private set; }
        public MapAnomaly_GlassCrater MapAnomalyGlassCrater { get; private set; }
        public MapAnomaly_MassGrave MapAnomalyMassGrave { get; private set; }
        public MapAnomaly_Mirage MapAnomalyMirage { get; private set; }
        public MapAnomaly_PetrifiedForest MapAnomalyPetrifiedForest { get; private set; }
        public MapAnomaly_QuietZone MapAnomalyQuietZone { get; private set; }
        public MapAnomaly_RustedTank MapAnomalyRustedTank { get; private set; }
        public MapAnomaly_ServerFarm MapAnomalyServerFarm { get; private set; }
        public MapAnomaly_Sinkhole MapAnomalySinkhole { get; private set; }
        public MapAnomaly_TangledDrop MapAnomalyTangledDrop { get; private set; }
        public MapAnomaly_TireFire MapAnomalyTireFire { get; private set; }
        public MapAnomaly_UXO_Nuke MapAnomalyUxoNuke { get; private set; }
        // Expedition biomes — terrain modifiers for map travel.
        public Biome_AshSwamp BiomeAshSwamp { get; private set; }
        public Biome_GlassDesert BiomeGlassDesert { get; private set; }
        public Biome_HighwayTunnel BiomeHighwayTunnel { get; private set; }
        public Biome_SaltFlats BiomeSaltFlats { get; private set; }
        public Biome_SkyscraperTops BiomeSkyscraperTops { get; private set; }
        public Biome_Suburbs BiomeSuburbs { get; private set; }
        // Special weather events — distinct from core WeatherSystem.
        public Weather_AcidSnow WeatherAcidSnow { get; private set; }
        public Weather_BioFog WeatherBioFog { get; private set; }
        public Weather_BlackSnow WeatherBlackSnow { get; private set; }
        public Weather_BloodRain WeatherBloodRain { get; private set; }
        public Weather_DeadWind WeatherDeadWind { get; private set; }
        public Weather_DeepFreeze WeatherDeepFreeze { get; private set; }
        public Weather_DustDevil WeatherDustDevil { get; private set; }
        public Weather_EMPStorm WeatherEmpStorm { get; private set; }
        public Weather_FalseSpring WeatherFalseSpring { get; private set; }
        public Weather_GlassStorm WeatherGlassStorm { get; private set; }
        public Weather_OzoneHole WeatherOzoneHole { get; private set; }
        public Weather_RadHail WeatherRadHail { get; private set; }
        public Weather_SilentSpring WeatherSilentSpring { get; private set; }
        public Weather_SolarFlare WeatherSolarFlare { get; private set; }
        public Weather_StaticCharge WeatherStaticCharge { get; private set; }
        // Prompts #319–#325 — Section X new weather events
        public Weather_AshLightning WeatherAshLightning { get; private set; }
        public Weather_FogOfParticulate WeatherFogOfParticulate { get; private set; }
        public Weather_ThermalInversion WeatherThermalInversion { get; private set; }
        public Weather_IceStorm WeatherIceStorm { get; private set; }
        public Weather_Silence WeatherSilence { get; private set; }
        // Expedition encounters — combat/exploration set pieces.
        public Encounter_Amalgamation EncounterAmalgamation { get; private set; }
        public BurrowersSystem EncounterBurrowers { get; private set; }
        public Encounter_FloodedMaze EncounterFloodedMaze { get; private set; }
        public Encounter_GlowingDead EncounterGlowingDead { get; private set; }
        public Encounter_GlowingStag EncounterGlowingStag { get; private set; }
        public Encounter_HitAndRun EncounterHitAndRun { get; private set; }
        public Encounter_Leeches EncounterLeeches { get; private set; }
        public Encounter_Mirelurker EncounterMirelurker { get; private set; }
        public Encounter_PressurePlate EncounterPressurePlate { get; private set; }
        public Encounter_RiverPirates EncounterRiverPirates { get; private set; }
        public Encounter_Roadblock EncounterRoadblock { get; private set; }
        public Encounter_RobotDog EncounterRobotDog { get; private set; }
        public Encounter_SleepingCamp EncounterSleepingCamp { get; private set; }
        public Encounter_TripwireMaze EncounterTripwireMaze { get; private set; }
        public Encounter_WarlordTank EncounterWarlordTank { get; private set; }
        // Shelter modules with CaptureState (46) — full CaptureState set wired.
        public ShelterModule_AcidTrap ShelterModuleAcidTrap { get; private set; }
        public ShelterModule_Autodoc ShelterModuleAutodoc { get; private set; }
        public ShelterModule_CCTV ShelterModuleCctv { get; private set; }
        public ShelterModule_Classroom ShelterModuleClassroom { get; private set; }
        public ShelterModule_Confessional ShelterModuleConfessional { get; private set; }
        public ShelterModule_Conveyor ShelterModuleConveyor { get; private set; }
        public ShelterModule_DaylightSensor ShelterModuleDaylightSensor { get; private set; }
        public ShelterModule_DroneStation ShelterModuleDroneStation { get; private set; }
        public ShelterModule_HoloEmitter ShelterModuleHoloEmitter { get; private set; }
        public ShelterModule_InsectFarm ShelterModuleInsectFarm { get; private set; }
        public ShelterModule_Lathe ShelterModuleLathe { get; private set; }
        public ShelterModule_Mortar ShelterModuleMortar { get; private set; }
        public ShelterModule_PanicButton ShelterModulePanicButton { get; private set; }
        public ShelterModule_Pitfall ShelterModulePitfall { get; private set; }
        public ShelterModule_Reloader ShelterModuleReloader { get; private set; }
        public ShelterModule_Sorter ShelterModuleSorter { get; private set; }
        public ShelterModule_Thermostat ShelterModuleThermostat { get; private set; }
        public ShelterModule_WasteChute ShelterModuleWasteChute { get; private set; }
        public ShelterModule_Autopsy ShelterModuleAutopsy { get; private set; }
        public ShelterModule_BatteryBank ShelterModuleBatteryBank { get; private set; }
        public ShelterModule_BioLatrine ShelterModuleBioLatrine { get; private set; }
        public ShelterModule_ChoreBoard ShelterModuleChoreBoard { get; private set; }
        public ShelterModule_DeadManSwitch ShelterModuleDeadManSwitch { get; private set; }
        public ShelterModule_DeconShower ShelterModuleDeconShower { get; private set; }
        public ShelterModule_Dialysis ShelterModuleDialysis { get; private set; }
        public ShelterModule_DistressBeacon ShelterModuleDistressBeacon { get; private set; }
        public ShelterModule_DronePad ShelterModuleDronePad { get; private set; }
        public ShelterModule_Garage ShelterModuleGarage { get; private set; }
        public ShelterModule_GunRack ShelterModuleGunRack { get; private set; }
        public ShelterModule_Hammock ShelterModuleHammock { get; private set; }
        public ShelterModule_HandCrank ShelterModuleHandCrank { get; private set; }
        public ShelterModule_HotShower ShelterModuleHotShower { get; private set; }
        public ShelterModule_Incinerator ShelterModuleIncinerator { get; private set; }
        public MagmaTapSystem ShelterModuleMagmaTap { get; private set; }
        public ShelterModule_MotionSensor ShelterModuleMotionSensor { get; private set; }
        public PanicRoomSystem ShelterModulePanicRoom { get; private set; }
        public ShelterModule_PrintingPress ShelterModulePrintingPress { get; private set; }
        public ShelterModule_PunchingBag ShelterModulePunchingBag { get; private set; }
        public ShelterModule_RainBarrel ShelterModuleRainBarrel { get; private set; }
        public ShelterModule_RecordPlayer ShelterModuleRecordPlayer { get; private set; }
        public ShelterModule_Sprinklers ShelterModuleSprinklers { get; private set; }
        public ThumperSystem ShelterModuleThumper { get; private set; }
        public ShelterModule_TreadmillGen ShelterModuleTreadmillGen { get; private set; }
        public ShelterModule_Turret ShelterModuleTurret { get; private set; }
        public ShelterModule_VaultDoor ShelterModuleVaultDoor { get; private set; }
        public ShelterModule_WoodStove ShelterModuleWoodStove { get; private set; }
        // Narrative Event_* systems with CaptureState (27) — full CaptureState set wired.
        public Event_Brawl EventBrawl { get; private set; }
        public Event_ComingOfAge EventComingOfAge { get; private set; }
        public Event_CultBlessing EventCultBlessing { get; private set; }
        public Event_CultInitiation EventCultInitiation { get; private set; }
        public Event_CultOfAI EventCultOfAi { get; private set; }
        public Event_EMPCascade EventEmpCascade { get; private set; }
        public Event_FeralRescue EventFeralRescue { get; private set; }
        public Event_FoundDiary EventFoundDiary { get; private set; }
        public Event_GriefCascade EventGriefCascade { get; private set; }
        public Event_HungerStrike EventHungerStrike { get; private set; }
        public Event_NodeCollapse EventNodeCollapse { get; private set; }
        public Event_RansomNote EventRansomNote { get; private set; }
        public Event_Schism EventSchism { get; private set; }
        public Event_SecretSociety EventSecretSociety { get; private set; }
        public Event_SiblingFeud EventSiblingFeud { get; private set; }
        public Event_SpontaneousMurder EventSpontaneousMurder { get; private set; }
        public Event_TeenRebellion EventTeenRebellion { get; private set; }
        public Event_WitchHunt EventWitchHunt { get; private set; }
        public Event_EuthanasiaPact EventEuthanasiaPact { get; private set; }
        public Event_FactionMerger EventFactionMerger { get; private set; }
        public Event_Mudslide EventMudslide { get; private set; }
        public Event_NumbersStation EventNumbersStation { get; private set; }
        public Event_ProjectSabotage EventProjectSabotage { get; private set; }
        public FoundationSinkholeSystem EventSinkhole { get; private set; }
        public Event_Triangulation EventTriangulation { get; private set; }
        public VaultCollisionSystem EventVaultCollision { get; private set; }
        public Event_WarlordSuccession EventWarlordSuccession { get; private set; }
        public Siege_Artillery SiegeArtillery { get; private set; }
        public Siege_Biowarfare SiegeBiowarfare { get; private set; }
        public Siege_Blockade SiegeBlockade { get; private set; }
        public Siege_HostageShield SiegeHostageShield { get; private set; }
        public Siege_NightRaid SiegeNightRaid { get; private set; }
        public Siege_Sappers SiegeSappers { get; private set; }
        public Siege_SmokeOut SiegeSmokeOut { get; private set; }
        public Siege_VehicleRam SiegeVehicleRam { get; private set; }
        // Prompt #55 — blood typing + transfusions.
        public BloodTransfusionSystem BloodTransfusion { get; private set; }
        // Prompt #56 — surgical amputation + phantom pain.
        public AmputationSystem AmputationSystem { get; private set; }
        // Prompt #57 — scurvy / VitaminC deficiency.
        public ScurvySystem ScurvySystem { get; private set; }
        // Prompt #60 — radiation mutagenesis stages.
        public RadiationMutagenesisSystem Mutagenesis { get; private set; }
        // Prompt #67 — cartography table.
        public CartographySystem CartographySystem { get; private set; }
        // Prompt #68 — bicycle logistics.
        public BicycleSystem BicycleSystem { get; private set; }
        // Prompt #69 — flooded ruins.
        public FloodedNodeSystem FloodedNodeSystem { get; private set; }
        // Prompt #569 — river crossings, bridges, faction blockades.
        public RiverNodeSystem RiverNodeSystem { get; private set; }
        // Prompt #71 — tracker (footprints in ash).
        public TrackerSystem TrackerSystem { get; private set; }
        // Prompt #72 — dead drops.
        public DeadDropSystem DeadDropSystem { get; private set; }
        // Prompt #73 — hostage situations.
        public HostageSystem HostageSystem { get; private set; }
        // Prompt #74 — propaganda broadcasting.
        public PropagandaSystem PropagandaSystem { get; private set; }
        // Prompt #75 — deserter/spy mechanic.
        public DeserterSystem DeserterSystem { get; private set; }
        // Prompt #76 — weather scapegoating.
        public WeatherScapegoatSystem ScapegoatSystem { get; private set; }
        // Prompt #77 — slave labor camps.
        public LaborCampSystem LaborCampSystem { get; private set; }
        // Prompt #78 — cult moral disgust.
        public CultMoralDisgustSystem CultMoralSystem { get; private set; }
        // Expansion II — The Weight of Factions: four faction-pressure systems.
        public System_GarrisonComplianceLedger GarrisonComplianceLedger { get; private set; }
        public System_MilitiaContributionTax MilitiaContributionTax { get; private set; }
        public System_CultLeash CultLeash { get; private set; }
        public System_WarlordTribute WarlordTributeSystem { get; private set; }
        // Prompt #79 — mutated ecosystem (flora/fauna).
        public MutatedEcosystemSystem EcosystemSystem { get; private set; }
        // Prompt #79–#84 — house-to-bunker transition.
        public HouseToBunkerSystem HouseToBunkerSystem { get; private set; }
        // Prompts #901/#903/#904 — narrative encounter state. Their EncounterSOs
        // were already registered; these hold the outcomes the choices resolve to.
        public Encounter_DeadLetterOffice DeadLetterOffice { get; private set; }
        public Encounter_WeatherStation WeatherStation { get; private set; }
        public Encounter_Pianist Pianist { get; private set; }
        /// <summary>The selected shelter layout for this run.</summary>
        public Shelter.ShelterMapSO ShelterLayout { get; private set; }
        /// <summary>Prompts #85–#94 — multi-stage location quests.</summary>
        public LocationQuestSystem LocationQuestSystem { get; private set; }
        public ExcavationSystem ExcavationSystem { get; private set; }
        public RoomFloodingSystem FloodingSystem { get; private set; }
        public HiddenStorageSystem HiddenStorageSystem { get; private set; }
        public CeilingCollapseSystem CeilingCollapseSystem { get; private set; }
        public PerimeterTrapSystem PerimeterTrapSystem { get; private set; }
        public TunnelingSystem TunnelingSystem { get; private set; }
        public HatchVisibilitySystem HatchVisibilitySystem { get; private set; }
        public EscapeHatchSystem EscapeHatchSystem { get; private set; }
        public MaterialShieldingSystem MaterialShieldingSystem { get; private set; }
        public AirlockSystem AirlockSystem { get; private set; }
        public NoiseSystem NoiseSystem { get; private set; }
        public ClothingDegradationSystem ClothingSystem { get; private set; }
        public ResilienceSystem ResilienceSystem { get; private set; }
        public CompostSystem CompostSystem { get; private set; }
        public ScrapWeaponSystem ScrapWeaponSystem { get; private set; }
        public SterilizationSystem SterilizationSystem { get; private set; }
        public ChelationSystem ChelationSystem { get; private set; }
        public WindTurbineSystem WindTurbineSystem { get; private set; }
        public AntibioticResistanceSystem AntibioticResistSystem { get; private set; }
        public InternalHaulingSystem HaulingSystem { get; private set; }
        public WeaponMaintenanceSystem WeaponMaintenanceSystem { get; private set; }
        public RoomAestheticsSystem AestheticsSystem { get; private set; }
        public HamRadioSystem HamRadioSystem { get; private set; }
        public TriageBoardSystem TriageSystem { get; private set; }
        public PolypharmacySystem PolypharmacySystem { get; private set; }

        // Prompt #6 — Phantom Intruders (fake hatch breach alerts).
        public PhantomIntruderSystem PhantomIntruders { get; private set; }

        // Prompt #9 — The Child dependent mechanic.
        public ChildDependentSystem ChildSystem { get; private set; }

        // Animal companions (morale, CO2, vermin suppression via cats).
        public PetSystem PetSystem { get; private set; }

        // Prompt #380 — pre-war gasoline varnish degradation → diesel burn cost.
        public FuelDecaySystem FuelDecaySystem { get; private set; }

        // Prompt #5 — Diary fragment catalog for Previous Tenants.
        public List<DiaryFragmentSO> DiaryCatalog { get; private set; }
        public List<Survivor> Survivors { get; private set; }
        public List<SurvivorAction> Actions { get; private set; }

        /// <summary>
        /// H-5: Central system registry. All systems are registered here during
        /// InitializeSystems(). TickSystems() dispatches via the registry.
        /// The registry detects the C-1 class of bug: system constructed but never
        /// registered in any tick list.
        /// Backed by the same instance as <c>_registry</c> (was previously never
        /// assigned, so diagnostics and tests always saw null).
        /// </summary>
        public SystemRegistry Registry => _registry;

        /// <summary>Number of per-substep tick registrations (for tests).</summary>
        public int PerSubstepTickCount => Registry?.PerSubstepCount ?? 0;

        /// <summary>Number of daily tick registrations (for tests).</summary>
        public int DailyTickCount => Registry?.DailyCount ?? 0;

        /// <summary>Ephemeral faction stockpiles for OpenTradeWithFaction.</summary>
        private readonly Dictionary<string, Inventory.Inventory> _factionStocks =
            new Dictionary<string, Inventory.Inventory>();

        /// <summary>Fast-forward speed for the F-key toggle.</summary>
        public const float FastForwardScale = 3f;

        /// <summary>
        /// Sub-step guard for the frame loop: after a long hitch the carried
        /// game-time is consumed in at most this many steps per frame; the
        /// remainder rolls into the next frame (no spiral of death, no lost time).
        ///
        /// Budget: at 1 game-hour per step (TimeSystem.MaxGameHoursPerStep),
        /// this is 128 game-hours of work per frame. At 3× fast-forward, that's
        /// ~42 real-time seconds of catch-up per frame (10 real-sec/game-hr ÷ 3×).
        /// If the player has been on a menu for 30 minutes at 3×, they'll catch
        /// up in ~43 frames (~0.7s at 60 FPS). The watchdog logs drops if this
        /// budget is exceeded.
        /// </summary>
        private const int MaxSubstepsPerFrame = 128;

        /// <summary>Game hours owed to the systems from previous frames (large-delta carry).</summary>
        private float _pendingGameHours;

        // --- H-1: TimeSystem substep watchdog (audit H-1) ---

        /// <summary>Number of frames where the per-frame substep budget was exceeded
        /// and game time was carried into the next frame. The carry itself is not
        /// lost (the leftover rolls into the next frame's budget) but the player is
        /// warned that real-time is outpacing game-time.</summary>
        public int DropEventCount { get; private set; }

        /// <summary>Cumulative game hours that were carried into the next frame due
        /// to substep-budget overflow. Reset is not exposed (intentional: the
        /// total over a session is the diagnostic signal).</summary>
        public float TotalDroppedGameHours { get; private set; }

        /// <summary>High-water mark of substeps run in a single frame. Useful for
        /// diagnosing hitches on slow hardware.</summary>
        public int PeakSubstepsInOneFrame { get; private set; }

        /// <summary>Last frame's dropped hours (used for diagnostics overlay).</summary>
        public float LastFrameDroppedGameHours { get; private set; }

        /// <summary>How often the watchdog logs a warning. Every Nth overflow event
        /// emits a log line, so the log isn't flooded when the player spends a long
        /// time on a menu then returns at high fast-forward.</summary>
        private const int WatchdogLogEveryNEvents = 30;

        /// <summary>Recycles journal entries so long sessions allocate no entry garbage.</summary>
        private GenericObjectPool<JournalEntry> _journalEntryPool;

        /// <summary>Reusable fog-of-war view buffer (no per-refresh list allocation).</summary>
        private readonly List<MapTilePlayerView> _knowledgeViewBuffer = new List<MapTilePlayerView>();

        /// <summary>Last day house artillery damage was applied (Prompt #79).</summary>
        private int _lastHouseDay;
        /// <summary>Prompt #658 — true while shelter-node danger boost from vultures is applied.</summary>
        private bool _carrionMapDangerApplied;
        /// <summary>Prompt #799 — re-entrancy guard while logic gates apply power actions.</summary>
        private bool _logicGatesTicking;

        // ── Day-tick GC caches (no per-hour new Random / context / lambda) ──
        private System.Random _mentalBreakRng;
        private System.Random _phantomRng;
        private System.Random _eventCtxRng;
        private System.Random _aiRng;

        private readonly AIContext _aiContextScratch = new AIContext();
        private readonly EventContext _eventContextScratch = new EventContext();
        private Func<string, float> _getFactionTrustEffective;
        private Func<string, float> _getFactionTrustStored;
        private Action<string, int, string> _scheduleEventCached;
        private Action<string, bool> _onEventFlagChangedCached;
        private Func<string, float, float, bool> _tryApplyPedalCostCached;
        private Func<IReadOnlyList<Survivor>> _getSurvivorsCached;
        // Audit C-1: per-day wiring object for the systems added in Prompts
        // #119-#178. The object lives in GameBootstrap so its state survives
        // across substeps; the wiring is idempotent on the same day.
        private SystemWiring _systemWiring;

        /// <summary>
        /// H-5: Central system registry. Every system is registered here in
        /// InitializeSystems() right after construction. TickSystems() dispatches
        /// via the registry. The registry's diagnostic (GetUntickedSystems)
        /// detects the C-1 class of bug: a system constructed but never ticked.
        /// </summary>
        private SystemRegistry _registry;

        /// <summary>
        /// C-1: bag of unsubscribe actions for the anonymous-lambda / instance-method
        /// event subscriptions made across the Init* partials. DisposeAll() runs from
        /// OnDestroy so a destroyed bootstrap can actually be garbage collected instead
        /// of being kept alive by every long-lived system it once subscribed to.
        /// </summary>
        private readonly AtomicWar._Game.Utilities.SubscriptionBag _subscriptions = new AtomicWar._Game.Utilities.SubscriptionBag();

        // -----------------------------------------------------------------
        // H-2: Cached delegate fields for OnDestroy cleanup.
        // The lambdas attached to class-level events below are also kept as
        // instance fields so OnDestroy can match the exact delegate
        // instance and unsubscribe cleanly. Without this, the static
        // events would hold the bootstrap alive forever.
        // -----------------------------------------------------------------

        private System.Action<AtomicWar._Game.Survivors.WorldPhase> _onWorldPhaseChanged;
        private System.Action<GamePhase> _onGameStateChanged;
        private System.Action<Survivor> _onNeedsDied;
        private System.Action<Survivor, AtomicWar._Game.Survivors.NeedKind, float> _onNeedChanged;
        private System.Action<int> _onDayTick_SetGameStateDay;
        private System.Action<Survivor, SurvivorStatus> _onRadiationStatusGained;
        private System.Action<Survivor, float> _onRadiationDoseChanged;
        private System.Action<int, int> _onHourTickHud;
        // H-2: cached so the OnExpeditionRequested subscription can be undone
        // when the bootstrap is destroyed. Without this, every Awake of a
        // bootstrap (Editor play-mode, scene reload) would leak the lambda.
        private System.Action<Survivor, string, ExpeditionPathRequest> _onMapExpeditionRequested;
        private System.Func<Survivor, LocationDefinitionSO, bool> _onScavengeDispatchRequested;
        private System.Action<ActiveMission> _onScavengeMissionStartedHud;
        private System.Action<ActiveMission, List<ItemDefinition>> _onScavengeMissionCompletedHud;
        private System.Action<ScavengeAfterActionReport> _onScavengeAfterActionHud;
        private System.Action<string> _onOverflowCrateTransferRequested;
        private System.Action<string> _onFieldGearEquipRequested;
        private System.Action<EquipSlot> _onFieldGearUnequipRequested;
        private System.Action<RationResource, int> _onRationLevelAdjustmentRequested;
        private System.Action<int> _onWaterQueueCycleRequested;
        private System.Action<AirHeatLoad, int> _onAirHeatPriorityAdjustmentRequested;
        private System.Action<AirHeatLoad> _onAirHeatRequestToggleRequested;
        private System.Action<MaintenanceTargetType, string> _onMaintenanceRepairRequested;
        private System.Action _onMaintenanceRepairCancellationRequested;
        private System.Action<string> _onMaintenanceSurvivorAssignmentRequested;
        private System.Action<int> _onMaintenancePriorityAdjustmentRequested;
        private System.Action<int> _onTaskBoardPriorityAdjustmentRequested;
        private System.Action _onTaskBoardCancellationRequested;
        private System.Action<WorkShiftDuty, string> _onTaskBoardShiftAssignmentRequested;
        private System.Action<WorkShiftDuty> _onTaskBoardShiftCancellationRequested;
        private System.Action _onTaskBoardRecommendationApprovalRequested;

        // -----------------------------------------------------------------
        // GameOver state
        // -----------------------------------------------------------------

        public bool IsGameOver { get; private set; }
        public string GameOverReason { get; private set; }
        public EndgameState EndgameState =>
            VictoryProject != null ? VictoryProject.State : EndgameState.Ongoing;

        // -----------------------------------------------------------------
        // Lifecycle
        // -----------------------------------------------------------------


        // -----------------------------------------------------------------
        // H-2: Lifecycle cleanup
        // -----------------------------------------------------------------



        // -----------------------------------------------------------------
        // H-1: Public TickFrame (extracted from Update for testability)
        // -----------------------------------------------------------------


        // -----------------------------------------------------------------
        // Initialization
        // -----------------------------------------------------------------





        // -----------------------------------------------------------------
        // Prompt #20 — Lifeboat Transmission
        // -----------------------------------------------------------------





        // ─────────────────────────────────────────────────────────────────
        // Prompt #47 — Blood for Water: link DynamicEconomy (#25) to
        // MedicalSystem (#24). When a faction convoy visits the hatch with
        // an empty inventory and demands biological payment, the choice
        // here inflicts the actual BloodLossAffliction on the donor and,
        // if the choice was forced, slams the affinity matrix.
        // ─────────────────────────────────────────────────────────────────






        /// <summary>Room ids we already auto-prompted for (avoid reopening every frame).</summary>
        private readonly HashSet<string> _fireAlertShownRooms = new HashSet<string>();




























        // -----------------------------------------------------------------
        // Game loop
        // -----------------------------------------------------------------





        // -----------------------------------------------------------------
        // Prompt #17 — Raid plan wiretap
        // -----------------------------------------------------------------




        // -----------------------------------------------------------------
        // Prompt #18 — Debt Collector
        // -----------------------------------------------------------------




        // -----------------------------------------------------------------
        // Win/Lose (VictoryProjectManager)
        // -----------------------------------------------------------------









        // -----------------------------------------------------------------
        // Public API (for UI buttons, input handler, etc.)
        // -----------------------------------------------------------------



        /// <summary>Current simulation speed (1 normal, 3 fast-forward).</summary>
        public float TimeScale => TimeSystem != null ? TimeSystem.TimeScale : 1f;



































        // ── CoreFamilies bulk properties (auto) ─────────────────────────
        public FalloutStormHazardSystem FalloutStormHazard { get; private set; }
        public Action_Crawlspace ActionCrawlspace { get; private set; }
        public Action_Play ActionPlay { get; private set; }
        public Action_SlaughterPet ActionSlaughterPet { get; private set; }
        public Action_TeachChild ActionTeachChild { get; private set; }
        public Action_TellStories ActionTellStories { get; private set; }
        public Item_AshGoat ItemAshGoat { get; private set; }
        public Item_Boots ItemBoots { get; private set; }
        public Item_LiveTrap ItemLiveTrap { get; private set; }
        public Item_MutantChicken ItemMutantChicken { get; private set; }
        public Item_Toys ItemToys { get; private set; }
        public Trait_AshTongue TraitAshTongue { get; private set; }
        public Trait_Kleptomaniac TraitKleptomaniac { get; private set; }
        public Trait_Mascot TraitMascot { get; private set; }
        public Trait_StuntedEmpathy TraitStuntedEmpathy { get; private set; }
        public Trait_Superstitious TraitSuperstitious { get; private set; }
        public Affliction_BunkerFever AfflictionBunkerFever { get; private set; }
        public Affliction_ZoonoticFlu AfflictionZoonoticFlu { get; private set; }
        public Module_RationLock ModuleRationLock { get; private set; }
        public Node_Orphanage NodeOrphanage { get; private set; }
        public Pet_GuardDog PetGuardDog { get; private set; }
        public Action_AdministerPlacebo ActionAdministerPlacebo { get; private set; }
        public Action_BarricadeDoor ActionBarricadeDoor { get; private set; }
        public Action_BoilBatteries ActionBoilBatteries { get; private set; }
        public Action_BroadcastPropaganda ActionBroadcastPropaganda { get; private set; }
        public Action_BurnCharcoal ActionBurnCharcoal { get; private set; }
        public Action_BuryTimeCapsule ActionBuryTimeCapsule { get; private set; }
        public Action_CallCaravan ActionCallCaravan { get; private set; }
        public Action_CoverTracks ActionCoverTracks { get; private set; }
        public Action_CrackMainframe ActionCrackMainframe { get; private set; }
        public Action_Decrypt ActionDecrypt { get; private set; }
        public Action_DemandTribute ActionDemandTribute { get; private set; }
        public Action_EstablishRoute ActionEstablishRoute { get; private set; }
        public Action_Exile ActionExile { get; private set; }
        public Action_Fish ActionFish { get; private set; }
        public Action_HarvestOrgans ActionHarvestOrgans { get; private set; }
        public Action_InfectSelf ActionInfectSelf { get; private set; }
        public Action_IsotopeTrace ActionIsotopeTrace { get; private set; }
        public Action_Mercy ActionMercy { get; private set; }
        public Action_MixCement ActionMixCement { get; private set; }
        public Action_MixChems ActionMixChems { get; private set; }
        public Action_Overwatch ActionOverwatch { get; private set; }
        public Action_PhysicalTherapy ActionPhysicalTherapy { get; private set; }
        public Action_PirateRadio ActionPirateRadio { get; private set; }
        public Action_PlaceBait ActionPlaceBait { get; private set; }
        public Action_PullTooth ActionPullTooth { get; private set; }
        public Action_RigCorpse ActionRigCorpse { get; private set; }
        public Action_RoutePower ActionRoutePower { get; private set; }
        public Action_Sabotage ActionSabotage { get; private set; }
        public Action_ScorchedEarth ActionScorchedEarth { get; private set; }
        public Action_SealRoom ActionSealRoom { get; private set; }
        public Action_SelfSurgery ActionSelfSurgery { get; private set; }
        public Action_SilentTakedown ActionSilentTakedown { get; private set; }
        public Action_SiphonGas ActionSiphonGas { get; private set; }
        public Action_StabilizeDNA ActionStabilizeDNA { get; private set; }
        public Action_Stargazing ActionStargazing { get; private set; }
        public Action_WorshipIdol ActionWorshipIdol { get; private set; }
        public Affliction_AdrenalineCrash AfflictionAdrenalineCrash { get; private set; }
        public AmnesiaSystem AfflictionAmnesia { get; private set; }
        public Affliction_Brainwashed AfflictionBrainwashed { get; private set; }
        public BrittleBonesSystem AfflictionBrittleBones { get; private set; }
        public CaveMadnessSystem AfflictionCaveMadness { get; private set; }
        public FeralRegressionSystem AfflictionFeralRegression { get; private set; }
        public ImaginaryFriendSystem AfflictionImaginaryFriend { get; private set; }
        public Affliction_NerveDamage AfflictionNerveDamage { get; private set; }
        public Affliction_OldAge AfflictionOldAge { get; private set; }
        public Affliction_PhantomLimb AfflictionPhantomLimb { get; private set; }
        public Affliction_RadHallucinations AfflictionRadHallucinations { get; private set; }
        public RadiationBlindnessSystem AfflictionRadiationBlindness { get; private set; }
        public Affliction_ScurvyDegeneration AfflictionScurvyDegeneration { get; private set; }
        public SporeLungSystem AfflictionSporeLung { get; private set; }
        public Affliction_Sterile AfflictionSterile { get; private set; }
        public SurvivorsGuiltSystem AfflictionSurvivorsGuilt { get; private set; }
        public Affliction_TBI AfflictionTBI { get; private set; }
        public Affliction_ThyroidCancer AfflictionThyroidCancer { get; private set; }
        public TrenchFootSystem AfflictionTrenchFoot { get; private set; }
        public AudioEvent_Deafening AudioEventDeafening { get; private set; }
        public AudioEvent_Heartbeat AudioEventHeartbeat { get; private set; }
        public Combat_BleedOut CombatBleedOut { get; private set; }
        public Combat_Flanking CombatFlanking { get; private set; }
        public Combat_Suppression CombatSuppression { get; private set; }
        public CombatStance_LastStand CombatStanceLastStand { get; private set; }
        public Crisis_FeralFlora CrisisFeralFlora { get; private set; }
        public Crisis_StructuralFailure CrisisStructuralFailure { get; private set; }
        public Durability_Suppressor DurabilitySuppressor { get; private set; }
        public Endgame_Ultimatum EndgameUltimatum { get; private set; }
        public Hazard_CookOff HazardCookOff { get; private set; }
        public Hazard_ExplosiveCrafting HazardExplosiveCrafting { get; private set; }
        public Hazard_FriendlyFire HazardFriendlyFire { get; private set; }
        public MethaneSystem HazardMethane { get; private set; }
        public Hazard_MimicCrate HazardMimicCrate { get; private set; }
        public Hazard_SurgicalBotch HazardSurgicalBotch { get; private set; }
        public Hazard_WeaponBurst HazardWeaponBurst { get; private set; }
        public HiddenStat_Unseen HiddenStatUnseen { get; private set; }
        public Item_AICoreData ItemAICoreData { get; private set; }
        public Item_AmmoTypes ItemAmmoTypes { get; private set; }
        /// <summary>Session expedition combat / encounter feedback lines for the HUD.</summary>
        public ExpeditionEncounterLog ExpeditionEncounterLog { get; private set; }
        public Item_Ammonia ItemAmmonia { get; private set; }
        public Item_Amphetamines ItemAmphetamines { get; private set; }
        public Item_AshGhillie ItemAshGhillie { get; private set; }
        public Item_AutoDoc ItemAutoDoc { get; private set; }
        public Item_BioPlastic ItemBioPlastic { get; private set; }
        public Item_BloodBag ItemBloodBag { get; private set; }
        public Item_BoneSaw ItemBoneSaw { get; private set; }
        public Item_C4 ItemC4 { get; private set; }
        public Item_Caltrops ItemCaltrops { get; private set; }
        public Item_CarrierBird ItemCarrierBird { get; private set; }
        public Item_ChildsDrawing ItemChildsDrawing { get; private set; }
        public Item_Cigarettes ItemCigarettes { get; private set; }
        public Item_ClimbingGear ItemClimbingGear { get; private set; }
        public Item_Decoy ItemDecoy { get; private set; }
        public Item_DogTags ItemDogTags { get; private set; }
        public Item_EMPGrenade ItemEMPGrenade { get; private set; }
        public Item_EncryptedDrive ItemEncryptedDrive { get; private set; }
        public Item_EpiPen ItemEpiPen { get; private set; }
        public Item_Exosuit ItemExosuit { get; private set; }
        public Item_FaradayPack ItemFaradayPack { get; private set; }
        public Item_ForeignBook ItemForeignBook { get; private set; }
        public Item_GeigerCalibrator ItemGeigerCalibrator { get; private set; }
        public GlowingMushroomSystem ItemGlowingMushroom { get; private set; }
        public Item_GoldBars ItemGoldBars { get; private set; }
        public Item_Guitar ItemGuitar { get; private set; }
        public Item_Heirloom ItemHeirloom { get; private set; }
        public Item_IBeam ItemIBeam { get; private set; }
        public Item_ImpureIodine ItemImpureIodine { get; private set; }
        public Item_JuggernautArmor ItemJuggernautArmor { get; private set; }
        public Item_KevlarVest ItemKevlarVest { get; private set; }
        public Item_Keycards ItemKeycards { get; private set; }
        public Item_Landmine ItemLandmine { get; private set; }
        public Item_LeadApron ItemLeadApron { get; private set; }
        public Item_LiquidStitches ItemLiquidStitches { get; private set; }
        public Item_Maggots ItemMaggots { get; private set; }
        public Item_MilGasMask ItemMilGasMask { get; private set; }
        public Item_MutantGland ItemMutantGland { get; private set; }
        public Item_Nanites ItemNanites { get; private set; }
        public Item_NightVision ItemNightVision { get; private set; }
        public Item_PackMule ItemPackMule { get; private set; }
        public Item_PasswordNote ItemPasswordNote { get; private set; }
        public Item_PhotoAlbum ItemPhotoAlbum { get; private set; }
        public Item_PotassiumIodide ItemPotassiumIodide { get; private set; }
        public Item_PresidentialSeal ItemPresidentialSeal { get; private set; }
        public Item_PrussianBlue ItemPrussianBlue { get; private set; }
        public Item_RTGBattery ItemRTGBattery { get; private set; }
        public Item_SeedLedger ItemSeedLedger { get; private set; }
        public Item_ShockCollar ItemShockCollar { get; private set; }
        public Item_Snowshoes ItemSnowshoes { get; private set; }
        public Item_SurgicalTubing ItemSurgicalTubing { get; private set; }
        public Item_TearGas ItemTearGas { get; private set; }
        public Item_TeddyBear ItemTeddyBear { get; private set; }
        public Item_TrashHazmat ItemTrashHazmat { get; private set; }
        public Item_UndeliveredMail ItemUndeliveredMail { get; private set; }
        public Item_VacuumTubes ItemVacuumTubes { get; private set; }
        public Item_VinylCollection ItemVinylCollection { get; private set; }
        public Item_Vitamins ItemVitamins { get; private set; }
        public Item_WalkieTalkie ItemWalkieTalkie { get; private set; }
        public Item_WastelandSoap ItemWastelandSoap { get; private set; }
        public Item_WaterTabs ItemWaterTabs { get; private set; }
        public Item_WeldingGoggles ItemWeldingGoggles { get; private set; }
        public Item_WristDosimeter ItemWristDosimeter { get; private set; }
        public Location_Arcade LocationArcade { get; private set; }
        public Location_SlaveMarket LocationSlaveMarket { get; private set; }
        public Location_StrandedYacht LocationStrandedYacht { get; private set; }
        public AquiferSystem MapAquifer { get; private set; }
        public AshDriftSystem AshDriftSystem { get; private set; }
        public BurnWardSystem BurnWardSystem { get; private set; }
        public CognitiveDecaySystem CognitiveDecaySystem { get; private set; }
        public LightningStrikesSystem LightningStrikesSystem { get; private set; }
        public LocationStateRuinSystem LocationStateRuinSystem { get; private set; }
        public MobileCampSystem MobileCampSystem { get; private set; }
        public MoralDilemmaSystem MoralDilemmaSystem { get; private set; }
        public NeedleSterilizationSystem NeedleSterilizationSystem { get; private set; }
        public NightScavengeSystem NightScavengeSystem { get; private set; }
        public ProstheticCraftingSystem ProstheticCraftingSystem { get; private set; }
        public SeismicVentsSystem SeismicVentsSystem { get; private set; }
        public SevereFrostbiteSystem SevereFrostbiteSystem { get; private set; }
        public TetanusAfflictionSystem TetanusAfflictionSystem { get; private set; }
        public ToothDecaySystem ToothDecaySystem { get; private set; }
        public VehicleStrandingSystem VehicleStrandingSystem { get; private set; }
        public VehicleSystem VehicleSystem { get; private set; }
        public VisionLossSystem VisionLossSystem { get; private set; }
        public VisitorRNGSystem VisitorRNGSystem { get; private set; }

        // ── Protocol Zero expansion systems ──────────────────────────
        public ThermalGridSystem ThermalGrid { get; private set; }
        public AshTideScheduler AshTideScheduler { get; private set; }
        public AtmosphereToxicitySystem AtmosphereToxicity { get; private set; }
        public ConvoyLogisticsSystem ConvoyLogistics { get; private set; }

        // ── Expansion II Addendum: Black Aquifer systems ──────────────
        public HydrostaticPressureSystem HydrostaticPressure { get; private set; }
        public TunnelingAndStructuralStress TunnelingStress { get; private set; }
        public MyceliumNetworkSystem MyceliumNetwork { get; private set; }

        // ── Expansion III: Dead Hand systems ──────────────────────────
        public UXOFieldSystem UXOField { get; private set; }
        public Encounters.AutomatedThreatSystem AutomatedThreats { get; private set; }
        public Shelter.ElectromagneticDecaySystem ElectromagneticDecay { get; private set; }

        public NPC_AddictsPassive NPCAddictsPassive { get; private set; }
        public NPC_AggroScavengers NPCAggroScavengers { get; private set; }
        public NPC_AggroTrader NPCAggroTrader { get; private set; }
        public NPC_Bandits NPCBandits { get; private set; }
        public NPC_BlackOps NPCBlackOps { get; private set; }
        public NPC_Broker NPCBroker { get; private set; }
        public NPC_Cannibals NPCCannibals { get; private set; }
        public NPC_ChemScientists NPCChemScientists { get; private set; }
        public NPC_CityResidents NPCCityResidents { get; private set; }
        public NPC_Collaborators NPCCollaborators { get; private set; }
        public NPC_Conscripts NPCConscripts { get; private set; }
        public NPC_DesperateFamily NPCDesperateFamily { get; private set; }
        public NPC_DrunksAggro NPCDrunksAggro { get; private set; }
        public NPC_Homeless NPCHomeless { get; private set; }
        public NPC_LonePsychopath NPCLonePsychopath { get; private set; }
        public NPC_Looters NPCLooters { get; private set; }
        public NPC_Mercenaries NPCMercenaries { get; private set; }
        public NPC_MilitaryPatrol NPCMilitaryPatrol { get; private set; }
        public NPC_PassiveScavengers NPCPassiveScavengers { get; private set; }
        public NPC_PassiveTrader NPCPassiveTrader { get; private set; }
        public NPC_PsychopathPair NPCPsychopathPair { get; private set; }
        public NPC_RebelMilitia NPCRebelMilitia { get; private set; }
        public NPC_RebelModerates NPCRebelModerates { get; private set; }
        public NPC_RebelSnipers NPCRebelSnipers { get; private set; }
        public NPC_RebelZealots NPCRebelZealots { get; private set; }
        public NPC_Slavers NPCSlavers { get; private set; }
        public NPC_SpecOps NPCSpecOps { get; private set; }
        public NPC_Survivalists NPCSurvivalists { get; private set; }
        public NPC_TaxCollector NPCTaxCollector { get; private set; }
        public NPC_Terrorists NPCTerrorists { get; private set; }
        public NPC_TheNegotiator NPCTheNegotiator { get; private set; }
        public NPC_TheOld NPCTheOld { get; private set; }
        public NPC_TheParents NPCTheParents { get; private set; }
        public NPC_TravelingCouple NPCTravelingCouple { get; private set; }
        public Node_AutomatedArmory NodeAutomatedArmory { get; private set; }
        public Node_GhostShip NodeGhostShip { get; private set; }
        public Node_MutantHive NodeMutantHive { get; private set; }
        public Node_PlayerBank NodePlayerBank { get; private set; }
        public Node_Sector7G NodeSector7G { get; private set; }
        public Node_SporeHive NodeSporeHive { get; private set; }
        public Pet_FeralCat PetFeralCat { get; private set; }
        public Project_BioReactor ProjectBioReactor { get; private set; }
        public Project_DeepWell ProjectDeepWell { get; private set; }
        public Project_Elevator ProjectElevator { get; private set; }
        public Project_Minecart ProjectMinecart { get; private set; }
        public Project_RadioArray ProjectRadioArray { get; private set; }
        public Project_SurfaceDome ProjectSurfaceDome { get; private set; }
        public ShelterEvent_CaravanAmbush ShelterEventCaravanAmbush { get; private set; }
        public ShelterEvent_FalseCure ShelterEventFalseCure { get; private set; }
        public ShelterEvent_Ransom ShelterEventRansom { get; private set; }
        public ShelterEvent_Refugees ShelterEventRefugees { get; private set; }
        public ShelterEvent_TheMirror ShelterEventTheMirror { get; private set; }
        public ShelterEvent_Tribute ShelterEventTribute { get; private set; }
        public Skirmish_Bandit_vs_Terror SkirmishBandit_vs_Terror { get; private set; }
        public Skirmish_Mil_vs_Rebel SkirmishMil_vs_Rebel { get; private set; }
        public Skirmish_Mil_vs_Terror SkirmishMil_vs_Terror { get; private set; }
        public Skirmish_Rebel_vs_Bandit SkirmishRebel_vs_Bandit { get; private set; }
        public Skirmish_Rebel_vs_Terror SkirmishRebel_vs_Terror { get; private set; }
        public Trader_PlagueConvoy TraderPlagueConvoy { get; private set; }
        public Trait_Anthropophobia TraitAnthropophobia { get; private set; }
        public ClairvoyantSystem TraitClairvoyant { get; private set; }
        public Trait_GenerationalTrauma TraitGenerationalTrauma { get; private set; }
        public Trait_InheritedGenetics TraitInheritedGenetics { get; private set; }
        public Trait_Matriarch TraitMatriarch { get; private set; }
        public Trait_PTSD TraitPTSD { get; private set; }
        public UIEvent_BlurredVision UIEventBlurredVision { get; private set; }
        public UIEvent_CorruptionScare UIEventCorruptionScare { get; private set; }
        public UIEvent_FalseInventory UIEventFalseInventory { get; private set; }
        public UIEvent_GhostRadio UIEventGhostRadio { get; private set; }
        public UIEvent_Hacking UIEventHacking { get; private set; }
        public UIEvent_LowPower UIEventLowPower { get; private set; }
        public UIEvent_MapRot UIEventMapRot { get; private set; }
        public PhantomBlipSystem UIEventPhantomBlip { get; private set; }
        public Vehicle_ArmoredTruck VehicleArmoredTruck { get; private set; }
        public Vehicle_Motorcycle VehicleMotorcycle { get; private set; }
        public Vehicle_Rowboat VehicleRowboat { get; private set; }
        public Visitor_AbandonedState VisitorAbandonedState { get; private set; }
        public Visitor_ChurchHostile VisitorChurchHostile { get; private set; }
        public Visitor_ChurchSanctuary VisitorChurchSanctuary { get; private set; }
        public Visitor_ExplodedState VisitorExplodedState { get; private set; }
        public Visitor_FleeingHorde VisitorFleeingHorde { get; private set; }
        public Visitor_HospitalPatients VisitorHospitalPatients { get; private set; }
        public Visitor_HospitalStaff VisitorHospitalStaff { get; private set; }
        public Visitor_MilTrainingYard VisitorMilTrainingYard { get; private set; }
        public Visitor_QuestFaction VisitorQuestFaction { get; private set; }
        public Visitor_RebelTrainingYard VisitorRebelTrainingYard { get; private set; }
        public Weapon_Chainsaw WeaponChainsaw { get; private set; }
        public Weapon_Flamethrower WeaponFlamethrower { get; private set; }
        public Weapon_HMG WeaponHMG { get; private set; }
        public Weapon_RPG WeaponRPG { get; private set; }
        public WorldEvent_Deforestation WorldEventDeforestation { get; private set; }
        public WorldEvent_FinalWinter WorldEventFinalWinter { get; private set; }
        public WorldEvent_Fissure WorldEventFissure { get; private set; }
        public WorldEvent_GreatFamine WorldEventGreatFamine { get; private set; }
        public WorldEvent_Megafauna WorldEventMegafauna { get; private set; }

        // -----------------------------------------------------------------
        // Radio intercept HUD strip
        // -----------------------------------------------------------------







        // -----------------------------------------------------------------
        // Post-repel parley offer (trade modal)
        // -----------------------------------------------------------------










        // ─────────────────────────────────────────────────────────────────
        // Prompt #9 — Child Found: create child survivor on "take in" choice.
        // ─────────────────────────────────────────────────────────────────


        // ─────────────────────────────────────────────────────────────────
        // Prompt #7 — Addiction: panic-destroy items during withdrawal.
        // ─────────────────────────────────────────────────────────────────



        // ─────────────────────────────────────────────────────────────────
        // Prompt #5 — Default diary factory for when no authored assets exist.
        // ─────────────────────────────────────────────────────────────────

        private struct DiarySeed
        {
            public string Id;
            public string Title;
            public string Text;
            public string Author;
            public string RoomId;
            public string WarnsSystem;
            public int Page;
            public int Total;
        }


    }
}
