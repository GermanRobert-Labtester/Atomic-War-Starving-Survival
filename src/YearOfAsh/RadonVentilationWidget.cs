using System;
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot 4.7+ UI Control for Phase VI Radon Gas Ventilation & Scrubber Monitoring.
    /// Thin presentation only: queries YearOfAshRadonSystem.
    /// Zero simulation logic.
    /// </summary>
    public partial class RadonVentilationWidget : PanelContainer
    {
        private YearOfAshHostSession _session;
        private Label _lblRadonLevel;
        private ProgressBar _pbRadonLevel;
        private Label _lblScrubberHealth;
        private ProgressBar _pbScrubberHealth;
        private Label _lblFissures;
        private Button _btnReplaceScrubber;
        private Button _btnSealFissures;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopLeft);
            CustomMinimumSize = new Vector2(340, 200);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "RADON-222 AIR SCRUBBER ARRAY",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblRadonLevel = new Label { Text = "Indoor Concentration: 120 Bq/m³ (SAFE)" };
            rootVbox.AddChild(_lblRadonLevel);

            _pbRadonLevel = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 2000,
                Value = 120,
                CustomMinimumSize = new Vector2(0, 14)
            };
            rootVbox.AddChild(_pbRadonLevel);

            _lblScrubberHealth = new Label { Text = "Filter Media Health: 100%" };
            rootVbox.AddChild(_lblScrubberHealth);

            _pbScrubberHealth = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 100,
                CustomMinimumSize = new Vector2(0, 14)
            };
            rootVbox.AddChild(_pbScrubberHealth);

            _lblFissures = new Label { Text = "Active Foundation Fissures: 0" };
            rootVbox.AddChild(_lblFissures);

            var buttonHbox = new HBoxContainer();
            buttonHbox.AddThemeConstantOverride("separation", 8);
            rootVbox.AddChild(buttonHbox);

            _btnReplaceScrubber = new Button { Text = "Replace Scrubber" };
            _btnReplaceScrubber.Pressed += OnReplaceScrubberPressed;
            buttonHbox.AddChild(_btnReplaceScrubber);

            _btnSealFissures = new Button { Text = "Seal Fissures" };
            _btnSealFissures.Pressed += OnSealFissuresPressed;
            buttonHbox.AddChild(_btnSealFissures);
        }

        private Action<float> _radonChangedHandler;

        public RadonVentilationWidget()
        {
            _radonChangedHandler = _ => RefreshView();
        }

        public void BindSession(YearOfAshHostSession session)
        {
            UnbindSession();
            _session = session;
            if (_session == null) return;

            _session.Radon.OnRadonLevelChanged += _radonChangedHandler;
            RefreshView();
        }

        private void UnbindSession()
        {
            if (_session == null) return;
            _session.Radon.OnRadonLevelChanged -= _radonChangedHandler;
            _session = null!;
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        private void OnReplaceScrubberPressed()
        {
            if (_session == null) return;
            _session.Radon.ReplaceScrubberFilter();
            RefreshView();
        }

        private void OnSealFissuresPressed()
        {
            if (_session == null) return;
            _session.Radon.SealFoundationFissures();
            RefreshView();
        }

        public void RefreshView()
        {
            if (_session == null) return;

            float radon = _session.Radon.IndoorRadonBqm3;
            string status = radon >= YearOfAshRadonSystem.DangerousRadonThreshold ? "DANGEROUS" : (radon > YearOfAshRadonSystem.SafeRadonThreshold ? "ELEVATED" : "SAFE");

            if (_lblRadonLevel != null)
                _lblRadonLevel.Text = $"Indoor Concentration: {radon:F0} Bq/m³ ({status})";

            if (_pbRadonLevel != null)
                _pbRadonLevel.Value = radon;

            if (_lblScrubberHealth != null)
                _lblScrubberHealth.Text = $"Filter Media Health: {_session.Radon.ScrubberHealthPercent:F0}%";

            if (_pbScrubberHealth != null)
                _pbScrubberHealth.Value = _session.Radon.ScrubberHealthPercent;

            if (_lblFissures != null)
                _lblFissures.Text = $"Active Foundation Fissures: {_session.Radon.ActiveFissures}";
        }
    }
}
