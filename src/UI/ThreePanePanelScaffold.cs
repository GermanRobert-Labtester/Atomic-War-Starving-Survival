// SPDX-License-Identifier: MIT
using System;
using Godot;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// Shared builders for the three-column telemetry / controls / data panel
    /// family (RAD/AQUI/... consoles). The frame, telemetry-row, and column
    /// mount were byte-identical private copies in 18 panels; a single owner
    /// keeps frame styling, row styling, and the "Margin" mount contract in
    /// sync. Panels keep their own content, state, and refresh logic.
    /// </summary>
    public static class ThreePanePanelScaffold
    {
        public static PanelContainer CreatePanelFrame(string headerText)
        {
            var panel = new PanelContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill, SizeFlagsVertical = Control.SizeFlags.ExpandFill };
            var vbox = new VBoxContainer();
            panel.AddChild(vbox);

            var title = new Label
            {
                Text = headerText
            };
            title.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Pale));
            vbox.AddChild(title);

            var margin = new MarginContainer { Name = "Margin", SizeFlagsVertical = Control.SizeFlags.ExpandFill };
            margin.AddThemeConstantOverride("margin_left", 8);
            margin.AddThemeConstantOverride("margin_top", 8);
            margin.AddThemeConstantOverride("margin_right", 8);
            margin.AddThemeConstantOverride("margin_bottom", 8);
            vbox.AddChild(margin);

            return panel;
        }

        public static HBoxContainer CreateTelemetryRow(string label, string value, Color valueColor)
        {
            var hbox = new HBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            var lbl = new Label { Text = label, SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            lbl.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            var val = new Label { Text = value };
            val.AddThemeColorOverride("font_color", valueColor);
            hbox.AddChild(lbl);
            hbox.AddChild(val);
            return hbox;
        }

        /// <summary>
        /// Build a column container and mount it into the frame's "Margin"
        /// node (the contract <see cref="CreatePanelFrame"/> establishes).
        /// Returns the column for content rows.
        /// </summary>
        public static VBoxContainer CreateColumn(PanelContainer frame, int separation)
        {
            var column = new VBoxContainer { SizeFlagsVertical = Control.SizeFlags.ExpandFill };
            column.AddThemeConstantOverride("separation", separation);
            frame.GetChild<VBoxContainer>(0).GetNode<MarginContainer>("Margin").AddChild(column);
            return column;
        }

        /// <summary>
        /// Assembled chrome references handed back to the owning panel.
        /// The panel keeps its content, state, and refresh logic; the scaffold
        /// owns backdrop, margins, header, body slot, and diagnostics log.
        /// </summary>
        public sealed class PanelChrome
        {
            public VBoxContainer Main = null!;
            public HBoxContainer Body = null!;
            public Label Title = null!;
            public Label Status = null!;
            public Button Close = null!;
            public Label Log = null!;
        }

        /// <summary>
        /// Build the full panel chrome (backdrop, 24px root margin, header bar,
        /// body slot, diagnostics log) and mount it on <paramref name="host"/>.
        /// Close hides the host and forwards to <paramref name="onClose"/>.
        /// </summary>
        public static PanelChrome BuildChrome(Control host, string titleText, string statusText, Color statusColor, string closeText, string logText, Action onClose)
        {
            var bg = new ColorRect { Color = AshfallUiHelpers.ToColor(DesignTheme.Ink) };
            bg.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            host.AddChild(bg);

            var rootMargin = new MarginContainer();
            rootMargin.SetAnchorsPreset(Control.LayoutPreset.FullRect);
            rootMargin.AddThemeConstantOverride("margin_left", 24);
            rootMargin.AddThemeConstantOverride("margin_top", 24);
            rootMargin.AddThemeConstantOverride("margin_right", 24);
            rootMargin.AddThemeConstantOverride("margin_bottom", 24);
            host.AddChild(rootMargin);

            var mainVBox = new VBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill, SizeFlagsVertical = Control.SizeFlags.ExpandFill };
            mainVBox.AddThemeConstantOverride("separation", 16);
            rootMargin.AddChild(mainVBox);

            // Top Header Bar
            var headerHBox = new HBoxContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill };
            var titleLabel = new Label
            {
                Text = titleText,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill
            };
            titleLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            headerHBox.AddChild(titleLabel);

            var statusLabel = new Label
            {
                Text = statusText
            };
            statusLabel.AddThemeColorOverride("font_color", statusColor);
            headerHBox.AddChild(statusLabel);

            var closeButton = new Button { Text = closeText };
            closeButton.Pressed += () =>
            {
                host.Visible = false;
                onClose?.Invoke();
            };
            headerHBox.AddChild(closeButton);
            mainVBox.AddChild(headerHBox);

            // Three-Column High-Density Grid
            var bodyHBox = new HBoxContainer
            {
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                SizeFlagsVertical = Control.SizeFlags.ExpandFill
            };
            bodyHBox.AddThemeConstantOverride("separation", 16);
            mainVBox.AddChild(bodyHBox);

            // Bottom Diagnostics Log
            var logPanel = new PanelContainer { SizeFlagsHorizontal = Control.SizeFlags.ExpandFill, CustomMinimumSize = new Vector2(0, 100) };
            var logMargin = new MarginContainer();
            logMargin.AddThemeConstantOverride("margin_left", 12);
            logMargin.AddThemeConstantOverride("margin_top", 8);
            logMargin.AddThemeConstantOverride("margin_right", 12);
            logMargin.AddThemeConstantOverride("margin_bottom", 8);
            logPanel.AddChild(logMargin);

            var logLabel = new Label
            {
                Text = logText,
                AutowrapMode = TextServer.AutowrapMode.WordSmart,
                SizeFlagsHorizontal = Control.SizeFlags.ExpandFill,
                SizeFlagsVertical = Control.SizeFlags.ExpandFill
            };
            logLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Dim));
            logMargin.AddChild(logLabel);
            mainVBox.AddChild(logPanel);

            return new PanelChrome
            {
                Main = mainVBox,
                Body = bodyHBox,
                Title = titleLabel,
                Status = statusLabel,
                Close = closeButton,
                Log = logLabel
            };
        }
    }
}
