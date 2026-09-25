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
    /// Plan 202: retort bay — waste plastic to synthetic fuel fractions.
    /// Presentation-only: every mutation routes through the bound
    /// <see cref="PlasticPyrolysisSystem"/>; feedback mirrors Core results.
    /// Communicates state → blocker → cost → consequence per the UI standard.
    /// No real-world process setpoints are displayed.
    /// </summary>
    public partial class PlasticPyrolysisPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private PlasticPyrolysisHostSession? _session;
        private PlasticPyrolysisSystem? System => _session?.System;
        private VBoxContainer _detail = null!;
        private ItemList _feedstockList = null!;
        private string _selectedProfileId = string.Empty;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _session != null;

        public void Bind(PlasticPyrolysisHostSession session) { _session = session; RefreshView(); }
        public void Unbind() { _session = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("RETORT BAY // WASTE PLASTIC RECLAMATION", minWidth: 1000, minHeight: 650);

            _feedstockList = new ItemList
            {
                CustomMinimumSize = new Vector2(280, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _feedstockList.ItemSelected += OnFeedstockSelected;

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_feedstockList);

            var detailScroll = new ScrollContainer
            {
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.ExpandFill
            };
            detailScroll.AddChild(_detail);
            bodyRow.AddChild(detailScroll);

            _shell.SetContent(bodyRow);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            AddChild(_shell);
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close()
        {
            if (!AtomicWar.GodotApp.UI.UiMotion.AnimateClose(this))
                Visible = false;
            OnClose?.Invoke();
        }

        private void OnFeedstockSelected(long index)
        {
            if (System == null) return;
            var profiles = System.Catalog.feedstock_profiles;
            if (index >= 0 && index < profiles.Count)
            {
                _selectedProfileId = profiles[(int)index].feedstock_profile_id;
                RefreshView();
            }
        }

        private PyrolysisFeedstockProfile? SelectedProfile()
        {
            if (System == null) return null;
            foreach (var p in System.Catalog.feedstock_profiles)
                if (p.feedstock_profile_id == _selectedProfileId) return p;
            return System.Catalog.feedstock_profiles.Count > 0 ? System.Catalog.feedstock_profiles[0] : null;
        }

        private void SetFeedback(string message, bool failure)
        {
            _feedbackText = message;
            _feedbackIsFailure = failure;
        }

        private Ashfall.Core.ActionResult RunCommand(Func<Ashfall.Core.ActionResult> action, string successText)
        {
            if (_session == null) return Ashfall.Core.ActionResult.Failed("unbound", "pyro.unbound");
            var result = action();
            SetFeedback(result.IsSuccess ? successText : $"Blocked: {DescribeFailure(result.FailureCode)}", !result.IsSuccess);
            RefreshView();
            return result;
        }

        private static string DescribeFailure(string? code) => code switch
        {
            "pyro.not_constructed" => "No retort bay constructed.",
            "pyro.already_constructed" => "The retort bay already stands.",
            "pyro.missing_construction_material" => "Construction materials missing.",
            "pyro.consume_failed" => "Materials vanished mid-build — retry.",
            "pyro.missing_maintenance_material" => "Maintenance needs scrap metal.",
            "pyro.batch_active" => "A batch is already running.",
            "pyro.condition_critical" => "Machine condition critical — maintenance required first.",
            "pyro.buffer_full" => "Output tanks full — claim staged outputs first.",
            "pyro.unknown_profile" => "Unknown feedstock profile.",
            "pyro.insufficient_feedstock" => "Not enough of that feedstock in storage for a full batch.",
            "pyro.nothing_to_claim" => "No outputs staged.",
            "pyro.machine_unavailable" => "Machine unavailable.",
            _ => code ?? "unknown error"
        };

        private static int CountOf(PlasticPyrolysisSystem sys, string itemId) => sys.CountItem(itemId);

        private static string PhaseText(PlasticPyrolysisState state)
        {
            if (!state.machine_constructed) return "NOT CONSTRUCTED";
            if (state.active_batch == null) return "IDLE";
            return state.active_batch.phase == "stalling" ? "BATCH STALLED (POWER)" : "BATCH REACTING";
        }

        public void RefreshView()
        {
            if (_session == null || _detail == null || System == null) return;
            var sys = System;
            AshfallUiHelpers.EmptyChildren(_detail);
            _feedstockList.Clear();

            var state = sys.State;

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("RETORT BAY STATUS"));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("State", PhaseText(state),
                !state.machine_constructed ? AshfallUiHelpers.ColorDim : AshfallUiHelpers.ColorText));

            if (!state.machine_constructed)
            {
                var costs = string.Join(", ", sys.Catalog.machine.construction_required_items
                    .Select(k => $"{k.Value}× material"));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Build cost", costs, AshfallUiHelpers.ColorWarning));
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("CONSTRUCTION"));
                _detail.AddChild(AshfallUiHelpers.MakeBody(
                    "Erect the back-draft retort in the generator bay. It renders waste plastic into rough fuel fractions and carbon black. The process runs hot, draws steady power, and punishes dirty feedstock."));
                BuildConstructionActions();
                return;
            }

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Condition", $"{state.machine_condition:F0} / {sys.Catalog.machine.max_condition:F0}",
                state.machine_condition < 25f ? AshfallUiHelpers.ColorCritical :
                state.machine_condition < 50f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            int overdue = Math.Max(0, state.days_since_maintenance - sys.Catalog.machine.maintenance_interval_days);
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Maintenance", overdue > 0 ? $"OVERDUE {overdue}d" : $"in {sys.Catalog.machine.maintenance_interval_days - state.days_since_maintenance}d",
                overdue > 0 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            if (state.active_batch != null)
            {
                var profile = sys.FindProfile(state.active_batch.feedstock_profile_id);
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Active batch", $"{state.active_batch.batch_id} — {profile?.display_name ?? state.active_batch.feedstock_profile_id}", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Progress", $"day {state.active_batch.progress_days} / {profile?.process_duration_days ?? 0}", AshfallUiHelpers.ColorText));
                if (state.active_batch.phase == "stalling")
                    _detail.AddChild(AshfallUiHelpers.MakeWarning("Power deficit — the retort is cold until the grid recovers."));
                if (state.active_batch.quality_degraded)
                    _detail.AddChild(AshfallUiHelpers.MakeWarning("Yield fouled by an earlier incident."));
            }
            else
            {
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Active batch", "—", AshfallUiHelpers.ColorText));
            }

            int buffered = state.output_buffer.Count;
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Staged outputs", buffered >= sys.Catalog.storage_buffer_max_batches
                ? $"{buffered} (TANKS FULL)"
                : $"{buffered} / {sys.Catalog.storage_buffer_max_batches}",
                buffered >= sys.Catalog.storage_buffer_max_batches ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Lifetime", $"{state.total_batches_completed} batches, {state.total_incidents} incidents", AshfallUiHelpers.ColorMuted));

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("FEEDSTOCK PROFILES"));

            for (int i = 0; i < sys.Catalog.feedstock_profiles.Count; i++)
            {
                var p = sys.Catalog.feedstock_profiles[i];
                int stock = 0;
                foreach (var id in p.accepted_item_ids) stock += Math.Max(0, CountOf(sys, id));
                bool canFeed = stock >= p.batch_input_units;
                _feedstockList.AddItem(
                    $"{p.display_name}\n      stock {stock} / needs {p.batch_input_units} — risk {p.hazard_risk_bp / 100.0:0.0#}%",
                    null, false);
                _feedstockList.SetItemDisabled(i, !canFeed);
                _feedstockList.SetItemTooltip(i, canFeed
                    ? $"Yields ≈ {p.liquid_fuel_yield_units + p.light_fraction_yield_units} fuel units + {p.solid_carbon_yield_units} carbon per batch."
                    : $"Insufficient feedstock in storage ({stock}/{p.batch_input_units}).");
                if (p.feedstock_profile_id == _selectedProfileId ||
                    (string.IsNullOrEmpty(_selectedProfileId) && i == 0))
                {
                    _feedstockList.Select(i);
                    if (string.IsNullOrEmpty(_selectedProfileId)) _selectedProfileId = p.feedstock_profile_id;
                }
            }

            var selected = SelectedProfile();
            if (selected != null)
            {
                float totalEnergy = selected.energy_cost_kwh_per_day * selected.process_duration_days;
                float credit = sys.ComputeOffgasCredit(selected);
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Selection", selected.display_name, AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Energy cost", $"{totalEnergy:F0} kWh over {selected.process_duration_days}d (offgas credits back ≈ {credit:F0} kWh)", AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Expected yield band",
                    $"{selected.liquid_fuel_yield_units}× retort fuel, {selected.light_fraction_yield_units}× fuel litres, {selected.solid_carbon_yield_units}× carbon black",
                    AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Mass check",
                    $"in {selected.input_mass_kg:0.#} kg → out {selected.liquid_mass_kg + selected.light_mass_kg + selected.carbon_mass_kg:0.#} kg + losses {selected.input_mass_kg - selected.liquid_mass_kg - selected.light_mass_kg - selected.carbon_mass_kg:0.#} kg",
                    AshfallUiHelpers.ColorMuted));
            }

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("BAY ACTIONS"));

            var row1 = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            row1.AddChild(MakeAction("START BATCH", "Consumes a full batch of the selected feedstock and daily grid power.",
                () => RunCommand(() => _session!.StartBatch(_selectedProfileId), "Batch charged and lit.")));
            row1.AddChild(MakeAction("CLAIM OUTPUTS", "Moves staged fuel and carbon into storage.",
                () => RunCommand(() => _session!.Claim() == null
                    ? Ashfall.Core.ActionResult.Blocked("pyro.nothing_to_claim", "pyro.nothing_to_claim")
                    : Ashfall.Core.ActionResult.Success("pyro.outputs_claimed"),
                    "Outputs claimed into storage.")));
            row1.AddChild(MakeAction("SERVICE RETORT", $"Costs {sys.Catalog.machine.maintenance_required_items.Values.Sum()}× material. Restores condition.",
                () => RunCommand(() => _session!.Maintain(), "Retort serviced.")));
            _detail.AddChild(row1);
        }

        private void BuildConstructionActions()
        {
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            row.AddChild(MakeAction("CONSTRUCT RETORT BAY", "Consumes the build materials listed above.",
                () => RunCommand(() => _session!.Construct(), "Retort bay constructed.")));
            _detail.AddChild(row);
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
