// SPDX-License-Identifier: MIT
// ============================================================================
// Main Partial : Plans 174–177 — flagship panel wiring (presentation wave)
// Panels       : KennelPanel, BeliefsPanel, AnomalyWatchPanel, CyberneticsPanel
// Contract     : presentation only — every panel reads the canonical Core
//                authority through its host accessor; no gameplay math here.
// ============================================================================
using System;
using Godot;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        // ── Plan 174 — Kennel ──────────────────────────────────────────

        private UI.KennelPanel? _kennelPanel;

        private void OpenKennelPanel()
        {
            SetupCompanionAnimals();
            if (_kennelPanel == null)
            {
                _kennelPanel = new UI.KennelPanel();
                _kennelPanel.Bind(() => _companions);
                _kennelPanel.Visible = false;
                AddChild(_kennelPanel);
            }
            _kennelPanel.Visible = true;
            _kennelPanel.RefreshView();
        }

        // ── Plan 175 — Beliefs ─────────────────────────────────────────

        private UI.BeliefsPanel? _beliefsPanel;

        private void OpenBeliefsPanel()
        {
            SetupZealotry();
            if (_beliefsPanel == null)
            {
                _beliefsPanel = new UI.BeliefsPanel();
                _beliefsPanel.Bind(() => _zealotry);
                _beliefsPanel.Visible = false;
                AddChild(_beliefsPanel);
            }
            _beliefsPanel.Visible = true;
            _beliefsPanel.RefreshView();
        }

        // ── Plan 176 — Anomaly Watch ───────────────────────────────────

        private UI.AnomalyWatchPanel? _anomalyWatchPanel;

        private void OpenAnomalyWatchPanel()
        {
            SetupAnomalyHazard();
            if (_anomalyWatchPanel == null)
            {
                _anomalyWatchPanel = new UI.AnomalyWatchPanel();
                _anomalyWatchPanel.Bind(() => _anomalyHazard, GetAnomalyDetectionCapability);
                _anomalyWatchPanel.Visible = false;
                AddChild(_anomalyWatchPanel);
            }
            _anomalyWatchPanel.Visible = true;
            _anomalyWatchPanel.RefreshView();
        }

        // ── Plan 177 — Cybernetics ─────────────────────────────────────

        private UI.CyberneticsPanel? _cyberneticsPanel;

        private void OpenCyberneticsPanel()
        {
            SetupBionics();
            if (_cyberneticsPanel == null)
            {
                _cyberneticsPanel = new UI.CyberneticsPanel();
                _cyberneticsPanel.Bind(() => _bionics, () => _amputation);
                _cyberneticsPanel.Visible = false;
                AddChild(_cyberneticsPanel);
            }
            _cyberneticsPanel.Visible = true;
            _cyberneticsPanel.RefreshView();
        }
    }
}
