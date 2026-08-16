using System;
using Godot;
using Ashfall.Core.UI;
using AtomicWar.GodotApp.UI;

namespace AtomicWar.GodotApp.UI
{
    /// <summary>
    /// ASHFALL — Radio Detail panel.
    /// Shows detailed radio communications, signal analysis, and communication logs.
    /// </summary>
    public partial class RadioDetailPanel : Control
    {
        public event Action? OnClose;

        private VBoxContainer _contentVBox = null!;
        private Label _lblSignalInfoTitle;
        private VBoxContainer _signalInfo;
        private Label _lblFrequencyTitle;
        private VBoxContainer _frequencyData;
        private Label _lblTransmissionsTitle;
        private VBoxContainer _transmissionsLog;
        private Label _lblAnalysisTitle;
        private VBoxContainer _signalAnalysis;

        private readonly string[] _placeholderSignalInfo = {
            "Current Signal: Unknown frequency (142.5 MHz)",
            "Strength: Moderate (-65 dBm)",
            "Modulation: AM (Amplitude Modulation)",
            "Pattern: Repeating every 30 seconds",
            "Origin: Unknown (Sector 7 area)",
            "Duration: Ongoing since Day 18"
        };

        private readonly string[] _placeholderFrequency = {
            "Primary Frequency: 142.5 MHz (Active)",
            "Secondary Frequency: 87.3 MHz (Idle)",
            "Emergency Frequency: 121.5 MHz (Standby)",
            "Bandwidth: 10 kHz (Narrowband)",
            "Noise Floor: -95 dBm (Low interference)",
            "Signal-to-Noise Ratio: 30 dB (Good)"
        };

        private readonly string[] _placeholderTransmissions = {
            "[Day 24] Unknown — '...supply drop coordinates...Sector 12...'",
            "[Day 22] Black Flotilla — Trade offer received",
            "[Day 20] Unknown — '...radiation levels rising...seek shelter...'",
            "[Day 18] Ledger Keepers — Knowledge exchange proposal",
            "[Day 15] Green Thread — Mutual defense request"
        };

        private readonly string[] _placeholderAnalysis = {
            "Signal Source: Likely nearby (within 5 km)",
            "Intent: Unknown (possibly distress or trade)",
            "Threat Level: Low (No hostile indicators)",
            "Recommended Action: Monitor and respond cautiously",
            "Decoding Status: Partial (Key phrases identified)",
            "Next Steps: Send response on 142.5 MHz"
        };

        public void Bind(object radio)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_signalInfo == null || _frequencyData == null || _transmissionsLog == null || _signalAnalysis == null) return;

            while (_signalInfo.GetChildCount() > 0) _signalInfo.RemoveChild(_signalInfo.GetChild(0));
            while (_frequencyData.GetChildCount() > 0) _frequencyData.RemoveChild(_frequencyData.GetChild(0));
            while (_transmissionsLog.GetChildCount() > 0) _transmissionsLog.RemoveChild(_transmissionsLog.GetChild(0));
            while (_signalAnalysis.GetChildCount() > 0) _signalAnalysis.RemoveChild(_signalAnalysis.GetChild(0));

            foreach (string info in _placeholderSignalInfo)
            {
                var label = new Label { Text = info };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                _signalInfo.AddChild(label);
            }

            foreach (string freq in _placeholderFrequency)
            {
                var label = new Label { Text = freq };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Warm));
                _frequencyData.AddChild(label);
            }

            foreach (string transmission in _placeholderTransmissions)
            {
                var label = new Label { Text = transmission };
                label.CustomMinimumSize = new Vector2(350, 30);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Lethe));
                _transmissionsLog.AddChild(label);
            }

            foreach (string analysis in _placeholderAnalysis)
            {
                var label = new Label { Text = analysis };
                label.CustomMinimumSize = new Vector2(350, 35);
                label.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
                label.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(Ashfall.Core.UI.Theme.Pale));
                _signalAnalysis.AddChild(label);
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
            vbox.CustomMinimumSize = new Vector2(550, 0);
            container.AddChild(vbox);

            var title = AshfallUiHelpers.MakeTitle("RADIO DETAIL", Ashfall.Core.UI.Theme.FontSizeH1);
            title.HorizontalAlignment = HorizontalAlignment.Center;
            vbox.AddChild(title);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblSignalInfoTitle = AshfallUiHelpers.MakeSectionHeader("SIGNAL INFORMATION");
            vbox.AddChild(_lblSignalInfoTitle);

            _signalInfo = new VBoxContainer();
            _signalInfo.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _signalInfo.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_signalInfo);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblFrequencyTitle = AshfallUiHelpers.MakeSectionHeader("FREQUENCY DATA");
            vbox.AddChild(_lblFrequencyTitle);

            _frequencyData = new VBoxContainer();
            _frequencyData.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _frequencyData.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_frequencyData);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblTransmissionsTitle = AshfallUiHelpers.MakeSectionHeader("TRANSMISSIONS LOG");
            vbox.AddChild(_lblTransmissionsTitle);

            _transmissionsLog = new VBoxContainer();
            _transmissionsLog.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _transmissionsLog.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_transmissionsLog);

            vbox.AddChild(AshfallUiHelpers.MakeSeparator());

            _lblAnalysisTitle = AshfallUiHelpers.MakeSectionHeader("SIGNAL ANALYSIS");
            vbox.AddChild(_lblAnalysisTitle);

            _signalAnalysis = new VBoxContainer();
            _signalAnalysis.AddThemeConstantOverride("separation", Ashfall.Core.UI.Theme.SpacingSm);
            _signalAnalysis.CustomMinimumSize = new Vector2(400, 0);
            vbox.AddChild(_signalAnalysis);

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
