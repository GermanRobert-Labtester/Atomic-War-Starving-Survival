// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// EN-02 / UNBLOCK-05: The Living Map route projection read model.
    /// Derives candidate path, per-edge conditions, flooded/amphibious status,
    /// total hops, and distance from canonical WastelandMapSystem.PlanRoute.
    /// Pure projection; writes no state and does not rebuild GraphTravelPlanner.
    /// </summary>
    public sealed class LivingMapRouteProjection
    {
        public string OriginNodeId { get; }
        public string DestinationNodeId { get; }
        public IReadOnlyList<string> NodeIds { get; }
        public float TotalDistanceKm { get; }
        public int TotalHops { get; }
        public bool HasFloodedEdges { get; }
        public bool HasAmphibiousEdges { get; }
        public IReadOnlyList<string> AllTags { get; }
        public bool IsValid { get; }

        public LivingMapRouteProjection(
            string originNodeId,
            string destinationNodeId,
            IEnumerable<string>? nodeIds,
            float totalDistanceKm,
            int totalHops,
            bool hasFloodedEdges,
            bool hasAmphibiousEdges,
            IEnumerable<string>? allTags,
            bool isValid = true)
        {
            OriginNodeId = originNodeId ?? string.Empty;
            DestinationNodeId = destinationNodeId ?? string.Empty;
            NodeIds = new List<string>(nodeIds ?? Array.Empty<string>()).AsReadOnly();
            TotalDistanceKm = Math.Max(0f, totalDistanceKm);
            TotalHops = Math.Max(0, totalHops);
            HasFloodedEdges = hasFloodedEdges;
            HasAmphibiousEdges = hasAmphibiousEdges;
            AllTags = new List<string>(allTags ?? Array.Empty<string>()).AsReadOnly();
            IsValid = isValid;
        }

        public static LivingMapRouteProjection Empty(string origin, string dest) =>
            new LivingMapRouteProjection(origin, dest, Array.Empty<string>(), 0f, 0, false, false, Array.Empty<string>(), false);
    }
}
