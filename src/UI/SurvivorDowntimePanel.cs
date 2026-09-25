// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using Godot;
using Ashfall.Core.Recreation;
using Ashfall.Core.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Plan 196: survivor hobbies and downtime.
    /// Presentation-only — session commands are emitted via
    /// <see cref="OnActionRequested"/>; the host resolves them against the
    /// bound <see cref="SurvivorDowntimeSystem"/>, the canonical roster
    /// (participants) and the inventory authority.
    /// </summary>
    public partial class SurvivorDowntimePanel : Control, IBindablePanel
    {
        public event Action? OnClose;
        public event Action<string, string>? OnActionRequested;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private SurvivorDowntimeSystem? _system;
        private ItemList _hobbyList = null!;
        private VBoxContainer _detail = null!;
        private int _selectedHobbyIndex = -1;
        private string _feedbackText = string.Empty;
        private bool _feedbackIsFailure;

        public bool IsBound => _system != null;

        public void Bind(SurvivorDowntimeSystem system) { _system = system; _feedbackText = string.Empty; RefreshView(); }
        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("COMMON HOURS // HOBBIES & DOWNTIME", minWidth: 1000, minHeight: 650);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Active Sessions", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("held", "Sessions Held", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);
            _statusRail.AddCard("skills", "Practiced Hands", "—", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("hobbies", "Hobbies Known", "—", AshfallMetricCard.Criticality.Normal, minWidth: 110);

            _hobbyList = new ItemList
            {
                CustomMinimumSize = new Vector2(290, 0),
                SizeFlagsVertical = SizeFlags.ExpandFill,
                SizeFlagsHorizontal = SizeFlags.Fill
            };
            _hobbyList.ItemSelected += index => { _selectedHobbyIndex = (int)index; RefreshView(); };

            _detail = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingSm);

            var bodyRow = new HBoxContainer();
            bodyRow.AddChild(_hobbyList);

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

        /// <summary>Host feedback strip — tied to the actual command result.</summary>
        public string LastFeedback { get; private set; } = string.Empty;

        public void ShowFeedback(string message, bool isFailure)
        {
            _feedbackText = message;
            _feedbackIsFailure = isFailure;
            LastFeedback = message;
            RefreshView();
        }

        private List<HobbyDef> SortedHobbies()
        {
            var defs = new List<HobbyDef>();
            if (_system == null) return defs;
            foreach (var kv in _system.GetHobbyCatalog()) defs.Add(kv.Value);
            defs.Sort((a, b) => string.CompareOrdinal(a.hobby_id, b.hobby_id));
            return defs;
        }

        public void RefreshView()
        {
            if (_system == null || _detail == null || _hobbyList == null) return;
            AshfallUiHelpers.EmptyChildren(_detail);

            var hobbies = SortedHobbies();
            _selectedHobbyIndex = Math.Clamp(_selectedHobbyIndex, -1, Math.Max(0, hobbies.Count - 1));

            // ── Status rail ──
            if (_statusRail != null)
            {
                _statusRail.Set("active", _system.State.activeSessions.Count.ToString(),
                    _system.State.activeSessions.Count > 0 ? AshfallMetricCard.Criticality.Normal : AshfallMetricCard.Criticality.Normal);
                int held = 0;
                foreach (var p in _system.State.profiles) held += p.totalSessionsCompleted;
                _statusRail.Set("held", held.ToString(), AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("skills", _system.State.profiles.Count > 0 ? $"{_system.State.profiles.Count}" : "—", AshfallMetricCard.Criticality.Normal);
                _statusRail.Set("hobbies", hobbies.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            }

            // ── Hobby list ──
            _hobbyList.Clear();
            for (int i = 0; i < hobbies.Count; i++)
            {
                string label = hobbies[i].display_name;
                int active = CountActiveFor(hobbies[i].hobby_id);
                if (active > 0) label += $" — {active} underway";
                _hobbyList.AddItem(label, null, false);
                if (i == _selectedHobbyIndex)
                    _hobbyList.Select(i);
            }

            HobbyDef? def = _selectedHobbyIndex >= 0 && _selectedHobbyIndex < hobbies.Count
                ? hobbies[_selectedHobbyIndex] : null;

            // ── Active sessions ──
            if (_system.State.activeSessions.Count > 0)
            {
                _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIVE SESSIONS"));
                foreach (var s in _system.State.activeSessions)
                {
                    var hobby = _system.GetHobby(s.hobbyId);
                    _detail.AddChild(AshfallUiHelpers.MakeDataRow(
                        hobby?.display_name ?? ItemDisplay.Prettify(s.hobbyId),
                        s.isFinished ? "finishing" : $"day {s.startedDay}",
                        AshfallUiHelpers.ColorSuccess));
                }
            }

            if (def == null)
            {
                _detail.AddChild(AshfallUiHelpers.MakeEmptyState(
                    "No pastimes are recorded for the shelter. A carved figurine, a card deck, an old song — small things that keep people human.",
                    title: "NO HOBBIES KNOWN"));
                return;
            }

            // ── Detail ──
            _detail.AddChild(AshfallUiHelpers.MakeSectionHeader(def.display_name.ToUpperInvariant()));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Duration", $"{def.duration_hours} h", AshfallUiHelpers.ColorText));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Stress relief", $"≈ {def.base_stress_relief:0} per session", AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Morale effect", $"+{def.morale_effect}", AshfallUiHelpers.ColorSuccess));
            _detail.AddChild(AshfallUiHelpers.MakeDataRow("Company", def.social_min == def.social_max
                ? (def.social_min == 1 ? "solitary" : $"{def.social_min} people")
                : $"{def.social_min}–{def.social_max} people", AshfallUiHelpers.ColorText));

            if (def.required_item_ids.Count > 0)
            {
                var names = new List<string>();
                foreach (var itemId in def.required_item_ids) names.Add(ItemDisplay.Name(itemId));
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Needs", string.Join(", ", names), AshfallUiHelpers.ColorText));
            }
            if (!string.IsNullOrEmpty(def.output_item_id))
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("May produce", ItemDisplay.Name(def.output_item_id), AshfallUiHelpers.ColorInfo));
            if (def.brawl_risk > 0f)
                _detail.AddChild(AshfallUiHelpers.MakeDataRow("Temper risk", def.brawl_risk >= 0.05f ? "real" : "slight",
                    def.brawl_risk >= 0.05f ? AshfallUiHelpers.ColorWarning : AshfallUiHelpers.ColorDim));

            if (!string.IsNullOrEmpty(_feedbackText))
            {
                _detail.AddChild(AshfallUiHelpers.MakeSeparator());
                _detail.AddChild(_feedbackIsFailure
                    ? AshfallUiHelpers.MakeWarning(_feedbackText)
                    : AshfallUiHelpers.MakeSuccess(_feedbackText));
            }

            // ── Actions ──
            _detail.AddChild(AshfallUiHelpers.MakeSeparator());
            _detail.AddChild(AshfallUiHelpers.MakeSubsectionHeader("ACTIONS"));
            var row = AshfallUiHelpers.MakeHBox(DesignTheme.SpacingSm);

            bool hasActive = CountActiveFor(def.hobby_id) > 0;
            if (!hasActive)
            {
                var startBtn = AshfallUiHelpers.MakeButton("START SESSION", () =>
                    OnActionRequested?.Invoke("start", def.hobby_id));
                startBtn.TooltipText = "Gathers available hands and begins the pastime. Completes at the end of the day.";
                row.AddChild(startBtn);
            }
            else
            {
                var note = AshfallUiHelpers.MakeMetadata("Session underway — it completes at the end of the day.");
                row.AddChild(note);
            }

            if (row.GetChildCount() == 0)
                row.AddChild(AshfallUiHelpers.MakeMetadata("Nothing to do right now."));
            _detail.AddChild(row);
        }

        private int CountActiveFor(string hobbyId)
        {
            if (_system == null) return 0;
            int n = 0;
            foreach (var s in _system.State.activeSessions)
                if (string.Equals(s.hobbyId, hobbyId, StringComparison.Ordinal)) n++;
            return n;
        }

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
