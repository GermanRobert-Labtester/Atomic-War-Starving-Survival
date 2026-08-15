using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class IronLeashDemandSnapshot
    {
        public string demandId;
        public int ammoTitheRequired;
        public int slaveHostagesCount;
        public float hoursRemaining;
        public float collarDetonationRiskPercent;
    }

    public class IronLeashSnapshot
    {
        public int totalTithesPaid;
        public List<IronLeashDemandSnapshot> demands = new List<IronLeashDemandSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Iron Leash Slaver Faction Demands HUD view-model.
    /// Monitors Iron Leash ammo tithe demands, hostage collar detonation threats,
    /// ransom payments, and anti-slavery raid counter-offensives.
    /// </summary>
    public class IronLeashHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDemandIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnIronLeashChanged;
        public event Action<string> OnPayAmmoTitheRequested; // (demandId)

        private Func<IronLeashSnapshot> _getSnapshot;
        private IronLeashSnapshot _snapshot;

        public void Bind(Func<IronLeashSnapshot> getSnapshot)
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
            ReportOutcome("Selected slaver demand: " + GetSelectedDemandName());
            return true;
        }

        public bool SelectPreviousDemand()
        {
            if (!IsOpen || _snapshot == null || _snapshot.demands == null || _snapshot.demands.Count == 0)
                return false;
            SelectedDemandIndex = (SelectedDemandIndex - 1 + _snapshot.demands.Count) % _snapshot.demands.Count;
            ReportOutcome("Selected slaver demand: " + GetSelectedDemandName());
            return true;
        }

        public bool RequestPayTithe()
        {
            if (!IsOpen || _snapshot == null || _snapshot.demands == null || _snapshot.demands.Count == 0)
            {
                ReportOutcome("No slaver demand selected for tithe payment.");
                return false;
            }

            var demand = GetSelectedDemand();
            if (demand == null) return false;

            if (OnPayAmmoTitheRequested == null)
            {
                ReportOutcome("Iron Leash courier radio link offline.");
                return false;
            }

            OnPayAmmoTitheRequested.Invoke(demand.demandId);
            ReportOutcome("Delivering Ammo Tithe (" + demand.ammoTitheRequired + " rounds) to Iron Leash Slavers...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No Iron Leash action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnIronLeashChanged?.Invoke();
        }

        private IronLeashDemandSnapshot GetSelectedDemand()
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
            return d != null ? d.demandId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("IRON LEASH SLAVER DEMANDS & TITHE MONITOR  [L] close  ·  [Tab] cycle  ·  [P] pay ammo tithe");

            if (_snapshot == null)
            {
                sb.Append("\nIron Leash radio telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nSLAVER STATS: Total Tithes Paid: ").Append(_snapshot.totalTithesPaid);

            sb.Append("\n\nACTIVE IRON LEASH TITHE DEMANDS:");
            if (_snapshot.demands == null || _snapshot.demands.Count == 0)
            {
                sb.Append("\n  No active tithe demands from Iron Leash.");
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
                      .Append(" — Required: ").Append(demand.ammoTitheRequired).Append(" ammo rounds")
                      .Append(" | Hostages: ").Append(demand.slaveHostagesCount)
                      .Append(" | Deadline: ").Append(demand.hoursRemaining.ToString("0.#")).Append(" hrs");

                    if (demand.hoursRemaining < 6f) sb.Append("  ★ [CRITICAL: COLLAR DETONATION IMMINENT!]");
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
