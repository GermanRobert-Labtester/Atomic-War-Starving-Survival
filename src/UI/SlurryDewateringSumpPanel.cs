using System;
using System.Linq;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Sludge-plant console for the sump drainage authority (Plan 70).
    /// Presentation only: renders SumpFloodingSystem state and issues commands
    /// through SumpFloodingHostSession. Core owns all sludge math.
    /// </summary>
    public partial class SlurryDewateringSumpPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private SumpFloodingHostSession? _host;
        private Label? _statusBadgeLabel;
        private Label? _logOutputLabel;
        private Label? _basinLevelValue = null!;
        private Label? _sludgeValue = null!;
        private Label? _cakeValue = null!;
        private Label? _tailingsValue = null!;
        private Label? _greywaterValue = null!;
        private Label? _mediaValue = null!;
        private Label? _conditionValue = null!;
        private Button? _flocculateButton;
        private Button? _centrifugeButton;
        private Button? _replaceMediaButton;

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
            if (_host is SumpFloodingHostSession previous)
                previous.StateChanged -= RefreshView;
            _host = session as SumpFloodingHostSession;
            if (_host != null)
                _host.StateChanged += RefreshView;
            IsBound = _host != null;
            RefreshView();
        }

        public void Unbind()
        {
            if (_host is SumpFloodingHostSession previous)
                previous.StateChanged -= RefreshView;
            _host = null;
            IsBound = false;
        }

        private SumpNode? BusiestSludgeNode =>
            _host?.System.State.nodes
                .OrderByDescending(n => n.settledSludgeKg)
                .ThenByDescending(n => n.suspendedSolidsKg)
                .FirstOrDefault();

        public void RefreshView()
        {
            if (_host == null || !IsInsideTree())
                return;

            var state = _host.System.State;
            var node = BusiestSludgeNode;

            if (_basinLevelValue != null && node != null)
                _basinLevelValue.Text = $"{node.waterLevelCm:F0} / {node.maxWaterLevelCm:F0} cm";
            if (_sludgeValue != null && node != null)
                _sludgeValue.Text = $"{node.settledSludgeKg:F1} kg settled / {node.suspendedSolidsKg:F1} kg suspended";
            if (_cakeValue != null)
                _cakeValue.Text = $"{state.dewateredCakeKg:F1} kg in cake bay";
            if (_tailingsValue != null)
                _tailingsValue.Text = $"{state.hazardousTailingsKg:F1} kg (sealed drums)";
            if (_greywaterValue != null)
                _greywaterValue.Text = state.unroutedGreywaterLiters > 0.01f
                    ? $"{state.unroutedGreywaterLiters:F1} L awaiting treatment routing"
                    : "routed to water treatment";
            if (_mediaValue != null)
                _mediaValue.Text = state.centrifugeFilterMedia <= SumpFloodingSystem.CentrifugeLowMediaThreshold
                    ? $"{state.centrifugeFilterMedia:F0}% WORN — replace"
                    : $"{state.centrifugeFilterMedia:F0}%";
            if (_conditionValue != null)
                _conditionValue.Text = $"{state.centrifugeCondition:F0}% | batches: {state.centrifugeBatchesCompleted}";

            if (_statusBadgeLabel != null)
            {
                _statusBadgeLabel.Text = node is { settledSludgeKg: > 0 } or { suspendedSolidsKg: > 0 }
                    ? "STATUS: SLURRY PROCESSING REQUIRED"
                    : "STATUS: SUMP PUMPS ACTIVE - NO SLUDGE BACKLOG";
            }

            // Commands state → blocker → cost → consequence.
            bool canTreat = node != null && node.suspendedSolidsKg > 0f;
            if (_flocculateButton != null)
                _flocculateButton.TooltipText = canTreat
                    ? $"Doses flocculant (chemicals ×{2 * SumpFloodingSystem.FlocculantUnitsPerDoseTier}) — settles suspended silt into sludge"
                    : "No suspended silt to treat";
            if (_flocculateButton != null) _flocculateButton.Disabled = !canTreat;

            bool canSpin = node is { settledSludgeKg: > 0 } && state.centrifugeCondition > 0f
                && state.centrifugeFilterMedia > 0f;
            if (_centrifugeButton != null)
                _centrifugeButton.TooltipText = canSpin
                    ? "Processes settled sludge into cake, tailings and greywater (needs power + 1 cloth)"
                    : "Requires settled sludge, a working centrifuge, power and filter cloth";
            if (_centrifugeButton != null) _centrifugeButton.Disabled = !canSpin;

            if (_replaceMediaButton != null)
                _replaceMediaButton.TooltipText = "Swaps in a fresh cloth filter (1 cloth)";
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
            var headerTitleLabel = new Label
            {
                Text = "SUBTERRANEAN HYDRAULICS // SLURRY DEWATERING SUMP [HYDRO-03]",
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            headerTitleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(headerTitleLabel);

            _statusBadgeLabel = new Label
            {
                Text = "STATUS: SUMP PUMPS ACTIVE"
            };
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

            // Three-Column High-Density Grid
            var bodyHBox = new HBoxContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            bodyHBox.AddThemeConstantOverride("separation", 16);
            mainVBox.AddChild(bodyHBox);

            // Left Column (Telemetry)
            var (leftPanel, leftMargin) = CreatePanelFrame("SUMP BASIN & SLUDGE STATE");
            bodyHBox.AddChild(leftPanel);
            var telemetryContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            telemetryContainer.AddThemeConstantOverride("separation", 8);
            leftMargin.AddChild(telemetryContainer);
            var basinRow = SplitRow(telemetryContainer, "BASIN LEVEL (WORST NODE)");
            _basinLevelValue = basinRow.value;
            var sludgeRow = SplitRow(telemetryContainer, "SLUDGE (SETTLED / SUSPENDED)");
            _sludgeValue = sludgeRow.value;
            var mediaRow = SplitRow(telemetryContainer, "CENTRIFUGE MEDIA");
            _mediaValue = mediaRow.value;
            var conditionRow = SplitRow(telemetryContainer, "CENTRIFUGE CONDITION");
            _conditionValue = conditionRow.value;

            // Center Column (Interactive Controls)
            var (centerPanel, centerMargin) = CreatePanelFrame("SLUDGE PROCESSING COMMANDS");
            bodyHBox.AddChild(centerPanel);
            var buttonContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            buttonContainer.AddThemeConstantOverride("separation", 12);
            centerMargin.AddChild(buttonContainer);

            _flocculateButton = new Button { Text = "[DOSE FLOCCULANT — SETTLE SILT]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _flocculateButton.Pressed += OnFlocculatePressed;
            buttonContainer.AddChild(_flocculateButton);

            _centrifugeButton = new Button { Text = "[RUN CENTRIFUGE BATCH]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _centrifugeButton.Pressed += OnCentrifugePressed;
            buttonContainer.AddChild(_centrifugeButton);

            _replaceMediaButton = new Button { Text = "[REPLACE FILTER CLOTH]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            _replaceMediaButton.Pressed += OnReplaceMediaPressed;
            buttonContainer.AddChild(_replaceMediaButton);

            var packCakeButton = new Button { Text = "[PACK CAKE FOR SMELTING]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            packCakeButton.Pressed += OnPackCakePressed;
            buttonContainer.AddChild(packCakeButton);

            var packDrumsButton = new Button { Text = "[SEAL TAILINGS DRUMS]", SizeFlagsHorizontal = SizeFlags.ExpandFill };
            packDrumsButton.Pressed += OnPackDrumsPressed;
            buttonContainer.AddChild(packDrumsButton);

            // Right Column (Data & Logistics)
            var (rightPanel, rightMargin) = CreatePanelFrame("DEWATERED OUTPUT & WASTE");
            bodyHBox.AddChild(rightPanel);
            var dataContainer = new VBoxContainer { SizeFlagsVertical = SizeFlags.ExpandFill };
            dataContainer.AddThemeConstantOverride("separation", 8);
            rightMargin.AddChild(dataContainer);
            var cakeRow = SplitRow(dataContainer, "SLUDGE CAKE STOCK");
            _cakeValue = cakeRow.value;
            var tailingsRow = SplitRow(dataContainer, "HAZARDOUS TAILINGS");
            _tailingsValue = tailingsRow.value;
            var greywaterRow = SplitRow(dataContainer, "RECLAIMED GREYWATER");
            _greywaterValue = greywaterRow.value;
            var disposalNote = new Label
            {
                Text = "Tailings are sealed for disposal. Cake awaits assay.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            disposalNote.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            dataContainer.AddChild(disposalNote);

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
                Text = "[HYDRO-03] Slurry plant standing by.",
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            _logOutputLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            logMargin.AddChild(_logOutputLabel);
            mainVBox.AddChild(logPanel);
        }

        private void OnFlocculatePressed()
        {
            var node = BusiestSludgeNode;
            if (_host == null || node == null) return;
            var res = _host.StartFlocculation(node.nodeId, 1);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? $"[HYDRO-03] Flocculant dosed into {node.displayName}. Suspended silt settling."
                    : $"[HYDRO-03] Flocculation refused ({res.MessageKey}). Check chemical stock.";
        }

        private void OnCentrifugePressed()
        {
            var node = BusiestSludgeNode;
            if (_host == null || node == null) return;
            var res = _host.RunCentrifugeBatch(node.nodeId);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? $"[HYDRO-03] Centrifuge batch complete for {node.displayName}. Greywater to treatment."
                    : $"[HYDRO-03] Centrifuge refused ({res.MessageKey}). Check power, cloth and sludge.";
        }

        private void OnReplaceMediaPressed()
        {
            if (_host == null) return;
            var res = _host.ReplaceCentrifugeMedia();
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[HYDRO-03] Fresh filter cloth installed."
                    : $"[HYDRO-03] No cloth available ({res.MessageKey}).";
        }

        private void OnPackCakePressed()
        {
            if (_host == null) return;
            var res = _host.PackCakeForSmelting(4);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[HYDRO-03] Cake blocks stacked for the foundry recovery melt."
                    : $"[HYDRO-03] Cake packing refused ({res.MessageKey}).";
        }

        private void OnPackDrumsPressed()
        {
            if (_host == null) return;
            var res = _host.PackTailingsDrums(4);
            if (_logOutputLabel != null)
                _logOutputLabel.Text = res.IsSuccess
                    ? "[HYDRO-03] Tailings drums sealed and ready for hauling."
                    : $"[HYDRO-03] Drum sealing refused ({res.MessageKey}).";
        }

        private static (HBoxContainer row, Label value) SplitRow(VBoxContainer container, string label)
        {
            var row = CreateTelemetryRow(label, "—", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            container.AddChild(row);
            return (row, (Label)row.GetChild(1));
        }

        private static (PanelContainer panel, MarginContainer margin) CreatePanelFrame(string headerText)
        {
            var panel = new PanelContainer
            {
                SizeFlagsHorizontal = SizeFlags.ExpandFill,
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
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

            return (panel, margin);
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
