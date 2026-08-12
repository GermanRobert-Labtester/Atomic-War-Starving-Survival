using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using AtomicWar._Game.Data;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Events;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Economy;
using AtomicWar._Game.Inventory;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Root HUD controller: subscribes to system events and routes state to child widgets
    /// (NeedsBar, DosimeterHUD, GeigerAudioHook, EnvironmentStatusHUD, EventModalUI).
    /// Purely event-driven; holds no game loop update logic except F2 debug key handling.
    /// UI Toolkit architecture.
    /// </summary>
    public class HUD : MonoBehaviour
    {
        [SerializeField] private NeedsBar _needsBar;
        [SerializeField] private DosimeterHUD _dosimeterHud;
        [SerializeField] private HealthTrajectoryHUD _healthTrajectoryHud;
        [SerializeField] private GeigerAudioHook _geigerAudioHook;
        [SerializeField] private EnvironmentStatusHUD _environmentStatusHud;
        [SerializeField] private EventModalUI _eventModalUi;
        [SerializeField] private MapKnowledgeHUD _mapKnowledgeHud;
        [SerializeField] private TradeScreenUI _tradeScreenUi;
        [SerializeField] private PowerGridHUD _powerGridHud;
        [SerializeField] private MapScreenUI _mapScreenUi;
        [SerializeField] private WorkbenchUI _workbenchUi;
        [SerializeField] private HatchDefenseHUD _hatchDefenseHud;
        [SerializeField] private ScavengeDispatchHUD _scavengeDispatchHud;
        [SerializeField] private OverflowCrateHUD _overflowCrateHud;
        [SerializeField] private FieldGearLoadoutHUD _fieldGearLoadoutHud;
        [SerializeField] private BunkerRationingHUD _bunkerRationingHud;
        [SerializeField] private WaterPurificationHUD _waterPurificationHud;
        [SerializeField] private AirHeatManagementHUD _airHeatManagementHud;
        [SerializeField] private BunkerMaintenanceHUD _bunkerMaintenanceHud;
        [SerializeField] private SurvivorTaskBoardHUD _survivorTaskBoardHud;
        [SerializeField] private RoomAssignmentHUD _roomAssignmentHud;
        [SerializeField] private RadioInterceptHUD _radioInterceptHud;
        [SerializeField] private FactionRadioVoHook _factionRadioVoHook;
        [SerializeField] private JournalBookUI _journalBookUi;
        [SerializeField] private InventoryStripUI _inventoryStripUi;
        [SerializeField] private EndgameSummaryUI _endgameSummaryUi;
        [SerializeField] private InternalHorrorHUD _internalHorrorHud;
        [SerializeField] private ExpeditionEncounterLogHUD _expeditionEncounterLogHud;
        [SerializeField] private DiegeticHudController _diegeticHud;
        [SerializeField] private MoralChronicleUI _moralChronicleUi;
        [SerializeField] private TutorialOverlay _tutorialOverlay;

        // ── Batch-20 UI Elements ─────────────────────────────────────────────
        [Header("Batch 20 UI Elements")]
        [SerializeField] private RadiationDosimeterWidget _radiationDosimeterWidget;
        [SerializeField] private GeigerSweepGauge         _geigerSweepGauge;
        [SerializeField] private AirFilterIntegrityBar    _airFilterIntegrityBar;
        [SerializeField] private FalloutStormWarningBanner _falloutStormWarningBanner;
        [SerializeField] private SurvivorPortraitCard     _survivorPortraitCard;
        [SerializeField] private MoralDecayMeter          _moralDecayMeter;
        [SerializeField] private RationAllocationDial     _rationAllocationDial;
        [SerializeField] private WaterPurityGauge         _waterPurityGauge;
        [SerializeField] private TemperatureReadoutWidget _temperatureReadoutWidget;
        [SerializeField] private PowerFlowSchematic       _powerFlowSchematic;
        [SerializeField] private FactionPressureRing      _factionPressureRing;
        [SerializeField] private ExpeditionCountdownTimer _expeditionCountdownTimer;
        [SerializeField] private RadioSignalStrengthBar   _radioSignalStrengthBar;
        [SerializeField] private CraftQueueStrip          _craftQueueStrip;
        [SerializeField] private AlertToastNotification   _alertToastNotification;
        [SerializeField] private BunkerFloorMapMiniature  _bunkerFloorMapMiniature;
        [SerializeField] private DayNightArcClock         _dayNightArcClock;
        [SerializeField] private BloodTypeIndicator       _bloodTypeIndicator;
        [SerializeField] private LootHaulTicker           _lootHaulTicker;
        [SerializeField] private EndgameVictoryPathTracker _endgameVictoryPathTracker;

        [SerializeField] private KeyCode _debugToggleKey = KeyCode.F2;
        [SerializeField] private bool _debugModeEnabled = false;

        public NeedsBar NeedsBar { get { EnsureWidgetReferences(); return _needsBar; } }
        public DosimeterHUD DosimeterHUD { get { EnsureWidgetReferences(); return _dosimeterHud; } }
        public HealthTrajectoryHUD HealthTrajectoryHUD { get { EnsureWidgetReferences(); return _healthTrajectoryHud; } }
        public GeigerAudioHook GeigerAudioHook { get { EnsureWidgetReferences(); return _geigerAudioHook; } }
        public EnvironmentStatusHUD EnvironmentStatusHud { get { EnsureWidgetReferences(); return _environmentStatusHud; } }
        public EventModalUI EventModalUI { get { EnsureWidgetReferences(); return _eventModalUi; } }
        public MapKnowledgeHUD MapKnowledgeHUD { get { EnsureWidgetReferences(); return _mapKnowledgeHud; } }
        public TradeScreenUI TradeScreenUI { get { EnsureWidgetReferences(); return _tradeScreenUi; } }
        public PowerGridHUD PowerGridHUD { get { EnsureWidgetReferences(); return _powerGridHud; } }
        public MapScreenUI MapScreenUI { get { EnsureWidgetReferences(); return _mapScreenUi; } }
        public WorkbenchUI WorkbenchUI { get { EnsureWidgetReferences(); return _workbenchUi; } }
        public HatchDefenseHUD HatchDefenseHUD { get { EnsureWidgetReferences(); return _hatchDefenseHud; } }
        public ScavengeDispatchHUD ScavengeDispatchHUD { get { EnsureWidgetReferences(); return _scavengeDispatchHud; } }
        public OverflowCrateHUD OverflowCrateHUD { get { EnsureWidgetReferences(); return _overflowCrateHud; } }
        public FieldGearLoadoutHUD FieldGearLoadoutHUD { get { EnsureWidgetReferences(); return _fieldGearLoadoutHud; } }
        public BunkerRationingHUD BunkerRationingHUD { get { EnsureWidgetReferences(); return _bunkerRationingHud; } }
        public WaterPurificationHUD WaterPurificationHUD { get { EnsureWidgetReferences(); return _waterPurificationHud; } }
        public AirHeatManagementHUD AirHeatManagementHUD { get { EnsureWidgetReferences(); return _airHeatManagementHud; } }
        public BunkerMaintenanceHUD BunkerMaintenanceHUD { get { EnsureWidgetReferences(); return _bunkerMaintenanceHud; } }
        public SurvivorTaskBoardHUD SurvivorTaskBoardHUD { get { EnsureWidgetReferences(); return _survivorTaskBoardHud; } }
        public RoomAssignmentHUD RoomAssignmentHUD { get { EnsureWidgetReferences(); return _roomAssignmentHud; } }
        public RadioInterceptHUD RadioInterceptHUD { get { EnsureWidgetReferences(); return _radioInterceptHud; } }
        public JournalBookUI JournalBookUI { get { EnsureWidgetReferences(); return _journalBookUi; } }
        public InventoryStripUI InventoryStripUI { get { EnsureWidgetReferences(); return _inventoryStripUi; } }
        public EndgameSummaryUI EndgameSummaryUI { get { EnsureWidgetReferences(); return _endgameSummaryUi; } }
        public InternalHorrorHUD InternalHorrorHUD { get { EnsureWidgetReferences(); return _internalHorrorHud; } }
        public ExpeditionEncounterLogHUD ExpeditionEncounterLogHUD { get { EnsureWidgetReferences(); return _expeditionEncounterLogHud; } }
        public DiegeticHudController DiegeticHud { get { EnsureWidgetReferences(); return _diegeticHud; } }
        public MoralChronicleUI MoralChronicleUI { get { EnsureWidgetReferences(); return _moralChronicleUi; } }
        public TutorialOverlay TutorialOverlay { get { EnsureWidgetReferences(); return _tutorialOverlay; } }

        // ── Batch-20 property accessors ──────────────────────────────────────
        public RadiationDosimeterWidget RadiationDosimeterWidget { get { EnsureWidgetReferences(); return _radiationDosimeterWidget; } }
        public GeigerSweepGauge         GeigerSweepGauge         { get { EnsureWidgetReferences(); return _geigerSweepGauge; } }
        public AirFilterIntegrityBar    AirFilterIntegrityBar    { get { EnsureWidgetReferences(); return _airFilterIntegrityBar; } }
        public FalloutStormWarningBanner FalloutStormWarningBanner{ get { EnsureWidgetReferences(); return _falloutStormWarningBanner; } }
        public SurvivorPortraitCard     SurvivorPortraitCard     { get { EnsureWidgetReferences(); return _survivorPortraitCard; } }
        public MoralDecayMeter          MoralDecayMeter          { get { EnsureWidgetReferences(); return _moralDecayMeter; } }
        public RationAllocationDial     RationAllocationDial     { get { EnsureWidgetReferences(); return _rationAllocationDial; } }
        public WaterPurityGauge         WaterPurityGauge         { get { EnsureWidgetReferences(); return _waterPurityGauge; } }
        public TemperatureReadoutWidget TemperatureReadoutWidget  { get { EnsureWidgetReferences(); return _temperatureReadoutWidget; } }
        public PowerFlowSchematic       PowerFlowSchematic       { get { EnsureWidgetReferences(); return _powerFlowSchematic; } }
        public FactionPressureRing      FactionPressureRing      { get { EnsureWidgetReferences(); return _factionPressureRing; } }
        public ExpeditionCountdownTimer ExpeditionCountdownTimer  { get { EnsureWidgetReferences(); return _expeditionCountdownTimer; } }
        public RadioSignalStrengthBar   RadioSignalStrengthBar   { get { EnsureWidgetReferences(); return _radioSignalStrengthBar; } }
        public CraftQueueStrip          CraftQueueStrip          { get { EnsureWidgetReferences(); return _craftQueueStrip; } }
        public AlertToastNotification   AlertToastNotification   { get { EnsureWidgetReferences(); return _alertToastNotification; } }
        public BunkerFloorMapMiniature  BunkerFloorMapMiniature  { get { EnsureWidgetReferences(); return _bunkerFloorMapMiniature; } }
        public DayNightArcClock         DayNightArcClock         { get { EnsureWidgetReferences(); return _dayNightArcClock; } }
        public BloodTypeIndicator       BloodTypeIndicator       { get { EnsureWidgetReferences(); return _bloodTypeIndicator; } }
        public LootHaulTicker           LootHaulTicker           { get { EnsureWidgetReferences(); return _lootHaulTicker; } }
        public EndgameVictoryPathTracker EndgameVictoryPathTracker{ get { EnsureWidgetReferences(); return _endgameVictoryPathTracker; } }
        public FactionRadioVoHook FactionRadioVoHook
        {
            get
            {
                EnsureWidgetReferences();
                return _factionRadioVoHook != null
                    ? _factionRadioVoHook
                    : (_radioInterceptHud != null ? _radioInterceptHud.VoHook : null);
            }
        }
        public bool DebugModeEnabled => _debugModeEnabled;

        private PersonalQuestSystem _personalQuests;
        private System.Random _needsUiRng;

        /// <summary>#255 Deceptive UI mask — bind PersonalQuestSystem for NeedsBar lies.</summary>
        public void BindPersonalQuests(PersonalQuestSystem personalQuests, System.Random rng = null)
        {
            _personalQuests = personalQuests;
            _needsUiRng = rng;
        }

        private void Awake()
        {
            EnsureWidgetReferences();
        }

        private void EnsureWidgetReferences()
        {
            EnsureWidget(ref _needsBar);
            EnsureWidget(ref _dosimeterHud);
            EnsureWidget(ref _healthTrajectoryHud);
            EnsureWidget(ref _geigerAudioHook);
            EnsureWidget(ref _environmentStatusHud);
            EnsureWidget(ref _eventModalUi);
            EnsureWidget(ref _mapKnowledgeHud);
            EnsureWidget(ref _tradeScreenUi);
            EnsureWidget(ref _powerGridHud);
            EnsureWidget(ref _mapScreenUi);
            EnsureWidget(ref _workbenchUi);
            EnsureWidget(ref _hatchDefenseHud);
            EnsureWidget(ref _scavengeDispatchHud);
            EnsureWidget(ref _overflowCrateHud);
            EnsureWidget(ref _fieldGearLoadoutHud);
            EnsureWidget(ref _bunkerRationingHud);
            EnsureWidget(ref _waterPurificationHud);
            EnsureWidget(ref _airHeatManagementHud);
            EnsureWidget(ref _bunkerMaintenanceHud);
            EnsureWidget(ref _survivorTaskBoardHud);
            EnsureWidget(ref _roomAssignmentHud);
            EnsureWidget(ref _radioInterceptHud);
            EnsureWidget(ref _journalBookUi);
            EnsureWidget(ref _inventoryStripUi);
            EnsureWidget(ref _endgameSummaryUi);
            EnsureWidget(ref _internalHorrorHud);
            EnsureWidget(ref _expeditionEncounterLogHud);
            EnsureWidget(ref _diegeticHud);
            EnsureWidget(ref _moralChronicleUi);
            EnsureWidget(ref _tutorialOverlay);
            // Batch-20 widgets
            EnsureWidget(ref _radiationDosimeterWidget);
            EnsureWidget(ref _geigerSweepGauge);
            EnsureWidget(ref _airFilterIntegrityBar);
            EnsureWidget(ref _falloutStormWarningBanner);
            EnsureWidget(ref _survivorPortraitCard);
            EnsureWidget(ref _moralDecayMeter);
            EnsureWidget(ref _rationAllocationDial);
            EnsureWidget(ref _waterPurityGauge);
            EnsureWidget(ref _temperatureReadoutWidget);
            EnsureWidget(ref _powerFlowSchematic);
            EnsureWidget(ref _factionPressureRing);
            EnsureWidget(ref _expeditionCountdownTimer);
            EnsureWidget(ref _radioSignalStrengthBar);
            EnsureWidget(ref _craftQueueStrip);
            EnsureWidget(ref _alertToastNotification);
            EnsureWidget(ref _bunkerFloorMapMiniature);
            EnsureWidget(ref _dayNightArcClock);
            EnsureWidget(ref _bloodTypeIndicator);
            EnsureWidget(ref _lootHaulTicker);
            EnsureWidget(ref _endgameVictoryPathTracker);
            if (_factionRadioVoHook == null)
            {
                _factionRadioVoHook = GetComponentInChildren<FactionRadioVoHook>();
                if (_factionRadioVoHook == null && _radioInterceptHud != null)
                    _factionRadioVoHook = _radioInterceptHud.VoHook;
            }
        }

        /// <summary>
        /// H-5: logs a warning the first time a widget isn't found via scene wiring
        /// or GetComponentInChildren and has to be auto-created, since AddComponent
        /// yields a default-constructed instance with none of its Inspector-set
        /// fields — silent auto-creation was hiding missing prefab wiring.
        /// </summary>
        private T EnsureWidget<T>(ref T field) where T : Component
        {
            if (field != null) return field;

            field = GetComponentInChildren<T>();
            if (field == null)
            {
                Debug.LogWarning(
                    $"[HUD] {typeof(T).Name} not found via scene wiring — auto-creating with AddComponent(). " +
                    "Wire it explicitly in the HUD prefab/scene instead of relying on this fallback.",
                    this);
                field = gameObject.AddComponent<T>();
            }
            return field;
        }

        /// <summary>Ensure the diegetic journal book exists on the HUD.</summary>
        public JournalBookUI EnsureJournalBook()
        {
            EnsureWidgetReferences();
            return _journalBookUi;
        }

        /// <summary>Ensure the post-game endgame summary screen exists on the HUD.</summary>
        public EndgameSummaryUI EnsureEndgameSummary()
        {
            EnsureWidgetReferences();
            return _endgameSummaryUi;
        }

        /// <summary>Ensure Internal Horror status / dispose / fire panels exist.</summary>
        public InternalHorrorHUD EnsureInternalHorrorHud()
        {
            EnsureWidgetReferences();
            return _internalHorrorHud;
        }

        /// <summary>Ensure the post-game moral chronicle screen exists on the HUD.</summary>
        public MoralChronicleUI EnsureMoralChronicle()
        {
            EnsureWidgetReferences();
            return _moralChronicleUi;
        }

        /// <summary>Ensure the first-run tutorial overlay exists on the HUD.</summary>
        public TutorialOverlay EnsureTutorialOverlay()
        {
            EnsureWidgetReferences();
            return _tutorialOverlay;
        }

        /// <summary>Push Internal Horror snapshot (corpses, fire, coma, rusted food).</summary>
        public void OnInternalHorrorUpdated(InternalHorrorSnapshot snap)
        {
            EnsureWidgetReferences();
            _internalHorrorHud?.ApplySnapshot(snap);
        }

        /// <summary>
        /// Bind the room-assignment widget to the live survivor list and
        /// shelter. Called once from GameBootstrap after the survivors
        /// are created and the shelter is fully populated.
        /// </summary>
        public void BindRoomAssignment(IReadOnlyList<AtomicWar._Game.Survivors.Survivor> survivors, Shelter.Shelter shelter)
        {
            EnsureWidgetReferences();
            _roomAssignmentHud?.Bind(survivors, shelter);
        }

        private void Update()
        {
            if (Input.GetKeyDown(_debugToggleKey))
            {
                SetDebugMode(!_debugModeEnabled);
            }

            RepaintEventModalIfChanged();
            RepaintEndgameIfChanged();
            RepaintPowerGridIfChanged();
        }

        private bool _lastPowerGridOpen;
        private string _lastPowerBudget;
        private string _lastPowerSources;
        private string _lastPowerLoads;

        /// <summary>
        /// Repaint the power budget when it opens, closes, or any of its three
        /// blocks change. Polled because PowerGridHUD publishes no event of its
        /// own -- it subscribes to PowerNetwork.OnPowerStateChanged and refreshes
        /// its cached strings, but never tells anyone.
        ///
        /// All three strings are compared, not just the budget line: a priority
        /// change (P1 -> P2) or a source flipping state at equal wattage rewrites
        /// ConsumersSummary / SourcesSummary while leaving BudgetSummary
        /// byte-identical, and the panel would freeze mid-interaction. When
        /// nothing changed these are the same string instances, so each compare
        /// short-circuits on the reference check.
        /// </summary>
        private void RepaintPowerGridIfChanged()
        {
            if (_diegeticHud == null || _powerGridHud == null) return;

            bool open = _powerGridHud.IsOpen;
            string budget = open ? _powerGridHud.BudgetSummary : null;
            string sources = open ? _powerGridHud.SourcesSummary : null;
            string loads = open ? _powerGridHud.ConsumersSummary : null;
            if (open == _lastPowerGridOpen
                && budget == _lastPowerBudget
                && sources == _lastPowerSources
                && loads == _lastPowerLoads)
            {
                return;
            }

            _lastPowerGridOpen = open;
            _lastPowerBudget = budget;
            _lastPowerSources = sources;
            _lastPowerLoads = loads;
            _diegeticHud.PaintPowerGrid(open, budget, sources, loads);
        }

        private bool _lastEndgameVisible;
        private string _lastEndgameStatus;

        /// <summary>
        /// Repaint the terminal campaign readout when it appears, disappears, or
        /// its tallies change. Polled because EndgameSummaryUI is the one HUD
        /// widget with no change event -- Show()/Hide()/Clear() all just call its
        /// private Refresh(). Comparing StatusLine covers the tallies too: it is
        /// rebuilt from state, days and radiation on every Refresh.
        /// </summary>
        private void RepaintEndgameIfChanged()
        {
            if (_diegeticHud == null || _endgameSummaryUi == null) return;

            bool visible = _endgameSummaryUi.IsVisible;
            string status = visible ? _endgameSummaryUi.StatusLine : null;
            if (visible == _lastEndgameVisible && status == _lastEndgameStatus) return;

            _lastEndgameVisible = visible;
            _lastEndgameStatus = status;
            _diegeticHud.PaintEndgame(visible, status, _endgameSummaryUi.DetailSummary);
        }

        private bool _lastModalOpen;
        private string _lastModalEventId;
        // Last-painted body text and choice fingerprint while the modal is open.
        // The body is re-resolved each frame from the live context (faction trust,
        // survivor state) so a context shift that flips the threatening-copy swap
        // or greys out a choice repaints without waiting for a new event.
        private string _lastPaintedBody;
        private int _lastPaintedChoiceFingerprint;

        /// <summary>
        /// Repaint the event prompt when it opens, closes, swaps to a different
        /// event, or its body / choice gating actually changes. Deliberately
        /// polled rather than driven from EventRunner.OnEventTriggered:
        /// EventModalUI subscribes to that same event and has to update its
        /// state before this paints it, and relying on subscriber registration
        /// order is a dependency no test would catch when it broke.
        /// </summary>
        private void RepaintEventModalIfChanged()
        {
            if (_eventModalUi == null || _diegeticHud == null) return;

            bool open = _eventModalUi.IsOpen;
            string id = open && _eventModalUi.ActiveEvent != null
                ? _eventModalUi.ActiveEvent.id
                : null;

            // Fast bail: same open/close + id as the last frame.
            if (open == _lastModalOpen && id == _lastModalEventId)
            {
                if (!open) return; // closed with same id: no work
                // Open with same id: re-resolve to track live context (faction
                // trust can shift the body swap; flags can change gating), but
                // skip the per-row Label rebuild when the fingerprint matches
                // the one we last drew.
                var evStable = _eventModalUi.ActiveEvent;
                var ctxStable = _eventModalUi.ActiveContext;
                if (evStable == null || ctxStable == null) return;
                string bodyStable = evStable.ResolveBodyText(ctxStable);
                var visibleStable = EventRunner.GetVisibleChoices(evStable, ctxStable);
                int fingerprintStable = ComputeChoicesFingerprint(visibleStable);
                if (fingerprintStable == _lastPaintedChoiceFingerprint
                    && bodyStable == _lastPaintedBody)
                {
                    return;
                }
                // Things changed; fall through to repaint.
                _lastPaintedBody = bodyStable;
                _lastPaintedChoiceFingerprint = fingerprintStable;
                EmitPaint(open, evStable, bodyStable, visibleStable);
                return;
            }

            // Open/close or id changed: redraw.
            _lastModalOpen = open;
            _lastModalEventId = id;
            if (!open)
            {
                _lastPaintedBody = null;
                _lastPaintedChoiceFingerprint = 0;
                _diegeticHud.PaintEventModal(open, null, null, null);
                return;
            }

            var ev = _eventModalUi.ActiveEvent;
            var ctx = _eventModalUi.ActiveContext;
            if (ev == null || ctx == null) return;
            string body = ev.ResolveBodyText(ctx);
            var visible = EventRunner.GetVisibleChoices(ev, ctx);
            int fingerprint = ComputeChoicesFingerprint(visible);
            _lastPaintedBody = body;
            _lastPaintedChoiceFingerprint = fingerprint;
            EmitPaint(open, ev, body, visible);
        }

        private void EmitPaint(
            bool open,
            GameEvent ev,
            string body,
            System.Collections.Generic.IReadOnlyList<PresentedEventChoice> visible)
        {
            var lines = new List<EventChoiceLine>(visible.Count);
            for (int i = 0; i < visible.Count; i++)
            {
                var c = visible[i];
                lines.Add(new EventChoiceLine(c.Text, c.IsAvailable && !c.IsGrayedOut));
            }
            _diegeticHud.PaintEventModal(open, ev.title, body, lines);
        }

        /// <summary>
        /// Cheap content fingerprint for the visible choice list. Hashes the
        /// (enabled, text-hash) pair of each row so a re-resolution that didn't
        /// actually change the gating or copy reads as equal even when the
        /// list is a freshly-allocated reference.
        /// </summary>
        private static int ComputeChoicesFingerprint(System.Collections.Generic.IReadOnlyList<PresentedEventChoice> choices)
        {
            unchecked
            {
                int h = 17;
                for (int i = 0; i < choices.Count; i++)
                {
                    var c = choices[i];
                    bool enabled = c.IsAvailable && !c.IsGrayedOut;
                    int textHash = c.Text == null ? 0 : c.Text.GetHashCode();
                    h = h * 31 + (enabled ? 1 : 0);
                    h = h * 31 + textHash;
                }
                return h;
            }
        }

        public void SetDebugMode(bool enabled)
        {
            _debugModeEnabled = enabled;
            EnsureWidgetReferences();
            if (_needsBar != null) _needsBar.SetShowRawValues(_debugModeEnabled);
            if (_dosimeterHud != null) _dosimeterHud.SetShowRawValues(_debugModeEnabled);
            if (_environmentStatusHud != null) _environmentStatusHud.SetShowRawValues(_debugModeEnabled);
        }

        /// <summary>Bind widgets to a specific survivor's state snapshot.</summary>
        public void Bind(Survivor survivor)
        {
            if (survivor == null) return;
            EnsureWidgetReferences();
            float health = survivor.Needs != null ? survivor.Needs.Health : 100f;
            _needsBar.SetNeeds(
                survivor.Needs,
                health,
                survivor.RadiationDose,
                survivor,
                _personalQuests,
                _needsUiRng);
            RepaintVitals();
        }

        /// <summary>Bind radiation system readings to Dosimeter and Geiger Audio.</summary>
        public void OnRadiationUpdated(float cumulativeDose, float currentRate)
        {
            EnsureWidgetReferences();
            if (_dosimeterHud != null) _dosimeterHud.SetReading(cumulativeDose, currentRate);
            if (_geigerAudioHook != null) _geigerAudioHook.UpdateExposureRate(currentRate);
            RepaintVitals();
        }

        /// <summary>
        /// Latest clock reading. Pushed in rather than pulled: HUD holds no
        /// bootstrap or TimeSystem reference, and every other value it shows is
        /// pushed too. The simulation's clock advances in whole hours, so the
        /// readout sits at HH:00 between ticks.
        /// </summary>
        private int _day = 1;
        private float _hour;

        public void SetClock(int day, float hour)
        {
            _day = day;
            _hour = hour;
            RepaintVitals();
        }

        /// <summary>
        /// The other diegetic panels repaint on discrete actions via
        /// RefreshDiegeticHud. Vitals cannot: needs and dose change continuously,
        /// and a panel painted only on mission events would sit frozen while the
        /// player starved. Null widgets paint nothing and throw nothing -- the
        /// HUD must never take down the simulation.
        /// </summary>
        private void RepaintVitals()
        {
            EnsureWidgetReferences();
            if (_diegeticHud == null || _needsBar == null) return;

            _diegeticHud.PaintVitals(
                _day,
                _hour,
                _dosimeterHud != null ? _dosimeterHud.CumulativeDose : 0f,
                _dosimeterHud != null ? _dosimeterHud.CurrentRate : 0f,
                _needsBar.NeedBars);
        }

        /// <summary>
        /// Perform a medical exam: the only way HealthTrajectoryHUD ever updates. Call from
        /// a doctor/medicine action, not from the per-frame tick -- the player should have
        /// to seek this out rather than have it pushed at them like the dosimeter's rate.
        /// </summary>
        public void OnMedicalExam(Survivor survivor, RadiationSystem radiationSystem)
        {
            if (survivor == null || radiationSystem == null) return;
            EnsureWidgetReferences();
            _healthTrajectoryHud.SetReading(radiationSystem.ExaminePrognosis(survivor));
        }

        /// <summary>
        /// Prompt #203 — medical exam with optional LatentDamage/OnsetTimer reveal.
        /// Radiologist examiners reveal without a kit; others need tryConsumeKit to succeed.
        /// </summary>
        public void OnMedicalExam(
            Survivor examiner,
            Survivor patient,
            RadiationSystem radiationSystem,
            MedicalPerkSystem medicalPerks,
            System.Func<bool> tryConsumeKit = null)
        {
            if (patient == null || radiationSystem == null) return;
            EnsureWidgetReferences();
            var estimate = radiationSystem.ExaminePrognosis(patient);
            if (medicalPerks != null
                && medicalPerks.TryRevealLatentDamage(
                    examiner, patient, tryConsumeKit,
                    out float latent, out float onset))
            {
                _healthTrajectoryHud.SetReading(estimate, latent, onset);
            }
            else
            {
                _healthTrajectoryHud.SetReading(estimate);
            }
        }

        /// <summary>Bind shelter aggregate stats to Environment status strip.</summary>
        public void OnShelterUpdated(Shelter.Shelter shelter)
        {
            if (shelter == null) return;
            EnsureWidgetReferences();
            var airModule = shelter.GetModule("air_filtration");
            float filterHealth = airModule != null ? airModule.FilterHealth : 100f;
            _environmentStatusHud.SetShelterStats(shelter.AirQuality, filterHealth);
        }

        /// <summary>Bind EventRunner to EventModalUI.</summary>
        public void BindEventRunner(EventRunner runner)
        {
            EnsureWidgetReferences();
            if (_eventModalUi != null) _eventModalUi.Bind(runner);
        }

        /// <summary>Bind DynamicEconomySystem to the trade / barter screen.</summary>
        public void BindEconomy(DynamicEconomySystem economy)
        {
            EnsureWidgetReferences();
            if (_tradeScreenUi != null) _tradeScreenUi.Bind(economy);
        }

        /// <summary>Bind shelter PowerNetwork to the power budget panel.</summary>
        public void BindPowerNetwork(PowerNetwork network)
        {
            EnsureWidgetReferences();
            if (_powerGridHud != null) _powerGridHud.Bind(network);
        }

        /// <summary>Bind proc-gen wasteland map + weather for expedition pathing UI.</summary>
        public void BindGeneratedMap(GeneratedMap map, System.Func<WeatherKind> getWeather = null)
        {
            EnsureWidgetReferences();
            if (_mapScreenUi != null) _mapScreenUi.Bind(map, getWeather);
        }

        /// <summary>Bind workbench disassembly / repair screen.</summary>
        public void BindWorkbench(AtomicWar._Game.Crafting.WorkbenchSystem workbench)
        {
            EnsureWidgetReferences();
            if (_workbenchUi != null) _workbenchUi.Bind(workbench);
        }

        /// <summary>Bind hatch defense / raid status panel.</summary>
        public void BindHatchDefense(HatchDefenseSystem hatch)
        {
            EnsureWidgetReferences();
            if (_hatchDefenseHud != null) _hatchDefenseHud.Bind(hatch);
        }

        /// <summary>
        /// Bind the player-facing dispatch board. Core owns the mission system
        /// and start eligibility; the HUD only presents its pushed state.
        /// </summary>
        public void BindScavengeDispatch(
            LocationCatalogSO catalog,
            System.Func<IReadOnlyList<Survivor>> getSurvivors,
            System.Func<Survivor, string> getDispatchBlockReason,
            System.Func<Survivor, string> getTaskLabel,
            System.Func<string> getMissionRoster,
            System.Func<Survivor, LocationDefinitionSO, string> getPreflightSummary,
            System.Func<string, string> getRadiationPreview,
            System.Func<float, string> getLootPreview)
        {
            EnsureWidgetReferences();
            _scavengeDispatchHud?.Bind(
                catalog,
                getSurvivors,
                getDispatchBlockReason,
                getTaskLabel,
                getMissionRoster,
                getPreflightSummary,
                getRadiationPreview,
                getLootPreview);
        }

        /// <summary>Bind the bunker receiving-crate panel to Core-owned snapshot data.</summary>
        public void BindOverflowCrate(System.Func<OverflowCrateSnapshot> getSnapshot)
        {
            EnsureWidgetReferences();
            _overflowCrateHud?.Bind(getSnapshot);
        }

        /// <summary>Bind the field face/body protection panel to Core snapshot data.</summary>
        public void BindFieldGearLoadout(System.Func<FieldGearLoadoutSnapshot> getSnapshot)
        {
            EnsureWidgetReferences();
            _fieldGearLoadoutHud?.Bind(getSnapshot);
        }

        /// <summary>Bind the daily bunker ration board to Core-owned policy state.</summary>
        public void BindBunkerRationing(System.Func<BunkerRationingSnapshot> getSnapshot)
        {
            EnsureWidgetReferences();
            _bunkerRationingHud?.Bind(getSnapshot);
        }

        /// <summary>Bind the cistern/purifier terminal and its ration projection.</summary>
        public void BindWaterPurification(
            System.Func<WaterPurificationSnapshot> getWaterSnapshot,
            System.Func<BunkerRationingSnapshot> getRationSnapshot)
        {
            EnsureWidgetReferences();
            _waterPurificationHud?.Bind(getWaterSnapshot, getRationSnapshot);
        }

        /// <summary>Bind the climate terminal to a Core-owned air/heat snapshot.</summary>
        public void BindAirHeatManagement(System.Func<AirHeatManagementSnapshot> getSnapshot)
        {
            EnsureWidgetReferences();
            _airHeatManagementHud?.Bind(getSnapshot);
        }

        /// <summary>Bind the repair-order terminal to Core-owned maintenance state.</summary>
        public void BindBunkerMaintenance(
            System.Func<BunkerMaintenanceSnapshot> getSnapshot,
            System.Func<System.Collections.Generic.IReadOnlyList<AtomicWar._Game.Survivors.Survivor>> getSurvivors,
            System.Func<RepairWorkOrderSnapshot> getWorkOrderSnapshot = null)
        {
            EnsureWidgetReferences();
            _bunkerMaintenanceHud?.Bind(getSnapshot, getSurvivors, getWorkOrderSnapshot);
        }

        /// <summary>Bind the survivor allocation board to its detached Core snapshot.</summary>
        public void BindSurvivorTaskBoard(System.Func<SurvivorTaskBoardSnapshot> getSnapshot)
        {
            EnsureWidgetReferences();
            _survivorTaskBoardHud?.Bind(getSnapshot);
        }

        /// <summary>Ensure expedition combat/encounter log strip exists.</summary>
        public ExpeditionEncounterLogHUD EnsureExpeditionEncounterLog()
        {
            EnsureWidgetReferences();
            return _expeditionEncounterLogHud;
        }

        /// <summary>
        /// Ensure UI Toolkit diegetic HUD is mounted on a live UIDocument (play mode),
        /// built, and bound to hatch / stores / encounter-log view-models.
        /// </summary>
        public DiegeticHudController EnsureDiegeticHud()
        {
            EnsureWidgetReferences();
            if (_diegeticHud == null) return null;

            _diegeticHud.EnsureDocumentMounted();
            _diegeticHud.EnsureBuilt();
            _diegeticHud.BindSources(_hatchDefenseHud, _inventoryStripUi, _expeditionEncounterLogHud, _workbenchUi, _endgameSummaryUi, _powerGridHud, _scavengeDispatchHud, _overflowCrateHud, _fieldGearLoadoutHud, _bunkerRationingHud, _waterPurificationHud, _airHeatManagementHud, _bunkerMaintenanceHud, _survivorTaskBoardHud);
            return _diegeticHud;
        }

        /// <summary>Repaint diegetic UI Toolkit panels from current view-models.</summary>
        public void RefreshDiegeticHud()
        {
            EnsureWidgetReferences();
            _diegeticHud?.Paint();
        }

        /// <summary>
        /// Ensure the radio intercept strip exists and VO stubs are ready.
        /// Lines are pushed from GameBootstrap (Core owns the intercept system).
        /// </summary>
        public RadioInterceptHUD EnsureRadioInterceptHud()
        {
            EnsureWidgetReferences();
            if (_radioInterceptHud != null)
            {
                var vo = _radioInterceptHud.VoHook;
                vo?.EnsureBuiltInStubs();
                if (_factionRadioVoHook == null)
                    _factionRadioVoHook = vo;
            }
            return _radioInterceptHud;
        }

        /// <summary>
        /// Poll-friendly update: call from GameBootstrap.Update to push environment
        /// data (time, weather, season) to the EnvironmentStatusHUD widget each frame.
        /// </summary>
        public void Tick(int day, float hour, string weatherName, string seasonName, float timeScale = 1f)
        {
            EnsureWidgetReferences();
            if (_environmentStatusHud != null)
            {
                _environmentStatusHud.SetEnvironment(day, hour, weatherName, seasonName);
                _environmentStatusHud.SetTimeScale(timeScale);
            }
            if (_hatchDefenseHud != null)
            {
                _hatchDefenseHud.SetDay(day);
            }
        }

        /// <summary>
        /// Push radiation fog-of-war views + "last calibrated: N days ago" strip.
        /// </summary>
        public void OnMapKnowledgeUpdated(
            System.Collections.Generic.IReadOnlyList<MapTilePlayerView> views,
            bool hasWorkingGeiger,
            int daysSinceCalibration)
        {
            EnsureWidgetReferences();
            if (_mapKnowledgeHud == null) return;
            _mapKnowledgeHud.SetViews(views, hasWorkingGeiger);
            _mapKnowledgeHud.SetCalibrationAge(daysSinceCalibration);
        }

        /// <summary>Push a transient event notification to the HUD (Expansion IV hook).</summary>
        public void PushEventText(string message)
        {
            Debug.Log("[HUD Event] " + message);
        }
    }
}
