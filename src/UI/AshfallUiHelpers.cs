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
            return lbl;
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
            return lbl;
        }

        public static Label MakeSubsectionHeader(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeSmall);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Muted));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeBody(string text, bool autowrap = true)
        {
            var lbl = new Label { Text = text };
            if (autowrap)
                lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeSmall(string text, bool autowrap = false)
        {
            var lbl = new Label { Text = text };
            if (autowrap)
                lbl.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeSmall);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeMono(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeMono);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Pale));
            ApplyFont(lbl, FontShareTechMono);
            return lbl;
        }

        public static Label MakeLabel(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeLabel);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Dim));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeLabel(string text, int fontSize, (float r, float g, float b, float a) colorToken)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", fontSize);
            lbl.AddThemeColorOverride("font_color", ToColor(colorToken));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
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
            return lbl;
        }

        public static Label MakeMetadata(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeLabel);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Muted));
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeWarning(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Entropy));
            ApplyFont(lbl, FontBarlowSemiBold);
            return lbl;
        }

        public static Label MakeCritical(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ToColor(Theme.Critical));
            ApplyFont(lbl, FontBarlowSemiBold);
            return lbl;
        }

        // ── Semantic Color Properties ──────────────────────────────────
        public static Color ColorBackdrop => ToColor(Theme.BackdropOverlay);
        public static Color ColorSurface => ToColor(Theme.Surface);
        public static Color ColorSurfaceCard => ToColor(Theme.SurfaceCard);
        public static Color ColorPrimary => ToColor(Theme.Warm);
        public static Color ColorHighlight => ToColor(Theme.Hot);
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
            return lbl;
        }

        public static Label MakeInfo(string text)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", ColorInfo);
            ApplyFont(lbl, FontBarlowRegular);
            return lbl;
        }

        public static Label MakeRadiation(string text, bool acute = false)
        {
            var lbl = new Label { Text = text };
            lbl.AddThemeFontSizeOverride("font_size", Theme.FontSizeBody);
            lbl.AddThemeColorOverride("font_color", acute ? ColorRadiationAcute : ColorRadiation);
            ApplyFont(lbl, FontBarlowSemiBold);
            return lbl;
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
            var tex = TryLoadTexture("res://assets/ui/Textures/tab_strip.png")
                   ?? TryLoadTexture("res://assets/ui/tab_strip.svg")
                   ?? TryLoadTexture("res://assets/ui/Textures/frame_9slice.png");
            if (tex != null)
            {
                return new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 12,
                    TextureMarginTop = 8,
                    TextureMarginRight = 12,
                    TextureMarginBottom = 8
                };
            }
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
            sep.AddThemeConstantOverride("separation", 6);
            return sep;
        }

        // ── Buttons ─────────────────────────────────────────────────────

        public static Button MakeButton(string text, Action onPressed, bool disabled = false)
        {
            var btn = new Button
            {
                Text = text,
                Disabled = disabled,
                CustomMinimumSize = new Vector2(0, Theme.FontSizeBody + Theme.SpacingMd)
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
            return btn;
        }

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
            return lbl;
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
            return lbl;
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
            rect.Texture = AssetRegistry.GetItem(itemId).Texture
                        ?? AssetRegistry.GetItem(key).Texture
                        ?? TryLoadTexture($"res://assets/art/{key}.jpg")
                        ?? TryLoadTexture($"res://assets/art/{itemId}.jpg")
                        ?? TryLoadTexture($"res://assets/art/{key}.png")
                        ?? TryLoadTexture($"res://assets/art/{itemId}.png")
                        ?? TryLoadTexture($"res://assets/ui/Icons/icon_pill_dependency.svg");
            return rect;
        }

        // ── Color Conversion ────────────────────────────────────────────

        public static Color ToColor((float r, float g, float b, float a) token)
        {
            return new Color(token.r, token.g, token.b, token.a);
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
        public static void EmptyChildren(Node parent)
        {
            if (parent == null || !GodotObject.IsInstanceValid(parent))
                return;

            // Bound the loop defensively against free-during-iteration races;
            // safety counter guards against pathological parents whose
            // GetChild() invariant breaks under teardown.
            int safety = parent.GetChildCount() + 8;
            while (parent.GetChildCount() > 0 && safety-- > 0)
            {
                var child = parent.GetChild(0);
                parent.RemoveChild(child);
                child.Free();
            }
        }
    }
}
