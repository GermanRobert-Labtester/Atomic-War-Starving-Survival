using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Weather History panel.
    /// Shows detailed weather history, historical weather patterns, and weather anomalies.
    /// </summary>
    public partial class WeatherHistoryPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblHistoryTitle;
        private VBoxContainer _weatherHistory;
        private Label _lblPatternsTitle;
        private VBoxContainer _weatherPatterns;
        private Label _lblAnomaliesTitle;
        private VBoxContainer _weatherAnomalies;

        private readonly string[] _placeholderHistory = {
            "[Day 1-5] Initial fallout period — High radiation, dust storms",
            "[Day 6-10] Nuclear winter onset — Temperature dropping, snow",
            "[Day 11-15] Stabilization — Radiation levels decreasing",
            "[Day 16-20] Mild period — Clear skies, moderate temperatures",
            "[Day 21-25] Current period — Nuclear winter persisting"
        };

        private readonly string[] _placeholderPatterns = {
            "Seasonal Pattern: Nuclear winter conditions",
            "Temperature Trend: Cooling (-5°C to -12°C)",
            "Precipitation Pattern: Low (Nuclear winter)",
            "Wind Pattern: Western flow, storm cycles",
            "Radiation Trend: Decreasing (Post-fallout)",
            "Historical Average: Similar to Day 15-20"
        };

        private readonly string[] _placeholderAnomalies = {
            "Day 8: Unusual radiation spike — Sector 4",
            "Day 12: Extended dust storm — 3 days duration",
            "Day 18: Temperature anomaly — +5°C above average",
            "Day 22: Unknown signal detected — Radio interference",
            "Day 25: Radiation stabilization — Normal operations"
        };

        public void Bind(object weatherHistory)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_weatherHistory == null || _weatherPatterns == null || _weatherAnomalies == null) return;

            AshfallUiHelpers.EmptyChildren(_weatherHistory);
            AshfallUiHelpers.EmptyChildren(_weatherPatterns);
            AshfallUiHelpers.EmptyChildren(_weatherAnomalies);

            foreach (string history in _placeholderHistory)
            {
                var label = new Label { Text = history };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _weatherHistory.AddChild(label);
            }

            foreach (string pattern in _placeholderPatterns)
            {
                var label = new Label { Text = pattern };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _weatherPatterns.AddChild(label);
            }

            foreach (string anomaly in _placeholderAnomalies)
            {
                var label = new Label { Text = anomaly };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
                _weatherAnomalies.AddChild(label);
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

            var title = AshfallUiHelpers.MakeTitle("WEATHER HISTORY", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblHistoryTitle = AshfallUiHelpers.MakeSectionHeader("WEATHER HISTORY");
            vbox.AddChild(_lblHistoryTitle);

            _weatherHistory = new VBoxContainer();
            _weatherHistory.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _weatherHistory.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_weatherHistory);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblPatternsTitle = AshfallUiHelpers.MakeSectionHeader("WEATHER PATTERNS");
            vbox.AddChild(_lblPatternsTitle);

            _weatherPatterns = new VBoxContainer();
            _weatherPatterns.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _weatherPatterns.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_weatherPatterns);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblAnomaliesTitle = AshfallUiHelpers.MakeSectionHeader("WEATHER ANOMALIES");
            vbox.AddChild(_lblAnomaliesTitle);

            _weatherAnomalies = new VBoxContainer();
            _weatherAnomalies.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _weatherAnomalies.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_weatherAnomalies);

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
