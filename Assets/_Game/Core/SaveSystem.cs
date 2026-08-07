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
using AtomicWar._Game.AI; // HallucinationSystem (audit wiring fix)
using AtomicWar._Game.Crafting; // CraftingSystem, WorkbenchSystem (audit wiring fix)

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Serializes/deserializes every save-safe system snapshot to disk (JSON)
    /// under a named slot. Coordinates with each system's serializable state so
    /// a load fully reconstructs a prior session.
    ///
    /// Forward-compatible: SaveData carries a saveVersion int and a migration
    /// stub (MigrateV1toV2) even before V2 exists.
    ///
    /// Corrupt-save detection: each file stores a SHA-256 checksum computed over
    /// the JSON body (with an empty checksum placeholder). A mismatch on load
    /// triggers a graceful fallback message instead of crashing.
    ///
    /// Lifecycle (audit H-2): SaveSystem is plain C# and subscribes to
    /// GameState.OnPhaseChanged in its constructor. Callers that replace
    /// SaveSystem (e.g. the test fixtures, or a future "new game" flow)
    /// must call <see cref="Dispose"/> on the old instance to release the
    /// subscription. The companion <see cref="ExpeditionSystem.UnsubscribeAll"/>
    /// follows the same pattern.
    /// </summary>
    public partial class SaveSystem : IDisposable
    {
        /// <summary>
        /// V1: initial format. V2: FlashpointChoreographerSave added.
        /// V3: ISaveable subsystem registry (H-4 refactor) — SubsystemSaveIds /
        ///    SubsystemSaveJsons paired lists added; positional fields preserved
        ///    for backward compat. Migration V2→V3 is a no-op (existing data stays
        ///    in positional fields; new saves also write the paired lists).
        /// </summary>
        public const int CurrentSaveVersion = 3;

        /// <summary>Core systems required to construct a SaveSystem.</summary>
        public sealed class CoreDeps
        {
            public GameState GameState;
            public WeatherSystem WeatherSystem;
            public TemperatureSystem TemperatureSystem;
            public NeedsSystem NeedsSystem;
            public RadiationSystem RadiationSystem;
            public Shelter.Shelter Shelter;
            public Func<IReadOnlyList<Survivor>> GetSurvivors;
            public Func<string, ItemDefinition> ItemLookup;
            public Func<string, ShelterModule> ModuleLookup;
            public string SavesDir;
        }

        private readonly GameState _gameState;
        private readonly WeatherSystem _weatherSystem;
        private readonly TemperatureSystem _temperatureSystem;
        private readonly NeedsSystem _needsSystem;
        private readonly RadiationSystem _radiationSystem;
        private readonly Shelter.Shelter _shelter;
        private readonly Func<IReadOnlyList<Survivor>> _getSurvivors;
        private readonly Func<string, ItemDefinition> _itemLookup;
        private readonly Func<string, ShelterModule> _moduleLookup;
        private readonly string _savesDir;

        // Optional — injected after construction to keep constructor signature stable
        private PhotoperiodSystem _photoPeriodSystem;
        private RadiationKnowledgeMap _knowledgeMap;
        private Inventory.Inventory _inventory;
        private ExpeditionSystem _expeditionSystem;
        private MedicalSystem _medicalSystem;
        private BloodTransfusionSystem _bloodTransfusion;
        private AmputationSystem _amputationSystem;
        private ScurvySystem _scurvySystem;
        private RadiationMutagenesisSystem _mutagenesisSystem;
        private DynamicEconomySystem _economySystem;
        private WorldPhaseSystem _worldPhaseSystem;
        private PowerNetwork _powerNetwork;
        private WaterStorage _waterStorage;
        private GeneratedMap _generatedMap;
        private HatchDefenseSystem _hatchDefense;
        private FactionRadioInterceptSystem _factionRadioIntercepts;
        private AtomicWar._Game.Survivors.MentalBreakSystem _mentalBreakSystem;
        private AtomicWar._Game.Survivors.PhantomIntruderSystem _phantomIntruderSystem;
        private JournalSystem _journalSystem;
        private VictoryProjectManager _victoryProject;
        private EventRunner _eventRunner;
        private SuspicionTracker _suspicionTracker;
        private HatchEntrapmentSystem _hatchEntrapment;
        private ShelterAtmosphereSystem _atmosphereSystem;
        private CorpseManagementSystem _corpseSystem;
        private PantryContaminationSystem _pantrySystem;
        private SabotagedCacheSystem _sabotagedCaches;
        private ShiftingHotspotSystem _shiftingHotspots;
        private FactionRaidPlanSystem _factionRaidPlans;
        private DebtCollectorSystem _debtCollector;
        private GhostStationSystem _ghostStations;
        private ChildDependentSystem _childSystem;
        private StructuralIntegritySystem _structuralIntegrity;
        private WasteSystem _wasteSystem;
        private VerminSystem _verminSystem;
        private JuryRigSystem _juryRigSystem;
        private FreezePipeSystem _freezePipeSystem;
        private CartographySystem _cartographySystem;
        private BicycleSystem _bicycleSystem;
        private FloodedNodeSystem _floodedNodeSystem;
        private TrackerSystem _trackerSystem;
        private DeadDropSystem _deadDropSystem;
        private HostageSystem _hostageSystem;
        private PropagandaSystem _propagandaSystem;
        private DeserterSystem _deserterSystem;
        private WeatherScapegoatSystem _scapegoatSystem;
        private LaborCampSystem _laborCampSystem;
        private CultMoralDisgustSystem _cultMoralSystem;
        private MutatedEcosystemSystem _ecosystemSystem;
        private HouseToBunkerSystem _houseToBunkerSystem;
        private LocationQuestSystem _locationQuestSystem;
        private ExcavationSystem _excavationSystem;
        private RoomFloodingSystem _floodingSystem;
        private HiddenStorageSystem _hiddenStorageSystem;
        private CeilingCollapseSystem _ceilingCollapseSystem;
        private PerimeterTrapSystem _perimeterTrapSystem;
        private TunnelingSystem _tunnelingSystem;
        private HatchVisibilitySystem _hatchVisibilitySystem;
        private EscapeHatchSystem _escapeHatchSystem;
        private MaterialShieldingSystem _materialShieldingSystem;
        private AirlockSystem _airlockSystem;
        private NoiseSystem _noiseSystem;
        private ClothingDegradationSystem _clothingSystem;
        private ResilienceSystem _resilienceSystem;
        private CompostSystem _compostSystem;
        private ScrapWeaponSystem _scrapWeaponSystem;
        private SterilizationSystem _sterilizationSystem;
        private ChelationSystem _chelationSystem;
        private WindTurbineSystem _windTurbineSystem;
        private AntibioticResistanceSystem _antibioticResistSystem;
        private InternalHaulingSystem _haulingSystem;
        private WeaponMaintenanceSystem _weaponMaintenanceSystem;
        private RoomAestheticsSystem _aestheticsSystem;
        private HamRadioSystem _hamRadioSystem;
        private TriageBoardSystem _triageSystem;
        private PolypharmacySystem _polypharmacySystem;
        private LifeboatTransmissionSystem _lifeboat;
        private SkillProgressionSystem _skillProgression;
        private CombatPerkSystem _combatPerkSystem;
        private SurvivalPerkSystem _survivalPerkSystem;
        private ShelterPerkSystem _shelterPerkSystem;
        private MedicalPerkSystem _medicalPerkSystem;
        private ExpeditionPerkSystem _expeditionPerkSystem;
        private SocialPerkSystem _socialPerkSystem;
        private PersonalQuestSystem _personalQuestSystem;
        private HallucinationSystem _hallucinationSystem;
        private InternalLockSystem _internalLockSystem;
        private SurvivorDiariesSystem _survivorDiariesSystem;
        private RadioBroadcastSystem _radioBroadcastSystem;
        private CraftingSystem _craftingSystem;
        private WorkbenchSystem _workbenchSystem;
        private LocationScavengingSystem _scavengingSystem;
        private PetSystem _petSystem;
        private FuelDecaySystem _fuelDecaySystem;
        private RadioTunerSystem _radioTunerSystem;
        private AddictionSystem _addictionSystem;
        private BloodToxicitySystem _bloodToxicitySystem;
        private GraftRejectionSystem _graftRejectionSystem;
        private PheromoneMaskingSystem _pheromoneMaskingSystem;
        private System_Tolerance _chemToleranceSystem;
        private LastWillSystem _lastWillSystem;
        private System_LegacyStart _legacyStartSystem;
        private System_BloodTypes _bloodTypesSystem;
        private System_EpilogueStats _epilogueStatsSystem;
        private System_Gossip _gossipSystem;
        private System_AdaptiveWarlords _adaptiveWarlordsSystem;
        private System_BilgePumps _bilgePumpsSystem;
        private System_CarrionBirds _carrionBirdsSystem;
        private System_LogicGates _logicGatesSystem;
        private System_ModLoader _modLoaderSystem;
        private System_TwitchAPI _twitchApiSystem;
        private DiseaseSystem_Expansion _diseaseExpansionSystem;
        private Dynamic_Scapegoat _dynamicScapegoatSystem;
        private Mode_IronMan _ironManMode;
        private NPC_Android _androidNpcSystem;
        private Role_Sheriff _sheriffRoleSystem;
        private UI_ScenarioGen _scenarioGenSystem;
        private UI_SpeedrunTimer _speedrunTimerSystem;
        private Victory_TrueEnding _trueEndingSystem;
        private Victory_Airlift _victoryAirliftSystem;
        private Victory_Ascendancy _victoryAscendancySystem;
        private Victory_BuriedAlive _victoryBuriedAliveSystem;
        private Victory_CannibalKing _victoryCannibalKingSystem;
        private Victory_Defection _victoryDefectionSystem;
        private Victory_Icebreaker _victoryIcebreakerSystem;
        private Victory_LoneSurvivor _victoryLoneSurvivorSystem;
        private Victory_MAD _victoryMadSystem;
        private Victory_Migration _victoryMigrationSystem;
        private Victory_TheBroadcast _victoryTheBroadcastSystem;
        private Victory_TheCure _victoryTheCureSystem;
        private Victory_TheMartian _victoryTheMartianSystem;
        private Victory_UndergroundCity _victoryUndergroundCitySystem;
        private Victory_Unifier _victoryUnifierSystem;
        private MapHazard_AcidGeyser _mapHazardAcidGeyser;
        private MapHazard_Ashlanche _mapHazardAshlanche;
        private MapHazard_BiometricDoor _mapHazardBiometricDoor;
        private MapHazard_CraterWall _mapHazardCraterWall;
        private MapHazard_Crevice _mapHazardCrevice;
        private MapHazard_FlammableGas _mapHazardFlammableGas;
        private MapHazard_GasPockets _mapHazardGasPockets;
        private MapHazard_MagneticAnomaly _mapHazardMagneticAnomaly;
        private MapHazard_SinkholeCollapse _mapHazardSinkholeCollapse;
        private MapHazard_VenusTrap _mapHazardVenusTrap;
        private MapAnomaly_AshDunes _mapAnomalyAshDunes;
        private MapAnomaly_BoilingLake _mapAnomalyBoilingLake;
        private MapAnomaly_Cherenkov _mapAnomalyCherenkov;
        private MapAnomaly_DogDen _mapAnomalyDogDen;
        private MapAnomaly_DontLook _mapAnomalyDontLook;
        private MapAnomaly_DryCoral _mapAnomalyDryCoral;
        private MapAnomaly_FloodedSubway _mapAnomalyFloodedSubway;
        private MapAnomaly_GlassCrater _mapAnomalyGlassCrater;
        private MapAnomaly_MassGrave _mapAnomalyMassGrave;
        private MapAnomaly_Mirage _mapAnomalyMirage;
        private MapAnomaly_PetrifiedForest _mapAnomalyPetrifiedForest;
        private MapAnomaly_QuietZone _mapAnomalyQuietZone;
        private MapAnomaly_RustedTank _mapAnomalyRustedTank;
        private MapAnomaly_ServerFarm _mapAnomalyServerFarm;
        private MapAnomaly_Sinkhole _mapAnomalySinkhole;
        private MapAnomaly_TangledDrop _mapAnomalyTangledDrop;
        private MapAnomaly_TireFire _mapAnomalyTireFire;
        private MapAnomaly_UXO_Nuke _mapAnomalyUxoNuke;
        private Biome_AshSwamp _biomeAshSwamp;
        private Biome_GlassDesert _biomeGlassDesert;
        private Biome_HighwayTunnel _biomeHighwayTunnel;
        private Biome_SaltFlats _biomeSaltFlats;
        private Biome_SkyscraperTops _biomeSkyscraperTops;
        private Biome_Suburbs _biomeSuburbs;
        private Weather_AcidSnow _weatherAcidSnow;
        private Weather_BioFog _weatherBioFog;
        private Weather_BlackSnow _weatherBlackSnow;
        private Weather_BloodRain _weatherBloodRain;
        private Weather_DeadWind _weatherDeadWind;
        private Weather_DeepFreeze _weatherDeepFreeze;
        private Weather_DustDevil _weatherDustDevil;
        private Weather_EMPStorm _weatherEmpStorm;
        private Weather_FalseSpring _weatherFalseSpring;
        private Weather_GlassStorm _weatherGlassStorm;
        private Weather_OzoneHole _weatherOzoneHole;
        private Weather_RadHail _weatherRadHail;
        private Weather_SilentSpring _weatherSilentSpring;
        private Weather_SolarFlare _weatherSolarFlare;
        private Weather_StaticCharge _weatherStaticCharge;
        private Encounter_Amalgamation _encounterAmalgamation;
        private BurrowersSystem _encounterBurrowers;
        private Encounter_FloodedMaze _encounterFloodedMaze;
        private Encounter_GlowingDead _encounterGlowingDead;
        private Encounter_GlowingStag _encounterGlowingStag;
        private Encounter_HitAndRun _encounterHitAndRun;
        private Encounter_Leeches _encounterLeeches;
        private Encounter_Mirelurker _encounterMirelurker;
        private Encounter_PressurePlate _encounterPressurePlate;
        private Encounter_RiverPirates _encounterRiverPirates;
        private Encounter_Roadblock _encounterRoadblock;
        private Encounter_RobotDog _encounterRobotDog;
        private Encounter_SleepingCamp _encounterSleepingCamp;
        private Encounter_TripwireMaze _encounterTripwireMaze;
        private Encounter_WarlordTank _encounterWarlordTank;
        private ShelterModule_AcidTrap _shelterModuleAcidTrap;
        private ShelterModule_Autodoc _shelterModuleAutodoc;
        private ShelterModule_CCTV _shelterModuleCctv;
        private ShelterModule_Classroom _shelterModuleClassroom;
        private ShelterModule_Confessional _shelterModuleConfessional;
        private ShelterModule_Conveyor _shelterModuleConveyor;
        private ShelterModule_DaylightSensor _shelterModuleDaylightSensor;
        private ShelterModule_DroneStation _shelterModuleDroneStation;
        private ShelterModule_HoloEmitter _shelterModuleHoloEmitter;
        private ShelterModule_InsectFarm _shelterModuleInsectFarm;
        private ShelterModule_Lathe _shelterModuleLathe;
        private ShelterModule_Mortar _shelterModuleMortar;
        private ShelterModule_PanicButton _shelterModulePanicButton;
        private ShelterModule_Pitfall _shelterModulePitfall;
        private ShelterModule_Reloader _shelterModuleReloader;
        private ShelterModule_Sorter _shelterModuleSorter;
        private ShelterModule_Thermostat _shelterModuleThermostat;
        private ShelterModule_WasteChute _shelterModuleWasteChute;
        private ShelterModule_Autopsy _shelterModuleAutopsy;
        private ShelterModule_BatteryBank _shelterModuleBatteryBank;
        private ShelterModule_BioLatrine _shelterModuleBioLatrine;
        private ShelterModule_ChoreBoard _shelterModuleChoreBoard;
        private ShelterModule_DeadManSwitch _shelterModuleDeadManSwitch;
        private ShelterModule_DeconShower _shelterModuleDeconShower;
        private ShelterModule_Dialysis _shelterModuleDialysis;
        private ShelterModule_DistressBeacon _shelterModuleDistressBeacon;
        private ShelterModule_DronePad _shelterModuleDronePad;
        private ShelterModule_Garage _shelterModuleGarage;
        private ShelterModule_GunRack _shelterModuleGunRack;
        private ShelterModule_Hammock _shelterModuleHammock;
        private ShelterModule_HandCrank _shelterModuleHandCrank;
        private ShelterModule_HotShower _shelterModuleHotShower;
        private ShelterModule_Incinerator _shelterModuleIncinerator;
        private MagmaTapSystem _shelterModuleMagmaTap;
        private ShelterModule_MotionSensor _shelterModuleMotionSensor;
        private PanicRoomSystem _shelterModulePanicRoom;
        private ShelterModule_PrintingPress _shelterModulePrintingPress;
        private ShelterModule_PunchingBag _shelterModulePunchingBag;
        private ShelterModule_RainBarrel _shelterModuleRainBarrel;
        private ShelterModule_RecordPlayer _shelterModuleRecordPlayer;
        private ShelterModule_Sprinklers _shelterModuleSprinklers;
        private ThumperSystem _shelterModuleThumper;
        private ShelterModule_TreadmillGen _shelterModuleTreadmillGen;
        private ShelterModule_Turret _shelterModuleTurret;
        private ShelterModule_VaultDoor _shelterModuleVaultDoor;
        private ShelterModule_WoodStove _shelterModuleWoodStove;
        private Event_Brawl _eventBrawl;
        private Event_ComingOfAge _eventComingOfAge;
        private Event_CultBlessing _eventCultBlessing;
        private Event_CultInitiation _eventCultInitiation;
        private Event_CultOfAI _eventCultOfAi;
        private Event_EMPCascade _eventEmpCascade;
        private Event_FeralRescue _eventFeralRescue;
        private Event_FoundDiary _eventFoundDiary;
        private Event_GriefCascade _eventGriefCascade;
        private Event_HungerStrike _eventHungerStrike;
        private Event_NodeCollapse _eventNodeCollapse;
        private Event_RansomNote _eventRansomNote;
        private Event_Schism _eventSchism;
        private Event_SecretSociety _eventSecretSociety;
        private Event_SiblingFeud _eventSiblingFeud;
        private Event_SpontaneousMurder _eventSpontaneousMurder;
        private Event_TeenRebellion _eventTeenRebellion;
        private Event_WitchHunt _eventWitchHunt;
        private Siege_Artillery _siegeArtillerySystem;
        private Siege_Biowarfare _siegeBiowarfareSystem;
        private Siege_Blockade _siegeBlockadeSystem;
        private Siege_HostageShield _siegeHostageShieldSystem;
        private Siege_NightRaid _siegeNightRaidSystem;
        private Siege_Sappers _siegeSappersSystem;
        private Siege_SmokeOut _siegeSmokeOutSystem;
        private Siege_VehicleRam _siegeVehicleRamSystem;
        private RiverNodeSystem _riverNodeSystem;
        // Choreographer is injected as capture/restore delegates rather than a
        // direct reference so Core stays agnostic of the Flashpoint module.
        private Func<FlashpointChoreographerSave> _captureChoreographer;
        private Action<FlashpointChoreographerSave> _restoreChoreographer;
        /// <summary>Optional hook run just before CaptureSnapshot (e.g. HUD → system sync).</summary>
        private Action _preCaptureHook;

        /// <summary>
        /// H-4: Generic saveable registry. Each system that implements ISaveable is
        /// auto-registered via Set* methods. On save/load, the SaveSystem iterates
        /// this list to capture/restore state generically rather than hard-coding
        /// each system. This prevents the B-6 class of bug (missing save fields).
        /// </summary>
        private readonly List<ISaveable> _saveables = new List<ISaveable>();

        /// <summary>Number of registered saveables (for tests).</summary>
        public int SaveableCount => _saveables.Count;

        /// <summary>
        /// AUDIT-004 / P1: When true, any ISaveable deserialize/restore failure aborts
        /// the whole Load (exception propagates; <see cref="Load"/> returns false).
        /// When false, best-effort restore continues after logging so a single broken
        /// subsystem does not reject the entire save.
        /// Production players default false; Editor / Development builds default true
        /// via <see cref="DefaultFailFastRestoreForEnvironment"/>.
        /// </summary>
        public bool FailFastRestore { get; set; }

        /// <summary>
        /// P1: Policy for new SaveSystem instances.
        /// True under UNITY_EDITOR or DEVELOPMENT_BUILD (includes game-ci batchmode
        /// EditMode tests). False in release player builds so a single bad subsystem
        /// snapshot does not brick a player's load mid-campaign.
        /// Override per-instance with <see cref="FailFastRestore"/> after construction.
        /// </summary>
        public static bool DefaultFailFastRestoreForEnvironment()
        {
#if UNITY_EDITOR || DEVELOPMENT_BUILD
            return true;
#else
            return false;
#endif
        }

        private readonly Dictionary<string, bool> _worldFlags = new Dictionary<string, bool>();

        public SaveSystem(CoreDeps deps)
        {
            if (deps == null) throw new ArgumentNullException(nameof(deps));
            _gameState = deps.GameState ?? throw new ArgumentNullException(nameof(deps.GameState));
            _weatherSystem = deps.WeatherSystem;
            _temperatureSystem = deps.TemperatureSystem;
            _needsSystem = deps.NeedsSystem;
            _radiationSystem = deps.RadiationSystem;
            _shelter = deps.Shelter;
            _getSurvivors = deps.GetSurvivors;
            _itemLookup = deps.ItemLookup;
            _moduleLookup = deps.ModuleLookup;
            _savesDir = deps.SavesDir ?? Path.Combine(Application.persistentDataPath, "saves");

            if (_gameState != null)
            {
                _gameState.OnPhaseChanged += OnPhaseChanged;
            }
        }

        /// <summary>Factory that mirrors the CoreDeps constructor (single parameter object).</summary>
        public static SaveSystem Create(CoreDeps deps) => new SaveSystem(deps);


        /// <summary>
        /// H-4: Register a saveable system. After registration, CaptureState() and
        /// RestoreState() are called automatically on every save/load. This is the
        /// canonical way to make a system save-safe — no need to add a positional
        /// field, a Set* setter, a CaptureSnapshot block, and a RestoreFromSnapshot
        /// block. The diff for a new system collapses to 1 line.
        ///
        /// Registration is idempotent: calling twice for the same SaveId replaces
        /// the previous registration (last-one-wins). Unregister by calling with null.
        /// </summary>
        public void Register(ISaveable saveable)
        {
            if (saveable == null) return;
            // Replace if already registered (idempotent by SaveId).
            for (int i = 0; i < _saveables.Count; i++)
            {
                if (_saveables[i].SaveId == saveable.SaveId)
                {
                    _saveables[i] = saveable;
                    return;
                }
            }
            _saveables.Add(saveable);
        }

        /// <summary>
        /// H-4: Helper — assign a system reference AND auto-register it via an
        /// ISaveable adapter. The adapter wraps the system's existing
        /// CaptureState() / RestoreState() methods, avoiding an asmdef cycle
        /// (ISaveable lives in Core; Simulation/Shelter/Environment assemblies
        /// can't reference Core because Core already references them).
        ///
        /// After this call, the system is automatically captured and restored
        /// on every save/load without needing 4 places in SaveSystem.
        /// </summary>
        private void RegisterSystem<T>(ref T field, T system, string saveId,
            Func<object> capture, Action<object> restore) where T : class
        {
            field = system;
            if (system != null)
            {
                var adapter = new SaveableAdapter(saveId, capture, restore);
                Register(adapter);
            }
        }

        /// <summary>
        /// H-4: Adapter that wraps a system's CaptureState/RestoreState into
        /// the ISaveable interface. This avoids requiring every system assembly
        /// to reference Core (which would create a cycle since Core references
        /// those assemblies). The adapter is created in each Set* method.
        /// </summary>
        private sealed class SaveableAdapter : ISaveable
        {
            private readonly Func<object> _capture;
            private readonly Action<object> _restore;
            public string SaveId { get; }

            public SaveableAdapter(string saveId, Func<object> capture, Action<object> restore)
            {
                SaveId = saveId ?? throw new ArgumentNullException(nameof(saveId));
                _capture = capture ?? throw new ArgumentNullException(nameof(capture));
                _restore = restore ?? throw new ArgumentNullException(nameof(restore));
            }

            public object CaptureState() => _capture();
            public void RestoreState(object state) => _restore(state);
        }





        /// <summary>Delete a save slot.</summary>
        public bool Delete(string slotId)
        {
            string path = SlotPath(slotId);
            if (!File.Exists(path)) return false;
            File.Delete(path);
            return true;
        }



        // -----------------------------------------------------------------
        // H-2: Lifecycle (IDisposable)
        // -----------------------------------------------------------------

        private bool _disposed;


        /// <summary>Set or clear a world flag (persisted across saves).</summary>
        public void SetWorldFlag(string key, bool value) => _worldFlags[key] = value;

        /// <summary>Get a world flag value (false if not set).</summary>
        public bool GetWorldFlag(string key) => _worldFlags.TryGetValue(key, out var v) && v;

        /// <summary>Read-only snapshot of all world flags.</summary>
        public IReadOnlyDictionary<string, bool> WorldFlags => _worldFlags;

        // -----------------------------------------------------------------
        // Checksum
        // -----------------------------------------------------------------



        // -----------------------------------------------------------------
        // Migration stubs
        // -----------------------------------------------------------------




        // -----------------------------------------------------------------
        // H-4: ISaveable generic capture / restore
        // -----------------------------------------------------------------







        // -----------------------------------------------------------------
        // Autosave on phase change
        // -----------------------------------------------------------------

        private void OnPhaseChanged(GamePhase phase)
        {
            if (phase == GamePhase.Running)
            {
                AutoSave();
            }
        }
    }
}
