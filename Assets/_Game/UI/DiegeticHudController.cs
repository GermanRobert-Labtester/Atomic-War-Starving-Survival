using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Mounts <see cref="DiegeticHudView"/> into a live <see cref="UIDocument"/>
    /// for play mode, and keeps hatch ammo / arms, encounter log, and stores
    /// focus tooltip in sync with the string view-models.
    /// EditMode tests use <see cref="BuildDetachedForTests"/> (no document).
    /// </summary>
    public class DiegeticHudController : MonoBehaviour
    {
        public const string UxmlResourcePath = "Assets/_Game/UI/DiegeticHud.uxml";
        public const string UssResourcePath = "Assets/_Game/UI/DiegeticHud.uss";
        public const string PanelSettingsAssetPath = "Assets/_Game/UI/DiegeticHudPanelSettings.asset";
        /// <summary>Fallback shared with main menu if dedicated diegetic settings missing.</summary>
        public const string SharedPanelSettingsPath = "Assets/_Game/UI/MainMenu/MainMenuPanelSettings.asset";

        /// <summary>Resources.Load paths (player builds / play mode without inspector wiring).</summary>
        public const string ResourcesPanelSettings = "UI/DiegeticHudPanelSettings";
        public const string ResourcesUxml = "UI/DiegeticHud";
        public const string ResourcesUss = "UI/DiegeticHud";

        public const float DefaultSortingOrder = 50f;

        [SerializeField] private UIDocument _document;
        [SerializeField] private PanelSettings _panelSettings;
        [SerializeField] private VisualTreeAsset _uxml;
        [SerializeField] private StyleSheet _uss;
        [SerializeField] private bool _createDocumentIfMissing = true;

        private DiegeticHudView _view = new DiegeticHudView();
        private HatchDefenseHUD _hatch;
        private InventoryStripUI _strip;
        private ExpeditionEncounterLogHUD _encounterLog;
        private WorkbenchUI _workbench;
        private EndgameSummaryUI _endgame;
        private PowerGridHUD _powerGrid;
        private ScavengeDispatchHUD _scavengeDispatch;
        private OverflowCrateHUD _overflowCrate;
        private FieldGearLoadoutHUD _fieldGearLoadout;
        private BunkerRationingHUD _bunkerRationing;
        private WaterPurificationHUD _waterPurification;
        private AirHeatManagementHUD _airHeatManagement;
        private BunkerMaintenanceHUD _bunkerMaintenance;
        private SurvivorTaskBoardHUD _survivorTaskBoard;
        // Expansion II: faction-pressure HUD widget. Painted in PaintFactionPressure().
        private FactionPressureHUD _factionPressure;
        private bool _built;
        private bool _tooltipPinned;
        private bool _preferDetached;
        private PanelSettings _runtimePanelSettings;
        // Set to true when EnsureDocumentMounted adds a UIDocument to the
        // GameObject because none was assigned in the inspector. OnDestroy
        // removes the component only in that case -- never a document the
        // designer wired up by hand.
        private bool _ownsDocument;

        /// <summary>Test / host access to the painted tree.</summary>
        public DiegeticHudView View => _view;
        public VisualElement Root => _view?.Root;
        public bool IsBuilt => _built;
        public UIDocument Document => _document;

        /// <summary>
        /// True when a UIDocument is present with panel settings assigned
        /// (play-mode paint path — not the detached EditMode host).
        /// </summary>
        public bool IsDocumentMounted =>
            !_preferDetached
            && _document != null
            && _document.panelSettings != null;

        /// <summary>
        /// When true, stores tooltip stays open after selection even if strip
        /// loses selection until cleared (keyboard pin via focus path).
        /// </summary>
        public bool TooltipPinned
        {
            get => _tooltipPinned;
            set
            {
                _tooltipPinned = value;
                Paint();
            }
        }

        private void Awake()
        {
            if (!_preferDetached)
                EnsureDocumentMounted();
            EnsureBuilt();
        }

        private void OnEnable()
        {
            if (!_preferDetached)
                EnsureDocumentMounted();
            EnsureBuilt();
            Paint();
        }

        private void OnDestroy()
        {
            UnbindSources();
            if (_runtimePanelSettings != null)
            {
                Destroy(_runtimePanelSettings);
                _runtimePanelSettings = null;
            }
            // Only destroy a UIDocument we added ourselves. The inspector
            // may have wired one up and the designer is responsible for it.
            if (_ownsDocument && _document != null)
            {
                Destroy(_document);
                _document = null;
            }
        }

        /// <summary>
        /// Ensure a UIDocument + PanelSettings + UXML/USS are wired so play mode
        /// paints to a real panel (not only a detached VisualElement host).
        /// Safe to call repeatedly. No-ops after <see cref="BuildDetachedForTests"/>.
        /// </summary>
        public bool EnsureDocumentMounted()
        {
            if (_preferDetached) return false;

            TryLoadAssets();

            if (_document == null)
                _document = GetComponent<UIDocument>();

            if (_document == null && _createDocumentIfMissing)
            {
                _document = gameObject.AddComponent<UIDocument>();
                _ownsDocument = true;
            }

            if (_document == null) return false;

            if (_panelSettings == null)
                _panelSettings = CreateFallbackPanelSettings();

            if (_document.panelSettings != _panelSettings)
                _document.panelSettings = _panelSettings;

            if (_uxml != null && _document.visualTreeAsset != _uxml)
                _document.visualTreeAsset = _uxml;

            // Force rebuild of view binding against the live document root.
            _built = false;
            return true;
        }

        /// <summary>
        /// Build tree into UIDocument when available; otherwise into a detached
        /// VisualElement host (EditMode unit tests).
        /// </summary>
        public void EnsureBuilt()
        {
            if (_built && _view.Root != null) return;

            TryLoadAssets();

            if (!_preferDetached && _document != null)
            {
                if (_panelSettings != null && _document.panelSettings != _panelSettings)
                    _document.panelSettings = _panelSettings;
                if (_uxml != null && _document.visualTreeAsset != _uxml)
                    _document.visualTreeAsset = _uxml;

                var docRoot = _document.rootVisualElement;
                if (docRoot != null)
                {
                    // UXML may already define diegetic-root; else build under doc root.
                    bool boundExisting = _view.BindExisting(docRoot);
                    if (!boundExisting)
                    {
                        // Never wipe an authored diegetic-root — Build() cannot recreate
                        // Phase 11 / expansion Template instances.
                        if (docRoot.Q(DiegeticHudView.RootName) != null)
                        {
                            ApplyStylesheet(docRoot);
                            _built = true;
                            return;
                        }
                        docRoot.Clear();
                        _view.Build(docRoot);
                    }
                    ApplyStylesheet(docRoot);
                    _built = true;
                    return;
                }
            }

            // Detached host (tests without a live panel, or document not ready yet).
            if (_view.Root == null)
                _view.Build();
            if (_view.Root != null)
                ApplyStylesheet(_view.Root);
            _built = true;
        }

        /// <summary>Unit-test entry: force detached VisualElement tree (no UIDocument).</summary>
        public void BuildDetachedForTests()
        {
            _preferDetached = true;
            _document = null;
            _built = false;
            _view = new DiegeticHudView();
            _view.Build();
            _built = true;
        }

        public void BindSources(
            HatchDefenseHUD hatch,
            InventoryStripUI strip,
            ExpeditionEncounterLogHUD encounterLog,
            WorkbenchUI workbench = null,
            EndgameSummaryUI endgame = null,
            PowerGridHUD powerGrid = null,
            ScavengeDispatchHUD scavengeDispatch = null,
            OverflowCrateHUD overflowCrate = null,
            FieldGearLoadoutHUD fieldGearLoadout = null,
            BunkerRationingHUD bunkerRationing = null,
            WaterPurificationHUD waterPurification = null,
            AirHeatManagementHUD airHeatManagement = null,
            BunkerMaintenanceHUD bunkerMaintenance = null,
            SurvivorTaskBoardHUD survivorTaskBoard = null,
            FactionPressureHUD factionPressure = null)
        {
            // Skip the full UnbindSources + rebind + Paint() when every source
            // is identical. The host only re-calls this when _diegeticHud is
            // null, but a future re-bind path that re-passes the same widgets
            // would otherwise run a full repaint each time.
            if (ReferenceEquals(_hatch, hatch)
                && ReferenceEquals(_strip, strip)
                && ReferenceEquals(_encounterLog, encounterLog)
                && ReferenceEquals(_workbench, workbench)
                && ReferenceEquals(_endgame, endgame)
                && ReferenceEquals(_powerGrid, powerGrid)
                && ReferenceEquals(_scavengeDispatch, scavengeDispatch)
                && ReferenceEquals(_overflowCrate, overflowCrate)
                && ReferenceEquals(_fieldGearLoadout, fieldGearLoadout)
                && ReferenceEquals(_bunkerRationing, bunkerRationing)
                && ReferenceEquals(_waterPurification, waterPurification)
                && ReferenceEquals(_airHeatManagement, airHeatManagement)
                && ReferenceEquals(_bunkerMaintenance, bunkerMaintenance)
                && ReferenceEquals(_survivorTaskBoard, survivorTaskBoard)
                && ReferenceEquals(_factionPressure, factionPressure))
            {
                return;
            }
            UnbindSources();
            _hatch = hatch;
            _strip = strip;
            _encounterLog = encounterLog;
            _workbench = workbench;
            // No subscription: EndgameSummaryUI exposes no change event, so the
            // host polls it (HUD.RepaintEndgameIfChanged). It is still held here
            // so the full Paint() sweep can draw it alongside the others.
            _endgame = endgame;
            // Also eventless: PowerGridHUD subscribes to
            // PowerNetwork.OnPowerStateChanged but publishes nothing itself,
            // so HUD.RepaintPowerGridIfChanged polls it.
            _powerGrid = powerGrid;
            _scavengeDispatch = scavengeDispatch;
            _overflowCrate = overflowCrate;
            _fieldGearLoadout = fieldGearLoadout;
            _bunkerRationing = bunkerRationing;
            _waterPurification = waterPurification;
            _airHeatManagement = airHeatManagement;
            _bunkerMaintenance = bunkerMaintenance;
            _survivorTaskBoard = survivorTaskBoard;
            _factionPressure = factionPressure;

            if (_strip != null)
                _strip.OnSelectionChanged += OnStripSelectionChanged;
            if (_encounterLog != null)
                _encounterLog.OnChanged += Paint;
            if (_hatch != null)
            {
                _hatch.OnOpenStateChanged += OnHatchOpenChanged;
                _hatch.OnRefreshed += Paint;
            }
            if (_workbench != null)
                _workbench.OnWorkbenchUiChanged += Paint;
            if (_scavengeDispatch != null)
                _scavengeDispatch.OnScavengeDispatchChanged += Paint;
            if (_overflowCrate != null)
                _overflowCrate.OnOverflowCrateChanged += Paint;
            if (_fieldGearLoadout != null)
                _fieldGearLoadout.OnFieldGearLoadoutChanged += Paint;
            if (_bunkerRationing != null)
                _bunkerRationing.OnBunkerRationingChanged += Paint;
            if (_waterPurification != null)
                _waterPurification.OnWaterPurificationChanged += Paint;
            if (_airHeatManagement != null)
                _airHeatManagement.OnAirHeatManagementChanged += Paint;
            if (_bunkerMaintenance != null)
                _bunkerMaintenance.OnBunkerMaintenanceChanged += Paint;
            if (_survivorTaskBoard != null)
                _survivorTaskBoard.OnSurvivorTaskBoardChanged += Paint;
            if (_factionPressure != null)
                _factionPressure.OnFactionPressureChanged += PaintFactionPressure;

            if (!_preferDetached)
                EnsureDocumentMounted();
            EnsureBuilt();
            Paint();
        }

        public void UnbindSources()
        {
            if (_strip != null)
                _strip.OnSelectionChanged -= OnStripSelectionChanged;
            if (_encounterLog != null)
                _encounterLog.OnChanged -= Paint;
            if (_hatch != null)
            {
                _hatch.OnOpenStateChanged -= OnHatchOpenChanged;
                _hatch.OnRefreshed -= Paint;
            }
            if (_workbench != null)
                _workbench.OnWorkbenchUiChanged -= Paint;
            if (_scavengeDispatch != null)
                _scavengeDispatch.OnScavengeDispatchChanged -= Paint;
            if (_overflowCrate != null)
                _overflowCrate.OnOverflowCrateChanged -= Paint;
            if (_fieldGearLoadout != null)
                _fieldGearLoadout.OnFieldGearLoadoutChanged -= Paint;
            if (_bunkerRationing != null)
                _bunkerRationing.OnBunkerRationingChanged -= Paint;
            if (_waterPurification != null)
                _waterPurification.OnWaterPurificationChanged -= Paint;
            if (_airHeatManagement != null)
                _airHeatManagement.OnAirHeatManagementChanged -= Paint;
            if (_bunkerMaintenance != null)
                _bunkerMaintenance.OnBunkerMaintenanceChanged -= Paint;
            if (_survivorTaskBoard != null)
                _survivorTaskBoard.OnSurvivorTaskBoardChanged -= Paint;
            if (_factionPressure != null)
                _factionPressure.OnFactionPressureChanged -= PaintFactionPressure;
            _hatch = null;
            _strip = null;
            _encounterLog = null;
            _workbench = null;
            _endgame = null;
            _powerGrid = null;
            _scavengeDispatch = null;
            _overflowCrate = null;
            _fieldGearLoadout = null;
            _bunkerRationing = null;
            _waterPurification = null;
            _airHeatManagement = null;
            _bunkerMaintenance = null;
            _survivorTaskBoard = null;
            _factionPressure = null;
        }

        private void OnHatchOpenChanged(bool _) => Paint();

        private void OnStripSelectionChanged()
        {
            // Keyboard focus path: any selection shows tooltip panel.
            if (_strip != null && _strip.SelectedIndex >= 0)
                _tooltipPinned = true;
            Paint();
        }

        /// <summary>
        /// Repaint the vitals readout. Separate from <see cref="Paint"/> on
        /// purpose: Paint() fires on discrete actions (missions, UI commands),
        /// while needs and dose change continuously, and a vitals panel repainted
        /// only on those events would sit frozen while the player starved.
        /// </summary>
        public void PaintVitals(
            int day, float hour, float cumulativeDose, float currentRate,
            IReadOnlyDictionary<string, NeedBarData> needs)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintVitals(day, hour, cumulativeDose, currentRate, needs);
        }

        /// <summary>Forward an event-prompt paint.</summary>
        public void PaintEventModal(
            bool open, string title, string body, IReadOnlyList<EventChoiceLine> choices)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintEventModal(open, title, body, choices);
        }

        /// <summary>Forward a workbench-readout paint.</summary>
        public void PaintWorkbench(bool open, string panelSummary)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintWorkbench(open, panelSummary);
        }

        /// <summary>Forward a terminal endgame-readout paint.</summary>
        public void PaintEndgame(bool visible, string statusLine, string detailSummary)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintEndgame(visible, statusLine, detailSummary);
        }

        /// <summary>Forward a power-budget readout paint.</summary>
        public void PaintPowerGrid(bool open, string budget, string sources, string loads)
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;
            _view.PaintPowerGrid(open, budget, sources, loads);
        }

        /// <summary>Repaint all diegetic panels from bound view-models.</summary>
        public void Paint()
        {
            EnsureBuilt();
            if (_view == null || _view.Root == null) return;

            bool hatchOpen = _hatch != null && _hatch.IsOpen;
            _view.PaintHatch(
                hatchOpen,
                _hatch?.StatusLine,
                _hatch?.AmmoStockpileLine,
                _hatch?.ArmsPreviewLine);

            _view.PaintEncounter(
                _encounterLog?.StatusLine,
                _encounterLog?.Lines);

            _view.PaintWorkbench(
                _workbench != null && _workbench.IsOpen,
                _workbench?.PanelSummary);

            _view.PaintEndgame(
                _endgame != null && _endgame.IsVisible,
                _endgame?.StatusLine,
                _endgame?.DetailSummary);

            _view.PaintPowerGrid(
                _powerGrid != null && _powerGrid.IsOpen,
                _powerGrid?.BudgetSummary,
                _powerGrid?.SourcesSummary,
                _powerGrid?.ConsumersSummary);

            _view.PaintScavengeDispatch(
                _scavengeDispatch != null && _scavengeDispatch.IsOpen,
                _scavengeDispatch?.PanelSummary);

            _view.PaintOverflowCrate(
                _overflowCrate != null && _overflowCrate.IsOpen,
                _overflowCrate?.PanelSummary);

            _view.PaintFieldGearLoadout(
                _fieldGearLoadout != null && _fieldGearLoadout.IsOpen,
                _fieldGearLoadout?.PanelSummary);

            _view.PaintBunkerRationing(
                _bunkerRationing != null && _bunkerRationing.IsOpen,
                _bunkerRationing?.PanelSummary);

            _view.PaintWaterPurification(
                _waterPurification != null && _waterPurification.IsOpen,
                _waterPurification?.PanelSummary);

            _view.PaintAirHeatManagement(
                _airHeatManagement != null && _airHeatManagement.IsOpen,
                _airHeatManagement?.PanelSummary);

            _view.PaintBunkerMaintenance(
                _bunkerMaintenance != null && _bunkerMaintenance.IsOpen,
                _bunkerMaintenance?.PanelSummary);

            _view.PaintSurvivorTaskBoard(
                _survivorTaskBoard != null && _survivorTaskBoard.IsOpen,
                _survivorTaskBoard?.PanelSummary);

            // Faction-pressure panel — driven by its own widget; we still
            // push a paint from Paint() so the initial render shows
            // compliant-state text even before the first OnFactionPressureChanged.
            if (_factionPressure != null)
            {
                _factionPressure.Refresh();
                _view.PaintFactionPressure(
                    _factionPressure.IsOpen,
                    FactionPressureHUD.FormatBody(_factionPressure.Capture()));
            }
            else
            {
                _view.PaintFactionPressure(false, string.Empty);
            }

            bool showStores = false;
            string summary = string.Empty;
            string tip = string.Empty;
            bool mil = false;
            if (_strip != null && (_strip.SelectedIndex >= 0 || _tooltipPinned))
            {
                var icon = _strip.SelectedIcon;
                if (icon != null)
                {
                    showStores = true;
                    summary = _strip.StripSummary ?? string.Empty;
                    tip = icon.Tooltip ?? string.Empty;
                    mil = icon.IsMilitaryExclusive;
                }
                else if (_tooltipPinned && !string.IsNullOrEmpty(_strip.SelectedTooltip))
                {
                    showStores = true;
                    summary = _strip.StripSummary ?? string.Empty;
                    tip = _strip.SelectedTooltip;
                }
            }
            _view.PaintStoresFocus(showStores, summary, tip, mil);
        }

        /// <summary>Clear stores focus pin (e.g. Esc after strip selection cleared).</summary>
        public void ClearStoresFocus()
        {
            _tooltipPinned = false;
            Paint();
        }

        /// <summary>
        /// Paint the faction-pressure terminal. Refreshes the snapshot from
        /// the bound widget, formats a 4-line body (GARRISON / MILITIA / CULT
        /// / WARLORD) and pushes it to the view.
        /// </summary>
        public void PaintFactionPressure()
        {
            if (_factionPressure == null) return;
            _factionPressure.Refresh();
            bool open = _factionPressure.IsOpen;
            string body = FactionPressureHUD.FormatBody(_factionPressure.Capture());
            _view.PaintFactionPressure(open, body);
        }

        private void ApplyStylesheet(VisualElement root)
        {
            if (root == null || _uss == null) return;
            if (!root.styleSheets.Contains(_uss))
                root.styleSheets.Add(_uss);
        }

        private void TryLoadAssets()
        {
#if UNITY_EDITOR
            if (_panelSettings == null)
            {
                _panelSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsAssetPath);
                if (_panelSettings == null)
                    _panelSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<PanelSettings>(SharedPanelSettingsPath);
            }
            if (_uxml == null)
                _uxml = UnityEditor.AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlResourcePath);
            if (_uss == null)
                _uss = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>(UssResourcePath);
