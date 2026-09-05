using System;
using Godot;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Bound operations console for the Plan 94–97 shelter systems.
    /// It exposes only Core session commands; state and inventory remain owned
    /// by the underlying authorities.
    /// </summary>
    public partial class Plans94To97Panel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private GrainProcessingHostSession? _grain;
        private CryogenicAirSeparationHostSession? _cryogenic;
        private HeliographHostSession? _heliograph;
        private Func<int>? _dayProvider;
        private VBoxContainer _content = null!;
        private VBoxContainer _systemContent = null!;
        private Label _eventLog = null!;

        public bool IsBound => _grain != null || _cryogenic != null || _heliograph != null;

        public void Bind(
            GrainProcessingHostSession grain,
            CryogenicAirSeparationHostSession cryogenic,
            HeliographHostSession heliograph,
            Func<int>? dayProvider = null)
        {
            Unbind();
            _grain = grain;
            _cryogenic = cryogenic;
            _heliograph = heliograph;
            _dayProvider = dayProvider;
            _grain.StateChanged += RefreshView;
            _cryogenic.StateChanged += RefreshView;
            _heliograph.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_grain != null) _grain.StateChanged -= RefreshView;
            if (_cryogenic != null) _cryogenic.StateChanged -= RefreshView;
            if (_heliograph != null) _heliograph.StateChanged -= RefreshView;
            _grain = null;
            _cryogenic = null;
            _heliograph = null;
            _dayProvider = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var background = new ColorRect { Color = new Color(0.04f, 0.04f, 0.05f, 0.94f) };
            background.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(background);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var shell = new AshfallDashboardShell(
                "SYS: SHELTER SUPPLY SYSTEMS // PLANS 94–97",
                minWidth: 1120,
                minHeight: 620);
            center.AddChild(shell);
            shell.AttachHeaderCloseButton("CLOSE [Esc]", Close);

            _content = new VBoxContainer();
            _content.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _content.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _content.SizeFlagsVertical = SizeFlags.ExpandFill;
            shell.SetContent(_content);

            _systemContent = new VBoxContainer();
            _systemContent.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _systemContent.SizeFlagsVertical = SizeFlags.ExpandFill;
            _systemContent.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            _content.AddChild(_systemContent);

            _eventLog = AshfallUiHelpers.MakeMetadata("No recent supply-system event.");
            _eventLog.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _content.AddChild(_eventLog);
            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public void RefreshView()
        {
            if (_systemContent == null) return;
            AshfallUiHelpers.EmptyChildren(_systemContent);

            if (!IsBound)
            {
                _systemContent.AddChild(AshfallUiHelpers.MakeEmptyStateLabel(
                    "No Plan 94–97 sessions bound", "offline"));
                return;
            }

            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            row.SizeFlagsVertical = SizeFlags.ExpandFill;
            _systemContent.AddChild(row);
            row.AddChild(BuildGrainColumn());
            row.AddChild(BuildCryogenicColumn());
            row.AddChild(BuildHeliographColumn());

            string eventText = FirstEvent();
            _eventLog.Text = string.IsNullOrEmpty(eventText)
                ? "Systems online. Actions remain subject to inventory, power, weather, and map state."
                : eventText;
        }

        private Control BuildGrainColumn()
        {
            var panel = MakeColumn("GRAIN MILL & SILO SAFETY", out var shell);
            var state = _grain!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "ACTIVE BATCHES", state.active_jobs.Count.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "COMPLETED", state.total_batches_completed.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            foreach (var silo in state.silos)
            {
                panel.AddChild(AshfallUiHelpers.MakeMono(
                    $"{silo.silo_id}: {silo.integrity:0}% integrity // {silo.moisture_pct:0.0}% moisture // {silo.pest_pressure:0.0}% pests"));
            }

            panel.AddChild(AshfallUiHelpers.MakeSeparator());
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Milled ash-barley becomes flour in the canonical shelter inventory. Silo treatment lowers pest pressure."));
            panel.AddChild(AshfallUiHelpers.MakeButton("MILL ASH-BARLEY BATCH", () =>
            {
                var result = _grain.StartMilling(
                    "recipe_ash_grain_flour", "grain_silo_holdfast");
                _eventLog.Text = result.IsSuccess
                    ? "Milling batch reserved. It will complete on the next processing tick."
                    : $"Milling blocked: {result.FailureCode}.";
                RefreshView();
            }));
            return shell;
        }

        private Control BuildCryogenicColumn()
        {
            var panel = MakeColumn("CRYOGENIC AIR SEPARATION", out var shell);
            var state = _cryogenic!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "PLANT", state.is_running ? "RUNNING" : "IDLE",
                AshfallUiHelpers.ToColor(state.is_running ? DesignTheme.Pale : DesignTheme.Warm)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "INTEGRITY", $"{state.plant_integrity:0}%", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "FILTER", $"{state.filter_condition:0}%", AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "CYCLES", state.cycles_completed.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Oxygen is a medical treatment input. Nitrogen is consumed by the weather-canister foundry line."));
            panel.AddChild(AshfallUiHelpers.MakeButton(
                state.is_running ? "STOP PLANT" : "START PLANT",
                () =>
                {
                    bool running = _cryogenic.SetRunning(!state.is_running);
                    _eventLog.Text = running
                        ? (_cryogenic.System.State.is_running ? "Air-separation plant started." : "Air-separation plant stopped.")
                        : "Plant command blocked.";
                    RefreshView();
                }));
            return shell;
        }

        private Control BuildHeliographColumn()
        {
            var panel = MakeColumn("HELIOGRAPH SIGNALING", out var shell);
            var state = _heliograph!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "MESSAGES", state.messages.Count.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "DELIVERED", state.delivered_count.ToString(),
                AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Transmission requires known map nodes, clear sight, and usable visibility. Distress handoff returns to the radio authority."));
            panel.AddChild(AshfallUiHelpers.MakeButton("TRANSMIT STATUS SIGNAL", () =>
            {
                var result = _heliograph.Transmit(
                    $"heliograph_status_{state.messages.Count}",
                    "heliograph_holdfast",
                    "heliograph_relay",
                    "shelter_status",
                    _dayProvider?.Invoke() ?? 0);
                _eventLog.Text = result.IsSuccess
                    ? "Optical status signal delivered."
                    : $"Transmission blocked: {result.FailureCode}.";
                RefreshView();
            }));
            return shell;
        }

        private static VBoxContainer MakeColumn(string title, out Control shell)
        {
            var panel = AshfallUiHelpers.MakePanel(minWidth: 320);
            panel.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            panel.SizeFlagsVertical = SizeFlags.ExpandFill;
            var margin = AshfallUiHelpers.MakeMargins(DesignTheme.SpacingSm);
            panel.AddChild(margin);
            var body = new VBoxContainer();
            body.AddThemeConstantOverride("separation", DesignTheme.SpacingSm);
            body.SizeFlagsVertical = SizeFlags.ExpandFill;
            margin.AddChild(body);
            body.AddChild(AshfallUiHelpers.MakeSectionHeader(title));
            shell = panel;
            return body;
        }

        private string FirstEvent()
        {
            if (_grain != null && !string.IsNullOrEmpty(_grain.LastEvent)) return _grain.LastEvent;
            if (_cryogenic != null && !string.IsNullOrEmpty(_cryogenic.LastEvent)) return _cryogenic.LastEvent;
            if (_heliograph != null && !string.IsNullOrEmpty(_heliograph.LastEvent)) return _heliograph.LastEvent;
            return string.Empty;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (Visible && @event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
