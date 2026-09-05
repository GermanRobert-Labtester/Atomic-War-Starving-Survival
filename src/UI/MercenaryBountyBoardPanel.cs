using System;
using Godot;
using Ashfall.Core.Economy;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class MercenaryBountyBoardPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private MercenarySystem? _system;

        public bool IsBound => _system != null;

        public void Bind(MercenarySystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind() { _system = null; }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);
            _shell = new AshfallDashboardShell("WARLORD CONTRACTS // MERCENARY BOUNTY BOARD", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("active", "Active Contracts", "0", AshfallMetricCard.Criticality.Normal);
            _statusRail.AddCard("guild", "Guild Standing", "Neutral", AshfallMetricCard.Criticality.Normal);

            _contentStack = new VBoxContainer();
            _detailText = new Label { Text = "No bounties available." };
            _contentStack.AddChild(_detailText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());
            Visible = false;
        }

        public void Open() { Visible = true; RefreshView(); }
        public void Close() { Visible = false; OnClose?.Invoke(); }

        public void RefreshView()
        {
            if (_system == null || _detailText == null || _statusRail == null) return;
            _statusRail.Set("active", _system.ActiveContracts.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _detailText.Text = "Bounty board online. No targets in local sector.";
        }
    }
}
