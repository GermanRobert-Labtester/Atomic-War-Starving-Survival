using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ExcavationPanel : Control
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;
        private Button _assignWorkersBtn = null!;
        private Button _shoringBtn = null!;

        private ExcavationHostSession? _host;

        public bool IsBound => _host != null;

        public void Bind(ExcavationHostSession session)
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

            _shell = new AshfallDashboardShell("Subterranean Excavation // Deep Strata", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_sites", "Excavation Sites", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.AutowrapMode = TextServer.AutowrapMode.WordSmart;
            _contentStack.AddChild(_detailText);

            var buttonRow = new HBoxContainer();
            buttonRow.AddThemeConstantOverride("separation", 10);

            _assignWorkersBtn = new Button { Text = "Assign Dig Crew (4 Labor)", CustomMinimumSize = new Vector2(200, 36) };
            _assignWorkersBtn.Pressed += () =>
            {
                if (_host != null)
                {
                    _host.AddSite("vault_strata_delta9", "blueprint_deep_vault_74", 100f, 0.2f);
                    _host.AssignWorkers("vault_strata_delta9", 4);
                }
            };
            buttonRow.AddChild(_assignWorkersBtn);

            _shoringBtn = new Button { Text = "Reinforce Shoring", CustomMinimumSize = new Vector2(160, 36) };
            _shoringBtn.Pressed += () => _host?.ApplyShoring("vault_strata_delta9");
            buttonRow.AddChild(_shoringBtn);

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
            _statusRail.Set("active_sites", s.sites.Count.ToString(), AshfallMetricCard.Criticality.Normal);

            if (_detailText != null)
            {
                string text = $"Subterranean Excavation Sites ({s.sites.Count} total):\n";
                foreach (var site in s.sites)
                {
                    text += $"  • [{site.siteId}] Progress: {site.progress:F0}/{site.requiredProgress:F0} | Workers: {site.assignedWorkerCount} | Shoring: {(site.shoringApplied ? "REINFORCED" : "UNSHORED")} | Cave-in Risk: {site.structuralRisk:P0}\n";
                }
                text += $"\nLast Event: {_host.LastEvent}";
                _detailText.Text = text;
            }
        }
    }
}
