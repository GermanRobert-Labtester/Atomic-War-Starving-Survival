// SPDX-License-Identifier: MIT
using System;
using Godot;
using Ashfall.Core.UI;
using Theme = Ashfall.Core.UI.Theme;

using Ashfall.Core.IO;
namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — shared UI construction helpers.
    /// Thin, stateless utilities that enforce the design-system tokens
    /// from Theme.cs. Every panel that builds its layout in C# should
    /// use these helpers instead of hard-coding font sizes, colors,
    /// or spacing values directly.
    ///
    /// No simulation logic — presentation only.
    /// </summary>
    public static class AshfallUiHelpers
    {
        // ── Canonical Fallback Texture Constants ─────────────────────────
        /// <summary>Canonical relative path for the fallback placeholder UI icon.</summary>
        public const string FallbackIconPath = "assets/ui/Icons/icon_placeholder.png";

        /// <summary>Canonical resource path for the fallback placeholder UI icon.</summary>
        public const string FallbackIconResPath = "res://assets/ui/Icons/icon_placeholder.png";

        /// <summary>Canonical resource path for the fallback survivor sprite/portrait.</summary>
        public const string FallbackSurvivorResPath = "res://assets/sprites/Characters/placeholder_survivor.png";

        /// <summary>Canonical relative path for the fallback survivor sprite/portrait.</summary>
        public const string FallbackSurvivorPath = "assets/sprites/Characters/placeholder_survivor.png";

        // ── Font Loading ────────────────────────────────────────────────
        // Lazy-loaded canonical fonts. Each property loads on first access
        // and caches the result. Returns null when the resource is missing
        // so callers can fall back to Godot's default system font.

        private static readonly System.Collections.Generic.Dictionary<string, FontFile?> _fontCache = new(StringComparer.Ordinal);
        private static FontFile? _fontBarlowRegular;
        private static FontFile? _fontBarlowSemiBold;
        private static FontFile? _fontBarlowBold;
        private static FontFile? _fontShareTechMono;

        /// <summary>
        /// Loads a FontFile from a res:// path. Returns cached instance or null on failure.
        /// </summary>
        public static FontFile? LoadFont(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (_fontCache.TryGetValue(path, out var cached))
                return cached;

            FontFile? loaded = null;
            try
            {
                if (ResourceLoader.Exists(path))
                    loaded = ResourceLoader.Load<FontFile>(path);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[AshfallUiHelpers] Failed to load font '{path}': {e.Message}");
            }
            _fontCache[path] = loaded;
            return loaded;
        }

        public static FontFile? FontBarlowRegular =>
            _fontBarlowRegular ??= LoadFont("res://assets/fonts/BarlowCondensed-Regular.ttf");

        public static FontFile? FontBarlowSemiBold =>
            _fontBarlowSemiBold ??= LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");

        public static FontFile? FontBarlowBold =>
            _fontBarlowBold ??= LoadFont("res://assets/fonts/BarlowCondensed-Bold.ttf");

        public static FontFile? FontShareTechMono =>
            _fontShareTechMono ??= LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");

        /// <summary>
        /// Applies a font override to a label. No-op when font is null
        /// (falls back to Godot's default system font).
        /// </summary>
        public static void ApplyFont(Label label, FontFile? font)
        {
            if (label == null || font == null) return;
            label.AddThemeFontOverride("font", font);
        }

        // ── Typography ──────────────────────────────────────────────────
        // Maps directly to Theme.cs font-size tokens.

        /// <summary>
        /// Shared exit seam for every label factory (a11y precision 2026-09-29).
        /// Godot Labels default to clip_text=false, so long text silently draws
        /// past its rect and collides with neighboring controls. Non-autowrap
        /// labels truncate with an ellipsis at their rect instead; autowrap
        /// labels keep wrapping and are untouched.
        /// </summary>
        private static Label FinishLabel(Label lbl)
        {
            if (lbl.AutowrapMode == TextServer.AutowrapMode.Off)
            {
                lbl.ClipText = true;
                lbl.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
            }
            return lbl;
        }

        public static Label MakeTitle(string text, int fontSize = Theme.FontSizeH1)
        {
            var lbl = new Label
            {
                Text = text,
                HorizontalAlignment = HorizontalAlignment.Center,
                Uppercase = true
            };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Warm));
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        public static Label MakeSectionHeader(string text)
        {
            var lbl = new Label
            {
                Text = text,
                Uppercase = true
            };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeH3);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        public static Label MakeSubsectionHeader(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeSmall);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Muted));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeBody(string text, bool autowrap = true)
        {
            var lbl = new Label { Text = text };
            if (autowrap)
                lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeSmall(string text, bool autowrap = false)
        {
            var lbl = new Label { Text = text };
            if (autowrap)
                lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeSmall);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeMono(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeMono);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontShareTechMono);
            return FinishLabel(lbl);
        }

        public static Label MakeLabel(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeLabel);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Dim));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeLabel(string text, int fontSize, (float r, float g, float b, float a) colorToken)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(colorToken));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeLabel(string text, int fontSize, Color color)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", color);
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        /// <summary>
        /// Creates a label with an explicit font size and weight. Bold selects the
        /// semi-bold face; otherwise the regular face is used. Used by panel
        /// headers (e.g. fontSize: 20, bold: true).
        /// </summary>
        public static Label MakeLabel(string text, int fontSize, bool bold)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, bold ? FontBarlowSemiBold : FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeMetadata(string text, bool autowrap = false)
        {
            var lbl = new Label { Text = text };
            if (autowrap)
            {
                lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
                lbl.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            }
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeLabel);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Muted));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeWarning(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Entropy));
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        public static Label MakeCritical(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Critical));
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        // ── Semantic Color Properties ──────────────────────────────────
        public static Color ColorBackdrop => ToColor(Theme.BackdropOverlay);
        public static Color ColorSurface => ToColor(Theme.Surface);
        public static Color ColorSurfaceCard => ToColor(Theme.SurfaceCard);
        public static Color ColorPrimary => ToColor(Theme.Warm);
        public static Color ColorHighlight => ToColor(Theme.Hot);

        /// <summary>
        /// Neutral modulate (no tint / identity). Named so panels never spell a raw
        /// engine colour — <c>Colors.White</c> is modulate identity, not a theme
        /// decision, and reading it as "white text" is a standing misinterpretation.
        /// </summary>
        public static Color ColorNeutral => Colors.White;
        public static Color ColorText => ToColor(Theme.Pale);
        public static Color ColorMuted => ToColor(Theme.Muted);
        public static Color ColorDim => ToColor(Theme.Dim);
        public static Color ColorSuccess => ToColor(Theme.Success);
        public static Color ColorWarning => ToColor(Theme.Warning);
        public static Color ColorCritical => ToColor(Theme.Critical);
        public static Color ColorRadiation => ToColor(Theme.Radiation);
        public static Color ColorRadiationAcute => ToColor(Theme.RadiationAcute);
        public static Color ColorInfo => ToColor(Theme.Info);

        /// <summary>
        /// Creates a full-screen semi-transparent backdrop overlay for modal panels.
        /// </summary>
        public static ColorRect MakeBackdropOverlay()
        {
            var bg = new ColorRect { Color = ColorBackdrop };
            bg.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            return bg;
        }

        public static Label MakeSuccess(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ColorSuccess);
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        public static Label MakeInfo(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ColorInfo);
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        public static Label MakeRadiation(string text, bool acute = false)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", acute ? ColorRadiationAcute : ColorRadiation);
            ApplyFont(lbl, FontBarlowSemiBold);
            return FinishLabel(lbl);
        }

        // ── Spacing & Layout ────────────────────────────────────────────

        public static VBoxContainer MakeVBox(int separation = Theme.SpacingSm)
        {
            var box = new VBoxContainer();
            box.AddThemeConstantOverride("separation", separation);
            return box;
        }

        public static HBoxContainer MakeHBox(int separation = Theme.SpacingSm)
        {
            var box = new HBoxContainer();
            box.AddThemeConstantOverride("separation", separation);
            return box;
        }

        public static MarginContainer MakeMargins(int all = Theme.HudPanelPadding)
        {
            return MakeMargins(all, all, all, all);
        }

        public static MarginContainer MakeMargins(int left, int top, int right, int bottom)
        {
            var margin = new MarginContainer();
            margin.AddThemeConstantOverride("margin_left", left);
            margin.AddThemeConstantOverride("margin_top", top);
            margin.AddThemeConstantOverride("margin_right", right);
            margin.AddThemeConstantOverride("margin_bottom", bottom);
            return margin;
        }

        // ── Empty State Helpers ─────────────────────────────────────────

        /// <summary>
        /// Creates a standardized empty-state placeholder container with an optional title,
        /// description message, and an informative action hint so empty panels never appear broken or blank.
        /// </summary>
        public static Control MakeEmptyState(
            string message,
            string title = "NO DATA RECORDED",
            string? actionHint = null,
            (float r, float g, float b, float a)? accentColor = null)
        {
            var panel = MakePanel();
            panel.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            panel.CustomMinimumSize = new Vector2(0, 100);

            var margin = MakeMargins((int)Theme.SpacingMd);
            panel.AddChild(margin);

            var vbox = MakeVBox((int)Theme.SpacingXs);
            vbox.Alignment = BoxContainer.AlignmentMode.Center;
            margin.AddChild(vbox);

            var color = accentColor.HasValue ? ToColor(accentColor.Value) : ToColor(Theme.Muted);

            var titleLabel = MakeTitle(title, Theme.FontSizeH3);
            titleLabel.HorizontalAlignment = HorizontalAlignment.Center;
            titleLabel.AddThemeColorOverride("font_color", color);
            vbox.AddChild(titleLabel);

            var msgLabel = MakeSmall(message, autowrap: true);
            msgLabel.HorizontalAlignment = HorizontalAlignment.Center;
            msgLabel.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            vbox.AddChild(msgLabel);

            if (!string.IsNullOrEmpty(actionHint))
            {
                var hintLabel = MakeMetadata(actionHint);
                hintLabel.HorizontalAlignment = HorizontalAlignment.Center;
                hintLabel.AddThemeColorOverride("font_color", ToColor(Theme.Dim));
                vbox.AddChild(hintLabel);
            }

            return panel;
        }

        /// <summary>
        /// Creates a lightweight inline empty-state label for sub-lists or tables with no items.
        /// </summary>
        public static Label MakeEmptyStateLabel(string message, string? hint = null)
        {
            string fullText = string.IsNullOrEmpty(hint) ? $"— {message} —" : $"— {message} [{hint}] —";
            var label = MakeSmall(fullText, autowrap: true);
            label.HorizontalAlignment = HorizontalAlignment.Center;
            label.AddThemeColorOverride("font_color", ToColor(Theme.Dim));
            return label;
        }

        // ── Panel Shells ────────────────────────────────────────────────

        /// <summary>
        /// Creates a PanelContainer with the standard 9-slice background
        /// (frame_9slice.png, 16px border) and internal padding.
        /// </summary>
        public static PanelContainer MakePanel(int minWidth = 0, int minHeight = 0)
        {
            var panel = new PanelContainer();
            if (minWidth > 0 || minHeight > 0)
                panel.CustomMinimumSize = new Vector2(minWidth, minHeight);

            var tex = TryLoadTexture("res://assets/ui/Textures/frame_9slice.png")
                   ?? TryLoadTexture("res://assets/ui/frame_9slice.svg")
                   ?? TryLoadTexture("res://assets/ui/Textures/panel_bg_9slice.png");

            if (tex != null)
            {
                var sb = new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 16,
                    TextureMarginTop = 16,
                    TextureMarginRight = 16,
                    TextureMarginBottom = 16
                };
                panel.AddThemeStyleboxOverride("panel", sb);
            }
            else
            {
                var sb = new StyleBoxFlat
                {
                    BgColor = ToColor(Theme.Ink),
                    BorderColor = ToColor(Theme.Line),
                };
                sb.SetBorderWidthAll(1);
                panel.AddThemeStyleboxOverride("panel", sb);
            }
            return panel;
        }

        // ── Standard Panel StyleBox (used by 11 direct call sites) ─────
        // Mirrors the MakePanel() fallback chain (frame_9slice.png →
        // frame_9slice.svg → panel_bg_9slice.png → flat fallback) so
        // callers that previously loaded "panel_bg_9slice.png" directly
        // now receive the properly framed 9-slice source. TextureMargin
        // values are unchanged from the legacy 16/16/16/16 contract.
        public static StyleBox MakePanelFrameStyleBox()
        {
            var tex = TryLoadTexture("res://assets/ui/Textures/frame_9slice.png")
                   ?? TryLoadTexture("res://assets/ui/frame_9slice.svg")
                   ?? TryLoadTexture("res://assets/ui/Textures/panel_bg_9slice.png")
                   ?? TryLoadTexture("res://assets/ui/Textures/panel_bg_9slice.png");
            if (tex != null)
            {
                return new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 16,
                    TextureMarginTop = 16,
                    TextureMarginRight = 16,
                    TextureMarginBottom = 16
                };
            }
            // Last-resort flat frame so the panel still has a border
            var flat = new StyleBoxFlat
            {
                BgColor = ToColor(Theme.Ink),
                BorderColor = ToColor(Theme.Line),
            };
            flat.SetBorderWidthAll(1);
            return flat;
        }

        // Standard header bar texture (margin 12/8/12/8)
        public static StyleBox MakeHeaderFrameStyleBox()
        {
            // Flat bar. The tab_strip 9-slice used to dress the whole header,
            // which painted a phantom empty tab slot on every panel and let
            // long titles run underneath it; the tab plate now lives only
            // behind the title (MakeTitleTabStyleBox).
            var flat = new StyleBoxFlat
            {
                BgColor = new Color(Theme.Ink.r, Theme.Ink.g, Theme.Ink.b, 0.95f),
                BorderColor = ToColor(Theme.Line),
            };
            flat.SetBorderWidthAll(1);
            return flat;
        }


        /// <summary>
        /// Creates the standard header bar with 9-slice background.
        /// </summary>
        public static PanelContainer MakeHeaderBar()
        {
            var header = new PanelContainer();
            header.AddThemeStyleboxOverride("panel", MakeHeaderFrameStyleBox());
            return header;
        }

        public static PanelContainer MakeCard(int minW = 0, int minH = 0) => MakePanel(minW, minH);

        /// <summary>
        /// Creates a card container with 9-slice framing and internal margin padding.
        /// </summary>
        public static PanelContainer MakeCardFrame(string title, string? subtitle = null, int minW = 0, int minH = 0)
        {
            var card = MakePanel(minW, minH);
            var margins = MakeMargins(Theme.SpacingSm);
            card.AddChild(margins);

            var vbox = MakeVBox(Theme.SpacingSm);
            margins.AddChild(vbox);

            var header = MakeHBox(Theme.SpacingSm);
            var titleLbl = MakeSectionHeader(title);
            header.AddChild(titleLbl);
            if (!string.IsNullOrEmpty(subtitle))
            {
                header.AddChild(new Control { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill });
                var subLbl = MakeMetadata(subtitle);
                header.AddChild(subLbl);
            }
            vbox.AddChild(header);
            vbox.AddChild(MakeSeparator());

            return card;
        }

        // ── Separators ──────────────────────────────────────────────────

        public static HSeparator MakeSeparator()
        {
            var sep = new HSeparator();
            sep.AddThemeConstantOverride("separation", Theme.SpacingSm);
            return sep;
        }

        // ── Buttons ─────────────────────────────────────────────────────

        public static Button? AddActionButton(VBoxContainer? parent, string label, Action onPressed)
        {
            if (parent == null || !GodotObject.IsInstanceValid(parent))
                return null;

            var button = MakeButton(label, onPressed);
            parent.AddChild(button);
            return button;
        }

        public static Button MakeButton(string text, Action onPressed, bool disabled = false)
        {
            var btn = new Button
            {
                Text = text,
                Disabled = disabled,
                // Overflow precision (a11y 2026-09-29): content-sized buttons are
                // unaffected; in fixed-width rows the label truncates with an
                // ellipsis instead of drawing past the button rect.
                ClipText = true,
                TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis,
                // Interactive target size (a11y audit 2026-09-29 §5d/§9.10):
                // fixed 28px floor, not font-coupled — FontSizeBody + SpacingMd
                // computed 27px and drifted with typography changes.
                CustomMinimumSize = new Vector2(0, Theme.MinInteractiveHeight)
            };
            btn.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            ApplyFont(btn, FontBarlowSemiBold);

            // Attempt to load raster button textures or use flat fallback
            var normalTex = TryLoadTexture("res://assets/ui/Textures/btn_default.png");
            var hoverTex = TryLoadTexture("res://assets/ui/Textures/btn_hover.png");
            var pressedTex = TryLoadTexture("res://assets/ui/Textures/btn_pressed.png");
            var disabledTex = TryLoadTexture("res://assets/ui/Textures/btn_disabled.png");

            if (normalTex != null && hoverTex != null && pressedTex != null && disabledTex != null)
            {
                btn.AddThemeStyleboxOverride("normal", new StyleBoxTexture { Texture = normalTex, TextureMarginLeft = 8, TextureMarginRight = 8, TextureMarginTop = 4, TextureMarginBottom = 4 });
                btn.AddThemeStyleboxOverride("hover", new StyleBoxTexture { Texture = hoverTex, TextureMarginLeft = 8, TextureMarginRight = 8, TextureMarginTop = 4, TextureMarginBottom = 4 });
                btn.AddThemeStyleboxOverride("pressed", new StyleBoxTexture { Texture = pressedTex, TextureMarginLeft = 8, TextureMarginRight = 8, TextureMarginTop = 4, TextureMarginBottom = 4 });
                btn.AddThemeStyleboxOverride("disabled", new StyleBoxTexture { Texture = disabledTex, TextureMarginLeft = 8, TextureMarginRight = 8, TextureMarginTop = 4, TextureMarginBottom = 4 });
            }
            else
            {
                btn.AddThemeStyleboxOverride("normal", MakeFlatBg(
                    new Color(Theme.Ink.r, Theme.Ink.g, Theme.Ink.b, 0.65f), ToColor(Theme.Line), 1, Theme.RadiusSm));
                btn.AddThemeStyleboxOverride("hover", MakeFlatBg(
                    new Color(Theme.Warm.r, Theme.Warm.g, Theme.Warm.b, 0.18f), ToColor(Theme.Warm), 1, Theme.RadiusSm));
                btn.AddThemeStyleboxOverride("pressed", MakeFlatBg(
                    new Color(Theme.Warm.r, Theme.Warm.g, Theme.Warm.b, 0.30f), ToColor(Theme.Hot), 1, Theme.RadiusSm));
                btn.AddThemeStyleboxOverride("disabled", MakeFlatBg(
                    new Color(Theme.Ink.r, Theme.Ink.g, Theme.Ink.b, 0.30f), ToColor(Theme.LineSoft), 1, Theme.RadiusSm));
            }

            btn.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            btn.AddThemeColorOverride("font_hover_color", ToColor(Theme.Hot));
            btn.AddThemeColorOverride("font_pressed_color", ToColor(Theme.Hot));
            btn.AddThemeColorOverride("font_disabled_color", ToColor(Theme.Dim));
            btn.Pressed += () =>
            {
                AtomicWar.GodotApp.Audio.AudioManager.Instance?.PlayUiClick();
                onPressed?.Invoke();
            };
            AshfallFocusPolicy.ApplyFocusVisibleStyle(btn);
            UiMotion.AttachButtonFx(btn);
            return btn;
        }

        /// <summary>
        /// Creates a disabled button with an explicit tooltip and visual state explaining
        /// why the action cannot currently be taken, eliminating player guesswork.
        /// </summary>
        public static Button MakeDisabledButton(string text, string reasonDisabled)
        {
            var btn = MakeButton(text, () => { }, disabled: true);
            btn.TooltipText = string.IsNullOrEmpty(reasonDisabled) ? "Action currently unavailable" : reasonDisabled;
            btn.AddThemeColorOverride("font_disabled_color", ToColor(Theme.Dim));
            return btn;
        }

        /// <summary>
        /// Creates a standardized multi-channel severity badge (Icon + Color + Text Label)
        /// guaranteeing that critical states never communicate by color alone.
        /// </summary>
        public static Control MakeSeverityBadge(SeverityLevel level, string text, string? customIcon = null)
        {
            var box = MakeHBox(Theme.SpacingXs);
            box.Alignment = BoxContainer.AlignmentMode.Center;

            string icon = customIcon ?? GetSeverityIcon(level);
            Color col = GetSeverityColor(level);

            var iconLbl = MakeLabel(icon, Theme.FontSizeMono, col);
            box.AddChild(iconLbl);

            var textLbl = MakeLabel(text, Theme.FontSizeSmall, col);
            ApplyFont(textLbl, FontBarlowSemiBold);
            box.AddChild(textLbl);

            return box;
        }

        public static Color GetSeverityColor(SeverityLevel level) => level switch
        {
            SeverityLevel.Normal => ColorSuccess,
            SeverityLevel.Attention => ColorWarning,
            SeverityLevel.Dangerous => ColorRadiation,
            SeverityLevel.Critical => ColorCritical,
            SeverityLevel.Unavailable => ColorDim,
            _ => ColorText
        };

        public static string GetSeverityIcon(SeverityLevel level) => level switch
        {
            SeverityLevel.Normal => "[OK]",
            SeverityLevel.Attention => "[▲]",
            SeverityLevel.Dangerous => "[RAD]",
            SeverityLevel.Critical => "[!]",
            SeverityLevel.Unavailable => "[X]",
            _ => "[•]"
        };

        private static void ApplyFont(Button btn, FontFile? font)
        {
            if (btn == null || font == null) return;
            btn.AddThemeFontOverride("font", font);
        }

        // ── Data Row ────────────────────────────────────────────────────

        /// <summary>
        /// Standard data row: label left, value right, optional value color.
        /// </summary>
        public static HBoxContainer MakeDataRow(string label, string value,
            Color? valueColor = null, int fontSize = Theme.FontSizeSmall)
        {
            var row = MakeHBox(Theme.SpacingSm);

            var lbl = new Label { Text = label, SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Muted));
            ApplyFont(lbl, FontBarlowRegular);
            row.AddChild(lbl);

            var val = new Label { Text = value, HorizontalAlignment = HorizontalAlignment.Right };
            val.AddThemeFontSizeOverride("font_size", fontSize);
            val.AddThemeColorOverride("font_color", valueColor ?? ToColor(Theme.Pale));
            ApplyFont(val, FontShareTechMono);
            row.AddChild(val);

            return row;
        }

        /// <summary>
        /// Creates a dim/styled label for muted informational text.
        /// Used for empty states, disabled messages, and secondary information.
        /// </summary>
        public static Label MakeDimLabel(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Dim));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        /// <summary>
        /// Creates a label with a specific color token.
        /// Used for status messages, afflictions, and colored notifications.
        /// </summary>
        public static Label MakeColoredLabel(string text, (float r, float g, float b, float a) colorToken,
            int fontSize = Theme.FontSizeBody)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(colorToken));
            ApplyFont(lbl, FontBarlowRegular);
            return FinishLabel(lbl);
        }

        /// <summary>
        /// Creates a horizontal action bar container for buttons.
        /// Standard separation and right-alignment by default.
        /// </summary>
        public static HBoxContainer MakeActionBar(int separation = Theme.SpacingSm)
        {
            var bar = new HBoxContainer
            {
                SizeFlagsHorizontal = Control.SizeFlags.ShrinkEnd,
                Alignment = BoxContainer.AlignmentMode.End
            };
            bar.AddThemeConstantOverride("separation", separation);
            return bar;
        }

        // ── Visual Asset Loaders ────────────────────────────────────────

        public static TextureRect MakeFactionEmblem(string factionId, int size = 40)
        {
            var rect = new TextureRect
            {
                CustomMinimumSize = new Vector2(size, size),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize
            };
            rect.Texture = FactionIconLoader.LoadFor(factionId);
            return rect;
        }

        public static TextureRect MakeBadgeIcon(string badgeId, int size = 32)
        {
            var rect = new TextureRect
            {
                CustomMinimumSize = new Vector2(size, size),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize
            };
            string key = badgeId.StartsWith("badge_") ? badgeId : $"badge_{badgeId}";
            rect.Texture = TryLoadTexture($"res://assets/ui/Icons/{key}.png")
                        ?? TryLoadTexture($"res://assets/ui/Icons/{badgeId}.svg")
                        ?? TryLoadTexture($"res://assets/ui/Icons/icon_biohazard.svg");
            return rect;
        }

        public static TextureRect MakeItemIcon(string itemId, int size = 32)
        {
            var rect = new TextureRect
            {
                CustomMinimumSize = new Vector2(size, size),
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize
            };
            string key = itemId.StartsWith("item_") ? itemId : $"item_{itemId}";
            rect.Texture = ResolveItemTexture(itemId);
            return rect;
        }

        /// <summary>
        /// Resolves an item's art through the canonical registry chain and falls
        /// back to the shared placeholder icon. Presentation-only.
        ///
        /// <see cref="MakeItemIcon"/> wraps this in a <see cref="TextureRect"/>;
        /// callers that need a raw texture (e.g.
        /// <c>AshfallDataGrid.Cell.IconTexture</c>) use this directly, so every
        /// surface resolves item art through exactly one chain.
        /// </summary>
        public static Texture2D? ResolveItemTexture(string itemId)
        {
            if (string.IsNullOrEmpty(itemId))
                return TryLoadTexture(AssetRegistry.FallbackIconPath);

            string key = itemId.StartsWith("item_") ? itemId : $"item_{itemId}";
            return AssetRegistry.GetItem(itemId).Texture
                ?? AssetRegistry.GetItem(key).Texture
                ?? TryLoadTexture($"res://assets/art/{key}.jpg")
                ?? TryLoadTexture($"res://assets/art/{itemId}.jpg")
                ?? TryLoadTexture($"res://assets/art/{key}.png")
                ?? TryLoadTexture($"res://assets/art/{itemId}.png")
                // Canonical placeholder — never a domain-specific sprite.
                // The old pill icon made every un-arted item look medical.
                ?? TryLoadTexture(AssetRegistry.FallbackIconPath);
        }

        /// <summary>
        /// Shared value-change feedback for plain readout labels: assigns the text
        /// and, only when it actually changed, plays the shared settle pulse.
        /// Same convention as <c>AshfallMetricCard.SetValue</c>, for readouts that
        /// are not metric cards. Callers keep ownership of colour/criticality.
        /// </summary>
        public static void SetTextPulsed(Label label, string text)
        {
            if (label == null || !GodotObject.IsInstanceValid(label)) return;
            string next = text ?? string.Empty;
            if (string.Equals(label.Text, next, StringComparison.Ordinal)) return;
            label.Text = next;
            UiPanelFlow.Pulse(label);
        }

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<ulong, Tween> _barTweens = new();

        /// <summary>
        /// Shared value-change cue for gauge / progress fills: eases the bar to its
        /// new value instead of snapping, so a moving meter reads as motion rather
        /// than a redraw. No-op when unchanged, and falls back to an instant set
        /// under ReducedMotion, headless, or capture — the accessible path is
        /// always available.
        /// </summary>
        public static void SetBarValue(ProgressBar bar, float value)
        {
            if (bar == null || !GodotObject.IsInstanceValid(bar)) return;

            float target = Math.Clamp(value, (float)bar.MinValue, (float)bar.MaxValue);
            ulong id = bar.GetInstanceId();

            if (!UiMotion.CanAnimate || Math.Abs(bar.Value - target) < 0.01f)
            {
                if (_barTweens.TryRemove(id, out var stale)) stale?.Kill();
                bar.Value = target;
                return;
            }

            // One tween per bar: a newer update supersedes the running one rather
            // than stacking, so rapid refreshes stay smooth instead of jittering.
            if (_barTweens.TryRemove(id, out var previous)) previous?.Kill();

            var tween = bar.CreateTween();
            tween.SetTrans(Tween.TransitionType.Cubic).SetEase(Tween.EaseType.Out);
            tween.TweenProperty(bar, "value", target, 0.18f);
            _barTweens[id] = tween;
            tween.Finished += () =>
            {
                if (_barTweens.TryGetValue(id, out var cur) && ReferenceEquals(cur, tween))
                    _barTweens.TryRemove(id, out _);
            };
        }

        // ── Color Conversion ────────────────────────────────────────────

        /// <summary>
        /// Canonical dense-panel/modal scrim background (a11y audit 2026-09-29
        /// §2d): the one authority for full-rect panel backdrops, replacing
        /// ~58 hand-rolled near-grey literals. Honors colorblind simulation
        /// via <see cref="ToColor"/> like every other token consumer.
        /// </summary>
        public static Color PanelScrim() => ToColor(Theme.InkPanelStrong);

        public static Color ToColor((float r, float g, float b, float a) token)
        {
            // Plan 184 — preference-aware CVD simulation; Theme constants stay unchanged.
            string mode = AtomicWar.GodotApp.Settings.UserSettingsStore.Current.ColorblindMode;
            var mapped = Ashfall.Core.Settings.ColorblindColorMapper.Map(token, mode);
            return new Color(mapped.r, mapped.g, mapped.b, mapped.a);
        }

        // ── Survivor portraits ──────────────────────────────────────────

        /// <summary>
        /// Portrait chip for a survivor: resolves `assets/art/{id}.jpg`, then
        /// the other AssetRegistry portrait probes, and loads the first hit.
        /// Returns null when no portrait art exists so panels can omit the
        /// chip instead of faking a face. Pure presentation — the survivor id
        /// is supplied by the owning system, never invented here.
        /// </summary>
        public static TextureRect? MakeSurvivorPortrait(string survivorId, int size = 56)
        {
            if (string.IsNullOrWhiteSpace(survivorId)) return null;
            string? path = AtomicWar.GodotApp.AssetRegistry.ResolvePortraitPath(survivorId);
            if (string.IsNullOrEmpty(path)) return null;
            var texture = TryLoadTexture(path);
            if (texture == null) return null;

            var rect = new TextureRect
            {
                Name = "SurvivorPortrait",
                Texture = texture,
                CustomMinimumSize = new Vector2(size, size),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCentered,
                MouseFilter = Control.MouseFilterEnum.Ignore,
                TooltipText = survivorId
            };
            return rect;
        }

        // ── Location art ────────────────────────────────────────────────

        /// <summary>
        /// Establishing image for a location: resolves the registry location
        /// probes (assets/art/{id}.jpg first) and returns a clipped banner.
        /// Returns null when no art exists so the panel keeps its text layout
        /// instead of showing an empty frame.
        /// </summary>
        public static TextureRect? MakeLocationArt(string locationId, int height = 140)
        {
            if (string.IsNullOrWhiteSpace(locationId)) return null;
            string? path = AtomicWar.GodotApp.AssetRegistry.ResolveLocationPath(locationId);
            if (string.IsNullOrEmpty(path)) return null;
            var texture = TryLoadTexture(path);
            if (texture == null) return null;

            return new TextureRect
            {
                Name = "LocationArt",
                Texture = texture,
                CustomMinimumSize = new Vector2(0, height),
                ExpandMode = TextureRect.ExpandModeEnum.IgnoreSize,
                StretchMode = TextureRect.StretchModeEnum.KeepAspectCovered,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                MouseFilter = Control.MouseFilterEnum.Ignore,
                TooltipText = locationId
            };
        }

        // ── Texture Loading ─────────────────────────────────────────────

        private static readonly System.Collections.Concurrent.ConcurrentDictionary<string, Texture2D> _fallbackTextureCache = new();

        public static Texture2D? TryLoadTexture(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;

            if (_fallbackTextureCache.TryGetValue(path, out var cached) && cached != null && GodotObject.IsInstanceValid(cached))
                return cached;

            // 1. Preferred: Native Godot ResourceLoader import pipeline
            try
            {
                if (ResourceLoader.Exists(path))
                {
                    var res = ResourceLoader.Load<Texture2D>(path);
                    if (res != null) return res;
                }
            }
            catch (Exception ex_CATDIAG)
            {
                CatalogDiagnostics.Warn(path, "Texture load (ResourceLoader)", ex_CATDIAG);
                // Fall back below
            }

            // 2. Case-normalization fallback: res://Assets/ -> res://assets/
            if (path.StartsWith("res://Assets/", StringComparison.Ordinal))
            {
                string alt = "res://assets/" + path.Substring(13);
                try
                {
                    if (ResourceLoader.Exists(alt))
                    {
                        var res = ResourceLoader.Load<Texture2D>(alt);
                        if (res != null) return res;
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(alt, "Texture load (case-normalized)", ex_CATDIAG);
                    // Fall back below
                }
            }

            // 3. Fallback: Direct filesystem loader
            string osPath = ProjectSettings.GlobalizePath(path);
            if (System.IO.File.Exists(osPath))
            {
                try
                {
                    var img = Godot.Image.LoadFromFile(osPath);
                    if (img != null)
                    {
                        var tex = ImageTexture.CreateFromImage(img);
                        // Image is a native Godot object. CreateFromImage copies the
                        // pixel data, so dropping the managed wrapper here would leak
                        // the native Image at exit — release it explicitly.
                        img.Dispose();
                        _fallbackTextureCache[path] = tex;
                        return tex;
                    }
                }
                catch (Exception ex_CATDIAG)
                {
                    CatalogDiagnostics.Warn(osPath, "Texture load (filesystem)", ex_CATDIAG);
                    // Fall through
                }
            }

            if (path.StartsWith("res://Assets/", StringComparison.Ordinal))
            {
                string altOsPath = ProjectSettings.GlobalizePath("res://assets/" + path.Substring(13));
                if (System.IO.File.Exists(altOsPath))
                {
                    try
                    {
                        var img = Godot.Image.LoadFromFile(altOsPath);
                        if (img != null)
                        {
                            var tex = ImageTexture.CreateFromImage(img);
                            // See above: the native Image must be released once the
                            // texture has copied its data.
                            img.Dispose();
                            _fallbackTextureCache[path] = tex;
                            return tex;
                        }
                    }
                    catch (Exception ex_CATDIAG)
                    {
                        CatalogDiagnostics.Warn(altOsPath, "Texture load (alt filesystem)", ex_CATDIAG);
                        // Fall through
                    }
                }
            }

            return null;
        }

        // ── Panel Background (flat fallback) ────────────────────────────

        public static StyleBoxFlat MakeFlatBg(Color bg, Color? border = null,
            int borderWidth = 1, int cornerRadius = 0)
        {
            var sb = new StyleBoxFlat { BgColor = bg };
            if (border.HasValue)
            {
                sb.BorderColor = border.Value;
                sb.SetBorderWidthAll(borderWidth);
            }
            if (cornerRadius > 0)
                sb.SetCornerRadiusAll(cornerRadius);
            return sb;
        }

        // ── Tree Management ─────────────────────────────────────────────

        /// <summary>
        /// Detach and synchronously free every direct child of <paramref name="parent"/>.
        /// Replaces the legacy `while (...) { RemoveChild(c); c.QueueFree(); }` idiom
        /// that appears across 18+ panels. QueueFree() on a node already detached
        /// from the SceneTree defers deletion to the next idle frame; that frame
        /// may not arrive in time during headless smoke tests or during fast
        /// panel rebinds, so freed children survive in ObjectDB after
        /// `tree.Quit()` and get reported as leaks. Free() is synchronous and
        /// safe on detached nodes.
        /// Tolerates null and freed parents at the call site so callers can
        /// drop their `if (parent == null) return` pattern. Matching the
        /// behaviour introduced in ShelterPanel yesterday (see audit
        /// AUDIT_2026-08-19_UI_AND_YESTERDAYS_ASSETS.md).
        /// </summary>
        public static void EmptyChildren(Node? parent)
        {
            if (parent == null || !GodotObject.IsInstanceValid(parent))
                return;

            // Bound the loop defensively against free-during-iteration races;
            // safety counter guards against pathological parents whose
            // child retrieval invariant breaks under teardown.
            int safety = parent.GetChildCount() + 8;
            while (parent.GetChildCount() > 0 && safety-- > 0)
            {
                var child = parent.GetChild(0);
                parent.RemoveChild(child);
                FreeDetached(child);
            }
        }

        /// <summary>
        /// Frees a node that has already been detached from its parent.
        ///
        /// Immediate free is the contract — callers (and the ownership gates) rely
        /// on the node being gone right away. But a refresh triggered from inside a
        /// control's own signal dispatch (the common <c>button.Pressed += RefreshView</c>
        /// pattern) reaches here while that very control is still being dispatched;
        /// the engine has it locked, so <c>Free()</c> fails with "Object is locked
        /// and can't be freed" and the node is orphaned forever. Detect that case
        /// and queue the free instead, so the node still dies this frame.
        /// </summary>
        private static void FreeDetached(Node child)
        {
            if (child == null || !GodotObject.IsInstanceValid(child)) return;
            child.Free();
            // Still valid ⇒ the free was refused (locked during its own dispatch).
            if (GodotObject.IsInstanceValid(child) && !child.IsQueuedForDeletion())
                child.QueueFree();
        }

        // ── Optional panel lifecycle hooks ──────────────────────────────────

        /// <summary>
        /// Invokes an optional panel lifecycle hook by name, only when the panel
        /// actually declares it; <paramref name="fallbackMethod"/> is tried when
        /// the primary hook is absent.
        ///
        /// <see cref="IBindablePanel"/> contracts only <c>IsBound</c> and
        /// <c>Unbind()</c>: <c>Open()</c> and <c>RefreshView()</c> are conventions,
        /// not API. An unguarded <c>Call("Open")</c> makes the engine log
        /// "Nonexistent function 'Open'" for every panel that omits the convention
        /// (47 at time of writing) even when the caller swallows the C# exception
        /// — pure error spam that masks real failures. Reflecting first keeps the
        /// engine log clean and lets callers prefer a hook the panel really has.
        /// </summary>
        public static void InvokePanelHook(Node panel, string method, string? fallbackMethod = null)
        {
            if (panel == null || !GodotObject.IsInstanceValid(panel)) return;
            if (TryInvokePanelHook(panel, method)) return;
            if (!string.IsNullOrEmpty(fallbackMethod)) TryInvokePanelHook(panel, fallbackMethod!);
        }

        private static bool TryInvokePanelHook(Node panel, string method)
        {
            var m = panel.GetType().GetMethod(
                method,
                System.Reflection.BindingFlags.Instance
                    | System.Reflection.BindingFlags.Public
                    | System.Reflection.BindingFlags.NonPublic);
            if (m == null || m.GetParameters().Length != 0) return false;
            try
            {
                m.Invoke(panel, null);
            }
            catch (Exception ex)
            {
                // Declared but refused (typically an unbound session). Reported as
                // a warning, not an error: the caller explicitly asked for an
                // optional hook and the panel answered.
                CatalogDiagnostics.Warn(panel.GetType().Name, $"panel hook '{method}'", ex);
            }
            return true;
        }

        /// <summary>
        /// Removes every child except the supplied persistent child.
        /// Useful for detail panes whose title/header is part of the pane
        /// and must survive content refreshes.
        /// </summary>
        public static void EmptyChildrenExcept(Node parent, Node preservedChild)
        {
            if (parent == null || !GodotObject.IsInstanceValid(parent))
                return;

            for (int i = parent.GetChildCount() - 1; i >= 0; i--)
            {
                var child = parent.GetChild(i);
                if (child == preservedChild)
                    continue;

                parent.RemoveChild(child);
                FreeDetached(child);
            }
        }

        // ── Dose formatting (Plan 81 UI audit 81AU/81AX fixes) ─────────
        // Presentation only: the unit authority stays in DoseLedgerSystem (mSv).
        // Values below 0.1 mSv are shown in µSv so the low-rate surface
        // geography added by Plan 81 no longer renders as "0.0 mSv".

        /// <summary>Format a ledger dose value for display. Sub-0.1 mSv values
        /// switch to µSv precision; invalid values render as an honest dash.</summary>
        public static string FormatDoseMsv(float msv)
        {
            if (float.IsNaN(msv) || float.IsInfinity(msv)) return "—";
            if (msv < 0f) msv = 0f;
            if (msv >= 0.1f) return $"{msv:0.0} mSv";
            return $"{msv * 1000f:0.##} µSv";
        }

        /// <summary>Format a nominal/booked reading pair in one shared unit so
        /// the pair stays comparable (audit fix: "0.0/0.0 mSv" hid surface doses).</summary>
        public static string FormatDosePairMsv(float nominalMsv, float bookedMsv)
        {
            if (float.IsNaN(nominalMsv) || float.IsInfinity(nominalMsv)
                || float.IsNaN(bookedMsv) || float.IsInfinity(bookedMsv))
                return "—";
            if (nominalMsv >= 0.1f || bookedMsv >= 0.1f)
                return $"{nominalMsv:0.0}/{bookedMsv:0.0} mSv";
            return $"{nominalMsv * 1000f:0.##}/{bookedMsv * 1000f:0.##} µSv";
        }

        /// <summary>Resolve a reading source ID to the dose location's display
        /// name. Falls back to the raw ID when the catalog does not know the
        /// source (e.g. "demo_scan") — never invents a label.</summary>
        public static string FormatDoseSource(Ashfall.Core.DoseContentCatalog? content, string? sourceId)
        {
            // "Always Show Hazard Text" off → omit humanized exposure labels.
            if (!AtomicWar.GodotApp.Settings.UserSettingsStore.Current.HazardTextLabels)
                return string.IsNullOrEmpty(sourceId) ? string.Empty : sourceId!;

            if (string.IsNullOrEmpty(sourceId)) return "— exposure —";
            if (content?.locations != null)
            {
                for (int i = 0; i < content.locations.Count; i++)
                {
                    var l = content.locations[i];
                    if (l != null && l.id == sourceId && !string.IsNullOrEmpty(l.displayName))
                        return l.displayName;
                }
            }
            return sourceId;
        }
    }
}
