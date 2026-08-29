using System;
using Godot;
using Ashfall.Core;
using Ashfall.Core.UI;
using AtomicWar.GodotApp;
using DesignTheme = Ashfall.Core.UI.Theme;

namespace AtomicWar.GodotApp.UI
{
    public partial class ExcavationPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

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

            _shell = new AshfallDashboardShell("Subterranean Excavation // Deep Strata", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active_sites", "Excavation Sites", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = AshfallUiHelpers.MakeBody("", autowrap: true);
            _contentStack.AddChild(_detailText);

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
                if (s.sites.Count == 0)
                {
                    _detailText.Text = "No subterranean excavation sites currently active.\nAssign surveying teams or discover buried deep-strata vaults to begin excavation operations.\n\nLast Event: " + (string.IsNullOrEmpty(_host.LastEvent) ? "None recorded" : _host.LastEvent);
                }
                else
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

        public override void _ExitTree()
        {
            Unbind();
            base._ExitTree();
        }
    }
}
