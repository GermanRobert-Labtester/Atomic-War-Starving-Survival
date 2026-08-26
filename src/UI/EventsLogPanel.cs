using System;
using System.Linq;
#pragma warning disable CS8618
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using AtomicWar.GodotApp.Host;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Events Log panel.
    /// Shows detailed event history, incident reports, and narrative progression.
    /// </summary>
    public partial class EventsLogPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblRecentTitle;
        private VBoxContainer _recentList;
        private Label _lblIncidentsTitle;
        private VBoxContainer _incidentsList;
        private Label _lblNarrativeTitle;
        private VBoxContainer _narrativeList;

        // Real data from host session
        private EventsHostSession? _eventsHost;

        public void Bind(object events)
        {
            _eventsHost = (EventsHostSession)events;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_recentList == null || _incidentsList == null || _narrativeList == null || _eventsHost == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_recentList);
            AshfallUiHelpers.EmptyChildren(_incidentsList);
            AshfallUiHelpers.EmptyChildren(_narrativeList);

            // Fetch and display recent events
            var recentEvents = _eventsHost.GetRecentEvents()
                .OrderByDescending(e => e.Day)
                .Take(5);
            foreach (var eventEntry in recentEvents)
            {
                var label = new Label { Text = $"[Day {eventEntry.Day}] {eventEntry.Description}" };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _recentList.AddChild(label);
            }

            // Fetch and display incidents
            var incidents = _eventsHost.GetIncidents()
                .OrderByDescending(i => i.Day)
                .Take(5);
            foreach (var incident in incidents)
            {
                var label = new Label { Text = $"[Day {incident.Day}] {incident.Description}" };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _incidentsList.AddChild(label);
            }

            // Fetch and display narrative progression
            var narrativeProgression = _eventsHost.GetNarrativeProgression()
                .OrderBy(n => n.Order);
            foreach (var narrative in narrativeProgression)
            {
                var label = new Label { Text = narrative.Description };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _narrativeList.AddChild(label);
            }
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

            var title = AshfallUiHelpers.MakeTitle("EVENTS LOG", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Recent events section
            _lblRecentTitle = AshfallUiHelpers.MakeSectionHeader("RECENT EVENTS");
            vbox.AddChild(_lblRecentTitle);

            _recentList = new VBoxContainer();
            _recentList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _recentList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_recentList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Incidents section
            _lblIncidentsTitle = AshfallUiHelpers.MakeSectionHeader("INCIDENTS & ALERTS");
            vbox.AddChild(_lblIncidentsTitle);

            _incidentsList = new VBoxContainer();
            _incidentsList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _incidentsList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_incidentsList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Narrative progression section
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

            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
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
