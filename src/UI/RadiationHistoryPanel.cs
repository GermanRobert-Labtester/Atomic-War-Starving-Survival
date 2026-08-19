using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radiation History panel.
    /// Shows detailed radiation exposure history, cumulative dosimetry, and radiation events timeline.
    /// </summary>
    public partial class RadiationHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _historyData;
        private Label _lblCumulativeTitle;
        private VBoxContainer _cumulativeData;
        private Label _lblEventsTitle;
        private VBoxContainer _radiationEvents;

        private readonly string[] _placeholderHistory = {
            "[Day 1] Initial exposure: 0.5 mSv (Low)",
            "[Day 5] Fallout event: +2.5 mSv cumulative",
            "[Day 10] Routine check: 5.2 mSv total",
            "[Day 15] Radiation spike: +3.8 mSv",
            "[Day 20] Medical treatment: Reduced exposure",
            "[Day 25] Current: 12.4 mSv cumulative (Low risk)"
        };

        private readonly string[] _placeholderCumulative = {
            "Total Exposure: 12.4 mSv (Low risk)",
            "Daily Average: 0.5 mSv/day",
            "Peak Daily: 3.8 mSv (Day 15)",
            "Weekly Average: 3.2 mSv/week",
            "Monthly Projection: 15.6 mSv (Safe limit)",
            "Annual Projection: 182.5 mSv (Above safe limit)",
            "Safe Limit: 100 mSv/year (Public)",
            "Occupational Limit: 500 mSv/year"
        };

        private readonly string[] _placeholderEvents = {
            "[Day 5] Fallout storm passed — +2.5 mSv",
            "[Day 12] Radiation monitoring calibrated",
            "[Day 15] Radiation spike detected — Sector 4",
            "[Day 18] Dosimeter recalibrated",
            "[Day 22] Fallout warning issued",
            "[Day 25] Levels stabilizing — Normal operations"
        };

        public void Bind(object radiationHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_historyData == null || _cumulativeData == null || _radiationEvents == null) return;

            AshfallUiHelpers.EmptyChildren(_historyData);
            AshfallUiHelpers.EmptyChildren(_cumulativeData);
            AshfallUiHelpers.EmptyChildren(_radiationEvents);

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _historyData.AddChild(label);
            }

            foreach (string cumulative in _placeholderCumulative)
            {
                var label = new Label { Text = cumulative };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _cumulativeData.AddChild(label);
            }

            foreach (string eventEntry in _placeholderEvents)
            {
                var label = new Label { Text = eventEntry };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _radiationEvents.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("RADIATION HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("EXPOSURE HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _historyData = new VBoxContainer();
            _historyData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _historyData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_historyData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblCumulativeTitle = AshfallUiHelpers.MakeSectionHeader("CUMULATIVE DOSIMETRY");
            vbox.AddChild(_lblCumulativeTitle);

            _cumulativeData = new VBoxContainer();
            _cumulativeData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _cumulativeData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_cumulativeData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblEventsTitle = AshfallUiHelpers.MakeSectionHeader("RADIATION EVENTS");
            vbox.AddChild(_lblEventsTitle);

            _radiationEvents = new VBoxContainer();
            _radiationEvents.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _radiationEvents.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_radiationEvents);

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
