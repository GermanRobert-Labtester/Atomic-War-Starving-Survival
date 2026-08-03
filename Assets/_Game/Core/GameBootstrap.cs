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
using AtomicWar._Game.Flashpoint;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;
using AtomicWar._Game.Economy;

namespace AtomicWar._Game.Core
{
    /// <summary>
    /// Composition root: initializes every system, owns the game loop, and wires
    /// the HUD, input, save/load, and win/lose detection. Drop on a GameObject in
    /// the main scene; no other MonoBehaviour needs to know about system wiring.
    /// </summary>
    public class GameBootstrap : MonoBehaviour
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
        public Inventory.Inventory Inventory { get; private set; }
        public CraftingSystem CraftingSystem { get; private set; }
        public WorkbenchSystem WorkbenchSystem { get; private set; }
        public UtilityAI UtilityAI { get; private set; }
        public EventRunner EventRunner { get; private set; }
        public SaveSystem SaveSystem { get; private set; }
        public LocationScavengingSystem ScavengingSystem { get; private set; }
        public ExpeditionSystem ExpeditionSystem { get; private set; }
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
        public FlashpointChoreographer FlashpointChoreographer { get; private set; }
        public MentalBreakSystem MentalBreakSystem { get; private set; }
        public HatchDilemmaPrompt HatchDilemmaPromptField { get; private set; }
        public List<Survivor> Survivors { get; private set; }
        public List<SurvivorAction> Actions { get; private set; }

        // -----------------------------------------------------------------
        // GameOver state
        // -----------------------------------------------------------------

        public bool IsGameOver { get; private set; }
        public string GameOverReason { get; private set; }

        // -----------------------------------------------------------------
        // Lifecycle
        // -----------------------------------------------------------------

        private void Awake()
        {
            InitializeSystems();
            WireHUD();
            GameState.Phase = GamePhase.Running;
        }

        private void Update()
        {
            if (GameState.Phase != GamePhase.Running) return;
            if (IsGameOver) return;

            float dt = Time.deltaTime;
            TimeSystem.Tick(dt);
            float gameHours = dt / TimeSystem.SecondsPerGameHour;

            // Day-30 Flashpoint choreography runs in real time (the flash is a
            // visual event, not a game-time event). Tick before the rest of
            // the systems so the EMP step's side effects (radiation pause,
            // weather force) are visible to the same frame's HUD push.
            FlashpointChoreographer?.Tick(dt);

            TickSystems(gameHours);
            CheckWinLose();

            // Push environment data to HUD every frame
            if (_hud != null)
            {
                string weatherName = WeatherSystem.Current.ToString();
                string seasonName = TemperatureSystem.CurrentSeason?.displayName ?? "Nuclear Winter";
                _hud.Tick(TimeSystem.CurrentDay, TimeSystem.CurrentHourFloat, weatherName, seasonName);
                _hud.OnShelterUpdated(Shelter);
            }
        }

        // -----------------------------------------------------------------
        // Initialization
        // -----------------------------------------------------------------

        private void InitializeSystems()
        {
            // Core
            GameState = new GameState();
            TimeSystem = new TimeSystem { SecondsPerGameHour = _secondsPerGameHour };

            // Environment
            WeatherSystem = new WeatherSystem(_seasonProfile, _worldSeed);
            TemperatureSystem = new TemperatureSystem(_seasonProfile, WeatherSystem);
            PhotoperiodSystem = new PhotoperiodSystem(_seasonProfile, WeatherSystem);

            // Shelter
            Shelter = new Shelter.Shelter();
            Shelter.AddModule(new ShelterModuleInstance("air_filtration", 2) { FilterHealth = 100f });
            Shelter.AddModule(new ShelterModuleInstance("radiation_shielding", 2));
            Shelter.AddModule(new ShelterModuleInstance("heater", 1) { Fuel = 50f });
            Shelter.AddModule(new ShelterModuleInstance("workbench", 1));
            // Grow-light starts installed but dry (no fuel). Player must scavenge fuel to light it.
            Shelter.AddModule(new ShelterModuleInstance("grow_light", 1) { Fuel = 0f });
            // Roof catchment starts open (player can close it to stop collecting during a storm).
            Shelter.AddModule(new ShelterModuleInstance("catchment_surface", 1) { IsEnabled = true });
            Shelter.AddModule(new ShelterModuleInstance("water_purifier", 1) { FilterHealth = 100f });
            // Sleep quarters: bed in "quarters"; diesel lives in "plant" next door (Prompt #32).
            Shelter.AddModule(new ShelterModuleInstance("bed", 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId,
                ComfortLevel = 1f,
                Capacity = 2
            });
            // Comfort station: a quiet corner of the quarters where the AI
            // is willing to spend comfort items on a broken survivor. Always
            // on (no fuel cost in the current tuning); the
            // MentalBreakComfortActionSO reads it via MedicalSystem.ComfortStationModuleId.
            Shelter.AddModule(new ShelterModuleInstance("comfort_station", 1)
            {
                RoomId = SleepQualitySystem.DefaultSleepRoomId,
                IsEnabled = true
            });
            Shelter.SetRoomsAdjacent(
                SleepQualitySystem.DefaultSleepRoomId,
                SleepQualitySystem.DefaultGeneratorRoomId);

            // Shelter power grid: finite watts, load-shedding, diesel + bicycle generators.
            // Fully-qualified type: property name PowerNetwork shadows the class.
            PowerNetwork = AtomicWar._Game.Shelter.PowerNetwork.CreateDefault(dieselFuel: 40f);
            var diesel = PowerNetwork.GetSource("diesel_generator");
            if (diesel != null)
            {
                diesel.RoomId = SleepQualitySystem.DefaultGeneratorRoomId;
            }
            // Heater/filter are installed and requested; grow light stays optional until fuel/power allow.
            PowerNetwork.SetRequested("grow_light", false);
            PowerNetwork.SetRequested("radio", false);
            PowerNetwork.SetRequested("water_purifier", true);
            PowerNetwork.ApplyToShelter(Shelter);

            // Bunker water economy: roof catchment + 3-tier purifier (Prompt #28).
            WaterStorage = new WaterStorage();
            WaterEconomySystem = new WaterEconomySystem();

            // Needs + Radiation
            NeedsSystem = new NeedsSystem(_needsProfile, sv => true);

            // Wire photoperiod into NeedsSystem (null-safe: skipped if LightProfile not assigned)
            if (_lightProfile != null)
            {
                NeedsSystem.SetPhotoPeriodSystem(
                    () => PhotoperiodSystem.EffectiveDaylightHours,
                    _lightProfile,
                    () => Shelter.IsGrowLightActive);
            }

            RadiationSystem = new RadiationSystem(NeedsSystem);

            BeliefSystem = new BeliefSystem(rng: new System.Random(_worldSeed + 31));
            RadiationSystem.OnStatusGained += (sv, status) =>
            {
                if (status == SurvivorStatus.AcuteRadiationSyndrome)
                {
                    BeliefSystem.ShockRecoverNumbness(sv);
                }
            };

            // World Phase (Civil War -> Flashpoint -> Nuclear Winter). Phase 1 defaults:
            // no radiation, no post-war weather hazards, until the exchange fires.
            WorldPhaseSystem = new WorldPhaseSystem(_worldPhaseConfig);
            RadiationSystem.IsPaused = true;
            WeatherSystem.RestrictToNonHazardWeather = true;
            WorldPhaseSystem.OnNuclearExchange += HandleNuclearExchange;
            TimeSystem.OnDayTick += WorldPhaseSystem.OnDayTick;

            // Inventory + Crafting + Workbench scrap economy
            Inventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 200f };
            CraftingSystem = new CraftingSystem(Inventory);
            CraftingSystem.AddStation(new CraftingStation
            {
                id = WorkbenchSystem.StationId,
                displayName = "Workbench",
                Condition = 100f
            });
            WorkbenchSystem = new WorkbenchSystem(
                Inventory,
                id => _itemCatalog?.GetById(id),
                CraftingSystem,
                () => Shelter,
                () => TimeSystem != null ? TimeSystem.CurrentDay : 0);

