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

        [Header("Log Rotation (A-11)")]
        [SerializeField] private LogRotationManager _logRotationManager;

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
        // Prompt #556 — graft/prosthetic rejection + immunosuppressants.
        public GraftRejectionSystem GraftRejection { get; private set; }
        // Prompt #558 — mutant pheromone camo (24h animal friendliness).
        public PheromoneMaskingSystem PheromoneMasking { get; private set; }
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
        // Prompt #79 — mutated ecosystem (flora/fauna).
        public MutatedEcosystemSystem EcosystemSystem { get; private set; }
        // Prompt #79–#84 — house-to-bunker transition.
        public HouseToBunkerSystem HouseToBunkerSystem { get; private set; }
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

        /// <summary>Last day phantom pain was rolled (Prompt #56).</summary>
        private int _lastSkillProgressionDay = -1;
        private int _lastPhantomPainDay;
        /// <summary>Last day scurvy was advanced (Prompt #57).</summary>
        private int _lastScurvyDay;
        /// <summary>Last day mutagenesis was evaluated (Prompt #60).</summary>
        private int _lastMutagenesisDay;
        /// <summary>Last day deserter spy check was run (Prompt #75).</summary>
        private int _lastDeserterDay;
        /// <summary>Last day ecosystem mutation was advanced (Prompt #79).</summary>
        private int _lastEcosystemDay;
        /// <summary>Last day house artillery damage was applied (Prompt #79).</summary>
        private int _lastHouseDay;
        private int _lastHatchVisDay;

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
