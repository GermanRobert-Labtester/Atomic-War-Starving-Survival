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
using AtomicWar._Game.Utilities;

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

        // Prompt #8 — Empath & Sociopath trait variance.
        public EmpathSystem EmpathSystem { get; private set; }

        // Prompt #7 — Addiction & Withdrawal pipeline.
        public AddictionSystem Addiction { get; private set; }

        // Prompt #6 — Phantom Intruders (fake hatch breach alerts).
        public PhantomIntruderSystem PhantomIntruders { get; private set; }

        // Prompt #9 — The Child dependent mechanic.
        public ChildDependentSystem ChildSystem { get; private set; }

        // Prompt #5 — Diary fragment catalog for Previous Tenants.
        public List<DiaryFragmentSO> DiaryCatalog { get; private set; }
        public List<Survivor> Survivors { get; private set; }
        public List<SurvivorAction> Actions { get; private set; }

        /// <summary>Ephemeral faction stockpiles for OpenTradeWithFaction.</summary>
        private readonly Dictionary<string, Inventory.Inventory> _factionStocks =
            new Dictionary<string, Inventory.Inventory>();

        /// <summary>Fast-forward speed for the F-key toggle.</summary>
        public const float FastForwardScale = 3f;

        /// <summary>
        /// Sub-step guard for the frame loop: after a long hitch the carried
        /// game-time is consumed in at most this many steps per frame; the
        /// remainder rolls into the next frame (no spiral of death, no lost time).
        /// </summary>
        private const int MaxSubstepsPerFrame = 128;

        /// <summary>Game hours owed to the systems from previous frames (large-delta carry).</summary>
        private float _pendingGameHours;

        /// <summary>Recycles journal entries so long sessions allocate no entry garbage.</summary>
        private GenericObjectPool<JournalEntry> _journalEntryPool;

        /// <summary>Reusable fog-of-war view buffer (no per-refresh list allocation).</summary>
        private readonly List<MapTilePlayerView> _knowledgeViewBuffer = new List<MapTilePlayerView>();

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

            float dt = Time.unscaledDeltaTime;

            // Day-30 Flashpoint choreography runs in real time (the flash is a
            // visual event, not a game-time event). Tick before the rest of
            // the systems so the EMP step's side effects (radiation pause,
            // weather force) are visible to the same frame's HUD push.
            FlashpointChoreographer?.Tick(dt);

            // Fast-forward-safe clock: TimeScale (1x / 3x) scales the simulated
            // delta, and the accumulated game-time is consumed in sub-steps of
            // at most TimeSystem.MaxGameHoursPerStep so systems + AI see every
            // hour chunk — large deltas (3x, hitches) never skip ticks.
            _pendingGameHours += dt * TimeSystem.TimeScale / TimeSystem.SecondsPerGameHour;
            int steps = 0;
            while (_pendingGameHours > 0f && steps < MaxSubstepsPerFrame)
            {
                float step = Mathf.Min(_pendingGameHours, TimeSystem.MaxGameHoursPerStep);
                _pendingGameHours -= step;
                TimeSystem.TickHours(step);
                TickSystems(step);
                steps++;
            }
            CheckWinLose();

            // Push environment data to HUD every frame
            if (_hud != null)
            {
                string weatherName = WeatherSystem.Current.ToString();
                string seasonName = TemperatureSystem.CurrentSeason?.displayName ?? "Nuclear Winter";
                _hud.Tick(TimeSystem.CurrentDay, TimeSystem.CurrentHourFloat, weatherName, seasonName, TimeSystem.TimeScale);
                _hud.OnShelterUpdated(Shelter);
                // Live radio hardware (signal + tuned label) on the intercept strip.
                PushRadioLiveStateToHud();
                // Internal Horror status strip (corpses / fire / coma / rust).
                RefreshInternalHorrorHud();
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

            // Prompt #5 — Previous Tenants: a sealed deep vault with diaries.
            // The player must clear rubble to access it. Contains diary warnings
            // about the filter, water, and shielding — diegetic system intel.
            var deepVault = new ShelterRoom("deep_vault", null)
            {
                UnlockState = RoomUnlockState.Sealed,
                RubbleClearHoursRemaining = 16f,
                RubbleClearHoursTotal = 16f,
                DiaryFragmentIds = new System.Collections.Generic.List<string>
                {
                    "diary_filter_is_a_lie",
                    "diary_water_truth",
                    "diary_shielding_rot"
                }
            };
            Shelter.RegisterRoom(deepVault);

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
            // Prompt #11 — Black Rain (constructed after WeatherSystem exists; see late bind).
            // WeatherSystem is created earlier in Awake — safe to construct here.
            if (WeatherSystem != null)
                BlackRainHazardSystem = new BlackRainHazardSystem(WeatherSystem);

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
                CreateAction<CaregiveActionSO>(),
                CreateAction<MentalBreakComfortActionSO>(),
                CreateAction<CraftActionSO>(),
                CreateAction<GuardActionSO>(),
                CreateAction<PedalGeneratorActionSO>(),
                CreateAction<SearchForChemsActionSO>(),
                CreateAction<ClearRubbleActionSO>()
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
            var eventPool = new List<GameEvent>();
            if (_eventCatalog != null && _eventCatalog.events != null)
                eventPool.AddRange(_eventCatalog.events);
            // Multi-stage emissary arc (trait/trust gates + day-gated follow-ups).
            // Always register factory versions so scheduleEvent ids resolve even if
            // the catalog has not been re-imported from events.json.
            EnsurePoolHasEmissaryChain(eventPool);

            // Prompt #46 — radio-triggered Safe Haven Broadcast. The radio
            // bridge in HandleRadioBroadcastTrigger raises this by id when
            // a survivor is at the dial; the pool must contain a matching
            // instance or FindInPool() returns null and the broadcast
            // surfaces nothing.
            EnsurePoolHasRadioTriggeredEvents(eventPool);
            // Internal mysteries (Missing Rations) — factory events for choice resolution.
            EnsurePoolHasMissingRationsChain(eventPool);
            // Prompt #47 — biological trade economy. The Blood for Water
            // event is registered as a faction-triggered event; the
            // bootstrap gates it on is_blood_for_water_offered (raised
            // when a faction at Rob/HostileRaid trade-stance visits the
            // hatch with an empty inventory).
            EnsurePoolHasBiologicalTradeEvents(eventPool);
            // Prompt #48 — Buried Alive + faction outside dig-out.
            EnsurePoolHasHatchEntrapmentEvents(eventPool);
            // Prompt #9 — Child Found in the Ash event.
            EnsurePoolHasChildFoundEvent(eventPool);
            EventRunner.SetPool(eventPool);

            SuspicionTracker = new SuspicionTracker();
            SuspicionTracker.Bind(EventRunner);

            // Diegetic journal — survivors write discoveries (no tutorial popups).
            // Entries run through a pool: evicted/cleared entries are recycled,
            // never collected, so 100-day fast-forward runs stay GC-flat.
            JournalSystem = new JournalSystem();
            _journalEntryPool = new GenericObjectPool<JournalEntry>(
                () => new JournalEntry(),
                e =>
                {
                    e.Id = null;
                    e.Text = null;
                    e.Timestamp = null;
                    e.AuthorName = null;
                    e.AuthorId = null;
                    e.KnowledgeKey = null;
                    e.Day = 0;
                    e.Hour = 0f;
                },
                // +1: at a full list the new entry is acquired before the
                // evicted one is released, so steady state needs cap+1 stock.
                initialCapacity: JournalSystem.MaxEntries + 1);
            JournalSystem.SetEntryFactory(_journalEntryPool.Acquire, _journalEntryPool.Release);
            JournalSystem.OnEntryAdded += entry =>
            {
                if (entry == null || string.IsNullOrEmpty(entry.Text)) return;
                Debug.Log($"[Journal] {entry.Timestamp} — {entry.AuthorName}: {entry.Text}");
                PushJournalEntryToHud(entry);
            };

            // Campaign win/loss — radio extraction + vehicle escape projects
            VictoryProject = new VictoryProjectManager();
            EndgameEngine = new EndgameEngine(GameModeKind.Story, _campaignLengthDays);
            VictoryProject.OnExtractionUnlocked += () =>
            {
                Debug.Log("[Endgame] Extraction coordinates unlocked (10 military intel). Survive to Day 100.");
            };
            VictoryProject.OnEndgameTriggered += summary =>
            {
                if (summary == null) return;
                ApplyEndgame(summary);
            };

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

            // ───────────────────────────────────────────────────────────
            // Prompt #10 — Skill Atrophy System
            // ───────────────────────────────────────────────────────────
            SkillAtrophy = new SkillAtrophySystem();

            // ───────────────────────────────────────────────────────────
            // Prompt #8 — Empath & Sociopath System
            // ───────────────────────────────────────────────────────────
            EmpathSystem = new EmpathSystem();
            // Wire death hook into NeedsSystem.OnDied
            NeedsSystem.OnDied += deceased =>
            {
                EmpathSystem.OnSurvivorDied(deceased, Survivors);
                ChildSystem?.CheckChildDeath(Survivors);
            };

            // ───────────────────────────────────────────────────────────
            // Prompt #7 — Addiction & Withdrawal System
            // ───────────────────────────────────────────────────────────
            Addiction = new AddictionSystem(new System.Random(_worldSeed + 71));
            // Register addictive items from the catalog
            if (_itemCatalog != null)
            {
                // Register known addictive item ids
                string[] addictiveIds = { "morphine", "anti_rad", "painkiller", "stimulant" };
                foreach (var id in addictiveIds)
                {
                    var item = _itemCatalog.GetById(id);
                    if (item != null)
                        Addiction.RegisterAddictiveItem(item.id);
                }
            }
            Addiction.RegisterAddictiveItem("morphine");
            Addiction.RegisterAddictiveItem("anti_rad");
            Addiction.PanicDestroyHandler = (sv, rng) => ForceAddictionPanicDestroy(sv, rng);

            // Wire medical treatment pathway into addiction tracking
            if (MedicalSystem != null)
            {
                MedicalSystem.GetCurrentDay = () => TimeSystem != null ? TimeSystem.CurrentDay : 1;
                MedicalSystem.OnTreatmentItemConsumed = (sv, itemId, day) =>
                {
                    Addiction?.OnItemConsumed(sv, itemId, day);
                };
            }

            // ───────────────────────────────────────────────────────────
            // Prompt #6 — Phantom Intruders System
            // ───────────────────────────────────────────────────────────
            PhantomIntruders = new PhantomIntruderSystem();
            PhantomIntruders.ConsumeAmmoHandler = amount =>
            {
                if (Inventory == null || _itemCatalog == null) return false;
                // Try common ammo types
                var ammoTypes = new[] { "ammo_9mm", "ammo_shotgun", "ammo_rifle" };
                foreach (var ammoId in ammoTypes)
                {
                    var def = _itemCatalog.GetById(ammoId);
                    if (def != null && Inventory.Remove(def, amount)) return true;
                }
                return false;
            };
            PhantomIntruders.OnWeaponFiredHandler = () =>
            {
                Debug.Log("[Phantom Intruder] Weapon fired at the hatch door!");
            };
            PhantomIntruders.OnPhantomIntruderTriggered += paranoid =>
            {
                Debug.Log($"[Phantom Intruder] {paranoid.DisplayName} sees a Hatch Breach that isn't there!");
            };
            PhantomIntruders.OnPhantomIntruderResolved += paranoid =>
            {
                Debug.Log($"[Phantom Intruder] {paranoid.DisplayName} realizes nothing was out there.");
            };

            // ───────────────────────────────────────────────────────────
            // Prompt #9 — The Child Dependent System
            // ───────────────────────────────────────────────────────────
            ChildSystem = new ChildDependentSystem();
            ChildSystem.ConsumeChildRationsHandler = (food, water) =>
            {
                if (Inventory == null || _itemCatalog == null) return;
                var foodItem = _itemCatalog.GetById("canned_food");
                if (foodItem != null) Inventory.Remove(foodItem, Mathf.CeilToInt(food / 20f));
                var waterItem = _itemCatalog.GetById("clean_water");
                if (waterItem != null) Inventory.Remove(waterItem, Mathf.CeilToInt(water / 20f));
            };
            ChildSystem.OnChildFound += child =>
            {
                if (Survivors != null)
                {
                    Survivors.Add(child);
                    NeedsSystem.Register(child);
                }
                Debug.Log("[Child] The child has been found and brought into the bunker. Hope rises.");
            };
            ChildSystem.OnChildDied += _ =>
            {
                Debug.Log("[Child] The child has died. The bunker's hope shatters.");
                if (SaveSystem != null)
                    SaveSystem.SetWorldFlag(ChildDependentSystem.ChildDiedFlag, true);
            };

            // ───────────────────────────────────────────────────────────
            // Prompt #5 — Diary Fragment Catalog (Previous Tenants)
            // ───────────────────────────────────────────────────────────
            DiaryCatalog = new List<DiaryFragmentSO>();
            // Load diary fragments from Resources or StreamingAssets
            var loadedDiaries = Resources.LoadAll<DiaryFragmentSO>("Diaries");
            if (loadedDiaries != null && loadedDiaries.Length > 0)
            {
                DiaryCatalog.AddRange(loadedDiaries);
            }
            // If no authored diaries exist, create default ones inline so the
            // rubble-clearing system has content to reveal.
            if (DiaryCatalog.Count == 0)
            {
                DiaryCatalog.Add(CreateDefaultDiary("diary_filter_is_a_lie", "Torn Notebook Page",
                    "The filter is a lie. I watched them install it. It doesn't purify anything — " +
                    "it just pushes the radon deeper into the vents. The reading at the intake looks " +
                    "clean because it bypasses the sensor. We've been breathing poison for three weeks. " +
                    "I don't know how to tell the others. — M.",
                    "M.", "deep_vault", "air_filtration", 0, 3));

                DiaryCatalog.Add(CreateDefaultDiary("diary_water_truth", "Water-Stained Journal",
                    "The catchment on the roof is cracked. Has been since the first mortar. " +
                    "Every time it rains, we cheer — but the water tastes like metal and the " +
                    "geiger clicks faster every time we boil it. I tried to patch it last week " +
                    "but the suit tore and I couldn't stay out there. The crack is getting wider. " +
                    "— Unknown",
                    "Unknown", "deep_vault", "water_purifier", 1, 3));

                DiaryCatalog.Add(CreateDefaultDiary("diary_shielding_rot", "Last Entry of the Engineer",
                    "The shielding in the deep vault was never finished. They poured half the " +
                    "concrete and ran out of aggregate. The plans say six inches. There's maybe " +
                    "two. I've been sleeping against the wrong wall for a month. The skin on " +
                    "my back is peeling and I don't think it's just dry air anymore. " +
                    "If you're reading this — check the east wall. Check it with a dosimeter, " +
                    "not the panel. The panel lies. — Engineer Kostya",
                    "Engineer Kostya", "deep_vault", "radiation_shielding", 2, 3));
            }
            // Wire diary reveal into JournalSystem (simplified — logs via debug; full
            // JournalSystem integration can use AddEntryFactory when needed)
            var clearRubbleAction = Actions.Find(a => a is ClearRubbleActionSO) as ClearRubbleActionSO;
            if (clearRubbleAction != null)
            {
                clearRubbleAction.OnDiaryRevealed = (roomId, fragmentIndex) =>
                {
                    if (DiaryCatalog != null)
                    {
                        foreach (var diary in DiaryCatalog)
                        {
                            if (diary != null && diary.foundInRoomId == roomId && diary.pageOrder == fragmentIndex && !diary.IsFound)
                            {
                                diary.IsFound = true;
                                Debug.Log($"[Diary] Found in {roomId}: \"{diary.title}\" — {diary.text}");
                                return diary.text;
                            }
                        }
                    }
                    return null;
                };
            }

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
            // Cult of the Glow (trustInversion): disposition tracks party radiation dose.
            EconomySystem.SetPartyRadiationProvider(GetPartyAverageRadiationDose);
            // #16 polish: ARS reverence + intact-hazmat contempt providers.
            EconomySystem.SetPartyHasArsProvider(PartyHasAcuteRadiationSyndrome);
            EconomySystem.SetPartyIntactHazmatProvider(PartyWearsIntactHazmat);
            EconomySystem.BindEventRunner(EventRunner);

            // Post-repel parley modal + faction radio intercept log
            ParleyOfferPromptField = new ParleyOfferPrompt();
            FactionRadioIntercepts = new FactionRadioInterceptSystem();
            FactionRadioIntercepts.Bind(
                EconomySystem,
                () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EconomySystem.OnRaidResolved += OnFactionRaidResolved_Handle;
            FactionRadioIntercepts.OnIntercept += entry =>
            {
                if (entry == null || string.IsNullOrEmpty(entry.Message)) return;
                Debug.Log($"[Radio intercept] {entry.Message}");
                PushRadioInterceptToHud(entry);
            };

            // Prompt #17 — inter-faction raid plans / wiretap choices.
            // Antenna gate uses RadioTunerSystem once it is constructed later in
            // InitializeSystems; the provider reads live state each call.
            // Map is rebound after GeneratedMap is created below.
            FactionRaidPlanSystem = new FactionRaidPlanSystem(new System.Random(_worldSeed + 21));
            FactionRaidPlanSystem.Bind(
                EconomySystem,
                FactionRadioIntercepts,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                isAntennaOperational: IsWiretapAntennaOperational,
                map: null);
            FactionRaidPlanSystem.OnInterceptOffered += HandleRaidPlanInterceptOffered;

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
            FactionRaidPlanSystem?.SetMap(GeneratedMap);

            // Knowledge map must exist before SaveSystem can capture it
            KnowledgeMap = new RadiationKnowledgeMap();
            SeedKnowledgeMap();

            // Save
            SaveSystem = new SaveSystem(
                GameState, WeatherSystem, TemperatureSystem, NeedsSystem,
                RadiationSystem, Shelter, () => Survivors,
                id =>
                {
                    var fromCatalog = _itemCatalog?.GetById(id);
                    if (fromCatalog != null) return fromCatalog;
                    // Prompt #13 — poisoned iodine may only exist as a runtime plant.
                    if (SabotagedCacheSystem != null
                        && string.Equals(id, SabotagedCacheSystem.PoisonedIodineItemId,
                            System.StringComparison.OrdinalIgnoreCase))
                        return SabotagedCacheSystem.PoisonedIodineDefinition;
                    return null;
                },
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
            SaveSystem.SetFactionRadioIntercepts(FactionRadioIntercepts);
            SaveSystem.SetFactionRaidPlanSystem(FactionRaidPlanSystem);
            SaveSystem.SetJournalSystem(JournalSystem);
            SaveSystem.SetVictoryProjectManager(VictoryProject);
            SaveSystem.SetEventRunner(EventRunner);
            SaveSystem.SetSuspicionTracker(SuspicionTracker);
            SaveSystem.SetPreCaptureHook(SnapshotRadioHudToInterceptSystem);
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

            // Prompt #13 — hostile factions learn scavenging habits and plant
            // poisoned medical crates. High Medical skill / Paranoid spots them.
            SabotagedCacheSystem = new SabotagedCacheSystem(new System.Random(_worldSeed + 19));
            SabotagedCacheSystem.BindEconomy(EconomySystem);
            SabotagedCacheSystem.SetPoisonedIodineDefinition(
                SabotagedCacheSystem.CreatePoisonedIodineDefinition());
            ExpeditionSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            SaveSystem.SetSabotagedCacheSystem(SabotagedCacheSystem);
            ExpeditionSystem.OnSabotagedCacheDetected += (exp, msg) =>
            {
                Debug.Log($"[Sabotaged Cache] Detected by {exp?.Survivor?.DisplayName}: {msg}");
            };

            // Prompt #14 — post-Day-30 windstorms move death-zone rad two path-hops.
            ShiftingHotspotSystem = new ShiftingHotspotSystem(new System.Random(_worldSeed + 20));
            ShiftingHotspotSystem.Bind(GeneratedMap, KnowledgeMap);
            SaveSystem.SetShiftingHotspotSystem(ShiftingHotspotSystem);
            ShiftingHotspotSystem.OnHotspotShifted += shift =>
            {
                if (shift == null) return;
                Debug.Log(
                    $"[Shifting Hotspot] Windstorm day {shift.Day}: " +
                    $"{shift.FromNodeId} → {shift.ToNodeId} " +
                    $"(moved {shift.MovedRad:F0} rad/hr)");
                RefreshMapKnowledgeHUD();
            };

            // Prompt #48 — weather buries/freezes the hatch after 72 continuous
            // hours of Blizzard/FalloutStorm; DigOut spikes entry CO2; broken
            // air filter while sealed starts suffocation countdown.
            HatchEntrapmentSystem = new HatchEntrapmentSystem();
            _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
            HatchEntrapmentSystem.OnHatchStateChanged += (prev, next) =>
            {
                SyncHatchExpeditionLock();
                Debug.Log($"[Hatch Entrapment] HatchState {prev} → {next}");
            };
            HatchEntrapmentSystem.OnBuriedAliveTriggered += () =>
            {
                // Present the Buried Alive event immediately when the seal lands.
                if (EventRunner == null) return;
                var buried = EventRunner.FindInPool(EventRunner.BuriedAliveEventId)
                             ?? EventRunner.CreateBuriedAliveEvent();
                var ctx = BuildEventContext(TimeSystem != null ? TimeSystem.CurrentDay : 1);
                ctx.SetEventFlag(HatchEntrapmentSystem.FlagBuriedAliveOffered, true);
                if (buried != null && buried.CanTrigger(ctx))
                    EventRunner.Run(buried, ctx);
            };
            SaveSystem.SetHatchEntrapment(HatchEntrapmentSystem);
            SaveSystem.SetChildDependentSystem(ChildSystem);
            SyncHatchExpeditionLock();

            // ───────────────────────────────────────────────────────────
            // Internal Horror — atmosphere / corpses / pantry rust
            // ───────────────────────────────────────────────────────────
            _storesRoom = new ShelterRoom("stores", null);
            AtmosphereSystem = new ShelterAtmosphereSystem(new System.Random(_worldSeed + 16));
            AtmosphereSystem.RegisterRoom(_entryRoom);
            AtmosphereSystem.RegisterRoom(_storesRoom);
            AtmosphereSystem.RegisterRoom(new ShelterRoom("quarters", null));
            AtmosphereSystem.RegisterRoom(new ShelterRoom("plant", null));

            CorpseSystem = new CorpseManagementSystem(
                NeedsSystem, Inventory, MedicalSystem, RadiationSystem,
                new System.Random(_worldSeed + 17));
            CorpseSystem.SetItemDefinitions(
                CorpseManagementSystem.CreateCorpseDefinition(),
                CorpseManagementSystem.CreateFertilizerDefinition());
            CorpseSystem.SetStoresRoom(_storesRoom);
            CorpseSystem.SetSurvivorProvider(() => Survivors);
            CorpseSystem.BindDeathHandler();

            PantrySystem = new PantryContaminationSystem(
                Inventory, new System.Random(_worldSeed + 18));
            PantrySystem.SetContaminatedFoodDefinition(
                PantryContaminationSystem.CreateContaminatedFoodDefinition());
            PantrySystem.SetStoresRoom(_storesRoom);

            SaveSystem.SetAtmosphereSystem(AtmosphereSystem);
            SaveSystem.SetCorpseSystem(CorpseSystem);
            SaveSystem.SetPantrySystem(PantrySystem);

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
                Inventory.OnInventoryChanged += RefreshInventoryStrip;
            }
            if (KnowledgeMap != null)
            {
                KnowledgeMap.OnKnowledgeChanged += RefreshMapKnowledgeHUD;
            }

            // Radio (broadcast only; tuner/intel extraction is separate)
            RadioSystem = new RadioBroadcastSystem();
            RadioSystem.SetCatalog(_radioCatalog);
            
            // Radio Tuner System (intel extraction)
            RadioTunerSystem = new RadioTunerSystem(
                new System.Random(_worldSeed + 31),
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            InitializeRadioFrequencies();
            
            // Wire up radio module fuel supply to RadioTunerSystem
            var radioModule = Shelter.GetModule("radio");
            if (radioModule != null && radioModule.IsOperational)
            {
                RadioTunerSystem.State.AvailableFuel = radioModule.Fuel;
                RadioTunerSystem.State.PowerConsumptionPerHour = 0.5f; // Default consumption
            }

            // Prompt #18 — Debt Collector (day+20 after faction dig-out).
            // Constructed after RadioTuner so antenna cut can EMP the live RadioState.
            DebtCollectorSystem = new DebtCollectorSystem();
            DebtCollectorSystem.Bind(
                EconomySystem,
                FactionRadioIntercepts,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                shelter: Shelter,
                water: WaterStorage,
                inventory: Inventory,
                radioState: RadioTunerSystem?.State);
            if (HatchEntrapmentSystem != null)
                HatchEntrapmentSystem.OnFactionRescueApplied += HandleFactionRescueApplied_ScheduleDebt;
            DebtCollectorSystem.OnCollectorArrived += HandleDebtCollectorArrived;
            SaveSystem.SetDebtCollectorSystem(DebtCollectorSystem);

            // Prompt #19 — Ghost Stations (unlock after EMP; never live/extraction intel).
            GhostStationSystem = new GhostStationSystem();
            GhostStationSystem.Bind(
                RadioTunerSystem,
                JournalSystem,
                getSurvivors: () => Survivors,
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0);
            EventBus.Subscribe<FlashpointEmptiedDevices>(OnFlashpointEmp_UnlockGhosts);
            SaveSystem.SetGhostStationSystem(GhostStationSystem);

            // Prompt #20 — Lifeboat Transmission (late-game single-seat extraction).
            LifeboatTransmissionSystem = new LifeboatTransmissionSystem();
            LifeboatTransmissionSystem.Bind(
                getDay: () => TimeSystem != null ? TimeSystem.CurrentDay : 0,
                getSurvivors: () => Survivors,
                isCampaignTerminal: () =>
                    (VictoryProject != null && VictoryProject.IsTerminal)
                    || (EndgameEngine != null && EndgameEngine.Result.IsTerminal)
                    || IsGameOver,
                endgame: EndgameEngine,
                victory: VictoryProject);
            LifeboatTransmissionSystem.OnContactOffered += HandleLifeboatContactOffered;
            SaveSystem.SetLifeboatTransmissionSystem(LifeboatTransmissionSystem);
            
            // Wire up intel extraction events
            RadioTunerSystem.OnIntelExtracted += intel =>
            {
                Debug.Log($"[Radio] Extracted intel: {intel.Type} - {intel.Text}");
                // Ghost loops intentionally skip VictoryProject / plume map paths.
                if (intel != null && intel.Type == IntelType.GhostLoop) return;
                VictoryProject?.NotifyIntel(intel);
                if (intel.Type == IntelType.PlumeReport)
                {
                    // Apply plume reports to knowledge map + proc-gen node reveal
                    RadioTunerSystem.ApplyPlumeReportToMap(intel, KnowledgeMap, GeneratedMap);
                    RefreshMapKnowledgeHUD();
                    _hud?.MapScreenUI?.Refresh();
                }
            };

            // Prompt #46 — Radio-triggered GameEvents. When a broadcast with
            // a non-empty triggerEventId plays AND a survivor is currently
            // at the radio (IsOnRadio on the EventContext), raise the named
            // event through the standard EventRunner path. This is how the
            // Safe Haven broadcast surfaces as a player choice: the radio
            // plays the loop, the player is at the dial, the event fires.
            RadioSystem.OnBroadcastStarted += HandleRadioBroadcastTrigger;
            EventRunner.OnChoiceApplied += HandleSafeHavenChoiceApplied;
            EventRunner.OnChoiceApplied += HandleBloodForWaterChoiceApplied;
            EventRunner.OnChoiceApplied += HandleHatchEntrapmentChoiceApplied;
            EventRunner.OnChoiceApplied += HandleChildFoundChoiceApplied;
            EventRunner.OnChoiceApplied += HandleRaidPlanChoiceApplied;
            EventRunner.OnChoiceApplied += HandleDebtCollectorChoiceApplied;
            EventRunner.OnChoiceApplied += HandleLifeboatChoiceApplied;

            TimeSystem.OnDayTick += day =>
            {
                RadioSystem.CheckForBroadcast(day);
                Inventory?.DriftAllDevices(1f);
                KnowledgeMap?.TickDay(day);
                // Prompt #14 — rare windstorm may move a death-zone after Day 30.
                ShiftingHotspotSystem?.TickDay(day);
                // Prompt #17 — latent inter-faction raid plans + wiretap window.
                FactionRaidPlanSystem?.TickDay(day);
                // Prompt #18 — delayed dig-out debt collectors.
                DebtCollectorSystem?.TickDay(day);
                // Prompt #20 — late-game lifeboat contact (Day ≥ 80).
                LifeboatTransmissionSystem?.TickDay(day, Survivors);
                RefreshMapKnowledgeHUD();
                // Radio win path: extraction coords + survive to Day 100.
                VictoryProject?.TickDay(day, Survivors);
                // Multi-stage narrative chains (Prompt #43): fire day-gated follow-ups.
                if (EventRunner != null)
                {
                    var dayCtx = BuildEventContext(day);
                    EventRunner.TickDay(day, dayCtx);
                }
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
                SaveSystem.SetPhantomIntruderSystem(PhantomIntruders);
            }
        }

        private void InitializeRadioFrequencies()
        {
            // Create default frequencies. interceptChannelTag links each band to
            // faction intercept filtering (intel extraction uses the same dial).
            var civilian = ScriptableObject.CreateInstance<RadioFrequencySO>();
            civilian.id = RadioFrequencySO.Ids.Civilian;
            civilian.displayName = "88.5 FM Civilian";
            civilian.frequencyMHz = 88.5f;
            civilian.type = RadioFrequencyType.Civilian;
            civilian.activeFromDay = 0;
            civilian.activeUntilDay = 30;
            civilian.baseSignalStrength = 0.7f;
            civilian.interferenceSusceptibility = 0.3f;
            civilian.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Civilian);

            var military = ScriptableObject.CreateInstance<RadioFrequencySO>();
            military.id = RadioFrequencySO.Ids.Military;
            military.displayName = "102.1 Military";
            military.frequencyMHz = 102.1f;
            military.type = RadioFrequencyType.Military;
            military.activeFromDay = 0;
            military.activeUntilDay = 30;
            military.baseSignalStrength = 0.6f;
            military.interferenceSusceptibility = 0.2f;
            military.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Military);

            var numbers = ScriptableObject.CreateInstance<RadioFrequencySO>();
            numbers.id = RadioFrequencySO.Ids.Numbers;
            numbers.displayName = "99.0 Numbers Station";
            numbers.frequencyMHz = 99.0f;
            numbers.type = RadioFrequencyType.NumbersStation;
            numbers.activeFromDay = 31;
            numbers.activeUntilDay = -1;
            numbers.baseSignalStrength = 0.4f;
            numbers.interferenceSusceptibility = 0.5f;
            numbers.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.NumbersStation);

            var emergency = ScriptableObject.CreateInstance<RadioFrequencySO>();
            emergency.id = RadioFrequencySO.Ids.Emergency;
            emergency.displayName = "107.0 Emergency";
            emergency.frequencyMHz = 107.0f;
            emergency.type = RadioFrequencyType.Emergency;
            emergency.activeFromDay = 31;
            emergency.activeUntilDay = -1;
            emergency.baseSignalStrength = 0.5f;
            emergency.interferenceSusceptibility = 0.4f;
            emergency.interceptChannelTag = RadioFrequencySO.DefaultChannelTagForType(RadioFrequencyType.Emergency);

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
                // Prompt #19 — ghost bands appear in the static after EMP.
                GhostStationSystem?.NotifyEmpOccurred();
                return;
            }

            FlashpointChoreographer.OnNuclearExchange();
        }

        /// <summary>Flashpoint EMP step → unlock ghost stations (Prompt #19).</summary>
        private void OnFlashpointEmp_UnlockGhosts(FlashpointEmptiedDevices _)
        {
            GhostStationSystem?.NotifyEmpOccurred();
        }

        // -----------------------------------------------------------------
        // Prompt #20 — Lifeboat Transmission
        // -----------------------------------------------------------------

        private void HandleLifeboatContactOffered(GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            ctx.SetEventFlag(LifeboatTransmissionSystem.FlagContacted, true);
            EventRunner.Run(ev, ctx);
            Debug.Log("[Lifeboat] Two-way contact. One seat. Choose who walks.");
        }

        private void HandleLifeboatChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || LifeboatTransmissionSystem == null) return;
            if (!string.Equals(ev.id, LifeboatTransmissionSystem.EventId, StringComparison.Ordinal))
                return;
            if (LifeboatTransmissionSystem.ApplyChoiceFromEvent(ev, choice, ctx))
            {
                Debug.Log(
                    $"[Lifeboat] Sent {LifeboatTransmissionSystem.ExtractedSurvivorName}. " +
                    $"{LifeboatTransmissionSystem.LeftBehindIds.Count} left behind.");
                // VictoryProject.OnEndgameTriggered → ApplyEndgame already wired.
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #46 — Radio → EventRunner bridge + Safe Haven ambush wiring.
        // The radio is a narrative tool, not just an intel sink: broadcasts
        // with a triggerEventId surface as player choices (send the team,
        // analyze the audio, warn other wastelanders) — and a careless
        // expedition on a Trap broadcast is a casualty-producing decision.
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// Radio-broadcast listener: when a broadcast with a triggerEventId
        /// plays AND a survivor is at the radio, raise the named event
        /// through EventRunner.Run. Mirrors the standard hourly event tick
        /// but uses a context tagged with <c>IsOnRadio=true</c> so the
        /// event's RequiredFlagId gate resolves and the modal fires.
        /// </summary>
        private void HandleRadioBroadcastTrigger(RadioBroadcastSO broadcast)
        {
            if (broadcast == null || string.IsNullOrEmpty(broadcast.triggerEventId)) return;
            if (EventRunner == null) return;

            // The player must be at the radio for the broadcast to surface
            // as an interactive choice. Without IsOnRadio, the event stays
            // in the pool — the loop is just audio flavor.
            bool anyoneAtRadio = false;
            if (Survivors != null)
            {
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var s = Survivors[i];
                    if (s == null || !s.IsAlive) continue;
                    // The listen-to-radio AI action sets CurrentRoomId to the
                    // radio station; in test scenes we accept the flag as well.
                    if (s.CurrentRoomId == "radio" || s.CurrentRoomId == "radio_station")
                    {
                        anyoneAtRadio = true;
                        break;
                    }
                }
            }
            if (!anyoneAtRadio) return;

            // Build a context tagged with IsOnRadio and the broadcast's id
            // so the named event can also gate on a per-broadcast flag.
            var ctx = BuildEventContext(TimeSystem != null ? TimeSystem.CurrentDay : 1);
            ctx.IsOnRadio = true;
            ctx.SetEventFlag("is_on_radio", true);
            ctx.SetEventFlag("broadcast_" + broadcast.id, true);

            // Prompt #47 — the medical convoy broadcast also opens the
            // Blood for Water gate. We do this here (rather than in the
            // event's CanTrigger) so the gate stays decoupled from the
            // radio bridge: the convoy can also be triggered by a
            // hatch-visit faction event in the future without code
            // changes to the radio path.
            if (broadcast.id == "medical_convoy_announcement")
            {
                ctx.SetEventFlag("is_blood_for_water_offered", true);
            }

            // Default reliability is Unverified; the player must verify
            // (or get ambushed) to flip it.
            ctx.ActiveIntelReliability = IntelReliability.Unverified;

            // Find the event by id; if it's already in the pool just Run it.
            var ev = EventRunner.FindInPool(broadcast.triggerEventId);
            if (ev == null)
            {
                Debug.LogWarning($"[GameBootstrap] Radio broadcast '{broadcast.id}' wants event " +
                                 $"'{broadcast.triggerEventId}' but it is not in the EventRunner pool.");
                return;
            }
            EventRunner.Run(ev, ctx);
        }

        /// <summary>
        /// EventRunner.OnChoiceApplied listener: resolves side effects of the
        /// Safe Haven Broadcast event. Specifically:
        ///  - <c>warn_others</c>: drains 5 fuel from the radio tuner (transmission
        ///    cost) and boosts trust with every registered faction by +3.
        ///  - <c>send_expedition</c>: if the broadcast was NOT verified as a
        ///    trap first, injects the Safe Haven ambush encounter into the
        ///    ExpeditionSystem so the next expedition to grid 4-7-North hits
        ///    a pre-positioned sniper. If the broadcast WAS verified, the
        ///    encounter pool is left clean — the player can scavenge the
        ///    empty cache without casualties.
        ///  - <c>analyze_audio</c> / <c>analyze_audio_science</c>: flips the
        ///    EventContext's ActiveIntelReliability to Trap on the running
        ///    context so downstream choices inherit the new reliability.
        /// </summary>
        private void HandleSafeHavenChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != EventRunner.SafeHavenBroadcastEventId) return;

            if (choice.ChoiceId == "warn_others")
            {
                // Transmission cost: pull from the radio tuner's fuel reserve.
                if (RadioTunerSystem != null && RadioTunerSystem.State != null)
                {
                    RadioTunerSystem.State.AvailableFuel = Mathf.Max(
                        0f, RadioTunerSystem.State.AvailableFuel - 5f);
                }
                // Karma/trust boost: every registered faction gets +3 trust.
                if (EconomySystem != null && EconomySystem.Factions != null)
                {
                    foreach (var fac in EconomySystem.Factions.Values)
                    {
                        if (fac == null) continue;
                        EconomySystem.ModifyTrust(fac.id, 3f);
                    }
                }
                Debug.Log("[Safe Haven] Broadcast warning transmitted. Radio fuel -5, all factions +3 trust.");
                return;
            }

            if (choice.ChoiceId == "analyze_audio" || choice.ChoiceId == "analyze_audio_science")
            {
                // Reliability flip is applied by EventRunner.ApplySafeHavenIntelEffects
                // during ApplyChoice (before this handler). Log only here.
                Debug.Log("[Safe Haven] Audio analyzed: the scrubber hum is a recorded loop. Trap confirmed.");
                return;
            }

            if (choice.ChoiceId == "send_expedition")
            {
                // Prompt #47 — radio intel reliability drives which location
                // encounter is injected for the Safe Haven grid.
                if (EventRunner.ShouldInjectSafeHavenAmbush(ctx))
                {
                    InjectSafeHavenAmbushEncounter();
                    Debug.Log("[Safe Haven] Unverified intel accepted. Sniper ambush injected at grid 4-7-North.");
                }
                else
                {
                    InjectSafeHavenEmptyCacheEncounter();
                    Debug.Log("[Safe Haven] Trap confirmed. Empty-cache encounter injected — no sniper.");
                }
                return;
            }
        }

        /// <summary>
        /// Inject location-bound sniper ambush (Unverified send). forceOnArrival
        /// guarantees the beat fires when the expedition reaches the grid.
        /// </summary>
        private void InjectSafeHavenAmbushEncounter()
        {
            if (ExpeditionSystem == null) return;
            ExpeditionSystem.AddEncounter(SafeHavenEncounters.CreateAmbush());
        }

        /// <summary>
        /// Inject empty-cache discovery after the player analyzed the loop.
        /// </summary>
        private void InjectSafeHavenEmptyCacheEncounter()
        {
            if (ExpeditionSystem == null) return;
            ExpeditionSystem.AddEncounter(SafeHavenEncounters.CreateEmptyCache());
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #47 — Blood for Water: link DynamicEconomy (#25) to
        // MedicalSystem (#24). When a faction convoy visits the hatch with
        // an empty inventory and demands biological payment, the choice
        // here inflicts the actual BloodLossAffliction on the donor and,
        // if the choice was forced, slams the affinity matrix.
        // ─────────────────────────────────────────────────────────────────

        /// <summary>
        /// EventRunner.OnChoiceApplied listener for the Blood for Water
        /// event. Inflicts <c>BloodLoss</c> on the donor survivor (resolved
        /// via <see cref="EventRunner.FindBloodDonor"/>) and, on a forced
        /// bleed, slams the donor's affinity with the bunker leader to
        /// <see cref="EventRunner.ForcedBleedAffinityFloor"/> so
        /// MentalBreakSystem can fire a ViolentParanoia break.
        /// </summary>
        private void HandleBloodForWaterChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != EventRunner.BloodForWaterEventId) return;

            // Refuse / ignore: nothing to inflict, the trust delta was
            // already applied by the runner via the choice's FactionId +
            // TrustDelta path.
            if (choice.ChoiceId == "refuse_convoy") return;
            if (choice.ChoiceId == "ignore_summons") return;

            // Resolve the donor. The choice text doesn't carry a survivor
            // id (the bunker has multiple, the player can pick). For the
            // bootstrap, prefer the explicit PrimarySurvivor (the UI sets
            // it to the highlighted donor); fall back to the union of the
            // event's two gates (Fatalist first, then any non-Paranoid).
            var donor = ctx != null ? ctx.PrimarySurvivor : null;
            if (donor == null || !donor.IsAlive)
            {
                donor = EventRunner.FindBloodDonor(Survivors);
            }
            if (donor == null || !donor.IsAlive)
            {
                Debug.LogWarning("[Blood for Water] No eligible donor in the bunker; skipping BloodLoss inflict.");
                return;
            }

            // Inflict the affliction. MedicalSystem.Inflict is a no-op if
            // the def is unknown or the survivor already has it.
            if (MedicalSystem != null)
            {
                bool applied = MedicalSystem.Inflict(donor, AfflictionSO.Ids.BloodLoss);
                if (!applied)
                {
                    Debug.LogWarning($"[Blood for Water] MedicalSystem.Inflict returned false for {donor.Id}.");
                }
                else
                {
                    Debug.Log($"[Blood for Water] BloodLoss inflicted on {donor.DisplayName}.");
                }
            }

            // Forced bleed: slam the donor's affinity with the bunker leader
            // (the highest-trust living survivor, or donor themselves if
            // alone) to the ForcedBleedAffinityFloor. MentalBreakSystem
            // reads this matrix in its roll; -100 is the input that
            // maximises a Paranoid survivor's chance of a ViolentParanoia
            // break.
            if (choice.ChoiceId == "bleed_paranoid_force"
                && ctx != null
                && ctx.MentalBreak != null
                && MentalBreakSystem != null)
            {
                Survivor leader = ResolveBunkerLeader();
                if (leader != null && leader != donor)
                {
                    MentalBreakSystem.Affinity.Set(
                        donor.Id, leader.Id,
                        EventRunner.ForcedBleedAffinityFloor);
                    Debug.Log($"[Blood for Water] Affinity {donor.DisplayName}↔{leader.DisplayName} slammed to {EventRunner.ForcedBleedAffinityFloor}.");
                }
            }
        }

        /// <summary>
        /// Pick the survivor to treat as the "leader" of the bunker for
        /// affinity bookkeeping. Defaults to the first living survivor
        /// (matches the convention used by MentalBreakSystem); falls back
        /// to the donor if they are the only living survivor.
        /// </summary>
        private Survivor ResolveBunkerLeader()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s != null && s.IsAlive) return s;
            }
            return null;
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
            KnowledgeMap.GetAllPlayerViews(_knowledgeViewBuffer, day, hasGeiger);
            int calAge = -1;
            var geiger = Inventory?.GetBestGeigerState();
            if (geiger != null)
            {
                calAge = InstrumentDevice.DaysSinceCalibration(geiger, day);
            }
            _hud.OnMapKnowledgeUpdated(_knowledgeViewBuffer, hasGeiger, calAge);
        }

        /// <summary>Resync the pooled inventory icon strip from live stock.</summary>
        private void RefreshInventoryStrip()
        {
            if (_hud == null) return;
            var strip = _hud.InventoryStripUI;
            if (strip != null)
                strip.Sync(Inventory);
            // Corpse / rusted-can counts also drive the Internal Horror strip.
            RefreshInternalHorrorHud();
        }

        /// <summary>Room ids we already auto-prompted for (avoid reopening every frame).</summary>
        private readonly HashSet<string> _fireAlertShownRooms = new HashSet<string>();

        /// <summary>
        /// Push corpse / fire / coma / contaminated-food state into InternalHorrorHUD.
        /// Safe when systems are not yet constructed.
        /// </summary>
        private void RefreshInternalHorrorHud()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;

            var snap = BuildInternalHorrorSnapshot();
            horror.ApplySnapshot(snap);

            // Auto-open fire panel once per room when a new blaze starts.
            if (snap?.Fires != null)
            {
                var live = new HashSet<string>();
                for (int i = 0; i < snap.Fires.Length; i++)
                {
                    var f = snap.Fires[i];
                    if (f == null || !f.IsOnFire || string.IsNullOrEmpty(f.RoomId)) continue;
                    live.Add(f.RoomId);
                    if (_fireAlertShownRooms.Add(f.RoomId) && !horror.IsFirePanelOpen)
                        horror.OpenFirePanel(f.RoomId);
                }
                // Drop rooms that are no longer on fire so a re-ignition re-prompts.
                _fireAlertShownRooms.RemoveWhere(id => !live.Contains(id));
            }
            else
            {
                _fireAlertShownRooms.Clear();
            }
        }

        private AtomicWar._Game.UI.InternalHorrorSnapshot BuildInternalHorrorSnapshot()
        {
            var snap = new AtomicWar._Game.UI.InternalHorrorSnapshot
            {
                CareIntervalHours = AtomicWar._Game.Medical.MedicalSystem.ComaCareIntervalHours
            };

            // Corpses
            int corpses = CorpseSystem != null
                ? CorpseSystem.CorpseCount
                : (Inventory != null ? Inventory.CountByType(ItemType.Corpse) : 0);
            snap.CorpseCount = corpses;
            float daylight = PhotoperiodSystem != null
                ? PhotoperiodSystem.EffectiveDaylightHours
                : 8f;
            snap.DaylightHoursAvailable = daylight;
            snap.CanBury = corpses > 0 && daylight >= CorpseManagementSystem.BuryHours
                && FindFirstLivingSurvivor() != null;

            // Contaminated food
            snap.ContaminatedFoodCount = Inventory != null
                ? Inventory.CountByType(ItemType.ContaminatedFood)
                : 0;

            // Fires
            if (AtmosphereSystem != null && AtmosphereSystem.Rooms != null)
            {
                var fireList = new List<AtomicWar._Game.UI.FireRoomSnapshot>();
                var rooms = AtmosphereSystem.Rooms;
                for (int i = 0; i < rooms.Count; i++)
                {
                    var r = rooms[i];
                    if (r == null || !r.IsOnFire) continue;
                    fireList.Add(new AtomicWar._Game.UI.FireRoomSnapshot
                    {
                        RoomId = r.RoomId,
                        IsOnFire = true,
                        Intensity = r.FireIntensity,
                        OxygenFraction = r.OxygenFraction,
                        LocalCoPpm = r.LocalCoPpm,
                        BulkheadSealed = r.BulkheadSealed
                    });
                }
                snap.Fires = fireList.ToArray();
            }

            // Coma patients
            if (MedicalSystem != null && Survivors != null)
            {
                var comaList = new List<AtomicWar._Game.UI.ComaPatientSnapshot>();
                bool anyUrgent = false;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    var sv = Survivors[i];
                    if (sv == null || !sv.IsAlive) continue;
                    if (!MedicalSystem.IsComatose(sv)) continue;
                    float sinceCare = 0f;
                    var active = MedicalSystem.GetActive(sv);
                    for (int a = 0; a < active.Count; a++)
                    {
                        if (active[a].AfflictionId == AtomicWar._Game.Medical.AfflictionSO.Ids.Coma)
                        {
                            sinceCare = active[a].HoursSinceLastCare;
                            break;
                        }
                    }
                    bool needs = MedicalSystem.NeedsCare(sv);
                    if (needs) anyUrgent = true;
                    comaList.Add(new AtomicWar._Game.UI.ComaPatientSnapshot
                    {
                        SurvivorId = sv.Id,
                        DisplayName = sv.DisplayName,
                        HoursSinceLastCare = sinceCare,
                        NeedsCare = needs
                    });
                }
                snap.Comas = comaList.ToArray();
                snap.ComaCareUrgent = anyUrgent;
            }

            return snap;
        }

        private Survivor FindFirstLivingSurvivor()
        {
            if (Survivors == null) return null;
            for (int i = 0; i < Survivors.Count; i++)
            {
                if (Survivors[i] != null && Survivors[i].IsAlive)
                    return Survivors[i];
            }
            return null;
        }

        /// <summary>Wire Internal Horror HUD action callbacks once.</summary>
        private void WireInternalHorrorHud()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;

            horror.OnBuryRequested -= HandleBuryTheDead;
            horror.OnBuryRequested += HandleBuryTheDead;
            horror.OnProcessFertilizerRequested -= HandleProcessFertilizer;
            horror.OnProcessFertilizerRequested += HandleProcessFertilizer;
            horror.OnFightFireRequested -= HandleFightFire;
            horror.OnFightFireRequested += HandleFightFire;
            horror.OnSealBulkheadRequested -= HandleSealBulkhead;
            horror.OnSealBulkheadRequested += HandleSealBulkhead;

            // Inventory corpse "click" → open dispose panel.
            var strip = _hud.InventoryStripUI;
            if (strip != null)
            {
                strip.OnIconActivated -= HandleInventoryIconActivated;
                strip.OnIconActivated += HandleInventoryIconActivated;
            }

            RefreshInternalHorrorHud();
        }

        /// <summary>
        /// Inventory strip activate (click / Enter): corpse stacks open dispose UI.
        /// </summary>
        private void HandleInventoryIconActivated(AtomicWar._Game.UI.InventoryIcon icon)
        {
            if (icon == null) return;
            if (icon.IsCorpse || icon.HasDisposeActions)
            {
                OpenCorpseDisposePanel();
            }
        }

        private void HandleBuryTheDead()
        {
            if (CorpseSystem == null) return;
            var digger = FindFirstLivingSurvivor();
            if (digger == null) return;
            float daylight = PhotoperiodSystem != null
                ? PhotoperiodSystem.EffectiveDaylightHours
                : CorpseManagementSystem.BuryHours;
            if (CorpseSystem.BuryTheDead(digger, daylight))
            {
                Debug.Log($"[Internal Horror] {digger.DisplayName} buried the dead. Four hours of light, gone.");
                RefreshInventoryStrip();
            }
        }

        private void HandleProcessFertilizer()
        {
            if (CorpseSystem == null) return;
            var processor = FindFirstLivingSurvivor();
            if (processor == null) return;
            if (CorpseSystem.ProcessForFertilizer(processor))
            {
                Debug.Log($"[Internal Horror] {processor.DisplayName} processed a body for fertilizer. Nobody speaks.");
                RefreshInventoryStrip();
            }
        }

        private void HandleFightFire(string roomId)
        {
            if (AtmosphereSystem == null || string.IsNullOrEmpty(roomId)) return;
            var fighter = FindFirstLivingSurvivor();
            if (fighter == null) return;
            bool out_ = AtmosphereSystem.FightFire(roomId, fighter, NeedsSystem);
            Debug.Log(out_
                ? $"[Internal Horror] {fighter.DisplayName} put out the fire in {roomId}."
                : $"[Internal Horror] {fighter.DisplayName} fought the fire in {roomId}. Still burning.");
            RefreshInternalHorrorHud();
        }

        private void HandleSealBulkhead(string roomId)
        {
            if (AtmosphereSystem == null || string.IsNullOrEmpty(roomId)) return;
            if (AtmosphereSystem.SealBulkhead(roomId, Shelter))
            {
                Debug.Log($"[Internal Horror] Bulkhead sealed on {roomId}. Whatever was inside stays inside.");
                RefreshInternalHorrorHud();
            }
        }

        /// <summary>
        /// Player API: open corpse dispose panel (bury / fertilizer) from inventory body slot.
        /// </summary>
        public bool OpenCorpseDisposePanel()
        {
            if (_hud == null) return false;
            RefreshInternalHorrorHud();
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null || horror.CorpseCount <= 0) return false;
            horror.OpenCorpsePanel();
            return true;
        }

        /// <summary>Player API: choose bury or fertilizer on the open corpse panel.</summary>
        public bool SelectCorpseDispose(AtomicWar._Game.UI.CorpseDisposeChoice choice)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectCorpseDispose(choice);
        }

        /// <summary>Player API: fight fire in a room (or active fire room).</summary>
        public bool SelectFightFire(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectFightFire(roomId);
        }

        /// <summary>Player API: seal bulkhead on a burning room.</summary>
        public bool SelectSealBulkhead(string roomId = null)
        {
            if (_hud == null) return false;
            var horror = _hud.EnsureInternalHorrorHud();
            return horror != null && horror.SelectSealBulkhead(roomId);
        }

        /// <summary>Close corpse dispose and/or fire panels (Esc).</summary>
        public void CloseInternalHorrorPanels()
        {
            if (_hud == null) return;
            var horror = _hud.EnsureInternalHorrorHud();
            if (horror == null) return;
            if (horror.IsCorpsePanelOpen) horror.CloseCorpsePanel();
            if (horror.IsFirePanelOpen) horror.CloseFirePanel();
        }

        /// <summary>
        /// Simulate inventory strip click at index. Corpse icons open dispose panel.
        /// </summary>
        public bool ActivateInventoryIcon(int index)
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateIndex(index);
        }

        /// <summary>Cycle inventory strip focus (keyboard path).</summary>
        public bool SelectNextInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.SelectNext();
        }

        /// <summary>Confirm focused inventory icon (Enter/E). Corpses open dispose.</summary>
        public bool ActivateSelectedInventoryIcon()
        {
            if (_hud == null) return false;
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateSelected();
        }

        /// <summary>Click first corpse stack in inventory (if any).</summary>
        public bool ActivateFirstCorpseInInventory()
        {
            if (_hud == null) return false;
            RefreshInventoryStrip();
            var strip = _hud.InventoryStripUI;
            return strip != null && strip.ActivateFirstCorpse();
        }

        /// <summary>True when corpse dispose panel is open (input priority).</summary>
        public bool IsCorpseDisposePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsCorpsePanelOpen;
        }

        /// <summary>True when fire fight/seal panel is open (input priority).</summary>
        public bool IsFirePanelOpen()
        {
            var horror = _hud != null ? _hud.InternalHorrorHUD : null;
            return horror != null && horror.IsFirePanelOpen;
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

            // Prompt #48 — continuous extreme weather seals the hatch.
            if (HatchEntrapmentSystem != null && WeatherSystem != null)
            {
                int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
                HatchEntrapmentSystem.Tick(
                    gameHours,
                    WeatherSystem.Current,
                    Shelter,
                    // Effective trust so Cult of the Glow (trustInversion) can dig
                    // out a highly-irradiated party even when stored trust is low.
                    factionId => EconomySystem != null
                        ? EconomySystem.GetEffectiveTrust(factionId)
                        : 0f,
                    (eventId, fireDay, originFlag) =>
                        EventRunner?.ScheduleEvent(eventId, fireDay, originFlag),
                    day);
                SyncHatchExpeditionLock();
            }

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

            // Internal Horror — fire/O2/CO/humidity, corpse rot, pantry rust
            AtmosphereSystem?.Tick(gameHours, PowerNetwork, Shelter);
            CorpseSystem?.Tick(gameHours, Survivors);
            PantrySystem?.Tick(gameHours, _storesRoom);

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

            // Prompt #10 — Skill Atrophy: morale < 20 for 14 days → skill downgrade.
            SkillAtrophy?.Tick(gameHours, Survivors);

            // Prompt #8 — Empath coupling: Empath's morale tracks bunker average.
            EmpathSystem?.Tick(gameHours, Survivors);

            // Prompt #7 — Addiction & Withdrawal: dose counting, withdrawal drains, panic destruction.
            int currentDay = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            Addiction?.Tick(gameHours, Survivors, currentDay);

            // Prompt #6 — Phantom Intruders: fake hatch breach when Anxiety+Fatigue max out.
            PhantomIntruders?.Tick(gameHours, Survivors, new System.Random(_worldSeed + 61));

            // Prompt #9 — Child: Hope buff, rations consumption, death check.
            ChildSystem?.Tick(gameHours, Survivors);

            // Hatch-dilemma prompt: advance the timeout. On expiry the
            // prompt auto-resolves with ForceDeconOutside.
            HatchDilemmaPromptField?.Tick(gameHours);
            ParleyOfferPromptField?.Tick(gameHours);

            // Radiation
            RadiationSystem.Tick(gameHours);
            // Cult of the Glow: rad drop across healthy ceiling → hatch raid cascade.
            EconomySystem?.NotifyPartyRadiationChanged();

            // Water economy: catchment collection + purifier conversion queue.
            WaterEconomySystem?.Tick(gameHours, WeatherSystem.Current, TimeSystem.CurrentDay, Shelter, WaterStorage);

            // Prompt #11 — Black Rain dread for outdoor scavengers + hatch listeners.
            if (BlackRainHazardSystem != null && Survivors != null)
            {
                BlackRainHazardSystem.TickDread(
                    Survivors,
                    isOutdoor: IsSurvivorOnExpedition,
                    isHatchListener: IsSurvivorHatchListener,
                    gameHours);
            }

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

            // Shared indoor temp for AI sleep context + event/journal discoveries.
            float indoorTemp = TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f;

            // AI (evaluate per survivor, every EvaluationInterval)
            UtilityAI.Tick(gameHours * TimeSystem.SecondsPerGameHour);
            if (UtilityAI.ShouldEvaluate())
            {
                // Fresh sleep-wave occupancy so capacity is per evaluation pass.
                SleepQualitySystem.ResetBedOccupancy(Shelter);
                // Guards re-assigned each AI wave (stale posts clear).
                HatchDefenseSystem?.ClearGuards();
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

                    // Prompt #7 — track addictive chem consumption
                    if (action != null && Addiction != null)
                    {
                        if (action.id == "action_use_antirad")
                            Addiction.OnItemConsumed(sv, "anti_rad", day);
                    }
                }
            }

            // Events (chance per hour)
            var eventContext = BuildEventContext(
                TimeSystem != null ? TimeSystem.CurrentDay : 1,
                TimeSystem != null ? TimeSystem.CurrentHourFloat : 12f,
                indoorTemp);
            EventRunner.Tick(gameHours, eventContext);

            // Internal mysteries: resource-starved Missing Rations pressure.
            SuspicionTracker?.Tick(gameHours, eventContext, EventRunner);

            // Diegetic journal discoveries (first-time atmosphere / rad / storm / etc.)
            if (JournalSystem != null)
                EventRunner.ObserveDiscoveries(JournalSystem, eventContext);

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
        // Event context + narrative chain pool helpers
        // -----------------------------------------------------------------

        /// <summary>
        /// Shared EventContext for hourly event ticks and day-gated scheduleEvent chains.
        /// Imports SaveSystem world flags and wires trust + flag persistence.
        /// </summary>
        private EventContext BuildEventContext(int day, float hour = 12f, float? indoorTempC = null)
        {
            float indoor = indoorTempC ?? (TemperatureSystem != null
                ? TemperatureSystem.GetIndoorTemperature(Shelter)
                : 15f);

            var ctx = new EventContext(
                Survivors != null && Survivors.Count > 0 ? Survivors[0] : null,
                Shelter,
                Inventory,
                new System.Random(_worldSeed + day))
            {
                CurrentDay = day,
                CurrentHour = hour,
                IsFalloutStorm = WeatherSystem != null && WeatherSystem.Current == WeatherKind.FalloutStorm,
                CurrentWeather = WeatherSystem != null ? WeatherSystem.Current : WeatherKind.Clear,
                AllSurvivors = Survivors,
                MentalBreak = MentalBreakSystem,
                CarbonMonoxidePpm = PowerNetwork != null ? PowerNetwork.CarbonMonoxidePpm : 0f,
                IndoorTemperatureC = indoor,
                GetFactionTrust = factionId =>
                    EconomySystem != null ? EconomySystem.GetTrust(factionId) : 0f,
                OnEventFlagChanged = (flagId, value) =>
                {
                    if (SaveSystem != null)
                        SaveSystem.SetWorldFlag(flagId, value);
                },
                // Primary survivor is POV for mystery suspect exclusion.
                PlayerSurvivorId = Survivors != null && Survivors.Count > 0 && Survivors[0] != null
                    ? Survivors[0].Id
                    : null,
                Suspicion = SuspicionTracker
            };
            if (SaveSystem != null)
                ctx.ImportFlags(SaveSystem.WorldFlags);
            if (SuspicionTracker != null)
            {
                SuspicionTracker.RefreshStarved(Inventory);
                ctx.IsResourceStarved = SuspicionTracker.IsResourceStarved;
            }
            return ctx;
        }

        private static void EnsurePoolHasMissingRationsChain(List<GameEvent> pool)
        {
            if (pool == null) return;
            var chain = SuspicionTracker.CreateMissingRationsChain();
            for (int i = 0; i < chain.Count; i++)
            {
                var next = chain[i];
                if (next == null || string.IsNullOrEmpty(next.id)) continue;
                bool exists = false;
                for (int j = 0; j < pool.Count; j++)
                {
                    if (pool[j] != null && pool[j].id == next.id)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    pool.Add(next);
            }
        }

        private static void EnsurePoolHasEmissaryChain(List<GameEvent> pool)
        {
            if (pool == null) return;
            var chain = EventRunner.CreateEmissaryChain();
            for (int i = 0; i < chain.Count; i++)
            {
                var next = chain[i];
                if (next == null || string.IsNullOrEmpty(next.id)) continue;
                bool exists = false;
                for (int j = 0; j < pool.Count; j++)
                {
                    if (pool[j] != null && pool[j].id == next.id)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists)
                    pool.Add(next);
            }
        }

        /// <summary>
        /// Register Prompt #46 radio-triggered events (Safe Haven Broadcast) into
        /// the EventRunner pool if not already present. Mirrors the pattern
        /// used by <see cref="EnsurePoolHasEmissaryChain"/>: factory versions
        /// stay in the pool so the radio bridge can resolve the id even when
        /// the catalog import has not been re-run.
        /// </summary>
        private static void EnsurePoolHasRadioTriggeredEvents(List<GameEvent> pool)
        {
            if (pool == null) return;
            var safeHaven = EventRunner.CreateSafeHavenBroadcastEvent();
            if (safeHaven == null || string.IsNullOrEmpty(safeHaven.id)) return;
            for (int j = 0; j < pool.Count; j++)
            {
                if (pool[j] != null && pool[j].id == safeHaven.id) return;
            }
            pool.Add(safeHaven);
        }

        /// <summary>
        /// Register Prompt #47 biological-trade events (Blood for Water)
        /// into the EventRunner pool if not already present. Mirrors the
        /// <see cref="EnsurePoolHasEmissaryChain"/> pattern.
        /// </summary>
        private static void EnsurePoolHasBiologicalTradeEvents(List<GameEvent> pool)
        {
            if (pool == null) return;
            var blood = EventRunner.CreateBloodForWaterEvent();
            if (blood == null || string.IsNullOrEmpty(blood.id)) return;
            for (int j = 0; j < pool.Count; j++)
            {
                if (pool[j] != null && pool[j].id == blood.id) return;
            }
            pool.Add(blood);
        }

        /// <summary>
        /// Register Prompt #48 Buried Alive + faction dig-out events.
        /// </summary>
        private static void EnsurePoolHasHatchEntrapmentEvents(List<GameEvent> pool)
        {
            if (pool == null) return;
            EnsurePoolHasEvent(pool, EventRunner.CreateBuriedAliveEvent());
            EnsurePoolHasEvent(pool, EventRunner.CreateFactionDigOutEvent());
        }

        private static void EnsurePoolHasEvent(List<GameEvent> pool, GameEvent ev)
        {
            if (pool == null || ev == null || string.IsNullOrEmpty(ev.id)) return;
            for (int j = 0; j < pool.Count; j++)
            {
                if (pool[j] != null && pool[j].id == ev.id) return;
            }
            pool.Add(ev);
        }

        /// <summary>
        /// Keep expedition hard-lock + map UI in sync with HatchState.
        /// </summary>
        private void SyncHatchExpeditionLock()
        {
            bool locked = HatchEntrapmentSystem != null && HatchEntrapmentSystem.AreExpeditionsLocked;
            if (ExpeditionSystem != null)
                ExpeditionSystem.HatchBlocksExpeditions = locked;
            if (_hud != null && _hud.MapScreenUI != null)
                _hud.MapScreenUI.IsExpeditionUiEnabled = !locked;
        }

        /// <summary>
        /// Buried Alive / faction dig-out choice side effects (Prompt #48).
        /// DigOut spikes entry-room CO2; faction rescue clears the hatch.
        /// </summary>
        private void HandleHatchEntrapmentChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || HatchEntrapmentSystem == null) return;

            if (ev.id == EventRunner.BuriedAliveEventId
                && choice.ChoiceId == EventRunner.ChoiceDigOut)
            {
                if (_entryRoom == null)
                    _entryRoom = new ShelterRoom(HatchEntrapmentSystem.EntryRoomId, null);
                HatchEntrapmentSystem.DigOut(_entryRoom, ctx);
                SyncHatchExpeditionLock();
                Debug.Log($"[Hatch Entrapment] DigOut complete. Entry CO2={_entryRoom.Co2Ppm:F0} ppm.");
                return;
            }

            if (ev.id == EventRunner.FactionDigOutEventId
                && choice.ChoiceId == EventRunner.ChoiceAcceptFactionRescue)
            {
                HatchEntrapmentSystem.ApplyFactionRescue(ctx);
                SyncHatchExpeditionLock();
                Debug.Log("[Hatch Entrapment] Faction dug the hatch open. Debt recorded.");
            }
        }

        // -----------------------------------------------------------------
        // Prompt #17 — Raid plan wiretap
        // -----------------------------------------------------------------

        /// <summary>
        /// High-tier radio/antenna operational for inter-faction wiretaps.
        /// Requires powered radio with remaining fuel and EMP damage below destroy.
        /// </summary>
        private bool IsWiretapAntennaOperational()
        {
            var state = RadioTunerSystem?.State;
            return state != null && state.IsOperational;
        }

        private void HandleRaidPlanInterceptOffered(FactionRaidPlan plan, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            Debug.Log($"[Raid Plan] Wiretap offered: {plan?.AttackerFactionId} → {plan?.TargetFactionId}");
        }

        private void HandleRaidPlanChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || FactionRaidPlanSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(FactionRaidPlanSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            FactionRaidPlanSystem.ApplyChoiceFromEvent(ev, choice);
        }

        // -----------------------------------------------------------------
        // Prompt #18 — Debt Collector
        // -----------------------------------------------------------------

        /// <summary>
        /// Faction dig-out accepted → schedule collector for day + 20.
        /// Short-term dig-out debt flag is already set by HatchEntrapmentSystem.
        /// </summary>
        private void HandleFactionRescueApplied_ScheduleDebt(string factionId)
        {
            if (DebtCollectorSystem == null || string.IsNullOrEmpty(factionId)) return;
            if (DebtCollectorSystem.HasPendingDebtFor(factionId)) return;
            var entry = DebtCollectorSystem.ScheduleDebt(factionId);
            if (entry != null)
                Debug.Log($"[Debt Collector] Scheduled for {factionId} on day {entry.CollectorDay}.");
        }

        private void HandleDebtCollectorArrived(DebtEntry debt, GameEvent ev)
        {
            if (ev == null || EventRunner == null) return;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 0;
            var ctx = BuildEventContext(day);
            EventRunner.Run(ev, ctx);
            Debug.Log($"[Debt Collector] {debt?.FactionId} demands half fuel + half clean water.");
        }

        private void HandleDebtCollectorChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null || DebtCollectorSystem == null) return;
            if (string.IsNullOrEmpty(ev.id)
                || !ev.id.StartsWith(DebtCollectorSystem.EventIdPrefix, StringComparison.Ordinal))
                return;
            DebtCollectorSystem.ApplyChoiceFromEvent(ev, choice, ctx);
        }

        // -----------------------------------------------------------------
        // Win/Lose (VictoryProjectManager)
        // -----------------------------------------------------------------

        private void CheckWinLose()
        {
            if (VictoryProject == null || VictoryProject.IsTerminal) return;
            if (Survivors == null) return;

            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;

            // Loss: all survivors dead → death-screen by cause (rads / hunger / breakdowns).
            VictoryProject.EvaluateLoss(Survivors, day);

            if (EndgameEngine != null && !EndgameEngine.Result.IsTerminal)
            {
                bool isExtractionUnlocked = VictoryProject != null && VictoryProject.ExtractionUnlocked;
                bool isHydroponicsWorking = Shelter != null && Shelter.IsGrowLightActive;
                int deadCount = 0;
                for (int i = 0; i < Survivors.Count; i++)
                {
                    if (Survivors[i] != null && !Survivors[i].IsAlive) deadCount++;
                }

                EndgameEngine.Evaluate(
                    day,
                    Survivors,
                    Shelter,
                    isExtractionUnlocked,
                    isHydroponicsWorking,
                    deadCount);
            }
        }

        /// <summary>
        /// Vehicle escape project: 50 mechanical_parts + 10 fuel + repaired engine.
        /// Explicit player action (not auto each frame).
        /// </summary>
        public bool TryVehicleEscape()
        {
            if (VictoryProject == null || Inventory == null) return false;
            int day = TimeSystem != null ? TimeSystem.CurrentDay : 1;
            var summary = VictoryProject.TryEscapeByVehicle(
                Inventory,
                id => _itemCatalog?.GetById(id) ?? MakeRuntimeItem(id),
                day,
                Survivors);
            return summary != null && summary.State == EndgameState.Escaped;
        }

        /// <summary>Record a resolved moral dilemma for the endgame tally.</summary>
        public void RecordMoralChoice()
        {
            VictoryProject?.RecordMoralChoice();
        }

        private void ApplyEndgame(EndgameSummaryData summary)
        {
            if (summary == null) return;
            IsGameOver = true;
            GameOverReason = summary.Reason ?? summary.OutcomeTitle;
            if (GameState != null)
            {
                GameState.IsPaused = true;
                GameState.Phase = GamePhase.GameOver;
            }
            // Halt TimeSystem by not ticking (Update already gates on Phase/IsGameOver).
            PushEndgameSummaryToHud(summary);
            Debug.Log($"[GameBootstrap] ENDGAME ({summary.State}): {summary.OutcomeTitle} — {summary.Reason}");
        }

        private void PushEndgameSummaryToHud(EndgameSummaryData summary)
        {
            if (_hud == null || summary == null) return;
            var ui = _hud.EnsureEndgameSummary();
            if (ui == null) return;
            ui.Show(
                summary.State.ToString(),
                summary.OutcomeTitle,
                summary.OutcomeBody,
                summary.DeathScreen == DeathScreenKind.None ? string.Empty : summary.DeathScreen.ToString(),
                summary.DaysSurvived,
                summary.TotalRadiationAbsorbed,
                summary.MoralChoicesMade,
                summary.MilitaryIntelDecrypted,
                summary.ExtractionUnlocked,
                summary.VehicleEscapeUsed);
        }

        /// <summary>
        /// Runtime item defs for tests / missing catalog entries (engine, parts, fuel).
        /// </summary>
        private static ItemDefinition MakeRuntimeItem(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            var item = ScriptableObject.CreateInstance<ItemDefinition>();
            item.id = id;
            item.displayName = id;
            item.stackMax = id == VictoryProjectManager.EngineItemId ? 1 : 99;
            item.weight = 0.1f;
            if (id == VictoryProjectManager.EngineItemId)
            {
                item.type = ItemType.Tool;
                item.durability = 100f;
            }
            else if (id == VictoryProjectManager.FuelItemId)
            {
                item.type = ItemType.Fuel;
            }
            else
            {
                item.type = ItemType.Material;
            }
            return item;
        }

        private void EndGame(string reason, string outcome)
        {
            // Legacy path — prefer VictoryProject triggers.
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
            _hud.EnsureRadioInterceptHud();
            WireRadioInterceptTuner();
            SyncRadioInterceptHudFromLog();
            _hud.EnsureJournalBook();
            SyncJournalBookFromSystem();
            RefreshInventoryStrip(); // initial pooled icon sync
            WireInternalHorrorHud();
            _hud.EnsureEndgameSummary();
            if (VictoryProject != null && VictoryProject.IsTerminal && VictoryProject.LastSummary != null)
                PushEndgameSummaryToHud(VictoryProject.LastSummary);
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
                // Anti-rad / scripted dose changes outside Tick still drive
                // trust-inversion raid cascades (healthy-ceiling cross).
                EconomySystem?.NotifyPartyRadiationChanged();
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

        /// <summary>Current simulation speed (1 normal, 3 fast-forward).</summary>
        public float TimeScale => TimeSystem != null ? TimeSystem.TimeScale : 1f;

        /// <summary>Toggle fast-forward: 1x <-> 3x (keybind F). Simulation-scaled only; Unity's Time.timeScale is untouched.</summary>
        public void ToggleFastForward()
        {
            if (TimeSystem == null) return;
            TimeSystem.SetTimeScale(TimeSystem.TimeScale > 1.5f ? 1f : FastForwardScale);
        }

        /// <summary>Explicit simulation speed (clamped by TimeSystem). For UI buttons/tests.</summary>
        public void SetTimeScale(float scale)
        {
            TimeSystem?.SetTimeScale(scale);
        }

        public void SaveGame(string slotId = "quicksave")
        {
            SnapshotRadioHudToInterceptSystem();
            SaveSystem.Save(slotId);
        }

        public void LoadGame(string slotId = "quicksave")
        {
            if (SaveSystem.Load(slotId))
            {
                // Restore endgame terminal state from VictoryProject if present.
                if (VictoryProject != null && VictoryProject.IsTerminal)
                {
                    IsGameOver = true;
                    GameOverReason = VictoryProject.TerminalReason;
                    if (GameState != null) GameState.Phase = GamePhase.GameOver;
                    if (VictoryProject.LastSummary != null)
                        PushEndgameSummaryToHud(VictoryProject.LastSummary);
                }
                else
                {
                    IsGameOver = false;
                    GameOverReason = null;
                    _hud?.EnsureEndgameSummary()?.Clear();
                }
                // Intercept log + open/unread/tuner restored — refresh HUD strip.
                SyncRadioInterceptHudFromLog();
                SyncJournalBookFromSystem();
                // Corpse counts / fire rooms / care urgency after atmosphere+inventory restore.
                RefreshInventoryStrip();
            }
        }

        /// <summary>Toggle diegetic journal book (keybind J).</summary>
        public void ToggleJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            if (book == null) return;
            book.Toggle();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = book.IsOpen;
                if (book.IsOpen)
                    JournalSystem.MarkRead();
            }
        }

        /// <summary>Open journal book and clear unread / ping.</summary>
        public void OpenJournalBook()
        {
            var book = _hud?.EnsureJournalBook();
            book?.Open();
            if (JournalSystem != null)
            {
                JournalSystem.HudIsOpen = true;
                JournalSystem.MarkRead();
            }
        }

        /// <summary>
        /// Copy live radio strip presentation into the intercept system so
        /// SaveSystem.CaptureState persists open / unread / tuner index.
        /// </summary>
        public void SnapshotRadioHudToInterceptSystem()
        {
            if (FactionRadioIntercepts == null) return;
            var strip = _hud != null ? _hud.EnsureRadioInterceptHud() : null;
            if (strip == null) return;
            FactionRadioIntercepts.HudIsOpen = strip.IsOpen;
            FactionRadioIntercepts.HudHasUnread = strip.HasUnread;
            FactionRadioIntercepts.HudTunerIndex = strip.TunerIndex;
        }

        public void ConsumeItem(Survivor sv, ItemDefinition item)
        {
            if (sv == null || item == null || !sv.IsAlive) return;
            if (Inventory == null || !Inventory.Consume(item, sv, RadiationSystem, NeedsSystem))
                return;

            // Prompt #13 — poisoned iodine looks clean until swallowed.
            SabotagedCacheSystem?.TryApplyPoisonOnConsume(item, sv, MedicalSystem);
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

        /// <summary>Open the expanded radio intercept log.</summary>
        public void OpenRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Open();
        }

        /// <summary>Toggle expanded radio intercept log (keybind R).</summary>
        public void ToggleRadioInterceptLog()
        {
            _hud?.EnsureRadioInterceptHud()?.Toggle();
        }

        /// <summary>Cycle radio frequency filter forward (keybind ]).</summary>
        public void CycleRadioTunerNext()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerNext();
        }

        /// <summary>Cycle radio frequency filter backward (keybind [).</summary>
        public void CycleRadioTunerPrev()
        {
            _hud?.EnsureRadioInterceptHud()?.CycleTunerPrev();
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

        /// <summary>
        /// Open trade using an ephemeral faction stock (created on first use).
        /// Used by the post-repel parley modal so the player need not hunt UI.
        /// </summary>
        public bool OpenTradeWithFaction(string factionId)
        {
            if (string.IsNullOrEmpty(factionId) || Inventory == null) return false;
            return OpenTrade(factionId, GetOrCreateFactionStock(factionId));
        }

        /// <summary>Demand parley / surrender on the open trade screen (keybind P).</summary>
        public bool DemandTradeParley()
        {
            return _hud?.TradeScreenUI != null && _hud.TradeScreenUI.TryDemandParley();
        }

        /// <summary>
        /// Demand parley for a faction. Opens trade when HUD is present so the
        /// strip shows STOOD DOWN; falls back to economy-only when headless.
        /// Used by the post-repel modal.
        /// </summary>
        public bool DemandParleyForFaction(string factionId)
        {
            if (EconomySystem == null || string.IsNullOrEmpty(factionId)) return false;
            if (OpenTradeWithFaction(factionId))
                return DemandTradeParley();
            return EconomySystem.DemandParley(factionId).Applied;
        }

        private Inventory.Inventory GetOrCreateFactionStock(string factionId)
        {
            if (_factionStocks.TryGetValue(factionId, out var existing) && existing != null)
                return existing;
            var stock = new Inventory.Inventory { Capacity = 40, MaxWeight = 200f };
            // Light seed stock so the screen is not empty after a stand-down.
            var water = _itemCatalog?.GetById("clean_water");
            var scrap = _itemCatalog?.GetById("scrap_metal");
            if (water != null) stock.Add(water, 2);
            if (scrap != null) stock.Add(scrap, 4);
            _factionStocks[factionId] = stock;
            return stock;
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

        /// <summary>
        /// Average RadiationDose across living survivors (0..100). Used by
        /// DynamicEconomySystem for trust-inversion factions (Cult of the Glow).
        /// </summary>
        private float GetPartyAverageRadiationDose()
        {
            if (Survivors == null || Survivors.Count == 0) return 0f;
            float sum = 0f;
            int n = 0;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                sum += s.RadiationDose;
                n++;
            }
            return n > 0 ? sum / n : 0f;
        }

        /// <summary>
        /// True when any living survivor has Acute Radiation Syndrome (flag or status).
        /// Cult of the Glow ARS reverence (#16 polish).
        /// </summary>
        private bool PartyHasAcuteRadiationSyndrome()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (s.HasAcuteRadiationSyndrome
                    || s.HasStatus(SurvivorStatus.AcuteRadiationSyndrome))
                    return true;
            }
            return false;
        }

        /// <summary>
        /// True when any living survivor wears an intact full hazmat suit.
        /// Cult of the Glow sealed-blood contempt (#16 polish).
        /// </summary>
        private bool PartyWearsIntactHazmat()
        {
            if (Survivors == null) return false;
            for (int i = 0; i < Survivors.Count; i++)
            {
                var s = Survivors[i];
                if (s == null || !s.IsAlive) continue;
                if (s.HasFullSuitEquipped) return true;
            }
            // Fallback: equipped protective gear with remaining durability on shared inventory.
            if (Inventory != null && Inventory.GetEquippedProtection() > 0f)
                return true;
            return false;
        }

        // -----------------------------------------------------------------
        // Radio intercept HUD strip
        // -----------------------------------------------------------------

        private void PushRadioInterceptToHud(FactionRadioInterceptSystem.InterceptEntry entry)
        {
            if (entry == null || _hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            strip?.Push(entry.Message, entry.Kind, entry.FactionId, entry.Day);
        }

        private void PushJournalEntryToHud(JournalEntry entry)
        {
            if (entry == null || _hud == null) return;
            var book = _hud.EnsureJournalBook();
            book?.Push(entry);
        }

        /// <summary>Rebuild journal book from JournalSystem (WireHUD / load).</summary>
        public void SyncJournalBookFromSystem()
        {
            if (_hud == null || JournalSystem == null) return;
            var book = _hud.EnsureJournalBook();
            if (book == null) return;
            book.SetEntries(JournalSystem.Entries);
            book.ApplyUiState(
                JournalSystem.HudIsOpen,
                JournalSystem.HasUnread,
                JournalSystem.NotificationPing);
        }

        /// <summary>
        /// Bind the intercept strip dial to RadioTunerSystem frequencies so
        /// [ / ] retunes intel extraction and filters faction intercepts together.
        /// Safe to call multiple times (rebinds bands + handler).
        /// </summary>
        public void WireRadioInterceptTuner()
        {
            if (_hud == null || RadioTunerSystem == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            // Push band list (ALL + each registered frequency).
            var coreBands = RadioTunerSystem.BuildTunerBands();
            var uiBands = new System.Collections.Generic.List<RadioInterceptHUD.TunerBand>(coreBands.Count);
            for (int i = 0; i < coreBands.Count; i++)
            {
                var b = coreBands[i];
                uiBands.Add(RadioInterceptHUD.TunerBand.FromParts(
                    b.FrequencyId, b.Label, b.ChannelTag));
            }
            strip.SetTunerBands(uiBands);

            // Avoid stacking handlers if WireHUD / load re-runs.
            strip.OnTunerBandChanged -= HandleRadioHudTunerChanged;
            strip.OnTunerBandChanged += HandleRadioHudTunerChanged;
            RadioTunerSystem.OnFrequencyChanged -= HandleRadioTunerFrequencyChanged;
            RadioTunerSystem.OnFrequencyChanged += HandleRadioTunerFrequencyChanged;

            // Align dial with current tuner state (detuned on fresh boot).
            strip.SyncFromFrequencyId(RadioTunerSystem.State?.CurrentFrequencyId);
            PushRadioLiveStateToHud();
        }

        private void HandleRadioHudTunerChanged(string frequencyId, string channelTag)
        {
            if (RadioTunerSystem == null) return;
            if (string.IsNullOrEmpty(frequencyId))
                RadioTunerSystem.Detune();
            else
                RadioTunerSystem.TuneToFrequency(frequencyId);
        }

        private void HandleRadioTunerFrequencyChanged(string frequencyId)
        {
            if (_hud == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            // Sync HUD without re-notifying (would loop into TuneToFrequency).
            strip?.SyncFromFrequencyId(frequencyId);
            PushRadioLiveStateToHud();
        }

        /// <summary>
        /// Push RadioTunerSystem.State (signal, tuned label, lock progress) onto
        /// the intercept strip so StatusLine / TunerLine stay live each frame.
        /// </summary>
        public void PushRadioLiveStateToHud()
        {
            if (_hud == null || RadioTunerSystem == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            var state = RadioTunerSystem.State;
            if (state == null)
            {
                strip.ClearLiveRadioState();
                return;
            }

            // Keep signal current even between radio ticks (weather / EMP can change).
            if (WeatherSystem != null)
                RadioTunerSystem.UpdateSignalStrength(WeatherSystem.Current);

            var freq = RadioTunerSystem.GetCurrentFrequency();
            string label = string.Empty;
            float mhz = 0f;
            if (freq != null)
            {
                mhz = freq.frequencyMHz;
                if (!string.IsNullOrEmpty(freq.displayName))
                    label = freq.displayName;
                else if (mhz > 0f)
                    label = $"{mhz:0.#} MHz";
                else
                    label = freq.id ?? string.Empty;

                // Append intercept channel tag when present (matches dial labels).
                string tag = freq.ResolveInterceptChannelTag();
                if (!string.IsNullOrEmpty(tag) && !label.Contains(tag))
                    label = $"{label} · {tag}";
            }

            strip.SetLiveRadioState(
                signalStrength: state.SignalStrength,
                tunedFrequencyLabel: label,
                frequencyMHz: mhz,
                tuningProgress: state.TuningProgress,
                isOperational: state.IsOperational);
        }

        /// <summary>
        /// Rebuild the radio strip from the intercept log (after WireHUD / save load).
        /// </summary>
        public void SyncRadioInterceptHudFromLog()
        {
            if (_hud == null || FactionRadioIntercepts == null) return;
            var strip = _hud.EnsureRadioInterceptHud();
            if (strip == null) return;

            // Ensure bands are bound before applying a saved tuner index.
            if (RadioTunerSystem != null && strip.BandCount <= 1)
                WireRadioInterceptTuner();

            var log = FactionRadioIntercepts.Log;
            var lines = new System.Collections.Generic.List<RadioInterceptHUD.Line>(log.Count);
            for (int i = 0; i < log.Count; i++)
            {
                var e = log[i];
                if (e == null || string.IsNullOrEmpty(e.Message)) continue;
                lines.Add(new RadioInterceptHUD.Line
                {
                    Message = e.Message,
                    Kind = e.Kind ?? string.Empty,
                    FactionId = e.FactionId ?? string.Empty,
                    Day = e.Day,
                    ChannelTag = DynamicEconomySystem.GetParleyChannelTag(e.FactionId)
                });
            }
            strip.SetLines(lines);
            // Restore presentation (open / unread / tuner). notifyTuner=true so
            // RadioTunerSystem re-tunes to the saved dial for intel extraction.
            strip.ApplyUiState(
                FactionRadioIntercepts.HudIsOpen,
                FactionRadioIntercepts.HudHasUnread,
                FactionRadioIntercepts.HudTunerIndex,
                notifyTuner: true);
        }

        // -----------------------------------------------------------------
        // Post-repel parley offer (trade modal)
        // -----------------------------------------------------------------

        /// <summary>
        /// After a hatch repel that did not auto-surrender, present a modal:
        /// demand parley now, open trade, or dismiss. Wired from Economy.OnRaidResolved.
        /// </summary>
        private void OnFactionRaidResolved_Handle(FactionRaidResult result)
        {
            if (result == null || !result.Launched || !result.Repelled) return;
            // Second repel may auto-surrender — no parley gate left.
            if (result.SurrenderedAfter) return;
            if (EconomySystem == null || !EconomySystem.CanDemandParley(result.FactionId)) return;
            // Avoid stacking over an existing offer.
            if (ParleyOfferPromptField != null && ParleyOfferPromptField.IsActive) return;

            PresentParleyOffer(result.FactionId);
        }

        /// <summary>Build + run the parley offer GameEvent; start soft timeout.</summary>
        public void PresentParleyOffer(string factionId)
        {
            if (EconomySystem == null || EventRunner == null || string.IsNullOrEmpty(factionId))
                return;
            if (!EconomySystem.CanDemandParley(factionId)) return;

            string leader = EconomySystem.GetLeaderName(factionId);
            ParleyOfferPromptField?.Begin(factionId, leader);
            if (ParleyOfferPromptField != null)
            {
                ParleyOfferPromptField.OnTimeout -= OnParleyOfferTimeout_Handle;
                ParleyOfferPromptField.OnTimeout += OnParleyOfferTimeout_Handle;
            }

            var eventSo = EconomySystem.CreateParleyOfferEvent(factionId);
            string eventId = eventSo.id;
            string capturedFaction = factionId;

            Action<GameEvent, EventChoice, EventContext> onChoice = null;
            onChoice = (gameEvent, choice, ctx) =>
            {
                if (gameEvent == null || choice == null) return;
                if (gameEvent.id != eventId) return;
                ApplyParleyOfferChoice(capturedFaction, choice.ChoiceId);
                EventRunner.OnChoiceApplied -= onChoice;
            };
            EventRunner.OnChoiceApplied += onChoice;

            var primary = Survivors != null && Survivors.Count > 0 ? Survivors[0] : null;
            var ctx = new EventContext(primary, Shelter, Inventory, new System.Random(_worldSeed + 17));
            EventRunner.Run(eventSo, ctx);
        }

        private void OnParleyOfferTimeout_Handle(ParleyOfferPrompt.Resolution resolution)
        {
            // Soft dismiss — they can still open trade later via [P] if repels hold.
            ParleyOfferPromptField?.Cancel();
        }

        /// <summary>Resolve a parley-offer choice id from the event modal.</summary>
        public void ApplyParleyOfferChoice(string factionId, string choiceId)
        {
            if (string.IsNullOrEmpty(choiceId)) choiceId = "dismiss";

            ParleyOfferPrompt.Resolution res = ParleyOfferPrompt.Resolution.Dismiss;
            switch (choiceId)
            {
                case "parley_now":
                    res = ParleyOfferPrompt.Resolution.DemandParley;
                    DemandParleyForFaction(factionId);
                    break;
                case "open_trade":
                    res = ParleyOfferPrompt.Resolution.OpenTrade;
                    OpenTradeWithFaction(factionId);
                    break;
                default:
                    res = ParleyOfferPrompt.Resolution.Dismiss;
                    break;
            }

            ParleyOfferPromptField?.Resolve(res);
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

        /// <summary>True when the survivor is currently on an outdoor expedition (Black Rain exposure).</summary>
        private bool IsSurvivorOnExpedition(Survivor s)
        {
            if (s == null || ExpeditionSystem?.ActiveExpeditions == null) return false;
            for (int i = 0; i < ExpeditionSystem.ActiveExpeditions.Count; i++)
            {
                var e = ExpeditionSystem.ActiveExpeditions[i];
                if (e?.Survivor != null && e.Survivor.Id == s.Id) return true;
            }
            return false;
        }

        /// <summary>
        /// Black Rain hatch listeners: anyone in the entry room, or anyone
        /// underground while the hatch is sealed/open and rain is audible.
        /// Simplified: entry-room assignment OR hatch not Clear during BlackRain.
        /// </summary>
        private bool IsSurvivorHatchListener(Survivor s)
        {
            if (s == null || !s.IsAlive) return false;
            if (string.Equals(s.CurrentRoomId, HatchEntrapmentSystem.EntryRoomId, StringComparison.OrdinalIgnoreCase))
                return true;
            // Sealed hatch transmits the hammer of rain into the bunker.
            if (HatchEntrapmentSystem != null
                && HatchEntrapmentSystem.State != HatchState.Clear
                && BlackRainHazardSystem != null
                && BlackRainHazardSystem.IsActive)
            {
                return true;
            }
            return false;
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #9 — Child Found: create child survivor on "take in" choice.
        // ─────────────────────────────────────────────────────────────────

        private void HandleChildFoundChoiceApplied(GameEvent ev, EventChoice choice, EventContext ctx)
        {
            if (ev == null || choice == null) return;
            if (ev.id != "child_found_in_ash") return;

            if (choice.ChoiceId == "take_the_child")
            {
                if (ChildSystem != null && !ChildSystem.WasChildFound)
                {
                    ChildSystem.CreateChild();
                    Debug.Log("[Child] The bunker has taken in the child. A fragile hope settles over the shelter.");
                }
            }

            // Either choice resolves the event — prevent re-triggering
            if (SaveSystem != null)
            {
                SaveSystem.SetWorldFlag(ChildDependentSystem.ChildFoundFlag, true);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #7 — Addiction: panic-destroy items during withdrawal.
        // ─────────────────────────────────────────────────────────────────

        private bool ForceAddictionPanicDestroy(Survivor sv, System.Random rng)
        {
            if (sv == null || Inventory == null || rng == null) return false;

            // Destroy 1-3 random inventory items, each from a different slot
            int count = rng.Next(1, 4);
            bool destroyed = false;
            var targetedIndices = new System.Collections.Generic.HashSet<int>();
            for (int i = 0; i < count; i++)
            {
                if (Inventory.Slots == null || Inventory.Slots.Count == 0) break;
                // Find a non-empty slot we haven't targeted yet
                int attempts = 0;
                int idx;
                InventorySlot slot;
                do
                {
                    idx = rng.Next(0, Inventory.Slots.Count);
                    slot = (idx >= 0 && idx < Inventory.Slots.Count) ? Inventory.Slots[idx] : null;
                    attempts++;
                } while ((slot == null || slot.Item == null || slot.Amount <= 0 || targetedIndices.Contains(idx)) && attempts < 20);

                if (slot == null || slot.Item == null || slot.Amount <= 0) continue;
                targetedIndices.Add(idx);
                int toRemove = rng.Next(1, Mathf.Min(slot.Amount, 3));
                if (Inventory.Remove(slot.Item, toRemove))
                {
                    destroyed = true;
                    Debug.Log($"[Addiction] {sv.DisplayName} destroyed {toRemove}x {slot.Item.id} in a withdrawal panic.");
                }
            }
            return destroyed;
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #9 — Child Found event factory.
        // ─────────────────────────────────────────────────────────────────

        private static void EnsurePoolHasChildFoundEvent(List<GameEvent> pool)
        {
            if (pool == null) return;
            var ev = CreateChildFoundEvent();
            if (ev != null && !string.IsNullOrEmpty(ev.id))
            {
                bool exists = false;
                for (int i = 0; i < pool.Count; i++)
                {
                    if (pool[i] != null && pool[i].id == ev.id)
                    {
                        exists = true;
                        break;
                    }
                }
                if (!exists) pool.Add(ev);
            }
        }

        // ─────────────────────────────────────────────────────────────────
        // Prompt #5 — Default diary factory for when no authored assets exist.
        // ─────────────────────────────────────────────────────────────────

        private static DiaryFragmentSO CreateDefaultDiary(
            string id, string title, string text, string author,
            string roomId, string warnsSystem, int page, int total)
        {
            var diary = ScriptableObject.CreateInstance<DiaryFragmentSO>();
            diary.id = id;
            diary.title = title;
            diary.text = text;
            diary.authorName = author;
            diary.foundInRoomId = roomId;
            diary.warnsAboutSystemId = warnsSystem;
            diary.pageOrder = page;
            diary.totalPages = total;
            return diary;
        }

        private static GameEvent CreateChildFoundEvent()
        {
            var ev = ScriptableObject.CreateInstance<GameEvent>();
            ev.id = "child_found_in_ash";
            ev.title = "A Small Figure in the Ash";
            ev.bodyText = "During a scavenging run, one of your survivors spots movement in the ash drifts. " +
                "At first it looks like an animal — but then the shape resolves. It's a child. Maybe eight years old. " +
                "Filthy, shivering, barely able to stand. They don't speak. They just stare at the scavenger with hollow, " +
                "exhausted eyes.\n\nThe child cannot work. Cannot fight. Cannot scavenge. They will consume food and " +
                "water like anyone else. But keeping them alive might mean something. Something the bunker has been losing.";
            ev.minDay = 8;
            ev.weight = 1f;

            // Choice 1: Take them in
            var takeIn = new EventChoice
            {
                ChoiceId = "take_the_child",
                Text = "Bring them into the bunker. They're just a child.",
                MoraleDelta = 15f,
                Effects = new List<EventEffect>
                {
                    new EventEffect
                    {
                        SetWorldFlag = ChildDependentSystem.ChildFoundFlag,
                        WorldFlagValue = true
                    }
                },
                SetEventFlags = new List<string> { ChildDependentSystem.ChildFoundFlag }
            };

            // Choice 2: Leave them
            var leave = new EventChoice
            {
                ChoiceId = "leave_them",
                Text = "We can barely feed ourselves. Keep moving.",
                MoraleDelta = -10f
            };

            ev.choices = new List<EventChoice> { takeIn, leave };
            return ev;
        }
    }
}