            // Seed inventory
            SeedStartingInventory();

            // Survivors
            Survivors = new List<Survivor>();
            CreateSurvivor("sv_elena", "Elena Vasquez");
            CreateSurvivor("sv_marcus", "Marcus Olejnik");
            CreateSurvivor("sv_suki", "Suki Tanaka");

            // AI
            UtilityAI = new UtilityAI();
            Actions = new List<SurvivorAction>
            {
                CreateAction<EatActionSO>(),
                CreateAction<DrinkActionSO>(),
                CreateAction<DrinkContaminatedWaterActionSO>(),
                CreateAction<SleepActionSO>(),
                CreateAction<RestActionSO>(),
                CreateAction<WarmUpActionSO>(),
                CreateAction<TakeIodineActionSO>(),
                CreateAction<ScavengeActionSO>(),
                CreateAction<SurveyActionSO>(),
                CreateAction<TreatPatientActionSO>(),
                CreateAction<MentalBreakComfortActionSO>(),
                CreateAction<CraftActionSO>(),
                CreateAction<GuardActionSO>(),
                CreateAction<PedalGeneratorActionSO>()
            };

            // Medical triage (afflictions drain health; treatments halt/cure)
            MedicalSystem = new MedicalSystem(NeedsSystem, Inventory, Shelter);
            foreach (var aff in AtomicWar._Game.Medical.MedicalSystem.CreateDefaultAfflictions())
                MedicalSystem.RegisterAffliction(aff);
            // Register common treatment recipes if items exist
            var bandage = _itemCatalog?.GetById("bandage");
            var tweezers = _itemCatalog?.GetById("tweezers");
            if (bandage != null)
            {
                MedicalSystem.RegisterTreatment(
                    AtomicWar._Game.Medical.MedicalSystem.CreateGunshotBandageHaltRecipe(bandage));
                if (tweezers != null)
                {
                    MedicalSystem.RegisterTreatment(
                        AtomicWar._Game.Medical.MedicalSystem.CreateGunshotFullRecipe(bandage, tweezers));
                }
            }

            // Events
            EventRunner = new EventRunner();
            if (_eventCatalog != null)
            {
                EventRunner.SetPool(_eventCatalog.events);
            }

            // Mental Break System (Prompt #29). Designers populate the catalog
            // with MentalBreakSO assets (BingeEater, ViolentParanoia, etc.).
            // If the catalog is null, the system is still constructed — just
            // empty — so the rest of the game continues to work; the Survivor
            // just never rolls for a break.
            MentalBreakSystem = new MentalBreakSystem();
            if (_mentalBreakCatalog != null)
            {
                foreach (var br in _mentalBreakCatalog.breaks)
                {
                    if (br != null) MentalBreakSystem.RegisterBreak(br);
                }
            }
            // Host-side binge/sabotage so Survivors assembly stays free of
            // Inventory/Shelter refs (avoids asmdef cycles).
            MentalBreakSystem.BingeEatHandler = (sv, br) => ForceMentalBreakBingeEat(sv, br);
            MentalBreakSystem.SabotageHandler = (sv, br, rng) => ForceMentalBreakSabotage(rng);
            // Host-side comfort-cure so the system can consume a Comfort
            // item from the Inventory without referencing the Inventory
            // assembly directly. Same asmdef-boundary pattern.
            MentalBreakSystem.ComfortCureHandler = (sv, br) => ForceMentalBreakComfortCure(sv, br);

            // Hatch-dilemma prompt: tracks the active "knock at the
            // hatch" decision and provides a timeout so the survivor
            // doesn't sit in AtHatchDilemma forever. The UI flow is
            // wired in OnHatchDilemmaReady_Handle (EventRunner.Run shows
            // the modal; the prompt's Tick advances the timeout).
            HatchDilemmaPromptField = new HatchDilemmaPrompt();

            // Hatch defense (Prompt #33): security vs raids, guard duty, loot theft
            HatchDefenseSystem = new HatchDefenseSystem(
                getShelter: () => Shelter,
                getInventory: () => Inventory,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                inflictTrauma: (sv, affId) => MedicalSystem?.Inflict(sv, affId),
                rng: new System.Random(_worldSeed + 33));
            // Starting hatch plate: reinforced locks at level 1
            Shelter.AddModule(new ShelterModuleInstance(
                HatchDefenseModuleSO.ReinforcedLocksId, 1)
            {
                SecurityContribution = 10f,
                FilterHealth = 100f
            });
            // Workbench lists hatch install / upgrade lines (scrap sink)
            WorkbenchSystem?.SetHatchDefense(HatchDefenseSystem);

