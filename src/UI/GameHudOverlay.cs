using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Minimal in-game HUD overlay.
    /// Sits at the top of the screen during gameplay.
    /// Shows: day, health, radiation, key status, and menu button.
    /// Thin presentation only — reads state from HoldfastRuntimeSession.
    /// </summary>
    public partial class GameHudOverlay : HBoxContainer
    {
        public event Action? OnMenuRequested;

        private Label _lblDay = null!;
        private Label _lblHealth = null!;
        private Label _lblRad = null!;
        private Label _lblValue = null!;
        private Label _lblFaction = null!;
        private Button _btnMenu = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopWide);
            AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);

            // Background strip
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
            _lblDay = AshfallUiHelpers.MakeSmall("Day 1");
            _lblDay.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            AddChild(_lblDay);

            AddChild(new VSeparator());

            // Health
            _lblHealth = AshfallUiHelpers.MakeSmall("HP: 100");
            _lblHealth.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            AddChild(_lblHealth);

            AddChild(new VSeparator());

            // Radiation
            _lblRad = AshfallUiHelpers.MakeSmall("RAD: 0 mSv");
            _lblRad.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
            AddChild(_lblRad);

            AddChild(new VSeparator());

            // Value
            _lblValue = AshfallUiHelpers.MakeSmall("Value: 100");
            _lblValue.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Hot));
            AddChild(_lblValue);

            AddChild(new VSeparator());

            // Faction
            _lblFaction = AshfallUiHelpers.MakeSmall("—");
            _lblFaction.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            AddChild(_lblFaction);

            // Spacer
            AddChild(new Control { SizeFlagsHorizontal = SizeFlags.ExpandFill });

            // Menu button
            _btnMenu = AshfallUiHelpers.MakeButton("MENU [Esc]", () => OnMenuRequested?.Invoke());
            _btnMenu.CustomMinimumSize = new Vector2(100, 28);
            AddChild(_btnMenu);
        }

        /// <summary>
        /// Update HUD from HoldfastRuntimeSession state.
        /// </summary>
        public void UpdateState(int day, long value, string factionId = "", string weather = "")
        {
            _lblDay.Text = $"Day {day}";
            _lblValue.Text = $"Value: {value}";
            _lblFaction.Text = string.IsNullOrEmpty(factionId) ? "—" : factionId.Replace("_", " ").ToUpperInvariant();
            if (!string.IsNullOrEmpty(weather))
                _lblFaction.Text += $" · {weather}";
        }

        public void UpdateHealth(int hp, int maxHp = 100)
        {
            _lblHealth.Text = $"HP: {hp}/{maxHp}";
            if (hp <= 25)
                _lblHealth.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (hp <= 50)
                _lblHealth.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblHealth.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
        }

        public void UpdateRadiation(float msv)
        {
            _lblRad.Text = $"RAD: {msv:F1} mSv";
            if (msv >= 100)
                _lblRad.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Critical));
            else if (msv >= 50)
                _lblRad.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Entropy));
            else
                _lblRad.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
        }
    }
}
