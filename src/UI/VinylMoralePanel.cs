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



        private OptionButton _recordSelector = null!;
        private Label _recordMetadata = null!;
        private string _selectedRecordId = string.Empty;

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

            _contentStack.AddChild(AshfallUiHelpers.MakeSeparator());

            var selectHeader = AshfallUiHelpers.MakeSectionHeader("PRE-WAR ALBUM SELECTION");
            _contentStack.AddChild(selectHeader);

            var selectRow = new HBoxContainer();
            selectRow.AddThemeConstantOverride("separation", 10);
            var selLabel = AshfallUiHelpers.MakeBody("Select Album:");
            selectRow.AddChild(selLabel);

            _recordSelector = new OptionButton { CustomMinimumSize = new Vector2(350, 36) };
            _recordSelector.ItemSelected += idx =>
            {
                if (_host != null && idx >= 0 && idx < _host.System.State.ownedRecordIds.Count)
                {
                    _selectedRecordId = _host.System.State.ownedRecordIds[(int)idx];
                    UpdateRecordPreview();
                }
            };
            selectRow.AddChild(_recordSelector);
            _contentStack.AddChild(selectRow);

            _recordMetadata = new Label();
            _recordMetadata.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _recordMetadata.AddThemeColorOverride("font_color", AshfallUiHelpers.ToColor(DesignTheme.Warm));
            _contentStack.AddChild(_recordMetadata);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _playBtn = new Button { Text = "Play Selected Album", CustomMinimumSize = new Vector2(180, 36) };
            _playBtn.Pressed += () =>
            {
                if (_host != null && !string.IsNullOrEmpty(_selectedRecordId))
                {
                    _host.PlayRecord(_selectedRecordId);
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
                    ? $"Cultural Broadcast: {s.lastBroadcastRecordId} (Day {s.lastBroadcastDay}, {s.lastBroadcastSignalStrength * 100f:F0}% signal, {s.broadcastCount} total) — Wanderers may hear this. 150W transmitter load."
                    : "Cultural Broadcast: IDLE (rare vinyl required — classical/jazz/symphony or ≥4 morale bonus)";
                _detailText.Text = $"Turntable State: {(_host.System.IsPlaying ? "ACTIVE" : "STANDBY")} | Power: {(broadcasting ? "150W TX LOAD" : "0W")}\n" +
                                   $"Total Plays: {s.totalPlays} | Last Played: {s.lastPlayedId} (Day {s.lastPlayedDay})\n" +
                                   $"{broadcastLine}\n" +
                                   $"Last Event: {_host.LastEvent}";
            }

            // Populate selector with owned records deterministically
            if (_recordSelector != null)
            {
                _recordSelector.Clear();
                if (s.ownedRecordIds.Count == 0)
                {
                    _recordSelector.AddItem("(No records acquired)", 0);
                    _recordSelector.Disabled = true;
                    _playBtn.Disabled = true;
                    _recordMetadata.Text = "NO VINYL RECORDS ACQUIRED — Scavenge ruins, search pre-war apartments, or trade with caravans to discover albums.";
                }
                else
                {
                    _recordSelector.Disabled = false;
                    _playBtn.Disabled = false;
                    int selectedIdx = 0;
                    for (int i = 0; i < s.ownedRecordIds.Count; i++)
                    {
                        string rid = s.ownedRecordIds[i];
                        var def = _host.System.GetRecord(rid);
                        string label = def != null ? $"{def.display_name} [{def.genre}] (+{def.morale_daily_bonus:F0} Morale)" : rid;
                        _recordSelector.AddItem(label, i);
                        if (rid == _selectedRecordId || (string.IsNullOrEmpty(_selectedRecordId) && i == 0))
                        {
                            selectedIdx = i;
                            _selectedRecordId = rid;
                        }
                    }
                    _recordSelector.Selected = selectedIdx;
                    UpdateRecordPreview();
                }
            }
        }

        private void UpdateRecordPreview()
        {
            if (_host == null || string.IsNullOrEmpty(_selectedRecordId) || _recordMetadata == null) return;
            var def = _host.System.GetRecord(_selectedRecordId);
            if (def != null)
            {
                _recordMetadata.Text = $"Selected: {def.display_name} | Genre: {def.genre} | Daily Morale: +{def.morale_daily_bonus:F0}\n" +
                                       $"Notes: {def.description}";
            }
            else
            {
                _recordMetadata.Text = $"Selected ID: {_selectedRecordId}";
            }
        }

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
