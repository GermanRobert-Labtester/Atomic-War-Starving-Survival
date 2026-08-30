using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class InductionCupolaFurnacePanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: SMELTING ACTIVE - MOLTEN HIGH-TUNGSTEN ALLOY / 1,640C";
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
                Text = "SUBTERRANEAN METALLURGY // INDUCTION CUPOLA FURNACE [MET-03]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: SMELTING ACTIVE - MOLTEN HIGH-TUNGSTEN ALLOY / 1,640C"
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
            var leftPanel = CreatePanelFrame("CRUCIBLE INDUCTION COILS & FLUX");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftPanel.GetNode<MarginContainer>("Margin").AddChild(_telemetryContainer);
            _telemetryContainer.AddChild(CreateTelemetryRow("CORE MELT TEMPERATURE", "1,640C @ 850 kW INDUCTION", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(CreateTelemetryRow("REFRACTORY LINING WEAR", "18.5% (ZIRCONIA BRICK OK)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("MAGNESIUM INJECTION FLUX", "14.2 KG/CHARGE [DESULFURIZING]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _telemetryContainer.AddChild(CreateTelemetryRow("SLAG VISCOSITY INDEX", "2.1 Poise [FLUID DRAIN]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("MAGNETIC STIRRING FREQ", "60 Hz AC [HOMOGENEOUS]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = CreatePanelFrame("TILTING HYDRAULICS & INGOT POURING");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _buttonContainer.AddThemeConstantOverride("separation", 12);
            centerPanel.GetNode<MarginContainer>("Margin").AddChild(_buttonContainer);
            _buttonContainer.AddChild(new Button { Text = "[ACTIVATE 850 KW INDUCTION COILS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[TILT CRUCIBLE FOR MOLTEN POUR]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[INJECT MAGNESIUM DESULFURIZER]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[SKIM ACCUMULATED SURFACE SLAG]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = CreatePanelFrame("CAST INGOT INVENTORY & ALLOY STOCK");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dataContainer.AddThemeConstantOverride("separation", 8);
            rightPanel.GetNode<MarginContainer>("Margin").AddChild(_dataContainer);
            _dataContainer.AddChild(CreateTelemetryRow("ARMOR-GRADE STEEL BILLETS", "24 INGOTS (1,200 KG)", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("LEAD-TUNGSTEN RADIATION SHIELDS", "8 PLATES CAST", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(CreateTelemetryRow("RAW SCRAP CHARGE HOPPER", "3,400 KG REBAR / RAILS", AshfallUiHelpers.ToColor(DesignTheme.Dim)));
            _dataContainer.AddChild(CreateTelemetryRow("SLAG DISPOSAL SUMP", "420 KG DIVERTED TO ROAD BED", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

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
                Text = "[MET-03] Induction coils drew 850 kW. Core melt reached 1,640C.\n[MET-03] 24 armor-grade steel billets poured into casting molds.",
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
