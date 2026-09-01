using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class AquiferTreatyConcessionPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: COMPACT RATIFIED - PURIFIED FLOW: 420 L/HR / TRIBUTE: 4 DAYS";
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
                Text = "DIPLOMATIC HYDRAULICS // HYDRO-BARONS AQUIFER TREATIES [AQUI-01]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: COMPACT RATIFIED - PURIFIED FLOW: 420 L/HR / TRIBUTE: 4 DAYS"
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
            var leftPanel = CreatePanelFrame("BASAL AQUIFER STRATA & SIPHON SENSORS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftPanel.GetNode<MarginContainer>("Margin").AddChild(_telemetryContainer);
            _telemetryContainer.AddChild(CreateTelemetryRow("BASAL AQUIFER SIPHON DEPTH", "-620 METERS (STRATA 4)", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(CreateTelemetryRow("WATER SALINITY PURITY", "120 ppm (POTABLE GRADE)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("TRITIUM HEAVY ISOTOPE", "4.2 Bq/L [SAFE COMMERCE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("HYDRO-BARON DIPLOMATIC FAVOR", "+45 [CONCESSIONARY ALLIANCE]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("WAR TENSION INDEX", "12% [CONTROLLED]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = CreatePanelFrame("MANIFOLD SLUICE VALVES & BYPASS");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _buttonContainer.AddThemeConstantOverride("separation", 12);
            centerPanel.GetNode<MarginContainer>("Margin").AddChild(_buttonContainer);
            _buttonContainer.AddChild(new Button { Text = "[RATIFY EMERGENCY WATER-FOR-AMMO ACCORD]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[THROTTLE TRIBUTARY BYPASS TO 50%]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[FLUSH CONTAMINATED SEDIMENT TRAP]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DISPATCH NEGOTIATOR TO HYDRO-BARON CONCLAVE]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = CreatePanelFrame("TRIBUTE VAULT & ARREARS LEDGER");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dataContainer.AddThemeConstantOverride("separation", 8);
            rightPanel.GetNode<MarginContainer>("Margin").AddChild(_dataContainer);
            _dataContainer.AddChild(CreateTelemetryRow("7.62x39mm AMMO TRIBUTE", "450 ROUNDS CRATED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("PURIFIED LEAD INGOTS", "120 KG IN ESCROW", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(CreateTelemetryRow("SEALED ANTIBIOTICS VIALS", "18 UNITS STORED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("DEFAULT LOCK-OUT TIMER", "4 DAYS UNTIL SIPHON CLOSURE", AshfallUiHelpers.ToColor(DesignTheme.Hot)));

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
                Text = "[AQUI-01] Siphon flow rate locked at 420.5 L/Hour.\n[AQUI-01] Tribute consignment verified by Hydro-Baron courier.",
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
