using System;
using Ashfall.Core.Radio;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radio panel.
    /// Shows radio signals, broadcasts, and communication logs with 142.850 MHz broadcast styling.
    /// </summary>
    public partial class RadioPanel : Control
    {
        public event Action? OnClose;
        public event Action? OnRadioBroadcastSent;

        private readonly (float Freq, string Label)[] _presets =
        {
            (142.850f, "142.850 MHz · COLD COUNT"),
            (104.200f, "104.200 MHz · HYDRO-BARONS"),
            (98.500f, "098.500 MHz · SCAVENGER NET"),
            (120.400f, "120.400 MHz · DISTRESS BEACON")
        };

        private Label _lblSignalsTitle = null!;
        private VBoxContainer _signalList = null!;
        private RadioHostSession? _radioHost;

        public bool IsBound => _radioHost != null;
        public int RenderedSignalCount => _signalList?.GetChildCount() ?? 0;

        public void Bind(RadioHostSession radio)
        {
            _radioHost = radio;
            RefreshView();
        }

        public void RefreshView()
        {
            if (_signalList == null) return;

            while (_signalList.GetChildCount() > 0)
            {
                var child = _signalList.GetChild(0);
                _signalList.RemoveChild(child);
                child.QueueFree();
            }

            if (_radioHost == null)
            {
                _signalList.AddChild(AshfallUiHelpers.MakeMetadata("No radio session bound. Tuner offline."));
                return;
            }

            if (_radioHost.History.Count == 0)
            {
                _signalList.AddChild(AshfallUiHelpers.MakeMetadata($"No transmissions recorded. Carrier frequency scanning on {_radioHost.CurrentFrequency:00.00} MHz."));
            }
            else
            {
                int first = Math.Max(0, _radioHost.History.Count - 16);
                for (int i = _radioHost.History.Count - 1; i >= first; i--)
                {
                    RadioIntercept signal = _radioHost.History[i];
                    string source = string.IsNullOrWhiteSpace(signal.FactionId)
                        ? signal.Callsign
                        : $"{signal.Callsign} · {signal.FactionId}";

                    var box = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingXs);
                    var headerRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);

                    var freq = AshfallUiHelpers.MakeMono($"[D{signal.Day:00} {signal.FrequencyMhz:00.00} MHz]");
                    freq.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                    headerRow.AddChild(freq);

                    var callsign = AshfallUiHelpers.MakeSmall(source);
                    callsign.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
                    headerRow.AddChild(callsign);

                    var sig = AshfallUiHelpers.MakeMono($"SIG {signal.SignalStrength}/5");
                    sig.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                    headerRow.AddChild(sig);

                    box.AddChild(headerRow);

                    var msg = AshfallUiHelpers.MakeBody(signal.Message);
                    msg.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(signal.Kind == RadioEventKind.Silence ? Ashfall.Core.UI.Theme.Dim : Ashfall.Core.UI.Theme.Pale));
                    box.AddChild(msg);

                    var panel = AshfallUiHelpers.MakePanel();
                    panel.AddChild(box);
                    _signalList.AddChild(panel);
                }
            }

            _lblSignalsTitle.Text = $"TUNER LOG · FREQ {_radioHost.CurrentFrequency:00.00} MHz · DAY {_radioHost.Day} · {_radioHost.Engine.FactionCount} MONITORED CHANNELS";
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            Visible = false;

            var bg = new ColorRect { Color = new Color(0.04f, 0.05f, 0.06f, 0.92f) };
            bg.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(bg);

            var center = new CenterContainer();
            center.SetAnchorsPreset(LayoutPreset.FullRect);
            AddChild(center);

            var panel = AshfallUiHelpers.MakePanel(760, 600);
            center.AddChild(panel);

            var margins = AshfallUiHelpers.MakeMargins(Ashfall.Core.UI.Theme.SpacingMd);
            panel.AddChild(margins);

            var vbox = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingMd);
            margins.AddChild(vbox);

            var header = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var title = AshfallUiHelpers.MakeTitle("RADIO COMMUNICATIONS & INTERCEPTS", Ashfall.Core.UI.Theme.FontSizeH2);
            title.HorizontalAlignment = HorizontalAlignment.Left;
            title.SizeFlagsHorizontal = Control.SizeFlags.ExpandFill;
            header.AddChild(title);

            var btnClose = AshfallUiHelpers.MakeButton("CLOSE [Esc]", () => Close());
            btnClose.CustomMinimumSize = new Vector2(110, 32);
            header.AddChild(btnClose);
            vbox.AddChild(header);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            // Frequency Preset Buttons
            var dialHeader = AshfallUiHelpers.MakeSectionHeader("FREQUENCY TUNER PRESETS");
            vbox.AddChild(dialHeader);

            var dialRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            foreach (var (freq, label) in _presets)
            {
                float targetFreq = freq;
                var btnFreq = AshfallUiHelpers.MakeButton(label, () =>
                {
                    if (_radioHost != null)
                    {
                        _radioHost.Listen(targetFreq);
                        RefreshView();
                    }
                });
                btnFreq.CustomMinimumSize = new Vector2(170, 32);
                btnFreq.SizeFlagsHorizontal = SizeFlags.ExpandFill;
                dialRow.AddChild(btnFreq);
            }
            vbox.AddChild(dialRow);

            var actionRow = AshfallUiHelpers.MakeHBox(Ashfall.Core.UI.Theme.SpacingSm);
            var btnBeacon = AshfallUiHelpers.MakeButton("BROADCAST HOLDFAST EMERGENCY BEACON", () =>
            {
                if (_radioHost != null)
                {
                    _radioHost.BroadcastBeacon("Holdfast shelter holding. Awaiting survivor response.");
                    OnRadioBroadcastSent?.Invoke();
                    RefreshView();
                }
            }, true);
            btnBeacon.CustomMinimumSize = new Vector2(0, 36);
            btnBeacon.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            actionRow.AddChild(btnBeacon);
            vbox.AddChild(actionRow);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblSignalsTitle = AshfallUiHelpers.MakeSectionHeader("RECENT SIGNALS");
            vbox.AddChild(_lblSignalsTitle);

            var scroll = new ScrollContainer
            {
                CustomMinimumSize = new Vector2(720, 340),
                SizeFlagsVertical = SizeFlags.ExpandFill
            };
            vbox.AddChild(scroll);

            _signalList = AshfallUiHelpers.MakeVBox(Ashfall.Core.UI.Theme.SpacingSm);
            _signalList.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            scroll.AddChild(_signalList);

            RefreshView();
        }

        public void Open()
        {
            RefreshView();
            Visible = true;
            QueueRedraw();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public override void _UnhandledInput(InputEvent @event)
        {
            if (!Visible) return;
            if (@event is InputEventKey key && key.Pressed && key.Keycode == Key.Escape)
            {
                Close();
                GetViewport().SetInputAsHandled();
            }
        }
    }
}
