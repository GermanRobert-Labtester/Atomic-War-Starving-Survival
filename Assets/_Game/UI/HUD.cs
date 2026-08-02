using UnityEngine;
using AtomicWar._Game.Survivors;
using AtomicWar._Game.Shelter;
using AtomicWar._Game.Events;

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
        [SerializeField] private GeigerAudioHook _geigerAudioHook;
        [SerializeField] private EnvironmentStatusHUD _environmentStatusHud;
        [SerializeField] private EventModalUI _eventModalUi;

        [SerializeField] private KeyCode _debugToggleKey = KeyCode.F2;
        [SerializeField] private bool _debugModeEnabled = false;

        public NeedsBar NeedsBar { get { EnsureWidgetReferences(); return _needsBar; } }
        public DosimeterHUD DosimeterHUD { get { EnsureWidgetReferences(); return _dosimeterHud; } }
        public GeigerAudioHook GeigerAudioHook { get { EnsureWidgetReferences(); return _geigerAudioHook; } }
        public EnvironmentStatusHUD EnvironmentStatusHud { get { EnsureWidgetReferences(); return _environmentStatusHud; } }
        public EventModalUI EventModalUI { get { EnsureWidgetReferences(); return _eventModalUi; } }
        public bool DebugModeEnabled => _debugModeEnabled;

        private void Awake()
        {
            EnsureWidgetReferences();
        }

        private void EnsureWidgetReferences()
        {
            if (_needsBar == null) _needsBar = GetComponentInChildren<NeedsBar>() ?? gameObject.AddComponent<NeedsBar>();
            if (_dosimeterHud == null) _dosimeterHud = GetComponentInChildren<DosimeterHUD>() ?? gameObject.AddComponent<DosimeterHUD>();
            if (_geigerAudioHook == null) _geigerAudioHook = GetComponentInChildren<GeigerAudioHook>() ?? gameObject.AddComponent<GeigerAudioHook>();
            if (_environmentStatusHud == null) _environmentStatusHud = GetComponentInChildren<EnvironmentStatusHUD>() ?? gameObject.AddComponent<EnvironmentStatusHUD>();
            if (_eventModalUi == null) _eventModalUi = GetComponentInChildren<EventModalUI>() ?? gameObject.AddComponent<EventModalUI>();
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
    }
}
