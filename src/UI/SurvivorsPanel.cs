using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Survivors panel.
    /// Shows survivor roster, needs, and radiation status.
    /// Placeholder implementation for UI architecture demonstration.
    /// </summary>
    public partial class SurvivorsPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblSurvivorsTitle;
        private VBoxContainer _survivorList;

        // Placeholder survivor data
        private readonly string[] _survivorNames = {
            "Elena (Leader)", "Marcus (Medic)", "Yuki (Scout)",
            "David (Engineer)", "Sofia (Trader)"
        };

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            // Background overlay
            var bg = new ColorRect
            {
                Color = new Color(0.05f, 0.05f, 0.05f, 0.92f)
            };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            // Content container
            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(500, 0);
            container.AddChild(vbox);

            // Title
            _lblSurvivorsTitle = AshfallUiHelpers.MakeTitle("SURVIVORS", Ashfall.Core.UI.Theme.FontSizeH1);
            _lblSurvivorsTitle.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(_lblSurvivorsTitle);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Survivor list
            _survivorList = new VBoxContainer();
            _survivorList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingMd);
            _survivorList.CustomMinimumSize = new Vector2(450, 0);

            foreach (string name in _survivorNames)
            {
                var survivorRow = new HBoxContainer();
                survivorRow.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);

                var nameLabel = AshfallUiHelpers.MakeSmall(name);
                nameLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                survivorRow.AddChild(nameLabel);

                // Placeholder status indicators
                var statusLabel = AshfallUiHelpers.MakeSmall("● Healthy");
                statusLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                survivorRow.AddChild(statusLabel);

                _survivorList.AddChild(survivorRow);
            }

            vbox.AddChild(_survivorList);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Stats summary
            var statsGroup = new VBoxContainer();
            statsGroup.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);

            var healthStats = AshfallUiHelpers.MakeSmall("Average Health: 85%");
            healthStats.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
            statsGroup.AddChild(healthStats);

            var avgRad = AshfallUiHelpers.MakeSmall("Average Radiation: 12 mSv");
            avgRad.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
            statsGroup.AddChild(avgRad);

            var morale = AshfallUiHelpers.MakeSmall("Morale: Stable");
            morale.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
            statsGroup.AddChild(morale);

            vbox.AddChild(statsGroup);
            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Close button
            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

            // Keyboard shortcut
            var hint = AshfallUiHelpers.MakeSmall("[Esc] to close");
            hint.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeLabel);
            hint.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Dim));
            vbox.AddChild(hint);
        }

        public void Open()
        {
            Visible = true;
            QueueRedraw();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;

            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                OnClose?.Invoke();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
