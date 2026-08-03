using System;
using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.AI;
using AtomicWar._Game.AI.Actions;
using AtomicWar._Game.Crafting;
using AtomicWar._Game.Data;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Events;
using AtomicWar._Game.Inventory;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Shelter.Modules;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.UI;
using AtomicWar._Game.Medical;

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
        public UtilityAI UtilityAI { get; private set; }
        public EventRunner EventRunner { get; private set; }
        public SaveSystem SaveSystem { get; private set; }
        public LocationScavengingSystem ScavengingSystem { get; private set; }
        public ExpeditionSystem ExpeditionSystem { get; private set; }
        public RadiationKnowledgeMap KnowledgeMap { get; private set; }
        public RadioBroadcastSystem RadioSystem { get; private set; }
        public RadioTunerSystem RadioTunerSystem { get; private set; }
        public BeliefSystem BeliefSystem { get; private set; }
        public MedicalSystem MedicalSystem { get; private set; }
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

            // Inventory + Crafting
            Inventory = new Inventory.Inventory { Capacity = 50, MaxWeight = 200f };
            CraftingSystem = new CraftingSystem(Inventory);

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
                CreateAction<SleepActionSO>(),
                CreateAction<RestActionSO>(),
                CreateAction<WarmUpActionSO>(),
                CreateAction<TakeIodineActionSO>(),
                CreateAction<ScavengeActionSO>(),
                CreateAction<SurveyActionSO>(),
                CreateAction<TreatPatientActionSO>(),
                CreateAction<CraftActionSO>(),
                CreateAction<GuardActionSO>()
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
            SaveSystem.SetInventory(Inventory);
            SaveSystem.SetMedicalSystem(MedicalSystem);

            // Subscribe to phase changes for autosave
            GameState.OnPhaseChanged += phase =>
            {
                if (phase == GamePhase.Running) SaveSystem.AutoSave();
            };

            // Scavenging + survey (shares KnowledgeMap with SaveSystem)
            ScavengingSystem = new LocationScavengingSystem(
                RadiationSystem, Inventory, _itemCatalog, _worldSeed,
                KnowledgeMap, () => TimeSystem.CurrentDay);
            ScavengingSystem.OnSurveyCompleted += (mission, success) => RefreshMapKnowledgeHUD();
            ScavengingSystem.OnMissionCompleted += (mission, loot) => RefreshMapKnowledgeHUD();

            // Expedition Engine (node-based events, stances, stamina drain, push-your-luck)
            ExpeditionSystem = new ExpeditionSystem(
                RadiationSystem, Inventory, _itemCatalog, WeatherSystem,
                KnowledgeMap, _worldSeed);
            SaveSystem.SetExpeditionSystem(ExpeditionSystem);
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
                    // Apply plume reports to knowledge map
                    RadioTunerSystem.ApplyPlumeReportToMap(intel, KnowledgeMap);
                    RefreshMapKnowledgeHUD();
                }
            };

            TimeSystem.OnDayTick += day =>
            {
                RadioSystem.CheckForBroadcast(day);
                Inventory?.DriftAllDevices(1f);
                KnowledgeMap?.TickDay(day);
                RefreshMapKnowledgeHUD();
            };
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

        private void SeedKnowledgeMap()
        {
            if (KnowledgeMap == null || _locationCatalog?.locations == null) return;
            var rng = new System.Random(_worldSeed + 17);
            foreach (var loc in _locationCatalog.locations)
            {
                if (loc == null || string.IsNullOrEmpty(loc.id)) continue;
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

        private void CreateSurvivor(string id, string name)
        {
            var sv = new Survivor { Id = id, DisplayName = name };
            // Elena is the medic by default; others baseline
            if (id == "sv_elena") sv.MedicalSkill = 0.85f;
            else if (id == "sv_marcus") sv.MedicalSkill = 0.35f;
            else sv.MedicalSkill = 0.25f;
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

            // Needs
            NeedsSystem.Tick(gameHours);

            // Medical triage — Health pressure from active afflictions
            MedicalSystem?.Tick(Survivors, gameHours);

            // Radiation
            RadiationSystem.Tick(gameHours);

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
                foreach (var sv in Survivors)
                {
                    if (!sv.IsAlive) continue;
                    float mapUncertainty = GetMapUncertaintyFor(sv);
                    BeliefSystem.Tick(sv, mapUncertainty, gameHours);
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
                IsFalloutStorm = WeatherSystem.Current == WeatherKind.FalloutStorm
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
    }
}
