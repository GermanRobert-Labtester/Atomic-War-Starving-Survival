using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class TraumaBondingCohortPanel : Control, IBindablePanel
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
                _statusBadgeLabel.Text = "STATUS: COHORT STRESS CRITICAL - TRAUMA RESONANCE: 78.4%";
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
                Text = "SHELTER SOCIAL PSYCHOLOGY // TRAUMA BONDING & COHORT MATRIX [SOC-03]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            _headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(_headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: COHORT STRESS CRITICAL - TRAUMA RESONANCE: 78.4%"
            };
            _statusBadgeLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Hot));
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
            var leftPanel = CreatePanelFrame("SURVIVOR PSYCHOMETRICS & CRISIS VECTORS");
            bodyHBox.AddChild(leftPanel);
            _telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftPanel.GetNode<MarginContainer>("Margin").AddChild(_telemetryContainer);
            _telemetryContainer.AddChild(CreateTelemetryRow("SHARED TRAUMA RESONANCE", "78.4% [SEVERE STRAIN]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(CreateTelemetryRow("VANE - ROSTOVA BOND INDEX", "+0.65 [MUTUAL LOSS]", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _telemetryContainer.AddChild(CreateTelemetryRow("MILLER - CARVER ANTAGONISM", "-0.82 [FOUNDRY ACCIDENT]", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _telemetryContainer.AddChild(CreateTelemetryRow("PANIC CONTAGION RISK", "44.0% [ISOLATION NEEDED]", AshfallUiHelpers.ToColor(DesignTheme.Hot)));
            _telemetryContainer.AddChild(CreateTelemetryRow("COLLECTIVE SHELTER GRIEF", "88.2% [DESPAIR SPIKE]", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

            // Center Column (Interactive Controls)
            var centerPanel = CreatePanelFrame("COHORT COHESION & SOCIAL INTERVENTIONS");
            bodyHBox.AddChild(centerPanel);
            _buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _buttonContainer.AddThemeConstantOverride("separation", 12);
            centerPanel.GetNode<MarginContainer>("Margin").AddChild(_buttonContainer);
            _buttonContainer.AddChild(new Button { Text = "[ADMINISTER EMOTIONAL DE-ESCALATION TALK]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[DISTRIBUTE EXTRA PHARMA SEDATIVES]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[SEPARATE SEDITIOUS CONFLICT PAIRS]", SizeFlagsHorizontal = SizeFlags.ExpandFill });
            _buttonContainer.AddChild(new Button { Text = "[ORDER MANDATORY RECREATION & VINYL LISTENING]", SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Right Column (Data & Logistics)
            var rightPanel = CreatePanelFrame("GRIEVANCE LEDGER & PHARMA STOCKS");
            bodyHBox.AddChild(rightPanel);
            _dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            _dataContainer.AddThemeConstantOverride("separation", 8);
            rightPanel.GetNode<MarginContainer>("Margin").AddChild(_dataContainer);
            _dataContainer.AddChild(CreateTelemetryRow("COHORT ALPHA (VETERANS)", "88% COHESION / STABLE", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("COHORT BETA (FOUNDRY)", "42% COHESION / FRACTURED", AshfallUiHelpers.ToColor(DesignTheme.Critical)));
            _dataContainer.AddChild(CreateTelemetryRow("COHORT GAMMA (BEREAVED)", "94% COHESION / UNITED", AshfallUiHelpers.ToColor(DesignTheme.Warm)));
            _dataContainer.AddChild(CreateTelemetryRow("DIAZEPAM / HALOPERIDOL VIALS", "14 DOSES REMAINING", AshfallUiHelpers.ToColor(DesignTheme.Dim)));

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
                Text = "[SOC-03] Rostova and Vane formed mutual grief pact.\n[SOC-03] Morale de-escalation protocol lowered conflict tension.",
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
