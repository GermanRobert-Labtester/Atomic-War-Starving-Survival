using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WaystationNetworkPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _unlockBtn = null!;
        private Button _stoveBtn = null!;

        private WaystationHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(WaystationHostSession session)
        {
            _host = session;
            if (_host != null)
            {
                _host.StateChanged += RefreshView;
            }
            RefreshView();
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Waystation A // Forward Outpost", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("status", "Camp Status", "LOCKED", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("stove", "Stove Heat", "EXTINGUISHED", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("filter", "Filter Health", "100%", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _unlockBtn = new Button { Text = "Unlock Waystation Camp", CustomMinimumSize = new Vector2(200, 36) };
            _unlockBtn.Pressed += () => _host?.Unlock();
            buttonRow.AddChild(_unlockBtn);

            _stoveBtn = new Button { Text = "Light Heating Stove", CustomMinimumSize = new Vector2(160, 36) };
            _stoveBtn.Pressed += () => _host?.AssignWatch(new[] { "Scout_1" });
            buttonRow.AddChild(_stoveBtn);

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
            _statusRail.Set("status", s.unlocked ? "UNLOCKED" : "LOCKED", s.unlocked ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("stove", s.stoveLit ? "LIT & WARM" : "EXTINGUISHED", s.stoveLit ? AshfallMetricCard.Criticality.Caution : AshfallMetricCard.Criticality.Warn);
            _statusRail.Set("filter", $"{s.filterHealth:F0}%", s.filterHealth < 30f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                _detailText.Text = $"Forward Outpost: {WaystationSystem.LocationId} | Bunks Occupied: {s.bunksOccupied}/{WaystationSystem.MaxBunks}\n" +
                                   $"Days Since Resupply: {s.daysSinceResupply} | Watch Sentries: {s.watchSurvivorIds.Length}\n" +
                                   $"Last Event: {_host.LastEvent}";
            }
        }
    }
}
