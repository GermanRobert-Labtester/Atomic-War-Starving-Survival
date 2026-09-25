// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Godot;
using Ashfall.Core.Narrative;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plans 190–193: shelter tribunal workflow.
    /// Presentation-only — crime reports are emitted via
    /// <see cref="OnActionRequested"/> and resolved by the host through the
    /// bound <see cref="JusticeSystem"/>; trials, verdicts and punishments
    /// advance through the Core day tick and its law catalog.
    /// </summary>
    public partial class JusticeTribunalPanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private JusticeSystem? _system;
        private ItemList _incidentList = null!;
        private VBoxContainer _detail = null!;
        private OptionButton _crimeSelect = null!;
        private int _selectedIncidentIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(JusticeSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("THE RECORD // SHELTER TRIBUNAL", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("open", "Open Cases", "—", AshfallMetricCard.Criticality.Normal, minWidth: 100);
            _statusRail.AddCard("prisoners", "Held", "—", AshfallMetricCard.Criticality.Normal, minWidth: 90);
            _statusRail.AddCard("pressure", "Vigilante Mood", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("laws", "Laws on Record", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _incidentList = new ItemList
            {
                CustomMinimumSize = new Vector2(300, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _incidentList.ItemSelected += index => { _selectedIncidentIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_incidentList);

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

        /// <summary>Last feedback line rendered by the panel (test/diagnostic surface).</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<CrimeIncident> SortedIncidents()
        {
            if (_system == null) return new List<CrimeIncident>();
            return _system.State.incidents
                .OrderBy(i => i.status == IncidentStatus.Unresolved ? 0 : 1)
                .ThenBy(i => i.incidentId, StringComparer.Ordinal)
                .ToList();
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _incidentList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var incidents = SortedIncidents();
            _selectedIncidentIndex = Math.Clamp(_selectedIncidentIndex, -1, Math.Max(0, incidents.Count - 1));

            int open = incidents.Count(i => i.status == IncidentStatus.Unresolved);

            if (_statusRail != null)
            {
                _statusRail.Set("open", open.ToString(), open > 0 ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("prisoners", _system.State.imprisonedSurvivorIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
                float pressure = _system.State.vigilantePressure;
                _statusRail.Set("pressure", pressure >= 60f ? "HIGH" : pressure >= 30f ? "rising" : "calm",
                    pressure >= 60f ? AshfallMetricCard.Criticality.Critical :
                    pressure >= 30f ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("laws", _system.Laws.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            }

            _incidentList.Clear();
            foreach (var inc in incidents)
            {
                string status = inc.status == IncidentStatus.Unresolved ? "OPEN" : inc.status.ToString().ToUpperInvariant();
                _incidentList.AddItem($"{CrimeText(inc.crimeType)} — {status} (day {inc.day})", null, false);
            }
            if (incidents.Count == 0)
                _incidentList.AddItem("No cases on record", null, false);

            var selected = _selectedIncidentIndex >= 0 && _selectedIncidentIndex < incidents.Count
                ? incidents[_selectedIncidentIndex] : null;

            // ── Detail ──
            if (selected != null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSectionHeader($"CASE: {CrimeText(selected.crimeType).ToUpperInvariant()}"));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Accused", ItemDisplay.Prettify(selected.accusedSurvivorId), AshfallUiHelpers.ColorText));
                if (!string.IsNullOrEmpty(selected.victimSurvivorId))
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow("Victim", ItemDisplay.Prettify(selected.victimSurvivorId), AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Opened", $"day {selected.day}", AshfallUiHelpers.ColorDim));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Status", selected.status == IncidentStatus.Unresolved ? "OPEN — awaiting tribunal day" : selected.status.ToString(), AshfallUiHelpers.ColorText));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Evidence on file", $"{selected.evidenceClues.Count}", AshfallUiHelpers.ColorText));
                if (selected.verdict.HasValue)
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow("Verdict", selected.verdict.Value.ToString(),
                        selected.verdict.Value == TrialVerdict.Guilty ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorText));
                if (selected.punishment.HasValue)
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow("Sentence", selected.punishment.Value.ToString(), AshfallUiHelpers.ColorWarning));
                if (selected.evidenceClues.Count > 0)
                {
                    foreach (var clue in selected.evidenceClues.Take(4))
                        _detail.AddChild(AshfallUiHelpers.MakeMetadata($"• {clue.description}"));
                }
            }
            else
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No cases stand before the record. Laws exist so that hunger does not decide guilt alone.",
                    title: "DOCKET CLEAR"));
                if (_system.Laws.Count > 0)
                {
                    _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("LAWS ON RECORD"));
                    foreach (var law in _system.Laws.Values.Take(6))
                        _detail.AddChild(AshfallUiHelpers.MakeDataRow(law.display_name, $"applies to {law.crime_type}", AshfallUiHelpers.ColorDim));
                }
            }

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("REPORT A CRIME"));
            _crimeSelect = new OptionButton { CustomMinimumSize = new Vector2(0, 26) };
            string[] crimes = { "Theft", "Assault", "Hoarding", "Sabotage", "Desertion" };
            foreach (var c in crimes) _crimeSelect.AddItem(c);
            _detail.AddChild(_crimeSelect);

            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);
            var reportBtn = AshfallUiHelpers.MakeButton("FILE REPORT (ACCUSED SELECTED CASE SUBJECT)", () =>
            {
                int i = _crimeSelect.Selected;
                string crime = i >= 0 && i < crimes.Length ? crimes[i] : "Theft";
                string accused = selected?.accusedSurvivorId ?? string.Empty;
                OnActionRequested?.Invoke("report", $"{accused}:{crime}");
            });
            reportBtn.TooltipText = "Brings the accused before the record. The tribunal weighs evidence on its day.";
            row.AddChild(reportBtn);
            _detail.AddChild(row);
        }

        private static string CrimeText(CrimeType crime) => crime switch
        {
            CrimeType.Theft => "Theft",
            CrimeType.Assault => "Assault",
            CrimeType.Murder => "Murder",
            CrimeType.Hoarding => "Hoarding",
            CrimeType.Sabotage => "Sabotage",
            CrimeType.Desertion => "Desertion",
            _ => crime.ToString()
        };

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
