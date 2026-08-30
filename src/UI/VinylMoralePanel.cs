using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class VinylMoralePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _playBtn = null!;
        private Button _stopBtn = null!;

        private VinylMoraleHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(VinylMoraleHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public void Unbind()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
                _host = null;
            }
        }



        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Common Room // Vinyl Turntable", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("now_playing", "Now Playing", "STOPPED", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("morale_applied", "Total Morale", "+0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("records_owned", "Albums Owned", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("broadcast", "Cultural Signal", "IDLE", AshfallMetricCard.Criticality.Normal, minWidth: 150);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _playBtn = new Button { Text = "Play Pre-War Jazz Album", CustomMinimumSize = new Vector2(180, 36) };
            _playBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    _host.AcquireRecord("record_04_midnight_in_moscow_jazz_octet");
                    _host.PlayRecord("record_04_midnight_in_moscow_jazz_octet");
                }
            };
            buttonRow.AddChild(_playBtn);

            _stopBtn = new Button { Text = "Stop Turntable", CustomMinimumSize = new Vector2(140, 36) };
            _stopBtn.Pressed += () => _host?.StopPlayback();
            buttonRow.AddChild(_stopBtn);

            _contentStack.AddChild(buttonRow);
            _shell.SetContent(_contentStack);

            _shell.AttachHeaderCloseButton("CLOSE", () =>
            {
                Visible = false;
                OnClose?.Invoke();
            });

            RefreshView();
        }

        public void RefreshView()
        {
            if (_host == null || _statusRail == null) return;

            var s = _host.System.State;
            _statusRail.Set("now_playing", _host.System.IsPlaying ? s.currentPlayingId : "STOPPED", _host.System.IsPlaying ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("morale_applied", $"+{s.totalMoraleApplied:F0}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("records_owned", s.ownedRecordIds.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            bool broadcasting = !string.IsNullOrEmpty(s.lastBroadcastRecordId) && s.lastBroadcastSignalStrength > 0.01f;
            _statusRail.Set("broadcast", broadcasting ? $"ON 98.6MHz ({s.broadcastCount})" : "IDLE", broadcasting ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string broadcastLine = broadcasting
                    ? $"Cultural Broadcast: {s.lastBroadcastRecordId} (Day {s.lastBroadcastDay}, {s.lastBroadcastSignalStrength*100f:F0}% signal, {s.broadcastCount} total) — Wanderers may hear this. 150W transmitter load."
                    : "Cultural Broadcast: IDLE (rare vinyl required — classical/jazz/symphony or ≥4 morale bonus)";
                _detailText.Text = $"Turntable State: {(_host.System.IsPlaying ? "ACTIVE" : "STANDBY")} | Power: {(broadcasting ? "150W TX LOAD" : "0W")}\n" +
                                   $"Total Plays: {s.totalPlays} | Last Played: {s.lastPlayedId} (Day {s.lastPlayedDay})\n" +
                                   $"{broadcastLine}\n" +
                                   $"Last Event: {_host.LastEvent}";
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
