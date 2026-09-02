// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Stealth & Concealment Readout (Plan 181)
// Displays party detection risk, camouflage ratings, weapon noise profiles, and Night Ops tradeoffs.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Combat;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class StealthReadoutPanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private StealthSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(StealthSystem system)
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

            _shell = new AshfallDashboardShell("Stealth // Camouflage & Detection Readout", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("bypasses", "Successful Bypasses", "0", AshfallMetricCard.Criticality.Normal, minWidth: 150);
            _statusRail.AddCard("ambushes", "Ambush Strikes", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("detections", "Detections", "0", AshfallMetricCard.Criticality.Normal, minWidth: 120);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No active expedition stealth parties tracked.";
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
                _detailText.Text = "Stealth system offline.";
                return;
            }

            var state = _system.State;
            _statusRail.Set("bypasses", state.totalBypasses.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("ambushes", state.totalAmbushes.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("detections", state.totalDetections.ToString(), AshfallMetricCard.Criticality.Normal);

            if (state.expeditionStealthMap.Count == 0)
            {
                _detailText.Text = "No active expedition stealth profiles. Dispatch sorties equipped with camouflage gear to monitor detection risk.";
                return;
            }

            var summary = new System.Text.StringBuilder();
            summary.AppendLine("EXPEDITION CONCEALMENT PROFILES:");
            summary.AppendLine("──────────────────────────────────────────────────");
            foreach (var kv in state.expeditionStealthMap)
            {
                var party = kv.Value;
                string camo = party.equippedCamoIds.Count > 0 ? string.Join(", ", party.equippedCamoIds) : "None (Uncamouflaged)";
                string status = party.isDetected ? "DETECTED / COMPROMISED" : (party.hasAmbushAdvantage ? "AMBUSH ADVANTAGE READY" : "CONCEALED");
                summary.AppendLine($"• {party.expeditionId} | Mode: {party.travelMode} | Noise: {party.accumulatedNoise:F2} | Bypasses: {party.consecutiveBypasses} | Status: {status} | Gear: {camo}");
            }

            _detailText.Text = summary.ToString();
        }
    }
}
