using System.Collections.Generic;
using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Events;
using AtomicWar._Game.Environment;
using AtomicWar._Game.Radiation;
using AtomicWar._Game.Economy;

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
        [SerializeField] private RoomAssignmentHUD _roomAssignmentHud;

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
        public RoomAssignmentHUD RoomAssignmentHUD { get { EnsureWidgetReferences(); return _roomAssignmentHud; } }
        public bool DebugModeEnabled => _debugModeEnabled;

        private void Awake()
        {
            EnsureWidgetReferences();
        }

        private void EnsureWidgetReferences()
        {
            if (_needsBar == null) _needsBar = GetComponentInChildren<NeedsBar>() ?? gameObject.AddComponent<NeedsBar>();
            if (_dosimeterHud == null) _dosimeterHud = GetComponentInChildren<DosimeterHUD>() ?? gameObject.AddComponent<DosimeterHUD>();
            if (_healthTrajectoryHud == null) _healthTrajectoryHud = GetComponentInChildren<HealthTrajectoryHUD>() ?? gameObject.AddComponent<HealthTrajectoryHUD>();
            if (_geigerAudioHook == null) _geigerAudioHook = GetComponentInChildren<GeigerAudioHook>() ?? gameObject.AddComponent<GeigerAudioHook>();
            if (_environmentStatusHud == null) _environmentStatusHud = GetComponentInChildren<EnvironmentStatusHUD>() ?? gameObject.AddComponent<EnvironmentStatusHUD>();
            if (_eventModalUi == null) _eventModalUi = GetComponentInChildren<EventModalUI>() ?? gameObject.AddComponent<EventModalUI>();
            if (_mapKnowledgeHud == null) _mapKnowledgeHud = GetComponentInChildren<MapKnowledgeHUD>() ?? gameObject.AddComponent<MapKnowledgeHUD>();
            if (_tradeScreenUi == null) _tradeScreenUi = GetComponentInChildren<TradeScreenUI>() ?? gameObject.AddComponent<TradeScreenUI>();
            if (_powerGridHud == null) _powerGridHud = GetComponentInChildren<PowerGridHUD>() ?? gameObject.AddComponent<PowerGridHUD>();
            if (_mapScreenUi == null) _mapScreenUi = GetComponentInChildren<MapScreenUI>() ?? gameObject.AddComponent<MapScreenUI>();
            if (_workbenchUi == null) _workbenchUi = GetComponentInChildren<WorkbenchUI>() ?? gameObject.AddComponent<WorkbenchUI>();
            if (_hatchDefenseHud == null) _hatchDefenseHud = GetComponentInChildren<HatchDefenseHUD>() ?? gameObject.AddComponent<HatchDefenseHUD>();
            if (_roomAssignmentHud == null) _roomAssignmentHud = GetComponentInChildren<RoomAssignmentHUD>() ?? gameObject.AddComponent<RoomAssignmentHUD>();
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
            _needsBar.SetNeeds(survivor.Needs, 100f, survivor.RadiationDose);
        }

        /// <summary>Bind radiation system readings to Dosimeter and Geiger Audio.</summary>
        public void OnRadiationUpdated(float cumulativeDose, float currentRate)
        {
            EnsureWidgetReferences();
            if (_dosimeterHud != null) _dosimeterHud.SetReading(cumulativeDose, currentRate);
            if (_geigerAudioHook != null) _geigerAudioHook.UpdateExposureRate(currentRate);
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
        /// Poll-friendly update: call from GameBootstrap.Update to push environment
        /// data (time, weather, season) to the EnvironmentStatusHUD widget each frame.
        /// </summary>
        public void Tick(int day, float hour, string weatherName, string seasonName)
        {
            EnsureWidgetReferences();
            if (_environmentStatusHud != null)
            {
                _environmentStatusHud.SetEnvironment(day, hour, weatherName, seasonName);
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
    }
}
