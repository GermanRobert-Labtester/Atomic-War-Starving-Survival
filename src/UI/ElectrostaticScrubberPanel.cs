using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Electrostatic scrubber console (Plan 72). Presentation only: renders the
    /// VentilationSystem electrostatic stage state and issues commands through
    /// VentilationHostSession. Core owns all filtration math.
    /// </summary>
    public partial class ElectrostaticScrubberPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private VentilationHostSession? _host;
        private Label? _statusBadgeLabel;
        private Label? _logOutputLabel;
        private Label? _profileValue = null!;
        private Label? _captureValue = null!;
        private Label? _powerValue = null!;
        private Label? _dustValue = null!;
        private Label? _hopperValue = null!;
        private Label? _ozoneValue = null!;
        private Label? _conditionValue = null!;
        private Button? _rapButton;
        private Button? _emptyHopperButton;
        private Button? _serviceButton;

        public bool IsBound { get; private set; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Bind(object? session)
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            _host = session as VentilationHostSession;
            if (_host != null)
                _host.StateChanged += RefreshView;
            IsBound = _host != null;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
                _host.StateChanged -= RefreshView;
            _host = null;
            IsBound = false;
        }

        private static ElectrostaticStageState? Stage(VentilationHostSession? host)
            => host?.System.State.electrostatic;

        private static ElectrostaticStageDef? StageDef(VentilationHostSession? host, ElectrostaticStageState? stage)
            => stage == null ? null : host!.System.Catalog.FirstOrDefault(d => d.stage_id == stage.stageId);

        public void RefreshView()
        {
            if (_host == null || !IsInsideTree())
                return;

            var stage = Stage(_host);
            var state = _host.System.State;

            if (stage is not { installed: true })
            {
                if (_statusBadgeLabel != null) _statusBadgeLabel.Text = "STATUS: NO PRECIPITATOR STAGE INSTALLED";
                if (_profileValue != null) _profileValue.Text = "—";
                if (_captureValue != null) _captureValue.Text = "—";
                if (_powerValue != null) _powerValue.Text = "—";
                if (_dustValue != null) _dustValue.Text = "—";
                if (_hopperValue != null) _hopperValue.Text = "—";
                if (_ozoneValue != null) _ozoneValue.Text = $"{state.ozonePpm:F0} ppm";
                if (_conditionValue != null) _conditionValue.Text = "—";
                return;
            }

            var def = StageDef(_host, stage);
            var profile = def?.operating_profiles.FirstOrDefault(p => p.profile_id == stage.profileId);

            if (_statusBadgeLabel != null)
            {
                _statusBadgeLabel.Text = stage.faulted
                    ? $"STATUS: STAGE FAULTED — {stage.faultReason.Replace('_', ' ').ToUpperInvariant()}"
                    : stage.energized
                        ? "STATUS: FIELD ENERGIZED — SCRUBBING"
                        : "STATUS: OFFLINE — NO ROOM POWER";
            }

            if (_profileValue != null) _profileValue.Text = profile?.display_name ?? stage.profileId;
            if (_captureValue != null && profile != null)
                _captureValue.Text = $"{profile.capture_efficiency_pm25 * 100f:F0}% PM2.5 / {profile.hot_ash_capture_efficiency * 100f:F0}% hot ash";
            if (_powerValue != null && profile != null)
                _powerValue.Text = stage.energized ? $"{profile.nominal_power_w:F0} W draw" : "no room power";
            if (_dustValue != null && def != null)
                _dustValue.Text = $"{stage.dustLoadKg:F1} / {def.dust_capacity_kg:F0} kg on plates";
            if (_hopperValue != null)
                _hopperValue.Text = $"{stage.hopperKg:F1} kg (sealed for disposal)";
            if (_ozoneValue != null)
                _ozoneValue.Text = $"{state.ozonePpm:F0} ppm" + (state.ozonePpm > VentilationSystem.OzoneWarnPpm ? " — VENTILATE" : "");
            if (_conditionValue != null)
                _conditionValue.Text = $"plates {stage.plateCondition:F0}% | transformer {stage.transformerCondition:F0}%";

            bool canRap = stage.dustLoadKg > 0f && stage.rappingCooldownDays == 0;
            if (_rapButton != null)
            {
                _rapButton.Disabled = !canRap;
                _rapButton.TooltipText = canRap
                    ? "Knocks plate dust into the hopper — capture halved while dust settles"
                    : "No plate dust, or rapping already in progress";
            }
            if (_emptyHopperButton != null)
            {
                _emptyHopperButton.Disabled = stage.hopperKg < VentilationSystem.HotDustKgPerDrum;
                _emptyHopperButton.TooltipText = "Seals hopper dust into marked drums for disposal hauling (10 kg each)";
            }
            if (_serviceButton != null)
            {
                _serviceButton.Disabled = false;
                _serviceButton.TooltipText = "Maintenance: clears faults, restores plate and transformer condition";
            }
        }

        private void BuildInterface()
        {
            var bg = new ColorRect { Color = AshfallUiHelpers.ToColor(DesignTheme.Ink) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var rootMargin = new MarginContainer();
            rootMargin.SetAnchorsPreset(LayoutPreset.FullRect);
            rootMargin.AddThemeConstantOverride("margin_left", 24);
            rootMargin.AddThemeConstantOverride("margin_top", 24);
            rootMargin.AddThemeConstantOverride("margin_right", 24);
            rootMargin.AddThemeConstantOverride("margin_bottom", 24);
            AddChild(rootMargin);

            var mainVBox = new VBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            mainVBox.AddThemeConstantOverride("separation", 16);
            rootMargin.AddChild(mainVBox);

            var headerHBox = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            var headerTitleLabel = new Label
            {
                Text = "AIR HANDLING // ELECTROSTATIC PRECIPITATOR [FILTER STAGE]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(headerTitleLabel);

            _statusBadgeLabel = new Label { Text = "STATUS: OFFLINE" };
            _statusBadgeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_statusBadgeLabel);

            var closeButton = new Button { Text = "[X] CLOSE CONSOLE" };
            closeButton.Pressed += () =>
            {
                Visible = false;
                OnClose?.Invoke();
            };
            headerHBox.AddChild(closeButton);
            mainVBox.AddChild(headerHBox);

            var bodyHBox = new HBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            bodyHBox.AddThemeConstantOverride("separation", 16);
            mainVBox.AddChild(bodyHBox);

            // Left column — stage state
            var (leftPanel, leftMargin) = CreatePanelFrame("PRECIPITATOR STATE");
            bodyHBox.AddChild(leftPanel);
            var telemetry = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            telemetry.AddThemeConstantOverride("separation", 8);
            leftMargin.AddChild(telemetry);
            _profileValue = AddRow(telemetry, "OPERATING PROFILE");
            _captureValue = AddRow(telemetry, "CAPTURE EFFICIENCY");
            _powerValue = AddRow(telemetry, "ELECTRICAL DRAW");
            _conditionValue = AddRow(telemetry, "PLATES / TRANSFORMER");

            // Center column — commands
            var (centerPanel, centerMargin) = CreatePanelFrame("STAGE COMMANDS");
            bodyHBox.AddChild(centerPanel);
            var buttons = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            buttons.AddThemeConstantOverride("separation", 12);
            centerMargin.AddChild(buttons);

            _rapButton = new Button { Text = "[RAP PLATES — DUST TO HOPPER]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _rapButton.Pressed += OnRapPressed;
            buttons.AddChild(_rapButton);

            _emptyHopperButton = new Button { Text = "[SEAL HOPPER DUST INTO DRUMS]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _emptyHopperButton.Pressed += OnEmptyHopperPressed;
            buttons.AddChild(_emptyHopperButton);

            _serviceButton = new Button { Text = "[SERVICE STAGE — CLEAR FAULTS]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _serviceButton.Pressed += OnServicePressed;
            buttons.AddChild(_serviceButton);

            // Right column — waste & gas
            var (rightPanel, rightMargin) = CreatePanelFrame("DUST WASTE & GAS");
            bodyHBox.AddChild(rightPanel);
            var data = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            data.AddThemeConstantOverride("separation", 8);
            rightMargin.AddChild(data);
            _dustValue = AddRow(data, "PLATE DUST LOAD");
            _hopperValue = AddRow(data, "HOPPER (RADIOACTIVE)");
            _ozoneValue = AddRow(data, "OZONE CONCENTRATION");
            var wasteNote = new Label
            {
                Text = "Drums are sealed and mapped for deep burial. Do not open indoors.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            wasteNote.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            data.AddChild(wasteNote);

            // Bottom diagnostics log
            var logPanel = new PanelContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 100) };
            var logMargin = new MarginContainer();
            logMargin.AddThemeConstantOverride("margin_left", 12);
            logMargin.AddThemeConstantOverride("margin_top", 8);
            logMargin.AddThemeConstantOverride("margin_right", 12);
            logMargin.AddThemeConstantOverride("margin_bottom", 8);
            logPanel.AddChild(logMargin);

            _logOutputLabel = new Label
            {
                Text = "[FILTER] Precipitator console standing by.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _logOutputLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            logMargin.AddChild(_logOutputLabel);
            mainVBox.AddChild(logPanel);
        }

        private void OnRapPressed()
        {
            if (_host == null) return;
            var res = _host.RapPlates();
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[FILTER] Plates rapped. Dust settling in the hopper — capture reduced for now."
                    : $"[FILTER] Rapping refused ({res.MessageKey}).";
        }

        private void OnEmptyHopperPressed()
        {
            if (_host == null) return;
            var res = _host.EmptyHopper(4);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[FILTER] Hopper dust sealed into drums. Route them to deep burial."
                    : $"[FILTER] Hopper refused ({res.MessageKey}).";
        }

        private void OnServicePressed()
        {
            if (_host == null) return;
            var res = _host.ServiceStage();
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[FILTER] Stage serviced. Insulation checked, faults cleared."
                    : $"[FILTER] Service refused ({res.MessageKey}).";
        }

        private Label AddRow(VBoxContainer container, string label)
        {
            var hbox = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            var lbl = new Label { Text = label, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            var val = new Label { Text = "—" };
            val.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            hbox.AddChild(lbl);
            hbox.AddChild(val);
            container.AddChild(hbox);
            return val;
        }

        private static (PanelContainer panel, MarginContainer margin) CreatePanelFrame(string headerText)
        {
            var panel = new PanelContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var vbox = new VBoxContainer();
            panel.AddChild(vbox);

            var title = new Label { Text = headerText };
            title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(title);

            var margin = new MarginContainer { Name = "Margin", SizeFlagsVertical = SizeFlags.ExpandFill };
            margin.AddThemeConstantOverride("margin_left", 8);
            margin.AddThemeConstantOverride("margin_top", 8);
            margin.AddThemeConstantOverride("margin_right", 8);
            margin.AddThemeConstantOverride("margin_bottom", 8);
            vbox.AddChild(margin);

            return (panel, margin);
        }
    }
}
