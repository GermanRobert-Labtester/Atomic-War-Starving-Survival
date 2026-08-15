using System;
using System.Collections.Generic;
using System.Text;
using UnityEngine;

namespace AtomicWar._Game.UI
{
    public class AshDriftRouteSnapshot
    {
        public string routeId;
        public bool isBlocked;
        public float clearHoursRequired;
        public float clearHoursCompleted;
    }

    public class AshDriftSnapshot
    {
        public int totalBlockedRoutes;
        public List<AshDriftRouteSnapshot> routes = new List<AshDriftRouteSnapshot>();
    }

    /// <summary>
    /// Protocol Zero — Ash Drifts & Route Blockage HUD view-model.
    /// Monitors map route blockages caused by fallout storm ash drifts, shovel clearing progress,
    /// detour routing options, and expedition path clearing telemetry.
    /// </summary>
    public class AshDriftHUD : MonoBehaviour
    {
        public bool IsOpen { get; private set; }
        public int SelectedRouteIndex { get; private set; } = 0;
        public string PanelSummary { get; private set; } = string.Empty;
        public string LastOutcome { get; private set; } = string.Empty;

        public event Action OnAshDriftChanged;
        public event Action<string, bool, float> OnClearPathRequested; // (routeId, hasShovel, hoursSpent)

        private Func<AshDriftSnapshot> _getSnapshot;
        private AshDriftSnapshot _snapshot;

        public void Bind(Func<AshDriftSnapshot> getSnapshot)
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

        public bool SelectNextRoute()
        {
            if (!IsOpen || _snapshot == null || _snapshot.routes == null || _snapshot.routes.Count == 0)
                return false;
            SelectedRouteIndex = (SelectedRouteIndex + 1) % _snapshot.routes.Count;
            ReportOutcome("Selected map route: " + GetSelectedRouteName());
            return true;
        }

        public bool SelectPreviousRoute()
        {
            if (!IsOpen || _snapshot == null || _snapshot.routes == null || _snapshot.routes.Count == 0)
                return false;
            SelectedRouteIndex = (SelectedRouteIndex - 1 + _snapshot.routes.Count) % _snapshot.routes.Count;
            ReportOutcome("Selected map route: " + GetSelectedRouteName());
            return true;
        }

        public bool RequestClearPath(bool hasShovel, float hoursSpent)
        {
            if (!IsOpen || _snapshot == null || _snapshot.routes == null || _snapshot.routes.Count == 0)
            {
                ReportOutcome("No blocked route selected for path clearing.");
                return false;
            }

            var route = GetSelectedRoute();
            if (route == null) return false;

            if (!route.isBlocked)
            {
                ReportOutcome("Route " + route.routeId + " is not blocked.");
                return false;
            }

            if (OnClearPathRequested == null)
            {
                ReportOutcome("Route clearing crew link offline.");
                return false;
            }

            OnClearPathRequested.Invoke(route.routeId, hasShovel, hoursSpent);
            ReportOutcome("Shoveling ash drift at " + route.routeId + " (" + hoursSpent.ToString("0.#") + " hrs" + (hasShovel ? " with Shovel 2x speed" : " hands only") + ")...");
            return true;
        }

        public void ReportOutcome(string message)
        {
            LastOutcome = string.IsNullOrEmpty(message) ? "No ash drift action logged." : message;
            Refresh();
        }

        public void Refresh()
        {
            _snapshot = _getSnapshot != null ? _getSnapshot() : null;
            RebuildPanel();
            OnAshDriftChanged?.Invoke();
        }

        private AshDriftRouteSnapshot GetSelectedRoute()
        {
            if (_snapshot != null && _snapshot.routes != null && SelectedRouteIndex >= 0 && SelectedRouteIndex < _snapshot.routes.Count)
            {
                return _snapshot.routes[SelectedRouteIndex];
            }
            return null;
        }

        private string GetSelectedRouteName()
        {
            var r = GetSelectedRoute();
            return r != null ? r.routeId : "None";
        }

        private void RebuildPanel()
        {
            var sb = new StringBuilder("ASH DRIFTS & ROUTE CLEARING MONITOR  [S] close  ·  [Tab] cycle  ·  [C] clear path with shovel");

            if (_snapshot == null)
            {
                sb.Append("\nAsh drift telemetry offline.");
                PanelSummary = sb.ToString();
                return;
            }

            sb.Append("\nMAP ROUTE STATS: Total Blocked Routes: ").Append(_snapshot.totalBlockedRoutes);

            sb.Append("\n\nWASTELAND EXPEDITION ROUTES:");
            if (_snapshot.routes == null || _snapshot.routes.Count == 0)
            {
                sb.Append("\n  No map routes currently monitored.");
            }
            else
            {
                for (int i = 0; i < _snapshot.routes.Count; i++)
                {
                    var route = _snapshot.routes[i];
                    if (route == null) continue;

                    bool selected = (i == SelectedRouteIndex);
                    sb.Append("\n").Append(selected ? "> " : "  ")
                      .Append("Route ").Append(route.routeId);

                    if (route.isBlocked)
                    {
                        sb.Append(" — [BLOCKED BY ASH DRIFT]")
                          .Append(" Progress: ").Append(route.clearHoursCompleted.ToString("0.#")).Append(" / ").Append(route.clearHoursRequired.ToString("0.#")).Append(" hrs");
                    }
                    else
                    {
                        sb.Append(" — ✔ [OPEN & CLEAR]");
                    }
                }
            }

            if (!string.IsNullOrEmpty(LastOutcome))
            {
                sb.Append("\n\nCLEARING LOG: ").Append(LastOutcome);
            }

            PanelSummary = sb.ToString();
        }
    }
}
