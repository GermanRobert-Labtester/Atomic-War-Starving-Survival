using System;
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot 4.7+ UI Control for Phase IV Deep Freeze Thermal Management.
    /// Thin presentation only: displays indoor temperature, intake icing, and thermal insulation status.
    /// Zero simulation logic.
    /// </summary>
    public partial class GeothermalHeatingWidget : PanelContainer
    {
        private YearOfAshHostSession _session;
        private Label _lblIndoorTemp;
        private ProgressBar _pbIndoorTemp;
        private Label _lblIntakeIce;
        private ProgressBar _pbIntakeIce;
        private Button _btnClearIntakeIce;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(340, 180);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "GEOTHERMAL HEAT & INTAKE ICING",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblIndoorTemp = new Label { Text = "Bunker Temperature: 18.0°C" };
            rootVbox.AddChild(_lblIndoorTemp);

            _pbIndoorTemp = new ProgressBar
            {
                MinValue = -20,
                MaxValue = 30,
                Value = 18,
                CustomMinimumSize = new Vector2(0, 14)
            };
            rootVbox.AddChild(_pbIndoorTemp);

            _lblIntakeIce = new Label { Text = "Intake Ice Thickness: 0 mm (CLEAR)" };
            rootVbox.AddChild(_lblIntakeIce);

            _pbIntakeIce = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 60,
                Value = 0,
                CustomMinimumSize = new Vector2(0, 14)
            };
            rootVbox.AddChild(_pbIntakeIce);

            _btnClearIntakeIce = new Button { Text = "De-Ice Intake Cowling" };
            _btnClearIntakeIce.Pressed += OnClearIntakeIcePressed;
            rootVbox.AddChild(_btnClearIntakeIce);
        }

        private Action<float> _temperatureChangedHandler;

        public GeothermalHeatingWidget()
        {
            _temperatureChangedHandler = _ => RefreshView();
        }

        public void BindSession(YearOfAshHostSession session)
        {
            UnbindSession();
            _session = session;
            if (_session == null) return;

            _session.DeepFreeze.OnTemperatureChanged += _temperatureChangedHandler;
            RefreshView();
        }

        private void UnbindSession()
        {
            if (_session == null) return;
            _session.DeepFreeze.OnTemperatureChanged -= _temperatureChangedHandler;
            _session = null;
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        private void OnClearIntakeIcePressed()
        {
            if (_session == null) return;
            _session.DeepFreeze.ClearIntakeIce();
            RefreshView();
        }

        public void RefreshView()
        {
            if (_session == null) return;

            float temp = _session.DeepFreeze.IndoorTempCelsius;
            float ice = _session.DeepFreeze.IntakeIceMm;
            bool blocked = _session.DeepFreeze.IsIntakeBlocked;

            if (_lblIndoorTemp != null)
                _lblIndoorTemp.Text = $"Bunker Temperature: {temp:F1}°C {(temp <= 0 ? "● FREEZING PIPES!" : "")}";

            if (_pbIndoorTemp != null)
                _pbIndoorTemp.Value = temp;

            if (_lblIntakeIce != null)
                _lblIntakeIce.Text = $"Intake Ice Thickness: {ice:F1} mm {(blocked ? "● BLOCKED!" : "○ OK")}";

            if (_pbIntakeIce != null)
                _pbIntakeIce.Value = ice;
        }
    }
}
