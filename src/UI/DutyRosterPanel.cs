using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Duty Roster panel.
    /// Shows survivor assignments, work shifts, and duty schedules.
    /// </summary>
    public partial class DutyRosterPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblRosterTitle;
        private VBoxContainer _rosterList;

        // Placeholder roster data
        private readonly string[] _placeholderRoster = {
            "Elena — Leader (Command)",
            "Marcus — Medic (Medical Bay)",
            "Yuki — Scout (Perimeter Watch)",
            "David — Engineer (Workshop)",
            "Sofia — Trader (Supply Route)"
        };

        // Real data from host session
        // private DutyRosterHostSession? _rosterHost;

        public void Bind(object roster) // placeholder for DutyRosterHostSession
        {
            // _rosterHost = (DutyRosterHostSession)roster;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_rosterList == null) return;

            // Clear existing roster entries
            while (_rosterList.GetChildCount() > 0)
                _rosterList.RemoveChild(_rosterList.GetChild(0));

            // Display placeholder roster
            foreach (string entry in _placeholderRoster)
            {
                var label = new Label { Text = entry };
                label.CustomMinimumSize = new Vector2(400, 40);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _rosterList.AddChild(label);
            }
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.05f, 0.05f, 0.05f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var container = new CenterContainer();
            container.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(container);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingLg);
            vbox.CustomMinimumSize = new Vector2(500, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("DUTY ROSTER", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblRosterTitle = AshfallUiHelpers.MakeSectionHeader("CURRENT ASSIGNMENTS");
            vbox.AddChild(_lblRosterTitle);

            _rosterList = new VBoxContainer();
            _rosterList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _rosterList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_rosterList);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => OnClose?.Invoke());
            btnClose.CustomMinimumSize = new Vector2(200, 40);
            vbox.AddChild(btnClose);

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
