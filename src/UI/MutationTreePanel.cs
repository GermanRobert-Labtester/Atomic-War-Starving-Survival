// SPDX-License-Identifier: MIT
// ============================================================================
// UI Panel: Mutation Tree & Genetics (Plan 180)
// Displays genetic instability, radiation exposure, active mutation branches, and clinical therapy.
// ============================================================================
using System;
using Godot;
using Ashfall.Core.Medical;
using Ashfall.Core.UI;

namespace AtomicWar.GodotApp.UI
{
    public partial class MutationTreePanel : Control, IBindablePanel
    {
        public event Action? OnClose;

        private AshfallDashboardShell _shell = null!;
        private AshfallStatusRail? _statusRail;
        private VBoxContainer _contentStack = null!;
        private Label _detailText = null!;

        private MutationSystem? _system;

        public bool IsBound => _system != null;

        public void Bind(MutationSystem system)
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

            _shell = new AshfallDashboardShell("Genetics // Mutation Trees & Instability", minWidth: 1000, minHeight: 650);
            AddChild(_shell);

            _statusRail = _shell.SetStatusRail();
            _statusRail.AddCard("profiles", "Tracked Survivors", "0", AshfallMetricCard.Criticality.Normal, minWidth: 140);
            _statusRail.AddCard("mutations", "Total Mutations", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);
            _statusRail.AddCard("therapies", "Gene Therapies", "0", AshfallMetricCard.Criticality.Normal, minWidth: 130);

            _contentStack = new VBoxContainer();
            _contentStack.AddThemeConstantOverride("separation", 12);
            _contentStack.SizeFlagsHorizontal = SizeFlags.ExpandFill;
            _contentStack.SizeFlagsVertical = SizeFlags.ExpandFill;

            _detailText = new Label();
            _detailText.Text = "No genetic instability profiles active.";
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
                _detailText.Text = "Mutation system offline.";
                return;
            }

            var state = _system.State;
            _statusRail.Set("profiles", state.profiles.Count.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("mutations", state.totalMutationsAcquired.ToString(), AshfallMetricCard.Criticality.Normal);
            _statusRail.Set("therapies", state.totalGeneTherapies.ToString(), AshfallMetricCard.Criticality.Normal);

            if (state.profiles.Count == 0)
            {
                _detailText.Text = "No biological mutation profiles registered. Severe radiation exposure will catalyze mutations.";
                return;
            }

            var summary = new System.Text.StringBuilder();
            summary.AppendLine("SURVIVOR MUTATION & INSTABILITY PROFILES:");
            summary.AppendLine("──────────────────────────────────────────────────");
            foreach (var p in state.profiles)
            {
                string muts = p.activeMutationIds.Count > 0 ? string.Join(", ", p.activeMutationIds) : "None (Clean Genome)";
                summary.AppendLine($"• {p.survivorId} | Instability: {p.geneticInstability:F1}% (Peak: {p.lifetimePeakInstability:F1}%) | Dose: {p.cumulativeRadDose:F0} mSv | Active: {muts}");
            }

            _detailText.Text = summary.ToString();
        }
    }
}
