// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.World;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 139 Phase 3 — InSAR deformation-intelligence map. Presentation only:
    /// shows survey coverage, coherence, trend, and excavation/travel warnings
    /// with honest uncertainty language. Never mutates terrain and never claims
    /// deterministic earthquake prediction.
    /// </summary>
    public partial class InSarMappingPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Label _sectorText = null!;
        private Button _recordBtn = null!;
        private Button _processBtn = null!;
        private OptionButton _sensorSelector = null!;
        private OptionButton _sectorSelector = null!;
        private string _selectedSensorId = string.Empty;
        private string _selectedSectorId = string.Empty;

        private InSarMappingHostSession? _host;

        public bool IsBound => _host != null;

        /// <summary>Wired by Main; panel does not reach into campaign state itself.</summary>
        public Func<int>? AppDayProvider { get; set; }

        /// <summary>Wired by Main; known survey sectors (stable order).</summary>
        public Func<IReadOnlyList<string>>? SectorProvider { get; set; }

        public void Bind(InSarMappingHostSession session)
        {
            _host = session;
            if (_host != null)
                _host.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Deformation Intelligence // InSAR Mapping", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("passes", "Survey Passes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("sectors", "Sectors Mapped", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("worst", "Worst Trend", "—", AshfallMetricCard.Criticality.Normal, minWidth: 160);
            _statusRail.AddCard("weather", "Weather Quality", "—", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;
            _contentStack.AddChild(_detailText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart });

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("SURVEY PLATFORM"));

            _sensorSelector = new OptionButton { CustomMinimumSize = new Vector2(420, 36) };
            _sensorSelector.ItemSelected += idx =>
            {
                if (_host == null) return;
                var sensors = SortedSensors();
                if ((int)idx >= 0 && (int)idx < sensors.Count)
                    _selectedSensorId = sensors[(int)idx].id;
            };
            _contentStack.AddChild(_sensorSelector);
            _contentStack.AddChild(AshfallUiHelpers.MakeBody(
                "Repeat passes must share a platform line. Two compatible passes over the same sector are required before a deformation map exists."));

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());
            _contentStack.AddChild(AshfallUiHelpers.MakeSectionHeader("REPEAT-PASS SURVEY"));

            _sectorSelector = new OptionButton { CustomMinimumSize = new Vector2(420, 36) };
            _sectorSelector.ItemSelected += idx =>
            {
                var sectors = SectorProvider?.Invoke() ?? Array.Empty<string>();
                if ((int)idx >= 0 && (int)idx < sectors.Count)
                    _selectedSectorId = sectors[(int)idx];
            };
            _contentStack.AddChild(_sectorSelector);

            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _recordBtn = new Button { Text = "Record Survey Pass", CustomMinimumSize = new Vector2(210, 36) };
            _recordBtn.Pressed += () =>
            {
                if (_host == null || string.IsNullOrEmpty(_selectedSectorId)) return;
                _host.RecordSurveyPass(_selectedSensorId, _selectedSectorId, SystemDay());
            };
            row.AddChild(_recordBtn);

            _processBtn = new Button { Text = "Process Deformation Map", CustomMinimumSize = new Vector2(230, 36) };
            _processBtn.Pressed += () =>
            {
                if (_host == null || string.IsNullOrEmpty(_selectedSectorId)) return;
                _host.ProcessSector(_selectedSectorId, 0.5);
            };
            row.AddChild(_processBtn);
            _contentStack.AddChild(row);

            var note = AshfallUiHelpers.MakeBody(
                "A low-coherence read is inconclusive — it is not a statement that the ground is safe. InSAR flags trends for excavation and route planning; it never collapses a tunnel or predicts a specific quake.");
            note.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            note.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(note);

            _sectorText = new Label { AutowrapMode = TextServer.AutowrapMode.WordSmart };
            _contentStack.AddChild(_sectorText);

            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        private int SystemDay() => AppDayProvider?.Invoke() ?? 1;

        private List<InSarSensorDef> SortedSensors()
        {
            var sensors = new List<InSarSensorDef>();
            if (_host != null)
            {
                foreach (var def in _host.System.Catalog.All) sensors.Add(def);
                sensors.Sort((a, b) => string.CompareOrdinal(a.id, b.id));
            }
            return sensors;
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var state = _host.System.State;
            _statusRail.Set("passes", state.Passes.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("sectors", state.Summaries.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            string worst = "—";
            var worstCriticality = AshfallMetricCard.Criticality.Normal;
            foreach (var s in state.Summaries)
            {
                if (s.Classification == InSarClassification.AcceleratingSubsidence ||
                    s.Classification == InSarClassification.AbruptDeformation)
                {
                    worst = s.Classification == InSarClassification.AbruptDeformation ? "ABRUPT" : "ACCELERATING";
                    worstCriticality = AshfallMetricCard.Criticality.Critical;
                    break;
                }
                if (s.Classification == InSarClassification.SlowSubsidence)
                {
                    worst = "SUBSIDING";
                    worstCriticality = AshfallMetricCard.Criticality.Caution;
                }
                else if (s.Classification == InSarClassification.LowConfidence && worst == "—")
                {
                    worst = "LOW CONFIDENCE";
                    worstCriticality = AshfallMetricCard.Criticality.Caution;
                }
            }
            _statusRail.Set("worst", worst, worstCriticality);

            int weather = _host.WeatherQualityBp();
            _statusRail.Set("weather", $"{weather}%",
                weather < 40 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text =
                    $"Platforms available: {SortedSensors().Count} | Known sectors: {(SectorProvider?.Invoke() ?? Array.Empty<string>()).Count} | Weather quality: {weather}%\n" +
                    $"Last event: {_host.LastEvent}";
            }

            SyncSelectors();

            if (_sectorText != null)
            {
                if (state.Summaries.Count == 0)
                {
                    _sectorText.Text = "No deformation maps processed yet.";
                }
                else
                {
                    var lines = new List<string>();
                    foreach (var s in state.Summaries)
                    {
                        lines.Add(
                            $"{s.SectorId}: {s.Classification} | LOS {s.RelativeDisplacementMm:F1} mm " +
                            $"({s.TrendVelocityMmPerDay:F2} mm/day) | coherence {s.Coherence:F2} | confidence {s.Confidence:P0} | " +
                            $"route {_host.System.GetTravelRisk(s.SectorId)} | {_host.System.GetExcavationWarning(s.SectorId)}");
                    }
                    _sectorText.Text = string.Join("\n", lines);
                }
            }

            if (_recordBtn != null)
                _recordBtn.Disabled = string.IsNullOrEmpty(_selectedSensorId) || string.IsNullOrEmpty(_selectedSectorId);
            if (_processBtn != null)
                _processBtn.Disabled = string.IsNullOrEmpty(_selectedSectorId);
        }

        private void SyncSelectors()
        {
            var sensors = SortedSensors();
            if (_sensorSelector != null)
            {
                _sensorSelector.Clear();
                int selected = 0;
                for (int i = 0; i < sensors.Count; i++)
                {
                    var def = sensors[i];
                    _sensorSelector.AddItem($"{def.display_name} [{def.id}]", i);
                    if (def.id == _selectedSensorId || (string.IsNullOrEmpty(_selectedSensorId) && i == 0))
                    {
                        selected = i;
                        _selectedSensorId = def.id;
                    }
                }
                _sensorSelector.Selected = selected;
            }

            var sectors = SectorProvider?.Invoke() ?? Array.Empty<string>();
            if (_sectorSelector != null)
            {
                _sectorSelector.Clear();
                int selected = 0;
                for (int i = 0; i < sectors.Count; i++)
                {
                    _sectorSelector.AddItem(sectors[i], i);
                    if (sectors[i] == _selectedSectorId || (string.IsNullOrEmpty(_selectedSectorId) && i == 0))
                    {
                        selected = i;
                        _selectedSectorId = sectors[i];
                    }
                }
                _sectorSelector.Selected = selected;
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
