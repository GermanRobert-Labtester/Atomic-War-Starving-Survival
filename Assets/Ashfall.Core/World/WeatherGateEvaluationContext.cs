using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// F17: Immutable snapshot of faction war state for contextual weather gate evaluation.
    /// Engine-agnostic; no concrete system references.
    /// </summary>
    public readonly record struct FactionWarSnapshot(
        bool IsAtWar,
        int ActiveWarTension,
        string DominantFactionId,
        bool IsDominantFactionHostile);

    /// <summary>
    /// F18: Immutable snapshot of territory control state for contextual weather gate evaluation.
    /// </summary>
    public readonly record struct TerritorySnapshot(
        TerritoryControlState State,
        string ControllerFactionId);

    /// <summary>
    /// F20: Immutable snapshot of active seasonal events for compounding hazard evaluation.
    /// </summary>
    public readonly record struct SeasonalEventSnapshot(
        IReadOnlySet<string>? ActiveEventIds);

    /// <summary>
    /// Complete input context passed into WeatherGateContextEvaluator (Section 3.1).
    /// Pure data; immutable.
    /// </summary>
    public sealed class WeatherGateEvaluationContext
    {
        public string TargetId { get; init; } = string.Empty;
        public WeatherKind CurrentWeather { get; init; } = WeatherKind.Clear;
        public IReadOnlyList<string> InventoryItems { get; init; } = Array.Empty<string>();

        public FactionWarSnapshot War { get; init; } = default;
        public TerritorySnapshot Territory { get; init; } = default;
        public SeasonalEventSnapshot Seasonal { get; init; } = default;

        public string RouteId { get; init; } = string.Empty;
        public string OriginHubId { get; init; } = string.Empty;
        public string DestinationHubId { get; init; } = string.Empty;
    }
}
