using System;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Strategic knowledge tier for travel route segments in the wasteland graph.
    /// </summary>
    public enum RouteKnowledgeTier
    {
        Unknown = 0, // Unexplored fog-of-war; completely invisible on travel graph
        Rumored = 1, // Vague destination known; hazard profile and distance unknown; travel refused
        Scouted = 2, // Basic trail blazed; hazards known; foot travel permitted, heavy vehicles refused
        Mapped = 3   // Surveyed corridor; full hazard profile known; all expeditions permitted
    }

    /// <summary>
    /// Immutable route access evaluation result produced by <see cref="TravelGraphKnowledgeGate"/>.
    /// </summary>
    public readonly struct RouteAccessResult
    {
        public bool IsAccessible { get; }
        public RouteKnowledgeTier RequiredKnowledge { get; }
        public RouteKnowledgeTier CurrentKnowledge { get; }
        public int EffectiveHazardScorePermille { get; }
        public string RefusalReason { get; }

        public RouteAccessResult(
            bool isAccessible,
            RouteKnowledgeTier requiredKnowledge,
            RouteKnowledgeTier currentKnowledge,
            int effectiveHazardScorePermille,
            string refusalReason)
        {
            IsAccessible = isAccessible;
            RequiredKnowledge = requiredKnowledge;
            CurrentKnowledge = currentKnowledge;
            EffectiveHazardScorePermille = Math.Max(0, Math.Min(1000, effectiveHazardScorePermille));
            RefusalReason = refusalReason ?? string.Empty;
        }
    }

    /// <summary>
    /// Pure domain engine for travel graph knowledge gating and route accessibility (Plan 32B/32C / D17 / UNBLOCK-04).
    /// Enforces route survey requirements for foot and convoy dispatch without duplicating graph pathfinding.
    /// </summary>
    public static class TravelGraphKnowledgeGate
    {
        public const int PermilleScale = 1000;

        /// <summary>
        /// Evaluates route accessibility based on strategic knowledge tier and expedition type.
        /// </summary>
        /// <param name="route">Travel route segment.</param>
        /// <param name="knowledge">Current player knowledge tier of the route.</param>
        /// <param name="isHeavyVehicleConvoy">True if expedition includes heavy vehicles.</param>
        /// <param name="hasAerialSurvey">True if aerial reconnaissance data has been mapped.</param>
        /// <returns>Immutable <see cref="RouteAccessResult"/>.</returns>
        public static RouteAccessResult EvaluateAccess(
            MapRoute route,
            RouteKnowledgeTier knowledge,
            bool isHeavyVehicleConvoy = false,
            bool hasAerialSurvey = false)
        {
            if (route == null)
            {
                return new RouteAccessResult(false, RouteKnowledgeTier.Mapped, RouteKnowledgeTier.Unknown, 1000, "Route does not exist.");
            }

            // Aerial survey elevates Rumored to Scouted, or Scouted to Mapped
            RouteKnowledgeTier effectiveKnowledge = knowledge;
            if (hasAerialSurvey)
            {
                if (effectiveKnowledge == RouteKnowledgeTier.Rumored)
                {
                    effectiveKnowledge = RouteKnowledgeTier.Scouted;
                }
                else if (effectiveKnowledge == RouteKnowledgeTier.Scouted)
                {
                    effectiveKnowledge = RouteKnowledgeTier.Mapped;
                }
            }

            // Unknown or Rumored routes cannot be traversed
            if (effectiveKnowledge == RouteKnowledgeTier.Unknown)
            {
                return new RouteAccessResult(
                    false,
                    RouteKnowledgeTier.Scouted,
                    effectiveKnowledge,
                    1000,
                    "Route lies entirely within unexplored territory.");
            }

            if (effectiveKnowledge == RouteKnowledgeTier.Rumored)
            {
                return new RouteAccessResult(
                    false,
                    RouteKnowledgeTier.Scouted,
                    effectiveKnowledge,
                    800,
                    "Route path is only rumored; exact corridor and hazards must be scouted prior to dispatch.");
            }

            // Heavy vehicle convoys require Mapped corridors
            if (isHeavyVehicleConvoy && effectiveKnowledge < RouteKnowledgeTier.Mapped)
            {
                return new RouteAccessResult(
                    false,
                    RouteKnowledgeTier.Mapped,
                    effectiveKnowledge,
                    600,
                    "Heavy vehicle convoy requires fully surveyed and mapped corridor to prevent immobilization.");
            }

            // Calculate effective hazard score:
            // Scouted routes have higher uncertainty hazard penalty (+150 permille)
            int baseHazard = (int)(route.WeatherHazard * 500); // 0..500
            if (route.IsFlooded) baseHazard += 250;
            if (effectiveKnowledge == RouteKnowledgeTier.Scouted)
            {
                baseHazard += 150; // uncertainty penalty
            }

            int finalHazard = Math.Max(50, Math.Min(PermilleScale, baseHazard));

            return new RouteAccessResult(
                isAccessible: true,
                requiredKnowledge: isHeavyVehicleConvoy ? RouteKnowledgeTier.Mapped : RouteKnowledgeTier.Scouted,
                currentKnowledge: effectiveKnowledge,
                effectiveHazardScorePermille: finalHazard,
                refusalReason: string.Empty
            );
        }
    }
}
