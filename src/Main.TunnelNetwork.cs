// SPDX-License-Identifier: MIT
// ============================================================================
// ASHFALL Plan 167 — Underground Tunnel Network host wiring.
// The authority is Ashfall.Core.Underground.TunnelNetworkSystem, owned by
// WastelandMapSystem.Tunnels and persisted inside the canonical world-map save
// section. This host adds the daily structural tick, a census projection, and
// the reinforce/clear-hazard commands; it is deliberately NOT a second store.
// ============================================================================

using System;
using Godot;
using Ashfall.Core.Underground;

namespace AtomicWar.GodotApp
{
    public partial class Main
    {
        /// <summary>
        /// Plan 167 — the live subterranean network. Null until the world is
        /// composed; always the canonical instance owned by the world map.
        /// </summary>
        public TunnelNetworkSystem? TunnelNetwork
        {
            get
            {
                SetupWorld();
                return _world?.WastelandMap?.Tunnels;
            }
        }

        /// <summary>
        /// Ensures the world map (and therefore the authored tunnel catalog) is
        /// composed. The catalog itself is seeded by WastelandMapCatalogLoader
        /// during SetupWorld, so this is a thin ordering guarantee.
        /// </summary>
        public void SetupTunnelNetwork()
        {
            SetupWorld();
        }

        /// <summary>
        /// Plan 167 — advances structural wear and collapse risk for the day.
        /// Called by the phase-5 tunnel day owner inside the fail-closed advance.
        /// </summary>
        public void TickTunnelNetwork(int day)
        {
            SetupWorld();
            _world?.WastelandMap?.Tunnels?.TickDay(day);
        }

        /// <summary>Read-only census of the live tunnel network for probes and the map panel.</summary>
        public TunnelNetworkCensus GetTunnelNetworkCensus()
        {
            SetupWorld();
            return _world?.WastelandMap?.Tunnels?.GetCensus() ?? default;
        }

        /// <summary>
        /// Plan 167 — reinforces a segment (spending nothing here; the caller /
        /// UI owns any resource cost). Routes through the canonical authority
        /// and marks the world save dirty.
        /// </summary>
        public bool ReinforceTunnelSegment(string segmentId, float integrityGain = 25f)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return false;
            SetupWorld();

            var tunnels = _world?.WastelandMap?.Tunnels;
            if (tunnels == null) return false;

            bool reinforced = tunnels.ReinforceSegment(segmentId.Trim(), integrityGain);
            if (reinforced)
            {
                _worldDirty = true;
                _mapPanel?.RefreshView();
            }
            return reinforced;
        }

        /// <summary>Plan 167 — clears a known hazard from a segment through the canonical authority.</summary>
        public bool ClearTunnelHazard(string segmentId, TunnelHazardType hazard)
        {
            if (string.IsNullOrWhiteSpace(segmentId)) return false;
            SetupWorld();

            var tunnels = _world?.WastelandMap?.Tunnels;
            if (tunnels == null) return false;

            bool cleared = tunnels.ClearHazard(segmentId.Trim(), hazard);
            if (cleared)
            {
                _worldDirty = true;
                _mapPanel?.RefreshView();
            }
            return cleared;
        }

        /// <summary>
        /// Plan 167 — evaluates whether an underground bypass exists between two
        /// locations and how many surface hours it saves.
        /// </summary>
        public (bool Found, float SavedHours, string Reason) EvaluateTunnelBypass(string fromLocation, string toLocation)
        {
            if (string.IsNullOrWhiteSpace(fromLocation) || string.IsNullOrWhiteSpace(toLocation))
                return (false, 0f, "Both endpoints are required.");

            SetupWorld();
            var tunnels = _world?.WastelandMap?.Tunnels;
            if (tunnels == null) return (false, 0f, "Tunnel network unavailable.");

            return tunnels.EvaluateSurfaceBypass(fromLocation.Trim(), toLocation.Trim());
        }
    }
}
