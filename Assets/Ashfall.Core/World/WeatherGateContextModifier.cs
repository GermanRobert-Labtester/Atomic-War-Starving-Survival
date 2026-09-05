using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Intermediate modifier emitted by an individual subsystem resolver (War, Territory, Seasonal).
    /// </summary>
    internal sealed class WeatherGateContextModifier
    {
        public float SeverityMultiplier { get; init; } = 1f;
        public bool ShelterAvailable { get; init; }
        public bool ForcedDetourSuggested { get; init; }
        public string EncounterTag { get; init; } = string.Empty;
        public float EncounterWeightMultiplier { get; init; } = 1f;
        public string Reason { get; init; } = string.Empty;
    }

    /// <summary>
    /// Enriched result of evaluating a weather gate with cross-system context (F17–F20).
    /// Immutable; deterministic; single source of truth for gate state and modifiers.
    /// </summary>
    public sealed class WeatherGateContextResult
    {
        public string GateId { get; init; } = string.Empty;
        public string TargetId { get; init; } = string.Empty;
        public bool IsBlocked { get; init; }
        public string BlockedReason { get; init; } = string.Empty;
        public bool OverrideAvailable { get; init; }

        public float ConsequenceSeverityMultiplier { get; init; } = 1f;
        public bool ShelterAvailable { get; init; }
        public bool ForcedDetourSuggested { get; init; }

        public string FactionEncounterTag { get; init; } = string.Empty;
        public float FactionEncounterWeightMultiplier { get; init; } = 1f;

        public bool WeatherDelayDebtEligible { get; init; }

        public IReadOnlyList<string> AppliedContextReasons { get; init; } = Array.Empty<string>();
        public string EvaluationTrace { get; init; } = string.Empty;
    }
}
