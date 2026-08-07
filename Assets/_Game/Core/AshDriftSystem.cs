using System;
using System.Collections.Generic;
using UnityEngine;

namespace AtomicWar._Game.Core
{
    [Serializable]
    public class RouteBlockageState
    {
        public string routeId; // e.g. "hospital_to_precinct"
        public bool isBlocked = false;
        public float clearHoursRequired = 3.0f;
        public float clearHoursCompleted = 0f;
    }

    /// <summary>
    /// Prompt #371: System: Ash Drifts (Map Blockages).
    /// Prolonged FalloutStorms block map routes with ash drifts.
    /// Player must execute a ClearPath action with a Shovel or find a longer detour.
    /// </summary>
    
    [Serializable]
    public class AshDriftSystemSave
    {
        public string systemId = "ash_drift_system";

        public List<RouteBlockageState> routes = new List<RouteBlockageState>();
    }
public class AshDriftSystem
    {
        private readonly Dictionary<string, RouteBlockageState> _routes = new Dictionary<string, RouteBlockageState>();

        public event Action<string> OnRouteBlockedByAshDrift;
        public event Action<string> OnRouteCleared;
        public event Action<string, float> OnClearPathProgressed;

        public IReadOnlyDictionary<string, RouteBlockageState> Routes => _routes;

        public void BlockRoute(string routeId)
        {
            if (string.IsNullOrEmpty(routeId)) return;
            if (!_routes.TryGetValue(routeId, out var state))
            {
                state = new RouteBlockageState { routeId = routeId };
                _routes[routeId] = state;
            }
            state.isBlocked = true;
            state.clearHoursCompleted = 0f;
            OnRouteBlockedByAshDrift?.Invoke(routeId);
        }

        public bool ClearPathAction(string routeId, bool hasShovel, float hoursSpent)
        {
            if (!_routes.TryGetValue(routeId, out var state) || !state.isBlocked)
                return true;

            float effectiveHours = hasShovel ? hoursSpent * 2.0f : hoursSpent;
            state.clearHoursCompleted += effectiveHours;
            OnClearPathProgressed?.Invoke(routeId, state.clearHoursCompleted);

            if (state.clearHoursCompleted >= state.clearHoursRequired)
            {
                state.isBlocked = false;
                OnRouteCleared?.Invoke(routeId);
                return true;
            }

            return false;
        }

        public bool IsRouteBlocked(string routeId)
        {
            return _routes.TryGetValue(routeId, out var state) && state.isBlocked;
        }
    
        // ── Save / Load ────────────────────────────────────────────────
        public AshDriftSystemSave CaptureState() => new AshDriftSystemSave
        {
            routes = SaveMap.Capture(_routes),
        };

        public void RestoreState(AshDriftSystemSave saved) =>
            SaveMap.Restore(_routes, saved?.routes, e => e.routeId);

}
}
