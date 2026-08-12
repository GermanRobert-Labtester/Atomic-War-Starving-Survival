using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class RationTitheDemandSnapshot
    {
        public string demandId;
        public string raiderFactionName;
        public float foodRequiredKg;
        public float waterRequiredLiters;
        public float hoursRemaining;
        public string penaltyDescription;
    }

    public class RationTitheSnapshot
    {
        public int totalTithesPaid;
        public List<RationTitheDemandSnapshot> demands = new List<RationTitheDemandSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Raider Ration Tithe & Extortion Demands HUD view-model.
    /// Monitors extortion demands from external raider warlords, food/water tithe deadlines,
    /// tithe refusal penalties (raid triggers), and resource tribute payments.
    /// </summary>
    public class RationTitheHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDemandIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnRationTitheChanged;
        public event Action<string> OnPayRationTitheRequested; // (demandId)
        public event Action<string> OnRefuseRationTitheRequested; // (demandId)

        private Func<RationTitheSnapshot> _getSnapshot;
        private RationTitheSnapshot _snapshot;

        public void Bind(Func<RationTitheSnapshot> getSnapshot)
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

        public bool SelectNextDemand()
        {
            if (!IsOpen || _snapshot == null || _snapshot.demands == null || _snapshot.demands.Count == 0)
                return false;
            SelectedDemandIndex = (SelectedDemandIndex + 1) % _snapshot.demands.Count;
            ReportOutcome("Selected raider demand: " + GetSelectedDemandName());
            return true;
        }

        public bool SelectPreviousDemand()
        {
            if (!IsOpen || _snapshot == null || _snapshot.demands == null || _snapshot.demands.Count == 0)
                return false;
            SelectedDemandIndex = (SelectedDemandIndex - 1 + _snapshot.demands.Count) % _snapshot.demands.Count;
            ReportOutcome("Selected raider demand: " + GetSelectedDemandName());
            return true;
        }

        public bool RequestPayTithe()
        {
            if (!IsOpen || _snapshot == null || _snapshot.demands == null || _snapshot.demands.Count == 0)
            {
                ReportOutcome("No raider demand selected for tithe payment.");
                return false;
            }

            var demand = GetSelectedDemand();
            if (demand == null) return false;

            if (OnPayRationTitheRequested == null)
            {
                ReportOutcome("Courier drop box link offline.");
                return false;
            }

            OnPayRationTitheRequested.Invoke(demand.demandId);
            ReportOutcome("Paying Ration Tithe (" + demand.foodRequiredKg.ToString("0.#") + " kg Food / " + demand.waterRequiredLiters.ToString("0.#") + " L Water) to " + demand.raiderFactionName + "...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No ration tithe action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnRationTitheChanged?.Invoke();
        }

        private RationTitheDemandSnapshot GetSelectedDemand()
        {
            if (_snapshot != null && _snapshot.demands != null && SelectedDemandIndex >= 0 && SelectedDemandIndex < _snapshot.demands.Count)
            {
                return _snapshot.demands[SelectedDemandIndex];
            }
            return null;
        }

        private string GetSelectedDemandName()
        {
            var d = GetSelectedDemand();
            return d != null ? (d.demandId + " — " + d.raiderFactionName) : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("RAIDER RATION TITHE & EXTORTION DEMANDS  [T] close  ·  [Tab] cycle  ·  [P] pay ration tithe");

            if (_snapshot == null)
            {
                sb.Append("\nRaider demand telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nTITHE STATS: Total Food Tithes Paid: ").Append(_snapshot.totalTithesPaid);

            sb.Append("\n\nRAIDER EXTORTION DEMANDS:");
            if (_snapshot.demands == null || _snapshot.demands.Count == 0)
            {
                sb.Append("\n  No active food tithe demands.");
            }
            else
            {
                for (int i = 0; i < _snapshot.demands.Count; i++)
                {
                    var demand = _snapshot.demands[i];
                    if (demand == null) continue;

                    bool selected = (i == SelectedDemandIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Demand ").Append(demand.demandId)
                      .Append(" (").Append(demand.raiderFactionName ?? "Warlord Raiders").Append(")")
                      .Append(" — Food: ").Append(demand.foodRequiredKg.ToString("0.#")).Append(" kg")
                      .Append(" | Water: ").Append(demand.waterRequiredLiters.ToString("0.#")).Append(" L")
                      .Append(" | Deadline: ").Append(demand.hoursRemaining.ToString("0.#")).Append(" hrs");

                    if (demand.hoursRemaining < 6f) sb.Append("  ★ [CRITICAL: EXTORTION DEADLINE IMMINENT!]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nTITHE LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