            // Dynamic phase economy + faction trust matrix
            EconomySystem = new DynamicEconomySystem(
                getPhase: () => WorldPhaseSystem.CurrentPhase,
                shelter: Shelter,
                rng: new System.Random(_worldSeed + 91));
            foreach (var fac in DynamicEconomySystem.CreateDefaultFactions())
                EconomySystem.RegisterFaction(fac);
            EconomySystem.SetHatchDefense(HatchDefenseSystem);
            EconomySystem.SetDayProvider(() => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EconomySystem.BindEventRunner(EventRunner);
            WorldPhaseSystem.OnPhaseChanged += phase =>
            {
                EconomySystem.NotifyPhaseChanged(phase);
                // Keep weather/rad systems in sync with campaign phase labels
                if (phase == WorldPhase.Flashpoint || phase == WorldPhase.NuclearWinter)
                {
                    // Exchange already unpauses rads in HandleNuclearExchange
                }
            };

            // Proc-gen wasteland map (seed-stable layout for this playthrough)
            GeneratedMap = MapGenerator.Generate(_worldSeed);

            // Knowledge map must exist before SaveSystem can capture it
            KnowledgeMap = new RadiationKnowledgeMap();
            SeedKnowledgeMap();

            // Save
            SaveSystem = new SaveSystem(
                GameState, WeatherSystem, TemperatureSystem, NeedsSystem,
                RadiationSystem, Shelter, () => Survivors,
                id => _itemCatalog?.GetById(id),
                id => null);
            SaveSystem.SetPhotoPeriodSystem(PhotoperiodSystem);
            SaveSystem.SetKnowledgeMap(KnowledgeMap);
            SaveSystem.SetGeneratedMap(GeneratedMap);
            SaveSystem.SetInventory(Inventory);
            SaveSystem.SetMedicalSystem(MedicalSystem);
            SaveSystem.SetWorldPhaseSystem(WorldPhaseSystem);
            SaveSystem.SetEconomySystem(EconomySystem);
            SaveSystem.SetPowerNetwork(PowerNetwork);
            SaveSystem.SetHatchDefense(HatchDefenseSystem);
            SaveSystem.SetWaterStorage(WaterStorage);
            // SetFlashpointChoreographer is called later in InitializeSystems
            // after the Choreographer itself is constructed (it depends on
            // RadioTunerSystem and other systems wired after SaveSystem).

            // Subscribe to phase changes for autosave
            GameState.OnPhaseChanged += phase =>
            {
                if (phase == GamePhase.Running) SaveSystem.AutoSave();
            };

            // Scavenging + survey (shares KnowledgeMap with SaveSystem)
            ScavengingSystem = new LocationScavengingSystem(
                RadiationSystem, Inventory, _itemCatalog, _worldSeed,
                KnowledgeMap, () => TimeSystem.CurrentDay,
                _lootTable, () => WorldPhaseSystem.CurrentPhase);
            ScavengingSystem.OnSurveyCompleted += (mission, success) => RefreshMapKnowledgeHUD();
            ScavengingSystem.OnMissionCompleted += (mission, loot) => RefreshMapKnowledgeHUD();

            // Expedition Engine (node-based events, stances, stamina drain, push-your-luck)
            // Wired with the MedicalSystem so the Day-30 flashpoint intercept
            // can inflict trauma afflictions on survivors caught outside, and
            // with the Shelter + Survivors list so the hatch-dilemma handler
            // can spike bunker contamination and propagate deny-entry morale.
            ExpeditionSystem = new ExpeditionSystem(
                RadiationSystem, Inventory, _itemCatalog, WeatherSystem,
                KnowledgeMap, MedicalSystem, Shelter, Survivors, _worldSeed);
            ExpeditionSystem.SetGeneratedMap(GeneratedMap);
            SaveSystem.SetExpeditionSystem(ExpeditionSystem);

            // Hatch dilemma: when a comms-severed expedition arrives at the
            // bunker, run a forced dilemma GameEventSO with three choices
            // (let them in, force decon, deny). The ExpeditionSystem raises
            // HatchDilemmaReadySignal when an expedition enters the
            // AtHatchDilemma phase; we build the event here and run it
            // through the EventRunner, then forward the choice back via
            // HatchDilemmaResolvedSignal (which the ExpeditionSystem listens to).
            ExpeditionSystem.OnHatchDilemmaReady += OnHatchDilemmaReady_Handle;
            if (Inventory != null)
            {
                Inventory.OnInventoryChanged += RefreshMapKnowledgeHUD;
            }
            if (KnowledgeMap != null)
            {
                KnowledgeMap.OnKnowledgeChanged += RefreshMapKnowledgeHUD;
            }

            // Radio (broadcast only; tuner/intel extraction is separate)
            RadioSystem = new RadioBroadcastSystem();
            RadioSystem.SetCatalog(_radioCatalog);
            
            // Radio Tuner System (intel extraction)
            RadioTunerSystem = new RadioTunerSystem(new System.Random(_worldSeed + 31));
            InitializeRadioFrequencies();
            
            // Wire up radio module fuel supply to RadioTunerSystem
            var radioModule = Shelter.GetModule("radio");
            if (radioModule != null && radioModule.IsOperational)
            {
                RadioTunerSystem.State.AvailableFuel = radioModule.Fuel;
                RadioTunerSystem.State.PowerConsumptionPerHour = 0.5f; // Default consumption
            }
            
            // Wire up intel extraction events
            RadioTunerSystem.OnIntelExtracted += intel =>
            {
                Debug.Log($"[Radio] Extracted intel: {intel.Type} - {intel.Text}");
                if (intel.Type == IntelType.PlumeReport)
                {
                    // Apply plume reports to knowledge map + proc-gen node reveal
                    RadioTunerSystem.ApplyPlumeReportToMap(intel, KnowledgeMap, GeneratedMap);
                    RefreshMapKnowledgeHUD();
                    _hud?.MapScreenUI?.Refresh();
                }
            };

            TimeSystem.OnDayTick += day =>
            {
                RadioSystem.CheckForBroadcast(day);
                Inventory?.DriftAllDevices(1f);
                KnowledgeMap?.TickDay(day);
                RefreshMapKnowledgeHUD();
            };

            // Day-30 Flashpoint Choreographer (narrative/UX layer over the
            // mechanical EMP/weather cascade). Owns the buildup days 25-29
            // and the second-by-second choreography. The mechanical side
            // effects of the exchange fire from the choreography's 'emp' step
            // so the EMP happens after the white flash, not before.
            //
            // Created at the END of InitializeSystems so the systems bundle
            // (Inventory, Survivors, EconomySystem, RadioTunerSystem) all have
            // real references; the EMP step needs every one of them on day 30.
            FlashpointChoreographer = new FlashpointChoreographer(
                sequence: _flashpointSequence,
                accessibilitySafeMode: () => GameState != null && GameState.AccessibilitySafeMode,
                systems: new FlashpointChoreographerSystems
                {
                    Inventory = Inventory,
                    Shelter = Shelter,
                    RadioState = RadioTunerSystem?.State,
                    WeatherSystem = WeatherSystem,
                    RadiationSystem = RadiationSystem,
                    EconomySystem = EconomySystem,
                    Survivors = Survivors,
                    ExchangeMoraleHit = WorldPhaseSystem.ExchangeMoraleHit,
                    ExpeditionSystem = ExpeditionSystem
                },
                hasFlashpointTriggered: () => WorldPhaseSystem != null && WorldPhaseSystem.HasTriggeredExchange);
            TimeSystem.OnDayTick += FlashpointChoreographer.OnDayTick;

            // Wire the Choreographer into SaveSystem so the buildup-day and
            // choreography-step state persist across save/load. Done here
            // (after the Choreographer is created) rather than near the
            // other SetXSystem calls because the Choreographer depends on
            // systems that are wired after SaveSystem in InitializeSystems.
            if (SaveSystem != null)
            {
                SaveSystem.SetFlashpointChoreographer(
                    FlashpointChoreographer.CaptureState,
                    FlashpointChoreographer.RestoreState);
                SaveSystem.SetMentalBreakSystem(MentalBreakSystem);
            }
        }

