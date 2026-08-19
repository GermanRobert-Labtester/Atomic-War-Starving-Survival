using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

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

        // Placeholder events data
        private readonly string[] _placeholderRecent = {
            "[Day 25] Supply cache discovered in Sector 12 — +15 rations",
            "[Day 24] Radio contact with Black Flotilla — trade offer received",
            "[Day 23] Fallout warning issued — all survivors sheltered",
            "[Day 22] Medical emergency — Marcus treated radiation sickness",
            "[Day 21] Expedition returned — 3 new survivors recruited"
        };

        private readonly string[] _placeholderIncidents = {
            "[Day 20] Radiation spike — Sector 4 elevated (1.2 mSv/hr)",
            "[Day 18] Bunker breach attempt — repelled by perimeter guard",
            "[Day 15] Water contamination — filtration system damaged",
            "[Day 12] Ambush in Sector 4 — 1 casualty, supplies lost",
            "[Day 8] Radio interference — unknown signal detected"
        };

        private readonly string[] _placeholderNarrative = {
            "Chapter 1 Complete: The Exchange — Nuclear detonations across the globe",
            "Chapter 2 Complete: Ashfall — Surviving the initial fallout and radiation",
            "Chapter 3 Active: The Bunker — Establishing shelter and community",
            "Chapter 4 Pending: First Contact — Encountering other survivors",
            "Chapter 5 Pending: The Long Winter — Nuclear winter conditions setting in"
        };

        // Real data from host session
        // private EventsHostSession? _eventsHost;

        public void Bind(object events) // placeholder for EventsHostSession
        {
            // _eventsHost = (EventsHostSession)events;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_recentList == null || _incidentsList == null || _narrativeList == null) return;

            // Clear existing lists
            AshfallUiHelpers.EmptyChildren(_recentList);
            AshfallUiHelpers.EmptyChildren(_incidentsList);
            AshfallUiHelpers.EmptyChildren(_narrativeList);

            // Display placeholder recent events
            foreach (string eventEntry in _placeholderRecent)
            {
                var label = new Label { Text = eventEntry };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _recentList.AddChild(label);
            }

            // Display placeholder incidents
            foreach (string incident in _placeholderIncidents)
            {
                var label = new Label { Text = incident };
                label.CustomMinimumSize = new Vector2(400, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _incidentsList.AddChild(label);
            }

            // Display placeholder narrative progression
            foreach (string narrative in _placeholderNarrative)
            {
                var label = new Label { Text = narrative };
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
