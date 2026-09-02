// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Detention & Interrogation (Plan 179)
// Displays captive cells, health, compliance, fear/trust metrics, and interrogation logs.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Factions;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class PrisonerPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private PrisonerSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(PrisonerSystem system)
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

            _shell = new AshfallDashboardShell("Detention // Prisoner Management & Intel", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("detained", "Detained", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("intel", "Intel Extracted", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("recruits", "Recruits", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);
            _statusRail.AddCard("escapes", "Escapes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No captives currently detained.";
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
                _detailText.Text = "Prisoner system offline.";
                return;
            }

            var state = _system.State;
            int active = 0;
            foreach (var c in state.captives)
            {
                if (c.status == CaptiveStatus.Detained) active++;
            }

            _statusRail.Set("detained", $"{active} / {state.maxCellCapacity}", AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("intel", state.extractedIntelRecords.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("recruits", state.totalRecruits.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("escapes", state.totalEscapes.ToString(), AshfallMetricCard.Criticality.Normal);

            if (state.captives.Count == 0)
            {
                _detailText.Text = "All detention cells vacant. Capture hostile combatants during wasteland sorties.";
                return;
            }

            var summary = new System.Text.StringBuilder();
            summary.AppendLine("CAPTIVE ROSTER & CELL STATUS:");
            summary.AppendLine("──────────────────────────────────────────────────");
            foreach (var c in state.captives)
            {
                string guard = string.IsNullOrEmpty(c.assignedGuardId) ? "UNGUARDED" : c.assignedGuardId;
                summary.AppendLine($"• {c.captiveId} [{c.sourceFactionId}] | Status: {c.status} | HP: {c.health:F0}% | Compliance: {c.compliance:F0}% | Trust: {c.trust:F0}% | Fear: {c.fear:F0}% | Escape: {c.escapeProgress:F0}% | Guard: {guard}");
            }

            if (state.extractedIntelRecords.Count > 0)
            {
                summary.AppendLine("\nEXTRACTED INTELLIGENCE ARCHIVE:");
                summary.AppendLine("──────────────────────────────────────────────────");
                foreach (var intel in state.extractedIntelRecords)
                {
                    string status = intel.isTrueIntel ? "VERIFIED" : "UNVERIFIED";
                    summary.AppendLine($"• [{intel.intelId}] Source: {intel.sourceCaptiveId} | Type: {intel.intelType} | Day: {intel.dayExtracted} | Status: {status}");
                }
            }

            _detailText.Text = summary.ToString();
        }
    }
}