        private void InitializeRadioFrequencies()
        {
            // Create default frequencies
            var civilian = ScriptableObject.CreateInstance<RadioFrequencySO>();
            civilian.id = "88.5_civilian";
            civilian.displayName = "88.5 FM Civilian";
            civilian.frequencyMHz = 88.5f;
            civilian.type = RadioFrequencyType.Civilian;
            civilian.activeFromDay = 0;
            civilian.activeUntilDay = 30;
            civilian.baseSignalStrength = 0.7f;
            civilian.interferenceSusceptibility = 0.3f;
            
            var military = ScriptableObject.CreateInstance<RadioFrequencySO>();
            military.id = "102.1_military";
            military.displayName = "102.1 Military";
            military.frequencyMHz = 102.1f;
            military.type = RadioFrequencyType.Military;
            military.activeFromDay = 0;
            military.activeUntilDay = 30;
            military.baseSignalStrength = 0.6f;
            military.interferenceSusceptibility = 0.2f;
            
            var numbers = ScriptableObject.CreateInstance<RadioFrequencySO>();
            numbers.id = "99.0_numbers";
            numbers.displayName = "99.0 Numbers Station";
            numbers.frequencyMHz = 99.0f;
            numbers.type = RadioFrequencyType.NumbersStation;
            numbers.activeFromDay = 31;
            numbers.activeUntilDay = -1;
            numbers.baseSignalStrength = 0.4f;
            numbers.interferenceSusceptibility = 0.5f;
            
            var emergency = ScriptableObject.CreateInstance<RadioFrequencySO>();
            emergency.id = "107.0_emergency";
            emergency.displayName = "107.0 Emergency";
            emergency.frequencyMHz = 107.0f;
            emergency.type = RadioFrequencyType.Emergency;
            emergency.activeFromDay = 31;
            emergency.activeUntilDay = -1;
            emergency.baseSignalStrength = 0.5f;
            emergency.interferenceSusceptibility = 0.4f;
            
            RadioTunerSystem.SetFrequencies(new[] { civilian, military, numbers, emergency });
        }

        /// <summary>
        /// The Day-30 atomic exchange cascade. Thinned: all mechanical
        /// side effects (EMP, weather force, radiation unpause, morale hit)
        /// run from the FlashpointChoreographer's 'emp' step, scheduled
        /// after the white flash. The choreographer is the single source
        /// of truth for the moment's timeline.
        /// </summary>
        private void HandleNuclearExchange()
        {
            if (FlashpointChoreographer == null)
            {
                // Fallback: if no choreographer is wired (test scene, broken
                // wiring), run the original cascade so the game still
                // advances to NuclearWinter. This matches the pre-Prompt-27
                // behavior and prevents soft-locks.
                var empResult = EMPEvent.ApplyGlobal(Inventory, Shelter, RadioTunerSystem?.State);
                Debug.Log($"[GameBootstrap] Nuclear exchange (fallback): {empResult.DevicesBroken} devices broken, " +
                          $"{empResult.ModulesDisabled} modules disabled, radio destroyed={empResult.RadioDestroyed}.");

                if (WeatherSystem != null)
                {
                    WeatherSystem.RestrictToNonHazardWeather = false;
                    WeatherSystem.ForceWeather(WeatherKind.Ashfall);
                }
                if (RadiationSystem != null) RadiationSystem.IsPaused = false;

                if (Survivors != null)
                {
                    float hit = WorldPhaseSystem?.ExchangeMoraleHit ?? 25f;
                    foreach (var sv in Survivors)
                    {
                        if (sv == null || !sv.IsAlive) continue;
                        sv.Needs.Morale = Mathf.Clamp(sv.Needs.Morale - hit, 0f, 100f);
                    }
                }
                return;
            }

            FlashpointChoreographer.OnNuclearExchange();
        }

