using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;
using DesignTheme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp
{
    /// <summary>
    /// ASHFALL — Main menu screen.
    /// Cold, utilitarian post-exchange entry point.
    /// Provides responsive navigation: New Game, Continue, Settings, Codex / Records, Quit.
    /// </summary>
    public partial class MainMenuPanel : Control
    {
        public event Action? OnNewGame;
        public event Action? OnContinue;
        public event Action? OnSettings;
        public event Action? OnCodex;
        public event Action? OnQuit;

        private Button _btnNewGame = null!;
        private Button _btnContinue = null!;
        private Button _btnSettings = null!;
        private Button _btnCodex = null!;
        private Button _btnQuit = null!;
        private Label _lblStatus = null!;
        private Label _lblSubtitle = null!;
        private Label _lblVersion = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            BuildLayout();
        }

        public void SetContinueEnabled(bool enabled)
        {
            if (_btnContinue != null)
            {
                _btnContinue.Disabled = !enabled;
                _btnContinue.Text = enabled ? "CONTINUE EXPEDITION" : "CONTINUE (NO ACTIVE SAVE)";
            }
        }

        public void EnableContinue(bool enabled) => SetContinueEnabled(enabled);

        public void SetStatusMessage(string msg)
        {
            if (_lblStatus != null)
            {
                _lblStatus.Text = msg;
                _lblStatus.Visible = !string.IsNullOrWhiteSpace(msg);
            }
        }

        public void FocusPrimary()
        {
            if (_btnContinue != null && !_btnContinue.Disabled)
            {
                _btnContinue.GrabFocus();
            }
            else if (_btnNewGame != null)
            {
                _btnNewGame.GrabFocus();
            }
        }

        private void BuildLayout()
        {
            // ── Background Carousel / Solid Underlay ──
            var bgSolid = new ColorRect
            {
                Color = AshfallUiHelpers.ToColor(DesignTheme.Ink)
            };
            bgSolid.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bgSolid);

            // Add carousel with safe fallback
            try
            {
                var carousel = new UiBackgroundCarousel(UiAssetManifest.MainMenuBackgrounds, 0.55f);
                carousel.SetAnchorsPreset(LayoutPreset.FullRect);
                AddChild(carousel);
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn("<unknown>", "unknown", ex_CATDIAG);
                // Fall back to solid background
            }

            // ── Center Container for Responsive Scaling ──
            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            // ── Main Content Box ──
            var panel = AshfallUiHelpers.MakePanel(520, 0);
            center.AddChild(panel);

            var vbox = AshfallUiHelpers.MakeVBox(DesignTheme.SpacingMd);
            vbox.CustomMinimumSize = new Vector2(480, 0);
            panel.AddChild(vbox);

            // ── Title Block ──
            var titleLabel = AshfallUiHelpers.MakeTitle("ASHFALL", DesignTheme.FontSizeH1);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(titleLabel);

            _lblSubtitle = new Label
            {
                Text = "ATOMIC WAR // STARVING SURVIVAL",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblSubtitle.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
            _lblSubtitle.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Lethe));
            vbox.AddChild(_lblSubtitle);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Tagline ──
            var tagline = new Label
            {
                Text = "The nuclear exchange is concluded. Atmospheric fallout is descending.\nManage radiation, rations, air filtration, and human survival.",
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            tagline.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            tagline.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(tagline);

            _lblStatus = new Label
            {
                Text = string.Empty,
                HorizontalAlignment = HorizontalAlignment.Center,
                Visible = false
            };
            _lblStatus.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
            _lblStatus.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            vbox.AddChild(_lblStatus);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Navigation Buttons ──
            _btnContinue = AshfallUiHelpers.MakeButton("CONTINUE (NO ACTIVE SAVE)", () => OnContinue?.Invoke());
            _btnContinue.CustomMinimumSize = new Vector2(320, 46);
            _btnContinue.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
            _btnContinue.Disabled = true;
            vbox.AddChild(_btnContinue);

            _btnNewGame = AshfallUiHelpers.MakeButton("COMMENCE NEW GAME", () => OnNewGame?.Invoke());
            _btnNewGame.CustomMinimumSize = new Vector2(320, 46);
            _btnNewGame.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeH3);
            vbox.AddChild(_btnNewGame);

            _btnSettings = AshfallUiHelpers.MakeButton("SETTINGS & CONFIGURATION", () => OnSettings?.Invoke());
            _btnSettings.CustomMinimumSize = new Vector2(320, 40);
            _btnSettings.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            vbox.AddChild(_btnSettings);

            _btnCodex = AshfallUiHelpers.MakeButton("ARCHIVE & CODEX RECORDS", () => OnCodex?.Invoke());
            _btnCodex.CustomMinimumSize = new Vector2(320, 40);
            _btnCodex.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            vbox.AddChild(_btnCodex);

            _btnQuit = AshfallUiHelpers.MakeButton("QUIT TO SYSTEM", () => OnQuit?.Invoke());
            _btnQuit.CustomMinimumSize = new Vector2(320, 40);
            _btnQuit.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeBody);
            vbox.AddChild(_btnQuit);

            // ── Footer ──
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblVersion = new Label
            {
                Text = "ASHFALL v1.0.0 // GODOT 4.7+ .NET ENGINE // HOST REVISION ACTIVE",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            _lblVersion.AddThemeFontSizeOverride("font_size", DesignTheme.FontSizeLabel);
            _lblVersion.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            vbox.AddChild(_lblVersion);
        }
    }
}
