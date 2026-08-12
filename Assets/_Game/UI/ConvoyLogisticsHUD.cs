using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class ConvoyDestinationSnapshot
    {
        public string nodeId;
        public string displayName;
        public float distanceKm;
        public float fuelRequiredLiters;
        public float travelHours;
        public bool discovered;
        public bool cleared;
    }

    public class ConvoyMissionSnapshot
    {
        public string missionId;
        public string destinationNodeId;
        public int survivorCount;
        public float travelProgressHours;
        public float totalTravelHours;
        public bool hasSled;
        public bool sledAbandoned;
        public bool caughtInBlizzard;
    }

    public class ConvoyLogisticsSnapshot
    {
        public bool hasSnowCrawler;
        public bool crawlerOperational;
        public float crawlerFuelLiters;
        public float crawlerOilCondition;
        public float crawlerTrackCondition;
        public bool hasHandCrankSled;
        public List<ConvoyDestinationSnapshot> knownDestinations = new List<ConvoyDestinationSnapshot>();
        public List<ConvoyMissionSnapshot> activeMissions = new List<ConvoyMissionSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Convoy Logistics HUD view-model.
    /// Controls long-range expedition planning, Snow-Crawler vehicle maintenance
    /// (fuel, oil, caterpillar track health), hand-crank sled status, active convoy
    /// progress tracking, and destination dispatch requests.
    /// </summary>
    public class ConvoyLogisticsHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedDestinationIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnConvoyLogisticsChanged;
        public event Action<string, bool> OnDispatchRequested; // (nodeId, useCrawler)
        public event Action<string> OnMaintenanceRequested;  // ("refuel", "oil", "tracks")

        private Func<ConvoyLogisticsSnapshot> _getSnapshot;
        private ConvoyLogisticsSnapshot _snapshot;

        public void Bind(Func<ConvoyLogisticsSnapshot> getSnapshot)
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

        public bool SelectNextDestination()
        {
            if (!IsOpen || _snapshot == null || _snapshot.knownDestinations == null || _snapshot.knownDestinations.Count == 0)
                return false;
            SelectedDestinationIndex = (SelectedDestinationIndex + 1) % _snapshot.knownDestinations.Count;
            ReportOutcome("Selected destination: " + GetSelectedDestinationName());
            return true;
        }

        public bool SelectPreviousDestination()
        {
            if (!IsOpen || _snapshot == null || _snapshot.knownDestinations == null || _snapshot.knownDestinations.Count == 0)
                return false;
            SelectedDestinationIndex = (SelectedDestinationIndex - 1 + _snapshot.knownDestinations.Count) % _snapshot.knownDestinations.Count;
            ReportOutcome("Selected destination: " + GetSelectedDestinationName());
            return true;
        }

        public bool RequestDispatch(bool useCrawler)
        {
            if (!IsOpen || _snapshot == null || _snapshot.knownDestinations == null || _snapshot.knownDestinations.Count == 0)
            {
                ReportOutcome("No valid destination selected for convoy dispatch.");
                return false;
            }

            if (SelectedDestinationIndex < 0 || SelectedDestinationIndex >= _snapshot.knownDestinations.Count)
                SelectedDestinationIndex = 0;

            var dest = _snapshot.knownDestinations[SelectedDestinationIndex];
            if (dest == null) return false;

            if (OnDispatchRequested == null)
            {
                ReportOutcome("Convoy logistics dispatch radio link offline.");
                return false;
            }

            OnDispatchRequested.Invoke(dest.nodeId, useCrawler);
            ReportOutcome("Dispatching convoy to " + dest.displayName + (useCrawler ? " via Snow-Crawler." : " on foot with sled."));
            return true;
        }

        public bool RequestMaintenance(string maintenanceType)
        {
            if (!IsOpen) return false;
            if (OnMaintenanceRequested == null)
            {
                ReportOutcome("Snow-Crawler workshop link offline.");
                return false;
            }

            OnMaintenanceRequested.Invoke(maintenanceType);
            ReportOutcome("Maintenance requested: " + maintenanceType);
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No convoy action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnConvoyLogisticsChanged?.Invoke();
        }

        private string GetSelectedDestinationName()
        {
            if (_snapshot != null && _snapshot.knownDestinations != null && SelectedDestinationIndex >= 0 && SelectedDestinationIndex < _snapshot.knownDestinations.Count)
            {
                return _snapshot.knownDestinations[SelectedDestinationIndex]?.displayName ?? "Unknown Node";
            }
            return "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("CONVOY LOGISTICS TERMINAL  [C] close  ·  [Tab] cycle dest  ·  [D] dispatch  ·  [M] maintain");

            if (_snapshot == null)
            {
                sb.Append("\nConvoy telemetry offline. Sled/Crawler status unavailable.");
                PanelSummary = sb.ToString();
                return;
            }

            // Vehicle Status Header
            sb.Append("\nVEHICLE STATUS: ");
            if (_snapshot.hasSnowCrawler)
            {
                string state = _snapshot.crawlerOperational ? "OPERATIONAL" : "DAMAGED/DISABLED";
                sb.Append("Snow-Crawler [").Append(state).Append("]")
                  .Append(" · Fuel: ").Append(_snapshot.crawlerFuelLiters.ToString("0.#")).Append(" L")
                  .Append(" · Oil: ").Append((_snapshot.crawlerOilCondition * 100f).ToString("0")).Append("%")
                  .Append(" · Tracks: ").Append((_snapshot.crawlerTrackCondition * 100f).ToString("0")).Append("%");
            }
            else
            {
                sb.Append("No Snow-Crawler available.");
            }

            sb.Append("\nEQUIPMENT: ");
            sb.Append(_snapshot.hasHandCrankSled ? "Hand-Crank Sled [READY]" : "No Sled [ON FOOT ONLY]");

            // Known Destinations List
            sb.Append("\n\nKNOWN DESTINATIONS:");
            if (_snapshot.knownDestinations == null || _snapshot.knownDestinations.Count == 0)
            {
                sb.Append("\n  No distant destinations discovered beyond immediate scavenge radius.");
            }
            else
            {
                for (int i = 0; i < _snapshot.knownDestinations.Count; i++)
                {
                    var dest = _snapshot.knownDestinations[i];
                    if (dest == null) continue;

                    bool selected = (i == SelectedDestinationIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append(dest.displayName ?? dest.nodeId)
                      .Append(" — ").Append(dest.distanceKm.ToString("0.#")).Append(" km")
                      .Append(" | Fuel req: ").Append(dest.fuelRequiredLiters.ToString("0.#")).Append(" L")
                      .Append(" | Est. Travel: ").Append(dest.travelHours.ToString("0.#")).Append(" hrs");

                    if (dest.cleared) sb.Append(" [SECURED]");
                    else if (!dest.discovered) sb.Append(" [UNEXPLORED]");
                }
            }

            // Active Missions
            sb.Append("\n\nACTIVE MISSIONS:");
            if (_snapshot.activeMissions == null || _snapshot.activeMissions.Count == 0)
            {
                sb.Append("\n  No convoys currently in transit.");
            }
            else
            {
                foreach (var mission in _snapshot.activeMissions)
                {
                    if (mission == null) continue;
                    float pct = mission.totalTravelHours > 0f ? (mission.travelProgressHours / mission.totalTravelHours) * 100f : 0f;
                    sb.Append("\n  Mission to ").Append(mission.destinationNodeId)
                      .Append(" — Progress: ").Append(pct.ToString("0")).Append("%")
                      .Append(" (").Append(mission.survivorCount).Append(" survivors)");

                    if (mission.caughtInBlizzard) sb.Append("  [BLIZZARD HAZARD!]");
                    if (mission.sledAbandoned) sb.Append("  [SLED ABANDONED]");
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nLOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
