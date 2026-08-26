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
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("EVENT DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEventInfoTitle = AshfallUiHelpers.MakeSectionHeader("RECENT EVENTS");
            vbox.AddChild(_lblEventInfoTitle);
            _eventInfoList = new VBoxContainer();
            _eventInfoList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventInfoList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_eventInfoList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("INCIDENTS LOG");
            vbox.AddChild(_lblHistoryTitle);
            _historyList = new VBoxContainer();
            _historyList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _historyList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_historyList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNarrativeTitle = AshfallUiHelpers.MakeSectionHeader("NARRATIVE PROGRESSION");
            vbox.AddChild(_lblNarrativeTitle);
            _narrativeList = new VBoxContainer();
            _narrativeList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _narrativeList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_narrativeList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);
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
