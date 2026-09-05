using System;
using System.Linq;
using Godot;
using Ashfall.Core.Expeditions;
using Ashfall.Core.Foundry;
using Ashfall.Core.Medical;
using Ashfall.Core.Radio;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Bound operations console for Plans 130–133. It is presentation-only:
    /// commands route through the four host sessions and railway remains the
    /// canonical train/track owner.
    /// </summary>
    public partial class Plans130To133Panel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private PowderMetallurgyHostSession? _powder;
        private NvisCommunicationsHostSession? _nvis;
        private LyophilizationHostSession? _lyophilization;
        private DraisineRerailingHostSession? _draisine;
        private RailwaySystem? _railway;
        private ExpeditionHostSession? _expeditions;
        private Func<int>? _dayProvider;
        private Action<string>? _acknowledgeRecall;
        private VBoxContainer _content = null!;
        private VBoxContainer _systemContent = null!;
        private Label _eventLog = null!;

        public bool IsBound => _powder != null && _nvis != null
            && _lyophilization != null && _draisine != null && _railway != null;

        public void Bind(
            PowderMetallurgyHostSession powder,
            NvisCommunicationsHostSession nvis,
            LyophilizationHostSession lyophilization,
            DraisineRerailingHostSession draisine,
            RailwaySystem railway,
            ExpeditionHostSession? expeditions,
            Func<int>? dayProvider,
            Action<string>? acknowledgeRecall)
        {
            Unbind();
            _powder = powder;
            _nvis = nvis;
            _lyophilization = lyophilization;
            _draisine = draisine;
            _railway = railway;
            _expeditions = expeditions;
            _dayProvider = dayProvider;
            _acknowledgeRecall = acknowledgeRecall;

            _powder.StateChanged += RefreshView;
            _nvis.StateChanged += RefreshView;
            _lyophilization.StateChanged += RefreshView;
            _draisine.StateChanged += RefreshView;
            RefreshView();
        }

        public void Unbind()
        {
            if (_powder != null) _powder.StateChanged -= RefreshView;
            if (_nvis != null) _nvis.StateChanged -= RefreshView;
            if (_lyophilization != null) _lyophilization.StateChanged -= RefreshView;
            if (_draisine != null) _draisine.StateChanged -= RefreshView;
            _powder = null;
            _nvis = null;
            _lyophilization = null;
            _draisine = null;
            _railway = null;
            _expeditions = null;
            _dayProvider = null;
            _acknowledgeRecall = null;
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
                "SYS: MATERIALS // REGIONAL COMMS // MEDICAL PRESERVATION // RAIL RECOVERY",
                minWidth: 1320,
                minHeight: 700);
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

            _eventLog = AshfallUiHelpers.MakeMetadata("No recent Plan 130–133 event.");
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
                    "Plan 130–133 sessions unavailable", "offline"));
                return;
            }

            var row = new HBoxContainer();
            row.AddThemeConstantOverride("separation", DesignTheme.SpacingMd);
            row.SizeFlagsVertical = SizeFlags.ExpandFill;
            _systemContent.AddChild(row);
            row.AddChild(BuildPowderColumn());
            row.AddChild(BuildNvisColumn());
            row.AddChild(BuildLyophilizationColumn());
            row.AddChild(BuildDraisineColumn());

            string eventText = FirstEvent();
            _eventLog.Text = string.IsNullOrEmpty(eventText)
                ? "Systems online. Actions remain subject to inventory, power, weather, and rail state."
                : eventText;
        }

        private Control BuildPowderColumn()
        {
            var panel = MakeColumn("ABSTRACT MATERIALS", out var shell);
            var state = _powder!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "STATUS", state.status.ToString().ToUpperInvariant(),
                AshfallUiHelpers.ToColor(state.status == PowderMetallurgyStatus.Ready
                    ? DesignTheme.Pale : DesignTheme.Warm)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "COMPLETED", state.completed_batches.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "OUTPUT UNITS", state.produced_units.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            var process = _powder.System.Processes.Values.FirstOrDefault();
            if (process != null)
            {
                panel.AddChild(AshfallUiHelpers.MakeSmall(process.display_name));
                panel.AddChild(AshfallUiHelpers.MakeButton("START MATERIAL BATCH", () =>
                {
                    var result = _powder.StartBatch(process.process_id, _dayProvider?.Invoke() ?? 0);
                    SetResult(result.IsSuccess ? "Material batch reserved." : $"Material batch blocked: {result.FailureCode}.");
                }));
            }
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Quality and reliability are abstract readiness modifiers. No real-world propellant or weapon recipe is represented."));
            return shell;
        }

        private Control BuildNvisColumn()
        {
            var panel = MakeColumn("REGIONAL NVIS / C4I", out var shell);
            var state = _nvis!.System.State;
            var channel = _nvis.System.GetChannel(state.selected_channel_id);
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "MODE", state.mode.ToString().ToUpperInvariant(),
                AshfallUiHelpers.ToColor(state.powered ? DesignTheme.Pale : DesignTheme.Warm)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "CHANNEL", channel?.display_name ?? "NONE",
                AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "DELIVERED", $"{state.delivered_transmissions}/{state.total_transmissions}",
                AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeButton("BROADCAST REGIONAL STATUS", () =>
            {
                var result = _nvis.BeginStatusTransmission(
                    "expedition_status",
                    _dayProvider?.Invoke() ?? 0,
                    _expeditions?.Engine?.Active.Count ?? 0);
                SetResult(result.IsSuccess ? "Regional status queued." : $"Transmission blocked: {result.FailureCode}.");
            }));
            string? activeSurvivor = _expeditions?.Engine?.Active.Keys.FirstOrDefault();
            if (!string.IsNullOrEmpty(activeSurvivor))
            {
                panel.AddChild(AshfallUiHelpers.MakeButton("REQUEST FIELD RECALL", () =>
                {
                    var result = _nvis.RequestRecall(activeSurvivor, _dayProvider?.Invoke() ?? 0);
                    SetResult(result.IsSuccess ? "Field recall request queued." : $"Recall blocked: {result.FailureCode}.");
                }));
            }
            var pending = state.recall_requests.LastOrDefault(request => request != null && !request.acknowledged);
            if (pending != null)
            {
                panel.AddChild(AshfallUiHelpers.MakeButton($"ACKNOWLEDGE RECALL: {pending.survivor_id}", () =>
                {
                    _acknowledgeRecall?.Invoke(pending.survivor_id);
                    RefreshView();
                }));
            }
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Status transmission completes on the next day tick. Recall acknowledgement returns through ExpeditionSystem."));
            return shell;
        }

        private Control BuildLyophilizationColumn()
        {
            var panel = MakeColumn("BIOLOGIC PRESERVATION", out var shell);
            var state = _lyophilization!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "STATUS", state.status.ToString().ToUpperInvariant(),
                AshfallUiHelpers.ToColor(state.status == LyophilizationStatus.Ready
                    || state.status == LyophilizationStatus.Complete
                    ? DesignTheme.Pale : DesignTheme.Warm)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "BATCHES", state.completed_batches.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "VIABLE UNITS", state.viable_units_produced.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));
            var recipe = _lyophilization.System.Recipes.Values.FirstOrDefault();
            if (recipe != null)
            {
                panel.AddChild(AshfallUiHelpers.MakeSmall(recipe.display_name));
                panel.AddChild(AshfallUiHelpers.MakeButton("START PRESERVATION BATCH", () =>
                {
                    var result = _lyophilization.StartBatch(recipe.recipe_id, _dayProvider?.Invoke() ?? 0);
                    SetResult(result.IsSuccess ? "Preservation batch reserved." : $"Preservation blocked: {result.FailureCode}.");
                }));
            }
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "Viability and expiry are tracked per sealed batch and consumed through the medical pipeline."));
            return shell;
        }

        private Control BuildDraisineColumn()
        {
            var panel = MakeColumn("DRAISINE RECOVERY", out var shell);
            var state = _draisine!.System.State;
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "RECOVERY", state.status.ToString().ToUpperInvariant(),
                AshfallUiHelpers.ToColor(state.status == DraisineRecoveryStatus.Recovered
                    || state.status == DraisineRecoveryStatus.Idle
                    ? DesignTheme.Pale : DesignTheme.Warm)));
            panel.AddChild(AshfallUiHelpers.MakeDataRow(
                "ATTEMPTS", state.attempts.ToString(), AshfallUiHelpers.ToColor(DesignTheme.Pale)));

            var derailed = _railway!.State.trains.FirstOrDefault(train =>
                train != null && train.status == TrainDispatchStatus.Derailment);
            var equipment = _draisine.System.Equipment.Values.FirstOrDefault();
            if (derailed != null && equipment != null && state.status != DraisineRecoveryStatus.Rerailing)
            {
                panel.AddChild(AshfallUiHelpers.MakeSmall($"Derailed unit: {derailed.displayName}"));
                panel.AddChild(AshfallUiHelpers.MakeButton("BEGIN RE-RAILING", () =>
                {
                    var result = _draisine.StartRecovery(
                        derailed.trainId,
                        equipment.equipment_id,
                        _dayProvider?.Invoke() ?? 0);
                    SetResult(result.IsSuccess ? "Re-railing job started." : $"Recovery blocked: {result.FailureCode}.");
                }));
            }
            if (state.status == DraisineRecoveryStatus.Rerailing)
            {
                panel.AddChild(AshfallUiHelpers.MakeButton("ABANDON RECOVERY", () =>
                {
                    var result = _draisine.Abandon();
                    SetResult(result.IsSuccess ? "Recovery abandoned." : $"Abandon blocked: {result.FailureCode}.");
                }));
            }
            panel.AddChild(AshfallUiHelpers.MakeSmall(
                "RailwaySystem remains the authority for train condition, track integrity, and derailment state."));
            return shell;
        }

        private void SetResult(string message)
        {
            _eventLog.Text = message;
            RefreshView();
        }

        private string FirstEvent()
        {
            if (_powder != null && !string.IsNullOrEmpty(_powder.LastEvent)) return _powder.LastEvent;
            if (_nvis != null && !string.IsNullOrEmpty(_nvis.LastEvent)) return _nvis.LastEvent;
            if (_lyophilization != null && !string.IsNullOrEmpty(_lyophilization.LastEvent)) return _lyophilization.LastEvent;
            if (_draisine != null && !string.IsNullOrEmpty(_draisine.LastEvent)) return _draisine.LastEvent;
            return string.Empty;
        }

        private static VBoxContainer MakeColumn(string title, out Control shell)
        {
            var panel = AshfallUiHelpers.MakePanel(minWidth: 290);
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
