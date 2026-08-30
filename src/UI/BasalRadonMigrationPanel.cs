using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class BasalRadonMigrationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private Label? _headerTitleLabel;
        private Label? _statusBadgeLabel;
        private Button? _closeButton;
        private VBoxContainer? _telemetryContainer;
        private VBoxContainer? _buttonContainer;
        private VBoxContainer? _dataContainer;
        private Label? _logOutputLabel;

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

        public bool IsBound { get; private set; } = true;

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
                _statusBadgeLabel.Text = "STATUS: RADON INVERSION ACTIVE - SUMP: 3,420 Bq/m3 (CRITICAL)";
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
                Text = "SHELTER RADIOLOGY // BASAL RADON INVERSION & STRATA MIGRATION [RAD-04]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: RADON INVERSION ACTIVE - SUMP: 3,420 Bq/m3 (CRITICAL)"
            };
            _statusBadgeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Critical));
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
            var leftPanel = CreatePanelFrame("GEOLOGICAL RADON DIFFUSION & SCINTILLATION");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftPanel.GetNode<MarginContainer>("Margin").AddChild(_telemetryContainer);
            _telemetryContainer.AddChild(CreateTelemetryRow("SUMP SCINTILLATION CHAMBER", "3,420 Bq/m3 [CRITICAL]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(CreateTelemetryRow("SUB-LEVEL 3 AIR DUCTWAYS", "1,240 Bq/m3 [HIGH]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(CreateTelemetryRow("RESIDENTIAL BARRACKS", "380 Bq/m3 [MODERATE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("222Rn ISOTOPE HALF-LIFE", "3.82 DAYS (DECAY ACTIVE)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(CreateTelemetryRow("BASAL BAROMETRIC INVERSION", "-18.4 mbar [NEGATIVE]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = CreatePanelFrame("VENTILATION CURTAINS & SUMP EXTRACTION");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _buttonContainer.AddThemeConstantOverride("separation", 12);
            centerPanel.GetNode<MarginContainer>("Margin").AddChild(_buttonContainer);
            _buttonContainer.AddChild(new Button { Text = "[MAXIMIZE SUMP EXHAUST BLOWERS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[PULSE POSITIVE PRESSURE AIR CURTAIN]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[BACKWASH ACTIVATED CHARCOAL FILTER BED]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ENGAGE EMERGENCY SECTOR 08 SEAL]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = CreatePanelFrame("BIOLOGICAL LUNG DOSE & FILTER INVENTORY");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dataContainer.AddThemeConstantOverride("separation", 8);
            rightPanel.GetNode<MarginContainer>("Margin").AddChild(_dataContainer);
            _dataContainer.AddChild(CreateTelemetryRow("ALPHA DECAY LUNG BURDEN", "142 mSv/SURVIVOR [HIGH]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(CreateTelemetryRow("POLONIUM-218 SATURATION", "84.2% IN EXHAUST PLUME", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _dataContainer.AddChild(CreateTelemetryRow("INTACT RESPIRATORY MASKS", "14 / 18 SURVIVORS", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("REPLACEMENT CHARCOAL CANISTERS", "28 REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

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
                Text = "[RAD-04] Tectonic fissure degassing detected beneath sub-level 4.\n[RAD-04] Exhaust blower velocity set to maximum negative draft.",
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
