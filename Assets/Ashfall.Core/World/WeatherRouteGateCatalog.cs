using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// One weather route/destination gate definition (Plan 48 data authority,
    /// `weather_route_gates.json`). A gate blocks a target (caravan route id
    /// or expedition destination id) while a listed weather is current, or
    /// when a required weather is NOT current, unless the override item is
    /// held. Gate state is derived — nothing here is saved.
    /// </summary>
    public sealed class WeatherGateDef
    {
        public string id { get; set; } = string.Empty;
        public string gate_type { get; set; } = "route";
        public string target { get; set; } = string.Empty;
        public List<string> blocked_weather { get; set; } = new List<string>();
        public List<string> required_weather { get; set; } = new List<string>();
        public string override_item { get; set; } = string.Empty;
        public string override_skill { get; set; } = string.Empty;
        public string consequence_on_force { get; set; } = string.Empty;
        /// <summary>GAP-48B — stamina the sortie starts short when the player
        /// forces through this gate. 0/absent = the gate cannot be forced.</summary>
        public float force_stamina_cost { get; set; } = 0f;
        /// <summary>GAP-48B — acute radiation dose (0–100 scale) applied to the
        /// dispatched survivor when forcing through. 0/absent = no dose.</summary>
        public float force_rad_dose { get; set; } = 0f;
        public string description { get; set; } = string.Empty;
    }

    /// <summary>Outcome of evaluating one gate against current weather.</summary>
    public sealed class WeatherGateBlock
    {
        public string GateId { get; set; } = string.Empty;
        public string Weather { get; set; } = string.Empty;
        public string Reason { get; set; } = string.Empty;
        /// <summary>Compact dispatch-bar label (the full Reason is the gate's
        /// description, too long for the row).</summary>
        public string ShortReason { get; set; } = string.Empty;
        /// <summary>GAP-48B — stamina the sortie starts short when forcing
        /// through. 0 = this gate cannot be forced.</summary>
        public float ForceStaminaCost { get; set; }
        /// <summary>GAP-48B — acute dose applied to the dispatched survivor on
        /// a forced entry (routed to the radiation system by the owner).</summary>
        public float ForceRadDose { get; set; }
        /// <summary>Player-facing consequence prose for the force action
        /// (consumes the gate's `consequence_on_force`).</summary>
        public string ForceConsequence { get; set; } = string.Empty;
    }

    /// <summary>
    /// Engine-agnostic catalog for weather gates (Plan 48). Pure evaluation:
    /// same gate + same weather + same inventory predicate always yields the
    /// same result. Gate state is derived from current weather — no save.
    /// </summary>
    public sealed class WeatherRouteGateCatalog
    {
        public const string DefaultFileName = "weather_route_gates.json";

        private readonly List<WeatherGateDef> _gates;
        private readonly Dictionary<string, List<WeatherGateDef>> _gatesByTarget;

        public WeatherRouteGateCatalog(IEnumerable<WeatherGateDef> gates)
        {
            _gates = gates?.Where(g => g != null && !string.IsNullOrEmpty(g.id) && !string.IsNullOrEmpty(g.target)).ToList()
                     ?? new List<WeatherGateDef>();
            _gatesByTarget = new Dictionary<string, List<WeatherGateDef>>(StringComparer.Ordinal);
            foreach (var gate in _gates)
            {
                if (!_gatesByTarget.TryGetValue(gate.target, out var list))
                    _gatesByTarget[gate.target] = list = new List<WeatherGateDef>();
                list.Add(gate);
            }
        }

        public IReadOnlyList<WeatherGateDef> Gates => _gates;

        public bool TryGetGatesForTarget(string targetId, out IReadOnlyList<WeatherGateDef> gates)
        {
            if (_gatesByTarget.TryGetValue(targetId ?? string.Empty, out var list))
            {
                gates = list;
                return true;
            }

            gates = Array.Empty<WeatherGateDef>();
            return false;
        }

        /// <summary>
        /// True when the gate blocks under <paramref name="current"/> weather.
        /// Semantics (Plan 48 authority): a non-empty `blocked_weather` list
        /// blocks while it contains the current weather; a non-empty
        /// `required_weather` list blocks while it does NOT contain it; the
        /// override item lifts the block when the predicate reports it held.
        /// </summary>
        public static bool IsGateBlocking(
            WeatherGateDef gate, string currentWeather, Func<string, bool> hasOverrideItem)
        {
            if (gate == null) return false;
            bool weatherBlocked = false;
            if (gate.blocked_weather != null && gate.blocked_weather.Count > 0)
            {
                weatherBlocked = Matches(gate.blocked_weather, currentWeather);
            }
            else if (gate.required_weather != null && gate.required_weather.Count > 0)
            {
                weatherBlocked = !Matches(gate.required_weather, currentWeather);
            }

            if (!weatherBlocked) return false;
            if (!string.IsNullOrEmpty(gate.override_item)
                && hasOverrideItem != null
                && hasOverrideItem(gate.override_item))
                return false;
            return true;
        }

        private static bool Matches(List<string> kinds, string current)
        {
            for (int i = 0; i < kinds.Count; i++)
            {
                if (string.Equals(kinds[i], current, StringComparison.OrdinalIgnoreCase)) return true;
            }

            return false;
        }

        /// <summary>
        /// First blocking gate for a target under the current weather, or null
        /// when the target is passable. The block carries the gate's
        /// description as the player-facing reason (Plan 48 integration path
        /// step 5).
        /// </summary>
        public WeatherGateBlock? EvaluateBlock(
            string targetId, string currentWeather, Func<string, bool> hasOverrideItem)
        {
            if (!_gatesByTarget.TryGetValue(targetId ?? string.Empty, out var gates))
                return null;

            for (int i = 0; i < gates.Count; i++)
            {
                var gate = gates[i];
                if (!IsGateBlocking(gate, currentWeather, hasOverrideItem)) continue;
                return new WeatherGateBlock
                {
                    GateId = gate.id,
                    Weather = currentWeather,
                    Reason = !string.IsNullOrEmpty(gate.description) ? gate.description : $"Weather gate active ({gate.id}).",
                    ShortReason = $"Weather gate — {currentWeather.ToLowerInvariant()}",
                    ForceStaminaCost = gate.force_stamina_cost,
                    ForceRadDose = gate.force_rad_dose,
                    ForceConsequence = gate.consequence_on_force
                };
            }

            return null;
        }

        public static WeatherRouteGateCatalog LoadFromDirectory(string dataDir, IFileIO fileIO)
        {
            if (fileIO == null || string.IsNullOrEmpty(dataDir)) return new WeatherRouteGateCatalog(Enumerable.Empty<WeatherGateDef>());
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path)) return new WeatherRouteGateCatalog(Enumerable.Empty<WeatherGateDef>());
            try
            {
                var parsed = CatalogLocator.LoadWrappedList<WeatherGateDef>(fileIO.ReadAllText(path), SystemTextJsonSerializer.Options);
                return new WeatherRouteGateCatalog(parsed);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("WeatherRouteGateCatalog", path, ex);
                return new WeatherRouteGateCatalog(Enumerable.Empty<WeatherGateDef>());
            }
        }
    }
}
