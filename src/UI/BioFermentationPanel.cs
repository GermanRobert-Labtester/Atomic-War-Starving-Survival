// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Shelter;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 126 — subterranean biological fermentation workflow.
    /// Presentation-only: every mutation routes through the bound
    /// <see cref="BioFermentationEngine"/>; feedback mirrors Core results.
    /// State → blocker → cost → consequence per the UI panel standard.
    /// No culturing or process protocol appears anywhere in this UI.
    /// </summary>
    public partial class BioFermentationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private BioFermentationEngine? _system;
        private VBoxContainer _detail = null!;
        private OptionButton _processSelect = null!;
        private OptionButton _feedstockSelect = null!;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        /// <summary>Operator identity for trait/skill snapshot (host-set).</summary>
        public string OperatorId { get; set; } = "player";
        /// <summary>Current campaign day (host-set, used for batch bookkeeping).</summary>
        public Func<int>? DayProvider { get; set; }

        public bool IsBound => _system != null;

        public void Bind(BioFermentationEngine system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind() => _system = null;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("FERMENTATION REACTOR // BIOLOGICAL PROCESS", minWidth: 1000, minHeight: 640);

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);
            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            _shell.SetContent(detailScroll);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        private int CurrentDay => DayProvider?.Invoke() ?? 0;

        private void SetFeedback(string message, bool failure)
        {
            _feedbackText = message;
            _feedbackIsFailure = failure;
        }

        private Ashfall.Core.ActionResult RunCommand(Func<Ashfall.Core.ActionResult> action, string successText)
        {
            if (_system == null) return Ashfall.Core.ActionResult.Failed("unbound", "bioferm.unbound");
            var result = action();
            SetFeedback(result.IsSuccess ? successText : $"Blocked: {DescribeFailure(result.FailureCode)}", !result.IsSuccess);
            RefreshView();
            return result;
        }

        private static string DescribeFailure(string? code) => code switch
        {
            "already_built" => "The reactor is already assembled.",
            "missing_parts" => "Not enough build materials (scrap, pipe, filter module).",
            "not_idle" => "The reactor is busy — abort or finish the current batch first.",
            "missing_sanitize_supplies" => "Sanitizing needs 2 clean water.",
            "nothing_to_service" => "Nothing to service right now.",
            "missing_kit" => "Service needs a reactor service kit (and a filter module when clogged).",
            "reactor_unbuilt" => "Build the reactor first.",
            "invalid_units" => "Stage a positive amount.",
            "invalid_item" => "That material cannot be fermented.",
            "not_sanitized" => "Sanitize the reactor before starting a batch.",
            "missing_process" => "Unknown process selection.",
            "needs_feedstock" => "Stage the required feedstock amount first.",
            "insufficient_inputs" => "Missing starter culture or staged feedstock in storage.",
            "nothing_active" => "No batch in progress.",
            "not_complete" => "The batch is not complete yet.",
            "grant_failed" => "Storage is full — the batch is held complete; try again.",
            _ => code ?? "unknown error"
        };

        private static string PhaseText(BioFermentationEngine system)
        {
            int phase = system.State.phase;
            if (!Enum.IsDefined(typeof(BioFermentationPhase), phase))
                return "UNKNOWN";
            return ((BioFermentationPhase)phase).ToString().Replace("_", " ").ToUpperInvariant();
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var state = _system.State;
            int phase = state.phase;

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("REACTOR STATUS"));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("State", PhaseText(_system),
                phase == (int)BioFermentationPhase.MaintenanceRequired
                || phase == (int)BioFermentationPhase.Contaminated
                ? AshfallUiHelpers.ColorCritical
                : phase == (int)BioFermentationPhase.Complete
                    ? AshfallUiHelpers.ColorSuccess
                    : AshfallUiHelpers.ColorText));

            var process = _system.GetProcess(state.process_id);
            if (process != null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Process", process.display_name, AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Progress",
                    $"{(state.batch_progress * 100f):F0}% (phase day {state.phase_days_elapsed}/{process.base_duration_days})",
                    AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Temperature",
                    $"{state.temperature_state.ToUpperInvariant()} (target {process.temperature_band.ToUpperInvariant()})",
                    state.temperature_state == process.temperature_band ? AshfallUiHelpers.ColorText : AshfallUiHelpers.ColorWarning));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Aeration",
                    process.aeration_required
                        ? (state.aeration_state ? "ON — required" : "OFF — REQUIRED!")
                        : (state.aeration_state ? "ON" : "off (not required)"),
                    process.aeration_required && !state.aeration_state ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Acidity", $"{state.acidity_state.ToUpperInvariant()} (target {process.acidity_target_band.ToUpperInvariant()})",
                    state.acidity_state == process.acidity_target_band ? AshfallUiHelpers.ColorText : AshfallUiHelpers.ColorWarning));
            }

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Culture health", $"{state.culture_health:F0}/100",
                state.culture_health < 35f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Process health", $"{state.process_health:F0}/100",
                state.process_health < 35f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Contamination", state.contamination_state.ToUpperInvariant(),
                state.contamination_state == "clear" ? AshfallUiHelpers.ColorText : AshfallUiHelpers.ColorCritical));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Filter condition", $"{state.filter_condition:F0}/100",
                state.filter_condition <= 0f ? AshfallUiHelpers.ColorCritical
                : state.filter_condition < 50f ? AshfallUiHelpers.ColorWarning
                : AshfallUiHelpers.ColorText));

            if (state.fault_state.Length > 0)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Fault", state.fault_state.ToUpperInvariant(), AshfallUiHelpers.ColorCritical));

            if (phase == (int)BioFermentationPhase.Complete)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Yield quality",
                    state.yield_quality >= 0f ? $"{state.yield_quality:F0}/100" : "resolving…",
                    state.yield_quality >= BioFermentationEngine.PremiumQualityThreshold ? AshfallUiHelpers.ColorSuccess : AshfallUiHelpers.ColorText));

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Batches completed", state.cycle_count.ToString(), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Last service", state.last_service_day >= 0 ? $"day {state.last_service_day}" : "never", AshfallUiHelpers.ColorText));

            // Feedstock staging readout.
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("STAGED FEEDSTOCK"));
            if (state.staged_feedstock.Count == 0)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "Nothing staged. Sugar or starch feedstock is loaded below.",
                    title: "EMPTY STAGE",
                    actionHint: "Use STAGE +1."));
            }
            else
            {
                foreach (var charge in state.staged_feedstock)
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow(charge.item_id, $"{charge.units} units", AshfallUiHelpers.ColorInfo));
            }

            BuildActionRows();
        }

        private void BuildActionRows()
        {
            if (_system == null) return;
            var state = _system.State;
            int phase = state.phase;

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("REACTOR ACTIONS"));

            if (phase == (int)BioFermentationPhase.SterilizedReady)
            {
                // Process selection (display names only — no raw process ids).
                _processSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
                foreach (var process in _system.Processes.Values.OrderBy(p => p.display_name, StringComparer.Ordinal))
                    _processSelect.AddItem(process.display_name);
                var selectRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                selectRow.AddChild(new Label { Text = "Process: ", CustomMinimumSize = new Vector2(90, 26) });
                selectRow.AddChild(_processSelect);
                _detail.AddChild(selectRow);
            }

            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            if (phase == (int)BioFermentationPhase.Unbuilt)
            {
                row.AddChild(MakeAction("BUILD REACTOR", "Costs 4 scrap metal + 2 metal pipe + 1 filter module.",
                    () => RunCommand(() => _system!.BuildReactor(), "Reactor assembled in the fermenter bay.")));
            }

            if (phase == (int)BioFermentationPhase.Idle)
            {
                row.AddChild(MakeAction("SANITIZE", "Costs 2 clean water. Prepares the sterile runway for a batch.",
                    () => RunCommand(() => _system!.SanitizeReactor(), "Reactor sanitized and ready.")));
            }

            if (_system.HasRunningBatch)
            {
                row.AddChild(MakeAction(state.aeration_state ? "AERATION: OFF" : "AERATION: ON",
                    state.aeration_state ? "Cut the air feed to the batch." : "Open the air feed to the batch.",
                    () =>
                    {
                        var next = !state.aeration_state;
                        _system!.SetAeration(next);
                        SetFeedback(next ? "Aeration opened — required processes can breathe." : "Aeration cut — aerated processes will suffer.", false);
                        RefreshView();
                        return Ashfall.Core.ActionResult.Success("bioferm.aeration");
                    }));
                row.AddChild(MakeAction("ABORT BATCH", "Deliberate loss — feedstock and starter are written off.",
                    () => RunCommand(() => _system!.AbortBatch(), "Batch aborted.")));
            }

            if (phase == (int)BioFermentationPhase.SterilizedReady)
            {
                row.AddChild(MakeAction("START BATCH", "Consumes 1 starter culture + the staged feedstock.",
                    () => StartBatchViaSelection()));
            }

            if (phase == (int)BioFermentationPhase.Complete)
            {
                row.AddChild(MakeAction("HARVEST OUTPUT", "Draws the output (and any waste byproduct) into storage once.",
                    () => RunCommand(() => _system!.HarvestBatch(), "Output drawn into storage.")));
            }

            if (phase == (int)BioFermentationPhase.MaintenanceRequired
                || phase == (int)BioFermentationPhase.Contaminated
                || phase == (int)BioFermentationPhase.Failed
                || state.fault_state.Length > 0
                || state.contamination_state != "clear"
                || state.filter_condition < 50f)
            {
                row.AddChild(MakeAction("SERVICE REACTOR", "Costs 1 service kit; a clogged filter also costs 1 filter module.",
                    () => RunCommand(() => _system!.ServiceReactor(CurrentDay), "Reactor serviced.")));
            }

            if (row.GetChildCount() > 0)
                _detail.AddChild(row);

            if (phase == (int)BioFermentationPhase.Unbuilt) return;

            // Feedstock staging controls.
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("FEEDSTOCK LOADING"));

            var uniqueFeedstocks = new List<string>();
            foreach (var process in _system.Processes.Values.OrderBy(p => p.display_name, StringComparer.Ordinal))
                foreach (var feed in process.feedstock_item_ids)
                    if (!uniqueFeedstocks.Contains(feed))
                        uniqueFeedstocks.Add(feed);

            _feedstockSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
            foreach (var feed in uniqueFeedstocks)
            {
                var owner = _system.Processes.Values.FirstOrDefault(p => p.feedstock_item_ids.Contains(feed));
                _feedstockSelect.AddItem(owner != null ? $"{feed} — usable for {owner.display_name}" : feed);
            }

            var stageRow = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            stageRow.AddChild(_feedstockSelect);
            stageRow.AddChild(MakeAction("STAGE +1",
                "Moves the selected feedstock onto the stage. Inventory is only consumed at batch start.",
                () =>
                {
                    int sel = _feedstockSelect?.Selected ?? -1;
                    if (sel < 0 || sel >= uniqueFeedstocks.Count)
                        return Ashfall.Core.ActionResult.Failed("no_selection", "bioferm.invalid_item");
                    return RunCommand(() => _system!.StageFeedstock(uniqueFeedstocks[sel], 1), "Feedstock staged.");
                }));
            stageRow.AddChild(MakeAction("CLEAR STAGE", "Returns the stage to empty (nothing was consumed).",
                () => RunCommand(() => _system!.ClearStagedFeedstock(), "Stage cleared.")));
            _detail.AddChild(stageRow);
        }

        private Ashfall.Core.ActionResult StartBatchViaSelection()
        {
            if (_system == null) return Ashfall.Core.ActionResult.Failed("unbound", "bioferm.unbound");
            var processes = _system.Processes.Values.OrderBy(p => p.display_name, StringComparer.Ordinal).ToList();
            int sel = _processSelect?.Selected ?? -1;
            if (sel < 0 || sel >= processes.Count)
                return Ashfall.Core.ActionResult.Failed("no_selection", "bioferm.missing_process");

            var process = processes[sel];
            return RunCommand(() => _system!.StartBatch(process.process_id, OperatorId, CurrentDay),
                $"Batch started: {process.display_name}.");
        }

        private Control MakeAction(string label, string consequence, Func<Ashfall.Core.ActionResult> command)
        {
            var btn = AshfallUiHelpers.MakeButton(label, () => command());
            btn.TooltipText = consequence;
            btn.CustomMinimumSize = new Vector2(0, 30);
            return btn;
        }
    }
}