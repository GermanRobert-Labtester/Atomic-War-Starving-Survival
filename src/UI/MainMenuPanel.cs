using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Main menu screen.
    /// Cold, utilitarian entry point. No gloss, no fantasy.
    /// Offers: New Game, Continue (if save exists), Quit.
    /// Backgrounds rotate with crossfade transitions.
    /// </summary>
    public partial class MainMenuPanel : Control
    {
        public event Action? OnNewGame;
        public event Action? OnContinue;
        public event Action? OnQuit;

        private Label _lblSubtitle = null!;
        private Button _btnContinue = null!;
        private Label _lblVersion = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            // ── Rotating full-screen background ──
            AddChild(new UiBackgroundCarousel(UiAssetManifest.MainMenuBackgrounds, 0.65f));

            // ── Center container ──
            var center = new CenterContainer
            {
                Modulate = new Color(1, 1, 1, 0f) // start invisible for fade-in
            };
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXl);
            vbox.CustomMinimumSize = new Vector2(480, 0);
            center.AddChild(vbox);

            // ── Title block ──
            var titleLabel = AshfallUiHelpers.MakeTitle("ASHFALL", Ashfall.Core.UI.Theme.FontSizeH1);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(titleLabel);

            _lblSubtitle = new Label
            {
                Text = "ATOMIC WAR: STARVING SURVIVAL",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblSubtitle.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
            _lblSubtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Muted));
            vbox.AddChild(_lblSubtitle);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Tagline ──
            var tagline = new Label
            {
                Text = "The exchange is over. The ash is settling.\nYou have a bunker, a dosimeter, and whatever you carried down the stairs.",
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            tagline.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            tagline.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            vbox.AddChild(tagline);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Buttons ──
            var btnNewGame = AshfallUiHelpers.MakeButton("NEW GAME", () => OnNewGame?.Invoke());
            btnNewGame.CustomMinimumSize = new Vector2(280, 48);
            btnNewGame.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
            vbox.AddChild(btnNewGame);

            _btnContinue = AshfallUiHelpers.MakeButton("CONTINUE", () => OnContinue?.Invoke());
            _btnContinue.CustomMinimumSize = new Vector2(280, 48);
            _btnContinue.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeH3);
            _btnContinue.Disabled = true; // enabled when save exists
            vbox.AddChild(_btnContinue);

            var btnQuit = AshfallUiHelpers.MakeButton("QUIT", () => OnQuit?.Invoke());
            btnQuit.CustomMinimumSize = new Vector2(280, 48);
            btnQuit.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            vbox.AddChild(btnQuit);

            // ── Version / footer ──
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblVersion = new Label
            {
                Text = "v0.1 · Godot 4.7+ · .NET Edition",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblVersion.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            _lblVersion.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(_lblVersion);

            // ── Controls hint ──
            var controls = new Label
            {
                Text = "[Enter] New Game  ·  [C] Continue  ·  [Esc] Quit",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            controls.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            controls.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(controls);

            // Fade in entire menu content after background establishes itself
            QueueFadeInUI(center, delaySeconds: 1.2f);
        }

        private void QueueFadeInUI(Control uiNode, float delaySeconds = 0f)
        {
            var tw = CreateTween();
            tw.TweenInterval(delaySeconds);
            tw.TweenProperty(uiNode, "modulate:a", 1f, 1.0f).SetTrans(Tween.TransitionType.Sine)
                .SetEase(Tween.EaseType.Out);
        }

        /// <summary>
        /// Enable the Continue button when a save file exists.
        /// </summary>
        public void EnableContinue(bool enabled)
        {
            _btnContinue.Disabled = !enabled;
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
                    case Key.C:
                        if (!_btnContinue.Disabled)
                            OnContinue?.Invoke();
                        GetViewport().SetInputAsHandled();
                        break;
                    case Key.Escape:
                        OnQuit?.Invoke();
                        GetViewport().SetInputAsHandled();
                        break;
                }
            }
        }
    }
}
