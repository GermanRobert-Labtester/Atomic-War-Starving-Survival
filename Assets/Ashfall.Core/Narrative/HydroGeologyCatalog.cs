using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Ashfall.Core.Narrative
{
    public sealed class WellContaminationEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("well_identifier")]
        public string WellIdentifier { get; set; } = string.Empty;

        [JsonPropertyName("aquifer_stratum")]
        public string AquiferStratum { get; set; } = string.Empty;

        [JsonPropertyName("contaminant_agent")]
        public string ContaminantAgent { get; set; } = string.Empty;

        [JsonPropertyName("activity_level_bq_l")]
        public float ActivityLevelBqL { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class CaveBiotaEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("species_designation")]
        public string SpeciesDesignation { get; set; } = string.Empty;

        [JsonPropertyName("cavern_location")]
        public string CavernLocation { get; set; } = string.Empty;

        [JsonPropertyName("bioluminescence_type")]
        public string BioluminescenceType { get; set; } = string.Empty;

        [JsonPropertyName("ecological_niche")]
        public string EcologicalNiche { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class SteamVentDiagnosticEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("vent_manifold_id")]
        public string VentManifoldId { get; set; } = string.Empty;

        [JsonPropertyName("steam_temperature_celsius")]
        public float SteamTemperatureCelsius { get; set; }

        [JsonPropertyName("line_pressure_bar")]
        public float LinePressureBar { get; set; }

        [JsonPropertyName("failure_diagnostic")]
        public string FailureDiagnostic { get; set; } = string.Empty;

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class StalactiteMineralAssayEntry
    {
        [JsonPropertyName("id")]
        public string Id { get; set; } = string.Empty;

        [JsonPropertyName("sample_specimen_id")]
        public string SampleSpecimenId { get; set; } = string.Empty;

        [JsonPropertyName("mineral_species")]
        public string MineralSpecies { get; set; } = string.Empty;

        [JsonPropertyName("primary_metal_assay_pct")]
        public float PrimaryMetalAssayPct { get; set; }

        [JsonPropertyName("radiation_emission_ur_hr")]
        public float RadiationEmissionUrHr { get; set; }

        [JsonPropertyName("timestamp_relative")]
        public string TimestampRelative { get; set; } = string.Empty;

        [JsonPropertyName("tags")]
        public List<string> Tags { get; set; } = new List<string>();

        [JsonPropertyName("prose")]
        public string Prose { get; set; } = string.Empty;
    }

    public sealed class HydroGeologyCatalog
    {
        private readonly List<WellContaminationEntry> _wellEntries = new List<WellContaminationEntry>();
        private readonly List<CaveBiotaEntry> _biotaEntries = new List<CaveBiotaEntry>();
        private readonly List<SteamVentDiagnosticEntry> _steamEntries = new List<SteamVentDiagnosticEntry>();
        private readonly List<StalactiteMineralAssayEntry> _mineralEntries = new List<StalactiteMineralAssayEntry>();

        private readonly Dictionary<string, object> _entriesById =
            new Dictionary<string, object>(StringComparer.OrdinalIgnoreCase);

        public IReadOnlyList<WellContaminationEntry> WellEntries => _wellEntries;
        public IReadOnlyList<CaveBiotaEntry> BiotaEntries => _biotaEntries;
        public IReadOnlyList<SteamVentDiagnosticEntry> SteamEntries => _steamEntries;
        public IReadOnlyList<StalactiteMineralAssayEntry> MineralEntries => _mineralEntries;

        public int TotalCount => _wellEntries.Count + _biotaEntries.Count + _steamEntries.Count + _mineralEntries.Count;

        public static HydroGeologyCatalog LoadFromDirectory(string directoryPath)
        {
            var catalog = new HydroGeologyCatalog();
            if (!Directory.Exists(directoryPath)) return catalog;

            var options = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                ReadCommentHandling = JsonCommentHandling.Skip,
                AllowTrailingCommas = true
            };

            // 1. Artesian Well Contamination Logs
            string wellPath = Path.Combine(directoryPath, "artesian_well_contamination_logs.json");
            if (File.Exists(wellPath))
            {
                var list = JsonSerializer.Deserialize<List<WellContaminationEntry>>(File.ReadAllText(wellPath), options);
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

            // 2. Cave Aquatic Biota Logs
            string biotaPath = Path.Combine(directoryPath, "cave_aquatic_biota_logs.json");
            if (File.Exists(biotaPath))
            {
                var list = JsonSerializer.Deserialize<List<CaveBiotaEntry>>(File.ReadAllText(biotaPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._biotaEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 3. Geothermal Steam Vent Diagnostics
            string steamPath = Path.Combine(directoryPath, "geothermal_steam_vent_diagnostics.json");
            if (File.Exists(steamPath))
            {
                var list = JsonSerializer.Deserialize<List<SteamVentDiagnosticEntry>>(File.ReadAllText(steamPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._steamEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            // 4. Stalactite Mineral Assay Reports
            string mineralPath = Path.Combine(directoryPath, "stalactite_mineral_assay_reports.json");
            if (File.Exists(mineralPath))
            {
                var list = JsonSerializer.Deserialize<List<StalactiteMineralAssayEntry>>(File.ReadAllText(mineralPath), options);
                if (list != null)
                {
                    foreach (var item in list)
                    {
                        if (item == null || string.IsNullOrWhiteSpace(item.Id)) continue;
                        catalog._mineralEntries.Add(item);
                        catalog._entriesById[item.Id] = item;
                    }
                }
            }

            return catalog;
        }

        public WellContaminationEntry GetWellContamination(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is WellContaminationEntry e ? e : null;
        }

        public CaveBiotaEntry GetCaveBiota(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is CaveBiotaEntry e ? e : null;
        }

        public SteamVentDiagnosticEntry GetSteamVent(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is SteamVentDiagnosticEntry e ? e : null;
        }

        public StalactiteMineralAssayEntry GetMineralAssay(string id)
        {
            if (string.IsNullOrEmpty(id)) return null;
            return _entriesById.TryGetValue(id, out var obj) && obj is StalactiteMineralAssayEntry e ? e : null;
        }
    }
}
