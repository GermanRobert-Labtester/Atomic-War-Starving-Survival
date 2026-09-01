using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.Host;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Event Detail panel.
    /// Shows recent events, incidents, and narrative progression — bound to
    /// the live EventsHostSession. Unbound renders an honest empty state.
    /// </summary>
    public partial class EventDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblEventInfoTitle;
        private VBoxContainer _eventInfoList;
        private Label _lblHistoryTitle;
        private VBoxContainer _historyList;
        private Label _lblNarrativeTitle;
        private VBoxContainer _narrativeList;

        private EventsHostSession? _events;

        public bool IsBound => _events != null;
        public int RenderedRowCount { get; private set; }

        public void Bind(EventsHostSession? events)
        {
            _events = events;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_eventInfoList == null || _historyList == null || _narrativeList == null) return;

            AshfallUiHelpers.EmptyChildren(_eventInfoList);
            AshfallUiHelpers.EmptyChildren(_historyList);
            AshfallUiHelpers.EmptyChildren(_narrativeList);

            RenderedRowCount = 0;

            if (_events == null)
            {
                _eventInfoList.AddChild(MakeDimLine("No events session bound."));
                return;
            }

            var recent = _events.GetRecentEvents();
            foreach (var ev in recent)
            {
                AddRow(_eventInfoList, $"[Day {ev.Day}] {ev.Description}", Ashfall.Core.UI.Theme.Pale);
                RenderedRowCount++;
            }
            if (recent.Count == 0)
                _eventInfoList.AddChild(MakeDimLine("No recent events."));

            var incidents = _events.GetIncidents();
            foreach (var inc in incidents)
            {
                AddRow(_historyList, $"[Day {inc.Day}] {inc.Description}", Ashfall.Core.UI.Theme.Warm);
                RenderedRowCount++;
            }
            if (incidents.Count == 0)
                _historyList.AddChild(MakeDimLine("No incidents logged."));

            var narrative = _events.GetNarrativeProgression();
            foreach (var nar in narrative)
            {
                AddRow(_narrativeList, $"[Order {nar.Order}] {nar.Description}", Ashfall.Core.UI.Theme.Lethe);
                RenderedRowCount++;
            }
            if (narrative.Count == 0)
                _narrativeList.AddChild(MakeDimLine("No narrative progression logged."));
        }

        private void AddRow(VBoxContainer parent, string text, (float r, float g, float b, float a) col)
        {
            var label = new Label { Text = text };
            label.CustomMinimumSize = new Vector2(400, 0);
            label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(col));
            parent.AddChild(label);
        }

        private Label MakeDimLine(string text)
        {
            var l = new Label { Text = text };
            l.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            l.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            return l;
        }

        public override void _Ready()
        {
            // Ticket #125: layout chrome owned by res://assets/ui/panels/EventDetailPanel.tscn; SceneBinder resolves typed unique-name nodes once.
            // Sibling refresh code is unchanged.
            var binder = new SceneBinder(this, typeof(EventDetailPanel));
            binder.Require<VBoxContainer>("EventInfoList");
            binder.Require<VBoxContainer>("HistoryList");
            binder.Require<VBoxContainer>("NarrativeList");
            binder.Require<Button>("CloseButton");
            _eventInfoList = binder.Get<VBoxContainer>("EventInfoList");
            _historyList = binder.Get<VBoxContainer>("HistoryList");
            _narrativeList = binder.Get<VBoxContainer>("NarrativeList");
            binder.Get<Button>("CloseButton").Pressed += () => OnClose?.Invoke();

            Visible = false;
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
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
