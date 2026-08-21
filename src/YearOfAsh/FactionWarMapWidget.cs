using System;
using Godot;
using AtomicWar.GodotApp.UI;
using Ashfall.Core.UI;
using Ashfall.Core.YearOfAsh;

namespace AtomicWar.GodotApp.YearOfAsh
{
    /// <summary>
    /// Godot 4.7+ UI Control for presenting the Year of Ash Faction War & Season Status.
    /// Thin presentation only: queries FactionWarSystem and YearOfAshTimelineSystem.
    /// Zero simulation logic.
    /// </summary>
    public partial class FactionWarMapWidget : PanelContainer
    {
        private YearOfAshHostSession _session;
        private Label _lblPhase;
        private Label _lblTemp;
        private Label _lblWarTension;
        private ProgressBar _pbWarTension;
        private VBoxContainer _factionsList;

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.TopRight);
            CustomMinimumSize = new Vector2(360, 240);

            // Apply standard panel 9-slice via shared helper (frame_9slice first)
            AddThemeStyleboxOverride("panel", AtomicWar.GodotApp.UI.AshfallUiHelpers.MakePanelFrameStyleBox());

            var rootVbox = new VBoxContainer();
            rootVbox.AddThemeConstantOverride("separation", 6);
            AddChild(rootVbox);

            var title = new Label
            {
                Text = "YEAR OF ASH: GEOPOLITICAL STATE",
                HorizontalAlignment = HorizontalAlignment.Center
            };
            title.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeBody);
            rootVbox.AddChild(title);

            _lblPhase = new Label { Text = "Phase: Phase IV - Deep Freeze" };
            rootVbox.AddChild(_lblPhase);

            _lblTemp = new Label { Text = "Surface Temp: -35.0°C" };
            rootVbox.AddChild(_lblTemp);

            _lblWarTension = new Label { Text = "Sector War Tension: 50%" };
            rootVbox.AddChild(_lblWarTension);

            _pbWarTension = new ProgressBar
            {
                MinValue = 0,
                MaxValue = 100,
                Value = 50,
                CustomMinimumSize = new Vector2(0, 16)
            };
            rootVbox.AddChild(_pbWarTension);

            _factionsList = new VBoxContainer();
            rootVbox.AddChild(_factionsList);
        }

        public void BindSession(YearOfAshHostSession session)
        {
            UnbindSession();
            _session = session;
            if (_session == null) return;

            _session.Timeline.OnPhaseTransitioned += OnPhaseTransition;
            _session.Timeline.OnDayAdvanced += OnDayAdvanced;
            _session.FactionWar.OnFactionStandingChanged += OnFactionStandingChanged;

            RefreshView();
        }

        private void UnbindSession()
        {
            if (_session == null) return;
            _session.Timeline.OnPhaseTransitioned -= OnPhaseTransition;
            _session.Timeline.OnDayAdvanced -= OnDayAdvanced;
            _session.FactionWar.OnFactionStandingChanged -= OnFactionStandingChanged;
            _session = null!;
        }

        private void OnFactionStandingChanged(string factionId, int standing)
        {
            RefreshView();
        }

        public override void _ExitTree()
        {
            UnbindSession();
            base._ExitTree();
        }

        private void OnPhaseTransition(YearOfAshPhase phase)
        {
            RefreshView();
        }

        private void OnDayAdvanced(int day)
        {
            RefreshView();
        }

        public void RefreshView()
        {
            if (_session == null) return;

            if (_lblPhase != null)
                _lblPhase.Text = $"Phase: {_session.Timeline.CurrentPhase} (Day {_session.Timeline.CurrentDay})";

            if (_lblTemp != null)
                _lblTemp.Text = $"Surface Temp: {_session.Timeline.AmbientTemperatureCelsius:F1}°C | Ash: {_session.Timeline.AshCloudOpacity * 100:F0}%";

            if (_lblWarTension != null)
                _lblWarTension.Text = $"Sector War Tension: {_session.FactionWar.WarTension}/100";

            if (_pbWarTension != null)
                _pbWarTension.Value = _session.FactionWar.WarTension;

            if (_factionsList != null)
            {
                foreach (Node child in _factionsList.GetChildren())
                {
                    child.QueueFree();
                }

                foreach (var f in _session.FactionWar.State.factions)
                {
                    var lbl = new Label
                    {
                        Text = $"{f.factionId}: Standing {f.standing} | Control {f.territorialControlPercent}%"
                    };
                    lbl.AddThemeFontSizeOverride("font_size", Ashfall.Core.UI.Theme.FontSizeSmall);
                    _factionsList.AddChild(lbl);
                }
            }
        }
    }
}