        private void SeedKnowledgeMap()
        {
            if (KnowledgeMap == null) return;

            // Prefer proc-gen map nodes (authoritative per-playthrough layout)
            if (GeneratedMap?.Nodes != null)
            {
                for (int i = 0; i < GeneratedMap.Nodes.Count; i++)
                {
                    var n = GeneratedMap.Nodes[i];
                    if (n == null || string.IsNullOrEmpty(n.NodeId) || n.IsShelter) continue;
                    KnowledgeMap.SeedTile(n.NodeId, n.TrueRad, n.RumoredRad, 1f);
                }
            }

            // Also seed catalog locations if present (legacy / static sites)
            if (_locationCatalog?.locations == null) return;
            var rng = new System.Random(_worldSeed + 17);
            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
                if (KnowledgeMap.GetTile(loc.id) != null) continue; // already seeded from map
                float rumorScale = 0.4f + (float)rng.NextDouble() * 0.4f;
                KnowledgeMap.SeedTile(loc.id, loc.baseRadsPerHour, loc.baseRadsPerHour * rumorScale, 1f);
            }
        }

        private void RefreshMapKnowledgeHUD()
        {
            if (_hud == null || KnowledgeMap == null) return;
            bool hasGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var views = KnowledgeMap.GetAllPlayerViews(day, hasGeiger);
            int calAge = -1;
            var geiger = Inventory?.GetBestGeigerState();
            if (geiger != null)
            {
                calAge = InstrumentDevice.DaysSinceCalibration(geiger, day);
            }
            _hud.OnMapKnowledgeUpdated(views, hasGeiger, calAge);
        }

        private void SeedStartingInventory()
        {
            if (_itemCatalog == null) return;
            foreach (var item in _itemCatalog.items)
            {
                if (item == null) continue;
                // Give a reasonable starting stock
                int amount = item.type switch
                {
                    ItemType.Food => 10,
                    ItemType.Water => 10,
                    ItemType.Iodine => 5,
                    ItemType.AntiRad => 3,
                    ItemType.Fuel => 8,
                    ItemType.Filter => 3,
                    ItemType.Material => 15,
                    _ => 1
                };
                Inventory.Add(item, amount);
            }
        }

        /// <summary>
        /// Mental-break binge: consume highest-value food × multiplier from bunker stock.
        /// Hosted in Core so Survivors does not reference Inventory.
        /// </summary>
        private int ForceMentalBreakBingeEat(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return 0;
            if (!sv.IsAlive) return 0;

            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            int scanned = 0;
            for (int i = 0; i < Inventory.Slots.Count && scanned < MentalBreakSystem.BingeEaterMaxSlotsScanned; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Food) continue;
                if (slot.Item.hungerRestore < br.minFoodValueForBinge) continue;
                if (slot.Item.hungerRestore > bestValue)
                {
                    best = slot;
                    bestValue = slot.Item.hungerRestore;
                }
                scanned++;
            }
            if (best == null) return 0;

            int wanted = Mathf.Max(1, Mathf.CeilToInt(br.consumptionMultiplier));
            int consumed = Mathf.Min(wanted, best.Amount);
            if (consumed <= 0) return 0;
            Inventory.Remove(best.Item, consumed);
            float restore = best.Item.hungerRestore * consumed;
            sv.Needs.Hunger = Mathf.Max(0f, sv.Needs.Hunger - restore);
            return consumed;
        }

        /// <summary>
        /// Mental-break comfort cure: pick a Comfort item from the
        /// inventory, consume one, and return true. Returns false if
        /// no Comfort item is available. Hosted in Core so Survivors
        /// does not reference Inventory.
        /// </summary>
        private bool ForceMentalBreakComfortCure(Survivor sv, MentalBreakSO br)
        {
            if (sv == null || br == null || Inventory == null || Inventory.Slots == null) return false;

            // Find a Comfort item (e.g. old_book, music_disc). Prefer the
            // one with the highest moraleRestore / sellValue as a stand-in
            // for "high-value".
            InventorySlot best = null;
            float bestValue = float.NegativeInfinity;
            for (int i = 0; i < Inventory.Slots.Count; i++)
            {
                var slot = Inventory.Slots[i];
                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                if (slot.Item.type != ItemType.Comfort) continue;
                // Use tradeValue + moraleEffect as a high-value proxy.
                float value = slot.Item.tradeValue + slot.Item.moraleEffect;
                if (value > bestValue)
                {
                    best = slot;
                    bestValue = value;
                }
            }
            if (best == null || best.Item == null) return false;

            // Consume one unit of the comfort item. The system-side
            // TryCureWithComfortItem will then advance mentalBreakCureProgress
            // by br.comfortItemCureAmount and call Cure() if the threshold
            // is met.
            return Inventory.Remove(best.Item, 1);
        }

        /// <summary>
        /// Mental-break sabotage: disable or degrade a random shelter module.
        /// Hosted in Core so Survivors does not reference Shelter.
        /// </summary>
        private void ForceMentalBreakSabotage(System.Random rng)
        {
            if (Shelter == null || Shelter.Modules == null || Shelter.Modules.Count == 0) return;
            if (rng == null) rng = new System.Random();
            int idx = rng.Next(Shelter.Modules.Count);
            var mod = Shelter.Modules[idx];
            if (mod == null) return;
            if (mod.IsEnabled)
                mod.IsEnabled = false;
            else
                mod.FilterHealth = Mathf.Max(0f, mod.FilterHealth - 25f);
        }

        private void CreateSurvivor(string id, string name)
        {
            var sv = new Survivor { Id = id, DisplayName = name };
            // Elena is the medic by default; others baseline
            if (id == "sv_elena") sv.MedicalSkill = 0.85f;
            else if (id == "sv_marcus") sv.MedicalSkill = 0.35f;
            else sv.MedicalSkill = 0.25f;
            // Default room assignment so the MentalBreakSystem has room
            // boundaries from day 1 (Prompt #29 follow-up). Elena stays
            // near the bed in quarters; Marcus watches the stores; Suki
            // is in the entry hallway (closest to the hatch).
            if (id == "sv_elena") sv.CurrentRoomId = "quarters";
            else if (id == "sv_marcus") sv.CurrentRoomId = "stores";
            else if (id == "sv_suki") sv.CurrentRoomId = "entry";
            Survivors.Add(sv);
            NeedsSystem.Register(sv);
            RadiationSystem.Register(sv);
        }

        private T CreateAction<T>() where T : SurvivorAction
        {
            var action = ScriptableObject.CreateInstance<T>();
            return action;
        }

        // -----------------------------------------------------------------
        // Game loop
        // -----------------------------------------------------------------

        private void TickSystems(float gameHours)
        {
            if (gameHours <= 0f) return;

            // Environment
            WeatherSystem.Tick(gameHours);
            TemperatureSystem.Tick(gameHours);
            PhotoperiodSystem.Tick(gameHours);

            // Shelter
            Shelter.Tick(gameHours);

            // Power grid (fuel burn, CO, pedaling, load-shed) then push to modules
            if (PowerNetwork != null)
            {
                string weatherName = WeatherSystem != null ? WeatherSystem.Current.ToString() : null;
                PowerNetwork.Tick(
                    gameHours,
                    weatherName,
                    (id, fatigueDelta, hungerDelta) =>
                    {
                        if (Survivors == null || string.IsNullOrEmpty(id)) return false;
                        Survivor pedaler = null;
                        for (int i = 0; i < Survivors.Count; i++)
                        {
                            if (Survivors[i] != null && Survivors[i].Id == id)
                            {
                                pedaler = Survivors[i];
                                break;
                            }
                        }
                        if (pedaler == null || !pedaler.IsAlive || pedaler.Needs == null)
                            return false;
                        if (pedaler.Needs.Fatigue >= 95f)
                            return false;
                        pedaler.Needs.Fatigue = Mathf.Clamp(
                            pedaler.Needs.Fatigue + fatigueDelta, 0f, 100f);
                        pedaler.Needs.Hunger = Mathf.Clamp(
                            pedaler.Needs.Hunger + hungerDelta, 0f, 100f);
                        return true;
                    });
                PowerNetwork.ApplyToShelter(Shelter);
            }

            // Hatch defense: outdoor generator noise + periodic post-Day-30 raid rolls
            HatchDefenseSystem?.Tick(gameHours, PowerNetwork);

            // Needs
            NeedsSystem.Tick(gameHours);

            // Medical triage — Health pressure from active afflictions
            MedicalSystem?.Tick(Survivors, gameHours);

            // Mental breaks: low-morale tracking, break rolls, BingeEater
            // consumption, ViolentParanoia sabotage, passive morale drain
            // to other survivors, and natural cure progress.
            if (MentalBreakSystem != null)
            {
                MentalBreakSystem.Tick(gameHours, Survivors, new System.Random(_worldSeed));
            }

            // Hatch-dilemma prompt: advance the timeout. On expiry the
            // prompt auto-resolves with ForceDeconOutside.
            HatchDilemmaPromptField?.Tick(gameHours);

            // Radiation
            RadiationSystem.Tick(gameHours);

            // Water economy: catchment collection + purifier conversion queue.
            WaterEconomySystem?.Tick(gameHours, WeatherSystem.Current, TimeSystem.CurrentDay, Shelter, WaterStorage);

            // Crafting
            CraftingSystem.Tick(gameHours);

            // Scavenging & Expeditions
            ScavengingSystem?.Tick(gameHours);
            ExpeditionSystem?.Tick(gameHours);
            
            // Radio Tuner (intel extraction)
            if (RadioTunerSystem != null && Shelter != null)
            {
                var radioModule = Shelter.GetModule("radio");
                if (radioModule != null && radioModule.IsOperational && radioModule.Fuel > 0f)
                {
                    RadioTunerSystem.Tick(gameHours, WeatherSystem.Current, TimeSystem.CurrentDay);
                }
            }

            // AI (evaluate per survivor, every EvaluationInterval)
            UtilityAI.Tick(gameHours * TimeSystem.SecondsPerGameHour);
            if (UtilityAI.ShouldEvaluate())
            {
                // Fresh sleep-wave occupancy so capacity is per evaluation pass.
                SleepQualitySystem.ResetBedOccupancy(Shelter);
                // Guards re-assigned each AI wave (stale posts clear).
                HatchDefenseSystem?.ClearGuards();
                float indoorTemp = TemperatureSystem != null
                    ? TemperatureSystem.GetIndoorTemperature(Shelter)
                    : 15f;
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
                float raidThreat = 0f;
                if (HatchDefenseSystem != null && day >= HatchDefenseSystem.RaidUnlockDay)
                {
                    raidThreat = 0.25f;
                    if (HatchDefenseSystem.GeneratorRunningOutside
                        || HatchDefenseSystem.ExternalNoise >= HatchDefenseSystem.ExternalGeneratorNoiseThreshold)
                        raidThreat = 0.7f;
                    if (EconomySystem != null)
                    {
                        foreach (var fac in EconomySystem.Factions.Values)
                        {
                            if (fac == null) continue;
                            if (EconomySystem.GetStance(fac.id) == TradeStance.HostileRaid)
                                raidThreat = Mathf.Max(raidThreat, 0.85f);
                        }
                    }
                }

                foreach (var sv in Survivors)
                {
                    if (!sv.IsAlive) continue;
                    float mapUncertainty = GetMapUncertaintyFor(sv);
                    BeliefSystem.Tick(sv, mapUncertainty, gameHours);
                    int scrapDeficit = WorkbenchSystem != null
                        ? WorkbenchSystem.GetCriticalElectronicScrapDeficit()
                        : 0;
                    var context = new AIContext(sv, Shelter, Inventory, new System.Random(_worldSeed + sv.Id.GetHashCode()))
                    {
                        IsFalloutStorm  = WeatherSystem.Current == WeatherKind.FalloutStorm,
                        AmbientRadRate  = 5f,
                        IsListless      = sv.IsListless,
                        GrowLightActive = Shelter.IsGrowLightActive,
                        OnRequestSurvey = RequestSurveyForSurvivor,
                        MapUncertainty  = mapUncertainty,
                        BeliefSystem    = BeliefSystem,
                        IsAnxious       = sv.HasRadiationAnxietyStatus,
                        IsNumb          = sv.IsNumb,
                        MedicalSystem   = MedicalSystem,
                        PowerNetwork    = PowerNetwork,
                        HatchDefense    = HatchDefenseSystem,
                        RaidThreatLevel = raidThreat,
                        CurrentDay      = day,
                        IndoorTemperatureC = indoorTemp,
                        SleepRoomId     = SleepQualitySystem.DefaultSleepRoomId,
                        AreRoomsAdjacent = Shelter.AreRoomsAdjacent,
                        WaterStorage    = WaterStorage,
                        NeedsElectronicScrapForCriticalRepair = scrapDeficit > 0,
                        JunkScavengeUrgency = scrapDeficit > 0
                            ? Mathf.Clamp01(scrapDeficit / 4f)
                            : 0f,
                        RadiationSystem = RadiationSystem,
                        GetSurvivors    = () => Survivors
                    };
                    var action = UtilityAI.SelectAction(context, Actions);
                    action?.Execute(context);
                }
            }

            // Events (chance per hour)
            var eventContext = new EventContext(Survivors.Count > 0 ? Survivors[0] : null, Shelter, Inventory,
                new System.Random(_worldSeed + TimeSystem.CurrentDay))
            {
                CurrentDay = TimeSystem.CurrentDay,
                CurrentHour = TimeSystem.CurrentHourFloat,
                IsFalloutStorm = WeatherSystem.Current == WeatherKind.FalloutStorm,
                AllSurvivors = Survivors,
                MentalBreak = MentalBreakSystem
            };
            EventRunner.Tick(gameHours, eventContext);

            // Try to trigger an event occasionally
            if (UnityEngine.Random.value < 0.05f) // ~5% chance per hour
            {
                var selectedEvent = EventRunner.SelectEvent(eventContext);
                if (selectedEvent != null)
                {
                    EventRunner.Run(selectedEvent, eventContext);
                }
            }
        }

        // -----------------------------------------------------------------
        // Win/Lose
        // -----------------------------------------------------------------

        private void CheckWinLose()
        {
            // Lose: all survivors dead
            bool allDead = true;
            foreach (var sv in Survivors)
            {
                if (sv.IsAlive) { allDead = false; break; }
            }
            if (allDead)
            {
                EndGame("All survivors have perished.", "lose");
                return;
            }

            // Win: survived to campaign end
            if (TimeSystem.CurrentDay >= _campaignLengthDays)
            {
                EndGame("You survived the nuclear winter.", "win");
            }
        }

        private void EndGame(string reason, string outcome)
        {
            IsGameOver = true;
            GameOverReason = reason;
            GameState.Phase = GamePhase.GameOver;
            Debug.Log($"[GameBootstrap] GAME OVER ({outcome}): {reason}");
        }

        // -----------------------------------------------------------------
        // HUD wiring
        // -----------------------------------------------------------------

        private void WireHUD()
        {
            if (_hud == null) return;


            _hud.BindEventRunner(EventRunner);
            _hud.BindEconomy(EconomySystem);
            _hud.BindRoomAssignment(Survivors, Shelter);
            _hud.BindPowerNetwork(PowerNetwork);
            _hud.BindHatchDefense(HatchDefenseSystem);
            _hud.BindGeneratedMap(GeneratedMap, () => WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear);
            _hud.BindWorkbench(WorkbenchSystem);

            // Map screen expedition requests → ExpeditionSystem
            if (_hud.MapScreenUI != null)
            {
                _hud.MapScreenUI.OnExpeditionRequested += (survivor, nodeId, pathReq) =>
                {
                    if (ExpeditionSystem == null || survivor == null || pathReq == null) return;
                    var node = GeneratedMap?.GetNode(nodeId);
                    if (node != null)
                        ExpeditionSystem.StartExpedition(survivor, node);
                    else
                        ExpeditionSystem.StartExpeditionFromPath(
                            survivor, nodeId, pathReq.TravelHours, pathReq.TrueRad,
                            pathReq.DangerLevel, pathReq.NodeId);
                };
            }

            // Wire radiation updates
            RadiationSystem.OnDoseChanged += (sv, dose) =>
            {
                if (sv == Survivors?[0]) // primary survivor
                {
                    _hud.OnRadiationUpdated(sv.LifetimeRadiationExposure, sv.RadiationDose);
                }
            };

            // Wire needs updates
            NeedsSystem.OnNeedChanged += (sv, kind, value) =>
            {
                if (sv == Survivors?[0])
                {
                    _hud.Bind(sv);
                }
            };

            // Wire shelter
            _hud.OnShelterUpdated(Shelter);

            // Initial fog-of-war push
            RefreshMapKnowledgeHUD();
        }

        // -----------------------------------------------------------------
        // Public API (for UI buttons, input handler, etc.)
        // -----------------------------------------------------------------

        public void PauseGame()
        {
            GameState.IsPaused = true;
            GameState.Phase = GamePhase.Paused;
        }

        public void ResumeGame()
        {
            GameState.IsPaused = false;
            GameState.Phase = GamePhase.Running;
        }

        public void SaveGame(string slotId = "quicksave")
        {
            SaveSystem.Save(slotId);
        }

        public void LoadGame(string slotId = "quicksave")
        {
            if (SaveSystem.Load(slotId))
            {
                IsGameOver = false;
                GameOverReason = null;
            }
        }

        public void ConsumeItem(Survivor sv, ItemDefinition item)
        {
            if (sv == null || item == null || !sv.IsAlive) return;
            Inventory.Consume(item, sv, RadiationSystem, NeedsSystem);
        }

        public void CraftRecipe(Recipe recipe)
        {
            if (recipe == null) return;
            CraftingSystem.StartCraft(recipe);
        }

        public void SelectEventChoice(int choiceIndex)
        {
            // Applies to the most recently triggered event context
            if (EventRunner.ActiveConsequences.Count > 0 || EventRunner.Pool.Count > 0)
            {
                // EventModalUI handles this via its own Bind
            }
        }

        public bool StartScavengeMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            return ScavengingSystem.StartMission(survivor, location);
        }

        public bool StartExpeditionMission(Survivor survivor, LocationDefinitionSO location, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || location == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, location, stance);
        }

        /// <summary>Start expedition to a proc-gen map node (weather-scaled travel).</summary>
        public bool StartExpeditionToNode(Survivor survivor, string nodeId, ExpeditionStance stance = ExpeditionStance.Stealth)
        {
            if (ExpeditionSystem == null || survivor == null || GeneratedMap == null) return false;
            var node = GeneratedMap.GetNode(nodeId);
            if (node == null) return false;
            return ExpeditionSystem.StartExpedition(survivor, node, stance);
        }

        /// <summary>Open the wasteland map screen (UI).</summary>
        public void OpenMapScreen()
        {
            _hud?.MapScreenUI?.Open();
        }

        /// <summary>Open the workbench disassembly / repair / hatch-install screen.</summary>
        public void OpenWorkbench()
        {
            _hud?.WorkbenchUI?.Open();
        }

        /// <summary>Toggle workbench panel (keybind B).</summary>
        public void ToggleWorkbench()
        {
            _hud?.WorkbenchUI?.Toggle();
        }

        /// <summary>Execute a workbench line by 0-based index (keybinds 1-9).</summary>
        public bool ExecuteWorkbenchLine(int lineIndex)
        {
            return _hud?.WorkbenchUI != null && _hud.WorkbenchUI.Execute(lineIndex);
        }

        /// <summary>Open hatch defense status panel.</summary>
        public void OpenHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Open();
        }

        /// <summary>Toggle hatch defense panel (keybind H).</summary>
        public void ToggleHatchDefense()
        {
            _hud?.HatchDefenseHUD?.Toggle();
        }

        /// <summary>
        /// Open trade with a faction stockpile (UI). Hostile factions still open
        /// so the player can demand parley after a hatch repel.
        /// </summary>
        public bool OpenTrade(string factionId, Inventory.Inventory factionStock)
        {
            if (_hud?.TradeScreenUI == null || Inventory == null || factionStock == null)
                return false;
            return _hud.TradeScreenUI.Open(factionId, Inventory, factionStock);
        }

        /// <summary>Demand parley / surrender on the open trade screen (keybind P).</summary>
        public bool DemandTradeParley()
        {
            return _hud?.TradeScreenUI != null && _hud.TradeScreenUI.TryDemandParley();
        }

        /// <summary>Send a survivor to survey a location with a working geiger.</summary>
        public bool StartSurveyMission(Survivor survivor, LocationDefinitionSO location)
        {
            if (ScavengingSystem == null || survivor == null || location == null) return false;
            bool started = ScavengingSystem.StartSurvey(survivor, location);
            if (started) RefreshMapKnowledgeHUD();
            return started;
        }

        /// <summary>
        /// AI/UI hook: survey the least-known location (unsurveyed first, then oldest measure).
        /// </summary>
        public bool RequestSurveyForSurvivor(Survivor survivor)
        {
            if (survivor == null || !survivor.IsAlive || ScavengingSystem == null) return false;
            if (Inventory == null || !Inventory.HasWorkingGeiger()) return false;
            if (_locationCatalog?.locations == null || _locationCatalog.locations.Count == 0) return false;

            LocationDefinitionSO best = null;
            int bestScore = int.MinValue;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null) continue;
                var tile = KnowledgeMap?.GetTile(loc.id);
                int score;
                if (tile == null || !tile.Surveyed) score = 1000;
                else score = day - tile.MeasuredAtDay;
                if (score > bestScore)
                {
                    bestScore = score;
                    best = loc;
                }
            }

            return best != null && StartSurveyMission(survivor, best);
        }

        private float GetMapUncertaintyFor(Survivor survivor)
        {
            if (KnowledgeMap == null || survivor == null) return 0.5f;

            bool hasWorkingGeiger = Inventory != null && Inventory.HasWorkingGeiger();
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;

            if (ScavengingSystem != null)
            {
                foreach (var mission in ScavengingSystem.ActiveMissions)
                {
                    if (mission?.SurvivorId == survivor.Id)
                    {
                        var view = KnowledgeMap.GetPlayerView(mission.LocationId, day, hasWorkingGeiger);
                        return Mathf.Clamp01(1f - view.Confidence);
                    }
                }
            }

            float totalConfidence = 0f;
            int count = 0;
            foreach (var id in KnowledgeMap.Tiles.Keys)
            {
                var view = KnowledgeMap.GetPlayerView(id, day, hasWorkingGeiger);
                totalConfidence += view.Confidence;
                count++;
            }
            if (count == 0) return hasWorkingGeiger ? 0.5f : 1f;
            return Mathf.Clamp01(1f - (totalConfidence / count));
        }

        // -----------------------------------------------------------------
        // Day-30 Flashpoint: Hatch Dilemma
        // -----------------------------------------------------------------

        /// <summary>
        /// Day-30 hatch dilemma handler. Called by the ExpeditionSystem when
        /// a comms-severed expedition enters the AtHatchDilemma phase. Builds
        /// a forced GameEventSO with three player choices, runs it through
        /// the EventRunner, and forwards the choice back via the typed
        /// HatchDilemmaResolvedSignal (which the ExpeditionSystem listens to).
        /// </summary>
        private void OnHatchDilemmaReady_Handle(ExpeditionState exp)
        {
            if (exp == null || EventRunner == null) return;

            string survivorName = exp.Survivor != null ? exp.Survivor.DisplayName : "the survivor";
            var eventSo = ScriptableObject.CreateInstance<GameEvent>();
            eventSo.id = $"evt_hatch_dilemma_{exp.ExpeditionId}";
            eventSo.title = "Knock at the Hatch";
            eventSo.bodyText =
                $"{survivorName} is at the door. Their suit and gear are soaked in fallout, " +
                "and the dosimeter on their chest is screaming. You have one chance to " +
                "decide what happens next.";
            eventSo.weight = 1f;
            eventSo.conditions = new EventConditions { MinDay = 1 };
            eventSo.choices = new List<EventChoice>
            {
                new EventChoice
                {
                    ChoiceId = "let_them_in",
                    Text = "Open the hatch. They are family.",
                    MoraleDelta = +5f
                },
                new EventChoice
                {
                    ChoiceId = "force_decon",
                    Text = "Force them to strip and decontaminate outside.",
                    MoraleDelta = -15f
                },
                new EventChoice
                {
                    ChoiceId = "deny_entry",
                    Text = "Do not open the hatch.",
                    MoraleDelta = -30f
                }
            };

            // Start the prompt (begins the timeout). On timeout, the
            // prompt fires OnTimeout which auto-resolves with
            // ForceDeconOutside (the safest option).
            if (HatchDilemmaPromptField != null)
            {
                HatchDilemmaPromptField.Begin(exp);
                HatchDilemmaPromptField.OnTimeout -= OnHatchDilemmaTimeout_Handle;
                HatchDilemmaPromptField.OnTimeout += OnHatchDilemmaTimeout_Handle;
                HatchDilemmaPromptField.OnChoiceApplied -= OnHatchDilemmaChoiceApplied_Handle;
                HatchDilemmaPromptField.OnChoiceApplied += OnHatchDilemmaChoiceApplied_Handle;
            }

            // Translate the EventRunner's choice event into our typed signal.
            string expeditionId = exp.ExpeditionId;
            Action<GameEvent, EventChoice, EventContext> onChoice = null;
            onChoice = (gameEvent, choice, ctx) =>
            {
                if (gameEvent == null || choice == null) return;
                if (gameEvent.id != eventSo.id) return;
                HatchDilemmaResolvedSignal.Resolution resolution = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
                switch (choice.ChoiceId)
                {
                    case "let_them_in":
                        resolution = HatchDilemmaResolvedSignal.Resolution.LetThemIn;
                        break;
                    case "force_decon":
                        resolution = HatchDilemmaResolvedSignal.Resolution.ForceDeconOutside;
                        break;
                    case "deny_entry":
                        resolution = HatchDilemmaResolvedSignal.Resolution.DenyEntry;
                        break;
                }
                EventBus.Raise(new HatchDilemmaResolvedSignal(expeditionId, resolution));
                // Stop the timeout so we do not double-resolve on Tick.
                HatchDilemmaPromptField?.ApplyChoice(resolution);
                EventRunner.OnChoiceApplied -= onChoice;
            };
            EventRunner.OnChoiceApplied += onChoice;

            // Build a minimal EventContext (no choices to apply directly; the
            // side effects are handled by the ExpeditionSystem on the resolve
            // signal). Run so the event modal is presented to the player.
            var ctx = new EventContext(exp.Survivor, Shelter, Inventory, new System.Random(_worldSeed));
            EventRunner.Run(eventSo, ctx);
        }

        /// <summary>
        /// Hatch-dilemma prompt timeout: auto-apply the timeout resolution
        /// (default ForceDeconOutside) by raising the resolved signal.
        /// The ExpeditionSystem listens and applies the consequence.
        /// </summary>
        private void OnHatchDilemmaTimeout_Handle(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            // Find the active expedition and raise the signal so the
            // ExpeditionSystem can apply the consequence. The prompt
            // already deactivated itself in Tick before firing OnTimeout.
            EventBus.Raise(new HatchDilemmaResolvedSignal(
                expeditionId: FindActiveHatchDilemmaExpeditionId(),
                choice: resolution));
        }

        /// <summary>
        /// Player (or AI) made a hatch-dilemma choice via the event
        /// modal. The OnChoiceApplied lambda already raised the
        /// HatchDilemmaResolvedSignal; here we just cancel the prompt
        /// timeout so the survivor doesn't wait indefinitely after the
        /// player has already chosen.
        /// </summary>
        private void OnHatchDilemmaChoiceApplied_Handle(HatchDilemmaResolvedSignal.Resolution resolution)
        {
            HatchDilemmaPromptField?.Cancel();
        }

        private string FindActiveHatchDilemmaExpeditionId()
        {
            // Best-effort: walk the active expeditions and find the one in
            // AtHatchDilemma. If none, return empty (the resolve signal
            // is a no-op without an expeditionId).
            if (ExpeditionSystem == null || ExpeditionSystem.ActiveExpeditions == null) return string.Empty;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e != null && e.Phase == ExpeditionPhase.AtHatchDilemma) return e.ExpeditionId;
            }
            return string.Empty;
        }
    }
}
