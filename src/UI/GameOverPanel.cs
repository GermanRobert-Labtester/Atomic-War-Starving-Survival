// SPDX-License-Identifier: MIT
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
        private Label _lblLedger = null!;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // ── Rotating background ──
            // Reuse the same crossfade behavior as the entry menu, but keep
            // the game-over palette to the medical/inventory surfaces.
            AddChild(new UiBackgroundCarousel(UiAssetManifest.GameOverBackgrounds, 0.90f)); // A11Y §3: 0.80 left the panel-less Muted/Dim labels at ~3.5:1 worst-case

            // ── Center content ──
            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(420, 0);
            center.AddChild(vbox);

            // ── Title ──
            var title = AshfallUiHelpers.MakeTitle(AshfallUiText.Tr("ui.game_over.title", "THE LEDGER IS CLOSED"), Ashfall.Core.UI.Theme.FontSizeH2);
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

            // ── Per-survivor loss ledger (P014) ──
            // One line per survivor in death order, then the survivors still
            // standing. Hidden when the host supplies no ledger.
            _lblLedger = new Label
            {
                Text = string.Empty,
                HorizontalAlignment = HorizontalAlignment.Center,
                AutowrapMode = TextServer.AutowrapMode.WordSmart
            };
            _lblLedger.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
            _lblLedger.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            _lblLedger.Visible = false;
            vbox.AddChild(_lblLedger);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // ── Buttons ──
            var btnNewGame = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.game_over.new_game", "NEW GAME"), () => OnNewGame?.Invoke());
            btnNewGame.CustomMinimumSize = new Vector2(240, 44);
            vbox.AddChild(btnNewGame);

            var btnMenu = AshfallUiHelpers.MakeButton(AshfallUiText.Tr("ui.game_over.return_menu", "RETURN TO MENU"), () => OnReturnToMenu?.Invoke());
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
        /// P014 — optional supplier of the per-survivor cause-of-death ledger.
        /// When set, every Game Over path renders the ledger without any caller
        /// needing to pass it. Read-only: the panel never authors the text.
        /// </summary>
        public Func<string>? LedgerProvider { get; set; }

        /// <summary>
        /// Show the game over screen with a cause and stats.
        /// </summary>
        public void ShowGameOver(string cause, string stats)
        {
            _lblCause.Text = cause;
            _lblStats.Text = stats;
            ApplyLedger(LedgerProvider?.Invoke() ?? string.Empty);
            Visible = true;
        }

        /// <summary>
        /// Show the game over screen with a cause, aggregate stats, and the
        /// per-survivor cause-of-death ledger (P014). An empty ledger hides the
        /// block so a death with no roster (e.g. a legacy path) reads unchanged.
        /// </summary>
        public void ShowGameOver(string cause, string stats, string ledger)
        {
            _lblCause.Text = cause;
            _lblStats.Text = stats;
            ApplyLedger(ledger);
            Visible = true;
        }

        private void ApplyLedger(string ledger)
        {
            _lblLedger.Text = ledger ?? string.Empty;
            _lblLedger.Visible = !string.IsNullOrEmpty(ledger);
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (AshfallInputActions.IsConfirm(@event))
            {
                OnNewGame?.Invoke();
                GetViewport()?.SetInputAsHandled();
            }
            else if (AshfallInputActions.IsCloseOrCancel(@event))
            {
                OnReturnToMenu?.Invoke();
                GetViewport()?.SetInputAsHandled();
            }
        }
    }
}
