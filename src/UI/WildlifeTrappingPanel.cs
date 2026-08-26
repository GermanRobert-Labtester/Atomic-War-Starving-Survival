using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class WildlifeTrappingPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _setTrapBtn = null!;
        private Button _checkTrapBtn = null!;

        private WildlifeTrappingHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(WildlifeTrappingHostSession session)
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

            _shell = new AshfallDashboardShell("Wildlife Trapping // Snare Network", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("traps_active", "Active Snares", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("total_catch", "Total Harvest", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _setTrapBtn = new Button { Text = "Set Snare at Perimeter", CustomMinimumSize = new Vector2(180, 36) };
            _setTrapBtn.Pressed += () => _host?.SetTrap("snare_perimeter_north", "bait_cured_meat", "Hunter");
            buttonRow.AddChild(_setTrapBtn);

            _checkTrapBtn = new Button { Text = "Check & Harvest Snares", CustomMinimumSize = new Vector2(180, 36) };
            _checkTrapBtn.Pressed += () => _host?.CheckTraps();
            buttonRow.AddChild(_checkTrapBtn);

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
            _statusRail.Set("traps_active", s.trapSites.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("total_catch", s.totalCatch.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Wildlife Trapping Network ({s.trapSites.Count} sites):\n";
                foreach (var t in s.trapSites)
                {
                    text += $"  • Site {t.siteId} (Hunter: {t.assignedHunterId}, Bait: {t.baitType}) — Status: {(t.hasCatch ? $"CATCH READY ({t.catchSpecies})" : "ARMED")}\n";
                }
                text += $"\nTotal Toxins Neutralized: {s.totalToxicRemoved} | Last Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }

        public override void _ExitTree()
        {
            if (_host != null)
            {
                _host.StateChanged -= RefreshView;
            }
            base._ExitTree();
        }
    }
}
