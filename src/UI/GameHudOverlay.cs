using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Polished in-game HUD overlay.
    /// Shows: day, health bar, radiation bar, value, faction, weather, and menu.
    /// Thin presentation only — reads state from HoldfastRuntimeSession.
    /// </summary>
    public partial class GameHudOverlay : HBoxContainer
    {
        public event Action? OnMenuRequested;

        private Label _lblDay = null!;
        private Label _lblHealthText = null!;
        private ProgressBar _barHealth = null!;
        private ProgressBar _barRad = null!;
        private Label _lblRadText = null!;
        private Label _lblValue = null!;
        private Label _lblFaction = null!;
        private Label _lblWeather = null!;
        private Button _btnMenu = null!;

        // Animation state
        private float _healthAnimProgress = 1f;
        private float _radAnimProgress = 0f;
        private bool _healthAnimating = false;
        private bool _radAnimating = false;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopWide);
            AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

            // Day
            _lblDay = AshfallUiHelpers.MakeTitle("Day 1", Ashfall.Core.UI.Theme.FontSizeH3);
            _lblDay.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            AddChild(_lblDay);

            AddChild(new VSeparator());

            // Health bar with animation
            var healthGroup = new HBoxContainer();
            healthGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var healthLabel = AshfallUiHelpers.MakeSmall("HP");
            healthLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            healthGroup.AddChild(healthLabel);

            _barHealth = MakeMeter(Ashfall.Core.UI.Theme.Warm);
            healthGroup.AddChild(_barHealth);

            _lblHealthText = AshfallUiHelpers.MakeSmall("100/100");
            _lblHealthText.CustomMinimumSize = new Vector2(60, 0);
            _lblHealthText.HorizontalAlignment = HorizontalAlignment.Right;
            healthGroup.AddChild(_lblHealthText);

            AddChild(healthGroup);

            AddChild(new VSeparator());

            // Radiation bar with animation
            var radGroup = new HBoxContainer();
            radGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var radLabel = AshfallUiHelpers.MakeSmall("RAD");
            radLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            radGroup.AddChild(radLabel);

            _barRad = MakeMeter(Ashfall.Core.UI.Theme.Lethe);
            radGroup.AddChild(_barRad);

            _lblRadText = AshfallUiHelpers.MakeSmall("0.0 mSv");
            _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
            radGroup.AddChild(_lblRadText);

            AddChild(radGroup);

            AddChild(new VSeparator());

            // Value counter
            _lblValue = AshfallUiHelpers.MakeSmall("Value: 100");
            _lblValue.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            AddChild(_lblValue);

            AddChild(new VSeparator());

            // Faction + weather
            _lblFaction = AshfallUiHelpers.MakeSmall("—");
            _lblFaction.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            AddChild(_lblFaction);

            _lblWeather = AshfallUiHelpers.MakeSmall("");
            _lblWeather.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            AddChild(_lblWeather);

            // Spacer
            AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Menu button
            _btnMenu = AshfallUiHelpers.MakeButton("MENU [Esc]", () => OnMenuRequested?.Invoke());
            _btnMenu.CustomMinimumSize = new Vector2(100, 28);
            AddChild(_btnMenu);

            // Disable per-frame process polling as meters update reactively via UpdateState/UpdateHealth/UpdateRadiation.
            SetProcess(false);
        }

        public override void _Process(double delta)
        {
            _healthAnimating = false;
            _radAnimating = false;
        }

        /// <summary>
        /// Update HUD from HoldfastRuntimeSession state.
        /// </summary>
        public void UpdateState(int day, long value, string factionId = "", string weather = "")
        {
            _lblDay.Text = $"Day {day}";
            _lblValue.Text = $"Value: {value}";
            _lblFaction.Text = string.IsNullOrEmpty(factionId) ? "—" : factionId.Replace("_", " ").ToUpperInvariant();
            _lblWeather.Text = string.IsNullOrEmpty(weather) ? "" : $"· {weather}";
        }

        public void UpdateHealth(int hp, int maxHp = 100)
        {
            _lblHealthText.Text = $"HP: {hp}/{maxHp}";
            _barHealth.MaxValue = Math.Max(1, maxHp);
            _barHealth.Value = Math.Clamp(hp, 0, (int)_barHealth.MaxValue);
            if (hp <= 25)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (hp <= 50)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
        }

        public void UpdateRadiation(float msv)
        {
            _lblRadText.Text = $"RAD: {msv:F1} mSv";
            _barRad.MaxValue = 100f;
            _barRad.Value = Mathf.Clamp(msv, 0f, 100f);
            if (msv >= 100)
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (msv >= 50)
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
        }

        private static ProgressBar MakeMeter((float r, float g, float b, float a) fillColor)
        {
            var meter = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 0,
                ShowPercentage = false,
                CustomMinimumSize = new Vector2(96, 12)
            };
            meter.AddThemeStyleboxOverride("background", AshfallUiHelpers.MakeFlatBg(
                new Color(Ashfall.Core.UI.Theme.Ink.r, Ashfall.Core.UI.Theme.Ink.g, Ashfall.Core.UI.Theme.Ink.b, 0.9f),
                AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.LineSoft), 1, Ashfall.Core.UI.Theme.RadiusSm));
            meter.AddThemeStyleboxOverride("fill", AshfallUiHelpers.MakeFlatBg(
                new Color(fillColor.r, fillColor.g, fillColor.b, 0.92f), null, 0, Ashfall.Core.UI.Theme.RadiusSm));
            return meter;
        }
    }
}
