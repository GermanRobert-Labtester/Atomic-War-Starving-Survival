using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class SlurryDewateringSumpPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label? _headerTitleLabel;
        private Label? _statusBadgeLabel;
        private Button? _closeButton;
        private VBoxContainer? _telemetryContainer;
        private VBoxContainer? _buttonContainer;
        private VBoxContainer? _dataContainer;
        private Label? _logOutputLabel;

        public bool IsBound { get; private set; } = true;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildInterface();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Bind(object? session)
        {
            IsBound = true;
            RefreshView();
        }

        public void Unbind()
        {
            IsBound = false;
        }

        public void RefreshView()
        {
            if (_statusBadgeLabel != null)
            {
                _statusBadgeLabel.Text = "STATUS: SUMP PUMPS ACTIVE - EXTRACTION: 1,850 L/MIN / SUMP LEVEL: 34%";
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

            // Top Header Bar
            var headerHBox = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _headerTitleLabel = new Label
            {
                Text = "SUBTERRANEAN HYDRAULICS // SLURRY DEWATERING SUMP [HYDRO-03]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: SUMP PUMPS ACTIVE - EXTRACTION: 1,850 L/MIN / SUMP LEVEL: 34%"
            };
            _statusBadgeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_statusBadgeLabel);

            _closeButton = new Button { Text = "[X] CLOSE CONSOLE" };
            _closeButton.Pressed += () =>
            {
                Visible = false;
                OnClose?.Invoke();
            };
            headerHBox.AddChild(_closeButton);
            mainVBox.AddChild(headerHBox);

            // Three-Column High-Density Grid
            var bodyHBox = new HBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            bodyHBox.AddThemeConstantOverride("separation", 16);
            mainVBox.AddChild(bodyHBox);

            // Left Column (Telemetry)
            var leftPanel = CreatePanelFrame("SUMP INFLOW & BASAL HYDROSTATICS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftPanel.GetNode<MarginContainer>("Margin").AddChild(_telemetryContainer);
            _telemetryContainer.AddChild(CreateTelemetryRow("SUMP FLOOD LEVEL", "34.2% [CONTROLLED RETENTION]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("NATURAL SEEPAGE INFLOW", "1,420 L/MIN FROM CRACK 04", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(CreateTelemetryRow("SLURRY SEDIMENT DENSITY", "1.18 KG/L [GRIT-HEAVY]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(CreateTelemetryRow("PUMP 01 & 02 COMBINED FLOW", "1,850 L/MIN DISCHARGE", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("IMPELLER CAVITATION RISK", "6.2% [NOMINAL BACKPRESSURE]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = CreatePanelFrame("CENTRIFUGAL PUMPS & SETTLING WEIRS");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _buttonContainer.AddThemeConstantOverride("separation", 12);
            centerPanel.GetNode<MarginContainer>("Margin").AddChild(_buttonContainer);
            _buttonContainer.AddChild(new Button { Text = "[RAMP UP CENTRIFUGAL PUMP SPEED]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[BACKWASH COARSE GRIT BASKET]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[OPEN WEIR DIVERSION GATE 03]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DIVERT SUPERNATANT WATER TO TREATMENT]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = CreatePanelFrame("WEIR SETTLING & SLUDGE CAKE");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dataContainer.AddThemeConstantOverride("separation", 8);
            rightPanel.GetNode<MarginContainer>("Margin").AddChild(_dataContainer);
            _dataContainer.AddChild(CreateTelemetryRow("SETTLED MINERAL SILT", "1.4 TONS IN SUMP HOPPER", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(CreateTelemetryRow("PURIFIED EFFLUENT DISCHARGE", "SURFACE RIVER CANAL", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("SUMP HIGH-WATER ALARM", "SET AT 80% THRESHOLD", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(CreateTelemetryRow("RECOVERED INDUSTRIAL WATER", "840 L/MIN SENT TO DIGESTER", AshfallUiHelpers.ToColor(DesignTheme.Warm)));

            // Bottom Diagnostics Log
            var logPanel = new PanelContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 100) };
            var logMargin = new MarginContainer();
            logMargin.AddThemeConstantOverride("margin_left", 12);
            logMargin.AddThemeConstantOverride("margin_top", 8);
            logMargin.AddThemeConstantOverride("margin_right", 12);
            logMargin.AddThemeConstantOverride("margin_bottom", 8);
            logPanel.AddChild(logMargin);

            _logOutputLabel = new Label
            {
                Text = "[HYDRO-03] Twin centrifugal slurry pumps engaged at 1,850 L/min.\n[HYDRO-03] Sump retention level decreased to 34.2%.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _logOutputLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            logMargin.AddChild(_logOutputLabel);
            mainVBox.AddChild(logPanel);
        }

        private static PanelContainer CreatePanelFrame(string headerText)
        {
            var panel = new PanelContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill, SizeFlagsVertical = SizeFlags.ExpandFill };
            var vbox = new VBoxContainer();
            panel.AddChild(vbox);

            var title = new Label
            {
                Text = headerText
            };
            title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(title);

            var margin = new MarginContainer { Name = "Margin", SizeFlagsVertical = SizeFlags.ExpandFill };
            margin.AddThemeConstantOverride("margin_left", 8);
            margin.AddThemeConstantOverride("margin_top", 8);
            margin.AddThemeConstantOverride("margin_right", 8);
            margin.AddThemeConstantOverride("margin_bottom", 8);
            vbox.AddChild(margin);

            return panel;
        }

        private static HBoxContainer CreateTelemetryRow(string label, string value, Color valueColor)
        {
            var hbox = new HBoxContainer { SizeFlagsHorizontal = SizeFlags.ExpandFill };
            var lbl = new Label { Text = label, SizeFlagsHorizontal = SizeFlags.ExpandFill };
            lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            var val = new Label { Text = value };
            val.AddThemeColorOverride("font_color", valueColor);
            hbox.AddChild(lbl);
            hbox.AddChild(val);
            return hbox;
        }
    }
}
