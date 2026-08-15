using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class UXOFieldNodeSnapshot
    {
        public string nodeId;
        public float mineDensity; // 0..1
        public bool probed;
        public bool cleared;
        public int minesRemaining;
        public int tripwiresRemaining;
        public float acousticSignature;
    }

    public class UXOFieldSnapshot
    {
        public float globalAcousticSignature;
        public int totalProbesPerformed;
        public int totalMinesDetonated;
        public int totalTripwiresCut;
        public int totalMinesDisarmed;
        public List<UXOFieldNodeSnapshot> nodes = new List<UXOFieldNodeSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — UXO Field & Mine Clearance HUD view-model.
    /// Manages unexploded ordnance probing (soil prodding with mine prods),
    /// tripwire wire-cutting risk, mine disarming, and acoustic signature telemetry
    /// to avoid attracting loitering suicide drones.
    /// </summary>
    public class UXOFieldHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedNodeIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnUXOFieldChanged;
        public event Action<string, string> OnProbeRequested;      // (nodeId, survivorId)
        public event Action<string, string> OnCutTripwireRequested; // (nodeId, survivorId)
        public event Action<string, string> OnDisarmMineRequested;  // (nodeId, survivorId)

        private Func<UXOFieldSnapshot> _getSnapshot;
        private UXOFieldSnapshot _snapshot;

        public void Bind(Func<UXOFieldSnapshot> getSnapshot)
        {
            _getSnapshot = getSnapshot;
            Refresh();
        }

        public void Open()
        {
            if (IsOpen) return;
            IsOpen = true;
            Refresh();
        }

        public void Close()
        {
            if (!IsOpen) return;
            IsOpen = false;
            Refresh();
        }

        public void Toggle()
        {
            if (IsOpen) Close();
            else Open();
        }

        public bool SelectNextNode()
        {
            if (!IsOpen || _snapshot == null || _snapshot.nodes == null || _snapshot.nodes.Count == 0)
                return false;
            SelectedNodeIndex = (SelectedNodeIndex + 1) % _snapshot.nodes.Count;
            ReportOutcome("Selected minefield zone: " + GetSelectedNodeName());
            return true;
        }

        public bool SelectPreviousNode()
        {
            if (!IsOpen || _snapshot == null || _snapshot.nodes == null || _snapshot.nodes.Count == 0)
                return false;
            SelectedNodeIndex = (SelectedNodeIndex - 1 + _snapshot.nodes.Count) % _snapshot.nodes.Count;
            ReportOutcome("Selected minefield zone: " + GetSelectedNodeName());
            return true;
        }

        public bool RequestProbeNode(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.nodes == null || _snapshot.nodes.Count == 0)
            {
                ReportOutcome("No UXO zone selected for probing.");
                return false;
            }

            var node = GetSelectedNode();
            if (node == null) return false;

            if (OnProbeRequested == null)
            {
                ReportOutcome("UXO field telemetry link offline.");
                return false;
            }

            OnProbeRequested.Invoke(node.nodeId, survivorId ?? "unassigned");
            ReportOutcome("Probing soil at " + node.nodeId + " with mine prod...");
            return true;
        }

        public bool RequestCutTripwire(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.nodes == null || _snapshot.nodes.Count == 0)
            {
                ReportOutcome("No UXO zone selected for wire cutting.");
                return false;
            }

            var node = GetSelectedNode();
            if (node == null) return false;

            if (OnCutTripwireRequested == null)
            {
                ReportOutcome("UXO field telemetry link offline.");
                return false;
            }

            OnCutTripwireRequested.Invoke(node.nodeId, survivorId ?? "unassigned");
            ReportOutcome("Attempting tripwire cut at " + node.nodeId + "...");
            return true;
        }

        public bool RequestDisarmMine(string survivorId)
        {
            if (!IsOpen || _snapshot == null || _snapshot.nodes == null || _snapshot.nodes.Count == 0)
            {
                ReportOutcome("No mine selected for disarming.");
                return false;
            }

            var node = GetSelectedNode();
            if (node == null) return false;

            if (OnDisarmMineRequested == null)
            {
                ReportOutcome("UXO field telemetry link offline.");
                return false;
            }

            OnDisarmMineRequested.Invoke(node.nodeId, survivorId ?? "unassigned");
            ReportOutcome("Attempting mine disarm at " + node.nodeId + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No UXO action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnUXOFieldChanged?.Invoke();
        }

        private UXOFieldNodeSnapshot GetSelectedNode()
        {
            if (_snapshot != null && _snapshot.nodes != null && SelectedNodeIndex >= 0 && SelectedNodeIndex < _snapshot.nodes.Count)
            {
                return _snapshot.nodes[SelectedNodeIndex];
            }
            return null;
        }

        private string GetSelectedNodeName()
        {
            var node = GetSelectedNode();
            return node != null ? node.nodeId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("UXO FIELD & MINE CLEARANCE TERMINAL  [U] close  ·  [Tab] cycle  ·  [P] probe  ·  [C] cut wire  ·  [D] disarm");

            if (_snapshot == null)
            {
                sb.Append("\nUXO field telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nFIELD STATS: Probes: ").Append(_snapshot.totalProbesPerformed)
              .Append("  ·  Disarmed: ").Append(_snapshot.totalMinesDisarmed)
              .Append("  ·  Tripwires Cut: ").Append(_snapshot.totalTripwiresCut)
              .Append("  ·  Detonations: ").Append(_snapshot.totalMinesDetonated);

            sb.Append("\nACOUSTIC THREAT: ").Append(_snapshot.globalAcousticSignature.ToString("0.#")).Append(" dB");
            if (_snapshot.globalAcousticSignature > 75f)
                sb.Append("  [DANGER: LOITERING SUICIDE DRONES INBOUND!]");
            else if (_snapshot.globalAcousticSignature > 45f)
                sb.Append("  [WARNING: ACOUSTIC SIGNATURE ELEVATED]");

            sb.Append("\n\nMINEFIELD SECTORS:");
            if (_snapshot.nodes == null || _snapshot.nodes.Count == 0)
            {
                sb.Append("\n  No active UXO minefields detected in current sector.");
            }
            else
            {
                for (int i = 0; i < _snapshot.nodes.Count; i++)
                {
                    var node = _snapshot.nodes[i];
                    if (node == null) continue;

                    bool selected = (i == SelectedNodeIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Sector ").Append(node.nodeId)
                      .Append(" — Density: ").Append((node.mineDensity * 100f).ToString("0")).Append("%")
                      .Append(" | Mines: ").Append(node.minesRemaining)
                      .Append(" | Tripwires: ").Append(node.tripwiresRemaining);

                    if (node.cleared) sb.Append(" [CLEARED]");
                    else if (node.probed) sb.Append(" [PROBED]");
                    else sb.Append(" [UNPROBED - HIGH DANGER]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTELEMETRY LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
