// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Settlement Politics & Council Chamber (Plan 185)
// Displays current leadership, governance mode, approval factors, active policies, and elections.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Narrative;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class PoliticsUI : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private PoliticsSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(PoliticsSystem system)
        {
            _system = system;
            RefreshView();
        }

        public void Unbind()
        {
            _system = null;
        }

        public override void _Ready()
        {
            SetAnchorsPreset(LayoutPreset.FullRect);

            _shell = new AshfallDashboardShell("Council Chamber // Settlement Politics & Law", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("mode", "Governance Mode", "Democratic", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("approval", "Public Approval", "60%", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("legitimacy", "Legitimacy Index", "75", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("election_days", "Next Election", "30d", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "Settlement council is currently adjourned.";
            _contentStack.AddChild(_detailText);

            _shell.SetContent(_contentStack);
            _shell.AttachHeaderCloseButton("CLOSE", () => OnClose?.Invoke());

            // Overlay panels start hidden; PanelRegistry drives visibility.
            Visible = false;

            RefreshView();
        }

        public void Open()
        {
            Visible = true;
            RefreshView();
        }

        public void Close()
        {
            Visible = false;
            OnClose?.Invoke();
        }

        public void RefreshView()
        {
            if (_statusRail == null || _detailText == null) return;
            if (_system == null)
            {
                _detailText.Text = "Politics system offline.";
                return;
            }

            var approvalCrit = _system.ApprovalRating < 35f ? AshfallMetricCard.Criticality.Critical
                : _system.ApprovalRating < 50f ? AshfallMetricCard.Criticality.Warn
                : AshfallMetricCard.Criticality.Normal;

            var legitCrit = _system.Legitimacy < 30f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal;

            _statusRail.Set("mode", _system.GovernanceMode, _system.IsMartialLaw ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("approval", $"{_system.ApprovalRating:F0}%", approvalCrit);
            _statusRail.Set("legitimacy", $"{_system.Legitimacy:F0}", legitCrit);
            _statusRail.Set("election_days", _system.IsMartialLaw ? "SUSPENDED" : $"{_system.DaysUntilElection}d", AshfallMetricCard.Criticality.Normal);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== EXECUTIVE LEADERSHIP & INSTITUTIONS ===");
            string leader = string.IsNullOrEmpty(_system.CurrentLeaderId) ? "[NO DESIGNATED LEADER]" : _system.CurrentLeaderId;
            sb.AppendLine($"  Current Designated Leader: {leader}");
            sb.AppendLine($"  Governance Constitution: {_system.GovernanceMode} | Martial Law: {(_system.IsMartialLaw ? "ACTIVE" : "INACTIVE")}");
            sb.AppendLine($"  Coup d'Etat Risk: {_system.CoupRisk * 100:F0}% | Total Elections Held: {_system.TotalElections} | Total Coups: {_system.TotalCoups}");

            sb.AppendLine();
            sb.AppendLine("=== ACTIVE SETTLEMENT POLICIES & ORDINANCES ===");
            if (_system.ActivePolicies.Count == 0)
            {
                sb.AppendLine("  No legislative policies enacted. Operating under baseline common law.");
            }
            else
            {
                foreach (var pId in _system.ActivePolicies)
                {
                    var def = _system.GetPolicy(pId);
                    string name = def?.name ?? pId;
                    sb.AppendLine($"  - [LAW] {name} ({def?.category ?? "Civil"}): {def?.description ?? ""}");
                }
            }

            _detailText.Text = sb.ToString();
        }
    }
}
