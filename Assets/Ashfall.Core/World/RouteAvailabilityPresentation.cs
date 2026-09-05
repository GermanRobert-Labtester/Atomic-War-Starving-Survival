namespace Ashfall.Core.World
{
    /// <summary>
    /// Presentation model for one route's availability (F10 / B2).
    /// Produced from the shared gate evaluator + existing route
    /// restrictions. Text and reason carry the state; colour never does
    /// (B6). GateId references the catalog gate that produced the state.
    /// </summary>
    public sealed class RouteAvailabilityPresentation
    {
        public RouteAvailabilityKind Kind { get; init; }

        /// <summary>Primary status line, e.g. "Available — frozen crossing open."</summary>
        public string PrimaryText { get; init; } = "";

        /// <summary>Player-facing reason for the state (B4/B8): the physical
        /// mechanism or the missing condition — never gate jargon.</summary>
        public string Reason { get; init; } = "";

        /// <summary>Catalog gate that produced this state, when known.</summary>
        public string? GateId { get; init; }
    }
}
