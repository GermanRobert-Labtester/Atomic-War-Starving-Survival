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
        private TextureRect _barHealth = null!;
        private TextureRect _barRad = null!;
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

            // Background strip with subtle gradient
            var bg = new StyleBoxFlat
            {
                BgColor = new Color(Ashfall.Core.UI.Theme.Ink.r, Ashfall.Core.UI.Theme.Ink.g, Ashfall.Core.UI.Theme.Ink.b, 0.95f),
                BorderColor = AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Line),
            };
            bg.SetBorderWidthAll(1);
            bg.ContentMarginLeft = Ashfall.Core.UI.Theme.SpacingMd;
            bg.ContentMarginRight = Ashfall.Core.UI.Theme.SpacingMd;
            bg.ContentMarginTop = Ashfall.Core.UI.Theme.SpacingSm;
            bg.ContentMarginBottom = Ashfall.Core.UI.Theme.SpacingSm;
            AddThemeStyleboxOverride("panel", bg);

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

            _barHealth = new TextureRect
            {
                CustomMinimumSize = new Vector2(120, 12),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                Modulate = new Color(1f, 1f, 1f, 0.3f) // base bar
            };
            healthGroup.AddChild(_barHealth);

            AddChild(healthGroup);

            AddChild(new VSeparator());

            // Radiation bar with animation
            var radGroup = new HBoxContainer();
            radGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            var radLabel = AshfallUiHelpers.MakeSmall("RAD");
            radLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            radGroup.AddChild(radLabel);

            _barRad = new TextureRect
            {
                CustomMinimumSize = new Vector2(120, 12),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                Modulate = new Color(1f, 1f, 1f, 0f) // starts empty
            };
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
        }

        public override void _Process(double delta)
        {
            // Animate health bar
            if (_healthAnimating)
            {
                _healthAnimProgress -= (float)delta * 2f; // 0.5s animation
                if (_healthAnimProgress <= 0f)
                {
                    _healthAnimProgress = 0f;
                    _healthAnimating = false;
                }
                _barHealth.Modulate = new Color(1f, 1f, 1f, _healthAnimProgress);
            }

            // Animate radiation bar
            if (_radAnimating)
            {
                _radAnimProgress += (float)delta * 1.5f; // ~0.67s animation
                if (_radAnimProgress >= 1f)
                {
                    _radAnimProgress = 1f;
                    _radAnimating = false;
                }
                _barRad.Modulate = new Color(1f, 1f, 1f, _radAnimProgress);
            }
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
            // Animate health bar fill
            _healthAnimating = true;
            _healthAnimProgress = 1f;

            _lblHealthText.Text = $"HP: {hp}/{maxHp}";
            if (hp <= 25)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (hp <= 50)
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblHealthText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
        }

        public void UpdateRadiation(float msv)
        {
            // Animate radiation bar fill
            _radAnimating = true;
            _radAnimProgress = 0f;

            _lblRadText.Text = $"RAD: {msv:F1} mSv";
            if (msv >= 100)
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (msv >= 50)
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblRadText.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
        }
    }
}
