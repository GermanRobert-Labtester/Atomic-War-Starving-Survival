using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class SlowSandSchmutzdeckeEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("filter_basin_id")]
        public string FilterBasinId { get; set; } = string.Empty;

        [JsonPropertyName("filtration_rate_meters_per_hour")]
        public float FiltrationRateMetersPerHour { get; set; }

        [JsonPropertyName("influent_turbidity_ntu")]
        public float InfluentTurbidityNtu { get; set; }

        [JsonPropertyName("effluent_turbidity_ntu")]
        public float EffluentTurbidityNtu { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class OzoneContactTowerEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("ozonator_unit_id")]
        public string OzonatorUnitId { get; set; } = string.Empty;

        [JsonPropertyName("applied_voltage_kilovolts")]
        public float AppliedVoltageKilovolts { get; set; }

        [JsonPropertyName("ozone_output_grams_per_hour")]
        public float OzoneOutputGramsPerHour { get; set; }

        [JsonPropertyName("contact_time_minutes")]
        public float ContactTimeMinutes { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CalciumHypochloriteTitrationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("dosing_station_id")]
        public string DosingStationId { get; set; } = string.Empty;

        [JsonPropertyName("hypochlorite_reagent_grade")]
        public string HypochloriteReagentGrade { get; set; } = string.Empty;

        [JsonPropertyName("free_chlorine_residual_mg_l")]
        public float FreeChlorineResidualMgL { get; set; }

        [JsonPropertyName("water_ph_at_sampling")]
        public float WaterPhAtSampling { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class ActivatedCarbonAdsorptionEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("carbon_filter_vessel_id")]
        public string CarbonFilterVesselId { get; set; } = string.Empty;

        [JsonPropertyName("carbon_base_feedstock")]
        public string CarbonBaseFeedstock { get; set; } = string.Empty;

        [JsonPropertyName("iodine_number_mg_g")]
        public float IodineNumberMgG { get; set; }

        [JsonPropertyName("bed_empty_bed_contact_time_min")]
        public float BedEmptyBedContactTimeMin { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class WaterTreatmentPotableCatalog
    {
        private readonly List<SlowSandSchmutzdeckeEntry> _sandEntries = new List<SlowSandSchmutzdeckeEntry>();
        private readonly List<OzoneContactTowerEntry> _ozoneEntries = new List<OzoneContactTowerEntry>();
        private readonly List<CalciumHypochloriteTitrationEntry> _chlorineEntries = new List<CalciumHypochloriteTitrationEntry>();
        private readonly List<ActivatedCarbonAdsorptionEntry> _carbonEntries = new List<ActivatedCarbonAdsorptionEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<SlowSandSchmutzdeckeEntry> SandEntries => _sandEntries;
        public IReadOnlyList<OzoneContactTowerEntry> OzoneEntries => _ozoneEntries;
        public IReadOnlyList<CalciumHypochloriteTitrationEntry> ChlorineEntries => _chlorineEntries;
        public IReadOnlyList<ActivatedCarbonAdsorptionEntry> CarbonEntries => _carbonEntries;

        public int TotalCount => _sandEntries.Count + _ozoneEntries.Count + _chlorineEntries.Count + _carbonEntries.Count;

        public static WaterTreatmentPotableCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new WaterTreatmentPotableCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Slow Sand Filter Schmutzdecke Ripening Logs
            string sandPath = Path.Combine(directoryPath, "slow_sand_schmutzdecke_logs.json");
            if (File.Exists(sandPath))
            {
                var list = JsonSerializer.Deserialize<List<SlowSandSchmutzdeckeEntry>>(File.ReadAllText(sandPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._sandEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Corona Discharge Ozonation Contact Tower Audits
            string ozPath = Path.Combine(directoryPath, "ozone_contact_tower_audits.json");
            if (File.Exists(ozPath))
            {
                var list = JsonSerializer.Deserialize<List<OzoneContactTowerEntry>>(File.ReadAllText(ozPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._ozoneEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Calcium Hypochlorite Residual Chlorine Titrations
            string clPath = Path.Combine(directoryPath, "calcium_hypochlorite_titration_reports.json");
            if (File.Exists(clPath))
            {
                var list = JsonSerializer.Deserialize<List<CalciumHypochloriteTitrationEntry>>(File.ReadAllText(clPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._chlorineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Activated Carbon Filter VOC Adsorption Records
            string cPath = Path.Combine(directoryPath, "activated_carbon_adsorption_records.json");
            if (File.Exists(cPath))
            {
                var list = JsonSerializer.Deserialize<List<ActivatedCarbonAdsorptionEntry>>(File.ReadAllText(cPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._carbonEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public SlowSandSchmutzdeckeEntry GetSand(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SlowSandSchmutzdeckeEntry e ? e : null;
        }

        public OzoneContactTowerEntry GetOzone(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is OzoneContactTowerEntry e ? e : null;
        }

        public CalciumHypochloriteTitrationEntry GetChlorine(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CalciumHypochloriteTitrationEntry e ? e : null;
        }

        public ActivatedCarbonAdsorptionEntry GetCarbon(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is ActivatedCarbonAdsorptionEntry e ? e : null;
        }
    }
}
