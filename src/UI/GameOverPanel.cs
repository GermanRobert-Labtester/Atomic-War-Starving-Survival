using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Game Over screen.
    /// Shown when the player's health reaches zero or all survivors perish.
    /// Cold, restrained, factual. No spectacle.
    /// </summary>
    public partial class GameOverPanel : Control
    {
        public event Action? OnReturnToMenu;
        public event Action? OnNewGame;

        private Label _lblCause = null!;
        private Label _lblStats = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // ── Rotating background ──
            // Reuse the same crossfade behavior as the entry menu, but keep
            // the game-over palette to the medical/inventory surfaces.
            AddChild(new UiBackgroundCarousel(UiAssetManifest.GameOverBackgrounds, 0.80f));

            // ── Center content ──
            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(420, 0);
            center.AddChild(vbox);

            // ── Title ──
            var title = AshfallUiHelpers.MakeTitle("THE LEDGER IS CLOSED", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Cause of death ──
            _lblCause = new Label
            {
                Text = "The bunker fell silent.",
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblCause.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            _lblCause.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            vbox.AddChild(_lblCause);

            // ── Stats ──
            _lblStats = new Label
            {
                Text = "",
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblStats.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblStats.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            vbox.AddChild(_lblStats);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Buttons ──
            var btnNewGame = AshfallUiHelpers.MakeButton("NEW GAME", () => OnNewGame?.Invoke());
            btnNewGame.CustomMinimumSize = new Vector2(240, 44);
            vbox.AddChild(btnNewGame);

            var btnMenu = AshfallUiHelpers.MakeButton("RETURN TO MENU", () => OnReturnToMenu?.Invoke());
            btnMenu.CustomMinimumSize = new Vector2(240, 44);
            vbox.AddChild(btnMenu);

            // ── Hint ──
            var hint = new Label
            {
                Text = "[Enter] New Game  ·  [Esc] Menu",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        /// <summary>
        /// Show the game over screen with a cause and stats.
        /// </summary>
        public void ShowGameOver(string cause, string stats)
        {
            _lblCause.Text = cause;
            _lblStats.Text = stats;
            Visible = true;
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed)
            {
                switch (key.Keycode)
                {
                    case Key.Enter:
                        OnNewGame?.Invoke();
                        GetViewport().SetInputAsHandled();
                        break;
                    case Key.Escape:
                        OnReturnToMenu?.Invoke();
                        GetViewport().SetInputAsHandled();
                        break;
                }
            }
        }
    }
}
