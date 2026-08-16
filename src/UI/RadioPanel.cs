using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radio panel.
    /// Shows radio signals, broadcasts, and communication logs.
    /// </summary>
    public partial class RadioPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblSignalsTitle;
        private VBoxContainer _signalList;

        // Placeholder signals
        private readonly string[] _placeholderSignals = {
            "[08:42] Unknown frequency - static",
            "[11:15] Distress signal - coordinates unknown",
            "[14:30] Supply drop coordinates received",
            "[18:05] Radio tower operational - sector 7",
            "[22:17] Interference detected - possible EMP"
        };

        // Real data from host session
        // private RadioHostSession? _radioHost;

        public void Bind(object radio) // placeholder for future RadioHostSession binding
        {
            // _radioHost = (RadioHostSession)radio;
            // RefreshView();
        }

        public void RefreshView()
        {
            if (_signalList == null) return;

            // Clear existing signals
            while (_signalList.GetChildCount() > 0)
            {
                _signalList.RemoveChild(_signalList.GetChild(0));
            }

            // Fall back to placeholders (RadioHostSession not yet implemented)
            foreach (string signal in _placeholderSignals)
            {
                var signalLabel = new Label { Text = signal };
                signalLabel.CustomMinimumSize = new Vector2(450, 40);
                signalLabel.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                signalLabel.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _signalList.AddChild(signalLabel);
            }
        }

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
            var title = AshfallUiHelpers.MakeTitle("RADIO COMMUNICATIONS", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Signals section
            _lblSignalsTitle = AshfallUiHelpers.MakeSectionHeader("RECENT SIGNALS");
            vbox.AddChild(_lblSignalsTitle);

            _signalList = new VBoxContainer();
            _signalList.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _signalList.CustomMinimumSize = new Vector2(450, 0);
            vbox.AddChild(_signalList);

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
