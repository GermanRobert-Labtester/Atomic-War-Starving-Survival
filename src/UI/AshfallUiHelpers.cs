using System;
using Godot;
using Ashfall.Core.UI;
using Theme = Ashfall.Core.UI.Theme;

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
        // ── Font Loading ────────────────────────────────────────────────
        // Lazy-loaded canonical fonts.  Each property loads on first access
        // and caches the result.  Returns null when the resource is missing
        // so callers can fall back to Godot's default system font.

        private static FontFile _fontBarlowRegular;
        private static FontFile _fontBarlowSemiBold;
        private static FontFile _fontBarlowBold;
        private static FontFile _fontShareTechMono;

        /// <summary>
        /// Loads a FontFile from a res:// path. Returns null on failure.
        /// </summary>
        public static FontFile LoadFont(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            try
            {
                if (ResourceLoader.Exists(path))
                    return ResourceLoader.Load<FontFile>(path);
            }
            catch (Exception e)
            {
                GD.PrintErr($"[AshfallUiHelpers] Failed to load font '{path}': {e.Message}");
            }
            return null;
        }

        public static FontFile FontBarlowRegular =>
            _fontBarlowRegular ??= LoadFont("res://assets/fonts/BarlowCondensed-Regular.ttf");

        public static FontFile FontBarlowSemiBold =>
            _fontBarlowSemiBold ??= LoadFont("res://assets/fonts/BarlowCondensed-SemiBold.ttf");

        public static FontFile FontBarlowBold =>
            _fontBarlowBold ??= LoadFont("res://assets/fonts/BarlowCondensed-Bold.ttf");

        public static FontFile FontShareTechMono =>
            _fontShareTechMono ??= LoadFont("res://assets/fonts/ShareTechMono-Regular.ttf");

        /// <summary>
        /// Applies a font override to a label. No-op when font is null
        /// (falls back to Godot's default system font).
        /// </summary>
        public static void ApplyFont(Label label, FontFile font)
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

        // ── Panel Shells ────────────────────────────────────────────────

        /// <summary>
        /// Creates a PanelContainer with the standard 9-slice background
        /// (panel_bg_9slice.png, 16px border) and internal padding.
        /// </summary>
        public static PanelContainer MakePanel(int minWidth = 0, int minHeight = 0)
        {
            var panel = new PanelContainer();
            if (minWidth > 0 || minHeight > 0)
                panel.CustomMinimumSize = new Vector2(minWidth, minHeight);

            var tex = TryLoadTexture("res://Assets/UI/Textures/panel_bg_9slice.png");
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
                // Fallback: flat style matching Theme.Ink
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

        /// <summary>
        /// Creates the standard header bar with 9-slice background.
        /// </summary>
        public static PanelContainer MakeHeaderBar()
        {
            var header = new PanelContainer();
            var tex = TryLoadTexture("res://Assets/UI/Textures/header_bar_9slice.png");
            if (tex != null)
            {
                var sb = new StyleBoxTexture
                {
                    Texture = tex,
                    TextureMarginLeft = 12,
                    TextureMarginTop = 8,
                    TextureMarginRight = 12,
                    TextureMarginBottom = 8
                };
                header.AddThemeStyleboxOverride("panel", sb);
            }
            else
            {
                var sb = new StyleBoxFlat
                {
                    BgColor = new Color(Theme.Ink.r, Theme.Ink.g, Theme.Ink.b, 0.95f),
                    BorderColor = ToColor(Theme.Line),
                };
                sb.SetBorderWidthAll(1);
                header.AddThemeStyleboxOverride("panel", sb);
            }
            return header;
        }

        // ── Separators ──────────────────────────────────────────────────

        public static HSeparator MakeSeparator()
        {
            var sep = new HSeparator();
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
            btn.Pressed += () => onPressed?.Invoke();
            return btn;
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
            row.AddChild(lbl);

            var val = new Label { Text = value, HorizontalAlignment = HorizontalAlignment.Right };
            val.AddThemeFontSizeOverride("font_size", fontSize);
            val.AddThemeColorOverride("font_color", valueColor ?? ToColor(Theme.Pale));
            row.AddChild(val);

            return row;
        }

        // ── Faction Emblem ──────────────────────────────────────────────

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

        // ── Color Conversion ────────────────────────────────────────────

        public static Color ToColor((float r, float g, float b, float a) token)
        {
            return new Color(token.r, token.g, token.b, token.a);
        }

        // ── Texture Loading ─────────────────────────────────────────────

        public static Texture2D? TryLoadTexture(string path)
        {
            if (string.IsNullOrEmpty(path)) return null;
            if (ResourceLoader.Exists(path))
                return ResourceLoader.Load<Texture2D>(path);

            string osPath = ProjectSettings.GlobalizePath(path);
            if (System.IO.File.Exists(osPath))
            {
                var img = Godot.Image.LoadFromFile(osPath);
                if (img != null) return ImageTexture.CreateFromImage(img);
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
    }
}
