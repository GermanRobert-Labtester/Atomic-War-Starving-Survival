using System;
using System.Collections.Generic;
using System.Linq;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Registry of weather gates. One authority for gate semantics —
    /// forecast (F9), route UI (F10), radio (F11) and encounters (F12)
    /// all consume this catalog; none re-implements gate rules.
    ///
    /// Validation (Gate Q2 / section 16):
    ///   - non-empty gate id, unique gate id
    ///   - valid target id
    ///   - every weather token must be a known WeatherKind
    ///   - no duplicate weather token within one gate
    ///   - a positive gate (required_weather) must list at least one kind
    ///   - a negative gate (blocked_weather) must list at least one kind
    ///   - required ∩ blocked ≠ ∅ is a catalog error (contradiction);
    ///     at evaluation time blocked_weather wins (fail-closed) and the
    ///     contradiction is reported, never silently resolved.
    /// </summary>
    public sealed class WeatherGateCatalog
    {
        private readonly Dictionary<string, WeatherGate> _gates =
            new Dictionary<string, WeatherGate>(StringComparer.Ordinal);
        private readonly Dictionary<string, WeatherGate> _gatesByTarget =
            new Dictionary<string, WeatherGate>(StringComparer.Ordinal);
        private readonly List<WeatherGate> _orderedGates = new List<WeatherGate>();

        public int Count => _gates.Count;
        public List<string> Errors { get; } = new List<string>();
        public bool IsValid => Errors.Count == 0;

        public void Register(WeatherGate gate)
        {
            if (gate == null)
                throw new ArgumentNullException(nameof(gate));
            Validate(gate);
            if (_gates.ContainsKey(gate.Id))
                throw new WeatherGateCatalogException($"duplicate gate id '{gate.Id}'");
            _gates[gate.Id] = gate;
            _orderedGates.Add(gate);
            if (!string.IsNullOrEmpty(gate.TargetId) && !_gatesByTarget.ContainsKey(gate.TargetId))
                _gatesByTarget[gate.TargetId] = gate;
        }

        public void Add(WeatherGate gate)
        {
            if (gate == null) return;
            if (!_gates.ContainsKey(gate.Id))
            {
                _gates[gate.Id] = gate;
                _orderedGates.Add(gate);
                if (!string.IsNullOrEmpty(gate.TargetId) && !_gatesByTarget.ContainsKey(gate.TargetId))
                    _gatesByTarget[gate.TargetId] = gate;
            }
        }

        public bool TryGet(string gateId, out WeatherGate? gate)
        {
            gate = null;
            if (string.IsNullOrEmpty(gateId))
                return false;
            return _gates.TryGetValue(gateId, out gate) && gate != null;
        }

        public WeatherGate? GetById(string id) => TryGet(id, out var g) ? g : null;

        public WeatherGate? FindByTargetId(string targetId)
        {
            if (string.IsNullOrEmpty(targetId)) return null;
            if (_gatesByTarget.TryGetValue(targetId, out var g)) return g;
            return _gates.Values.FirstOrDefault(gate => string.Equals(gate.TargetId, targetId, StringComparison.Ordinal));
        }

        public WeatherGate? GetByTarget(string target) => FindByTargetId(target);

        /// <summary>Registration order first, then ordinal id sort. Stable.</summary>
        public IReadOnlyList<WeatherGate> GetAll()
        {
            return _gates.Values
                .OrderBy(g => g.Id, StringComparer.Ordinal)
                .ToList();
        }

        public IEnumerable<string> AllGateIds => _gates.Keys.OrderBy(k => k, StringComparer.Ordinal);

        public static WeatherGateCatalog LoadFromJson(string json, Ashfall.Core.IJsonSerializer? jsonSerializer = null) =>
            WeatherGateCatalogLoader.LoadFromJson(json, jsonSerializer);

        public static WeatherGateCatalog LoadFromDirectory(string dataDir, Ashfall.Core.IFileIO fileIO, Ashfall.Core.IJsonSerializer? jsonSerializer = null) =>
            WeatherGateCatalogLoader.LoadFromDirectory(dataDir, fileIO, jsonSerializer);

        // ── Validation ────────────────────────────────────────────────

        private static void Validate(WeatherGate gate)
        {
            var errors = new List<string>();

            if (string.IsNullOrWhiteSpace(gate.Id))
                errors.Add("gate id must be non-empty");
            if (string.IsNullOrWhiteSpace(gate.TargetId))
                errors.Add($"gate '{gate.Id}' target must be non-empty");

            var knownKinds = Enum.GetNames(typeof(WeatherKind));
            foreach (var token in gate.BlockedWeather.Concat(gate.RequiredWeather).Distinct())
            {
                if (Array.IndexOf(knownKinds, token) < 0)
                    errors.Add($"gate '{gate.Id}' references unknown weather kind '{token}'");
            }

            var dupBlocked = gate.BlockedWeather
                .GroupBy(w => w, StringComparer.Ordinal)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (dupBlocked.Count > 0)
                errors.Add($"gate '{gate.Id}' has duplicate blocked_weather entries: {string.Join(", ", dupBlocked)}");

            var dupRequired = gate.RequiredWeather
                .GroupBy(w => w, StringComparer.Ordinal)
                .Where(g => g.Count() > 1)
                .Select(g => g.Key)
                .ToList();
            if (dupRequired.Count > 0)
                errors.Add($"gate '{gate.Id}' has duplicate required_weather entries: {string.Join(", ", dupRequired)}");

            if (gate.BlockedWeather.Count == 0 && gate.RequiredWeather.Count == 0)
                errors.Add($"gate '{gate.Id}' must define at least one blocked or required weather kind");

            if (!string.IsNullOrEmpty(gate.Id) && gate.Id.IndexOf("positive", StringComparison.OrdinalIgnoreCase) >= 0 && gate.RequiredWeather.Count == 0)
                errors.Add($"positive gate '{gate.Id}' must define at least one required weather kind");

            if (!string.IsNullOrEmpty(gate.Id) && gate.Id.IndexOf("negative", StringComparison.OrdinalIgnoreCase) >= 0 && gate.BlockedWeather.Count == 0)
                errors.Add($"negative gate '{gate.Id}' must define at least one blocked weather kind");

            var contradiction = gate.BlockedWeather.Intersect(gate.RequiredWeather, StringComparer.Ordinal).ToList();
            if (contradiction.Count > 0)
                errors.Add($"gate '{gate.Id}' contradiction: kinds both required and blocked ({string.Join(", ", contradiction)}). Evaluation precedence: blocked_weather wins (fail-closed).");

            if (gate.WarStateModifier != null)
            {
                var war = gate.WarStateModifier;
                if (war.min_tension < 0 || war.min_tension > 100)
                    errors.Add($"gate '{gate.Id}' war modifier min_tension must be in 0..100 (got {war.min_tension})");
                if (war.severity_multiplier < 0f)
                    errors.Add($"gate '{gate.Id}' war modifier severity_multiplier must be >= 0 (got {war.severity_multiplier})");
                if (war.encounter_weight_multiplier < 0f)
                    errors.Add($"gate '{gate.Id}' war modifier encounter_weight_multiplier must be >= 0 (got {war.encounter_weight_multiplier})");
                if (Math.Abs(war.encounter_weight_multiplier - 1.0f) > 0.001f && string.IsNullOrWhiteSpace(war.encounter_tag))
                    errors.Add($"gate '{gate.Id}' war modifier must specify non-empty encounter_tag when encounter_weight_multiplier differs from 1.0");
            }

            if (gate.TerritoryModifier != null)
            {
                var tm = gate.TerritoryModifier;
                void ValidateStateMod(string stateName, TerritoryStateModifierDefinition? sm)
                {
                    if (sm == null) return;
                    if (sm.severity_multiplier < 0f)
                        errors.Add($"gate '{gate.Id}' territory modifier {stateName} severity_multiplier must be >= 0 (got {sm.severity_multiplier})");
                    if (sm.forced_passage_modifier < 0f)
                        errors.Add($"gate '{gate.Id}' territory modifier {stateName} forced_passage_modifier must be >= 0 (got {sm.forced_passage_modifier})");
                }
                ValidateStateMod("controlled", tm.controlled);
                ValidateStateMod("contested", tm.contested);
                ValidateStateMod("unclaimed", tm.unclaimed);
            }

            if (gate.CompoundEventModifier != null)
            {
                foreach (var kvp in gate.CompoundEventModifier)
                {
                    if (string.IsNullOrWhiteSpace(kvp.Key))
                        errors.Add($"gate '{gate.Id}' compound_event_modifier has empty event ID");
                    if (kvp.Value < 1.0f)
                        errors.Add($"gate '{gate.Id}' compound_event_modifier for '{kvp.Key}' must be >= 1.0 (got {kvp.Value})");
                }
            }

            if (errors.Count > 0)
                throw new WeatherGateCatalogException(string.Join("; ", errors));
        }
    }

    /// <summary>Raised when the gate catalog fails validation at load time.</summary>
    public sealed class WeatherGateCatalogException : Exception
    {
        public WeatherGateCatalogException(string message) : base(message) { }
    }
}
