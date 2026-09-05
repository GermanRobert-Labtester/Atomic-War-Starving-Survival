using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// Dedicated loader for weather_route_gates.json.
    /// Provides load-time validation, schema version checking, error aggregation,
    /// and deterministic catalog construction without throwing exceptions on malformed data.
    /// </summary>
    public static class WeatherGateCatalogLoader
    {
        public const string DefaultFileName = "weather_route_gates.json";
        public const int CurrentSchemaVersion = 1;

        public static WeatherGateCatalog LoadFromJson(string json, IJsonSerializer? jsonSerializer = null)
        {
            var catalog = new WeatherGateCatalog();
            if (string.IsNullOrWhiteSpace(json))
            {
                catalog.Errors.Add("weather_route_gates.json: empty or missing JSON");
                return catalog;
            }

            WeatherGateFile.WeatherGateEnvelope? envelope = null;
            try
            {
                var serializer = jsonSerializer ?? new SystemTextJsonSerializer();
                envelope = serializer.Deserialize<WeatherGateFile.WeatherGateEnvelope>(json);
            }
            catch (Exception ex)
            {
                catalog.Errors.Add($"weather_route_gates.json: malformed JSON ({ex.GetType().Name}): {ex.Message}");
                return catalog;
            }

            if (envelope == null)
            {
                catalog.Errors.Add("weather_route_gates.json: parsed root envelope is null");
                return catalog;
            }

            if (envelope.schema_version > CurrentSchemaVersion)
            {
                catalog.Errors.Add($"weather_route_gates.json: schema_version {envelope.schema_version} is newer than supported version {CurrentSchemaVersion}");
                return catalog;
            }

            if (envelope.gates == null)
            {
                catalog.Errors.Add("weather_route_gates.json: missing gates collection");
                return catalog;
            }

            var knownKinds = Enum.GetNames(typeof(WeatherKind));
            var seenIds = new HashSet<string>(StringComparer.Ordinal);

            foreach (var def in envelope.gates)
            {
                if (def == null) continue;

                var gate = WeatherGateEvaluator.FromDef(def);

                // Normalization
                gate.Id ??= string.Empty;
                gate.TargetId ??= string.Empty;
                gate.GateType = string.IsNullOrWhiteSpace(def.gate_type) ? "route" : def.gate_type;
                gate.BlockedWeather ??= new List<string>();
                gate.RequiredWeather ??= new List<string>();
                gate.OverrideItem ??= string.Empty;
                gate.OverrideSkill ??= string.Empty;
                gate.ConsequenceOnForce ??= string.Empty;
                gate.Description ??= string.Empty;

                bool isDuplicate = false;

                // Rule A: ID non-empty and unique
                if (string.IsNullOrWhiteSpace(gate.Id))
                {
                    catalog.Errors.Add("gate id must be non-empty");
                }
                else if (seenIds.Contains(gate.Id))
                {
                    catalog.Errors.Add($"duplicate gate id '{gate.Id}'");
                    isDuplicate = true;
                }
                else
                {
                    seenIds.Add(gate.Id);
                }

                // Rule A2: GateType valid
                var validGateTypes = new[] { "route", "location", "region", "destination" };
                if (!string.IsNullOrWhiteSpace(gate.GateType) && Array.IndexOf(validGateTypes, gate.GateType.ToLowerInvariant()) < 0)
                {
                    catalog.Errors.Add($"gate '{gate.Id}' has invalid gate_type '{gate.GateType}'");
                }

                // Rule B: Target non-empty
                if (string.IsNullOrWhiteSpace(gate.TargetId))
                {
                    catalog.Errors.Add($"gate '{gate.Id}' target must be non-empty");
                }

                // Rule C: Weather IDs must be canonical
                foreach (var token in gate.BlockedWeather.Concat(gate.RequiredWeather).Distinct(StringComparer.Ordinal))
                {
                    if (Array.IndexOf(knownKinds, token) < 0)
                    {
                        catalog.Errors.Add($"gate '{gate.Id}' references unknown weather kind '{token}'");
                    }
                }

                // Duplicate weather within lists
                var dupBlocked = gate.BlockedWeather
                    .GroupBy(w => w, StringComparer.Ordinal)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                if (dupBlocked.Count > 0)
                {
                    catalog.Errors.Add($"gate '{gate.Id}' has duplicate blocked_weather entries: {string.Join(", ", dupBlocked)}");
                }

                var dupRequired = gate.RequiredWeather
                    .GroupBy(w => w, StringComparer.Ordinal)
                    .Where(g => g.Count() > 1)
                    .Select(g => g.Key)
                    .ToList();
                if (dupRequired.Count > 0)
                {
                    catalog.Errors.Add($"gate '{gate.Id}' has duplicate required_weather entries: {string.Join(", ", dupRequired)}");
                }

                // At least one weather definition
                if (gate.BlockedWeather.Count == 0 && gate.RequiredWeather.Count == 0)
                {
                    catalog.Errors.Add($"gate '{gate.Id}' must define at least one blocked or required weather kind");
                }

                if (!string.IsNullOrEmpty(gate.Id) && gate.Id.IndexOf("positive", StringComparison.OrdinalIgnoreCase) >= 0 && gate.RequiredWeather.Count == 0)
                {
                    catalog.Errors.Add($"positive gate '{gate.Id}' must define at least one required weather kind");
                }

                if (!string.IsNullOrEmpty(gate.Id) && gate.Id.IndexOf("negative", StringComparison.OrdinalIgnoreCase) >= 0 && gate.BlockedWeather.Count == 0)
                {
                    catalog.Errors.Add($"negative gate '{gate.Id}' must define at least one blocked weather kind");
                }

                // Rule D: Required/Blocked Intersection
                var contradiction = gate.BlockedWeather.Intersect(gate.RequiredWeather, StringComparer.Ordinal).ToList();
                if (contradiction.Count > 0)
                {
                    catalog.Errors.Add($"gate '{gate.Id}' contradiction: kinds both required and blocked ({string.Join(", ", contradiction)}). Evaluation precedence: blocked_weather wins (fail-closed).");
                }

                if (!isDuplicate)
                {
                    catalog.Add(gate);
                }
            }

            return catalog;
        }

        public static WeatherGateCatalog LoadFromDirectory(string directoryPath, IFileIO fileIO, IJsonSerializer? jsonSerializer = null)
        {
            if (string.IsNullOrEmpty(directoryPath) || fileIO == null)
            {
                var empty = new WeatherGateCatalog();
                empty.Errors.Add("weather_route_gates.json: directory path is null/empty or IFileIO is null");
                return empty;
            }

            string path = fileIO.Combine(directoryPath, DefaultFileName);
            if (!fileIO.FileExists(path))
            {
                var empty = new WeatherGateCatalog();
                empty.Errors.Add($"weather_route_gates.json: file does not exist at '{path}'");
                return empty;
            }

            string raw = fileIO.ReadAllText(path);
            return LoadFromJson(raw, jsonSerializer);
        }
    }
}
