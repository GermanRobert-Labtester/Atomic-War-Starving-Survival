using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Gate polarity. Derived from the catalog — never serialized.
    /// Rule (section 6.2):
    ///   RequiredWeather.Count > 0      ⇒ PositiveRequiredWeather
    ///   else BlockedWeather.Count > 0  ⇒ NegativeBlockedDuring
    /// If both lists are populated the gate classifies as
    /// PositiveRequiredWeather for UI purposes, while evaluation still
    /// applies the documented precedence (blocked wins, fail-closed).
    /// </summary>
    public enum WeatherGatePolarity
    {
        NegativeBlockedDuring,
        PositiveRequiredWeather
    }

    /// <summary>
    /// State of one gate under one weather kind. Pure data — the same
    /// seed + catalog + weather must always produce the same state.
    /// </summary>
    public sealed class WeatherGateState
    {
        public string GateId { get; init; } = "";
        public string TargetId { get; init; } = "";
        public bool IsOpen { get; init; }
        public bool IsPositiveGate { get; init; }
        public WeatherGatePolarity Polarity { get; init; }
        public string Reason { get; init; } = "";
        public string Description { get; init; } = "";
    }

    /// <summary>
    /// Structured gate status attached to a forecast day (F9) or a route
    /// row (F10). Additive to the existing forecast entries — no existing
    /// field is replaced or renamed.
    /// </summary>
    public sealed class ForecastGateStatus
    {
        public string GateId { get; init; } = "";
        public string TargetId { get; init; } = "";
        public bool IsOpen { get; init; }
        public bool IsPositiveGate { get; init; }
        public string Reason { get; init; } = "";
        public string Description { get; init; } = "";

        public static ForecastGateStatus FromState(WeatherGateState state)
        {
            if (state == null) throw new ArgumentNullException(nameof(state));
            return new ForecastGateStatus
            {
                GateId = state.GateId,
                TargetId = state.TargetId,
                IsOpen = state.IsOpen,
                IsPositiveGate = state.IsPositiveGate,
                Reason = state.Reason,
                Description = state.Description
            };
        }

        /// <summary>
        /// Map a Plan 48 <see cref="WeatherGateDef"/> (snake_case DTO from
        /// weather_route_gates.json) onto the domain <see cref="WeatherGate"/>.
        /// Single conversion point — nothing else re-implements this.
        /// </summary>
        public static WeatherGate FromDef(WeatherGateDef def)
        {
            if (def == null) throw new ArgumentNullException(nameof(def));
            return new WeatherGate
            {
                Id = def.id ?? "",
                TargetId = def.target ?? "",
                BlockedWeather = def.blocked_weather?.ToList() ?? new List<string>(),
                RequiredWeather = def.required_weather?.ToList() ?? new List<string>(),
                OverrideItem = def.override_item ?? "",
                OverrideSkill = def.override_skill ?? "",
                ConsequenceOnForce = def.consequence_on_force ?? "",
                Description = def.description ?? "",
                ForceStaminaCost = (int)def.force_stamina_cost,
                ForceRadDose = (int)def.force_rad_dose,
                WarStateModifier = def.war_state_modifier,
                TerritoryModifier = def.territory_modifier,
                WeatherDelayDebt = def.weather_delay_debt,
                CompoundEventModifier = def.compound_event_modifier != null
                    ? new Dictionary<string, float>(def.compound_event_modifier, StringComparer.Ordinal)
                    : null
            };
        }
    }

    /// <summary>
    /// One gate transition between two weather states (F11). Pure data;
    /// ordering is the caller's concern (catalog order, then ordinal id).
    /// </summary>
    public sealed record WeatherGateTransition(
        string GateId,
        string TargetId,
        bool WasOpen,
        bool IsOpen,
        WeatherGatePolarity Polarity);

    /// <summary>
    /// A contiguous span of forecast days on which a positive gate stays
    /// open (F10 / A6). Deterministic: derived only from the forecast
    /// entries and the catalog.
    /// </summary>
    public sealed class ForecastGateWindow
    {
        public string GateId { get; init; } = "";
        public int StartDay { get; init; }
        public int EndDay { get; init; }
        public bool IsOpenWindow { get; init; }
        public WeatherKind TriggerWeather { get; init; }
    }

    /// <summary>
    /// The single weather-gate authority. Every consumer — forecast (F9),
    /// route UI (F10), radio hooks (F11), encounter eligibility (F12) —
    /// asks this evaluator; none duplicates gate semantics.
    /// </summary>
    public sealed class WeatherGateEvaluator
    {
        private readonly WeatherGateCatalog _catalog;

        public WeatherGateEvaluator(WeatherGateCatalog catalog)
        {
            _catalog = catalog ?? throw new ArgumentNullException(nameof(catalog));
        }

        public WeatherGateCatalog Catalog => _catalog;

        /// <summary>
        /// Map a Plan 48 <see cref="WeatherGateDef"/> onto the domain gate.
        /// Delegates to <see cref="ForecastGateStatus.FromDef"/> — the single
        /// conversion point; nothing else re-implements the mapping.
        /// </summary>
        public static WeatherGate FromDef(WeatherGateDef def) =>
            ForecastGateStatus.FromDef(def);

        // ── Weather-only evaluation (forecast, radio diff, topology) ──

        /// <summary>
        /// Evaluate one gate against one hypothetical weather kind.
        /// No inventory, no live world state, no player context — the
        /// forecast must never need a fake inventory to ask "would this
        /// gate be physically open?" (section 5.1 / 6.1).
        /// </summary>
        public WeatherGateState EvaluateWeatherOnly(string gateId, WeatherKind weather)
        {
            var gate = _catalog.TryGet(gateId, out var g) && g != null
                ? g
                : null;

            if (gate == null)
            {
                return new WeatherGateState
                {
                    GateId = gateId ?? "",
                    TargetId = "",
                    IsOpen = false,
                    IsPositiveGate = false,
                    Polarity = WeatherGatePolarity.NegativeBlockedDuring,
                    Reason = "unknown_gate",
                    Description = $"Gate '{gateId}' is not in the catalog."
                };
            }

            return EvaluateGate(gate, weather);
        }

        public WeatherGateState EvaluateLive(string gateId, WeatherKind weather, IEnumerable<string>? inventoryItemIds = null)
        {
            return EvaluateWeatherOnly(gateId, weather);
        }

        /// <summary>Classify a gate's polarity from its catalog fields alone.</summary>
        public static WeatherGatePolarity ClassifyPolarity(WeatherGate gate)
        {
            if (gate == null) throw new ArgumentNullException(nameof(gate));
            if (gate.RequiredWeather.Count > 0)
                return WeatherGatePolarity.PositiveRequiredWeather;
            if (gate.BlockedWeather.Count > 0)
                return WeatherGatePolarity.NegativeBlockedDuring;
            // Neither list populated: the gate is unconditional. Catalog
            // validation rejects this shape; classification still returns
            // deterministically rather than guessing.
            return WeatherGatePolarity.NegativeBlockedDuring;
        }

        /// <summary>
        /// Evaluate one gate under one weather kind — full semantics.
        /// Precedence when a gate declares both lists:
        ///   1. any blocked_weather match ⇒ gate is BLOCKED (fail-closed);
        ///   2. else required_weather present and matched ⇒ OPEN;
        ///   3. else required present, not matched ⇒ CLOSED (not yet available);
        ///   4. else (unconditional) ⇒ OPEN.
        /// </summary>
        public WeatherGateState EvaluateGate(WeatherGate gate, WeatherKind weather)
        {
            if (gate == null) throw new ArgumentNullException(nameof(gate));
            string kind = weather.ToString();

            // 1. fail-closed: blocked wins
            foreach (var blocked in gate.BlockedWeather)
            {
                if (string.Equals(blocked, kind, StringComparison.Ordinal))
                {
                    return BlockedState(gate, weather,
                        $"Blocked — {HumanKind(kind)} conditions make {HumanTarget(gate.TargetId)} impassable.");
                }
            }

            // 2. positive: required matched ⇒ open
            if (gate.RequiredWeather.Count > 0)
            {
                if (gate.RequiredWeather.Any(r => string.Equals(r, kind, StringComparison.Ordinal)))
                {
                    return OpenState(gate, weather, positive: true,
                        reason: "weather_opportunity",
                        description: PositiveOpenDescription(gate, kind));
                }

                return ClosedState(gate, weather,
                    PositiveUnavailableDescription(gate, kind));
            }

            // 3. unconditional gate with no lists ⇒ open
            return OpenState(gate, weather, positive: false,
                reason: "unconditional",
                description: string.IsNullOrEmpty(gate.Description)
                    ? $"{HumanTarget(gate.TargetId)} is passable."
                    : gate.Description);
        }

        /// <summary>
        /// Diff two weather states into the set of meaningful gate
        /// transitions (F11). Stable ordering: catalog order, then
        /// ordinal gate id. Pure — never mutates catalog or live state.
        /// </summary>
        public IReadOnlyList<WeatherGateTransition> CompareWeatherStates(
            WeatherKind previous, WeatherKind current)
        {
            var transitions = new List<WeatherGateTransition>();

            foreach (var gate in _catalog.GetAll())
            {
                var before = EvaluateGate(gate, previous);
                var after = EvaluateGate(gate, current);

                if (before.IsOpen != after.IsOpen)
                {
                    transitions.Add(new WeatherGateTransition(
                        gate.Id,
                        gate.TargetId,
                        before.IsOpen,
                        after.IsOpen,
                        after.IsPositiveGate
                            ? WeatherGatePolarity.PositiveRequiredWeather
                            : WeatherGatePolarity.NegativeBlockedDuring));
                }
            }

            // Stable ordering: catalog order (GetAll is already sorted),
            // then ordinal gate id within the same pass.
            return transitions
                .OrderBy(t => t.GateId, StringComparer.Ordinal)
                .ToList();
        }

        // ── State factories ──────────────────────────────────────────

        private static WeatherGateState OpenState(
            WeatherGate gate, WeatherKind weather, bool positive,
            string reason, string description)
        {
            return new WeatherGateState
            {
                GateId = gate.Id,
                TargetId = gate.TargetId,
                IsOpen = true,
                IsPositiveGate = positive,
                Polarity = positive
                    ? WeatherGatePolarity.PositiveRequiredWeather
                    : WeatherGatePolarity.NegativeBlockedDuring,
                Reason = reason,
                Description = description
            };
        }

        /// <summary>
        /// Static (non-instance) gate evaluation for callers that hold only
        /// the gate definition and a weather kind — the semantics authority
        /// for Plan 48 integration and radio/encounter simulation.
        /// </summary>
        public static WeatherGateState EvaluateGateStatic(WeatherGate gate, WeatherKind weather)
        {
            var evaluator = new WeatherGateEvaluator(new WeatherGateCatalog());
            return evaluator.EvaluateGate(gate, weather);
        }

        private static WeatherGateState BlockedState(WeatherGate gate, WeatherKind weather, string reason)
        {
            return new WeatherGateState
            {
                GateId = gate.Id,
                TargetId = gate.TargetId,
                IsOpen = false,
                IsPositiveGate = false,
                Polarity = WeatherGatePolarity.NegativeBlockedDuring,
                Reason = reason,
                Description = reason
            };
        }

        private static WeatherGateState ClosedState(WeatherGate gate, WeatherKind weather, string description)
        {
            return new WeatherGateState
            {
                GateId = gate.Id,
                TargetId = gate.TargetId,
                IsOpen = false,
                IsPositiveGate = true,
                Polarity = WeatherGatePolarity.PositiveRequiredWeather,
                Reason = "required_weather_not_matched",
                Description = description
            };
        }

        private static string HumanKind(string kind) =>
            kind switch
            {
                "Blizzard" => "blizzard",
                "BlackRain" => "black rain",
                "FalloutStorm" => "fallout storm",
                "BioFog" => "contaminated fog",
                "EMPStorm" => "EMP storm",
                "IceStorm" => "ice storm",
                _ => kind.ToLowerInvariant()
            };

        private static string HumanTarget(string targetId)
        {
            if (string.IsNullOrEmpty(targetId))
                return "the route";
            if (targetId.StartsWith("loc_", StringComparison.Ordinal))
                return "the destination";
            if (targetId.StartsWith("route_", StringComparison.Ordinal))
                return "the route";
            return "the target";
        }

        private static string PositiveOpenDescription(WeatherGate gate, string kind) =>
            $"Available — {HumanTarget(gate.TargetId)} open: " +
            $"sustained {HumanKind(kind)} has made it traversable.";

        private static string PositiveUnavailableDescription(WeatherGate gate, string kind) =>
            $"Requires sustained {HumanKind(kind)} before {HumanTarget(gate.TargetId)} can support travel.";
    }
}
