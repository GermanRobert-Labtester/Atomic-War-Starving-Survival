// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Chemical Engineering & Narcotics Lab (Plan 184)
// Displays chemical formulas, patient toxicity, dependency levels, and rehab beds.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class ChemUI : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private NarcoticsSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(NarcoticsSystem system)
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

            _shell = new AshfallDashboardShell("Pharmacy // Chemical Engineering & Narcotics", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("patients", "Active Profiles", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("doses", "Doses Given", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("overdoses", "Overdose Emergencies", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("rehabs", "Rehab Discharges", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No chemical medicine profiles active.";
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
                _detailText.Text = "Narcotics system offline.";
                return;
            }

            _statusRail.Set("patients", _system.Profiles.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("doses", _system.TotalDoses.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("overdoses", _system.TotalOverdoses.ToString(), _system.TotalOverdoses > 0 ? AshfallMetricCard.Criticality.Critical : AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("rehabs", _system.TotalRehabs.ToString(), AshfallMetricCard.Criticality.Normal);

            var sb = new System.Text.StringBuilder();
            sb.AppendLine("=== PATIENT TOXICITY & DEPENDENCY TELEMETRY ===");
            if (_system.Profiles.Count == 0)
            {
                sb.AppendLine("  No colonists currently under active pharmacological regimen or dependency tracking.");
            }
            else
            {
                foreach (var p in _system.Profiles)
                {
                    string rehabTag = p.inRehabBed ? $"[IN REHAB BED ({p.rehabProgressDays:F1}/14d)]" : "[OUTPATIENT]";
                    sb.AppendLine($"  - Survivor {p.survivorId}: Blood Toxicity: {p.bloodToxicity:F1}/100 {rehabTag}");

                    if (p.activeEffects.Count > 0)
                    {
                        sb.AppendLine($"      Active Effects: {p.activeEffects.Count} active dose(s)");
                    }
                    if (p.dependencies.Count > 0)
                    {
                        foreach (var dep in p.dependencies)
                        {
                            string withdr = dep.isWithdrawing ? " [WITHDRAWAL CRISIS]" : "";
                            sb.AppendLine($"      Dependency ({dep.chemId}): Level {dep.dependencyLevel:F0}% | Tolerance {dep.tolerance * 100:F0}% | Lapse: {dep.hoursSinceLastDose:F1}h{withdr}");
                        }
                    }
                }
            }

            _detailText.Text = sb.ToString();
        }
    }
}
