// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Crafting;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 201: pre-war automaton workshop.
    /// Presentation-only — reactivation, directive programming and repair are
    /// emitted via <see cref="OnActionRequested"/>; the host resolves them
    /// against the bound <see cref="RoboticsSystem"/> and the inventory
    /// authority. Rogue units are reported, never fixed from the console.
    /// </summary>
    public partial class RoboticsWorkshopPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private RoboticsSystem? _system;
        private ItemList _unitList = null!;
        private VBoxContainer _detail = null!;
        private OptionButton _directiveSelect = null!;
        private int _selectedUnitIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;
        private string _mode = "units"; // units | catalog

        public bool IsBound => _system != null;

        public void Bind(RoboticsSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("AWAKENED STEEL // ROBOTICS WORKSHOP", minWidth: 1050, minHeight: 680);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("units", "Units Active", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("rogue", "Rogue Units", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("reactivated", "Total Raised", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("rogueEvents", "Rogue Events", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _unitList = new ItemList
            {
                CustomMinimumSize = new Vector2(300, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _unitList.ItemSelected += OnUnitSelected;

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_unitList);

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

        private void OnUnitSelected(long index)
        {
            _selectedUnitIndex = (int)index;
            RefreshView();
        }

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private RobotUnitState? SelectedUnit()
        {
            if (_system == null) return null;
            int idx = _selectedUnitIndex;
            var units = _system.Units;
            if (_mode == "catalog" || idx < 0 || idx >= units.Count) return null;
            return units[idx];
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _unitList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var units = _system.Units;
            _selectedUnitIndex = Math.Clamp(_selectedUnitIndex, -1, Math.Max(0, units.Count - 1));

            // ── Status rail ──
            if (_statusRail != null)
            {
                int rogue = 0;
                foreach (var u in units) if (u.IsRogue) rogue++;
                _statusRail.Set("units", units.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("rogue", rogue.ToString(),
                    rogue > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("reactivated", _system.State.TotalUnitsReactivated.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("rogueEvents", _system.State.TotalRogueEventsTriggered.ToString(),
                    _system.State.TotalRogueEventsTriggered > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            }

            // ── Unit list ──
            _unitList.Clear();
            if (units.Count == 0)
                _unitList.AddItem("No units reactivated", null, false);
            for (int i = 0; i < units.Count; i++)
            {
                var u = units[i];
                string defName = DefinitionName(u.DefinitionId);
                string status = u.IsRogue ? "ROGUE" : u.IsEmpDisabled ? "EMP-DISABLED" : DirectiveDisplay.Name(u.AssignedDirective);
                _unitList.AddItem($"{defName} — {status}", null, false);
                if (i == _selectedUnitIndex)
                    _unitList.Select(i);
            }

            var selected = _selectedUnitIndex >= 0 && _selectedUnitIndex < units.Count
                ? units[_selectedUnitIndex] : null;

            // ── Detail ──
            if (selected == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No machines are awake. Dormant units can be raised in the workshop when their cores and parts are found.",
                    title: "NO UNITS ACTIVE"));
                BuildReactivationSection();
                return;
            }

            var def = _system.RobotCatalog.TryGetValue(selected.DefinitionId, out var d) ? d : null;

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(DefinitionName(selected.DefinitionId).ToUpperInvariant()));
            if (def != null)
                _detail.AddChild(AshfallUiHelpers.MakeMetadata(def.Description));

            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Role", ItemDisplay.Prettify(def?.Role ?? "utility"), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Chassis integrity", $"{selected.ChassisIntegrity} / {def?.MaxChassisIntegrity ?? 1000}",
                selected.ChassisIntegrity < 300 ? AshfallUiHelpers.ColorCritical :
                selected.ChassisIntegrity < 700 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Logic integrity", $"{selected.LogicIntegrity} / 1000",
                selected.LogicIntegrity < 300 ? AshfallUiHelpers.ColorCritical :
                selected.LogicIntegrity < 600 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Core charge", $"{selected.CoreChargeWh} / {def?.MaxCoreChargeWh ?? 0} Wh",
                selected.CoreChargeWh <= 0 ? AshfallUiHelpers.ColorCritical :
                selected.CoreChargeWh < (def?.MaxCoreChargeWh ?? 1) * 0.25f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));

            string directiveText = selected.IsRogue ? "REJECTED — ROGUE" :
                selected.IsEmpDisabled ? $"EMP-DISABLED ({selected.EmpDisableHoursRemaining} h left)" :
                DirectiveDisplay.Name(selected.AssignedDirective);
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Directive", directiveText,
                selected.IsRogue || selected.IsEmpDisabled ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorText));

            if (def != null && def.CompatibleTasks.Count > 0)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Capable tasks", string.Join(", ", ToDisplayList(def.CompatibleTasks)), AshfallUiHelpers.ColorDim));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("UNIT ACTIONS"));

            if (selected.IsRogue)
            {
                _detail.AddChild(AshfallUiHelpers.MakeWarning(
                    "The logic core is corrupted. It ignores directives and cannot be reprogrammed from this console."));
            }
            else if (selected.IsEmpDisabled)
            {
                _detail.AddChild(AshfallUiHelpers.MakeWarning(
                    $"The unit is locked up by electromagnetic disturbance — {selected.EmpDisableHoursRemaining} hours remaining."));
            }
            else
            {
                _directiveSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
                foreach (var dir in DirectiveDisplay.KnownDirectives)
                    _directiveSelect.AddItem(dir.display);
                for (int i = 0; i < DirectiveDisplay.KnownDirectives.Count; i++)
                {
                    if (string.Equals(DirectiveDisplay.KnownDirectives[i].id, selected.AssignedDirective, StringComparison.Ordinal))
                        _directiveSelect.Select(i);
                }
                _detail.AddChild(_directiveSelect);

                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var programBtn = AshfallUiHelpers.MakeButton("PROGRAM DIRECTIVE", () =>
                {
                    int i = _directiveSelect.Selected;
                    string directiveId = i >= 0 && i < DirectiveDisplay.KnownDirectives.Count
                        ? DirectiveDisplay.KnownDirectives[i].id : "directive_idle";
                    OnActionRequested?.Invoke("program", $"{selected.UnitId}:{directiveId}");
                });
                programBtn.TooltipText = "Rewrites the unit's standing order. Fragile logic cores and careless programmers break things.";
                row.AddChild(programBtn);

                var repairBtn = AshfallUiHelpers.MakeButton("REPAIR CHASSIS", () =>
                    OnActionRequested?.Invoke("repair", selected.UnitId));
                repairBtn.TooltipText = "Costs 2 scrap metal. Restores 250 chassis integrity.";
                row.AddChild(repairBtn);
                _detail.AddChild(row);
            }

            BuildReactivationSection();
        }

        private void BuildReactivationSection()
        {
            if (_system == null || _system.RobotCatalog.Count == 0) return;
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("REACTIVATE DORMANT UNIT"));

            foreach (var kv in _system.RobotCatalog)
            {
                var def = kv.Value;
                var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
                var btn = AshfallUiHelpers.MakeButton($"RAISE {def.DisplayName.ToUpperInvariant()}", () =>
                    OnActionRequested?.Invoke("reactivate", def.Id));
                btn.TooltipText = DescribeMaterials(def);
                row.AddChild(btn);

                string cost = DescribeMaterials(def);
                row.AddChild(AshfallUiHelpers.MakeMetadata(cost));
                _detail.AddChild(row);
            }
        }

        private static string DescribeMaterials(RobotDefinition def)
        {
            if (def.ReactivationMaterials.Count == 0) return "No parts required.";
            var parts = new List<string>();
            foreach (var m in def.ReactivationMaterials)
                parts.Add($"{m.Quantity}× {ItemDisplay.Name(m.ItemId)}");
            return "Requires " + string.Join(", ", parts) + ".";
        }

        private static List<string> ToDisplayList(List<string> tasks)
        {
            var result = new List<string>();
            foreach (var t in tasks) result.Add(ItemDisplay.Prettify(t));
            return result;
        }

        private string DefinitionName(string definitionId) =>
            _system != null && _system.RobotCatalog.TryGetValue(definitionId, out var def) && !string.IsNullOrEmpty(def.DisplayName)
                ? def.DisplayName
                : ItemDisplay.Prettify(definitionId ?? string.Empty);

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