#endif
            // Player builds / play mode without inspector wiring.
            if (_panelSettings == null)
                _panelSettings = Resources.Load<PanelSettings>(ResourcesPanelSettings);
            if (_uxml == null)
                _uxml = Resources.Load<VisualTreeAsset>(ResourcesUxml);
            if (_uss == null)
                _uss = Resources.Load<StyleSheet>(ResourcesUss);
        }

        /// <summary>
        /// Last-resort PanelSettings so UIDocument can still render when no
        /// asset is assigned (theme may warn; our USS still applies).
        /// </summary>
        private PanelSettings CreateFallbackPanelSettings()
        {
            if (_runtimePanelSettings != null) return _runtimePanelSettings;

            var ps = ScriptableObject.CreateInstance<PanelSettings>();
            ps.name = "DiegeticHudPanelSettings_Runtime";
            ps.scaleMode = PanelScaleMode.ScaleWithScreenSize;
            ps.referenceResolution = new Vector2Int(1920, 1080);
            ps.screenMatchMode = PanelScreenMatchMode.MatchWidthOrHeight;
            ps.match = 1f;
            ps.sortingOrder = DefaultSortingOrder;
            ps.clearColor = false;
            ps.colorClearValue = Color.black;
            _runtimePanelSettings = ps;
            return ps;
        }
    }
}
