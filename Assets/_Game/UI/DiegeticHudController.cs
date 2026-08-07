using System;
using UnityEngine;
using UnityEngine.UIElements;

namespace AtomicWar._Game.UI
{
    /// <summary>
    /// Mounts <see cref="DiegeticHudView"/> into a UIDocument (or an in-memory
    /// host for tests) and keeps hatch ammo / arms, encounter log, and stores
    /// focus tooltip in sync with the string view-models.
    /// </summary>
    public class DiegeticHudController : MonoBehaviour
    {
        public const string UxmlResourcePath = "Assets/_Game/UI/DiegeticHud.uxml";
        public const string UssResourcePath = "Assets/_Game/UI/DiegeticHud.uss";
        public const string PanelSettingsPath = "Assets/_Game/UI/MainMenu/MainMenuPanelSettings.asset";

        [SerializeField] private UIDocument _document;
        [SerializeField] private PanelSettings _panelSettings;
        [SerializeField] private VisualTreeAsset _uxml;
        [SerializeField] private StyleSheet _uss;

        private DiegeticHudView _view = new DiegeticHudView();
        private HatchDefenseHUD _hatch;
        private InventoryStripUI _strip;
        private ExpeditionEncounterLogHUD _encounterLog;
        private bool _built;
        private bool _tooltipPinned;

        /// <summary>Test / host access to the painted tree.</summary>
        public DiegeticHudView View => _view;
        public VisualElement Root => _view?.Root;
        public bool IsBuilt => _built;

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
            if (_document == null)
                _document = GetComponent<UIDocument>();
            EnsureBuilt();
        }

        private void OnEnable()
        {
            EnsureBuilt();
            Paint();
        }

        private void OnDestroy()
        {
            UnbindSources();
        }

        /// <summary>
        /// Build tree into UIDocument when available; otherwise into a detached
        /// VisualElement host (EditMode unit tests).
        /// </summary>
        public void EnsureBuilt()
        {
            if (_built && _view.Root != null) return;

            TryLoadAssets();

            if (_document != null)
            {
                if (_panelSettings != null)
                    _document.panelSettings = _panelSettings;
                if (_uxml != null)
                    _document.visualTreeAsset = _uxml;

                var docRoot = _document.rootVisualElement;
                if (docRoot != null)
                {
                    // UXML may already define diegetic-root; else build under doc root.
                    if (!_view.BindExisting(docRoot))
                    {
                        docRoot.Clear();
                        _view.Build(docRoot);
                    }
                    if (_uss != null && !docRoot.styleSheets.Contains(_uss))
                        docRoot.styleSheets.Add(_uss);
                    _built = true;
                    return;
                }
            }

            // Detached host (tests without a live panel).
            _view.Build();
            _built = true;
        }

        /// <summary>Unit-test entry: force detached VisualElement tree.</summary>
        public void BuildDetachedForTests()
        {
            _document = null;
            _built = false;
            _view = new DiegeticHudView();
            _view.Build();
            _built = true;
        }

        public void BindSources(
            HatchDefenseHUD hatch,
            InventoryStripUI strip,
            ExpeditionEncounterLogHUD encounterLog)
        {
            UnbindSources();
            _hatch = hatch;
            _strip = strip;
            _encounterLog = encounterLog;

            if (_strip != null)
                _strip.OnSelectionChanged += OnStripSelectionChanged;
            if (_encounterLog != null)
                _encounterLog.OnChanged += Paint;
            if (_hatch != null)
            {
                _hatch.OnOpenStateChanged += OnHatchOpenChanged;
                _hatch.OnRefreshed += Paint;
            }

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
            _hatch = null;
            _strip = null;
            _encounterLog = null;
        }

        private void OnHatchOpenChanged(bool _) => Paint();

        private void OnStripSelectionChanged()
        {
            // Keyboard focus path: any selection shows tooltip panel.
            if (_strip != null && _strip.SelectedIndex >= 0)
                _tooltipPinned = true;
            Paint();
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

        private void TryLoadAssets()
        {
#if UNITY_EDITOR
            if (_panelSettings == null)
                _panelSettings = UnityEditor.AssetDatabase.LoadAssetAtPath<PanelSettings>(PanelSettingsPath);
            if (_uxml == null)
                _uxml = UnityEditor.AssetDatabase.LoadAssetAtPath<VisualTreeAsset>(UxmlResourcePath);
            if (_uss == null)
                _uss = UnityEditor.AssetDatabase.LoadAssetAtPath<StyleSheet>(UssResourcePath);
#endif
        }
    }
}
