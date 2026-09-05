namespace Ashfall.Core.World
{
    /// <summary>
    /// Why a route is (un)available right now. Never serialized — derived
    /// from the gate evaluator and existing route restrictions at
    /// presentation time (F10 / B2).
    /// </summary>
    public enum RouteAvailabilityKind
    {
        /// <summary>Ordinary availability. No weather involvement.</summary>
        Available,

        /// <summary>Positive gate satisfied: the enabling weather is active.
        /// Distinct from ordinary availability (B3) — the player must be
        /// able to see WHY the route appeared.</summary>
        AvailableByWeatherOpportunity,

        /// <summary>Negative gate active: conditions make the route
        /// impassable (danger closure, B5).</summary>
        BlockedByWeatherDanger,

        /// <summary>Positive gate defined but its required weather has not
        /// occurred yet (B4) — the enabling condition is absent, not
        /// hostile.</summary>
        UnavailableRequiredWeather,

        /// <summary>Blocked by a non-weather restriction (story lock,
        /// undiscovered, vehicle restriction, hard block) — B10.</summary>
        BlockedOther
    }
}
