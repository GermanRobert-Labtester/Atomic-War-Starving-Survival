// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Forced Labor & Captivity Operations (Plan 183)
// Displays captive labor details, guard oversight, cruelty index, and rebellion risk.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Factions;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class LaborUI : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private ForcedLaborSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(ForcedLaborSystem system)
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

            _shell = new AshfallDashboardShell("Forced Labor // Captivity & Work Details", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("laborers", "Coerced Captives", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("cruelty", "Cruelty Index", "0.0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("rebellion_risk", "Rebellion Risk", "Low", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("guards", "Armed Guards", "2", AshfallMetricCard.Criticality.Normal, minWidth: 100);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No captives assigned to forced labor details.";
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
                _detailText.Text = "Forced labor system offline.";
                return;
            }

            var risk = _system.CalculateRebellionRisk();
            string riskLabel = risk.totalRisk switch
            {
                > 0.6f => "EXTREME",
                > 0.35f => "HIGH",
                > 0.15f => "MODERATE",
                _ => "LOW"
            };
            var riskCrit = risk.totalRisk > 0.35f ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal;
            var crueltyCrit = _system.CrueltyIndex > 40f ? AshfallMetricCard.Criticality.Warn : AshfallMetricCard.Criticality.Normal;

            _statusRail.Set("laborers", _system.Laborers.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("cruelty", $"{_system.CrueltyIndex:F1}", crueltyCrit);
            _statusRail.Set("rebellion_risk", riskLabel, riskCrit);
            _statusRail.Set("guards", _system.GuardCount.ToString(), AshfallMetricCard.Criticality.Normal);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== ASSIGNED CAPTIVE LABOR ROSTER ===");
            if (_system.Laborers.Count == 0)
            {
                sb.AppendLine("  No captives assigned to hazardous work details. Coercion at baseline.");
            }
            else
            {
                foreach (var l in _system.Laborers)
                {
                    var camp = _system.GetCamp(l.campId);
                    string campName = camp?.name ?? l.campId;
                    string restraint = l.isRestrained ? "[CHAINED]" : "[UNFETTERED]";
                    sb.AppendLine($"  - Captive {l.captiveId} -> {campName} | Health: {l.health:F0}% | Physical Strain: {l.physicalStrain:F0}% | Resentment: {l.individualResentment:F0}% {restraint}");
                }
            }

            sb.AppendLine();
            sb.AppendLine("=== SECURITY & REBELLION TELEMETRY ===");
            sb.AppendLine($"  Active Rebellion: {(_system.IsRebellionActive ? "CRITICAL OUTBREAK IN PROGRESS" : "Normal Containment")}");
            sb.AppendLine($"  Total Escapes: {_system.TotalEscaped} | Total Quelled Rebellions: {_system.TotalRebellions} | Sabotage Incidents: {_system.TotalSabotages}");
            sb.AppendLine($"  Population Pressure: {risk.populationPressure * 100:F0}% | Guard Deficiency Risk: {risk.guardDeficiency * 100:F0}%");

            _detailText.Text = sb.ToString();
        }
    }
}
