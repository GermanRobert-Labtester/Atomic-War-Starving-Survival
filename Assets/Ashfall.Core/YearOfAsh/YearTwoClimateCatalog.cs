// SPDX-License-Identifier: MIT
using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.YearOfAsh
{
    [Serializable]
    public class YearTwoClimatePhaseDef
    {
        [JsonPropertyName("phase_id")]
        public string phase_id { get; set; } = string.Empty;

        [JsonIgnore]
        public string id => phase_id;

        [JsonPropertyName("quarter")]
        public int quarter { get; set; }

        [JsonPropertyName("title")]
        public string title { get; set; } = string.Empty;

        [JsonPropertyName("start_day")]
        public int start_day { get; set; }

        [JsonPropertyName("end_day")]
        public int end_day { get; set; }

        [JsonPropertyName("curve_type")]
        public string curve_type { get; set; } = "linear";

        [JsonPropertyName("temp_start_c")]
        public float temp_start_c { get; set; }

        [JsonPropertyName("temp_end_c")]
        public float temp_end_c { get; set; }

        [JsonPropertyName("temp_min_c")]
        public float temp_min_c { get; set; }

        [JsonPropertyName("temp_max_c")]
        public float temp_max_c { get; set; }

        [JsonPropertyName("ash_opacity_start")]
        public float ash_opacity_start { get; set; }

        [JsonPropertyName("ash_opacity_end")]
        public float ash_opacity_end { get; set; }

        [JsonPropertyName("radon_start")]
        public float radon_start { get; set; }

        [JsonPropertyName("radon_end")]
        public float radon_end { get; set; }

        [JsonPropertyName("thermal_stress_start")]
        public float thermal_stress_start { get; set; }

        [JsonPropertyName("thermal_stress_end")]
        public float thermal_stress_end { get; set; }
    }

    [Serializable]
    public class YearTwoClimateData
    {
        [JsonPropertyName("schema_version")]
        public int schema_version { get; set; } = 1;

        [JsonPropertyName("title")]
        public string title { get; set; } = string.Empty;

        [JsonPropertyName("start_day")]
        public int start_day { get; set; } = 361;

        [JsonPropertyName("end_day")]
        public int end_day { get; set; } = 720;

        [JsonPropertyName("phases")]
        public List<YearTwoClimatePhaseDef> phases { get; set; } = new List<YearTwoClimatePhaseDef>();
    }

    public readonly struct YearTwoClimateCatalogValidationReport
    {
        public bool IsValid { get; }
        public IReadOnlyList<string> Errors { get; }

        public YearTwoClimateCatalogValidationReport(bool isValid, IReadOnlyList<string> errors)
        {
            IsValid = isValid;
            Errors = errors ?? Array.Empty<string>();
        }
    }

    public readonly struct ClimateSample
    {
        public float TemperatureCelsius { get; }
        public float AshCloudOpacity { get; }
        public float RadonInfiltrationRate { get; }
        public float ThermalStressLevel { get; }
        public string PhaseId { get; }
        public int Quarter { get; }

        public ClimateSample(
            float temp,
            float opacity,
            float radon,
            float thermalStress,
            string phaseId,
            int quarter)
        {
            TemperatureCelsius = temp;
            AshCloudOpacity = opacity;
            RadonInfiltrationRate = radon;
            ThermalStressLevel = thermalStress;
            PhaseId = phaseId ?? string.Empty;
            Quarter = quarter;
        }
    }

    /// <summary>
    /// Pure domain catalog for Year Two: The Long Thaw (Days 361–720) climate progression.
    /// Strictly validates phases, guarantees continuous day coverage, and computes
    /// deterministic daily environmental curves without engine dependencies.
    /// </summary>
    public sealed class YearTwoClimateCatalog
    {
        public const int DefaultStartDay = 361;
        public const int DefaultEndDay = 720;

        private readonly List<YearTwoClimatePhaseDef> _phases;
        private readonly List<string> _validationErrors = new List<string>();

        public IReadOnlyList<YearTwoClimatePhaseDef> Phases => _phases;
        public IReadOnlyList<string> ValidationErrors => _validationErrors;
        public bool IsValid => _validationErrors.Count == 0 && _phases.Count > 0;

        public YearTwoClimateCatalog(IEnumerable<YearTwoClimatePhaseDef>? phases = null)
        {
            _phases = phases != null ? new List<YearTwoClimatePhaseDef>(phases) : new List<YearTwoClimatePhaseDef>();
            Validate();
        }

        public static YearTwoClimateCatalog LoadFromJson(string json)
        {
            if (string.IsNullOrWhiteSpace(json))
            {
                var empty = new YearTwoClimateCatalog();
                empty._validationErrors.Add("YearTwoClimateCatalog JSON is empty or null.");
                return empty;
            }

            try
            {
                var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };
                var data = JsonSerializer.Deserialize<YearTwoClimateData>(json, options);
                if (data == null || data.phases == null)
                {
                    var invalid = new YearTwoClimateCatalog();
                    invalid._validationErrors.Add("Failed to deserialize YearTwoClimateData: root or phases null.");
                    return invalid;
                }

                return new YearTwoClimateCatalog(data.phases);
            }
            catch (Exception ex)
            {
                var failed = new YearTwoClimateCatalog();
                failed._validationErrors.Add($"Deserialization exception: {ex.Message}");
                return failed;
            }
        }

        public YearTwoClimateCatalogValidationReport Validate()
        {
            _validationErrors.Clear();

            if (_phases.Count == 0)
            {
                _validationErrors.Add("No climate phases defined.");
                return new YearTwoClimateCatalogValidationReport(IsValid, _validationErrors);
            }

            // Sort by start_day
            _phases.Sort((a, b) => a.start_day.CompareTo(b.start_day));

            if (_phases[0].start_day != DefaultStartDay)
            {
                _validationErrors.Add($"First phase start_day must be {DefaultStartDay}, found {_phases[0].start_day}.");
            }

            if (_phases[_phases.Count - 1].end_day != DefaultEndDay)
            {
                _validationErrors.Add($"Final phase end_day must be {DefaultEndDay}, found {_phases[_phases.Count - 1].end_day}.");
            }

            var quartersSeen = new HashSet<int>();
            var phaseIdsSeen = new HashSet<string>(StringComparer.OrdinalIgnoreCase);

            for (int i = 0; i < _phases.Count; i++)
            {
                var p = _phases[i];

                if (string.IsNullOrWhiteSpace(p.phase_id))
                {
                    _validationErrors.Add($"Phase at index {i} has empty phase_id.");
                }
                else if (!phaseIdsSeen.Add(p.phase_id))
                {
                    _validationErrors.Add($"Duplicate phase_id: '{p.phase_id}'.");
                }

                if (p.quarter < 1 || p.quarter > 4)
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' quarter must be in [1, 4], found {p.quarter}.");
                }
                else if (!quartersSeen.Add(p.quarter))
                {
                    _validationErrors.Add($"Duplicate quarter {p.quarter} in phase '{p.phase_id}'.");
                }

                if (p.start_day > p.end_day)
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' has start_day {p.start_day} > end_day {p.end_day}.");
                }

                if (i > 0)
                {
                    var prev = _phases[i - 1];
                    if (p.start_day <= prev.end_day)
                    {
                        _validationErrors.Add($"Phase '{p.phase_id}' overlaps with previous phase '{prev.phase_id}' (start: {p.start_day}, prev end: {prev.end_day}).");
                    }
                    else if (p.start_day > prev.end_day + 1)
                    {
                        _validationErrors.Add($"Gap detected between '{prev.phase_id}' (end: {prev.end_day}) and '{p.phase_id}' (start: {p.start_day}).");
                    }
                }

                // Opacity bounds [0, 1]
                if (p.ash_opacity_start < 0f || p.ash_opacity_start > 1f ||
                    p.ash_opacity_end < 0f || p.ash_opacity_end > 1f)
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' ash opacity out of bounds [0, 1].");
                }

                // Radon bounds [0, 1]
                if (p.radon_start < 0f || p.radon_start > 1f ||
                    p.radon_end < 0f || p.radon_end > 1f)
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' radon out of bounds [0, 1].");
                }

                // Thermal stress bounds [0, 1]
                if (p.thermal_stress_start < 0f || p.thermal_stress_start > 1f ||
                    p.thermal_stress_end < 0f || p.thermal_stress_end > 1f)
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' thermal stress out of bounds [0, 1].");
                }

                // Supported curves
                string curve = p.curve_type?.ToLowerInvariant() ?? "";
                if (curve != "linear" && curve != "late_snap" && curve != "winter_trough")
                {
                    _validationErrors.Add($"Phase '{p.phase_id}' has unsupported curve '{p.curve_type}'.");
                }
            }

            return new YearTwoClimateCatalogValidationReport(IsValid, _validationErrors);
        }

        public bool TryGetPhaseForDay(int day, out YearTwoClimatePhaseDef? phase)
        {
            phase = null;
            if (day < DefaultStartDay || day > DefaultEndDay) return false;

            for (int i = 0; i < _phases.Count; i++)
            {
                if (day >= _phases[i].start_day && day <= _phases[i].end_day)
                {
                    phase = _phases[i];
                    return true;
                }
            }
            return false;
        }

        public bool EvaluateDay(int day, out ClimateSample sample)
        {
            if (!TryGetPhaseForDay(day, out var phase) || phase == null)
            {
                sample = default;
                return false;
            }

            int span = Math.Max(1, phase.end_day - phase.start_day);
            float t = Math.Max(0f, Math.Min(1f, (day - phase.start_day) / (float)span));

            float temp;
            float ash;
            float radon;
            float thermal;

            switch (phase.curve_type.ToLowerInvariant())
            {
                case "late_snap":
                    // Q3 curve: warms up to max during first 60%, then late snap down sharply
                    if (t <= 0.6f)
                    {
                        float subT = t / 0.6f;
                        temp = phase.temp_start_c + (phase.temp_max_c - phase.temp_start_c) * (float)Math.Sin(subT * Math.PI / 2.0);
                        ash = phase.ash_opacity_start;
                        thermal = phase.thermal_stress_start;
                    }
                    else
                    {
                        float subT = (t - 0.6f) / 0.4f;
                        temp = phase.temp_max_c - (phase.temp_max_c - phase.temp_end_c) * subT;
                        ash = phase.ash_opacity_start + (phase.ash_opacity_end - phase.ash_opacity_start) * subT;
                        thermal = phase.thermal_stress_start + (phase.thermal_stress_end - phase.thermal_stress_start) * subT;
                    }
                    radon = phase.radon_start + (phase.radon_end - phase.radon_start) * t;
                    break;

                case "winter_trough":
                    // Q4 curve: smooth temperature descent into peak deep cold trough at t ~ 0.5, then slight easing
                    float troughT = (float)Math.Sin(t * Math.PI);
                    float baseLinear = phase.temp_start_c + (phase.temp_end_c - phase.temp_start_c) * t;
                    float dip = (phase.temp_min_c - Math.Min(phase.temp_start_c, phase.temp_end_c)) * troughT;
                    temp = baseLinear + dip;
                    ash = phase.ash_opacity_start + (phase.ash_opacity_end - phase.ash_opacity_start) * t;
                    radon = phase.radon_start + (phase.radon_end - phase.radon_start) * t;
                    thermal = phase.thermal_stress_start + (phase.thermal_stress_end - phase.thermal_stress_start) * t;
                    break;

                case "linear":
                default:
                    temp = phase.temp_start_c + (phase.temp_end_c - phase.temp_start_c) * t;
                    ash = phase.ash_opacity_start + (phase.ash_opacity_end - phase.ash_opacity_start) * t;
                    radon = phase.radon_start + (phase.radon_end - phase.radon_start) * t;
                    thermal = phase.thermal_stress_start + (phase.thermal_stress_end - phase.thermal_stress_start) * t;
                    break;
            }

            sample = new ClimateSample(
                temp,
                Math.Max(0f, Math.Min(1f, ash)),
                Math.Max(0f, Math.Min(1f, radon)),
                Math.Max(0f, Math.Min(1f, thermal)),
                phase.phase_id,
                phase.quarter);

            return true;
        }

        public ClimateSample EvaluateDay(int day)
        {
            EvaluateDay(day, out var sample);
            return sample;
        }
    }
}
