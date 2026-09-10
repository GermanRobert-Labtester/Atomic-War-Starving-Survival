// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Combat;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 198: tactical CBRN hazard monitor and shelter decon dispatch.
    /// Presentation-only — hazard state comes from the bound
    /// <see cref="ChemWarfareSystem"/>; cleanup commands are emitted via
    /// <see cref="OnActionRequested"/> for the host to resolve. All agent
    /// values are deliberately abstract gameplay figures.
    /// </summary>
    public partial class ChemWarfareDefensePanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private ChemWarfareSystem? _system;
        private ItemList _hazardList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedHazardIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(ChemWarfareSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("ATMOSPHERE WATCH // TOXIC HAZARD MONITOR", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("hazards", "Active Hazards", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("worst", "Worst Density", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("residue", "Residue Incidents", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("agents", "Known Agents", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _hazardList = new ItemList
            {
                CustomMinimumSize = new Vector2(300, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _hazardList.ItemSelected += index => { _selectedHazardIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_hazardList);

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
        public void Close() { Visible = false; OnClose?.Invoke(); }

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            RefreshView();
        }

        private ToxicHazardZoneState? SelectedHazard()
        {
            if (_system == null) return null;
            int idx = _selectedHazardIndex;
            var hazards = _system.State.ActiveHazards;
            if (idx < 0 || idx >= hazards.Count) return null;
            return hazards[idx];
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _hazardList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var hazards = _system.State.ActiveHazards;
            _selectedHazardIndex = Math.Clamp(_selectedHazardIndex, -1, Math.Max(0, hazards.Count - 1));

            // ── Status rail ──
            if (_statusRail != null)
            {
                int worst = 0;
                foreach (var h in hazards)
                    worst = Math.Max(worst, h.DensityPermille);
                _statusRail.Set("hazards", hazards.Count.ToString(),
                    hazards.Count > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("worst", hazards.Count > 0 ? $"{worst / 10.0:0}%" : "—",
                    worst >= 700 ? AshfallMetricCard.Criticality.Critical :
                    worst >= 400 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("residue", _system.State.TotalResidueIncidentsLogged.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("agents", _system.AgentCatalog.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            }

            // ── Hazard list ──
            _hazardList.Clear();
            if (hazards.Count == 0)
            {
                _hazardList.AddItem("Atmosphere clear", null, false);
            }
            else
            {
                for (int i = 0; i < hazards.Count; i++)
                {
                    var h = hazards[i];
                    string agentName = AgentName(h.AgentId);
                    _hazardList.AddItem($"Lane {h.CombatLane + 1} — {agentName} — {h.DensityPermille / 10.0:0}%", null, false);
                    if (i == _selectedHazardIndex)
                        _hazardList.Select(i);
                }
            }

            var selected = SelectedHazard();

            // ── Detail ──
            if (selected == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No toxic hazards are active in any combat lane. Filters can rest; the atmosphere is merely ash and cold.",
                    title: "ATMOSPHERE CLEAR"));
                BuildAgentReference();
                return;
            }

            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader("ACTIVE HAZARD"));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Agent", AgentName(selected.AgentId), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Hazard class", AgentClass(selected.AgentId), AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Combat lane", $"Lane {selected.CombatLane + 1}", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Density", DensityTier(selected.DensityPermille),
                selected.DensityPermille >= 700 ? AshfallUiHelpers.ColorCritical :
                selected.DensityPermille >= 400 ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Dispersing in", $"{selected.RemainingTicks} checks", AshfallUiHelpers.ColorDim));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var deconBtn = AshfallUiHelpers.MakeButton("DISPATCH DECON TEAM", () =>
                OnActionRequested?.Invoke("clear_hazard", selected.HazardId));
            deconBtn.TooltipText = "Orders a masked team to disperse the hazard now, instead of waiting out the wind.";
            row.AddChild(deconBtn);
            _detail.AddChild(row);

            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSmall(
                "Unprotected exposure in a hazard lane causes escalating harm; a sound respirator absorbs most of it. Worn filters let breakthrough through.", autowrap: true));

            BuildAgentReference();
        }

        private void BuildAgentReference()
        {
            if (_system == null || _system.AgentCatalog.Count == 0) return;
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("FIELD AGENT PROFILES (ABSTRACT)"));
            foreach (var kv in _system.AgentCatalog)
            {
                var a = kv.Value;
                _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                    a.DisplayName,
                    $"{a.HazardClass} — severity {a.ExposureSeverity}, filter wear {a.FilterWearPermille / 10.0:0}% per exposure",
                    a.ExposureSeverity >= 3 ? AshfallUiHelpers.ColorCritical : AshfallUiHelpers.ColorText));
            }
        }

        private string AgentName(string agentId) =>
            _system != null && _system.AgentCatalog.TryGetValue(agentId, out var def) && !string.IsNullOrEmpty(def.DisplayName)
                ? def.DisplayName
                : ItemDisplay.Prettify(agentId ?? string.Empty);

        private string AgentClass(string agentId) =>
            _system != null && _system.AgentCatalog.TryGetValue(agentId, out var def)
                ? ItemDisplay.Prettify(def.HazardClass)
                : "unknown";

        private static string DensityTier(int densityPermille) =>
            densityPermille >= 800 ? $"CRITICAL ({densityPermille / 10.0:0}%)" :
            densityPermille >= 600 ? $"HIGH ({densityPermille / 10.0:0}%)" :
            densityPermille >= 300 ? $"MEDIUM ({densityPermille / 10.0:0}%)" :
            $"LOW ({densityPermille / 10.0:0}%)";

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
