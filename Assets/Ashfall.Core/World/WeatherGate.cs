using System;
using System.Collections.Generic;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Territory control ladder for route corridor context (F18).
    /// </summary>
    public enum TerritoryControlState
    {
        Unclaimed = 0,
        Contested = 1,
        Controlled = 2
    }

    /// <summary>
    /// F17: Contextual war-state modifier on a weather gate.
    /// Active only during wartime conditions; never flips an open gate to blocked.
    /// </summary>
    [Serializable]
    public sealed class WarStateModifierDefinition
    {
        public bool enabled { get; set; } = true;
        public bool hostile_only { get; set; } = true;
        public int min_tension { get; set; } = 0;
        public float severity_multiplier { get; set; } = 1.0f;
        public string encounter_tag { get; set; } = string.Empty;
        public float encounter_weight_multiplier { get; set; } = 1.0f;
        public bool force_detour { get; set; } = false;
    }

    /// <summary>
    /// F18: Contextual modifier per territory control state.
    /// </summary>
    [Serializable]
    public sealed class TerritoryStateModifierDefinition
    {
        public float severity_multiplier { get; set; } = 1.0f;
        public bool shelter_available { get; set; } = false;
        public float forced_passage_modifier { get; set; } = 1.0f;
    }

    /// <summary>
    /// F18: Container for territory-state modifiers (controlled, contested, unclaimed).
    /// </summary>
    [Serializable]
    public sealed class TerritoryModifierDefinition
    {
        public TerritoryStateModifierDefinition? controlled { get; set; }
        public TerritoryStateModifierDefinition? contested { get; set; }
        public TerritoryStateModifierDefinition? unclaimed { get; set; }

        public TerritoryStateModifierDefinition? GetForState(TerritoryControlState state)
        {
            return state switch
            {
                TerritoryControlState.Controlled => controlled,
                TerritoryControlState.Contested => contested,
                TerritoryControlState.Unclaimed => unclaimed,
                _ => null
            };
        }
    }

    /// <summary>
    /// One weather-gate definition. The JSON authority is
    /// Assets/StreamingAssets/Data/weather_route_gates.json (schema_version 1).
    /// snake_case ids only — never invent an id outside the gate file.
    /// </summary>
    [Serializable]
    public sealed class WeatherGate
    {
        public string Id = string.Empty;
        public string GateType = "route";           // "route" | "destination"
        public string TargetId = string.Empty;      // route_XX_... or loc_XX_...
        public List<string> BlockedWeather = new List<string>();
        public List<string> RequiredWeather = new List<string>();
        public string OverrideItem = string.Empty;
        public string OverrideSkill = string.Empty;
        public string ConsequenceOnForce = string.Empty;
        public string Description = string.Empty;
        public int ForceStaminaCost = 0;
        public int ForceRadDose = 0;

        // F17–F20 cross-system contextual modifier overlays
        public WarStateModifierDefinition? WarStateModifier;
        public TerritoryModifierDefinition? TerritoryModifier;
        public bool WeatherDelayDebt;
        public Dictionary<string, float>? CompoundEventModifier;
    }
}
