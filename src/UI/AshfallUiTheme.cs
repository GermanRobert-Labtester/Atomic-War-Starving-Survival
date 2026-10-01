// SPDX-License-Identifier: MIT
using Godot;
using AtomicWar.GodotApp.Localization;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL global UI theme (UI/UX audit 2026-09-25).
    ///
    /// Precision/consistency seam: panels built through <see cref="AshfallUiHelpers"/>
    /// already carry explicit ASHFALL styleboxes, but ~58 panel files construct
    /// raw <c>new Button</c> / <c>new LineEdit</c> / <c>new ItemList</c> nodes and
    /// previously fell through to Godot's light default theme — inconsistent
    /// chrome and, for buttons, no visible keyboard focus. This theme installs
    /// the same ASHFALL visual language as class defaults on the Window root, so
    /// every control of these types renders coherently unless a panel
    /// deliberately overrides it.
    ///
    /// Values mirror the canonical helper fallbacks (MakeButton flat family,
    /// AshfallFocusPolicy focus box) so per-node overrides and theme defaults
    /// cannot drift apart. Install is idempotent and respects an existing theme.
    /// </summary>
    public static class AshfallUiTheme
    {
        private static Godot.Theme? _cached;

        /// <summary>
        /// Installs the ASHFALL class-default theme on the active Window root.
        /// No-op when a theme is already present on that root.
        /// </summary>
        public static void Install(Node context)
        {
            Window? root = context?.GetTree()?.Root;
            if (root == null || root.Theme != null)
                return;

            _cached ??= Build();
            root.Theme = _cached;
        }

        /// <summary>
        /// Installs the theme on an explicit host Control. Used by the snapshot
        /// orchestrator so captures render the same class defaults as the live
        /// UI (its panels live in an isolated SubViewport that never runs the
        /// main window bootstrap).
        /// </summary>
        public static void InstallOn(Control host)
        {
            if (host == null || !GodotObject.IsInstanceValid(host) || host.Theme != null)
                return;

            _cached ??= Build();
            host.Theme = _cached;
        }

        /// <summary>
        /// Normalizes directly-constructed controls to the audit defaults
        /// (a11y target-size sweep 2, 2026-09-29). Panels build ~230 buttons
        /// and many fixed-width labels with raw <c>new</c>, bypassing
        /// AshfallUiHelpers — walking the subtree at panel-open time gives
        /// them the same 28px interactive-target floor and fixed-width
        /// ellipsis clipping the factories apply, without touching layout
        /// intent elsewhere. Idempotent: already-compliant controls pass
        /// through unchanged, and larger explicit sizes win.
        /// </summary>
        public static void EnforceControlDefaults(Node? root)
        {
            if (root == null) return;

            if (root is Button button)
            {
                if (button.CustomMinimumSize.Y < DesignTheme.MinInteractiveHeight)
                {
                    button.CustomMinimumSize = new Vector2(
                        button.CustomMinimumSize.X, DesignTheme.MinInteractiveHeight);
                }

                // Disabled-control affordance (audit convention: a disabled control
                // must never be silently unexplained). This is a *fallback* only —
                // panels that know the real reason should author a tooltip, and an
                // authored tooltip is never overwritten. Applied at the shared open
                // seam so it also covers panels built with raw `new`.
                if (button.Disabled && string.IsNullOrEmpty(button.TooltipText))
                    button.TooltipText = AshfallLocalization.Tr(
                        "ui.tooltip.prerequisites_unmet",
                        "Currently unavailable — prerequisites not met.");
            }
            else if (root is LineEdit edit
                && edit.CustomMinimumSize.Y < DesignTheme.MinInteractiveHeight)
            {
                // Single-line inputs are interactive controls under the same
                // 28px audit rule (pkg 12, 2026-09-29); SpinBox benefits via
                // its internal LineEdit child. Multiline TextEdit is exempt.
                edit.CustomMinimumSize = new Vector2(
                    edit.CustomMinimumSize.X, DesignTheme.MinInteractiveHeight);
            }
            else if (root is Label label
                && !label.ClipText
                && label.AutowrapMode == TextServer.AutowrapMode.Off
                && label.CustomMinimumSize.X > 0f)
            {
                // Fixed-width, non-wrapping label: clip with an ellipsis
                // instead of silently drawing past the rect.
                label.ClipText = true;
                label.TextOverrunBehavior = TextServer.OverrunBehavior.TrimEllipsis;
            }

            foreach (Node child in root.GetChildren())
                EnforceControlDefaults(child);
        }

        private static Godot.Theme Build()
        {
            var theme = new Godot.Theme();

            Color ink = ToColor(DesignTheme.Ink);
            Color line = ToColor(DesignTheme.Line);
            Color lineSoft = ToColor(DesignTheme.LineSoft);
            Color warm = ToColor(DesignTheme.Warm);
            Color hot = ToColor(DesignTheme.Hot);
            Color pale = ToColor(DesignTheme.Pale);
            Color muted = ToColor(DesignTheme.Muted);
            Color dim = ToColor(DesignTheme.Dim);

            // ── Focus indicator (identical values to AshfallFocusPolicy) ──
            var focusBox = new StyleBoxFlat
            {
                BgColor = new Color(hot.R, hot.G, hot.B, 0.08f),
                BorderColor = hot,
                DrawCenter = true
            };
            focusBox.SetBorderWidthAll(2);
            focusBox.SetCornerRadiusAll(0);

            // ── Button family (OptionButton / CheckButton / CheckBox inherit) ──
            var buttonNormal = Flat(new Color(ink.R, ink.G, ink.B, 0.65f), line);
            var buttonHover = Flat(new Color(warm.R, warm.G, warm.B, 0.18f), warm);
            var buttonPressed = Flat(new Color(warm.R, warm.G, warm.B, 0.30f), hot);
            var buttonDisabled = Flat(new Color(ink.R, ink.G, ink.B, 0.30f), lineSoft);

            theme.SetStylebox("normal", "Button", buttonNormal);
            theme.SetStylebox("hover", "Button", buttonHover);
            theme.SetStylebox("pressed", "Button", buttonPressed);
            theme.SetStylebox("disabled", "Button", buttonDisabled);
            theme.SetStylebox("focus", "Button", focusBox);

            theme.SetColor("font_color", "Button", pale);
            theme.SetColor("font_hover_color", "Button", hot);
            theme.SetColor("font_pressed_color", "Button", hot);
            theme.SetColor("font_focus_color", "Button", pale);
            theme.SetColor("font_disabled_color", "Button", dim);
            theme.SetFontSize("font_size", "Button", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "Button", AshfallUiHelpers.FontBarlowSemiBold);

            // ── LineEdit ──
            var editNormal = Flat(new Color(ink.R, ink.G, ink.B, 0.65f), line);
            var editFocus = Flat(new Color(ink.R, ink.G, ink.B, 0.85f), hot, borderWidth: 2);
            var editReadOnly = Flat(new Color(ink.R, ink.G, ink.B, 0.35f), lineSoft);
            editNormal.ContentMarginLeft = DesignTheme.SpacingSm;
            editNormal.ContentMarginRight = DesignTheme.SpacingSm;
            editFocus.ContentMarginLeft = DesignTheme.SpacingSm;
            editFocus.ContentMarginRight = DesignTheme.SpacingSm;
            editReadOnly.ContentMarginLeft = DesignTheme.SpacingSm;
            editReadOnly.ContentMarginRight = DesignTheme.SpacingSm;

            theme.SetStylebox("normal", "LineEdit", editNormal);
            theme.SetStylebox("focus", "LineEdit", editFocus);
            theme.SetStylebox("read_only", "LineEdit", editReadOnly);
            theme.SetColor("font_color", "LineEdit", pale);
            theme.SetColor("font_placeholder_color", "LineEdit", dim);
            theme.SetColor("font_uneditable_color", "LineEdit", muted);
            theme.SetColor("caret_color", "LineEdit", hot);
            theme.SetColor("selection_color", "LineEdit", new Color(warm.R, warm.G, warm.B, 0.35f));
            theme.SetFontSize("font_size", "LineEdit", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "LineEdit", AshfallUiHelpers.FontBarlowRegular);

            // ── TextEdit ──
            var textNormal = Flat(new Color(ink.R, ink.G, ink.B, 0.65f), line);
            var textFocus = Flat(new Color(ink.R, ink.G, ink.B, 0.85f), hot, borderWidth: 2);
            var textReadOnly = Flat(new Color(ink.R, ink.G, ink.B, 0.35f), lineSoft);
            foreach (var box in new[] { textNormal, textFocus, textReadOnly })
            {
                box.ContentMarginLeft = DesignTheme.SpacingSm;
                box.ContentMarginRight = DesignTheme.SpacingSm;
                box.ContentMarginTop = DesignTheme.SpacingXs;
                box.ContentMarginBottom = DesignTheme.SpacingXs;
            }

            theme.SetStylebox("normal", "TextEdit", textNormal);
            theme.SetStylebox("focus", "TextEdit", textFocus);
            theme.SetStylebox("read_only", "TextEdit", textReadOnly);
            theme.SetColor("font_color", "TextEdit", pale);
            theme.SetColor("font_placeholder_color", "TextEdit", dim);
            theme.SetColor("caret_color", "TextEdit", hot);
            theme.SetColor("selection_color", "TextEdit", new Color(warm.R, warm.G, warm.B, 0.35f));
            theme.SetFontSize("font_size", "TextEdit", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "TextEdit", AshfallUiHelpers.FontBarlowRegular);

            // ── ItemList ──
            var listPanel = Flat(new Color(ink.R, ink.G, ink.B, 0.80f), line);
            var listSelected = Flat(new Color(warm.R, warm.G, warm.B, 0.25f), warm);
            var listCursor = Flat(new Color(warm.R, warm.G, warm.B, 0.18f), hot);

            theme.SetStylebox("panel", "ItemList", listPanel);
            theme.SetStylebox("selected", "ItemList", listSelected);
            theme.SetStylebox("selected_focus", "ItemList", listSelected);
            theme.SetStylebox("cursor", "ItemList", listCursor);
            theme.SetStylebox("cursor_unfocused", "ItemList", listCursor);
            theme.SetStylebox("focus", "ItemList", focusBox);
            theme.SetColor("font_color", "ItemList", pale);
            theme.SetColor("font_selected_color", "ItemList", hot);
            theme.SetColor("font_disabled_color", "ItemList", dim);
            theme.SetFontSize("font_size", "ItemList", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "ItemList", AshfallUiHelpers.FontBarlowRegular);

            // ── PopupMenu (OptionButton dropdowns, context menus) ──
            theme.SetStylebox("panel", "PopupMenu", Flat(new Color(ink.R, ink.G, ink.B, 0.97f), line));
            theme.SetStylebox("hover", "PopupMenu", Flat(new Color(warm.R, warm.G, warm.B, 0.22f), warm));
            theme.SetStylebox("separator", "PopupMenu", Flat(lineSoft, lineSoft));
            theme.SetColor("font_color", "PopupMenu", pale);
            theme.SetColor("font_hover_color", "PopupMenu", hot);
            theme.SetColor("font_disabled_color", "PopupMenu", dim);
            theme.SetColor("font_separator_color", "PopupMenu", muted);
            theme.SetFontSize("font_size", "PopupMenu", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "PopupMenu", AshfallUiHelpers.FontBarlowRegular);

            // ── Slider (HSlider/VSlider inherit) ──
            // Godot's Slider has no `focus` stylebox: the focus affordance is the
            // highlighted grabber/area, so those use Hot while the rest state uses
            // Warm. Grabbers are generated flat squares (brutalist, no round
            // default art dependency).
            theme.SetStylebox("slider", "Slider", Flat(new Color(ink.R, ink.G, ink.B, 0.85f), line));
            theme.SetStylebox("grabber_area", "Slider", Flat(warm));
            theme.SetStylebox("grabber_area_highlight", "Slider", Flat(hot));
            theme.SetIcon("grabber", "Slider", MakeFlatTexture(12, warm, line));
            theme.SetIcon("grabber_highlight", "Slider", MakeFlatTexture(12, hot, hot));
            theme.SetIcon("grabber_disabled", "Slider", MakeFlatTexture(12, dim, lineSoft));

            // ── ScrollBar (HScrollBar/VScrollBar inherit) ──
            theme.SetStylebox("scroll", "ScrollBar", Flat(new Color(ink.R, ink.G, ink.B, 0.90f), lineSoft));
            theme.SetStylebox("grabber", "ScrollBar", Flat(new Color(line.R, line.G, line.B, 0.85f), line));
            theme.SetStylebox("grabber_highlight", "ScrollBar", Flat(warm));
            theme.SetStylebox("grabber_pressed", "ScrollBar", Flat(hot));

            // ── Tooltips ──
            // Without these, tooltips render Godot's light default bubble on
            // ink panels (pkg 13, 2026-09-29).
            theme.SetStylebox("panel", "TooltipPanel", Flat(new Color(ink.R, ink.G, ink.B, 0.97f), line));
            theme.SetColor("font_color", "TooltipLabel", pale);
            theme.SetFontSize("font_size", "TooltipLabel", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "TooltipLabel", AshfallUiHelpers.FontBarlowRegular);

            // ── Separators (HSeparator/VSeparator inherit) ──
            theme.SetStylebox("separator", "HSeparator", Flat(lineSoft));
            theme.SetStylebox("separator", "VSeparator", Flat(lineSoft));

            // ── Label (base type) ──
            // Factory labels override per-label; this brands the ~359 direct
            // `new Label` sites that bypass the factory seam (pkg 13).
            theme.SetColor("font_color", "Label", pale);
            theme.SetFontSize("font_size", "Label", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "Label", AshfallUiHelpers.FontBarlowRegular);

            // ── ProgressBar (a11y series pkg 11, 2026-09-29) ──
            // ProgressBar's class chain never reaches Button/LineEdit, so its
            // background/fill styleboxes resolved to Godot's light default art
            // on every direct `new ProgressBar` site.
            theme.SetStylebox("background", "ProgressBar", Flat(new Color(ink.R, ink.G, ink.B, 0.85f), line));
            theme.SetStylebox("fill", "ProgressBar", Flat(warm));
            theme.SetColor("font_color", "ProgressBar", pale);
            theme.SetColor("font_background_color", "ProgressBar", muted);
            theme.SetFontSize("font_size", "ProgressBar", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "ProgressBar", AshfallUiHelpers.FontBarlowSemiBold);

            // ── CheckBox / CheckButton ──
            // Text entries fall back to Button; the check icons are
            // type-specific and previously showed Godot's default art.
            var checkIcon = MakeFlatTexture(14, hot, ink);
            var uncheckedIcon = MakeFlatTexture(14, new Color(ink.R, ink.G, ink.B, 0.65f), line);
            foreach (var type in new[] { "CheckBox", "CheckButton" })
            {
                theme.SetIcon("checked", type, checkIcon);
                theme.SetIcon("unchecked", type, uncheckedIcon);
                theme.SetIcon("radio_checked", type, checkIcon);
                theme.SetIcon("radio_unchecked", type, uncheckedIcon);
            }

            // ── OptionButton / SpinBox ──
            // Styleboxes fall back to Button/LineEdit; the arrows do not.
            theme.SetIcon("arrow", "OptionButton", MakeFlatTexture(10, hot, hot));
            theme.SetIcon("updown", "SpinBox", MakeFlatTexture(12, warm, line));

            // ── TabContainer / TabBar ──
            var tabSelected = Flat(new Color(warm.R, warm.G, warm.B, 0.30f), hot);
            var tabUnselected = Flat(new Color(ink.R, ink.G, ink.B, 0.65f), line);
            theme.SetStylebox("panel", "TabContainer", Flat(new Color(ink.R, ink.G, ink.B, 0.90f), lineSoft));
            theme.SetStylebox("tab_selected", "TabContainer", tabSelected);
            theme.SetStylebox("tab_unselected", "TabContainer", tabUnselected);
            theme.SetColor("font_selected_color", "TabContainer", pale);
            theme.SetColor("font_unselected_color", "TabContainer", muted);
            theme.SetFontSize("font_size", "TabContainer", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "TabContainer", AshfallUiHelpers.FontBarlowSemiBold);
            theme.SetStylebox("tab_selected", "TabBar", tabSelected);
            theme.SetStylebox("tab_unselected", "TabBar", tabUnselected);
            theme.SetColor("font_selected_color", "TabBar", pale);
            theme.SetColor("font_unselected_color", "TabBar", muted);
            theme.SetFontSize("font_size", "TabBar", DesignTheme.FontSizeBody);
            SetFontIfAvailable(theme, "TabBar", AshfallUiHelpers.FontBarlowSemiBold);

            // ── RichTextLabel ──
            theme.SetColor("default_color", "RichTextLabel", pale);
            theme.SetFontSize("normal_font_size", "RichTextLabel", DesignTheme.FontSizeBody);
            theme.SetFontSize("bold_font_size", "RichTextLabel", DesignTheme.FontSizeBody);
            theme.SetFontSize("mono_font_size", "RichTextLabel", DesignTheme.FontSizeMono);
            SetFontIfAvailable(theme, "RichTextLabel", AshfallUiHelpers.FontBarlowRegular);
            if (AshfallUiHelpers.FontBarlowSemiBold != null)
                theme.SetFont("bold_font", "RichTextLabel", AshfallUiHelpers.FontBarlowSemiBold);
            if (AshfallUiHelpers.FontShareTechMono != null)
                theme.SetFont("mono_font", "RichTextLabel", AshfallUiHelpers.FontShareTechMono);

            return theme;
        }

        private static ImageTexture MakeFlatTexture(int size, Color fill, Color border)
        {
            var image = Image.CreateEmpty(size, size, false, Image.Format.Rgba8);
            image.Fill(fill);
            if (border != fill)
            {
                for (int i = 0; i < size; i++)
                {
                    image.SetPixel(i, 0, border);
                    image.SetPixel(i, size - 1, border);
                    image.SetPixel(0, i, border);
                    image.SetPixel(size - 1, i, border);
                }
            }
            return ImageTexture.CreateFromImage(image);
        }

        private static StyleBoxFlat Flat(Color bg, Color? border = null, int borderWidth = 1)
        {
            var sb = new StyleBoxFlat { BgColor = bg };
            if (border.HasValue)
            {
                sb.BorderColor = border.Value;
                sb.SetBorderWidthAll(borderWidth);
            }
            sb.SetCornerRadiusAll(0); // Brutalist sharp, matches Theme.RadiusSm
            return sb;
        }

        private static void SetFontIfAvailable(Godot.Theme theme, string type, FontFile? font)
        {
            if (font != null)
                theme.SetFont("font", type, font);
        }

        private static Color ToColor((float r, float g, float b, float a) token)
            => AshfallUiHelpers.ToColor(token);
    }
}
