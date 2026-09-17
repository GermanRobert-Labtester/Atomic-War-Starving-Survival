// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Linq;
using Ashfall.Core.IO;

namespace Ashfall.Core.World
{
    /// <summary>
    /// One authored row of the C2 / Plan 20A weather-effects authority
    /// (weather_effects.json). Field names are snake_case per data policy.
    /// 20A consumes only <c>outdoor_rad_modifier</c>; 20C extends this record
    /// with travel/trap/caravan/thermal/visibility fields behind the same
    /// schema, so 20A does not create a shape 20C must discard (plan §11.3).
    /// </summary>
    public sealed class WeatherEffectsDef
    {
        public string weather { get; set; } = string.Empty;
        public float outdoor_rad_modifier { get; set; }

        /// <summary>Line-of-sight fraction 0..1 (forecast/map display, foraging).</summary>
        public float visibility_modifier { get; set; } = 1.0f;

        /// <summary>Shelter thermal load delta in °C (Plan 20C §42 — consumed
        /// through WeatherSystem's temperature penalty, never a second drain).</summary>
        public float thermal_load_additive_c { get; set; }

        /// <summary>Expedition travel speed × (1 = unaffected).</summary>
        public float travel_speed_multiplier { get; set; } = 1.0f;

        /// <summary>Expedition encounter chance × (1 = unaffected).</summary>
        public float travel_encounter_multiplier { get; set; } = 1.0f;

        /// <summary>Trapping/foraging yield × (1 = unaffected).</summary>
        public float trap_yield_multiplier { get; set; } = 1.0f;

        /// <summary>Caravan availability weight × (1 = unaffected).</summary>
        public float caravan_availability_multiplier { get; set; } = 1.0f;

        /// <summary>Plan 20C §44 — explicit mechanical-neutrality classification:
        /// a row with all-identity effects must declare itself neutral rather
        /// than silently defaulting; the coverage gate enforces the pairing.</summary>
        public bool explicitly_neutral { get; set; }
    }

    /// <summary>
    /// Data authority for per-weather-kind mechanical effects. Every
    /// <see cref="WeatherKind"/> value must have an explicit entry — a new enum
    /// value without a data row is a load error (no silent identity defaults,
    /// plan §35.1). Consumers (WeatherSystem.OutdoorRadModifier and the
    /// forecast projection) share this one table so a weather multiplier can
    /// never drift between forecast and runtime (plan §57.3).
    /// </summary>
    public sealed class WeatherEffectsCatalog
    {
        public const string DefaultFileName = "weather_effects.json";

        private readonly List<WeatherEffectsDef> _rows;
        private readonly Dictionary<WeatherKind, float> _modifiers;
        private readonly Dictionary<WeatherKind, WeatherEffectsDef>? _rowsByKind;
        private readonly List<string> _errors;

