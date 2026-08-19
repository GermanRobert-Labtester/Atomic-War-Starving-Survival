using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Event Detail panel.
    /// Shows detailed event information, event history, and narrative progression.
    /// </summary>
    public partial class EventDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblEventInfoTitle;
        private VBoxContainer _eventInfo;
        private Label _lblHistoryTitle;
        private VBoxContainer _eventHistory;
        private Label _lblNarrativeTitle;
        private VBoxContainer _narrativeProgress;

        private readonly string[] _placeholderEventInfo = {
            "Event: Supply Cache Discovery",
            "Type: Resource Event",
            "Status: Completed",
            "Date: Day 25",
            "Location: Sector 12",
            "Impact: +15 rations, +3 medicine"
        };

        private readonly string[] _placeholderHistory = {
            "[Day 25] Supply cache discovered — +15 rations, +3 medicine",
            "[Day 24] Radio contact with Black Flotilla — Trade offer",
            "[Day 23] Fallout warning — All survivors sheltered",
            "[Day 22] Medical emergency — Marcus treated radiation",
            "[Day 21] Expedition returned — 3 new survivors recruited"
        };

        private readonly string[] _placeholderNarrative = {
            "Day 21-25: Survival and Expansion Phase",
            "Focus: Resource acquisition and community growth",
            "Key Events: Supply discovery, radio contact, medical emergency",
            "Outcome: Community expanded to 8 survivors",
            "Next Phase: Exploration and diplomacy",
            "Narrative Arc: Building resilience in the wasteland"
        };

        public void Bind(object eventDetail)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_eventInfo == null || _eventHistory == null || _narrativeProgress == null) return;

            AshfallUiHelpers.EmptyChildren(_eventInfo);
            AshfallUiHelpers.EmptyChildren(_eventHistory);
            AshfallUiHelpers.EmptyChildren(_narrativeProgress);

            foreach (string info in _placeholderEventInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _eventInfo.AddChild(label);
            }

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _eventHistory.AddChild(label);
            }

            foreach (string narrative in _placeholderNarrative)
            {
                var label = new Label { Text = narrative };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _narrativeProgress.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("EVENT DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEventInfoTitle = AshfallUiHelpers.MakeSectionHeader("EVENT INFORMATION");
            vbox.AddChild(_lblEventInfoTitle);

            _eventInfo = new VBoxContainer();
            _eventInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_eventInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("EVENT HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _eventHistory = new VBoxContainer();
            _eventHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _eventHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_eventHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblNarrativeTitle = AshfallUiHelpers.MakeSectionHeader("NARRATIVE PROGRESSION");
            vbox.AddChild(_lblNarrativeTitle);

            _narrativeProgress = new VBoxContainer();
            _narrativeProgress.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _narrativeProgress.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_narrativeProgress);

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
