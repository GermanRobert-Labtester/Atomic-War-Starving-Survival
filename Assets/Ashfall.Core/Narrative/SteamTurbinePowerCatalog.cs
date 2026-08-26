using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class GeothermalSteamWellEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("wellhead_designation")]
        public string WellheadDesignation { get; set; } = string.Empty;

        [JsonPropertyName("reservoir_enthalpy_kj_kg")]
        public float ReservoirEnthalpyKjKg { get; set; }

        [JsonPropertyName("wellhead_pressure_bar")]
        public float WellheadPressureBar { get; set; }

        [JsonPropertyName("silica_deposition_rate_mm_month")]
        public float SilicaDepositionRateMmMonth { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class TurbineBladeErosionEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("turbine_unit_identifier")]
        public string TurbineUnitIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("rotor_speed_rpm")]
        public float RotorSpeedRpm { get; set; }

        [JsonPropertyName("steam_wetness_fraction_pct")]
        public float SteamWetnessFractionPct { get; set; }

        [JsonPropertyName("blade_stage_number")]
        public int BladeStageNumber { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class BoilerFeedwaterDeaeratorEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("boiler_plant_id")]
        public string BoilerPlantId { get; set; } = string.Empty;

        [JsonPropertyName("dissolved_oxygen_ppb")]
        public float DissolvedOxygenPpb { get; set; }

        [JsonPropertyName("feedwater_ph")]
        public float FeedwaterPh { get; set; }

        [JsonPropertyName("sludge_blowdown_interval_hours")]
        public float SludgeBlowdownIntervalHours { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SteamTrapWaterHammerEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("steam_distribution_bay_id")]
        public string SteamDistributionBayId { get; set; } = string.Empty;

        [JsonPropertyName("trap_mechanism_type")]
        public string TrapMechanismType { get; set; } = string.Empty;

        [JsonPropertyName("condensate_load_kg_hr")]
        public float CondensateLoadKgHr { get; set; }

        [JsonPropertyName("system_line_pressure_bar")]
        public float SystemLinePressureBar { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SteamTurbinePowerCatalog
    {
        private readonly List<GeothermalSteamWellEntry> _wellEntries = new List<GeothermalSteamWellEntry>();
        private readonly List<TurbineBladeErosionEntry> _turbineEntries = new List<TurbineBladeErosionEntry>();
        private readonly List<BoilerFeedwaterDeaeratorEntry> _boilerEntries = new List<BoilerFeedwaterDeaeratorEntry>();
        private readonly List<SteamTrapWaterHammerEntry> _trapEntries = new List<SteamTrapWaterHammerEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<GeothermalSteamWellEntry> WellEntries => _wellEntries;
        public IReadOnlyList<TurbineBladeErosionEntry> TurbineEntries => _turbineEntries;
        public IReadOnlyList<BoilerFeedwaterDeaeratorEntry> BoilerEntries => _boilerEntries;
        public IReadOnlyList<SteamTrapWaterHammerEntry> TrapEntries => _trapEntries;

        public int TotalCount => _wellEntries.Count + _turbineEntries.Count + _boilerEntries.Count + _trapEntries.Count;

        public static SteamTurbinePowerCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new SteamTurbinePowerCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Geothermal Steam Wellhead Logs
            string wellPath = Path.Combine(directoryPath, "geothermal_steam_well_logs.json");
            if (File.Exists(wellPath))
            {
                var list = CatalogLocator.LoadWrappedList<GeothermalSteamWellEntry>(File.ReadAllText(wellPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._wellEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 2. Steam Turbine Blade Droplet Erosion & Governor Records
            string turbPath = Path.Combine(directoryPath, "turbine_blade_erosion_reports.json");
            if (File.Exists(turbPath))
            {
                var list = CatalogLocator.LoadWrappedList<TurbineBladeErosionEntry>(File.ReadAllText(turbPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._turbineEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Boiler Feedwater Deaeration & Sludge Blowdown Audits
            string boilPath = Path.Combine(directoryPath, "boiler_feedwater_deaerator_audits.json");
            if (File.Exists(boilPath))
            {
                var list = CatalogLocator.LoadWrappedList<BoilerFeedwaterDeaeratorEntry>(File.ReadAllText(boilPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._boilerEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Steam Trap Condensation & Expansion Loop Logs
            string trapPath = Path.Combine(directoryPath, "steam_trap_water_hammer_logs.json");
            if (File.Exists(trapPath))
            {
                var list = CatalogLocator.LoadWrappedList<SteamTrapWaterHammerEntry>(File.ReadAllText(trapPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._trapEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public GeothermalSteamWellEntry? GetWell(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is GeothermalSteamWellEntry e ? e : null;
        }

        public TurbineBladeErosionEntry? GetTurbine(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is TurbineBladeErosionEntry e ? e : null;
        }

        public BoilerFeedwaterDeaeratorEntry? GetBoiler(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is BoilerFeedwaterDeaeratorEntry e ? e : null;
        }

        public SteamTrapWaterHammerEntry? GetTrap(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SteamTrapWaterHammerEntry e ? e : null;
        }
    }
}