        public WeatherEffectsCatalog(IEnumerable<WeatherEffectsDef>? rows)
        {
            _rows = rows?.Where(r => r != null).ToList() ?? new List<WeatherEffectsDef>();
            _modifiers = new Dictionary<WeatherKind, float>();
            _errors = new List<string>();
            _rowsByKind = new Dictionary<WeatherKind, WeatherEffectsDef>();

            foreach (var row in _rows)
            {
                if (string.IsNullOrWhiteSpace(row.weather))
                {
                    _errors.Add("weather_effects row with empty 'weather' id");
                    continue;
                }

                if (!Enum.TryParse<WeatherKind>(row.weather, ignoreCase: true, out var kind)
                    || !Enum.IsDefined(typeof(WeatherKind), kind))
                {
                    _errors.Add($"weather_effects: unknown weather kind '{row.weather}'");
                    continue;
                }

                if (float.IsNaN(row.outdoor_rad_modifier) || float.IsInfinity(row.outdoor_rad_modifier))
                {
                    _errors.Add($"weather_effects[{row.weather}]: outdoor_rad_modifier must be finite");
                    continue;
                }

                if (row.outdoor_rad_modifier < 0f)
                {
                    _errors.Add($"weather_effects[{row.weather}]: outdoor_rad_modifier must be non-negative");
                    continue;
                }

                // C2 / Plan 20C (§35/§44) — validate the full effects record.
                if (!ValidateRange(row.visibility_modifier, 0f, 1f)
                    || !ValidateRange(row.travel_speed_multiplier, 0f, 5f)
                    || !ValidateRange(row.travel_encounter_multiplier, 0f, 5f)
                    || !ValidateRange(row.trap_yield_multiplier, 0f, 5f)
                    || !ValidateRange(row.caravan_availability_multiplier, 0f, 5f))
                {
                    _errors.Add($"weather_effects[{row.weather}]: effect multiplier out of range");
                    continue;
                }
                if (float.IsNaN(row.thermal_load_additive_c)
                    || MathF.Abs(row.thermal_load_additive_c) > 60f)
                {
                    _errors.Add($"weather_effects[{row.weather}]: thermal_load_additive_c out of range [-60,60]°C");
                    continue;
                }

                bool anyIdentity = row.outdoor_rad_modifier != 0f
                    || row.visibility_modifier != 1.0f
                    || row.thermal_load_additive_c != 0f
                    || row.travel_speed_multiplier != 1.0f
                    || row.travel_encounter_multiplier != 1.0f
                    || row.trap_yield_multiplier != 1.0f
                    || row.caravan_availability_multiplier != 1.0f;
                if (anyIdentity == row.explicitly_neutral)
                {
                    // Neutral must be all-identity; non-neutral must have ≥1 effect.
                    _errors.Add(row.explicitly_neutral
                        ? $"weather_effects[{row.weather}]: declared explicitly_neutral but has mechanical effects"
                        : $"weather_effects[{row.weather}]: all-identity row must declare explicitly_neutral (no silent defaults)");
                    continue;
                }

                if (_modifiers.ContainsKey(kind))
                {
                    _errors.Add($"weather_effects: duplicate row for weather kind '{row.weather}'");
                    continue;
                }

                _modifiers[kind] = row.outdoor_rad_modifier;
                if (_rowsByKind != null) _rowsByKind[kind] = row;
            }
        }

        public IReadOnlyList<string> Errors => _errors;
        public IReadOnlyList<WeatherEffectsDef> Rows => _rows;
        public int LoadedCount => _modifiers.Count;

        private static bool ValidateRange(float value, float min, float max)
            => !float.IsNaN(value) && !float.IsInfinity(value) && value >= min && value <= max;

        /// <summary>O(1) modifier lookup. True only for a validated, explicit row.</summary>
        public bool TryGetModifier(WeatherKind kind, out float modifier)
        {
            return _modifiers.TryGetValue(kind, out modifier);
        }

        /// <summary>O(1) full-row lookup (Plan 20C consumers: travel, trapping,
        /// caravan, thermal, forecast UI). True only for a validated row.</summary>
        public bool TryGetEffects(WeatherKind kind, out WeatherEffectsDef? effects)
        {
            effects = null;
            if (_rowsByKind == null) return false;
            return _rowsByKind.TryGetValue(kind, out effects);
        }

        /// <summary>
        /// Table-driven no-silent-defaults gate support: every WeatherKind value
        /// missing an explicit data row, in enum order. Empty means the catalog
        /// is complete; the integrity validator and tests fail loudly on any
        /// remainder (plan §35.1 / §44).
        /// </summary>
        public List<WeatherKind> MissingKinds()
        {
            var missing = new List<WeatherKind>();
            foreach (WeatherKind kind in Enum.GetValues(typeof(WeatherKind)))
                if (!_modifiers.ContainsKey(kind))
                    missing.Add(kind);
            return missing;
        }

        public static WeatherEffectsCatalog LoadFromDirectory(string dataDir, IFileIO fileIO)
        {
            if (fileIO == null || string.IsNullOrEmpty(dataDir))
                return new WeatherEffectsCatalog(null);
            string path = fileIO.Combine(dataDir, DefaultFileName);
            if (!fileIO.FileExists(path))
                return new WeatherEffectsCatalog(null);
            try
            {
                var rows = CatalogLocator.LoadWrappedList<WeatherEffectsDef>(
                    fileIO.ReadAllText(path), SystemTextJsonSerializer.Options);
                return new WeatherEffectsCatalog(rows);
            }
            catch (Exception ex)
            {
                CatalogDiagnostics.Warn("WeatherEffectsCatalog", path, ex);
                return new WeatherEffectsCatalog(null);
            }
        }
    }
}
